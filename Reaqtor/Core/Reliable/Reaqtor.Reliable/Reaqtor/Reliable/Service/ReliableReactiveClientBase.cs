// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public abstract class ReliableReactiveClientBase : IReliableReactiveClient
    {
        #region Constructor & fields

        private readonly IReactiveExpressionServices _expressionServices;

        /// <summary>
        /// Creates a new reactive processing client using the specified expression services object.
        /// </summary>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        protected ReliableReactiveClientBase(IReactiveExpressionServices expressionServices)
        {
            _expressionServices = expressionServices ?? throw new ArgumentNullException(nameof(expressionServices));
        }

        #endregion

        #region GetStreamFactory

        public IReliableQubjectFactory<TInput, TOutput> GetStreamFactory<TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TInput, TOutput>(uri);
        }

        protected virtual IReliableQubjectFactory<TInput, TOutput> GetStreamFactoryCore<TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<IReliableMultiQubject<TInput, TOutput>>), uri);
            return Provider.CreateQubjectFactory<TInput, TOutput>(expression);
        }

        public IReliableQubjectFactory<TInput, TOutput, TArgs> GetStreamFactory<TArgs, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArgs, TInput, TOutput>(uri);
        }

        protected virtual IReliableQubjectFactory<TInput, TOutput, TArgs> GetStreamFactoryCore<TArgs, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArgs, IReliableMultiQubject<TInput, TOutput>>), uri);
            return Provider.CreateQubjectFactory<TArgs, TInput, TOutput>(expression);
        }

        #endregion

        #region GetStream

        public IReliableMultiQubject<TInput, TOutput> GetStream<TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamCore<TInput, TOutput>(uri);
        }

        protected virtual IReliableMultiQubject<TInput, TOutput> GetStreamCore<TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(IReliableMultiQubject<TInput, TOutput>), uri);
            return Provider.CreateQubject<TInput, TOutput>(expression);
        }

        #endregion

        #region GetObservable

        public IReliableQbservable<T> GetObservable<T>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<T>(uri);
        }

        protected virtual IReliableQbservable<T> GetObservableCore<T>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(IReliableQbservable<T>), uri);
            return Provider.CreateQbservable<T>(expression);
        }

        public Func<TArgs, IReliableQbservable<TResult>> GetObservable<TArgs, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArgs, TResult>(uri);
        }

        protected virtual Func<TArgs, IReliableQbservable<TResult>> GetObservableCore<TArgs, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArgs, IReliableQbservable<TResult>>(uri);
            return Provider.CreateQbservable<TArgs, TResult>(function);
        }

        #endregion

        #region GetObserver

        public IReliableQbserver<T> GetObserver<T>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<T>(uri);
        }

        protected virtual IReliableQbserver<T> GetObserverCore<T>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(IReliableQbserver<T>), uri);
            return Provider.CreateQbserver<T>(expression);
        }

        public Func<TArgs, IReliableQbserver<TResult>> GetObserver<TArgs, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArgs, TResult>(uri);
        }

        protected virtual Func<TArgs, IReliableQbserver<TResult>> GetObserverCore<TArgs, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArgs, IReliableQbserver<TResult>>(uri);
            return Provider.CreateQbserver<TArgs, TResult>(function);
        }

        #endregion

        #region GetSubscriptionFactory

        public IReliableQubscriptionFactory GetSubscriptionFactory(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore(uri);
        }

        protected virtual IReliableQubscriptionFactory GetSubscriptionFactoryCore(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<IReliableQubscription>), uri);
            return Provider.CreateQubscriptionFactory(expression);
        }

        public IReliableQubscriptionFactory<TArgs> GetSubscriptionFactory<TArgs>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArgs>(uri);
        }

        protected virtual IReliableQubscriptionFactory<TArgs> GetSubscriptionFactoryCore<TArgs>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArgs, IReliableQubscription>), uri);
            return Provider.CreateQubscriptionFactory<TArgs>(expression);
        }

        #endregion

        #region GetSubscription

        public IReliableQubscription GetSubscription(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionCore(uri);
        }

        protected virtual IReliableQubscription GetSubscriptionCore(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(IReliableQubscription), uri);
            return Provider.CreateQubscription(expression);
        }

        #endregion

        #region Provider

        public abstract IReliableQueryProvider Provider
        {
            get;
        }

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
            return _expressionServices.GetNamedExpression(type, uri);
        }

        #endregion
    }
}
