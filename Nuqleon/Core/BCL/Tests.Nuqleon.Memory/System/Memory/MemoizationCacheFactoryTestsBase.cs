// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Memory;
using System.Time;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    public class MemoizationCacheFactoryTestsBase
    {
        protected static void Nop_ArgumentChecking(IMemoizationCacheFactory mcf)
        {
            Assert.ThrowsException<ArgumentNullException>(() => mcf.Create(default(Func<int, int>), MemoizationOptions.None, EqualityComparer<int>.Default));
        }

        protected static void Nop_Simple(IMemoizationCacheFactory mcf)
        {
            foreach (var options in new[] { MemoizationOptions.None, MemoizationOptions.CacheException })
            {
                var n = 0;
                var cache = mcf.Create<string, int>(s => { n++; return s.Length; }, options, comparer: null);

                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(0, n);

                Assert.AreEqual(3, cache.GetOrAdd("foo"));
                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(1, n);

                Assert.AreEqual(3, cache.GetOrAdd("foo"));
                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(2, n);

                Assert.AreEqual(4, cache.GetOrAdd("test"));
                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(3, n);

                cache.Clear();

                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(3, n);

                Assert.AreEqual(3, cache.GetOrAdd("foo"));
                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(4, n);

                Assert.AreEqual(3, cache.GetOrAdd("foo"));
                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(5, n);

                Assert.AreEqual(4, cache.GetOrAdd("test"));
                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(6, n);

                Assert.IsTrue(string.IsNullOrEmpty(cache.DebugView));
            }
        }

        protected static void Unbounded_ArgumentChecking(IMemoizationCacheFactory mcf)
        {
            Assert.ThrowsException<ArgumentNullException>(() => mcf.Create(default(Func<int, int>), MemoizationOptions.None, EqualityComparer<int>.Default));
        }

        protected static void Unbounded_Simple(IMemoizationCacheFactory mcf)
        {
            foreach (var options in new[] { MemoizationOptions.None, MemoizationOptions.CacheException })
            {
                var n = 0;
                var cache = mcf.Create<string, int>(s => { n++; return s.Length; }, options, comparer: null);

                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(0, n);

                Assert.AreEqual(3, cache.GetOrAdd("foo"));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(1, n);

                Assert.AreEqual(3, cache.GetOrAdd("foo"));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(1, n);

                Assert.AreEqual(4, cache.GetOrAdd("test"));
                Assert.AreEqual(2, cache.Count);
                Assert.AreEqual(2, n);

                cache.Clear();

                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(2, n);

                Assert.AreEqual(3, cache.GetOrAdd("foo"));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(3, n);

                Assert.AreEqual(3, cache.GetOrAdd("foo"));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(3, n);

                Assert.AreEqual(4, cache.GetOrAdd("test"));
                Assert.AreEqual(2, cache.Count);
                Assert.AreEqual(4, n);

                Assert.IsTrue(!string.IsNullOrEmpty(cache.DebugView));
            }
        }

        protected static void Unbounded_NoErrorCaching(IMemoizationCacheFactory mcf)
        {
            var n = 0;
            var cache = mcf.Create<string, int>(s => { n++; return s.Length; }, MemoizationOptions.None, comparer: null);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            for (var i = 1; i <= 3; i++)
            {
                Assert.ThrowsException<NullReferenceException>(() => cache.GetOrAdd(argument: null));
                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(i, n);
            }
        }

        protected static void Unbounded_ErrorCaching(IMemoizationCacheFactory mcf)
        {
            var n = 0;
            var cache = mcf.Create<string, int>(s => { n++; return s.Length; }, MemoizationOptions.CacheException, comparer: null);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            for (var i = 1; i <= 3; i++)
            {
                Assert.ThrowsException<NullReferenceException>(() => cache.GetOrAdd(argument: null));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(1, n);
            }
        }

        protected static void Unbounded_Trim1(IMemoizationCacheFactory mcf)
        {
            var res = mcf.Create<int, int>(x => x * 2, MemoizationOptions.None, EqualityComparer<int>.Default);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(1, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => kv.Key % 2 == 0);
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Key % 3 == 0);
            Assert.AreEqual(0, res.Count);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Value % 2 != 0);
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Value % 3 == 0);
            Assert.AreEqual(0, res.Count);
        }

        protected static void Unbounded_Trim2(IMemoizationCacheFactory mcf)
        {
            var res = mcf.Create<int, int>(x => 100 / x, MemoizationOptions.CacheException, EqualityComparer<int>.Default);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd(0));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(50, res.GetOrAdd(2));
            Assert.AreEqual(2, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => kv.Key % 2 == 0);
            Assert.AreEqual(1, res.Count);

            var trimErr = res.AsTrimmableByArgumentAndResultOrError();
            Assert.IsNotNull(trimErr);

            trimErr.Trim(kv => kv.Value.Kind == ValueOrErrorKind.Error && kv.Key == 0);
            Assert.AreEqual(0, res.Count);

            trimErr.Trim(kv => kv.Key % 3 == 0);
            Assert.AreEqual(0, res.Count);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd(0));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(50, res.GetOrAdd(2));
            Assert.AreEqual(2, res.Count);
        }

        protected static void Lru_ArgumentChecking(Func<int, IMemoizationCacheFactory> createFactory)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createFactory(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createFactory(0));
            Assert.ThrowsException<ArgumentNullException>(() => createFactory(1).Create(default(Func<int, int>), MemoizationOptions.None, EqualityComparer<int>.Default));
        }

        protected static void Lru_NoError_Simple(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var mcf = createFactory(4);

            var n = 0;
            var cache = mcf.Create<int, int>(x => { n++; return x * 2; }, MemoizationOptions.None, comparer: null);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            Assert.AreEqual(2, cache.GetOrAdd(1)); // [1->2]
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, n);

            Assert.AreEqual(2, cache.GetOrAdd(1)); // [1->2]
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, n);

            Assert.AreEqual(4, cache.GetOrAdd(2)); // [2->4, 1->2]
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(2, n);

            Assert.AreEqual(2, cache.GetOrAdd(1)); // [1->2, 2->4]
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(2, n);

            Assert.AreEqual(6, cache.GetOrAdd(3)); // [3->6, 1->2, 2->4]
            Assert.AreEqual(3, cache.Count);
            Assert.AreEqual(3, n);

            Assert.AreEqual(8, cache.GetOrAdd(4)); // [4->8, 3->6, 1->2, 2->4]
            Assert.AreEqual(4, cache.Count);
            Assert.AreEqual(4, n);

            Assert.AreEqual(4, cache.GetOrAdd(2)); // [2->4, 4->8, 3->6, 1->2]
            Assert.AreEqual(4, cache.Count);
            Assert.AreEqual(4, n);

            Assert.AreEqual(10, cache.GetOrAdd(5)); // [5->10, 2->4, 4->8, 3->6]
            Assert.AreEqual(4, cache.Count);
            Assert.AreEqual(5, n);

            cache.Clear();

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(5, n);

            Assert.AreEqual(2, cache.GetOrAdd(1)); // [1->2]
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(6, n);
        }

        protected static void Lru_NoError_Pattern1(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var M = 8;
            var N = 10;

            for (var i = 1; i <= M; i++)
            {
                var mcf = createFactory(i);

                var n = 0;
                var cache = mcf.Create<int, int>(x => { n++; return x * 2; }, MemoizationOptions.None, comparer: null);

                var xs = Enumerable.Range(0, i);

                for (var j = 0; j < N; j++)
                {
                    foreach (var x in xs)
                    {
                        Assert.AreEqual(x * 2, cache.GetOrAdd(x));
                        Assert.AreEqual(n, cache.Count);
                    }
                }

                Assert.AreEqual(n, i);
            }
        }

        protected static void Lru_NoError_Pattern2(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var M = 8;
            var N = 10;

            for (var i = 1; i <= M; i++)
            {
                var mcf = createFactory(i);

                var n = 0;
                var cache = mcf.Create<int, int>(x => { n++; return x * 2; }, MemoizationOptions.None, comparer: null);

                var xs = Enumerable.Range(0, i + 1); // too large for cache, always evicts after one iteration

                for (var j = 1; j <= N; j++)
                {
                    foreach (var x in xs)
                    {
                        Assert.AreEqual(x * 2, cache.GetOrAdd(x));
                    }

                    Assert.AreEqual(i, cache.Count);
                    Assert.AreEqual(n, (i + 1) * j);
                }

                Assert.AreEqual(n, (i + 1) * N);
            }
        }

        protected static void Lru_NoError_Random(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var r = new Random(1983);

            for (var c = 1; c <= 10000; c *= 10)
            {
                var mcf = createFactory(c);

                for (var i = 1; i <= 10000; i *= 10)
                {
                    var n = 0;
                    var cache = mcf.Create<int, int>(x => { n++; return x * 2; }, MemoizationOptions.None, comparer: null);

                    var xs = Enumerable.Range(0, c).Select(_ => r.Next(0, 100)).ToList();
                    var unique = xs.Distinct().Count();
                    var bigEnough = unique <= c;
                    var seen = new HashSet<int>();

                    var e = 0;
                    foreach (var x in xs)
                    {
                        Assert.AreEqual(x * 2, cache.GetOrAdd(x));

                        if (bigEnough)
                        {
                            if (seen.Add(x))
                            {
                                e++;
                            }

                            Assert.AreEqual(e, n);
                        }
                    }

                    Assert.AreEqual(cache.Count, Math.Min(unique, c));
                }
            }
        }

        protected static void Lru_NoError_Error(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var mcf = createFactory(4);

            var n = 0;
            var cache = mcf.Create<int, int>(x => { n++; return 1000 / x; }, MemoizationOptions.None, comparer: null);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            Assert.AreEqual(100, cache.GetOrAdd(10));
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, n);

            Assert.AreEqual(50, cache.GetOrAdd(20));
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(2, n);

            Assert.ThrowsException<DivideByZeroException>(() => cache.GetOrAdd(0));
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(3, n);

            Assert.ThrowsException<DivideByZeroException>(() => cache.GetOrAdd(0));
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(4, n);

            Assert.AreEqual(20, cache.GetOrAdd(50));
            Assert.AreEqual(3, cache.Count);
            Assert.AreEqual(5, n);
        }

        protected static void Lru_CacheError_Error(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var mcf = createFactory(4);

            var n = 0;
            var cache = mcf.Create<int, int>(x => { n++; return 1000 / x; }, MemoizationOptions.CacheException, comparer: null);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            Assert.AreEqual(100, cache.GetOrAdd(10));
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, n);

            Assert.AreEqual(50, cache.GetOrAdd(20));
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(2, n);

            Assert.ThrowsException<DivideByZeroException>(() => cache.GetOrAdd(0));
            Assert.AreEqual(3, cache.Count);
            Assert.AreEqual(3, n);

            Assert.ThrowsException<DivideByZeroException>(() => cache.GetOrAdd(0));
            Assert.AreEqual(3, cache.Count);
            Assert.AreEqual(3, n);

            Assert.AreEqual(20, cache.GetOrAdd(50));
            Assert.AreEqual(4, cache.Count);
            Assert.AreEqual(4, n);
        }

        protected static void Lru_Trim1(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var mcf = createFactory(10);
            var res = mcf.Create<int, int>(x => x * 2, MemoizationOptions.None, EqualityComparer<int>.Default);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(1, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => kv.Key % 2 == 0);
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Key % 3 == 0);
            Assert.AreEqual(0, res.Count);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Value % 2 != 0);
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Value % 3 == 0);
            Assert.AreEqual(0, res.Count);
        }

        protected static void Lru_Trim2(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var mcf = createFactory(10);
            var res = mcf.Create<int, int>(x => 100 / x, MemoizationOptions.CacheException, EqualityComparer<int>.Default);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd(0));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(50, res.GetOrAdd(2));
            Assert.AreEqual(2, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => kv.Key % 2 == 0);
            Assert.AreEqual(1, res.Count);

            var trimErr = res.AsTrimmableByArgumentAndResultOrError();
            Assert.IsNotNull(trimErr);

            trimErr.Trim(kv => kv.Value.Kind == ValueOrErrorKind.Error && kv.Key == 0);
            Assert.AreEqual(0, res.Count);

            trimErr.Trim(kv => kv.Key % 3 == 0);
            Assert.AreEqual(0, res.Count);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd(0));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(50, res.GetOrAdd(2));
            Assert.AreEqual(2, res.Count);
        }

        protected static void Lru_Trim3(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var mcf = createFactory(10);
            var res = mcf.Create<int, int>(x => x * 2, MemoizationOptions.None, EqualityComparer<int>.Default);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);
        }

        protected static void Lru_Trim4(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var mcf = createFactory(10);
            var res = mcf.Create<int, int>(x => x * 2, MemoizationOptions.None, EqualityComparer<int>.Default);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            {
                Assert.AreEqual(42, res.GetOrAdd(21));
                Assert.AreEqual(1, res.Count);

                Assert.AreEqual(44, res.GetOrAdd(22));
                Assert.AreEqual(2, res.Count);

                trim.Trim(kv => kv.Key == 22);
                Assert.AreEqual(1, res.Count);

                res.Clear();
            }

            {
                Assert.AreEqual(44, res.GetOrAdd(22));
                Assert.AreEqual(1, res.Count);

                Assert.AreEqual(42, res.GetOrAdd(21));
                Assert.AreEqual(2, res.Count);

                trim.Trim(kv => kv.Key == 22);
                Assert.AreEqual(1, res.Count);

                res.Clear();
            }
        }

        protected static void Lru_Trim5(Func<int, IMemoizationCacheFactory> createFactory)
        {
            var mcf = createFactory(10);
            var res = mcf.Create<int, int>(x => x * 2, MemoizationOptions.None, EqualityComparer<int>.Default);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            for (var i = 1; i <= 5; i++)
            {
                Assert.AreEqual(i * 2, res.GetOrAdd(i));
                Assert.AreEqual(i, res.Count);
            }

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);
        }

        protected static void Evict_ArgumentChecking(Func<Func<IMemoizationCacheEntryMetrics, int>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByHighest, Func<Func<IMemoizationCacheEntryMetrics, int>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByLowest)
        {
            Assert.ThrowsException<ArgumentNullException>(() => createEvictedByHighest(null, 1, 1.0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByHighest(e => 0, -1, 1.0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByHighest(e => 0, 0, 1.0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByHighest(e => 0, 1, 0.0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByHighest(e => 0, 1, -0.1, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByHighest(e => 0, 1, 1.1, null));

            Assert.ThrowsException<ArgumentNullException>(() => createEvictedByLowest(null, 1, 1.0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByLowest(e => 0, -1, 1.0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByLowest(e => 0, 0, 1.0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByLowest(e => 0, 1, 0.0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByLowest(e => 0, 1, -0.1, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => createEvictedByLowest(e => 0, 1, 1.1, null));

            Assert.ThrowsException<ArgumentNullException>(() => createEvictedByHighest(e => 0, 1, 1.0, null).Create(default(Func<int, int>), MemoizationOptions.None, EqualityComparer<int>.Default));
            Assert.ThrowsException<ArgumentNullException>(() => createEvictedByLowest(e => 0, 1, 1.0, null).Create(default(Func<int, int>), MemoizationOptions.None, EqualityComparer<int>.Default));
        }

        protected static void Evict_Lowest_HitCount(Func<Func<IMemoizationCacheEntryMetrics, int>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByLowest)
        {
            var cache = createEvictedByLowest(m => m.HitCount, 4, 1.0, null);

            var n = 0;
            var f = cache.Create<int, int>(x => { n++; return x + 1; }, MemoizationOptions.None, EqualityComparer<int>.Default);

            Assert.AreEqual(2, f.GetOrAdd(1));
            Assert.AreEqual(2, f.GetOrAdd(1));
            Assert.AreEqual(1, n);

            // [1]

            Assert.AreEqual(3, f.GetOrAdd(2));
            Assert.AreEqual(2, n);

            // [2, 1]

            Assert.AreEqual(4, f.GetOrAdd(3));
            Assert.AreEqual(4, f.GetOrAdd(3));
            Assert.AreEqual(4, f.GetOrAdd(3));
            Assert.AreEqual(4, f.GetOrAdd(3));
            Assert.AreEqual(3, n);

            // [2, 1, 3]

            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(4, n);

            // [2, 1, 4, 3]

            Assert.AreEqual(6, f.GetOrAdd(5));
            Assert.AreEqual(6, f.GetOrAdd(5));
            Assert.AreEqual(6, f.GetOrAdd(5));
            Assert.AreEqual(6, f.GetOrAdd(5));
            Assert.AreEqual(6, f.GetOrAdd(5));
            Assert.AreEqual(5, n);

            // [1, 4, 3, 5]

            Assert.AreEqual(3, f.GetOrAdd(2));
            Assert.AreEqual(6, n);

            // [2, 4, 3, 5]

            Assert.AreEqual(7, f.GetOrAdd(6));
            Assert.AreEqual(7, f.GetOrAdd(6));
            Assert.AreEqual(7, f.GetOrAdd(6));
            Assert.AreEqual(7, f.GetOrAdd(6));
            Assert.AreEqual(7, f.GetOrAdd(6));
            Assert.AreEqual(7, f.GetOrAdd(6));
            Assert.AreEqual(7, n);

            // [4, 3, 5, 6]

            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(4, f.GetOrAdd(3));
            Assert.AreEqual(6, f.GetOrAdd(5));
            Assert.AreEqual(7, f.GetOrAdd(6));
            Assert.AreEqual(7, n);

            f.Clear();

            // []

            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(8, n);
        }

        protected static void Evict_Highest_HitCount(Func<Func<IMemoizationCacheEntryMetrics, int>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByHighest)
        {
            var cache = createEvictedByHighest(m => m.HitCount, 4, 1.0, null);

            var n = 0;
            var f = cache.Create<int, int>(x => { n++; return x + 1; }, MemoizationOptions.None, EqualityComparer<int>.Default);

            Assert.AreEqual(2, f.GetOrAdd(1));
            Assert.AreEqual(2, f.GetOrAdd(1));
            Assert.AreEqual(1, n);

            // [1]

            Assert.AreEqual(3, f.GetOrAdd(2));
            Assert.AreEqual(2, n);

            // [2, 1]

            Assert.AreEqual(4, f.GetOrAdd(3));
            Assert.AreEqual(4, f.GetOrAdd(3));
            Assert.AreEqual(4, f.GetOrAdd(3));
            Assert.AreEqual(4, f.GetOrAdd(3));
            Assert.AreEqual(3, n);

            // [2, 1, 3]

            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(4, n);

            // [2, 1, 4, 3]

            Assert.AreEqual(6, f.GetOrAdd(5));
            Assert.AreEqual(5, n);

            // [5, 2, 1, 4]

            Assert.AreEqual(3, f.GetOrAdd(2));
            Assert.AreEqual(5, n);

            // [5, 2, 1, 4]

            Assert.AreEqual(6, f.GetOrAdd(5));
            Assert.AreEqual(3, f.GetOrAdd(2));
            Assert.AreEqual(2, f.GetOrAdd(1));
            Assert.AreEqual(5, f.GetOrAdd(4));
            Assert.AreEqual(5, n);
        }

        protected static void Evict_Lowest_InvokeDuration(Func<Func<IMemoizationCacheEntryMetrics, TimeSpan>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByLowest)
        {
            var c = new VirtualTimeClock();

            var cache = createEvictedByLowest(m => m.InvokeDuration, 4, 1.0, StopwatchFactory.FromClock(c));

            var n = 0;
            var f = cache.Create<int, int>(x => { n++; c.Now += x * 1000; return x + 1; }, MemoizationOptions.None, EqualityComparer<int>.Default);

            Assert.AreEqual(3, f.GetOrAdd(2)); // [2]
            Assert.AreEqual(2000, c.Now);

            Assert.AreEqual(2, f.GetOrAdd(1)); // [1, 2]
            Assert.AreEqual(3000, c.Now);

            Assert.AreEqual(4, f.GetOrAdd(3)); // [1, 2, 3]
            Assert.AreEqual(6000, c.Now);

            Assert.AreEqual(2, f.GetOrAdd(1)); // [1, 2, 3]
            Assert.AreEqual(6000, c.Now);

            Assert.AreEqual(5, f.GetOrAdd(4)); // [1, 2, 3, 4]
            Assert.AreEqual(10000, c.Now);

            Assert.AreEqual(6, f.GetOrAdd(5)); // [2, 3, 4, 5]
            Assert.AreEqual(15000, c.Now);

            Assert.AreEqual(2, f.GetOrAdd(1)); // [1, 3, 4, 5]
            Assert.AreEqual(16000, c.Now);

            Assert.AreEqual(3, f.GetOrAdd(2)); // [2, 3, 4, 5]
            Assert.AreEqual(18000, c.Now);
        }

        protected static void Evict_Lowest_LastAccessTime(Func<Func<IMemoizationCacheEntryMetrics, TimeSpan>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByLowest)
        {
            var c = new VirtualTimeClock();

            var cache = createEvictedByLowest(m => m.LastAccessTime, 4, 1.0, StopwatchFactory.FromClock(c));

            var n = 0;
            var f = cache.Create<int, int>(x => { n++; c.Now += x * 1000; return x + 1; }, MemoizationOptions.None, EqualityComparer<int>.Default);

            Assert.AreEqual(3, f.GetOrAdd(2)); // [2]
            Assert.AreEqual(2000, c.Now);

            Assert.AreEqual(2, f.GetOrAdd(1)); // [1, 2]
            Assert.AreEqual(3000, c.Now);

            Assert.AreEqual(4, f.GetOrAdd(3)); // [3, 1, 2]
            Assert.AreEqual(6000, c.Now);

            Assert.AreEqual(2, f.GetOrAdd(1)); // [1, 3, 2]
            Assert.AreEqual(6000, c.Now);

            Assert.AreEqual(5, f.GetOrAdd(4)); // [4, 1, 3, 2]
            Assert.AreEqual(10000, c.Now);

            Assert.AreEqual(6, f.GetOrAdd(5)); // [5, 4, 1, 3]
            Assert.AreEqual(15000, c.Now);

            Assert.AreEqual(2, f.GetOrAdd(1)); // [1, 5, 4, 3]
            Assert.AreEqual(15000, c.Now);

            Assert.AreEqual(3, f.GetOrAdd(2)); // [2, 1, 5, 4]
            Assert.AreEqual(17000, c.Now);
        }

        protected static void Evict_Error_NoCaching(Func<Func<IMemoizationCacheEntryMetrics, int>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByLowest)
        {
            var cache = createEvictedByLowest(m => m.HitCount, 4, 1.0, null);

            var n = 0;
            var f = cache.Create<int, int>(x => { n++; return 1000 / x; }, MemoizationOptions.None, EqualityComparer<int>.Default);

            Assert.AreEqual(100, f.GetOrAdd(10));
            Assert.AreEqual(1, n);

            Assert.AreEqual(100, f.GetOrAdd(10));
            Assert.AreEqual(1, n);

            Assert.AreEqual(50, f.GetOrAdd(20));
            Assert.AreEqual(2, n);

            Assert.AreEqual(20, f.GetOrAdd(50));
            Assert.AreEqual(3, n);

            Assert.AreEqual(10, f.GetOrAdd(100));
            Assert.AreEqual(4, n);

            Assert.ThrowsException<DivideByZeroException>(() => f.GetOrAdd(0));
            Assert.AreEqual(5, n);

            Assert.ThrowsException<DivideByZeroException>(() => f.GetOrAdd(0));
            Assert.AreEqual(6, n);

            Assert.AreEqual(100, f.GetOrAdd(10));
            Assert.AreEqual(6, n);
        }

        protected static void Evict_Error_Caching(Func<Func<IMemoizationCacheEntryMetrics, int>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByLowest)
        {
            var cache = createEvictedByLowest(m => m.HitCount, 4, 1.0, null);

            var n = 0;
            var f = cache.Create<int, int>(x => { n++; return 1000 / x; }, MemoizationOptions.CacheException, EqualityComparer<int>.Default);

            Assert.AreEqual(100, f.GetOrAdd(10));
            Assert.AreEqual(1, n);

            Assert.AreEqual(100, f.GetOrAdd(10));
            Assert.AreEqual(1, n);

            Assert.AreEqual(50, f.GetOrAdd(20));
            Assert.AreEqual(2, n);

            Assert.AreEqual(20, f.GetOrAdd(50));
            Assert.AreEqual(3, n);

            Assert.AreEqual(10, f.GetOrAdd(100));
            Assert.AreEqual(4, n);

            Assert.ThrowsException<DivideByZeroException>(() => f.GetOrAdd(0));
            Assert.AreEqual(5, n);

            Assert.ThrowsException<DivideByZeroException>(() => f.GetOrAdd(0));
            Assert.AreEqual(5, n);

            Assert.AreEqual(100, f.GetOrAdd(10));
            Assert.AreEqual(5, n);
        }

        protected static void Evict_Trim1(Func<Func<IMemoizationCacheEntryMetrics, int>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByLowest)
        {
            var mcf = createEvictedByLowest(e => e.HitCount, 10, 0.9, null);
            var res = mcf.Create<int, int>(x => x * 2, MemoizationOptions.None, EqualityComparer<int>.Default);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(1, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => kv.Key % 2 == 0);
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Key % 3 == 0);
            Assert.AreEqual(0, res.Count);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Value % 2 != 0);
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Value % 3 == 0);
            Assert.AreEqual(0, res.Count);
        }

        protected static void Evict_Trim2(Func<Func<IMemoizationCacheEntryMetrics, int>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByLowest)
        {
            var mcf = createEvictedByLowest(e => e.HitCount, 10, 0.9, null);
            var res = mcf.Create<int, int>(x => 100 / x, MemoizationOptions.CacheException, EqualityComparer<int>.Default);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd(0));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(50, res.GetOrAdd(2));
            Assert.AreEqual(2, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => kv.Key % 2 == 0);
            Assert.AreEqual(1, res.Count);

            var trimErr = res.AsTrimmableByArgumentAndResultOrError();
            Assert.IsNotNull(trimErr);

            trimErr.Trim(kv => kv.Value.Kind == ValueOrErrorKind.Error && kv.Key == 0);
            Assert.AreEqual(0, res.Count);

            trimErr.Trim(kv => kv.Key % 3 == 0);
            Assert.AreEqual(0, res.Count);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd(0));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(50, res.GetOrAdd(2));
            Assert.AreEqual(2, res.Count);
        }

        protected static void Evict_Trim3(Func<Func<IMemoizationCacheEntryMetrics, int>, int, double, IStopwatchFactory, IMemoizationCacheFactory> createEvictedByLowest)
        {
            var mcf = createEvictedByLowest(e => e.HitCount, 10, 0.9, null);
            var res = mcf.Create<int, int>(x => x * 2, MemoizationOptions.CacheException, EqualityComparer<int>.Default);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(44, res.GetOrAdd(22));
            Assert.AreEqual(2, res.Count);

            Assert.AreEqual(42, res.GetOrAdd(21));
            Assert.AreEqual(2, res.Count);

            var trim = res.AsTrimmableByMetrics();
            Assert.IsNotNull(trim);

            trim.Trim(e => e.HitCount == 1);
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual(44, res.GetOrAdd(22));
            Assert.AreEqual(2, res.Count);
        }
    }
}
