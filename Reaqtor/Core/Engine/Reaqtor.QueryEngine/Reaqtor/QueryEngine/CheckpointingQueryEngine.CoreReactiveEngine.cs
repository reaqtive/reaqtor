// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;
using Reaqtive.Expressions;

using Reaqtor.Expressions.Core;
using Reaqtor.QueryEngine.Events;
using Reaqtor.QueryEngine.Metrics;
using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        private sealed partial class CoreReactiveEngine : ICheckpointable
        {
            private static readonly MethodInfo s_createInputMethod = ((MethodInfo)ReflectionHelpers.InfoOf((CoreReactiveEngine e) => e.CreateInput<object>(null))).GetGenericMethodDefinition();
            private static readonly MethodInfo s_createOutputMethod = ((MethodInfo)ReflectionHelpers.InfoOf((CoreReactiveEngine e) => e.CreateOutput<object>(null))).GetGenericMethodDefinition();
            private static readonly MethodInfo s_createObserverForSubjectMethod = ((MethodInfo)ReflectionHelpers.InfoOf(() => CoreReactiveEngine.CreateObserverForSubject<object, object>(null))).GetGenericMethodDefinition();
            private readonly FullBinder _fullBinder;
            private readonly DefinitionInliningBinder _inliningBinder;
            private readonly ReliableEdgeBinder _edgeBinder;
            private readonly EdgeRewriter _edgeRewriter;
            private readonly RemoveMitigator _removeMitigator;
            private readonly GarbageCollector _garbageCollector;

            // TODO: Implement a cache eviction strategy.
            private readonly QueryEngineRegistryTemplatizer _templatizer;

            private TemplateMigrationTask _migrationTask;

            public CoreReactiveEngine(CheckpointingQueryEngine queryEngine)
            {
                Debug.Assert(queryEngine != null);

                Parent = queryEngine;

                _fullBinder = new FullBinder(Registry);
                _inliningBinder = new DefinitionInliningBinder(Registry, Parent.LookupForeignFunction);
                _edgeBinder = new ReliableEdgeBinder(Registry, Parent._serviceResolver);
                _edgeRewriter = new EdgeRewriter(queryEngine.Uri.OriginalString, Registry);
                MetadataQueryProvider = new RegistryQueryProvider(Registry);
                _removeMitigator = new RemoveMitigator(this);
                _garbageCollector = new GarbageCollector(queryEngine, RemoveEntitySafe);

                _templatizer = new QueryEngineRegistryTemplatizer(queryEngine._registry);
            }

            public CheckpointingQueryEngine Parent { get; private set; }

            public IReactiveExpressionServices ExpressionService => Parent._expressionService;

            public QueryEngineRegistry Registry => Parent._registry;

            public IQueryProvider MetadataQueryProvider { get; private set; }

            #region Subscription

            // TODO We need to remove this lock and use the scheduler instead
            // Prevents checkpoints and recoveries from interrupting a creation operation.
            private readonly AsyncReaderWriterLock _checkpointAndRecoverLock = new();

            public Task CreateSubscriptionAsync(Uri subscriptionUri, Expression subscription, object state, CancellationToken token)
            {
                return CreateArtifactAsync(
                    subscriptionUri,
                    subscription,
                    state,
                    token,
                    CreateSubscription,
                    DeleteSubscription,
                    Registry.Subscriptions,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).Subscriptions
                );
            }

            public Task DeleteSubscriptionAsync(Uri subscriptionUri, CancellationToken token)
            {
                return DeleteArtifactAsync(
                    subscriptionUri,
                    token,
                    ReactiveEntityKind.Subscription,
                    DeleteSubscription,
                    Registry.Subscriptions,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).Subscriptions
                );
            }

            public void CreateSubscription(Uri subscriptionUri, Expression subscription, object state)
            {
                Debug.Assert(subscriptionUri != null);
                Debug.Assert(subscription != null);

                if (IsEdgeSubscription(subscription))
                {
                    Expression reliable = RewriteSubscriptionToReliableSubscription(ExpressionService, subscription);
                    CreateReliableSubscription(subscriptionUri, reliable, state);
                }
                else
                {
                    var stopwatch = Stopwatch.StartNew();
                    Expression inlined = _inliningBinder.Bind(subscription);
                    var elapsedInlining = stopwatch.Elapsed;

                    stopwatch.Restart();
                    var delegator = new DelegationBinder(Registry);
                    var delegated = delegator.Bind(inlined);
                    var elapsedDelegating = stopwatch.Elapsed;

                    Expression edgeless = _edgeRewriter.Rewrite(delegated, out IEnumerable<EdgeDescription> edges);

                    bool edgesCreated = false;
                    bool success = false;

                    try
                    {
                        EdgeDescription outputEdge = CreateEdges(subscriptionUri, edges);
                        edgesCreated = true;

                        if (outputEdge != null)
                        {
                            Debug.Assert(outputEdge.InternalSubscriptionUri != null);
                            subscriptionUri = outputEdge.InternalSubscriptionUri;
                        }

                        var templatized = edgeless;
                        var elapsedTemplatizing = default(TimeSpan);
                        if (Parent.Options.TemplatizeExpressions)
                        {
                            stopwatch.Restart();
                            templatized = _templatizer.Templatize(edgeless);
                            elapsedTemplatizing = stopwatch.Elapsed;
                        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Entity will be owned by the registry after Create completes.)

                        var entity = new SubscriptionEntity(subscriptionUri, templatized, state);
                        entity.SetMetric(EntityMetric.Inline, elapsedInlining);
                        entity.SetMetric(EntityMetric.Delegate, elapsedDelegating);
                        entity.SetMetric(EntityMetric.Templatize, elapsedTemplatizing);

                        CreateSubscriptionCore(entity, state: null, recovering: false);

#pragma warning restore CA2000
#pragma warning restore IDE0079

                        success = true;
                    }
                    finally
                    {
                        if (!success)
                        {
                            if (edgesCreated && edges != null)
                            {
                                foreach (var edge in edges)
                                {
                                    DeleteStream(edge.InternalUri);
                                }
                            }
                        }
                    }
                }
            }

            private void CreateSubscriptionCore(SubscriptionEntity entity, IOperatorStateReaderFactory state, bool recovering)
            {
                var instance = default(ISubscription);
                bool subscriptionAdded = false;
                bool success = false;

                try
                {
                    entity.Cache(Parent.Options.ExpressionPolicy.InMemoryCache);

                    // Add the subscription to the registry as it might get disposed and removed while it is being created.
                    if (!Registry.Subscriptions.TryAdd(entity.Uri.ToCanonicalString(), entity))
                    {
                        string paramName = "subscriptionUri"; // NB: Workaround for CA2208 complaining that subscriptionUri is not a parameter of this method.
                        throw new EntityAlreadyExistsException(entity.Uri, ReactiveEntityKind.Subscription, Parent.Uri, paramName);
                    }

                    subscriptionAdded = true;

                    using (entity.Measure(EntityMetric.Evaluate))
                    {
                        TryMitigate(
                            () =>
                            {
                                instance = EvaluateSubscription(entity);
                            },
                            entity,
                            recovering,
                            _removeMitigator);
                    }

                    TryMitigate(
                        () =>
                        {
                            InitializeSubscription(entity, instance, state, recovering);
                        },
                        entity,
                        recovering,
                        new FullMitigator(_ =>
                        {
                            instance = EvaluateSubscription(entity);
                            InitializeSubscription(entity, instance, state: null, recovering);
                            success = true;
                            OnEntityCreated(new SubscriptionEventArgs(entity));
                        }, this));

                    if (!success)
                    {
                        success = true;
                        OnEntityCreated(new SubscriptionEventArgs(entity));
                    }
                }
                finally
                {
                    if (!success && !recovering)
                    {
                        // If we're recovering, failure to reinitialize the subscription should not have the effect of a destructive
                        // deletion in the registry, which will cause the checkpoint state to get discarded. Subsequent recovery
                        // attempts can retry the recovery, e.g. after a bug fix. We may need to revisit this behavior, e.g. to
                        // restart a subscription with an empty state in case a recovery failure occurs, based on a host-controlled
                        // policy or something.
                        if (subscriptionAdded)
                        {
                            DeleteSubscription(entity.Uri);
                        }

                        // If the subscription failed to load runtime state or to start, the instance may have been created, but
                        // not been assigned to the Instance property of the RuntimeEntity.  If that is the case, the call to
                        // `DeleteSubscription` will not result in the instance being disposed.
                        // Disposing the instance deletes the edges, too.
                        instance?.Dispose();
                    }
                }
            }

            private ISubscription EvaluateSubscription(SubscriptionEntity entity)
            {
                var detemplatized = _templatizer.Detemplatize(entity.Expression);
                var bound = _fullBinder.Bind(detemplatized);
                var quoted = Parent.Options.Quoter.Visit(bound);
                var instance = Evaluate<ISubscription>(quoted);

                return instance;
            }

            public void DeleteSubscription(Uri subscriptionUri)
            {
                Debug.Assert(subscriptionUri != null);

                if (!Registry.Subscriptions.TryRemove(subscriptionUri.ToCanonicalString(), out SubscriptionEntity entity))
                {
                    throw new EntityNotFoundException(subscriptionUri, ReactiveEntityKind.Subscription, Parent.Uri, nameof(subscriptionUri));
                }

                Debug.Assert(entity != null);
                entity.Dispose();

                OnEntityDeleted(new SubscriptionEventArgs(entity));
            }

            // TODO: Merge with CreateSubscription and execute in case of subscriptions to rebindable stream.
            public void CreateReliableSubscription(Uri subscriptionUri, Expression subscription, object state)
            {
                Debug.Assert(subscriptionUri != null);
                Debug.Assert(subscription != null);

                var stopwatch = Stopwatch.StartNew();
                Expression inlined = _inliningBinder.Bind(subscription);
                var elapsedInlining = stopwatch.Elapsed;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Entity will be owned by the registry after Create completes.)

                var entity = new ReliableSubscriptionEntity(subscriptionUri, inlined, state);
                entity.SetMetric(EntityMetric.Inline, elapsedInlining);

                CreateReliableSubscriptionCore(entity);

