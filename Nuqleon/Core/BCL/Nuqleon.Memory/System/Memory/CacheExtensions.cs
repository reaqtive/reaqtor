// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 11/14/2014 - Generated the code in this file.
//

namespace System.Memory
{
    internal static class CacheExtensions
    {
        public static IReference<T> CreateOrGetEntry<T>(this ICache<T> cache, T item)
        {
            if (item == null)
            {
                return DefaultDiscardable<T>.Instance;
            }

            if (cache is Cache<T> leafCache)
            {
                var entry = leafCache.Storage.GetEntry(item);
                if (entry == null)
                {
                    throw new InvalidOperationException("Unexpected null entry returned from cache storage.");
                }
                return entry;
            }
            else
            {
                return cache.Create(item);
            }
        }

        public static void ReleaseIfEntry<T>(this ICache<T> cache, IReference<T> item)
        {
            if (item is not IDiscardable<T>)
            {
                ((Cache<T>)cache).Storage.ReleaseEntry(item);
            }
        }

        public static void ReleaseOrDispose<T>(this ICache<T> cache, IReference<T> item)
        {
            if (item is IDiscardable<T> discardable)
            {
                discardable.Dispose();
            }
            else
            {
                ((Cache<T>)cache).Storage.ReleaseEntry(item);
            }
        }
    }
}
