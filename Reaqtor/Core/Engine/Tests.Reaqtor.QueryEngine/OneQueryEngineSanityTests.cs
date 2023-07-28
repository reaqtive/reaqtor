// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Subjects;

using Reaqtor;
using Reaqtor.Expressions.Core;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.Events;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;
using Reaqtor.QueryEngine.Mocks;
using Reaqtor.Reliable;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class OneQueryEngineSanityTests : PhysicalTimeEngineTest
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable IDE0060 // Remove unused parameter (MSTest)
        [ClassInitialize]
        public static void ClassSetup(TestContext ignored)
        {
            PhysicalTimeEngineTest.ClassSetup();
        }
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0079 // Remove unnecessary suppression

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
        public void CheckpointingQueryEngine_ArgumentChecks()
        {
            var uri = new Uri("qe:/qe1");
            var invalidUri = new Uri("test://qe/");

            var sch = GetScheduler();
            var kvs = new InMemoryKeyValueStore();
            var ser = SerializationPolicy.Default;

            Assert.ThrowsException<ArgumentNullException>(() => new CheckpointingQueryEngine(null, Resolver, sch, Context, kvs, ser, null));
            Assert.ThrowsException<ArgumentException>(() => new CheckpointingQueryEngine(invalidUri, Resolver, GetScheduler(), Context, kvs, ser, null));
            Assert.ThrowsException<ArgumentNullException>(() => new CheckpointingQueryEngine(uri, null, GetScheduler(), Context, kvs, ser, null));
            Assert.ThrowsException<ArgumentNullException>(() => new CheckpointingQueryEngine(uri, Resolver, null, Context, kvs, ser, null));
            Assert.ThrowsException<ArgumentNullException>(() => new CheckpointingQueryEngine(uri, Resolver, GetScheduler(), null, kvs, ser, null));
            Assert.ThrowsException<ArgumentNullException>(() => new CheckpointingQueryEngine(uri, Resolver, GetScheduler(), Context, null, ser, null));
            Assert.ThrowsException<ArgumentNullException>(() => new CheckpointingQueryEngine(uri, Resolver, GetScheduler(), Context, kvs, null, null));
        }

        [TestMethod]
        public void DefineUndefineObservable()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            Uri uri = new Uri("io:/o1");
            Expression<Func<string, IReactiveQbservable<T>>> obs = id => MockObservable.CreateObservable<T>(id).AsQbservable();

            ctx.DefineObservable<string, T>(uri, obs, null);

            Assert.IsTrue(new ExpressionComparator().Equals(ctx.Observables[uri].Expression, Rewrite(obs)));
            Assert.ThrowsException<EntityAlreadyExistsException>(() => ctx.DefineObservable<string, T>(uri, obs, null));

            ctx.UndefineObservable(uri);

            Assert.ThrowsException<EntityNotFoundException>(() => ctx.UndefineObservable(uri));
        }

        [TestMethod]
        public async Task DefineUndefineObservableAsync()
        {
#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);
            var asyncCtx = new TupletizingClientContext(qe.ServiceProvider);

            Uri uri = new Uri("io:/o1");
            Expression<Func<string, IAsyncReactiveQbservable<T>>> obs = id => MockObservable.CreateObservable<T>(id).AsQbservable().To<IReactiveQbservable<T>, IAsyncReactiveQbservable<T>>();

            await asyncCtx.DefineObservableAsync<string, T>(uri, obs, null, CancellationToken.None);

            Assert.IsTrue(new ExpressionComparator().Equals(ctx.Observables[uri].Expression, Rewrite((Expression<Func<string, IReactiveQbservable<T>>>)(id => MockObservable.CreateObservable<T>(id).AsQbservable()))));
            await Assert.ThrowsExceptionAsync<EntityAlreadyExistsException>(() => asyncCtx.DefineObservableAsync<string, T>(uri, obs, null, CancellationToken.None));

            await asyncCtx.UndefineObservableAsync(uri, CancellationToken.None);

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(() => asyncCtx.UndefineObservableAsync(uri, CancellationToken.None));
        }

        [TestMethod]
        public void DefineUndefineObserver()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            Uri uri = new Uri("iv:/v1");
            Expression<Func<string, IReactiveQbserver<T>>> obv = (id) => MockObserver.CreateObserver<T>(id).AsQbserver();

            ctx.DefineObserver<string, T>(uri, obv, null);

            Assert.IsTrue(new ExpressionComparator().Equals(ctx.Observers[uri].Expression, Rewrite(obv)));
            Assert.ThrowsException<EntityAlreadyExistsException>(() => ctx.DefineObserver<string, T>(uri, obv, null));

            ctx.UndefineObserver(uri);

            Assert.ThrowsException<EntityNotFoundException>(() => ctx.UndefineObserver(uri));
        }

        [TestMethod]
        public async Task DefineUndefineObserverAsync()
        {
#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);
            var asyncCtx = new TupletizingClientContext(qe.ServiceProvider);

            Uri uri = new Uri("iv:/v1");
            Expression<Func<string, IAsyncReactiveQbserver<T>>> obv = (id) => MockObserver.CreateObserver<T>(id).To<IObserver<T>, IAsyncReactiveQbserver<T>>();

            await asyncCtx.DefineObserverAsync<string, T>(uri, obv, null, CancellationToken.None);

            Assert.IsTrue(new ExpressionComparator().Equals(ctx.Observers[uri].Expression, (Expression<Func<string, IObserver<T>>>)(id => MockObserver.CreateObserver<T>(id))));
            await Assert.ThrowsExceptionAsync<EntityAlreadyExistsException>(() => asyncCtx.DefineObserverAsync<string, T>(uri, obv, null, CancellationToken.None));

            await asyncCtx.UndefineObserverAsync(uri, CancellationToken.None);

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(() => asyncCtx.UndefineObserverAsync(uri, CancellationToken.None));
        }

        [TestMethod]
        public void DefineUndefineStreamFactory()
        {
            using var qe = CreateQueryEngine();

            var ctx = qe.ReactiveService;

            Uri uri = new Uri("sf:/s2");
            var sf = ctx.GetStreamFactory<int, int>(new Uri("sf:/s1"));

            ctx.DefineStreamFactory<int, int>(uri, sf, null);

            Assert.IsTrue(new ExpressionComparator().Equals(ctx.StreamFactories[uri].Expression, Rewrite(sf.Expression)));
            Assert.ThrowsException<EntityAlreadyExistsException>(() => ctx.DefineStreamFactory<int, int>(uri, sf, null));

            ctx.UndefineStreamFactory(uri);

            Assert.ThrowsException<EntityNotFoundException>(() => ctx.UndefineStreamFactory(uri));
        }

        [TestMethod]
        public async Task DefineUndefineStreamFactoryAsync()
        {
#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using var qe = CreateQueryEngine();

            var ctx = qe.ReactiveService;
            var asyncCtx = new TupletizingClientContext(qe.ServiceProvider);

            Uri uri = new Uri("sf:/s2");
            var sf = asyncCtx.GetStreamFactory<int, int>(new Uri("sf:/s1"));

            await asyncCtx.DefineStreamFactoryAsync<int, int>(uri, sf, null, CancellationToken.None);

            Assert.IsTrue(new ExpressionComparator().Equals(ctx.StreamFactories[uri].Expression, Rewrite(sf.Expression)));
            await Assert.ThrowsExceptionAsync<EntityAlreadyExistsException>(() => asyncCtx.DefineStreamFactoryAsync<int, int>(uri, sf, null, CancellationToken.None));

            await asyncCtx.UndefineStreamFactoryAsync(uri, CancellationToken.None);

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(() => asyncCtx.UndefineStreamFactoryAsync(uri, CancellationToken.None));
        }

        [TestMethod]
        public void SubscriptionLifecycle()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            AssertObservableNotCreated<int>("o1");
            AssertObserverNotCreated<int>("v1");

            var uri = new Uri("s:/sub1");

            var src = ctx.GetObservable<string, int>(MockObservableUri)("o1");
            var obv = ctx.GetObserver<string, int>(MockObserverUri)("v1");
            var sub = src.Subscribe(obv, uri, null);

            Assert.ThrowsException<EntityAlreadyExistsException>(() =>
            {
                // Subscription with uri already exists
                src.Subscribe(obv, uri, null);
            });

            var o1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(o1);
            Assert.AreEqual(1, o1.SubscriptionCount);

            var v1 = GetMockObserver<int>("v1");
            Assert.AreEqual(uri, v1.InstanceId);

            o1.OnNext(0);
            o1.OnNext(1);

            Assert.IsFalse(v1.Completed);
            Assert.IsFalse(v1.Error);
            AssertEx.AreSequenceEqual(new[] { 0, 1 }, v1.Values);

            sub.Dispose();

            Assert.AreEqual(0, o1.SubscriptionCount);

            o1.OnNext(2); // ignored

            Assert.ThrowsException<EntityNotFoundException>(() =>
            {
                // Subscription with uri doesn't exist
                sub.Dispose();
            });

            // Lazy evaluation - doesn't throw until Dispose is called
            sub = ctx.GetSubscription(uri);

            Assert.ThrowsException<EntityNotFoundException>(() =>
            {
                // Subscription with uri doesn't exist
                sub.Dispose();
            });

            o1.OnNext(3); // ignored

            Assert.IsFalse(v1.Completed);
            Assert.IsFalse(v1.Error);
            AssertEx.AreSequenceEqual(new[] { 0, 1 }, v1.Values);

            sub = src.Subscribe(obv, uri, null);

            Assert.AreEqual(1, o1.SubscriptionCount);
            Assert.AreEqual(uri, v1.InstanceId);

            o1.OnNext(4);
            o1.OnNext(5);

            sub = ctx.GetSubscription(uri);
            sub.Dispose();

            Assert.AreEqual(0, o1.SubscriptionCount);

            o1.OnNext(6); // ignored

            Assert.IsFalse(v1.Completed);
            Assert.IsFalse(v1.Error);
            AssertEx.AreSequenceEqual(new[] { 0, 1, 4, 5 }, v1.Values);
        }

        [TestMethod]
        public async Task SubscriptionLifecycleAsync()
        {
#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using var qe = CreateQueryEngine();

            var ctx = new TupletizingClientContext(qe.ServiceProvider);

            AssertObservableNotCreated<int>("o1");
            AssertObserverNotCreated<int>("v1");

            var uri = new Uri("s:/sub1");

            var src = ctx.GetObservable<string, int>(MockObservableUri)("o1");
            var obv = ctx.GetObserver<string, int>(MockObserverUri)("v1");
            var sub = await src.SubscribeAsync(obv, uri, null, CancellationToken.None);

            await Assert.ThrowsExceptionAsync<EntityAlreadyExistsException>(async () =>
            {
                // Subscription with uri already exists
                await src.SubscribeAsync(obv, uri, null);
            });

            var o1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(o1);
            Assert.AreEqual(1, o1.SubscriptionCount);

            var v1 = GetMockObserver<int>("v1");
            Assert.AreEqual(uri, v1.InstanceId);

            o1.OnNext(0);
            o1.OnNext(1);

            Assert.IsFalse(v1.Completed);
            Assert.IsFalse(v1.Error);
            AssertEx.AreSequenceEqual(new[] { 0, 1 }, v1.Values);

            await sub.DisposeAsync(CancellationToken.None);

            Assert.AreEqual(0, o1.SubscriptionCount);

            o1.OnNext(2); // ignored

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                // Subscription with uri doesn't exist
                await sub.DisposeAsync(CancellationToken.None);
            });

            // Lazy evaluation - doesn't throw until Dispose is called
            sub = ctx.GetSubscription(uri);

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                // Subscription with uri doesn't exist
                await sub.DisposeAsync(CancellationToken.None);
            });

            o1.OnNext(3); // ignored

            Assert.IsFalse(v1.Completed);
            Assert.IsFalse(v1.Error);
            AssertEx.AreSequenceEqual(new[] { 0, 1 }, v1.Values);

            sub = await src.SubscribeAsync(obv, uri, null, CancellationToken.None);

            Assert.AreEqual(1, o1.SubscriptionCount);
            Assert.AreEqual(uri, v1.InstanceId);

            o1.OnNext(4);
            o1.OnNext(5);

            sub = ctx.GetSubscription(uri);
            await sub.DisposeAsync(CancellationToken.None);

            Assert.AreEqual(0, o1.SubscriptionCount);

            o1.OnNext(6); // ignored

            Assert.IsFalse(v1.Completed);
            Assert.IsFalse(v1.Error);
            AssertEx.AreSequenceEqual(new[] { 0, 1, 4, 5 }, v1.Values);
        }

        [TestMethod]
        public void PassThroughQuery()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            AssertObservableNotCreated<int>("o1");
            AssertObserverNotCreated<int>("v1");

            var sub =
                ctx.GetObservable<string, int>(MockObservableUri)("o1")
                .Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null);

            var o1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(o1);
            Assert.AreEqual(1, o1.SubscriptionCount);

            var v1 = GetMockObserver<int>("v1");
            Assert.AreEqual(new Uri("s:/sub1"), v1.InstanceId);

            o1.OnNext(0);
            o1.OnNext(1);

            Assert.IsFalse(v1.Completed);
            Assert.IsFalse(v1.Error);

            o1.OnCompleted();

            Assert.IsTrue(v1.Completed);
            Assert.IsFalse(v1.Error);

            sub.Dispose();

            Assert.AreEqual(0, o1.SubscriptionCount);

            o1.OnNext(3);
            o1.OnNext(4);

            AssertResult(v1, 2, (i, v) => Assert.AreEqual(i, v));
        }

        [TestMethod]
        public void ErrorInStream()
        {
            using var engine = CreateQueryEngine();

            var reactive = GetQueryEngineReactiveService(engine);

            // Create stream.
            var sourceUri = new Uri("reactor:/source");
            var subscriptionUri = new Uri("reactor:/subscription");
            var src = reactive.CreateStream<int>(sourceUri);
            var observable = reactive.GetObservable<int>(sourceUri);

            // Create subscription
            observable.Subscribe(reactive.GetObserver<string, int>(MockObserverUri)("result"), subscriptionUri, null);

            var srcs = src.Synchronize(engine.Scheduler);

            srcs.OnNext(0);

            var result = GetObserver<int>("result");
            AssertResult(result, 1, Assert.AreEqual);
            Assert.IsFalse(result.Completed);
            Assert.IsFalse(result.Error);

            srcs.OnError(new Exception());
            Assert.IsFalse(result.Completed);
            Assert.IsTrue(result.Error);

            srcs.OnNext(1);
            AssertResult(result, 1, Assert.AreEqual);
        }

        [TestMethod]
        public void SimpleQuery()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            AssertObservableNotCreated<int>("o1");
            AssertObserverNotCreated<int>("v1");

            var sub =
                ctx.GetObservable<string, int>(MockObservableUri)("o1")
                .Where(x => x % 2 == 0)
                .Take(5)
                .Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null);

            var o1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(o1);
            Assert.AreEqual(1, o1.SubscriptionCount);

            var o1s = o1.Synchronize(qe.Scheduler);

            var v1 = GetMockObserver<int>("v1");
            Assert.AreEqual(new Uri("s:/sub1"), v1.InstanceId);

            for (int i = 0; i < 20; i++)
            {
                o1s.OnNext(i);
            }

            Assert.IsTrue(v1.Completed);
            Assert.IsFalse(v1.Error);

            o1s.OnCompleted();

            Assert.IsTrue(v1.Completed);
            Assert.IsFalse(v1.Error);

            sub.Dispose();

            Assert.AreEqual(0, o1.SubscriptionCount);

            AssertResult(v1, 5, (i, v) => Assert.AreEqual(i * 2, v));
        }

        [TestMethod]
        public void ForeignFunction()
        {
            using var qe = CreateQueryEngine();

            // NB: Have to use async here; the sync path uses the UriToReactiveBinder which does not
            //     support foreign functions because it can't leave any parameter unbound.

            var ctx = GetQueryEngineAsyncReactiveService(qe);

            AssertObservableNotCreated<int>("o1");
            AssertObserverNotCreated<int>("v1");

            var sub =
                ctx.GetObservable<string, int>(MockObservableUri)("o1")
                .Select(i => Square(i))
                .SubscribeAsync(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null, CancellationToken.None).Result;

            var o1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(o1);
            Assert.AreEqual(1, o1.SubscriptionCount);

            var o1s = o1.Synchronize(qe.Scheduler);

            var v1 = GetMockObserver<int>("v1");
            Assert.AreEqual(new Uri("s:/sub1"), v1.InstanceId);

            for (int i = 0; i < 10; i++)
            {
                o1s.OnNext(i);
            }

            sub.DisposeAsync(CancellationToken.None)
#if NET6_0 || NETCOREAPP3_1
                .AsTask()
#endif
                .Wait();

            AssertResult(v1, 10, (i, v) => Assert.AreEqual(i * i, v));
        }

        [KnownResource("function://int/square")]
        public static int Square(int x)
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DistinctUntilQuery()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            var o1 = MockObservable.Get<int>("o1");
            Assert.IsNull(o1);

            var v1 = MockObserver.Get<int>("v1");
            Assert.IsNull(v1);

            var sub =
                ctx.GetObservable<string, int>(MockObservableUri)("o1")
                .DistinctUntilChanged(x => x)
                .Take(10)
                .Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null);

            o1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(o1);
            Assert.AreEqual(1, o1.SubscriptionCount);

            v1 = MockObserver.Get<int>("v1");
            Assert.IsNotNull(v1);
            Assert.IsFalse(v1.Completed);
            Assert.IsFalse(v1.Error);
            Assert.AreEqual(0, v1.Values.Count);

            var o1s = o1.Synchronize(qe.Scheduler);

            for (int i = 0; i < 20; i++)
            {
                o1s.OnNext(i / 2);
            }

            Assert.IsTrue(v1.Completed);
            Assert.IsFalse(v1.Error);

            o1s.OnCompleted();

            Assert.IsTrue(v1.Completed);
            Assert.IsFalse(v1.Error);

            sub.Dispose();

            Assert.AreEqual(0, o1.SubscriptionCount);

            Assert.AreEqual(10, v1.Values.Count);

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(i, v1.Values[i]);
            }
        }

        [TestMethod]
        public void CreateDeleteStream()
        {
            using var qe = CreateQueryEngine();

            Uri uri = new Uri("str:/s1");
            var s1 = qe.ReactiveService.CreateStream<int>(uri);
            Assert.IsNotNull(s1);
            s1.Dispose();
        }

        [TestMethod]
        public void StreamPassThrough()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            Uri str1 = new Uri("str:/s1");

            var s1 = ctx.CreateStream<int>(str1);
            Assert.IsNotNull(s1);

            var s1o = ctx.GetObservable<int>(str1);
            Assert.IsNotNull(s1o);

            AssertObserverNotCreated<int>("v1");

            var sub = s1o
                .Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), new Uri("s:/sub1"), null);

            var v1 = GetMockObserver<int>("v1");

            var s1v = qe.ReactiveService.GetObserver<int>(str1);
            Assert.IsNotNull(s1v);

            var s1vs = s1v.Synchronize(qe.Scheduler);

            for (int i = 0; i < 5; i++)
            {
                s1vs.OnNext(i);
            }

            sub.Dispose();
            s1.Dispose();

            AssertResult(v1, 5, (i, v) => Assert.AreEqual(i, v));
        }

        [TestMethod]
        public void TimerQuery()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            ctx.DefineObservable(new Uri("qe:/engine1/timer"), ctx.Timer(DateTime.Now), null);
            var o1 = ctx.GetObservable<long>(new Uri("qe:/engine1/timer"));

            _ = o1.Subscribe(ctx.GetObserver<string, long>(MockObserverUri)("result"), new Uri("s:/sub1"), null);

            var res = MockObserver.Get<long>("result");
            Assert.IsNotNull(res);

            Assert.IsTrue(res.WaitForCompleted(Timeout));

            Assert.IsTrue(res.Completed);
            Assert.IsFalse(res.Error);
            Assert.AreEqual(1, res.Values.Count);
        }

        [TestMethod]
        public void PeriodicTimerQuery()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            ctx.DefineObservable(new Uri("qe:/engine1/timer"), ctx.Timer(DateTime.Now, TimeSpan.FromMinutes(5)), null);
            var o1 = ctx.GetObservable<long>(new Uri("qe:/engine1/timer"));

            ctx.DefineObservable(new Uri("qe:/engine2/o2"), o1.Take(2), null);
            var o2 = ctx.GetObservable<long>(new Uri("qe:/engine2/o2"));

            _ = o2.Subscribe(ctx.GetObserver<string, long>(MockObserverUri)("result"), new Uri("s:/sub1"), null);

            var res = MockObserver.Get<long>("result");
            Assert.IsNotNull(res);

            Assert.IsTrue(res.WaitForCount(1, Timeout));

            Assert.IsFalse(res.Completed);
            Assert.IsFalse(res.Error);
            Assert.AreEqual(1, res.Values.Count);
        }

        [TestMethod]
        public void SelectMany()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            var o1 = ctx.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx.GetObservable<string, int>(MockObservableUri)("o2");

            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1 = o1.SelectMany(x => o2.Where(y => x < y), (x, y) => x + y).Subscribe(v1, new Uri("s:/s1"), null);

            var s3 = GetMockObserver<int>("v1");

            var s1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(s1);
            Assert.AreEqual(1, s1.SubscriptionCount);

            var s1s = s1.Synchronize(qe.Scheduler);

            s1s.OnNext(3);

            var s2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(s2);
            Assert.AreEqual(1, s2.SubscriptionCount);

            var s2s = s2.Synchronize(qe.Scheduler);

            s2s.OnNext(4);

            var res = s3.Values;
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(7, res[0]);

            sub1.Dispose();
        }

        [TestMethod]
        public void Throttle()
        {
            // Throttles (i.e., ignores) two observations from the main stream. Testing that
            // resources are persisted through the QE's reactive service context (there are
            // comprehensive one-machine tests elsewhere)

            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            var o1 = ctx.GetObservable<string, int>(MockObservableUri)("o1");
            var o2 = ctx.GetObservable<string, int>(MockObservableUri)("o2");
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            var sub1 = o1.Throttle<int, int>((_) => o2).Subscribe(v1, new Uri("s:/s1"), null);

            var s3 = GetMockObserver<int>("v1");

            var s1 = MockObservable.Get<int>("o1");
            Assert.IsNotNull(s1);
            Assert.AreEqual(1, s1.SubscriptionCount);

            var s1v = s1.Synchronize(qe.Scheduler);

            s1v.OnNext(1);  // ignored

            var s2 = MockObservable.Get<int>("o2");
            Assert.IsNotNull(s2);
            Assert.AreEqual(1, s2.SubscriptionCount);

            var s2v = s2.Synchronize(qe.Scheduler);

            s1v.OnNext(2);  // ignored
            s1v.OnNext(3);
            s2v.OnNext(99);  // throttle signal
            s1v.OnNext(4);

            Assert.AreEqual(1, s3.Values.Count);
            Assert.AreEqual(3, s3.Values[0]);

            s1v.OnCompleted();

            Assert.AreEqual(2, s3.Values.Count);
            Assert.AreEqual(4, s3.Values[1]);

            sub1.Dispose();
        }

        [TestMethod]
        public void Switch()
        {
            // Observables to the switch, test observables before the one that
            // is currently subsribed to are ignored when they fire off an OnNext.

            using var qe = CreateQueryEngine();

            var ctx1 = GetQueryEngineReactiveService(qe);

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

            var in1s = in1.Synchronize(qe.Scheduler);

            in1s.OnNext(2);  // generates observable identified as o2

            var in2 = GetObservable<int>("o2");

            var in2s = in2.Synchronize(qe.Scheduler);

            in2s.OnNext(97);

            in1s.OnNext(3);  // generates observable identified as o3
            var in3 = GetObservable<int>("o3");

            var in3s = in3.Synchronize(qe.Scheduler);

            in2s.OnNext(98); // ignored
            in3s.OnNext(99);

            AssertResultSequence(out1, new int[] { 97, 99 });
        }

        [TestMethod]
        public void Switch_SimpleQeCrash()
        {
            InMemoryStateStore chkpt1;

            var sub1uri = new Uri("s:/s1");

            using (var qe1 = CreateQueryEngine())
            {
                var ctx1 = GetQueryEngineReactiveService(qe1);

                var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                var generator = ctx1.GetObservable<string, int>(MockObservableUri);
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

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

                chkpt1 = Checkpoint(qe1);

                // Clear state ------------------------------------------------------------

                Crash();
            }

            using (var qe2 = CreateQueryEngine())
            {
                var ctx2 = GetQueryEngineReactiveService(qe2);

                // Recover ----------------------------------------------------------------

                Recover(qe2, chkpt1);

                // Validate ---------------------------------------------------------------

                var sub1_validation = ctx2.GetSubscription(sub1uri);
                Assert.IsNotNull(sub1_validation);

                var o1_validation = MockObservable.Get<int>("o1");
                Assert.AreEqual(1, o1_validation.SubscriptionCount);

                var v1_validation = GetMockObserver<int>("v1");
            }
        }

        [TestMethod]
        public void Metadata()
        {
            var defaultVersion = BridgeVersion.Version;
            BridgeVersion.Version = Versioning.v1;

            var ivUri = new Uri("iv:/o1");
            var subUri = new Uri("s:/sub1");
            var sub2Uri = new Uri("s:/sub2");
            var testIvUri = new Uri("iv:/test");

            var state = "foo";
            var state2 = "bar";
            var testIvState = "qux";

            var testIvExpr = Expression.Parameter(typeof(IReactiveQbserver<int>), "rx://observer/nop");

            InMemoryStateStore chkpt1;

            var res = default(IEnumerable<string>);

            void assertMetadata(CheckpointingQueryEngine qe)
            {
                var se = qe.ReactiveService.Subscriptions[subUri];

                Assert.AreEqual(subUri, se.Uri);
                Assert.AreEqual(state, se.State);

                var fvs = FreeVariableScanner.Scan(se.Expression).OrderBy(p => p.Name).Select(p => p.Name).ToList();

                if (res != null)
                {
                    Assert.IsTrue(fvs.SequenceEqual(res), "[" + string.Join(", ", res) + "] != [" + string.Join(", ", fvs) + "]");
                }
                else
                {
                    res = fvs;
                }

                se = null;

                Assert.IsTrue(qe.ReactiveService.Subscriptions.TryGetValue(subUri, out se));
                Assert.AreEqual(subUri, se.Uri);
                Assert.AreEqual(state, se.State);

                var keys = qe.ReactiveService.Subscriptions.Keys.ToList();

                Assert.IsTrue(keys.Contains(subUri) && keys.Contains(sub2Uri));

                var vals = qe.ReactiveService.Subscriptions.Values.ToList();
                vals = vals.Where(val => !val.Uri.ToCanonicalString().StartsWith("mgmt://")).ToList();

                Assert.AreEqual(state, vals.Single(v => v.Uri == subUri).State);
                Assert.AreEqual(state2, vals.Single(v => v.Uri == sub2Uri).State);

                var subs = qe.ReactiveService.Subscriptions.ToList();
                subs = subs.Where(kv => !kv.Key.ToCanonicalString().StartsWith("mgmt://")).ToList();

                Assert.AreEqual(state, subs.Single(s => s.Value.Uri == subUri).Value.State);
                Assert.AreEqual(state2, subs.Single(s => s.Value.Uri == sub2Uri).Value.State);

                var ios = qe.ReactiveService.Observables.ToList();
                var upstreamObservables = ios.Where(io => io.Value.Uri.ToCanonicalString().StartsWith("rx://bridge")).ToList();
                Assert.AreEqual(1, upstreamObservables.Count);
                var upstreamSubscriptions = subs.Where(s => s.Value.Uri.ToCanonicalString().StartsWith("rx://bridge")).ToList();
                Assert.AreEqual(1, upstreamSubscriptions.Count);
                var streams = qe.ReactiveService.Streams.ToList();
                var bridges = streams.Where(s => s.Value.Uri.ToCanonicalString().StartsWith("rx://bridge")).ToList();
                Assert.AreEqual(1, bridges.Count);
                var ivs = qe.ReactiveService.Observers.ToList();
                var testObserver = ivs.Where(kv => kv.Value.Uri == testIvUri).Select(kv => kv.Value).SingleOrDefault();
                Assert.IsNotNull(testObserver);
                Assert.AreEqual(testIvExpr.Name, ((ParameterExpression)testObserver.Expression).Name);
            }

            using (var qe1 = CreateQueryEngine())
            {
                var ctx = GetQueryEngineReactiveService(qe1);

                AssertObservableNotCreated<int>("o1");
                AssertObserverNotCreated<int>("v1");

                var sub =
                    ctx.GetObservable<string, int>(MockObservableUri)("o1")
                    .Where(x => x % 2 == 0)
                    .Take(5)
                    .Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v1"), subUri, state);

                var l = LockManager.NewAutoReset();
                var sub2 = ctx.Never<int>()
                    .StartWith(1)
                    .SelectMany(x => ctx.Never<int>().StartWith(x), (_, c) => c)
                    .AwaitDo(l)
                    .Subscribe(ctx.GetObserver<string, int>(MockObserverUri)("v2"), sub2Uri, state2);

                ctx.DefineObserver<int>(testIvUri, ctx.Provider.CreateQbserver<int>(testIvExpr), testIvState);

                // Note: the edges of the subscription above don't show up in the expression revealed by the QE due
                // to aggressive inlining. As such, the expression for the expression in the QE isn't a good object
                // for analysis purposes.

                LockManager.Wait(l);

                assertMetadata(qe1);

                // Checkpoint -------------------------------------------------------------

                chkpt1 = Checkpoint(qe1);

                // Clear state ------------------------------------------------------------

                Crash();
            }

            using (var qe2 = CreateQueryEngine())
            {
                var ctx2 = GetQueryEngineReactiveService(qe2);

                // Recover ----------------------------------------------------------------

                Recover(qe2, chkpt1);

                // Validate ---------------------------------------------------------------

                assertMetadata(qe2);
            }

            BridgeVersion.Version = defaultVersion;
        }

        [TestMethod]
        public void CheckpointDuringStart()
        {
            InMemoryStateStore chkpt1, chkpt2;

            var sub1uri = new Uri("s:/s1");

            using (var qe1 = CreateQueryEngine())
            {
                var ctx1 = GetQueryEngineReactiveService(qe1);

                var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                var l1 = LockManager.NewManualReset();
                var l2 = LockManager.NewManualReset();

                var subTask = Task.Run(() =>
                {
                    var sub1 = o1
                        .BlockSubscribe(l1)
                        .AwaitSubscribe(l2)
                        .Subscribe(v1, sub1uri, null);
                });

                // Checkpoint -------------------------------------------------------------

                LockManager.Wait(l2);

                chkpt1 = Checkpoint(qe1);

                LockManager.Signal(l1);
                subTask.Wait();

                chkpt2 = Checkpoint(qe1);

                // Clear state ------------------------------------------------------------

                Crash();
            }

            using (var qe2 = CreateQueryEngine())
            {
                var ctx2 = GetQueryEngineReactiveService(qe2);

                // Recover ----------------------------------------------------------------

                Recover(qe2, chkpt1);

                Assert.IsFalse(ctx2.Subscriptions.ContainsKey(sub1uri));

                var qe3 = CreateQueryEngine();
                var ctx3 = GetQueryEngineReactiveService(qe3);

                Recover(qe3, chkpt2);

                Assert.IsTrue(ctx3.Subscriptions.ContainsKey(sub1uri));
            }
        }

        [TestMethod]
        public async Task RecoverFromTransactionLogAsync()
        {
            var kvs = new InMemoryKeyValueStore();

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe1 = CreateQueryEngine(kvs))
            {
                var ctx1 = new TupletizingClientContext(qe1.ServiceProvider);

                var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                await o1.SubscribeAsync(v1, new Uri("test://sub1"), null, CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe2 = CreateQueryEngine(kvs))
            {
                var ctx2 = GetQueryEngineReactiveService(qe2);

                Recover(qe2, new InMemoryStateStore("foo"));

                ctx2.Subscriptions[new Uri("test://sub1")].Dispose();
            }
        }

        [TestMethod]
        public async Task RecoverFromTransactionLog2Async()
        {
            var kvs = new InMemoryKeyValueStore();

            InMemoryStateStore state;

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe1 = CreateQueryEngine(kvs))
            {
                var ctx1 = new TupletizingClientContext(qe1.ServiceProvider);

                var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                var sub = await o1.SubscribeAsync(v1, new Uri("test://sub1"), null, CancellationToken.None);

                state = Checkpoint(qe1);

                await sub.DisposeAsync(CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe2 = CreateQueryEngine(kvs))
            {
                var ctx2 = GetQueryEngineReactiveService(qe2);

                Recover(qe2, state);

                Assert.ThrowsException<KeyNotFoundException>(() => ctx2.Subscriptions[new Uri("test://sub1")].Dispose());
            }
        }

        [TestMethod]
        public async Task RecoverFromTransactionLog3Async()
        {
            var kvs = new InMemoryKeyValueStore();

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe1 = CreateQueryEngine(kvs))
            {
                var ctx1 = new TupletizingClientContext(qe1.ServiceProvider);

                var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                var sub = await o1.SubscribeAsync(v1, new Uri("test://sub1"), null, CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe2 = CreateQueryEngine(kvs))
            {
                var ctx2 = new TupletizingClientContext(qe2.ServiceProvider);

                Recover(qe2, new InMemoryStateStore("foo"));

                await ctx2.GetSubscription(new Uri("test://sub1")).DisposeAsync(CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe3 = CreateQueryEngine(kvs))
            {
                var ctx3 = new TupletizingClientContext(qe3.ServiceProvider);

                Recover(qe3, new InMemoryStateStore("foo"));

                await Assert.ThrowsExceptionAsync<EntityNotFoundException>(() =>
                    ctx3.GetSubscription(new Uri("test://sub1")).DisposeAsync(CancellationToken.None)
#if NET6_0 || NETCOREAPP3_1
                        .AsTask()
#endif
                );
            }
        }

        [TestMethod]
        public async Task RecoverFromTransactionLog4Async()
        {
            var kvs = new InMemoryKeyValueStore();

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe1 = CreateQueryEngine(kvs))
            {
                var ctx1 = new TupletizingClientContext(qe1.ServiceProvider);

                var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                var sub = await o1.SubscribeAsync(v1, new Uri("test://sub1"), null, CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe2 = CreateQueryEngine(kvs))
            {
                var ctx2 = new TupletizingClientContext(qe2.ServiceProvider);

                Recover(qe2, new InMemoryStateStore("foo"));

                var o1 = ctx2.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx2.GetObserver<string, int>(MockObserverUri)("v1");

                await ctx2.GetSubscription(new Uri("test://sub1")).DisposeAsync(CancellationToken.None);
                await o1.SubscribeAsync(v1, new Uri("test://sub1"), null, CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe3 = CreateQueryEngine(kvs))
            {
                var ctx3 = new TupletizingClientContext(qe3.ServiceProvider);

                Recover(qe3, new InMemoryStateStore("foo"));

                await ctx3.GetSubscription(new Uri("test://sub1")).DisposeAsync(CancellationToken.None);
            }
        }

        [TestMethod]
        public async Task RecoverFromTransactionLog5Async()
        {
            var kvs = new InMemoryKeyValueStore();

            InMemoryStateStore state;

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe1 = CreateQueryEngine(kvs))
            {
                var ctx1 = new TupletizingClientContext(qe1.ServiceProvider);

                var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                var sub = await o1.SubscribeAsync(v1, new Uri("test://sub1"), null, CancellationToken.None);

                state = Checkpoint(qe1);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe2 = CreateQueryEngine(kvs))
            {
                var ctx2 = new TupletizingClientContext(qe2.ServiceProvider);

                Recover(qe2, state);

                var o1 = ctx2.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx2.GetObserver<string, int>(MockObserverUri)("v1");

                await ctx2.GetSubscription(new Uri("test://sub1")).DisposeAsync(CancellationToken.None);
                await o1.SubscribeAsync(v1, new Uri("test://sub1"), null, CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe3 = CreateQueryEngine(kvs))
            {
                var ctx3 = new TupletizingClientContext(qe3.ServiceProvider);

                Recover(qe3, state);

                await ctx3.GetSubscription(new Uri("test://sub1")).DisposeAsync(CancellationToken.None);
            }
        }

        [TestMethod]
        public async Task RecoverFromTransactionLogCoalesceTestAsync()
        {
            var kvs = new InMemoryKeyValueStore();

            InMemoryStateStore state;

            var create_delete_id = new Uri("test://sub1");
            var create_deleteCreate_id = new Uri("test://sub2");
            var delete_create_id = new Uri("test://sub3");
            var deleteCreate_delete_id = new Uri("test://sub4");
            var deleteCreate_deleteCreate_id = new Uri("test://sub5");

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe1 = CreateQueryEngine(kvs))
            {
                var ctx1 = new TupletizingClientContext(qe1.ServiceProvider);

                var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                // Setup existing subs

                var delete_create = await o1.SubscribeAsync(v1, delete_create_id, null, CancellationToken.None);

                var deleteCreate_delete = await o1.SubscribeAsync(v1, deleteCreate_delete_id, null, CancellationToken.None);

                var deleteCreate_deleteCreate = await o1.SubscribeAsync(v1, deleteCreate_deleteCreate_id, null, CancellationToken.None);

                var testId = new Uri("test://test");
                var test = await o1.SubscribeAsync(v1, testId, null, CancellationToken.None);

                state = Checkpoint(qe1);

                await test.DisposeAsync(CancellationToken.None);
                test = await o1.SubscribeAsync(v1, testId, null, CancellationToken.None);
                await test.DisposeAsync(CancellationToken.None);

                // Creates
                var create_delete = await o1.SubscribeAsync(v1, create_delete_id, null, CancellationToken.None);
                var create_deleteCreate = await o1.SubscribeAsync(v1, create_deleteCreate_id, null, CancellationToken.None);

                // Deletes
                await delete_create.DisposeAsync(CancellationToken.None);

                // Delete+Create
                await deleteCreate_delete.DisposeAsync(CancellationToken.None);
                deleteCreate_delete = await o1.SubscribeAsync(v1, deleteCreate_delete_id, null, CancellationToken.None);

                await deleteCreate_deleteCreate.DisposeAsync(CancellationToken.None);
                deleteCreate_deleteCreate = await o1.SubscribeAsync(v1, deleteCreate_deleteCreate_id, null, CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe2 = CreateQueryEngine(kvs))
            {
                var ctx2 = new TupletizingClientContext(qe2.ServiceProvider);

                Recover(qe2, state);

                var o1 = ctx2.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx2.GetObserver<string, int>(MockObserverUri)("v1");

                // Creates
                await o1.SubscribeAsync(v1, delete_create_id, null, CancellationToken.None);

                // Deletes
                await ctx2.GetSubscription(deleteCreate_delete_id).DisposeAsync(CancellationToken.None);

                await ctx2.GetSubscription(create_delete_id).DisposeAsync(CancellationToken.None);

                // Delete+Create
                await ctx2.GetSubscription(deleteCreate_deleteCreate_id).DisposeAsync(CancellationToken.None);
                _ = await o1.SubscribeAsync(v1, deleteCreate_deleteCreate_id, null, CancellationToken.None);

                await ctx2.GetSubscription(create_deleteCreate_id).DisposeAsync(CancellationToken.None);
                _ = await o1.SubscribeAsync(v1, create_deleteCreate_id, null, CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe3 = CreateQueryEngine(kvs))
            {
                _ = new TupletizingClientContext(qe3.ServiceProvider);

                Recover(qe3, state);
            }
        }

        [TestMethod]
        public async Task RecoverFromTransactionLogCorruptionTestAsync()
        {
            var testId = new Uri("test://test");

            // Inconsistency between state store and tx
            {
                InMemoryStateStore state;

                // First create an engine persist a subscription creation in the state store
                var kvs1 = new InMemoryKeyValueStore();

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
                await
#endif
                using (var qe1 = CreateQueryEngine(kvs1))
                {
                    var ctx1 = new TupletizingClientContext(qe1.ServiceProvider);

                    var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                    var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                    var test = await o1.SubscribeAsync(v1, testId, null, CancellationToken.None);

                    state = Checkpoint(qe1);

                    Crash();
                }

                // Then create an engine and persist subscription creation only in kvs

                var kvs2 = new InMemoryKeyValueStore();

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
                await
#endif
                using (var qe2 = CreateQueryEngine(kvs2))
                {
                    var ctx2 = new TupletizingClientContext(qe2.ServiceProvider);

                    var o2 = ctx2.GetObservable<string, int>(MockObservableUri)("o1");
                    var v2 = ctx2.GetObserver<string, int>(MockObserverUri)("v1");

                    await o2.SubscribeAsync(v2, testId, null, CancellationToken.None);

                    Crash();
                }

                // Now create engine with first state store (the one that has sub) and second kvs (also has sub)

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
                await
#endif
                using (var qe3 = CreateQueryEngine(kvs2))
                {
                    AssertEx.ThrowsException<AggregateException>(() => Recover(qe3, state), ex =>
                    {
                        var flattened = ex.Flatten();

                        if (flattened.InnerException is not EntityAlreadyExistsException)
                            Assert.Fail();
                    });
                }

                // Do it again but handle the exception

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
                await
#endif
                using (var qe4 = CreateQueryEngine(kvs2))
                {
                    var handled = false;
                    qe4.EntityReplayFailed += (object sender, ReactiveEntityReplayFailedEventArgs e) =>
                    {
                        if (e.Uri == testId)
                        {
                            e.Handled = true;
                            handled = true;
                        }
                        else
                            Assert.Fail();
                    };

                    Recover(qe4, state);
                    Assert.IsTrue(handled);
                }
            }

            // internal tx inconsistency
            {
                InMemoryStateStore state;

                var kvs = new InMemoryKeyValueStore();

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
                await
#endif
                using (var qe1 = CreateQueryEngine(kvs))
                {
                    var ctx1 = new TupletizingClientContext(qe1.ServiceProvider);
                    state = Checkpoint(qe1);

                    var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                    var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                    await o1.SubscribeAsync(v1, testId, null, CancellationToken.None);
                }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
                await
#endif
                using (var qe2 = CreateQueryEngine(kvs))
                {
                    var ctx2 = new TupletizingClientContext(qe2.ServiceProvider);

                    var o2 = ctx2.GetObservable<string, int>(MockObservableUri)("o1");
                    var v2 = ctx2.GetObserver<string, int>(MockObserverUri)("v1");

                    await o2.SubscribeAsync(v2, testId, null, CancellationToken.None);
                }

                // Unhandled

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
                await
#endif
                using (var qe3 = CreateQueryEngine(kvs))
                {
                    AssertEx.ThrowsException<AggregateException>(() => Recover(qe3, state), ex =>
                    {
                        var flattened = ex.Flatten();

                        if (flattened.InnerException.Message != "Create, Create")
                            Assert.Fail();
                    });
                }

                // Handled

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
                await
#endif
                using (var qe4 = CreateQueryEngine(kvs))
                {
                    var handled = false;
                    qe4.EntityReplayFailed += (object sender, ReactiveEntityReplayFailedEventArgs e) =>
                    {
                        if (e.Uri == testId)
                        {
                            e.Handled = true;
                            handled = true;
                        }
                        else
                            Assert.Fail();
                    };

                    Recover(qe4, state);
                    Assert.IsTrue(handled);
                }
            }
        }

        [TestMethod]
        public async Task TransactionLogGarbageCollectionFailure()
        {
            var id = new Uri("test://sub1");

            var kvs = new InMemoryKeyValueStore();

            InMemoryStateStore state;

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe1 = CreateQueryEngine(kvs))
            {
                var ctx1 = new TupletizingClientContext(qe1.ServiceProvider);

                var o1 = ctx1.GetObservable<string, int>(MockObservableUri)("o1");
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                var sub = await o1.SubscribeAsync(v1, id, null, CancellationToken.None);

                var ex = new Exception();
                kvs.StartingOperation += (e, op) => { if (op.OperationType == OperationType.Remove) { throw ex; } };

                state = Checkpoint(qe1);

                await sub.DisposeAsync(CancellationToken.None);

                Crash();
            }

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe2 = CreateQueryEngine(kvs))
            {
                var ctx2 = GetQueryEngineReactiveService(qe2);

                Recover(qe2, state);

                Assert.IsFalse(qe2.ReactiveService.Subscriptions.ContainsKey(id));
            }
        }

        [TestMethod]
        public async Task DeleteFailureCleanupTestAsync()
        {
            var kvs = new InMemoryKeyValueStore();

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using (var qe = CreateQueryEngine(kvs))
            {
                var ctx = new TupletizingClientContext(qe.ServiceProvider);

                var obs = ctx.GetObservable<string, int>(MockObservableUri)("obs");
                var obv = ctx.GetObserver<string, int>(MockObserverUri)("obv");

                var testId = new Uri("test://test");
                var test = await obs.SubscribeAsync(obv, testId, null, CancellationToken.None);

                var ex = new MyException();
                void fail(object sender, ReifiedOperation<string, byte[]> operation) { throw ex; }

                kvs.StartingOperation += fail;
                await AssertEx.ThrowsExceptionAsync<MyException>(() =>
                    test.DisposeAsync(CancellationToken.None)
#if NET6_0 || NETCOREAPP3_1
                        .AsTask()
#endif
                    ,
                    e => Assert.AreSame(ex, e));

                kvs.StartingOperation -= fail;
                await test.DisposeAsync(CancellationToken.None);
            }
        }

        private sealed class MyException : Exception
        {
        }

        [TestMethod]
        public void CheckpointReliableSubscription()
        {
            var relId = Guid.NewGuid().ToString();

            void AssertActions(ReliableEvent[] events) => AssertEx.AreSequenceEqual(events, ReliableObservableManager.Actions[relId]);

            InMemoryStateStore chkpt1, chkpt2, chkpt3, chkpt4;

            using (var qe1 = CreateQueryEngine())
            {
                var ctx1 = GetQueryEngineReactiveService(qe1);

                var relObsUri = new Uri("test://rel/obs");

                ctx1.DefineObservable<string, int>(
                    relObsUri,
                    s => new MyReliableObservable<int>(s).ToSubscribable().AsQbservable(),
                    null
                );

                var o1 = ctx1.GetObservable<string, int>(relObsUri)(relId);
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                o1.Take(2).Subscribe(v1, new Uri("s:/s1"), null);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(0),
                });

                // Checkpoint -------------------------------------------------------------

                chkpt1 = Checkpoint(qe1);

                // Clear state ------------------------------------------------------------

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(0),
                    ReliableEvent.AcknowledgeRange(-1),
                });

                ReliableObservableManager.Clear(relId);
                Crash();
            }

            using (var qe2 = CreateQueryEngine())
            {
                var ctx2 = GetQueryEngineReactiveService(qe2);

                // Recover ----------------------------------------------------------------

                Recover(qe2, chkpt1);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(0),
                });

                var obs2 = ReliableObservableManager.GetObserver<int>(relId).Synchronize(qe2.Scheduler);
                obs2.OnNext(42, 17);

                // Checkpoint -------------------------------------------------------------

                chkpt2 = Checkpoint(qe2);

                // Clear state ------------------------------------------------------------

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(0),
                    ReliableEvent.AcknowledgeRange(17),
                });

                ReliableObservableManager.Clear(relId);
                Crash();
            }

            using (var qe3 = CreateQueryEngine())
            {
                var ctx3 = GetQueryEngineReactiveService(qe3);

                // Recover ----------------------------------------------------------------

                Recover(qe3, chkpt2);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(18),
                });

                var obs3 = ReliableObservableManager.GetObserver<int>(relId).Synchronize(qe3.Scheduler);
                obs3.OnNext(43, 25);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(18),
                    ReliableEvent.Stop(),
                });

                // Checkpoint -------------------------------------------------------------

                chkpt3 = Checkpoint(qe3);

                // Clear state ------------------------------------------------------------

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(18),
                    ReliableEvent.Stop(),
                    ReliableEvent.AcknowledgeRange(25),
                    ReliableEvent.Dispose(),
                });

                ReliableObservableManager.Clear(relId);
                Crash();
            }

            using (var qe4 = CreateQueryEngine())
            {
                var ctx4 = GetQueryEngineReactiveService(qe4);

                // Recover ----------------------------------------------------------------

                Recover(qe4, chkpt3);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Dispose(),
                });

                // Checkpoint -------------------------------------------------------------

                chkpt4 = Checkpoint(qe4);

                // Clear state ------------------------------------------------------------

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Dispose(),
                });

                ReliableObservableManager.Clear(relId);
                Crash();
            }

            using (var qe5 = CreateQueryEngine())
            {
                var ctx5 = GetQueryEngineReactiveService(qe5);

                // Recover ----------------------------------------------------------------

                Recover(qe5, chkpt4);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Dispose(),
                });
            }
        }

        [TestMethod]
        public void CheckpointReliableSubscription_ContextSwitched()
        {
            var relId = Guid.NewGuid().ToString();

            void AssertActions(ReliableEvent[] events) => AssertEx.AreSequenceEqual(events, ReliableObservableManager.Actions[relId]);

            var l1 = LockManager.NewAutoReset();

            InMemoryStateStore chkpt1, chkpt2, chkpt3, chkpt4;

            using (var qe1 = CreateQueryEngine())
            {
                var ctx1 = GetQueryEngineReactiveService(qe1);

                var relObsUri = new Uri("test://rel/obs");

                ctx1.DefineObservable<string, int>(
                    relObsUri,
                    s => new MyReliableObservable<int>(s).ToSubscribable(true).AsQbservable(),
                    null
                );

                var o1 = ctx1.GetObservable<string, int>(relObsUri)(relId);
                var v1 = ctx1.GetObserver<string, int>(MockObserverUri)("v1");

                o1.AwaitDispose(l1).Take(2).Subscribe(v1, new Uri("s:/s1"), null);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(0),
                });

                // Checkpoint -------------------------------------------------------------

                chkpt1 = Checkpoint(qe1);

                // Clear state ------------------------------------------------------------

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(0),
                    ReliableEvent.AcknowledgeRange(-1),
                });

                ReliableObservableManager.Clear(relId);
                Crash();
            }

            using (var qe2 = CreateQueryEngine())
            {
                var ctx2 = GetQueryEngineReactiveService(qe2);

                // Recover ----------------------------------------------------------------

                Recover(qe2, chkpt1);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(0),
                });

                var obs2 = ReliableObservableManager.GetObserver<int>(relId).Synchronize(qe2.Scheduler);
                obs2.OnNext(42, 17);

                // Checkpoint -------------------------------------------------------------

                chkpt2 = Checkpoint(qe2);

                // Clear state ------------------------------------------------------------

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(0),
                    ReliableEvent.AcknowledgeRange(17),
                });

                ReliableObservableManager.Clear(relId);
                Crash();
            }

            using (var qe3 = CreateQueryEngine())
            {
                var ctx3 = GetQueryEngineReactiveService(qe3);

                // Recover ----------------------------------------------------------------

                Recover(qe3, chkpt2);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(18),
                });

                var obs3 = ReliableObservableManager.GetObserver<int>(relId).Synchronize(qe3.Scheduler);
                obs3.OnNext(43, 25);
                LockManager.Wait(l1);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(18),
                    ReliableEvent.Stop(),
                });

                // Checkpoint -------------------------------------------------------------

                chkpt3 = Checkpoint(qe3);

                // Clear state ------------------------------------------------------------

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Start(18),
                    ReliableEvent.Stop(),
                    ReliableEvent.AcknowledgeRange(25),
                    ReliableEvent.Dispose(),
                });

                ReliableObservableManager.Clear(relId);
                Crash();
            }

            using (var qe4 = CreateQueryEngine())
            {
                var ctx4 = GetQueryEngineReactiveService(qe4);

                // Recover ----------------------------------------------------------------

                Recover(qe4, chkpt3);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Dispose(),
                });

                // Checkpoint -------------------------------------------------------------

                chkpt4 = Checkpoint(qe4);

                // Clear state ------------------------------------------------------------

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Dispose(),
                });

                ReliableObservableManager.Clear(relId);
                Crash();
            }

            using (var qe5 = CreateQueryEngine())
            {
                var ctx5 = GetQueryEngineReactiveService(qe5);

                // Recover ----------------------------------------------------------------

                Recover(qe5, chkpt4);

                AssertActions(new[]
                {
                    ReliableEvent.Subscribe(),
                    ReliableEvent.Dispose(),
                });
            }
        }

        [TestMethod]
        public void ReliableSubscriptionInput_ContextSwitched_OnError()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            var relObsUri = new Uri("test://rel/obs");

            ctx.DefineObservable<string, int>(
                relObsUri,
                s => new MyReliableObservable<int>(s).ToSubscribable(true).AsQbservable(),
                null
            );

            var relId = Guid.NewGuid().ToString();

            var o1 = ctx.GetObservable<string, int>(relObsUri)(relId);
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            var l1 = LockManager.NewManualReset();
            o1.AwaitDo(null, l1).Subscribe(v1, new Uri("s:/s1"), null);

            var obs = ReliableObservableManager.GetObserver<int>(relId);
            obs.OnError(new Exception());

            LockManager.Wait(l1);
        }

        [TestMethod]
        public void ReliableSubscriptionInput_ContextSwitched_OnCompleted()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            var relObsUri = new Uri("test://rel/obs");

            ctx.DefineObservable<string, int>(
                relObsUri,
                s => new MyReliableObservable<int>(s).ToSubscribable(true).AsQbservable(),
                null
            );

            var relId = Guid.NewGuid().ToString();

            var o1 = ctx.GetObservable<string, int>(relObsUri)(relId);
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            var l1 = LockManager.NewManualReset();
            o1.AwaitDo(null, null, l1).Subscribe(v1, new Uri("s:/s1"), null);

            var obs = ReliableObservableManager.GetObserver<int>(relId);
            obs.OnCompleted();

            LockManager.Wait(l1);
        }

        [TestMethod]
        public void CheckpointingQueryEngine_Unload()
        {
            using var qe = CreateQueryEngine();

            qe.UnloadAsync().Wait();

            Assert.ThrowsException<EngineUnloadedException>(() => qe.ReactiveService.Empty<int>());
            Assert.ThrowsException<EngineUnloadedException>(() => qe.ReliableReactiveService.GetObserver<int>(new Uri("test://iv")));
            Assert.ThrowsExceptionAsync<EngineUnloadedException>(() => qe.CheckpointAsync(new InMemoryStateWriter(new InMemoryStateStore("store"), CheckpointKind.Full)));
            Assert.ThrowsExceptionAsync<EngineUnloadedException>(() => qe.RecoverAsync(new InMemoryStateReader(new InMemoryStateStore("store"))));
        }

        [TestMethod]
        public void CreateStreamFromMetadata_AsObservable()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            var sfUri = new Uri("test://streamFactory");
            var streamUri = new Uri("test://stream");
            var upstreamUri = new Uri("test://upstream");
            var downstreamUri = new Uri("test://downstream");
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
            var sf = Context.Provider.CreateQubjectFactory<T, T>((Expression<Func<IReactiveQubject>>)(() => (IReactiveQubject)new SimpleSubject()));
