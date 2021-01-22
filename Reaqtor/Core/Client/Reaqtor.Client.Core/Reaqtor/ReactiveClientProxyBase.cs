// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Base class for reactive processing clients.
    /// </summary>
    public abstract partial class ReactiveClientProxyBase : IReactiveClientProxy
    {
        #region Constructor & fields

        private readonly AsyncReactiveQueryProviderBase _queryProvider;

        /// <summary>
        /// Creates a new reactive processing client using the specified query provider object.
        /// </summary>
        /// <param name="queryProvider">Query provider that is used to build observables, observers, and streams.</param>
        protected ReactiveClientProxyBase(AsyncReactiveQueryProviderBase queryProvider)
        {
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        #endregion

        #region GetStreamFactory

        /// <summary>
        /// Gets the stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IAsyncReactiveQubjectFactory<TInput, TOutput> GetStreamFactory<TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IAsyncReactiveQubjectFactory<TInput, TOutput> GetStreamFactoryCore<TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<IAsyncReactiveQubject<TInput, TOutput>>), uri);
            return Provider.CreateQubjectFactory<TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> GetStreamFactory<TArgs, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArgs, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> GetStreamFactoryCore<TArgs, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArgs, IAsyncReactiveQubject<TInput, TOutput>>), uri);
            return Provider.CreateQubjectFactory<TArgs, TInput, TOutput>(expression);
        }

        #endregion

        #region GetStream

        /// <summary>
        /// Gets the stream, represented as a subject, with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="uri">URI identifying the stream.</param>
        /// <returns>Subject object that can be used to receive and publish data.</returns>
        public IAsyncReactiveQubject<TInput, TOutput> GetStream<TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamCore<TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the stream, represented as a subject, with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="uri">URI identifying the stream.</param>
        /// <returns>Subject object that can be used to receive and publish data.</returns>
        protected virtual IAsyncReactiveQubject<TInput, TOutput> GetStreamCore<TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(IAsyncReactiveQubject<TInput, TOutput>), uri);
            return Provider.CreateQubject<TInput, TOutput>(expression);
        }

        #endregion

        #region GetObservable

        /// <summary>
        /// Gets the observable with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public IAsyncReactiveQbservable<T> GetObservable<T>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<T>(uri);
        }

        /// <summary>
        /// Gets the observable with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual IAsyncReactiveQbservable<T> GetObservableCore<T>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(IAsyncReactiveQbservable<T>), uri);
            return Provider.CreateQbservable<T>(expression);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArgs, IAsyncReactiveQbservable<TResult>> GetObservable<TArgs, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArgs, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArgs, IAsyncReactiveQbservable<TResult>> GetObservableCore<TArgs, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArgs, IAsyncReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArgs, TResult>(function);
        }

        #endregion

        #region GetObserver

        /// <summary>
        /// Gets the observer with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public IAsyncReactiveQbserver<T> GetObserver<T>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<T>(uri);
        }

        /// <summary>
        /// Gets the observer with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual IAsyncReactiveQbserver<T> GetObserverCore<T>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(IAsyncReactiveQbserver<T>), uri);
            return Provider.CreateQbserver<T>(expression);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArgs, IAsyncReactiveQbserver<TResult>> GetObserver<TArgs, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArgs, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArgs, IAsyncReactiveQbserver<TResult>> GetObserverCore<TArgs, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArgs, IAsyncReactiveQbserver<TResult>>(uri);
            return Provider.CreateQbserver<TArgs, TResult>(function);
        }

        #endregion

        #region GetStreamFactory

        /// <summary>
        /// Gets the subscription factory with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IAsyncReactiveQubscriptionFactory GetSubscriptionFactory(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore(uri);
        }

        /// <summary>
        /// Gets the subscription factory with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IAsyncReactiveQubscriptionFactory GetSubscriptionFactoryCore(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<IAsyncReactiveQubscription>), uri);
            return Provider.CreateQubscriptionFactory(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IAsyncReactiveQubscriptionFactory<TArgs> GetSubscriptionFactory<TArgs>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArgs>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IAsyncReactiveQubscriptionFactory<TArgs> GetSubscriptionFactoryCore<TArgs>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArgs, IAsyncReactiveQubscription>), uri);
            return Provider.CreateQubscriptionFactory<TArgs>(expression);
        }

        #endregion

        #region GetSubscription

        /// <summary>
        /// Gets the subscription with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription.</param>
        /// <returns>Subscription object that can be used to dispose the subscription.</returns>
        public IAsyncReactiveQubscription GetSubscription(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionCore(uri);
        }

        /// <summary>
        /// Gets the subscription with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription.</param>
        /// <returns>Subscription object that can be used to dispose the subscription.</returns>
        protected virtual IAsyncReactiveQubscription GetSubscriptionCore(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(IAsyncReactiveQubscription), uri);
            return Provider.CreateQubscription(expression);
        }

        #endregion

        #region Provider

        /// <summary>
        /// Gets the query provider that is used to build observables, observers, and streams.
        /// </summary>
        public IAsyncReactiveQueryProvider Provider => _queryProvider;

        #endregion

        #region Private implementation

        private Expression<Func<TArgs, TResult>> GetFunctionExpression<TArgs, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArgs, TResult>), uri);
            var operand = Expression.Parameter(typeof(TArgs));
            return Expression.Lambda<Func<TArgs, TResult>>(Expression.Invoke(function, operand), operand);
        }

        private Expression GetKnownResourceExpression(Type type, Uri uri)
        {
            return _queryProvider.ExpressionServices.GetNamedExpression(type, uri);
        }

        #endregion
    }
}
