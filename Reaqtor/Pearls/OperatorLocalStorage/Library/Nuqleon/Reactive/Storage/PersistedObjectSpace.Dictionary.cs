// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Reaqtive.Serialization;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Base class for storage entities representing a dictionary.
        /// </summary>
        /// <remarks>
        /// Persistence of a dictionary looks as follows:
        /// <c>
        /// items/0 = (a, 42)
        /// items/1 = (b, 43)
        /// items/2 = (e, 46)
        /// items/4 = (c, 44)
        /// items/6 = (d, 45)
        /// </c>
        /// The keys of the items are insignificant due to the unordered nature of the dictionary. Holes can exist in the key range (e.g. 3 in the same above).
        /// <list type="bullet">
        ///   <item><c>dictionary.Add(k, v)</c> results in <c> Add($"items/{getUnusedKey()}", (k, v))</c></item>
        ///   <item><c>dictionary.Remove(k)</c> results in <c>Delete($"items/{getKeyOf(k)}")</c></item>
        /// </list>
        /// </remarks>
        private abstract partial class Dictionary : Heap
        {
            /// <summary>
            /// Creates a new entity representing a dictionary.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public Dictionary(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Statically typed wrapper for a persisted dictionary with key type <typeparamref name="TKey"/> and value type <typeparamref name="TValue"/>.
            /// </summary>
            /// <typeparam name="TKey">The type of keys stored in the dictionary.</typeparam>
            /// <typeparam name="TValue">The type of values stored in the dictionary.</typeparam>
            /// <typeparam name="TDictionary">The type of the in-memory dictionary, inheriting from <see cref="IDictionary{TKey, TValue}"/>.</typeparam>
            protected abstract class WrapperBase<TKey, TValue, TDictionary> : PersistedBase, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IHeapPersistence
                where TDictionary : IDictionary<TKey, TValue>
            {
                /// <summary>
                /// The storage entity being wrapped.
                /// </summary>
                private readonly Dictionary _storage;

                /// <summary>
                /// The stored dictionary, always reflecting the latest in-memory state.
                /// </summary>
                protected readonly TDictionary _dictionary;

                /// <summary>
                /// The bi-directional map used to associate the storage key used for each element in <see cref="_dictionary"/>.
                /// </summary>
                private readonly Map<TKey, long> _storageKeys;

                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the dictionary.</param>
                /// <param name="storage">The storage entity representing the dictionary.</param>
                /// <param name="dictionary">The initial dictionary. This could either be the result of deserializing persisted state, or an empty dictionary for a new entity.</param>
                /// <param name="storageKeys">The initial bi-directional map associating the storage key used for each element in <paramref name="dictionary"/>.</param>
                public WrapperBase(string id, Dictionary storage, TDictionary dictionary, Map<TKey, long> storageKeys)
                    : base(id)
                {
                    _storage = storage;
                    _dictionary = dictionary;
                    _storageKeys = storageKeys;
                }

                /// <summary>
                /// Gets or sets the element with the specified key.
                /// </summary>
                /// <param name="key">The key of the element to get or set.</param>
                /// <returns>The element with the specified <paramref name="key"/>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
                /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception>
                public TValue this[TKey key]
                {
                    get => _dictionary[key];

                    set
                    {
                        if (_dictionary.ContainsKey(key))
                        {
                            _dictionary[key] = value;

                            var storageKey = _storageKeys.GetByKey(key);
                            _storage.Edit(storageKey);
                        }
                        else
                        {
                            Add(key, value);
                        }
                    }
                }

                /// <summary>
                /// Gets the element with the specified key.
                /// </summary>
                /// <param name="key">The key of the element to get.</param>
                /// <returns>The element with the specified <paramref name="key"/>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
                /// <exception cref="KeyNotFoundException">The specified <paramref name="key"/> is not found.</exception>
                TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => _dictionary[key];

                /// <summary>
                /// Gets the number of elements in the set.
                /// </summary>
                public int Count => _dictionary.Count;

                /// <summary>
                /// Gets a collection containing the keys in the dictionary.
                /// </summary>
                public abstract IReadOnlyCollection<TKey> Keys { get; }

                /// <summary>
                /// Gets a collection containing the values in the dictionary.
                /// </summary>
                public abstract IReadOnlyCollection<TValue> Values { get; }

                /// <summary>
                /// Gets a value indicating whether the dictionry is read-only. Always returns <c>false</c>.
                /// </summary>
                public bool IsReadOnly => false;

                /// <summary>
                /// Gets a collection containing the keys in the dictionary.
                /// </summary>
                ICollection<TKey> IDictionary<TKey, TValue>.Keys => _dictionary.Keys;

                /// <summary>
                /// Gets a collection containing the keys in the dictionary.
                /// </summary>
                IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _dictionary.Keys;

                /// <summary>
                /// Gets a collection containing the values in the dictionary.
                /// </summary>
                ICollection<TValue> IDictionary<TKey, TValue>.Values => _dictionary.Values;

                /// <summary>
                /// Gets a collection containing the values in the dictionary.
                /// </summary>
                IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _dictionary.Values;

                /// <summary>
                /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the dictionary.
                /// </summary>
                /// <param name="key">The key of the element to add.</param>
                /// <param name="value">The value of the element to add. The value can be <c>null</c> for reference types.</param>
                /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
                /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
                public void Add(TKey key, TValue value)
                {
                    //
                    // First try to add the element to the dictionary. This can cause an exception prior to editing the storage.
                    //

                    _dictionary.Add(key, value);

                    //
                    // NB: We use Add on the _storageKey map to catch failed invariants. In particular:
                    //
                    //     * _dictionary and _storageKeys should always be updated together, and,
                    //     * keys returned from the storage entity Add method should not be in use in _storageKeys.
                    //

                    var storageKey = _storage.Add();
                    _storageKeys.Add(key, storageKey);
                }

                /// <summary>
                /// Adds a dictionary entry specified in <paramref name="item"/>.
                /// </summary>
                /// <param name="item">The dictionary entry to add.</param>
                /// <exception cref="ArgumentNullException">The key of the specified entry is <c>null</c>.</exception>
                /// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
                public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

                /// <summary>
                /// Removes all entries from the dictionary.
                /// </summary>
                public void Clear()
                {
                    //
                    // Get all the keys currently in use. This is more efficient than having the storage entity apply all edit pages to _keys to derive this set.
                    //

                    var keys = new SortedSet<long>(_storageKeys.Values);

                    //
                    // Clear the dictionary and the map containing the storage keys.
                    //

                    _dictionary.Clear();
                    _storageKeys.Clear();

                    //
                    // Mark all the keys as deleted in the storage entity.
                    //

                    _storage.Clear(keys);
                }

                /// <summary>
                /// Determines whether the dictionary contains the specified <paramref name="item"/> representing a dictionary entry.
                /// </summary>
                /// <param name="item">The entry to locate in the dictionary.</param>
                /// <returns><c>true</c> if the entry is found in the dictionary; otherwise, <c>false</c>.</returns>
                public bool Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Contains(item);

                /// <summary>
                /// Determines whether the dictionary contains an element that has the specified <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The key to locate.</param>
                /// <returns><c>true</c> if the dictionary contains an element that has the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
                public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

                /// <summary>
                /// Copies the entries of the dictionary to the specified <paramref name="array"/> starting at the specified <paramref name="arrayIndex"/>.
                /// </summary>
                /// <param name="array">The array to copy the dictionary entries to.</param>
                /// <param name="arrayIndex">The index in the array to copy the first entry to.</param>
                /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
                /// <exception cref="ArgumentException">The number of entries in the dictionary is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
                public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);

                /// <summary>
                /// Gets an enumerator to enumerate over the entries in the dictionary. Enumeration order is undefined.
                /// </summary>
                /// <returns>An enumerator to enumerate over the entries in the dictionary.</returns>
                public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

                /// <summary>
                /// Removes the value with the specified <paramref name="key"/> from the dictionary.
                /// </summary>
                /// <param name="key">The key of the element to remove.</param>
                /// <returns><c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>.</returns>
                public bool Remove(TKey key)
                {
                    //
                    // Remove the item from the underlying set and free the key from the storage entity if the item was found and removed.
                    //

                    if (_dictionary.Remove(key))
                    {
                        //
                        // NB: We use the Remove method on the _storageKey map to catch failed invariants when the key is not found. In particular:
                        //
                        //     * _dictionary and _storageKeys should always be updated together, and,
                        //     * an entry found in the dictionary should always have a key in the _storageKeys map.
                        //

                        var storageKey = _storageKeys.Remove(key);
                        _storage.Remove(storageKey);
                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Removes the specified <paramref name="item"/> representing a dictionary entry.
                /// </summary>
                /// <param name="item">The dictionary entry to remove.</param>
                /// <returns><c>true</c> if the entry is successfully found and removed; otherwise, <c>false</c>.</returns>
                public bool Remove(KeyValuePair<TKey, TValue> item)
                {
                    //
                    // Remove the item from the underlying set and free the key from the storage entity if the item was found and removed.
                    //

                    if (((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Remove(item))
                    {
                        //
                        // NB: We use the Remove method on the _storageKey map to catch failed invariants when the key is not found. In particular:
                        //
                        //     * _dictionary and _storageKeys should always be updated together, and,
                        //     * an entry found in the dictionary should always have a key in the _storageKeys map.
                        //

                        var key = _storageKeys.Remove(item.Key);
                        _storage.Remove(key);
                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Gets the value that is associated with the specified <paramref name="key"/>.
                /// </summary>
                /// <param name="key">The key to locate.</param>
                /// <param name="value">When this method returns, the value associated with the specified <paramref name="key"/>, if the key is found; otherwise, the default value for the type of the value parameter.</param>
                /// <returns><c>true</c> if the dictionary contains an element that has the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
                /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
                public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

                /// <summary>
                /// Gets an enumerator to enumerate over the entries in the dictionary. Enumeration order is undefined.
                /// </summary>
                /// <returns>An enumerator to enumerate over the entries in the dictionary.</returns>
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                /// <summary>
                /// Saves the dictionary entry stored at the specified <paramref name="storageKey"/> to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the dictionary entry to.</param>
                /// <param name="storageKey">The key of the dictionary entry to save.</param>
                void IHeapPersistence.Save(ISerializerFactory serializerFactory, Stream stream, string storageKey)
                {
                    var keyValue = GetIndexForKey(storageKey);
                    var key = _storageKeys.GetByValue(keyValue);
                    var entry = new KeyValuePair<TKey, TValue>(key, this[key]);
                    serializerFactory.GetSerializer<KeyValuePair<TKey, TValue>>().Serialize(entry, stream);
                }
            }
        }
    }
}
