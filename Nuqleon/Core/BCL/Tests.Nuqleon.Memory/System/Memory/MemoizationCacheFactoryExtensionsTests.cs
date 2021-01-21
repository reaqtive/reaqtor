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
using System.Memory;
using System.Threading;

namespace Tests
{
    [TestClass]
    public class MemoizationCacheFactoryExtensionsTests
    {
        [TestMethod]
        public void MemoizationCacheFactoryExtensions_WithThreadLocal_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheFactoryExtensions.WithThreadLocal(factory: null));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheFactoryExtensions.WithThreadLocal(factory: null, exposeThreadLocalView: false));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheFactoryExtensions.WithThreadLocal(MemoizationCacheFactory.Unbounded).Create(default(Func<int, int>), MemoizationOptions.None, EqualityComparer<int>.Default));
        }

        [TestMethod]
        public void MemoizationCacheFactoryExtensions_WithThreadLocal_GlobalView()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded.WithThreadLocal(exposeThreadLocalView: false));

            var n = 0;
            var f = m.Memoize<int, int>(x => { Interlocked.Increment(ref n); return x + 1; });

            var b1_1 = new ManualResetEvent(false);
            var r1_1 = new ManualResetEvent(false);
            var b1_2 = new ManualResetEvent(false);
            var r1_2 = new ManualResetEvent(false);
            var b1_3 = new ManualResetEvent(false);
            var r1_3 = new ManualResetEvent(false);

            var d1 = default(string);
            var d2 = default(string);

            var t1 = new Thread(() =>
            {
                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(2, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_1.Set();
                r1_1.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                b1_2.Set();
                r1_2.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(5, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(6, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_3.Set();
                r1_3.WaitOne();

                d1 = f.Cache.DebugView;
            });

            t1.Start();
            b1_1.WaitOne();

            var b2_1 = new ManualResetEvent(false);
            var r2_1 = new ManualResetEvent(false);
            var b2_2 = new ManualResetEvent(false);
            var r2_2 = new ManualResetEvent(false);
            var b2_3 = new ManualResetEvent(false);
            var r2_3 = new ManualResetEvent(false);

            var t2 = new Thread(() =>
            {
                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(3, n);
                Assert.AreEqual(3, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                b2_1.Set();
                r2_1.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                b2_2.Set();
                r2_2.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(7, n);
                Assert.AreEqual(3, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(8, n);
                Assert.AreEqual(4, f.Cache.Count);

                b2_3.Set();
                r2_3.WaitOne();

                d2 = f.Cache.DebugView;
            });

            t2.Start();
            b2_1.WaitOne();

            r1_1.Set();
            b1_2.WaitOne();

            r2_1.Set();
            b2_2.WaitOne();

            f.Cache.Clear();

            r1_2.Set();
            b1_3.WaitOne();

            r2_2.Set();
            b2_3.WaitOne();

            r1_3.Set();
            t1.Join();

            r2_3.Set();
            t2.Join();

            Assert.AreEqual(d1, d2);
            Assert.AreEqual(d1, f.Cache.DebugView);
        }

        [TestMethod]
        public void MemoizationCacheFactoryExtensions_WithThreadLocal_GlobalView_Trim()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded.WithThreadLocal(exposeThreadLocalView: false));

            var n = 0;
            var f = m.Memoize<int, int>(x => { Interlocked.Increment(ref n); return x + 1; });

            var t = f.Cache.AsTrimmableByArgumentAndResult<int, int>();

            var b1_1 = new ManualResetEvent(false);
            var r1_1 = new ManualResetEvent(false);
            var b1_2 = new ManualResetEvent(false);
            var r1_2 = new ManualResetEvent(false);
            var b1_3 = new ManualResetEvent(false);
            var r1_3 = new ManualResetEvent(false);

            var d1 = default(int);
            var d2 = default(int);

            var t1 = new Thread(() =>
            {
                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(2, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_1.Set();
                r1_1.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                b1_2.Set();
                r1_2.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(5, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(6, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_3.Set();
                r1_3.WaitOne();

                d1 = f.Cache.Count;
            });

            t1.Start();
            b1_1.WaitOne();

            var b2_1 = new ManualResetEvent(false);
            var r2_1 = new ManualResetEvent(false);
            var b2_2 = new ManualResetEvent(false);
            var r2_2 = new ManualResetEvent(false);
            var b2_3 = new ManualResetEvent(false);
            var r2_3 = new ManualResetEvent(false);

            var t2 = new Thread(() =>
            {
                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(3, n);
                Assert.AreEqual(3, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                b2_1.Set();
                r2_1.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                b2_2.Set();
                r2_2.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(7, n);
                Assert.AreEqual(3, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(8, n);
                Assert.AreEqual(4, f.Cache.Count);

                b2_3.Set();
                r2_3.WaitOne();

                d2 = f.Cache.Count;
            });

            t2.Start();
            b2_1.WaitOne();

            r1_1.Set();
            b1_2.WaitOne();

            r2_1.Set();
            b2_2.WaitOne();

            f.Cache.Clear();

            r1_2.Set();
            b1_3.WaitOne();

            r2_2.Set();
            b2_3.WaitOne();

            t.Trim(_ => true);

            r1_3.Set();
            t1.Join();

            r2_3.Set();
            t2.Join();

            Assert.AreEqual(0, d1);
            Assert.AreEqual(d1, d2);
        }

        [TestMethod]
        public void MemoizationCacheFactoryExtensions_WithThreadLocal_ThreadLocalView()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded.WithThreadLocal());

            var n = 0;
            var f = m.Memoize<int, int>(x => { Interlocked.Increment(ref n); return x + 1; });

            var b1_1 = new ManualResetEvent(false);
            var r1_1 = new ManualResetEvent(false);
            var b1_2 = new ManualResetEvent(false);
            var r1_2 = new ManualResetEvent(false);
            var b1_3 = new ManualResetEvent(false);
            var r1_3 = new ManualResetEvent(false);

            var d1 = default(string);
            var d2 = default(string);

            var t1 = new Thread(() =>
            {
                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(2, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_1.Set();
                r1_1.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                f.Cache.Clear();
                Assert.AreEqual(0, f.Cache.Count);

                b1_2.Set();
                r1_2.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(5, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(6, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_3.Set();
                r1_3.WaitOne();

                d1 = f.Cache.DebugView;
            });

            t1.Start();
            b1_1.WaitOne();

            var b2_1 = new ManualResetEvent(false);
            var r2_1 = new ManualResetEvent(false);
            var b2_2 = new ManualResetEvent(false);
            var r2_2 = new ManualResetEvent(false);
            var b2_3 = new ManualResetEvent(false);
            var r2_3 = new ManualResetEvent(false);

            var t2 = new Thread(() =>
            {
                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(3, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                b2_1.Set();
                r2_1.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                f.Cache.Clear();
                Assert.AreEqual(0, f.Cache.Count);

                b2_2.Set();
                r2_2.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(7, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(8, n);
                Assert.AreEqual(2, f.Cache.Count);

                b2_3.Set();
                r2_3.WaitOne();

                d2 = f.Cache.DebugView;
            });

            t2.Start();
            b2_1.WaitOne();

            r1_1.Set();
            b1_2.WaitOne();

            r2_1.Set();
            b2_2.WaitOne();

            f.Cache.Clear();

            r1_2.Set();
            b1_3.WaitOne();

            r2_2.Set();
            b2_3.WaitOne();

            r1_3.Set();
            t1.Join();

            r2_3.Set();
            t2.Join();

            Assert.AreEqual(d1, d2);
            Assert.AreNotEqual(d1, f.Cache.DebugView);
        }

        [TestMethod]
        public void MemoizationCacheFactoryExtensions_WithThreadLocal_ThreadLocalView_Trim()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded.WithThreadLocal());

            var n = 0;
            var f = m.Memoize<int, int>(x => { Interlocked.Increment(ref n); return x + 1; });

            var b1_1 = new ManualResetEvent(false);
            var r1_1 = new ManualResetEvent(false);
            var b1_2 = new ManualResetEvent(false);
            var r1_2 = new ManualResetEvent(false);
            var b1_3 = new ManualResetEvent(false);
            var r1_3 = new ManualResetEvent(false);

            var t1 = new Thread(() =>
            {
                var t = f.Cache.AsTrimmableByArgumentAndResult<int, int>();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(2, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_1.Set();
                r1_1.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                f.Cache.Clear();
                Assert.AreEqual(0, f.Cache.Count);

                b1_2.Set();
                r1_2.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(5, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(6, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_3.Set();
                r1_3.WaitOne();

                Assert.AreEqual(2, f.Cache.Count);

                t.Trim(_ => true);
                Assert.AreEqual(0, f.Cache.Count);
            });

            t1.Start();
            b1_1.WaitOne();

            var b2_1 = new ManualResetEvent(false);
            var r2_1 = new ManualResetEvent(false);
            var b2_2 = new ManualResetEvent(false);
            var r2_2 = new ManualResetEvent(false);
            var b2_3 = new ManualResetEvent(false);
            var r2_3 = new ManualResetEvent(false);

            var t2 = new Thread(() =>
            {
                var t = f.Cache.AsTrimmableByArgumentAndResult<int, int>();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(3, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                b2_1.Set();
                r2_1.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                f.Cache.Clear();
                Assert.AreEqual(0, f.Cache.Count);

                b2_2.Set();
                r2_2.WaitOne();

                Assert.AreEqual(2, f.Delegate(1));
                Assert.AreEqual(7, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual(3, f.Delegate(2));
                Assert.AreEqual(8, n);
                Assert.AreEqual(2, f.Cache.Count);

                b2_3.Set();
                r2_3.WaitOne();

                Assert.AreEqual(2, f.Cache.Count);

                t.Trim(_ => true);
                Assert.AreEqual(0, f.Cache.Count);
            });

            t2.Start();
            b2_1.WaitOne();

            r1_1.Set();
            b1_2.WaitOne();

            r2_1.Set();
            b2_2.WaitOne();

            f.Cache.Clear();

            r1_2.Set();
            b1_3.WaitOne();

            r2_2.Set();
            b2_3.WaitOne();

            r1_3.Set();
            t1.Join();

            r2_3.Set();
            t2.Join();
        }

        [TestMethod]
        public void MemoizationCacheFactoryExtensions_WithThreadLocal_Dispose()
        {
            foreach (var b in new[] { true, false })
            {
                var c = MemoizationCacheFactory.Unbounded.WithThreadLocal(b).Create<int, int>(x => x, MemoizationOptions.None, EqualityComparer<int>.Default);

                Assert.AreEqual(1, c.GetOrAdd(1));

                c.Dispose();

                Assert.ThrowsException<ObjectDisposedException>(() => c.GetOrAdd(2));
            }

            {
                var c = MemoizationCacheFactory.Unbounded.WithThreadLocal().Create<int, int>(x => x, MemoizationOptions.None, EqualityComparer<int>.Default);

                Assert.AreEqual(1, c.GetOrAdd(1));

                c.Dispose();

                Assert.ThrowsException<ObjectDisposedException>(() => c.GetOrAdd(2));
            }
        }

        [TestMethod]
        public void MemoizationCacheFactoryExtensions_Synchronized_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheFactoryExtensions.Synchronized(factory: null));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheFactoryExtensions.Synchronized(MemoizationCacheFactory.Unbounded).Create(default(Func<int, int>), MemoizationOptions.None, EqualityComparer<int>.Default));
        }

        [TestMethod]
        public void MemoizationCacheFactoryExtensions_Synchronized_Simple()
        {
            var f = new Func<int, int>(x => x * 2);

            using var c = MemoizationCacheFactory.Unbounded.Synchronized().Create(f, MemoizationOptions.None, EqualityComparer<int>.Default);

            Assert.AreEqual(0, c.Count);

            Assert.AreEqual(42, c.GetOrAdd(21));
            Assert.AreEqual(42, c.GetOrAdd(21));
            Assert.AreEqual(1, c.Count);

            Assert.IsTrue(!string.IsNullOrEmpty(c.DebugView));

            c.Clear();
            Assert.AreEqual(0, c.Count);
        }

        [TestMethod]
        public void MemoizationCacheFactoryExtensions_Synchronized_Trim()
        {
            var f = new Func<int, int>(x => x * 2);

            using var c = MemoizationCacheFactory.Unbounded.Synchronized().Create(f, MemoizationOptions.None, EqualityComparer<int>.Default);

            var trim = c.AsTrimmableByArgumentAndResult<int, int>();

            Assert.AreEqual(0, c.Count);

            Assert.AreEqual(42, c.GetOrAdd(21));
            Assert.AreEqual(42, c.GetOrAdd(21));
            Assert.AreEqual(1, c.Count);

            trim.Trim(_ => true);
            Assert.AreEqual(0, c.Count);
        }

        [TestMethod]
        public void MemoizationCacheFactoryExtensions_Synchronized_NonBlockingFunctionInvocation()
        {
            var e1 = new ManualResetEvent(false);
            var e2 = new ManualResetEvent(false);

            var f = new Func<int, int>(x =>
            {
                if (x == 0)
                {
                    e1.Set();
                    e2.WaitOne();
                    return 1;
                }

                return x * 2;
            });

            using var c = MemoizationCacheFactory.Unbounded.Synchronized().Create(f, MemoizationOptions.None, EqualityComparer<int>.Default);

            var t = new Thread(() =>
            {
                Assert.AreEqual(1, c.GetOrAdd(0));
            });

            t.Start();
            e1.WaitOne();

            Assert.AreEqual(2, c.GetOrAdd(1));

            e2.Set();
            t.Join();
        }
    }
}
