// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// Thread-safe in memory state store which holds key-value pair item sorted by categories
    /// and track items removal.
    /// </summary>
    [Serializable]
    public sealed class InMemoryStateStore
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>> _internalStore;
        private readonly ConcurrentDictionary<string, ConcurrentBag<string>> _removedItems;

        /// <summary>
        /// Create a new <see cref="InMemoryStateStore"/> instance.
        /// </summary>
        /// <param name="id">Store identifier.</param>
        public InMemoryStateStore(string id)
        {
            Id = id;
            _internalStore = new ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>>();
            _removedItems = new ConcurrentDictionary<string, ConcurrentBag<string>>();
        }

        /// <summary>
        /// Loads the state store from the given stream.
        /// </summary>
        /// <param name="stream">Stream to load the store from.</param>
        /// <returns>Store instance loaded from the stream.</returns>
        public static InMemoryStateStore Load(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var doc = XDocument.Load(stream);
            return Load(doc);
        }

        /// <summary>
        /// Loads the state store from the given file.
        /// </summary>
        /// <param name="file">File to load the store from.</param>
        /// <returns>Store instance loaded from the file.</returns>
        public static InMemoryStateStore Load(string file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var doc = XDocument.Load(file);
            return Load(doc);
        }

        private static InMemoryStateStore Load(XDocument doc)
        {
            var store = doc.Element("Store");
            var id = store.Attribute("Id").Value;

            var res = new InMemoryStateStore(id);

            var categories = store.Elements("Category");
            foreach (var category in categories)
            {
                var categoryName = category.Attribute("Name").Value;

                var entries = category.Elements("Entry");
                foreach (var entry in entries)
                {
                    var keyName = entry.Attribute("Name").Value;
                    var base64 = entry.Nodes().OfType<XCData>().Single().Value;

                    var value = Convert.FromBase64String(base64);

                    res.AddOrUpdateItem(categoryName, keyName, value);
                }
            }

            return res;
        }

        /// <summary>
        /// Saves the state store to the given file.
        /// </summary>
        /// <param name="file">File to save the store to.</param>
        public void Save(string file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var doc = Save();
            doc.Save(file);
        }

        /// <summary>
        /// Saves the state store to the given stream.
        /// </summary>
        /// <param name="file">Stream to save the store to.</param>
        public void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var doc = Save();
            doc.Save(stream);
        }

        private XDocument Save()
        {
            var idAttr = new XAttribute("Id", Id);
            var categories = new List<XElement>();

            foreach (var kv in _internalStore)
            {
                var catNameAttr = new XAttribute("Name", kv.Key);
                var entries = new List<XElement>();

                foreach (var e in kv.Value)
                {
                    var valNameAttr = new XAttribute("Name", e.Key);
                    var base64 = Convert.ToBase64String(e.Value);

                    var entry = new XElement("Entry", valNameAttr, new XCData(base64));
                    entries.Add(entry);
                }

                var category = new XElement("Category", new object[] { catNameAttr }.Concat(entries).ToArray());
                categories.Add(category);
            }

            var doc = new XDocument(
                new XElement("Store",
                    new object[] { idAttr }.Concat(categories).ToArray()
                )
            );
            return doc;
        }

        /// <summary>
        /// Store id.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Get the categories present in the store.
        /// </summary>
        /// <returns>The categories.</returns>
        public IEnumerable<string> GetCategories()
        {
            return _internalStore.Keys.ToArray();
        }

        /// <summary>
        /// Get the items removed from the provided <paramref name="category"/>.
        /// <remarks>If an item was removed multiple times, it will appear
        /// mulitple times in the list. Similarly, if an item was first removed and
        /// then added, it will also appear in the list of removed item.</remarks>
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>The keys of the items removed.</returns>
        public IEnumerable<string> GetRemovedItems(string category)
        {
            if (_removedItems.TryGetValue(category, out var items))
            {
                return items;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Get the list of keys corresponding to the items present in <paramref name="category"/>.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
        {
            if (!_internalStore.TryGetValue(category, out var itemsInCategory))
            {
                keys = default;
                return false;
            }
            keys = itemsInCategory.Keys;
            return true;
        }

        /// <summary>
        /// Tries retrieving the item identified by the provided <paramref name="category"/>
        /// and <paramref name="key"/>.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="key">The item key.</param>
        /// <param name="value">The retrieved content.</param>
        /// <returns><b>true</b> if the item could be retrieved and stored in <paramref name="value"/>
        /// and <b>false</b> if no matching item could be found.</returns>
        public bool TryGetItem(string category, string key, out byte[] value)
        {
            if (_internalStore.TryGetValue(category, out var items))
            {
                return items.TryGetValue(key, out value);
            }
            value = default;
            return false;
        }

        /// <summary>
        /// Remove an item from the store.
        /// <remarks>
        /// Even if the store does not contain the specified <paramref name="key"/> in the
        /// specified <paramref name="category"/>, it keeps track of the deletion so that
        /// when applying the content of a store to another one, deletion can be forwarded
        /// which is used when doing differential checkpointing for example.
        /// </remarks>
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="key">The item key.</param>
        /// <returns><b>true</b> if the item was present and removed, <b>false</b> otherwise.</returns>
        public bool RemoveItem(string category, string key)
        {
            bool removed = false;
            if (_internalStore.TryGetValue(category, out var values))
            {
                removed = values.TryRemove(key, out _);
            }
            var removedKeys = _removedItems.GetOrAdd(category, c => new ConcurrentBag<string>());
            removedKeys.Add(key);
            return removed;
        }

        /// <summary>
        /// Add or update the value for the item identified by <paramref name="category"/>
        /// and <paramref name="key"/>.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="key">The item key.</param>
        /// <param name="value">The value to use as new item value.</param>
        public void AddOrUpdateItem(string category, string key, byte[] value)
        {
            var items = _internalStore.GetOrAdd(
                category,
                c => new ConcurrentDictionary<string, byte[]>());
            items.AddOrUpdate(key, value, (k, _) => value);
        }

        /// <summary>
        /// Update the content of the store with the content of <paramref name="update"/>.
        /// Any value present in <paramref name="update"/> overwrite the existing value in the store
        /// if present.
        /// Similarly, any deleted value in <paramref name="update"/>  trigger the deletion of
        /// the value with the corresponding category and key in the store if present.
        /// <remarks>As part of this operation, the content of <paramref name="update"/> is cleared.</remarks>
        /// </summary>
        /// <param name="update">The store containing the updated values</param>
        public void Update(InMemoryStateStore update)
        {
            if (update == null)
                throw new ArgumentNullException(nameof(update));

            foreach (var category in update._removedItems.Keys)
            {
                if (update._removedItems.TryGetValue(category, out var items))
                {
                    while (items.TryTake(out var key))
                    {
                        RemoveItem(category, key);
                    }
                }
            }

            foreach (var category in update._internalStore.Keys)
            {
                if (update._internalStore.TryGetValue(category, out var items))
                {
                    foreach (var key in items.Keys)
                    {
                        if (items.TryGetValue(key, out var value))
                        {
                            AddOrUpdateItem(category, key, value);
                        }
                    }
                }
            }
            update.Clear();
        }

        /// <summary>
        /// Clear the content of the store.
        /// <remarks>Should be used when the store is no longer used to help garbage collection.
        /// This also clear the list of removed items.</remarks>
        /// </summary>
        public void Clear()
        {
            // we do it in 2 passes to clear the content of each categories.
            string[] categories = _internalStore.Keys.ToArray();
            foreach (var category in categories)
            {
                if (_internalStore.TryGetValue(category, out var items))
                {
                    items.Clear();
                }
            }
            _internalStore.Clear();

            categories = _removedItems.Keys.ToArray();
            foreach (var category in categories)
            {
                if (_removedItems.TryGetValue(category, out var items))
                {
                    while (items.TryTake(out _)) { };
                }
            }
            _removedItems.Clear();
        }

        public string DebugView
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine("Store Id = " + Id);
                sb.AppendLine();
                sb.AppendLine();

                foreach (var kv in _internalStore)
                {
                    _removedItems.TryGetValue(kv.Key, out var rem);

                    sb.AppendLine(kv.Key);
                    sb.AppendLine(new string('=', kv.Key.Length));
                    sb.AppendLine();

                    foreach (var kv2 in kv.Value)
                    {
                        var rm = rem != null && rem.Contains(kv2.Key) ? " (removed)" : "";

                        sb.AppendLine(kv2.Key + rm);
                        sb.AppendLine(new string('-', kv2.Key.Length));
                        sb.AppendLine();

                        foreach (var chunk in kv2.Value.Buffer(40))
                        {
                            var bytes = BitConverter.ToString(chunk)
#if NET8_0 || NETSTANDARD2_1
                                .Replace("-", " ", StringComparison.Ordinal)
#else
                                .Replace("-", " ")
#endif
                                ;

                            var text = chunk.Select(b => (char)b).Aggregate(new StringBuilder(), (sbi, c) => char.IsControl(c) ? sbi.Append('?') : sbi.Append(c), sbi => sbi.ToString());

                            var pad = "    ";

                            if (chunk.Length != 40)
                            {
                                pad += new string(' ', (40 - chunk.Length) * 3);
                            }

                            sb.AppendLine(bytes + pad + text);
                        }

                        sb.AppendLine();
                        sb.AppendLine();
                    }

                    sb.AppendLine();
                    sb.AppendLine();
                }

                return sb.ToString();
            }
        }
    }
}
