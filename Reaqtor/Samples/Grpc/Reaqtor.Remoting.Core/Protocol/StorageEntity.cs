// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reaqtor.Remoting.Protocol
{
    [Serializable]
    public class StorageEntity
    {
        private readonly Dictionary<string, StorageEntityProperty> _properties;

        public StorageEntity(Dictionary<string, StorageEntityProperty> properties)
        {
            _properties = properties;
        }

        public ReadOnlyDictionary<string, StorageEntityProperty> Properties => new(_properties);
    }
}
