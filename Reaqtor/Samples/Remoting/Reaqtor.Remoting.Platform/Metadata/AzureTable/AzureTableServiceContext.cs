// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Wrapper of Azure TableServiceContext to implement ITableServiceContext interface.
    /// </summary>
    public class AzureTableServiceContext : ITableServiceContext
    {
        /// <summary>
        /// Gets the IQueryable interface for a given entity set name.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entitySetName">The entity set name.</param>
        /// <returns>An IQueryable interface.</returns>
        public IQueryable<T> CreateQuery<T>(string entitySetName)
            where T : new()
        {
            // NB: Removed after switch to Cosmos.Table. This used to leverage TableServiceContext.
            throw new NotSupportedException();
        }
    }
}
