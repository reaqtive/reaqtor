// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// The resolver to resolve the storage table and the partition key.
    /// </summary>
    public sealed class AzureStorageResolver : IStorageResolver
    {
        /// <summary>
        /// The observable table name.
        /// </summary>
        private const string ObservableTableName = "MockObservable";

        /// <summary>
        /// The observer table name.
        /// </summary>
        private const string ObserverTableName = "MockObserver";

        /// <summary>
        /// The stream factory table name.
        /// </summary>
        private const string StreamFactoryTableName = "MockStreamFactory";

        /// <summary>
        /// The subscription factory table name.
        /// </summary>
        private const string SubscriptionFactoryTableName = "MockSubscriptionFactory";

        /// <summary>
        /// The stream table name.
        /// </summary>
        private const string StreamTableName = "MockStream";

        /// <summary>
        /// The subscription table name.
        /// </summary>
        private const string SubscriptionsTableName = "MockSubscriptions";

        /// <summary>
        /// The default table name.
        /// </summary>
        private const string DefaultTableName = "MockMetadata";

        /// <summary>
        /// The default partition name.
        /// </summary>
        private const string DefaultPartitionName = "Default";

        /// <summary>
        /// Resolves table name.
        /// </summary>
        /// <param name="collectionUri">The collectionUri</param>
        /// <returns>The table name.</returns>
        public string ResolveTable(string collectionUri)
        {
            Contract.RequiresNotNull(collectionUri);

            switch (collectionUri)
            {
                case Constants.MetadataObservablesUri:
                    {
                        return ObservableTableName;
                    }

                case Constants.MetadataObserversUri:
                    {
                        return ObserverTableName;
                    }

                case Constants.MetadataStreamFactoriesUri:
                    {
                        return StreamFactoryTableName;
                    }

                case Constants.MetadataSubscriptionFactoriesUri:
                    {
                        return SubscriptionFactoryTableName;
                    }

                case Constants.MetadataStreamsUri:
                    {
                        return StreamTableName;
                    }

                case Constants.MetadataSubscriptionsUri:
                    {
                        return SubscriptionsTableName;
                    }

                default:
                    return DefaultTableName;
            }
        }

        /// <summary>
        /// Resolves a partition.
        /// </summary>
        /// <param name="entityUri">The entity URI.</param>
        /// <returns>The partition name.</returns>
        public string ResolvePartition(string entityUri)
        {
            return DefaultPartitionName;
        }
    }
}
