// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Metadata
{
    public class StorageConnection : KeyValueStoreConnection<string, ConcurrentDictionary<string, StorageEntity>>, IReactiveStorageConnection
    {
        public void AddEntity(string collection, string key, StorageEntity entity)
        {
            if (!TryGetValue(collection, out var table))
            {
                table = new ConcurrentDictionary<string, StorageEntity>();
                if (!TryAdd(collection, table))
                {
                    throw new ReactiveProcessingStorageException(
                        ReactiveProcessingStorageException.ErrorCodes.OperationFailed,
                        string.Format(CultureInfo.InvariantCulture, "Could not create table '{0}'.", collection),
                        inner: null);
                }
            }

            if (table.ContainsKey(key))
            {
                throw new ReactiveProcessingStorageException(ReactiveProcessingStorageException.ErrorCodes.EntityAlreadyExists, message: null, inner: null);
            }

            table.TryAdd(key, entity);
        }

        public void DeleteEntity(string collection, string key)
        {
            if (!TryGetValue(collection, out var table) || !table.ContainsKey(key))
            {
                throw new ReactiveProcessingStorageException(ReactiveProcessingStorageException.ErrorCodes.EntityNotFound, message: null, inner: null);
            }

            table.TryRemove(key, out _);
        }

        public bool TryGetEntity(string collection, string key, out StorageEntity entity)
        {
            if (!TryGetValue(collection, out var table))
            {
                entity = default;
                return false;
            }

            return table.TryGetValue(key, out entity);
        }

        public List<StorageEntity> GetEntities(string collection)
        {
            if (!TryGetValue(collection, out var table))
            {
                throw new ReactiveProcessingStorageException(ReactiveProcessingStorageException.ErrorCodes.OperationFailed, message: null, inner: null);
            }

            return table.Values.ToList();
        }
    }
}
