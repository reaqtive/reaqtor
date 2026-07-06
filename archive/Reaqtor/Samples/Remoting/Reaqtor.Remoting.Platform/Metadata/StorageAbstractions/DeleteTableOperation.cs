// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Represents a delete operation on a table.
    /// </summary>
    public class DeleteTableOperation : TableOperationBase
    {
        /// <summary>
        /// Constructs a delete table operation.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public DeleteTableOperation(ITableEntity entity)
            : base(entity, TableOperationType.Delete)
        {
        }
    }
}
