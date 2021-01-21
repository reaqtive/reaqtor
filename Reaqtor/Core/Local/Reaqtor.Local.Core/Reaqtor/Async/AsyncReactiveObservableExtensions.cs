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
    /// Provides a set of extension methods for IAsyncReactiveObservable&lt;T&gt;.
    /// </summary>
    public static class AsyncReactiveObservableExtensions
    {
        #region SubscribeAsync

        /// <summary>
        /// Subscribes to the observable using the given observer.
        /// </summary>
        /// <param name="observable">Observable to subscribe to.</param>
        /// <param name="observer">Observer to send the observable's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>Task returning a subscription object that can be used to cancel the subscription, or an exception if the subscription was unsuccessful.</returns>
        public static Task<IAsyncReactiveSubscription> SubscribeAsync<T>(this IAsyncReactiveObservable<T> observable, IAsyncReactiveObserver<T> observer, Uri subscriptionUri, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return observable.SubscribeAsync(observer, subscriptionUri, state, CancellationToken.None);
        }

        #endregion
    }
}
