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

#if !NET5_0
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Tests
{
    [TestClass]
    public class PooledDictionaryTests : TestBase
    {
        [TestMethod]
        public void PooledDictionary_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => DictionaryPool<string, int>.Create(4, comparer: null));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => DictionaryPool<string, int>.Create(4, -1));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => DictionaryPool<string, int>.Create(4, -1, EqualityComparer<string>.Default));
            Assert.ThrowsException<ArgumentNullException>(() => DictionaryPool<string, int>.Create(4, 16, comparer: null));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => DictionaryPool<string, int>.Create(4, 16, EqualityComparer<string>.Default, -1));
        }

        [TestMethod]
        public void PooledDictionary_ManOrBoy()
        {
            PooledDictionary_ManOrBoy_Impl(false);
        }

        [TestMethod]
        public void PooledDictionary_ManOrBoy_RAII()
        {
            PooledDictionary_ManOrBoy_Impl(true);
        }

        private void PooledDictionary_ManOrBoy_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 1000, 5000, 10000 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, map =>
                {
                    var N = 128;

                    for (var i = 0; i < N; i++)
                    {
                        map.Add(i.ToString(), i);
                    }

                    return N;
                });
            }
        }

        [TestMethod]
        public void PooledDictionary_ManOrBoy_Random()
        {
            PooledDictionary_ManOrBoy_Random_Impl(false);
        }

        [TestMethod]
        public void PooledDictionary_ManOrBoy_Random_RAII()
        {
            PooledDictionary_ManOrBoy_Random_Impl(true);
        }

        private void PooledDictionary_ManOrBoy_Random_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 100, 200, 500 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, map =>
                {
                    var rand = GetRandom();

                    var L = rand.Next(1, 128);

                    for (var i = 0; i < L; i++)
                    {
                        map.Add(i.ToString(), i);
                    }

                    return L;
                });
            }
        }

        private void Do(int C, int P, int M, bool noisy, bool useRAII, Func<Dictionary<string, int>, int> test)
        {
            var pool = DictionaryPool<string, int>.Create(C);

            void testCore(Dictionary<string, int> map)
            {
                var len = map.Count;
                Assert.AreEqual(0, len);

                var L = test(map);

                len = map.Count;
                Assert.AreEqual(L, len);
            }

            if (useRAII)
            {
                Run(() => pool.New(), o => o.Dictionary, o => o.Dispose(), testCore, P, M, noisy);
            }
            else
            {
                Run(() => pool.Allocate(), o => o, o => pool.Free(o), testCore, P, M, noisy);
            }
        }

        [TestMethod]
        public void PooledDictionary_Simple1()
        {
            var pool = DictionaryPool<string, int>.Create(4);
            PooledDictionary_Simple_Impl(pool);
        }

        [TestMethod]
        public void PooledDictionary_Simple2()
        {
            var pool = DictionaryPool<string, int>.Create(4, EqualityComparer<string>.Default);
            PooledDictionary_Simple_Impl(pool);
        }

        [TestMethod]
        public void PooledDictionary_Simple3()
        {
            var pool = DictionaryPool<string, int>.Create(4, 16);
            PooledDictionary_Simple_Impl(pool);
        }

        [TestMethod]
        public void PooledDictionary_Simple4()
        {
            var pool = DictionaryPool<string, int>.Create(4, 16, EqualityComparer<string>.Default);
            PooledDictionary_Simple_Impl(pool);
        }

        private static void PooledDictionary_Simple_Impl(DictionaryPool<string, int> pool)
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = i % 2 == 0 ? pool.New() : PooledDictionary<string, int>.New(pool);

                var map = obj.Dictionary;

                Assert.AreEqual(0, map.Count);

                map.Add("qux", 42);
                map.Add("foo", 43);
                map.Add("bar", 44);
                map.Add("baz", 45);
            }
        }

        [TestMethod]
        public void PooledDictionary_GlobalPool()
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = PooledDictionary<string, int>.New();

                var map = obj.Dictionary;

                Assert.AreEqual(0, map.Count);

                map.Add("qux", 42);
                map.Add("foo", 43);
                map.Add("bar", 44);
                map.Add("baz", 45);
            }
        }

        [TestMethod]
        public void PooledDictionary_GetInstance()
        {
            for (var i = 0; i < 100; i++)
            {
                var map = PooledDictionary<string, int>.GetInstance();

                Assert.AreEqual(0, map.Count);

                map.Add("qux", 42);
                map.Add("foo", 43);
                map.Add("bar", 44);
                map.Add("baz", 45);

                map.Free();
            }
        }

#if !NET5_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void PooledDictionary_Serialization()
        {
            var ms = new MemoryStream();

            using (var obj = PooledDictionary<string, int>.New())
            {
                var map = obj.Dictionary;

                map.Add("qux", 42);
                map.Add("foo", 43);
                map.Add("bar", 44);
                map.Add("baz", 45);

                var fmt = new BinaryFormatter();
                fmt.Serialize(ms, map);
            }

            ms.Position = 0;

            {
                var fmt = new BinaryFormatter();
                var map = (PooledDictionary<string, int>)fmt.Deserialize(ms);

                Assert.IsTrue(map.TryGetValue("qux", out var val1) && val1 == 42);
                Assert.IsTrue(map.TryGetValue("foo", out var val2) && val2 == 43);
                Assert.IsTrue(map.TryGetValue("bar", out var val3) && val3 == 44);
                Assert.IsTrue(map.TryGetValue("baz", out var val4) && val4 == 45);

                map.Free(); // no-op but doesn't throw
            }
        }
#endif

        [TestMethod]
        public void PooledDictionary_GottenTooBig()
        {
            var bigPool = DictionaryPool<int, string>.Create(1, 16, EqualityComparer<int>.Default, 2048);
            var smallPool = DictionaryPool<int, string>.Create(1, 16, EqualityComparer<int>.Default, 16);
            TooBig(() => PooledDictionary<int, string>.New(), h => h.Dictionary, (h, n) => { for (var i = 0; i < n; i++) h.Add(i, ""); }, 1024);
            TooBig(() => bigPool.New(), h => h.Dictionary, (h, n) => { for (var i = 0; i < n; i++) h.Add(i, ""); }, 2048);
            TooBig(() => smallPool.New(), h => h.Dictionary, (h, n) => { for (var i = 0; i < n; i++) h.Add(i, ""); }, 16);
        }
    }
}
