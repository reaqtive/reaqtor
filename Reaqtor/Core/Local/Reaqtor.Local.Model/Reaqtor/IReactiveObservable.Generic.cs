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
    /// Interface for observables representing event streams.
    /// </summary>
    /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
    public interface IReactiveObservable<out T>
    {
        /// <summary>
        /// Subscribes to the observable using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the observable's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>Subscription object that can be used to cancel the subscription, or an exception if the subscription was unsuccessful.</returns>
        IReactiveSubscription Subscribe(IReactiveObserver<T> observer, Uri subscriptionUri, object state = null);
    }
}
