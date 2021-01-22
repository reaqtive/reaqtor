// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Resolution mechanism to get a table address and a partition key from collection and entity URIs, respectively.
    /// </summary>
    public interface IStorageResolver
    {
        /// <summary>
        /// Resolves a table address from a collection URI.
        /// </summary>
        /// <param name="collectionUri">The collection URI.</param>
        /// <returns>The table address.</returns>
        string ResolveTable(string collectionUri);

        /// <summary>
        /// Resolves a partition key from an entity URI.
        /// </summary>
        /// <param name="entityUri">The entity URI.</param>
        /// <returns>The partition key.</returns>
        string ResolvePartition(string entityUri);
    }
}
