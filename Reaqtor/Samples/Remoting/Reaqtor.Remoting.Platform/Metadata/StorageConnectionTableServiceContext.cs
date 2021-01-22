// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Metadata
{
    public class StorageConnectionTableServiceContext : ITableServiceContext
    {
        private readonly IReactiveStorageConnection _connection;

        public StorageConnectionTableServiceContext(IReactiveStorageConnection connection)
        {
            _connection = connection;
        }

        public IQueryable<T> CreateQuery<T>(string entitySetName)
            where T : new()
        {
            return new StorageConnectionQueryable<T>(entitySetName, _connection);
        }
    }
}
