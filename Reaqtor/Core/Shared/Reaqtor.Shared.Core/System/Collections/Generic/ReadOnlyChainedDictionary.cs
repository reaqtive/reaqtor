// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2014 - Created this file.
//

using System.Collections.ObjectModel;
using System.Linq;

namespace System.Collections.Generic
{
    internal class ReadOnlyChainedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _first;
        private readonly IDictionary<TKey, TValue> _second;

        public ReadOnlyChainedDictionary(IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            _first = first;
            _second = second;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            // Note: can't use LINQ's Contains method, because it checks for ICollection,
            //       causing infinite recursion and a stack overflow.

            foreach (var kv in this)
            {
                if (object.Equals(kv, item))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return _first.ContainsKey(key) || _second.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                // Note: Not caching because the underlying collection may be mutable.

                var res = _first.Keys.Union(_second.Keys).ToList();
                return new ReadOnlyCollection<TKey>(res);
            }
        }

        public bool TryGetValue(TKey key, out TValue value) => _first.TryGetValue(key, out value) || _second.TryGetValue(key, out value);

        public ICollection<TValue> Values
        {
            get
            {
                // Note: Not caching because the underlying collection may be mutable.

                var res = this.Select(kv => kv.Value).ToList();
                return new ReadOnlyCollection<TValue>(res);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out var res))
                    throw new KeyNotFoundException();

                return res;
            }

            set => throw new NotImplementedException();
        }

        public int Count => Keys.Count;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var hidden = new HashSet<TKey>();
            foreach (var kv in _first)
            {
                hidden.Add(kv.Key);
                yield return kv;
            }

            foreach (var kv in _second)
            {
                if (!hidden.Contains(kv.Key))
                {
                    yield return kv;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsReadOnly => true;

        #region Not supported

        public void Add(TKey key, TValue value) => throw new NotImplementedException();

        public void Add(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotImplementedException();

        public bool Remove(TKey key) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

        #endregion
    }

}
