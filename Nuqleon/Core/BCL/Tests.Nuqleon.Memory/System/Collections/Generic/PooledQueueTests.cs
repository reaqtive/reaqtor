// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/16/2014 - Wrote these tests.
//

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PooledQueueTests : TestBase
    {
        private static readonly string[] exp = new[] { "qux", "foo", "bar", "baz" };

        [TestMethod]
        public void PooledQueue_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => QueuePool<int>.Create(4, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => QueuePool<int>.Create(4, 16, -1));
        }

        [TestMethod]
        public void PooledQueue_ManOrBoy()
        {
            PooledQueue_ManOrBoy_Impl(false);
        }

        [TestMethod]
        public void PooledQueue_ManOrBoy_RAII()
        {
            PooledQueue_ManOrBoy_Impl(true);
        }

        private void PooledQueue_ManOrBoy_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 1000, 5000, 10000 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, queue =>
                {
                    var N = 128;

                    for (var i = 0; i < N; i++)
                    {
                        queue.Enqueue(i);
                    }

                    return N;
                });
            }
        }

        [TestMethod]
        public void PooledQueue_ManOrBoy_Random()
        {
            PooledQueue_ManOrBoy_Random_Impl(false);
        }

        [TestMethod]
        public void PooledQueue_ManOrBoy_Random_RAII()
        {
            PooledQueue_ManOrBoy_Random_Impl(true);
        }

        private void PooledQueue_ManOrBoy_Random_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 100, 200, 500 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, queue =>
                {
                    var rand = GetRandom();

                    var L = rand.Next(1, 128);

                    for (var i = 0; i < L; i++)
                    {
                        queue.Enqueue(i);
                    }

                    return L;
                });
            }
        }

        private void Do(int C, int P, int M, bool noisy, bool useRAII, Func<Queue<int>, int> test)
        {
            var pool = QueuePool<int>.Create(C);

            void testCore(Queue<int> queue)
            {
                var len = queue.Count;
                Assert.AreEqual(0, len);

                var L = test(queue);

                len = queue.Count;
                Assert.AreEqual(L, len);
            }

            if (useRAII)
            {
                Run(() => pool.New(), o => o.Queue, o => o.Dispose(), testCore, P, M, noisy);
            }
            else
            {
                Run(() => pool.Allocate(), o => o, o => pool.Free(o), testCore, P, M, noisy);
            }
        }

        [TestMethod]
        public void PooledQueue_Simple1()
        {
            var pool = QueuePool<string>.Create(4);
            PooledQueue_Simple_Impl(pool);
        }

        [TestMethod]
        public void PooledQueue_Simple2()
        {
            var pool = QueuePool<string>.Create(4, 16);
            PooledQueue_Simple_Impl(pool);
        }

        private static void PooledQueue_Simple_Impl(QueuePool<string> pool)
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = i % 2 == 0 ? pool.New() : PooledQueue<string>.New(pool);

                var queue = obj.Queue;

                Assert.AreEqual(0, queue.Count);

                queue.Enqueue("qux");
                queue.Enqueue("foo");
                queue.Enqueue("bar");
                queue.Enqueue("baz");

                Assert.IsTrue(exp.SequenceEqual(queue));
            }
        }

        [TestMethod]
        public void PooledQueue_GlobalPool()
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = PooledQueue<string>.New();

                var queue = obj.Queue;

                Assert.AreEqual(0, queue.Count);

                queue.Enqueue("qux");
                queue.Enqueue("foo");
                queue.Enqueue("bar");
                queue.Enqueue("baz");

                Assert.IsTrue(exp.SequenceEqual(queue));
            }
        }

        [TestMethod]
        public void PooledQueue_GetInstance()
        {
            for (var i = 0; i < 100; i++)
            {
                var queue = PooledQueue<string>.GetInstance();

                Assert.AreEqual(0, queue.Count);

                queue.Enqueue("qux");
                queue.Enqueue("foo");
                queue.Enqueue("bar");
                queue.Enqueue("baz");

                Assert.IsTrue(exp.SequenceEqual(queue));

                queue.Free();
            }
        }

        [TestMethod]
        public void PooledQueue_GottenTooBig()
        {
            var bigPool = QueuePool<int>.Create(1, 16, 2048);
            var smallPool = QueuePool<int>.Create(1, 16, 16);
            TooBig(() => PooledQueue<int>.New(), h => h.Queue, (h, n) => { for (var i = 0; i < n; i++) h.Enqueue(i); }, 1024);
            TooBig(() => bigPool.New(), h => h.Queue, (h, n) => { for (var i = 0; i < n; i++) h.Enqueue(i); }, 2048);
            TooBig(() => smallPool.New(), h => h.Queue, (h, n) => { for (var i = 0; i < n; i++) h.Enqueue(i); }, 16);
        }
    }
}
