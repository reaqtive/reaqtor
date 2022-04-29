// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Wrote these tests.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

#if !NET6_0
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Tests
{
    [TestClass]
    public class PooledHashSetTests : TestBase
    {
        private static readonly string[] exp = new[] { "qux", "foo", "bar", "baz" };

        [TestMethod]
        public void PooledHashSet_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => HashSetPool<string>.Create(4, comparer: null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => HashSetPool<string>.Create(4, EqualityComparer<string>.Default, -1));
        }

        [TestMethod]
        public void PooledHashSet_ManOrBoy()
        {
            PooledHashSet_ManOrBoy_Impl(false);
        }

        [TestMethod]
        public void PooledHashSet_ManOrBoy_RAII()
        {
            PooledHashSet_ManOrBoy_Impl(true);
        }

        private void PooledHashSet_ManOrBoy_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 1000, 5000, 10000 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, set =>
                {
                    var N = 128;

                    for (var i = 0; i < N; i++)
                    {
                        set.Add(i);
                    }

                    return N;
                });
            }
        }

        [TestMethod]
        public void PooledHashSet_ManOrBoy_Random()
        {
            PooledHashSet_ManOrBoy_Random_Impl(false);
        }

        [TestMethod]
        public void PooledHashSet_ManOrBoy_Random_RAII()
        {
            PooledHashSet_ManOrBoy_Random_Impl(true);
        }

        private void PooledHashSet_ManOrBoy_Random_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 100, 200, 500 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, set =>
                {
                    var rand = GetRandom();

                    var L = rand.Next(1, 128);

                    for (var i = 0; i < L; i++)
                    {
                        set.Add(i);
                    }

                    return L;
                });
            }
        }

        private void Do(int C, int P, int M, bool noisy, bool useRAII, Func<HashSet<int>, int> test)
        {
            var pool = HashSetPool<int>.Create(C);

            void testCore(HashSet<int> set)
            {
                var len = set.Count;
                Assert.AreEqual(0, len);

                var L = test(set);

                len = set.Count;
                Assert.AreEqual(L, len);
            }

            if (useRAII)
            {
                Run(() => pool.New(), o => o.HashSet, o => o.Dispose(), testCore, P, M, noisy);
            }
            else
            {
                Run(() => pool.Allocate(), o => o, o => pool.Free(o), testCore, P, M, noisy);
            }
        }

        [TestMethod]
        public void PooledHashSet_Simple1()
        {
            var pool = HashSetPool<string>.Create(4);
            PooledHashSet_Simple_Impl(pool);
        }

        [TestMethod]
        public void PooledHashSet_Simple2()
        {
            var pool = HashSetPool<string>.Create(4, EqualityComparer<string>.Default);
            PooledHashSet_Simple_Impl(pool);
        }

        private static void PooledHashSet_Simple_Impl(HashSetPool<string> pool)
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = i % 2 == 0 ? pool.New() : PooledHashSet<string>.New(pool);

                var set = obj.HashSet;

                Assert.AreEqual(0, set.Count);

                set.Add("qux");
                set.Add("foo");
                set.Add("bar");
                set.Add("baz");

                Assert.IsTrue(set.SetEquals(exp));
            }
        }

        [TestMethod]
        public void PooledHashSet_GlobalPool()
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = PooledHashSet<string>.New();

                var set = obj.HashSet;

                Assert.AreEqual(0, set.Count);

                set.Add("qux");
                set.Add("foo");
                set.Add("bar");
                set.Add("baz");

                Assert.IsTrue(set.SetEquals(exp));
            }
        }

        [TestMethod]
        public void PooledHashSet_GetInstance()
        {
            for (var i = 0; i < 100; i++)
            {
                var set = PooledHashSet<string>.GetInstance();

                Assert.AreEqual(0, set.Count);

                set.Add("qux");
                set.Add("foo");
                set.Add("bar");
                set.Add("baz");

                Assert.IsTrue(set.SetEquals(exp));

                set.Free();
            }
        }

#if !NET6_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void PooledHashSet_Serialization()
        {
            var ms = new MemoryStream();

            using (var obj = PooledHashSet<string>.New())
            {
                var set = obj.HashSet;

                set.Add("qux");
                set.Add("foo");
                set.Add("bar");
                set.Add("baz");

                var fmt = new BinaryFormatter();
                fmt.Serialize(ms, set);
            }

            ms.Position = 0;

            {
                var fmt = new BinaryFormatter();
                var set = (PooledHashSet<string>)fmt.Deserialize(ms);

                Assert.IsTrue(set.SetEquals(exp));

                set.Free(); // no-op but doesn't throw
            }
        }
#endif

        [TestMethod]
        public void PooledHashSet_GottenTooBig()
        {
            var bigPool = HashSetPool<int>.Create(1, EqualityComparer<int>.Default, 2048);
            var smallPool = HashSetPool<int>.Create(1, EqualityComparer<int>.Default, 16);
            TooBig(() => PooledHashSet<int>.New(), h => h.HashSet, (h, n) => h.UnionWith(Enumerable.Range(0, n)), 1024);
            TooBig(() => bigPool.New(), h => h.HashSet, (h, n) => h.UnionWith(Enumerable.Range(0, n)), 2048);
            TooBig(() => smallPool.New(), h => h.HashSet, (h, n) => h.UnionWith(Enumerable.Range(0, n)), 16);
        }
    }
}
