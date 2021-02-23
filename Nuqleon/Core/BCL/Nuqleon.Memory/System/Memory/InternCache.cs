// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/04/2015 - Adding intern caches.
//

using System.Collections.Generic;

namespace System.Memory
{
    /// <summary>
    /// Provides extension methods to create intern caches to reuse values.
    /// </summary>
    public static partial class InternCache
    {
        /// <summary>
        /// Creates a new intern cache for values of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the values to intern.</typeparam>
        /// <param name="cacheFactory">The cache factory used to create an intern cache.</param>
        /// <param name="comparer">Comparer to compare the function argument during lookup in the memoization cache.</param>
        /// <returns>Empty intern cache for values of type <typeparamref name="T"/>.</returns>
        public static IInternCache<T> CreateInternCache<T>(this IMemoizationCacheFactory cacheFactory, IEqualityComparer<T> comparer = null)
            where T : class
        {
            if (cacheFactory == null)
                throw new ArgumentNullException(nameof(cacheFactory));

            var create = new Func<T, T>(x => x);
            var res = Memoizer.Create(cacheFactory).Memoize(create, MemoizationOptions.None, comparer);
            return new Strong<T>(res);
        }

        /// <summary>
        /// Creates a new intern cache for values of type <typeparamref name="T"/> that supports trimming entries.
        /// </summary>
        /// <typeparam name="T">The type of the values to intern.</typeparam>
        /// <param name="cacheFactory">The cache factory used to create an intern cache.</param>
        /// <param name="clone">Function to clone values in order to maintain intern cache entries. See remarks for more information.</param>
        /// <returns>Empty intern cache for values of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// Intern caches perform an equality based lookup to locate an existing interned copy of the provided value
        /// in an internal dictionary. After looking up an interned value, the caller can discard the original value
        /// it provided to the Intern method. The interned value is kept in the intern cache using a weak reference,
        /// so it can be trimmed from the cache if no more strong references are held to it. In order to prevent the
        /// intern cache's dictionary from keeping the weak references alive, the keys in the dictionary are created
        /// using the specified <paramref name="clone"/> function. This causes the reference held in the value to be
        /// different from the one in the key, which is only used for equality checks. A trim operation locates weak
        /// references in the value slots with a dangling reference and removes the cache entry from the dictionary.
        /// </remarks>
        public static IWeakInternCache<T> CreateWeakInternCache<T>(this IMemoizationCacheFactory cacheFactory, Func<T, T> clone)
            where T : class
        {
            if (cacheFactory == null)
                throw new ArgumentNullException(nameof(cacheFactory));
            if (clone == null)
                throw new ArgumentNullException(nameof(clone));

            var cloneSafe = new Func<T, T>(x =>
            {
                var copy = clone(x);

                if (ReferenceEquals(copy, x))
                    throw new InvalidOperationException("Clone function did return the same object reference.");

                return copy;
            });

            var create = new Func<T, WeakReference<T>>(x => new WeakReference<T>(cloneSafe(x)));
            var res = Memoizer.Create(cacheFactory).Memoize(create);
            return new Weak<T>(cloneSafe, res);
        }
    }
}
