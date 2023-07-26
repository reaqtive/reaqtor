// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Expressions;
using Reaqtive.Linq;
using Reaqtive.Subjects;
using Reaqtive.Testing;

using Reaqtor;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.Events;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;
using Reaqtor.QueryEngine.Metrics;
using Reaqtor.QueryEngine.Mocks;
using Reaqtor.Reliable;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class CheckpointingTests : PhysicalTimeEngineTest
    {
#pragma warning disable IDE0060 // Remove unused parameter (MSTest)
        [ClassInitialize]
        public static void ClassSetup(TestContext ignored) => PhysicalTimeEngineTest.ClassSetup();
#pragma warning restore IDE0060 // Remove unused parameter

        [ClassCleanup]
        public static new void ClassCleanup() => PhysicalTimeEngineTest.ClassCleanup();

        [TestInitialize]
        public new void TestInitialize() => base.Setup();

        [TestCleanup]
        public new void TestCleanup() => base.Cleanup();

        [TestMethod]
        public void InvalidParametersForCheckpointing()
        {
            var engine = CreateQueryEngine();
            var invalidWriter = new InMemoryStateWriter(new InMemoryStateStore("c1"), (CheckpointKind)int.MaxValue);

            static void rethrowInnerException(Action func)
            {
                try
                {
                    func();
                }
                catch (AggregateException e)
                {
                    throw e.InnerException;
                }
            }

            ReactiveAssert.Throws<ArgumentNullException>(() => rethrowInnerException(() => engine.CheckpointAsync(null).Wait()));
            ReactiveAssert.Throws<ArgumentNullException>(() => rethrowInnerException(() => engine.RecoverAsync(null).Wait()));
            ReactiveAssert.Throws<InvalidOperationException>(() => rethrowInnerException(() => engine.CheckpointAsync(invalidWriter).Wait()));
        }

        [TestMethod]
        public void CleanDeletedEntitiesWithNextCheckpoint()
        {
            var checkpointWithEntities = new InMemoryStateStore("checkpoint1");
            var checkpointWithoutEntities = new InMemoryStateStore("checkpoint2");
            var sourceUri = new Uri("reactor:/source");
            var subscriptionUri = new Uri("reactor:/subscription");
            var observerUri = new Uri("reactor:/observer");
            var observableUri = new Uri("reactor:/observable");
            var anotherSubscriptionUri = new Uri("reactor:/anothersubscription");

            {
                var engine = CreateQueryEngine();
                var reactive = GetQueryEngineReactiveService(engine);

                // Create stream.
                var source = reactive.CreateStream<int>(sourceUri);
                var observable = reactive.GetObservable<int>(sourceUri);

                // Create observer
                reactive.DefineObserver<object, int>(observerUri, (v) => Observer.Create<int>((value) => Assert.AreEqual(value, 1)).AsQbserver(), null);

                // Create observable
                reactive.DefineObservable<int, int>(observableUri, (value) => Observable.Return(value).AsQbservable(), null);

                // Create subscription
                var subscription = observable.Subscribe(reactive.GetObserver<string, int>(MockObserverUri)("result"), subscriptionUri, null);

                var srcs = source.Synchronize(engine.Scheduler);

                srcs.OnNext(0);
                AssertResult(GetObserver<int>("result"), 1, Assert.AreEqual);

                // Take checkpoint with stream.
                Checkpoint(engine, checkpointWithEntities);

                reactive.UndefineObserver(observerUri);
                reactive.UndefineObservable(observableUri);
                subscription.Dispose();
                source.Dispose();

                Checkpoint(engine, checkpointWithoutEntities);

                RemoveQueryEngine(engine);
            }

            // Check that entities exist in the first checkpoint.
            {
                var engine = CreateQueryEngine();
                var reactive = GetQueryEngineReactiveService(engine);

                Recover(engine, checkpointWithEntities);

                var source = reactive.GetStream<int, int>(sourceUri);

                var srcs = source.Synchronize(engine.Scheduler);
                srcs.OnNext(1);
                AssertResult(GetObserver<int>("result"), 2, Assert.AreEqual);

                reactive.GetObservable<int, int>(observableUri)(1)
                    .Subscribe(
                        reactive.GetObserver<object, int>(observerUri)(null),
                        anotherSubscriptionUri, null);

                RemoveQueryEngine(engine);
            }

            // Check that entities do not exist in the second.
            {
                var engine = CreateQueryEngine();
                var reactive = GetQueryEngineReactiveService(engine);

                Recover(engine, checkpointWithoutEntities);

                var source = reactive.GetStream<int, int>(sourceUri);
                var srcs = source.Synchronize(engine.Scheduler);
                var noStream = false;
                try
                {
                    srcs.OnNext(2);
                }
                catch (ArgumentException)
                {
                    noStream = true;
                }

                Assert.IsTrue(noStream);
                AssertResult(GetObserver<int>("result"), 2, Assert.AreEqual);

                var noDefinitions = false;
                try
                {
                    reactive.GetObservable<int, int>(observableUri)(1)
                    .Subscribe(
                        reactive.GetObserver<object, int>(observerUri)(null),
                        new Uri("reactor:/shouldfail"), null);
                }
                catch (InvalidOperationException)
                {
                    noDefinitions = true;
                }

                Assert.IsTrue(noDefinitions);
            }
        }

        [TestMethod]
        public void CheckpointSanityCheck()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            var str1 = new Uri("str:/s1");

            var s1 = ctx.CreateStream<int>(str1);
            Assert.IsNotNull(s1);

            var s1o = ctx.GetObservable<int>(str1);
            Assert.IsNotNull(s1o);

            AssertObserverNotCreated<int>("v1");

            var sub = s1o.Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null);

            var v1 = GetMockObserver<int>("v1");
            Assert.AreEqual(new Uri("s:/sub1"), v1.InstanceId);

            var s1v = ctx.GetObserver<int>(str1);
            Assert.IsNotNull(s1v);

            var s1vs = s1v.Synchronize(qe.Scheduler);

            for (var i = 0; i < 5; i++)
            {
                s1vs.OnNext(i);
            }

            var store = Checkpoint(qe);

            sub.Dispose();
            s1.Dispose();
            RemoveQueryEngine(qe);

            AssertResult(v1, 5, (i, v) => Assert.AreEqual(i, v));

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);
            Recover(qe2, store);
            s1v = ctx2.GetObserver<int>(str1);
            s1vs = s1v.Synchronize(qe2.Scheduler);

            for (var i = 5; i < 10; i++)
            {
                s1vs.OnNext(i);
            }

            AssertResult(v1, 10, (i, v) => Assert.AreEqual(i, v));
        }

        [TestMethod]
        public void CheckpointSanityCheck_V3_WithTemplates()
        {
            var qe = CreateQueryEngine("qe:/" + Guid.NewGuid(), GetScheduler(), serializationPolicy: new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));
            qe.Options.TemplatizeExpressions = true;

            var ctx = GetQueryEngineReactiveService(qe);

            var str1 = new Uri("str:/s1");

            var s1 = ctx.CreateStream<int>(str1);
            Assert.IsNotNull(s1);

            var s1o = ctx.GetObservable<int>(str1);
            Assert.IsNotNull(s1o);

            AssertObserverNotCreated<int>("v1");

            var sub = s1o.Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null);

            var v1 = GetMockObserver<int>("v1");
            Assert.AreEqual(new Uri("s:/sub1"), v1.InstanceId);

            var s1v = ctx.GetObserver<int>(str1);
            Assert.IsNotNull(s1v);

            var s1vs = s1v.Synchronize(qe.Scheduler);

            for (var i = 0; i < 5; i++)
            {
                s1vs.OnNext(i);
            }

            var store = Checkpoint(qe);

            sub.Dispose();
            s1.Dispose();
            RemoveQueryEngine(qe);

            AssertResult(v1, 5, (i, v) => Assert.AreEqual(i, v));

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);
            Recover(qe2, store);
            s1v = ctx2.GetObserver<int>(str1);
            s1vs = s1v.Synchronize(qe2.Scheduler);

            for (var i = 5; i < 10; i++)
            {
                s1vs.OnNext(i);
            }

            AssertResult(v1, 10, (i, v) => Assert.AreEqual(i, v));
        }

        [TestMethod]
        public void CheckpointSanityCheck_V3_WithoutTemplates()
        {
            var qe = CreateQueryEngine("qe:/" + Guid.NewGuid(), GetScheduler(), serializationPolicy: new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));

            var ctx = GetQueryEngineReactiveService(qe);

            var str1 = new Uri("str:/s1");

            var s1 = ctx.CreateStream<int>(str1);
            Assert.IsNotNull(s1);

            var s1o = ctx.GetObservable<int>(str1);
            Assert.IsNotNull(s1o);

            AssertObserverNotCreated<int>("v1");

            var sub = s1o.Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null);

            var v1 = GetMockObserver<int>("v1");
            Assert.AreEqual(new Uri("s:/sub1"), v1.InstanceId);

            var s1v = ctx.GetObserver<int>(str1);
            Assert.IsNotNull(s1v);

            var s1vs = s1v.Synchronize(qe.Scheduler);

            for (var i = 0; i < 5; i++)
            {
                s1vs.OnNext(i);
            }

            var store = Checkpoint(qe);

            sub.Dispose();
            s1.Dispose();
            RemoveQueryEngine(qe);

            AssertResult(v1, 5, (i, v) => Assert.AreEqual(i, v));

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);
            Recover(qe2, store);
            s1v = ctx2.GetObserver<int>(str1);
            s1vs = s1v.Synchronize(qe2.Scheduler);

            for (var i = 5; i < 10; i++)
            {
                s1vs.OnNext(i);
            }

            AssertResult(v1, 10, (i, v) => Assert.AreEqual(i, v));
        }

        [TestMethod]
        public void CheckpointSelectManySanityCheck()
        {
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx1.GetObservable<string, int>(MockObservableUri)("o2");

            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1.SelectMany(x => o2.Where(y => x < y), (x, y) => x + y).Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");

            var in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(3);

            var in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            var in2s = in2.Synchronize(qe1.Scheduler);

            in2s.OnNext(4);
            in2s.OnNext(10);

            AssertResultSequence(out1, new int[] { 7, 13 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe1);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            in1s = in1.Synchronize(qe2.Scheduler);

            in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            in2s = in2.Synchronize(qe2.Scheduler);

            in2s.OnNext(2);
            in2s.OnNext(5);

            AssertResultSequence(out1, new int[] { 8 });

            in1s.OnNext(10);

            in2s.OnNext(1);
            in2s.OnNext(6);

            Assert.IsTrue(out1.WaitForCount(2, Timeout));
            AssertResultSequence(out1, new int[] { 8, 9 });

            in2s.OnNext(11);

            Assert.IsTrue(out1.WaitForCount(4, Timeout));

            AssertResultSequence(out1, new int[] { 8, 9, 14, 21 });

            sub1.Dispose();
        }

        [TestMethod]
        public void CheckpointSwitchSanityCheck()
        {
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var generator = ctx1.GetObservable<string, int>(MockObservableUri);
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .Select(x => generator("o" + x))
                .Switch()
                .Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");

            var in1 = GetObservable<int>("o1");
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(2);  // generates observable identified as o2

            var in2 = GetObservable<int>("o2");
            var in2s = in2.Synchronize(qe1.Scheduler);

            in2s.OnNext(97);

            in1s.OnNext(3);  // generates observable identified as o3

            var in3 = GetObservable<int>("o3");
            var in3s = in3.Synchronize(qe1.Scheduler);

            in2s.OnNext(98); // ignored
            in3s.OnNext(99);

            AssertResultSequence(out1, new int[] { 97, 99 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe1);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            in1s = in1.Synchronize(qe2.Scheduler);

            in3 = GetObservable<int>("o3");
            in3s = in3.Synchronize(qe2.Scheduler);

            in3s.OnNext(48);

            in1s.OnNext(4);  // generates observable identified as o4

            var in4 = GetObservable<int>("o4");
            var in4s = in4.Synchronize(qe2.Scheduler);

            in3s.OnNext(49); // ignored
            in4s.OnNext(50);

            AssertResultSequence(out1, new int[] { 48, 50 });
        }

        [TestMethod]
        public void CheckpointSwitchStartWithSanityCheck()
        {
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var generator = ctx1.GetObservable<string, int>(MockObservableUri);
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var l = LockManager.NewAutoReset();    // <-- StartWith asynchrony

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .AwaitSubscribe(l)
                .Select(x => generator("o" + x))
                .StartWith(generator("o0"))        // <-- introduces concurrency
                .Switch()
                .Subscribe(v1, sub1uri, null);

            Assert.IsTrue(LockManager.Wait(l));

            var out1 = GetMockObserver<int>("v1");

            var in1 = GetObservable<int>("o1");
            var in1s = in1.Synchronize(qe1.Scheduler);

            var in0 = GetObservable<int>("o0");
            var in0s = in0.Synchronize(qe1.Scheduler);

            AssertResultSequence(out1, Array.Empty<int>());

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe1);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            Assert.IsTrue(LockManager.Wait(l));

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            in1s = in1.Synchronize(qe2.Scheduler);

            in0 = GetObservable<int>("o0");
            in0s = in0.Synchronize(qe2.Scheduler);

            in0s.OnNext(42);

            in1s.OnNext(2);  // generates observable identified as o2

            var in2 = GetObservable<int>("o2");
            var in2s = in2.Synchronize(qe2.Scheduler);

            in0s.OnNext(43); // ignored
            in2s.OnNext(44);

            AssertResultSequence(out1, new int[] { 42, 44 });

            // Checkpoint -------------------------------------------------------------

            var chkpt2 = Checkpoint(qe2);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe2);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe3 = CreateQueryEngine();
            var ctx3 = GetQueryEngineReactiveService(qe3);

            Recover(qe3, chkpt2);

            sub1 = ctx3.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            Assert.IsTrue(LockManager.Wait(l));

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            in1s = in1.Synchronize(qe3.Scheduler);

            in2 = GetObservable<int>("o2");
            in2s = in2.Synchronize(qe3.Scheduler);

            in2s.OnNext(45);

            in1s.OnNext(3);  // generates observable identified as o3

            var in3 = GetObservable<int>("o3");
            var in3s = in3.Synchronize(qe3.Scheduler);

            in2s.OnNext(46); // ignored
            in3s.OnNext(47);

            AssertResultSequence(out1, new int[] { 45, 47 });
        }

        [TestMethod]
        public void CheckpointPoorCombineLatestCarrierPigeonOfHigherOrderSequences()
        {
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var ol = ctx1.GetObservable<string, int>(MockObservableUri)("ol");
            var or = ctx1.GetObservable<string, int>(MockObservableUri)("or");
            var generator = ctx1.GetObservable<string, int>(MockObservableUri);
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0050 // Convert to tuple (https://github.com/dotnet/roslyn/issues/50468)

            var sub1 = ol
                .CombineLatest(or.Select(x => generator("o" + x)), (l, r) => new { l, r })
                .SelectMany(lr => lr.r, (lr, r) => lr.l + r)
                .Subscribe(v1, sub1uri, null);

#pragma warning restore IDE0050
#pragma warning restore IDE0079

            var out1 = GetMockObserver<int>("v1");

            var inl = GetObservable<int>("ol");
            var inls = inl.Synchronize(qe1.Scheduler);

            var inr = GetObservable<int>("or");
            var inrs = inr.Synchronize(qe1.Scheduler);

            inls.OnNext(1); // CL(l = 1, r = ?)

            AssertResultSequence(out1, Array.Empty<int>());

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe1);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            inl = GetObservable<int>("ol");
            inls = inl.Synchronize(qe2.Scheduler);

            inr = GetObservable<int>("or");
            inrs = inr.Synchronize(qe2.Scheduler);

            inrs.OnNext(2); // CL(l = 1, r = Quoted{o2}) & SM(Persisted{o2.Sub(b1)})

            AssertResultSequence(out1, Array.Empty<int>());

            // Checkpoint -------------------------------------------------------------

            var chkpt2 = Checkpoint(qe2);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe2);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe3 = CreateQueryEngine();
            var ctx3 = GetQueryEngineReactiveService(qe3);

            Recover(qe3, chkpt2);

            sub1 = ctx3.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            inl = GetObservable<int>("ol");
            inls = inl.Synchronize(qe3.Scheduler);

            inr = GetObservable<int>("or");
            inrs = inr.Synchronize(qe3.Scheduler);

            var in2 = GetObservable<int>("o2");
            var in2s = in2.Synchronize(qe3.Scheduler);

            Assert.AreEqual(1, in2.SubscriptionCount);

            in2s.OnNext(42); // -> 1 + 42
            in2s.OnNext(43); // -> 1 + 43

            inrs.OnNext(3); // CL(l = 1, r = Quoted{o3}) & SM(Persisted{1, o2.Sub(b1)}, Persisted{1, o3.Sub(b2)})

            var in3 = GetObservable<int>("o3");
            var in3s = in3.Synchronize(qe3.Scheduler);

            Assert.AreEqual(1, in3.SubscriptionCount);

            in3s.OnNext(44); // -> 1 + 44

            inls.OnNext(2); // CL(l = 2, r = Quoted{o3}) & SM(Persisted{1, o2.Sub(b1)}, Persisted{1, o3.Sub(b2)}, Persisted{2, o3.Sub(b2)})

            Assert.AreEqual(2, in3.SubscriptionCount);

            in3s.OnNext(45); // -> 1 + 45, 2 + 45
            in3s.OnNext(47); // -> 1 + 47, 2 + 47

            AssertResultSequenceSet(out1, new int[] { 43, 44, 45, 46, 47, 48, 49 }); // SelectMany concurrent merge allows for different orderings

            // Checkpoint -------------------------------------------------------------

            var chkpt3 = Checkpoint(qe3);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe3);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe4 = CreateQueryEngine();
            var ctx4 = GetQueryEngineReactiveService(qe4);

            Recover(qe4, chkpt3);

            sub1 = ctx4.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            inl = GetObservable<int>("ol");
            inls = inl.Synchronize(qe4.Scheduler);

            inr = GetObservable<int>("or");
            inrs = inr.Synchronize(qe4.Scheduler);

            in2 = GetObservable<int>("o2");
            in2s = in2.Synchronize(qe4.Scheduler);

            Assert.AreEqual(1, in2.SubscriptionCount);

            in3 = GetObservable<int>("o3");
            in3s = in3.Synchronize(qe4.Scheduler);

            Assert.AreEqual(2, in3.SubscriptionCount);

            in2s.OnNext(49); // -> 1 + 49
            in3s.OnNext(50); // -> 1 + 50, 2 + 50

            AssertResultSequenceSet(out1, new int[] { 50, 51, 52 }); // SelectMany concurrent merge allows for different orderings

            in2s.OnCompleted();
            in3s.OnCompleted();
            inls.OnCompleted();
            inrs.OnCompleted();

            Assert.AreEqual(0, inl.SubscriptionCount);
            Assert.AreEqual(0, inr.SubscriptionCount);
            Assert.AreEqual(0, in2.SubscriptionCount);
            Assert.AreEqual(0, in3.SubscriptionCount);
            Assert.IsTrue(out1.Completed);
        }

        [TestMethod]
        public void CheckpointWindowSanityCheck()
        {
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx1.GetObservable<string, int>(MockObservableUri)("o2");

            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var l = LockManager.NewAutoReset();    // <-- Window asynchrony

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .AwaitSubscribe(l)
                .Window(3)
                .SelectMany(w => w.Sum(), (x, y) => y)
                .Subscribe(v1, sub1uri, null);

            Assert.IsTrue(LockManager.Wait(l));

            var out1 = GetMockObserver<int>("v1");

            var in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(1);
            in1s.OnNext(2);
            in1s.OnNext(3);
            in1s.OnNext(4);

            AssertResultSequence(out1, new int[] { 6 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            var dir1 = Dir(chkpt1);

            Assert.AreEqual(1, dir1["Subscriptions"].Count(s => s.StartsWith("rx://bridge")));
            Assert.AreEqual(1, dir1["Subjects"].Count(s => s.StartsWith("rx://bridge")));
            Assert.AreEqual(1, dir1["Subjects"].Count(s => s.StartsWith("rx://tunnel")));
            Assert.AreEqual(1, dir1["Subjects"].Count(s => s.StartsWith("rx://tollbooth")));

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe1);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            Assert.IsTrue(LockManager.Wait(l));

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            in1s = in1.Synchronize(qe2.Scheduler);

            in1s.OnNext(5);
            in1s.OnNext(6);

            AssertResultSequence(out1, new int[] { 15 });

            // Checkpoint -------------------------------------------------------------

            var chkpt2 = Checkpoint(qe2);

            var dir2 = Dir(chkpt2);

            Assert.AreEqual(1, dir2["Subscriptions"].Count(s => s.StartsWith("rx://bridge")));
            Assert.AreEqual(1, dir2["Subjects"].Count(s => s.StartsWith("rx://bridge")));
            Assert.AreEqual(1, dir2["Subjects"].Count(s => s.StartsWith("rx://tunnel")));
            Assert.AreEqual(1, dir2["Subjects"].Count(s => s.StartsWith("rx://tollbooth")));

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe2);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe3 = CreateQueryEngine();
            var ctx3 = GetQueryEngineReactiveService(qe3);

            Recover(qe3, chkpt2);

            sub1 = ctx3.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            Assert.IsTrue(LockManager.Wait(l));

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            in1s = in1.Synchronize(qe3.Scheduler);

            in1s.OnNext(7);
            in1s.OnNext(8);
            in1s.OnNext(9);

            AssertResultSequence(out1, new int[] { 24 });

            in1s.OnNext(10);

            sub1.Dispose();

            // Checkpoint -------------------------------------------------------------

            var chkpt3 = Checkpoint(qe3);

            var dir3 = Dir(chkpt3);

            Assert.AreEqual(0, dir3["Subscriptions"].Count(s => s.StartsWith("rx://bridge")));
            Assert.AreEqual(0, dir3["Subjects"].Count(s => s.StartsWith("rx://bridge")));
            Assert.AreEqual(0, dir3["Subjects"].Count(s => s.StartsWith("rx://tunnel")));
            Assert.AreEqual(0, dir3["Subjects"].Count(s => s.StartsWith("rx://tollbooth")));
        }

        private static GentleDictionary<string, IEnumerable<string>> Dir(InMemoryStateStore store)
        {
            var res = new Dictionary<string, IEnumerable<string>>();

            foreach (var cat in store.GetCategories())
            {
                if (store.TryGetItemKeys(cat, out var keys))
                {
                    var lst = new List<string>();
                    res[cat] = lst;

                    var rem = store.GetRemovedItems(cat);
                    lst.AddRange(keys.Except(rem));
                }
            }

            return new GentleDictionary<string, IEnumerable<string>>(res, () => Array.Empty<string>());
        }

        private sealed class GentleDictionary<K, T>
        {
            private readonly IDictionary<K, T> _dictionary;
            private readonly Func<T> _getEmpty;

            public GentleDictionary(IDictionary<K, T> dictionary, Func<T> getEmpty)
            {
                _dictionary = dictionary;
                _getEmpty = getEmpty;
            }

            public T this[K key]
            {
                get
                {
                    if (!_dictionary.TryGetValue(key, out var res))
                    {
                        res = _getEmpty();
                    }

                    return res;
                }
            }
        }

        [TestMethod]
        public void CheckpointAlmightyEnvironmentThouShaltNotGetLost()
        {
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var generator = ctx1.GetObservable<string, int>(MockObservableUri);
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .SelectMany(x => generator("o" + x).SelectMany(y => generator("o" + (x + y)), (y, z) => y * z), (x, a) => x + a)
                .Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");

            var in1 = GetObservable<int>("o1");
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(2); // o2.SM(y => o(2 + y))

            var in2 = GetObservable<int>("o2");
            var in2s = in2.Synchronize(qe1.Scheduler);

            in2s.OnNext(3); // o(2 + 3)

            var in5 = GetObservable<int>("o5");
            var in5s = in5.Synchronize(qe1.Scheduler);

            in5s.OnNext(17); // 2 + 3 * 17

            AssertResultSequence(out1, new int[] { 2 + 3 * 17 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe1);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in2 = GetObservable<int>("o2");
            in2s = in2.Synchronize(qe2.Scheduler);

            in2s.OnNext(4); // o(2 + 4)  <--  where 2 got restored

            var in6 = GetObservable<int>("o6");
            var in6s = in6.Synchronize(qe2.Scheduler);

            in6s.OnNext(24); // 2 + 4 * 24

            AssertResultSequence(out1, new int[] { 2 + 4 * 24 });
        }

        [TestMethod]
        public void FullAndDifferentialCheckpoint()
        {
            var store = new InMemoryStateStore("someId");
            var str1 = new Uri("str:/s1");
            var sub1 = new Uri("s:/sub1");
            MockObserver<int> result;
            {
                // Define some entities before crash.
                var qe = CreateQueryEngine();
                var ctx = GetQueryEngineReactiveService(qe);

                var s1 = ctx.CreateStream<int>(str1);
                Assert.IsNotNull(s1);

                var s1o1 = ctx.GetObservable<int>(str1);
                Assert.IsNotNull(s1o1);

                Checkpoint(qe, store);
                RemoveQueryEngine(qe);
            }

            {
                var qe1 = CreateQueryEngine();
                var ctx1 = GetQueryEngineReactiveService(qe1);

                Recover(qe1, store);
                AssertObserverNotCreated<int>("v1");

                var s1o = ctx1.GetObservable<int>(str1).Take(2);
                var sub = s1o.Subscribe(ctx1.GetObserver<string, int>(MockObserverUri)("v1"), sub1, null);

                result = GetMockObserver<int>("v1");

                var s1v = ctx1.GetObserver<int>(str1);
                Assert.IsNotNull(s1v);

                var s1vs = s1v.Synchronize(qe1.Scheduler);

                s1vs.OnNext(0);

                Checkpoint(qe1, store);
                AssertResult(result, 1, (i, v) => Assert.AreEqual(i, v));

                RemoveQueryEngine(qe1);
            }

            {
                var qe2 = CreateQueryEngine();
                var ctx2 = GetQueryEngineReactiveService(qe2);

                Recover(qe2, store);

                var s2v = ctx2.GetObserver<int>(str1);

                var s2vs = s2v.Synchronize(qe2.Scheduler);

                for (var i = 1; i < 3; i++)
                {
                    s2vs.OnNext(i);
                }

                AssertResult(result, 2, (i, v) => Assert.AreEqual(i, v));
                ctx2.GetSubscription(sub1).Dispose();
            }
        }

        [TestMethod]
        public void TolerateCheckpointCommitFailures()
        {
            var store = new InMemoryStateStore("someId");

            var sourceUri = new Uri("str:/s1");
            var subUri = new Uri("s:/sub1");
            {
                var qe = CreateQueryEngine();
                var ctx = GetQueryEngineReactiveService(qe);

                var sourceStream = ctx.CreateStream<int>(sourceUri);
                Assert.IsNotNull(sourceStream);

                var source = ctx.GetObservable<int>(sourceUri);
                Assert.IsNotNull(source);

                var sub = source.Take(2)
                    .Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("result"), subUri, null);

                var res = MockObserver.Get<int>("result");
                Assert.IsNotNull(res);
                Assert.IsFalse(res.Completed);
                Assert.IsFalse(res.Error);

                var src = ctx.GetObserver<int>(sourceUri);

                var srcs = src.Synchronize(qe.Scheduler);

                srcs.OnNext(0);
                srcs.OnNext(1);

                // Take full and fail.

                var hasCheckpointFailed = false;

                using (var stateWriter = new InMemoryStateWriter(store, CheckpointKind.Full, onCommit: () =>
                {
                    throw new InvalidOperationException();
                }))
                {
                    try
                    {
                        qe.CheckpointAsync(stateWriter).Wait();
                    }
                    catch (Exception)
                    {
                        hasCheckpointFailed = true;
                    }
                }
                Assert.IsTrue(hasCheckpointFailed);

                // Take another differential
                using (var stateWriter = new InMemoryStateWriter(store, CheckpointKind.Differential))
                {
                    qe.CheckpointAsync(stateWriter).Wait();
                }
                RemoveQueryEngine(qe);
            }

            {
                var qe1 = CreateQueryEngine();
                var ctx = GetQueryEngineReactiveService(qe1);

                Recover(qe1, store);
                var sub = ctx.GetSubscription(subUri);
                var src = ctx.GetObserver<int>(sourceUri);
                var srcs = src.Synchronize(qe1.Scheduler);
                srcs.OnNext(2);
                srcs.OnNext(3);

                var res = MockObserver.Get<int>("result");
                Assert.IsNotNull(res);
                Assert.IsTrue(res.Completed);
                Assert.IsFalse(res.Error);
                AssertResult(res, 2, (i, v) => Assert.AreEqual(i, v));
            }
        }