#pragma warning restore CA2000
#pragma warning restore IDE0079

                void CreateReliableSubscriptionCore(ReliableSubscriptionEntity entity)
                {
                    Expression bound = _edgeBinder.Bind(entity.Expression);

                    // First add the subscription to the registry as it might get disposed and removed while it is being created.
                    if (!Registry.ReliableSubscriptions.TryAdd(entity.Uri.ToCanonicalString(), entity))
                    {
                        throw new EntityAlreadyExistsException(entity.Uri, ReactiveEntityKind.ReliableSubscription, Parent.Uri, nameof(subscriptionUri));
                    }

                    bool success = false;

                    try
                    {
                        entity.Cache(Parent.Options.ExpressionPolicy.InMemoryCache);

                        using (entity.Measure(EntityMetric.Evaluate))
                        {
                            var instance = (IReliableSubscription)bound.Evaluate();
                            var context = Parent.CreateOperatorContext(entity.Uri);
                            SubscriptionInitializeVisitor.Subscribe(instance);
                            SubscriptionInitializeVisitor.SetContext(instance, context);
                            entity.Instance = instance;
                        }

                        success = true;

                        OnEntityCreated(new ReliableSubscriptionEventArgs(entity));
                    }
                    finally
                    {
                        if (!success)
                        {
                            DeleteReliableSubscription(entity.Uri);
                        }
                    }
                }
            }

            // TODO: Merge with DeleteSubscription and execute in case of subscriptions to rebindable stream.
            public void DeleteReliableSubscription(Uri subscriptionUri)
            {
                Debug.Assert(subscriptionUri != null);

                if (!Registry.ReliableSubscriptions.TryRemove(subscriptionUri.ToCanonicalString(), out ReliableSubscriptionEntity entity))
                {
                    throw new EntityNotFoundException(subscriptionUri, ReactiveEntityKind.ReliableSubscription, Parent.Uri, nameof(subscriptionUri));
                }

                Debug.Assert(entity != null);
                entity.Dispose();

                OnEntityDeleted(new ReliableSubscriptionEventArgs(entity));
            }

            public void StartSubscription(Uri subscriptionUri, long sequenceId)
            {
                var entity = GetReliableSubscription(subscriptionUri);

                // CONSIDER: Could use Start on the entity which maintains the top-level lifecycle of the artifact.
                entity.Start(sequenceId);
            }

            public void AcknowledgeRange(Uri subscriptionUri, long sequenceId)
            {
                GetReliableSubscription(subscriptionUri).Instance.AcknowledgeRange(sequenceId);
            }

            public Uri GetSubscriptionResubscribeUri(Uri subscriptionUri)
            {
                return GetReliableSubscription(subscriptionUri).Instance.ResubscribeUri;
            }

            private ReliableSubscriptionEntity GetReliableSubscription(Uri subscriptionUri)
            {
                Debug.Assert(subscriptionUri != null);

                if (!Registry.ReliableSubscriptions.TryGetValue(subscriptionUri.ToCanonicalString(), out ReliableSubscriptionEntity entity))
                {
                    throw new EntityNotFoundException(subscriptionUri, ReactiveEntityKind.ReliableSubscription, Parent.Uri, nameof(subscriptionUri));
                }

                return entity;
            }

            private void InitializeSubscription(SubscriptionEntity entity, ISubscription subscription, IOperatorStateReaderFactory state, bool recovering)
            {
                Debug.Assert(subscription != null);

                Uri subscriptionUri;
                if (entity.State is IDictionary<string, object> entityState && entityState.TryGetValue(QueryEngineConstants.ParentUri, out object subscriptionUriObject))
                {
                    subscriptionUri = new Uri((string)subscriptionUriObject);
                }
                else
                {
                    subscriptionUri = entity.Uri;
                }

                var context = Parent.CreateOperatorContext(subscriptionUri);

                using (entity.Measure(EntityMetric.Subscribe))
                {
                    SubscriptionInitializeVisitor.Subscribe(subscription);
                }

                using (entity.Measure(EntityMetric.SetContext))
                {
                    SubscriptionInitializeVisitor.SetContext(subscription, context);
                }

                if (state != null)
                {
                    using (entity.Measure(EntityMetric.LoadState))
                    {
                        SubscriptionStateVisitor.LoadState(subscription, state);
                    }
                }

                // Upon recovery, the Start operation is handled by the StateLoader.Start method.
                if (recovering)
                {
                    entity.Instance = subscription;
                }
                else
                {
                    // CONSIDER: Could use Start on the entity which maintains the top-level lifecycle of the artifact.
                    entity.Start(subscription);
                }
            }

            private bool IsEdgeSubscription(Expression subscription)
            {
                Debug.Assert(subscription != null);

                if (subscription.Type != typeof(ISubscription))
                {
                    return false;
                }

                if (subscription is not InvocationExpression invoke)
                {
                    return false;
                }

                if (invoke.Arguments.Count < 1)
                {
                    return false;
                }

                if (invoke.Arguments[0] is not ParameterExpression observable)
                {
                    return false;
                }

                if (!Registry.Subjects.TryGetValue(observable.Name, out SubjectEntity entity) || !entity.IsInitialized)
                {
                    return false;
                }

                Type type = entity.Instance.GetType();
                if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(OutputEdge<>))
                {
                    return false;
                }

                return true;
            }

            private EdgeDescription CreateEdges(Uri subscriptionUri, IEnumerable<EdgeDescription> edges)
            {
                Debug.Assert(subscriptionUri != null);

                EdgeDescription outputEdge = default;

                if (edges == null || !edges.Any())
                {
                    return null;
                }

                // TODO: In case of failure, delete the already created edges before (re)throwing.
                foreach (var edge in edges)
                {
                    Debug.Assert(edge.Expression != null);
                    Debug.Assert(edge.Expression.Type.IsGenericType);
                    Type type = edge.Expression.Type;
                    Type genericType = type.GetGenericTypeDefinition();

                    // TODO: Refactor and handle parameterized observables/observers.
                    if (genericType == typeof(ISubscribable<>))
                    {
                        Type elementType = type.GetGenericArguments().Single();
                        CreateEdge(s_createInputMethod, elementType, edge);
                    }
                    else if (genericType == typeof(IObserver<>))
                    {
                        Debug.Assert(outputEdge == null);
                        outputEdge = edge;
                        edge.ExternalSubscriptionUri = subscriptionUri;

                        Type elementType = type.GetGenericArguments().Single();
                        CreateEdge(s_createOutputMethod, elementType, edge);
                    }
                    // TODO: else -- parameterized.
                }

                return outputEdge;
            }

            private void CreateEdge(MethodInfo method, Type elementType, EdgeDescription edge)
            {
                try
                {
                    var tmp = (IDisposable)method.MakeGenericMethod(elementType).Invoke(this, new object[] { edge });
                }
                catch (TargetInvocationException e) when (e.InnerException != null)
                {
                    throw e.InnerException;
                }
            }

            private IReactiveQubject<T, T> CreateInput<T>(EdgeDescription edge)
            {
                Debug.Assert(edge != null);
                var factory = Parent.ReactiveService.GetStreamFactory<EdgeDescription, T, T>(new Uri(QueryEngineConstants.InputUri));
                return factory.Create(edge.InternalUri, edge, state: null);
            }

            private IReactiveQubject<T, T> CreateOutput<T>(EdgeDescription edge)
            {
                Debug.Assert(edge != null);
                var factory = Parent.ReactiveService.GetStreamFactory<EdgeDescription, T, T>(new Uri(QueryEngineConstants.OutputUri));
                return factory.Create(edge.InternalUri, edge, state: null);
            }

            #endregion

            #region Observer

            public Task DefineObserverAsync(Uri observerUri, Expression observer, object state, CancellationToken token)
            {
                return CreateArtifactAsync(
                    observerUri,
                    observer,
                    state,
                    token,
                    DefineObserver,
                    UndefineObserver,
                    Registry.Observers,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).Observers
                );
            }

            public Task UndefineObserverAsync(Uri observerUri, CancellationToken token)
            {
                return DeleteArtifactAsync(
                    observerUri,
                    token,
                    ReactiveEntityKind.Observer,
                    UndefineObserver,
                    Registry.Observers,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).Observers
                );
            }

            public void DefineObserver(Uri observerUri, Expression observer, object state)
            {
                Debug.Assert(observerUri != null);
                Debug.Assert(observer != null);

                var templatized = observer;
                var elapsedTemplatizing = TimeSpan.Zero;
                if (Parent.Options.TemplatizeExpressions)
                {
                    var stopwatch = Stopwatch.StartNew();
                    templatized = _templatizer.Templatize(observer);
                    elapsedTemplatizing = stopwatch.Elapsed;
                }

                var entity = new ObserverDefinitionEntity(observerUri, templatized, state);
                entity.SetMetric(EntityMetric.Templatize, elapsedTemplatizing);

                DefineObserverCore(entity);
            }

            private void DefineObserverCore(ObserverDefinitionEntity entity)
            {
                Debug.Assert(entity != null);

                if (!Registry.Observers.TryAdd(entity.Uri.ToCanonicalString(), entity))
                {
                    throw new EntityAlreadyExistsException(entity.Uri, ReactiveEntityKind.Observer, Parent.Uri, nameof(entity));
                }

                entity.Cache(Parent.Options.ExpressionPolicy.InMemoryCache);
                OnEntityDefined(new ObserverEventArgs(entity));
            }

            public void UndefineObserver(Uri observerUri)
            {
                Debug.Assert(observerUri != null);

                if (!Registry.Observers.TryRemove(observerUri.ToCanonicalString(), out ObserverDefinitionEntity entity))
                {
                    throw new EntityNotFoundException(observerUri, ReactiveEntityKind.Observer, Parent.Uri, nameof(observerUri));
                }

                OnEntityUndefined(new ObserverEventArgs(entity));
            }

            public IReliableObserver<T> GetObserver<T>(Uri observerUri)
            {
                if (observerUri == null)
                {
                    throw new ArgumentNullException(nameof(observerUri));
                }

                return (IReliableObserver<T>)GetObserver(observerUri);
            }

            private object GetObserver(Uri observerUri)
            {
                Debug.Assert(observerUri != null);

                // TODO: Reliable observers and observers for rebindable streams.

                if (!Registry.Subjects.TryGetValue(observerUri.ToCanonicalString(), out SubjectEntity subjectEntity))
                {
                    throw new EntityNotFoundException(observerUri, ReactiveEntityKind.Observer, Parent.Uri, nameof(observerUri));
                }

                var subjectType = subjectEntity.Instance.GetType().FindGenericType(typeof(IReliableMultiSubject<,>));
                Debug.Assert(subjectType != null);
                Debug.Assert(subjectType.GetGenericArguments().Length == 2);

                var inputType = subjectType.GetGenericArguments()[0];
                var outputType = subjectType.GetGenericArguments()[1];

                return s_createObserverForSubjectMethod.MakeGenericMethod(inputType, outputType).Invoke(null, new object[] { subjectEntity.Instance });
            }

            private static IReliableObserver<TInput> CreateObserverForSubject<TInput, TOutput>(IReliableMultiSubject<TInput, TOutput> subject)
            {
                Debug.Assert(subject != null);
                return subject.CreateObserver();
            }

            #endregion

            #region Stream

            public Task CreateStreamAsync(Uri streamUri, Expression stream, object state, CancellationToken token)
            {
                return CreateArtifactAsync(
                    streamUri,
                    stream,
                    state,
                    token,
                    CreateStream,
                    DeleteStream,
                    Registry.Subjects,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).Subjects);
            }

            public Task DeleteStreamAsync(Uri streamUri, CancellationToken token)
            {
                return DeleteArtifactAsync(
                    streamUri,
                    token,
                    ReactiveEntityKind.Stream,
                    DeleteStream,
                    Registry.Subjects,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).Subjects
                );
            }

            public void CreateStream(Uri streamUri, Expression stream, object state)
            {
                Debug.Assert(streamUri != null);
                Debug.Assert(stream != null);

                if (Registry.Subjects.ContainsKey(streamUri.ToCanonicalString()))
                {
                    throw new EntityAlreadyExistsException(streamUri, ReactiveEntityKind.Stream, Parent.Uri, nameof(streamUri));
                }

                var stopwatch = Stopwatch.StartNew();
                Expression inlined = _inliningBinder.Bind(stream);
                var elapsedInlining = stopwatch.Elapsed;

                var templatized = inlined;
                var elapsedTemplatizing = default(TimeSpan);
                if (Parent.Options.TemplatizeExpressions)
                {
                    stopwatch.Restart();
                    templatized = _templatizer.Templatize(inlined);
                    elapsedTemplatizing = stopwatch.Elapsed;
                }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Entity will be owned by the registry after Create completes.)

                var entity = new SubjectEntity(streamUri, templatized, state);
                entity.SetMetric(EntityMetric.Inline, elapsedInlining);
                entity.SetMetric(EntityMetric.Templatize, elapsedTemplatizing);

                CreateStreamCore(entity, state: null, recovering: false);

