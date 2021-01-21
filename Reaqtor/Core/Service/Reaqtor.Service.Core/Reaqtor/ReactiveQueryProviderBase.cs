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
    /// Base class for reactive processing query providers, used to build observables, observers, and subjects represented by expression trees.
    /// </summary>
    public abstract partial class ReactiveQueryProviderBase : IReactiveQueryProvider
    {
        #region Constructor & fields

        /// <summary>
        /// Creates a new reactive processing query provider with default factory method implementations.
        /// </summary>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        protected ReactiveQueryProviderBase(IReactiveExpressionServices expressionServices)
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

        internal IReactiveQubscription Subscribe<T>(IReactiveQbservable<T> observable, IReactiveQbserver<T> observer, Uri subscriptionUri, object state)
        {
            var subscribe = Expression.Parameter(typeof(Func<IReactiveQbservable<T>, IReactiveQbserver<T>, IReactiveQubscription>), Constants.SubscribeUri);

            var observableExpression = observable.Expression;
            if (observableExpression.Type != typeof(IReactiveQbservable<T>) && observableExpression.NodeType == ExpressionType.Parameter)
            {
                observableExpression = Expression.Parameter(typeof(IReactiveQbservable<T>), ((ParameterExpression)observableExpression).Name);
            }

            var observerExpression = observer.Expression;
            if (observerExpression.Type != typeof(IReactiveQbserver<T>) && observerExpression.NodeType == ExpressionType.Parameter)
            {
                observerExpression = Expression.Parameter(typeof(IReactiveQbserver<T>), ((ParameterExpression)observerExpression).Name);
            }

            var expression = Expression.Invoke(subscribe, observableExpression, observerExpression);

            var normalized = ExpressionServices.Normalize(expression);

            var subscription = new KnownQubscription(normalized, subscriptionUri, this);

            CreateSubscription(subscription, state);

            return subscription;
        }

        #endregion

        #region Observer operations

        internal IReactiveObserver<T> GetObserver<T>(IReactiveQbserver<T> observer) => GetObserverCore<T>(observer);

        /// <summary>
        /// Gets an observer to send notifications to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observer">Expression tree representation of an observer to get a publication observer for.</param>
        /// <returns>Observer to send notifications to.</returns>
        protected abstract IReactiveObserver<T> GetObserverCore<T>(IReactiveQbserver<T> observer);

        #endregion

        #region Subscription operations

        internal IReactiveQubscription CreateSubscription(IReactiveQubscriptionFactory factory, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, Array.Empty<Expression>(), subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArgs>(IReactiveQubscriptionFactory<TArgs> factory, TArgs argument, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(argument, typeof(TArgs)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription(IReactiveExpressible factory, Expression[] arguments, Uri subscriptionUri, object state)
        {
            var expression = Expression.Invoke(factory.Expression, arguments);

            var normalized = ExpressionServices.Normalize(expression);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (IDisposable semantics are used for deletion behavior here.)

            var qubject = new KnownQubscription(normalized, subscriptionUri, this);
            CreateSubscriptionCore(qubject, state);

#pragma warning restore CA2000
#pragma warning restore IDE0079

            var parameterExpression = Expression.Parameter(typeof(IReactiveQubscription), subscriptionUri.ToCanonicalString());
            var parameterQubscription = new KnownQubscription(parameterExpression, subscriptionUri, this);
            return parameterQubscription;
        }

        internal void CreateSubscription(IReactiveQubscription subscription, object state) => CreateSubscriptionCore(subscription, state);

        /// <summary>
        /// Creates a subscription.
        /// </summary>
        /// <param name="subscription">Subscription to create.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected abstract void CreateSubscriptionCore(IReactiveQubscription subscription, object state);

        internal void DeleteSubscription(IReactiveQubscription subscription)
        {
            DeleteSubscriptionCore(subscription);
        }

        /// <summary>
        /// Deletes a subscription.
        /// </summary>
        /// <param name="subscription">Subscription to delete.</param>
        protected abstract void DeleteSubscriptionCore(IReactiveQubscription subscription);

        #endregion

        #region Stream operations

        internal IReactiveQubject<TInput, TOutput> CreateStream<TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput> factory, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, Array.Empty<Expression>(), streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArgs, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArgs> factory, TArgs argument, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(argument, typeof(TArgs)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TInput, TOutput>(IReactiveExpressible factory, Expression[] arguments, Uri streamUri, object state)
        {
            var expression = Expression.Invoke(factory.Expression, arguments);

            var normalized = ExpressionServices.Normalize(expression);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (IDisposable semantics are used for deletion behavior here.)

            var qubject = new KnownQubject<TInput, TOutput>(normalized, streamUri, this);
            CreateStreamCore(qubject, state);

#pragma warning restore CA2000
#pragma warning restore IDE0079

            var parameterExpression = Expression.Parameter(typeof(IReactiveQubject<TInput, TOutput>), streamUri.ToCanonicalString());
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
        protected abstract void CreateStreamCore<TInput, TOutput>(IReactiveQubject<TInput, TOutput> stream, object state);

        internal void DeleteStream<TInput, TOutput>(IReactiveQubject<TInput, TOutput> stream) => DeleteStreamCore<TInput, TOutput>(stream);

        /// <summary>
        /// Deletes a stream.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="stream">Stream to delete.</param>
        protected abstract void DeleteStreamCore<TInput, TOutput>(IReactiveQubject<TInput, TOutput> stream);

        #endregion

        #endregion

        #region IReactiveQueryProvider implementation

        #region CreateQbservable

        /// <summary>
        /// Creates an observable based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Observable represented by the given expression.</returns>
        public IReactiveQbservable<T> CreateQbservable<T>(Expression expression)
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
        public Func<TArgs, IReactiveQbservable<TResult>> CreateQbservable<TArgs, TResult>(Expression<Func<TArgs, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArgs, IReactiveQbservable<TResult>>(arg =>
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
        public IReactiveQbserver<T> CreateQbserver<T>(Expression expression)
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
        public Func<TArgs, IReactiveQbserver<TResult>> CreateQbserver<TArgs, TResult>(Expression<Func<TArgs, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArgs, IReactiveQbserver<TResult>>(arg =>
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
        public IReactiveQubjectFactory<TInput, TOutput> CreateQubjectFactory<TInput, TOutput>(Expression expression)
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
        public IReactiveQubjectFactory<TInput, TOutput, TArgs> CreateQubjectFactory<TArgs, TInput, TOutput>(Expression expression)
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
        public IReactiveQubject<TInput, TOutput> CreateQubject<TInput, TOutput>(Expression expression)
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
        public IReactiveQubscription CreateQubscription(Expression expression)
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
        public IReactiveQubscriptionFactory CreateQubscriptionFactory(Expression expression)
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
        public IReactiveQubscriptionFactory<TArgs> CreateQubscriptionFactory<TArgs>(Expression expression)
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
