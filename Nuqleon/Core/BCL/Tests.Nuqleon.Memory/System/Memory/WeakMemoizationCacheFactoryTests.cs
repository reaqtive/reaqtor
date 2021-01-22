// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Memory;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Time;

namespace Tests
{
    [TestClass]
    public class WeakMemoizationCacheFactoryTests
    {
        [TestMethod]
        public void WeakMemoizationCacheFactory_Nop_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactory.Nop.Create(default(Func<string, string>), MemoizationOptions.None));
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Nop_Simple()
        {
            foreach (var options in new[] { MemoizationOptions.None, MemoizationOptions.CacheException })
            {
                var n = 0;
                var cache = WeakMemoizationCacheFactory.Nop.Create<string, int>(s => { n++; return s.Length; }, options);

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

        [TestMethod]
        public void WeakMemoizationCacheFactory_Unbounded_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactory.Unbounded.Create(default(Func<string, string>), MemoizationOptions.None));
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Unbounded_Simple()
        {
            var mcf = WeakMemoizationCacheFactory.Unbounded;

            foreach (var options in new[] { MemoizationOptions.None, MemoizationOptions.CacheException })
            {
                var n = 0;
                var cache = mcf.Create<string, string>(s => { n++; return s.ToUpper(); }, options);

                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(0, n);

                Assert.AreEqual("FOO", cache.GetOrAdd("foo"));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(1, n);

                Assert.AreEqual("FOO", cache.GetOrAdd("foo"));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(1, n);

                Assert.AreEqual("TEST", cache.GetOrAdd("test"));
                Assert.AreEqual(2, cache.Count);
                Assert.AreEqual(2, n);

                cache.Clear();

                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(2, n);

                Assert.AreEqual("FOO", cache.GetOrAdd("foo"));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(3, n);

                Assert.AreEqual("FOO", cache.GetOrAdd("foo"));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(3, n);

                Assert.AreEqual("TEST", cache.GetOrAdd("test"));
                Assert.AreEqual(2, cache.Count);
                Assert.AreEqual(4, n);
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Unbounded_NoErrorCaching()
        {
            var mcf = WeakMemoizationCacheFactory.Unbounded;

            var n = 0;
            var cache = mcf.Create<string, string>(s => { n++; return s.ToUpper(); }, MemoizationOptions.None);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            for (var i = 1; i <= 3; i++)
            {
                Assert.ThrowsException<NullReferenceException>(() => cache.GetOrAdd(argument: null));
                Assert.AreEqual(0, cache.Count);
                Assert.AreEqual(i, n);
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Unbounded_ErrorCaching()
        {
            var mcf = WeakMemoizationCacheFactory.Unbounded;

            var n = 0;
            var cache = mcf.Create<string, string>(s => { n++; return s.ToUpper(); }, MemoizationOptions.CacheException);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            for (var i = 1; i <= 3; i++)
            {
                Assert.ThrowsException<NullReferenceException>(() => cache.GetOrAdd(argument: null));
                Assert.AreEqual(1, cache.Count);
                Assert.AreEqual(1, n);
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Unbounded_Lifetime()
        {
            lock (typeof(Obj)) // ensuring no concurrent tests are run
            {
                Obj.Reset();
                try
                {
                    var mcf = WeakMemoizationCacheFactory.Unbounded;

                    var cache = mcf.Create<Obj, string>(x => x.ToString(), MemoizationOptions.None);

                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    Assert.AreEqual(1, Obj.FinalizeCount);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateLru(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateLru(0));
            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactory.CreateLru(1).Create(default(Func<string, string>), MemoizationOptions.None));
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_NoError_Simple()
        {
            var mcf = WeakMemoizationCacheFactory.CreateLru(4);

            var n = 0;
            var cache = mcf.Create<string, string>(x => { n++; return x + "!"; }, MemoizationOptions.None);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            Assert.AreEqual("1!", cache.GetOrAdd("1")); // [1->1!]
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, n);

            Assert.AreEqual("1!", cache.GetOrAdd("1")); // [1->1!]
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, n);

            Assert.AreEqual("2!", cache.GetOrAdd("2")); // [2->2!, 1->1!]
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(2, n);

            Assert.AreEqual("1!", cache.GetOrAdd("1")); // [1->1!, 2->2!]
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(2, n);

            Assert.AreEqual("3!", cache.GetOrAdd("3")); // [3->3!, 1->1!, 2->2!]
            Assert.AreEqual(3, cache.Count);
            Assert.AreEqual(3, n);

            Assert.AreEqual("4!", cache.GetOrAdd("4")); // [4->4!, 3->3!, 1->1!, 2->2!]
            Assert.AreEqual(4, cache.Count);
            Assert.AreEqual(4, n);

            Assert.AreEqual("2!", cache.GetOrAdd("2")); // [2->2!, 4->4!, 3->3!, 1->1!]
            Assert.AreEqual(4, cache.Count);
            Assert.AreEqual(4, n);

            Assert.AreEqual("5!", cache.GetOrAdd("5")); // [5->5!, 2->2!, 4->4!, 3->3!]
            Assert.AreEqual(4, cache.Count);
            Assert.AreEqual(5, n);

            cache.Clear();

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(5, n);

            Assert.AreEqual("1!", cache.GetOrAdd("1")); // [1->1!]
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(6, n);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_NoError_Pattern1()
        {
            var M = 8;
            var N = 10;

            for (var i = 1; i <= M; i++)
            {
                var mcf = WeakMemoizationCacheFactory.CreateLru(i);

                var n = 0;
                var cache = mcf.Create<string, string>(x => { n++; return x + "!"; }, MemoizationOptions.None);

                var xs = Enumerable.Range(0, i).Select(x => x.ToString()).ToArray();

                for (var j = 0; j < N; j++)
                {
                    foreach (var x in xs)
                    {
                        Assert.AreEqual(x + "!", cache.GetOrAdd(x));
                        Assert.AreEqual(n, cache.Count);
                    }
                }

                Assert.AreEqual(n, i);
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_NoError_Pattern2()
        {
            var M = 8;
            var N = 10;

            for (var i = 1; i <= M; i++)
            {
                var mcf = WeakMemoizationCacheFactory.CreateLru(i);

                var n = 0;
                var cache = mcf.Create<string, string>(x => { n++; return x + "!"; }, MemoizationOptions.None);

                var xs = Enumerable.Range(0, i + 1).Select(x => x.ToString()).ToArray(); // too large for cache, always evicts after one iteration

                for (var j = 1; j <= N; j++)
                {
                    foreach (var x in xs)
                    {
                        Assert.AreEqual(x + "!", cache.GetOrAdd(x));
                    }

                    Assert.AreEqual(i, cache.Count);
                    Assert.AreEqual(n, (i + 1) * j);
                }

                Assert.AreEqual(n, (i + 1) * N);
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_NoError_Random()
        {
            var r = new Random(1983);

            var nums = Enumerable.Range(0, 100).Select(i => i.ToString()).ToArray();

            for (var c = 1; c <= 10000; c *= 10)
            {
                var mcf = WeakMemoizationCacheFactory.CreateLru(c);

                for (var i = 1; i <= 10000; i *= 10)
                {
                    var n = 0;
                    var cache = mcf.Create<string, string>(x => { n++; return x + "!"; }, MemoizationOptions.None);

                    var xs = Enumerable.Range(0, c).Select(_ => nums[r.Next(0, 100)]).ToList();
                    var unique = xs.Distinct().Count();
                    var bigEnough = unique <= c;
                    var seen = new HashSet<string>();

                    var e = 0;
                    foreach (var x in xs)
                    {
                        Assert.AreEqual(x + "!", cache.GetOrAdd(x));

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

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_NoError_Error()
        {
            var mcf = WeakMemoizationCacheFactory.CreateLru(4);

            var n = 0;
            var cache = mcf.Create<string, string>(x => { n++; return (1000 / x.Length).ToString(); }, MemoizationOptions.None);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            Assert.AreEqual("1000", cache.GetOrAdd("*"));
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, n);

            Assert.AreEqual("500", cache.GetOrAdd("**"));
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(2, n);

            Assert.ThrowsException<DivideByZeroException>(() => cache.GetOrAdd(""));
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(3, n);

            Assert.ThrowsException<DivideByZeroException>(() => cache.GetOrAdd(""));
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(4, n);

            Assert.AreEqual("250", cache.GetOrAdd("****"));
            Assert.AreEqual(3, cache.Count);
            Assert.AreEqual(5, n);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_CacheError_Error()
        {
            var mcf = WeakMemoizationCacheFactory.CreateLru(4);

            var n = 0;
            var cache = mcf.Create<string, string>(x => { n++; return (1000 / x.Length).ToString(); }, MemoizationOptions.CacheException);

            Assert.AreEqual(0, cache.Count);
            Assert.AreEqual(0, n);

            Assert.AreEqual("1000", cache.GetOrAdd("*"));
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(1, n);

            Assert.AreEqual("500", cache.GetOrAdd("**"));
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(2, n);

            Assert.ThrowsException<DivideByZeroException>(() => cache.GetOrAdd(""));
            Assert.AreEqual(3, cache.Count);
            Assert.AreEqual(3, n);

            Assert.ThrowsException<DivideByZeroException>(() => cache.GetOrAdd(""));
            Assert.AreEqual(3, cache.Count);
            Assert.AreEqual(3, n);

            Assert.AreEqual("250", cache.GetOrAdd("****"));
            Assert.AreEqual(4, cache.Count);
            Assert.AreEqual(4, n);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_Trim1()
        {
            var mcf = WeakMemoizationCacheFactory.CreateLru(10);
            var res = mcf.Create<string, string>(s => s + "!", MemoizationOptions.None);

            Assert.AreEqual("1!", res.GetOrAdd("1"));
            Assert.AreEqual(1, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => kv.Key.StartsWith("2"));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Key.StartsWith("1"));
            Assert.AreEqual(0, res.Count);

            Assert.AreEqual("1!", res.GetOrAdd("1"));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Key.StartsWith("2"));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Key.StartsWith("1"));
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_Trim2()
        {
            var mcf = WeakMemoizationCacheFactory.CreateLru(10);
            var res = mcf.Create<string, string>(s => (100 / int.Parse(s)).ToString(), MemoizationOptions.CacheException);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd("0"));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual("50", res.GetOrAdd("2"));
            Assert.AreEqual(2, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => int.Parse(kv.Key) % 2 == 0);
            Assert.AreEqual(1, res.Count);

            var trimErr = res.AsTrimmableByArgumentAndResultOrError();
            Assert.IsNotNull(trimErr);

            trimErr.Trim(kv => kv.Value.Kind == ValueOrErrorKind.Error && kv.Key == "0");
            Assert.AreEqual(0, res.Count);

            trimErr.Trim(kv => int.Parse(kv.Key) % 3 == 0);
            Assert.AreEqual(0, res.Count);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd("0"));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual("50", res.GetOrAdd("2"));
            Assert.AreEqual(2, res.Count);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_Trim3()
        {
            var mcf = WeakMemoizationCacheFactory.CreateLru(10);
            var res = mcf.Create<string, string>(s => s + "!", MemoizationOptions.None);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);

            Assert.AreEqual("1!", res.GetOrAdd("1"));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);

            Assert.AreEqual("1!", res.GetOrAdd("1"));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_Trim4()
        {
            var mcf = WeakMemoizationCacheFactory.CreateLru(10);
            var res = mcf.Create<string, string>(s => s + "!", MemoizationOptions.None);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            {
                Assert.AreEqual("1!", res.GetOrAdd("1"));
                Assert.AreEqual(1, res.Count);

                Assert.AreEqual("2!", res.GetOrAdd("2"));
                Assert.AreEqual(2, res.Count);

                trim.Trim(kv => kv.Key == "1");
                Assert.AreEqual(1, res.Count);

                res.Clear();
            }

            {
                Assert.AreEqual("2!", res.GetOrAdd("2"));
                Assert.AreEqual(1, res.Count);

                Assert.AreEqual("1!", res.GetOrAdd("1"));
                Assert.AreEqual(2, res.Count);

                trim.Trim(kv => kv.Key == "2");
                Assert.AreEqual(1, res.Count);

                res.Clear();
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_Trim5()
        {
            var mcf = WeakMemoizationCacheFactory.CreateLru(10);
            var res = mcf.Create<string, string>(s => s + "!", MemoizationOptions.None);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            for (var i = 1; i <= 5; i++)
            {
                Assert.AreEqual(i.ToString() + "!", res.GetOrAdd(i.ToString()));
                Assert.AreEqual(i, res.Count);
            }

            trim.Trim(kv => true);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_Lifetime()
        {
            lock (typeof(Obj)) // ensuring no concurrent tests are run
            {
                Obj.Reset();
                try
                {
                    var mcf = WeakMemoizationCacheFactory.CreateLru(4);

                    var cache = mcf.Create<Obj, string>(x => x.ToString(), MemoizationOptions.None);

                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));
                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));
                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));
                    Assert.AreEqual(3, cache.Count);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    Assert.AreEqual(3, Obj.FinalizeCount);

                    Assert.IsTrue(cache.DebugView.Contains("(empty slot)"));
                    Assert.AreEqual(3, cache.Count);

                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj")); // causes pruning

                    Assert.IsFalse(cache.DebugView.Contains("(empty slot)"));
                    Assert.AreEqual(1, cache.Count);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_Lifetime_Trim()
        {
            lock (typeof(Obj)) // ensuring no concurrent tests are run
            {
                Obj.Reset();
                try
                {
                    var mcf = WeakMemoizationCacheFactory.CreateLru(4);

                    var cache = mcf.Create<Obj, string>(x => x.ToString(), MemoizationOptions.None);

                    var trim = cache.AsTrimmableByArgumentAndResult();
                    Assert.IsNotNull(trim);

                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));
                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));
                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));
                    Assert.AreEqual(3, cache.Count);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    Assert.AreEqual(3, Obj.FinalizeCount);

                    Assert.IsTrue(cache.DebugView.Contains("(empty slot)"));
                    Assert.AreEqual(3, cache.Count);

                    trim.Trim(_ => false); // causes pruning

                    Assert.IsFalse(cache.DebugView.Contains("(empty slot)"));
                    Assert.AreEqual(0, cache.Count);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Lru_Dispose()
        {
            var mcf = WeakMemoizationCacheFactory.CreateLru(10);

            var res = mcf.Create<string, string>(s => s + "!", MemoizationOptions.None);

            res.Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() => res.GetOrAdd("1"));
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactory.CreateEvictedByHighest(default(Func<IMemoizationCacheEntryMetrics, int>), 1, 1.0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByHighest(e => 0, -1, 1.0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByHighest(e => 0, 0, 1.0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByHighest(e => 0, 1, 0.0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByHighest(e => 0, 1, -0.1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByHighest(e => 0, 1, 1.1));

            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactory.CreateEvictedByLowest(default(Func<IMemoizationCacheEntryMetrics, int>), 1, 1.0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByLowest(e => 0, -1, 1.0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByLowest(e => 0, 0, 1.0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByLowest(e => 0, 1, 0.0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByLowest(e => 0, 1, -0.1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WeakMemoizationCacheFactory.CreateEvictedByLowest(e => 0, 1, 1.1));

            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactory.CreateEvictedByHighest(e => 0, 1).Create(default(Func<string, string>), MemoizationOptions.None));
            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactory.CreateEvictedByLowest(e => 0, 1).Create(default(Func<string, string>), MemoizationOptions.None));
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Lowest_HitCount()
        {
            var cache = WeakMemoizationCacheFactory.CreateEvictedByLowest(m => m.HitCount, 4, 1.0);

            var n = 0;
            var f = cache.Create<string, string>(x => { n++; return x + "!"; }, MemoizationOptions.None);

            Assert.AreEqual("1!", f.GetOrAdd("1"));
            Assert.AreEqual("1!", f.GetOrAdd("1"));
            Assert.AreEqual(1, n);
            Assert.AreEqual(1, f.Count);

            // [1]

            Assert.AreEqual("2!", f.GetOrAdd("2"));
            Assert.AreEqual(2, n);
            Assert.AreEqual(2, f.Count);

            // [2, 1]

            Assert.AreEqual("3!", f.GetOrAdd("3"));
            Assert.AreEqual("3!", f.GetOrAdd("3"));
            Assert.AreEqual("3!", f.GetOrAdd("3"));
            Assert.AreEqual("3!", f.GetOrAdd("3"));
            Assert.AreEqual(3, n);
            Assert.AreEqual(3, f.Count);

            // [2, 1, 3]

            Assert.AreEqual("4!", f.GetOrAdd("4"));
            Assert.AreEqual("4!", f.GetOrAdd("4"));
            Assert.AreEqual("4!", f.GetOrAdd("4"));
            Assert.AreEqual(4, n);
            Assert.AreEqual(4, f.Count);

            // [2, 1, 4, 3]

            Assert.AreEqual("5!", f.GetOrAdd("5"));
            Assert.AreEqual("5!", f.GetOrAdd("5"));
            Assert.AreEqual("5!", f.GetOrAdd("5"));
            Assert.AreEqual("5!", f.GetOrAdd("5"));
            Assert.AreEqual("5!", f.GetOrAdd("5"));
            Assert.AreEqual(5, n);
            Assert.AreEqual(4, f.Count);

            // [1, 4, 3, 5]

            Assert.AreEqual("2!", f.GetOrAdd("2"));
            Assert.AreEqual(6, n);
            Assert.AreEqual(4, f.Count);

            // [2, 4, 3, 5]

            Assert.AreEqual("6!", f.GetOrAdd("6"));
            Assert.AreEqual("6!", f.GetOrAdd("6"));
            Assert.AreEqual("6!", f.GetOrAdd("6"));
            Assert.AreEqual("6!", f.GetOrAdd("6"));
            Assert.AreEqual("6!", f.GetOrAdd("6"));
            Assert.AreEqual("6!", f.GetOrAdd("6"));
            Assert.AreEqual(7, n);
            Assert.AreEqual(4, f.Count);

            // [4, 3, 5, 6]

            Assert.AreEqual("4!", f.GetOrAdd("4"));
            Assert.AreEqual("3!", f.GetOrAdd("3"));
            Assert.AreEqual("5!", f.GetOrAdd("5"));
            Assert.AreEqual("6!", f.GetOrAdd("6"));
            Assert.AreEqual(7, n);
            Assert.AreEqual(4, f.Count);

            // []

            f.Clear();
            Assert.AreEqual(0, f.Count);

            // [1]

            Assert.AreEqual("1!", f.GetOrAdd("1"));
            Assert.AreEqual(8, n);
            Assert.AreEqual(1, f.Count);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Highest_HitCount()
        {
            var cache = WeakMemoizationCacheFactory.CreateEvictedByHighest(m => m.HitCount, 4, 1.0);

            var n = 0;
            var f = cache.Create<string, string>(x => { n++; return x + "!"; }, MemoizationOptions.None);

            Assert.AreEqual("1!", f.GetOrAdd("1"));
            Assert.AreEqual("1!", f.GetOrAdd("1"));
            Assert.AreEqual(1, n);

            // [1]

            Assert.AreEqual("2!", f.GetOrAdd("2"));
            Assert.AreEqual(2, n);

            // [2, 1]

            Assert.AreEqual("3!", f.GetOrAdd("3"));
            Assert.AreEqual("3!", f.GetOrAdd("3"));
            Assert.AreEqual("3!", f.GetOrAdd("3"));
            Assert.AreEqual("3!", f.GetOrAdd("3"));
            Assert.AreEqual(3, n);

            // [2, 1, 3]

            Assert.AreEqual("4!", f.GetOrAdd("4"));
            Assert.AreEqual("4!", f.GetOrAdd("4"));
            Assert.AreEqual("4!", f.GetOrAdd("4"));
            Assert.AreEqual(4, n);

            // [2, 1, 4, 3]

            Assert.AreEqual("5!", f.GetOrAdd("5"));
            Assert.AreEqual(5, n);

            // [5, 2, 1, 4]

            Assert.AreEqual("2!", f.GetOrAdd("2"));
            Assert.AreEqual(5, n);

            // [5, 2, 1, 4]

            Assert.AreEqual("5!", f.GetOrAdd("5"));
            Assert.AreEqual("2!", f.GetOrAdd("2"));
            Assert.AreEqual("1!", f.GetOrAdd("1"));
            Assert.AreEqual("4!", f.GetOrAdd("4"));
            Assert.AreEqual(5, n);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Lowest_InvokeDuration()
        {
            var c = new VirtualTimeClock();

            var cache = WeakMemoizationCacheFactory.CreateEvictedByLowest(m => m.InvokeDuration, 4, 1.0, StopwatchFactory.FromClock(c));

            var n = 0;
            var f = cache.Create<string, string>(x => { n++; c.Now += int.Parse(x) * 1000; return x + "!"; }, MemoizationOptions.None);

            Assert.AreEqual("2!", f.GetOrAdd("2")); // [2]
            Assert.AreEqual(2000, c.Now);

            Assert.AreEqual("1!", f.GetOrAdd("1")); // [1, 2]
            Assert.AreEqual(3000, c.Now);

            Assert.AreEqual("3!", f.GetOrAdd("3")); // [1, 2, 3]
            Assert.AreEqual(6000, c.Now);

            Assert.AreEqual("1!", f.GetOrAdd("1")); // [1, 2, 3]
            Assert.AreEqual(6000, c.Now);

            Assert.AreEqual("4!", f.GetOrAdd("4")); // [1, 2, 3, 4]
            Assert.AreEqual(10000, c.Now);

            Assert.AreEqual("5!", f.GetOrAdd("5")); // [2, 3, 4, 5]
            Assert.AreEqual(15000, c.Now);

            Assert.AreEqual("1!", f.GetOrAdd("1")); // [1, 3, 4, 5]
            Assert.AreEqual(16000, c.Now);

            Assert.AreEqual("2!", f.GetOrAdd("2")); // [2, 3, 4, 5]
            Assert.AreEqual(18000, c.Now);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Lowest_LastAccessTime()
        {
            var c = new VirtualTimeClock();

            var cache = WeakMemoizationCacheFactory.CreateEvictedByLowest(m => m.LastAccessTime, 4, 1.0, StopwatchFactory.FromClock(c));

            var n = 0;
            var f = cache.Create<string, string>(x => { n++; c.Now += int.Parse(x) * 1000; return x + "!"; }, MemoizationOptions.None);

            Assert.AreEqual("2!", f.GetOrAdd("2")); // [2]
            Assert.AreEqual(2000, c.Now);

            Assert.AreEqual("1!", f.GetOrAdd("1")); // [1, 2]
            Assert.AreEqual(3000, c.Now);

            Assert.AreEqual("3!", f.GetOrAdd("3")); // [3, 1, 2]
            Assert.AreEqual(6000, c.Now);

            Assert.AreEqual("1!", f.GetOrAdd("1")); // [1, 3, 2]
            Assert.AreEqual(6000, c.Now);

            Assert.AreEqual("4!", f.GetOrAdd("4")); // [4, 1, 3, 2]
            Assert.AreEqual(10000, c.Now);

            Assert.AreEqual("5!", f.GetOrAdd("5")); // [5, 4, 1, 3]
            Assert.AreEqual(15000, c.Now);

            Assert.AreEqual("1!", f.GetOrAdd("1")); // [1, 5, 4, 3]
            Assert.AreEqual(15000, c.Now);

            Assert.AreEqual("2!", f.GetOrAdd("2")); // [2, 1, 5, 4]
            Assert.AreEqual(17000, c.Now);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Error_NoCaching()
        {
            var cache = WeakMemoizationCacheFactory.CreateEvictedByLowest(m => m.HitCount, 4, 1.0);

            var n = 0;
            var f = cache.Create<string, string>(x => { n++; return (1000 / int.Parse(x)).ToString(); }, MemoizationOptions.None);

            Assert.AreEqual("100", f.GetOrAdd("10"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("100", f.GetOrAdd("10"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("50", f.GetOrAdd("20"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("20", f.GetOrAdd("50"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("10", f.GetOrAdd("100"));
            Assert.AreEqual(4, n);

            Assert.ThrowsException<DivideByZeroException>(() => f.GetOrAdd("0"));
            Assert.AreEqual(5, n);

            Assert.ThrowsException<DivideByZeroException>(() => f.GetOrAdd("0"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("100", f.GetOrAdd("10"));
            Assert.AreEqual(6, n);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Error_Caching()
        {
            var cache = WeakMemoizationCacheFactory.CreateEvictedByLowest(m => m.HitCount, 4, 1.0);

            var n = 0;
            var f = cache.Create<string, string>(x => { n++; return (1000 / int.Parse(x)).ToString(); }, MemoizationOptions.CacheException);

            Assert.AreEqual("100", f.GetOrAdd("10"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("100", f.GetOrAdd("10"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("50", f.GetOrAdd("20"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("20", f.GetOrAdd("50"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("10", f.GetOrAdd("100"));
            Assert.AreEqual(4, n);

            Assert.ThrowsException<DivideByZeroException>(() => f.GetOrAdd("0"));
            Assert.AreEqual(5, n);

            Assert.ThrowsException<DivideByZeroException>(() => f.GetOrAdd("0"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("100", f.GetOrAdd("10"));
            Assert.AreEqual(5, n);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Trim1()
        {
            var mcf = WeakMemoizationCacheFactory.CreateEvictedByLowest(e => e.HitCount, 10, 0.9, stopwatchFactory: null);
            var res = mcf.Create<string, string>(s => s + "!", MemoizationOptions.None);

            Assert.AreEqual("1!", res.GetOrAdd("1"));
            Assert.AreEqual(1, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => kv.Key.StartsWith("2"));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Key.StartsWith("1"));
            Assert.AreEqual(0, res.Count);

            Assert.AreEqual("1!", res.GetOrAdd("1"));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Key.StartsWith("2"));
            Assert.AreEqual(1, res.Count);

            trim.Trim(kv => kv.Key.StartsWith("1"));
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Trim2()
        {
            var mcf = WeakMemoizationCacheFactory.CreateEvictedByLowest(e => e.HitCount, 10, 0.9, stopwatchFactory: null);
            var res = mcf.Create<string, string>(s => (100 / int.Parse(s)).ToString(), MemoizationOptions.CacheException);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd("0"));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual("50", res.GetOrAdd("2"));
            Assert.AreEqual(2, res.Count);

            var trim = res.AsTrimmableByArgumentAndResult();
            Assert.IsNotNull(trim);

            trim.Trim(kv => int.Parse(kv.Key) % 2 == 0);
            Assert.AreEqual(1, res.Count);

            var trimErr = res.AsTrimmableByArgumentAndResultOrError();
            Assert.IsNotNull(trimErr);

            trimErr.Trim(kv => kv.Value.Kind == ValueOrErrorKind.Error && kv.Key == "0");
            Assert.AreEqual(0, res.Count);

            trimErr.Trim(kv => int.Parse(kv.Key) % 3 == 0);
            Assert.AreEqual(0, res.Count);

            Assert.ThrowsException<DivideByZeroException>(() => res.GetOrAdd("0"));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual("50", res.GetOrAdd("2"));
            Assert.AreEqual(2, res.Count);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Trim3()
        {
            var mcf = WeakMemoizationCacheFactory.CreateEvictedByLowest(e => e.HitCount, 10, 0.9, stopwatchFactory: null);
            var res = mcf.Create<string, string>(s => s + "!", MemoizationOptions.CacheException);

            Assert.AreEqual("1!", res.GetOrAdd("1"));
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual("2!", res.GetOrAdd("2"));
            Assert.AreEqual(2, res.Count);

            Assert.AreEqual("1!", res.GetOrAdd("1"));
            Assert.AreEqual(2, res.Count);

            var trim = res.AsTrimmableByMetrics();
            Assert.IsNotNull(trim);

            trim.Trim(e => e.HitCount == 1);
            Assert.AreEqual(1, res.Count);

            Assert.AreEqual("2!", res.GetOrAdd("2"));
            Assert.AreEqual(2, res.Count);
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Lifetime_Trim()
        {
            lock (typeof(Obj)) // ensuring no concurrent tests are run
            {
                Obj.Reset();
                try
                {
                    var mcf = WeakMemoizationCacheFactory.CreateEvictedByLowest(e => e.HitCount, 10, 0.9, stopwatchFactory: null);

                    var cache = mcf.Create<Obj, string>(x => x.ToString(), MemoizationOptions.None);

                    var trim = cache.AsTrimmableByArgumentAndResult();
                    Assert.IsNotNull(trim);

                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));
                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));
                    Assert.IsTrue(GetOrAdd(cache).Contains("Obj"));
                    Assert.AreEqual(3, cache.Count);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    Assert.AreEqual(3, Obj.FinalizeCount);

                    Assert.IsTrue(cache.DebugView.Contains("(empty slot)"));
                    Assert.AreEqual(3, cache.Count);

                    trim.Trim(_ => false); // causes pruning

                    Assert.IsFalse(cache.DebugView.Contains("(empty slot)"));
                    Assert.AreEqual(0, cache.Count);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void WeakMemoizationCacheFactory_Evict_Dispose()
        {
            var mcf = WeakMemoizationCacheFactory.CreateEvictedByLowest(m => m.HitCount, 4, 1.0);

            var res = mcf.Create<string, string>(s => s + "!", MemoizationOptions.None);

            res.Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() => res.GetOrAdd("1"));
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static string GetOrAdd(IMemoizationCache<Obj, string> cache)
        {
            return cache.GetOrAdd(new Obj());
        }

        private sealed class Obj
        {
            public static int FinalizeCount;

            public static void Reset()
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                FinalizeCount = 0;
            }

            ~Obj()
            {
                Interlocked.Increment(ref FinalizeCount);
            }
        }
    }
}
