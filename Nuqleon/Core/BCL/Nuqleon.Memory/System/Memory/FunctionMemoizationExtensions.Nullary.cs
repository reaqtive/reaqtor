// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

namespace System.Memory
{
    /// <summary>
    /// Provides a set of extension methods for memoization of functions.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Generated code")]
    public static partial class FunctionMemoizationExtensions
    {
        /// <summary>
        /// Memoizes the specified <paramref name="function"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        public static IMemoizedDelegate<Func<TResult>> Memoize<TResult>(this Func<TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            if ((options & MemoizationOptions.CacheException) > MemoizationOptions.None)
            {
                return MemoizeWithError(function);
            }
            else
            {
                return MemoizeWithoutError(function);
            }
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        public static IMemoizedDelegate<Func<TResult>> Memoize<TResult>(this IMemoizer memoizer, Func<TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var res = memoizer.Memoize<object, TResult>(_ => function(), options, comparer: null);
            var del = res.Delegate;
            return new MemoizedDelegate<Func<TResult>>(() => del(null), res.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        public static IMemoizedDelegate<Func<TResult>> MemoizeWeak<TResult>(this IWeakMemoizer memoizer, Func<TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var res = memoizer.MemoizeWeak<object, TResult>(_ => function(), options);
            var del = res.Delegate;
            return new MemoizedDelegate<Func<TResult>>(() => del(null), res.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T">Type of the function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        public static IMemoizedDelegate<Func<T, TResult>> Memoize<T, TResult>(this IMemoizer memoizer, Func<T, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return memoizer.Memoize<T, TResult>(function, options, comparer: null);
        }

        private static IMemoizedDelegate<Func<TResult>> MemoizeWithoutError<TResult>(Func<TResult> f)
        {
            var cache = new WithoutException<TResult>();

            var res = new Func<TResult>(() =>
            {
                return cache.GetOrAdd(f);
            });

            return new MemoizedDelegate<Func<TResult>>(res, cache);
        }

        private static IMemoizedDelegate<Func<TResult>> MemoizeWithError<TResult>(Func<TResult> f)
        {
            var cache = new WithException<TResult>();

            var res = new Func<TResult>(() =>
            {
                return cache.GetOrAdd(f);
            });

            return new MemoizedDelegate<Func<TResult>>(res, cache);
        }

        internal sealed class WithoutException<T> : SynchronizedMemoizationCacheBase<Func<T>, T>
        {
            private readonly object _gate = new();
            private bool _hasValue;
            private T _value;

            protected override object SyncRoot => _gate;

            protected override int CountCore => _hasValue ? 1 : 0;

            protected override string DebugViewCore => "Has value = " + _hasValue;

            public override T GetOrAdd(Func<T> f)
            {
                //
                // NB: We override GetOrAdd in addition to GetOrAddCore to get more control over the
                //     locking, such that we can avoid invoking the function under the lock. It is
                //     assumed the function is pure, so concurrent invocation is fine and should yield
                //     the same result.
                //

                lock (SyncRoot)
                {
                    CheckDisposed();

                    if (_hasValue)
                    {
                        return _value;
                    }
                }

                return GetOrAddCore(f);
            }

            protected override T GetOrAddCore(Func<T> f)
            {
                var value = f();

                lock (SyncRoot)
                {
                    CheckDisposed();

                    if (!_hasValue)
                    {
                        _value = value;
                        _hasValue = true;
                    }

                    return _value;
                }
            }

            protected override void ClearCore(bool disposing)
            {
                _value = default;
                _hasValue = false;
            }
        }

        internal sealed class WithException<T> : SynchronizedMemoizationCacheBase<Func<T>, T>
        {
            private readonly object _gate = new();
            private IValueOrError<T> _value;

            protected override object SyncRoot => _gate;

            protected override int CountCore => _value != null ? 1 : 0;

            protected override string DebugViewCore => "Has value = " + (_value != null);

            public override T GetOrAdd(Func<T> f)
            {
                //
                // NB: We override GetOrAdd in addition to GetOrAddCore to get more control over the
                //     locking, such that we can avoid invoking the function under the lock. It is
                //     assumed the function is pure, so concurrent invocation is fine and should yield
                //     the same result.
                //

                lock (SyncRoot)
                {
                    CheckDisposed();

                    if (_value != null)
                    {
                        return _value.Value;
                    }
                }

                return GetOrAddCore(f);
            }

            protected override T GetOrAddCore(Func<T> f)
            {
                IValueOrError<T> value;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1031 // Do not catch general exception types (by design)

                try
                {
                    value = ValueOrError.CreateValue<T>(f());
                }
                catch (Exception ex)
                {
                    value = ValueOrError.CreateError<T>(ex);
                }

#pragma warning restore CA1031
#pragma warning restore IDE0079

                lock (SyncRoot)
                {
                    CheckDisposed();

                    _value ??= value;

                    return value.Value;
                }
            }

            protected override void ClearCore(bool disposing) => _value = null;
        }
    }
}
