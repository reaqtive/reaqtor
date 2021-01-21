// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System.Collections.Concurrent;

namespace Reaqtor.Remoting.Protocol
{
    public class KeyValueStoreConnection<TKey, TValue> : ReactiveConnectionBase, IKeyValueStoreConnection<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _store;

        public KeyValueStoreConnection()
        {
            _store = new ConcurrentDictionary<TKey, TValue>();
        }

        public bool TryAdd(TKey key, TValue value)
        {
            return _store.TryAdd(key, value);
        }

        public bool TryRemove(TKey key, out TValue value)
        {
            return _store.TryRemove(key, out value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _store.TryGetValue(key, out value);
        }

        public void Clear()
        {
            _store.Clear();
        }
    }
}
