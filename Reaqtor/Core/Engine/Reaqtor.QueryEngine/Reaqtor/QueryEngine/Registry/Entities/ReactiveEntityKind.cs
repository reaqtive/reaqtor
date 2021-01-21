// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Enum with different reactive entity kinds.
    /// </summary>
    public enum ReactiveEntityKind
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Observable entity.
        /// </summary>
        Observable = 1,

        /// <summary>
        /// Observer entity.
        /// </summary>
        Observer = 2,

        /// <summary>
        /// Stream entity.
        /// </summary>
        Stream = 3,

        /// <summary>
        /// Stream factory entity.
        /// </summary>
        StreamFactory = 4,

        /// <summary>
        /// Subscription entity.
        /// </summary>
        Subscription = 5,

        /// <summary>
        /// Reliable subscription entity.
        /// </summary>
        ReliableSubscription = 6,

        /// <summary>
        /// Subscription factory entity.
        /// </summary>
        SubscriptionFactory = 7,

        /// <summary>
        /// Template entity.
        /// </summary>
        Template = 1023,

        /// <summary>
        /// Other entity.
        /// </summary>
        Other = 1024,
    }
}
