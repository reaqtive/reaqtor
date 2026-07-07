// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Collections.Generic;

namespace Reaqtor.Remoting.Protocol
{
    public interface IReactiveStateStoreConnection : IReactiveConnection
    {
        IEnumerable<string> GetCategories();

        bool TryGetItemKeys(string category, out IEnumerable<string> keys);

        bool TryGetItem(string category, string key, out byte[] value);

        void AddOrUpdateItem(string category, string key, byte[] value);

        void RemoveItem(string category, string key);

        void Clear();

        byte[] SerializeStateStore();

        void DeserializeStateStore(byte[] bytes);
    }
}
