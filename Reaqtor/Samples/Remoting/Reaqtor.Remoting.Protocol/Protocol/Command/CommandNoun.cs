// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// Nouns for commands sent to a reactive service.
    /// </summary>
    public enum CommandNoun
    {
        /// <summary>
        /// The command applies to an observable resource.
        /// </summary>
        Observable,

        /// <summary>
        /// The command applies to an observer resource.
        /// </summary>
        Observer,

        /// <summary>
        /// The command applies to a stream factory.
        /// </summary>
        StreamFactory,

        /// <summary>
        /// The command applies to a stream.
        /// </summary>
        Stream,

        /// <summary>
        /// The command applies to a subscription.
        /// </summary>
        Subscription,

        /// <summary>
        /// The command applies to metadata.
        /// </summary>
        Metadata,

        /// <summary>
        /// The command applies to a subscription factory.
        /// </summary>
        SubscriptionFactory,
    }
}
