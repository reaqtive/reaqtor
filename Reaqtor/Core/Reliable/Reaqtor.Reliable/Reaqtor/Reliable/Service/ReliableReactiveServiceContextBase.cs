// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public abstract class ReliableReactiveServiceContextBase : IReliableReactive
    {
        #region Client

        protected abstract IReliableReactiveClient Client { get; }

        public IReliableQubjectFactory<TInput, TOutput> GetStreamFactory<TInput, TOutput>(Uri uri) => Client.GetStreamFactory<TInput, TOutput>(uri);

        public IReliableQubjectFactory<TInput, TOutput, TArgs> GetStreamFactory<TArgs, TInput, TOutput>(Uri uri) => Client.GetStreamFactory<TArgs, TInput, TOutput>(uri);

        public IReliableMultiQubject<TInput, TOutput> GetStream<TInput, TOutput>(Uri uri) => Client.GetStream<TInput, TOutput>(uri);

        public IReliableQbservable<T> GetObservable<T>(Uri uri) => Client.GetObservable<T>(uri);

        public Func<TArgs, IReliableQbservable<TResult>> GetObservable<TArgs, TResult>(Uri uri) => Client.GetObservable<TArgs, TResult>(uri);

        public IReliableQbserver<T> GetObserver<T>(Uri uri) => Client.GetObserver<T>(uri);

        public Func<TArgs, IReliableQbserver<TResult>> GetObserver<TArgs, TResult>(Uri uri) => Client.GetObserver<TArgs, TResult>(uri);

        public IReliableQubscriptionFactory GetSubscriptionFactory(Uri uri) => Client.GetSubscriptionFactory(uri);

        public IReliableQubscriptionFactory<TArgs> GetSubscriptionFactory<TArgs>(Uri uri) => Client.GetSubscriptionFactory<TArgs>(uri);

        public IReliableQubscription GetSubscription(Uri uri) => Client.GetSubscription(uri);

        public IReliableQueryProvider Provider => Client.Provider;

        #endregion
    }
}
