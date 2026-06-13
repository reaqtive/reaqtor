// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// An abstraction of operations that can be performed on storage tables.
    /// </summary>
    public interface ITableOperation
    {
        /// <summary>
        /// The entity to perform the operation on.
        /// </summary>
        ITableEntity Entity { get; }

        /// <summary>
        /// The type of table operation.
        /// </summary>
        TableOperationType Type { get; }
    }
}
