// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

#if DEBUG
using System.Diagnostics;
#endif

namespace System.Memory
{
    public partial class WeakMemoizationCacheFactory
    {
        /// <summary>
        /// Implementation of a factory for weak memoization caches with unbounded storage.
        /// </summary>
        private sealed class UnboundedImpl : IWeakMemoizationCacheFactory
        {
            /// <summary>
            /// Creates a memoization cache for the specified <paramref name="function"/> that doesn't keep cache entry keys alive.
            /// </summary>
            /// <typeparam name="T">Type of the memoization cache entry keys. This type has to be a reference type.</typeparam>
            /// <typeparam name="R">Type of the memoization cache entry values.</typeparam>
            /// <param name="function">The function to memoize.</param>
            /// <param name="options">Flags to influence the memoization behavior.</param>
            /// <returns>An empty memoization cache instance.</returns>
            public IMemoizationCache<T, R> Create<T, R>(Func<T, R> function, MemoizationOptions options) where T : class
            {
                if (function == null)
                    throw new ArgumentNullException(nameof(function));

                if ((options & MemoizationOptions.CacheException) > MemoizationOptions.None)
                {
                    return new CacheWithException<T, R>(function);
                }
                else
                {
                    return new Cache<T, R>(function);
                }
            }


            //
            // NB: No trimming support because we don't have a way to enumerate the cache entries due to CWT's design.
            //

            /// <summary>
            /// Base class for weak caches.
            /// </summary>
            /// <typeparam name="T">Type of the memoization cache entry keys. This type has to be a reference type.</typeparam>
            /// <typeparam name="R">Type of the memoization cache entry values.</typeparam>
            private abstract class CacheBase<T, R> : MemoizationCacheBase<T, R>
                where T : class
            {
                protected volatile int _count;
#if DEBUG
                protected volatile int _accessCount;
                protected volatile int _hitCount;

                protected long _invocationTicks;
                protected long _lookupTicks;
#endif

                protected override int CountCore => _count;

                [ExcludeFromCodeCoverage]
                protected override string DebugViewCore
                {
                    get
                    {
                        var sb = new StringBuilder();

                        sb.AppendLine("Number of entries = " + _count);
#if DEBUG
                        var avgInvocationTime = _count > 0 ? new TimeSpan(_invocationTicks / _count) : TimeSpan.Zero;
                        var avgLookupTime = _hitCount > 0 ? new TimeSpan(_lookupTicks / _hitCount) : TimeSpan.Zero;
                        var speedUp = ((double)_invocationTicks / _count) / ((double)_lookupTicks / _hitCount);

                        sb.AppendLine();
                        sb.AppendLine("Statistics");
                        sb.AppendLine("----------");
                        sb.AppendLine();
                        sb.AppendLine("  Access count          = " + _accessCount);
                        sb.AppendLine("  Cache hit count       = " + _hitCount);
                        sb.AppendLine("  Total invocation time = " + new TimeSpan(_invocationTicks));
                        sb.AppendLine("  Avg invocation time   = " + avgInvocationTime);
                        sb.AppendLine("  Total lookup time     = " + new TimeSpan(_lookupTicks));
                        sb.AppendLine("  Avg lookup time       = " + avgLookupTime);
                        sb.AppendLine("  Avg speedup factor    = " + speedUp + (speedUp < 1 ? " [DEGRADATION]" : ""));
#endif
                        return sb.ToString();
                    }
                }

                protected override void ClearCore(bool disposing)
                {
                    _count = 0;
                }
            }

