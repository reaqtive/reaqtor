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
    /// Interface for client operations of a reactive processing engine.
    /// </summary>
    public interface IReactiveClientEngineProvider
    {
        #region Subscription

        /// <summary>
        /// Creates a new subscription using the specified expression tree representation.
        /// </summary>
        /// <param name="subscriptionUri">URI to identify the new subscription.</param>
        /// <param name="subscription">Expression representing the subscription creation. (E.g. an invocation of the subscription operation on an observable.)</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        void CreateSubscription(Uri subscriptionUri, Expression subscription, object state);

        /// <summary>
        /// Deletes the subscription identified by the specified URI.
        /// </summary>
        /// <param name="subscriptionUri">URI of the subscription to delete.</param>
        void DeleteSubscription(Uri subscriptionUri);

        #endregion

        #region Stream

        /// <summary>
        /// Creates a new stream using the specified expression tree representation.
        /// </summary>
        /// <param name="streamUri">URI to identify the new stream.</param>
        /// <param name="stream">Expression representing the stream creation. (E.g. an invocation of a known stream factory.)</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        void CreateStream(Uri streamUri, Expression stream, object state);

        /// <summary>
        /// Deletes the stream identified by the specified URI.
        /// </summary>
        /// <param name="streamUri">URI of the stream to delete.</param>
        void DeleteStream(Uri streamUri);

        #endregion

        #region Observer

        /// <summary>
        /// Gets an observer to send notifications to.
        /// </summary>
        /// <typeparam name="T">Type of the value to send to the observer.</typeparam>
        /// <param name="observerUri">URI of the observer to send the notification to.</param>
        /// <returns>Observer that can be used to send notifications to.</returns>
        IReactiveObserver<T> GetObserver<T>(Uri observerUri);

        #endregion
    }
}
