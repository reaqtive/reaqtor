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

namespace Reaqtor
{
    /// <summary>
    /// Exposes reactive processing definition operations using a provider to perform service-side operations.
    /// </summary>
    public partial class ReactiveDefinition : ReactiveDefinitionBase
    {
        #region Constructor & fields

        private readonly IReactiveDefinitionEngineProvider _provider;
        private readonly IReactiveExpressionServices _expressionServices;

        /// <summary>
        /// Creates a new reactive processing definition object using the specified client operations provider.
        /// </summary>
        /// <param name="provider">Definition operations provider.</param>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        public ReactiveDefinition(IReactiveDefinitionEngineProvider provider, IReactiveExpressionServices expressionServices)
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
        protected override void DefineStreamFactoryCore<TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput> streamFactory, object state = null)
        {
            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
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
        protected override void DefineStreamFactoryCore<TArgs, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state = null)
        {
            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the stream factory.</param>
        protected override void UndefineStreamFactoryCore(Uri uri)
        {
            _provider.UndefineStreamFactory(uri);
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
        protected override void DefineObservableCore<T>(Uri uri, IReactiveQbservable<T> observable, object state = null)
        {
            var expression = _expressionServices.Normalize(observable.Expression);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observable.</param>
        protected override void UndefineObservableCore(Uri uri)
        {
            _provider.UndefineObservable(uri);
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
        protected override void DefineObserverCore<T>(Uri uri, IReactiveQbserver<T> observer, object state = null)
        {
            var expression = _expressionServices.Normalize(observer.Expression);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the observer.</param>
        protected override void UndefineObserverCore(Uri uri)
        {
            _provider.UndefineObserver(uri);
        }

        #endregion

        #region SubscriptionFactory

        /// <summary>
        /// Defines a subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore(Uri uri, IReactiveQubscriptionFactory subscriptionFactory, object state = null)
        {
            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArgs>(Uri uri, IReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state = null)
        {
            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        protected override void UndefineSubscriptionFactoryCore(Uri uri)
        {
            _provider.UndefineSubscriptionFactory(uri);
        }

        #endregion
    }
}
