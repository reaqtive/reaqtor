// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtive
{
    /// <summary>
    /// Represents a subscription that can hold a dynamic number of inner subscriptions.
    /// </summary>
    public interface ICompositeSubscription : IEnumerable<ISubscription>, ISubscription
    {
        /// <summary>
        /// Adds an inner subscription to the composite subscription.
        /// </summary>
        /// <param name="subscription">Subscription to add.</param>
        void Add(ISubscription subscription);

        /// <summary>
        /// Removes an inner subscription from the composite subscription.
        /// </summary>
        /// <param name="subscription">Subscription to remove.</param>
        void Remove(ISubscription subscription);
    }
}
