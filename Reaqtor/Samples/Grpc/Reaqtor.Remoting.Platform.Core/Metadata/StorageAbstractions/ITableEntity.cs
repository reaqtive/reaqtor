// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.Remoting.Metadata;

/// <summary>
/// An abstraction of a storage table entity.
/// </summary>
/// <remarks>
/// This is a Cosmos-free replacement for <c>Microsoft.Azure.Cosmos.Table.ITableEntity</c> (see plan §2.6),
/// carrying only the members the transport-neutral storage layer reads. Property values are exposed via the
/// local <see cref="EntityProperty"/> type.
/// </remarks>
public interface ITableEntity
{
    /// <summary>Gets or sets the partition key of the entity.</summary>
    string PartitionKey { get; set; }

    /// <summary>Gets or sets the row key of the entity.</summary>
    string RowKey { get; set; }

    /// <summary>Gets or sets the entity's timestamp.</summary>
    DateTimeOffset Timestamp { get; set; }

    /// <summary>Gets or sets the entity's current ETag.</summary>
    string ETag { get; set; }

    /// <summary>
    /// Serializes the entity's properties to a dictionary of <see cref="EntityProperty"/>.
    /// </summary>
    /// <param name="operationContext">An opaque operation context (unused by the in-memory storage layer).</param>
    /// <returns>The serialized properties keyed by property name.</returns>
    IDictionary<string, EntityProperty> WriteEntity(object operationContext);

    /// <summary>
    /// Deserializes the entity from a dictionary of <see cref="EntityProperty"/>.
    /// </summary>
    /// <param name="properties">The properties keyed by property name.</param>
    /// <param name="operationContext">An opaque operation context (unused by the in-memory storage layer).</param>
    void ReadEntity(IDictionary<string, EntityProperty> properties, object operationContext);
}
