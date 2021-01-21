// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.QueryEngine.KeyValueStore.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine.Operators
{
    [TestClass]
    public class PeriodicTimer : VirtualTimeEngineTest
    {
        [TestMethod]
        public void Timer_Periodic_CompleteBeforeFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(50), TimeSpan.FromTicks(100)).Take(4);

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(240, 0L),
                OnNext(340, 1L),
                OnNext(440, 2L),
                OnNext(540, 3L),
                OnCompleted<long>(540),
                OnNext(800, 3L),
                OnCompleted<long>(800));
        }

        [TestMethod]
        public void Timer_Periodic_CompleteBeforeCheckpoint()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(50), TimeSpan.FromTicks(100)).Take(3);

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(240, 0L),
                OnNext(340, 1L),
                OnNext(440, 2L),
                OnCompleted<long>(440));
        }

        [TestMethod]
        public void Timer_Periodic_MultipleFireThroughFailure_CompleteDuringFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(50), TimeSpan.FromTicks(100)).Take(5);

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(240, 0L),
                OnNext(340, 1L),
                OnNext(440, 2L),
                OnNext(540, 3L),
                OnNext(800, 3L),         //540
                OnNext(800, 4L),         //640
                OnCompleted<long>(800)); //640
        }

        [TestMethod]
        public void Timer_Periodic_MultipleFiresThroughFailure_CompleteAfterFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(50), TimeSpan.FromTicks(100)).Take(7);

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(240, 0L),
                OnNext(340, 1L),
                OnNext(440, 2L),
                OnNext(540, 3L),
                OnNext(800, 3L), // 540
                OnNext(800, 4L), // 640
                OnNext(800, 5L), // 740
                OnNext(840, 6L),
                OnCompleted<long>(840));
        }

        [TestMethod]
        public void Timer_Periodic_FirstFireDuringFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(500), TimeSpan.FromTicks(100)).Take(3);

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(800, 0L), // 690
                OnNext(800, 1L), // 790
                OnNext(890, 2L),
                OnCompleted<long>(890));
        }

        [TestMethod]
        public void Timer_Periodic_FirstFireBeforeFailureAfterCheckpoint()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(400), TimeSpan.FromTicks(100)).Take(4);

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(590, 0L),
                OnNext(800, 0L), // 590
                OnNext(800, 1L), // 690
                OnNext(800, 2L), // 790
                OnNext(890, 3L),
                OnCompleted<long>(890));
        }

        [TestMethod]
        public void Timer_Periodic_FirstFireAfterFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(700), TimeSpan.FromTicks(100)).Take(2);

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(890, 0L),
                OnNext(990, 1L),
                OnCompleted<long>(990));
        }

        [TestMethod]
        public void Timer_Periodic_AbsoluteTimeSanityCheck()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(new DateTimeOffset(750, TimeSpan.Zero), TimeSpan.FromTicks(100)).Take(3);

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(800, 0L), // 750
                OnNext(850, 1L),
                OnNext(950, 2L),
                OnCompleted<long>(950));
        }

        [TestMethod]
        public void Timer_Periodic_AbsoluteTimeSanityCheck2()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(new DateTimeOffset(550, TimeSpan.Zero), TimeSpan.FromTicks(100)).Take(2);

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(550, 0L),
                OnNext(800, 0L),         // 550
                OnNext(800, 1L),         // 650
                OnCompleted<long>(800)); // 650
        }
    }
}
