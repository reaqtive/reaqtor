// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Reaqtor
{
    /// <summary>
    /// Interface for the data operations on reactive processing services.
    /// </summary>
    public interface IReactiveClient
    {
        #region GetStreamFactory

        /// <summary>
        /// Gets the stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        IReactiveQubjectFactory<TInput, TOutput> GetStreamFactory<TInput, TOutput>(Uri uri);

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the stream factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        IReactiveQubjectFactory<TInput, TOutput, TArgs> GetStreamFactory<TArgs, TInput, TOutput>(Uri uri);

        #endregion

        #region GetStream

        /// <summary>
        /// Gets the stream, represented as a subject, with the specified URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="uri">URI identifying the stream.</param>
        /// <returns>Subject object that can be used to receive and publish data.</returns>
        IReactiveQubject<TInput, TOutput> GetStream<TInput, TOutput>(Uri uri);

        #endregion

        #region GetObservable

        /// <summary>
        /// Gets the observable with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        IReactiveQbservable<T> GetObservable<T>(Uri uri);

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        Func<TArgs, IReactiveQbservable<TResult>> GetObservable<TArgs, TResult>(Uri uri);

        #endregion

        #region GetObserver

        /// <summary>
        /// Gets the observer with the specified URI.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        IReactiveQbserver<T> GetObserver<T>(Uri uri);

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        Func<TArgs, IReactiveQbserver<TResult>> GetObserver<TArgs, TResult>(Uri uri);

        #endregion

        #region GetSubscriptionFactory

        /// <summary>
        /// Gets the subscription factory with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        IReactiveQubscriptionFactory GetSubscriptionFactory(Uri uri);

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        IReactiveQubscriptionFactory<TArgs> GetSubscriptionFactory<TArgs>(Uri uri);

        #endregion

        #region GetSubscription

        /// <summary>
        /// Gets the subscription with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the subscription.</param>
        /// <returns>Subscription object that can be used to dispose the subscription.</returns>
        IReactiveQubscription GetSubscription(Uri uri);

        #endregion

        #region Provider

        /// <summary>
        /// Gets the query provider that is used to build observables, observers, and streams.
        /// </summary>
        IReactiveQueryProvider Provider
        {
            get;
        }

        #endregion
    }
}
