// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks for protected methods.

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Exposes reactive processing definition operations using a provider to perform service-side operations.
    /// </summary>
    public partial class ReactiveDefinitionProxy : ReactiveDefinitionProxyBase
    {
        #region Constructor & fields

        private readonly IReactiveDefinitionServiceProvider _provider;
        private readonly IReactiveExpressionServices _expressionServices;

        /// <summary>
        /// Creates a new reactive processing definition object using the specified client operations provider.
        /// </summary>
        /// <param name="provider">Definition operations provider.</param>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        public ReactiveDefinitionProxy(IReactiveDefinitionServiceProvider provider, IReactiveExpressionServices expressionServices)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _expressionServices = expressionServices ?? throw new ArgumentNullException(nameof(expressionServices));

            var thisParameter = ResourceNaming.GetThisReferenceExpression(this);
            expressionServices.RegisterObject(this, thisParameter);
        }

        #endregion

        #region StreamFactory

        /// <summary>
        /// Defines a stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory definition.</returns>
        protected override Task DefineStreamFactoryAsyncCore<TInput, TOutput>(Uri uri, IAsyncReactiveQubjectFactory<TInput, TOutput> streamFactory, object state = null, CancellationToken token = default)
        {
            var expression = _expressionServices.Normalize(streamFactory.Expression);
            return _provider.DefineStreamFactoryAsync(uri, expression, state, token);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory definition.</returns>
        protected override Task DefineStreamFactoryAsyncCore<TArgs, TInput, TOutput>(Uri uri, IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state = null, CancellationToken token = default)
        {
            var expression = _expressionServices.Normalize(streamFactory.Expression);
            return _provider.DefineStreamFactoryAsync(uri, expression, state, token);
        }

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory undefinition.</returns>
        protected override Task UndefineStreamFactoryAsyncCore(Uri uri, CancellationToken token)
        {
            return _provider.UndefineStreamFactoryAsync(uri, token);
        }

        #endregion

        #region Observable

        /// <summary>
        /// Defines an observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable definition.</returns>
        protected override Task DefineObservableAsyncCore<T>(Uri uri, IAsyncReactiveQbservable<T> observable, object state = null, CancellationToken token = default)
        {
            var expression = _expressionServices.Normalize(observable.Expression);
            return _provider.DefineObservableAsync(uri, expression, state, token);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable definition.</returns>
        protected override Task DefineObservableAsyncCore<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IAsyncReactiveQbservable<TResult>>> observable, object state = null, CancellationToken token = default)
        {
            var expression = _expressionServices.Normalize(observable);
            return _provider.DefineObservableAsync(uri, expression, state, token);
        }

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable undefinition.</returns>
        protected override Task UndefineObservableAsyncCore(Uri uri, CancellationToken token)
        {
            return _provider.UndefineObservableAsync(uri, token);
        }

        #endregion

        #region Observer

        /// <summary>
        /// Defines an observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer definition.</returns>
        protected override Task DefineObserverAsyncCore<T>(Uri uri, IAsyncReactiveQbserver<T> observer, object state = null, CancellationToken token = default)
        {
            var expression = _expressionServices.Normalize(observer.Expression);
            return _provider.DefineObserverAsync(uri, expression, state, token);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer definition.</returns>
        protected override Task DefineObserverAsyncCore<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IAsyncReactiveQbserver<TResult>>> observer, object state = null, CancellationToken token = default)
        {
            var expression = _expressionServices.Normalize(observer);
            return _provider.DefineObserverAsync(uri, expression, state, token);
        }

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer undefinition.</returns>
        protected override Task UndefineObserverAsyncCore(Uri uri, CancellationToken token)
        {
            return _provider.UndefineObserverAsync(uri, token);
        }

        #endregion

        #region SubscriptionFactory

        /// <summary>
        /// Defines a subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        protected override Task DefineSubscriptionFactoryAsyncCore(Uri uri, IAsyncReactiveQubscriptionFactory subscriptionFactory, object state = null, CancellationToken token = default)
        {
            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            return _provider.DefineSubscriptionFactoryAsync(uri, expression, state, token);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        protected override Task DefineSubscriptionFactoryAsyncCore<TArgs>(Uri uri, IAsyncReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state = null, CancellationToken token = default)
        {
            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            return _provider.DefineSubscriptionFactoryAsync(uri, expression, state, token);
        }

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory undefinition.</returns>
        protected override Task UndefineSubscriptionFactoryAsyncCore(Uri uri, CancellationToken token)
        {
            return _provider.UndefineSubscriptionFactoryAsync(uri, token);
        }

        #endregion
    }
}
