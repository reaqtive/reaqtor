// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Metadata service provider for client-side access to metadata stored in Azure table storage.
    /// </summary>
    public class AzureMetadataServiceProvider : IReactiveMetadataServiceProvider, IReactiveMetadataEngineProvider
    {
        /// <summary>
        /// Creates a new Azure metadata service provider using the cloud table client provided.
        /// </summary>
        /// <param name="tableClient">The cloud table client.</param>
        /// <param name="storageResolver">The table address and partition key resolver.</param>
        public AzureMetadataServiceProvider(ITableClient tableClient, IStorageResolver storageResolver)
        {
            var defaultClientBackoff = TimeSpan.FromMilliseconds(100);
            var maxAttempts = 3;

            var requestOptions = new TableRequestOptions
            {
                RetryPolicy = new ExponentialRetry(defaultClientBackoff, maxAttempts)
            };

            Provider = new AzureMetadataQueryProvider(tableClient, storageResolver, requestOptions);
        }

        /// <summary>
        /// Gets the query provider exposed to IRP facilities to compose metadata queries.
        /// </summary>
        public IQueryProvider Provider { get; }
    }
}
