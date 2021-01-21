// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Interface for client operations of a reactive processing service.
    /// </summary>
    /// <typeparam name="TExpression">Type used for expression tree representation.</typeparam>
    public interface IReactiveClientServiceProvider<TExpression>
    {
        #region Subscription

        /// <summary>
        /// Creates a new subscription using the specified expression tree representation.
        /// </summary>
        /// <param name="subscriptionUri">URI to identify the new subscription.</param>
        /// <param name="subscription">Expression representing the subscription creation. (E.g. an invocation of the subscription operation on an observable.)</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the creation of the subscription, or an exception.</returns>
        Task CreateSubscriptionAsync(Uri subscriptionUri, TExpression subscription, object state, CancellationToken token);

        /// <summary>
        /// Deletes the subscription identified by the specified URI.
        /// </summary>
        /// <param name="subscriptionUri">URI of the subscription to delete.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the deletion of the subscription, or an exception.</returns>
        Task DeleteSubscriptionAsync(Uri subscriptionUri, CancellationToken token);

        #endregion

        #region Stream

        /// <summary>
        /// Creates a new stream using the specified expression tree representation.
        /// </summary>
        /// <param name="streamUri">URI to identify the new stream.</param>
        /// <param name="stream">Expression representing the stream creation. (E.g. an invocation of a known stream factory.)</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the creation of the stream, or an exception.</returns>
        Task CreateStreamAsync(Uri streamUri, TExpression stream, object state, CancellationToken token);

        /// <summary>
        /// Deletes the stream identified by the specified URI.
        /// </summary>
        /// <param name="streamUri">URI of the stream to delete.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the deletion of the stream, or an exception.</returns>
        Task DeleteStreamAsync(Uri streamUri, CancellationToken token);

        #endregion

        #region Observer

        /// <summary>
        /// Gets an observer to send notifications to.
        /// </summary>
        /// <typeparam name="T">Type of the value to send to the observer.</typeparam>
        /// <param name="observerUri">URI of the observer to send the notification to.</param>
        /// <param name="token">Token used to observe cancellation requests.</param>
        /// <returns>Observer that can be used to send notifcations to.</returns>
        Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(Uri observerUri, CancellationToken token);

        #endregion
    }
}
