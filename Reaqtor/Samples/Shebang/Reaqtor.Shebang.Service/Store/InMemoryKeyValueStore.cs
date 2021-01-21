// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Reaqtor.QueryEngine;

namespace Reaqtor.Shebang.Service
{
    //
    // Provides an in-memory mock implementation of a key/value store used by the engine for persistence.
    //
    // There are two parts to persistence in the engine:
    //
    // - Transaction log records due to Create/Delete operations. This is a write-ahead log.
    // - Checkpoint state, including query operator state. See CheckpointAsync on the engine.
    //
    // For historical reasons, these use a different interface:
    //
    // - The transaction log is passed as an IKeyValueStore to the constructor of the engine.
    // - The checkpoint store is passed as an IState[Reader|Writer] to [Recover|Checkpoint]Async.
    //
    // Here, we implement both using IKeyValueStore and provide GetReader and GetWriter methods to adapt
    // to the required interfaces for checkpointing. This is the recommended way going forward.
    //

    public sealed class InMemoryKeyValueStore : IQueryEngineStateStore
    {
        private readonly Dictionary<string, Dictionary<string, byte[]>> _data;

        public InMemoryKeyValueStore() => _data = new();

        private InMemoryKeyValueStore(Dictionary<string, Dictionary<string, byte[]>> data) => _data = data;

        public IKeyValueStoreTransaction CreateTransaction() => new Transaction(this);

        public IKeyValueTable<string, byte[]> GetTable(string name) => new Table(name);

        public IStateReader GetReader() => new Reader(this);

        public IStateWriter GetWriter() => new Writer(this);

        public void Save(string fileName)
        {
            var doc =
                new XDocument(
                    new XElement("Tables",
                        _data.Select(table =>
                            new XElement("Table",
                                new XAttribute("Name", table.Key),
                                table.Value.Select(row =>
                                    new XElement("Row",
                                        new XAttribute("Key", row.Key),
                                        new XCData(Convert.ToBase64String(row.Value))
                                    )
                                )
                            )
                        )
                    )
                );

            doc.Save(fileName);
        }

        public static InMemoryKeyValueStore Load(string fileName)
        {
            var doc = XDocument.Load(fileName);

            var data =
                doc.Element("Tables").Elements("Table").Select(table =>
                    new KeyValuePair<string, Dictionary<string, byte[]>>(
                        table.Attribute("Name").Value,
                        table.Elements("Row").Select(row =>
                            new KeyValuePair<string, byte[]>(
                                row.Attribute("Key").Value,
                                Convert.FromBase64String(row.Value)
                            )
                        ).ToDictionary(kv => kv.Key, kv => kv.Value)
                    )
                ).ToDictionary(kv => kv.Key, kv => kv.Value);

            return new InMemoryKeyValueStore(data);
        }

        public string DebugView
        {
            get
            {
                var sb = new StringBuilder();

                lock (_data)
                {
                    var totalSize = 0;

                    foreach (var table in _data)
                    {
                        var tableSize = table.Key.Length * 2;

                        sb.AppendLine($"Table '{table.Key}':");
                        sb.AppendLine();

                        foreach (var row in table.Value)
                        {
                            var rowSize = row.Value.Length + row.Key.Length * 2;

                            sb.AppendLine($"  Key '{row.Key}':");
                            sb.AppendLine($"    Bytes = {BitConverter.ToString(row.Value).Replace('-', ' ')}");
                            sb.AppendLine($"    ASCII = {new string(row.Value.Select(b => (char)b).ToArray())}");
                            sb.AppendLine($"    Size  = {rowSize}");
                            sb.AppendLine();

                            tableSize += rowSize;
                        }

                        sb.AppendLine();
                        sb.AppendLine($"  Size  = {tableSize}");
                        sb.AppendLine();

                        totalSize += tableSize;
                    }

                    sb.AppendLine($"Total size = {totalSize}");
                }

                return sb.ToString();
            }
        }

        public long TotalSize
        {
            get
            {
                lock (_data)
                {
                    var totalSize = 0;

                    foreach (var table in _data)
                    {
                        var tableSize = table.Key.Length * 2;

                        foreach (var row in table.Value)
                        {
                            var rowSize = row.Value.Length + row.Key.Length * 2;
                            tableSize += rowSize;
                        }

                        totalSize += tableSize;
                    }

                    return totalSize;
                }
            }
        }

        private sealed class Transaction : IKeyValueStoreTransaction
        {
            private readonly InMemoryKeyValueStore _parent;
            private readonly Dictionary<string, Dictionary<string, byte[]>> _edits = new();

            public Transaction(InMemoryKeyValueStore parent) => _parent = parent;

