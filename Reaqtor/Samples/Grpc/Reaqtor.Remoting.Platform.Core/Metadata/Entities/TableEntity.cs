// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Reaqtor.Remoting.Metadata;

/// <summary>
/// Base class for the metadata table entities.
/// </summary>
/// <remarks>
/// ADAPTATION (plan §2.6): this is a Cosmos-free replacement for <c>Microsoft.Azure.Cosmos.Table.TableEntity</c>,
/// which the archived (net472) metadata entities derived from. None of the entities override
/// <see cref="WriteEntity"/>/<see cref="ReadEntity"/>; they rely entirely on this base for property
/// (de)serialization. The reflection-based serialization here reproduces the behavior the Cosmos base class
/// provided: it maps the concrete entity's read/write public properties (other than the four system properties)
/// to/from <see cref="EntityProperty"/> values by CLR type. Properties of unsupported CLR types are skipped, as
/// the Cosmos implementation did. The <see cref="ITableEntity"/> contract is the local Cosmos-free abstraction
/// (see <c>Metadata/StorageAbstractions/ITableEntity.cs</c>).
/// </remarks>
public abstract class TableEntity : ITableEntity
{
    /// <summary>
    /// The names of the four system properties that are stored out-of-band and must not be (de)serialized as
    /// regular entity properties.
    /// </summary>
    // NB: The Cosmos TableEntity treated PartitionKey/RowKey/Timestamp/ETag as system properties that were not
    //     part of the property dictionary produced by WriteEntity. The in-memory storage layer
    //     (StorageConnectionTable.Insert) adds these four separately and would throw on a duplicate key if
    //     WriteEntity also emitted them, so excluding them here is load-bearing.
    private static readonly HashSet<string> s_systemProperties = new(StringComparer.Ordinal)
    {
        nameof(PartitionKey),
        nameof(RowKey),
        nameof(Timestamp),
        nameof(ETag),
    };

    /// <summary>
    /// Gets or sets the partition key of the entity.
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// Gets or sets the row key of the entity.
    /// </summary>
    public string RowKey { get; set; }

    /// <summary>
    /// Gets or sets the entity's timestamp.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the entity's current ETag.
    /// </summary>
    public string ETag { get; set; }

    /// <summary>
    /// Serializes the entity's properties to a dictionary of <see cref="EntityProperty"/>.
    /// </summary>
    /// <param name="operationContext">An opaque operation context (unused by the in-memory storage layer).</param>
    /// <returns>The serialized properties keyed by property name.</returns>
    public IDictionary<string, EntityProperty> WriteEntity(object operationContext)
    {
        var result = new Dictionary<string, EntityProperty>();

        foreach (var property in GetSerializableProperties())
        {
            var value = property.GetValue(this);

            var entityProperty = CreateEntityProperty(property.PropertyType, value);
            if (entityProperty != null)
            {
                // NB: A null value for an unsupported CLR type yields a null EntityProperty and is skipped,
                //     mirroring the reflection behavior of the Cosmos TableEntity base class.
                result[property.Name] = entityProperty;
            }
        }

        return result;
    }

    /// <summary>
    /// Deserializes the entity from a dictionary of <see cref="EntityProperty"/>.
    /// </summary>
    /// <param name="properties">The properties keyed by property name.</param>
    /// <param name="operationContext">An opaque operation context (unused by the in-memory storage layer).</param>
    public void ReadEntity(IDictionary<string, EntityProperty> properties, object operationContext)
    {
        ArgumentNullException.ThrowIfNull(properties);

        foreach (var property in GetSerializableProperties())
        {
            if (properties.TryGetValue(property.Name, out var entityProperty))
            {
                var value = ConvertEntityProperty(property.PropertyType, entityProperty);
                if (value != null)
                {
                    property.SetValue(this, value);
                }
            }
        }
    }

