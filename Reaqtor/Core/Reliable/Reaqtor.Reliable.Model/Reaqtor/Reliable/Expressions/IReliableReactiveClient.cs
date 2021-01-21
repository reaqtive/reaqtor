// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Reliable.Expressions
{
    public interface IReliableReactiveClient
    {
        #region GetStreamFactory

        IReliableQubjectFactory<TInput, TOutput> GetStreamFactory<TInput, TOutput>(Uri uri);
        IReliableQubjectFactory<TInput, TOutput, TArgs> GetStreamFactory<TArgs, TInput, TOutput>(Uri uri);

        #endregion

        #region GetStream

        IReliableMultiQubject<TInput, TOutput> GetStream<TInput, TOutput>(Uri uri);

        #endregion

        #region GetObservable

        IReliableQbservable<T> GetObservable<T>(Uri uri);
        Func<TArgs, IReliableQbservable<TResult>> GetObservable<TArgs, TResult>(Uri uri);

        #endregion

        #region GetObserver

        IReliableQbserver<T> GetObserver<T>(Uri uri);
        Func<TArgs, IReliableQbserver<TResult>> GetObserver<TArgs, TResult>(Uri uri);

        #endregion

        #region GetSubscriptionFactory

        IReliableQubscriptionFactory GetSubscriptionFactory(Uri uri);
        IReliableQubscriptionFactory<TArgs> GetSubscriptionFactory<TArgs>(Uri uri);

        #endregion

        #region GetSubscription

        IReliableQubscription GetSubscription(Uri uri);

        #endregion

        #region Provider

        /// <summary>
        /// Gets the query provider that is used to build observables, observers, and streams.
        /// </summary>
        IReliableQueryProvider Provider
        {
            get;
        }

        #endregion
    }
}
