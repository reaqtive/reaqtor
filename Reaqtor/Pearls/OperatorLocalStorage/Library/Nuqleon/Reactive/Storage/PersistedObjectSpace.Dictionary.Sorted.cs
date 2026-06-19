// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Creates a persisted sorted dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the sorted dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the sorted dictionary.</typeparam>
        /// <param name="id">The identifier to use for the dictionary.</param>
        /// <returns>A new persisted sorted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        public IPersistedSortedDictionary<TKey, TValue> CreateSortedDictionary<TKey, TValue>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var set = new SortedDictionary(this);
            _items.Add(id, set);
            return CreateSortedDictionaryCore<TKey, TValue>(id, set);
        }

        /// <summary>
        /// Gets a persisted sorted dictionary with the specified identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the sorted dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the sorted dictionary.</typeparam>
        /// <param name="id">The identifier of the sorted dictionary to retrieve.</param>
        /// <returns>An existing persisted sorted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted sorted dictionary type.</exception>
        public IPersistedSortedDictionary<TKey, TValue> GetSortedDictionary<TKey, TValue>(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return CreateSortedDictionaryCore<TKey, TValue>(id, (SortedDictionary)_items[id]);
        }

        /// <summary>
        /// Creates a statically typed wrapper around the specified sorted <paramref name="dictionary"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the sorted dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the sorted dictionary.</typeparam>
        /// <param name="id">The identifier of the sorted dictionary.</param>
        /// <param name="dictionary">The storage entity representing the sorted dictionary</param>
        /// <returns>A statically typed wrapper around the specified sorted <paramref name="dictionary"/>.</returns>
        private static IPersistedSortedDictionary<TKey, TValue> CreateSortedDictionaryCore<TKey, TValue>(string id, SortedDictionary dictionary) => dictionary.Create<TKey, TValue>(id);

        /// <summary>
        /// Storage entity representing a sorted dictionary.
        /// </summary>
        private sealed partial class SortedDictionary : Dictionary
        {
            /// <summary>
            /// Creates a new entity representing a set.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public SortedDictionary(PersistedObjectSpace parent)
                : base(parent)
            {
            }

            /// <summary>
            /// Gets the kind of the entity. Always returns <see cref="PersistableKind.SortedDictionary"/>.
            /// </summary>
            public override PersistableKind Kind => PersistableKind.SortedDictionary;

            /// <summary>
            /// Creates a statically typed wrapper around the storage entity. Multiple calls to this method are valid when using the same types <typeparamref name="TKey"/> and <typeparamref name="TValue"/>, returning the same wrapper instance.
            /// </summary>
            /// <typeparam name="TKey">The type of keys stored in the dictionary.</typeparam>
            /// <typeparam name="TValue">The type of values stored in the dictionary.</typeparam>
            /// <param name="id">The identifier of the dictionary.</param>
            /// <returns>A statically typed wrapper around the storage entity.</returns>
            /// <exception cref="InvalidCastException">The type <typeparamref name="TKey"/> or the type <typeparamref name="TValue"/> is incompatible with previously requested statically typed wrappers for the storage entity.</exception>
            public IPersistedSortedDictionary<TKey, TValue> Create<TKey, TValue>(string id)
            {
                if (_wrapper == null)
                {
                    //
                    // NB: Restoring a dictionary to an in-memory data structure returns both the set (of type Dictionary<TKey, TValue>) and the map of dictionary keys to storage keys (of type Map<TKey, long>).
                    //

                    var (dictionary, storageKeys) = Restore<TKey, TValue>();

                    var wrapper = new Wrapper<TKey, TValue>(id, this, dictionary, storageKeys);
                    _wrapper = wrapper;
                    return wrapper;
                }
                else
                {
                    return (IPersistedSortedDictionary<TKey, TValue>)_wrapper;
                }
            }

            /// <summary>
            /// Restores the storage entity to a strongly typed in-memory sorted sorted dictionary representation with key type <typeparamref name="TKey"/> and value type <typeparamref name="TValue"/> by deserializing state that was loaded by <see cref="Load"/>.
            /// If the entity has not been persisted before, this methods returns a new empty sorted sorted dictionary instance.
            /// </summary>
            /// <typeparam name="TKey">The type of keys stored in the sorted dictionary.</typeparam>
            /// <typeparam name="TValue">The type of values stored in the sorted dictionary.</typeparam>
            /// <returns>A pair consisting of an instance of type SortedDictionary{<typeparamref name="TKey"/>, <typeparamref name="TValue"/>} containing the data represented by the storage entity, and a map that associates all dictionary entries with their storage key.</returns>
            private (SortedDictionary<TKey, TValue> dictionary, Map<TKey, long> storageKeys) Restore<TKey, TValue>()
            {
                //
                // The equality comparer to use for values, both in the dictionary (to determine uniqueness) and the map (for the value-to-key entries).
                //
                // REVIEW: Consider making this configurable, which requires the persistence of an expression to construct the comparer in the metadata (likely using a wildcard type so it can be late-bound).
                //
                // NB: We assume that the default equality and ordinal comparers are consistent when it comes to performing equality checks (i.e. (Compare(a, b) == 0) == Equals(a, b)).
                //

                var eq = EqualityComparer<TKey>.Default;
                var cmp = Comparer<TKey>.Default;

                //
                // Always allocate an empty dictionary and map. In case we were called for a new dictionary, we'll just return the empty ones.
                //

                var dictionary = new SortedDictionary<TKey, TValue>(cmp);
                var storageKeys = new Map<TKey, long>(eq, EqualityComparer<long>.Default);

                //
                // If the state was loaded by LoadCore, we have a eventual objects to deserialize dictionary elements from. Keep track of both the values and the storage keys.
                //

                if (_data != null)
                {
                    //
                    // Obtain the deserializer once to reduce overhead.
                    //

                    var deserializer = SerializationFactory.GetDeserializer<KeyValuePair<TKey, TValue>>();

                    //
                    // Deserialize each eventual object and populate the dictionary and map entries.
                    //

                    foreach (var (key, data) in _data)
                    {
                        var value = data.Deserialize(deserializer);

                        //
                        // NB: The use of the Add method on the storageKeys map has the side-effect of asserting uniqueness of entries, causing restoration to fail if duplicate entries are detected.
                        //     It is assumed that the equality comparer for TKey is stable across recoveries.
                        //

                        dictionary.Add(value.Key, value.Value);
                        storageKeys.Add(value.Key, key);
                    }

                    _data = null;
                }

                return (dictionary, storageKeys);
            }

            /// <summary>
            /// Statically typed wrapper for a persisted dictionary with key type <typeparamref name="TKey"/> and value type <typeparamref name="TValue"/>.
            /// </summary>
            /// <typeparam name="TKey">The type of keys stored in the dictionary.</typeparam>
            /// <typeparam name="TValue">The type of values stored in the dictionary.</typeparam>
            private sealed class Wrapper<TKey, TValue> : WrapperBase<TKey, TValue, SortedDictionary<TKey, TValue>>, IPersistedSortedDictionary<TKey, TValue>
            {
                /// <summary>
                /// Creates a new wrapper around the specified <paramref name="storage"/> entity.
                /// </summary>
                /// <param name="id">The identifier of the dictionary.</param>
                /// <param name="storage">The storage entity representing the dictionary.</param>
                /// <param name="dictionary">The initial dictionary. This could either be the result of deserializing persisted state, or an empty dictionary for a new entity.</param>
                /// <param name="storageKeys">The initial bi-directional map associating the storage key used for each element in <paramref name="dictionary"/>.</param>
                public Wrapper(string id, Dictionary storage, SortedDictionary<TKey, TValue> dictionary, Map<TKey, long> storageKeys)
                    : base(id, storage, dictionary, storageKeys)
                {
                }

                /// <summary>
                /// Gets a collection containing the keys in the dictionary.
                /// </summary>
                public override IReadOnlyCollection<TKey> Keys => _dictionary.Keys;

                /// <summary>
                /// Gets a collection containing the values in the dictionary.
                /// </summary>
                public override IReadOnlyCollection<TValue> Values => _dictionary.Values;
            }
        }
    }
}
