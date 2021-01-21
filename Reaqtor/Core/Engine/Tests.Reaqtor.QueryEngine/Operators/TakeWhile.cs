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
    public class TakeWhile : VirtualTimeEngineTest
    {
        [TestMethod]
        public void TakeWhile_TriggerAfterCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(100, 1), // 110
                OnNext(200, 2), // 210
                OnNext(300, 3), // 310
                OnNext(400, 4), // 410
                OnNext(490, 5), // 500
                OnNext(540, 6), // 550, 800 + 540
                                // crash at 600
                OnNext(900, 9) // 800 + 900
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.TakeWhile(i => i < 6).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 540),
                ReactiveTest.Subscribe(800, 800 + 540));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(110, 1),
                OnNext(210, 2),
                OnNext(310, 3),
                OnNext(410, 4),
                OnNext(500, 5),
                OnCompleted<int>(550),
                OnCompleted<int>(800 + 540));
        }

        [TestMethod]
        public void TakeWhile_TriggerBeforeCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(100, 1), // 110
                OnNext(200, 2), // 210
                OnNext(300, 3), // 310
                OnNext(400, 4), // 410
                OnNext(490, 5), // 500
                                // crash at 600
                OnNext(900, 9) // 800 + 900
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.TakeWhile(i => i < 5).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 500),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(110, 1),
                OnNext(210, 2),
                OnNext(310, 3),
                OnNext(410, 4),
                OnCompleted<int>(500));
        }

        [TestMethod]
        public void TakeWhile_SourceErrorBeforeCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();
            var ex = new Exception();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(100, 1), // 110
                OnNext(200, 2), // 210
                OnNext(300, 3), // 310
                OnNext(400, 4), // 410
                OnError<int>(490, ex) // 500
                                      // crash at 600
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.TakeWhile(i => i < 5).Subscribe(result, new Uri("s:/sub1"), null));
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
                OnNext(110, 1),
                OnNext(210, 2),
                OnNext(310, 3),
                OnNext(410, 4),
                OnError<int>(500, ex));
        }

        [TestMethod]
        public void TakeWhile_SourceCompletedBeforeCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(100, 1), // 110
                OnNext(200, 2), // 210
                OnNext(300, 3), // 310
                OnNext(400, 4), // 410
                OnCompleted<int>(490) // 500
                                      // crash at 600 
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.TakeWhile(i => i < 5).Subscribe(result, new Uri("s:/sub1"), null));
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
                OnNext(110, 1),
                OnNext(210, 2),
                OnNext(310, 3),
                OnNext(410, 4),
                OnCompleted<int>(500));
        }

        [TestMethod]
        public void TakeWhile_TriggerErrorBeforeCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(100, 1), // 110
                OnNext(200, 2), // 210
                OnNext(300, 3), // 310
                OnNext(400, 4), // 410
                OnNext(540, 6), // 550, 800 + 540
                                // crash at 600
                OnNext(900, 9) // 800 + 900
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.TakeWhile(x => (x / (x - x)) == 0).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 110),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnError<int>(110, ex => ex is DivideByZeroException));
        }

        [TestMethod]
        public void TakeWhile_TriggerErrorAfterCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(100, 1), // 110
                OnNext(200, 2), // 210
                OnNext(300, 3), // 310
                OnNext(400, 4), // 410
                OnNext(490, 5), // 500
                OnNext(540, 6), // 550, 800 + 540
                                // crash at 600
                OnNext(900, 9) // 800 + 900
                );
            Assert.IsNotNull(source);

            var result = GetTestableQbserver<int>(reactive, "result");

#pragma warning disable IDE0075 // Simplify conditional expression (used in expression tree)
            Scheduler.ScheduleAbsolute(10, () => source.TakeWhile(x => (x > 5) ? (x / (x - x)) == 0 : true).Subscribe(result, new Uri("s:/sub1"), null));
#pragma warning restore IDE0075 // Simplify conditional expression
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 540),
                ReactiveTest.Subscribe(800, 800 + 540));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(110, 1),
                OnNext(210, 2),
                OnNext(310, 3),
                OnNext(410, 4),
                OnNext(500, 5),
                OnError<int>(550, ex => ex is DivideByZeroException),
                OnError<int>(800 + 540, ex => ex is DivideByZeroException));
        }
    }
}
