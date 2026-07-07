// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// NB: ADAPTATION (plan §2.6): removed `using Microsoft.Azure.Cosmos.Table;`. ITableEntity and TableOperationType
//     bind to the local Cosmos-free abstractions in this namespace.

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Represents an insert operation on a table.
    /// </summary>
    public class RetrieveTableOperation : TableOperationBase
    {
        /// <summary>
        /// Constructs a retrieve table operation.
        /// </summary>
        /// <param name="entity">The entity to retrieve.</param>
        public RetrieveTableOperation(ITableEntity entity)
            : base(entity, TableOperationType.Retrieve)
        {
        }
    }
}
