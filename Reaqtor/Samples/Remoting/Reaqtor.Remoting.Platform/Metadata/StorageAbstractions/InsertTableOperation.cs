// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Represents an insert operation on a table.
    /// </summary>
    public class InsertTableOperation : TableOperationBase
    {
        /// <summary>
        /// Constructs an insert table operation.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public InsertTableOperation(ITableEntity entity)
            : base(entity, TableOperationType.Insert)
        {
        }
    }
}
