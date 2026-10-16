// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Metadata;

/// <summary>
/// An abstraction of operations that can be performed on storage tables.
/// </summary>
/// <remarks>
/// Cosmos-free replacement for the original <c>ITableOperation</c> (which used
/// <c>Microsoft.Azure.Cosmos.Table</c>); see plan §2.6. <see cref="Entity"/> is exposed via the local
/// <see cref="ITableEntity"/> type and <see cref="Type"/> via the local <see cref="TableOperationType"/> enum.
/// </remarks>
public interface ITableOperation
{
    /// <summary>
    /// Gets the entity to perform the operation on.
    /// </summary>
    ITableEntity Entity { get; }

    /// <summary>
    /// Gets the type of table operation.
    /// </summary>
    TableOperationType Type { get; }
}
