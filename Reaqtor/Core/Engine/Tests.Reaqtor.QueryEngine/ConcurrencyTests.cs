// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ConcurrencyTests : PhysicalTimeEngineTest
    {
        private const int SubCount = 1000;
        private const int AddedSubCount = 100;
        private const int QeCount = 10;
        private const int Parallelism = 8;

        private const string NopId = "rx://observer/nop";

        private static readonly Uri NopUri = new(NopId);

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
        public void SingleQueryEngine_Concurrency_CreateSubscription()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);
            var subBase = "test://sub/";

            var subUris = Enumerable.Range(0, SubCount).Select(i => new Uri(subBase + i)).ToList();

            Parallel.ForEach(
                subUris,
                uri => ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                    .Subscribe(ctx.GetObserver<int>(NopUri), uri, null));

            foreach (var uri in subUris)
            {
                Assert.IsTrue(qe.ReactiveService.Subscriptions.TryGetValue(uri, out _));
            }
        }

        [TestMethod]
        public void SingleQueryEngine_Concurrency_CreateSubscription_HigherOrder()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);
            var subBase = "test://sub/";

            var subUris = Enumerable.Range(0, SubCount).Select(i => new Uri(subBase + i)).ToList();

            Parallel.ForEach(
                subUris,
                uri => ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                    .Subscribe(ctx.GetObserver<int>(NopUri), uri, null));

            foreach (var uri in subUris)
            {
                Assert.IsTrue(qe.ReactiveService.Subscriptions.TryGetValue(uri, out _));
            }
        }

        [TestMethod]
        public void SingleQueryEngine_Concurrency_Checkpoint_Recovery()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);
            var subBase = "test://sub/";

            var subUris = Enumerable.Range(0, SubCount).Select(i => new Uri(subBase + i)).ToList();

            Parallel.ForEach(
                subUris,
                uri => ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                    .Subscribe(ctx.GetObserver<int>(NopUri), uri, null));

            qe.Options.CheckpointDegreeOfParallelism = Parallelism;
            var store = Checkpoint(qe);

            RemoveQueryEngine(qe);

            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);
            qe.Options.RecoveryDegreeOfParallelism = Parallelism;

            Recover(qe, store);

            foreach (var uri in subUris)
            {
                Assert.IsTrue(qe.ReactiveService.Subscriptions.TryGetValue(uri, out _));
            }
        }

        [TestMethod]
        public void SingleQueryEngine_Concurrency_CreateDuringCheckpoint()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);
            var subBase = "test://sub/";

            var subUris = Enumerable.Range(0, SubCount).Select(i => new Uri(subBase + i)).ToList();
            var extraSubs = Enumerable.Range(SubCount, AddedSubCount).Select(i => new Uri(subBase + i)).ToList();

            Parallel.ForEach(
                subUris,
                uri => ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                    .Subscribe(ctx.GetObserver<int>(NopUri), uri, null));

            var onStart = new AutoResetEvent(false);
            var beforeCompleted = new AutoResetEvent(false);
            var onSubscribed = new AutoResetEvent(false);
            var onCompleted = new AutoResetEvent(false);

            qe.SchedulerPaused += (s, a) => onStart.Set();
            qe.SchedulerContinued += (s, a) => onSubscribed.WaitOne();

            qe.Options.CheckpointDegreeOfParallelism = Parallelism;
            var store = new InMemoryStateStore(Guid.NewGuid().ToString());
            Task.Run(async () =>
            {
                await qe.CheckpointAsync(new InMemoryStateWriter(store, CheckpointKind.Full));
                onCompleted.Set();
            });

            onStart.WaitOne();
            Parallel.ForEach(
                extraSubs,
                uri =>
                {
                    ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                        .Subscribe(ctx.GetObserver<int>(NopUri), uri, null);
                    onSubscribed.Set();
                });
            onCompleted.WaitOne();

            foreach (var uri in subUris.Concat(extraSubs))
            {
                Assert.IsTrue(qe.ReactiveService.Subscriptions.TryGetValue(uri, out _));
            }
        }

        [TestMethod]
        public void SingleQueryEngine_Concurrency_CreateDuringRecovery()
        {
            var qe = CreateQueryEngine();
            var ctx = GetQueryEngineReactiveService(qe);
            var subBase = "test://sub/";

            var subUris = Enumerable.Range(0, SubCount).Select(i => new Uri(subBase + i)).ToList();
            var extraSubs = Enumerable.Range(SubCount, AddedSubCount).Select(i => new Uri(subBase + i)).ToList();

            Parallel.ForEach(
                subUris,
                uri => ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                    .Subscribe(ctx.GetObserver<int>(NopUri), uri, null));

            qe.Options.CheckpointDegreeOfParallelism = Parallelism;
            var store = Checkpoint(qe);

            RemoveQueryEngine(qe);

            qe = CreateQueryEngine();
            ctx = GetQueryEngineReactiveService(qe);
            qe.Options.RecoveryDegreeOfParallelism = Parallelism;

            var onCompleted = new AutoResetEvent(false);
            var onSubscribed = new AutoResetEvent(false);
            var onStarted = new AutoResetEvent(false);

            qe.SchedulerPaused += (s, a) => onStarted.Set();
            qe.SchedulerContinuing += (s, a) => onSubscribed.WaitOne();

            Task.Run(async () =>
            {
                await qe.RecoverAsync(new InMemoryStateReader(store));
                onCompleted.Set();
            });

            onStarted.WaitOne();
            Parallel.ForEach(
                extraSubs,
                uri =>
                {
                    ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                        .Subscribe(ctx.GetObserver<int>(NopUri), uri, null);

                    onSubscribed.Set();
                });
            onCompleted.WaitOne();

            foreach (var uri in subUris.Concat(extraSubs))
            {
                Assert.IsTrue(qe.ReactiveService.Subscriptions.TryGetValue(uri, out _));
            }
        }

        [TestMethod]
        public async Task MultipleQueryEngine_Concurrency_CreateSubscription()
        {
            {
                var qes = Enumerable.Range(0, QeCount).Select(i => CreateQueryEngine()).ToList();
                var ctxs = qes.Select(qe => GetQueryEngineReactiveService(qe)).ToList();

                var subBase = "test://sub/";

                var subUris = Enumerable.Range(0, SubCount / QeCount).Select(i => new Uri(subBase + i)).ToList();

                foreach (var uri in subUris)
                {
                    Parallel.ForEach(
                        ctxs,
                        ctx => ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                            .Subscribe(ctx.GetObserver<int>(NopUri), uri, null));
                }

                foreach (var qe in qes)
                {
                    foreach (var uri in subUris)
                    {
                        Assert.IsTrue(qe.ReactiveService.Subscriptions.TryGetValue(uri, out _));
                    }
                }
            }

            {
                var qes = Enumerable.Range(0, QeCount).Select(i => CreateQueryEngine()).ToList();
                var ctxs = qes.Select(qe => GetQueryEngineAsyncReactiveService(qe)).ToList();

                var subBase = "test://sub/";

                var subUris = Enumerable.Range(0, SubCount / QeCount).Select(i => new Uri(subBase + i)).ToList();

                foreach (var uri in subUris)
                {
                    await Task.WhenAll(ctxs.Select(
                        ctx => ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                            .SubscribeAsync(ctx.GetObserver<int>(NopUri), uri, null, CancellationToken.None)));
                }

                foreach (var qe in qes)
                {
                    foreach (var uri in subUris)
                    {
                        Assert.IsTrue(qe.ReactiveService.Subscriptions.TryGetValue(uri, out _));
                    }
                }
            }
        }

        [TestMethod]
        public void MultipleQueryEngine_Concurrency_Checkpoint_Recovery()
        {
            var qes = Enumerable.Range(0, QeCount).Select(_ => CreateQueryEngine()).ToList();
            var ctxs = qes.Select(qe => GetQueryEngineReactiveService(qe)).ToList();

            var subBase = "test://sub/";

            var subUris = Enumerable.Range(0, SubCount / QeCount).Select(i => new Uri(subBase + i)).ToList();

            foreach (var uri in subUris)
            {
                Parallel.ForEach(
                    ctxs,
                    ctx => ctx.Never<int>().Where(x => x > 0).Select(x => x * x)
                        .Subscribe(ctx.GetObserver<int>(NopUri), uri, null));
            }

            qes.ForEach(qe => qe.Options.CheckpointDegreeOfParallelism = Parallelism);
            var stores = Enumerable.Range(0, QeCount).Select(_ => new InMemoryStateStore(Guid.NewGuid().ToString())).ToList();
            var chkptTasks = Enumerable.Range(0, QeCount).Select(i => qes[i].CheckpointAsync(new InMemoryStateWriter(stores[i], CheckpointKind.Full)));

            Task.WaitAll(chkptTasks.ToArray());

            RemoveAllQueryEngines();

            qes = Enumerable.Range(0, QeCount).Select(_ => CreateQueryEngine()).ToList();
            ctxs = qes.Select(qe => GetQueryEngineReactiveService(qe)).ToList();
            qes.ForEach(qe => qe.Options.RecoveryDegreeOfParallelism = Parallelism);

            var rcvryTasks = Enumerable.Range(0, QeCount).Select(i => qes[i].RecoverAsync(new InMemoryStateReader(stores[i])));

            Task.WaitAll(rcvryTasks.ToArray());

            foreach (var qe in qes)
            {
                foreach (var uri in subUris)
                {
                    Assert.IsTrue(qe.ReactiveService.Subscriptions.TryGetValue(uri, out _));
                }
            }
        }
    }
}
