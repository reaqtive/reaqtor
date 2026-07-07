// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Metadata;

namespace Reaqtor.Remoting.QueryCoordinator;

//
// NB: The archived QueryCoordinatorServiceProvider was public, and its assembly was [assembly: CLSCompliant(false)]
//     "Due to dependency on Azure library which is not CLS-compliant." That Cosmos/Azure coupling is gone, so this
//     assembly is correctly [assembly: CLSCompliant(true)] (see Properties/AssemblyInfo.cs). This type's public ctor
//     takes AzureReactiveMetadataProxy, which lives in Reaqtor.Remoting.Platform.Core (an assembly that does not
//     assert CLS compliance), so exposing it on a *public* member would trip CS3001 (CLS error under
//     TreatWarningsAsErrors). The type is only ever constructed within this assembly (from
//     QueryCoordinatorServiceConnection.Start), exactly like its sibling QueryEvaluatorServiceProvider which is
//     already internal, so we narrow it to internal. This is behaviour-neutral and keeps the assembly CLS-compliant
//     without flipping the flag to false or modifying Platform.Core.
//
internal class QueryCoordinatorServiceProvider : IReactiveServiceProvider<ExpressionSlim>
{
    private static readonly MethodInfo DefineObservableAsyncMethodDefinition = ((MethodInfo)ReflectionHelpers.InfoOf(() => DefineFireHoseObservableAsync<object>(null, null, null))).GetGenericMethodDefinition();
    private static readonly MethodInfo DefineObserverAsyncMethodDefinition = ((MethodInfo)ReflectionHelpers.InfoOf(() => DefineFireHoseObserverAsync<object>(null, null, null))).GetGenericMethodDefinition();

    private readonly AzureReactiveMetadataProxy _azureMetadataProxy;
    private readonly IReadOnlyList<IReactiveServiceProvider<ExpressionSlim>> _queryEvaluators;
    private readonly IQueryEvaluatorSelector _selector;

    public QueryCoordinatorServiceProvider(AzureReactiveMetadataProxy azureMetadataProxy, IEnumerable<IReactiveServiceProvider<ExpressionSlim>> queryEvaluators)
        : this(azureMetadataProxy, queryEvaluators, FirstQueryEvaluatorSelector.Instance)
    {
    }

    public QueryCoordinatorServiceProvider(AzureReactiveMetadataProxy azureMetadataProxy, IEnumerable<IReactiveServiceProvider<ExpressionSlim>> queryEvaluators, IQueryEvaluatorSelector selector)
    {
        _queryEvaluators = [.. (queryEvaluators ?? throw new ArgumentNullException(nameof(queryEvaluators)))];
        _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        _azureMetadataProxy = azureMetadataProxy;
        Provider = new MetadataBindingQueryProvider(azureMetadataProxy);
    }

    // QC -> QE placement (plan §4.6): pick the evaluator that owns this entity URI. Keyed on the URI so a
    // create and its later delete resolve to the same evaluator. With one evaluator (or the default selector)
    // this is always _queryEvaluators[0] — the archived _queryEvaluators.First() behaviour.
    private IReactiveServiceProvider<ExpressionSlim> SelectEvaluator(Uri entityUri)
        => _queryEvaluators[_selector.SelectIndex(_queryEvaluators.Count, entityUri)];

    #region IReactiveServiceProvider

    public async Task CreateSubscriptionAsync(Uri subscriptionUri, ExpressionSlim subscription, object state, CancellationToken token)
    {
        await SelectEvaluator(subscriptionUri).CreateSubscriptionAsync(subscriptionUri, subscription, state, token).ConfigureAwait(false);
    }

    public async Task DeleteSubscriptionAsync(Uri subscriptionUri, CancellationToken token)
    {
        await SelectEvaluator(subscriptionUri).DeleteSubscriptionAsync(subscriptionUri, token).ConfigureAwait(false);
    }

