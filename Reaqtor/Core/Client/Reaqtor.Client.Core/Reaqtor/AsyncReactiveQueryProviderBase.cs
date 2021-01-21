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
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Base class for reactive processing query providers, used to build observables, observers, and subjects represented by expression trees.
    /// </summary>
    public abstract partial class AsyncReactiveQueryProviderBase : IAsyncReactiveQueryProvider
    {
        #region Constructor & fields

        /// <summary>
        /// Creates a new reactive processing query provider with default factory method implementations.
        /// </summary>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        protected AsyncReactiveQueryProviderBase(IReactiveExpressionServices expressionServices)
        {
            ExpressionServices = expressionServices ?? throw new ArgumentNullException(nameof(expressionServices));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression services object, used to perform expression tree manipulations.
        /// </summary>
        public IReactiveExpressionServices ExpressionServices { get; }

        #endregion

        #region Operations

        #region Observable operations

        internal async Task<IAsyncReactiveQubscription> SubscribeAsync<T>(IAsyncReactiveQbservable<T> observable, IAsyncReactiveQbserver<T> observer, Uri subscriptionUri, object state, CancellationToken token)
        {
            var subscribe = Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<T>, IAsyncReactiveQbserver<T>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri);

            var observableExpression = observable.Expression;
            if (observableExpression.Type != typeof(IAsyncReactiveQbservable<T>) && observableExpression.NodeType == ExpressionType.Parameter)
            {
                observableExpression = Expression.Parameter(typeof(IAsyncReactiveQbservable<T>), ((ParameterExpression)observableExpression).Name);
            }

            var observerExpression = observer.Expression;
            if (observerExpression.Type != typeof(IAsyncReactiveQbserver<T>) && observerExpression.NodeType == ExpressionType.Parameter)
            {
                observerExpression = Expression.Parameter(typeof(IAsyncReactiveQbserver<T>), ((ParameterExpression)observerExpression).Name);
            }

            var expression = Expression.Invoke(subscribe, observableExpression, observerExpression);

            var normalized = ExpressionServices.Normalize(expression);

            var subscription = new KnownQubscription(normalized, subscriptionUri, this);

            await CreateSubscriptionAsync(subscription, state, token).ConfigureAwait(false);

            return subscription;
        }

        #endregion

        #region Observer operations

        internal Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(IAsyncReactiveQbserver<T> observer, CancellationToken token) => GetObserverAsyncCore<T>(observer, token);

        /// <summary>
        /// Gets an observer to send notifications to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observer">Expression tree representation of an observer to get a publication observer for.</param>
        /// <param name="token">Token used to observe cancellation requests.</param>
        /// <returns>Observer to send notifications to.</returns>
        protected abstract Task<IAsyncReactiveObserver<T>> GetObserverAsyncCore<T>(IAsyncReactiveQbserver<T> observer, CancellationToken token);

        #endregion

        #region Subscription operations

        internal Task<IAsyncReactiveQubscription> CreateSubscriptionAsync(IAsyncReactiveQubscriptionFactory factory, Uri subscriptionUri, object state, CancellationToken token)
        {
            return CreateSubscriptionAsync(factory, Array.Empty<Expression>(), subscriptionUri, state, token);
        }

        internal Task<IAsyncReactiveQubscription> CreateSubscriptionAsync<TArgs>(IAsyncReactiveQubscriptionFactory<TArgs> factory, TArgs argument, Uri subscriptionUri, object state, CancellationToken token)
        {
            return CreateSubscriptionAsync(factory, new Expression[] { Expression.Constant(argument, typeof(TArgs)) }, subscriptionUri, state, token);
        }

        internal async Task<IAsyncReactiveQubscription> CreateSubscriptionAsync(IAsyncReactiveExpressible factory, Expression[] arguments, Uri subscriptionUri, object state, CancellationToken token)
        {
            var expression = Expression.Invoke(factory.Expression, arguments);

            var normalized = ExpressionServices.Normalize(expression);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (IDisposable semantics are used for deletion behavior here.)

            var qubscription = new KnownQubscription(normalized, subscriptionUri, this);
            await CreateSubscriptionAsyncCore(qubscription, state, token).ConfigureAwait(false);

#pragma warning restore CA2000
#pragma warning restore IDE0079

            var parameterExpression = Expression.Parameter(typeof(IAsyncReactiveQubscription), subscriptionUri.ToCanonicalString());
            var parameterQubscription = new KnownQubscription(parameterExpression, subscriptionUri, this);
            return parameterQubscription;
        }

        internal Task CreateSubscriptionAsync(IAsyncReactiveQubscription subscription, object state, CancellationToken token) => CreateSubscriptionAsyncCore(subscription, state, token);

        /// <summary>
        /// Creates a subscription.
        /// </summary>
        /// <param name="subscription">Subscription to create.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the creation of the subscription, or an exception.</returns>
        protected abstract Task CreateSubscriptionAsyncCore(IAsyncReactiveQubscription subscription, object state, CancellationToken token);

        internal Task DeleteSubscriptionAsync(IAsyncReactiveQubscription subscription, CancellationToken token) => DeleteSubscriptionAsyncCore(subscription, token);

        /// <summary>
        /// Deletes a subscription.
        /// </summary>
        /// <param name="subscription">Subscription to delete.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the deletion of the subscription, or an exception.</returns>
        protected abstract Task DeleteSubscriptionAsyncCore(IAsyncReactiveQubscription subscription, CancellationToken token);

        #endregion

        #region Stream operations

        internal Task<IAsyncReactiveQubject<TInput, TOutput>> CreateStreamAsync<TInput, TOutput>(IAsyncReactiveQubjectFactory<TInput, TOutput> factory, Uri streamUri, object state, CancellationToken token)
        {
            return CreateStreamAsync<TInput, TOutput>(factory, Array.Empty<Expression>(), streamUri, state, token);
        }

        internal Task<IAsyncReactiveQubject<TInput, TOutput>> CreateStreamAsync<TArgs, TInput, TOutput>(IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> factory, TArgs argument, Uri streamUri, object state, CancellationToken token)
        {
            return CreateStreamAsync<TInput, TOutput>(factory, new Expression[] { Expression.Constant(argument, typeof(TArgs)) }, streamUri, state, token);
        }

        internal async Task<IAsyncReactiveQubject<TInput, TOutput>> CreateStreamAsync<TInput, TOutput>(IAsyncReactiveExpressible factory, Expression[] arguments, Uri streamUri, object state, CancellationToken token)
        {
            var expression = Expression.Invoke(factory.Expression, arguments);

            var normalized = ExpressionServices.Normalize(expression);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (IDisposable semantics are used for deletion behavior here.)

            var qubject = new KnownQubject<TInput, TOutput>(normalized, streamUri, this);
            await CreateStreamAsyncCore(qubject, state, token).ConfigureAwait(false);

#pragma warning restore CA2000
#pragma warning restore IDE0079

            var parameterExpression = Expression.Parameter(typeof(IAsyncReactiveQubject<TInput, TOutput>), streamUri.ToCanonicalString());
            var parameterQubject = new KnownQubject<TInput, TOutput>(parameterExpression, streamUri, this);
            return parameterQubject;
        }

        /// <summary>
        /// Creates a stream.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="stream">Stream to create.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the creation of the stream, or an exception.</returns>
        protected abstract Task CreateStreamAsyncCore<TInput, TOutput>(IAsyncReactiveQubject<TInput, TOutput> stream, object state, CancellationToken token);

        internal Task DeleteStreamAsync<TInput, TOutput>(IAsyncReactiveQubject<TInput, TOutput> stream, CancellationToken token) => DeleteStreamAsyncCore<TInput, TOutput>(stream, token);

        /// <summary>
        /// Deletes a stream.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="stream">Stream to delete.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the deletion of the stream, or an exception.</returns>
        protected abstract Task DeleteStreamAsyncCore<TInput, TOutput>(IAsyncReactiveQubject<TInput, TOutput> stream, CancellationToken token);

        #endregion

        #endregion

        #region IAsyncReactiveQueryProvider implementation

        #region CreateQbservable

        /// <summary>
        /// Creates an observable based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Observable represented by the given expression.</returns>
        public IAsyncReactiveQbservable<T> CreateQbservable<T>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out Uri uri))
            {
                return new KnownQbservable<T>(expression, uri, this);
            }

            return new Qbservable<T>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArgs, IAsyncReactiveQbservable<TResult>> CreateQbservable<TArgs, TResult>(Expression<Func<TArgs, IAsyncReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArgs, IAsyncReactiveQbservable<TResult>>(arg =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg, typeof(TArgs))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        #endregion

        #region CreateQbserver

        /// <summary>
        /// Creates an observer based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Observer represented by the given expression.</returns>
        public IAsyncReactiveQbserver<T> CreateQbserver<T>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out Uri uri))
            {
                return new KnownQbserver<T>(expression, uri, this);
            }

            return new Qbserver<T>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArgs, IAsyncReactiveQbserver<TResult>> CreateQbserver<TArgs, TResult>(Expression<Func<TArgs, IAsyncReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArgs, IAsyncReactiveQbserver<TResult>>(arg =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg, typeof(TArgs))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        #endregion

        #region CreateQubjectFactory

        /// <summary>
        /// Creates a subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Subject factory represented by the given expression.</returns>
        public IAsyncReactiveQubjectFactory<TInput, TOutput> CreateQubjectFactory<TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out Uri uri))
            {
                return new KnownQubjectFactory<TInput, TOutput>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> CreateQubjectFactory<TArgs, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out Uri uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArgs>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArgs>(expression, this);
        }

        #endregion

        #region CreateQubject

        /// <summary>
        /// Creates a subject based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the observer.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the observer.</typeparam>
        /// <param name="expression">Expression representing the subject.</param>
        /// <returns>Subject represented by the given expression.</returns>
        public IAsyncReactiveQubject<TInput, TOutput> CreateQubject<TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out Uri uri))
            {
                return new KnownQubject<TInput, TOutput>(expression, uri, this);
            }

            return new Qubject<TInput, TOutput>(expression, this);
        }

        #endregion

        #region CreateQubscription

        /// <summary>
        /// Creates a subscription based on the given expression.
        /// </summary>
        /// <param name="expression">Expression representing the subscription.</param>
        /// <returns>Subscription represented by the given expression.</returns>
        public IAsyncReactiveQubscription CreateQubscription(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out Uri uri))
            {
                return new KnownQubscription(expression, uri, this);
            }

            return new Qubscription(expression, this);
        }

        #endregion

        #region CreateQubscriptionFactory

        /// <summary>
        /// Creates a subscription factory based on the given expression.
        /// </summary>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Subscription factory represented by the given expression.</returns>
        public IAsyncReactiveQubscriptionFactory CreateQubscriptionFactory(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out Uri uri))
            {
                return new KnownQubscriptionFactory(expression, uri, this);
            }

            return new QubscriptionFactory(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IAsyncReactiveQubscriptionFactory<TArgs> CreateQubscriptionFactory<TArgs>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out Uri uri))
            {
                return new KnownQubscriptionFactory<TArgs>(expression, uri, this);
            }

            return new QubscriptionFactory<TArgs>(expression, this);
        }

        #endregion

        #endregion
    }
}
