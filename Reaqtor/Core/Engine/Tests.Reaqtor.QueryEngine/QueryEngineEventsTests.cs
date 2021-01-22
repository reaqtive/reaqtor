// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Threading;

using Reaqtor;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.Events;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class QueryEngineEventsTests : PhysicalTimeEngineTest
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

        [TestMethod]
        public void CheckpointingQueryEngine_Events_Scheduler()
        {
            var qe1 = CreateQueryEngine();
            var schedulerPausingCount = 0;
            var schedulerPausedCount = 0;
            var schedulerContinuingCount = 0;
            var schedulerContinuedCount = 0;

            qe1.SchedulerPausing += (sender, e) => schedulerPausingCount++;
            qe1.SchedulerPaused += (sender, e) => schedulerPausedCount++;
            qe1.SchedulerContinuing += (sender, e) => schedulerContinuingCount++;
            qe1.SchedulerContinued += (sender, e) => schedulerContinuedCount++;

            var stateStore = Checkpoint(qe1);

            Assert.AreEqual(schedulerPausingCount, 1);
            Assert.AreEqual(schedulerPausedCount, 1);
            Assert.AreEqual(schedulerContinuingCount, 1);
            Assert.AreEqual(schedulerContinuedCount, 1);
        }

        [TestMethod]
        public void CheckpointingQueryEngine_Events_CreateSubscription()
        {
            var qe1 = CreateQueryEngine();
            var subscriptionCreatedCount = 0;
            var subscriptionDeletedCount = 0;

            qe1.EntityCreated += (sender, e) =>
            {
                if (e.EntityType == ReactiveEntityKind.Subscription)
                {
                    subscriptionCreatedCount++;
                }
            };

            qe1.EntityDeleted += (sender, e) =>
            {
                if (e.EntityType == ReactiveEntityKind.Subscription)
                {
                    subscriptionDeletedCount++;
                }
            };

            var ctx = qe1.ReactiveService;
            var io = ctx.GetObservable<int>(EmptyObservableUri);
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");
            var sub = io.Subscribe(v1, new Uri("eg://foo"), null);
            sub.Dispose();

            Assert.AreEqual(subscriptionCreatedCount, 1);
            Assert.AreEqual(subscriptionDeletedCount, 1);
        }

        [TestMethod]
        public void CheckpointingQueryEngine_Events_CreateSubscriptionHigherOrder()
        {
            var defaultVersion = BridgeVersion.Version;
            BridgeVersion.Version = Versioning.v1;

            var qe1 = CreateQueryEngine();

            var subscriptionCreatedCount = new CountdownEvent(2);
            var subscriptionDeletedCount = new CountdownEvent(2);
            var observableDefinedCount = new CountdownEvent(1);
            var observableUndefinedCount = new CountdownEvent(1);
            var streamCreatedCount = new CountdownEvent(1);
            var streamDeletedCount = new CountdownEvent(1);

            qe1.EntityDefined += (sender, e) =>
            {
                switch (e.EntityType)
                {
                    case ReactiveEntityKind.Observable:
                        observableDefinedCount.Signal();
                        break;
                }
            };

            qe1.EntityUndefined += (sender, e) =>
            {
                switch (e.EntityType)
                {
                    case ReactiveEntityKind.Observable:
                        observableUndefinedCount.Signal();
                        break;
                }
            };

            qe1.EntityCreated += (sender, e) =>
            {
                switch (e.EntityType)
                {
                    case ReactiveEntityKind.Subscription:
                        subscriptionCreatedCount.Signal();
                        break;
                    case ReactiveEntityKind.Stream:
                        streamCreatedCount.Signal();
                        break;
                }
            };

            qe1.EntityDeleted += (sender, e) =>
            {
                switch (e.EntityType)
                {
                    case ReactiveEntityKind.Subscription:
                        subscriptionDeletedCount.Signal();
                        break;
                    case ReactiveEntityKind.Stream:
                        streamDeletedCount.Signal();
                        break;
                }
            };

            var ctx = qe1.ReactiveService;
            var innerEmpty = ctx.GetObservable<int>(EmptyObservableUri);
            var io = ctx.GetObservable<IReactiveQbservable<int>>(EmptyObservableUri);
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");
            var sub = io.StartWith(innerEmpty).SelectMany(x => x, (x, y) => y).Subscribe(v1, new Uri("eg://foo"), null);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(30000);

            subscriptionCreatedCount.Wait(cts.Token);
            streamCreatedCount.Wait(cts.Token);

            sub.Dispose();

            subscriptionDeletedCount.Wait(cts.Token);
            streamDeletedCount.Wait(cts.Token);

            BridgeVersion.Version = defaultVersion;
        }

        [TestMethod]
        public void CheckpointingQueryEngine_Events_EntitySaveFailed()
        {
            var testObservableUri = new Uri("rx://test/observable/fail");
            var testSubscriptionUri = new Uri("rx://test/subscription");

            Expression<Func<IReactiveQbservable<int>>> observableFactory = () => new SaveFailureOperator().AsQbservable();
            var observable = Context.Provider.CreateQbservable<int>(observableFactory.Body);
            Context.DefineObservable<int>(testObservableUri, observable, null);

            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);
            var v1 = ctx.GetObserver<string, int>(MockObserverUri)("v1");

            ctx.GetObservable<int>(testObservableUri).Subscribe(v1, testSubscriptionUri, null);

            var saveFailureCount = 0;

            void handled(object sender, ReactiveEntitySaveFailedEventArgs args)
            {
                saveFailureCount++;
                Assert.AreEqual(testSubscriptionUri, args.Entity.Uri);
                args.Handled = true;
            }

            void unhandled(object sender, ReactiveEntitySaveFailedEventArgs args)
            {
                saveFailureCount++;
                Assert.AreEqual(testSubscriptionUri, args.Entity.Uri);
            }

            qe.EntitySaveFailed += handled;

            Checkpoint(qe);
            Assert.AreEqual(1, saveFailureCount);

            qe.EntitySaveFailed -= handled;

            qe.EntitySaveFailed += unhandled;

            AssertEx.ThrowsException<AggregateException>(() => Checkpoint(qe), ex =>
            {
                var entitySaveFailedException = ex.Flatten().InnerException as EntitySaveFailedException;
                Assert.IsNotNull(entitySaveFailedException);
                Assert.IsTrue(entitySaveFailedException.InnerException is SaveFailureOperator.TestException);
            });

            Assert.AreEqual(2, saveFailureCount);
        }
    }
}