    public async Task CreateStreamAsync(Uri streamUri, ExpressionSlim stream, object state, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(streamUri);
        ArgumentNullException.ThrowIfNull(stream);

        var streamExpr = stream.ToExpression();

#pragma warning disable CA2000 // Dispose objects before losing scope. (Would trigger deletion of the artifact. Not desirable here.)
        var entity = new AsyncReactiveStreamTableEntity(streamUri, streamExpr, state);
#pragma warning restore CA2000

        if (IsFireHose(stream))
        {
            // TODO [SLIMIFY] - Infer type from slim expression and drop dependency on CLR types
            var subjectType = streamExpr.Type.FindGenericType(typeof(IAsyncReactiveQubject<,>));

            if (subjectType != null)
            {
                var subjectTypeArguments = subjectType.GetGenericArguments();
                var subjectInputType = subjectTypeArguments[0];
                var subjectOutputType = subjectTypeArguments[1];

                if (subjectInputType != subjectOutputType)
                {
                    throw new QueryCoordinatorHostException(string.Format(
                        CultureInfo.InvariantCulture,
                        "Input type '{0}' and output type '{1}' used in the creation of a stream using stream factory '{2}' don't match. This stream factory expects equal input and output types.",
                        subjectInputType,
                        subjectOutputType,
                        Platform.Constants.Identifiers.Observable.FireHose.String));
                }

                var elementType = subjectOutputType;

                var topicUri = streamUri;

                // TODO [SLIMIFY] - Infer type from slim expression and drop dependency on CLR types, eliminating the generic parameter
                var defineObservableAsyncMethod = DefineObservableAsyncMethodDefinition.MakeGenericMethod(elementType);
                var defineObservableAsyncTask = (Task)defineObservableAsyncMethod.Invoke(null, [this, streamUri, topicUri]);

                // TODO [SLIMIFY] - Infer type from slim expression and drop dependency on CLR types, eliminating the generic parameter
                var defineObserverAsyncMethod = DefineObserverAsyncMethodDefinition.MakeGenericMethod(elementType);
                var defineObserverAsyncTask = (Task)defineObserverAsyncMethod.Invoke(null, [this, streamUri, topicUri]);

                await Task.WhenAll(defineObservableAsyncTask, defineObserverAsyncTask).ConfigureAwait(false);

                await _azureMetadataProxy.Streams.AddAsync(streamUri, entity).ConfigureAwait(false);
            }
            else
            {
                throw new QueryCoordinatorHostException(string.Format(CultureInfo.InvariantCulture, "Unable to determine the desired element type for stream '{0}' from its creation expression '{1}'.", streamUri, streamExpr.ToCSharpString(allowCompilerGeneratedNames: true)));
            }
        }
        else
        {
            await SelectEvaluator(streamUri).CreateStreamAsync(streamUri, stream, state, token).ConfigureAwait(false);
        }
    }

    public async Task DeleteStreamAsync(Uri streamUri, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(streamUri);

        if (_azureMetadataProxy.Streams.TryGetValue(streamUri, out var entity))
        {
            if (IsFireHose(entity.Expression.ToExpressionSlim()))
            {
                await Task.WhenAll(
                    UndefineObservableAsync(streamUri, token),
                    UndefineObserverAsync(streamUri, token)
                ).ConfigureAwait(false);
            }

            await _azureMetadataProxy.Streams.RemoveAsync(streamUri).ConfigureAwait(false);
        }
    }

    public Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(Uri observerUri, CancellationToken token)
    {
        if (!_azureMetadataProxy.Observers.TryGetValue(observerUri, out _))
        {
            throw new KeyNotFoundException();
        }

        //
        // NB: Implementation omitted here; most concrete implementations of QCs simply return an expression tree to
        //     the client, representing the observer to instantiate in a client library in order to submit events. In
        //     fact, they'd return a more derived Qbserver<T> type, containing an expression, to perform reverse
        //     shipping of expression trees to the client. Note that this expression can contain a composition of
        //     observers and operators, e.g. some `Catch` operator on an observer for fallback, or a `RoundRobin` to
        //     combine multiple observers to pick from (for scaling out), or some `Partitioned` to apply a partition
        //     selector to events of type `T` received by `OnNext` in order to pick an underlying `IObserver<T>` that
        //     refers to the partition endpoint to publish to. Similarly, the expression can encapsulate retry logic,
        //     and logic to re-resolve the observer by calling the QC again (e.g. based on a "lease" time or when an
        //     error occurs).
        //

        throw new NotImplementedException();
    }

    public async Task DefineObservableAsync(Uri observableUri, ExpressionSlim observable, object state, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(observableUri);
        ArgumentNullException.ThrowIfNull(observable);

        var entity = new AsyncReactiveObservableTableEntity(observableUri, observable.ToExpression(), state);

        await _azureMetadataProxy.Observables.AddAsync(observableUri, entity).ConfigureAwait(false);
    }

    public async Task UndefineObservableAsync(Uri observableUri, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(observableUri);

        await _azureMetadataProxy.Observables.RemoveAsync(observableUri).ConfigureAwait(false);
    }

    public async Task DefineObserverAsync(Uri observerUri, ExpressionSlim observer, object state, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(observerUri);
        ArgumentNullException.ThrowIfNull(observer);

        var entity = new AsyncReactiveObserverTableEntity(observerUri, observer.ToExpression(), state);

        await _azureMetadataProxy.Observers.AddAsync(observerUri, entity).ConfigureAwait(false);
    }

