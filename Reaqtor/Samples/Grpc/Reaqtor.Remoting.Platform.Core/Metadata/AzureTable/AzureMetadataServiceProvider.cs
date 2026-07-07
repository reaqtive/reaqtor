// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;

// NB: ADAPTATION (plan §2.6): removed `using Microsoft.Azure.Cosmos.Table;` and dropped the Cosmos
//     `TableRequestOptions`/`ExponentialRetry` construction (the in-memory/gRPC-Storage backend has no remote retry
//     semantics). The query provider is now created without request options. The `using System;` for `TimeSpan`
//     became unused once the retry policy was removed and was dropped too.

namespace Reaqtor.Remoting.Metadata;

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
        Provider = new AzureMetadataQueryProvider(tableClient, storageResolver);
    }

    /// <summary>
    /// Gets the query provider exposed to IRP facilities to compose metadata queries.
    /// </summary>
    public IQueryProvider Provider { get; }
}