#pragma warning restore IDE0004
            Context.DefineStreamFactory<T, T>(sfUri, sf, null);
            var stream = Context.GetStreamFactory<T, T>(sfUri).Create(streamUri, null);

            var l = LockManager.NewAutoReset();
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            // Unfortunately, the context held by the stream is not the engine's context
            // so we have to use the IReactiveClient.GetObservable<T> interface here.
            ctx.GetObservable<int>(streamUri).AwaitDo(l).Subscribe(v1, downstreamUri, null);
            ctx.Return(42).Subscribe(ctx.GetObserver<int>(streamUri), upstreamUri, null);

            LockManager.Wait(l);
        }

        [TestMethod]
        public void CreateStreamFromMetadata_AsObserver()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            var sfUri = new Uri("test://streamFactory");
            var streamUri = new Uri("test://stream");
            var upstreamUri = new Uri("test://upstream");
            var downstreamUri = new Uri("test://downstream");
            var dummyUri = new Uri("test://dummysub");

#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
            var sf = Context.Provider.CreateQubjectFactory<T, T>((Expression<Func<IReactiveQubject>>)(() => (IReactiveQubject)new SimpleSubject()));
#pragma warning restore IDE0004
            Context.DefineStreamFactory<T, T>(sfUri, sf, null);
            var stream = Context.GetStreamFactory<T, T>(sfUri).Create(streamUri, null);

            var l = LockManager.NewAutoReset();
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            // Unfortunately, the context held by the stream is not the engine's context
            // so we have to use the IReactiveClient.GetObservable<T> interface here.
            ctx.Never<int>().Subscribe(ctx.GetObserver<int>(streamUri), dummyUri, null);
            ctx.GetObservable<int>(streamUri).AwaitDo(l).Subscribe(v1, downstreamUri, null);
            ctx.Return(42).Subscribe(ctx.GetObserver<int>(streamUri), upstreamUri, null);

            LockManager.Wait(l);
        }

        [TestMethod]
        public void CreateStreamFromMetadata_SpecificType()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            var sfUri = new Uri("test://streamFactory");
            var streamUri = new Uri("test://stream");
            var upstreamUri = new Uri("test://upstream");
            var downstreamUri = new Uri("test://downstream");
            var dummyUri = new Uri("test://dummysub");

