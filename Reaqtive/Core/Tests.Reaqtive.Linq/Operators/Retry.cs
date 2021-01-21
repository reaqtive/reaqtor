// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Linq;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Retry : OperatorTestBase
    {
        [TestMethod]
        public void Retry_Observable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Retry<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Retry().Subscribe(null));
        }

        [TestMethod]
        public void Retry_Observable_Throws1()
        {
            Run(scheduler =>
            {
                var xs = Subscribable.Return(1).Apply(scheduler).Retry();

                var sub = xs.Subscribe(Observer.Create<int>(x => { throw new InvalidOperationException(); }, ex => { }, () => { }));

                InitializeSubscription(sub, scheduler);

                ReactiveAssert.Throws<InvalidOperationException>(() => scheduler.Start());
            });
        }

        [TestMethod]
        public void Retry_Observable_Throws2()
        {
            Run(scheduler =>
            {
                var ys = Subscribable.Throw<int>(new Exception()).Apply(scheduler).Retry();

                var sub = ys.Subscribe(Observer.Create<int>(x => { }, ex => { throw new InvalidOperationException(); }, () => { }));

                InitializeSubscription(sub, scheduler);

                scheduler.ScheduleAbsolute(210, () => sub.Dispose());

                scheduler.Start();
            });
        }

        [TestMethod]
        public void Retry_Observable_Throws3()
        {
            Run(scheduler =>
            {
                var zs = Subscribable.Return(1).Apply(scheduler).Retry();

                var sub = zs.Subscribe(Observer.Create<int>(x => { }, ex => { }, () => { throw new InvalidOperationException(); }));

                InitializeSubscription(sub, scheduler);

                ReactiveAssert.Throws<InvalidOperationException>(() => scheduler.Start());
            });
        }

        [TestMethod]
        public void Retry_Observable_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Retry<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Retry().Subscribe(null));
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Retry<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummySubscribable<int>.Instance.Retry(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Retry(0).Subscribe(null));
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Throws()
        {
            Run(scheduler =>
            {
                var xs = Subscribable.Return(1).Apply(scheduler).Retry(3);

                var sub = xs.Subscribe(Observer.Create<int>(x => { throw new InvalidOperationException(); }, ex => { }, () => { }));

                InitializeSubscription(sub, scheduler);

                ReactiveAssert.Throws<InvalidOperationException>(() => scheduler.Start());
            });

            Run(scheduler =>
            {
                var ys = Subscribable.Throw<int>(new Exception()).Apply(scheduler).Retry(100);

                var sub = ys.Subscribe(Observer.Create<int>(x => { }, ex => { throw new InvalidOperationException(); }, () => { }));

                InitializeSubscription(sub, scheduler);

                scheduler.ScheduleAbsolute(10, () => sub.Dispose());

                scheduler.Start();
            });

            Run(scheduler =>
            {
                var zs = Subscribable.Return(1).Apply(scheduler).Retry(100);

                var sub = zs.Subscribe(Observer.Create<int>(x => { }, ex => { }, () => { throw new InvalidOperationException(); }));

                InitializeSubscription(sub, scheduler);

                ReactiveAssert.Throws<InvalidOperationException>(() => scheduler.Start());
            });

            Run(scheduler =>
            {
                var xss = Observable.Create<int>(new Func<IObserver<int>, IDisposable>(o => { throw new InvalidOperationException(); })).ToSubscribable().Retry(3);
                new SubscriptionInitializeVisitor(xss.Subscribe(Observer.Create<int>(x => { }, ex => { throw ex; }, () => { }))).Initialize(scheduler.CreateContext());
                ReactiveAssert.Throws<InvalidOperationException>(() => scheduler.Start());
            });
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Default_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Retry<int>(default(ISubscribable<int>), 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummySubscribable<int>.Instance.Retry(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Retry(0).Subscribe(null));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }
    }
}
