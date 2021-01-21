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
    public class FirstAsync : VirtualTimeEngineTest
    {
        [TestMethod]
        public void FirstAsync_TriggerAfterCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                // crash at 600
                OnNext(900, 1) // 800 + 900
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.FirstAsync().Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10),
                ReactiveTest.Subscribe(800, 800 + 900));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(800 + 900, 1),
                OnCompleted<int>(800 + 900));
        }

        [TestMethod]
        public void FirstAsync_TriggerBeforeCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(100, 1) // 110
                               // crash at 600
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.FirstAsync().Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 100),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(110, 1),
                OnCompleted<int>(10 + 100));
        }

        [TestMethod]
        public void FirstAsync_SourceErrorBeforeCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();
            var ex = new Exception();

            var source = GetTestableQbservable(reactive, "source",
                OnError<int>(490, ex) // 500
                                      // crash at 600
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.FirstAsync().Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 490),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnError<int>(500, ex));
        }

        [TestMethod]
        public void FirstAsync_SourceCompletedBeforeCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnCompleted<int>(490) // 500
                                      // crash at 600
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.FirstAsync().Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 490),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnError<int>(500, ex => ex is InvalidOperationException));
        }

        [TestMethod]
        public void FirstAsync_SourceErrorAfterCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();
            var ex = new Exception();

            var source = GetTestableQbservable(reactive, "source",
                OnError<int>(550, ex) // 550, 800 + 550
                                      // crash at 600
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.FirstAsync().Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 560),
                ReactiveTest.Subscribe(800, 800 + 550));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnError<int>(560, ex),
                OnError<int>(800 + 550, ex));
        }

        [TestMethod]
        public void FirstAsync_SourceCompletedAfterCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnCompleted<int>(550) // 550, 800 + 550
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.FirstAsync().Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 550),
                ReactiveTest.Subscribe(800, 800 + 550));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnError<int>(10 + 550, ex => ex is InvalidOperationException),
                OnError<int>(800 + 550, ex => ex is InvalidOperationException));
        }
    }
}
