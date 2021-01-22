// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2016 - Created this file.
//

using System;

namespace Reaqtor
{
    /// <summary>
    /// Interface for a subscription factory.
    /// </summary>
    public interface IReactiveSubscriptionFactory
    {
        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription Create(Uri subscriptionUri, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
    /// </summary>
    /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
    public interface IReactiveSubscriptionFactory<TArgs>
    {
        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="argument">Parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription Create(Uri subscriptionUri, TArgs argument, object state = null);
    }
}
