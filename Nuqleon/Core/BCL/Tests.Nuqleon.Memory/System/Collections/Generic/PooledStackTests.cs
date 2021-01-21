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
    public class PooledStackTests : TestBase
    {
        private static readonly string[] exp = new[] { "qux", "foo", "bar", "baz" }.Reverse().ToArray();

        [TestMethod]
        public void PooledStack_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StackPool<int>.Create(4, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StackPool<int>.Create(4, 16, -1));
        }

        [TestMethod]
        public void PooledStack_ManOrBoy()
        {
            PooledStack_ManOrBoy_Impl(false);
        }

        [TestMethod]
        public void PooledStack_ManOrBoy_RAII()
        {
            PooledStack_ManOrBoy_Impl(true);
        }

        private void PooledStack_ManOrBoy_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 1000, 5000, 10000 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, stack =>
                {
                    var N = 128;

                    for (var i = 0; i < N; i++)
                    {
                        stack.Push(i);
                    }

                    return N;
                });
            }
        }

        [TestMethod]
        public void PooledStack_ManOrBoy_Random()
        {
            PooledStack_ManOrBoy_Random_Impl(false);
        }

        [TestMethod]
        public void PooledStack_ManOrBoy_Random_RAII()
        {
            PooledStack_ManOrBoy_Random_Impl(true);
        }

        private void PooledStack_ManOrBoy_Random_Impl(bool useRAII)
        {
            var res = from C in new[] { 4, 8, 16, 32, 64 }
                      from P in Enumerable.Range(1, Math.Min(Environment.ProcessorCount * 2, 8))
                      from M in new[] { 100, 200, 500 }
                      select (C, P, M);

            foreach (var cpm in res.Trim())
            {
                Do(cpm.C, cpm.P, cpm.M, false, useRAII, stack =>
                {
                    var rand = GetRandom();

                    var L = rand.Next(1, 128);

                    for (var i = 0; i < L; i++)
                    {
                        stack.Push(i);
                    }

                    return L;
                });
            }
        }

        private void Do(int C, int P, int M, bool noisy, bool useRAII, Func<Stack<int>, int> test)
        {
            var pool = StackPool<int>.Create(C);

            void testCore(Stack<int> stack)
            {
                var len = stack.Count;
                Assert.AreEqual(0, len);

                var L = test(stack);

                len = stack.Count;
                Assert.AreEqual(L, len);
            }

            if (useRAII)
            {
                Run(() => pool.New(), o => o.Stack, o => o.Dispose(), testCore, P, M, noisy);
            }
            else
            {
                Run(() => pool.Allocate(), o => o, o => pool.Free(o), testCore, P, M, noisy);
            }
        }

        [TestMethod]
        public void PooledStack_Simple1()
        {
            var pool = StackPool<string>.Create(4);
            PooledStack_Simple_Impl(pool);
        }

        [TestMethod]
        public void PooledStack_Simple2()
        {
            var pool = StackPool<string>.Create(4, 16);
            PooledStack_Simple_Impl(pool);
        }

        private static void PooledStack_Simple_Impl(StackPool<string> pool)
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = i % 2 == 0 ? pool.New() : PooledStack<string>.New(pool);

                var stack = obj.Stack;

                Assert.AreEqual(0, stack.Count);

                stack.Push("qux");
                stack.Push("foo");
                stack.Push("bar");
                stack.Push("baz");

                Assert.IsTrue(exp.SequenceEqual(stack));
            }
        }

        [TestMethod]
        public void PooledStack_GlobalPool()
        {
            for (var i = 0; i < 100; i++)
            {
                using var obj = PooledStack<string>.New();

                var stack = obj.Stack;

                Assert.AreEqual(0, stack.Count);

                stack.Push("qux");
                stack.Push("foo");
                stack.Push("bar");
                stack.Push("baz");

                Assert.IsTrue(exp.SequenceEqual(stack));
            }
        }

        [TestMethod]
        public void PooledStack_GetInstance()
        {
            for (var i = 0; i < 100; i++)
            {
                var stack = PooledStack<string>.GetInstance();

                Assert.AreEqual(0, stack.Count);

                stack.Push("qux");
                stack.Push("foo");
                stack.Push("bar");
                stack.Push("baz");

                Assert.IsTrue(exp.SequenceEqual(stack));

                stack.Free();
            }
        }

        [TestMethod]
        public void PooledStack_GottenTooBig()
        {
            var bigPool = StackPool<int>.Create(1, 16, 2048);
            var smallPool = StackPool<int>.Create(1, 16, 16);
            TooBig(() => PooledStack<int>.New(), h => h.Stack, (h, n) => { for (var i = 0; i < n; i++) h.Push(i); }, 1024);
            TooBig(() => bigPool.New(), h => h.Stack, (h, n) => { for (var i = 0; i < n; i++) h.Push(i); }, 2048);
            TooBig(() => smallPool.New(), h => h.Stack, (h, n) => { for (var i = 0; i < n; i++) h.Push(i); }, 16);
        }
    }
}
