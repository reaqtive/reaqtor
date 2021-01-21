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
using System.IO;
using System.Linq;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.StateStore
{
    public class StateStoreConnection : ReactiveConnectionBase, IReactiveStateStoreConnection
    {
        private const string StateStoreId = "CheckpointConnectionStateStore";

        private InMemoryStateStore _stateStore;

        public StateStoreConnection()
        {
            _stateStore = new InMemoryStateStore(StateStoreId);
        }

        public IEnumerable<string> GetCategories()
        {
            return _stateStore.GetCategories().ToList();
        }

        public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
        {
            if (_stateStore.TryGetItemKeys(category, out keys))
            {
                keys = keys.ToList();
                return true;
            }
            return false;
        }

        public bool TryGetItem(string category, string key, out byte[] value)
        {
            return _stateStore.TryGetItem(category, key, out value);
        }

        public void AddOrUpdateItem(string category, string key, byte[] value)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _stateStore.AddOrUpdateItem(category, key, value);
        }

        public void RemoveItem(string category, string key)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _stateStore.RemoveItem(category, key);
        }

        public void Clear()
        {
            _stateStore.Clear();
        }

        public byte[] SerializeStateStore()
        {
            var stream = new MemoryStream();
            _stateStore.Save(stream);
            return stream.ToArray();
        }

        public void DeserializeStateStore(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            _stateStore = InMemoryStateStore.Load(stream);
        }
    }
}
