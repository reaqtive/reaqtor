// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using Reaqtive.Scheduler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Test.Reaqtive.Scheduler.Tasks;

namespace Test.Reaqtive.Scheduler
{
    [TestClass]
    public class AccountingTests
    {
        /// <summary>
        /// The physical scheduler.
        /// </summary>
        private PhysicalScheduler _physicalScheduler;

        /// <summary>
        /// The root logical scheduler.
        /// </summary>
        private LogicalScheduler _root;

        /// <summary>
        /// Initializes test instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _physicalScheduler = PhysicalScheduler.Create();
            _root = new LogicalScheduler(_physicalScheduler);
        }

        /// <summary>
        /// Cleans up test instance.
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            _root.Dispose();
            _physicalScheduler.Dispose();
        }

        [TestMethod]
        public void Scheduler_PerformanceCounters_NoTime()
        {
            using var scheduler = _root.CreateChildScheduler();

            const int N = 10;

            for (var i = 0; i < N; i++)
            {
                var worked = new AutoResetEvent(false);

                //
                // Schedule work without due time, and wait for its completion.
                //

                scheduler.Schedule(ActionTask.Create(
                    s =>
                    {
                        worked.Set();
                        return true;
                    },
                    0));

                //
                // Resume the scheduler if we were previously paused. This accummulates PausedTime to assert on.
                //

                if (i > 0)
                {
                    scheduler.Continue();
                }

                //
                // Wait for the work to be completed.
                //

                worked.WaitOne();

                //
                // Pause the scheduler to wait for all the worker threads to stop processing work.
                //
                // NB: This is critical for the asserts, because accounting happens after the user work completes,
                //     and our user code signals an event, so we end up with a race between threads. By pausing, we
                //     ensure the tasks have drained completely.
                //

                scheduler.PauseAsync().Wait();
            }

            //
            // Check the counters of the child scheduler.
            //

            var childCounters = ((ISchedulerPerformanceCountersProvider)scheduler).QueryPerformanceCounters(includeChildren: false);

            Assert.AreEqual(N, childCounters.TaskExecutionCount);
            Assert.AreEqual(0, childCounters.TimerTickCount);

            Assert.IsTrue(childCounters.Uptime > TimeSpan.Zero);
            Assert.IsTrue(childCounters.PausedTime > TimeSpan.Zero);

            //
            // Check the counters of the root scheduler.
            //

            var rootCounters = _root.QueryPerformanceCounters(includeChildren: true);

            Assert.AreEqual(N, rootCounters.TaskExecutionCount);
            Assert.AreEqual(0, rootCounters.TimerTickCount);

            //
            // Assert struct math on SchedulerPerformanceCounters.
            //

            AssertStructMath(childCounters);
            AssertStructMath(rootCounters);
        }

        [TestMethod]
        public void Scheduler_PerformanceCounters_Time()
        {
            using var scheduler = _root.CreateChildScheduler();

            const int N = 4;
            const int DelayInMs = 50; // NB: See remarks on AssertTimerTickCount.

            for (var i = 0; i < N; i++)
            {
                var worked = new AutoResetEvent(false);

                //
                // Schedule work with a due time.
                //

                scheduler.Schedule(TimeSpan.FromMilliseconds(DelayInMs), ActionTask.Create(
                    s =>
                    {
                        worked.Set();
                        return true;
                    },
                    0));

                //
                // Resume the scheduler if we were previously paused. This accummulates PausedTime to assert on.
                //

                if (i > 0)
                {
                    scheduler.Continue();
                }

                //
                // Wait for the work to be completed.
                //

                worked.WaitOne();

                //
                // Pause the scheduler to wait for all the worker threads to stop processing work.
                //
                // NB: This is critical for the asserts, because accounting happens after the user work completes,
                //     and our user code signals an event, so we end up with a race between threads. By pausing, we
                //     ensure the tasks have drained completely.
                //

                scheduler.PauseAsync().Wait();
            }

            //
            // Check the counters of the child scheduler.
            //

            var childCounters = ((ISchedulerPerformanceCountersProvider)scheduler).QueryPerformanceCounters(includeChildren: false);

            static void AssertTimerTickCount(long timerTickCount)
            {
                if (timerTickCount < N / 2)
                {
                    //
                    // NB: This happens when the `item.DueTime <= PhysicalScheduler.Now` check in `Worker.AddItem` causes the
                    //     time-based task to get dispatched directly.
                    //

                    Assert.Inconclusive($"TimerTickCount = {timerTickCount} but {N} was expected. This can be due to an inherent by-design race inside the scheduler's dispatch logic. Please review this test if failure is reported repeatedly.");
                }
            }

            Assert.AreEqual(N, childCounters.TaskExecutionCount);
            AssertTimerTickCount(childCounters.TimerTickCount);

            Assert.IsTrue(childCounters.Uptime > TimeSpan.Zero);
            Assert.IsTrue(childCounters.PausedTime > TimeSpan.Zero);

            //
            // Check the counters of the root scheduler.
            //

            var rootCounters = _root.QueryPerformanceCounters(includeChildren: true);

            Assert.AreEqual(N, rootCounters.TaskExecutionCount);
            AssertTimerTickCount(rootCounters.TimerTickCount);

            //
            // Assert struct math on SchedulerPerformanceCounters.
            //

            AssertStructMath(childCounters);
            AssertStructMath(rootCounters);
        }

        private static void AssertStructMath(SchedulerPerformanceCounters counters)
        {
            //
            // Some trivial asserts for the SchedulerPerformanceCounters struct.
            //

            var c1 = counters;
            var c2 = counters;

            Assert.IsTrue(c1.Equals(c2));
            Assert.IsTrue(c1 == c2);
            Assert.IsFalse(c1 != c2);

            Assert.IsTrue(c1.Equals((object)c2));
            Assert.IsFalse(c1.Equals(""));
            Assert.IsFalse(c1.Equals(null));

            //
            // NB: 0.00000000023283 chance for it to be 0. We'll take the gamble.
            //

            Assert.AreNotEqual(0, c1.GetHashCode());

            //
            // Some more trivial math.
            //

            var zero1 = c1.Subtract(c2);
            var zero2 = c1 - c2;

            Assert.AreEqual(zero1, zero2);
            Assert.AreEqual(0, zero1.TaskExecutionCount);
            Assert.AreEqual(0, zero1.TimerTickCount);
            Assert.AreEqual(TimeSpan.Zero, zero1.Uptime);
            Assert.AreEqual(TimeSpan.Zero, zero1.PausedTime);

            var twice1 = c1.Add(c2);
            var twice2 = c1 + c2;

            Assert.AreEqual(twice1, twice2);
            Assert.AreEqual(2 * c1.TaskExecutionCount, twice1.TaskExecutionCount);
            Assert.AreEqual(2 * c1.TimerTickCount, twice1.TimerTickCount);
        }
    }
}
