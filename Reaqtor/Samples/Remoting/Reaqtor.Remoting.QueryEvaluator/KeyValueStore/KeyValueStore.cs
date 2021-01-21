// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.QueryEvaluator
{
    internal sealed class KeyValueStore : IKeyValueStore
    {
        private readonly ITransactionalKeyValueStoreConnection _connection;

        public KeyValueStore(ITransactionalKeyValueStoreConnection connection)
        {
            _connection = connection;
        }

        public IKeyValueStoreTransaction CreateTransaction()
        {
            return new Transaction(_connection, _connection.CreateTransaction());
        }

        public IKeyValueTable<string, byte[]> GetTable(string name)
        {
            return new KeyValueTable(name);
        }

        private sealed class KeyValueTable : IKeyValueTable<string, byte[]>
        {
            private readonly string _prefix;

            public KeyValueTable(string prefix)
            {
                _prefix = prefix;
            }

            public ITransactedKeyValueTable<string, byte[]> Enter(IKeyValueStoreTransaction transaction)
            {
                return new TransactedKeyValueTable(_prefix, transaction);
            }
        }

        private sealed class TransactedKeyValueTable : ITransactedKeyValueTable<string, byte[]>
        {
            private readonly string _prefix;
            private readonly IKeyValueStoreTransaction _transaction;

            public TransactedKeyValueTable(string prefix, IKeyValueStoreTransaction transaction)
            {
                _prefix = prefix;
                _transaction = transaction;
            }

            public byte[] this[string key] => _transaction[_prefix, key];

            public void Add(string key, byte[] value)
            {
                _transaction.Add(_prefix, key, value);
            }

            public bool Contains(string key)
            {
                return _transaction.Contains(_prefix, key);
            }

            public void Update(string key, byte[] value)
            {
                _transaction.Update(_prefix, key, value);
            }

            public void Remove(string key)
            {
                _transaction.Remove(_prefix, key);
            }

            public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator()
            {
                return _transaction.GetEnumerator(_prefix);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class Transaction : IKeyValueStoreTransaction
        {
            private readonly ITransactionalKeyValueStoreConnection _connection;

            public Transaction(ITransactionalKeyValueStoreConnection connection, long id)
            {
                _connection = connection;
                Id = id;
            }

            public long Id { get; }

            public byte[] this[string tableName, string key] => _connection[Id, tableName, key];

            public void Add(string tableName, string key, byte[] value)
            {
                _connection.Add(Id, tableName, key, value);
            }

            public bool Contains(string tableName, string key)
            {
                return _connection.Contains(Id, tableName, key);
            }

            public void Update(string tableName, string key, byte[] value)
            {
                _connection.Update(Id, tableName, key, value);
            }

            public void Remove(string tableName, string key)
            {
                _connection.Remove(Id, tableName, key);
            }

            public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator(string tableName)
            {
                return _connection.GetEnumerator(Id, tableName).GetEnumerator();
            }

            public Task CommitAsync(CancellationToken token)
            {
                _connection.Commit(Id);
                return Task.FromResult(true);
            }

            public void Rollback()
            {
                _connection.Rollback(Id);
            }

            public void Dispose()
            {
                _connection.Dispose(Id);
            }
        }
    }
}
