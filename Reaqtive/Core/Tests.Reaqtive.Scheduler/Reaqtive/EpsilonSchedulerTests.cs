// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive.Scheduler;

using Test.Reaqtive.Scheduler.Tasks;

namespace Test.Reaqtive.Scheduler
{
    // REVIEW: Many of the tests in this file have non-determinism. We should review these and try to reduce
    //         the degree of non-determinism. One way would be to introduce a concurrency abstraction layer
    //         in the product, just like we did in Rx. That way we can positively assert the absence of work
    //         at lower levels.

    /// <summary>
    /// Scheduler tests.
    /// </summary>
    [TestClass]
    public sealed class EpsilonSchedulerTests
    {
        /// <summary>
        /// The seed for randomizers.
        /// </summary>
        private const int Seed = 13;

        /// <summary>
        /// A TimeSpan value for <see cref="WaitHandle.WaitOne(TimeSpan)"/> operations that are expected to be
        /// completed almost immediately.
        /// </summary>
        private static readonly TimeSpan ExpectedVerySoon = TimeSpan.FromSeconds(0.05);

        /// <summary>
        /// A TimeSpan value for <see cref="WaitHandle.WaitOne(TimeSpan)"/> operations that are expected to be
        /// completed soon.
        /// </summary>
        private static readonly TimeSpan ExpectedSoon = TimeSpan.FromSeconds(1);

        /// <summary>
        /// A TimeSpan value for <see cref="WaitHandle.WaitOne(TimeSpan)"/> operations that are expected to
        /// never run.
        /// </summary>
        private static readonly TimeSpan NotExpected = TimeSpan.FromSeconds(0.1);

        /// <summary>
        /// A TimeSpan value for <see cref="WaitHandle.WaitOne(TimeSpan)"/> operations that are expected to be
        /// completed eventually, but allowing for the test to not hang indefinitely.
        /// </summary>
        private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(2);

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

