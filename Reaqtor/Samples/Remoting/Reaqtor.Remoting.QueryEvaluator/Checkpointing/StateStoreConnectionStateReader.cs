// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Reaqtor.QueryEngine;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.QueryEvaluator
{
    internal sealed class StateStoreConnectionStateReader : IStateReader
    {
        private readonly IReactiveStateStoreConnection _connection;
        private volatile bool _disposed;

        public StateStoreConnectionStateReader(IReactiveStateStoreConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<string> GetCategories()
        {
            CheckDisposed();

            return _connection.GetCategories();
        }

        public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
        {
            CheckDisposed();

            return _connection.TryGetItemKeys(category, out keys);
        }

        public bool TryGetItemReader(string category, string key, out Stream stream)
        {
            CheckDisposed();

            if (!_connection.TryGetItem(category, key, out var value))
            {
                stream = default;
                return false;
            }
            stream = new MemoryStream(CompressionUtils.Decompress(value), writable: false);
            return true;
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("InMemoryStateReader instance already disposed.");
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
