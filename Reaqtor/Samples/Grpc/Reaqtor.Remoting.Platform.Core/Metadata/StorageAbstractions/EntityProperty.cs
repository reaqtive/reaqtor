// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// A single typed property value held by an <see cref="ITableEntity"/>.
    /// </summary>
    /// <remarks>
    /// This is a Cosmos-free replacement for <c>Microsoft.Azure.Cosmos.Table.EntityProperty</c> (see plan §2.6),
    /// carrying only the members the transport-neutral storage layer reads. The property's logical type is exposed
    /// via <see cref="PropertyType"/> using the local <see cref="StorageEntityPropertyType"/> enum.
    /// </remarks>
    public sealed class EntityProperty
    {
        private EntityProperty(StorageEntityPropertyType propertyType, object value)
        {
            PropertyType = propertyType;
            _value = value;
        }

        private readonly object _value;

        /// <summary>
        /// Gets the logical type of the property value.
        /// </summary>
        public StorageEntityPropertyType PropertyType { get; }

        /// <summary>Gets the value as a <see cref="bool"/>, or <c>null</c> if not a boolean.</summary>
        public bool? BooleanValue => PropertyType == StorageEntityPropertyType.Boolean ? (bool?)_value : null;

        /// <summary>Gets the value as a <see cref="DateTimeOffset"/>, or <c>null</c> if not a date/time.</summary>
        public DateTimeOffset? DateTimeOffsetValue => PropertyType == StorageEntityPropertyType.DateTime ? (DateTimeOffset?)_value : null;

        /// <summary>Gets the value as a <see cref="double"/>, or <c>null</c> if not a double.</summary>
        public double? DoubleValue => PropertyType == StorageEntityPropertyType.Double ? (double?)_value : null;

        /// <summary>Gets the value as a <see cref="System.Guid"/>, or <c>null</c> if not a guid.</summary>
        public Guid? GuidValue => PropertyType == StorageEntityPropertyType.Guid ? (Guid?)_value : null;

        /// <summary>Gets the value as a 32-bit integer, or <c>null</c> if not an Int32.</summary>
        public int? Int32Value => PropertyType == StorageEntityPropertyType.Int32 ? (int?)_value : null;

        /// <summary>Gets the value as a 64-bit integer, or <c>null</c> if not an Int64.</summary>
        public long? Int64Value => PropertyType == StorageEntityPropertyType.Int64 ? (long?)_value : null;

        /// <summary>Gets the value as a <see cref="string"/>, or <c>null</c> if not a string.</summary>
        public string StringValue => PropertyType == StorageEntityPropertyType.String ? (string)_value : null;

        /// <summary>Creates a boolean property.</summary>
        public static EntityProperty GeneratePropertyForBool(bool? value) => new(StorageEntityPropertyType.Boolean, value);

        /// <summary>Creates a date/time property.</summary>
        public static EntityProperty GeneratePropertyForDateTimeOffset(DateTimeOffset? value) => new(StorageEntityPropertyType.DateTime, value);

        /// <summary>Creates a double property.</summary>
        public static EntityProperty GeneratePropertyForDouble(double? value) => new(StorageEntityPropertyType.Double, value);

        /// <summary>Creates a guid property.</summary>
        public static EntityProperty GeneratePropertyForGuid(Guid? value) => new(StorageEntityPropertyType.Guid, value);

        /// <summary>Creates a 32-bit integer property.</summary>
        public static EntityProperty GeneratePropertyForInt(int? value) => new(StorageEntityPropertyType.Int32, value);

        /// <summary>Creates a 64-bit integer property.</summary>
        public static EntityProperty GeneratePropertyForLong(long? value) => new(StorageEntityPropertyType.Int64, value);

        /// <summary>Creates a string property.</summary>
        public static EntityProperty GeneratePropertyForString(string value) => new(StorageEntityPropertyType.String, value);
    }
}
