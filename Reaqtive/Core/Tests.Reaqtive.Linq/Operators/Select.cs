// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Select : OperatorTestBase
    {
        [TestInitialize]
        public void Initialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        public void Select_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).Select<int, int>(DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Select<int, int>((Func<int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Select<int, int>(DummyFunc<int, int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Select_DisposeInsideSelector()
        {
            var xs = Scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(500, 3),
                OnNext(600, 4));

            var invoked = 0;

            var res = Scheduler.CreateObserver<int>();

            var d = default(ISubscription);
            d = xs.Select(x =>
            {
                invoked++;
                if (Scheduler.Clock > 400)
                    d.Dispose();
                return x;
            }).Subscribe(res);

            SubscriptionInitializeVisitor.Initialize(d, Scheduler.CreateContext());

            Scheduler.ScheduleAbsolute(Disposed, d.Dispose);

            Scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(100, 1),
                OnNext(200, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(0, 500));

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void Select_Completed()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(180, 1),
                    OnNext(210, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(350, 5),
                    OnCompleted<int>(400),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var res = client.Start(() =>
                    xs.Select(x =>
                    {
                        invoked++;
                        return x + 1;
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 3),
                    OnNext(240, 4),
                    OnNext(290, 5),
                    OnNext(350, 6),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void Select_NotCompleted()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(180, 1),
                    OnNext(210, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(350, 5)
                );

                var res = client.Start(() =>
                    xs.Select(x =>
                    {
                        invoked++;
                        return x + 1;
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 3),
                    OnNext(240, 4),
                    OnNext(290, 5),
                    OnNext(350, 6)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000));

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void Select_Error()
        {
            Run(client =>
            {
                var invoked = 0;

                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(180, 1),
                    OnNext(210, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(350, 5),
                    OnError<int>(400, ex),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var res = client.Start(() =>
                    xs.Select(x =>
                    {
                        invoked++;
                        return x + 1;
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 3),
                    OnNext(240, 4),
                    OnNext(290, 5),
                    OnNext(350, 6),
                    OnError<int>(400, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void Select_SelectorThrows()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(180, 1),
                    OnNext(210, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(350, 5),
                    OnCompleted<int>(400),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var ex = new Exception();

                var res = client.Start(() =>
                    xs.Select(x =>
                    {
                        invoked++;
                        if (invoked == 3)
                            throw ex;
                        return x + 1;
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 3),
                    OnNext(240, 4),
                    OnError<int>(290, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 290));

                Assert.AreEqual(3, invoked);
            });
        }

        [TestMethod]
        public void SelectWithIndex_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).Select<int, int>(DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Select<int, int>((Func<int, int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Select<int, int>(DummyFunc<int, int, int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void SelectWithIndex_DisposeInsideSelector()
        {
            var xs = Scheduler.CreateHotObservable(
                OnNext(100, 4),
                OnNext(200, 3),
                OnNext(500, 2),
                OnNext(600, 1)
            );

            var invoked = 0;

            var res = Scheduler.CreateObserver<int>();

            var d = default(ISubscription);
            d = xs.Select((x, index) =>
            {
                invoked++;
                if (Scheduler.Clock > 400)
                    d.Dispose();
                return x + index * 10;
            }).Subscribe(res);

            SubscriptionInitializeVisitor.Initialize(d, Scheduler.CreateContext());

            Scheduler.ScheduleAbsolute(Disposed, d.Dispose);

            Scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(100, 4),
                OnNext(200, 13)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(0, 500));

            Assert.AreEqual(3, invoked);
        }

        [TestMethod]
        public void SelectWithIndex_Completed()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnCompleted<int>(400),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var res = client.Start(() =>
                    xs.Select((x, index) =>
                    {
                        invoked++;
                        return (x + 1) + (index * 10);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 5),
                    OnNext(240, 14),
                    OnNext(290, 23),
                    OnNext(350, 32),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void SelectWithIndex_NotCompleted()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1)
                );

                var res = client.Start(() =>
                    xs.Select((x, index) =>
                    {
                        invoked++;
                        return (x + 1) + (index * 10);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 5),
                    OnNext(240, 14),
                    OnNext(290, 23),
                    OnNext(350, 32)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000));

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void SelectWithIndex_Error()
        {
            Run(client =>
            {
                var invoked = 0;
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnError<int>(400, ex),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var res = client.Start(() =>
                    xs.Select((x, index) =>
                    {
                        invoked++;
                        return (x + 1) + (index * 10);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 5),
                    OnNext(240, 14),
                    OnNext(290, 23),
                    OnNext(350, 32),
                    OnError<int>(400, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void SelectWithIndex_SelectorThrows()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnCompleted<int>(400),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var ex = new Exception();

                var res = client.Start(() =>
                    xs.Select((x, index) =>
                    {
                        invoked++;
                        if (invoked == 3)
                            throw ex;
                        return (x + 1) + (index * 10);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 5),
                    OnNext(240, 14),
                    OnError<int>(290, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 290));

                Assert.AreEqual(3, invoked);
            });
        }

        [TestMethod]
        public void SelectWithIndex_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(275, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1),
                OnNext(270, 2),
                // state saved
                OnNext(280, 3),
                // state loaded
                OnNext(310, 4),
                OnNext(320, 5),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () =>
                    xs.Select((x, index) => x + index).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(270, 3),
                OnNext(280, 5),
                OnNext(310, 6),
                OnNext(320, 8),
                OnCompleted<int>(400)
            );
        }
    }
}
