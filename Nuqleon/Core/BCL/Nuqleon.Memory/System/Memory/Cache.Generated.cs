// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;

namespace System.Memory
{
    /// <summary>
    /// Utility for deconstructing objects into cacheable and non-cacheable
    /// components and sharing the parts that are cacheable.
    /// </summary>
    /// <typeparam name="T">Type of the objects kept in the cache.</typeparam>
    /// <typeparam name="TCached1">Type of the first cacheable component of the cached type.</typeparam>
    /// <typeparam name="TCached2">Type of the second cacheable component of the cached type.</typeparam>
    /// <typeparam name="TNonCached">Type of the non-cacheable component of the cached type.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "By design to support arbitrary breakdowns of types for caching.")]
    public abstract class Cache<T, TCached1, TCached2, TNonCached> : ICache<T>
    {
        private readonly ICache<TCached1> _innerCache1;
        private readonly ICache<TCached2> _innerCache2;

        /// <summary>
        /// Instantiates the cache with a default cache policies and equality comparers.
        /// </summary>
        protected Cache()
            : this(new Cache<TCached1>(), new Cache<TCached2>())
        {
        }

        /// <summary>
        /// Instantiates the cache with the caches to use for cached components.
        /// </summary>
        /// <param name="innerCache1">The first inner cache.</param>
        /// <param name="innerCache2">The second inner cache.</param>
        protected Cache(ICache<TCached1> innerCache1, ICache<TCached2> innerCache2)
        {
            if (innerCache1 == null)
            {
                throw new ArgumentNullException(nameof(innerCache1));
            }

            if (innerCache2 == null)
            {
                throw new ArgumentNullException(nameof(innerCache2));
            }

            _innerCache1 = innerCache1;
            _innerCache2 = innerCache2;
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
            var entry1 = _innerCache1.CreateOrGetEntry(deconstructed.Cached1);
            var entry2 = _innerCache2.CreateOrGetEntry(deconstructed.Cached2);

            return new CacheReference(entry1, entry2, deconstructed.NonCached, this);
        }

        /// <summary>
        /// Deconstructs an instance of the cached type into its cacheable and non-cacheable components.
        /// </summary>
        /// <param name="value">The instance to cache.</param>
        /// <returns>The deconstructed instance.</returns>
        protected abstract Deconstructed<TCached1, TCached2, TNonCached> Deconstruct(T value);

        /// <summary>
        /// Reconstructs an instance of the cached type from its cacheable and non-cacheable components.
        /// </summary>
        /// <param name="deconstructed">The deconstructed instance.</param>
        /// <returns>The reconstructed instance.</returns>
        protected abstract T Reconstruct(Deconstructed<TCached1, TCached2, TNonCached> deconstructed);

        private sealed class CacheReference : DiscardableBase<T>
        {
            private readonly TNonCached _nonCached;
            private readonly Cache<T, TCached1, TCached2, TNonCached> _cache;
            private IReference<TCached1> _cached1;
            private IReference<TCached2> _cached2;

            public CacheReference(
                IReference<TCached1> cached1,
                IReference<TCached2> cached2,
                TNonCached nonCached,
                Cache<T, TCached1, TCached2, TNonCached> cache)
            {
                _cached1 = cached1;
                _cached2 = cached2;
                _nonCached = nonCached;
                _cache = cache;
            }

            public override T Value
            {
                get
                {
                    var cached1 = _cached1;
                    var cached2 = _cached2;
                    if (cached1 == null || cached2 == null)
                    {
                        throw new ObjectDisposedException("this");
                    }

                    var deconstructed = Deconstructed.Create(cached1.Value, cached2.Value, _nonCached);
                    return _cache.Reconstruct(deconstructed);
                }
            }

            protected override void Dispose(bool disposing)
            {
                var cached1 = Interlocked.Exchange(ref _cached1, null);
                if (cached1 != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache1.ReleaseOrDispose(cached1);
                    }
                    else
                    {
                        _cache._innerCache1.ReleaseIfEntry(cached1);
                    }
                }

