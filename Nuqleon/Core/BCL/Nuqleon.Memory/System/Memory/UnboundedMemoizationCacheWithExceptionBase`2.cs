// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections.Generic;
using System.Text;

namespace System.Memory
{
    internal class UnboundedMemoizationCacheWithExceptionBase<T, R> : MemoizationCacheBase<T, R>, IServiceProvider
    {
        private readonly Func<T, IValueOrError<R>> _function;
        private readonly ICacheDictionary<T, IValueOrError<R>> _cache;

        public UnboundedMemoizationCacheWithExceptionBase(Func<T, R> function, ICacheDictionary<T, IValueOrError<R>> cache)
        {
            _function = args =>
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1031 // Do not catch general exception types (by design)
                try
                {
                    return ValueOrError.CreateValue<R>(function(args));
                }
                catch (Exception ex)
                {
                    return ValueOrError.CreateError<R>(ex);
                }
#pragma warning restore CA1031
#pragma warning restore IDE0079
            };

            _cache = cache;
        }

        protected override int CountCore => _cache.Count;

        protected override string DebugViewCore
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine("Number of entries = " + _cache.Count);

                return sb.ToString();
            }
        }

        protected override R GetOrAddCore(T args) => _cache.GetOrAdd(args, _function).Value;

        protected override void ClearCore(bool disposing) => _cache.Clear();

        public object GetService(Type serviceType)
        {
            var res = default(object);

            if (serviceType == typeof(ITrimmable<KeyValuePair<T, R>>))
            {
                res = Trimmable.Create<KeyValuePair<T, R>>(shouldTrim => TrimBy(kv => kv.Value.Kind == ValueOrErrorKind.Value, kv => new KeyValuePair<T, R>(kv.Key, kv.Value.Value), shouldTrim));
            }
            else if (serviceType == typeof(ITrimmable<KeyValuePair<T, IValueOrError<R>>>))
            {
                res = Trimmable.Create<KeyValuePair<T, IValueOrError<R>>>(shouldTrim => TrimBy(kv => true, kv => new KeyValuePair<T, IValueOrError<R>>(kv.Key, kv.Value), shouldTrim));
            }

            //
            // NB: No trim by key or value; those types could unify to the same ITrimmable<>.
            //     The drawback is that users of N-ary function need to use Args<> types.
            //

            return res;
        }

        private int TrimBy<K>(Func<KeyValuePair<T, IValueOrError<R>>, bool> filter, Func<KeyValuePair<T, IValueOrError<R>>, K> selector, Func<K, bool> shouldTrim)
        {
            var keys = new HashSet<T>();

            foreach (var entry in _cache)
            {
                if (filter(entry) && shouldTrim(selector(entry)))
                {
                    keys.Add(entry.Key);
                }
            }

            foreach (var key in keys)
            {
                _cache.Remove(key);
            }

            return keys.Count;
        }
    }
}