            /// <summary>
            /// Weak cache that doesn't cache exceptions.
            /// </summary>
            /// <typeparam name="T">Type of the memoization cache entry keys. This type has to be a reference type.</typeparam>
            /// <typeparam name="R">Type of the memoization cache entry values.</typeparam>
            private sealed class Cache<T, R> : CacheBase<T, R>
                where T : class
            {
                private volatile IWeakCacheDictionary<T, object> _cache;
#if DEBUG
                private readonly ThreadLocal<bool> _isNew; // assumes the function is not recursive
#endif
                private readonly ConditionalWeakTable<T, object>.CreateValueCallback _function;

                public Cache(Func<T, R> function)
                {
                    _cache = new WeakCacheDictionary<T, object>();
#if DEBUG
                    _isNew = new ThreadLocal<bool>();
#endif
                    _function = args =>
                    {
#if DEBUG
                        var swInvoke = Stopwatch.StartNew();
                        _isNew.Value = true;
#endif
                        var value = function(args);
#if DEBUG
                        Interlocked.Add(ref _invocationTicks, swInvoke.ElapsedTicks);
#endif
                        Interlocked.Increment(ref _count);

                        return value;
                    };
                }

                protected override R GetOrAddCore(T args)
                {
#if DEBUG
                    Interlocked.Increment(ref _accessCount);

                    var swTotal = Stopwatch.StartNew();
#endif
                    var res = (R)_cache.GetOrAdd(args, _function);
#if DEBUG
                    if (!_isNew.Value)
                    {
                        Interlocked.Add(ref _lookupTicks, swTotal.ElapsedTicks);
                        Interlocked.Increment(ref _hitCount);
                    }

                    _isNew.Value = false;
#endif
                    return res;
                }

                protected override void ClearCore(bool disposing)
                {
                    base.ClearCore(disposing);

                    //
                    // Unfortunately, CWT does not expose its Clear method publicly.
                    //
                    _cache = new WeakCacheDictionary<T, object>();
                }
            }

            /// <summary>
            /// Weak cache that caches exceptions.
            /// </summary>
            /// <typeparam name="T">Type of the memoization cache entry keys. This type has to be a reference type.</typeparam>
            /// <typeparam name="R">Type of the memoization cache entry values.</typeparam>
            private sealed class CacheWithException<T, R> : CacheBase<T, R>
                where T : class
            {
                private volatile IWeakCacheDictionary<T, IValueOrError<R>> _cache;
#if DEBUG
                private readonly ThreadLocal<bool> _isNew; // assumes the function is not recursive
#endif
                private readonly ConditionalWeakTable<T, IValueOrError<R>>.CreateValueCallback _function;

                public CacheWithException(Func<T, R> function)
                {
                    _cache = new WeakCacheDictionary<T, IValueOrError<R>>();
#if DEBUG
                    _isNew = new ThreadLocal<bool>();
#endif
                    _function = args =>
                    {
#if DEBUG
                        _isNew.Value = true;
                        var swInvoke = Stopwatch.StartNew();
#endif
                        var value = default(IValueOrError<R>);

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1031 // Do not catch general exception types (by design)

                        try
                        {
                            value = ValueOrError.CreateValue(function(args));
                        }
                        catch (Exception ex)
                        {
                            value = ValueOrError.CreateError<R>(ex);
                        }

#pragma warning restore CA1031
#pragma warning restore IDE0079

#if DEBUG
                        Interlocked.Add(ref _invocationTicks, swInvoke.ElapsedTicks);
#endif
                        Interlocked.Increment(ref _count);

                        return value;
                    };
                }

                protected override R GetOrAddCore(T args)
                {
#if DEBUG
                    Interlocked.Increment(ref _accessCount);

                    var swTotal = Stopwatch.StartNew();
#endif
                    //
                    // NB: CWT does not call the function under its internal lock.
                    //
                    var res = _cache.GetOrAdd(args, _function);
#if DEBUG
                    try
#endif
                    {
                        return res.Value;
                    }
#if DEBUG
                    finally
                    {
                        if (!_isNew.Value)
                        {
                            Interlocked.Add(ref _lookupTicks, swTotal.ElapsedTicks);
                            Interlocked.Increment(ref _hitCount);
                        }

                        _isNew.Value = false;
                    }
#endif
                }

                protected override void ClearCore(bool disposing)
                {
                    base.ClearCore(disposing);

                    //
                    // Unfortunately, CWT does not expose its Clear method publicly.
                    //
                    _cache = new WeakCacheDictionary<T, IValueOrError<R>>();
                }
            }
        }
    }
}
