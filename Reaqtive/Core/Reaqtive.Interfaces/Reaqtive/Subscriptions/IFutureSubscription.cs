// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive
{
    /// <summary>
    /// Represents a subscription that can be assigned with an inner subscription.
    /// </summary>
    public interface IFutureSubscription : ISubscription
    {
        /// <summary>
        /// Gets or sets the inner subscription.
        /// </summary>
        ISubscription Subscription
        {
            get;
            set;
        }
    }
}
