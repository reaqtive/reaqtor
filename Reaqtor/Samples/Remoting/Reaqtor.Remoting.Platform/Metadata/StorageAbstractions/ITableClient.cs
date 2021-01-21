// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// An abstraction of a storage table client.
    /// </summary>
    public interface ITableClient
    {
        /// <summary>
        /// Gets a storage table by name.
        /// </summary>
        /// <param name="tableName">The name of the table to retrieve.</param>
        /// <returns>The table.</returns>
        ITable GetTableReference(string tableName);

        /// <summary>
        /// Gets a service context for the table.
        /// </summary>
        /// <returns>The service context for the table.</returns>
        ITableServiceContext GetTableServiceContext();
    }
}
