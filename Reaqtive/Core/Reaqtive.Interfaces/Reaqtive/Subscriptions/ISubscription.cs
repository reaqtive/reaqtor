// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents a subscription to a subscribable source.
    /// </summary>
    public interface ISubscription : IDisposable
    {
        /// <summary>
        /// Accepts a visitor that will be dispatched through the subscription.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        void Accept(ISubscriptionVisitor visitor);
    }
}
