// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 10/27/2014 - Created this type.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1724 // The type name Cache conflicts in whole or in part with the namespace name 'System.Net.Cache'.

using System.Threading;

namespace System.Memory
{
    /// <summary>
    /// Cache implementation for types that do not need to be deconstructed.
    /// </summary>
    /// <typeparam name="T">Type of the objects kept in the cache.</typeparam>
    public class Cache<T> : ICache<T>
    {
        /// <summary>
        /// Instantiates the cache with a default cache storage.
        /// </summary>
        public Cache()
            : this(new CacheStorage<T>())
        {
        }

        /// <summary>
        /// Instantiates the cache with the provided cache storage.
        /// </summary>
        /// <param name="storage">The cache storage.</param>
        public Cache(ICacheStorage<T> storage)
        {
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        /// <summary>
        /// Internally visible cache storage used to optimize composed caches.
        /// </summary>
        internal ICacheStorage<T> Storage { get; private set; }

        /// <summary>
        /// Deconstructs the instance into cacheable and non-cacheable
        /// components, caches the cacheable component, and returns a handle
        /// that can be used to reconstruct the original instance.
        /// </summary>
        /// <param name="value">The instance to cache.</param>
        /// <returns>A handle to reconstruct the original instance.</returns>
        public virtual IDiscardable<T> Create(T value)
        {
            if (value == null)
            {
                return DefaultDiscardable<T>.Instance;
            }

            var entry = Storage.GetEntry(value);
            if (entry == null)
            {
                throw new InvalidOperationException("Unexpected null entry returned from cache storage.");
            }

            return new CacheReference(entry, this);
        }

        private sealed class CacheReference : DiscardableBase<T>
        {
            private readonly Cache<T> _cache;
            private IReference<T> _entry;

            public CacheReference(IReference<T> entry, Cache<T> cache)
            {
                _entry = entry;
                _cache = cache;
            }

            public override T Value
            {
                get
                {
                    var entry = _entry;
                    if (entry == null)
                    {
                        throw new ObjectDisposedException("this");
                    }

                    return entry.Value;
                }
            }

            protected override void Dispose(bool disposing)
            {
                var entry = Interlocked.Exchange(ref _entry, null);
                if (entry != null)
                {
                    _cache.Storage.ReleaseEntry(entry);
                }
            }
        }
    }

    /// <summary>
    /// Utility for deconstructing objects into cacheable and non-cacheable
    /// components and sharing the parts that are cacheable.
    /// </summary>
    /// <typeparam name="T">Type of the objects kept in the cache.</typeparam>
    /// <typeparam name="TCached">Type of the cacheable component of the cached type.</typeparam>
    /// <typeparam name="TNonCached">Type of the non-cacheable component of the cached type.</typeparam>
    public abstract class Cache<T, TCached, TNonCached> : ICache<T>
    {
        private readonly ICache<TCached> _innerCache;

        /// <summary>
        /// Instantiates the cache with a default cache policy and equality comparer.
        /// </summary>
        protected Cache()
            : this(new Cache<TCached>())
        {
        }

        /// <summary>
        /// Instantiates the cache with the cache to use for cached component.
        /// </summary>
        /// <param name="innerCache">The inner cache.</param>
        protected Cache(ICache<TCached> innerCache)
        {
            _innerCache = innerCache ?? throw new ArgumentNullException(nameof(innerCache));
        }

        /// <summary>
        /// Deconstructs the instance into cacheable and non-cacheable
        /// components, caches the cacheable component, and returns a handle
        /// that can be used to reconstruct the original instance.
        /// </summary>
        /// <param name="value">The instance to cache.</param>
        /// <returns>A handle to reconstruct the original instance.</returns>
        public IDiscardable<T> Create(T value)
        {
            if (value == null)
            {
                return DefaultDiscardable<T>.Instance;
            }

            var deconstructed = Deconstruct(value);
            var entry = _innerCache.CreateOrGetEntry(deconstructed.Cached);

            return new CacheReference(entry, deconstructed.NonCached, this);
        }

        /// <summary>
        /// Deconstructs an instance of the cached type into its cacheable and non-cacheable components.
        /// </summary>
        /// <param name="value">The instance to cache.</param>
        /// <returns>The deconstructed instance.</returns>
        protected abstract Deconstructed<TCached, TNonCached> Deconstruct(T value);

        /// <summary>
        /// Reconstructs an instance of the cached type from its cacheable and non-cacheable components.
        /// </summary>
        /// <param name="deconstructed">The deconstructed instance.</param>
        /// <returns>The reconstructed instance.</returns>
        protected abstract T Reconstruct(Deconstructed<TCached, TNonCached> deconstructed);

        private sealed class CacheReference : DiscardableBase<T>
        {
            private readonly TNonCached _nonCached;
            private readonly Cache<T, TCached, TNonCached> _cache;
            private IReference<TCached> _cached;

            public CacheReference(
                IReference<TCached> cached,
                TNonCached nonCached,
                Cache<T, TCached, TNonCached> cache)
            {
                _cached = cached;
                _nonCached = nonCached;
                _cache = cache;
            }

            public override T Value
            {
                get
                {
                    var cached = _cached;
                    if (cached == null)
                    {
                        throw new ObjectDisposedException("this");
                    }

                    var deconstructed = Deconstructed.Create(cached.Value, _nonCached);
                    return _cache.Reconstruct(deconstructed);
                }
            }

            protected override void Dispose(bool disposing)
            {
                var cached = Interlocked.Exchange(ref _cached, null);
                if (cached != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache.ReleaseOrDispose(cached);
                    }
                    else
                    {
                        _cache._innerCache.ReleaseIfEntry(cached);
                    }
                }
            }
        }
    }
}
