// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Reaqtive;

using Reaqtor.Metadata;
using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        /// <summary>
        /// Contains definitions that are specific (intrinsic) to this particular Query Engine implementation.
        /// Uses an external IReactiveMetadata provider to "import" external/global definitions.
        /// All definitions provided by this registry are either intrinsic or imported and should not be persisted in checkpoints.
        /// </summary>
        private sealed class BuiltinEntitiesRegistry : IQueryEngineRegistry, IDisposable
        {
            private readonly CheckpointingQueryEngine _queryEngine;
            private readonly ReactiveEntityCollection<string, SubjectEntity> _subjects;

            private readonly ExternalLookupReactiveEntityCollection<ObservableDefinitionEntity> _observables;
            private readonly ExternalLookupReactiveEntityCollection<ObserverDefinitionEntity> _observers;
            private readonly ExternalLookupReactiveEntityCollection<StreamFactoryDefinitionEntity> _subjectFactories;
            private readonly ExternalLookupReactiveEntityCollection<SubscriptionFactoryDefinitionEntity> _subscriptionFactories;

            private readonly ReactiveEntityCollection<string, DefinitionEntity> _other;
            private readonly InvertedLookupReactiveEntityCollection<string, DefinitionEntity> _templates;

            private readonly ReactiveEntityCollection<string, SubscriptionEntity> _subscriptions;
            private readonly ReactiveEntityCollection<string, ReliableSubscriptionEntity> _reliableSubscriptions;

            private readonly ConditionalWeakTable<Expression, Expression> _metadataRewrites;

            private readonly object _createStreamGate = new();

            private bool _disposed;

            public BuiltinEntitiesRegistry(CheckpointingQueryEngine queryEngine, IReactiveMetadata metadata)
            {
                Debug.Assert(queryEngine != null);
                Debug.Assert(metadata != null);

                _queryEngine = queryEngine;
                _subjects = new ReactiveEntityCollection<string, SubjectEntity>(StringComparer.Ordinal);

                var localObservables = new ReactiveEntityCollection<string, ObservableDefinitionEntity>(StringComparer.Ordinal);
                var localObservablesAndSubjects = new ChainedLookupReactiveEntityCollection<string, ObservableDefinitionEntity, SubjectEntity>(localObservables, _subjects, s => ObservableDefinitionEntity.FromSubject(s));
                _observables = new ExternalLookupReactiveEntityCollection<ObservableDefinitionEntity>(localObservablesAndSubjects, TryLookupObservable, metadata);

                var localObservers = new ReactiveEntityCollection<string, ObserverDefinitionEntity>(StringComparer.Ordinal);
                var localObserversAndSubjects = new ChainedLookupReactiveEntityCollection<string, ObserverDefinitionEntity, SubjectEntity>(localObservers, _subjects, s => ObserverDefinitionEntity.FromSubject(s));
                _observers = new ExternalLookupReactiveEntityCollection<ObserverDefinitionEntity>(localObserversAndSubjects, TryLookupObserver, metadata);

                var localSubjectFactories = new ReactiveEntityCollection<string, StreamFactoryDefinitionEntity>(StringComparer.Ordinal);
                _subjectFactories = new ExternalLookupReactiveEntityCollection<StreamFactoryDefinitionEntity>(localSubjectFactories, TryLookupStreamFactory, metadata);

                var localSubscriptionFactories = new ReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity>(StringComparer.Ordinal);
                _subscriptionFactories = new ExternalLookupReactiveEntityCollection<SubscriptionFactoryDefinitionEntity>(localSubscriptionFactories, TryLookupSubscriptionFactory, metadata);

                _other = new ReactiveEntityCollection<string, DefinitionEntity>(StringComparer.Ordinal);
                _templates = new InvertedLookupReactiveEntityCollection<string, DefinitionEntity>(StringComparer.Ordinal, InvertedDefinitionEntityComparer.Default);

                _subscriptions = new ReactiveEntityCollection<string, SubscriptionEntity>(StringComparer.Ordinal);
                _reliableSubscriptions = new ReactiveEntityCollection<string, ReliableSubscriptionEntity>(StringComparer.Ordinal);

                _metadataRewrites = new ConditionalWeakTable<Expression, Expression>();

                InitializeBuiltinDefinitions(queryEngine);
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    _subjects.Dispose();
                    _observables.Dispose();
                    _observers.Dispose();
                    _subjectFactories.Dispose();
                    _subscriptionFactories.Dispose();
                    _other.Dispose();
                    _templates.Dispose();
                    _subscriptions.Dispose();
                    _reliableSubscriptions.Dispose();

                    _disposed = true;
                }
            }

            #region IQueryEngineRegistry

            public IReactiveEntityCollection<string, ObservableDefinitionEntity> Observables => _observables;

            public IReactiveEntityCollection<string, ObserverDefinitionEntity> Observers => _observers;

            public IReactiveEntityCollection<string, StreamFactoryDefinitionEntity> SubjectFactories => _subjectFactories;

            public IReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity> SubscriptionFactories => _subscriptionFactories;

            public IReactiveEntityCollection<string, DefinitionEntity> Other => _other;

            public IInvertedLookupReactiveEntityCollection<string, DefinitionEntity> Templates => _templates;

            public IReactiveEntityCollection<string, SubscriptionEntity> Subscriptions => _subscriptions;

            public IReactiveEntityCollection<string, SubjectEntity> Subjects => _subjects;

            public IReactiveEntityCollection<string, ReliableSubscriptionEntity> ReliableSubscriptions => _reliableSubscriptions;

            public void Clear()
            {
                _observables.Clear();
                _observers.Clear();
                _subjectFactories.Clear();
                _subscriptionFactories.Clear();
                _other.Clear();
                _subscriptions.Clear();
                _subjects.Clear();
                _reliableSubscriptions.Clear();
            }

            #endregion

            /// <summary>
            /// Creates the intrinsic definitions.
            /// </summary>
            private void InitializeBuiltinDefinitions(CheckpointingQueryEngine queryEngine)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Entities are owned by the registry.)

                // Subscribe implementation for Subscribable and ReliableObservable.
                _other.TryAdd(QueryEngineConstants.SubscribeUri, new OtherDefinitionEntity(new Uri(QueryEngineConstants.SubscribeUri), (Expression<Func<ISubscribable<T>, IObserver<T>, ISubscription>>)((o, v) => o.SubscribeRoot(v)), null));
                _other.TryAdd(QueryEngineConstants.ReliableSubscribeUri, new OtherDefinitionEntity(new Uri(QueryEngineConstants.ReliableSubscribeUri), (Expression<Func<IReliableObservable<T>, IReliableObserver<T>, IReliableSubscription>>)((o, v) => o.Subscribe(v)), null));

                // NB: All of the *Id definitions are legacy, prior to standardizing on `rx://builtin/identity`. They've been kept to ensure existing
                //     expressions can continue to bind and recover. They're harmless.

                // Conversions from Observable to Subscribable and from Subscribable to Subscribable (identity).
                _observables.TryAdd(QueryEngineConstants.ObservableId, new ObservableDefinitionEntity(new Uri(QueryEngineConstants.ObservableId), (Expression<Func<IObservable<T>, ISubscribable<T>>>)((source) => source.ToSubscribable()), null));
                _observables.TryAdd(QueryEngineConstants.SubscribableId, new ObservableDefinitionEntity(new Uri(QueryEngineConstants.SubscribableId), (Expression<Func<ISubscribable<T>, ISubscribable<T>>>)((source) => source), null));
                _observables.TryAdd(QueryEngineConstants.QbservableId, new ObservableDefinitionEntity(new Uri(QueryEngineConstants.QbservableId), (Expression<Func<ISubscribable<T>, ISubscribable<T>>>)((source) => source), null)); // TODO: consider eliminating the identities from the expression

                // Conversion from Observer to Observer (identity).
                _observers.TryAdd(QueryEngineConstants.ObserverId, new ObserverDefinitionEntity(new Uri(QueryEngineConstants.ObserverId), (Expression<Func<IObserver<T>, IObserver<T>>>)(observer => observer), null));

                // Subject factories for input and output edges.
                _subjectFactories.TryAdd(QueryEngineConstants.InputUri, new StreamFactoryDefinitionEntity(new Uri(QueryEngineConstants.InputUri), (Expression<Func<EdgeDescription, IReliableMultiSubject<T, T>>>)((edge) => new InputEdge<T>(edge, queryEngine._serviceResolver, queryEngine._reliableService)), null));
                _subjectFactories.TryAdd(QueryEngineConstants.OutputUri, new StreamFactoryDefinitionEntity(new Uri(QueryEngineConstants.OutputUri), (Expression<Func<EdgeDescription, IReliableMultiSubject<T, T>>>)((edge) => new OutputEdge<T>(edge, queryEngine._serviceResolver, queryEngine._reliableService)), null));
                _subjectFactories.TryAdd(QueryEngineConstants.BridgeUri, new StreamFactoryDefinitionEntity(new Uri(QueryEngineConstants.BridgeUri), (Expression<Func<Expression, IReliableMultiSubject<T, T>>>)((expr) => new Bridge<T>(queryEngine.Options.ExpressionPolicy.InMemoryCache.Create(expr))), null));
                _subjectFactories.TryAdd(QueryEngineConstants.InnerSubjectUri, new StreamFactoryDefinitionEntity(new Uri(QueryEngineConstants.InnerSubjectUri), (Expression<Func<IReliableMultiSubject<T, T>>>)(() => new InnerSubject<T>()), null));
                _subjectFactories.TryAdd(QueryEngineConstants.InnerSubjectRefCountUri, new StreamFactoryDefinitionEntity(new Uri(QueryEngineConstants.InnerSubjectRefCountUri), (Expression<Func<Uri, Uri, IReliableMultiSubject<T, T>>>)((refCountUri, collectorUri) => new RefCountSubject<T>(refCountUri, collectorUri)), null));

                // Built-in subscription factory for observable.Subscribe(observer)
                _subscriptionFactories.TryAdd(QueryEngineConstants.SubscribeUri, new SubscriptionFactoryDefinitionEntity(new Uri(QueryEngineConstants.SubscribeUri), (Expression<Func<ISubscribable<T>, IObserver<T>, ISubscription>>)((o, v) => o.SubscribeRoot(v)), null));
                _subscriptionFactories.TryAdd(QueryEngineConstants.ReliableSubscribeUri, new SubscriptionFactoryDefinitionEntity(new Uri(QueryEngineConstants.ReliableSubscribeUri), (Expression<Func<IReliableObservable<T>, IReliableObserver<T>, IReliableSubscription>>)((o, v) => o.Subscribe(v)), null));

