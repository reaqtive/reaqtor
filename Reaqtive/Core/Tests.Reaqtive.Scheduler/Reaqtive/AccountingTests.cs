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
        public void Scheduler_PerformanceCounters()
        {
            using var scheduler = _root.CreateChildScheduler();

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

            worked.WaitOne();

            //
            // Pause to see the effect of accounting pause time.
            //

            scheduler.PauseAsync().Wait();

            //
            // Schedule work with a due time.
            //

            scheduler.Schedule(TimeSpan.FromMilliseconds(10), ActionTask.Create(
                s =>
                {
                    worked.Set();
                    return true;
                },
                0));

            //
            // Resume the scheduler and wait for work to be done.
            //

            scheduler.Continue();

            worked.WaitOne();

            //
            // Pause the scheduler to wait for all the worker threads to stop processing work.
            //
            // NB: This is critical for the asserts, because accounting happens after the user work completes,
            //     and our user code signals an event, so we end up with a race between threads. By pausing, we
            //     ensure the tasks have drained completely.
            //

            scheduler.PauseAsync().Wait();

            //
            // Check the counters of the child scheduler.
            //

            var childCounters = ((ISchedulerPerformanceCountersProvider)scheduler).QueryPerformanceCounters(includeChildren: false);

            Assert.AreEqual(2, childCounters.TaskExecutionCount);
            Assert.AreEqual(1, childCounters.TimerTickCount);

            Assert.IsTrue(childCounters.Uptime > TimeSpan.Zero);
            Assert.IsTrue(childCounters.PausedTime > TimeSpan.Zero);

            //
            // Check the counters of the root scheduler.
            //

            var rootCounters = _root.QueryPerformanceCounters(includeChildren: true);

            Assert.AreEqual(2, rootCounters.TaskExecutionCount);
            Assert.AreEqual(1, rootCounters.TimerTickCount);

            //
            // Some trivial asserts for the SchedulerPerformanceCounters struct.
            //

            var c1 = childCounters;
            var c2 = childCounters;

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
            Assert.AreEqual(4, twice1.TaskExecutionCount);
            Assert.AreEqual(2, twice1.TimerTickCount);
        }
    }
}
