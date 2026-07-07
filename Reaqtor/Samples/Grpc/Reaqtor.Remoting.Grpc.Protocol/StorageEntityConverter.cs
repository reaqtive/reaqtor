// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Protocol
{
    //
    // Marshals the engine's CLR StorageEntity (a read-only property bag with no settable members) to/from the wire
    // StorageEntityData DTO (plan §4.4.2). Each property carries the local StorageEntityPropertyType ordinal as an int
    // plus the string data — no EdmType / Microsoft.Azure.Cosmos.Table (§2.6).
    //
    /// <summary>Converts between <see cref="StorageEntity"/> and the wire <see cref="StorageEntityData"/>.</summary>
    public static class StorageEntityConverter
    {
        /// <summary>Engine → wire.</summary>
        public static StorageEntityData ToWire(StorageEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var properties = new Dictionary<string, StoragePropertyData>();
            foreach (var property in entity.Properties)
            {
                properties[property.Key] = new StoragePropertyData
                {
                    Type = property.Value.Type,
                    Data = property.Value.Data,
                };
            }

            return new StorageEntityData { Properties = properties };
        }

        /// <summary>Wire → engine.</summary>
        public static StorageEntity FromWire(StorageEntityData data)
        {
            ArgumentNullException.ThrowIfNull(data);

            var properties = new Dictionary<string, StorageEntityProperty>();
            if (data.Properties != null)
            {
                foreach (var property in data.Properties)
                {
                    properties[property.Key] = new StorageEntityProperty
                    {
                        Type = property.Value.Type,
                        Data = property.Value.Data,
                    };
                }
            }

            return new StorageEntity(properties);
        }
    }
}
