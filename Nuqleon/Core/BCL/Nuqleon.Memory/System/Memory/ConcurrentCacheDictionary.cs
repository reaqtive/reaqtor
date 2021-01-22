// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Memory
{
    internal sealed class ConcurrentCacheDictionary<K, T> : ICacheDictionary<K, T>
    {
        private readonly ConcurrentDictionary<Maybe<K>, T> _dictionary;

        public ConcurrentCacheDictionary(IEqualityComparer<K> comparer)
        {
            _dictionary = new ConcurrentDictionary<Maybe<K>, T>(new MaybeEqualityComparer<K>(comparer ?? FastEqualityComparer<K>.Default));
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

            var keyOrNull = new Maybe<K>(key);

            //
            // NB: Not using GetOrAdd in order to avoid closures. This breaks atomicity with deletes, but that's
            //     fine in all cases. If we succeed getting the value, the caller gets the memoized function
            //     result, regardless of whether there's a race with a deletion. If we don't succeed getting the
            //     value, we compute it and store it in the dictionary. It may get removed immediately again but
            //     we can always recompute it upon a subsequent access attempt.
            //
            if (!_dictionary.TryGetValue(keyOrNull, out T res))
            {
                res = valueFactory(key);
                _dictionary.TryAdd(keyOrNull, res);
            }

            return res;
        }

        public bool Remove(K key) => _dictionary.TryRemove(new Maybe<K>(key), out _);

        public void Clear() => _dictionary.Clear();

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
