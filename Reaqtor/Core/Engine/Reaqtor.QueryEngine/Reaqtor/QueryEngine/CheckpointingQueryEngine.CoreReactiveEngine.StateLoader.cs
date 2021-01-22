// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;

using Reaqtor.Metadata;
using Reaqtor.QueryEngine.Events;
using Reaqtor.QueryEngine.Metrics;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        private partial class CoreReactiveEngine
        {
            /// <summary>
            /// State loader to read from a <see cref="IStateReader"/> and to repopulate an engine's registry.
            /// </summary>
            private sealed class StateLoader : IDisposable
            {
                private readonly IStateReader _reader;
                private readonly IQueryEngineRegistry _registry;
                private readonly CoreReactiveEngine _engine;
                private readonly ReactiveEntityRecoveryFailureMitigator _placeholderMitigator;

                /// <summary>
                /// Initializes a new instance of the <see cref="StateLoader"/> class.
                /// </summary>
                /// <param name="reader">The reader.</param>
                /// <param name="engine">The engine.</param>
                public StateLoader(IStateReader reader, CoreReactiveEngine engine)
                {
                    Debug.Assert(reader != null, "Reader should not be null.");
                    Debug.Assert(engine != null, "Engine should not be null.");

                    _reader = reader;
                    _engine = engine;
                    _registry = new ShadowRegistry();
                    _placeholderMitigator = new PlaceholderMitigator(engine);
                }

                public void Dispose()
                {
                    _registry.Dispose();
                }

                /// <summary>
                /// Loads the state.
                /// </summary>
                public void Load(CancellationToken token)
                {
                    token.ThrowIfCancellationRequested();

                    Debug.Assert(_registry != null, "Registry should not be null.");
                    Debug.Assert(_reader != null, "Reader should not be null.");

                    var unhandled = new ConcurrentBag<Exception>();

                    using (var blobLogger = new BlobLogger(_engine, token))
                    {
                        Load(blobLogger, unhandled.Add, token);
                        Start(unhandled.Add, token);
                        Summarize();
                    }

                    if (!unhandled.IsEmpty)
                    {
                        throw new AggregateException(unhandled);
                    }
                }

                private void Load(BlobLogger blobLogger, Action<Exception> unhandled, CancellationToken token)
                {
                    var sw = Stopwatch.StartNew();

                    var trace = _engine.Parent.TraceSource;

                    var onError = new Func<ReactiveEntityKind, Action<string, IReactiveResource, Exception>>(kind => (key, entity, ex) =>
                    {
                        HandleError(kind, key, entity, ex, unhandled);
                    });

                    trace.Recovery_LoadStarted(_engine.Parent.Uri);

                    LoadDefinitions<OtherDefinitionEntity>(Category.Templates, ReactiveEntityKind.Other, template =>
                    {
                        StateOperationTracer.Trace(trace, TraceVerb.Load, TraceNoun.Other | TraceNoun.Definition, template.Uri, () =>
                        {
                            Add(_engine.Registry.Other, template);

                            // TODO: add more phasing so we can populate the shadow registry first, check consistency, define/create, check more consistency, start
                            Add(_registry.Other, template);
                            template.AdvanceState(TransactionState.Active);
                        });
                    }, onError(ReactiveEntityKind.Other), blobLogger, token);

                    LoadDefinitions<OtherDefinitionEntity>(Category.Templates, ReactiveEntityKind.Other, template =>
                    {
                        StateOperationTracer.Trace(trace, TraceVerb.Load, TraceNoun.Template | TraceNoun.Definition, template.Uri, () =>
                        {
                            Add(_engine.Registry.Templates, template);

                            // TODO: add more phasing so we can populate the shadow registry first, check consistency, define/create, check more consistency, start
                            Add(_registry.Templates, template);
                            template.AdvanceState(TransactionState.Active);
                        });
                    }, onError(ReactiveEntityKind.Template), blobLogger, token);

                    LoadDefinitions<ObserverDefinitionEntity>(Category.Observers, ReactiveEntityKind.Observer, observer =>
                    {
                        StateOperationTracer.Trace(trace, TraceVerb.Load, TraceNoun.Observer | TraceNoun.Definition, observer.Uri, () =>
                        {
                            _engine.DefineObserverCore(observer);

                            // TODO: add more phasing so we can populate the shadow registry first, check consistency, define/create, check more consistency, start
                            Add(_registry.Observers, observer);
                            observer.AdvanceState(TransactionState.Active);
                        });
                    }, onError(ReactiveEntityKind.Observer), blobLogger, token);

                    LoadDefinitions<ObservableDefinitionEntity>(Category.Observables, ReactiveEntityKind.Observable, observable =>
                    {
                        StateOperationTracer.Trace(trace, TraceVerb.Load, TraceNoun.Observable | TraceNoun.Definition, observable.Uri, () =>
                        {
                            _engine.DefineObservableCore(observable);

                            // TODO: add more phasing so we can populate the shadow registry first, check consistency, define/create, check more consistency, start
                            Add(_registry.Observables, observable);
                            observable.AdvanceState(TransactionState.Active);
                        });
                    }, onError(ReactiveEntityKind.Observable), blobLogger, token);

                    LoadDefinitions<StreamFactoryDefinitionEntity>(Category.SubjectFactories, ReactiveEntityKind.StreamFactory, factory =>
                    {
                        StateOperationTracer.Trace(trace, TraceVerb.Load, TraceNoun.SubjectFactory | TraceNoun.Definition, factory.Uri, () =>
                        {
                            _engine.DefineSubjectFactoryCore(factory);

                            // TODO: add more phasing so we can populate the shadow registry first, check consistency, define/create, check more consistency, start
                            Add(_registry.SubjectFactories, factory);
                            factory.AdvanceState(TransactionState.Active);
                        });
                    }, onError(ReactiveEntityKind.StreamFactory), blobLogger, token);

                    LoadDefinitions<SubscriptionFactoryDefinitionEntity>(Category.SubscriptionFactories, ReactiveEntityKind.SubscriptionFactory, factory =>
                    {
                        StateOperationTracer.Trace(trace, TraceVerb.Load, TraceNoun.SubscriptionFactory | TraceNoun.Definition, factory.Uri, () =>
                        {
                            _engine.DefineSubscriptionFactoryCore(factory);

                            // TODO: add more phasing so we can populate the shadow registry first, check consistency, define/create, check more consistency, start
                            Add(_registry.SubscriptionFactories, factory);
                            factory.AdvanceState(TransactionState.Active);
                        });
                    }, onError(ReactiveEntityKind.SubscriptionFactory), blobLogger, token);

                    LoadStatefulEntities<SubjectEntity>(Category.Subjects, Category.SubjectsRuntimeState, ReactiveEntityKind.Stream, (subject, stream) =>
                    {
                        StateOperationTracer.Trace(trace, TraceVerb.Load, TraceNoun.Subject | TraceNoun.State, subject.Uri, () =>
                        {
                            if (stream == null)
                            {
                                trace.Recovery_LoadSubjectWithoutState(subject.Uri, _engine.Parent.Uri);

                                _engine.CreateStreamCore(subject, state: null, recovering: true);
                            }
                            else
                            {
                                var policy = _engine.Parent._serializationPolicy;

                                using var stateReaderFactory = new OperatorStateReaderFactory(stream, policy);

                                stateReaderFactory.ReadHeader();

                                _engine.CreateStreamCore(subject, state: stateReaderFactory, recovering: true);

                                stateReaderFactory.ReadFooter();
                            }

                            // TODO: add more phasing so we can populate the shadow registry first, check consistency, define/create, check more consistency, start
                            Add(_registry.Subjects, subject);
                            subject.AdvanceState(TransactionState.Active);
                        });
                    }, onError(ReactiveEntityKind.Stream), blobLogger, token);

                    LoadDefinitions<ReliableSubscriptionEntity>(Category.ReliableSubscriptions, ReactiveEntityKind.ReliableSubscription, reliableSub =>
                    {
                        // Empty - start happens in the next stage; just populating the registry here

                        // TODO: add more phasing so we can populate the shadow registry first, check consistency, define/create, check more consistency, start
                        Add(_registry.ReliableSubscriptions, reliableSub);
                        reliableSub.AdvanceState(TransactionState.Active);
                    }, onError(ReactiveEntityKind.ReliableSubscription), blobLogger, token);

                    LoadStatefulEntities<SubscriptionEntity>(Category.Subscriptions, Category.SubscriptionsRuntimeState, ReactiveEntityKind.Subscription, (subscription, stream) =>
                    {
                        StateOperationTracer.Trace(trace, TraceVerb.Load, TraceNoun.Subscription | TraceNoun.State, subscription.Uri, () =>
                        {
                            //
                            // Notice we add the entity to the registry first. This enables the case where
                            // the Regenerate mitigation is used during load failures, resulting in a fresh
                            // entity in the proper QE registry, which we need to see for the subsequent
                            // call to Start made by the next phase of recovery using the shadow registry.
                            //
                            // This has one side-effect though, namely that we can end up with an entity in
                            // the shadow registry that we fail to load, but yet we'll attempt to start it.
                            // Right now, this works out fine because the entity's Instance property will
                            // not be set if the load phase fails, so the Start phase will skip it. Going
                            // forward we should mediate all state changes through the entities, such that
                            // we can keep track of state transitions. Failure to load would leave it in a
                            // LoadFailed state, preventing a Start request from transitioning to Starting
                            // and Started. With this, we may be able to move mitigations outside the core
                            // code path as well and use a registry artifact analysis to find artifacts that
                            // are in various *Failed states.
                            //

                            // TODO: add more phasing so we can populate the shadow registry first, check consistency, define/create, check more consistency, start
                            Add(_registry.Subscriptions, subscription);
                            subscription.AdvanceState(TransactionState.Active);

                            if (stream == null)
                            {
                                trace.Recovery_LoadSubscriptionWithoutState(subscription.Uri, _engine.Parent.Uri);

                                _engine.CreateSubscriptionCore(subscription, state: null, recovering: true);
                            }
                            else
                            {
                                var policy = _engine.Parent._serializationPolicy;

                                using var stateReaderFactory = new OperatorStateReaderFactory(stream, policy);

                                stateReaderFactory.ReadHeader();

                                _engine.CreateSubscriptionCore(subscription, state: stateReaderFactory, recovering: true);

                                stateReaderFactory.ReadFooter();
                            }
                        });
                    }, onError(ReactiveEntityKind.Subscription), blobLogger, token);

                    trace.Recovery_LoadCompleted(_engine.Parent.Uri, sw.ElapsedMilliseconds);
                }

                private static void Add<TEntity>(IReactiveEntityCollection<string, TEntity> collection, TEntity entity)
                    where TEntity : ReactiveEntity
                {
                    collection.Add(entity.Uri.ToCanonicalString(), entity);
                }

                private void Start(Action<Exception> unhandled, CancellationToken token)
                {
                    var sw = Stopwatch.StartNew();

                    var trace = _engine.Parent.TraceSource;

                    var onError = new Func<ReactiveEntityKind, Action<IReactiveResource, Exception>>(kind => (entity, ex) =>
                    {
                        HandleError(kind, entity.Uri.ToCanonicalString(), entity, ex, unhandled);
                    });

                    trace.Recovery_StartStarted(_engine.Parent.Uri);

                    var streamOnError = onError(ReactiveEntityKind.Stream);
                    var reliableSubscriptionOnError = onError(ReactiveEntityKind.ReliableSubscription);
                    var subscriptionOnError = onError(ReactiveEntityKind.Subscription);

                    var subjects = _registry.Subjects.Values;

                    trace.Recovery_StartSubjects(_engine.Parent.Uri, subjects.Count);

                    Parallel.ForEach(
                        subjects,
                        new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _engine.Parent.Options.RecoveryDegreeOfParallelism,
                            TaskScheduler = RecoveryScheduler.Default,
                            CancellationToken = token,
                        },
                        subject =>
                        {
                            if (subject.Instance is IOperator op)
                            {
                                try
                                {
                                    StateOperationTracer.Trace(trace, TraceVerb.Start, TraceNoun.Subject, subject.Uri, () =>
                                    {
                                        // CONSIDER: Could use Start on the entity which maintains the top-level lifecycle of the artifact.
                                        _engine.TryMitigate(
                                            () => subject.Start(),
                                            subject,
                                            true,
                                            _engine._removeMitigator);
                                    });
                                }
                                catch (MitigationBailOutException) { }
#pragma warning disable CA1031 // Do not catch general exception types. (By design; mitigation callback is the "handler".)
                                catch (Exception ex)
                                {
                                    streamOnError(subject, ex);
                                }
#pragma warning restore CA1031
                            }
                        });

                    token.ThrowIfCancellationRequested();

                    // TODO: We can also move the start logic and potential de-dup in the output edge and execute it during recovery only.
                    //       This way we can get rid of the magic (-1) sequencenumber constant.

                    // Reliable subscriptions would be recreated by the output edges on the previous step. We just have to start them here.
                    // Start from sequence number (-1) means, start from the position in the queue as of the last checkpoint.

                    var reliableSubs = _registry.ReliableSubscriptions.Values;

                    trace.Recovery_StartReliableSubscriptions(_engine.Parent.Uri, reliableSubs.Count);

                    Parallel.ForEach(
                        reliableSubs,
                        new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _engine.Parent.Options.RecoveryDegreeOfParallelism,
                            TaskScheduler = RecoveryScheduler.Default,
                            CancellationToken = token,
                        },
                        reliableSub =>
                        {
                            try
                            {
                                StateOperationTracer.Trace(trace, TraceVerb.Start, TraceNoun.ReliableSubscription, reliableSub.Uri, () =>
                                {
                                    _engine.TryMitigate(
                                        () => _engine.StartSubscription(reliableSub.Uri, -1),
                                        reliableSub,
                                        true,
                                        _engine._removeMitigator);
                                });
                            }
                            catch (MitigationBailOutException) { }
#pragma warning disable CA1031 // Do not catch general exception types. (By design; mitigation callback is the "handler".)
                            catch (Exception ex)
                            {
                                reliableSubscriptionOnError(reliableSub, ex);
                            }
#pragma warning restore CA1031
                        });

                    token.ThrowIfCancellationRequested();

                    var subscriptions = _registry.Subscriptions.Values;

                    trace.Recovery_StartSubscriptions(_engine.Parent.Uri, subscriptions.Count);

                    Parallel.ForEach(
                        subscriptions,
                        new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _engine.Parent.Options.RecoveryDegreeOfParallelism,
                            TaskScheduler = RecoveryScheduler.Default,
                            CancellationToken = token,
                        },
                        subscription =>
                        {
                            var sub = subscription.Instance;
                            if (sub != null)
                            {
                                try
                                {
                                    StateOperationTracer.Trace(trace, TraceVerb.Start, TraceNoun.Subscription, subscription.Uri, () =>
                                    {
                                        // CONSIDER: Could use Start on the entity which maintains the top-level lifecycle of the artifact.
                                        _engine.TryMitigate(
                                            () => subscription.Start(),
                                            subscription,
                                            true,
                                            _engine._removeMitigator);
                                    });
                                }
                                catch (MitigationBailOutException) { }
#pragma warning disable CA1031 // Do not catch general exception types. (By design; mitigation callback is the "handler".)
                                catch (Exception ex)
                                {
                                    subscriptionOnError(subscription, ex);
                                }
#pragma warning restore CA1031
                            }
                        });

                    token.ThrowIfCancellationRequested();

                    trace.Recovery_StartCompleted(_engine.Parent.Uri, sw.ElapsedMilliseconds);
                }

                private void HandleError(ReactiveEntityKind kind, string key, IReactiveResource entity, Exception ex, Action<Exception> unhandled)
                {
                    if (ex is EngineUnloadedException)
                        return;

                    var uri = new Uri(key);

                    var error = new EntityLoadFailedException(uri, kind, ex);

                    // TODO: support mitigation for any kind of artifact
                    if (!_engine.OnEntityLoadFailed(uri, entity, kind, error, out _))
                    {
                        unhandled(error);
                    }
                }

                /// <summary>
                /// Loads stateful entities.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="category">The category.</param>
                /// <param name="stateCategory">The state category.</param>
                /// <param name="kind">The reactive entity kind.</param>
                /// <param name="onLoading">The function is called for each loading entity.</param>
                /// <param name="onError">Function to report an error.</param>
                /// <param name="blobLogger">The blob logger to write raw recovery blobs to.</param>
                /// <param name="token">Cancellation token.</param>
                private void LoadStatefulEntities<TEntity>(
                    string category,
                    string stateCategory,
                    ReactiveEntityKind kind,
                    Action<TEntity, Stream> onLoading,
                    Action<string, TEntity, Exception> onError,
                    BlobLogger blobLogger,
                    CancellationToken token)
                    where TEntity : ReactiveEntity
                {
                    Debug.Assert(!string.IsNullOrEmpty(stateCategory), "Category should not be null or empty.");
                    Debug.Assert(onLoading != null, "onLoading should not be null.");

                    LoadDefinitions<TEntity>(category, kind, entity =>
                    {
                        var key = entity.Uri.ToCanonicalString();

                        var stopwatch = Stopwatch.StartNew();
                        if (!_reader.TryGetItemReader(stateCategory, key, out Stream stateStream))
                        {
                            // Stateless entity, i.e. no state has been written yet.
                            // At the very least, there will always be a header if anything has been written.
                            stateStream = null;
                        }
                        else
                        {
                            entity.SetMetric(EntityMetric.ReadState, stopwatch.Elapsed);

                            blobLogger.Append(stateCategory, key, stateStream);
                        }

                        using (stateStream) // notice null is fine for a C# using statement
                        {
                            try
                            {
                                onLoading(entity, stateStream);
                            }
                            catch (MitigationBailOutException) { throw; }
                            catch (Exception ex)
                            {
                                _engine.Parent.TraceSource.Recovery_LoadingStateFailure(_engine.Parent.Uri, category, stateCategory, key, ex.Message);

                                throw;
                            }
                        }
                    }, onError, blobLogger, token);
                }

                /// <summary>
                /// Loads the definitions.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="category">The category.</param>
                /// <param name="kind">Reactive entity kind.</param>
                /// <param name="onLoading">The function is called for each loading entity.</param>
                /// <param name="onError">Function to report an error.</param>
                /// <param name="blobLogger">The blob logger to write raw recovery blobs to.</param>
                /// <param name="token">Cancellation token</param>
                private void LoadDefinitions<TEntity>(
                    string category,
                    ReactiveEntityKind kind,
                    Action<TEntity> onLoading,
                    Action<string, TEntity, Exception> onError,
                    BlobLogger blobLogger,
                    CancellationToken token)
                    where TEntity : ReactiveEntity
                {
                    Debug.Assert(!string.IsNullOrEmpty(category), "Category should not be null or empty.");
                    Debug.Assert(onLoading != null, "onLoading should not be null.");

                    var trace = _engine.Parent.TraceSource;

                    trace.Recovery_LoadingDefinitionsStarted(_engine.Parent.Uri, category);

                    if (!_reader.TryGetItemKeys(category, out IEnumerable<string> entities))
                    {
                        entities = Array.Empty<string>();
                    }

                    var total = 0;
                    var failed = 0;

                    Parallel.ForEach(
                        entities,
                        new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _engine.Parent.Options.RecoveryDegreeOfParallelism,
                            TaskScheduler = RecoveryScheduler.Default,
                            CancellationToken = token,
                        },
                        key =>
                        {
                            var entity = default(TEntity);

                            try
                            {

                                var stopwatch = Stopwatch.StartNew();
                                if (!_reader.TryGetItemReader(category, key, out Stream stream))
                                {
                                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No items in key '{0}' for category '{1}'.", key, category));
                                }
                                var elapsedReading = stopwatch.Elapsed;

                                blobLogger.Append(category, key, stream);

                                var policy = _engine.Parent._serializationPolicy;

                                stopwatch.Restart();
                                using (var reader = new EntityReader(stream, _engine.Registry, policy))
                                {
                                    reader.ReadHeader();
                                    _engine.TryMitigate(
                                        () => entity = (TEntity)reader.Load(kind),
                                        ReactiveEntity.CreateInvalidInstance(new Uri(key), kind),
                                        true,
                                        _placeholderMitigator);
                                    reader.ReadFooter();
                                }
                                var elapsedLoading = stopwatch.Elapsed;

                                entity.SetMetric(EntityMetric.ReadEntity, elapsedReading);
                                entity.SetMetric(EntityMetric.LoadEntity, elapsedLoading);

                                onLoading(entity);
                            }
                            catch (MitigationBailOutException) { }
#pragma warning disable CA1031 // Do not catch general exception types. (By design; mitigation callback is the "handler".)
                            catch (Exception ex)
                            {
                                Interlocked.Increment(ref failed);

                                trace.Recovery_LoadingDefinitionsFailure(_engine.Parent.Uri, category, key, ex.Message);

                                onError(key, entity, ex);
                            }
#pragma warning restore CA1031

                            Interlocked.Increment(ref total);
                        });

                    token.ThrowIfCancellationRequested();

                    trace.Recovery_LoadingDefinitionsCompleted(_engine.Parent.Uri, category, total, failed);
                }

                private void Summarize()
                {
                    Func<IEnumerable<IReactiveResource>, string> summarizeEntities = SummarizeEntities;
                    _engine.Parent.TraceSource.Recovery_Summary("Other", _engine.Parent.Uri, _registry.Other.Values, summarizeEntities);
                    _engine.Parent.TraceSource.Recovery_Summary("Templates", _engine.Parent.Uri, _registry.Templates.Values, summarizeEntities);
                    _engine.Parent.TraceSource.Recovery_Summary("Observables", _engine.Parent.Uri, _registry.Observables.Values, summarizeEntities);
                    _engine.Parent.TraceSource.Recovery_Summary("Observers", _engine.Parent.Uri, _registry.Observers.Values, summarizeEntities);
                    _engine.Parent.TraceSource.Recovery_Summary("Stream Factories", _engine.Parent.Uri, _registry.SubjectFactories.Values, summarizeEntities);
                    _engine.Parent.TraceSource.Recovery_Summary("Streams", _engine.Parent.Uri, _registry.Subjects.Values, summarizeEntities);
                    _engine.Parent.TraceSource.Recovery_Summary("Reliable Subscriptions", _engine.Parent.Uri, _registry.ReliableSubscriptions.Values, summarizeEntities);
                    _engine.Parent.TraceSource.Recovery_Summary("Subscriptions", _engine.Parent.Uri, _registry.Subscriptions.Values, summarizeEntities);
                    _engine.Parent.TraceSource.Recovery_Summary("Subscription Factories", _engine.Parent.Uri, _registry.SubscriptionFactories.Values, summarizeEntities);
                }

                private static string SummarizeEntities(IEnumerable<IReactiveResource> entities)
                {
                    var distinctMetrics = from e in entities
                                          from m in e.GetMetrics()
                                          group m by m.Key;

                    var list = new List<string>();

                    foreach (var group in distinctMetrics)
                    {
                        var metric = group.Key;

                        var curr = group.Select(kv => kv.Value.TotalMilliseconds).OrderBy(x => x).ToList();

                        var count = curr.Count;
                        var min = curr[0];
                        var max = curr[count - 1];
                        var avg = curr.Average();
                        var vari = curr.Average(c => Math.Pow(c - avg, 2));
                        var pct90 = Percentile(curr, 0.9);
                        var pct95 = Percentile(curr, 0.95);
                        var pct99 = Percentile(curr, 0.99);

                        list.Add(string.Format(CultureInfo.InvariantCulture, "Metric({0}): Count({1}), Min({2}), Max({3}), Avg({4}), Variance({5}), P90({6}), P95({7}), P99({8})", metric, count, min, max, avg, vari, pct90, pct95, pct99));
                    }

                    return string.Join("; ", list);

                    static double Percentile(List<double> metrics, double percentile)
                    {
                        return metrics[(int)((metrics.Count - 1) * percentile)];
                    }
                }

                private sealed class ShadowRegistry : IQueryEngineRegistry
                {
                    public ShadowRegistry()
                    {
                        Observables = new ReactiveEntityCollection<string, ObservableDefinitionEntity>(StringComparer.Ordinal);
                        Observers = new ReactiveEntityCollection<string, ObserverDefinitionEntity>(StringComparer.Ordinal);
                        SubjectFactories = new ReactiveEntityCollection<string, StreamFactoryDefinitionEntity>(StringComparer.Ordinal);
                        SubscriptionFactories = new ReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity>(StringComparer.Ordinal);
                        Other = new ReactiveEntityCollection<string, DefinitionEntity>(StringComparer.Ordinal);
                        Templates = new InvertedLookupReactiveEntityCollection<string, DefinitionEntity>(StringComparer.Ordinal, InvertedDefinitionEntityComparer.Default);
                        Subscriptions = new ReactiveEntityCollection<string, SubscriptionEntity>(StringComparer.Ordinal);
                        Subjects = new ReactiveEntityCollection<string, SubjectEntity>(StringComparer.Ordinal);
                        ReliableSubscriptions = new ReactiveEntityCollection<string, ReliableSubscriptionEntity>(StringComparer.Ordinal);
                    }

                    public IReactiveEntityCollection<string, ObservableDefinitionEntity> Observables { get; }

                    public IReactiveEntityCollection<string, ObserverDefinitionEntity> Observers { get; }

                    public IReactiveEntityCollection<string, StreamFactoryDefinitionEntity> SubjectFactories { get; }

                    public IReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity> SubscriptionFactories { get; }

                    public IReactiveEntityCollection<string, DefinitionEntity> Other { get; }

                    public IInvertedLookupReactiveEntityCollection<string, DefinitionEntity> Templates { get; }

                    public IReactiveEntityCollection<string, SubscriptionEntity> Subscriptions { get; }

                    public IReactiveEntityCollection<string, SubjectEntity> Subjects { get; }

                    public IReactiveEntityCollection<string, ReliableSubscriptionEntity> ReliableSubscriptions { get; }

                    public void Clear()
                    {
                        Observables.Clear();
                        Observers.Clear();
                        SubjectFactories.Clear();
                        SubscriptionFactories.Clear();
                        Other.Clear();
                        Subscriptions.Clear();
                        Subjects.Clear();
                        ReliableSubscriptions.Clear();
                    }

                    public void Dispose()
                    {
                        Observables.Dispose();
                        Observers.Dispose();
                        SubjectFactories.Dispose();
                        SubscriptionFactories.Dispose();
                        Other.Dispose();
                        Templates.Dispose();
                        Subscriptions.Dispose();
                        Subjects.Dispose();
                        ReliableSubscriptions.Dispose();
                    }
                }

                private sealed class BlobLogger : IDisposable
                {
#pragma warning disable CA2213 // Disposable fields should be disposed. (See remarks in Dispose.)
                    private readonly BlockingCollection<string> _blobLogs;
#pragma warning restore CA2213
                    private readonly CancellationToken _token;

                    public BlobLogger(CoreReactiveEngine engine, CancellationToken token)
                    {
                        _token = token;

                        var options = engine.Parent.Options;
                        var engineUri = engine.Parent.Uri;
                        var trace = engine.Parent.TraceSource;

#pragma warning disable CA2000 // Dispose objects before losing scope. (See remarks in Dispose.)
                        if (options.DumpRecoveryStateBlobs && TryGetBlobLogStreamWriter(options.DumpRecoveryStatePath, engineUri, trace, out var blobLogPath, out var stream))
                        {
                            _blobLogs = new BlockingCollection<string>(new ConcurrentQueue<string>());

                            WriteBlobLogsAsync(stream, token).ContinueWith(t =>
                            {
                                if (t.IsCanceled)
                                {
                                    trace.BlobLogs_Done_Canceled(blobLogPath, engineUri);
                                }
                                else if (t.Exception != null)
                                {
                                    trace.BlobLogs_Done_Error(blobLogPath, engineUri, t.Exception);
                                }
                                else
                                {
                                    trace.BlobLogs_Done_Success(blobLogPath, engineUri);
                                }
                            }, token, TaskContinuationOptions.None, TaskScheduler.Default);
                        }
#pragma warning restore CA2000
                    }

                    public void Dispose()
                    {
                        //
                        // NB: Disposal of the blob logs blocking collection is managed by the continuation of the blob logging task, see WriteBlobLogsAsync.
                        //

                        try
                        {
                            _blobLogs?.CompleteAdding();
                        }
                        catch (ObjectDisposedException)
                        {
                            //
                            // NB: Any error in the WriteBlobLogsAsyncCore method will trigger disposal, including the case where recovery gets cancelled.
                            //
                            //     We shouldn't fail recovery because of this, and a failure to call CompleteAdding is not an issue because the enumeration
                            //     in WriteBlobLogsAsyncCore has exited, causing us to observe the disposed exception here.
                            //
                        }
                    }

                    public void Append(string category, string key, Stream stateStream)
                    {
                        if (_blobLogs != null)
                        {
                            string trace;

                            using (var writer = new StringWriter())
                            {
                                writer.WriteLine(category + "\t" + key);
                                writer.WriteLine(stateStream.GetBase64Blob());

                                trace = writer.ToString();
                            }

#pragma warning disable CA1031 // Do not catch general exception types. (Use of filter is reasonable here.)
                            try
                            {
                                _blobLogs.Add(trace);
                            }
                            catch when (_token.IsCancellationRequested)
                            {
                                //
                                // NB: Cancellation can be observed by the blob logger for early bail-out.
                                //
                            }
#pragma warning restore CA1031
                        }
                    }

                    private static bool TryGetBlobLogStreamWriter(string blobDir, Uri engineUri, TraceSource trace, out string blobLogPath, out StreamWriter stream)
                    {
                        var dt = DateTime.UtcNow;

                        blobLogPath = dt.ToString("'blobs_'yyyyMMdd'_'HHmmssfff'.log'", CultureInfo.InvariantCulture);
                        stream = default;

#pragma warning disable CA1031 // Do not catch general exception types. (By design; we don't want to take an engine down due to failed blob logging.)
                        try
                        {
                            if (blobDir != null)
                            {
                                var qeEncoded = Uri.EscapeDataString(engineUri.ToCanonicalString());
                                var blobDirWithQe = Path.Combine(blobDir, qeEncoded);
                                if (!Directory.Exists(blobDirWithQe))
                                {
                                    Directory.CreateDirectory(blobDirWithQe);
                                }

                                blobLogPath = Path.Combine(blobDirWithQe, blobLogPath);
                            }

                            stream = File.CreateText(blobLogPath);

                            trace.BlobLogs_Created(blobLogPath, engineUri);

                            return true;
                        }
                        catch (Exception ex)
                        {
                            trace.BlobLogs_CreateFailed(blobLogPath, engineUri, ex);
                        }
#pragma warning restore CA1031

                        return false;
                    }

                    private Task WriteBlobLogsAsync(StreamWriter stream, CancellationToken token)
                    {
                        //
                        // NB: GetConsumingEnumerable in WriteBlobLogsAsyncCore will block until the first element is produced;
                        //     if we were to wait for this, the BlobLogger constructor would deadlock, so we spawn a background task.
                        //
                        return Task.Factory.StartNew(() => WriteBlobLogsAsyncCore(stream, token), token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                                           .Unwrap();
                    }

                    private async Task WriteBlobLogsAsyncCore(StreamWriter stream, CancellationToken token)
                    {
                        Debug.Assert(stream != null);
                        Debug.Assert(_blobLogs != null);

                        //
                        // NB: This disposes semaphore resources in the blocking collection. We can get here due to cancellation
                        //     or successful completion (where Dispose causes use to break from the loop). In the former case,
                        //     concurrent writes to the blob logs can fail, which we guard against in Append.
                        //
                        using (_blobLogs)
                        {
                            //
                            // NB: Stream is exclusively accessed here; after finishing the loop, we can dispose it.
                            //
                            using (stream)
                            {
                                foreach (var blob in _blobLogs.GetConsumingEnumerable(token))
                                {
                                    token.ThrowIfCancellationRequested(); // Try to bail out sooner on cancellation of recovery.

                                    await stream.WriteLineAsync(blob).ConfigureAwait(false);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
