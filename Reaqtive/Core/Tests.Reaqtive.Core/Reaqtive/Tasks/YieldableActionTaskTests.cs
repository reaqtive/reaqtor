// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive.Scheduler;
using Reaqtive.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class YieldableActionTaskTests
    {
        [TestMethod]
        public void YieldableActionTask_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => new YieldableActionTask(default(Func<YieldToken, bool>)));
            Assert.ThrowsException<ArgumentNullException>(() => new YieldableActionTask<int>(default(Func<int, YieldToken, bool>), 42));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void YieldableActionTask_Simple()
        {
            var e = new ManualResetEvent(false);
            var task = new YieldableActionTask(token => e.Set());
            task.RecalculatePriority();

            Assert.IsTrue(task.IsRunnable);
            Assert.AreEqual(1L, task.Priority);

            using var s = PhysicalScheduler.Create();
            using var l = new LogicalScheduler(s);

            l.Schedule(task);
            e.WaitOne();
        }

        [TestMethod]
        public void YieldableActionTask_Simple_Yield()
        {
            YieldableActionTask_Simple_Yield_Async().Wait();
        }

        private static async Task YieldableActionTask_Simple_Yield_Async()
        {
            var e = new ManualResetEvent(false);

            var task = new YieldableActionTask(token =>
            {
                Assert.IsFalse(token.IsYieldRequested);
                e.Set();

                while (!token.IsYieldRequested)
                    ;

                return true;
            });

            task.RecalculatePriority();

            Assert.IsTrue(task.IsRunnable);
            Assert.AreEqual(1L, task.Priority);

            using var s = PhysicalScheduler.Create();
            using var l = new LogicalScheduler(s);

            l.Schedule(task);
            e.WaitOne();

            await l.PauseAsync();
        }

        [TestMethod]
        public void YieldableActionTask_Simple_YieldAndResume()
        {
            YieldableActionTask_Simple_YieldAndResume_Async().Wait();
        }

        private static async Task YieldableActionTask_Simple_YieldAndResume_Async()
        {
            var e1 = new AutoResetEvent(false);
            var e2 = new AutoResetEvent(false);
            var p1 = new ManualResetEvent(false);

            var state = 0;

            var task = new YieldableActionTask(token =>
            {
                Assert.IsFalse(token.IsYieldRequested);

                switch (state)
                {
                    case 0:
                        e1.Set();

                        while (!token.IsYieldRequested)
                            ;

                        p1.Set();

                        state++;
                        return false;
                    case 1:
                        e2.Set();
                        state++;
                        return true;
                    default:
                        Assert.Fail();
                        break;
                }

                return true;
            });

            task.RecalculatePriority();

            Assert.IsTrue(task.IsRunnable);
            Assert.AreEqual(1L, task.Priority);

            using var s = PhysicalScheduler.Create();
            using var l = new LogicalScheduler(s);

            l.Schedule(task);
            e1.WaitOne();

            await l.PauseAsync();
            p1.WaitOne();

            l.Continue();
            e2.WaitOne();
        }

        [TestMethod]
        public void YieldableActionTask_Generic_Simple()
        {
            var e = new ManualResetEvent(false);
            var x = default(int);
            var task = new YieldableActionTask<int>((state, token) => { x = state; e.Set(); return true; }, 42);
            task.RecalculatePriority();

            Assert.IsTrue(task.IsRunnable);
            Assert.AreEqual(1L, task.Priority);

            using (var s = PhysicalScheduler.Create())
            using (var l = new LogicalScheduler(s))
            {
                l.Schedule(task);
                e.WaitOne();
            }

            Assert.AreEqual(42, x);
        }

        [TestMethod]
        public void YieldableActionTask_Generic_Simple_Yield()
        {
            YieldableActionTask_Generic_Simple_Yield_Async().Wait();
        }

        private static async Task YieldableActionTask_Generic_Simple_Yield_Async()
        {
            var e = new ManualResetEvent(false);

            var x = default(int);
            var task = new YieldableActionTask<int>((state, token) =>
            {
                Assert.IsFalse(token.IsYieldRequested);
                x = state;
                e.Set();

                while (!token.IsYieldRequested)
                    ;

                return true;
            }, 42);

            task.RecalculatePriority();

            Assert.IsTrue(task.IsRunnable);
            Assert.AreEqual(1L, task.Priority);

            using (var s = PhysicalScheduler.Create())
            using (var l = new LogicalScheduler(s))
            {
                l.Schedule(task);
                e.WaitOne();

                await l.PauseAsync();
            }

            Assert.AreEqual(42, x);
        }

        [TestMethod]
        public void YieldableActionTask_Generic_YieldAndResume()
        {
            const int IterationCount = 1; // NB: Increase this to debug subtle issues.

            for (var i = 0; i < IterationCount; i++)
            {
                YieldableActionTask_Generic_YieldAndResume_Async().Wait();
            }
        }

        private static async Task YieldableActionTask_Generic_YieldAndResume_Async()
        {
            const int N = 10;

            var isRunning = new AutoResetEvent(false);
            var stoppedRunning = new AutoResetEvent(false);
            var iterationCount = new CountdownEvent(N);

            var i = 2;
            var primes = new List<int>();

            var task = new YieldableActionTask<List<int>>((primes2, token) =>
            {
                Assert.AreSame(primes, primes2);

                isRunning.Set();

                var sw = Stopwatch.StartNew();

                while (true)
                {
                    if (sw.ElapsedMilliseconds > 50 && token.IsYieldRequested)
                    {
                        break;
                    }

                    var isPrime = true;

                    for (var d = 2; d <= Math.Sqrt(i); d++)
                    {
                        if (i % d == 0)
                        {
                            isPrime = false;
                            break;
                        }
                    }

                    if (isPrime)
                    {
                        primes2.Add(i);
                    }

                    i++;
                }

                iterationCount.Signal();

                try
                {
                    if (iterationCount.CurrentCount == 0)
                    {
                        return true;
                    }

                    return false;
                }
                finally
                {
                    stoppedRunning.Set();
                }
            }, primes);

            task.RecalculatePriority();

            Assert.IsTrue(task.IsRunnable);
            Assert.AreEqual(1L, task.Priority);

            using (var s = PhysicalScheduler.Create())
            using (var l = new LogicalScheduler(s))
            {
                l.Schedule(task);

                for (var j = 0; j < N; j++)
                {
                    isRunning.WaitOne();
                    await l.PauseAsync();
                    stoppedRunning.WaitOne();
                    l.Continue();
                }

                iterationCount.Wait();
            }

            Assert.IsTrue(Primes().Take(primes.Count).SequenceEqual(primes));
        }

        private static IEnumerable<int> Primes()
        {
            for (var i = 2; i < int.MaxValue; i++)
            {
                var isPrime = true;

                for (var d = 2; d <= Math.Sqrt(i); d++)
                {
                    if (i % d == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    yield return i;
                }
            }
        }
    }
}
