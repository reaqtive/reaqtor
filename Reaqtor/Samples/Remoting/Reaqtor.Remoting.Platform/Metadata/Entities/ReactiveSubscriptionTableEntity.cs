// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Table entity representing a subscription.
    /// </summary>
    public class ReactiveSubscriptionTableEntity : ReactiveProcessResourceTableEntity, IReactiveSubscriptionProcess
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        public ReactiveSubscriptionTableEntity()
        {
        }

        /// <summary>
        /// Gets a subscription object that can be used to interact with the active subscription.
        /// </summary>
        /// <returns>Subscription object to interact with the subscription.</returns>
        public IReactiveQubscription ToSubscription()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on table entities.");
        }
    }
}
