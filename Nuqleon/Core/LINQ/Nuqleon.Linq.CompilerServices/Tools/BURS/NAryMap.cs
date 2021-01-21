// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq.CompilerServices
{
    internal sealed class NAryMap<TKey, TValue> : IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>>
    {
        private readonly Dictionary<TKey, NAryMap<TKey, TValue>> _map;
        private readonly Dictionary<TKey, TValue> _leaf;

        public NAryMap(int n)
        {
            Debug.Assert(n > 0);

            Levels = n;

            if (n == 1)
                _leaf = new Dictionary<TKey, TValue>();
            else
                _map = new Dictionary<TKey, NAryMap<TKey, TValue>>();
        }

        public int Levels { get; }

        public TValue this[params TKey[] keys]
        {
            get
            {
                Debug.Assert(keys != null);

                return this[(IEnumerable<TKey>)keys];
            }

            set
            {
                Debug.Assert(keys != null);

                this[(IEnumerable<TKey>)keys] = value;
            }
        }

        public TValue this[IEnumerable<TKey> keys]
        {
            get
            {
                Debug.Assert(keys != null);

                if (!TryGetValue(keys, out TValue res))
                    throw new KeyNotFoundException();

                return res;
            }

            set
            {
                Debug.Assert(keys != null);

                Add(keys.GetEnumerator(), value);
            }
        }

        private void Add(IEnumerator<TKey> keys, TValue value)
        {
            Debug.Assert(keys != null);

            if (keys.MoveNext())
            {
                var i = keys.Current;

                if (Levels == 1)
                {
                    try
                    {
                        if (keys.MoveNext())
                            throw new ArgumentOutOfRangeException(nameof(keys), "Too many key values.");
                    }
                    finally
                    {
                        keys.Dispose();
                    }

                    _leaf[i] = value;
                }
                else
                {
                    if (!_map.TryGetValue(i, out NAryMap<TKey, TValue> m))
                    {
                        m = new NAryMap<TKey, TValue>(Levels - 1);
                        _map[i] = m;
                    }

                    m.Add(keys, value);
                }
            }
            else
            {
                keys.Dispose();
                throw new ArgumentOutOfRangeException(nameof(keys), "Insufficient key values.");
            }
        }

        public bool TryGetValue(IEnumerable<TKey> keys, out TValue res)
        {
            Debug.Assert(keys != null);

            return TryGetValue(keys.GetEnumerator(), out res);
        }

        private bool TryGetValue(IEnumerator<TKey> keys, out TValue res)
        {
            if (keys.MoveNext())
            {
                var i = keys.Current;

                if (Levels == 1)
                {
                    var ok = _leaf.TryGetValue(i, out res);

                    try
                    {
                        if (keys.MoveNext())
                            throw new ArgumentOutOfRangeException(nameof(keys), "Too many key values.");

                        return ok;
                    }
                    finally
                    {
                        keys.Dispose();
                    }
                }
                else
                {
                    if (_map.TryGetValue(i, out NAryMap<TKey, TValue> m))
                    {
                        return m.TryGetValue(keys, out res);
                    }
                }
            }
            else
            {
                keys.Dispose();
                throw new ArgumentOutOfRangeException(nameof(keys), "Insufficient key values.");
            }

            res = default;
            return false;
        }

        public bool TryGetValue(TKey key, out NAryMapOrLeaf<TKey, TValue> res)
        {
            res = default;

            if (Levels == 1)
            {
                var ok = _leaf.TryGetValue(key, out TValue i);
                if (ok)
                    res = NAryMapOrLeaf<TKey, TValue>.CreateLeaf(i);

                return ok;
            }
            else
            {
                var ok = _map.TryGetValue(key, out NAryMap<TKey, TValue> m);
                if (ok)
                    res = NAryMapOrLeaf<TKey, TValue>.CreateMap(m);

                return ok;
            }
        }

        public IEnumerator<KeyValuePair<IEnumerable<TKey>, TValue>> GetEnumerator()
        {
            if (Levels == 1)
            {
                foreach (var kv in _leaf)
                    yield return new KeyValuePair<IEnumerable<TKey>, TValue>(new[] { kv.Key }, kv.Value);
            }
            else
            {
                foreach (var m in _map)
                {
                    foreach (var kv in m.Value)
                    {
                        yield return new KeyValuePair<IEnumerable<TKey>, TValue>(new[] { m.Key }.Concat(kv.Key), kv.Value);
                    }
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal sealed class NAryMapOrLeaf<TKey, TValue>
    {
        private NAryMapOrLeaf()
        {
        }

        public NAryMap<TKey, TValue> Map { get; private set; }
        public TValue Leaf { get; private set; }

        public static NAryMapOrLeaf<TKey, TValue> CreateMap(NAryMap<TKey, TValue> map) => new() { Map = map };

        public static NAryMapOrLeaf<TKey, TValue> CreateLeaf(TValue leaf) => new() { Leaf = leaf };
    }
}
