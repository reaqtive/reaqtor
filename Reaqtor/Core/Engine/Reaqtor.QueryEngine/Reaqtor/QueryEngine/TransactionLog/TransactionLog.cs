// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Implementation of a transaction log for creation and deletion operations on reactive entities.
    /// </summary>
    /// <remarks>
    /// The engine employs a write-ahead logging strategy to ensure persistence of DDL operations to reactive entities
    /// in the absence of checkpoints.
    ///
    /// For example, when a subscription is created, a creation entry is appendd to a transaction log and committed to
    /// the key/value store. When a subsequent checkpoint occurs, the subscription entry makes it to permanent storage,
    /// and the transaction log gets truncated. If a failure occurs before a checkpoint, the transaction log takes care
    /// of replaying the subscription creation.
    ///
    /// Note that this can cause some "interesting" behavior on the events observed by the subscription because the
    /// state persisted for a subscription in the transaction log does not incorporate watermarks of events on incoming
    /// streams in order to be able to replay from the same point upon replay of the creation operation. In particular,
    /// the engine will re-create the subscription from the transaction log and restore existing streams which may be
    /// consumed by the subscription. This includes watermarks to replay from, so the subscription may get a flashback
    /// in time to an earlier watermark than the one it saw the first time it got created.
    ///
    /// This is a known limitation that could be lifted by more advanced checkpointing strategies (where a lightweight
    /// checkpoint can be made to just incorporate the newly created artifact, including its dependencies), so the role
    /// of the transaction log diminishes. There are some alternative schemes.
    ///
    /// However, in practice, this often doesn't turn out to be an issue. Subscriptions already expect to go back in
    /// time upon recovery, and users who really care will thread sequence IDs through the computation and have an
    /// egress subsystem with deduplication behavior (either using monotonically increasing IDs if the environment is
    /// set up to have consistent ordering across events in different streams, e.g. by virtue of an ingress manager
    /// that performs a merge sort across sources that feed into an engine, cf. LINQ to Traces (Tx)'s approach, or by
    /// having hashes of events or other keys that can be used to determine duplication).
    ///
    /// Yet another alternative that has been considered in the past is to support "negative" [Skip|Take][Until]
    /// operators with a time parameter which can be a sequence ID. This effectively enables subscriptions to request
    /// to "start from" a specific point. For example, xs.Skip(-TimeSpan.FromSeconds(5)) would just request replay
    /// of 5 seconds (in the past, hence negative) worth of data relative to the creation of a subscription. This shows
    /// a more traditional use of time (which can be dependent on semantics of the source, either wall clock time or
    /// more commonly use time stamps on the events), but alternatives with sequence IDs can be used (e.g. using
    /// SkipUntil with a long value that refers to a sequence ID, which may be in the past). At that point, every
    /// re-creation of a subscription that's in the transaction log (but not in the checkpoint yet) will trigger the
    /// same creation/start logic, with an "anchor" to a particular sequence ID or point in time, causing consistency
    /// across recoveries.
    /// </remarks>
    internal sealed class TransactionLog : ITransactionLog
    {
        //
        // These are the prefixes for th enames of the tables that keep logs on a per-entity type basis. As the log
        // moves forward on checkpoint boundaries (see TransactionLogManager notes for more information), increasing
        // version numbers get appended to create table names, e.g. `TxSubscriptions123`. Log trunaction after a
        // checkpoint will take care of removing tables with older version numbers.
        //

        private const string SubjectFactoriesKey = "TxSubjectFactories";
        private const string ObserversKey = "TxObservers";
        private const string ObservablesKey = "TxObservables";
        private const string SubjectsKey = "TxSubjects";
        private const string SubscriptionsKey = "TxSubscriptions";
        private const string SubscriptionFactoriesKey = "TxSubscriptionFactories";

        private readonly IKeyValueStore _keyValueStore;

        /// <summary>
        /// Creates a transaction log that uses the specified underlying key/value store for the per-artifact operation tables.
        /// </summary>
        /// <param name="keyValueStore">The underlying key/value store to use.</param>
        /// <param name="policy">The serialization policy to use when serializing objects.</param>
        /// <param name="version">A monotonically increasing version number; each transaction creates a unique version of the tables.</param>
        public TransactionLog(IKeyValueStore keyValueStore, ISerializationPolicy policy, long version)
        {
            _keyValueStore = keyValueStore;

            SubjectFactories = GetArtifactTable(_keyValueStore, policy, version, SubjectFactoriesKey);
            Observers = GetArtifactTable(_keyValueStore, policy, version, ObserversKey);
            Observables = GetArtifactTable(_keyValueStore, policy, version, ObservablesKey);
            Subjects = GetArtifactTable(_keyValueStore, policy, version, SubjectsKey);
            Subscriptions = GetArtifactTable(_keyValueStore, policy, version, SubscriptionsKey);
            SubscriptionFactories = GetArtifactTable(_keyValueStore, policy, version, SubscriptionFactoriesKey);
        }

        private static IKeyValueTable<string, ArtifactOperation> GetArtifactTable(IKeyValueStore keyValueStore, ISerializationPolicy policy, long version, string name)
        {
            return new SerializingKeyValueTable<ArtifactOperation>(
                    keyValueStore.GetTable(GetInternalTableName(version, name)),
                    (o, s) =>
                    {
                        using var writer = new TransactionLogOperationWriter(s, policy);

                        writer.WriteHeader();
                        writer.Save(o);
                    },
                    s =>
                    {
                        using var reader = new TransactionLogOperationReader(s, policy);

                        reader.ReadHeader();
                        var ret = reader.Load();
                        reader.ReadFooter();
                        return ret;
                    }
                );
        }

        private static string GetInternalTableName(long version, string tableName) => version + tableName;

        /// <summary>
        /// Gets the table for artifact operations applied to subject factories.
        /// </summary>
        public IKeyValueTable<string, ArtifactOperation> SubjectFactories { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to observers.
        /// </summary>
        public IKeyValueTable<string, ArtifactOperation> Observers { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to observables.
        /// </summary>
        public IKeyValueTable<string, ArtifactOperation> Observables { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to subjects.
        /// </summary>
        public IKeyValueTable<string, ArtifactOperation> Subjects { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to subscriptions.
        /// </summary>
        public IKeyValueTable<string, ArtifactOperation> Subscriptions { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to subscription factories.
        /// </summary>
        public IKeyValueTable<string, ArtifactOperation> SubscriptionFactories { get; }

        /// <summary>
        /// Enters a transaction scope (by entering transactions on the underlying tables in the key/value store).
        /// </summary>
        /// <param name="transaction">The transaction object to apply edits on (in the form of table, key, value triplets).</param>
        /// <returns>Handle to the transaction scope; enables abandoning changes by clearing the transaction.</returns>
        public IScopedTransactionLog Scope(IKeyValueStoreTransaction transaction) => new ScopedTransactionLog(this, transaction);

        private sealed class ScopedTransactionLog : IScopedTransactionLog
        {
            private readonly ITransactionLog _transactionLog;
            private readonly IKeyValueStoreTransaction _transaction;

            public ScopedTransactionLog(ITransactionLog transactionLog, IKeyValueStoreTransaction transaction)
            {
                _transactionLog = transactionLog;
                _transaction = transaction;
            }

            public void Clear()
            {
                _transactionLog.SubjectFactories.Enter(_transaction).Clear();
                _transactionLog.Observers.Enter(_transaction).Clear();
                _transactionLog.Observables.Enter(_transaction).Clear();
                _transactionLog.Subjects.Enter(_transaction).Clear();
                _transactionLog.Subscriptions.Enter(_transaction).Clear();
                _transactionLog.SubscriptionFactories.Enter(_transaction).Clear();
            }
        }
    }
}