#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
            var sf = Context.Provider.CreateQubjectFactory<T, T>((Expression<Func<IReactiveQubject>>)(() => (IReactiveQubject)new SimpleSubject()));
#pragma warning restore IDE0004
            Context.DefineStreamFactory<T, T>(sfUri, sf, null);
            var stream = Context.GetStreamFactory<int, int>(sfUri).Create(streamUri, null);

            var l = LockManager.NewAutoReset();
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            // Unfortunately, the context held by the stream is not the engine's context
            // so we have to use the IReactiveClient.GetObservable<T> interface here.
            ctx.Never<int>().Subscribe(stream, dummyUri, null);
            ctx.GetObservable<int>(streamUri).AwaitDo(l).Subscribe(v1, downstreamUri, null);
            ctx.Return(42).Subscribe(stream, upstreamUri, null);

            LockManager.Wait(l);
        }

        [TestMethod]
        public void CreateStreamFromMetadata_SpecificType_SubscribedWithMismatchType_Success()
        {
            using var qe = CreateQueryEngine();

            var ctx = GetQueryEngineReactiveService(qe);

            var sfUri = new Uri("test://streamFactory");
            var streamUri = new Uri("test://stream");
            var upstreamUri = new Uri("test://upstream");
            var downstreamUri = new Uri("test://downstream");
            var dummyUri = new Uri("test://dummysub");

#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
            var sf = Context.Provider.CreateQubjectFactory<T, T>((Expression<Func<IReactiveQubject>>)(() => (IReactiveQubject)new SimpleSubject()));
#pragma warning restore IDE0004
            Context.DefineStreamFactory<T, T>(sfUri, sf, null);

            // Note that the stream is defined over strings, but later subscribed to using int.
            var stream = Context.GetStreamFactory<string, string>(sfUri).Create(streamUri, null);

            var l = LockManager.NewAutoReset();
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            // Unfortunately, the context held by the stream is not the engine's context
            // so we have to use the IReactiveClient.GetObservable<T> interface here.
            ctx.Never<int>().Subscribe(ctx.GetObserver<int>(streamUri), dummyUri, null);
            ctx.GetObservable<int>(streamUri).AwaitDo(l).Subscribe(v1, downstreamUri, null);
            ctx.Return(42).Subscribe(ctx.GetObserver<int>(streamUri), upstreamUri, null);

            LockManager.Wait(l);
        }

        [TestMethod]
        public void CreateStreamFromMetadata_Delete()
        {
            using var qe = CreateQueryEngine();

            var actx = GetQueryEngineAsyncReactiveService(qe);

            var sfUri = new Uri("test://streamFactory");
            var streamUri = new Uri("test://stream");
            var dummyUri = new Uri("test://dummysub");

#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
            var sf = Context.Provider.CreateQubjectFactory<T, T>((Expression<Func<IReactiveQubject>>)(() => (IReactiveQubject)new SimpleSubject()));
#pragma warning restore IDE0004
            Context.DefineStreamFactory<T, T>(sfUri, sf, null);

            var stream = Context.GetStreamFactory<string, string>(sfUri).Create(streamUri, null);

            Assert.IsFalse(actx.Streams.TryGetValue(streamUri, out var p));

            var sub = actx.Never<int>().SubscribeAsync(actx.GetObserver<int>(streamUri), dummyUri, null, CancellationToken.None).Result;

            Assert.IsTrue(actx.Streams.TryGetValue(streamUri, out p));

            sub.DisposeAsync(CancellationToken.None)
#if NET6_0 || NETCOREAPP3_1
                .AsTask()
#endif
                .Wait();

            actx.GetStream<string, string>(streamUri).DisposeAsync(CancellationToken.None)
#if NET6_0 || NETCOREAPP3_1
                .AsTask()
#endif
                .Wait();

            Assert.IsFalse(actx.Streams.TryGetValue(streamUri, out p));
        }

        [TestMethod]
        public async Task Artifact_Creation()
        {
            var trueOrFalse = new object[] { true, false };
            foreach (var choice in new DeterministicRandomizer(new[] { trueOrFalse, trueOrFalse, trueOrFalse, trueOrFalse, trueOrFalse }))
            {
                var kvs = new InMemoryKeyValueStore();
                var qe = CreateQueryEngine(kvs);
                var ctx = new TupletizingClientContext(qe.ServiceProvider);

                var state = Checkpoint(qe);

                Crash();

                qe = CreateQueryEngine(kvs);
                ctx = new TupletizingClientContext(qe.ServiceProvider);

                Recover(qe, state);

                var i = 0;

                await ctx.DefineObservableAsync<T>(new Uri("custom:never"), ctx.Never<T>(), null, CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;

                await ctx.DefineObserverAsync<T>(new Uri("custom:nop"), ctx.Nop<T>(), null, CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;

#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
                var sf = ctx.Provider.CreateQubjectFactory<T, T>((Expression<Func<IAsyncReactiveQubject<T, T>>>)(() => (IAsyncReactiveQubject<T, T>)new SimpleSubject<T>()));
#pragma warning restore IDE0004
                await ctx.DefineStreamFactoryAsync<T, T>(new Uri("custom:sf"), sf, null, CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;

                await ctx.GetObservable<int>(new Uri("custom:never")).SubscribeAsync(ctx.GetObserver<int>(new Uri("custom:nop")), new Uri("custom:sub"), null, CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;

                await ctx.GetStreamFactory<int, int>(new Uri("custom:sf")).CreateAsync(new Uri("custom:stream"), null, CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;
            }
        }

        [TestMethod]
        public async Task Artifact_Deletion()
        {
            var trueOrFalse = new object[] { true, false };
            foreach (var choice in new DeterministicRandomizer(new[] { trueOrFalse, trueOrFalse, trueOrFalse, trueOrFalse, trueOrFalse }))
            {
                var kvs = new InMemoryKeyValueStore();
                var qe = CreateQueryEngine(kvs);
                var ctx = new TupletizingClientContext(qe.ServiceProvider);

                await ctx.DefineObservableAsync<T>(new Uri("custom:never"), ctx.Never<T>(), null, CancellationToken.None);
                await ctx.DefineObserverAsync<T>(new Uri("custom:nop"), ctx.Nop<T>(), null, CancellationToken.None);
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
                var sf = ctx.Provider.CreateQubjectFactory<T, T>((Expression<Func<IAsyncReactiveQubject>>)(() => (IAsyncReactiveQubject)new SimpleSubject()));
#pragma warning restore IDE0004
                await ctx.DefineStreamFactoryAsync<T, T>(new Uri("custom:sf"), sf, null, CancellationToken.None);
                var sub = await ctx.GetObservable<int>(new Uri("custom:never")).SubscribeAsync(ctx.GetObserver<int>(new Uri("custom:nop")), new Uri("custom:sub"), null, CancellationToken.None);
                var subj = await sf.CreateAsync(new Uri("custom:stream"), null, CancellationToken.None);

                var state = Checkpoint(qe);

                Crash();

                qe = CreateQueryEngine(kvs);
                ctx = new TupletizingClientContext(qe.ServiceProvider);

                Recover(qe, state);

                var i = 0;

                await ctx.GetStream<int, int>(new Uri("custom:stream")).DisposeAsync(CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;

                await ctx.GetSubscription(new Uri("custom:sub")).DisposeAsync(CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;

                await ctx.UndefineStreamFactoryAsync(new Uri("custom:sf"), CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;

                await ctx.UndefineObserverAsync(new Uri("custom:nop"), CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;

                await ctx.UndefineObservableAsync(new Uri("custom:never"), CancellationToken.None);

                if ((bool)choice[i])
                {
                    qe = CrashAndRecreate(qe, kvs);
                    ctx = new TupletizingClientContext(qe.ServiceProvider);
                }
                i++;
            }
        }

        private sealed class DeterministicRandomizer : IEnumerable<IList<object>> // can be tuple based for stricter typing/avoiding boxing
        {
            private readonly IList<IList<object>> _ranges;

            private readonly int[] _current;

            public DeterministicRandomizer(IList<IList<object>> ranges)
            {
                foreach (var range in ranges)
                    if (range.Count == 0)
                        throw new ArgumentException("Ranges should not be empty.", nameof(ranges));

                _ranges = ranges;
                _current = new int[ranges.Count];
            }

            public bool MoveNext()
            {
                var carry = true;
                for (var i = 0; carry && i < _ranges.Count; i++)
                {
                    var curr = _current[i];
                    curr++;
                    if (curr >= _ranges[i].Count)
                    {
                        curr = 0;
                        carry = true;
                    }
                    else
                        carry = false;

                    _current[i] = curr;
                }

                return !carry;
            }

            public IList<object> Current
            {
                get
                {
                    var ret = new object[_current.Length];
                    for (var i = 0; i < ret.Length; i++)
                        ret[i] = _ranges[i][_current[i]];
                    return ret;
                }
            }

            public IEnumerator<IList<object>> GetEnumerator()
            {
                var done = false;
                while (!done)
                {
                    var ret = new object[_current.Length];
                    for (var i = 0; i < ret.Length; i++)
                        ret[i] = _ranges[i][_current[i]];

                    yield return ret;

                    var carry = true;
                    for (var i = 0; carry && i < _ranges.Count; i++)
                    {
                        var curr = _current[i];
                        curr++;
                        if (curr >= _ranges[i].Count)
                        {
                            curr = 0;
                            carry = true;
                        }
                        else
                            carry = false;

                        _current[i] = curr;
                    }

                    done = carry;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [TestMethod]
        public async Task Metadata_AsyncProvider()
        {
            var kvs = new InMemoryKeyValueStore();

#if NET6_0 || NETCOREAPP3_1 // NB: Only using ValueTask-based DisposeAsync in .NET Standard 2.1 and beyond at the moment.
            await
#endif
            using var qe = CreateQueryEngine(kvs);

            var ctx = new TupletizingClientContext(qe.ServiceProvider);
            var syncCtx = GetQueryEngineReactiveService(qe);

            await ctx.DefineObservableAsync<T>(new Uri("custom:never"), ctx.Never<T>(), null, CancellationToken.None);
            await ctx.DefineObserverAsync<T>(new Uri("custom:nop"), ctx.Nop<T>(), null, CancellationToken.None);
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
            var sf = ctx.Provider.CreateQubjectFactory<T, T>((Expression<Func<IAsyncReactiveQubject>>)(() => (IAsyncReactiveQubject)new SimpleSubject()));
#pragma warning restore IDE0004
            await ctx.DefineStreamFactoryAsync<T, T>(new Uri("custom:sf"), sf, null, CancellationToken.None);
            await ctx.GetObservable<int>(new Uri("custom:never")).SubscribeAsync(ctx.GetObserver<int>(new Uri("custom:nop")), new Uri("custom:sub"), null, CancellationToken.None);
            await sf.CreateAsync(new Uri("custom:stream"), null, CancellationToken.None);

            Assert.AreSame(ctx.Observables[new Uri("custom:never")].Expression, syncCtx.Observables[new Uri("custom:never")].Expression);
            Assert.AreSame(ctx.Observers[new Uri("custom:nop")].Expression, syncCtx.Observers[new Uri("custom:nop")].Expression);
            Assert.AreSame(ctx.StreamFactories[new Uri("custom:sf")].Expression, syncCtx.StreamFactories[new Uri("custom:sf")].Expression);
            Assert.AreSame(ctx.Subscriptions[new Uri("custom:sub")].Expression, syncCtx.Subscriptions[new Uri("custom:sub")].Expression);
            Assert.AreSame(ctx.Streams[new Uri("custom:stream")].Expression, syncCtx.Streams[new Uri("custom:stream")].Expression);

            // Not implemented yet
            Assert.ThrowsException<NotImplementedException>(() => ctx.Observables[new Uri("custom:never")].ToObservable<T>());
            Assert.ThrowsException<NotImplementedException>(() => ctx.Observables[new Uri("custom:never")].ToObservable<T1, T>());
            Assert.ThrowsException<NotImplementedException>(() => ctx.Observers[new Uri("custom:nop")].ToObserver<T>());
            Assert.ThrowsException<NotImplementedException>(() => ctx.Observers[new Uri("custom:nop")].ToObserver<T1, T>());
            Assert.ThrowsException<NotImplementedException>(() => ctx.StreamFactories[new Uri("custom:sf")].ToStreamFactory<T, R>());
            Assert.ThrowsException<NotImplementedException>(() => ctx.StreamFactories[new Uri("custom:sf")].ToStreamFactory<T1, T, R>());
            Assert.ThrowsException<NotImplementedException>(() => ctx.Subscriptions[new Uri("custom:sub")].ToSubscription());
            Assert.ThrowsException<NotImplementedException>(() => ctx.Streams[new Uri("custom:stream")].ToStream<T, R>());

            var l = syncCtx.Observables.AsEnumerable().Select(kvp => kvp.Key).ToList();
            CollectionAssert.AreEquivalent(ctx.Observables.AsEnumerable().Select(kvp => kvp.Key).ToList(), syncCtx.Observables.AsEnumerable().Select(kvp => kvp.Key).ToList());
            CollectionAssert.AreEquivalent(ctx.Observers.AsEnumerable().Select(kvp => kvp.Key).ToList(), syncCtx.Observers.AsEnumerable().Select(kvp => kvp.Key).ToList());
            CollectionAssert.AreEquivalent(ctx.StreamFactories.AsEnumerable().Select(kvp => kvp.Key).ToList(), syncCtx.StreamFactories.AsEnumerable().Select(kvp => kvp.Key).ToList());
            CollectionAssert.AreEquivalent(ctx.Subscriptions.AsEnumerable().Select(kvp => kvp.Key).ToList(), syncCtx.Subscriptions.AsEnumerable().Select(kvp => kvp.Key).ToList());
            CollectionAssert.AreEquivalent(ctx.Streams.AsEnumerable().Select(kvp => kvp.Key).ToList(), syncCtx.Streams.AsEnumerable().Select(kvp => kvp.Key).ToList());
        }

        private CheckpointingQueryEngine CrashAndRecreate(CheckpointingQueryEngine qe, IKeyValueStore keyValueStore)
        {
            InMemoryStateStore state;
            List<Uri>[] artifacts;

            using (qe)
            {
                state = Checkpoint(qe);

                artifacts = new List<Uri>[]
                {
                    qe.ReactiveService.StreamFactories.Select(artifact => artifact.Key).ToList(),
                    qe.ReactiveService.Observables.Select(artifact => artifact.Key).ToList(),
                    qe.ReactiveService.Observers.Select(artifact => artifact.Key).ToList(),
                    qe.ReactiveService.Streams.Select(artifact => artifact.Key).ToList(),
                    qe.ReactiveService.Subscriptions.Select(artifact => artifact.Key).ToList(),
                };

                Crash();
            }

            var newQe = CreateQueryEngine(keyValueStore);

            Recover(newQe, state);

            var recoveredArtifacts = new List<Uri>[]
            {
                newQe.ReactiveService.StreamFactories.Select(artifact => artifact.Key).ToList(),
                newQe.ReactiveService.Observables.Select(artifact => artifact.Key).ToList(),
                newQe.ReactiveService.Observers.Select(artifact => artifact.Key).ToList(),
                newQe.ReactiveService.Streams.Select(artifact => artifact.Key).ToList(),
                newQe.ReactiveService.Subscriptions.Select(artifact => artifact.Key).ToList(),
            };

            for (var i = 0; i < artifacts.Length; i++)
                CollectionAssert.AreEquivalent(artifacts[i], recoveredArtifacts[i]);

            return newQe;
        }

#pragma warning restore IDE0063 // 'using' statement can be simplified
#pragma warning restore IDE0079 // Remove unnecessary suppression

        private sealed class ExpressionComparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && x.Type == y.Type;
            }
        }

        private static Expression Rewrite(Expression expr)
        {
            var rewrite = new SubstituteAndUnquoteRewriter(
                new Dictionary<Type, Type>
                {
                    { typeof(IReactiveQubjectFactory<,>), typeof(IReliableSubjectFactory<,>) },
                    { typeof(IReactiveSubjectFactory<,>), typeof(IReliableSubjectFactory<,>) },
                    { typeof(IReactiveQubject),           typeof(IMultiSubject)              },
                    { typeof(IReactiveQubject<,>),        typeof(IReliableMultiSubject<,>)   },
                    { typeof(IReactiveSubject<,>),        typeof(IReliableMultiSubject<,>)   },
                    { typeof(IReactiveQbservable<>),      typeof(ISubscribable<>)            },
                    { typeof(IReactiveObservable<>),      typeof(ISubscribable<>)            },
                    { typeof(IReactiveQbserver<>),        typeof(IObserver<>)                },
                    { typeof(IReactiveObserver<>),        typeof(IObserver<>)                },
                    { typeof(IReactiveQubscription),      typeof(ISubscription)              },
                    { typeof(IReactiveSubscription),      typeof(ISubscription)              },
                    { typeof(ReactiveQbservable),         typeof(Subscribable)               },
                    { typeof(ReactiveQbserver),           typeof(Observer)                   },
                    { typeof(ReactiveQubjectFactory),     typeof(ReliableSubjectFactory)     },
                });

            var asyncToSyncRewrite = new AsyncToSyncRewriter(new Dictionary<Type, Type>()).Rewrite(expr);
            var idRewrite = new IdRewriter().Visit(asyncToSyncRewrite);
            return rewrite.Apply(idRewrite);
        }

        private sealed class IdRewriter : ExpressionVisitor
        {
            private static readonly MethodInfo s_asQbserver = ((MethodInfo)ReflectionHelpers.InfoOf((IObserver<int> obv) => obv.AsQbserver())).GetGenericMethodDefinition();
            private static readonly MethodInfo s_asQbservable = ((MethodInfo)ReflectionHelpers.InfoOf((IObservable<int> obs) => obs.AsQbservable())).GetGenericMethodDefinition();

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.IsGenericMethod && node.Method.GetGenericMethodDefinition() == s_asQbserver)
                {
                    var f = typeof(Func<,>).MakeGenericType(typeof(IObserver<>).MakeGenericType(node.Method.GetGenericArguments()), typeof(IReactiveQbserver<>).MakeGenericType(node.Method.GetGenericArguments()));
                    return Expression.Invoke(Expression.Parameter(f, "rx:/observer/id"), node.Arguments);
                }

                if (node.Method.IsGenericMethod && node.Method.GetGenericMethodDefinition() == s_asQbservable)
                {
                    var f = typeof(Func<,>).MakeGenericType(typeof(IObservable<>).MakeGenericType(node.Method.GetGenericArguments()), typeof(IReactiveQbservable<>).MakeGenericType(node.Method.GetGenericArguments()));
                    return Expression.Invoke(Expression.Parameter(f, "rx:/observable/id"), node.Arguments);
                }

                return base.VisitMethodCall(node);
            }
        }

        private class SimpleSubject : IMultiSubject
        {
            private readonly object _gate = new();

            private object _singleton;

            public IObserver<T> GetObserver<T>()
            {
                return GetSubject<T>();
            }

            public ISubscribable<T> GetObservable<T>()
            {
                return GetSubject<T>().ToSubscribable<T>();
            }

            private Subject<T> GetSubject<T>()
            {
                if (_singleton == null)
                {
                    lock (_gate)
                    {
                        _singleton ??= new Subject<T>();
                    }
                }

                return (Subject<T>)_singleton;
            }

            public void Dispose()
            {
                if (_singleton != null)
                {
                    lock (_gate)
                    {
                        if (_singleton is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }

                        _singleton = null;
                    }
                }
            }
        }

        private class SimpleSubject<T> : IMultiSubject<T>, IReliableMultiSubject<T>
        {
            public IObserver<T> CreateObserver() => throw new NotImplementedException();

            public void Dispose() => throw new NotImplementedException();

            public ISubscription Subscribe(IObserver<T> observer) => throw new NotImplementedException();

            public IReliableSubscription Subscribe(IReliableObserver<T> observer) => throw new NotImplementedException();

            IReliableObserver<T> IReliableMultiSubject<T, T>.CreateObserver() => throw new NotImplementedException();

            IDisposable IObservable<T>.Subscribe(IObserver<T> observer) => throw new NotImplementedException();
        }

        private static class ReliableObservableManager
        {
            public static readonly Dictionary<string, List<ReliableEvent>> Actions = new();
            public static readonly Dictionary<string, object> Observers = new();

            public static IReliableObserver<T> GetObserver<T>(string s)
            {
                return (IReliableObserver<T>)Observers[s];
            }

            public static void Clear(string id)
            {
                Actions.Remove(id);
                Observers.Remove(id);
            }
        }

        private sealed class ReliableEvent
        {
            private ReliableEventType EventType
            {
                get;
                set;
            }

            private long SequenceId
            {
                get;
                set;
            }

            public static ReliableEvent AcknowledgeRange(long sequenceId)
            {
                return new ReliableEvent { EventType = ReliableEventType.AcknowledgeRange, SequenceId = sequenceId };
            }

            public static ReliableEvent Dispose()
            {
                return new ReliableEvent { EventType = ReliableEventType.Dispose };
            }

            public static ReliableEvent Start(long sequenceId)
            {
                return new ReliableEvent { EventType = ReliableEventType.Start, SequenceId = sequenceId };
            }

            public static ReliableEvent Stop()
            {
                return new ReliableEvent { EventType = ReliableEventType.Stop };
            }

            public static ReliableEvent Subscribe()
            {
                return new ReliableEvent { EventType = ReliableEventType.Subscribe };
            }

            public override bool Equals(object obj)
            {
                return obj is ReliableEvent other && other.EventType == EventType && other.SequenceId == SequenceId;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return EventType.GetHashCode() * 17 + SequenceId.GetHashCode();
                }
            }

            public override string ToString()
            {
                return EventType switch
                {
                    ReliableEventType.AcknowledgeRange => $"AcknowledgeRange({SequenceId})",
                    ReliableEventType.Dispose => "Dispose()",
                    ReliableEventType.Start => $"Start({SequenceId})",
                    ReliableEventType.Stop => "Stop()",
                    ReliableEventType.Subscribe => "Subscribe()",
                    _ => base.ToString(),
                };
            }

            private enum ReliableEventType
            {
                AcknowledgeRange,
                Dispose,
                Start,
                Stop,
                Subscribe,
            }
        }

        private sealed class MyReliableObservable<T> : IReliableObservable<T>
        {
            private readonly string _s;
            private readonly List<ReliableEvent> _actions;

            public MyReliableObservable(string s)
            {
                _s = s;
                _actions = new List<ReliableEvent>();

                ReliableObservableManager.Actions.Add(s, _actions);
            }

            public IReliableSubscription Subscribe(IReliableObserver<T> observer)
            {
                _actions.Add(ReliableEvent.Subscribe());
                ReliableObservableManager.Observers[_s] = observer;

                return new Sub(this);
            }

            private sealed class Sub : ReliableSubscriptionBase
            {
                private readonly MyReliableObservable<T> _parent;

                public Sub(MyReliableObservable<T> parent)
                {
                    _parent = parent;
                }

                public override Uri ResubscribeUri => throw new NotImplementedException();

                public override void Start(long sequenceId)
                {
                    _parent._actions.Add(ReliableEvent.Start(sequenceId));
                }

                public override void AcknowledgeRange(long sequenceId)
                {
                    _parent._actions.Add(ReliableEvent.AcknowledgeRange(sequenceId));
                }

                protected override void Stop()
                {
                    base.Stop();

                    _parent._actions.Add(ReliableEvent.Stop());
                }

                public override void DisposeCore()
                {
                    _parent._actions.Add(ReliableEvent.Dispose());
                }
            }
        }
    }

    internal static class Helpers
    {
        public static Task AsTask(this Task t) => t;
    }
}
