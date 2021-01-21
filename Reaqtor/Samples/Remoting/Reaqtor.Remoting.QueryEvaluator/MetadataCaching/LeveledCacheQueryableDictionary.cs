// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

using Reaqtive.Disposables;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Represents a metadata dictionary with three cache levels.
    /// 1. Immutable resources are kept locally across all uses of the cache.
    /// 2. Storage local to the current call context is consulted.
    /// 3. External metadata is queried for cache misses. The result is stored in call context local storage, or in the local cache depending on the resource's properties.
    /// </summary>
    /// <typeparam name="TMetadataEntity">Type of the entities stored in the metadata dictionary.</typeparam>
    internal class LeveledCacheQueryableDictionary<TMetadataEntity> : IQueryableDictionary<Uri, TMetadataEntity>, IScoped
        where TMetadataEntity : IReactiveResource
    {
        /// <summary>
        /// Default lifetime of cached entities that were retrieved from the external metadata provider.
        /// </summary>
        private static readonly TimeSpan DefaultCacheEntryLifetime = TimeSpan.FromMinutes(30);

        /// <summary>
        /// External metadata dictionary to fall back to for lookups.
        /// </summary>
        private readonly IQueryableDictionary<Uri, TMetadataEntity> _external;

        /// <summary>
        /// Local metadata dictionary used for caching of immutable resources that can be shared safely across users of the metadata dictionary.
        /// </summary>
        /// <remarks>This collection is thread-safe for lookup and add calls.</remarks>
        private readonly CacheDictionary<Uri, TMetadataEntity> _local;

        /// <summary>
        /// Call context local storage for cached metadata. Entries in this cache are valid for the duration of an IReactive service operation.
        /// </summary>
        private readonly CallContextLocal<CallContextLocalCache> _callLocal;

        /// <summary>
        /// Creates a new metadata dictionary using the specified external and local metadata services.
        /// </summary>
        /// <param name="external">
        /// External metadata dictionary to fall back to for lookups.
        /// </param>
        /// <param name="local">
        /// Local metadata dictionary used for caching of immutable resources that can be shared safely across users of the metadata dictionary.
        /// This dictionary should be thread-safe for lookup and add calls.
        /// </param>
        public LeveledCacheQueryableDictionary(IQueryableDictionary<Uri, TMetadataEntity> external, IDictionary<Uri, TMetadataEntity> local)
        {
            _external = external;
            _local = new CacheDictionary<Uri, TMetadataEntity>(local);
            _callLocal = new CallContextLocal<CallContextLocalCache>(() => new CallContextLocalCache());
        }

        #region Unsupported properties

        /// <summary>
        /// Gets the keys in the dictionary.
        /// Not supported due to lazy loading from remote caches.
        /// </summary>
        public IQueryable<Uri> Keys => throw new NotImplementedException();

        /// <summary>
        /// Gets the keys in the dictionary.
        /// Not supported due to lazy loading from remote caches.
        /// </summary>
        IEnumerable<Uri> IReadOnlyDictionary<Uri, TMetadataEntity>.Keys => throw new NotImplementedException();

        /// <summary>
        /// Gets the values in the dictionary.
        /// Not supported due to lazy loading from remote caches.
        /// </summary>
        public IQueryable<TMetadataEntity> Values => throw new NotImplementedException();

        /// <summary>
        /// Gets the values in the dictionary.
        /// Not supported due to lazy loading from remote caches.
        /// </summary>
        IEnumerable<TMetadataEntity> IReadOnlyDictionary<Uri, TMetadataEntity>.Values => throw new NotImplementedException();

        /// <summary>
        /// Gets the type of elements stored in the queryable dictionary.
        /// </summary>
        public Type ElementType => typeof(KeyValuePair<Uri, TMetadataEntity>);

        /// <summary>
        /// Gets the current instance as an expression.
        /// </summary>
        public Expression Expression => Expression.Constant(this);

        /// <summary>
        /// Gets the query provider to formulate queries over the dictionary.
        /// </summary>
        public IQueryProvider Provider => Enumerable.Empty<object>().AsQueryable().Provider;

        /// <summary>
        /// Gets the number of entries in the dictionary.
        /// Not supported due to lazy loading from remote caches.
        /// </summary>
        public int Count => throw new NotImplementedException();

        #endregion

        /// <summary>
        /// Gets the metadata entity with the specified key.
        /// </summary>
        /// <param name="key">URI key identifying the entity to retrieve.</param>
        /// <returns>The metadata entity with the specified key, if found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no entity with the specified URI key was found.</exception>
        public TMetadataEntity this[Uri key]
        {
            get
            {
                if (!TryGetValue(key, out var res))
                {
                    return res;
                }
                else
                {
                    throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "No metadata entity with URI '{0}' was found.", key));
                }
            }
        }

        /// <summary>
        /// Tries to get a metadata entity from the cache.
        /// </summary>
        /// <param name="key">URI key identifying the entity to retrieve.</param>
        /// <param name="value">Upon successful retrieval, contains the retrieved entity.</param>
        /// <returns>true if an entity was found and stored in <paramref name="value"/>; otherwise, false.</returns>
        public bool TryGetValue(Uri key, out TMetadataEntity value)
        {
            return _callLocal.Value.TryGetOrAdd(key, TryLookupLocal, out value);
        }

        /// <summary>
        /// Creates and pushes a new call context local scope.
        /// Recommended usage is to surround code that will interact with metadata in a <c>using</c> statement, reflecting the cache scoping lexically.
        /// </summary>
        /// <returns>Disposable used to pop and clean up the call context scope.</returns>
        public IDisposable CreateScope()
        {
            // Executed for the side-effect of triggering cache creation.
            _ = _callLocal.Value;

            return Disposable.Create(_callLocal.Clear);
        }

        #region Unsupported methods

        /// <summary>
        /// Gets an enumerator to iterate over the entries in the dictionary.
        /// Not supported due to lazy loading from remote caches.
        /// </summary>
        /// <returns>Enumerator to iterate over the entries in the dictionary.</returns>
        public IEnumerator<KeyValuePair<Uri, TMetadataEntity>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an enumerator to iterate over the entries in the dictionary.
        /// Not supported due to lazy loading from remote caches.
        /// </summary>
        /// <returns>Enumerator to iterate over the entries in the dictionary.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether the specified key exists in the dictionary.
        /// Not supported due to lazy loading from remote caches.
        /// </summary>
        /// <param name="key">Key to look up in the dictionary.</param>
        /// <returns>true if an entry with the specified key is found; otherwise, false.</returns>
        public bool ContainsKey(Uri key)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Helper function to determine whether a retrieved entity can be considered for local cross-call caching.
        /// </summary>
        /// <param name="entity">Entity to determine shareability for.</param>
        /// <returns>true if the entity is deemed immutable and can be cached for sharing; otherwise, false.</returns>
        private static bool IsImmutable(TMetadataEntity entity)
        {
            // This is a very simple heuristic that will cause caching of rx://* metadata entities upon their first retrieval.
            // We can refine this policy later, e.g. by discovering all the query operator methods in loaded operator libraries,
            // or by including metadata in the entity to denote whether it's immutable (though that'd have to intersect with
            // various other definition operations to enforce this).
            return entity.Uri.Scheme == "rx";
        }

        /// <summary>
        /// Tries to get a metadata entry from the local cache, falling back to the external storage if it's not found.
        /// </summary>
        /// <param name="key">URI key identifying the entity to retrieve.</param>
        /// <param name="value">Upon successful retrieval, contains the retrieved entity.</param>
        /// <returns>true if an entity was found and stored in <paramref name="value"/>; otherwise, false.</returns>
        private bool TryLookupLocal(Uri key, out TMetadataEntity value)
        {
            if (_local.TryGetValue(key, out value))
            {
                return true;
            }
            else
            {
                if (_external.TryGetValue(key, out value))
                {
                    // To address long latencies with external store lookups, we'll cache the retrieved
                    // entries for a certain duration, assuming these rarely change. This is something
                    // that needs to be revisited later. Ideally, Define* operations performed at the QC
                    // would trickle down to QEs to invalidate caches. Or, we keep these entities in a
                    // performant DC-local store that's much cheaper to read things from.
                    var lifetime = IsImmutable(value) ? TimeSpan.MaxValue : DefaultCacheEntryLifetime;
                    _local.Add(key, value, lifetime);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Call context local metadata cache.
        /// </summary>
        private class CallContextLocalCache
        {
            /// <summary>
            /// Dictionary with cached metadata entries.
            /// </summary>
            private readonly IDictionary<Uri, TMetadataEntity> _cache;

            /// <summary>
            /// Creates a new call context local metadata cache.
            /// </summary>
            public CallContextLocalCache()
            {
                _cache = new Dictionary<Uri, TMetadataEntity>();
            }

            /// <summary>
            /// Tries to get an existing entry from the cache, deferring to the specified value factory in case the entry is not found in the cache.
            /// If a metadata entry is returned from the value factory, it gets cached.
            /// </summary>
            /// <param name="uri">URI of the entity to look up.</param>
            /// <param name="valueFactory">Factory to invoke if no corresponding entry is found in the cache. Upon successful invocation, the result of this function will be cached.</param>
            /// <param name="value">Result of the cache lookup or creation of a new entity value.</param>
            /// <returns>true if an entry was retrieved and stored in <paramref name="value"/>; otherwise, false.</returns>
            public bool TryGetOrAdd(Uri uri, TryFunc<Uri, TMetadataEntity> valueFactory, out TMetadataEntity value)
            {
                lock (_cache)
                {
                    if (_cache.TryGetValue(uri, out value))
                    {
                        return true;
                    }
                }

                if (valueFactory(uri, out var newValue))
                {
                    lock (_cache)
                    {
                        if (!_cache.TryGetValue(uri, out value))
                        {
                            value = newValue;
                            _cache.Add(uri, value);
                        }
                    }

                    return true;
                }

                value = default;
                return false;
            }
        }

        /// <summary>
        /// Dictionary for a cache with time-based expiration of entries.
        /// </summary>
        /// <typeparam name="TKey">Type of the keys in the cache.</typeparam>
        /// <typeparam name="TValue">Type of the values in the cache.</typeparam>
        private class CacheDictionary<TKey, TValue>
        {
            /// <summary>
            /// Store whose entries are subject to an expiration policy.
            /// </summary>
            /// <remarks>
            /// This dictionary should be thread-safe for addition and lookup operations.
            /// </remarks>
            private readonly IDictionary<TKey, TValue> _store;

            /// <summary>
            /// Expirations on a per-key basis.
            /// </summary>
            /// <remarks>
            /// This dictionary should be thread-safe for addition and lookup operations.
            /// </remarks>
            private readonly IDictionary<TKey, Expiration> _expirations;

            /// <summary>
            /// Creates a new dictionary-based cache over the specified external dictionary.
            /// </summary>
            /// <param name="store">Store whose entries are subject to an expiration policy.</param>
            public CacheDictionary(IDictionary<TKey, TValue> store)
            {
                _store = store;
                _expirations = new ConcurrentDictionary<TKey, Expiration>();
            }

            /// <summary>
            /// Tries to retrieve a value with the given key from the cache.
            /// </summary>
            /// <param name="key">Key of the entry to look up.</param>
            /// <param name="value">Value of the entry, if found.</param>
            /// <returns>true if a non-expired entry was found; otherwise, false.</returns>
            public bool TryGetValue(TKey key, out TValue value)
            {
                if (_store.TryGetValue(key, out value))
                {
                    if (_expirations.TryGetValue(key, out var expiration))
                    {
                        if (!expiration.IsExpired)
                        {
                            return true;
                        }
                        else
                        {
                            // There's an okay race here. If we remove an expiration while another
                            // thread has concurrently observed the store entry in TryGetValue but
                            // is on its way to check the expiration, it won't find it and induce
                            // a cache miss. During contention, a few external lookups could then
                            // be made, and entries will eventually be added through Add.
                            _expirations.Remove(key);
                            _store.Remove(key);
                        }
                    }
                }

                value = default;
                return false;
            }

            /// <summary>
            /// Adds an entry to the cache with the specified lifetime used for expiration.
            /// </summary>
            /// <param name="key">Key of the entry to add.</param>
            /// <param name="value">Value of the entry to add.</param>
            /// <param name="lifetime">Lifetime of the entry.</param>
            public void Add(TKey key, TValue value, TimeSpan lifetime)
            {
                // There's an okay race here. If we slot in the new expiration right at the time
                // when TryGetValue has found a store entry and is about to check the expiration,
                // it will return the original item. Next time, the new entry will be picked up.
                _expirations[key] = new Expiration(lifetime);
                _store[key] = value;
            }

            /// <summary>
            /// Expiration tracker for passive caches. This enables checking for expired cache entries upon lookups rather than having active timers to remove expired entries.
            /// </summary>
            private class Expiration
            {
                /// <summary>
                /// Relative time clock to detect reads of expired entries.
                /// </summary>
                private readonly Stopwatch _clock;

                /// <summary>
                /// Lifetime of the entry. If a lookup happens after this time has elapsed from the time it was created, the entry can be removed.
                /// </summary>
                private readonly TimeSpan _lifetime;

                /// <summary>
                /// Creates a new expiration tracker object. Upon creation, the clock tracking the object's lifetime is started.
                /// </summary>
                /// <param name="lifetime">Lifetime of the entry. If a lookup happens after this time has elapsed from the time it was created, the entry can be removed.</param>
                public Expiration(TimeSpan lifetime)
                {
                    _lifetime = lifetime;
                    _clock = Stopwatch.StartNew();
                }

                /// <summary>
                /// Checks whether the entry is expired with regards to its declared lifetime.
                /// </summary>
                public bool IsExpired => _clock.Elapsed > _lifetime;
            }
        }
    }
}
