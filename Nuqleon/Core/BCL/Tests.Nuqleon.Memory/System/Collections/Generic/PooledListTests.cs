// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/16/2014 - Wrote these tests.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class PooledListTests : TestBase
    {
        private static readonly string[] exp = new[] { "qux", "foo", "bar", "baz" };

        [TestMethod]
        public void PooledList_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ListPool<int>.Create(4, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ListPool<int>.Create(4, 16, -1));
        }

        [TestMethod]
        public void PooledList_ManOrBoy()
        {
            PooledList_ManOrBoy_Impl(false);
        }

        [TestMethod]
        public void PooledList_ManOrBoy_RAII()
        {
            PooledList_ManOrBoy_Impl(true);
        }

        private void PooledList_ManOrBoy_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 1000, 5000, 10000 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, lst =>
                {
                    var N = 128;

                    for (var i = 0; i < N; i++)
                    {
                        lst.Add(i);
                    }

                    return N;
                });
            }
        }

        [TestMethod]
        public void PooledList_ManOrBoy_Random()
        {
            PooledList_ManOrBoy_Random_Impl(false);
        }

        [TestMethod]
        public void PooledList_ManOrBoy_Random_RAII()
        {
            PooledList_ManOrBoy_Random_Impl(true);
        }

        private void PooledList_ManOrBoy_Random_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 100, 200, 500 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, lst =>
                {
                    var rand = GetRandom();

                    var L = rand.Next(1, 128);

                    for (var i = 0; i < L; i++)
                    {
                        lst.Add(i);
                    }

                    return L;
                });
            }
        }

        private void Do(int C, int P, int M, bool noisy, bool useRAII, Func<List<int>, int> test)
        {
            var pool = ListPool<int>.Create(C);

            void testCore(List<int> lst)
            {
                var len = lst.Count;
                Assert.AreEqual(0, len);

                var L = test(lst);

                len = lst.Count;
                Assert.AreEqual(L, len);
            }

            if (useRAII)
            {
                Run(() => pool.New(), o => o.List, o => o.Dispose(), testCore, P, M, noisy);
            }
            else
            {
                Run(() => pool.Allocate(), o => o, o => pool.Free(o), testCore, P, M, noisy);
            }
        }

        [TestMethod]
        public void PooledList_Simple1()
        {
            var pool = ListPool<string>.Create(4);
            PooledList_Simple_Impl(pool);
        }

        [TestMethod]
        public void PooledList_Simple2()
        {
            var pool = ListPool<string>.Create(4, 16);
            PooledList_Simple_Impl(pool);
        }

        private static void PooledList_Simple_Impl(ListPool<string> pool)
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = i % 2 == 0 ? pool.New() : PooledList<string>.New(pool);

                var lst = obj.List;

                Assert.AreEqual(0, lst.Count);

                lst.Add("qux");
                lst.Add("foo");
                lst.Add("bar");
                lst.Add("baz");

                Assert.IsTrue(exp.SequenceEqual(lst));
            }
        }

        [TestMethod]
        public void PooledList_GlobalPool()
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = PooledList<string>.New();

                var lst = obj.List;

                Assert.AreEqual(0, lst.Count);

                lst.Add("qux");
                lst.Add("foo");
                lst.Add("bar");
                lst.Add("baz");

                Assert.IsTrue(exp.SequenceEqual(lst));
            }
        }

        [TestMethod]
        public void PooledList_GetInstance()
        {
            for (var i = 0; i < 100; i++)
            {
                var lst = PooledList<string>.GetInstance();

                Assert.AreEqual(0, lst.Count);

                lst.Add("qux");
                lst.Add("foo");
                lst.Add("bar");
                lst.Add("baz");

                Assert.IsTrue(exp.SequenceEqual(lst));

                lst.Free();
            }
        }

        [TestMethod]
        public void PooledList_AsReadOnly()
        {
            for (var i = 0; i < 100; i++)
            {
                var lst = PooledList<string>.GetInstance();

                Assert.AreEqual(0, lst.Count);

                lst.Add("qux");
                lst.Add("foo");
                lst.Add("bar");

#pragma warning disable 618
                _ = lst.AsReadOnly();
#pragma warning restore 618

                lst.Add("baz");

                Assert.IsTrue(exp.SequenceEqual(lst));
            }
        }

        [TestMethod]
        public void PooledList_AsReadOnlyCopy()
        {
            for (var i = 0; i < 100; i++)
            {
                var lst = PooledList<string>.GetInstance();

                Assert.AreEqual(0, lst.Count);

                lst.Add("qux");
                lst.Add("foo");
                lst.Add("bar");

                var res = lst.AsReadOnlyCopy();

                lst.Add("baz");

                Assert.IsTrue(exp.Take(3).SequenceEqual(res));
            }
        }

        [TestMethod]
        public void PooledList_AsReadOnlyCopyAndFree()
        {
            for (var i = 0; i < 100; i++)
            {
                var lst = PooledList<string>.GetInstance();

                Assert.AreEqual(0, lst.Count);

                lst.Add("qux");
                lst.Add("foo");
                lst.Add("bar");
                lst.Add("baz");

                var res = lst.AsReadOnlyCopyAndFree();

                Assert.IsTrue(exp.SequenceEqual(res));

                Assert.AreEqual(0, lst.Count); // Relies on implementation detail of timing of Free
            }
        }

        [TestMethod]
        public void PooledList_GottenTooBig()
        {
            var bigPool = ListPool<int>.Create(1, 16, 2048);
            var smallPool = ListPool<int>.Create(1, 16, 16);
            TooBig(() => PooledList<int>.New(), h => h.List, (h, n) => h.AddRange(Enumerable.Range(0, n)), 1024);
            TooBig(() => bigPool.New(), h => h.List, (h, n) => h.AddRange(Enumerable.Range(0, n)), 2048);
            TooBig(() => smallPool.New(), h => h.List, (h, n) => h.AddRange(Enumerable.Range(0, n)), 16);
        }
    }
}