#pragma warning restore CA2000
#pragma warning restore IDE0079
            }

            private void CreateStreamCore(SubjectEntity entity, IOperatorStateReaderFactory state, bool recovering)
            {
                var instance = default(IDisposable);
                var streamAdded = false;
                var success = false;

                try
                {
                    entity.Cache(Parent.Options.ExpressionPolicy.InMemoryCache);

                    using (entity.Measure(EntityMetric.Evaluate))
                    {
                        instance = Evaluate<IDisposable>(_fullBinder.Bind(_templatizer.Detemplatize(entity.Expression)));
                    }

                    // First add the stream to the registry and then initialize, because initialize might
                    // trigger more subscriptions which need to see the stream.
                    // This could still fail, even though we checked above, because of race conditions.
                    if (!Registry.Subjects.TryAdd(entity.Uri.ToCanonicalString(), entity))
                    {
                        string paramName = "streamUri"; // NB: Workaround for CA2208 complaining that streamUri is not a parameter of this method.
                        throw new EntityAlreadyExistsException(entity.Uri, ReactiveEntityKind.Stream, Parent.Uri, paramName);
                    }

                    streamAdded = true;

                    InitializeStream(entity, instance, state, recovering);
                    success = true;

                    OnEntityCreated(new StreamEventArgs(entity));
                }
                finally
                {
                    // If we're recovering, failure to reinitialize the stream should not have the effect of a destructive
                    // deletion in the registry, which will cause the checkpoint state to get discarded. Subsequent recovery
                    // attempts can retry the recovery, e.g. after a bug fix. We may need to revisit this behavior, e.g. to
                    // reinitialize a stream with an empty state in case a recovery failure occurs, based on a host-controlled
                    // policy or something.
                    if (!success && !recovering)
                    {
                        if (streamAdded)
                        {
                            DeleteStream(entity.Uri);
                        }

                        // If the subject failed to load runtime state or to start, the instance may have been created, but
                        // not been assigned to the Instance property of the RuntimeEntity.  If that is the case, the call to
                        // `DeleteStream` will not result in the instance being disposed.
                        instance?.Dispose();
                    }
                }
            }

            public void DeleteStream(Uri streamUri)
            {
                Debug.Assert(streamUri != null);

                if (!Registry.Subjects.TryRemove(streamUri.ToCanonicalString(), out SubjectEntity entity))
                {
                    throw new EntityNotFoundException(streamUri, ReactiveEntityKind.Stream, Parent.Uri, nameof(streamUri));
                }

                Debug.Assert(entity != null);
                entity.Dispose();

                OnEntityDeleted(new StreamEventArgs(entity));
            }

            private void InitializeStream(SubjectEntity entity, IDisposable stream, IOperatorStateReaderFactory state, bool recovering)
            {
                Debug.Assert(stream != null);

                if (stream is IOperator streamOp)
                {
                    var context = Parent.CreateOperatorContext(entity.Uri);

                    using (entity.Measure(EntityMetric.SetContext))
                    {
                        streamOp.SetContext(context);
                    }

                    if (state != null)
                    {
                        if (streamOp is IStatefulOperator statefulOperator)
                        {
                            using (entity.Measure(EntityMetric.LoadState))
                            {
                                state.LoadState(statefulOperator);
                            }
                        }
                    }
                }

                // TODO: The initialization of the entity is needed before the Start because OutputEdge relies on the entity being visible.
                //       This is asymmetric compared to subscription initialization.
                entity.Instance = stream;

                // Upon recovery, the Start operation is handled by the StateLoader.Start method.
                if (!recovering)
                {
                    // CONSIDER: Could use Start on the entity which maintains the top-level lifecycle of the artifact.
                    entity.Start();
                }
            }

            #endregion

            #region Observable

            public Task DefineObservableAsync(Uri observableUri, Expression observable, object state, CancellationToken token)
            {
                return CreateArtifactAsync(
                    observableUri,
                    observable,
                    state,
                    token,
                    DefineObservable,
                    UndefineObservable,
                    Registry.Observables,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).Observables
                );
            }

            public Task UndefineObservableAsync(Uri observableUri, CancellationToken token)
            {
                return DeleteArtifactAsync(
                    observableUri,
                    token,
                    ReactiveEntityKind.Observable,
                    UndefineObservable,
                    Registry.Observables,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).Observables
                );
            }

            public void DefineObservable(Uri observableUri, Expression observable, object state)
            {
                Debug.Assert(observableUri != null);
                Debug.Assert(observable != null);

                var templatized = observable;
                var elapsedTemplatizing = TimeSpan.Zero;
                if (Parent.Options.TemplatizeExpressions)
                {
                    var stopwatch = Stopwatch.StartNew();
                    templatized = _templatizer.Templatize(observable);
                    elapsedTemplatizing = stopwatch.Elapsed;
                }

                var entity = new ObservableDefinitionEntity(observableUri, templatized, state);
                entity.SetMetric(EntityMetric.Templatize, elapsedTemplatizing);

                DefineObservableCore(entity);
            }

            private void DefineObservableCore(ObservableDefinitionEntity entity)
            {
                Debug.Assert(entity != null);

                if (!Registry.Observables.TryAdd(entity.Uri.ToCanonicalString(), entity))
                {
                    throw new EntityAlreadyExistsException(entity.Uri, ReactiveEntityKind.Observable, Parent.Uri, nameof(entity));
                }

                entity.Cache(Parent.Options.ExpressionPolicy.InMemoryCache);
                OnEntityDefined(new ObservableEventArgs(entity));
            }

            public void UndefineObservable(Uri observableUri)
            {
                Debug.Assert(observableUri != null);

                if (!Registry.Observables.TryRemove(observableUri.ToCanonicalString(), out ObservableDefinitionEntity entity))
                {
                    throw new EntityNotFoundException(observableUri, ReactiveEntityKind.Observable, Parent.Uri, nameof(observableUri));
                }

                OnEntityUndefined(new ObservableEventArgs(entity));
            }

            #endregion

            #region Subject Factory

            public Task DefineSubjectFactoryAsync(Uri subjectFactoryUri, Expression subjectFactory, object state, CancellationToken token)
            {
                return CreateArtifactAsync(
                    subjectFactoryUri,
                    subjectFactory,
                    state,
                    token,
                    DefineSubjectFactory,
                    UndefineSubjectFactory,
                    Registry.SubjectFactories,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).SubjectFactories
                );
            }

            public Task UndefineSubjectFactoryAsync(Uri subjectFactoryUri, CancellationToken token)
            {
                return DeleteArtifactAsync(
                    subjectFactoryUri,
                    token,
                    ReactiveEntityKind.StreamFactory,
                    UndefineSubjectFactory,
                    Registry.SubjectFactories,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).SubjectFactories
                );
            }

            public void DefineSubjectFactory(Uri subjectFactoryUri, Expression subjectFactory, object state)
            {
                Debug.Assert(subjectFactoryUri != null);
                Debug.Assert(subjectFactory != null);

                var entity = new StreamFactoryDefinitionEntity(subjectFactoryUri, subjectFactory, state);
                DefineSubjectFactoryCore(entity);
            }

            private void DefineSubjectFactoryCore(StreamFactoryDefinitionEntity entity)
            {
                Debug.Assert(entity != null);

                if (!Registry.SubjectFactories.TryAdd(entity.Uri.ToCanonicalString(), entity))
                {
                    throw new EntityAlreadyExistsException(entity.Uri, ReactiveEntityKind.StreamFactory, Parent.Uri, nameof(entity));
                }

                entity.Cache(Parent.Options.ExpressionPolicy.InMemoryCache);
                OnEntityDefined(new StreamFactoryEventArgs(entity));
            }

            public void UndefineSubjectFactory(Uri subjectFactoryUri)
            {
                Debug.Assert(subjectFactoryUri != null);

                if (!Registry.SubjectFactories.TryRemove(subjectFactoryUri.ToCanonicalString(), out StreamFactoryDefinitionEntity entity))
                {
                    throw new EntityNotFoundException(subjectFactoryUri, ReactiveEntityKind.StreamFactory, Parent.Uri, nameof(subjectFactoryUri));
                }

                OnEntityUndefined(new StreamFactoryEventArgs(entity));
            }

            #endregion

            #region Subscription Factory

            public Task DefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state, CancellationToken token)
            {
                return CreateArtifactAsync(
                    subscriptionFactoryUri,
                    subscriptionFactory,
                    state,
                    token,
                    DefineSubscriptionFactory,
                    UndefineSubscriptionFactory,
                    Registry.SubscriptionFactories,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).SubscriptionFactories
                );
            }

            public Task UndefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, CancellationToken token)
            {
                return DeleteArtifactAsync(
                    subscriptionFactoryUri,
                    token,
                    ReactiveEntityKind.SubscriptionFactory,
                    UndefineSubscriptionFactory,
                    Registry.SubscriptionFactories,
                    async () => (await Parent._transactionLogManager.TransactionLog.ConfigureAwait(false)).SubscriptionFactories
                );
            }

            public void DefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
            {
                Debug.Assert(subscriptionFactoryUri != null);
                Debug.Assert(subscriptionFactory != null);

                var entity = new SubscriptionFactoryDefinitionEntity(subscriptionFactoryUri, subscriptionFactory, state);
                DefineSubscriptionFactoryCore(entity);
            }

            private void DefineSubscriptionFactoryCore(SubscriptionFactoryDefinitionEntity entity)
            {
                Debug.Assert(entity != null);

                if (!Registry.SubscriptionFactories.TryAdd(entity.Uri.ToCanonicalString(), entity))
                {
                    throw new EntityAlreadyExistsException(entity.Uri, ReactiveEntityKind.SubscriptionFactory, Parent.Uri, nameof(entity));
                }

                entity.Cache(Parent.Options.ExpressionPolicy.InMemoryCache);
                OnEntityDefined(new SubscriptionFactoryEventArgs(entity));
            }

            public void UndefineSubscriptionFactory(Uri subscriptionFactoryUri)
            {
                Debug.Assert(subscriptionFactoryUri != null);

                if (!Registry.SubscriptionFactories.TryRemove(subscriptionFactoryUri.ToCanonicalString(), out SubscriptionFactoryDefinitionEntity entity))
                {
                    throw new EntityNotFoundException(subscriptionFactoryUri, ReactiveEntityKind.SubscriptionFactory, Parent.Uri, nameof(subscriptionFactoryUri));
                }

                OnEntityUndefined(new SubscriptionFactoryEventArgs(entity));
            }

            #endregion

            #region Expression Rewrites

            private T Evaluate<T>(Expression expression)
            {
                return Parent.Options.ExpressionPolicy.Evaluate<T>(expression);
            }

            /// <summary>
            /// Rewrites a subscription expression from ISubscribable to IReliableObservable family of interfaces.
            /// The input expression must be in URI form. The method will fail if the input expression does not
            /// represent an invocation of "Subscribe".
            /// </summary>
            /// <param name="expressionService">The expression service to use to create named expressions.</param>
            /// <param name="expr">The input expresssion (using ISubscribable family of interfaces).</param>
            /// <returns>The result (using IReliableObservable family of interfaces).</returns>
            private static Expression RewriteSubscriptionToReliableSubscription(IReactiveExpressionServices expressionService, Expression expr)
            {
                Debug.Assert(expressionService != null);
                Debug.Assert(expr != null);

                if (expr.Type != typeof(ISubscription))
                {
                    throw new InvalidOperationException("Expected a subscription expression.");
                }

                if (expr is not InvocationExpression invocationExpression)
                {
                    throw new InvalidOperationException("Expected the subscription expression to be invocation expression.");
                }

                var reliableSubscription =
                    Expression.Invoke(
                        expressionService.GetNamedExpression(invocationExpression.Expression.Type, new Uri(QueryEngineConstants.ReliableSubscribeUri)),
                        invocationExpression.Arguments);

                var rewrite = new SubstituteAndUnquoteRewriter(
                    new Dictionary<Type, Type>
                    {
                        { typeof(IMultiSubject<,>), typeof(IReliableMultiSubject<,>) },
                        { typeof(ISubscribable<>), typeof(IReliableObservable<>) },
                        { typeof(IObserver<>), typeof(IReliableObserver<>) },
                        { typeof(ISubscription), typeof(IReliableSubscription) },
                    });

                Expression result = rewrite.Apply(reliableSubscription);

                return result;
            }

            #endregion

