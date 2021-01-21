// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.QueryEngine.KeyValueStore.InMemory;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public sealed class VirtualTimeEngineTests : VirtualTimeEngineTest
    {
        #region +++Checkpointing+++

        [TestMethod]
        public void CheckpointSingleEngine()
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

            var result = GetTestableQbserver<int>(reactive, "result");

            Scheduler.ScheduleAbsolute(10, () => source.Subscribe(result, new Uri("s:/sub1"), null));
            Scheduler.ScheduleAbsolute(510, () => Checkpoint(engine, store));
            Scheduler.ScheduleAbsolute(600, () => RemoveQueryEngine(engine));
            Scheduler.ScheduleAbsolute(800, () => CreateQueryEngine().RecoverAsync(new InMemoryStateReader(store)).Wait());

            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                Subscribe(10),
                Subscribe(800)
            );

            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(
                OnNext(110, 1),
                OnNext(210, 2),
                OnNext(310, 3),
                OnNext(410, 4),
                OnNext(500, 5),
                OnNext(800 + 900, 9)
            );
        }

        #endregion

        #region +++Processing+++

        [TestMethod]
        public void HugeBatch()
        {
            const int BatchSize = 1024;
            var engine1 = CreateQueryEngine("reactor:/engine1");
            var reactive1 = engine1.GetReactiveService();

            var engine2 = CreateQueryEngine("reactor:/engine2");
            var reactive2 = engine2.GetReactiveService();

            var batch = new Recorded<Notification<int>>[BatchSize];
            for (int i = 0; i < BatchSize; i++)
            {
                batch[i] = ReactiveTest.OnNext(100, i);
            }

            var source = GetTestableQbservable(reactive1, "source", batch.ToArray());
            Assert.IsNotNull(source);

            var engine1ObservableUri = new Uri("reactor:/engine1/middle");
            reactive1.DefineObservable(engine1ObservableUri, source.Where(x => true), null);
            var engine1Observable = reactive1.GetObservable<int>(engine1ObservableUri);

            var engine2ObservableUri = new Uri("reactor:/engine2/middle");
            reactive2.DefineObservable(engine2ObservableUri, engine1Observable.Where(x => true), null);
            var engine2Observable = reactive2.GetObservable<int>(engine1ObservableUri);

            var result = GetTestableQbserver<int>(reactive2, "result");

            Scheduler.ScheduleAbsolute(10, () => engine2Observable.Subscribe(result, new Uri("reactor:/subscription"), null));
            Scheduler.Start();

            var sub = GetTestableSubscribable<int>("source");
            sub.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(10));

            var output = new Recorded<Notification<int>>[BatchSize];
            for (int i = 0; i < BatchSize; i++)
            {
                output[i] = ReactiveTest.OnNext(10 + 100, i);
            }
            var res = GetTestableObserver<int>("result");
            res.Messages.AssertEqual(output);
        }

        #endregion
    }
}
