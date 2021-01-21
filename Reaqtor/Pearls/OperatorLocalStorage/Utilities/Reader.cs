// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;
using System.IO;

using Reaqtor.QueryEngine;

namespace Utilities
{
    /// <summary>
    /// Implementation of <see cref="IStateReader"/> for the in-memory key/value store implementation in <see cref="Store"/>.
    /// </summary>
    public sealed class Reader : IStateReader
    {
        private readonly Store _store;

        public Reader(Store store)
        {
            _store = store;
        }

        public void Dispose() { }

        public IEnumerable<string> GetCategories() => _store.Data.Keys;

        public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
        {
            if (_store.Data.TryGetValue(category, out var table))
            {
                keys = table.Keys;
                return true;
            }

            keys = null;
            return false;
        }

        public bool TryGetItemReader(string category, string key, out Stream stream)
        {
            if (_store.Data.TryGetValue(category, out var table) && table.TryGetValue(key, out var data))
            {
                stream = new MemoryStream(data);
                return true;
            }

            stream = null;
            return false;
        }
    }
}
