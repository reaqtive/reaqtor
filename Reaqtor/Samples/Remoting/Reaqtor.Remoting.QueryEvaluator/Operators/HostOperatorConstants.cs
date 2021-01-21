// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting
{
    /// <summary>
    /// URI constants for host-aware operators.
    /// </summary>
    internal static class HostOperatorConstants
    {
        /// <summary>
        /// URI for the host-aware subscription cleanup operator.
        /// </summary>
        public const string CleanupSubscriptionUri = "reactor://platform.bing.com/qeh/operators/cleanupSubscription";

        /// <summary>
        /// URI for a subscription factory that applies the host-aware subscription cleanup operator.
        /// </summary>
        public const string SubscribeWithCleanupSubscriptionUri = "reactor://platform.bing.com/qeh/operators/withCleanupSubscription";
    }
}
