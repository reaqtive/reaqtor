// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

namespace Reaqtor.Remoting.Protocol
{
    public interface IKeyValueStoreConnection<TKey, TValue>
    {
        bool TryAdd(TKey key, TValue value);

        bool TryRemove(TKey key, out TValue value);

        bool TryGetValue(TKey key, out TValue value);

        void Clear();
    }
}
