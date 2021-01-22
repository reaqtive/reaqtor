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
    public class SkipUntil : VirtualTimeEngineTest
    {
        [TestMethod]
        public void SkipUntil_TriggerAfterCheckpoint_FailAndReload()
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

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnNext(520, 1) // 530
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10),
                ReactiveTest.Subscribe(800));

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 520),
                ReactiveTest.Subscribe(800, 800 + 520));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(800 + 900, 9));
        }

        [TestMethod]
        public void SkipUntil_TriggerBeforeCheckpoint_FailAndReload()
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

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnNext(450, 1),       // 460
                OnCompleted<int>(490) // 500
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10),
                ReactiveTest.Subscribe(800));

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 450),
                ReactiveTest.Subscribe(800, 800)
            );

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(500, 5),
                OnNext(800 + 900, 9));
        }

        [TestMethod]
        public void SkipUntil_RepeatedValues_FailAndReload()
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
                OnNext(550, 6), // 550, 800 + 550
                                // crash at 600
                OnNext(900, 9) // 800 + 900
                );
            Assert.IsNotNull(source);

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnNext(520, 1) // 530, 800 + 530
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10),
                ReactiveTest.Subscribe(800));

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 520),
                ReactiveTest.Subscribe(800, 800 + 520));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(560, 6),
                OnNext(800 + 550, 6),
                OnNext(800 + 900, 9));
        }

        [TestMethod]
        public void SkipUntil_NeverTrigger_FailAndReload()
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
                OnNext(550, 6), // 550, 800 + 550
                                // crash at 600
                OnNext(900, 9) // 800 + 900
                );
            Assert.IsNotNull(source);

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnCompleted<int>(490) // 500
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10),
                ReactiveTest.Subscribe(800));

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 490),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual();
        }

        [TestMethod]
        public void SkipUntil_SourceErrorBeforeCheckpoint_FailAndReload()
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

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnNext<int>(510, 1) // 520
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 490),
                ReactiveTest.Subscribe(800, 800)); // REVIEW: Why is this subscription recreated?

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 490),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnError<int>(500, ex));
        }

        [TestMethod]
        public void SkipUntil_SourceCompletedBeforeCheckpoint_FailAndReload()
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

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnNext<int>(510, 1) // 520
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 490),
                ReactiveTest.Subscribe(800, 800));

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 510),
                ReactiveTest.Subscribe(800, 800 + 510));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual();
        }

        [TestMethod]
        public void SkipUntil_SourceErrorAfterCheckpoint_FailAndReload()
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
                OnError<int>(510, ex) // 520
                                      // crash at 600
                );
            Assert.IsNotNull(source);

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnNext<int>(310, 1) // 320
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 510),
                ReactiveTest.Subscribe(800, 800 + 510));

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 310),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(10 + 400, 4),
                OnError<int>(10 + 510, ex),
                OnError<int>(800 + 510, ex));
        }

        [TestMethod]
        public void SkipUntil_SourceCompletedAfterCheckpoint_FailAndReload()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(100, 1), // 110
                OnNext(200, 2), // 210
                OnNext(300, 3), // 310
                OnNext(400, 4), // 410
                OnCompleted<int>(510) // 520
                                      // crash at 600
                );
            Assert.IsNotNull(source);

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnNext<int>(310, 1) // 320
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 510),
                ReactiveTest.Subscribe(800, 800 + 510));

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 310),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(10 + 400, 4),
                OnCompleted<int>(10 + 510),
                OnCompleted<int>(800 + 510));
        }

        [TestMethod]
        public void SkipUntil_TriggerErrorBeforeCheckpoint_FailAndReload()
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
                OnNext(490, 5), // 500
                OnNext(550, 6), // 550, 800 + 550
                                // crash at 600
                OnNext(900, 9) // 800 + 900
                );
            Assert.IsNotNull(source);

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnError<int>(490, ex) // 500
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 500),
                ReactiveTest.Subscribe(800, 800));

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 490),
                ReactiveTest.Subscribe(800, 800));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnError<int>(500, ex));
        }

        [TestMethod]
        public void SkipUntil_TriggerErrorAfterCheckpoint_FailAndReload()
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
                OnNext(490, 5), // 500
                OnNext(550, 6), // 550, 800 + 550
                                // crash at 600
                OnNext(900, 9) // 800 + 900
                );
            Assert.IsNotNull(source);

            var triggeringSource = GetTestableQbservable(reactive, "triggeringSource",
                OnError<int>(510, ex) // 520
                );
            Assert.IsNotNull(triggeringSource);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.SkipUntil(triggeringSource).Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 510),
                ReactiveTest.Subscribe(800, 800 + 510));

            var subTrigger = GetTestableSubscribable<int>("triggeringSource");
            subTrigger.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10, 10 + 510),
                ReactiveTest.Subscribe(800, 800 + 510));

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnError<int>(10 + 510, ex),
                OnError<int>(800 + 510, ex));
        }
    }
}
