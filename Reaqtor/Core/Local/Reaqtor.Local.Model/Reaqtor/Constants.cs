// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace Reaqtor
{
    /// <summary>
    /// Provides a set of symbolic constants for built-in operations.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Identifier for the subscribe operation.
        /// </summary>
        public const string SubscribeUri = "rx://observable/subscribe";

        /// <summary>
        /// Identifier for the current instance.
        /// </summary>
        public const string CurrentInstanceUri = "rx://builtin/this";

        /// <summary>
        /// Identifier for the identity function.
        /// </summary>
        public const string IdentityFunctionUri = "rx://builtin/id";

        #region Metadata

        /// <summary>
        /// Identifier for metadata collection of stream factories.
        /// </summary>
        public const string MetadataStreamFactoriesUri = "rx://metadata/streamFactories";

        /// <summary>
        /// Identifier for metadata collection of streams.
        /// </summary>
        public const string MetadataStreamsUri = "rx://metadata/streams";

        /// <summary>
        /// Identifier for metadata collection of observables.
        /// </summary>
        public const string MetadataObservablesUri = "rx://metadata/observables";

        /// <summary>
        /// Identifier for metadata collection of observers.
        /// </summary>
        public const string MetadataObserversUri = "rx://metadata/observers";

        /// <summary>
        /// Identifier for metadata collection of subscription factories.
        /// </summary>
        public const string MetadataSubscriptionFactoriesUri = "rx://metadata/subscriptionFactories";

        /// <summary>
        /// Identifier for metadata collection of subscriptions.
        /// </summary>
        public const string MetadataSubscriptionsUri = "rx://metadata/subscriptions";

        #endregion
    }
}