    /// <summary>
    /// Gets the concrete entity's public instance properties that participate in (de)serialization: those that
    /// have both a public getter and a public setter and are not one of the four system properties.
    /// </summary>
    /// <returns>The properties to (de)serialize.</returns>
    private IEnumerable<PropertyInfo> GetSerializableProperties()
    {
        foreach (var property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (s_systemProperties.Contains(property.Name))
                continue;

            if (property.GetIndexParameters().Length != 0)
                continue;

            var getter = property.GetGetMethod(nonPublic: false);
            var setter = property.GetSetMethod(nonPublic: false);

            // NB: Require both a public getter AND a public setter, matching the Cosmos TableEntity behavior.
            //     This auto-excludes computed get-only members (e.g. KnownTableEntity.Uri, ReactiveResource*'s
            //     Expression/IsParameterized) and the get-only-with-private-set members (State, CreationTime,
            //     DefinitionTime) - none of which are persisted.
            if (getter != null && setter != null)
            {
                yield return property;
            }
        }
    }

    /// <summary>
    /// Maps a CLR property value to an <see cref="EntityProperty"/> using the property's CLR type.
    /// </summary>
    /// <param name="propertyType">The CLR type of the property.</param>
    /// <param name="value">The property value.</param>
    /// <returns>The matching <see cref="EntityProperty"/>, or <c>null</c> if the CLR type is not supported.</returns>
    private static EntityProperty CreateEntityProperty(Type propertyType, object value)
    {
        if (propertyType == typeof(string))
            return EntityProperty.GeneratePropertyForString((string)value);
        if (propertyType == typeof(int))
            return EntityProperty.GeneratePropertyForInt((int?)value);
        if (propertyType == typeof(long))
            return EntityProperty.GeneratePropertyForLong((long?)value);
        if (propertyType == typeof(double))
            return EntityProperty.GeneratePropertyForDouble((double?)value);
        if (propertyType == typeof(bool))
            return EntityProperty.GeneratePropertyForBool((bool?)value);
        if (propertyType == typeof(Guid))
            return EntityProperty.GeneratePropertyForGuid((Guid?)value);
        if (propertyType == typeof(DateTimeOffset))
            return EntityProperty.GeneratePropertyForDateTimeOffset((DateTimeOffset?)value);

        // NB: Cosmos stored DateTime as an Edm.DateTime; we map it onto the local DateTimeOffset property type,
        //     converting the value across.
        if (propertyType == typeof(DateTime))
            return EntityProperty.GeneratePropertyForDateTimeOffset(value == null ? null : new DateTimeOffset((DateTime)value));

        // NB: Unsupported CLR types are skipped (the Cosmos TableEntity reflection ignored them too).
        return null;
    }

    /// <summary>
    /// Reads a CLR value from an <see cref="EntityProperty"/> for the specified CLR property type.
    /// </summary>
    /// <param name="propertyType">The CLR type of the target property.</param>
    /// <param name="entityProperty">The serialized property value.</param>
    /// <returns>The CLR value to assign, or <c>null</c> if the CLR type is not supported.</returns>
    private static object ConvertEntityProperty(Type propertyType, EntityProperty entityProperty)
    {
        if (entityProperty == null)
            return null;

        if (propertyType == typeof(string))
            return entityProperty.StringValue;
        if (propertyType == typeof(int))
            return entityProperty.Int32Value;
        if (propertyType == typeof(long))
            return entityProperty.Int64Value;
        if (propertyType == typeof(double))
            return entityProperty.DoubleValue;
        if (propertyType == typeof(bool))
            return entityProperty.BooleanValue;
        if (propertyType == typeof(Guid))
            return entityProperty.GuidValue;
        if (propertyType == typeof(DateTimeOffset))
            return entityProperty.DateTimeOffsetValue;

        // NB: Convert a stored Edm.DateTime (held as a DateTimeOffset) back to a DateTime when the target
        //     property is DateTime, mirroring the Cosmos TableEntity round-trip.
        if (propertyType == typeof(DateTime))
            return entityProperty.DateTimeOffsetValue?.UtcDateTime;

        // NB: Unsupported CLR types are skipped (the Cosmos TableEntity reflection ignored them too).
        return null;
    }
}