                var cached2 = Interlocked.Exchange(ref _cached2, null);
                if (cached2 != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache2.ReleaseOrDispose(cached2);
                    }
                    else
                    {
                        _cache._innerCache2.ReleaseIfEntry(cached2);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Utility for deconstructing objects into cacheable and non-cacheable
    /// components and sharing the parts that are cacheable.
    /// </summary>
    /// <typeparam name="T">Type of the objects kept in the cache.</typeparam>
    /// <typeparam name="TCached1">Type of the first cacheable component of the cached type.</typeparam>
    /// <typeparam name="TCached2">Type of the second cacheable component of the cached type.</typeparam>
    /// <typeparam name="TCached3">Type of the third cacheable component of the cached type.</typeparam>
    /// <typeparam name="TNonCached">Type of the non-cacheable component of the cached type.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "By design to support arbitrary breakdowns of types for caching.")]
    public abstract class Cache<T, TCached1, TCached2, TCached3, TNonCached> : ICache<T>
    {
        private readonly ICache<TCached1> _innerCache1;
        private readonly ICache<TCached2> _innerCache2;
        private readonly ICache<TCached3> _innerCache3;

        /// <summary>
        /// Instantiates the cache with a default cache policies and equality comparers.
        /// </summary>
        protected Cache()
            : this(new Cache<TCached1>(), new Cache<TCached2>(), new Cache<TCached3>())
        {
        }

        /// <summary>
        /// Instantiates the cache with the caches to use for cached components.
        /// </summary>
        /// <param name="innerCache1">The first inner cache.</param>
        /// <param name="innerCache2">The second inner cache.</param>
        /// <param name="innerCache3">The third inner cache.</param>
        protected Cache(ICache<TCached1> innerCache1, ICache<TCached2> innerCache2, ICache<TCached3> innerCache3)
        {
            if (innerCache1 == null)
            {
                throw new ArgumentNullException(nameof(innerCache1));
            }

            if (innerCache2 == null)
            {
                throw new ArgumentNullException(nameof(innerCache2));
            }

            if (innerCache3 == null)
            {
                throw new ArgumentNullException(nameof(innerCache3));
            }

            _innerCache1 = innerCache1;
            _innerCache2 = innerCache2;
            _innerCache3 = innerCache3;
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
            var entry1 = _innerCache1.CreateOrGetEntry(deconstructed.Cached1);
            var entry2 = _innerCache2.CreateOrGetEntry(deconstructed.Cached2);
            var entry3 = _innerCache3.CreateOrGetEntry(deconstructed.Cached3);

            return new CacheReference(entry1, entry2, entry3, deconstructed.NonCached, this);
        }

        /// <summary>
        /// Deconstructs an instance of the cached type into its cacheable and non-cacheable components.
        /// </summary>
        /// <param name="value">The instance to cache.</param>
        /// <returns>The deconstructed instance.</returns>
        protected abstract Deconstructed<TCached1, TCached2, TCached3, TNonCached> Deconstruct(T value);

        /// <summary>
        /// Reconstructs an instance of the cached type from its cacheable and non-cacheable components.
        /// </summary>
        /// <param name="deconstructed">The deconstructed instance.</param>
        /// <returns>The reconstructed instance.</returns>
        protected abstract T Reconstruct(Deconstructed<TCached1, TCached2, TCached3, TNonCached> deconstructed);

        private sealed class CacheReference : DiscardableBase<T>
        {
            private readonly TNonCached _nonCached;
            private readonly Cache<T, TCached1, TCached2, TCached3, TNonCached> _cache;
            private IReference<TCached1> _cached1;
            private IReference<TCached2> _cached2;
            private IReference<TCached3> _cached3;

            public CacheReference(
                IReference<TCached1> cached1,
                IReference<TCached2> cached2,
                IReference<TCached3> cached3,
                TNonCached nonCached,
                Cache<T, TCached1, TCached2, TCached3, TNonCached> cache)
            {
                _cached1 = cached1;
                _cached2 = cached2;
                _cached3 = cached3;
                _nonCached = nonCached;
                _cache = cache;
            }

            public override T Value
            {
                get
                {
                    var cached1 = _cached1;
                    var cached2 = _cached2;
                    var cached3 = _cached3;
                    if (cached1 == null || cached2 == null || cached3 == null)
                    {
                        throw new ObjectDisposedException("this");
                    }

                    var deconstructed = Deconstructed.Create(cached1.Value, cached2.Value, cached3.Value, _nonCached);
                    return _cache.Reconstruct(deconstructed);
                }
            }

            protected override void Dispose(bool disposing)
            {
                var cached1 = Interlocked.Exchange(ref _cached1, null);
                if (cached1 != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache1.ReleaseOrDispose(cached1);
                    }
                    else
                    {
                        _cache._innerCache1.ReleaseIfEntry(cached1);
                    }
                }

                var cached2 = Interlocked.Exchange(ref _cached2, null);
                if (cached2 != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache2.ReleaseOrDispose(cached2);
                    }
                    else
                    {
                        _cache._innerCache2.ReleaseIfEntry(cached2);
                    }
                }

                var cached3 = Interlocked.Exchange(ref _cached3, null);
                if (cached3 != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache3.ReleaseOrDispose(cached3);
                    }
                    else
                    {
                        _cache._innerCache3.ReleaseIfEntry(cached3);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Utility for deconstructing objects into cacheable and non-cacheable
    /// components and sharing the parts that are cacheable.
    /// </summary>
    /// <typeparam name="T">Type of the objects kept in the cache.</typeparam>
    /// <typeparam name="TCached1">Type of the first cacheable component of the cached type.</typeparam>
    /// <typeparam name="TCached2">Type of the second cacheable component of the cached type.</typeparam>
    /// <typeparam name="TCached3">Type of the third cacheable component of the cached type.</typeparam>
    /// <typeparam name="TCached4">Type of the fourth cacheable component of the cached type.</typeparam>
    /// <typeparam name="TNonCached">Type of the non-cacheable component of the cached type.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "By design to support arbitrary breakdowns of types for caching.")]
    public abstract class Cache<T, TCached1, TCached2, TCached3, TCached4, TNonCached> : ICache<T>
    {
        private readonly ICache<TCached1> _innerCache1;
        private readonly ICache<TCached2> _innerCache2;
        private readonly ICache<TCached3> _innerCache3;
        private readonly ICache<TCached4> _innerCache4;

        /// <summary>
        /// Instantiates the cache with a default cache policies and equality comparers.
        /// </summary>
        protected Cache()
            : this(new Cache<TCached1>(), new Cache<TCached2>(), new Cache<TCached3>(), new Cache<TCached4>())
        {
        }

        /// <summary>
        /// Instantiates the cache with the caches to use for cached components.
        /// </summary>
        /// <param name="innerCache1">The first inner cache.</param>
        /// <param name="innerCache2">The second inner cache.</param>
        /// <param name="innerCache3">The third inner cache.</param>
        /// <param name="innerCache4">The fourth inner cache.</param>
        protected Cache(ICache<TCached1> innerCache1, ICache<TCached2> innerCache2, ICache<TCached3> innerCache3, ICache<TCached4> innerCache4)
        {
            if (innerCache1 == null)
            {
                throw new ArgumentNullException(nameof(innerCache1));
            }

            if (innerCache2 == null)
            {
                throw new ArgumentNullException(nameof(innerCache2));
            }

            if (innerCache3 == null)
            {
                throw new ArgumentNullException(nameof(innerCache3));
            }

            if (innerCache4 == null)
            {
                throw new ArgumentNullException(nameof(innerCache4));
            }

            _innerCache1 = innerCache1;
            _innerCache2 = innerCache2;
            _innerCache3 = innerCache3;
            _innerCache4 = innerCache4;
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
            var entry1 = _innerCache1.CreateOrGetEntry(deconstructed.Cached1);
            var entry2 = _innerCache2.CreateOrGetEntry(deconstructed.Cached2);
            var entry3 = _innerCache3.CreateOrGetEntry(deconstructed.Cached3);
            var entry4 = _innerCache4.CreateOrGetEntry(deconstructed.Cached4);

            return new CacheReference(entry1, entry2, entry3, entry4, deconstructed.NonCached, this);
        }

        /// <summary>
        /// Deconstructs an instance of the cached type into its cacheable and non-cacheable components.
        /// </summary>
        /// <param name="value">The instance to cache.</param>
        /// <returns>The deconstructed instance.</returns>
        protected abstract Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached> Deconstruct(T value);

        /// <summary>
        /// Reconstructs an instance of the cached type from its cacheable and non-cacheable components.
        /// </summary>
        /// <param name="deconstructed">The deconstructed instance.</param>
        /// <returns>The reconstructed instance.</returns>
        protected abstract T Reconstruct(Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached> deconstructed);

        private sealed class CacheReference : DiscardableBase<T>
        {
            private readonly TNonCached _nonCached;
            private readonly Cache<T, TCached1, TCached2, TCached3, TCached4, TNonCached> _cache;
            private IReference<TCached1> _cached1;
            private IReference<TCached2> _cached2;
            private IReference<TCached3> _cached3;
            private IReference<TCached4> _cached4;

            public CacheReference(
                IReference<TCached1> cached1,
                IReference<TCached2> cached2,
                IReference<TCached3> cached3,
                IReference<TCached4> cached4,
                TNonCached nonCached,
                Cache<T, TCached1, TCached2, TCached3, TCached4, TNonCached> cache)
            {
                _cached1 = cached1;
                _cached2 = cached2;
                _cached3 = cached3;
                _cached4 = cached4;
                _nonCached = nonCached;
                _cache = cache;
            }

            public override T Value
            {
                get
                {
                    var cached1 = _cached1;
                    var cached2 = _cached2;
                    var cached3 = _cached3;
                    var cached4 = _cached4;
                    if (cached1 == null || cached2 == null || cached3 == null || cached4 == null)
                    {
                        throw new ObjectDisposedException("this");
                    }

                    var deconstructed = Deconstructed.Create(cached1.Value, cached2.Value, cached3.Value, cached4.Value, _nonCached);
                    return _cache.Reconstruct(deconstructed);
                }
            }

            protected override void Dispose(bool disposing)
            {
                var cached1 = Interlocked.Exchange(ref _cached1, null);
                if (cached1 != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache1.ReleaseOrDispose(cached1);
                    }
                    else
                    {
                        _cache._innerCache1.ReleaseIfEntry(cached1);
                    }
                }

                var cached2 = Interlocked.Exchange(ref _cached2, null);
                if (cached2 != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache2.ReleaseOrDispose(cached2);
                    }
                    else
                    {
                        _cache._innerCache2.ReleaseIfEntry(cached2);
                    }
                }

                var cached3 = Interlocked.Exchange(ref _cached3, null);
                if (cached3 != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache3.ReleaseOrDispose(cached3);
                    }
                    else
                    {
                        _cache._innerCache3.ReleaseIfEntry(cached3);
                    }
                }

                var cached4 = Interlocked.Exchange(ref _cached4, null);
                if (cached4 != null)
                {
                    if (disposing)
                    {
                        _cache._innerCache4.ReleaseOrDispose(cached4);
                    }
                    else
                    {
                        _cache._innerCache4.ReleaseIfEntry(cached4);
                    }
                }
            }
        }
    }

}
