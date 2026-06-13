// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Table entity representing a subscription.
    /// </summary>
    public class AsyncReactiveSubscriptionTableEntity : AsyncReactiveProcessResourceTableEntity, IAsyncReactiveSubscriptionProcess
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        public AsyncReactiveSubscriptionTableEntity()
        {
        }

        /// <summary>
        /// Creates a new table entity representing a subscription with the specified URI and expression representation.
        /// </summary>
        /// <param name="uri">URI identifying the subscription represented by the table entity.</param>
        /// <param name="expression">Expression representation of the subscription.</param>
        /// <param name="state">The state.</param>
        public AsyncReactiveSubscriptionTableEntity(Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        /// <summary>
        /// Gets a subscription object that can be used to interact with the active subscription.
        /// </summary>
        /// <returns>Subscription object to interact with the subscription.</returns>
        public IAsyncReactiveQubscription ToSubscription()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on table entities.");
        }
    }
}
