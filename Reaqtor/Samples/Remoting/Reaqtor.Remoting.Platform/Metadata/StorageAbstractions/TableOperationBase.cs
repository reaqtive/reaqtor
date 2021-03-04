// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Base class for table operations.
    /// </summary>
    public abstract class TableOperationBase : ITableOperation
    {
        /// <summary>
        /// Base constructor for table operations.
        /// </summary>
        /// <param name="entity">The entity to perform the operation on.</param>
        /// <param name="type">The type of table operation.</param>
        protected TableOperationBase(ITableEntity entity, TableOperationType type)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            Type = type;
        }

        /// <summary>
        /// The entity to perform the operation on.
        /// </summary>
        public ITableEntity Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// The type of table operation.
        /// </summary>
        public TableOperationType Type
        {
            get;
            private set;
        }
    }
}
