// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Reactive entity collection with a fallback function for failed lookups.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    /// <typeparam name="TArg">The type of an additional argument passed threaded to the lookup function.</typeparam>
    internal class CooperativeLookupReactiveEntityCollection<TKey, TValue, TArg> : IReactiveEntityCollection<TKey, TValue>
    {
        private readonly IReactiveEntityCollection<TKey, TValue> _local;
        private readonly TryLookup<TArg, TKey, TValue> _tryLookup;
        private readonly TArg _arg;

        /// <summary>
        /// Creates a reactive entity collection wrapping a specified underlying collection and a fallback function.
        /// </summary>
        /// <param name="local">The underlying collection to use for lookups.</param>
        /// <param name="tryLookup">The fallback function to use in case lookup in the collection fails.</param>
        /// <param name="arg">The argument to pass to the fallback function (avoids a closure).</param>
        public CooperativeLookupReactiveEntityCollection(IReactiveEntityCollection<TKey, TValue> local, TryLookup<TArg, TKey, TValue> tryLookup, TArg arg)
        {
            _local = local;
            _tryLookup = tryLookup;
            _arg = arg;
        }

        public bool ContainsKey(TKey key) => _local.ContainsKey(key) || _tryLookup(_arg, key, out var dummy);

        public bool TryAdd(TKey key, TValue value) => _local.TryAdd(key, value);

        public bool TryGetValue(TKey key, out TValue value) => _local.TryGetValue(key, out value) || _tryLookup(_arg, key, out value);

        public bool TryRemove(TKey key, out TValue value) => _local.TryRemove(key, out value);

        public ICollection<TValue> Values => _local.Values;

        public IEnumerable<TKey> RemovedKeys => _local.RemovedKeys;

        public void ClearRemovedKeys(IEnumerable<TKey> keys) => _local.ClearRemovedKeys(keys);

        public void Clear() => _local.Clear();

        public void Dispose() => _local.Dispose();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _local.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal delegate bool TryLookup<TArg, TKey, TValue>(TArg argument, TKey key, out TValue value);
}
