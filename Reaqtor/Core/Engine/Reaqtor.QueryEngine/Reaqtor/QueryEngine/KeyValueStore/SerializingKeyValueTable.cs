// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Key value table that supports serialization and deserialization of values of type <typeparamref name="TValue"/> by wrapping
    /// an underlying key value type using values of type <c>byte[]</c>.
    /// </summary>
    /// <typeparam name="TValue">The type of the values stored.</typeparam>
    internal class SerializingKeyValueTable<TValue> : IKeyValueTable<string, TValue>
    {
        private readonly IKeyValueTable<string, byte[]> _table;
        private readonly Action<TValue, Stream> _serialize;
        private readonly Func<Stream, TValue> _deserialize;

        /// <summary>
        /// Creates a new serializing key value table using the specified underlying table and the specified serialization and deserialization functions.
        /// </summary>
        /// <param name="table">The underlying table, storing values of type <c>byte[]</c>.</param>
        /// <param name="serialize">The function to serialize a value of type <typeparamref name="TValue"/>.</param>
        /// <param name="deserialize">The function to deserialize a value of type <typeparamref name="TValue"/>.</param>
        public SerializingKeyValueTable(IKeyValueTable<string, byte[]> table, Action<TValue, Stream> serialize, Func<Stream, TValue> deserialize)
        {
            _table = table;
            _serialize = serialize;
            _deserialize = deserialize;
        }

        /// <summary>
        /// Enters the table into an open transaction. This means any operations on the table will
        /// be done on the transaction.
        /// </summary>
        /// <param name="transaction">The transaction with which to scope the table.</param>
        /// <returns>The transacted key value table.</returns>
        public ITransactedKeyValueTable<string, TValue> Enter(IKeyValueStoreTransaction transaction)
        {
            return new Impl(_table.Enter(transaction), _serialize, _deserialize);
        }

        private sealed class Impl : ITransactedKeyValueTable<string, TValue>
        {
            private readonly Action<TValue, Stream> _serialize;
            private readonly Func<Stream, TValue> _deserialize;
            private readonly ITransactedKeyValueTable<string, byte[]> _table;

            public Impl(ITransactedKeyValueTable<string, byte[]> table, Action<TValue, Stream> serialize, Func<Stream, TValue> deserialize)
            {
                _table = table;
                _serialize = serialize;
                _deserialize = deserialize;
            }

            public TValue this[string key]
            {
                get
                {
                    var stream = _table[key];

                    using var s = new MemoryStream(stream);

                    return _deserialize(s);
                }
            }

            public void Add(string key, TValue value)
            {
                using var s = PooledMemoryStream.New();

                _serialize(value, s.MemoryStream);
                _table.Add(key, s.MemoryStream.ToArray());
            }

            public void Remove(string key) => _table.Remove(key);

            public bool Contains(string key) => _table.Contains(key);

            public void Update(string key, TValue value)
            {
                using var s = PooledMemoryStream.New();

                _serialize(value, s.MemoryStream);
                _table.Update(key, s.MemoryStream.ToArray());
            }

            public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
            {
                foreach (var item in _table)
                {
                    using var stream = new MemoryStream(item.Value);

                    yield return new KeyValuePair<string, TValue>(item.Key, _deserialize(stream));
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
