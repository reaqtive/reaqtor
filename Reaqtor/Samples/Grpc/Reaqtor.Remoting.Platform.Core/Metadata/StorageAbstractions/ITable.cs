// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// An abstraction of a storage table that can execute operations.
    /// </summary>
    /// <remarks>
    /// Ported from the archived <c>Reaqtor.Remoting.Platform</c>. The original signatures took
    /// <c>Microsoft.Azure.Cosmos.Table.TableRequestOptions</c> and returned its <c>TableResult</c>; the Cosmos
    /// dependency has been removed (plan §2.6). The optional request-options parameter (always passed
    /// <c>null</c>/defaulted by the in-memory storage layer) is dropped, and <see cref="ITableOperation"/> /
    /// <see cref="TableResult"/> are the local Cosmos-free equivalents.
    /// </remarks>
    public interface ITable
    {
        /// <summary>
        /// Creates the table if it does not exist.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>
        /// true if table created, false otherwise.
        /// </returns>
        Task<bool> CreateIfNotExistsAsync(object state);

        /// <summary>
        /// Executes a table operation on the table.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        Task<TableResult> ExecuteAsync(ITableOperation operation, object state);
    }
}