        /// <summary>
        /// Schedules a simple task and waits for its completion.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependency in <see cref="WaitForSoon"/>.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_TestSimpleTask()
        {
            using var scheduler = _root.CreateChildScheduler();

            var worked = new AutoResetEvent(false);

            scheduler.Schedule(ActionTask.Create(
                s =>
                {
                    worked.Set();
                    return true;
                },
                0));

            WaitForSoon(worked); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Pauses the scheduler, schedules a new task, continues and waits its completion.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependency in <see cref="WaitForSoon"/> and
        /// <see cref="WaitForNever"/>.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_TestPausedScheduler()
        {
            using var scheduler = _root.CreateChildScheduler();

            scheduler.PauseAsync().Wait();

            var worked = new AutoResetEvent(false);

            scheduler.Schedule(ActionTask.Create(
                s =>
                {
                    worked.Set();
                    return true;
                },
                0));

            WaitForNever(worked); // NB: This is non-deterministic, but should not be flaky in the happy case.

            scheduler.Continue();

            WaitForSoon(worked); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Schedules one million of tasks and waits for their completion.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to an assumption that all CPUs will run work.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_MillionTasks()
        {
            const int NumberOfTasks = 1_000_000;

            using var scheduler = _root.CreateChildScheduler();

            var threadIds = new HashSet<int>();
            int counter = 0;
            var worked = new AutoResetEvent(false);

            for (int i = 0; i < NumberOfTasks; ++i)
            {
                scheduler.Schedule(ActionTask.Create(
                    s =>
                    {
                        lock (threadIds)
                        {
                            threadIds.Add(Thread.CurrentThread.ManagedThreadId);
                            counter++;

                            if (counter == NumberOfTasks)
                            {
                                worked.Set();
                            }
                        }

                        return true;
                    },
                    0));
            }

            WaitWithTimeout(worked);

            Assert.AreEqual(NumberOfTasks, counter);

            if (Environment.ProcessorCount != threadIds.Count)
            {
                Assert.Inconclusive($"Not all CPUs did perform scheduled work; please review if this is a glitch. CPU count = {Environment.ProcessorCount}, Unique thread IDs = {threadIds.Count}.");
            }
        }

        /// <summary>
        /// Schedules 100 tasks, pauses the scheduler, checks that tasks were pause and continues.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependency in <see cref="WaitForSoon"/> and
        /// <see cref="WaitForNever"/>.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_TestPauseContinue()
        {
            const int NumberOfTasks = 100;

            using var scheduler = _root.CreateChildScheduler();

            var worked = new AutoResetEvent(false);
            var started = new AutoResetEvent(false);
            var @continue = new ManualResetEvent(false);
            int counter = 0;

            for (int i = 0; i < NumberOfTasks; ++i)
            {
                scheduler.Schedule(ActionTask.Create(
                    s =>
                    {
                        started.Set();

                        if (Interlocked.Increment(ref counter) == NumberOfTasks)
                        {
                            worked.Set();
                        }

                        WaitWithTimeout(@continue);

                        return true;
                    },
                    0));
            }

            WaitForSoon(started); // NB: This is non-deterministic.

            var pauseTask = scheduler.PauseAsync();
            @continue.Set();
            pauseTask.Wait();

            int intermediateValue = counter;

            Assert.AreNotEqual(0, counter);
            Assert.AreNotEqual(NumberOfTasks, counter);

            started.Reset();

            WaitForNever(started); // NB: This is non-deterministic, but should not be flaky in the happy case.
            Assert.AreEqual(intermediateValue, counter);

            scheduler.Continue();

            WaitWithTimeout(worked);
        }

        /// <summary>
        /// Schedules the non runnable task and checks that it is not executed.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependency in <see cref="WaitForSoon"/> and
        /// <see cref="WaitForNever"/>.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_TestRunnableStateChange()
        {
            using var scheduler = _root.CreateChildScheduler();

            var worked = new AutoResetEvent(false);

            var task = ActionTask.Create(
                s =>
                {
                    worked.Set();
                    return true;
                },
                0);

            task.IsRunnable = false;

            scheduler.Schedule(task);

            WaitForNever(worked); // NB: This is non-deterministic, but should not be flaky in the happy case.

            task.IsRunnable = true;
            scheduler.RecalculatePriority();

            WaitForSoon(worked); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Schedules tasks on several logical schedulers and waits for their completion.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependency in <see cref="WaitForNever"/>.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_TestMultipleLogicalSchedulers()
        {
            const int NumberOfTasks = 100;
            const int NumberOfSchedulers = 5;

            using var scheduler = _root.CreateChildScheduler();

            var schedulers = new List<IScheduler>();

            for (int i = 0; i < NumberOfSchedulers; ++i)
            {
                schedulers.Add(scheduler.CreateChildScheduler());
            }

            var started = new AutoResetEvent(false);
            var worked = new AutoResetEvent(false);
            int counter = 0;

            for (int i = 0; i < NumberOfTasks; ++i)
            {
                var j = i;

                schedulers[j % NumberOfSchedulers].Schedule(ActionTask.Create(
                    s =>
                    {
                        if (Interlocked.Increment(ref counter) == NumberOfTasks)
                        {
                            worked.Set();
                        }

                        started.Set();

                        Thread.Sleep(TimeSpan.FromMilliseconds(new Random(Seed + j).Next(10, 20)));

                        return true;
                    },
                    0));
            }

            WaitWithTimeout(started);
            scheduler.PauseAsync().Wait();

            int intermediateCounter = counter;

            Assert.AreNotEqual(0, counter);
            Assert.AreNotEqual(NumberOfTasks, counter);

            started.Reset();

            WaitForNever(started); // NB: This is non-deterministic, but should not be flaky in the happy case.
            Assert.AreEqual(intermediateCounter, counter);

            scheduler.Continue();

            WaitWithTimeout(worked);
        }

        /// <summary>
        /// Schedules a time task.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependencies.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Strong")] // Tests timing.
        public void Scheduler_SingleTimerEvent()
        {
            var ltDueTime = TimeSpan.FromSeconds(0.1);
            var eqDueTime = TimeSpan.FromSeconds(0.2);
            var gtDueTime = TimeSpan.FromSeconds(0.3);

            using var scheduler = _root.CreateChildScheduler();

            var worked = new AutoResetEvent(false);

            scheduler.Schedule(eqDueTime, ActionTask.Create(
                s =>
                {
                    worked.Set();
                    return true;
                },
                0));

            Assert.IsFalse(worked.WaitOne(ltDueTime)); // NB: This is non-deterministic.
            Assert.IsTrue(worked.WaitOne(gtDueTime)); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Schedules a recursive timer task.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependencies.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Strong")] // Tests timing.
        public void Scheduler_RecursiveTimerEvent()
        {
            const int Count = 3;

            var period = TimeSpan.FromSeconds(0.1);
            var beforeAllAreDue = TimeSpan.FromSeconds(0.3);
            var afterAllWereDue = TimeSpan.FromSeconds(0.5);

            using var scheduler = _root.CreateChildScheduler();

            int recursiveCall = 0;
            var worked = new AutoResetEvent(false);

            bool action(IScheduler s)
            {
                if (recursiveCall < Count)
                {
                    recursiveCall++;
                    s.Schedule(period, ActionTask.Create(action, 0));
                }
                else
                {
                    worked.Set();
                }

                return true;
            }

            var task = ActionTask.Create(action, 0);
            scheduler.Schedule(period, task);

            Assert.IsFalse(worked.WaitOne(beforeAllAreDue)); // NB: This is non-deterministic.
            Assert.IsTrue(worked.WaitOne(afterAllWereDue)); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Schedules a test with absolute due time.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependencies.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Strong")] // Tests timing.
        public void Scheduler_TestAbsoluteTimer()
        {
            var ltDueTime = TimeSpan.FromSeconds(0.1);
            var eqDueTime = TimeSpan.FromSeconds(0.2);
            var gtDueTime = TimeSpan.FromSeconds(0.3);

            using var scheduler = _root.CreateChildScheduler();

            var worked = new AutoResetEvent(false);

            DateTimeOffset offset = DateTime.UtcNow + eqDueTime;

            scheduler.Schedule(offset, ActionTask.Create(
                s =>
                {
                    worked.Set();
                    return true;
                },
                0));

            Assert.IsFalse(worked.WaitOne(ltDueTime)); // NB: This is non-deterministic.
            Assert.IsTrue(worked.WaitOne(gtDueTime)); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Schedules long running tasks and checks its runnable/not runnable states.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependency in <see cref="WaitForSoon"/> and
        /// <see cref="WaitForNever"/>.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_TestSimpleLongRunningTask()
        {
            const int IterationCount = 10;
            const int MessageCount = 100;
            const int BatchSize = 10;

            using var scheduler = _root.CreateChildScheduler();

            var queue = new ConcurrentQueue<int>();

            var countdown = new CountdownEvent(IterationCount);

            bool action(IScheduler s, ConcurrentQueue<int> q, int batch, int state)
            {
                countdown.Signal();

                for (int i = 0; i < batch; ++i)
                {
                    q.TryDequeue(out int result);
                }

                return false;
            }

            var task = new MessageQueueBasedTask<int>(queue, BatchSize, 0, action);
            scheduler.Schedule(task);

            Thread.Sleep(/* no work is expected */ NotExpected);

            //
            // NB: No messages have been enqueued yet, so the task is not runnable, so it
            //     should not get executed, thus leaving the count down event untouched.
            //

            Assert.AreEqual(IterationCount, countdown.CurrentCount);

            for (var i = 0; i < MessageCount; i++)
            {
                queue.Enqueue(i);
            }

            scheduler.RecalculatePriority();

            WaitForSoon(countdown.WaitHandle); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Schedules two timers in opposite order.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependencies.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Strong")] // Tests timing.
        public void Scheduler_TwoTimerEventsInReverseOrder()
        {
            var task1Due = TimeSpan.FromSeconds(0.3);
            var task2Due = TimeSpan.FromSeconds(0.1);

            using var scheduler = _root.CreateChildScheduler();

            var worked1 = new AutoResetEvent(false);

            var task1 = ActionTask.Create(
                s =>
                {
                    worked1.Set();
                    return true;
                },
                0);

            var worked2 = new AutoResetEvent(false);

            var task2 = ActionTask.Create(
                s =>
                {
                    worked2.Set();
                    return true;
                },
                0);

            scheduler.Schedule(task1Due, task1);
            scheduler.Schedule(task2Due, task2); // NB: This is non-deterministic.

            Assert.AreEqual(1, WaitHandle.WaitAny(new[] { worked1, worked2 }));
            worked1.WaitOne();

            // REVIEW: We don't really test the order the tasks executed in.
        }

        /// <summary>
        /// Schedules timer with negative time.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependency in <see cref="WaitForVerySoon"/>.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_NegativeTimer()
        {
            using var scheduler = _root.CreateChildScheduler();

            var worked = new AutoResetEvent(false);

            scheduler.Schedule(TimeSpan.FromSeconds(-2), ActionTask.Create(
                s =>
                {
                    worked.Set();
                    return true;
                },
                0));

            WaitForVerySoon(worked); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Checks execution order of tasks with different priorities.
        /// </summary>
        [TestMethod]
        public void Scheduler_CheckPriorityExecutionOfLongRunningTasks()
        {
            const int BatchSize = 1;

            using var physicalScheduler = PhysicalScheduler.Create(1);
            using var scheduler = new LogicalScheduler(physicalScheduler);

            var countdown = new CountdownEvent(2);

            var queue0 = new ConcurrentQueue<int>();
            var queue1 = new ConcurrentQueue<int>();

            bool firstHasCalled = true;

            bool action(IScheduler s, ConcurrentQueue<int> q, int batch, int state)
            {
                if (state == 0)
                {
                    Volatile.Write(ref firstHasCalled, true);
                }
                else
                {
                    Assert.AreEqual(true, Volatile.Read(ref firstHasCalled));
                }

                while (!q.IsEmpty)
                {
                    q.TryDequeue(out int result);
                }

                countdown.Signal();

                return false;
            }

            //
            // NB: Tasks don't become runnable before items have been enqueued.
            //

            var task0 = new MessageQueueBasedTask<int>(queue0, BatchSize, 0, action);
            var task1 = new MessageQueueBasedTask<int>(queue1, BatchSize, 1, action);

            scheduler.Schedule(task1);
            scheduler.Schedule(task0);

            queue0.Enqueue(0);
            queue1.Enqueue(1);

            scheduler.RecalculatePriority();

            countdown.Wait();
        }

        /// <summary>
        /// Schedules 1000 timer tasks.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependency in <see cref="WaitForSoon"/>.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_1000TimerTasks()
        {
            const int NumberOfTasks = 1000;

            using var scheduler = _root.CreateChildScheduler();

            int count = 0;
            var worked = new AutoResetEvent(false);

            var task = ActionTask.Create(
                s =>
                {
                    if (Interlocked.Increment(ref count) == NumberOfTasks)
                    {
                        worked.Set();
                    }

                    return true;
                },
                0);

            var random = new Random(Seed);

            for (int i = 0; i < NumberOfTasks; ++i)
            {
                scheduler.Schedule(TimeSpan.FromMilliseconds(random.Next(5, 900)), task);
            }

            WaitForSoon(worked); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Schedules 100 long running tasks.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic.
        /// </remarks>
        [TestMethod]
        public void Scheduler_100LongRunningTasks()
        {
            const int NumberOfTasks = 100;

            using var scheduler = _root.CreateChildScheduler();

            var queues = new List<ConcurrentQueue<int>>();

            for (int i = 0; i < NumberOfTasks; ++i)
            {
                queues.Add(new ConcurrentQueue<int>());
            }

            int numberOfCalls = 0;
            var worked = new AutoResetEvent(false);

            bool action(IScheduler s, ConcurrentQueue<int> q, int batch, int state)
            {
                if (Interlocked.Increment(ref numberOfCalls) == NumberOfTasks)
                {
                    worked.Set();
                }

                while (!q.IsEmpty)
                {
                    q.TryDequeue(out _);
                }

                return false;
            }

            for (int i = 0; i < NumberOfTasks; ++i)
            {
                scheduler.Schedule(new MessageQueueBasedTask<int>(queues[i], 10, i, action));
            }

            foreach (var queue in queues)
            {
                queue.Enqueue(0);
            }

            scheduler.RecalculatePriority();

            WaitWithTimeout(worked);
        }

        /// <summary>
        /// Schedules pause on a scheduler with no tasks.
        /// </summary>
        [TestMethod]
        public void Scheduler_PauseWithSyncContinue()
        {
            using var scheduler = _root.CreateChildScheduler();

            var paused = scheduler.PauseAsync();

            Assert.IsTrue(paused.IsCompleted);
        }

        /// <summary>
        /// Checks pause and continue functionality.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependencies.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Strong")] // Relies on small delay timing.
        public void Scheduler_PauseWithAsyncContinue()
        {
            //
            //     0.000          0.010          0.110  0.200     ~0.200
            // ------+--------------+--------------+------+----------+
            //       |              |              |      |          |
            //   Schedule      PauseAsync^         |    done    PauseAsync$
            //       |              |              |      |
            //       |<-pauseDelay-> <-pauseCheck->       |
            //       |                                    |
            //       |<--------workCompletionDelay------->|
            //

            var workCompletionDelay = TimeSpan.FromSeconds(0.2);
            var pauseDelay = TimeSpan.FromSeconds(0.01);
            var pauseCheck = TimeSpan.FromSeconds(0.1);

            using var scheduler = _root.CreateChildScheduler();

            var worked = new AutoResetEvent(false);

            scheduler.Schedule(ActionTask.Create(
                s =>
                {
                    Thread.Sleep(workCompletionDelay);
                    worked.Set();
                    return true;
                },
                0));

            Thread.Sleep(pauseDelay); // NB: This is non-deterministic.

            Task paused = scheduler.PauseAsync();

            Assert.IsFalse(worked.WaitOne(pauseCheck)); // NB: This is non-deterministic.
            Assert.IsFalse(paused.IsCompleted);

            paused.Wait();

            Assert.IsTrue(worked.WaitOne(TimeSpan.Zero));
        }

        /// <summary>
        /// Checks pausing a container from a task within the container.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic due to physical time dependency in <see cref="WaitForSoon"/>.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Weak")] // May time-out and be inconclusive.
        public void Scheduler_PauseFromWithinATask()
        {
            using var scheduler = _root.CreateChildScheduler();

            var worked = new AutoResetEvent(false);

            scheduler.Schedule(ActionTask.Create(
                s =>
                {
                    scheduler.PauseAsync().ContinueWith(_ => { worked.Set(); });
                    return true;
                },
                0));

            WaitForSoon(worked); // NB: This is non-deterministic.
        }

        /// <summary>
        /// Schedules a null task.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Scheduler_NullTask()
        {
            _root.Schedule(null);
        }

        /// <summary>
        /// Schedules a null task at due time.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Scheduler_DateTimeNullTask()
        {
            _root.Schedule(DateTime.UtcNow, null);
        }

        /// <summary>
        /// Schedules a null task at due time.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Scheduler_TimespanNullTask()
        {
            _root.Schedule(TimeSpan.FromSeconds(1), null);
        }

        /// <summary>
        /// Creation of logical scheduler without physical one.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Scheduler_NullPhysicalScheduler()
        {
            new LogicalScheduler(null);
        }

        /// <summary>
        /// Creation of physical scheduler with negative number of workers.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Scheduler_PhysicalSchedulerWithNegativeWorkers()
        {
            PhysicalScheduler.Create(-1);
        }

        /// <summary>
        /// Creation of physical scheduler with zero workers.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Scheduler_PhysicalSchedulerWithZeroWorkers()
        {
            PhysicalScheduler.Create(0);
        }

        /// <summary>
        /// Schedules a throwing task.
        /// </summary>
        [TestMethod]
        public void Scheduler_ThrowingTask()
        {
            const int NumberOfTasks = 1000;
            const int NumberOfExceptions = 10;

            using var scheduler = _root.CreateChildScheduler();

            var ex = new ArgumentNullException();

            scheduler.UnhandledException += (o, e) =>
            {
                Assert.AreSame(ex, e.Exception);
                e.Handled = true;
            };

            int counter = 0;
            int exceptionCounter = 0;
            var worked = new AutoResetEvent(false);

            for (int i = 0; i < NumberOfTasks; ++i)
            {
                scheduler.Schedule(ActionTask.Create(
                    s =>
                    {
                        lock (worked)
                        {
                            counter++;

                            if (counter % 3 == 0 && exceptionCounter < NumberOfExceptions)
                            {
                                exceptionCounter++;
                                throw ex;
                            }

                            if (counter == NumberOfTasks - NumberOfExceptions)
                            {
                                worked.Set();
                            }
                        }

                        return true;
                    },
                    0));
            }

            WaitWithTimeout(worked);
        }

        /// <summary>
        /// Schedules work way in the future, where System.Threading.Timer can't reach.
        /// </summary>
        [TestMethod]
        public void Scheduler_BigStepForHumanKindSmallStepForTimerKind()
        {
            var err = default(Exception);

            var h = new UnhandledExceptionEventHandler((o, e) =>
            {
                err = (Exception)e.ExceptionObject;
            });

            try
            {
                AppDomain.CurrentDomain.UnhandledException += h;

                using var ph = PhysicalScheduler.Create();
                using var lg = new LogicalScheduler(ph);

                // The BCL has a limit on 0xfffffffe milliseconds, which is some 49 days (see Worker.NormalizeForTimer).
                // If not handled correctly in Worker.cs, those attempts at scheduling would cause the worker to die.
                lg.Schedule(TimeSpan.FromDays(50), ActionTask.Create(_ => true, 1));
                lg.Schedule(DateTimeOffset.Now.AddDays(50), ActionTask.Create(_ => true, 1));

                // Ensure disposal doesn't happen before the worker thread had a chance to evaluate timer expirations,
                // which is what would cause the test process to die on an invalid call to Timer.Change.
                var e = new ManualResetEvent(false);
                lg.Schedule(ActionTask.Create(_ => { e.Set(); return true; }, 1));
                e.WaitOne();
            }
            finally
            {
                AppDomain.CurrentDomain.UnhandledException -= h;
            }

            if (err != null)
            {
                ExceptionDispatchInfo.Capture(err).Throw();
            }
        }

        /// <summary>
        /// Disposing a logical scheduler disposes its internal children and all of their scheduled work items.
        /// </summary>
        /// <remarks>
        /// This test is non-deterministic.
        /// </remarks>
        [TestMethod]
        [TestCategory("Scheduler_NonDeterministic_Strong")] // Relies on timing.
        public void Scheduler_DisposeChildSchedulers()
        {
            IScheduler parent, child;

            using var ev = new AutoResetEvent(false);

            using (parent = _root.CreateChildScheduler())
            {
                child = parent.CreateChildScheduler();

                child.Schedule(
                    DateTimeOffset.Now.AddMilliseconds(200),
                    ActionTask.Create(
                        s =>
                            {
                                ev.Set();
                                return true;
                            },
                        0));
            }

            Assert.IsTrue(!ev.WaitOne(1000));
        }

        private static void WaitForVerySoon(WaitHandle e) => WaitForExpected(e, ExpectedVerySoon);

        private static void WaitForSoon(WaitHandle e) => WaitForExpected(e, ExpectedSoon);

        private static void WaitForExpected(WaitHandle e, TimeSpan t)
        {
            var sw = Stopwatch.StartNew();

            var b = e.WaitOne(t);

            if (!b)
            {
                if (!e.WaitOne(Timeout))
                {
                    Assert.Fail($"Test timed out ({Timeout}) which could be caused by a hang. Elapsed time = {sw.Elapsed}");
                }
                else
                {
                    Assert.Inconclusive($"Event expected to be signaled within {t} but took longer than expected. Elapsed time = {sw.Elapsed}");
                }
            }
            else
            {
                Assert.IsTrue(b);
            }
        }

        private static void WaitWithTimeout(WaitHandle e)
        {
            var sw = Stopwatch.StartNew();

            if (!e.WaitOne(Timeout))
            {
                Assert.Fail($"Test timed out ({Timeout}) which could be caused by a hang. Elapsed time = {sw.Elapsed}");
            }
        }

        private static void WaitForNever(WaitHandle e)
        {
            Assert.IsFalse(e.WaitOne(NotExpected), "Unexpected signaling of event.");
        }
    }
}
