// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;
using Reaqtor.QueryEngine.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class TwoQueryEnginesSanityTests : PhysicalTimeEngineTest
    {
#pragma warning disable IDE0060 // Remove unused parameter (MSTest)
        [ClassInitialize]
        public static void ClassSetup(TestContext ignored)
        {
            PhysicalTimeEngineTest.ClassSetup();
        }
#pragma warning restore IDE0060 // Remove unused parameter

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            PhysicalTimeEngineTest.ClassCleanup();
        }

        [TestInitialize]
        public new void TestInitialize()
        {
            base.Setup();
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.Cleanup();
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable IDE0063 // 'using' statement can be simplified (clarity of the code)

        [TestMethod]
        public void OneQueryTwoQueryEngines()
        {
            var qe1Id = "qe:/1/" + Guid.NewGuid();
            var qe2Id = "qe:/2/" + Guid.NewGuid();

            var qe1 = CreateQueryEngine(qe1Id);
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var qe2 = CreateQueryEngine(qe2Id);
            var ctx2 = GetQueryEngineReactiveService(qe2);

            AssertObservableNotCreated<int>("o1");
            AssertObserverNotCreated<int>("v1");

            ctx1.DefineObservable<int>(
                new Uri(qe1Id + "/obs1"),
                ctx1.GetObservable<string, int>(MockObservableUri)("o1").Where(x => x > 0).Take(10),
                null);

            var obs1 = ctx1.GetObservable<int>(new Uri(qe1Id + "/obs1"));

            ctx2.DefineObservable<int>(
                new Uri(qe2Id + "/obs2"),
                obs1.Where(x => x % 2 == 1).Take(5),
                null);

            var obs2 = ctx2.GetObservable<int>(new Uri(qe2Id + "/obs2"));

            var sub1 = obs2.Subscribe(
                ctx2.GetObserver<string, int>(MockObserverUri)("v1"), new Uri(qe2Id + "/sub1"), null);

            var o1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(o1);
            Assert.AreEqual(1, o1.SubscriptionCount);

            var o1s = o1.Synchronize(qe1.Scheduler);

            var v1 = GetMockObserver<int>("v1");

            for (int i = 0; i < 4; i++)
            {
                o1s.OnNext(i);
            }

            Assert.IsTrue(v1.WaitForCount(2, TimeSpan.FromSeconds(1)));

            Assert.IsFalse(v1.Completed);
            Assert.IsFalse(v1.Error);

            o1s.OnCompleted();

            Assert.IsTrue(v1.WaitForCompleted(TimeSpan.FromSeconds(1)));
            Assert.IsFalse(v1.Error);

            sub1.Dispose();

            Assert.AreEqual(0, o1.SubscriptionCount);

            AssertResult(v1, 2, (i, v) => Assert.AreEqual(i * 2 + 1, v));
        }

        [TestMethod]
        public void TwoQueryEnginesSecondFailAndRecover()
        {
            var qe1Id = "qe:/1/" + Guid.NewGuid();
            var qe2Id = "qe:/2/" + Guid.NewGuid();

            Uri sub = new Uri("my:/sub1");

            IObserver<int> o1s;
            MockObservable<int> o1;
            MockObserver<int> v1;

            InMemoryStateStore store;

            var qe1 = CreateQueryEngine(qe1Id);
            {
                var ctx1 = GetQueryEngineReactiveService(qe1);

                var qe2 = CreateQueryEngine(qe2Id);
                {
                    var ctx2 = GetQueryEngineReactiveService(qe2);

                    AssertObservableNotCreated<int>("o1");
                    AssertObserverNotCreated<int>("v1");

                    ctx1.DefineObservable<int>(
                        new Uri(qe1Id + "/obs1"),
                        ctx1.GetObservable<string, int>(MockObservableUri)("o1").Where(x => x > 0).Take(10),
                        null);

                    var obs1 = ctx1.GetObservable<int>(new Uri(qe1Id + "/obs1"));

                    ctx2.DefineObservable<int>(
                        new Uri(qe2Id + "/obs2"),
                        obs1.Where(x => x % 2 == 1).Take(5),
                        null);

                    var obs2 = ctx2.GetObservable<int>(new Uri(qe2Id + "/obs2"));

                    var sub1 = obs2.Subscribe(
                        ctx2.GetObserver<string, int>(MockObserverUri)("v1"), sub, null);

                    o1 = MockObservable.Get<int>("o1");
                    Assert.IsNotNull(o1);
                    Assert.AreEqual(1, o1.SubscriptionCount);

                    o1s = o1.Synchronize(qe1.Scheduler);

                    v1 = GetMockObserver<int>("v1");

                    for (int i = 0; i < 4; i++)
                    {
                        o1s.OnNext(i);
                    }

                    Assert.IsTrue(v1.WaitForCount(2, TimeSpan.FromSeconds(1)));

                    Assert.IsFalse(v1.Completed);
                    Assert.IsFalse(v1.Error);

                    AssertResult(v1, 2, (i, v) => Assert.AreEqual(i * 2 + 1, v));

                    store = Checkpoint(qe2);
                    RemoveQueryEngine(qe2);
                }

                qe2 = CreateQueryEngine(qe2Id);
                {
                    var ctx2 = GetQueryEngineReactiveService(qe2);

                    using (var stateReader = new InMemoryStateReader(store))
                    {
                        qe2.RecoverAsync(stateReader).Wait();
                    }

                    var sub1 = ctx2.GetSubscription(sub);
                    Assert.IsNotNull(sub1);

                    for (int i = 4; i < 20; i++)
                    {
                        o1s.OnNext(i);
                    }

                    Assert.IsTrue(v1.WaitForCount(5, TimeSpan.FromSeconds(1)));

                    Assert.IsTrue(v1.Completed);
                    Assert.IsFalse(v1.Error);

                    sub1.Dispose();

                    Assert.AreEqual(0, o1.SubscriptionCount);

                    AssertResult(v1, 5, (i, v) => Assert.AreEqual(i * 2 + 1, v));
                }
            }
        }

        [TestMethod]
        public void TwoQueryEnginesFirstFailAndRecover()
        {
            var qe1Id = "qe:/1/" + Guid.NewGuid();
            var qe2Id = "qe:/2/" + Guid.NewGuid();

            Uri sub = new Uri("bing:/sub1");

            IObserver<int> o1s;
            MockObservable<int> o1;
            MockObserver<int> v1;
            IReactiveQubscription sub1;

            InMemoryStateStore store;

            CheckpointingQueryEngine qe1 = CreateQueryEngine(qe1Id);

            var ctx1 = GetQueryEngineReactiveService(qe1);

            var qe2 = CreateQueryEngine(qe2Id);

            var ctx2 = GetQueryEngineReactiveService(qe2);

            //
            // BUG: There's a known test bug here with the resolver not being smart enough to deal with a disposed
            //      upstream engine. When QE2 triggers disposal of the upstream subscription (after having sent 5
            //      events causing Take to reach OnCompleted and thus disposing its source), its InputEdge is faced
            //      with an external subscription into the original QE1 instance. If this instance got disposed by
            //      means of `using (qe1)`, an ObjectDisposedException will propagate out of QE1 into the Dispose
            //      path triggered inside QE2.
            //
            //      In a real setup, the resolver returns IReactive interface implementations that encapsulate a
            //      transport layer where failures are detected and re-resolution takes place if there's a broken
            //      channel. For this particular case, the original instance of QE1 having disappeared would result
            //      in such an error (e.g. having failed over to another node in a cluster).
            //
            //      For our test setup, we lack re-resolution logic, so disposing QE1 is problematic. One option is
            //      to rewrite the mock resolver to have proper failure detection. Alternatively, this test would
            //      have to be built on a more realistic setup (e.g. using the remoting stack).
            //

            //using (qe1)
            {
                AssertObservableNotCreated<int>("o1");
                AssertObserverNotCreated<int>("v1");

                ctx1.DefineObservable<int>(
                    new Uri(qe1Id + "/obs1"),
                    ctx1.GetObservable<string, int>(MockObservableUri)("o1").Where(x => x > 0).Take(10),
                    null);

                var obs1 = ctx1.GetObservable<int>(new Uri(qe1Id + "/obs1"));

                ctx2.DefineObservable<int>(
                    new Uri(qe2Id + "/obs2"),
                    obs1.Where(x => x % 2 == 1).Take(5),
                    null);

                var obs2 = ctx2.GetObservable<int>(new Uri(qe2Id + "/obs2"));

                sub1 = obs2.Subscribe(
                    ctx2.GetObserver<string, int>(MockObserverUri)("v1"), sub, null);

                o1 = MockObservable.Get<int>("o1");
                Assert.IsNotNull(o1);
                Assert.AreEqual(1, o1.SubscriptionCount);

                o1s = o1.Synchronize(qe1.Scheduler);

                v1 = GetMockObserver<int>("v1");

                for (int i = 0; i < 4; i++)
                {
                    o1s.OnNext(i);
                }

                Assert.IsTrue(v1.WaitForCount(2, TimeSpan.FromSeconds(1)));

                Assert.IsFalse(v1.Completed);
                Assert.IsFalse(v1.Error);

                AssertResult(v1, 2, (i, v) => Assert.AreEqual(i * 2 + 1, v));

                store = Checkpoint(qe1);
                RemoveQueryEngine(qe1);
            }

            o1.DropAllSubscriptions();
            Assert.AreEqual(0, o1.SubscriptionCount);

            qe1 = CreateQueryEngine(qe1Id);

            //using (qe1)
            {
                using (var stateReader = new InMemoryStateReader(store))
                {
                    qe1.RecoverAsync(stateReader).Wait();
                }

                Assert.AreEqual(1, o1.SubscriptionCount);

                o1s = o1.Synchronize(qe1.Scheduler);

                for (int i = 4; i < 20; i++)
                {
                    o1s.OnNext(i);
                }

                Assert.IsTrue(v1.WaitForCount(5, TimeSpan.FromSeconds(1)));
                Assert.IsTrue(v1.WaitForCompleted(TimeSpan.FromSeconds(1)));

                Assert.IsTrue(v1.Completed);
                Assert.IsFalse(v1.Error);

                sub1.Dispose();

                Assert.AreEqual(0, o1.SubscriptionCount);

                AssertResult(v1, 5, (i, v) => Assert.AreEqual(i * 2 + 1, v));
            }
        }

        [TestMethod]
        public void ErrorPropagationThruTwoEngines()
        {
            var qe1Id = "qe:/1/" + Guid.NewGuid();
            var qe2Id = "qe:/2/" + Guid.NewGuid();

            var engine1 = CreateQueryEngine(qe1Id);
            var reactive1 = GetQueryEngineReactiveService(engine1);

            var engine2 = CreateQueryEngine(qe2Id);
            var reactive2 = GetQueryEngineReactiveService(engine2);

            var engine1ObservableUri = new Uri(qe1Id + "/observable1");
            reactive1.DefineObservable(
                engine1ObservableUri,
                reactive1.GetObservable<string, int>(MockObservableUri)("source").Where(x => x >= 0),
                null);

            var engine1Observable = reactive1.GetObservable<int>(engine1ObservableUri);

            var engine2ObservableUri = new Uri(qe2Id + "/observable2");
            reactive2.DefineObservable(
                engine2ObservableUri,
                engine1Observable.Take(10),
                null);

            var subscriptionUri = new Uri("reactor:/subscription");
            var sub = reactive2
                .GetObservable<int>(engine2ObservableUri)
                .Subscribe(reactive2.GetObserver<string, int>(MockObserverUri)("result"), subscriptionUri, null);

            var src = GetObservable<int>("source");
            var srcs = src.Synchronize(engine1.Scheduler);

            srcs.OnNext(0);
            var result = GetObserver<int>("result");
            Assert.IsTrue(result.WaitForCount(1, TimeSpan.FromSeconds(1)));

            AssertResult(result, 1, Assert.AreEqual);
            Assert.IsFalse(result.Completed);
            Assert.IsFalse(result.Error);

            srcs.OnError(new Exception());
            Assert.IsTrue(result.WaitForError(TimeSpan.FromSeconds(1)));
            Assert.IsFalse(result.Completed);
            Assert.IsTrue(result.Error);

            sub.Dispose();
            RemoveQueryEngine(engine1);
            RemoveQueryEngine(engine2);
        }

#pragma warning restore IDE0063 // 'using' statement can be simplified
#pragma warning restore IDE0079 // Remove unnecessary suppression
    }
}