#pragma warning restore CA2000
#pragma warning restore IDE0079
            }

            private sealed class ExternalLookupReactiveEntityCollection<TDefinitionEntity> : CooperativeLookupReactiveEntityCollection<string, TDefinitionEntity, IReactiveMetadata>
                where TDefinitionEntity : DefinitionEntity
            {
                public ExternalLookupReactiveEntityCollection(IReactiveEntityCollection<string, TDefinitionEntity> local, TryLookup<IReactiveMetadata, string, TDefinitionEntity> lookupFunc, IReactiveMetadata external)
                    : base(local, lookupFunc, external)
                {
                }
            }

            public bool TryLookupObserver(IReactiveMetadata metadata, string key, out ObserverDefinitionEntity entity)
            {
                if (metadata.Observers.TryGetValue(new Uri(key), out IReactiveObserverDefinition observerDefinition))
                {
                    var expression = _metadataRewrites.GetValue(
                        observerDefinition.Expression,
                        e => _queryEngine.RewriteQuotedReactiveToSubscribable(e));
                    entity = new ObserverDefinitionEntity(observerDefinition.Uri, expression, observerDefinition.State);
                    return true;
                }

                // Will result in a stream being created if a definition exists
                // in the remote metadata store. This path should only succeed
                // when hit by the definition inlining binder.
                if (TryCreateStreamFromMetadata(metadata, key, out var subject))
                {
                    entity = ObserverDefinitionEntity.FromSubject(subject);
                    return true;
                }

                entity = null;
                return false;
            }

            public bool TryLookupObservable(IReactiveMetadata metadata, string key, out ObservableDefinitionEntity entity)
            {
                if (metadata.Observables.TryGetValue(new Uri(key), out IReactiveObservableDefinition observableDefinition))
                {
                    var expression = _metadataRewrites.GetValue(
                        observableDefinition.Expression,
                        e => _queryEngine.RewriteQuotedReactiveToSubscribable(e));
                    entity = new ObservableDefinitionEntity(observableDefinition.Uri, expression, observableDefinition.State);
                    return true;
                }

                // Will result in a stream being created if a definition exists
                // in the remote metadata store. This path should only succeed
                // when hit by the definition inlining binder.
                if (TryCreateStreamFromMetadata(metadata, key, out var subject))
                {
                    entity = ObservableDefinitionEntity.FromSubject(subject);
                    return true;
                }

                entity = null;
                return false;
            }

            public bool TryLookupStreamFactory(IReactiveMetadata metadata, string key, out StreamFactoryDefinitionEntity entity)
            {
                if (metadata.StreamFactories.TryGetValue(new Uri(key), out IReactiveStreamFactoryDefinition streamFactoryDefinition))
                {
                    var expression = _metadataRewrites.GetValue(
                        streamFactoryDefinition.Expression,
                        e => _queryEngine.RewriteQuotedReactiveToSubscribable(e));
                    entity = new StreamFactoryDefinitionEntity(streamFactoryDefinition.Uri, expression, streamFactoryDefinition.State);
                    return true;
                }

                entity = null;
                return false;
            }

            public bool TryLookupSubscriptionFactory(IReactiveMetadata metadata, string key, out SubscriptionFactoryDefinitionEntity entity)
            {
                if (metadata.SubscriptionFactories.TryGetValue(new Uri(key), out IReactiveSubscriptionFactoryDefinition subscriptionFactoryDefinition))
                {
                    var expression = _metadataRewrites.GetValue(
                        subscriptionFactoryDefinition.Expression,
                        e => _queryEngine.RewriteQuotedReactiveToSubscribable(e));
                    entity = new SubscriptionFactoryDefinitionEntity(subscriptionFactoryDefinition.Uri, expression, subscriptionFactoryDefinition.State);
                    return true;
                }

                entity = null;
                return false;
            }

            private bool TryCreateStreamFromMetadata(IReactiveMetadata metadata, string key, out SubjectEntity subject)
            {
                //
                // Lookup the stream definition from metadata. If we find a
                // remote definition, a singleton instance should be created.
                // Note, in a proper architecture, the stream would have
                // already been created (via invocation of a stream factory or
                // otherwise) before we reached this point in subscription
                // creation. Thus, we would have found a stream instance in the
                // local registry, and this code would not be executed.
                //
                if (metadata.Streams.TryGetValue(new Uri(key), out IReactiveStreamProcess streamProcess))
                {
                    lock (_createStreamGate)
                    {
                        // TODO: rewrite stream expression based on bound type of stream factory
                        if (!_queryEngine._registry.Subjects.TryGetValue(key, out subject))
                        {
                            var streamExpr = RewriteQuotedReactiveStreamToUntyped(key, streamProcess.Expression);
                            Expression expr = _queryEngine.RewriteQuotedReactiveToSubscribable(streamExpr);
                            _queryEngine._engine.CreateStream(new Uri(key), expr, null);
                            _queryEngine.TraceSource.LazyStream_Created(key, _queryEngine.Uri, expr.ToTraceString());
                            if (_queryEngine._registry.Subjects.TryGetValue(key, out subject))
                            {
                                subject.AdvanceState(TransactionState.Active);
                            }
                            else
                            {
                                throw new InvalidOperationException(
                                    string.Format(CultureInfo.InvariantCulture, "Attempt to lazily create stream '{0}' on query engine '{1}' has failed.", key, _queryEngine.Uri.ToCanonicalString()));
                            }
                        }
                    }

                    return true;
                }

                subject = null;
                return false;
            }

            /// <summary>
            /// Rewrites a stream to an untyped stream if the stream definition
            /// is closed over wildcard types, and the stream factory returns
            /// an untyped stream.
            /// </summary>
            /// <param name="key">The stream identifier.</param>
            /// <param name="expr">The stream expression.</param>
            /// <returns>
            /// The stream expression with types rewritten to the untyped stream
            /// variant if the expression meets the expected criteria.
            /// </returns>
            private Expression RewriteQuotedReactiveStreamToUntyped(string key, Expression expr)
            {
                // Search for free variables
                var freeVariables = FreeVariableScanner.Scan(expr).ToArray();

                // For now, with the absence of stream factory operators, it is safe to
                // throw if there is more than one free variable as all stream expressions
                // will be invocations of exactly one unbound stream factory parameter.
                if (freeVariables.Length > 1)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture, "Unexpected stream expression '{0}' for key '{1}' on query engine '{2}'.", expr.ToTraceString(), key, _queryEngine.Uri.ToCanonicalString()));
                }

                if (freeVariables.Length == 1)
                {
                    // Find single stream factory parameter expression
                    var streamFactoryParam = freeVariables[0];
                    if (streamFactoryParam != null)
                    {
                        // Resolve stream factory type from metadata
                        if (_queryEngine._registry.SubjectFactories.TryGetValue(streamFactoryParam.Name, out var streamFactoryDefinition))
                        {
                            // TODO: support rewrites based on stream factory classes
                            // If definition is a lambda expression, rewrite to the return type
                            var streamFactoryType = streamFactoryDefinition.Expression.Type;
                            var invokeMethod = streamFactoryType.GetMethod("Invoke");
                            if (invokeMethod != null && invokeMethod.ReturnType == typeof(IMultiSubject))
                            {
                                var actualStreamType = expr.Type;
                                var expectedStreamType = invokeMethod.ReturnType;
                                if (actualStreamType != expectedStreamType)
                                {
                                    var substitutionVisitor = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
                                    {
                                        { actualStreamType, expectedStreamType }
                                    });

                                    _queryEngine.TraceSource.LazyStream_TypeRewrite(key, actualStreamType, expectedStreamType, _queryEngine.Uri);

                                    return substitutionVisitor.Apply(expr);
                                }
                            }
                        }
                    }
                }

                return expr;
            }
        }
    }
}
