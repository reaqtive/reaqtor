// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.KeyValueStore
{
    public class KeyValueStoreConnection : ReactiveConnectionBase, ITransactionalKeyValueStoreConnection
    {
        private long _transactionId = 0;
        private InMemoryKeyValueStore _kvs;

        private readonly ConcurrentDictionary<long, IKeyValueStoreTransaction> _transactionMap;

        public KeyValueStoreConnection()
        {
            _kvs = new InMemoryKeyValueStore();
            _transactionMap = new ConcurrentDictionary<long, IKeyValueStoreTransaction>();
        }

        public byte[] this[long transactionId, string tableName, string key] => _transactionMap[transactionId][tableName, key];

        public void Add(long transactionId, string tableName, string key, byte[] value) => _transactionMap[transactionId].Add(tableName, key, value);

        public bool Contains(long transactionId, string tableName, string key) => _transactionMap[transactionId].Contains(tableName, key);

        public void Update(long transactionId, string tableName, string key, byte[] value) => _transactionMap[transactionId].Update(tableName, key, value);

        public void Remove(long transactionId, string tableName, string key) => _transactionMap[transactionId].Remove(tableName, key);

        public List<KeyValuePair<string, byte[]>> GetEnumerator(long transactionId, string tableName)
        {
            var enumerator = _transactionMap[transactionId].GetEnumerator(tableName);

            var list = new List<KeyValuePair<string, byte[]>>();
            while (enumerator.MoveNext())
                list.Add(enumerator.Current);

            return list;
        }

        public long CreateTransaction()
        {
            var id = Interlocked.Increment(ref _transactionId);
            var tx = _kvs.CreateTransaction();
            _transactionMap[id] = tx;
            return id;
        }

        public void Commit(long transactionId) => _transactionMap[transactionId].CommitAsync().Wait(); // ok to Wait since internal implementation is sync

        public void Rollback(long transactionId) => _transactionMap[transactionId].Rollback();

        public void Dispose(long transactionId)
        {
            var tx = _transactionMap[transactionId];
            tx.Dispose();

            _transactionMap.TryRemove(transactionId, out _);
        }

        public byte[] SerializeStore()
        {
            var stream = new MemoryStream();
            _kvs.Save(stream);
            return stream.ToArray();
        }

        public void DeserializeStore(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            _kvs = InMemoryKeyValueStore.Load(stream);
        }

        public void Clear() => _kvs = new InMemoryKeyValueStore();
    }
}
