// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Event arguments involving a reliable subscription.
    /// </summary>
    internal sealed class ReliableSubscriptionEventArgs : ReactiveEntityEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="ReliableSubscriptionEventArgs"/> class for the specified entity.
        /// </summary>
        /// <param name="entity">The entity representing the reliable subscription.</param>
        public ReliableSubscriptionEventArgs(ReliableSubscriptionEntity entity)
            : base(entity.Uri, entity, ReactiveEntityKind.ReliableSubscription)
        {
        }

        /// <summary>
        /// Gets the reliable subscription entity.
        /// </summary>
        public new IReactiveSubscriptionProcess Entity => (IReactiveSubscriptionProcess)base.Entity;
    }
}
