// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;

using Reaqtor.Expressions.Core;
using Reaqtor.Metadata;
using Reaqtor.QueryEngine.Events;
using Reaqtor.QueryEngine.Metrics;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        //
        // The file extracts all the checkpoint/recover logic for the core engine.
        //

        private partial class CoreReactiveEngine
        {
            //
            // NB: Canaries are used to assert health of a store. E.g. if a store ends up being emptied or fails to be read from,
            //     the lack of canary entities raises a warning.
            //

            private static readonly Uri s_subscriptionCanaryUri = new("mgmt://canary/subscription");
            private static readonly Uri s_observableCanaryUri = new("mgmt://canary/observable");
            private static readonly Uri s_observerCanaryUri = new("mgmt://canary/observer");

            public async Task CheckpointAsync(IStateWriter writer, CancellationToken token, IProgress<int> progress)
            {
                //
                // Steps for a checkpoint are:
                //
                // - If there are any artifacts to delete due to a GC pass, do so now in order to piggyback on the upcoming commit to the store.
                // - Pause the scheduler such that state in running queries is no longer being mutated.
                //   - This is done cooperatively using YieldToken support, e.g. to have ingress observables bail out their pushing of events.
                //   - In practice, pausing takes at most a few milliseconds.
                // - Acquire the checkpoint lock, which is also shared with create/delete code paths for transaction logging.
                //   - All incoming create/delete operations will be queued up behind us holding the lock.
                //   - Time under the lock is kept limited.
                // - Do two things concurrently:
                //   - Snapshot the current transaction log. All entries are subsumed by the upcoming commit of the checkpoint. If it succeeds,
                //     we'll prune the transaction log using the snapshot. (Note that the moment the lock is released, a new generation of log
                //     entries will start, so future create/delete operations running while we're still in the checkpoint path won't be part
                //     of the snapshot.)
                //   - Write all the engine state (insert/delete of expressions, insert/update/delete of operator state) to the state writer,
                //     either in a full or differential manner. This iterates over the registry and uses visitors to go over operator state,
                //     including checks for dirty flags. All writes are buffered to memory inside the writer, to make this step go fast (i.e.
                //     not hitting I/O, though it's entirely up to the IStateWriter implementation to ensure this).
                // - Release the lock. Create/delete operations can come in again, but aren't part of the transaction snapshot or the state
                //   persisted earlier.
                // - Resume the scheduler. State in operators can start mutating again due to event processing resuming. However, dirty flags
                //   are tri-state based, so when we succeed the commit the checkpoint (further on), we won't reset a dirty flag to false is
                //   another change of state happens from this point on. (I.e. clean -> dirty -> more dirty where the last transition happens
                //   if a write happens before we commit the transaction, and more dirty -> dirty is used. Only dirty -> clean is possible.)
                // - Commit the transaction. If it fails, we'll roll back, the transaction log won't be pruned, and dirty state flags won't
                //   get reset to clean. A subsequent checkpoint attempt will be needed to try to persist state.
                // - If commit succeeds:
                //   - Start pruning the transaction log.
                //   - Mark dirty states of checkpointed items as clean, unless they were re-dirtied while the async commit was in flight.
                //
                // Note that event processing is stalled for a minimal amount of time:
                //
                // - In flight create/delete operations need to be drained (typically a low volume).
                // - Event processing needs to pause (typically granted quickly with item processing tasks yielding).
                // - State is snapshotted in memory.
                //
                // The commit of the transaction happens asynchronously and after the scheduler resumes. Only after a successful checkpoint
                // we go over a snapshot of all persisted entities in order to adjust dirty flags (in support of differential checkpoints).
                //
                // In typical production systems we've seen pause times that are limited to a few milliseconds, and transaction commits that
                // involve I/O and state replication (e.g. in Service Fabric) and take tens to hundreds of milliseconds. Many deployments just
                // checkpoint once a minute, so "time spent paused during checkpoints" is far below 1%.
                //
                // NB: An alternative design where an engine decides when to checkpoint (rather than being told to do so) has been considered
                //     but the implementation hasn't been merged here. The core logic would be the same, but a policy piece would be written
                //     on top to compute stats (e.g. scan for dirty state periodically and assess how big such state would be), with a
                //     feedback loop to self-adjust (based on observed pause and commit times), and taking configuration into account (e.g.
                //     maximum time between checkpoints, in order to cap the amount of events to replay in case of recovery).
                //

                if (Parent.Options.GarbageCollectionSweepEnabled)
                {
                    _garbageCollector.Sweep();
                }

                await PauseSchedulerAsync().ConfigureAwait(false);

                var stateSaver = default(StateSaver);
                var oldTx = default(TransactionLogManager.SnapshotCleanup);

                try
                {
                    try
                    {
                        using (await _checkpointAndRecoverLock.EnterWriteAsync().ConfigureAwait(false))
                        {
                            stateSaver = CreateStateSaver(writer);
                            var oldTxSnapshotter = Parent._transactionLogManager.SnapshotAsync();
                            stateSaver.Save(token);
                            oldTx = await oldTxSnapshotter.ConfigureAwait(false);
                        }
                    }
                    finally
                    {
                        ContinueScheduler();
                    }

                    await writer.CommitAsync().ConfigureAwait(false);

                    // TODO pass in transaction from the writer above when writer is inside engine
                    var reclaim = await oldTx.LoseReferenceAsync().ConfigureAwait(false);
                    _ = reclaim.ReclaimAsync().ContinueWith(t => Tracing.Transaction_Log_Garbage_Collection_Failed(null, Parent.Uri, t.Exception), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
                }
                catch
                {
                    writer.Rollback();
                    throw;
                }

                stateSaver.OnSaved();

                if (_migrationTask != null)
                {
                    _migrationTask.ResetQuota();
                    Parent.Scheduler.RecalculatePriority();
                }
            }

            private StateSaver CreateStateSaver(IStateWriter writer)
            {
                Debug.Assert(writer != null, "Writer should not be null.");

                if (writer.CheckpointKind == CheckpointKind.Full)
                {
                    return new FullStateSaver(writer, this);
                }

                if (writer.CheckpointKind == CheckpointKind.Differential)
                {
                    return new DifferentialStateSaver(writer, this);
                }

                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Unknown type of checkpoint specified: '{0}'.", writer.CheckpointKind));
            }

            public async Task RecoverAsync(IStateReader reader, CancellationToken token, IProgress<int> progress)
            {
                //
                // Recovery is fairly easy and typically runs right after instantiating an engine after a failover. However, it has been
                // designed such that multiple sequential recoveries from different state readers can be used to "merge" multiple engines,
                // typically followed by a full checkpoint to a new writer for the "merged" engine. This has been used to defragment
                // clusters that consist of many small engines, or during migrations where all new artifacts are created in a new generation
                // of QEs, while the old QEs are being drained and as they get smaller, we decide to merge them.
                //
                // Steps for recovery are:
                //
                // - Pause the scheduler (in support of doing a recovery on top of an existing engine, as explained above).
                // - Grab the checkpoint lock to prevent checkpoints or new create/delete operations.
                // - Recover all the state (entities and their runtime state) from the given state reader.
                // - Replay the transaction log for any create/delete operations that came in after the checkpoint.
                // - Release the lock; new create/delete operations can come in.
                // - Assert canaries are present and warn if not (to spot store issues).
                // - Optionally start rewriting artifacts to use templates in the background (see TemplateMigrationTask).
                // - Run the GC to find orphaned artifacts in the store, which can be deleted in a subsequent checkpoint.
                // - Resume the scheduler.
                //
                // Note that it's generally a good idea for the state store to be replicated and local to the node when a QE fails over to,
                // because state is fine-grained. So example, 1000 subscriptions correspond to at least 2000 key/value entries in a table,
                // of which 1000 are the expression trees and 1000 are the runtime state. There can be more if higher order operators are
                // used (e.g. Select, Window, GroupBy, which rely on auxiliary bridges, tunnels, tollbooths, with additional observable and
                // subscription entities, the amount of which is proportional in event volumes). State is kept in a fine-grained manner to
                // improve efficiency of checkpoints (less I/O, fine-grained differential checkpoints, etc.). When a local store is used,
                // as the result of replication between a primary and its secondaries, the multitude of small local reads is far less of an
                // issue than a multitude of small reads from a central state store.
                //
                // NB: A prototype of a "generational" state store has been built, which had mixed results. The idea is basically that
                //     long-lived entities (e.g. subscriptions that persist across many checkpoints) are being grouped into bigger persisted
                //     entries (to get less small I/O in favor of a few big I/Os). When such a long-lived entity eventually dies, a tombstone
                //     record is persisted to indicate its deletion. When the ratio of dead versus alive entries in an "old" generation
                //     crosses a certain threshold, the generation is compacted by removing the deleted entries and their tombstones (which
                //     are kept separately as fine-grained entries in an auxiliary table), and rewriting the generation, possibly coalescing
                //     it with the a younger generation that has been aging. The storage format of a generation is effectively a giant
                //     expression tree containing a Dictionary<Uri, byte[]> constant (assigned to a variable) for runtime state, and a whole
                //     bunch of Expression.Quote(...) values stored in a similar dictionary. Recovery obtains the list of tombstones, and
                //     then proceeds going over these dictionaries (with the exception of tombstoned entries, per the given list) in order
                //     to apply create/delete operations and recover state. While this approach helps for recovery, it adds a lot of work
                //     for compaction that can interfere with event processing (though a policy could be devised that does this work in the
                //     background when nothing else is going on). In practice, having lots of small QEs has been more practical to cap the
                //     recovery time on QEs.
                //

                await PauseSchedulerAsync().ConfigureAwait(false);

                try
                {
                    using (await _checkpointAndRecoverLock.EnterWriteAsync().ConfigureAwait(false))
                    using (var stateLoader = new StateLoader(reader, this))
                    {
                        stateLoader.Load(token);
                        ReplayTransactionLog();
                    }

                    CheckCanaryRecovery();
                    StartTemplateMigration();
                    _garbageCollector.Collect();
                }
                finally
                {
                    ContinueScheduler();
                }
            }

            private void ReplayTransactionLog()
            {
                var exceptions = new List<Exception>();
                var log = Parent._transactionLogManager.GetReplayLog();

                foreach (var failure in log.InvalidReplaySequence)
                {
                    var ex = new InvalidOperationException(string.Join(", ", failure.Value.Select(x => x.OperationKind.ToString())));
                    if (!OnEntityReplayFailed(new Uri(failure.Key), ex))
                        exceptions.Add(ex);
                }

                ReplayArtifacts(log.SubjectFactories, Parent._engine.DefineSubjectFactory, Parent._engine.UndefineSubjectFactory, Registry.SubjectFactories, ReactiveEntityKind.StreamFactory, exceptions);
                ReplayArtifacts(log.Observers, Parent._engine.DefineObserver, Parent._engine.UndefineObserver, Registry.Observers, ReactiveEntityKind.Observer, exceptions);
                ReplayArtifacts(log.Observables, Parent._engine.DefineObservable, Parent._engine.UndefineObservable, Registry.Observables, ReactiveEntityKind.Observable, exceptions);
                ReplayArtifacts(log.SubscriptionFactories, Parent._engine.DefineSubscriptionFactory, Parent._engine.UndefineSubscriptionFactory, Registry.SubscriptionFactories, ReactiveEntityKind.SubscriptionFactory, exceptions);
                ReplayArtifacts(log.Subjects, Parent._engine.CreateStream, Parent._engine.DeleteStream, Registry.Subjects, ReactiveEntityKind.Stream, exceptions);
                ReplayArtifacts(log.Subscriptions, Parent._engine.CreateSubscription, Parent._engine.DeleteSubscription, Registry.Subscriptions, ReactiveEntityKind.Subscription, exceptions);

                if (exceptions.Count > 0)
                    throw new AggregateException(exceptions);
            }

            private void ReplayArtifacts<TEntityType>(IDictionary<string, ArtifactOperation> log, Action<Uri, Expression, object> create, Action<Uri> delete, IReactiveEntityCollection<string, TEntityType> collection, ReactiveEntityKind kind, List<Exception> failures)
                where TEntityType : ReactiveEntity
            {
                TEntityType entity;
                foreach (var artifact in log)
                {
                    var id = new Uri(artifact.Key);
                    Parent.TraceSource.Transaction_Log_Replay(Parent.Uri, artifact.Value.OperationKind.ToString(), kind.ToString(), id);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (By design to harden against engine unavailablility; gets traced and reported.)

                    try
                    {
                        switch (artifact.Value.OperationKind)
                        {
                            case ArtifactOperationKind.Create:
                                create(id, artifact.Value.Expression, artifact.Value.State);
                                collection.TryGetValue(id.ToCanonicalString(), out entity);
                                entity.AdvanceState(TransactionState.Active);
                                break;
                            case ArtifactOperationKind.DeleteCreate:
                                delete(id);
                                create(id, artifact.Value.Expression, artifact.Value.State);
                                collection.TryGetValue(id.ToCanonicalString(), out entity);
                                entity.AdvanceState(TransactionState.Active);
                                break;
                            case ArtifactOperationKind.Delete:
                                delete(id);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Parent.TraceSource.Transaction_Log_Replay_Failure(Parent.Uri, artifact.Value.OperationKind.ToString(), kind.ToString(), id, ex);
                        if (!OnEntityReplayFailed(id, ex))
                            failures.Add(ex);
                    }

#pragma warning restore CA1031
#pragma warning restore IDE0079
                }
            }

            public async Task UnloadAsync(IProgress<int> progress)
            {
                await PauseSchedulerAsync().ConfigureAwait(false);

                var errors = new List<Exception>();

                UnloadEntities(errors);

                FailSafe(Parent.Scheduler.Dispose, errors);
                // REVIEW: QE instance may be clearing a global resource
                FailSafe(Parent.Options.ExpressionPolicy.DelegateCache.Clear, errors);

                if (errors.Count > 0)
                {
                    throw new AggregateException(errors);
                }
            }

            private void UnloadEntities(List<Exception> errors)
            {
                var unloader = SubscriptionVisitor.Do<IUnloadableOperator>(op => op.Unload());

                foreach (var sub in Registry.Subscriptions.Values)
                {
                    var instance = sub.Instance;
                    if (instance != null)
                    {
                        using (sub.Measure(EntityMetric.Unload))
                        {
                            FailSafe(() => unloader.Apply(instance), errors);
                        }
                    }
                }

                foreach (var sub in Registry.ReliableSubscriptions.Values)
                {
                    var instance = sub.Instance;
                    if (instance != null)
                    {
                        using (sub.Measure(EntityMetric.Unload))
                        {
                            FailSafe(() => unloader.Apply(instance), errors);
                        }
                    }
                }

                /*
                 * NB: We had ;ots of noise due to bridge cleanup failures. At least during unload we shouldn't care about these.
                 *     We should revisit what unloading subjects means. Dispose isn't the right thing anyway.
                 */
                foreach (var sub in Registry.Subjects.Values)
                {
                    if (sub.Instance is ISubscription subOperator)
                    {
                        using (sub.Measure(EntityMetric.Unload))
                        {
                            FailSafe(() => unloader.Apply(subOperator), errors);
                        }
                    }
                }

                FailSafe(Registry.Clear, errors);
            }

            private async Task PauseSchedulerAsync()
            {
                OnSchedulerPausing();

                await Parent.Scheduler.PauseAsync().ConfigureAwait(false);

                OnSchedulerPaused();
            }

            private void CheckCanaryRecovery()
            {
                var context = Parent._reactiveService;

                if (!context.Observables.TryGetValue(s_observableCanaryUri, out IReactiveObservableDefinition observableCanaryDefinition))
                {
                    Parent.TraceSource.Canary_NotRecovered_Observable(s_observableCanaryUri, Parent.Uri);

                    Expression<Func<IReactiveQbservable<int>>> observableCanaryFactory = () => ReactiveQbservable.Empty<int>();
                    var observableCanary = context.Provider.CreateQbservable<int>(observableCanaryFactory.Body);
                    context.DefineObservable<int>(s_observableCanaryUri, observableCanary, state: null);

                    Parent.TraceSource.Canary_Created_Observable(s_observableCanaryUri, Parent.Uri);
                }

                if (!context.Observers.TryGetValue(s_observerCanaryUri, out IReactiveObserverDefinition observerCanaryDefinition))
                {
                    Parent.TraceSource.Canary_NotRecovered_Observer(s_observerCanaryUri, Parent.Uri);

                    Expression<Func<IReactiveQbserver<int>>> observerCanaryFactory = () => ReactiveQbserver.Nop<int>();
                    var observerCanary = context.Provider.CreateQbserver<int>(observerCanaryFactory.Body);
                    context.DefineObserver<int>(s_observerCanaryUri, observerCanary, state: null);

                    Parent.TraceSource.Canary_Created_Observer(s_observerCanaryUri, Parent.Uri);
                }

                if (!context.Subscriptions.TryGetValue(s_subscriptionCanaryUri, out IReactiveSubscriptionProcess canary))
                {
                    Parent.TraceSource.Canary_NotRecovered_Subscription(s_subscriptionCanaryUri, Parent.Uri);

                    context.GetObservable<int>(s_observableCanaryUri).Subscribe(context.GetObserver<int>(s_observerCanaryUri), s_subscriptionCanaryUri, state: null);

                    Parent.TraceSource.Canary_Created_Subscription(s_subscriptionCanaryUri, Parent.Uri);
                }
            }

            private void StartTemplateMigration()
            {
                var quota = Parent.Options.TemplatizeRecoveredEntitiesQuota;
                var pattern = Parent.Options.TemplatizeRecoveredEntitiesRegex ?? ".";

                if (quota > 0 && Parent.Options.TemplatizeExpressions)
                {
                    if (TryParse(pattern, out Regex regex))
                    {
                        var task = new TemplateMigrationTask(Parent, _templatizer, regex, quota, Parent.TraceSource);
                        Parent.Scheduler.Schedule(task);
                        Parent.TraceSource.TemplateMigration_Started(Parent.Uri, pattern);
                        _migrationTask = task;
                    }
                    else
                    {
                        Parent.TraceSource.TemplateMigration_RegexInvalid(Parent.Uri, Parent.Options.TemplatizeRecoveredEntitiesRegex);
                    }
                }
            }

            private void ContinueScheduler([CallerMemberName] string caller = "")
            {
                if (!Parent.HasUnloadStarted)
                {
                    OnSchedulerContinuing();

                    Parent.Scheduler.Continue();

                    OnSchedulerContinued();
                }
                else
                {
                    Parent.TraceSource.Checkpoint_ContinueScheduler_Unloaded(Parent.Uri, caller);
                }
            }

            private static class Category
            {
                public const string SubjectFactories = "SubjectFactories";
                public const string SubscriptionFactories = "SubscriptionFactories";
                public const string Observables = "Observables";
                public const string Observers = "Observers";
                public const string Subjects = "Subjects";
                public const string SubjectsRuntimeState = "SubjectsRuntimeState";
                public const string Subscriptions = "Subscriptions";
                public const string SubscriptionsRuntimeState = "SubscriptionsRuntimeState";
                public const string ReliableSubscriptions = "ReliableSubscriptions";
                public const string Templates = "Templates";
                public const string Other = "Other";
            }

            private static class StateOperationTracer
            {
                private static readonly Stopwatch s_timer = Stopwatch.StartNew();

                public static void Trace(TraceSource trace, TraceVerb verb, TraceNoun noun, Uri entityId, Action action)
                {
                    try
                    {
                        var start = s_timer.Elapsed;

                        action();

                        var elapsed = s_timer.Elapsed - start;

                        trace.StateOperation_Executed(verb, noun, entityId, (long)elapsed.TotalMilliseconds);
                    }
                    catch (Exception ex) when (Trace(trace, verb, noun, entityId, ex))
                    {
                        throw; // NB: Unreachable code.
                    }
                }

                private static bool Trace(TraceSource trace, TraceVerb verb, TraceNoun noun, Uri entityId, Exception ex)
                {
                    if (ex is EntityAlreadyExistsException)
                    {
                        trace.StateOperation_Warning(verb, noun, entityId, ex);
                    }
                    else if (ex is not MitigationBailOutException)
                    {
                        trace.StateOperation_Error(verb, noun, entityId, ex);
                    }

                    return false;
                }
            }
        }
    }
}
