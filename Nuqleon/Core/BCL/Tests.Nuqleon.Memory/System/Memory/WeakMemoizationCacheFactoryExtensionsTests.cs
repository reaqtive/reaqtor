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
using System.Memory;
using System.Threading;

namespace Tests
{
    [TestClass]
    public class WeakMemoizationCacheFactoryExtensionsTests
    {
        [TestMethod]
        public void WeakMemoizationCacheFactoryExtensions_WithThreadLocal_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactoryExtensions.WithThreadLocal(factory: null));
            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactoryExtensions.WithThreadLocal(WeakMemoizationCacheFactory.Unbounded).Create(default(Func<string, string>), MemoizationOptions.None));
            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactoryExtensions.WithThreadLocal(factory: null, exposeThreadLocalView: false));
            Assert.ThrowsException<ArgumentNullException>(() => WeakMemoizationCacheFactoryExtensions.WithThreadLocal(WeakMemoizationCacheFactory.Unbounded, false).Create(default(Func<string, string>), MemoizationOptions.None));
        }

        [TestMethod]
        public void WeakMemoizationCacheFactoryExtensions_WithThreadLocal_GlobalView()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded.WithThreadLocal(exposeThreadLocalView: false));

            var n = 0;
            var f = m.MemoizeWeak<string, string>(x => { Interlocked.Increment(ref n); return x + "!"; });

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
                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
                Assert.AreEqual(2, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_1.Set();
                r1_1.WaitOne();

                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                b1_2.Set();
                r1_2.WaitOne();

                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(5, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
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
                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(3, n);
                Assert.AreEqual(3, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                b2_1.Set();
                r2_1.WaitOne();

                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(4, f.Cache.Count);

                b2_2.Set();
                r2_2.WaitOne();

                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(7, n);
                Assert.AreEqual(3, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
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
        public void WeakMemoizationCacheFactoryExtensions_WithThreadLocal_ThreadLocalView()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded.WithThreadLocal());

            var n = 0;
            var f = m.MemoizeWeak<string, string>(x => { Interlocked.Increment(ref n); return x + "!"; });

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
                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
                Assert.AreEqual(2, n);
                Assert.AreEqual(2, f.Cache.Count);

                b1_1.Set();
                r1_1.WaitOne();

                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                f.Cache.Clear();
                Assert.AreEqual(0, f.Cache.Count);

                b1_2.Set();
                r1_2.WaitOne();

                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(5, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
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
                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(3, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                b2_1.Set();
                r2_1.WaitOne();

                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
                Assert.AreEqual(4, n);
                Assert.AreEqual(2, f.Cache.Count);

                f.Cache.Clear();
                Assert.AreEqual(0, f.Cache.Count);

                b2_2.Set();
                r2_2.WaitOne();

                Assert.AreEqual("1!", f.Delegate("1"));
                Assert.AreEqual(7, n);
                Assert.AreEqual(1, f.Cache.Count);

                Assert.AreEqual("2!", f.Delegate("2"));
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

#if !DEBUG // contains local timings
            Assert.AreEqual(d1, d2);
            Assert.AreNotEqual(d1, f.Cache.DebugView);
#endif
        }
    }
}
