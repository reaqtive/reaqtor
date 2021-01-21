// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Reaqtive;

using Reaqtor;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class PerformanceTests : PhysicalTimeEngineTest
    {
        #region Test Setup

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _ = testContext;

            PhysicalTimeEngineTest.ClassSetup();
        }

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

        protected override void AddCommonDefinitions(IReactive ctx)
        {
            base.AddCommonDefinitions(ctx);

            ctx.DefineObservable<Tuple<IReactiveQbservable<T>>, T>(new Uri("rx://observable/subscribeonscheduler"), t => TestQbservable.SubscribeOnInternalScheduler(t.Item1), null);
        }

        #endregion

        [TestMethod]
        [TestCategory("Performance")]
        [Ignore, Description("Please, use it for profiling.")]
        public void HundredQeWithThousandSubscriptions()
        {
            const int NumberOfEngines = 100;
            const int NumberOfSubscriptions = 1000;

            // Creating engines.
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var engines = new List<ICheckpointingQueryEngine>();
            var tasks = new List<Task>();
            var sources = new ConcurrentQueue<MockObservable<int>>();
            for (int i = 0; i < NumberOfEngines; ++i)
            {
                var engine = CreateQueryEngine();
                engines.Add(engine);
                tasks.Add(new TaskFactory().StartNew(() => AddSubscriptions(engine, sources, NumberOfSubscriptions)));
            }
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();

            Trace.WriteLine(
                string.Format("Time for creation of '{0}' engines with '{1}' subscriptions each: '{2}' milliseconds.",
                    NumberOfEngines,
                    NumberOfSubscriptions,
                    stopwatch.ElapsedMilliseconds));

            // Sending events.
            var inputs = sources.ToArray();
            stopwatch.Restart();
            for (int j = 0; j < 100; j++)
            {
                foreach (var source in inputs)
                {
                    source.OnNext(j);
                }
            }

            foreach (var input in inputs)
            {
                var result = MockObserver.Get<int>(input.Id);
                if (result == null || !result.WaitForCount(100, TimeSpan.FromSeconds(100)))
                {
                    Assert.Fail("Not everything was received.");
                }
                result.Clear();
            }
            stopwatch.Stop();

            Trace.WriteLine(
                string.Format("Time for processing '{0}' events: '{1}' milliseconds.",
                    100 * inputs.Length,
                    stopwatch.ElapsedMilliseconds));
        }

        private static void AddSubscriptions(
            ICheckpointingQueryEngine engine,
            ConcurrentQueue<MockObservable<int>> inputs,
            int numberOfSubscriptions)
        {
            var reactive = GetQueryEngineReactiveService(engine);
            for (int i = 0; i < numberOfSubscriptions; i++)
            {
                var sourceUri = new Uri(engine.Uri + "/source" + i);
                var source = reactive.GetObservable<string, int>(MockObservableUri)(sourceUri.ToString());
                var subscriptionUri = new Uri(engine.Uri + "/subscription" + i);
                source.SubscribeOnScheduler().Subscribe(
                    reactive.GetObserver<string, int>(MockObserverUri)(sourceUri.ToString()),
                    subscriptionUri,
                    null);

                inputs.Enqueue(MockObservable.Get<int>(sourceUri.ToString()));
            }
        }
    }

    public static class QbservableExtensions
    {
        [KnownResource("rx://observable/subscribeonscheduler")]
        public static IReactiveQbservable<T> SubscribeOnScheduler<T>(this IReactiveQbservable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression
                    )
            );
        }
    }

    public static class TestQbservable
    {
        public static IReactiveQbservable<TSource> SubscribeOnInternalScheduler<TSource>(this IReactiveQbservable<TSource> source)
        {
            throw new NotImplementedException();
        }

        public static ISubscribable<TSource> SubscribeOnInternalScheduler<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new SubscribableOnScheduler<TSource>(source);
        }
    }
}
