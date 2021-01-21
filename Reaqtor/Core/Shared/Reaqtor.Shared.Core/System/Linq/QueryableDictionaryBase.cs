// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// Base class for queryable dictionaries.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">Type of the values in the dictionary.</typeparam>
    public abstract class QueryableDictionaryBase<TKey, TValue> : IQueryableDictionary<TKey, TValue>
    {
        /// <summary>
        /// Gets an enumerator to enumerate over the entries in the dictionary.
        /// </summary>
        /// <returns>Enumerator to enumerate over the entries in the dictionary.</returns>
        public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        /// <summary>
        /// Gets the element type of entires in the dictionary.
        /// </summary>
        public virtual Type ElementType => typeof(KeyValuePair<TKey, TValue>);

        /// <summary>
        /// Gets the expression representing the dictionary.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Gets the query provider used to build queries over the dictionary.
        /// </summary>
        public abstract IQueryProvider Provider { get; }

        /// <summary>
        /// Gets an enumerator to enumerate over the entries in the dictionary.
        /// </summary>
        /// <returns>Enumerator to enumerate over the entries in the dictionary.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Checks whether the dictionary contains an entry for the given key.
        /// </summary>
        /// <param name="key">Key to look up in the dictionary.</param>
        /// <returns>true if the dictionary contains an entry with the specified key; otherwise, false.</returns>
        public virtual bool ContainsKey(TKey key)
        {
            var f = GetFilterByKey(key);
            return this.Any(f);
        }

        /// <summary>
        /// Gets the keys in the dictionary.
        /// </summary>
        public IQueryable<TKey> Keys => this.Select(kv => kv.Key);

        /// <summary>
        /// Gets the keys in the dictionary.
        /// </summary>
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys.AsEnumerable();

        /// <summary>
        /// Tries to obtain the value for the specified key.
        /// </summary>
        /// <param name="key">Key to look up in the dictionary.</param>
        /// <param name="value">Value for the specified key, if found.</param>
        /// <returns>true if an entry is found; otherwise, false.</returns>
        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            var f = GetFilterByKey(key);

            var r = this.SingleOrDefault(f);
            if (!object.Equals(r, default(KeyValuePair<TKey, TValue>)))
            {
                value = r.Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Gets the values in the dictionary.
        /// </summary>
        public IQueryable<TValue> Values => this.Select(kv => kv.Value);

        /// <summary>
        /// Gets the values in the dictionary.
        /// </summary>
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values.AsEnumerable();

        /// <summary>
        /// Retrieves the dictionary entry with the given key from the dictionary.
        /// </summary>
        /// <param name="key">Key to look up in the dictionary.</param>
        /// <returns>Value for the specified key, if found. If no entry with the specified key is found, a KeyNotFoundException is thrown.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no entry with the specified key is found.</exception>
        public virtual TValue this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out TValue value))
                {
                    throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "No entry with key '{0}' found.", key));
                }

                return value;
            }
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression (only o n.NET 5.0)
#pragma warning disable CA1829 // Use Length/Count property instead of Count() when available (https://github.com/dotnet/roslyn/issues/50202)

        /// <summary>
        /// Gets the number of entries in the dictionary.
        /// </summary>
        public virtual int Count => this.Count();

#pragma warning restore CA1829
#pragma warning restore IDE0079

        private static Expression<Func<KeyValuePair<TKey, TValue>, bool>> GetFilterByKey(TKey key)
        {
            var p = Expression.Parameter(typeof(KeyValuePair<TKey, TValue>), "item");

            var e = Expression.Equal(
                Expression.Property(p, p.Type.GetProperty(nameof(KeyValuePair<TKey, TValue>.Key))),
                Expression.Constant(key)
            );

            var f = Expression.Lambda<Func<KeyValuePair<TKey, TValue>, bool>>(e, p);
            return f;
        }
    }
}
