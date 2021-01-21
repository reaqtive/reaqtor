// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices.TypeSystem;

using Reaqtive;
using Reaqtive.Scheduler;
using Reaqtive.Testing;
using Reaqtive.TestingFramework.Mocks;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    using TestScheduler = global::Reaqtive.TestingFramework.TestScheduler;

    [TestClass]
    public abstract class VirtualTimeEngineTest : QueryEngineTest
    {
        internal static readonly Uri TestableObservableUri = new("a:/TestableObservable");
        internal static readonly Uri TestableObserverUri = new("b:/TestableObserver");

        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();
            Scheduler = new TestScheduler();
            TestableEntityFactory.Scheduler = Scheduler;
            TestableEntityFactory.Clear();
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            TestableEntityFactory.Scheduler = null;
            Scheduler.Dispose();
            Scheduler = null;
            base.TestCleanup();
        }

        protected override void AddCommonDefinitions(IReactive context)
        {
            base.AddCommonDefinitions(context);

            context.DefineObservable<Tuple<string, Recorded<Notification<T>>[]>, T>(
                TestableObservableUri,
                t => TestableEntityFactory.CreateColdSubscribable(t.Item1, t.Item2).AsQbservable(),
                null);
            context.DefineObserver<Tuple<string>, T>(
                TestableObserverUri,
                t => TestableEntityFactory.CreateObserver<T>(t.Item1).AsQbserver(),
                null);
        }

        public TestScheduler Scheduler { get; private set; }

        protected override IScheduler GetScheduler()
        {
            return Scheduler;
        }

        protected static ITestableSubscribable<T> GetTestableSubscribable<T>(string name)
        {
            return TestableEntityFactory.GetSubscribable<T>(name);
        }

        protected static ITestableObserver<T> GetTestableObserver<T>(string name)
        {
            return TestableEntityFactory.GetObserver<T>(name);
        }

        protected static IReactiveQbservable<T> GetTestableQbservable<T>(ReactiveServiceContext reactive, string name, params Recorded<Notification<T>>[] messages)
        {
            return reactive.GetObservable<string, Recorded<Notification<T>>[], T>(TestableObservableUri)(name, messages);
        }

        protected static IReactiveQbserver<T> GetTestableQbserver<T>(ReactiveServiceContext reactive, string name)
        {
            return reactive.GetObserver<string, T>(TestableObserverUri)(name);
        }

        protected static Recorded<Notification<T>> OnNext<T>(long ticks, T value)
        {
            return ReactiveTest.OnNext(ticks, value);
        }

        protected static Recorded<Notification<T>> OnError<T>(long ticks, Exception exception)
        {
            return ReactiveTest.OnError<T>(ticks, exception);
        }

        protected static Recorded<Notification<T>> OnError<T>(long ticks, Func<Exception, bool> predicate)
        {
            return ReactiveTest.OnError<T>(ticks, predicate);
        }

        protected static Recorded<Notification<T>> OnCompleted<T>(long ticks)
        {
            return ReactiveTest.OnCompleted<T>(ticks);
        }

        protected static Subscription Subscribe(long start)
        {
            return ReactiveTest.Subscribe(start);
        }

        protected static Subscription Subscribe(long start, long end)
        {
            return ReactiveTest.Subscribe(start, end);
        }
    }
}
