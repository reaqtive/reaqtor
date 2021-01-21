// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Transaction log manager used by a query engine in order to reliably persist create/delete operations for reactive artifacts.
    /// </summary>
    /// <remarks>
    /// Also see notes on <see cref="QueryEngine.TransactionLog"/> for some more information. The basic interactions between engine and the
    /// transaction log are:
    ///
    /// * Creation and deletion operations use <see cref="TransactionLog"/> to append to the current log.
    /// * <see cref="CheckpointingQueryEngine.CheckpointAsync(IStateWriter, CancellationToken, IProgress{int})"/> calls:
    ///   - <see cref="SnapshotAsync"/> to get a snapshot of the log, and to start a new generation of logging. The current snapshot will be
    ///     superseded by the upcoming checkpoint.
    ///   - <see cref="SnapshotCleanup.LoseReferenceAsync"/> after a checkpoint succeeds (and only then), to start pruning the transaction log
    ///     and reclaim storage; this advances some version numbers in the store prior to starting garbage collection in the store.
    ///   - <see cref="ReclaimResource.ReclaimAsync"/> in a fire-and-forget manner to start deleting stale transaction log entries from the
    ///     store to reclaim storage.
    /// * <see cref="CheckpointingQueryEngine.RecoverAsync(IStateReader, CancellationToken, IProgress{int})"/> calls:
    ///   - <see cref="GetReplayLog"/> to obtain the operations to replay from the transaction log.
    ///
    /// The transaction log manager keeps a separate table in the key/value store, named `TxMetadata`, which it uses to keep track of ever-
    /// increasing version numbers. Each snapshot increases the number to start a new segment in the log for subsequent create/delete operations
    /// to be appended to (this is stored as a value called `Latest`). Besides this, it keeps track of the range of older versions that still
    /// need to be pruned in the underlying store, in order to allow for crashes during pruning (and be able to continue garbage collection).
    ///
    /// For example, consider the transaction log is at version 42, and a create subscription request comes in. There will be a table called
    /// `TxSubscriptions42` to which we'll append a `Create` entry for the new subscription. When the next checkpoint happens (relative to this
    /// creation operation), a new table called `TxSubscriptions43` will be created, and entries in the old table are part of a "snapshot". If
    /// the checkpoint succeeds, the "lose the reference" to `TxSubscriptions42` by updating metadata in `TxMetadata`, and kick off a background
    /// operation to prune the store by starting the removal of `TxSubscriptions42`. If the checkpoint does not succeed, the old table remains,
    /// and a future checkpoint attempt will create `TxSubscriptions44`. If that checkpoint succeeds, tables 42 and 43 will be removed. In case
    /// of a failover, replay happens across all still-active transaction log tables (typically just one, but if checkpoints failed, possibly
    /// more than one). The <see cref="ReplayTransactionLog"/> takes care of replay across different versions of the log.
    /// </remarks>
    internal sealed class TransactionLogManager : IDisposable
    {
        private const string ActiveCountKey = "ActiveCount";
        private const string HeldCountKey = "HeldCount";
        private const string LatestKey = "Latest";

        private const string MetadataTableName = "TxMetadata";

        private readonly Uri _engineId;
        private readonly IKeyValueStore _keyValueStore;
        private readonly ISerializationPolicy _policy;

        // Versioning/Garbage collection lock.
        private readonly AsyncLock _lock;

        private readonly IKeyValueTable<string, string> _metadataTable;
        private readonly Queue<ITransactionLog> _versionedLogs;

        private ITransactionLog _transactionLog;

        private bool _disposed;

        // Cached values
        private long _activeCount;
        private long _heldCount;
        private long _latest;
        private bool _initializeBeforeRecovery;
        private bool _recoveryHappened;

        /// <summary>
        /// Creates a new transaction log manager.
        /// </summary>
        /// <param name="engineId">The engine this transaction log manager is used for. Only used for logging purposes.</param>
        /// <param name="keyValueStore">The key/value store to write transaction logs to.</param>
        /// <param name="policy">The serialization policy to use when serializing objects.</param>
        public TransactionLogManager(Uri engineId, IKeyValueStore keyValueStore, ISerializationPolicy policy)
        {
            _engineId = engineId;
            _keyValueStore = keyValueStore;
            _policy = policy;

            _lock = new AsyncLock();
            _versionedLogs = new Queue<ITransactionLog>();
            _metadataTable = new SerializingKeyValueTable<string>(
                _keyValueStore.GetTable(MetadataTableName),
                (str, s) =>
                {
                    using var writer = new BinaryWriter(s);

                    writer.Write(str);
                },
                s =>
                {
                    using var reader = new BinaryReader(s);

                    return reader.ReadString();
                }
            );

            using (var t = _keyValueStore.CreateTransaction())
            {
                var tx = _metadataTable.Enter(t);
                if (!tx.Contains(ActiveCountKey))
                {
                    Invariant.Assert(!tx.Contains(LatestKey) && !tx.Contains(HeldCountKey), "Transaction log versioning keys are only partially populated.");

                    _activeCount = _heldCount = _latest = 0;
                }
                else
                {
                    Invariant.Assert(tx.Contains(LatestKey) && tx.Contains(HeldCountKey), "Transaction log versioning keys are only partially populated.");

                    _activeCount = long.Parse(tx[ActiveCountKey], CultureInfo.InvariantCulture);
                    _heldCount = long.Parse(tx[HeldCountKey], CultureInfo.InvariantCulture);
                    _latest = long.Parse(tx[LatestKey], CultureInfo.InvariantCulture);
                }
            }

            Tracing.Transaction_Log_Initialization(null, _engineId, _latest, _activeCount, _heldCount);

            for (var i = _latest - _heldCount + 1; i <= _latest; i++)
            {
                _versionedLogs.Enqueue(new TransactionLog(_keyValueStore, _policy, i));
            }
        }

        /// <summary>
        /// Disposes managed resources.
        /// </summary>
        /// <remarks>
        /// The owner of the instance is responsible to call <see cref="Dispose"/> after all active operations on
        /// the instance have completed. Calling <see cref="Dispose"/> while any operation is in flight will cause
        /// undefined behavior.
        /// </remarks>
        public void Dispose()
        {
            if (!_disposed)
            {
                _lock.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Gets the current transaction log to append create/delete operations to.
        /// </summary>
        public Task<ITransactionLog> TransactionLog => GetTransactionLogAsync();

        private async Task<ITransactionLog> GetTransactionLogAsync()
        {
            // TODO cache the task
            await EnsureInitializedAsync().ConfigureAwait(false);

            AssertInvariants();

            return _transactionLog;
        }

        /// <summary>
        /// Snapshots the current transaction log. The returned <see cref="SnapshotCleanup"/> object can be used to trigger truncation after
        /// a checkpoint succeeds.
        /// </summary>
        /// <returns>Task completing after the snapshot succeeds (merely updating some metadata), and returning a cleanup object.</returns>
        public async Task<SnapshotCleanup> SnapshotAsync()
        {
            AssertInvariants();

            using (var t = _keyValueStore.CreateTransaction())
            {
                var tx = _metadataTable.Enter(t);
                if (tx.Contains(LatestKey))
                {
                    tx.Update(LatestKey, (_latest + 1).ToString(CultureInfo.InvariantCulture));
                    tx.Update(ActiveCountKey, (_activeCount + 1).ToString(CultureInfo.InvariantCulture));
                    tx.Update(HeldCountKey, (_heldCount + 1).ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    tx.Add(LatestKey, "1");
                    tx.Add(ActiveCountKey, "1");
                    tx.Add(HeldCountKey, "1");
                }

                await t.CommitAsync().ConfigureAwait(false);
            }

            _latest++;
            _activeCount++;
            _heldCount++;

            _transactionLog = new TransactionLog(_keyValueStore, _policy, _latest);
            _versionedLogs.Enqueue(_transactionLog);

            Tracing.Transaction_Log_Snapshot(null, _engineId, _latest, _activeCount, _heldCount);

            AssertInvariants();

            return new SnapshotCleanup(this);
        }

        /// <summary>
        /// Cleanup object returned from <see cref="SnapshotAsync"/>, used to trigger truncation after successful checkpoints.
        /// </summary>
        public sealed class SnapshotCleanup
        {
            private readonly TransactionLogManager _parent;

            public SnapshotCleanup(TransactionLogManager parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Triggers truncation of the transaction log (everything up to and including the current snapshot that has been
            /// superseded by the state persisted by the successful checkpoint).
            /// </summary>
            /// <param name="transaction">The transaction to use to update the underlying tables.</param>
            /// <returns>
            /// Task completing when metadata has been updated to indicate the intent to truncate; the returned object is then used
            /// to trigger a background GC using <see cref="ReclaimResource.ReclaimAsync"/>.
            /// </returns>
            /// <remarks>
            /// If a failure happens after a successful checkpoint but before calling (and completing) the lose reference operation,
            /// subsequent recovery will cause coalescing of the not-yet-discarded snapshotted-but-checkpointed log entries with any
            /// newer entries that were added. We're hardended against these operations that will cause "create existing" and "delete
            /// non-existing" resources, but it's not ideal.
            ///
            /// When/if we supersede the IStateWriter approach to checkpoints and fully commit to using the key/value store passed to
            /// the engine's constructor, we can have a transaction that spans both updates, and not have to worry about this edge case.
            ///
            /// The reason for having the stores in two places is historical when early versions of Reaqtor managed the transaction log
            /// externally to an engine using specialized transaction log facilities (akin to CLFS) that were not integrated with the
            /// key/value store used (i.e. Service Fabric KVS). Over time, we moved away from this model, but we had to keep the
            /// IStateWriter and IStateReader that's used in other environments as well.
            ///
            /// It's worth considering an alternative engine implementation (alongside the current one for starters) that's "active"
            /// and doesn't have to be told when to checkpoint either. It simply uses the key/value store passed to its constructor,
            /// supports `RecoverAsync()` to recover from the given store (or has a static factory to do so, rather than a separate
            /// construction step), and performs checkpointing when it needs to, akin to a GC deciding it's time to perform a GC. It
            /// can do this by measuring the amount of dirty state and maybe have a configure maximum delay in between checkpoints to
            /// ensure recovery time is bounded (and replay of events is limited). It can furthermore have probes on ingress pieces
            /// (reachable through some IIngressProbe interface implemented by operators, so it can hunt for these using visitors over
            /// the subscriptions and subjects) to compute metrics and be self-tuning. There can still be a `CheckpointAsync` which
            /// is similar to `GC.Collect()` where an external party can trigger a checkpoint, e.g. right before unloading the engine
            /// or when a known burst of events has passed through, so it's worth evacuating state to disk.
            /// </remarks>
            public async Task<ReclaimResource> LoseReferenceAsync(IKeyValueStoreTransaction transaction = null)
            {
                _parent.AssertInvariants();

                var createdNewTransaction = false;
                if (transaction == null)
                {
                    createdNewTransaction = true;
                    transaction = _parent._keyValueStore.CreateTransaction();
                }

                // Very important to make the active count = 1 at the very least. Garbage collection is secondary (but important too).
                using (await _parent._lock.EnterAsync().ConfigureAwait(false))
                {
                    var tx = _parent._metadataTable.Enter(transaction);
                    try
                    {
                        tx.Update(ActiveCountKey, "1");
                        await transaction.CommitAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        if (createdNewTransaction)
                            transaction.Dispose();
                    }

                    _parent._activeCount = 1;
                }

                Tracing.Transaction_Log_Lost_Reference(null, _parent._engineId, _parent._latest, _parent._activeCount, _parent._heldCount);

                _parent.AssertInvariants();

                return new ReclaimResource(_parent);
            }
        }

        /// <summary>
        /// Object returned from <see cref="SnapshotCleanup.LoseReferenceAsync(IKeyValueStoreTransaction)"/>, used to trigger a "garbage collection"
        /// that removes transaction log entries that are no longer reachable.
        /// </summary>
        public sealed class ReclaimResource
        {
            private readonly TransactionLogManager _parent;

            public ReclaimResource(TransactionLogManager parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Starts reclaiming transaction log entries that are no longer reachable. Typically called in a fire-and-forget fashion.
            /// </summary>
            /// <returns>Task to indicate when garbage collection is finished.</returns>
            public async Task ReclaimAsync()
            {
                _parent.AssertInvariants();

                if (_parent._heldCount > _parent._activeCount)
                {
                    using (await _parent._lock.EnterAsync().ConfigureAwait(false))
                    {
                        if (_parent._heldCount > _parent._activeCount)
                        {
                            Tracing.Transaction_Log_Garbage_Collection_Start(null, _parent._engineId, _parent._latest, _parent._activeCount, _parent._heldCount);

                            var difference = _parent._heldCount - _parent._activeCount;
                            using (var tx = _parent._keyValueStore.CreateTransaction())
                            {
                                foreach (var table in _parent._versionedLogs.Take((int)difference))
                                {
                                    table.Scope(tx).Clear();
                                }

                                var metadata = _parent._metadataTable.Enter(tx);
                                metadata.Update(HeldCountKey, _parent._activeCount.ToString(CultureInfo.InvariantCulture));

                                await tx.CommitAsync().ConfigureAwait(false);
                                Tracing.Transaction_Log_Garbage_Collection(null, _parent._engineId, difference, _parent._latest, _parent._activeCount);

                                for (var i = 0; i < difference; i++)
                                    _parent._versionedLogs.Dequeue();

                                _parent._heldCount = _parent._activeCount;
                            }

                            Tracing.Transaction_Log_Garbage_Collection_End(null, _parent._engineId, _parent._latest, _parent._activeCount, _parent._heldCount);
                        }
                    }
                }

                _parent.AssertInvariants();
            }
        }

        private async Task EnsureInitializedAsync()
        {
            if (_transactionLog == null)
            {
                using (await _lock.EnterAsync().ConfigureAwait(false))
                {
                    if (_transactionLog == null)
                    {
                        if (!_recoveryHappened)
                            _initializeBeforeRecovery = true;

                        var tlog = new TransactionLog(_keyValueStore, _policy, _latest + 1);
                        using (var tx = _keyValueStore.CreateTransaction())
                        {
                            var metadata = _metadataTable.Enter(tx);

                            if (metadata.Contains(LatestKey))
                            {
                                metadata.Update(LatestKey, (_latest + 1).ToString(CultureInfo.InvariantCulture));
                                metadata.Update(ActiveCountKey, (_activeCount + 1).ToString(CultureInfo.InvariantCulture));
                                metadata.Update(HeldCountKey, (_heldCount + 1).ToString(CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                metadata.Add(LatestKey, "1");
                                metadata.Add(ActiveCountKey, "1");
                                metadata.Add(HeldCountKey, "1");
                            }

                            await tx.CommitAsync().ConfigureAwait(false);
                        }

                        _latest++;
                        _activeCount++;
                        _heldCount++;

                        _transactionLog = tlog;
                        _versionedLogs.Enqueue(_transactionLog);
                    }
                }
            }

            AssertInvariants();
        }

        public IReplayTransactionLog GetReplayLog()
        {
            var logs = _versionedLogs.Skip((int)(_heldCount - _activeCount)).ToList();
            if (_initializeBeforeRecovery)
                logs.RemoveAt(logs.Count - 1);

            _recoveryHappened = true;
            return new ReplayTransactionLog(logs, this);
        }

        [Conditional("DEBUG")]
        private void AssertInvariants()
        {
            if (_transactionLog == null)
                return;

            if (_heldCount < _activeCount)
                throw new InvariantException("Transaction log held count is less than active count.");

            if (_latest < _heldCount)
                throw new InvariantException("Transaction log latest version is less than held count.");

            if (_activeCount == 0)
                throw new InvariantException("Zero active versions after transaction log has been initialized.");
        }

        /// <summary>
        /// In-memory summary of transaction log entries to replay upon recovery. This includes coalescin across multiple transaction
        /// log versions, e.g. a Delete after Create cancels out.
        /// </summary>
        private sealed class ReplayTransactionLog : IReplayTransactionLog
        {
            private readonly TransactionLogManager _parent;
            private readonly Dictionary<string, ArtifactOperation> _subjectFactories;
            private readonly Dictionary<string, ArtifactOperation> _subscriptionFactories;
            private readonly Dictionary<string, ArtifactOperation> _observers;
            private readonly Dictionary<string, ArtifactOperation> _observables;
            private readonly Dictionary<string, ArtifactOperation> _subjects;
            private readonly Dictionary<string, ArtifactOperation> _subscriptions;
            private readonly Dictionary<string, List<ArtifactOperation>> _invalidReplaySequence;

            public ReplayTransactionLog(IList<ITransactionLog> logs, TransactionLogManager parent)
            {
                _parent = parent;
                _invalidReplaySequence = new Dictionary<string, List<ArtifactOperation>>();

                _subjectFactories = Coalesce(parent._keyValueStore, logs, l => l.SubjectFactories);
                _subscriptionFactories = Coalesce(parent._keyValueStore, logs, l => l.SubscriptionFactories);
                _observers = Coalesce(parent._keyValueStore, logs, l => l.Observers);
                _observables = Coalesce(parent._keyValueStore, logs, l => l.Observables);
                _subjects = Coalesce(parent._keyValueStore, logs, l => l.Subjects);
                _subscriptions = Coalesce(parent._keyValueStore, logs, l => l.Subscriptions);
            }

            private Dictionary<string, ArtifactOperation> Coalesce(
                IKeyValueStore keyValueStore,
                IList<ITransactionLog> logs,
                Func<ITransactionLog, IKeyValueTable<string, ArtifactOperation>> tableSelector)
            {
                var result = new Dictionary<string, ArtifactOperation>();
                foreach (var log in logs)
                {
                    var table = tableSelector(log);

                    using var tx = keyValueStore.CreateTransaction();

                    var txTable = table.Enter(tx);

                    foreach (var item in txTable)
                    {
                        if (result.TryGetValue(item.Key, out ArtifactOperation op))
                        {
                            Tracing.Transaction_Log_Coalesce(null, _parent._engineId, item.Key, op.OperationKind.ToString(), item.Value.OperationKind.ToString());

                            //
                            //                                    SECOND
                            //
                            //                        Create         Delete      DeleteCreate
                            //                   +--------------+--------------+--------------+
                            // F         Create  |   Invalid    |     No-op    |   Create     |
                            // I                 +--------------+--------------+--------------+
                            // R         Delete  | DeleteCreate |    Invalid   |   Invalid    |
                            // S                 +--------------+--------------+--------------+
                            // T   DeleteCreate  |   Invalid    |    Delete    | DeleteCreate |
                            //                   +--------------+--------------+--------------+
                            //

                            switch (op.OperationKind)
                            {
                                case ArtifactOperationKind.Create:
                                    switch (item.Value.OperationKind)
                                    {
                                        case ArtifactOperationKind.Create:
                                            UpdateInvalidReplays(item.Key, item.Value, result, _invalidReplaySequence);
                                            break;
                                        case ArtifactOperationKind.Delete:
                                            result.Remove(item.Key);
                                            break;
                                        case ArtifactOperationKind.DeleteCreate:
                                            result[item.Key] = ArtifactOperation.Create(item.Value.Expression, item.Value.State);
                                            break;
                                    }
                                    break;
                                case ArtifactOperationKind.Delete:
                                    switch (item.Value.OperationKind)
                                    {
                                        case ArtifactOperationKind.Create:
                                            result[item.Key] = ArtifactOperation.DeleteCreate(item.Value.Expression, item.Value.State);
                                            break;
                                        case ArtifactOperationKind.Delete:
                                            UpdateInvalidReplays(item.Key, item.Value, result, _invalidReplaySequence);
                                            break;
                                        case ArtifactOperationKind.DeleteCreate:
                                            UpdateInvalidReplays(item.Key, item.Value, result, _invalidReplaySequence);
                                            break;
                                    }
                                    break;
                                case ArtifactOperationKind.DeleteCreate:
                                    switch (item.Value.OperationKind)
                                    {
                                        case ArtifactOperationKind.Create:
                                            UpdateInvalidReplays(item.Key, item.Value, result, _invalidReplaySequence);
                                            break;
                                        case ArtifactOperationKind.Delete:
                                            result[item.Key] = item.Value;
                                            break;
                                        case ArtifactOperationKind.DeleteCreate:
                                            result[item.Key] = item.Value;
                                            break;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            result.Add(item.Key, item.Value);
                        }
                    }
                }

                return result;
            }

            private static void UpdateInvalidReplays(string id, ArtifactOperation failingOperation, IDictionary<string, ArtifactOperation> replayList, IDictionary<string, List<ArtifactOperation>> failedReplays)
            {
                if (!failedReplays.TryGetValue(id, out List<ArtifactOperation> lst))
                {
                    var prev = replayList[id];
                    replayList.Remove(id);
                    failedReplays[id] = lst = new List<ArtifactOperation> { prev };
                }

                lst.Add(failingOperation);
            }

            public IDictionary<string, ArtifactOperation> SubjectFactories => _subjectFactories;

            public IDictionary<string, ArtifactOperation> SubscriptionFactories => _subscriptionFactories;

            public IDictionary<string, ArtifactOperation> Observers => _observers;

            public IDictionary<string, ArtifactOperation> Observables => _observables;

            public IDictionary<string, ArtifactOperation> Subjects => _subjects;

            public IDictionary<string, ArtifactOperation> Subscriptions => _subscriptions;

            public IDictionary<string, List<ArtifactOperation>> InvalidReplaySequence => _invalidReplaySequence;
        }
    }
}
