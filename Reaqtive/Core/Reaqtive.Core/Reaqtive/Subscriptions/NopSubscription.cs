// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive
{
    /// <summary>
    /// A simple subscription that ignores any visitors.
    /// </summary>
    public sealed class NopSubscription : ISubscription
    {
        /// <summary>
        /// Internal singleton for use in UnaryOperator.
        /// </summary>
        internal static NopSubscription NopChild = new();

        /// <summary>
        /// Accepts a visitor that will be dispatched through the subscription.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        /// <remarks>This is a no-op.</remarks>
        public void Accept(ISubscriptionVisitor visitor)
        {
        }

        /// <summary>
        /// Disposes the subscription.
        /// </summary>
        /// <remarks>This is a no-op.</remarks>
        public void Dispose()
        {
        }
    }
}
