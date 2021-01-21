// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface for an inverted reactive entity collection, i.e. supporting a lookup based on values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    internal interface IInvertedLookupReactiveEntityCollection<TKey, TValue> : IReactiveEntityCollection<TKey, TValue>
    {
        /// <summary>
        /// Tries to locate an entry with the specified value, returning the corresponding key.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        /// <param name="key">The key of the entry with the specified key, if found.</param>
        /// <returns>true if the value is found; otherwise, false.</returns>
        bool TryGetKey(TValue value, out TKey key);
    }
}
