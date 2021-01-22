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
    internal class UnboundedMemoizationCacheBase<T, R> : MemoizationCacheBase<T, R>, IServiceProvider
    {
        private readonly Func<T, R> _function;
        private readonly ICacheDictionary<T, R> _cache;

        public UnboundedMemoizationCacheBase(Func<T, R> function, ICacheDictionary<T, R> cache)
        {
            _function = function;
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

        protected override R GetOrAddCore(T args) => _cache.GetOrAdd(args, _function);

        protected override void ClearCore(bool disposing) => _cache.Clear();

        public object GetService(Type serviceType)
        {
            var res = default(object);

            if (serviceType == typeof(ITrimmable<KeyValuePair<T, R>>))
            {
                res = Trimmable.Create<KeyValuePair<T, R>>(shouldTrim => TrimBy(kv => kv, shouldTrim));
            }

            //
            // NB: No trim by key or value; those types could unify to the same ITrimmable<>.
            //     The drawback is that users of N-ary function need to use Args<> types.
            //

            return res;
        }

        private int TrimBy<K>(Func<KeyValuePair<T, R>, K> selector, Func<K, bool> shouldTrim)
        {
            var keys = new HashSet<T>();

            foreach (var entry in _cache)
            {
                if (shouldTrim(selector(entry)))
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
