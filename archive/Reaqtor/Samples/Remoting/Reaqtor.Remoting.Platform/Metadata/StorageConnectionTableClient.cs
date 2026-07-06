// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Metadata
{
    public class StorageConnectionTableClient : ITableClient
    {
        private readonly IReactiveStorageConnection _connection;

        public StorageConnectionTableClient(IReactiveStorageConnection storageConnection)
        {
            _connection = storageConnection ?? throw new ArgumentNullException(nameof(storageConnection));
        }

        public ITable GetTableReference(string tableName)
        {
            return new StorageConnectionTable(tableName, _connection);
        }

        public ITableServiceContext GetTableServiceContext()
        {
            return new StorageConnectionTableServiceContext(_connection);
        }
    }
}
