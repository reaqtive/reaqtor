// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

namespace System.Collections.ObjectModel
{
    /// <summary>
    /// Provides a set of extension methods for <see cref="ReadOnlyCollection{T}"/>.
    /// </summary>
    internal static class ReadOnlyCollectionExtensions
    {
        /// <summary>
        /// Returns a new read-only collection with the specified <paramref name="first"/> element prepended to the specified <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="source">The read-only collection to prepend an element to.</param>
        /// <param name="first">First first element in the resulting collection.</param>
        /// <returns>A new read-only collection with the specified <paramref name="first"/> element prepended to the specified <paramref name="source"/>.</returns>
        public static ReadOnlyCollection<T> AddFirst<T>(this ReadOnlyCollection<T> source, T first)
        {
            var count = source.Count;

            var res = new T[count + 1];

            res[0] = first;

            for (var i = 0; i < count; i++)
            {
                res[i + 1] = source[i];
            }

            return new ReadOnlyCollection<T>(res);
        }
    }
}
