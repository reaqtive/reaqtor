// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/15/2014 - Wrote these tests.
//

using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PooledMemoryStreamTests : TestBase
    {
        private static readonly byte[] bytes = new byte[] { 0x42, 0xDE, 0xAD, 0x43 };

        [TestMethod]
        public void PooledMemoryStream_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => MemoryStreamPool.Create(4, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => MemoryStreamPool.Create(4, 16, -1));
        }

        [TestMethod]
        public void PooledMemoryStream_ManOrBoy()
        {
            PooledMemoryStream_ManOrBoy_Impl(false);
        }

        [TestMethod]
        public void PooledMemoryStream_ManOrBoy_RAII()
        {
            PooledMemoryStream_ManOrBoy_Impl(true);
        }

        private void PooledMemoryStream_ManOrBoy_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 1000, 10000, 100000 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, ms =>
                {
                    var N = 128;
                    var L = N;

                    for (var i = 0; i < N; i++)
                    {
                        ms.WriteByte(bytes[i % 4]);
                    }

                    return L;
                });
            }
        }


        [TestMethod]
        public void PooledMemoryStream_ManOrBoy_Random()
        {
            PooledMemoryStream_ManOrBoy_Random_Impl(false);
        }

        [TestMethod]
        public void PooledMemoryStream_ManOrBoy_Random_RAII()
        {
            PooledMemoryStream_ManOrBoy_Random_Impl(true);
        }

        private void PooledMemoryStream_ManOrBoy_Random_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 1000, 5000, 10000 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, ms =>
                {
                    var rand = GetRandom();

                    var L = rand.Next(1, 256);

                    for (var i = 0; i < L; i++)
                    {
                        ms.WriteByte((byte)rand.Next(0, 255));
                    }

                    return L;
                });
            }
        }

        private void Do(int C, int P, int M, bool noisy, bool useRAII, Func<MemoryStream, int> test)
        {
            var pool = MemoryStreamPool.Create(C);

            void testCore(MemoryStream ms)
            {
                var len = ms.Length;
                Assert.AreEqual(0, len);

                var L = test(ms);

                len = ms.Length;
                Assert.AreEqual(L, len);
            }

            if (useRAII)
            {
                Run(() => pool.New(), o => o.MemoryStream, o => o.Dispose(), testCore, P, M, noisy);
            }
            else
            {
                Run(() => pool.Allocate(), o => (MemoryStream)o, o => pool.Free(o), testCore, P, M, noisy);
            }
        }

        [TestMethod]
        public void PooledMemoryStream_Simple1()
        {
            var pool = MemoryStreamPool.Create(4);
            PooledMemoryStream_Simple_Impl(pool);
        }

        [TestMethod]
        public void PooledMemoryStream_Simple2()
        {
            var pool = MemoryStreamPool.Create(4, 16);
            PooledMemoryStream_Simple_Impl(pool);
        }

        private static void PooledMemoryStream_Simple_Impl(MemoryStreamPool pool)
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = i % 2 == 0 ? pool.New() : PooledMemoryStream.New(pool);

                var ms = obj.MemoryStream;

                Assert.AreEqual(0, ms.Length);

                ms.Write(bytes, 0, 4);
            }
        }

        [TestMethod]
        public void PooledMemoryStream_GlobalPool()
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = PooledMemoryStream.New();

                var ms = obj.MemoryStream;

                Assert.AreEqual(0, ms.Length);

                ms.Write(bytes, 0, 4);
            }
        }

        [TestMethod]
        public void PooledMemoryStream_GetInstanceAndConvert()
        {
            for (var i = 0; i < 100; i++)
            {
                var obj = PooledMemoryStream.GetInstance();
                var ms = (MemoryStream)obj;

                Assert.AreEqual(0, ms.Length);

                ms.Write(bytes, 0, 4);

                obj.Free();
            }
        }

        [TestMethod]
        public void PooledMemoryStream_GetBufferAndFree_ArgumentChecking()
        {
            var obj = PooledMemoryStream.GetInstance();
            Assert.ThrowsException<ArgumentNullException>(() => obj.GetBufferAndFree(processBuffer: null));
        }

        [TestMethod]
        public void PooledMemoryStream_GetBufferAndFree()
        {
            for (var i = 0; i < 100; i++)
            {
                var obj = PooledMemoryStream.GetInstance();
                var ms = (MemoryStream)obj;

                Assert.AreEqual(0, ms.Length);

                ms.Write(bytes, 0, 4);

                Assert.AreEqual(bytes.Length, ms.Length);

                obj.GetBufferAndFree(res =>
                {
                    var head = res.Take(bytes.Length);
                    Assert.IsTrue(bytes.SequenceEqual(head));

                    var tail = res.Skip(bytes.Length);
                    Assert.IsTrue(tail.All(b => b == 0));
                });
            }
        }

        [TestMethod]
        public void PooledMemoryStream_ToArray()
        {
            for (var i = 0; i < 100; i++)
            {
                var obj = PooledMemoryStream.GetInstance();
                var ms = (MemoryStream)obj;

                Assert.AreEqual(0, ms.Length);

                ms.Write(bytes, 0, 4);

                var res = obj.ToArray();

                Assert.IsTrue(bytes.SequenceEqual(res));
            }
        }

        [TestMethod]
        public void PooledMemoryStream_ToArrayAndFree()
        {
            for (var i = 0; i < 100; i++)
            {
                var obj = PooledMemoryStream.GetInstance();
                var ms = (MemoryStream)obj;

                Assert.AreEqual(0, ms.Length);

                ms.Write(bytes, 0, 4);

                var res = obj.ToArrayAndFree();

                Assert.IsTrue(bytes.SequenceEqual(res));
            }
        }

        [TestMethod]
        public void PooledMemoryStream_GetBuffer()
        {
            for (var i = 0; i < 100; i++)
            {
                var obj = PooledMemoryStream.GetInstance();
                var ms = (MemoryStream)obj;

                Assert.AreEqual(0, ms.Length);

                ms.Write(bytes, 0, 4);

                Assert.AreEqual(bytes.Length, ms.Length);
                var res = obj.GetBuffer();
                var head = res.Take(bytes.Length);
                Assert.IsTrue(bytes.SequenceEqual(head));

                var tail = res.Skip(bytes.Length);
                Assert.IsTrue(tail.All(b => b == 0));
            }
        }

        [TestMethod]
        public void PooledMemoryStream_GottenTooBig()
        {
            var bigPool = MemoryStreamPool.Create(1, 16, 2048);
            var smallPool = MemoryStreamPool.Create(1, 16, 16);
            TooBig(() => PooledMemoryStream.New(), h => h.MemoryStream, (h, n) => h.Write(new byte[n], 0, n), 1024);
            TooBig(() => bigPool.New(), h => h.MemoryStream, (h, n) => h.Write(new byte[n], 0, n), 2048);
            TooBig(() => smallPool.New(), h => h.MemoryStream, (h, n) => h.Write(new byte[n], 0, n), 16);
        }

        [TestMethod]
        public void PooledMemoryStream_DiscardIfNotWritable()
        {
            var i = 0;
            TooBig(() => PooledMemoryStream.New(), h => h.MemoryStream, (h, n) => { if (++i == 2) { h.Dispose(); } }, 16);
        }
    }
}
