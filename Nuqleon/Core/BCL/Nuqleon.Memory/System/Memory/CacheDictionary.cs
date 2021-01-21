// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Memory
{
    internal sealed class CacheDictionary<K, T> : ICacheDictionary<K, T>
    {
        private readonly Dictionary<Maybe<K>, T> _dictionary;

        public CacheDictionary(IEqualityComparer<K> comparer)
        {
            _dictionary = new Dictionary<Maybe<K>, T>(new MaybeEqualityComparer<K>(comparer ?? FastEqualityComparer<K>.Default));
        }

        public int Count => _dictionary.Count;

        public IEnumerable<T> Values
        {
            get
            {
                //
                // NB: Needs to be lazy for the ranker, so each iteration gets the current snapshot.
                //

                foreach (var value in _dictionary.Values)
                {
                    yield return value;
                }
            }
        }

        public T GetOrAdd(K key, Func<K, T> valueFactory)
        {
            Debug.Assert(valueFactory != null);

            if (!TryGetValue(key, out T res))
            {
                res = valueFactory(key);
                Add(key, res);
            }

            return res;
        }

        public bool Remove(K key)
        {
            var keyOrNull = new Maybe<K>(key);

            return _dictionary.Remove(keyOrNull);
        }

        public void Clear() => _dictionary.Clear();

        private bool TryGetValue(K key, out T value)
        {
            var keyOrNull = new Maybe<K>(key);

            return _dictionary.TryGetValue(keyOrNull, out value);
        }

        private void Add(K key, T value)
        {
            var keyOrNull = new Maybe<K>(key);

            //
            // NB: We don't use Add; this allows to use Synchronized to escape the lock while calling
            //     the function and have more than one write of the value. This should be fine based
            //     on the assumption that the memoized function is pure.
            //
            _dictionary[keyOrNull] = value;
        }

        public IEnumerator<KeyValuePair<K, T>> GetEnumerator()
        {
            foreach (var kv in _dictionary)
            {
                yield return new KeyValuePair<K, T>(kv.Key.Value, kv.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
