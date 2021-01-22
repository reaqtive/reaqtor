// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Client;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public abstract class ReliableQueryProviderBase : IReliableQueryProvider
    {
        #region Constructor & fields

        private readonly IReactiveExpressionServices _expressionServices;

        /// <summary>
        /// Creates a new reactive processing query provider with default factory method implementations.
        /// </summary>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        protected ReliableQueryProviderBase(IReactiveExpressionServices expressionServices)
        {
            _expressionServices = expressionServices ?? throw new ArgumentNullException(nameof(expressionServices));
        }

        #endregion

        #region Operations

        #region Observable

        internal IReliableQubscription Subscribe<T>(IReliableQbservable<T> observable, IReliableQbserver<T> observer, Uri subscriptionUri, object state)
        {
            var subscribe = Expression.Parameter(typeof(Func<IReliableQbservable<T>, IReliableQbserver<T>, IReliableQubscription>), Constants.SubscribeUri);
            var expression = Expression.Invoke(subscribe, observable.Expression, observer.Expression);

            var normalized = _expressionServices.Normalize(expression);

            var subscription = new KnownReliableQubscription(normalized, subscriptionUri, this);

            CreateSubscription(subscription, state);

            return subscription;
        }

        #endregion

        #region Observer

        internal IReliableReactiveObserver<T> GetObserver<T>(IReliableQbserver<T> observer) => GetObserverCore(observer);

        protected abstract IReliableReactiveObserver<T> GetObserverCore<T>(IReliableQbserver<T> observer);

        #endregion

        #region Subscription

        internal IReliableQubscription CreateSubscription(IReliableQubscriptionFactory factory, Uri subscriptionUri, object state)
        {
            var expression = Expression.Invoke(factory.Expression);

            var normalized = _expressionServices.Normalize(expression);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (IDisposable semantics are used for deletion behavior here.)

            var qubscription = new KnownReliableQubscription(normalized, subscriptionUri, this);
            CreateSubscriptionCore(qubscription, state);

#pragma warning restore CA2000
#pragma warning restore IDE0079

            var parameterExpression = Expression.Parameter(typeof(IReliableQubscription), subscriptionUri.AbsoluteUri);
            var parameterQubscription = new KnownReliableQubscription(parameterExpression, subscriptionUri, this);
            return parameterQubscription;
        }

        internal IReliableQubscription CreateSubscription<TArgs>(IReliableQubscriptionFactory<TArgs> factory, TArgs argument, Uri subscriptionUri, object state)
        {
            var expression = Expression.Invoke(factory.Expression, Expression.Constant(argument, typeof(TArgs)));

            var normalized = _expressionServices.Normalize(expression);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (IDisposable semantics are used for deletion behavior here.)

            var qubscription = new KnownReliableQubscription(normalized, subscriptionUri, this);
            CreateSubscriptionCore(qubscription, state);

#pragma warning restore CA2000
#pragma warning restore IDE0079

            return qubscription;
        }

        internal void CreateSubscription(IReliableQubscription subscription, object state) => CreateSubscriptionCore(subscription, state);

        protected abstract void CreateSubscriptionCore(IReliableQubscription subscription, object state);

        internal void DeleteSubscription(IReliableQubscription subscription) => DeleteSubscriptionCore(subscription);

        protected abstract void DeleteSubscriptionCore(IReliableQubscription subscription);

        internal void StartSubscription(IReliableQubscription subscription, long sequenceId) => StartSubscriptionCore(subscription, sequenceId);

        protected abstract void StartSubscriptionCore(IReliableQubscription subscription, long sequenceId);

        internal void AcknowledgeRange(IReliableQubscription subscription, long sequenceId) => AcknowledgeRangeCore(subscription, sequenceId);

        protected abstract void AcknowledgeRangeCore(IReliableQubscription subscription, long sequenceId);

        internal Uri GetSubscriptionResubscribeUri(IReliableQubscription subscription) => GetSubscriptionResubscribeUriCore(subscription);

        protected abstract Uri GetSubscriptionResubscribeUriCore(IReliableQubscription subscription);

        #endregion

        #region Subject

        internal IReliableMultiQubject<TInput, TOutput> CreateStream<TInput, TOutput>(IReliableQubjectFactory<TInput, TOutput> factory, Uri streamUri, object state)
        {
            var expression = Expression.Invoke(factory.Expression);

            var normalized = _expressionServices.Normalize(expression);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (IDisposable semantics are used for deletion behavior here.)

            var qubject = new KnownReliableQubject<TInput, TOutput>(normalized, streamUri, this);
            CreateStreamCore(qubject, state);

#pragma warning restore CA2000
#pragma warning restore IDE0079

            var parameterExpression = Expression.Parameter(typeof(IReliableMultiQubject<TInput, TOutput>), streamUri.AbsoluteUri);
            var parameterQubject = new KnownReliableQubject<TInput, TOutput>(parameterExpression, streamUri, this);
            return parameterQubject;
        }

        internal IReliableMultiQubject<TInput, TOutput> CreateStream<TArgs, TInput, TOutput>(IReliableQubjectFactory<TInput, TOutput, TArgs> factory, TArgs argument, Uri streamUri, object state)
        {
            var expression = Expression.Invoke(factory.Expression, Expression.Constant(argument, typeof(TArgs)));

            var normalized = _expressionServices.Normalize(expression);

            var qubject = new KnownReliableQubject<TInput, TOutput>(normalized, streamUri, this);
            CreateStreamCore(qubject, state);

            return qubject;
        }

        protected abstract void CreateStreamCore<TInput, TOutput>(IReliableMultiQubject<TInput, TOutput> stream, object state);

        internal void DeleteStream<TInput, TOutput>(IReliableMultiQubject<TInput, TOutput> stream) => DeleteStreamCore(stream);

        protected abstract void DeleteStreamCore<TInput, TOutput>(IReliableMultiQubject<TInput, TOutput> stream);

        internal IReliableQbserver<TInput> CreateObserver<TInput, TOutput>(IReliableMultiQubject<TInput, TOutput> stream) => CreateObserverCore(stream);

        protected abstract IReliableQbserver<TInput> CreateObserverCore<TInput, TOutput>(IReliableMultiQubject<TInput, TOutput> stream);

        #endregion

        #endregion

        #region IReliableQueryProvider implementation

        #region CreateQbservable

        public IReliableQbservable<T> CreateQbservable<T>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (_expressionServices.TryGetName(expression, out var uri))
            {
                return new KnownReliableQbservable<T>(expression, uri, this);
            }

            return new ReliableQbservable<T>(expression, this);
        }

        public Func<TArgs, IReliableQbservable<TResult>> CreateQbservable<TArgs, TResult>(Expression<Func<TArgs, IReliableQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArgs, IReliableQbservable<TResult>>(arg =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg, typeof(TArgs))));
            });

            _expressionServices.RegisterObject(res, expression);

            return res;
        }

        #endregion

        #region CreateQbserver

        public IReliableQbserver<T> CreateQbserver<T>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (_expressionServices.TryGetName(expression, out var uri))
            {
                return new KnownReliableQbserver<T>(expression, uri, this);
            }

            return new ReliableQbserver<T>(expression, this);
        }

        public Func<TArgs, IReliableQbserver<TResult>> CreateQbserver<TArgs, TResult>(Expression<Func<TArgs, IReliableQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArgs, IReliableQbserver<TResult>>(arg =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg, typeof(TArgs))));
            });

            _expressionServices.RegisterObject(res, expression);

            return res;
        }

        #endregion

        #region CreateQubjectFactory

        public IReliableQubjectFactory<TInput, TOutput> CreateQubjectFactory<TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (_expressionServices.TryGetName(expression, out var uri))
            {
                return new KnownReliableQubjectFactory<TInput, TOutput>(expression, uri, this);
            }

            return new ReliableQubjectFactory<TInput, TOutput>(expression, this);
        }

        public IReliableQubjectFactory<TInput, TOutput, TArgs> CreateQubjectFactory<TArgs, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (_expressionServices.TryGetName(expression, out var uri))
            {
                return new KnownReliableQubjectFactory<TInput, TOutput, TArgs>(expression, uri, this);
            }

            return new ReliableQubjectFactory<TInput, TOutput, TArgs>(expression, this);
        }

        #endregion

        #region CreateQubject

        public IReliableMultiQubject<TInput, TOutput> CreateQubject<TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (_expressionServices.TryGetName(expression, out var uri))
            {
                return new KnownReliableQubject<TInput, TOutput>(expression, uri, this);
            }

            return new ReliableQubject<TInput, TOutput>(expression, this);
        }

        #endregion

        #region CreateQubscriptionFactory

        public IReliableQubscriptionFactory CreateQubscriptionFactory(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (_expressionServices.TryGetName(expression, out var uri))
            {
                return new KnownReliableQubscriptionFactory(expression, uri, this);
            }

            return new ReliableQubscriptionFactory(expression, this);
        }

        public IReliableQubscriptionFactory<TArgs> CreateQubscriptionFactory<TArgs>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (_expressionServices.TryGetName(expression, out var uri))
            {
                return new KnownReliableQubscriptionFactory<TArgs>(expression, uri, this);
            }

            return new ReliableQubscriptionFactory<TArgs>(expression, this);
        }

        #endregion

        #region CreateQubscription

        public IReliableQubscription CreateQubscription(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (_expressionServices.TryGetName(expression, out var uri))
            {
                return new KnownReliableQubscription(expression, uri, this);
            }

            return new ReliableQubscription(expression, this);
        }

        #endregion

        #endregion
    }
}
