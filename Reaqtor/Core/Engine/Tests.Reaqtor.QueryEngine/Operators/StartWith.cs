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
    public class StartWith : VirtualTimeEngineTest
    {
        [TestMethod]
        public void StartWith_BeforeSourceSubscribe()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var query = source.StartWith(-3, -2, -1);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(100, () =>
            {
                query.Subscribe(result, new Uri("s:/sub1"), null);
            });

            Scheduler.ScheduleAbsolute(102, () =>
            {
                Checkpoint(engine, store);
                RemoveQueryEngine(engine);
            });

            Scheduler.ScheduleAbsolute(200, () => Recover(CreateQueryEngine(), store));


            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                Subscribe(202, 252)
            );

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(101, -3),
                OnNext(201, -2),
                OnNext(202, -1),
                OnNext(212, 1),
                OnNext(222, 2),
                OnNext(232, 3),
                OnNext(242, 4),
                OnCompleted<int>(252)
            );
        }

        [TestMethod]
        public void StartWith_DuringSource()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var query = source.StartWith(-3, -2, -1);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(100, () =>
            {
                query.Subscribe(result, new Uri("s:/sub1"), null);
            });

            Scheduler.ScheduleAbsolute(130, () =>
            {
                Checkpoint(engine, store);
                RemoveQueryEngine(engine);
            });

            Scheduler.ScheduleAbsolute(200, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                Subscribe(103),
                Subscribe(200, 250)
            );

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(101, -3),
                OnNext(102, -2),
                OnNext(103, -1),
                OnNext(113, 1),
                OnNext(123, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void StartWith_AfterSourceComplete()
        {
            var store = new InMemoryStateStore("someId");
            var engine = CreateQueryEngine();
            var reactive = engine.GetReactiveService();

            var source = GetTestableQbservable(reactive, "source",
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var query = source.StartWith(-3, -2, -1);

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(100, () => query.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(200, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(210, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(220, () => Recover(CreateQueryEngine(), store));

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                Subscribe(103, 153),
                Subscribe(220, 220)
            );

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(101, -3),
                OnNext(102, -2),
                OnNext(103, -1),
                OnNext(113, 1),
                OnNext(123, 2),
                OnNext(133, 3),
                OnNext(143, 4),
                OnCompleted<int>(153)
            );
        }
    }
}