#pragma warning disable IDE0079 // Remove unnecessary suppression (only for .NET 5.0)
#pragma warning disable IDE0063 // Use simple 'using' statement (stylistically useful for lock)
#pragma warning disable CA1068 // CancellationToken parameters must come last (doesn't matter for private methods)
            private async Task CreateArtifactAsync<T>(
                Uri uri,
                Expression artifact,
                object state,
                CancellationToken token,
                Action<Uri, Expression, object> createInEngine,
                Action<Uri> deleteInEngine, // for rollback
                IReactiveEntityCollection<string, T> registry,
                Func<Task<IKeyValueTable<string, ArtifactOperation>>> transactionLogGetter /* will only be invoked under a lock. */)
                where T : ReactiveEntity
            {
                using var _ = Parent._tracker.Enter();

                // A main requirement of this method is to create the artifact and add it
                // to the transaction log. We want create to happen before the Tx add so we can
                // inform the client of any exceptions (otherwise it is possible that Tx add happens
                // first, machine crashes, we retry creation and get recovery errors from evaluation
                // of the expression). We lock here to ensure that Tx add will happen right after
                // creation without getting interrupted by a checkpoint. If there is an interruption,
                // then there will again be recovery errors if create succeeds, checkpoint happens,
                // Tx add happens and a crash occurs. Now the new machine recovers from checkpoint state
                // and then replays Tx log - both of which contain the new artifact.

                // This implementation of a reader/writer doesn't guarantee any ordering of continuations when a lock is acquired
                // after waiting. Revisit this if we want a strong ordering guarantee.
                using (var checkpointLock = await _checkpointAndRecoverLock.EnterReadAsync().ConfigureAwait(false))
                {
                    token.ThrowIfCancellationRequested();

                    createInEngine(uri, artifact, state);

                    var id = uri.ToCanonicalString();

                    var committed = default(bool);
                    try
                    {
                        var table = await transactionLogGetter().ConfigureAwait(false);

                        using var tx = Parent._keyValueStore.CreateTransaction();

                        var txTable = table.Enter(tx);

                        // TODO Move to a queue based implementation of the transaction log.
                        if (txTable.TryGet(id, out ArtifactOperation old))
                        {
                            Debug.Assert(old.OperationKind == ArtifactOperationKind.Delete);
                            txTable.Update(id, ArtifactOperation.DeleteCreate(artifact, state));
                        }
                        else
                        {
                            txTable.Add(id, ArtifactOperation.Create(artifact, state));
                        }

                        await tx.CommitAsync(token).ConfigureAwait(false);
                        committed = true;
                    }
                    catch (Exception e1)
                    {
                        if (committed)
                        {
                            // Everything succeeded except disposal of the transaction. Log and continue.
                            Tracing.Create_Artifact_Unexpected_Transaction_Disposal_Exception(null, uri, e1);
                        }
                        else
                        {
                            try
                            {
                                deleteInEngine(uri);
                            }
                            catch (Exception e2)
                            {
                                throw new AggregateException(e1, e2);
                            }

                            throw;
                        }
                    }

                    Invariant.Assert(registry.TryGetValue(id, out T entity), "Entity was created but is not in the registry.");
                    entity.AdvanceState(TransactionState.Active);
                }
            }

            public async Task DeleteArtifactAsync<T>(
                Uri uri,
                CancellationToken token,
                ReactiveEntityKind entityKind,
                Action<Uri> deleteInEngine,
                IReactiveEntityCollection<string, T> registry,
                Func<Task<IKeyValueTable<string, ArtifactOperation>>> transactionLogGetter /* will only be invoked under a lock. */)
                where T : ReactiveEntity
            {
                using var _ = Parent._tracker.Enter();

                // We update the transaction log before deleting the artifact because a delete cannot be rolled back (easily at least).

                // This implementation of a reader/writer doesn't guarantee any ordering of continuations when a lock is acquired
                // after waiting. Revisit this if we want a strong ordering guarantee.
                using (var checkpointLock = await _checkpointAndRecoverLock.EnterReadAsync().ConfigureAwait(false))
                {
                    token.ThrowIfCancellationRequested();

                    var id = uri.ToCanonicalString();

                    if (!registry.TryGetValue(id, out T entity))
                        throw new EntityNotFoundException(uri, entityKind, Parent.Uri, nameof(uri));

                    var old = default(ArtifactOperation);
                    var table = default(IKeyValueTable<string, ArtifactOperation>);
                    var edi = default(ExceptionDispatchInfo);
                    var updatedTx = false;

                    // This state advance is to prevent multiple deletes of the same entity.
                    entity.AdvanceState(TransactionState.Deleting);
                    try
                    {
                        table = await transactionLogGetter().ConfigureAwait(false);

                        using (var tx = Parent._keyValueStore.CreateTransaction())
                        {
                            var txTable = table.Enter(tx);

                            // TODO Move to a queue based implementation of the transaction log.
                            if (txTable.TryGet(id, out old))
                            {
                                switch (old.OperationKind)
                                {
                                    case ArtifactOperationKind.Create:
                                        txTable.Remove(id);
                                        break;
                                    case ArtifactOperationKind.DeleteCreate:
                                        txTable.Update(id, ArtifactOperation.Delete());
                                        break;
                                    default:
                                        Invariant.Assert(false, "Entity deleted from transaction log previously.");
                                        break;
                                }
                            }
                            else
                            {
                                txTable.Add(id, ArtifactOperation.Delete());
                            }

                            await tx.CommitAsync(token).ConfigureAwait(false);
                        }

                        updatedTx = true;

                        deleteInEngine(uri);
                        entity.AdvanceState(TransactionState.Deleted);
                    }
                    catch (Exception e)
                    {
                        entity.RollbackState(TransactionState.Active);

                        if (updatedTx)
                        {
                            // Execute cleanup code after this catch block
                            edi = ExceptionDispatchInfo.Capture(e);
                        }
                        else
                        {
                            // No cleanup necessary.. just throw
                            throw;
                        }
                    }

                    // Delete tx entry if there was an exception
                    if (edi != default(ExceptionDispatchInfo))
                    {
                        using (var tx = Parent._keyValueStore.CreateTransaction())
                        {
                            var txTable = table.Enter(tx);
                            if (old != null) // Delete annihilated create in tx
                            {
                                switch (old.OperationKind)
                                {
                                    case ArtifactOperationKind.Create:
                                        txTable.Add(id, old);
                                        break;
                                    case ArtifactOperationKind.DeleteCreate:
                                        txTable.Update(id, old);
                                        break;
                                    default:
                                        Invariant.Assert(false, "Previous operation in transaction log is not Create or DeleteCreate.");
                                        break;
                                }
                            }
                            else
                            {
                                txTable.Remove(id);
                            }

                            try
                            {
                                // Can't support cancellation because we need to rollback to maintain consistency between registry and tx log
                                await tx.CommitAsync().ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                // Well, this won't end well...
                                throw new AggregateException(edi.SourceException, e);
                            }
                        }

                        edi.Throw();
                    }
                }
            }
#pragma warning restore CA1068 // CancellationToken parameters must come last
#pragma warning restore IDE0063 // Use simple 'using' statement
#pragma warning restore IDE0079 // Remove unnecessary suppression
        }
    }
}