#if NETFRAMEWORK
        [TestMethod]
        public void RestoreInDifferentDomains()
        {
            var checkpoint = RunInDifferentDomain(
                (CheckpointingTests test) =>
                {
                    var store = new InMemoryStateStore("someId");
                    var sourceUri = new Uri("reactor:/source");
                    var subscriptionUri = new Uri("reactor:/subscription");

                    var engine = test.CreateQueryEngine();
                    var reactive = GetQueryEngineReactiveService(engine);

                    var sourceStream = reactive.CreateStream<int>(sourceUri);
                    Assert.IsNotNull(sourceStream);

                    var source = reactive.GetObservable<int>(sourceUri);
                    Assert.IsNotNull(source);

                    source.Take(2).Subscribe(reactive.GetObserver<string, int>(MockObserverUri)("result"), subscriptionUri, null);

                    var res = MockObserver.Get<int>("result");
                    Assert.IsNotNull(res);

                    var o1 = reactive.GetObserver<int>(sourceUri);

                    var o1s = o1.Synchronize(engine.Scheduler);

                    o1s.OnNext(0);
                    Assert.IsFalse(res.Completed);
                    Assert.IsFalse(res.Error);

                    using (var stateWriter = new InMemoryStateWriter(store, CheckpointKind.Full))
                    {
                        engine.CheckpointAsync(stateWriter).Wait();
                    }

                    return store;
                });

            RunInDifferentDomain(
                (CheckpointingTests test, InMemoryStateStore store) =>
                {
                    var sourceUri = new Uri("reactor:/source");

                    var engine = test.CreateQueryEngine();
                    var reactive = GetQueryEngineReactiveService(engine);

                    using (var stateReader = new InMemoryStateReader(store))
                    {
                        engine.RecoverAsync(stateReader).Wait();
                    }

                    var o1 = reactive.GetObserver<int>(sourceUri);

                    var o1s = o1.Synchronize(engine.Scheduler);

                    var res = MockObserver.Get<int>("result");
                    Assert.IsNotNull(res);

                    o1s.OnNext(1);
                    Assert.IsTrue(res.Completed);
                    Assert.IsFalse(res.Error);

                    o1s.OnNext(2);

                    // Resulting observer does not preserve the state, but take operator does.
                    // Only the last one should arrive.
                    AssertResult(res, 1, (i, v) => Assert.AreEqual(i + 1, v));
                },
                checkpoint);
        }
