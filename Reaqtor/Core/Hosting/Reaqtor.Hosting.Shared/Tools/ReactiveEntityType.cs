// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Hosting.Shared.Tools
{
    /// <summary>
    /// The set of Reactive entity types.
    /// </summary>
    [Flags]
    public enum ReactiveEntityType
    {
        /// <summary>
        /// Value for non-Reactive entity type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Reactive entity type for observables.
        /// </summary>
        Observable = 1,

        /// <summary>
        /// Reactive entity type for observers.
        /// </summary>
        Observer = 2,

        /// <summary>
        /// Reactive entity type for subscriptions.
        /// </summary>
        Subscription = 4,

        /// <summary>
        /// Reactive entity type for stream factories.
        /// </summary>
        StreamFactory = 8,

        /// <summary>
        /// Reactive entity type for streams.
        /// </summary>
        Stream = 16,

        /// <summary>
        /// Reactive entity type for subscription factories.
        /// </summary>
        SubscriptionFactory = 32,

        /// <summary>
        /// Reactive entity type for parameterized Reactive entity types.
        /// </summary>
        Func = 1024,
    }
}
