// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System; // NB: Used for XML doc comments.
using System.Collections.Generic;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Bi-directional map allowing for efficient lookup by <typeparamref name="TKey"/> or <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored in the map.</typeparam>
    /// <typeparam name="TValue">The type of the values stored in the map.</typeparam>
    internal sealed class Map<TKey, TValue>
    {
        /// <summary>
        /// Dictionary from keys to values.
        /// </summary>
        private readonly Dictionary<TKey, TValue> _keyToValue;

        /// <summary>
        /// Dictionary from values to keys.
        /// </summary>
        private readonly Dictionary<TValue, TKey> _valueToKey;

        /// <summary>
        /// Creates a new bi-directional map using the specified comparers.
        /// </summary>
        /// <param name="keyComparer">The comparer to use to compare keys.</param>
        /// <param name="valueComparer">The compare to use to compare values.</param>
        public Map(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            _keyToValue = new Dictionary<TKey, TValue>(keyComparer);
            _valueToKey = new Dictionary<TValue, TKey>(valueComparer);
        }

        /// <summary>
        /// Gets the keys in the map.
        /// </summary>
        public IEnumerable<TKey> Keys => _keyToValue.Keys;

        /// <summary>
        /// Gets the values in the map.
        /// </summary>
        public IEnumerable<TValue> Values => _valueToKey.Keys;

        /// <summary>
        /// Adds an entry to the map with the specified <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key of the entry to add to the map.</param>
        /// <param name="value">The value of the entry to add to the map.</param>
        /// <exception cref="ArgumentException">An entry with the specified <paramref name="key"/> or <paramref name="value"/> already exists.</exception>
        /// <remarks>
        /// When this method throws, the map may be left in an invalid state.
        /// </remarks>
        public void Add(TKey key, TValue value)
        {
            _keyToValue.Add(key, value);
            _valueToKey.Add(value, key);
        }

        /// <summary>
        /// Clears all entries in the map.
        /// </summary>
        public void Clear()
        {
            _keyToValue.Clear();
            _valueToKey.Clear();
        }

        /// <summary>
        /// Checks if an entry with the specified <paramref name="key"/> exists.
        /// </summary>
        /// <param name="key">The key of the entry to look up.</param>
        /// <returns><c>true</c> if an entry with the specified <paramref name="key"/> exists; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(TKey key) => _keyToValue.ContainsKey(key);

        /// <summary>
        /// Checks if an entry with the specified <paramref name="value"/> exists.
        /// </summary>
        /// <param name="value">The value of the entry to look up.</param>
        /// <returns><c>true</c> if an entry with the specified <paramref name="value"/> exists; otherwise, <c>false</c>.</returns>
        public bool ContainsValue(TValue value) => _valueToKey.ContainsKey(value);

        /// <summary>
        /// Gets the value corresponding to the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the entry to look up.</param>
        /// <returns>The value of the entry with the specified <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">An entry with the specified <paramref name="key"/> is not found.</exception>
        public TValue GetByKey(TKey key) => _keyToValue[key];

        /// <summary>
        /// Gets the key corresponding to the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value of the entry to look up.</param>
        /// <returns>The key of the entry with the specified <paramref name="value"/>.</returns>
        /// <exception cref="KeyNotFoundException">An entry with the specified <paramref name="value"/> is not found.</exception>
        public TKey GetByValue(TValue value) => _valueToKey[value];

        /// <summary>
        /// Removes the entry with the specified <paramref name="key"/> and returns the corresponding value.
        /// </summary>
        /// <param name="key">The key of the entry to look up.</param>
        /// <returns>The value corresponding to the specified <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">An entry with the specified <paramref name="key"/> is not found.</exception>
        public TValue Remove(TKey key)
        {
            var value = _keyToValue[key];

            _keyToValue.Remove(key);
            _valueToKey.Remove(value);

            return value;
        }
    }
}