#endif

        [TestMethod]
        public void DifferentialSavesNewEntities()
        {
            var checkpoint = new InMemoryStateStore("checkpoint1");
            var sourceUri = new Uri("reactor:/source");
            var subscriptionUri = new Uri("reactor:/subscription");
            var observerUri = new Uri("reactor:/observer");
            var observableUri = new Uri("reactor:/observable");
            var anotherSubscriptionUri = new Uri("reactor:/anothersubscription");

            {
                var engine = CreateQueryEngine();
                var reactive = GetQueryEngineReactiveService(engine);

                // Take checkpoint with stream.
                Checkpoint(engine, checkpoint);

                // Create entities.
                var source = reactive.CreateStream<int>(sourceUri);
                var observable = reactive.GetObservable<int>(sourceUri);
                reactive.DefineObserver<object, int>(observerUri, (v) => Observer.Create<int>((value) => Assert.AreEqual(value, 1)).AsQbserver(), null);
                reactive.DefineObservable<int, int>(observableUri, (value) => Observable.Return(value).AsQbservable(), null);
                observable.Subscribe(reactive.GetObserver<string, int>(MockObserverUri)("result"), subscriptionUri, null);

                var srcs = source.Synchronize(engine.Scheduler);
                srcs.OnNext(0);
                AssertResult(GetObserver<int>("result"), 1, Assert.AreEqual);

                Checkpoint(engine, checkpoint, CheckpointKind.Differential);

                RemoveQueryEngine(engine);
            }

            // Check that entities exist in the checkpoint.
            {
                var engine = CreateQueryEngine();
                var reactive = GetQueryEngineReactiveService(engine);

                Recover(engine, checkpoint);

                var source = reactive.GetStream<int, int>(sourceUri);
                var srcs = source.Synchronize(engine.Scheduler);
                srcs.OnNext(1);
                AssertResult(GetObserver<int>("result"), 2, Assert.AreEqual);

                reactive.GetObservable<int, int>(observableUri)(1)
                    .Subscribe(
                        reactive.GetObserver<object, int>(observerUri)(null),
                        anotherSubscriptionUri, null);

                var subscriptionExists = false;
                try
                {
                    reactive.GetObservable<int, int>(observableUri)(1)
                    .Subscribe(
                        reactive.GetObserver<object, int>(observerUri)(null),
                        subscriptionUri, null);
                }
                catch (ArgumentException)
                {
                    subscriptionExists = true;
                }
                Assert.IsTrue(subscriptionExists);

                RemoveQueryEngine(engine);
            }
        }

        [TestMethod]
        public void DifferentialDoesNotSaveRecoveredEntities()
        {
            var subCount = 100;
            var subIdBase = "test://sub/";

            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            for (var i = 0; i < subCount; ++i)
            {
                _ = ctx.Never<int>().Subscribe(v1, new Uri(subIdBase + i), null);
            }

            var store = Checkpoint(qe);

            RemoveQueryEngine(qe);

            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);

            Recover(qe, store);

            var store2 = Checkpoint(qe, CheckpointKind.Differential);

            var recheckpointed = 0;
            Assert.IsTrue(store.TryGetItemKeys("Subscriptions", out var keys));
            foreach (var key in keys)
            {
                Assert.IsTrue(store.TryGetItem("Subscriptions", key, out var first));
                if (store2.TryGetItem("Subscriptions", key, out var second))
                {
                    recheckpointed++;
                }
            }

            Assert.AreEqual(0, recheckpointed);
        }

        [TestMethod]
        public void CheckpointWhenNoEventWasSent()
        {
            var checkpoint = new InMemoryStateStore("someId");
            var sourceUri = new Uri("reactor:/source");
            var sinkUri = new Uri("reactor:/sink");
            {
                var engine = CreateQueryEngine();
                var reactive = GetQueryEngineReactiveService(engine);

                reactive.CreateStream<int>(sourceUri);
                reactive.CreateStream<int>(sinkUri);

                var source = reactive.GetObservable<int>(sourceUri);
                Assert.IsNotNull(source);

                var sink = reactive.GetObserver<int>(sinkUri);
                Assert.IsNotNull(sink);

                var sinkObservable = reactive.GetObservable<int>(sinkUri);
                sinkObservable.Subscribe(reactive.GetObserver<string, int>(MockObserverUri)("result"), new Uri("reactor:/tmp"), null);
                var sub = source.Subscribe(sink, new Uri("reactor:/sub"), null);
                Assert.IsNotNull(sub);

                Checkpoint(engine, checkpoint);

                sub.Dispose();
                RemoveQueryEngine(engine);
            }

            {
                var engine = CreateQueryEngine();
                var reactive = GetQueryEngineReactiveService(engine);

                Recover(engine, checkpoint);

                var src = reactive.GetObserver<int>(sourceUri);

                var srcs = src.Synchronize(engine.Scheduler);

                var res = MockObserver.Get<int>("result");
                Assert.IsNotNull(res);

                srcs.OnNext(0);
                AssertResult(GetObserver<int>("result"), 1, Assert.AreEqual);
                RemoveQueryEngine(engine);
            }
        }

        [TestMethod]
        public void DisposeSubscriptionDuringCommitAsync()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            var str1 = new Uri("str:/s1");

            var s1 = ctx.CreateStream<int>(str1);
            Assert.IsNotNull(s1);

            var s1o = ctx.GetObservable<int>(str1);
            Assert.IsNotNull(s1o);

            AssertObserverNotCreated<int>("v1");

            var sub = s1o.Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null);

            var v1 = GetMockObserver<int>("v1");
            Assert.AreEqual(new Uri("s:/sub1"), v1.InstanceId);

            var s1v = ctx.GetObserver<int>(str1);
            Assert.IsNotNull(s1v);

            var s1vs = s1v.Synchronize(qe.Scheduler);

            for (var i = 0; i < 5; i++)
            {
                s1vs.OnNext(i);
            }

            var store = new InMemoryStateStore("someId");

            using (var stateWriter = new MyStateWriter(store, CheckpointKind.Full))
            {
                Task.Run(() =>
                {
                    stateWriter.CommitStarted.Wait();
                    sub.Dispose();
                    stateWriter.ContinueCommit();
                });

                qe.CheckpointAsync(stateWriter).Wait();
            }

            s1.Dispose();
            RemoveQueryEngine(qe);

            AssertResult(v1, 5, (i, v) => Assert.AreEqual(i, v));

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);
            Recover(qe2, store);
            s1v = ctx2.GetObserver<int>(str1);
            s1vs = s1v.Synchronize(qe2.Scheduler);

            for (var i = 5; i < 10; i++)
            {
                s1vs.OnNext(i);
            }

            AssertResult(v1, 10, (i, v) => Assert.AreEqual(i, v));
        }

        [TestMethod]
        public void CheckpointRecoveryFailureTransientBehavior()
        {
            var store = new InMemoryStateStore("someId");

            var id = Guid.NewGuid().ToString();

            {
                var qe1 = CreateQueryEngine();
                var ctx = GetQueryEngineReactiveService(qe1);

                var str1 = new Uri("ob:/o1");
                ctx.DefineObservable<string, int>(str1, s => new MySubscribable(s).AsQbservable(), null);

                var o1 = ctx.GetObservable<string, int>(str1)(id);

                var sub = o1.Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null);
                Assert.AreEqual(1, MySubscribable._start[id]);
                Assert.AreEqual(43, MySubscribable._state[id]);

                Checkpoint(qe1, store);

                sub.Dispose();

                RemoveQueryEngine(qe1);
            }

            {
                var failed = false;

                var qe2 = CreateQueryEngine();
                qe2.EntityLoadFailed += (o, e) =>
                {
                    e.Handled = true;
                    e.Mitigation = ReactiveEntityRecoveryFailureMitigation.Ignore;
                    failed = true;
                };

                MySubscribable._fail[id] = true;

                Recover(qe2, store);

                Assert.IsTrue(failed);
                Assert.AreEqual(2, MySubscribable._start[id]);
                Assert.AreEqual(44, MySubscribable._state[id]); // state was recovered prior to failure

                Checkpoint(qe2, store, CheckpointKind.Differential);

                RemoveQueryEngine(qe2);
            }

            {
                var failed = false;

                var qe3 = CreateQueryEngine();
                qe3.EntityLoadFailed += (o, e) => { e.Handled = true; failed = true; };

                MySubscribable._fail[id] = false;

                Recover(qe3, store);

                Assert.IsFalse(failed);
                Assert.AreEqual(3, MySubscribable._start[id]);
                Assert.AreEqual(44, MySubscribable._state[id]); // state did not get reset during previous checkpoint + was recovered from existing state

                RemoveQueryEngine(qe3);
            }
        }

        [TestMethod]
        public void CheckpointStartWithTake()
        {
            var qe1 = CreateQueryEngine();

            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var generator = ctx1.GetObservable<string, int>(MockObservableUri);
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var s = LockManager.NewAutoReset();
            var d = LockManager.NewAutoReset();

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .AwaitSubscribe(s)
                .AwaitDispose(d)
                .Take(int.MaxValue) // something stateful in the source of StartWith
                .StartWith(42)
                .Take(1)
                .Subscribe(v1, sub1uri, null);

            Assert.IsTrue(LockManager.Wait(s));
            Assert.IsTrue(LockManager.Wait(d));

            var out1 = GetMockObserver<int>("v1", validate: false);

            AssertResultSequence(out1, new int[] { 42 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe1);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);
        }

        [TestMethod]
        public void CheckpointRecovery_DoubleStart()
        {
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");
            ctx1.Never<int>().Subscribe(v1, new Uri("sub:/test/1"), null);

            var chkpt1 = Checkpoint(qe1);

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);
            var l = LockManager.NewManualReset();

            var v2 = ctx1.GetObserver<string, int>(MockObserverUri)("v2");

            qe2.SchedulerPaused += (sender, args) =>
            {
                ctx2.Never<int>().StartWith().Subscribe(v2, new Uri("sub:/test/2"), null);
            };

            Recover(qe2, chkpt1);

            var mv1 = GetMockObserver<int>("v1");

            var reset = new ManualResetEvent(false);
            var ex = default(Exception);

            qe2.Scheduler.UnhandledException += (sender, args) =>
            {
                args.Handled = true;
                ex = args.Exception;
                reset.Set();
            };

            if (reset.WaitOne(1000))
            {
                Assert.IsNull(ex, ex?.ToString());
                Assert.Fail("Unexpected exception.");
            }
        }

        [TestMethod]
        public void CheckpointRecovery_DoubleStart2()
        {
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");
            ctx1.Never<int>().Subscribe(v1, new Uri("sub:/test/1"), null);

            var chkpt1 = Checkpoint(qe1);

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);
            var l = LockManager.NewManualReset();

            var v2 = ctx1.GetObserver<string, int>(MockObserverUri)("v2");

            qe2.SchedulerPaused += (sender, args) =>
            {
                ctx2.Return(4).StartWith(1, 2, 3).AwaitDo(null, null, l).Subscribe(v2, new Uri("sub:/test/2"), null);
            };

            Recover(qe2, chkpt1);

            var mv2 = GetMockObserver<int>("v2", validate: false);

            var reset = new ManualResetEvent(false);
            var ex = default(Exception);

            qe2.Scheduler.UnhandledException += (sender, args) =>
            {
                args.Handled = true;
                ex = args.Exception;
                reset.Set();
            };

            if (reset.WaitOne(1000))
            {
                Assert.IsNull(ex, ex?.ToString());
                Assert.Fail("Unexpected exception.");
            }

            Assert.IsTrue(LockManager.Wait(l, 1000));
            Assert.IsTrue(mv2.Values.SequenceEqual(new[] { 1, 2, 3, 4 }));
        }

        private sealed class MySubscribable : SubscribableBase<int>
        {
            public static readonly Dictionary<string, bool> _fail = new();
            public static readonly Dictionary<string, int> _start = new();
            public static readonly Dictionary<string, int> _state = new();
            private readonly string _id;

            public MySubscribable(string s)
            {
                _id = s;

                if (!_fail.ContainsKey(s))
                {
                    _fail[s] = false;
                    _start[s] = 0;
                    _state[s] = 0;
                }
            }

            protected override ISubscription SubscribeCore(IObserver<int> observer) => new _(this, observer);

            private sealed class _ : StatefulUnaryOperator<MySubscribable, int>
            {
                private int _state;

                public _(MySubscribable p, IObserver<int> o)
                    : base(p, o)
                {
                    _state = 42;
                }

                protected override void OnStart()
                {
                    base.OnStart();

                    MySubscribable._state[Params._id] = ++_state;
                    MySubscribable._start[Params._id]++;

                    if (MySubscribable._fail[Params._id])
                    {
                        throw new Exception("Oops!");
                    }
                }

                protected override void SaveStateCore(IOperatorStateWriter writer)
                {
                    base.SaveStateCore(writer);

                    writer.Write<int>(_state);
                }

                protected override void LoadStateCore(IOperatorStateReader reader)
                {
                    base.LoadStateCore(reader);

                    _state = reader.Read<int>();
                }

                public override string Name => "xyz";

                public override Version Version => new(1, 0, 0, 0);
            }
        }

        public class MyStateWriter : InMemoryStateWriter
        {
            private readonly TaskCompletionSource<bool> _start;
            private readonly TaskCompletionSource<bool> _continue;
            private readonly TaskCompletionSource<bool> _completed;

            public MyStateWriter(InMemoryStateStore stateStore, CheckpointKind checkpointKind)
                : base(stateStore, checkpointKind)
            {
                _start = new TaskCompletionSource<bool>();
                _continue = new TaskCompletionSource<bool>();
                _completed = new TaskCompletionSource<bool>();
            }

            public Task CommitStarted => _start.Task;

            public Task CommitCompleted => _completed.Task;

            public void ContinueCommit()
            {
                _continue.SetResult(true);
            }

            public override async Task CommitAsync(CancellationToken token, IProgress<int> progress)
            {
                _start.SetResult(true);
                await _continue.Task;
                await base.CommitAsync(token, progress);
                _completed.SetResult(true);
            }
        }

        #region DelaySubscription

        [TestMethod]
        public void DelaySubscription_SubscribedCorrectly()
        {
            // DelaySubscription with timer of 0 seconds should cause timer to
            // immediately fire, which should cause us to subscribe to o1. This
            // does not test the OnNext behavior of the underlying operator.
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            _ = o1.DelaySubscription(TimeSpan.FromSeconds(0)).Subscribe(v1, new Uri("s:/s1"), null);
        }

        #endregion

        #region Switch

        [TestMethod]
        public void CheckpointSwitch_WorksBeforeAndAfter()
        {
            // Build stream of observables. Verify that switch's subscription
            // is "switched" to always be a subscription to the most current
            // observable -- meaning that if a previous observable fires, we
            // ignore it. Finally, bring down the QE and bring it back up and
            // verify (1) that previous observables are lost, and (2) the
            // current subscription is restored.
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var generator = ctx1.GetObservable<string, int>(MockObservableUri);
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .Select(x => generator("o" + x))
                .Switch()
                .Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");
            var in1 = GetObservable<int>("o1");
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(2);  // generates observable o2

            var in2 = GetObservable<int>("o2");
            var in2s = in2.Synchronize(qe1.Scheduler);

            in2s.OnNext(97);

            in1s.OnNext(3);  // generates observable o3
            var in3 = GetObservable<int>("o3");
            var in3s = in3.Synchronize(qe1.Scheduler);

            in2s.OnNext(98); // ignored
            in3s.OnNext(99);

            AssertResultSequence(out1, new int[] { 97, 99 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            in1s = in1.Synchronize(qe2.Scheduler);
            Assert.AreEqual(1, in1.SubscriptionCount);

            in2 = MockObservable.Get<int>("o2");
            Assert.IsNull(in2);

            in3 = MockObservable.Get<int>("o3");
            in3s = in3.Synchronize(qe2.Scheduler);
            Assert.AreEqual(1, in3.SubscriptionCount);

            in1s.OnNext(4);
            var in4 = GetObservable<int>("o4");
            var in4s = in4.Synchronize(qe2.Scheduler);
            in3s.OnNext(100);
            in4s.OnNext(101);

            AssertResultSequence(out1, new int[] { 101 });

            sub1.Dispose();
        }

        [TestMethod]
        public void CheckpointSwitch_WorksBeforeAndAfterHighOrder()
        {
            // Build stream of observables. Verify that switch's subscription
            // is "switched" to always be a subscription to the most current
            // observable -- meaning that if a previous observable fires, we
            // ignore it. Finally, bring down the QE and bring it back up and
            // verify (1) that previous observables are lost, and (2) the
            // current subscription is restored.
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var generator = ctx1.GetObservable<string, int>(MockObservableUri);
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .Select(x => generator("o" + x))
                .Switch()
                .Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");
            var in1 = GetObservable<int>("o1");
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(2);  // generates observable o2

            var in2 = GetObservable<int>("o2");
            var in2s = in2.Synchronize(qe1.Scheduler);

            in2s.OnNext(97);

            in1s.OnNext(3);  // generates observable o3
            var in3 = GetObservable<int>("o3");
            var in3s = in3.Synchronize(qe1.Scheduler);

            in2s.OnNext(98); // ignored
            in3s.OnNext(99);

            AssertResultSequence(out1, new int[] { 97, 99 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            in1s = in1.Synchronize(qe2.Scheduler);
            Assert.AreEqual(1, in1.SubscriptionCount);

            in2 = MockObservable.Get<int>("o2");
            Assert.IsNull(in2);

            in3 = MockObservable.Get<int>("o3");
            in3s = in3.Synchronize(qe2.Scheduler);
            Assert.AreEqual(1, in3.SubscriptionCount);

            in1s.OnNext(4);
            var in4 = GetObservable<int>("o4");
            var in4s = in4.Synchronize(qe2.Scheduler);
            in3s.OnNext(100);
            in4s.OnNext(101);

            AssertResultSequence(out1, new int[] { 101 });

            sub1.Dispose();
        }

        [TestMethod]
        public void CheckpointSwitch_ConcurrencyOnHigherOrderDirtyFlag()
        {
            // Build stream of observables. Verify that switch's subscription
            // is "switched" to always be a subscription to the most current
            // observable -- meaning that if a previous observable fires, we
            // ignore it. Finally, bring down the QE and bring it back up and
            // verify (1) that previous observables are lost, and (2) the
            // current subscription is restored.
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var generator = ctx1.GetObservable<string, int>(MockObservableUri);
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .Select(x => generator("o" + x))
                .Switch()
                .Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");
            var in1 = GetObservable<int>("o1");
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(2);  // generates observable o2

            var in2 = GetObservable<int>("o2");
            var in2s = in2.Synchronize(qe1.Scheduler);

            in2s.OnNext(97);

            in1s.OnNext(3);  // generates observable o3
            var in3 = GetObservable<int>("o3");
            var in3s = in3.Synchronize(qe1.Scheduler);

            in2s.OnNext(98); // ignored
            in3s.OnNext(99);

            AssertResultSequence(out1, new int[] { 97, 99 });

            // Checkpoint -------------------------------------------------------------

            var store = new InMemoryStateStore(Guid.NewGuid().ToString("D"));

            Checkpoint(qe1, store, onCommit: () =>
            {
                in1s.OnNext(4);  // generates observable o4
            });

            Checkpoint(qe1, store, CheckpointKind.Differential);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, store);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            in1s = in1.Synchronize(qe2.Scheduler);
            Assert.AreEqual(1, in1.SubscriptionCount);

            in2 = MockObservable.Get<int>("o2");
            Assert.IsNull(in2);

            var in4 = MockObservable.Get<int>("o4");
            var in4s = in4.Synchronize(qe2.Scheduler);
            Assert.AreEqual(1, in4.SubscriptionCount);

            in1s.OnNext(5);
            var in5 = GetObservable<int>("o5");
            var in5s = in5.Synchronize(qe2.Scheduler);
            in4s.OnNext(100);
            in5s.OnNext(101);

            AssertResultSequence(out1, new int[] { 101 });

            sub1.Dispose();
        }

        [TestMethod]
        public void CheckpointSwitch_ProperRegistryCleanUp()
        {
            // Build stream of observables. Verify that switch's subscription
            // is "switched" to always be a subscription to the most current
            // observable -- meaning that if a previous observable fires, we
            // ignore it. Finally, bring down the QE and bring it back up and
            // verify (1) that previous observables are lost, and (2) the
            // current subscription is restored.
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var generator = ctx1.GetObservable<string, int>(MockObservableUri);
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .Select(x => generator("o" + x))
                .Switch()
                .Subscribe(v1, sub1uri, null);

            var in1 = GetObservable<int>("o1");
            var in1s = in1.Synchronize(qe1.Scheduler);

            // Checkpoint 1 -----------------------------------------------------------

            var store = new InMemoryStateStore(Guid.NewGuid().ToString("D"));
            Checkpoint(qe1, store);

            // Continue ---------------------------------------------------------------

            in1s.OnNext(2);  // generates observable o2
            in1s.OnNext(3);  // generates observable o3

            // Checkpoint 2 -----------------------------------------------------------

            Checkpoint(qe1, store, CheckpointKind.Differential);

            // Continue ---------------------------------------------------------------

            in1s.OnNext(4);  // generates observable o4

            // Checkpoint 3 -----------------------------------------------------------

            Checkpoint(qe1, store, CheckpointKind.Differential);

            // Continue ---------------------------------------------------------------

            in1s.OnNext(5);  // generates observable o5

            // Checkpoint 4 -----------------------------------------------------------

            Checkpoint(qe1, store, CheckpointKind.Differential);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, store);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            // Checkpoint 5 -----------------------------------------------------------

            var final = new InMemoryStateStore(Guid.NewGuid().ToString("D"));
            Checkpoint(qe2, final);

            // Assert -----------------------------------------------------------------

            Assert.IsTrue(final.TryGetItemKeys("Observers", out var obv));
            Assert.AreEqual(0, obv.Count(x => !x.StartsWith("mgmt"))); // none

            Assert.IsTrue(final.TryGetItemKeys("Subjects", out var str));
            Assert.AreEqual(1, str.Count(x => !x.StartsWith("mgmt"))); // bridge

            Assert.IsTrue(final.TryGetItemKeys("Subscriptions", out var sub));
            Assert.AreEqual(2, sub.Count(x => !x.StartsWith("mgmt"))); // upstream + test

            sub1.Dispose();
        }

        #endregion

        #region Throttle

        [TestMethod]
        public void CheckpointThrottle_WorksBeforeAndAfterRestore1()
        {
            // Fire both source and throttling observables before and after
            // checkpointing to verify that operator works both before and after.
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx1.GetObservable<string, int>(MockObservableUri)("o2");
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1.Throttle((_) => o2).Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");
            var in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(1);  // ignored

            var in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            var in2s = in2.Synchronize(qe1.Scheduler);

            in1s.OnNext(2);
            in2s.OnNext(99);

            AssertResultSequence(out1, new int[] { 2 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            in1s = in1.Synchronize(qe2.Scheduler);

            in1s.OnNext(3);

            in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            in2s = in2.Synchronize(qe2.Scheduler);

            in2s.OnNext(99);

            AssertResultSequence(out1, new int[] { 3 });

            sub1.Dispose();
        }

        [TestMethod]
        public void CheckpointThrottle_WorksBeforeAndAfterRestore2()
        {
            // Fire both source and throttling observables before and after
            // checkpointing to verify that operator works both before and after.
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx1.GetObservable<string, int>(MockObservableUri)("o2");
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1.Throttle((_) => o2).Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");
            var in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(1);  // ignored

            var in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            var in2s = in2.Synchronize(qe1.Scheduler);

            in1s.OnNext(2);

            AssertResultSequence(out1, Array.Empty<int>());

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            in1s = in1.Synchronize(qe2.Scheduler);

            in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            in2s = in2.Synchronize(qe2.Scheduler);

            in2s.OnNext(99);

            AssertResultSequence(out1, new int[] { 2 });

            sub1.Dispose();
        }

        [TestMethod]
        public void CheckpointThrottle_IgnoreAfterRestore()
        {
            // Clear state ------------------------------------------------------------

            Crash();

            // Initialize -------------------------------------------------------------

            // Observe element from source observable, fail QE, then observe
            // an element from the source observable, and (afterwards) from
            // the throttling observable. Verify that the element observed
            // before QE failure is lost.
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx1.GetObservable<string, int>(MockObservableUri)("o2");
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1.Throttle((_) => o2).Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");
            var in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(1);  // ignored

            var in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            var in2s = in2.Synchronize(qe1.Scheduler);

            in1s.OnNext(2);
            in2s.OnNext(99);
            in1s.OnNext(3);

            AssertResultSequence(out1, new int[] { 2 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            in1s = in1.Synchronize(qe2.Scheduler);

            in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            in2s = in2.Synchronize(qe2.Scheduler);

            in1s.OnNext(4);
            in2s.OnNext(99);

            AssertResultSequence(out1, new int[] { 4 });

            sub1.Dispose();
        }

        [TestMethod]
        public void CheckpointThrottle_PropagateElementFromBeforeFailure()
        {
            // Observe element from source observable, fail QE, then observe
            // an element from the throttling observable. Verify that the
            // element observed before QE failure is propagated.
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx1.GetObservable<string, int>(MockObservableUri)("o2");
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1.Throttle((_) => o2).Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");
            var in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(1);  // ignored

            var in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);

            in1s.OnNext(2);

            AssertResultSequence(out1, Array.Empty<int>());

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);

            in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            var in2s = in2.Synchronize(qe2.Scheduler);

            in2s.OnNext(99);

            AssertResultSequence(out1, new int[] { 2 });

            sub1.Dispose();
        }

        #endregion

        #region SelectMany

        [TestMethod]
        public void CheckpointSelectMany_WorksBeforeAndAfter()
        {
            var qe1 = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe1);

            var o1 = ctx.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx.GetObservable<string, int>(MockObservableUri)("o2");

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1.SelectMany(x => o2.Where(y => x < y), (x, y) => x + y).Subscribe(v1, new Uri("s:/s1"), null);

            var s3 = GetMockObserver<int>("v1");

            var s1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(s1);
            Assert.AreEqual(1, s1.SubscriptionCount);
            Assert.AreEqual(sub1uri, s1.Subscriptions.Single().Uri);
            var s1s = s1.Synchronize(qe1.Scheduler);

            s1s.OnNext(3);

            var s2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(s2);
            Assert.AreEqual(1, s2.SubscriptionCount);
            Assert.AreEqual(sub1uri, s2.Subscriptions.Single().Uri);
            var s2s = s2.Synchronize(qe1.Scheduler);

            s2s.OnNext(4);

            var res = s3.Values;
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(7, res[0]);

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            var out1 = GetMockObserver<int>("v1");

            var in1 = MockObservable.Get<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            Assert.AreEqual(sub1uri, in1.Subscriptions.Single().Uri);
            var in1s = in1.Synchronize(qe2.Scheduler);

            var in2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(in2);
            Assert.AreEqual(sub1uri, in2.Subscriptions.Single().Uri);
            var in2s = in2.Synchronize(qe2.Scheduler);

            in1s.OnNext(4);
            in2s.OnNext(5);

            res = out1.Values;
            AssertResultSequence(out1, new int[] { 8, 9 });

            sub1.Dispose();
        }

        [TestMethod]
        public void CheckpointSelectMany_WorksBeforeAndAfterHighOrder()
        {
            var qe1 = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe1);

            var o1 = ctx.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx.GetObservable<string, int>(MockObservableUri)("o2");
            var generator = ctx.GetObservable<string, int>(MockObservableUri);

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1
                .Select(x => generator("o" + x))
                .SelectMany(obs => obs, (obs, v) => v)
                .Subscribe(v1, new Uri("s:/s1"), null);

            var out1 = GetMockObserver<int>("v1");

            var s1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(s1);
            Assert.AreEqual(1, s1.SubscriptionCount);
            Assert.AreEqual(sub1uri, s1.Subscriptions.Single().Uri);
            var s1s = s1.Synchronize(qe1.Scheduler);

            s1s.OnNext(2);

            var s2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(s2);
            Assert.AreEqual(1, s2.SubscriptionCount);
            Assert.AreEqual(sub1uri, s2.Subscriptions.Single().Uri);
            var s2s = s2.Synchronize(qe1.Scheduler);

            s2s.OnNext(95);

            AssertResultSequence(out1, new int[] { 95 });

            s1s.OnNext(3);

            var s3 = MockObservable.Get<int>("o3");
            Assert.IsNotNull(s3);
            Assert.AreEqual(1, s3.SubscriptionCount);
            Assert.AreEqual(sub1uri, s3.Subscriptions.Single().Uri);
            var s3s = s3.Synchronize(qe1.Scheduler);

            s2s.OnNext(96);
            s3s.OnNext(97);

            AssertResultSequence(out1, new int[] { 95, 96, 97 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            sub1 = ctx2.GetSubscription(sub1uri);
            Assert.IsNotNull(sub1);

            out1 = GetMockObserver<int>("v1");

            var in1 = MockObservable.Get<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            Assert.AreEqual(sub1uri, in1.Subscriptions.Single().Uri);
            var in1s = in1.Synchronize(qe2.Scheduler);

            var in2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(in2);
            Assert.AreEqual(sub1uri, in2.Subscriptions.Single().Uri);
            var in2s = in2.Synchronize(qe2.Scheduler);

            var in3 = MockObservable.Get<int>("o3");
            Assert.IsNotNull(in3);
            Assert.AreEqual(sub1uri, in3.Subscriptions.Single().Uri);
            var in3s = in3.Synchronize(qe2.Scheduler);

            in1s.OnNext(4);

            var in4 = MockObservable.Get<int>("o4");
            Assert.IsNotNull(in4);
            Assert.AreEqual(sub1uri, in4.Subscriptions.Single().Uri);
            var in4s = in4.Synchronize(qe2.Scheduler);

            in2s.OnNext(98);
            in3s.OnNext(99);
            in4s.OnNext(100);

            AssertResultSequence(out1, new int[] { 98, 99, 100 });

            sub1.Dispose();
        }

        [TestMethod]
        public void CheckpointSelectMany_DifferentialCheckpoint_WorksBeforeAndAfter()
        {
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx1.GetObservable<string, int>(MockObservableUri)("o2");

            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1.SelectMany(x => o2.Where(y => x < y), (x, y) => x + y).Subscribe(v1, sub1uri, null);

            var out1 = GetMockObserver<int>("v1");

            var in1 = GetObservable<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            var in1s = in1.Synchronize(qe1.Scheduler);

            in1s.OnNext(3);

            var in2 = GetObservable<int>("o2");
            Assert.AreEqual(1, in2.SubscriptionCount);
            var in2s = in2.Synchronize(qe1.Scheduler);

            in2s.OnNext(4);
            in2s.OnNext(10);

            AssertResultSequence(out1, new int[] { 7, 13 });

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Complete Inner Subscription --------------------------------------------

            in2s.OnCompleted();

            // Differential Checkpoint ------------------------------------------------

            Checkpoint(qe1, chkpt1, CheckpointKind.Differential);

            // Clear state ------------------------------------------------------------

            sub1.Dispose();
            RemoveQueryEngine(qe1);
            MockObservable.Clear();
            MockObserver.Clear();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, chkpt1);

            // Not throwing an exception is sufficient to pass.
        }

        #endregion

        #region Subjects

        [TestMethod]
        public void CheckpointReliableSubject_AsObservable()
        {
            var mgr1 = new SubjectManager();
            DefineSubjectFactories(mgr1);
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            // Create the stream against the metadata context (so it will be lazily initialized)
            var sf = Context.GetStreamFactory<string, int, int>(new Uri("cpt://subject/reliable/create"));
            sf.Create(new Uri("test://subject"), "s1", null);

            // Get a mock observer
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            // Use the stream in a higher-order subscription
            ctx1.Return(ctx1.GetObservable<int>(new Uri("test://subject")))
                .SelectMany(x => x, (c, x) => x)
                .Subscribe(v1, new Uri("test://subscription"), null);

            // Wait for inner subscription to be set up
            mgr1.AwaitSubscribe("s1");

            // Assert some output
            var mv1 = GetMockObserver<int>("v1");
            var s1 = mgr1.GetReliableObserver<int>("s1");
            var s1s = s1.Synchronize(qe1.Scheduler);
            s1s.OnNext(42, 1L);
            Assert.AreEqual(1, mv1.Values.Count);
            Assert.AreEqual(42, mv1.Values[0]);

            // Checkpoint -------------------------------------------------------------

            var store = new InMemoryStateStore(Guid.NewGuid().ToString("D"));
            Checkpoint(qe1, store);

            // Tear Down --------------------------------------------------------------

            Crash();
            UndefineSubjectFactories();

            // Recover ----------------------------------------------------------------

            var mgr2 = new SubjectManager();
            DefineSubjectFactories(mgr2);
            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            // Should not need to await the subscription to the subject,
            // as it is already in an inner position and will be fully
            // setup upon recovery.
            Recover(qe2, store);

            // Assert more output
            var mv2 = GetMockObserver<int>("v1");
            var s2 = mgr2.GetReliableObserver<int>("s1");
            var s2s = s2.Synchronize(qe2.Scheduler);
            s2s.OnNext(43, 2L);

            // Note that the `Crash()` caused the MockObserver to be cleared
            Assert.AreEqual(1, mv2.Values.Count);
            Assert.AreEqual(43, mv2.Values[0]);

            Crash();
            UndefineSubjectFactories();
        }

        [TestMethod]
        public void CheckpointReliableSubject_AsObserver()
        {
            var mgr1 = new SubjectManager();
            DefineSubjectFactories(mgr1);
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            // Create the stream against the metadata context (so it will be lazily initialized)
            var sf = Context.GetStreamFactory<string, int, int>(new Uri("cpt://subject/reliable/create"));
            sf.Create(new Uri("test://subject/1"), "s1", null);
            sf.Create(new Uri("test://subject/2"), "s2", null);

            // Get some mock observers
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");
            var v2 = ctx1.GetObserver<string, int>(MockObserverUri)("v2");

            // Set up a simple subscription between the observer stream and some mock
            ctx1.GetObservable<int>(new Uri("test://subject/2")).Subscribe(v2, new Uri("test://subscription/2"), null);

            // Use the stream in a higher-order subscription
            ctx1.Return(ctx1.GetObservable<int>(new Uri("test://subject/1")))
                .SelectMany(x => x.Do(ctx1.GetObserver<int>(new Uri("test://subject/2"))), (c, x) => x)
                .Select(y => y + 2)
                .Subscribe(v1, new Uri("test://subscription/1"), null);

            // Wait for inner subscription to be set up
            mgr1.AwaitSubscribe("s1");

            // Assert some output
            var mv1 = GetMockObserver<int>("v1");
            var mv2 = GetMockObserver<int>("v2");
            var s1 = mgr1.GetReliableObserver<int>("s1");
            var s1s = s1.Synchronize(qe1.Scheduler);
            s1s.OnNext(42, 1L);
            Assert.AreEqual(1, mv1.Values.Count);
            Assert.AreEqual(44, mv1.Values[0]);
            Assert.AreEqual(1, mv2.Values.Count);
            Assert.AreEqual(42, mv2.Values[0]);

            // Checkpoint -------------------------------------------------------------

            var store = new InMemoryStateStore(Guid.NewGuid().ToString("D"));
            Checkpoint(qe1, store);

            // Tear Down --------------------------------------------------------------

            Crash();
            UndefineSubjectFactories();

            // Recover ----------------------------------------------------------------

            var mgr2 = new SubjectManager();
            DefineSubjectFactories(mgr2);
            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            // Should not need to await the subscription to the subject,
            // as it is already in an inner position and will be fully
            // setup upon recovery.
            Recover(qe2, store);

            // Assert more output
            var mv3 = GetMockObserver<int>("v1");
            var mv4 = GetMockObserver<int>("v2");
            var s2 = mgr2.GetReliableObserver<int>("s1");
            var s2s = s2.Synchronize(qe2.Scheduler);
            s2s.OnNext(43, 2L);

            // Note that the `Crash()` caused the MockObserver to be cleared
            Assert.AreEqual(1, mv3.Values.Count);
            Assert.AreEqual(45, mv3.Values[0]);
            Assert.AreEqual(1, mv4.Values.Count);
            Assert.AreEqual(43, mv4.Values[0]);

            Crash();
            UndefineSubjectFactories();
        }

        [TestMethod]
        public void CheckpointMultiSubject_AsObservable()
        {
            var mgr1 = new SubjectManager();
            DefineSubjectFactories(mgr1);
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            // Create the stream against the metadata context (so it will be lazily initialized)
            var sf = Context.GetStreamFactory<string, int, int>(new Uri("cpt://subject/untyped/create"));
            sf.Create(new Uri("test://subject"), "s1", null);

            // Get a mock observer
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            // Use the stream in a higher-order subscription
            ctx1.Return(ctx1.GetObservable<int>(new Uri("test://subject")))
                .SelectMany(x => x, (c, x) => x)
                .Subscribe(v1, new Uri("test://subscription"), null);

            // Wait for inner subscription to be set up
            mgr1.AwaitSubscribe("s1");

            // Assert some output
            var mv1 = GetMockObserver<int>("v1");
            var s1 = mgr1.GetObserver<int>("s1");
            var s1s = s1.Synchronize(qe1.Scheduler);
            s1s.OnNext(42);
            Assert.AreEqual(1, mv1.Values.Count);
            Assert.AreEqual(42, mv1.Values[0]);

            // Checkpoint -------------------------------------------------------------

            var store = new InMemoryStateStore(Guid.NewGuid().ToString("D"));
            Checkpoint(qe1, store);

            // Tear Down --------------------------------------------------------------

            Crash();
            UndefineSubjectFactories();

            // Recover ----------------------------------------------------------------

            var mgr2 = new SubjectManager();
            DefineSubjectFactories(mgr2);
            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            // Should not need to await the subscription to the subject,
            // as it is already in an inner position and will be fully
            // setup upon recovery.
            Recover(qe2, store);

            // Assert more output
            var mv2 = GetMockObserver<int>("v1");
            var s2 = mgr2.GetObserver<int>("s1");
            var s2s = s2.Synchronize(qe2.Scheduler);
            s2s.OnNext(43);

            // Note that the `Crash()` caused the MockObserver to be cleared
            Assert.AreEqual(1, mv2.Values.Count);
            Assert.AreEqual(43, mv2.Values[0]);

            Crash();
            UndefineSubjectFactories();
        }

        [TestMethod]
        public void CheckpointMultiSubject_AsObserver()
        {
            var mgr1 = new SubjectManager();
            DefineSubjectFactories(mgr1);
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            // Create the stream against the metadata context (so it will be lazily initialized)
            var sf = Context.GetStreamFactory<string, int, int>(new Uri("cpt://subject/untyped/create"));
            sf.Create(new Uri("test://subject/1"), "s1", null);
            sf.Create(new Uri("test://subject/2"), "s2", null);

            // Get some mock observers
            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");
            var v2 = ctx1.GetObserver<string, int>(MockObserverUri)("v2");

            // Set up a simple subscription between the observer stream and some mock
            ctx1.GetObservable<int>(new Uri("test://subject/2")).Subscribe(v2, new Uri("test://subscription/2"), null);

            // Use the stream in a higher-order subscription
            ctx1.Return(ctx1.GetObservable<int>(new Uri("test://subject/1")))
                .SelectMany(x => x.Do(ctx1.GetObserver<int>(new Uri("test://subject/2"))), (c, x) => x)
                .Select(y => y + 2)
                .Subscribe(v1, new Uri("test://subscription/1"), null);

            // Wait for inner subscription to be set up
            mgr1.AwaitSubscribe("s1");

            // Assert some output
            var mv1 = GetMockObserver<int>("v1");
            var mv2 = GetMockObserver<int>("v2");
            var s1 = mgr1.GetObserver<int>("s1");
            var s1s = s1.Synchronize(qe1.Scheduler);
            s1s.OnNext(42);
            Assert.AreEqual(1, mv1.Values.Count);
            Assert.AreEqual(44, mv1.Values[0]);
            Assert.AreEqual(1, mv2.Values.Count);
            Assert.AreEqual(42, mv2.Values[0]);

            // Checkpoint -------------------------------------------------------------

            var store = new InMemoryStateStore(Guid.NewGuid().ToString("D"));
            Checkpoint(qe1, store);

            // Tear Down --------------------------------------------------------------

            Crash();
            UndefineSubjectFactories();

            // Recover ----------------------------------------------------------------

            var mgr2 = new SubjectManager();
            DefineSubjectFactories(mgr2);
            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            // Should not need to await the subscription to the subject,
            // as it is already in an inner position and will be fully
            // setup upon recovery.
            Recover(qe2, store);

            // Assert more output
            var mv3 = GetMockObserver<int>("v1");
            var mv4 = GetMockObserver<int>("v2");
            var s2 = mgr2.GetObserver<int>("s1");
            var s2s = s2.Synchronize(qe2.Scheduler);
            s2s.OnNext(43);

            // Note that the `Crash()` caused the MockObserver to be cleared
            Assert.AreEqual(1, mv3.Values.Count);
            Assert.AreEqual(45, mv3.Values[0]);
            Assert.AreEqual(1, mv4.Values.Count);
            Assert.AreEqual(43, mv4.Values[0]);

            Crash();
            UndefineSubjectFactories();
        }

        private void DefineSubjectFactories(SubjectManager mgr)
        {
            var untypedQubjectFactory = Context.Provider.CreateQubjectFactory<Tuple<string>, T, T>((Expression<Func<Tuple<string>, IReactiveQubject>>)(t => (IReactiveQubject)mgr.CreateUntyped(t.Item1)));
            Context.DefineStreamFactory<Tuple<string>, T, T>(new Uri("cpt://subject/untyped/create"), untypedQubjectFactory, null);

            var reliableQubjectFactory = Context.Provider.CreateQubjectFactory<Tuple<string>, T, T>((Expression<Func<Tuple<string>, IReactiveQubject<T, T>>>)(t => (IReactiveQubject<T, T>)mgr.CreateReliable<T>(t.Item1)));
            Context.DefineStreamFactory<Tuple<string>, T, T>(new Uri("cpt://subject/reliable/create"), reliableQubjectFactory, null);
        }

        private void UndefineSubjectFactories()
        {
            Context.UndefineStreamFactory(new Uri("cpt://subject/untyped/create"));
            Context.UndefineStreamFactory(new Uri("cpt://subject/reliable/create"));
        }

        private sealed class SubjectManager
        {
            private readonly ConcurrentDictionary<string, object> _subjects = new();

            public IMultiSubject<T> Create<T>(string id)
            {
                var subject = new S<T>(() => Remove(id));
                Add(id, subject);
                return subject;
            }

            public IMultiSubject CreateUntyped(string id)
            {
                var subject = new U(() => Remove(id));
                Add(id, subject);
                return subject;
            }

            public IReliableMultiSubject<T> CreateReliable<T>(string id)
            {
                var subject = new R<T>(() => Remove(id));
                Add(id, subject);
                return subject;
            }

            public IObserver<T> GetObserver<T>(string id)
            {
                var obj = Get(id);
                if (obj is U untyped)
                {
                    return untyped.GetObserver<T>();
                }
                else
                {
                    return ((S<T>)obj).CreateObserver();
                }
            }

            public IReliableObserver<T> GetReliableObserver<T>(string id)
            {
                return ((R<T>)Get(id)).CreateObserver();
            }

            public void AwaitSubscribe(string id)
            {
                var d = (Disposable)Get(id);
                d.OnSubscribe.WaitOne();
            }

            private void Add(string id, object obj)
            {
                if (!_subjects.TryAdd(id, obj))
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture, "Subject with URI '{0}' already exists.", id));
                }
            }

            private void Remove(string id)
            {
                if (!_subjects.TryRemove(id, out _))
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture, "Could not find subject with URI '{0}'.", id));
                }
            }

            private object Get(string id)
            {
                if (!_subjects.TryGetValue(id, out var obj))
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture, "Could not find subject with URI '{0}'.", id));
                }
                return obj;
            }

            private abstract class Disposable : IDisposable
            {
                private readonly Action _onDispose;

                public Disposable(Action onDispose) => _onDispose = onDispose;

                public void Dispose() => _onDispose();

                public abstract EventWaitHandle OnSubscribe { get; }
            }

            private sealed class R<T> : Disposable, IReliableMultiSubject<T>
            {
                private readonly Subject<Tuple<T, long>> _subject = new();
                private readonly AutoResetEvent _onSubscribe = new(false);

                public R(Action onDispose)
                    : base(onDispose)
                {
                }

                public override EventWaitHandle OnSubscribe => _onSubscribe;

                public IReliableObserver<T> CreateObserver() => new Observer(this);

                public IReliableSubscription Subscribe(IReliableObserver<T> observer) => new Sub(this, observer);

                private sealed class Observer : IReliableObserver<T>
                {
                    private readonly R<T> _parent;

                    public Observer(R<T> parent) => _parent = parent;

                    public Uri ResubscribeUri => throw new NotImplementedException();

                    public void OnNext(T item, long sequenceId) => _parent._subject.OnNext(Tuple.Create(item, sequenceId));

                    public void OnStarted() { }

                    public void OnError(Exception error) => _parent._subject.OnError(error);

                    public void OnCompleted() => _parent._subject.OnCompleted();
                }

                private sealed class Sub : ReliableSubscriptionBase
                {
                    private readonly R<T> _parent;
                    private readonly IReliableObserver<T> _observer;

                    private IDisposable _disposable;

                    public Sub(R<T> parent, IReliableObserver<T> observer)
                    {
                        _parent = parent;
                        _observer = observer;
                    }

                    public override Uri ResubscribeUri => throw new NotImplementedException();

                    public override void Start(long sequenceId)
                    {
                        _disposable = _parent._subject.Subscribe(
                            t => _observer.OnNext(t.Item1, t.Item2),
                            e => _observer.OnError(e),
                            () => _observer.OnCompleted());

                        _parent._onSubscribe.Set();
                    }

                    public override void AcknowledgeRange(long sequenceId)
                    {
                    }

                    public override void DisposeCore()
                    {
                        using (_disposable) { }
                    }
                }
            }

            private sealed class S<T> : Disposable, IMultiSubject<T>
            {
                private readonly Subject<T> _subject = new();
                private readonly AutoResetEvent _onSubscribe = new(false);

                public S(Action onDispose)
                    : base(onDispose)
                {
                }

                public override EventWaitHandle OnSubscribe => _onSubscribe;

                public IObserver<T> CreateObserver() => _subject;

                public ISubscription Subscribe(IObserver<T> observer) => new Sub(this, observer);

                IDisposable IObservable<T>.Subscribe(IObserver<T> observer) => Subscribe(observer);

                private sealed class Sub : Operator<S<T>, T>
                {
                    private IDisposable _disposable;

                    public Sub(S<T> parent, IObserver<T> observer)
                        : base(parent, observer)
                    {
                    }

                    protected override void OnStart()
                    {
                        _disposable = Params._subject.Subscribe(
                            x => Output.OnNext(x),
                            e => Output.OnError(e),
                            () => Output.OnCompleted());

                        Params._onSubscribe.Set();
                    }

                    protected override void OnDispose()
                    {
                        using (_disposable) { }
                    }
                }
            }

            private sealed class U : Disposable, IMultiSubject
            {
                private readonly AutoResetEvent _onSubscribe = new(false);

                private readonly object _subjectGate = new();
                private object _subject;

                public U(Action onDispose)
                    : base(onDispose)
                {
                }

                public override EventWaitHandle OnSubscribe => _onSubscribe;

                public IObserver<T> GetObserver<T>() => Get<T>();

                public ISubscribable<T> GetObservable<T>() => new IO<T>(this);

                private Subject<T> Get<T>()
                {
                    if (_subject == null)
                    {
                        lock (_subjectGate)
                        {
                            _subject ??= new Subject<T>();
                        }
                    }

                    return (Subject<T>)_subject;
                }

                private sealed class IO<T> : SubscribableBase<T>
                {
                    private readonly U _parent;

                    public IO(U parent) => _parent = parent;

                    protected override ISubscription SubscribeCore(IObserver<T> observer) => new _(this, observer);

                    private sealed class _ : Operator<IO<T>, T>
                    {
                        private IDisposable _disposable;

                        public _(IO<T> parent, IObserver<T> observer)
                            : base(parent, observer)
                        {
                        }

                        protected override void OnStart()
                        {
                            _disposable = Params._parent.Get<T>().Subscribe(
                                x => Output.OnNext(x),
                                e => Output.OnError(e),
                                () => Output.OnCompleted());

                            Params._parent._onSubscribe.Set();
                        }

                        protected override void OnDispose()
                        {
                            using (_disposable) { }
                        }
                    }
                }
            }
        }

        private void DefineSubscriptionFactories()
        {
            var subscriptionFactory = Context.Provider.CreateQubscriptionFactory<Tuple<IReactiveQbservable<T>, IReactiveQbserver<T>>>((Expression<Func<Tuple<IReactiveQbservable<T>, IReactiveQbserver<T>>, IReactiveQubscription>>)(t => SubscribeAlias(t.Item1, t.Item2)));
            Context.DefineSubscriptionFactory(new Uri("cpt://subscription/create"), subscriptionFactory, null);
        }

        private void UndefineSubscriptionFactories()
        {
            Context.UndefineSubscriptionFactory(new Uri("cpt://subscription/create"));
        }

        #endregion

        #region Subscription factories

        [TestMethod]
        public void CheckpointSubscription_UsingSubscriptionFactory()
        {
            DefineSubscriptionFactories();
            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            // Create stream and get proxies
            var s1 = ctx1.CreateStream<int>(new Uri("test://mysub"));
            var o1 = ctx1.GetObservable<int>(new Uri("test://mysub"));

            // Get a mock observer
            var w1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

            // Use subscription factory (and use ReactiveService to avoid tupletizing twice!)
            var sf = qe1.ReactiveService.GetSubscriptionFactory<Tuple<IReactiveQbservable<int>, IReactiveQbserver<int>>>(new Uri("cpt://subscription/create"));
            sf.Create(new Uri("test://subscription"), Tuple.Create(o1, w1), null);

            // Assert some output
            var mv1 = GetMockObserver<int>("v1");
            var v1 = ctx1.GetObserver<int>(new Uri("test://mysub"));
            var in1 = v1.Synchronize(qe1.Scheduler);
            in1.OnNext(42);

            Assert.AreEqual(1, mv1.Values.Count);
            Assert.AreEqual(42, mv1.Values[0]);

            // Checkpoint -------------------------------------------------------------

            var store = new InMemoryStateStore(Guid.NewGuid().ToString("D"));
            Checkpoint(qe1, store);

            // Tear Down --------------------------------------------------------------

            Crash();
            UndefineSubscriptionFactories();

            // Recover ----------------------------------------------------------------

            DefineSubscriptionFactories();
            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            Recover(qe2, store);

            // Assert more output
            var mv2 = GetMockObserver<int>("v1");
            var v2 = ctx2.GetObserver<int>(new Uri("test://mysub"));
            var in2 = v2.Synchronize(qe2.Scheduler);
            in2.OnNext(43);

            // Note that the `Crash()` caused the MockObserver to be cleared
            Assert.AreEqual(1, mv2.Values.Count);
            Assert.AreEqual(43, mv2.Values[0]);

            Crash();
            UndefineSubscriptionFactories();
        }

        [KnownResource(Constants.SubscribeUri)]
        private static IReactiveQubscription SubscribeAlias<T>(IReactiveQbservable<T> observable, IReactiveQbserver<T> observer)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region State Transitioning

        [TestMethod]
        public void CheckpointRecovery_StateTransitionObservable()
        {
            StatefulTransitionObservable.IsStateful = false;

            var c = 4;
            var l = LockManager.NewCountdownEvent(c);

            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var testObservableUri = new Uri("rx://test/observable/state");
            var testSubscriptionUri = new Uri("rx://test/subscription");

            Context.DefineObservable<Tuple<int>, int>(testObservableUri, t => new StatefulTransitionObservable(t.Item1).AsQbservable(), null);

            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");
            ctx1.GetObservable<int, int>(testObservableUri)(c).AwaitDo(l).Subscribe(v1, testSubscriptionUri, null);

            // No values pushed in stateless observable
            Assert.IsFalse(LockManager.Wait(l, 100));

            var state = Checkpoint(qe1);
            RemoveQueryEngine(qe1);

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            StatefulTransitionObservable.IsStateful = true;

            Recover(qe2, state);

            // No state is recovered on first transition, so no values are pushed.
            Assert.IsFalse(LockManager.Wait(l, 100));

            var newState = Checkpoint(qe2);
            RemoveQueryEngine(qe2);

            var qe3 = CreateQueryEngine();

            Recover(qe3, newState);

            // Finally, after recovering from the checkpoint with the stateful variant, values are pushed
            Assert.IsTrue(LockManager.Wait(l));
        }

        [TestMethod]
        public void CheckpointRecovery_StateTransitionOperator()
        {
            StatefulTransitionOperator.IsStateful = false;

            var c = 4;
            var l = LockManager.NewCountdownEvent(c);

            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var testObservableUri = new Uri("rx://test/observable/state");
            var testSubscriptionUri = new Uri("rx://test/subscription");

            Context.DefineObservable<Tuple<IReactiveQbservable<int>, int>, int>(
                testObservableUri,
                t => new StatefulTransitionOperator(t.Item1.To(default(ISubscribable<int>)), t.Item2).AsQbservable(),
                null);

            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");
            ctx1.GetObservable<IReactiveQbservable<int>, int, int>(testObservableUri)(ctx1.Never<int>(), c).AwaitDo(l).Subscribe(v1, testSubscriptionUri, null);

            // No values pushed in stateless observable
            Assert.IsFalse(LockManager.Wait(l, 100));

            var state = Checkpoint(qe1);
            RemoveQueryEngine(qe1);

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            StatefulTransitionOperator.IsStateful = true;

            Recover(qe2, state);

            // No state is recovered on first transition, so no values are pushed.
            Assert.IsFalse(LockManager.Wait(l, 100));

            var newState = Checkpoint(qe2);
            RemoveQueryEngine(qe2);

            var qe3 = CreateQueryEngine();

            Recover(qe3, newState);

            // Finally, after recovering from the checkpoint with the stateful variant, values are pushed
            Assert.IsTrue(LockManager.Wait(l));
        }

        [TestMethod]
        public void CheckpointRecovery_StateTransitionObserver()
        {
            StatefulTransitionObserver.IsStateful = false;

            var l = LockManager.NewAutoReset();

            var qe1 = CreateQueryEngine();
            var ctx1 = GetQueryEngineReactiveService(qe1);

            var testObserverUri = new Uri("rx://test/observer/state");
            var testSubscriptionUri = new Uri("rx://test/subscription");

            Context.DefineObserver<Tuple<IReactiveQbserver<int>>, int>(
                testObserverUri,
                t => StatefulTransitionObserver.CreateInstance(t.Item1).To(default(IReactiveQbserver<int>)),
                null);

            var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");
            var observer = ctx1.GetObserver<IReactiveQbserver<int>, int>(testObserverUri);
            ctx1.Never<int>().AwaitSubscribe(l).Subscribe(observer(v1), testSubscriptionUri, null);

            // No values pushed in stateless observable
            Assert.IsTrue(LockManager.Wait(l));

            var mv1 = GetMockObserver<int>("v1");
            Assert.AreEqual(0, mv1.Values.Count);

            var state = Checkpoint(qe1);
            RemoveQueryEngine(qe1);

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            StatefulTransitionObserver.IsStateful = true;

            Recover(qe2, state);

            // No state is recovered on first transition, so no values are pushed.
            Assert.AreEqual(0, mv1.Values.Count);

            var newState = Checkpoint(qe2);
            RemoveQueryEngine(qe2);

            var qe3 = CreateQueryEngine();

            Recover(qe3, newState);

            Assert.IsTrue(mv1.Values.SequenceEqual(new[] { 42 }));
        }

        #endregion

        #region Bridge V1 to V2

        [TestMethod]
        public void Checkpoint_Bridge_V1_to_V2()
        {
            var defaultVersion = BridgeVersion.Version;
            BridgeVersion.Version = Versioning.v1;

            var qe1 = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe1);

            var o1 = ctx.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx.GetObservable<string, int>(MockObservableUri)("o2");

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1.SelectMany(x => o2.Where(y => x < y), (x, y) => x + y).Subscribe(v1, new Uri("s:/s1"), null);

            var s3 = GetMockObserver<int>("v1");

            var s1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(s1);
            Assert.AreEqual(1, s1.SubscriptionCount);
            Assert.AreEqual(sub1uri, s1.Subscriptions.Single().Uri);
            var s1s = s1.Synchronize(qe1.Scheduler);

            s1s.OnNext(3);

            var s2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(s2);
            Assert.AreEqual(1, s2.SubscriptionCount);
            Assert.AreEqual(sub1uri, s2.Subscriptions.Single().Uri);
            var s2s = s2.Synchronize(qe1.Scheduler);

            s2s.OnNext(4);

            var res = s3.Values;
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(7, res[0]);

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            BridgeVersion.Version = Versioning.v2;

            Recover(qe2, chkpt1);

            var out1 = GetMockObserver<int>("v1");

            var in1 = MockObservable.Get<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            Assert.AreEqual(sub1uri, in1.Subscriptions.Single().Uri);
            var in1s = in1.Synchronize(qe2.Scheduler);

            var in2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(in2);
            Assert.AreEqual(sub1uri, in2.Subscriptions.Single().Uri);
            var in2s = in2.Synchronize(qe2.Scheduler);

            in1s.OnNext(4);
            in2s.OnNext(5);

            res = out1.Values;
            AssertResultSequence(out1, new int[] { 8, 9 });

            // Checkpoint -------------------------------------------------------------

            var chkpt2 = Checkpoint(qe2);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe3 = CreateQueryEngine();
            var ctx3 = GetQueryEngineReactiveService(qe3);

            Recover(qe3, chkpt2);
            in2s = in2.Synchronize(qe3.Scheduler);

            in2s.OnNext(3);
            in2s.OnNext(7);

            res = out1.Values;
            AssertResultSequence(out1, new int[] { 8, 9, 10, 11 });

            sub1 = ctx3.GetSubscription(sub1uri);
            sub1.Dispose();

            Assert.IsNull(ctx3.Observables.ToList().Select(kv => kv.Value).SingleOrDefault(entity => entity.Uri.ToCanonicalString().StartsWith("rx://bridge")));
            Assert.IsNull(ctx3.Subscriptions.ToList().Select(kv => kv.Value).SingleOrDefault(entity => entity.Uri.ToCanonicalString().StartsWith("rx://bridge")));
            Assert.IsNull(ctx3.Streams.ToList().Select(kv => kv.Value).SingleOrDefault(entity => entity.Uri.ToCanonicalString().StartsWith("rx://bridge")));

            BridgeVersion.Version = defaultVersion;
        }

        [TestMethod]
        public void Checkpoint_Bridge_V2_to_V1()
        {
            var defaultVersion = BridgeVersion.Version;
            BridgeVersion.Version = Versioning.v2;

            var qe1 = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe1);

            var o1 = ctx.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx.GetObservable<string, int>(MockObservableUri)("o2");

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1uri = new Uri("s:/s1");
            var sub1 = o1.SelectMany(x => o2.Where(y => x < y), (x, y) => x + y).Subscribe(v1, new Uri("s:/s1"), null);

            var s3 = GetMockObserver<int>("v1");

            var s1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(s1);
            Assert.AreEqual(1, s1.SubscriptionCount);
            Assert.AreEqual(sub1uri, s1.Subscriptions.Single().Uri);
            var s1s = s1.Synchronize(qe1.Scheduler);

            s1s.OnNext(3);

            var s2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(s2);
            Assert.AreEqual(1, s2.SubscriptionCount);
            Assert.AreEqual(sub1uri, s2.Subscriptions.Single().Uri);
            var s2s = s2.Synchronize(qe1.Scheduler);

            s2s.OnNext(4);

            var res = s3.Values;
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(7, res[0]);

            // Checkpoint -------------------------------------------------------------

            var chkpt1 = Checkpoint(qe1);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe2 = CreateQueryEngine();
            var ctx2 = GetQueryEngineReactiveService(qe2);

            BridgeVersion.Version = Versioning.v1;

            Recover(qe2, chkpt1);

            var out1 = GetMockObserver<int>("v1");

            var in1 = MockObservable.Get<int>("o1");
            Assert.AreEqual(1, in1.SubscriptionCount);
            Assert.AreEqual(sub1uri, in1.Subscriptions.Single().Uri);
            var in1s = in1.Synchronize(qe2.Scheduler);

            var in2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(in2);
            Assert.AreEqual(sub1uri, in2.Subscriptions.Single().Uri);
            var in2s = in2.Synchronize(qe2.Scheduler);

            in1s.OnNext(4);
            in2s.OnNext(5);

            res = out1.Values;
            AssertResultSequence(out1, new int[] { 8, 9 });

            // Checkpoint -------------------------------------------------------------

            var chkpt2 = Checkpoint(qe2);

            // Clear state ------------------------------------------------------------

            Crash();

            // Recover ----------------------------------------------------------------

            var qe3 = CreateQueryEngine();
            var ctx3 = GetQueryEngineReactiveService(qe3);

            Recover(qe3, chkpt2);
            in2s = in2.Synchronize(qe3.Scheduler);

            in2s.OnNext(3);
            in2s.OnNext(7);

            res = out1.Values;
            AssertResultSequence(out1, new int[] { 8, 9, 10, 11 });

            sub1 = ctx3.GetSubscription(sub1uri);
            sub1.Dispose();

            Assert.IsNull(ctx3.Observables.ToList().Select(kv => kv.Value).SingleOrDefault(entity => entity.Uri.ToCanonicalString().StartsWith("rx://bridge")));
            Assert.IsNull(ctx3.Subscriptions.ToList().Select(kv => kv.Value).SingleOrDefault(entity => entity.Uri.ToCanonicalString().StartsWith("rx://bridge")));
            Assert.IsNull(ctx3.Streams.ToList().Select(kv => kv.Value).SingleOrDefault(entity => entity.Uri.ToCanonicalString().StartsWith("rx://bridge")));

            BridgeVersion.Version = defaultVersion;
        }

        #endregion

        #region Canaries

        [TestMethod]
        public void CheckpointCanariesTest()
        {
            var canaryTraceCount = 0;
            var traceSource = CreateTraceSource(s =>
            {
                if (s.StartsWith("Canary")) canaryTraceCount++;
            });

            var qe = CreateQueryEngine();
            var store = Checkpoint(qe);
            RemoveQueryEngine(qe);

            var qe2 = CreateQueryEngine(traceSource);
            Recover(qe2, store);
            Checkpoint(qe2, store);
            RemoveQueryEngine(qe2);
            Assert.AreEqual(6, canaryTraceCount);

            var qe3 = CreateQueryEngine(traceSource);
            Recover(qe3, store);
            RemoveQueryEngine(qe3);
            Assert.AreEqual(6, canaryTraceCount);
        }

        #endregion

        #region Recovery Mitigation

        [TestMethod]
        public void CheckpointRecoveryMitigation_SubscriptionLoadStateError_Remove()
        {
            var subId = new Uri("test://sub");
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            var sub = ctx.Never<int>().StartWith(1).Subscribe(v1, subId, null);

            var store = Checkpoint(qe);

            RemoveQueryEngine(qe);

            var failedCount = 0;

            store.TryGetItem("SubscriptionsRuntimeState", subId.ToCanonicalString(), out var bytes);
            store.AddOrUpdateItem("SubscriptionsRuntimeState", subId.ToCanonicalString(), bytes.Take(bytes.Length - 50).ToArray());

            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);

            qe.EntityLoadFailed += (s, a) =>
            {
                Assert.IsTrue(a.Error is EntityLoadFailedException);
                a.Handled = true;
                a.Mitigation = ReactiveEntityRecoveryFailureMitigation.Remove;
                Interlocked.Increment(ref failedCount);
            };

            Recover(qe, store);

            Assert.AreEqual(1, failedCount);

            Assert.IsFalse(ctx.Subscriptions.ContainsKey(subId));
        }

        [TestMethod]
        public void CheckpointRecoveryMitigation_SubscriptionLoadStateError_Regenerate()
        {
            var subId = new Uri("test://sub");
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");
            var l = LockManager.NewAutoReset();

            var sub = ctx.Never<int>().StartWith(1).AwaitDo(l).Subscribe(v1, subId, null);

            Assert.IsTrue(LockManager.Wait(l));

            var store = Checkpoint(qe);

            RemoveQueryEngine(qe);

            var failedCount = 0;

            store.TryGetItem("SubscriptionsRuntimeState", subId.ToCanonicalString(), out var bytes);
            store.AddOrUpdateItem("SubscriptionsRuntimeState", subId.ToCanonicalString(), bytes.Take(bytes.Length - 50).ToArray());

            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);

            qe.EntityLoadFailed += (s, a) =>
            {
                Assert.IsTrue(a.Error is EntityLoadFailedException);
                a.Handled = true;
                a.Mitigation = ReactiveEntityRecoveryFailureMitigation.Regenerate;
                Interlocked.Increment(ref failedCount);
            };

            Recover(qe, store);

            Assert.AreEqual(1, failedCount);

            Assert.IsTrue(ctx.Subscriptions.ContainsKey(subId));

            Assert.IsTrue(LockManager.Wait(l));
            var mv1 = GetObserver<int>("v1");
            Assert.IsTrue(mv1.Values.SequenceEqual(new[] { 1, 1 }));
        }

        [TestMethod]
        public void CheckpointRecoveryMitigation_SubscriptionCompilationError_Remove()
        {
            var subId = new Uri("test://sub");
            var ivId = new Uri("test://observer");

            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            var l = LockManager.NewManualReset();

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");
            var sub = ctx.Return(1).SelectMany(x => ctx.Never<int>().StartWith(1), (_, x) => x).AwaitDo(l).Subscribe(v1, subId, null);

            Assert.IsTrue(LockManager.Wait(l));

            var store = Checkpoint(qe);

            RemoveQueryEngine(qe);

            // Remove the bridge from the state store to force recovery failure on
            // both the parent and the upstream subscription
            store.TryGetItemKeys("Subjects", out var keys);
            Debug.Assert(keys.Count() == 1);
            store.RemoveItem("Subjects", keys.First());

            qe = CreateQueryEngine();

            ctx = GetQueryEngineReactiveService(qe);

            var failedCount = 0;

            qe.EntityLoadFailed += (s, a) =>
            {
                Assert.IsTrue(a.Error is EntityLoadFailedException);
                a.Handled = true;
                a.Mitigation = ReactiveEntityRecoveryFailureMitigation.Remove;
                Interlocked.Increment(ref failedCount);
            };

            Recover(qe, store);

            // The first callback will occur because of the compilation failure
            // of the upstream subscription (due to missing bridge binding).
            //
            // The second callback will occur when the parent subscription is
            // started and cannot find the bridge instance.
            Assert.AreEqual(failedCount, 2);

            Assert.IsFalse(ctx.Subscriptions.ContainsKey(subId));

            var mv1 = GetObserver<int>("v1");
            Assert.IsTrue(mv1.Values.SequenceEqual(new[] { 1 }));

            Checkpoint(qe, store);

            RemoveQueryEngine(qe);

            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);

            qe.EntityLoadFailed += (s, a) =>
            {
                Assert.AreEqual(subId, a.Uri);
            };

            // No more errors (all failing entities have been mitigated).
            Recover(qe, store);
        }

        [TestMethod]
        public void CheckpointRecoveryMitigation_SubscriptionCompilationError_Remove_GarbageCollect()
        {
            var trace = new TraceSource("test_gc", SourceLevels.All);
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            trace.Listeners.Add(new TextWriterTraceListener(sw));

            var qe = CreateQueryEngineWithLegacyRedirects();
            qe.Options.GarbageCollectionEnabled = true;
            qe.Options.GarbageCollectionSweepEnabled = true;
            qe.Options.GarbageCollectionSweepBudgetPerCheckpoint = 1000;

            var failedUris = new HashSet<Uri>();

            qe.EntityLoadFailed += (s, a) =>
            {
                lock (failedUris)
                {
                    failedUris.Add(a.Uri);
                }

                Assert.IsTrue(a.Error is EntityLoadFailedException);
                a.Handled = true;
                a.Mitigation = ReactiveEntityRecoveryFailureMitigation.Remove;
            };

            var store = GetStore("/Data/storewithmissingbridge.txt");

            Assert.IsTrue(store.TryGetItem("Observables", "rx://bridge/v2/e2d763c7-a0a1-4680-9020-a51e39576915/upstream-observable", out _));
            Assert.IsTrue(store.TryGetItem("Subscriptions", "rx://bridge/v2/e2d763c7-a0a1-4680-9020-a51e39576915/upstream-subscription", out _));
            Assert.IsTrue(store.TryGetItem("SubscriptionsRuntimeState", "rx://bridge/v2/e2d763c7-a0a1-4680-9020-a51e39576915/upstream-subscription", out _));
            Assert.IsTrue(store.TryGetItem("Subscriptions", "test://sub/", out _));
            Assert.IsTrue(store.TryGetItem("SubscriptionsRuntimeState", "test://sub/", out _));

            Recover(qe, store);

            Assert.IsTrue(failedUris.Contains(new Uri("rx://bridge/v2/e2d763c7-a0a1-4680-9020-a51e39576915/upstream-subscription")));

            store = Checkpoint(qe);

            Assert.IsFalse(store.TryGetItem("Observables", "rx://bridge/v2/e2d763c7-a0a1-4680-9020-a51e39576915/upstream-observable", out _));
            Assert.IsFalse(store.TryGetItem("Subscriptions", "rx://bridge/v2/e2d763c7-a0a1-4680-9020-a51e39576915/upstream-subscription", out _));
            Assert.IsFalse(store.TryGetItem("SubscriptionsRuntimeState", "rx://bridge/v2/e2d763c7-a0a1-4680-9020-a51e39576915/upstream-subscription", out _));
            Assert.IsFalse(store.TryGetItem("Subscriptions", "test://sub/", out _));
            Assert.IsFalse(store.TryGetItem("SubscriptionsRuntimeState", "test://sub/", out _));

            RemoveQueryEngine(qe);

            qe = CreateQueryEngine(trace);

            Recover(qe, store);
        }

        [TestMethod]
        public void CheckpointRecoveryMitigation_SubscriptionCompilationError_Regenerate_InvalidMitigationForState()
        {
            var subId = new Uri("test://sub");
            var ivId = new Uri("test://observer");

            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            var l = LockManager.NewManualReset();

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");
            var sub = ctx.Return(1).SelectMany(x => ctx.Never<int>().StartWith(1), (_, x) => x).AwaitDo(l).Subscribe(v1, subId, null);

            Assert.IsTrue(LockManager.Wait(l));

            var store = Checkpoint(qe);
            RemoveQueryEngine(qe);

            // Remove the bridge from the state store to force recovery failure
            // on both the parent and the upstream subscription
            store.TryGetItemKeys("Subjects", out var keys);
            Debug.Assert(keys.Count() == 1);
            store.RemoveItem("Subjects", keys.First());

            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);

            var failedCount = 0;
            var upstreamSubscriptionUri = default(Uri);

            qe.EntityLoadFailed += (s, a) =>
            {
                // Only EntityLoadFailedExceptions should come through this channel.
                Assert.IsTrue(a.Error is EntityLoadFailedException);
                var inner = a.Error.InnerException;

                a.Handled = true;
                // This is an invalid mitigation strategy for the point of failure.
                a.Mitigation = ReactiveEntityRecoveryFailureMitigation.Regenerate;
                Interlocked.Increment(ref failedCount);

                if (a.Uri.ToCanonicalString().EndsWith("upstream-subscription")) upstreamSubscriptionUri = a.Uri;
            };

            Recover(qe, store);

            Assert.AreEqual(2, failedCount);

            Assert.IsTrue(ctx.Subscriptions.ContainsKey(subId));

            // The upstream subscription will be reachable through any delete APIs for cleanup, but not through the metadata API.
            Assert.IsNotNull(upstreamSubscriptionUri);
            Assert.IsFalse(ctx.Subscriptions.AsEnumerable().Any(kv => kv.Key == upstreamSubscriptionUri));

            // Does not throw KeyNotFoundException...
            ctx.GetSubscription(upstreamSubscriptionUri).Dispose();
        }

        [TestMethod]
        public void CheckpointRecoveryMitigation_ReactiveEntity_DeserializationError_ExistsForDeletionOnly()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            // Test URIs
            var nopObserver = ctx.GetObserver<int>(new Uri("rx://observer/nop"));
            var intReturnUri = new Uri("test://observable/return/int");
            var intNopUri = new Uri("test://observer/nop/int");
            var sfUri = new Uri("rx://subject/create");
            var intStreamUri = new Uri("test://subject/int");
            var subUri = new Uri("test://subscription");

            // Create test artifacts
            ctx.DefineObservable<int>(intReturnUri, ctx.Return(42), null);
            ctx.DefineObserver<int>(intNopUri, nopObserver, null);
            ctx.GetStreamFactory<int, int>(sfUri).Create(intStreamUri, null);
            ctx.Never<int>().Subscribe(nopObserver, subUri, null);

            var store = Checkpoint(qe);

            // Modify checkpoint state
            Assert.IsTrue(store.TryGetItem("Observables", intReturnUri.ToCanonicalString(), out var bytes));
            var modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Observables", intReturnUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("Observers", intNopUri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Observers", intNopUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("Subjects", intStreamUri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Subjects", intStreamUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("Subscriptions", subUri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Subscriptions", subUri.ToCanonicalString(), modifiedBytes);

            // Recover query engine from modified state
            RemoveQueryEngine(qe);

            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);

            var count = 0;
            qe.EntityLoadFailed += (s, a) => { Interlocked.Increment(ref count); a.Handled = true; };

            Recover(qe, store);

            Assert.AreEqual(4, count);

            // Ensure modified expressions are not exposed by QE metadata
            Assert.IsFalse(ctx.Observables.ContainsKey(intReturnUri));
            Assert.IsNull(ctx.Observables.Values.ToList().SingleOrDefault(v => v.Uri == intReturnUri));
            Assert.IsNull(ctx.Observables.Keys.ToList().SingleOrDefault(k => k == intReturnUri));
            Assert.AreEqual(default, ctx.Observables.SingleOrDefault(kv => kv.Key == intReturnUri));

            Assert.IsFalse(ctx.Observers.ContainsKey(intNopUri));
            Assert.IsNull(ctx.Observers.Values.ToList().SingleOrDefault(v => v.Uri == intNopUri));
            Assert.IsNull(ctx.Observers.Keys.ToList().SingleOrDefault(k => k == intNopUri));
            Assert.AreEqual(default, ctx.Observers.SingleOrDefault(kv => kv.Key == intNopUri));

            Assert.IsFalse(ctx.Streams.ContainsKey(intStreamUri));
            Assert.IsNull(ctx.Streams.Values.ToList().SingleOrDefault(v => v.Uri == intStreamUri));
            Assert.IsNull(ctx.Streams.Keys.ToList().SingleOrDefault(k => k == intStreamUri));
            Assert.AreEqual(default, ctx.Streams.SingleOrDefault(kv => kv.Key == intStreamUri));

            Assert.IsFalse(ctx.Subscriptions.ContainsKey(subUri));
            Assert.IsNull(ctx.Subscriptions.Values.ToList().SingleOrDefault(v => v.Uri == subUri));
            Assert.IsNull(ctx.Subscriptions.Keys.ToList().SingleOrDefault(k => k == subUri));
            Assert.AreEqual(default, ctx.Subscriptions.SingleOrDefault(kv => kv.Key == subUri));

            // Ensure we can't use these artifacts in subscriptions
            var failed = false;
            try { ctx.GetObservable<int>(intReturnUri).Subscribe(nopObserver, new Uri("test://subscription/2"), null); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intReturnUri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            failed = false;
            try { ctx.Never<int>().Subscribe(ctx.GetObserver<int>(intNopUri), new Uri("test://subscription/2"), null); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intNopUri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            failed = false;
            try { ctx.GetObservable<int>(intStreamUri).Subscribe(nopObserver, new Uri("test://subscription/2"), null); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intStreamUri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            failed = false;
            try { ctx.Never<int>().Subscribe(ctx.GetObserver<int>(intStreamUri), new Uri("test://subscription/2"), null); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intStreamUri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            // Can remove still...
            var notFound = false;
            ctx.UndefineObservable(intReturnUri);
            try { ctx.UndefineObservable(intReturnUri); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            ctx.UndefineObserver(intNopUri);
            try { ctx.UndefineObserver(intNopUri); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            ctx.GetStream<int, int>(intStreamUri).Dispose();
            try { ctx.GetStream<int, int>(intStreamUri).Dispose(); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            ctx.GetSubscription(subUri).Dispose();
            try { ctx.GetSubscription(subUri).Dispose(); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            // Checkpoint again and confirm it does not come back
            store = Checkpoint(qe);
            RemoveQueryEngine(qe);
            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);
            Recover(qe, store);

            notFound = false;
            try { ctx.UndefineObservable(intReturnUri); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { ctx.UndefineObserver(intNopUri); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { ctx.GetStream<int, int>(intStreamUri).Dispose(); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { ctx.GetSubscription(subUri).Dispose(); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);
        }

        [TestMethod]
        public async Task CheckpointRecoveryMitigation_ReactiveEntity_DeserializationOrEvalError_ExistsForDeletionOnly_Async()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineAsyncReactiveService(qe);

            // Test URIs
            var nopObserver = ctx.GetObserver<int>(new Uri("rx://observer/nop"));
            var intReturnUri = new Uri("test://observable/return/int");
            var intNopUri = new Uri("test://observer/nop/int");
            var sfUri = new Uri("rx://subject/create");
            var intStreamUri = new Uri("test://subject/int");
            var intStream2Uri = new Uri("test://subject2/int");
            var subUri = new Uri("test://subscription");
            var sub2Uri = new Uri("test://subscription2");

            // Create test artifacts
            await ctx.DefineObservableAsync<int>(intReturnUri, ctx.Return(42), null, CancellationToken.None);
            await ctx.DefineObserverAsync<int>(intNopUri, nopObserver, null, CancellationToken.None);
            await ctx.GetStreamFactory<int, int>(sfUri).CreateAsync(intStreamUri, null, CancellationToken.None);
            await ctx.GetStreamFactory<int, int>(sfUri).CreateAsync(intStream2Uri, null, CancellationToken.None);
            await ctx.Never<int>().SubscribeAsync(nopObserver, subUri, null, CancellationToken.None);
            await ctx.Never<int>().SubscribeAsync(nopObserver, sub2Uri, null, CancellationToken.None);

            var store = Checkpoint(qe);

            // Modify checkpoint state
            Assert.IsTrue(store.TryGetItem("Observables", intReturnUri.ToCanonicalString(), out var bytes));
            var modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Observables", intReturnUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("Observers", intNopUri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Observers", intNopUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("Subjects", intStreamUri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Subjects", intStreamUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("SubjectsRuntimeState", intStream2Uri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Subjects", intStream2Uri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("Subscriptions", subUri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Subscriptions", subUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("SubscriptionsRuntimeState", sub2Uri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("SubscriptionsRuntimeState", sub2Uri.ToCanonicalString(), modifiedBytes);

            // Recover query engine from modified state
            RemoveQueryEngine(qe);

            qe = CreateQueryEngine();
            ctx = GetQueryEngineAsyncReactiveService(qe);

            var count = 0;
            qe.EntityLoadFailed += (s, a) => { Interlocked.Increment(ref count); a.Handled = true; };

            Recover(qe, store);

            Assert.AreEqual(6, count);

            // Ensure modified expressions are not exposed by QE metadata
            Assert.IsFalse(ctx.Observables.ContainsKey(intReturnUri));
            Assert.IsNull(ctx.Observables.Values.ToList().SingleOrDefault(v => v.Uri == intReturnUri));
            Assert.IsNull(ctx.Observables.Keys.ToList().SingleOrDefault(k => k == intReturnUri));
            Assert.AreEqual(default, ctx.Observables.SingleOrDefault(kv => kv.Key == intReturnUri));

            Assert.IsFalse(ctx.Observers.ContainsKey(intNopUri));
            Assert.IsNull(ctx.Observers.Values.ToList().SingleOrDefault(v => v.Uri == intNopUri));
            Assert.IsNull(ctx.Observers.Keys.ToList().SingleOrDefault(k => k == intNopUri));
            Assert.AreEqual(default, ctx.Observers.SingleOrDefault(kv => kv.Key == intNopUri));

            Assert.IsFalse(ctx.Streams.ContainsKey(intStreamUri));
            Assert.IsNull(ctx.Streams.Values.ToList().SingleOrDefault(v => v.Uri == intStreamUri));
            Assert.IsNull(ctx.Streams.Keys.ToList().SingleOrDefault(k => k == intStreamUri));
            Assert.AreEqual(default, ctx.Streams.SingleOrDefault(kv => kv.Key == intStreamUri));

            Assert.IsFalse(ctx.Streams.ContainsKey(intStream2Uri));
            Assert.IsNull(ctx.Streams.Values.ToList().SingleOrDefault(v => v.Uri == intStream2Uri));
            Assert.IsNull(ctx.Streams.Keys.ToList().SingleOrDefault(k => k == intStream2Uri));
            Assert.AreEqual(default, ctx.Streams.SingleOrDefault(kv => kv.Key == intStream2Uri));

            Assert.IsFalse(ctx.Subscriptions.ContainsKey(subUri));
            Assert.IsNull(ctx.Subscriptions.Values.ToList().SingleOrDefault(v => v.Uri == subUri));
            Assert.IsNull(ctx.Subscriptions.Keys.ToList().SingleOrDefault(k => k == subUri));
            Assert.AreEqual(default, ctx.Subscriptions.SingleOrDefault(kv => kv.Key == subUri));

            Assert.IsFalse(ctx.Subscriptions.ContainsKey(sub2Uri));
            Assert.IsNull(ctx.Subscriptions.Values.ToList().SingleOrDefault(v => v.Uri == sub2Uri));
            Assert.IsNull(ctx.Subscriptions.Keys.ToList().SingleOrDefault(k => k == sub2Uri));
            Assert.AreEqual(default, ctx.Subscriptions.SingleOrDefault(kv => kv.Key == sub2Uri));

            // Ensure we can't use these artifacts in subscriptions
            var failed = false;
            try { await ctx.GetObservable<int>(intReturnUri).SubscribeAsync(nopObserver, new Uri("test://subscription/2"), null, CancellationToken.None); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intReturnUri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            failed = false;
            try { await ctx.Never<int>().SubscribeAsync(ctx.GetObserver<int>(intNopUri), new Uri("test://subscription/2"), null, CancellationToken.None); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intNopUri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            failed = false;
            try { await ctx.GetObservable<int>(intStreamUri).SubscribeAsync(nopObserver, new Uri("test://subscription/2"), null, CancellationToken.None); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intStreamUri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            failed = false;
            try { await ctx.Never<int>().SubscribeAsync(ctx.GetObserver<int>(intStream2Uri), new Uri("test://subscription/2"), null, CancellationToken.None); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intStream2Uri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            failed = false;
            try { await ctx.GetObservable<int>(intStream2Uri).SubscribeAsync(nopObserver, new Uri("test://subscription/2"), null, CancellationToken.None); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intStream2Uri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            failed = false;
            try { await ctx.Never<int>().SubscribeAsync(ctx.GetObserver<int>(intStreamUri), new Uri("test://subscription/2"), null, CancellationToken.None); }
            catch (InvalidOperationException ex) { failed = true; Assert.IsTrue(ex.Message.Contains(intStreamUri.ToCanonicalString())); }
            Assert.IsTrue(failed);

            // Can remove still...
            var notFound = false;
            await ctx.UndefineObservableAsync(intReturnUri, CancellationToken.None);
            try { await ctx.UndefineObservableAsync(intReturnUri, CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            await ctx.UndefineObserverAsync(intNopUri, CancellationToken.None);
            try { await ctx.UndefineObserverAsync(intNopUri, CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            await ctx.GetStream<int, int>(intStreamUri).DisposeAsync(CancellationToken.None);
            try { await ctx.GetStream<int, int>(intStreamUri).DisposeAsync(CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            await ctx.GetStream<int, int>(intStream2Uri).DisposeAsync(CancellationToken.None);
            try { await ctx.GetStream<int, int>(intStream2Uri).DisposeAsync(CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            await ctx.GetSubscription(subUri).DisposeAsync(CancellationToken.None);
            try { await ctx.GetSubscription(subUri).DisposeAsync(CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            await ctx.GetSubscription(sub2Uri).DisposeAsync(CancellationToken.None);
            try { await ctx.GetSubscription(sub2Uri).DisposeAsync(CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            // Checkpoint again and confirm it does not come back
            store = Checkpoint(qe);
            RemoveQueryEngine(qe);
            qe = CreateQueryEngine();
            ctx = GetQueryEngineAsyncReactiveService(qe);
            Recover(qe, store);

            notFound = false;
            try { await ctx.UndefineObservableAsync(intReturnUri, CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { await ctx.UndefineObserverAsync(intNopUri, CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { await ctx.GetStream<int, int>(intStreamUri).DisposeAsync(CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { await ctx.GetStream<int, int>(intStream2Uri).DisposeAsync(CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { await ctx.GetSubscription(subUri).DisposeAsync(CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { await ctx.GetSubscription(sub2Uri).DisposeAsync(CancellationToken.None); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);
        }

        [TestMethod]
        public void CheckpointRecoveryMitigation_ReactiveEntity_DeserializationError_RemoveMitigation()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            // Test URIs
            var nopObserver = ctx.GetObserver<int>(new Uri("rx://observer/nop"));
            var intReturnUri = new Uri("test://observable/return/int");
            var intNopUri = new Uri("test://observer/nop/int");
            var sfUri = new Uri("rx://subject/create");
            var intStreamUri = new Uri("test://subject/int");
            var subUri = new Uri("test://subscription");

            // Create test artifacts
            ctx.DefineObservable<int>(intReturnUri, ctx.Return(42), null);
            ctx.DefineObserver<int>(intNopUri, nopObserver, null);
            ctx.GetStreamFactory<int, int>(sfUri).Create(intStreamUri, null);
            ctx.Never<int>().Subscribe(nopObserver, subUri, null);

            var store = Checkpoint(qe);

            // Modify checkpoint state
            Assert.IsTrue(store.TryGetItem("Observables", intReturnUri.ToCanonicalString(), out var bytes));
            var modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Observables", intReturnUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("Observers", intNopUri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Observers", intNopUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("Subjects", intStreamUri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Subjects", intStreamUri.ToCanonicalString(), modifiedBytes);

            Assert.IsTrue(store.TryGetItem("Subscriptions", subUri.ToCanonicalString(), out bytes));
            modifiedBytes = bytes.Take(50).Concat(bytes.Skip(55)).ToArray();
            store.AddOrUpdateItem("Subscriptions", subUri.ToCanonicalString(), modifiedBytes);

            // Recover query engine from modified state
            RemoveQueryEngine(qe);

            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);

            var count = 0;
            qe.EntityLoadFailed += (s, a) =>
            {
                Interlocked.Increment(ref count);
                a.Handled = true;
                a.Mitigation = ReactiveEntityRecoveryFailureMitigation.Remove;
            };

            Recover(qe, store);

            Assert.AreEqual(4, count);

            // Ensure modified expressions are not exposed by QE metadata
            Assert.IsFalse(ctx.Observables.ContainsKey(intReturnUri));
            Assert.IsNull(ctx.Observables.Values.ToList().SingleOrDefault(v => v.Uri == intReturnUri));
            Assert.IsNull(ctx.Observables.Keys.ToList().SingleOrDefault(k => k == intReturnUri));
            Assert.AreEqual(default, ctx.Observables.SingleOrDefault(kv => kv.Key == intReturnUri));

            Assert.IsFalse(ctx.Observers.ContainsKey(intNopUri));
            Assert.IsNull(ctx.Observers.Values.ToList().SingleOrDefault(v => v.Uri == intNopUri));
            Assert.IsNull(ctx.Observers.Keys.ToList().SingleOrDefault(k => k == intNopUri));
            Assert.AreEqual(default, ctx.Observers.SingleOrDefault(kv => kv.Key == intNopUri));

            Assert.IsFalse(ctx.Streams.ContainsKey(intStreamUri));
            Assert.IsNull(ctx.Streams.Values.ToList().SingleOrDefault(v => v.Uri == intStreamUri));
            Assert.IsNull(ctx.Streams.Keys.ToList().SingleOrDefault(k => k == intStreamUri));
            Assert.AreEqual(default, ctx.Streams.SingleOrDefault(kv => kv.Key == intStreamUri));

            Assert.IsFalse(ctx.Subscriptions.ContainsKey(subUri));
            Assert.IsNull(ctx.Subscriptions.Values.ToList().SingleOrDefault(v => v.Uri == subUri));
            Assert.IsNull(ctx.Subscriptions.Keys.ToList().SingleOrDefault(k => k == subUri));
            Assert.AreEqual(default, ctx.Subscriptions.SingleOrDefault(kv => kv.Key == subUri));

            // Can't remove because they do not exist.
            var notFound = false;
            try { ctx.UndefineObservable(intReturnUri); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { ctx.UndefineObserver(intNopUri); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { ctx.GetStream<int, int>(intStreamUri).Dispose(); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            notFound = false;
            try { ctx.GetSubscription(subUri).Dispose(); }
            catch (EntityNotFoundException) { notFound = true; }
            Assert.IsTrue(notFound);

            // Checkpoint again and confirm no errors upon recovery
            store = Checkpoint(qe);
            RemoveQueryEngine(qe);
            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);

            count = 0;
            qe.EntityLoadFailed += (s, a) => { Interlocked.Increment(ref count); a.Handled = true; };
            Recover(qe, store);
            Assert.AreEqual(0, count);
        }

        #endregion

        #region Debugging

        [TestMethod]
        public void Checkpointing_BlobLogsAndSummary()
        {
            var subIdBase = "test://sub";
            var subUris = new List<Uri>();

            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            for (var i = 0; i < 32; ++i)
            {
                var subUri = new Uri(subIdBase + "/" + i);
                subUris.Add(subUri);

                var sub = ctx.Never<int>().Subscribe(v1, subUri, null);
            }

            var store = Checkpoint(qe);
            RemoveQueryEngine(qe);

            var summaryRegex = new Regex("Recovery summary for");
            var summaries = 0;
            var onWrite = new Action<string>(trace =>
            {
                if (summaryRegex.IsMatch(trace)) summaries++;
            });

            qe = CreateQueryEngine(CreateTraceSource(onWrite, SourceLevels.Information));
            ctx = GetQueryEngineReactiveService(qe);

            var wasTracking = EntityMetricsTracker.ShouldTrack;
            EntityMetricsTracker.ShouldTrack = true;

            qe.Options.DumpRecoveryStateBlobs = true;
            qe.Options.DumpRecoveryStatePath = Environment.CurrentDirectory;

            var qePath = Path.Combine(Environment.CurrentDirectory, Uri.EscapeDataString(qe.Uri.ToCanonicalString()));

            Recover(qe, store);

            {
                var blobLogPattern = new Regex(@"blobs_.*\.log", RegexOptions.Compiled);
                var blobLog = Directory.EnumerateFiles(qePath).SingleOrDefault(f => blobLogPattern.IsMatch(f));
                Assert.IsNotNull(blobLog);

                //
                // NB: This test is flaky on Linux where the file can be read before its fully written.
                //

                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return;
                }

                static string ReadBlobLog(string path)
                {
                    //
                    // NB: Blob logging is fire-and-forget async so the file may not be  closed by the time we want to read it.
                    //     To reduce flakiness in the test suite, we have a little retry loop below.
                    //
                    //     By setting the maximum number of attempts to 12, the maximum run duration is 40950 ms, which should
                    //     be plenty of time for the file to get written to disk.
                    //

                    var delay = 10;
                    var maxAttempts = 12;
                    var attempt = 0;

                    while (true)
                    {
                        try
                        {
                            return File.ReadAllText(path);
                        }
                        catch (IOException) when (attempt++ < maxAttempts)
                        {
                            Thread.Sleep(delay);

                            delay *= 2;
                        }
                    }
                }

                var blobLogData = ReadBlobLog(blobLog);

                foreach (var subUri in subUris)
                {
                    Assert.IsTrue(blobLogData.Contains("Subscriptions\t" + subUri), "Missing subscription for " + subUri);
                    Assert.IsTrue(blobLogData.Contains("SubscriptionsRuntimeState\t" + subUri), "Missing subscription runtime state for " + subUri);
                }

                try
                {
                    File.Delete(blobLog);
                }
                catch { }
            }

            Assert.AreEqual(9, summaries);

            EntityMetricsTracker.ShouldTrack = wasTracking;
        }

        #endregion

        #region Garbage Collection

        [TestMethod]
        public void GarbageCollection_OrphanedBridges()
        {
            var trace = new TraceSource("test_gc", SourceLevels.All);
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            trace.Listeners.Add(new TextWriterTraceListener(sw));

            var qe = CreateQueryEngineWithLegacyRedirects(trace);
            qe.Options.GarbageCollectionEnabled = true;
            qe.Options.GarbageCollectionSweepEnabled = true;
            qe.Options.GarbageCollectionSweepBudgetPerCheckpoint = 1;

            var undefined = new List<Uri>();
            qe.EntityUndefined += (o, e) =>
            {
                lock (undefined)
                {
                    undefined.Add(e.Uri);
                }
            };

            var store = GetStore("/Data/storewithorphanedbridges.txt");

            Recover(qe, store);

            Assert.AreEqual(0, undefined.Count);

            Checkpoint(qe, CheckpointKind.Full);

            Assert.AreEqual(1, undefined.Count);

            var final = Checkpoint(qe, CheckpointKind.Full);

            Assert.AreEqual(2, undefined.Count);

            Assert.IsTrue(final.TryGetItemKeys("Observables", out var obs));
            Assert.AreEqual(1, obs.Count(x => !x.StartsWith("mgmt"))); // upstream

            Assert.IsTrue(final.TryGetItemKeys("Observers", out var obv));
            Assert.AreEqual(0, obv.Count(x => !x.StartsWith("mgmt"))); // none

            Assert.IsTrue(final.TryGetItemKeys("Subjects", out var str));
            Assert.AreEqual(1, str.Count(x => !x.StartsWith("mgmt"))); // bridge

            Assert.IsTrue(final.TryGetItemKeys("Subscriptions", out var sub));
            Assert.AreEqual(2, sub.Count(x => !x.StartsWith("mgmt"))); // upstream + test
        }

        private static InMemoryStateStore GetStore(string fileName)
        {
            fileName = fileName.Replace("/", ".").Replace(@"\", ".");

            var asm = Assembly.GetExecutingAssembly();
            var name = asm.GetName().Name;
            var stream = asm.GetManifestResourceStream(name + fileName);

            var store = InMemoryStateStore.Load(stream);
            return store;
        }

        private CheckpointingQueryEngine CreateQueryEngineWithLegacyRedirects(TraceSource traceSource = null)
        {
            return CreateQueryEngine("qe:/" + Guid.NewGuid(), GetScheduler(), serializationPolicy: new SerializationPolicy(new LegacyRedirectExpressionPolicy(), Versioning.v3), traceSource: traceSource);
        }

        private sealed class LegacyRedirectExpressionPolicy : IExpressionPolicy
        {
            public ICompiledDelegateCache DelegateCache => DefaultExpressionPolicy.Instance.DelegateCache;

            public ICache<Expression> InMemoryCache => DefaultExpressionPolicy.Instance.InMemoryCache;

            public System.Linq.CompilerServices.IConstantHoister ConstantHoister => DefaultExpressionPolicy.Instance.ConstantHoister;

            public bool OutlineCompilation => DefaultExpressionPolicy.Instance.OutlineCompilation;

            public IReflectionProvider ReflectionProvider { get; } = new RedirectReflectionProvider();

            public IExpressionFactory ExpressionFactory => DefaultExpressionPolicy.Instance.ExpressionFactory;

            public IMemoizer LiftMemoizer => DefaultExpressionPolicy.Instance.LiftMemoizer;

            public IMemoizer ReduceMemoizer => DefaultExpressionPolicy.Instance.ReduceMemoizer;

            private sealed class RedirectReflectionProvider : DefaultReflectionProvider
            {
                public override Type GetType(string typeName, bool throwOnError, bool ignoreCase) => base.GetType(typeName switch
                {
                    //
                    // NB: This is a minimal list to make tests pass. When/if we migrate existing Reactor deployments to the OSS Reaqtor bits,
                    //     a redirect policy can be built for all types in all assemblies.
                    //
                    "Microsoft.Reactive.Subscribable, Microsoft.Reactive, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" => typeof(Subscribable).AssemblyQualifiedName,
                    "Microsoft.Reactive.ISubscribable`1, Microsoft.Reactive, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" => typeof(ISubscribable<>).AssemblyQualifiedName,
                    "Microsoft.Reactive.ISubscription, Microsoft.Reactive, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" => typeof(ISubscription).AssemblyQualifiedName,
                    "Microsoft.Reactive.Observer, Microsoft.Reactive, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" => typeof(Observer).AssemblyQualifiedName,
                    "Microsoft.Reactive.Reliable.IReliableMultiSubject`2, Microsoft.Reactive.Reliable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" => typeof(IReliableMultiSubject<,>).AssemblyQualifiedName,
                    "Tests.Microsoft.Reactive.QueryEngine.TestExtensions, Tests.Microsoft.Reactive.QueryEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" => typeof(TestExtensions).AssemblyQualifiedName,
                    "Tests.Microsoft.Reactive.QueryEngine.MockObserver, Tests.Microsoft.Reactive.QueryEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" => typeof(MockObserver).AssemblyQualifiedName,
                    "Tests.Microsoft.Reactive.QueryEngine.MockObservable, Tests.Microsoft.Reactive.QueryEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" => typeof(MockObservable).AssemblyQualifiedName,
                    _ => typeName,
                }, throwOnError, ignoreCase);
            }
        }

        #endregion

        #region Template Migration

        [TestMethod]
        public void Checkpointing_TemplateMigration_Quota()
        {
            var subCount = 100;
            var quota = 10;
            var regex = "test";
            var subIdBase = "test://sub/";

            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);
            qe.Options.TemplatizeExpressions = false;

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            for (var i = 0; i < subCount; ++i)
            {
                var sub = ctx.Never<int>().Subscribe(v1, new Uri(subIdBase + i), null);
            }

            var store = Checkpoint(qe);

            RemoveQueryEngine(qe);

            var templatizeCountdown = new CountdownEvent(quota);

            qe = CreateQueryEngine(CreateTraceSource(trace =>
            {
                if (trace.Contains("templatized expression for entity"))
                {
                    if (templatizeCountdown.CurrentCount > 0)
                    {
                        templatizeCountdown.Signal();
                    }
                }
            }));

            ctx = GetQueryEngineReactiveService(qe);
            qe.Options.TemplatizeExpressions = true;
            qe.Options.TemplatizeRecoveredEntitiesQuota = quota;
            qe.Options.TemplatizeRecoveredEntitiesRegex = regex;

            Recover(qe, store);

            templatizeCountdown.Wait();
            templatizeCountdown.Reset(quota);

            var store2 = Checkpoint(qe);

            var updated = 0;
            Assert.IsTrue(store.TryGetItemKeys("Subscriptions", out var keys));
            foreach (var key in keys)
            {
                Assert.IsTrue(store.TryGetItem("Subscriptions", key, out var first));
                Assert.IsTrue(store2.TryGetItem("Subscriptions", key, out var second));

                if (!first.SequenceEqual(second)) updated++;
            }

            Assert.AreEqual(quota, updated);

            templatizeCountdown.Wait();
            templatizeCountdown.Reset(quota);

            var store3 = Checkpoint(qe);

            updated = 0;
            var updated2 = 0;
            foreach (var key in keys)
            {
                Assert.IsTrue(store.TryGetItem("Subscriptions", key, out var first));
                Assert.IsTrue(store2.TryGetItem("Subscriptions", key, out var second));
                Assert.IsTrue(store3.TryGetItem("Subscriptions", key, out var third));

                if (!first.SequenceEqual(third)) updated++;
                if (!second.SequenceEqual(third)) updated2++;
            }

            Assert.AreEqual(quota * 2, updated);
            Assert.AreEqual(quota, updated2);
        }

        [TestMethod]
        public void Checkpointing_TemplateMigration_Regex()
        {
            var subCount = 100;
            var quota = 20;
            var total = 30;
            var regex = "[123]$";
            var subIdBase = "test://sub/";

            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);
            qe.Options.TemplatizeExpressions = false;

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            for (var i = 0; i < subCount; ++i)
            {
                var sub = ctx.Never<int>().Subscribe(v1, new Uri(subIdBase + i), null);
            }

            var store = Checkpoint(qe);

            RemoveQueryEngine(qe);

            var templatizeCountdown = new CountdownEvent(quota);

            qe = CreateQueryEngine(CreateTraceSource(trace =>
            {
                if (trace.Contains("templatized expression for entity"))
                {
                    if (templatizeCountdown.CurrentCount > 0)
                    {
                        templatizeCountdown.Signal();
                    }
                    else
                    {
                        Task.Yield();
                    }
                }
            }));

            ctx = GetQueryEngineReactiveService(qe);
            qe.Options.TemplatizeExpressions = true;
            qe.Options.TemplatizeRecoveredEntitiesQuota = quota;
            qe.Options.TemplatizeRecoveredEntitiesRegex = regex;

            Recover(qe, store);

            templatizeCountdown.Wait();
            templatizeCountdown.Reset(total - quota);

            var store2 = Checkpoint(qe);

            var updated = 0;
            Assert.IsTrue(store.TryGetItemKeys("Subscriptions", out var keys));
            foreach (var key in keys)
            {
                Assert.IsTrue(store.TryGetItem("Subscriptions", key, out var first));
                Assert.IsTrue(store2.TryGetItem("Subscriptions", key, out var second));

                if (!first.SequenceEqual(second)) updated++;
            }

            Assert.AreEqual(quota, updated);

            templatizeCountdown.Wait();

            var store3 = Checkpoint(qe);

            updated = 0;
            var updated2 = 0;
            foreach (var key in keys)
            {
                Assert.IsTrue(store.TryGetItem("Subscriptions", key, out var first));
                Assert.IsTrue(store2.TryGetItem("Subscriptions", key, out var second));
                Assert.IsTrue(store3.TryGetItem("Subscriptions", key, out var third));

                if (!first.SequenceEqual(third)) updated++;
                if (!second.SequenceEqual(third)) updated2++;
            }

            Assert.AreEqual(30, updated);
            Assert.AreEqual(30 - quota, updated2);

            //
            // Finally, assert that nothing is re-templatized.
            //
            RemoveQueryEngine(qe);

            templatizeCountdown.Reset(1);
            qe = CreateQueryEngine(CreateTraceSource(trace =>
            {
                if (trace.Contains("templatized expression for entity"))
                {
                    if (templatizeCountdown.CurrentCount > 0)
                    {
                        templatizeCountdown.Signal();
                    }
                    else
                    {
                        Task.Yield();
                    }
                }
            }));
            ctx = GetQueryEngineReactiveService(qe);
            qe.Options.TemplatizeExpressions = true;
            qe.Options.TemplatizeRecoveredEntitiesQuota = quota;
            qe.Options.TemplatizeRecoveredEntitiesRegex = regex;

            Recover(qe, store3);

            Assert.IsFalse(templatizeCountdown.Wait(5000));
        }

        #endregion

        #region Checkpoint Cancellation

        [TestMethod]
        public async Task Checkpoint_EarlyTermination()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);
            var subCount = 100;
            var terminateOn = 5;

            for (var i = 0; i < subCount; ++i)
            {
                ctx.Never<int>().Subscribe(ctx.Nop<int>(), new Uri("sub:/test/" + i), null);
            }

            var stateWriter = new ThrowingStateWriter(terminateOn);

            try
            {
                try
                {
                    try
                    {
                        await qe.CheckpointAsync(stateWriter);
                    }
                    catch (AggregateException ex)
                    {
                        Assert.AreEqual(1, ex.InnerExceptions.Count);
                        throw ex.InnerException;
                    }
                }
                catch (EntitySaveFailedException ex)
                {
                    throw ex.InnerException;
                }
            }
            catch (InvalidOperationException)
            {
            }

            Assert.IsTrue(stateWriter.IsRollbackCalled);
            Assert.AreEqual(terminateOn, stateWriter.GetItemWriterCalls);
        }

        private sealed class ThrowingStateWriter : IStateWriter
        {
            private readonly ConcurrentQueue<Stream> _stream = new();

            private readonly int _throwAfter;

            private int _getItemWriterCalls;

            public ThrowingStateWriter(int throwAfter)
            {
                _throwAfter = throwAfter;
            }

            public bool IsRollbackCalled
            {
                get;
                private set;
            }

            public int GetItemWriterCalls => _getItemWriterCalls;

            public CheckpointKind CheckpointKind => CheckpointKind.Full;

            public Task CommitAsync(CancellationToken token, IProgress<int> progress)
            {
                return Task.FromResult(true);
            }

            public void Rollback()
            {
                IsRollbackCalled = true;
            }

            public Stream GetItemWriter(string category, string key)
            {
                if (Interlocked.Increment(ref _getItemWriterCalls) >= _throwAfter)
                {
                    throw new InvalidOperationException();
                }

                var stream = new MemoryStream();
                _stream.Enqueue(stream);
                return stream;
            }

            public void DeleteItem(string category, string key)
            {
            }

            public void Dispose()
            {
                while (_stream.TryDequeue(out _))
                {
                    using (default(Stream)) { }
                }
            }
        }

        #endregion

        #region Helper methods

#if NETFRAMEWORK
        private static TReturn RunInDifferentDomain<TTestClass, TState, TReturn>(Func<TTestClass, TState, TReturn> func, TState state)
        {
            AppDomain domain = AppDomain.CreateDomain(
                "Function Executor " + func.GetHashCode(),
                null,
                new AppDomainSetup
                {
                    ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                    PrivateBinPath = AppDomain.CurrentDomain.RelativeSearchPath,
                    DisallowBindingRedirects = true, // Disable the test agent's config to be picked up, e.g. causing Newtonsoft.Json to be redirected.
                });

            try
            {
                domain.SetData("toInvoke", func);
                domain.SetData("state", state);
                domain.DoCallBack(() =>
                {
                    var testObjest = Activator.CreateInstance<TTestClass>();
                    var classInitialization =
                        typeof(TTestClass)
                            .GetMethods()
                            .Single(m => m.GetCustomAttribute<ClassInitializeAttribute>() != null);
                    classInitialization.Invoke(null, new object[] { null });

                    var testInitialization = typeof(TTestClass)
                            .GetMethods()
                            .Single(m => m.GetCustomAttribute<TestInitializeAttribute>() != null);
                    testInitialization.Invoke(testObjest, Array.Empty<object>());

                    var f = AppDomain.CurrentDomain.GetData("toInvoke") as Func<TTestClass, TState, TReturn>;
                    var result = f(testObjest, (TState)AppDomain.CurrentDomain.GetData("state"));
                    AppDomain.CurrentDomain.SetData("result", result);
                });

                return (TReturn)domain.GetData("result");
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }

        private static TReturn RunInDifferentDomain<TTestClass, TReturn>(Func<TTestClass, TReturn> func)
        {
            return RunInDifferentDomain<TTestClass, object, TReturn>(new SerializableFunction<TTestClass, TReturn> { Function = func }.Invoke, null);
        }

        private static void RunInDifferentDomain<TTestClass, TState>(Action<TTestClass, TState> action, TState state)
        {
            RunInDifferentDomain<TTestClass, TState, int>(new SerializableAction<TTestClass, TState> { Action = action }.Invoke, state);
        }

        [Serializable]
        private class SerializableFunction<TTestClass, TReturn>
        {
            public Func<TTestClass, TReturn> Function { get; set; }

            public TReturn Invoke(TTestClass testClass, object _)
            {
                return Function(testClass);
            }
        }

        [Serializable]
        private class SerializableAction<TTestClass, TState>
        {
            public Action<TTestClass, TState> Action { get; set; }

            public int Invoke(TTestClass testClass, TState state)
            {
                Action(testClass, state);
                return 0;
            }
        }
#endif

        #endregion
    }
}
