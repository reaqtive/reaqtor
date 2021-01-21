// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// An abstraction of a storage table that can execute operations.
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Creates the table if it does not exist.
        /// </summary>
        /// <param name="options">Options to use to perform the operation.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// true if table created, false otherwise.
        /// </returns>
        Task<bool> CreateIfNotExistsAsync(TableRequestOptions options, object state);

        /// <summary>
        /// Executes a table operation on the table.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="options">Options to use to perform the operation.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        Task<TableResult> ExecuteAsync(ITableOperation operation, TableRequestOptions options, object state);
    }
}
