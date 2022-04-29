// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// In-memory implementation of <see cref="IKeyValueStore"/>.
    /// </summary>
    public class InMemoryKeyValueStore : IKeyValueStore
    {
        private ImmutableSortedDictionary<string, Sequenced<byte[]>> _dictionary = ImmutableSortedDictionary.Create<string, Sequenced<byte[]>>();
        private readonly object _lock = new();

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1003 // Non-default EventArgs parameter.
        /// <summary>
        /// Event invoked when an operation is started.
        /// </summary>
        public event EventHandler<ReifiedOperation<string, byte[]>> StartingOperation;

        /// <summary>
        /// Event invoked when an operation is finished.
        /// </summary>
        public event EventHandler<ReifiedOperation<string, byte[]>> FinishedOperation;
#pragma warning restore CA1003
#pragma warning restore IDE0079

        /// <summary>
        /// Creates a transaction over the whole store that can be used to scope table in the store.
        /// </summary>
        /// <returns>A transaction for the store.</returns>
        public IKeyValueStoreTransaction CreateTransaction() => new Transaction(this);

        /// <summary>
        /// Gets a table by name.
        /// </summary>
        /// <param name="name">The name of the table to get.</param>
        /// <returns>The key value table.</returns>
        public IKeyValueTable<string, byte[]> GetTable(string name) => new KeyValueTableImpl(name);

        /// <summary>
        /// Loads the store from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <returns>A new key/value store loaded from the stream.</returns>
        public static InMemoryKeyValueStore Load(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var doc = XDocument.Load(stream);
            return Load(doc);

            static InMemoryKeyValueStore Load(XDocument doc)
            {
                var store = doc.Element("Store");

                var res = new InMemoryKeyValueStore();
                var internalDict = res._dictionary;

                var entries = store.Elements("Entry");

                foreach (var entry in entries)
                {
                    var keyName = entry.Attribute("Name").Value;
                    var base64 = entry.Nodes().OfType<XCData>().Single().Value;

                    var value = Convert.FromBase64String(base64);

                    internalDict = internalDict.Add(keyName, new Sequenced<byte[]>(value));
                }

                res._dictionary = internalDict;

                return res;
            }
        }

        /// <summary>
        /// Saves the store to the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var doc = Save();
            doc.Save(stream);

            XDocument Save()
            {
                var entries = new List<XElement>();

                foreach (var kv in _dictionary)
                {
                    var valNameAttr = new XAttribute("Name", kv.Key);
                    var base64 = Convert.ToBase64String(kv.Value.Object);

                    var entry = new XElement("Entry", valNameAttr, new XCData(base64));
                    entries.Add(entry);
                }

                var doc = new XDocument(
                    new XElement("Store",
                        entries.ToArray()
                    )
                );

                return doc;
            }
        }

        private sealed class Transaction : IKeyValueStoreTransaction
        {
            private static readonly Task s_completedTask = Task.FromResult(false);

            private ImmutableSortedDictionary<string, Sequenced<byte[]>> _snapshot;
            private readonly List<Tuple<ReifiedOperation<string, byte[]>, OperationResult<string, byte[]>>> _operations;
            private readonly InMemoryKeyValueStore _parent;

            public Transaction(InMemoryKeyValueStore parent)
            {
                _parent = parent;
                _snapshot = _parent._dictionary;
                _operations = new List<Tuple<ReifiedOperation<string, byte[]>, OperationResult<string, byte[]>>>();
            }

            public byte[] this[string tableName, string key]
            {
                get
                {
                    var k = GetInternalId(tableName, key);
                    var op = ReifiedOperation.Get<string, byte[]>(k);

                    InvokeStartingOperationEvent(op);

                    var res = (GetOperationResult<string, byte[]>)op.Apply(ref _snapshot);
                    _operations.Add(new Tuple<ReifiedOperation<string, byte[]>, OperationResult<string, byte[]>>(op, res));

                    InvokeFinishedOperationEvent(op);

                    if (res.Exception != null)
                        throw res.Exception;

                    return (byte[])res.Result;
                }
            }

            public Task CommitAsync(CancellationToken token)
            {
                lock (_parent._lock)
                {
                    var current = _parent._dictionary;

                    foreach (var op in _operations)
                    {
                        var res = op.Item1.Apply(ref current);
                        if (!res.Equals(op.Item2))
                            throw new WriteConflictException();
                    }

                    _parent._dictionary = current;
                }

                return s_completedTask;
            }

            public void Rollback()
            {
            }

            private static string GetInternalId(string tableName, string key) => tableName + key;

            public void Add(string tableName, string key, byte[] value)
            {
                var k = GetInternalId(tableName, key);
                var op = ReifiedOperation.Add(k, value);

                InvokeStartingOperationEvent(op);

                var res = (AddOperationResult<string, byte[]>)op.Apply(ref _snapshot);
                _operations.Add(new Tuple<ReifiedOperation<string, byte[]>, OperationResult<string, byte[]>>(op, res));

                InvokeFinishedOperationEvent(op);

                if (res.Exception != null)
                    throw res.Exception;
            }

            public bool Contains(string tableName, string key)
            {
                var k = GetInternalId(tableName, key);
                var op = ReifiedOperation.Contains<string, byte[]>(k);

                InvokeStartingOperationEvent(op);

                var res = (ContainsOperationResult<string, byte[]>)op.Apply(ref _snapshot);
                _operations.Add(new Tuple<ReifiedOperation<string, byte[]>, OperationResult<string, byte[]>>(op, res));

                InvokeFinishedOperationEvent(op);

                if (res.Exception != null)
                    throw res.Exception;

                return (bool)res.Result;
            }

            public void Update(string tableName, string key, byte[] value)
            {
                var k = GetInternalId(tableName, key);
                var op = ReifiedOperation.Update(k, value);

                InvokeStartingOperationEvent(op);

                var res = (UpdateOperationResult<string, byte[]>)op.Apply(ref _snapshot);
                _operations.Add(new Tuple<ReifiedOperation<string, byte[]>, OperationResult<string, byte[]>>(op, res));

                InvokeFinishedOperationEvent(op);

                if (res.Exception != null)
                    throw res.Exception;
            }

            public void Remove(string tableName, string key)
            {
                var k = GetInternalId(tableName, key);
                var op = ReifiedOperation.Remove<string, byte[]>(k);

                InvokeStartingOperationEvent(op);

                var res = (RemoveOperationResult<string, byte[]>)op.Apply(ref _snapshot);
                _operations.Add(new Tuple<ReifiedOperation<string, byte[]>, OperationResult<string, byte[]>>(op, res));

                InvokeFinishedOperationEvent(op);

                if (res.Exception != null)
                    throw res.Exception;
            }

            public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator(string tableName)
            {
                var op = ReifiedOperation.GetEnumerator<string, byte[]>(s => s.StartsWith(tableName, StringComparison.Ordinal));

                InvokeStartingOperationEvent(op);

                var res = (EnumerateOperationResult<string, byte[]>)op.Apply(ref _snapshot);
                _operations.Add(new Tuple<ReifiedOperation<string, byte[]>, OperationResult<string, byte[]>>(op, res));

                InvokeFinishedOperationEvent(op);

                if (res.Exception != null)
                    throw res.Exception;

                var len = tableName.Length;

                return
                    new ProjectingEnumerator<KeyValuePair<string, byte[]>, KeyValuePair<string, byte[]>>(
                        (IEnumerator<KeyValuePair<string, byte[]>>)res.Result,
                        kvp => new KeyValuePair<string, byte[]>(
#if NET6_0 || NETSTANDARD2_1
                            kvp.Key[len..],
#else
                            kvp.Key.Substring(len),
#endif
                            kvp.Value
                        )
                    );
            }

            public void Dispose()
            {
            }

            private void InvokeStartingOperationEvent(ReifiedOperation<string, byte[]> operation)
            {
                _parent.StartingOperation?.Invoke(this, operation);
            }

            private void InvokeFinishedOperationEvent(ReifiedOperation<string, byte[]> operation)
            {
                _parent.FinishedOperation?.Invoke(this, operation);
            }

            private sealed class ProjectingEnumerator<T, R> : IEnumerator<R>
            {
                private readonly Func<T, R> _selector;
                private readonly IEnumerator<T> _source;

                public ProjectingEnumerator(IEnumerator<T> source, Func<T, R> selector)
                {
                    _source = source;
                    _selector = selector;
                }

                public R Current => _selector(_source.Current);

                object IEnumerator.Current => Current;

                public void Dispose() => _source.Dispose();

                public bool MoveNext() => _source.MoveNext();

                public void Reset() => _source.Reset();
            }
        }

        private sealed class KeyValueTableImpl : IKeyValueTable<string, byte[]>
        {
            internal readonly string _prefix;

            public KeyValueTableImpl(string prefix)
            {
                _prefix = prefix;
            }

            public ITransactedKeyValueTable<string, byte[]> Enter(IKeyValueStoreTransaction transaction)
            {
                return new TransactedKeyValueTableImpl(transaction, this);
            }
        }

        private sealed class TransactedKeyValueTableImpl : ITransactedKeyValueTable<string, byte[]>
        {
            private readonly Transaction _transaction;
            private readonly string _tableName;

            public TransactedKeyValueTableImpl(ITransaction transaction, KeyValueTableImpl keyValueTable)
            {
                if (transaction is not Transaction t)
                    throw new NotSupportedException();

                _transaction = t;
                _tableName = keyValueTable._prefix;
            }

            public byte[] this[string key] => _transaction[_tableName, key];

            public void Add(string key, byte[] value) => _transaction.Add(_tableName, key, value);

            public bool Contains(string key) => _transaction.Contains(_tableName, key);

            public void Update(string key, byte[] value) => _transaction.Update(_tableName, key, value);

            public void Remove(string key) => _transaction.Remove(_tableName, key);

            public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator() => _transaction.GetEnumerator(_tableName);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
