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
using System.Linq;
using System.Text;

namespace Tests
{
    [TestClass]
    public class PooledStringBuilderTests : TestBase
    {
        private static readonly string[] words = new string[] { "bar", "foo", "qux", "baz" };

        [TestMethod]
        public void PooledStringBuilder_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringBuilderPool.Create(4, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringBuilderPool.Create(4, 16, -1));
        }

        [TestMethod]
        public void PooledStringBuilder_ManOrBoy()
        {
            PooledStringBuilder_ManOrBoy_Impl(false);
        }

        [TestMethod]
        public void PooledStringBuilder_ManOrBoy_RAII()
        {
            PooledStringBuilder_ManOrBoy_Impl(true);
        }

        private void PooledStringBuilder_ManOrBoy_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 1000, 10000, 100000 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, sb =>
                {
                    var N = 128 / 3;
                    var L = N * 3;

                    for (var i = 0; i < N; i++)
                    {
                        sb.Append(words[i % 4]);
                    }

                    return L;
                });
            }
        }


        [TestMethod]
        public void PooledStringBuilder_ManOrBoy_Random()
        {
            PooledStringBuilder_ManOrBoy_Random_Impl(false);
        }

        [TestMethod]
        public void PooledStringBuilder_ManOrBoy_Random_RAII()
        {
            PooledStringBuilder_ManOrBoy_Random_Impl(true);
        }

        private void PooledStringBuilder_ManOrBoy_Random_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 1000, 5000, 10000 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, sb =>
                {
                    var rand = GetRandom();

                    var L = rand.Next(1, 256);

                    for (var i = 0; i < L; i++)
                    {
                        sb.Append((char)rand.Next(32, 127));
                    }

                    return L;
                });
            }
        }

        private void Do(int C, int P, int M, bool noisy, bool useRAII, Func<StringBuilder, int> test)
        {
            var pool = StringBuilderPool.Create(C);

            void testCore(StringBuilder sb)
            {
                var len = sb.Length;
                Assert.AreEqual(0, len);

                var L = test(sb);

                len = sb.Length;
                Assert.AreEqual(L, len);
            }

            if (useRAII)
            {
                Run(() => pool.New(), o => o.StringBuilder, o => o.Dispose(), testCore, P, M, noisy);
            }
            else
            {
                Run(() => pool.Allocate(), o => o.StringBuilder, o => pool.Free(o), testCore, P, M, noisy);
            }
        }

        [TestMethod]
        public void PooledStringBuilder_Simple1()
        {
            var pool = StringBuilderPool.Create(4);
            PooledStringBuilder_Simple_Impl(pool);
        }

        [TestMethod]
        public void PooledStringBuilder_Simple2()
        {
            var pool = StringBuilderPool.Create(4, 16);
            PooledStringBuilder_Simple_Impl(pool);
        }

        private static void PooledStringBuilder_Simple_Impl(StringBuilderPool pool)
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = i % 2 == 0 ? pool.New() : PooledStringBuilder.New(pool);

                var sb = obj.StringBuilder;

                Assert.AreEqual(0, sb.Length);

                sb.Append("qux foo bar baz");
            }
        }

        [TestMethod]
        public void PooledStringBuilder_GlobalPool()
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = PooledStringBuilder.New();

                var sb = obj.StringBuilder;

                Assert.AreEqual(0, sb.Length);

                sb.Append("qux foo bar baz");
            }
        }

        [TestMethod]
        public void PooledStringBuilder_GetInstanceAndConvert()
        {
            for (var i = 0; i < 100; i++)
            {
                var obj = PooledStringBuilder.GetInstance();
                var sb = (StringBuilder)obj;

                Assert.AreEqual(0, sb.Length);

                sb.Append("qux foo bar baz");

                obj.Free();
            }
        }

        [TestMethod]
        public void PooledStringBuilder_ToStringAndFree()
        {
            for (var i = 0; i < 100; i++)
            {
                var obj = PooledStringBuilder.GetInstance();
                var sb = (StringBuilder)obj;

                Assert.AreEqual(0, sb.Length);

                sb.Append("qux foo bar baz");

                var res = obj.ToStringAndFree();

                Assert.AreEqual("qux foo bar baz", res);

                Assert.AreEqual(0, sb.Length); // Relies on implementation detail of timing of Free
            }
        }

        [TestMethod]
        public void PooledStringBuilder_ToString()
        {
            for (var i = 0; i < 100; i++)
            {
                var obj = PooledStringBuilder.GetInstance();
                var sb = (StringBuilder)obj;

                Assert.AreEqual(0, sb.Length);

                sb.Append("qux foo bar baz");

#pragma warning disable 618
                var res = obj.ToString();
#pragma warning restore 618

                Assert.AreEqual("qux foo bar baz", res);
            }
        }

        [TestMethod]
        public void PooledStringBuilder_GottenTooBig()
        {
            var bigPool = StringBuilderPool.Create(1, 16, 2048);
            var smallPool = StringBuilderPool.Create(1, 16, 16);
            TooBig(() => PooledStringBuilder.New(), h => h.StringBuilder, (h, n) => h.Append(new string('*', n)), 1024);
            TooBig(() => bigPool.New(), h => h.StringBuilder, (h, n) => h.Append(new string('*', n)), 2048);
            TooBig(() => smallPool.New(), h => h.StringBuilder, (h, n) => h.Append(new string('*', n)), 16);
        }
    }
}
