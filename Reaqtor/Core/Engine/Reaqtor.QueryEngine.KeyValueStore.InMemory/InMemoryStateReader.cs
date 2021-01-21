// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// Thread-safe state reader reading item from an in-memory state store.
    /// </summary>
    public sealed class InMemoryStateReader : IStateReader
    {
        private readonly InMemoryStateStore _store;

        private volatile bool _disposed;

        /// <summary>
        /// Create a new instance of <see cref="InMemoryStateReader"/>.        
        /// </summary>
        /// <param name="store">The store containing the state.</param>
        public InMemoryStateReader(InMemoryStateStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        /// <summary>
        /// Get all categories present in the store.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCategories()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("InMemoryStateReader instance already disposed.");
            }

            return _store.GetCategories();
        }

        /// <summary>
        /// Try retrieving the keys corresponding to the items of the provided <paramref name="category"/>.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="keys">The keys of the items in the <paramref name="category"/>.</param>
        /// <returns><b>true</b> if the <paramref name="category"/> exists, false otherwise.</returns>
        public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("InMemoryStateReader instance already disposed.");
            }

            var res = _store.TryGetItemKeys(category, out var allKeys);
            if (res)
            {
                keys = allKeys.Except(_store.GetRemovedItems(category)).ToArray();
                return true;
            }
            else
            {
                keys = null;
                return false;
            }
        }

        /// <summary>
        /// Get a stream containing the content of the item identified by the provided <paramref name="category"/> and <paramref name="key"/>.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="key">The item key.</param>
        /// <param name="stream">The item content.</param>
        /// <returns><b>true</b> if such an item is present, <b>false</b> otherwise.</returns>
        public bool TryGetItemReader(string category, string key, out Stream stream)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("InMemoryStateReader instance already disposed.");
            }

            if (!_store.TryGetItem(category, key, out var value))
            {
                stream = default;
                return false;
            }

            stream = new MemoryStream(value, writable: false);
            return true;
        }

        /// <summary>
        /// Dispose managed resource.
        /// <remarks> The internal store is not cleared upon disposing.</remarks>
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }
}
