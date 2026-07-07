// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Metadata
{
#pragma warning disable CA1720 // Identifier contains type name. (Members deliberately mirror the Cosmos EdmType names this enum replaces.)

    /// <summary>
    /// The set of property data types supported by the storage abstraction.
    /// </summary>
    /// <remarks>
    /// This mirrors the integer values stored in <see cref="Reaqtor.Remoting.Protocol.StorageEntityProperty.Type"/>.
    /// In the archived (net472) code these were the values of <c>Microsoft.Azure.Cosmos.Table.EdmType</c>; this
    /// local enum removes the <c>Microsoft.Azure.Cosmos.Table</c> dependency from the transport-neutral metadata
    /// layer (see plan §2.6). The numeric ordering matches the original <c>EdmType</c> members that were used
    /// (<c>String=0, Binary=1, Boolean=2, DateTime=3, Double=4, Guid=5, Int32=6, Int64=7</c>); we keep the same
    /// ordinals for the members we use so that previously-persisted <see cref="Reaqtor.Remoting.Protocol.StorageEntityProperty.Type"/>
    /// integer values continue to round-trip.
    /// </remarks>
    public enum StorageEntityPropertyType
    {
        /// <summary>An Edm.String data type.</summary>
        String = 0,

        /// <summary>An Edm.Boolean data type.</summary>
        Boolean = 2,

        /// <summary>An Edm.DateTime data type.</summary>
        DateTime = 3,

        /// <summary>An Edm.Double data type.</summary>
        Double = 4,

        /// <summary>An Edm.Guid data type.</summary>
        Guid = 5,

        /// <summary>An Edm.Int32 data type.</summary>
        Int32 = 6,

        /// <summary>An Edm.Int64 data type.</summary>
        Int64 = 7,
    }

#pragma warning restore CA1720
}