    public async Task UndefineObserverAsync(Uri observerUri, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(observerUri);

        await _azureMetadataProxy.Observers.RemoveAsync(observerUri).ConfigureAwait(false);
    }

    public async Task DefineStreamFactoryAsync(Uri streamFactoryUri, ExpressionSlim streamFactory, object state, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(streamFactoryUri);
        ArgumentNullException.ThrowIfNull(streamFactory);

        var entity = new AsyncReactiveStreamFactoryTableEntity(streamFactoryUri, streamFactory.ToExpression(), state);

        await _azureMetadataProxy.StreamFactories.AddAsync(streamFactoryUri, entity).ConfigureAwait(false);
    }

    public async Task UndefineStreamFactoryAsync(Uri streamFactoryUri, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(streamFactoryUri);

        await _azureMetadataProxy.StreamFactories.RemoveAsync(streamFactoryUri).ConfigureAwait(false);
    }

    public async Task DefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, ExpressionSlim subscriptionFactory, object state, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFactoryUri);
        ArgumentNullException.ThrowIfNull(subscriptionFactory);

        // NB: The functionality to insert the hook needs to be revisited in order to have an effect on subscriptions created via subscription
        //     factories. However, its applicability will be limited when factories for things like "composite subscriptions" are used. While
        //     only observable.Subscribe(observer) subscriptions can be "hooked" using a Finally operator, it's unclear how the same technique
        //     could be applied to e.g. composite subscriptions. The better option is likely to add a mechanism that can detect the transition
        //     of a subscription to a disposed state trigger from within the engine. However, that will lack information about the cause of
        //     termination, e.g. due to an OnError or OnCompleted message (in fact, in a general notion of subscriptions there's no requirement
        //     to have an observer given that the ISubscription algebra is one that operates on "traversable resources" in the most general
        //     sense imaginable).
        //
        //     One temporary workaround could be for the QC to intercept subscription factory definition operations and scan for a single
        //     top-level occurrence of rx://observable/subscribe, and attach the hook the the observable portion. Other non-hookable expressions
        //     could be rejected for the time being. The mechanism to put in this hook could simply rewrite rx://observable/subscribe to some
        //     other subscription factory which expands into (o, v) => rx://observable/subscribe(o.CleanupHook(), v).
        //
        //     See Reaqtor.Remoting.QueryEvaluator\QueryEvaluatorServiceProvider.cs for the corresponding QE code.

        subscriptionFactory = InjectCleanupHook(subscriptionFactory);

        var entity = new AsyncReactiveSubscriptionFactoryTableEntity(subscriptionFactoryUri, subscriptionFactory.ToExpression(), state);

        await _azureMetadataProxy.SubscriptionFactories.AddAsync(subscriptionFactoryUri, entity).ConfigureAwait(false);
    }

    public async Task UndefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(subscriptionFactoryUri);

        await _azureMetadataProxy.SubscriptionFactories.RemoveAsync(subscriptionFactoryUri).ConfigureAwait(false);
    }

    public IQueryProvider Provider { get; }

    #endregion

    #region Private methods

    /// <summary>
    /// Defines the observable side of a firehose stream.
    /// </summary>
    /// <typeparam name="T">Type of data received by the firehose consumer.</typeparam>
    /// <param name="qc">The query coordinator service provider.</param>
    /// <param name="streamUri">The URI identifying the stream as specified by the user.</param>
    /// <param name="topicUri">The topic for firehose observable constructor.</param>
    /// <returns>Task used to await the completion of the definition of the firehose observable, or an exception.</returns>
    private static Task DefineFireHoseObservableAsync<T>(QueryCoordinatorServiceProvider qc, Uri streamUri, Uri topicUri)
    {
        var firehoseObservable = Expression.Parameter(typeof(Func<Uri, IAsyncReactiveQbservable<T>>), Platform.Constants.Identifiers.Observable.FireHose.String);

        var invokeFirehose = Expression.Invoke(firehoseObservable, Expression.Constant(topicUri, typeof(Uri)));

        return qc.DefineObservableAsync(streamUri, invokeFirehose.ToExpressionSlim(), null, CancellationToken.None);
    }

    /// <summary>
    /// Defines the observer side of a firehose stream.
    /// </summary>
    /// <typeparam name="T">Type of data received by the firehose producer.</typeparam>
    /// <param name="qc">The query coordinator service provider.</param>
    /// <param name="uri">The URI identifying the stream as specified by the user.</param>
    /// <param name="topic">The topic for firehose observer constructor.</param>
    /// <returns>Task used to await the completion of the definition of the firehose observer, or an exception.</returns>
    private static Task DefineFireHoseObserverAsync<T>(QueryCoordinatorServiceProvider qc, Uri uri, Uri topic)
    {
        var firehoseObserver = Expression.Parameter(typeof(Func<Uri, IAsyncReactiveQbserver<T>>), Platform.Constants.Identifiers.Observer.FireHose.String);

        var invokeFirehose = Expression.Invoke(firehoseObserver, Expression.Constant(topic, typeof(Uri)));

        return qc.DefineObserverAsync(uri, invokeFirehose.ToExpressionSlim(), null, CancellationToken.None);
    }

    /// <summary>
    /// Determines whether the stream [is fire hose].
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>true if it is a firehose stream, false otherwise</returns>
    private static bool IsFireHose(ExpressionSlim stream)
    {
        if (stream is InvocationExpressionSlim invoke)
        {
            if (invoke.Expression is ParameterExpressionSlim parameter && parameter.Name == Platform.Constants.Identifiers.Observable.FireHose.String)
            {
                return true;
            }
        }

        return false;
    }

    private static ExpressionSlim InjectCleanupHook(ExpressionSlim subscriptionFactory)
    {
        if (subscriptionFactory is not LambdaExpressionSlim lambda ||
            lambda.Body is not InvocationExpressionSlim invoke ||
            invoke.Expression is not ParameterExpressionSlim parameter ||
            parameter.Name != Constants.SubscribeUri)
        {
            throw new NotSupportedException("Only subscription factories that are defined as parameterized subscriptions to observable sequences are supported right now.");
        }

        // NB: Loose coupling between QC and QE; the constant below is the same as the one defined in HostOperatorConstants.cs in the QE project.

        var withCleanupHook = ExpressionSlim.Parameter(parameter.Type, "reactor://platform.bing.com/qeh/operators/withCleanupSubscription");

        var res = lambda.Update(invoke.Update(withCleanupHook, invoke.Arguments), lambda.Parameters);

        return res;
    }

    #endregion

    #region Private classes

    private sealed class MetadataBindingQueryProvider : IQueryProvider
    {
        private static readonly MethodInfo s_genericToList = ((MethodInfo)ReflectionHelpers.InfoOf(() => Enumerable.ToList<object>(null))).GetGenericMethodDefinition();

        private readonly IReactiveMetadataProxy _metadata;

        public MetadataBindingQueryProvider(IReactiveMetadataProxy metadata)
        {
            _metadata = metadata;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            var binder = new MetadataParameterBinder(_metadata);
            var boundExpression = binder.Visit(expression);
            var results = boundExpression.Evaluate();

            // Convert IEnumerable<> results to a IList<>
            var queryableType = results.GetType().FindGenericType(typeof(IEnumerable<>));
            if (queryableType != null)
            {
                var toList = s_genericToList.MakeGenericMethod(queryableType.GenericTypeArguments);
                results = toList.Invoke(null, [results]);
            }

            return results;
        }

        private sealed class MetadataParameterBinder : ScopedExpressionVisitor<ParameterExpression>
        {
            private readonly IReactiveMetadataProxy _metadataProxy;

            public MetadataParameterBinder(IReactiveMetadataProxy metadataProxy)
            {
                _metadataProxy = metadataProxy;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (!TryLookup(node, out _) && node.Name != null)
                {
                    switch (node.Name)
                    {
                        case Constants.CurrentInstanceUri:
                            return Expression.Constant(_metadataProxy, typeof(IReactiveMetadataProxy));
                        case Constants.MetadataObservablesUri:
                            return Expression.Constant(_metadataProxy.Observables, node.Type);
                        case Constants.MetadataObserversUri:
                            return Expression.Constant(_metadataProxy.Observers, node.Type);
                        case Constants.MetadataStreamFactoriesUri:
                            return Expression.Constant(_metadataProxy.StreamFactories, node.Type);
                        case Constants.MetadataSubscriptionFactoriesUri:
                            return Expression.Constant(_metadataProxy.SubscriptionFactories, node.Type);
                        case Constants.MetadataStreamsUri:
                            return Expression.Constant(_metadataProxy.Streams, node.Type);
                        case Constants.MetadataSubscriptionsUri:
                            return Expression.Constant(_metadataProxy.Subscriptions, node.Type);
                    }
                }

                return base.VisitParameter(node);
            }

            protected override ParameterExpression GetState(ParameterExpression parameter)
            {
                return parameter;
            }
        }
    }

    #endregion
}
