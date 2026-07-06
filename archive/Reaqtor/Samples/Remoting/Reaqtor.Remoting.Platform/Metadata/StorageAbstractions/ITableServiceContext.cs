// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// An abstraction of storage table service context.
    /// </summary>
    public interface ITableServiceContext
    {
        /// <summary>
        /// Gets the IQueryable interface for a given entity set name.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entitySetName">The entity set name.</param>
        /// <returns>An IQueryable interface.</returns>
        IQueryable<T> CreateQuery<T>(string entitySetName)
            where T : new();
    }
}
