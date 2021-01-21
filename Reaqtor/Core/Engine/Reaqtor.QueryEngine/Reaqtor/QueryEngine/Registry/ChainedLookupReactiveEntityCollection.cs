// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Reactive entity collection that looks in a first underlying collection. If an entity is not found in that collection,
    /// it falls back to a second underyling collection. Both collections can have different value types.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value in the first underlying collection.</typeparam>
    /// <typeparam name="TOtherValue">The type of the value in the second underlying collection.</typeparam>
    internal sealed class ChainedLookupReactiveEntityCollection<TKey, TValue, TOtherValue> : CooperativeLookupReactiveEntityCollection<TKey, TValue, Tuple<IReactiveEntityCollection<TKey, TOtherValue>, Func<TOtherValue, TValue>>>
    {
        /// <summary>
        /// Creates a new reactive entity collection using a primary and secondary (fallback) underlying collection.
        /// </summary>
        /// <param name="collection">The first underlying collection to look up entities.</param>
        /// <param name="otherCollection">The second underlying collection to fall back to if lookup for an entity fails in the first collection.</param>
        /// <param name="convertOther">Conversion function to make the second underlying collection's values compatible with the first underlying collection's values.</param>
        public ChainedLookupReactiveEntityCollection(IReactiveEntityCollection<TKey, TValue> collection, IReactiveEntityCollection<TKey, TOtherValue> otherCollection, Func<TOtherValue, TValue> convertOther)
            : base(collection, TryLookupOther, Tuple.Create(otherCollection, convertOther))
        {
        }

        private static bool TryLookupOther(Tuple<IReactiveEntityCollection<TKey, TOtherValue>, Func<TOtherValue, TValue>> tuple, TKey key, out TValue value)
        {
            if (tuple.Item1.TryGetValue(key, out TOtherValue other))
            {
                value = tuple.Item2(other);
                return true;
            }

            value = default;
            return false;
        }
    }

    /// <summary>
    /// Reactive entity collection that looks in a first underlying collection. If an entity is not found in that collection,
    /// it falls back to a second underyling collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    internal sealed class ChainedLookupReactiveEntityCollection<TKey, TValue> : CooperativeLookupReactiveEntityCollection<TKey, TValue, IReadOnlyReactiveEntityCollection<TKey, TValue>>
    {
        /// <summary>
        /// Creates a new reactive entity collection using a primary and secondary (fallback) underlying collection.
        /// </summary>
        /// <param name="collection">The first underlying collection to look up entities.</param>
        /// <param name="otherCollection">The second underlying collection to fall back to if lookup for an entity fails in the first collection.</param>
        public ChainedLookupReactiveEntityCollection(IReactiveEntityCollection<TKey, TValue> collection, IReadOnlyReactiveEntityCollection<TKey, TValue> otherCollection)
            : base(collection, TryLookupOther, otherCollection)
        {
        }

        private static bool TryLookupOther(IReadOnlyReactiveEntityCollection<TKey, TValue> otherCollection, TKey key, out TValue value)
        {
            return otherCollection.TryGetValue(key, out value);
        }
    }
}