            public byte[] this[string tableName, string key]
            {
                get
                {
                    if (tableName == null)
                        throw new ArgumentNullException(nameof(tableName));
                    if (key == null)
                        throw new ArgumentNullException(nameof(key));

                    lock (_edits)
                    {
                        if (_edits.TryGetValue(tableName, out var table))
                        {
                            if (table.TryGetValue(key, out var value))
                            {
                                if (value == null)
                                {
                                    throw new KeyNotFoundException(tableName, key);
                                }

                                return value;
                            }
                        }
                    }

                    lock (_parent._data)
                    {
                        if (!_parent._data.TryGetValue(tableName, out var table))
                        {
                            throw new TableNotFoundException(tableName);
                        }

                        if (!table.TryGetValue(key, out var value))
                        {
                            throw new KeyNotFoundException(tableName, key);
                        }

                        return value;
                    }
                }
            }

            public void Add(string tableName, string key, byte[] value)
            {
                if (tableName == null)
                    throw new ArgumentNullException(nameof(tableName));
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                lock (_edits)
                {
                    if (_edits.TryGetValue(tableName, out var table))
                    {
                        if (table.TryGetValue(key, out var existingValue))
                        {
                            if (existingValue != null)
                            {
                                throw new InvalidOperationException("Entry already exits.");
                            }
                        }

                        table[key] = value;
                    }
                    else
                    {
                        lock (_parent._data)
                        {
                            if (_parent._data.TryGetValue(tableName, out var existingTable))
                            {
                                if (existingTable.ContainsKey(key))
                                {
                                    throw new InvalidOperationException("Entry already exits.");
                                }
                            }
                        }

                        _edits[tableName] = table = new Dictionary<string, byte[]>();

                        table[key] = value;
                    }
                }
            }

            public Task CommitAsync(CancellationToken token)
            {
                lock (_edits)
                {
                    lock (_parent._data)
                    {
                        foreach (var table in _edits)
                        {
                            if (!_parent._data.TryGetValue(table.Key, out var existingTable))
                            {
                                _parent._data[table.Key] = existingTable = new Dictionary<string, byte[]>();
                            }

                            foreach (var entry in table.Value)
                            {
                                if (entry.Value == null)
                                {
                                    existingTable.Remove(entry.Key);
                                }
                                else
                                {
                                    existingTable[entry.Key] = entry.Value;
                                }
                            }
                        }
                    }
                }

                return Task.CompletedTask;
            }

            public bool Contains(string tableName, string key)
            {
                if (tableName == null)
                    throw new ArgumentNullException(nameof(tableName));
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                lock (_edits)
                {
                    if (_edits.TryGetValue(tableName, out var table))
                    {
                        if (table.TryGetValue(key, out var value))
                        {
                            return value != null;
                        }
                    }
                }

                lock (_parent._data)
                {
                    if (!_parent._data.TryGetValue(tableName, out var table))
                    {
                        return false;
                    }

                    return table.ContainsKey(key);
                }
            }

            public void Dispose() { }

            public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator(string tableName)
            {
                if (tableName == null)
                    throw new ArgumentNullException(nameof(tableName));

                return Core();

                IEnumerator<KeyValuePair<string, byte[]>> Core()
                {
                    var res = new Dictionary<string, byte[]>();

                    lock (_edits)
                    {
                        if (_edits.TryGetValue(tableName, out var edits))
                        {
                            foreach (var entry in edits)
                            {
                                res.Add(entry.Key, entry.Value);
                            }
                        }
                    }

                    lock (_parent._data)
                    {
                        if (_parent._data.TryGetValue(tableName, out var table))
                        {
                            foreach (var entry in table)
                            {
                                if (!res.ContainsKey(entry.Key))
                                {
                                    res.Add(entry.Key, entry.Value);
                                }
                            }
                        }

                        // NB: Non-existing table is assumed empty.
                    }

                    foreach (var entry in res)
                    {
                        if (entry.Value != null)
                        {
                            yield return entry;
                        }
                    }
                }
            }

            public void Remove(string tableName, string key)
            {
                if (tableName == null)
                    throw new ArgumentNullException(nameof(tableName));
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                UpdateCore(tableName, key, null);
            }

            public void Rollback() { }

            public void Update(string tableName, string key, byte[] value)
            {
                if (tableName == null)
                    throw new ArgumentNullException(nameof(tableName));
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                UpdateCore(tableName, key, value);
            }

            private void UpdateCore(string tableName, string key, byte[] value)
            {
                lock (_edits)
                {
                    if (!_edits.TryGetValue(tableName, out var table))
                    {
                        _edits[tableName] = table = new Dictionary<string, byte[]>();
                    }

                    if (table.TryGetValue(key, out var existingValue))
                    {
                        if (existingValue == null)
                        {
                            throw new KeyNotFoundException(tableName, key);
                        }
                    }

                    table[key] = value;
                }
            }
        }

