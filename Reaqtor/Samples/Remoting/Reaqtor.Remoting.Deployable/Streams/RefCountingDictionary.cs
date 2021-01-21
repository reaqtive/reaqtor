// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Reaqtor.Remoting.Deployable.Streams
{
    internal static class RefCountingDictionary
    {
        public static void Release<TKey, TValue>(this IRefCountingDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.Release(key, out _);
        }
    }

    internal class RefCountingDictionary<TKey, TValue> : IRefCountingDictionary<TKey, TValue>
    {
        /// <summary>
        /// We support null for keys and refcounting for values by wrapping them in structs. For atomicity guarantees we 
        /// implement equality checks on the ref counting struct (this is just a simple comparison of refcounts).
        /// </summary>
        private readonly ConcurrentDictionary<Holder<TKey>, RefCounted<TValue>> _dictionary;

        public RefCountingDictionary()
            : this(EqualityComparer<TKey>.Default)
        {
        }

        public RefCountingDictionary(IEqualityComparer<TKey> keyComparer)
        {
            _dictionary = new ConcurrentDictionary<Holder<TKey>, RefCounted<TValue>>(new WrappedEqualityComparer<TKey>(keyComparer));
        }

        public bool Release(TKey key, out TValue value)
        {
            var ret = _dictionary.AddOrUpdate(key,
                k => { throw new InvalidOperationException("Releasing an item that had no references."); },
                (k, v) => new RefCounted<TValue>() { value = v.value, refcount = v.refcount - 1 });

            if (ret.refcount == 0)
            {
                ((ICollection<KeyValuePair<Holder<TKey>, RefCounted<TValue>>>)_dictionary).Remove(new KeyValuePair<Holder<TKey>, RefCounted<TValue>>(key, ret));
                value = ret.value;
                return true;
            }

            value = default;
            return false;
        }

        public TValue AddRef(TKey key, Func<TKey, TValue> factory)
        {
            var ret = _dictionary.AddOrUpdate(key,
                k => new RefCounted<TValue>() { value = factory(k), refcount = 1 },
                (k, v) => v.refcount == 0 ? new RefCounted<TValue>() { value = factory(k), refcount = 1 } : new RefCounted<TValue>() { value = v.value, refcount = v.refcount + 1 });

            return ret.value;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var ret = _dictionary.TryGetValue(key, out RefCounted<TValue> val);
            value = val.value;
            return ret;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var kvp in _dictionary)
            {
                yield return new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value.value);
            }
        }
    }

    internal struct Holder<T>
    {
        public T obj;

        public static implicit operator T(Holder<T> obj) => obj.obj;

        public static implicit operator Holder<T>(T obj) => new Holder<T> { obj = obj };
    }

    internal struct RefCounted<T> : IEquatable<RefCounted<T>>
    {
        public T value;
        public long refcount;

        public bool Equals(RefCounted<T> other) => refcount == other.refcount;

        public override bool Equals(object obj) => obj is RefCounted<T> counted && Equals(counted);

        public override int GetHashCode() => (int)(EqualityComparer<T>.Default.GetHashCode(value) * 37 + refcount);
    }

    internal sealed class WrappedEqualityComparer<T> : IEqualityComparer<Holder<T>>
    {
        private readonly IEqualityComparer<T> _equalityComparer;

        public WrappedEqualityComparer(IEqualityComparer<T> equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool Equals(Holder<T> x, Holder<T> y) => _equalityComparer.Equals(x.obj, y.obj);

        public int GetHashCode(Holder<T> obj) => _equalityComparer.GetHashCode(obj.obj);
    }
}
