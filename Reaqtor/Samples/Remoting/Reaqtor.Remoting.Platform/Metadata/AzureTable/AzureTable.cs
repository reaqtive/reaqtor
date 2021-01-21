// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Wrapper of Azure CloudTable to implement ITable interface.
    /// </summary>
    public class AzureTable : ITable
    {
        /// <summary>
        /// The Azure CloudTable.
        /// </summary>
        private readonly CloudTable _table;

        /// <summary>
        /// Instatiates Azure CloudTable wrapper.
        /// </summary>
        /// <param name="table">The Azure CloudTable.</param>
        public AzureTable(CloudTable table)
        {
            Contract.RequiresNotNull(table);

            _table = table;
        }

        /// <summary>
        /// Creates the table if it does not exist.
        /// </summary>
        /// <param name="options">Options to use to perform the operation.</param>
        /// <param name="state">The state.</param>
        /// <returns>True if table created, false otherwise.</returns>
        public Task<bool> CreateIfNotExistsAsync(TableRequestOptions options, object state)
        {
            return _table.CreateIfNotExistsAsync(options, null /* TODO: context */);
        }

        /// <summary>
        /// Executes a table operation on the table.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="options">Options to use to perform the operation.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        /// <exception cref="ReactiveProcessingStorageException">The specified resource already exists or The specified resource does not exist or Unknown</exception>
        public Task<TableResult> ExecuteAsync(ITableOperation operation, TableRequestOptions options, object state)
        {
            var azureOperation = operation.Type switch
            {
                TableOperationType.Delete => TableOperation.Delete(operation.Entity),
                TableOperationType.Insert => TableOperation.Insert(operation.Entity),
                TableOperationType.InsertOrMerge => TableOperation.InsertOrMerge(operation.Entity),
                TableOperationType.InsertOrReplace => TableOperation.InsertOrReplace(operation.Entity),
                TableOperationType.Merge => TableOperation.Merge(operation.Entity),
                TableOperationType.Replace => TableOperation.Replace(operation.Entity),
                TableOperationType.Retrieve => TableOperation.Retrieve(operation.Entity.PartitionKey, operation.Entity.RowKey),
                _ => throw new NotSupportedException(
                    string.Format(
                    CultureInfo.InvariantCulture,
                    "'{0}' operation is not supported; only insert and delete are currently supported.",
                    operation.Type)),
            };

            return _table.ExecuteAsync(azureOperation, options, null /* TODO: context */);
        }
    }
}