        private sealed class Table : IKeyValueTable<string, byte[]>
        {
            private readonly string _name;

            public Table(string name) => _name = name;

            public ITransactedKeyValueTable<string, byte[]> Enter(IKeyValueStoreTransaction transaction) => new Impl(transaction, _name);

            private sealed class Impl : ITransactedKeyValueTable<string, byte[]>
            {
                private readonly string _name;
                private readonly IKeyValueStoreTransaction _transaction;

                public Impl(IKeyValueStoreTransaction transaction, string name) => (_transaction, _name) = (transaction, name);

                public byte[] this[string key] => _transaction[_name, key];

                public void Add(string key, byte[] value) => _transaction.Add(_name, key, value);

                public bool Contains(string key) => _transaction.Contains(_name, key);

                public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator() => _transaction.GetEnumerator(_name);

                public void Remove(string key) => _transaction.Remove(_name, key);

                public void Update(string key, byte[] value) => _transaction.Update(_name, key, value);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }
        }

        private sealed class Reader : IStateReader
        {
            private readonly InMemoryKeyValueStore _store;

            public Reader(InMemoryKeyValueStore store) => _store = store;

            public void Dispose() { }

            public IEnumerable<string> GetCategories() => throw new NotImplementedException("Unused by engine.");

            public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
            {
                using var tx = _store.CreateTransaction();

                try
                {
                    var table = _store.GetTable(category).Enter(tx);

                    var res = new List<string>();

                    using (var e = table.GetEnumerator())
                    {
                        while (e.MoveNext())
                        {
                            res.Add(e.Current.Key);
                        }
                    }

                    keys = res;
                    return true;
                }
                catch (TableNotFoundException)
                {
                    // NB: Normally, both IKeyValueStore and IStateReader/IStateWriter would talk to a common infrastructure rather than being layered like this.
                    //     As such, it'd be easier to check for table existence at the layer below rather than translating an exception.

                    keys = null;
                    return false;
                }
            }

            public bool TryGetItemReader(string category, string key, out Stream stream)
            {
                using (var tx = _store.CreateTransaction())
                {
                    var table = _store.GetTable(category).Enter(tx);

                    if (table.Contains(key))
                    {
                        stream = new MemoryStream(table[key]);
                        return true;
                    }
                }

                stream = null;
                return false;
            }
        }

        private sealed class Writer : IStateWriter
        {
            private readonly InMemoryKeyValueStore _store;
            private readonly Dictionary<(string, string), MemoryStream> _edits = new();

            public Writer(InMemoryKeyValueStore store) => _store = store;

            public CheckpointKind CheckpointKind => CheckpointKind.Differential;

            public async Task CommitAsync(CancellationToken token, IProgress<int> progress)
            {
                using var tx = _store.CreateTransaction();

                var tables = new Dictionary<string, ITransactedKeyValueTable<string, byte[]>>();

                foreach (var edit in _edits)
                {
                    var (category, key) = edit.Key;

                    if (!tables.TryGetValue(category, out var table))
                    {
                        tables[category] = table = _store.GetTable(category).Enter(tx);
                    }

                    if (edit.Value == null)
                    {
                        table.Remove(key);
                    }
                    else
                    {
                        var value = edit.Value.ToArray();

                        if (table.Contains(key))
                        {
                            table.Update(key, value);
                        }
                        else
                        {
                            table.Add(key, value);
                        }
                    }
                }

                await tx.CommitAsync(token).ConfigureAwait(false);
            }

            public void DeleteItem(string category, string key)
            {
                _edits[(category, key)] = null;
            }

            public void Dispose() { }

            public Stream GetItemWriter(string category, string key)
            {
                var ms = new MemoryStream();
                _edits[(category, key)] = ms;
                return ms;
            }

            public void Rollback() { }
        }
    }

#pragma warning disable CA1032 // Implement standard exception constructors. (Only constructed internally.)
    [Serializable]
    public sealed class TableNotFoundException : Exception
    {
        internal TableNotFoundException(string tableName)
        {
            TableName = tableName;
        }

        private TableNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            TableName = serializationInfo.GetString(nameof(TableName));
        }

        public string TableName { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(TableName), TableName);

            base.GetObjectData(info, context);
        }
    }

    [Serializable]
    public sealed class KeyNotFoundException : Exception
    {
        internal KeyNotFoundException(string tableName, string key)
        {
            TableName = tableName;
            Key = key;
        }

        private KeyNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            TableName = serializationInfo.GetString(nameof(TableName));
            Key = serializationInfo.GetString(nameof(Key));
        }

        public string TableName { get; }
        public string Key { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(TableName), TableName);
            info.AddValue(nameof(Key), Key);

            base.GetObjectData(info, context);
        }
    }
#pragma warning restore CA1032
}
