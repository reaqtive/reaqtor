// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace System.Memory
{
    /// <summary>
    /// Extension methods for IMemoizationCacheFactory.
    /// </summary>
    public static class MemoizationCacheFactoryExtensions
    {
        /// <summary>
        /// Creates a memoization cache factory that keeps function memoization caches for each thread on which the memoized function gets invoked.
        /// This is useful to reduce cache access lock contention and can be used to make a memoization cache safe for concurrent access, at the expense of keeping a cache per thread.
        /// </summary>
        /// <param name="factory">The memoization cache factory to wrap with thread-local caching behavior.</param>
        /// <returns>A memoization cache factory that wraps the specified <paramref name="factory"/> and adds thread-local isolation to it.</returns>
        public static IMemoizationCacheFactory WithThreadLocal(this IMemoizationCacheFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new ThreadLocalFactory(factory);
        }

        /// <summary>
        /// Creates a memoization cache factory that keeps function memoization caches for each thread on which the memoized function gets invoked.
        /// This is useful to reduce cache access lock contention and can be used to make a memoization cache safe for concurrent access, at the expense of keeping a cache per thread.
        /// </summary>
        /// <param name="factory">The memoization cache factory to wrap with thread-local caching behavior.</param>
        /// <param name="exposeThreadLocalView">Indicates whether the caches returned from the resulting factory provide a thread-local view on the cache, for properties like Count and methods like Clear.</param>
        /// <returns>A memoization cache factory that wraps the specified <paramref name="factory"/> and adds thread-local isolation to it.</returns>
        public static IMemoizationCacheFactory WithThreadLocal(this IMemoizationCacheFactory factory, bool exposeThreadLocalView)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new ThreadLocalFactory(factory, exposeThreadLocalView);
        }

        /// <summary>
        /// Creates a memoization cache factory that uses locking to access function memoization caches.
        /// This can be used to make a memoization cache safe for concurrent access, at the expense of potential lock contention.
        /// </summary>
        /// <param name="factory">The memoization cache factory to wrap with synchronized access behavior.</param>
        /// <returns>A memoization cache factory that wraps the specified <paramref name="factory"/> and adds synchronized access behavior to it.</returns>
        public static IMemoizationCacheFactory Synchronized(this IMemoizationCacheFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new SynchronizedFactory(factory);
        }

        internal class ThreadLocalFactory : IMemoizationCacheFactory
        {
            private readonly IMemoizationCacheFactory _factory;

            public ThreadLocalFactory(IMemoizationCacheFactory factory) => _factory = factory;

            private readonly bool _exposeGlobalView;

            public ThreadLocalFactory(IMemoizationCacheFactory factory, bool exposeThreadLocalView)
            {
                _factory = factory;
                _exposeGlobalView = !exposeThreadLocalView;
            }

            public IMemoizationCache<T, R> Create<T, R>(Func<T, R> function, MemoizationOptions options, IEqualityComparer<T> comparer)
            {
                if (function == null)
                    throw new ArgumentNullException(nameof(function));

                if (_exposeGlobalView)
                {
                    return new ImplUnion<T, R>(() => _factory.Create(function, options, comparer));
                }

                return new Impl<T, R>(() => _factory.Create(function, options, comparer));
            }

            internal abstract class ImplBase<T, R> : MemoizationCacheBase<T, R>, IServiceProvider
            {
#pragma warning disable CA2213 // "never disposed." Analyzer hasn't understood DisposeCore
                protected readonly ThreadLocal<IMemoizationCache<T, R>> _cache;
#pragma warning restore CA2213, IDE0079

                public ImplBase(Func<IMemoizationCache<T, R>> factory) => _cache = new ThreadLocal<IMemoizationCache<T, R>>(factory, trackAllValues: true);

                protected override R GetOrAddCore(T argument) => _cache.Value.GetOrAdd(argument);

                protected override void DisposeCore()
                {
                    foreach (var value in _cache.Values)
                    {
                        value.Dispose();
                    }

                    _cache.Dispose();
                }

                public abstract object GetService(Type serviceType);
            }

            internal class Impl<T, R> : ImplBase<T, R>
            {
                public Impl(Func<IMemoizationCache<T, R>> factory)
                    : base(factory)
                {
                }

                protected override int CountCore => _cache.Value.Count;

                protected override string DebugViewCore => _cache.Value.DebugView;

                protected override void ClearCore(bool disposing) => _cache.Value.Clear();

                public override object GetService(Type serviceType) => _cache.Value.GetService(serviceType);
            }

            internal class ImplUnion<T, R> : ImplBase<T, R>
            {
                public ImplUnion(Func<IMemoizationCache<T, R>> factory)
                    : base(factory)
                {
                }

                protected override int CountCore => _cache.Values.Select(v => v.Count).Sum();

                protected override string DebugViewCore
                {
                    get
                    {
                        var sb = new StringBuilder();

                        var i = 0;
                        foreach (var value in _cache.Values)
                        {
                            sb.AppendLine("Thread-local cache " + i++);
                            sb.AppendLine();
                            sb.AppendLine(value.DebugView);
                        }

                        return sb.ToString();
                    }
                }

                protected override void ClearCore(bool disposing)
                {
                    foreach (var value in _cache.Values)
                    {
                        value.Clear();
                    }
                }

                public override object GetService(Type serviceType)
                {
                    var res = default(object);

                    if (serviceType != null && serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(ITrimmable<>))
                    {
                        var union = typeof(UnionTrimmable<>).MakeGenericType(serviceType.GetGenericArguments());
                        res = Activator.CreateInstance(union, new object[] { Values });
                    }

                    return res;
                }

                private IEnumerable Values
                {
                    get
                    {
                        foreach (var value in _cache.Values)
                        {
                            yield return value;
                        }
                    }
                }
            }

            private sealed class UnionTrimmable<T> : ITrimmable<T>
            {
                private readonly IEnumerable _children;

                public UnionTrimmable(IEnumerable children) => _children = children;

                public int Trim(Func<T, bool> shouldTrim)
                {
                    var res = 0;

                    foreach (var child in _children)
                    {
                        var trimmable = child.GetService<ITrimmable<T>>();
                        if (trimmable != null)
                        {
                            res += trimmable.Trim(shouldTrim);
                        }
                    }

                    return res;
                }
            }
        }

        private sealed class SynchronizedFactory : IMemoizationCacheFactory
        {
            private readonly IMemoizationCacheFactory _factory;

            public SynchronizedFactory(IMemoizationCacheFactory factory) => _factory = factory;

            public IMemoizationCache<T, R> Create<T, R>(Func<T, R> function, MemoizationOptions options, IEqualityComparer<T> comparer)
            {
                if (function == null)
                    throw new ArgumentNullException(nameof(function));

                var gate = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

                var cache = _factory.Create(x =>
                {
                    Debug.Assert(gate.IsWriteLockHeld);

                    //
                    // NB: We downgrade the lock when executing the function. See GetOrAddCode methods.
                    //
                    gate.ExitWriteLock();
                    try
                    {
                        return function(x);
                    }
                    finally
                    {
                        gate.EnterWriteLock();
                    }
                }, options, comparer);

                return new Impl<T, R>(cache, gate);
            }

            private sealed class Impl<T, R> : MemoizationCacheBase<T, R>, IServiceProvider
            {
                private readonly IMemoizationCache<T, R> _cache;
                private readonly ReaderWriterLockSlim _gate;

                public Impl(IMemoizationCache<T, R> cache, ReaderWriterLockSlim gate)
                {
                    _cache = cache;
                    _gate = gate;
                }

                protected override int CountCore
                {
                    get
                    {
                        _gate.EnterReadLock();
                        try
                        {
                            return _cache.Count;
                        }
                        finally
                        {
                            _gate.ExitReadLock();
                        }
                    }
                }

                protected override string DebugViewCore
                {
                    get
                    {
                        _gate.EnterReadLock();
                        try
                        {
                            return _cache.DebugView;
                        }
                        finally
                        {
                            _gate.ExitReadLock();
                        }
                    }
                }

                protected override void ClearCore(bool disposing)
                {
                    _gate.EnterWriteLock();
                    try
                    {
                        _cache.Clear();
                    }
                    finally
                    {
                        _gate.ExitWriteLock();
                    }
                }

                protected override R GetOrAddCore(T argument)
                {
                    //
                    // NB: Ideally, this would be an upgradeable lock. However, it doesn't compose well from the outside
                    //     given the underlying collections that are being used. See other NB on limiting the lock scope.
                    //

                    _gate.EnterWriteLock();
                    try
                    {
                        //
                        // NB: We downgrade the lock when executing the function if no existing entry is found.
                        //
                        return _cache.GetOrAdd(argument);
                    }
                    finally
                    {
                        _gate.ExitWriteLock();
                    }
                }

                public object GetService(Type serviceType)
                {
                    var res = default(object);

                    if (serviceType != null && serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(ITrimmable<>))
                    {
                        res = _cache.GetService(serviceType);

                        if (res != null)
                        {
                            var synchronized = typeof(SynchronizedTrimmable<>).MakeGenericType(serviceType.GetGenericArguments());
                            res = Activator.CreateInstance(synchronized, new object[] { res, _gate });
                        }
                    }

                    return res;
                }
            }

            private sealed class SynchronizedTrimmable<T> : ITrimmable<T>
            {
                private readonly ITrimmable<T> _trimmable;
                private readonly ReaderWriterLockSlim _gate;

                public SynchronizedTrimmable(ITrimmable<T> trimmable, ReaderWriterLockSlim gate)
                {
                    _trimmable = trimmable;
                    _gate = gate;
                }

                public int Trim(Func<T, bool> shouldTrim)
                {
                    _gate.EnterWriteLock();
                    try
                    {
                        return _trimmable.Trim(shouldTrim);
                    }
                    finally
                    {
                        _gate.ExitWriteLock();
                    }
                }
            }
        }
    }
}
