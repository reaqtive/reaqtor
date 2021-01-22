// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Diagnostics;
using System.Runtime.CompilerServices;

#if DEBUG
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#endif

namespace System.Memory
{
    internal sealed class WeakCacheDictionary<K, T> : IWeakCacheDictionary<K, T>
        where K : class
        where T : class
    {
#if DEBUG
        private static readonly PropertyInfo s_Keys = typeof(ConditionalWeakTable<K, T>).GetProperty("Keys", BindingFlags.Instance | BindingFlags.NonPublic);
#endif

        private readonly ConditionalWeakTable<K, T> _dictionary;
        private bool _hasNullValue;
        private T _nullValue;

        public WeakCacheDictionary() => _dictionary = new ConditionalWeakTable<K, T>();

#if DEBUG
        public ICollection<K> Keys
        {
            get
            {
                ICollection<K> keys;

                if (_dictionary is IEnumerable<KeyValuePair<K, T>> enumerable)
                {
                    keys = enumerable.Select(kv => kv.Key).ToList();
                }
                else
                {
                    keys = (ICollection<K>)s_Keys.GetValue(_dictionary, index: null);
                }

                lock (_dictionary) // protects null value
                {
                    if (_hasNullValue)
                    {
                        keys.Add(item: null);
                    }
                }

                return keys;
            }
        }
#endif

        public T GetOrAdd(K key, ConditionalWeakTable<K, T>.CreateValueCallback valueFactory)
        {
            Debug.Assert(valueFactory != null);

            if (key == null)
            {
                lock (_dictionary) // protects null value
                {
                    if (!_hasNullValue)
                    {
                        _nullValue = valueFactory(key);
                        _hasNullValue = true;
                    }

                    return _nullValue;
                }
            }
            else
            {
                return _dictionary.GetValue(key, valueFactory);
            }
        }

        public bool Remove(K key)
        {
            if (key == null)
            {
                lock (_dictionary) // protects null value
                {
                    var hasNullValue = _hasNullValue;

                    _nullValue = null;
                    _hasNullValue = false;

                    return hasNullValue;
                }
            }
            else
            {
                return _dictionary.Remove(key);
            }
        }
    }
}
