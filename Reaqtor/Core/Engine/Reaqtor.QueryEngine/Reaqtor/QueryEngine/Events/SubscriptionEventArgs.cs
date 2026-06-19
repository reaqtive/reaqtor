// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Event arguments involving a subscription.
    /// </summary>
    internal sealed class SubscriptionEventArgs : ReactiveEntityEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="SubscriptionEventArgs"/> class for the specified entity.
        /// </summary>
        /// <param name="entity">The entity representing the subscription.</param>
        public SubscriptionEventArgs(SubscriptionEntity entity)
            : base(entity.Uri, entity, ReactiveEntityKind.Subscription)
        {
        }

        /// <summary>
        /// Gets the subscription entity.
        /// </summary>
        public new IReactiveSubscriptionProcess Entity => (IReactiveSubscriptionProcess)base.Entity;
    }
}
