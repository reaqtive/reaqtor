// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.QueryEngine.KeyValueStore.InMemory;

namespace Tests.Reaqtor.QueryEngine.Operators
{
    [TestClass]
    public class Timer : VirtualTimeEngineTest
    {
        [TestMethod]
        public void Timer_Relative_TriggerBeforeCheckpoint()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(300));

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(490, 0L),
                OnCompleted<long>(490));
        }

        [TestMethod]
        public void Timer_Relative_TriggerAfterCheckpointBeforeFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(400));

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
                OnCompleted<long>(590),
                OnNext(800, 0L),         // 590
                OnCompleted<long>(800)); // 590
        }

        [TestMethod]
        public void Timer_Relative_TriggerDuringFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(500));

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(800, 0L),         // 690
                OnCompleted<long>(800)); // 690
        }

        [TestMethod]
        public void Timer_Relative_TriggerAfterFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(TimeSpan.FromTicks(700));

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
                OnCompleted<long>(890));
        }

        [TestMethod]
        public void Timer_Absolute_TriggerBeforeCheckpoint()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(new DateTimeOffset(300, TimeSpan.Zero));

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(300, 0L),
                OnCompleted<long>(300));
        }

        [TestMethod]
        public void Timer_Absolute_TriggerAfterCheckpointBeforeFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(new DateTimeOffset(550, TimeSpan.Zero));

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
                OnCompleted<long>(550),
                OnNext(800, 0L),         // 740
                OnCompleted<long>(800)); // 740
        }

        [TestMethod]
        public void Timer_Absolute_TriggerDuringFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(new DateTimeOffset(650, TimeSpan.Zero));

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(800, 0L),         // 650
                OnCompleted<long>(800)); // 650
        }

        [TestMethod]
        public void Timer_Absolute_TriggerAfterFailure()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = reactive.Timer(new DateTimeOffset(850, TimeSpan.Zero));

            Assert.IsNotNull(source);

            var result = GetTestableQbserver<long>(reactive, "result");

            Scheduler.ScheduleAbsolute(190, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var res = GetTestableObserver<long>("result");
            res.Messages.AssertEqual(
                OnNext(850, 0L),
                OnCompleted<long>(850));
        }
    }
}
