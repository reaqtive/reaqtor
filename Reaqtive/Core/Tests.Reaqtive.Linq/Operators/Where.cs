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
    public partial class Where : OperatorTestBase
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

        private static bool IsPrime(int i)
        {
            if (i <= 1)
                return false;

            var max = (int)Math.Sqrt(i);
            for (var j = 2; j <= max; ++j)
                if (i % j == 0)
                    return false;

            return true;
        }

        [TestMethod]
        public void Where_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).Where<int>(DummyFunc<int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Where<int>((Func<int, bool>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Where<int>(DummyFunc<int, bool>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Where_Complete()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(340, 5),
                    OnNext(390, 7),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));

                Assert.AreEqual(9, invoked);
            });
        }

        [TestMethod]
        public void Where_True()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where(x =>
                    {
                        invoked++;
                        return true;
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));

                Assert.AreEqual(9, invoked);
            });
        }

        [TestMethod]
        public void Where_False()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where(x =>
                    {
                        invoked++;
                        return false;
                    })
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));

                Assert.AreEqual(9, invoked);
            });
        }

        [TestMethod]
        public void Where_Dispose()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    }),
                    400
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(340, 5),
                    OnNext(390, 7)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));

                Assert.AreEqual(5, invoked);
            });
        }

        [TestMethod]
        public void Where_Error()
        {
            Run(client =>
            {
                var invoked = 0;

                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnError<int>(600, ex),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(340, 5),
                    OnNext(390, 7),
                    OnNext(580, 11),
                    OnError<int>(600, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));

                Assert.AreEqual(9, invoked);
            });
        }

        [TestMethod]
        public void Where_Throw()
        {
            Run(client =>
            {
                var invoked = 0;
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where(x =>
                    {
                        invoked++;
                        if (x > 5)
                            throw ex;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(340, 5),
                    OnError<int>(380, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 380));

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void Where_DisposeInPredicate()
        {
            var invoked = 0;

            var xs = Scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = Scheduler.CreateObserver<int>();

            var d = default(ISubscription);
            var ys = default(ISubscribable<int>);

            Scheduler.ScheduleAbsolute(Created, () => ys = xs.Where(x =>
            {
                invoked++;
                if (x == 8)
                    d.Dispose();
                return IsPrime(x);
            }));

            Scheduler.ScheduleAbsolute(Subscribed, () =>
            {
                d = ys.Subscribe(res);
                SubscriptionInitializeVisitor.Initialize(d, Scheduler.CreateContext());
            });

            Scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            Scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450));

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void WhereWhereOptimization_SecondPredicateThrows()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where(x => x > 3).Where(x =>
                    {
                        if (x <= 3)
                            throw new Exception();

                        return x % 2 == 0;
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(270, 4),
                    OnNext(380, 6),
                    OnNext(450, 8),
                    OnNext(560, 10),
                    OnCompleted<int>(600)
                );
            });
        }

        [TestMethod]
        public void WhereIndex_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).Where<int>(DummyFunc<int, int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Where<int>((Func<int, int, bool>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Where<int>(DummyFunc<int, int, bool>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void WhereIndex_Complete()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) =>
                    {
                        invoked++;
                        return IsPrime(x + i * 10);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(390, 7),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));

                Assert.AreEqual(9, invoked);
            });
        }

        [TestMethod]
        public void WhereIndex_True()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) =>
                    {
                        invoked++;
                        return true;
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));

                Assert.AreEqual(9, invoked);
            });
        }

        [TestMethod]
        public void WhereIndex_False()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) =>
                    {
                        invoked++;
                        return false;
                    })
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));

                Assert.AreEqual(9, invoked);
            });
        }

        [TestMethod]
        public void WhereIndex_Dispose()
        {
            Run(client =>
            {
                var invoked = 0;

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) =>
                    {
                        invoked++;
                        return IsPrime(x + i * 10);
                    }),
                    400
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(390, 7)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));

                Assert.AreEqual(5, invoked);
            });
        }

        [TestMethod]
        public void WhereIndex_Error()
        {
            Run(client =>
            {
                var invoked = 0;
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnError<int>(600, ex),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) =>
                    {
                        invoked++;
                        return IsPrime(x + i * 10);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(390, 7),
                    OnError<int>(600, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));

                Assert.AreEqual(9, invoked);
            });
        }

        [TestMethod]
        public void WhereIndex_Throw()
        {
            Run(client =>
            {
                var invoked = 0;
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) =>
                    {
                        invoked++;
                        if (x > 5)
                            throw ex;
                        return IsPrime(x + i * 10);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnError<int>(380, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 380));

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void WhereIndex_DisposeInPredicate()
        {
            var invoked = 0;

            var xs = Scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = Scheduler.CreateObserver<int>();

            var d = default(ISubscription);
            var ys = default(ISubscribable<int>);

            Scheduler.ScheduleAbsolute(Created, () => ys = xs.Where((x, i) =>
            {
                invoked++;
                if (x == 8)
                    d.Dispose();
                return IsPrime(x + i * 10);
            }));

            Scheduler.ScheduleAbsolute(Subscribed, () =>
            {
                d = ys.Subscribe(res);
                SubscriptionInitializeVisitor.Initialize(d, Scheduler.CreateContext());
            });

            Scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            Scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450));

            Assert.AreEqual(6, invoked);
        }

        [TestMethod]
        public void Where_Where1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Where(x => x > 3).Where(x => x < 6)
                );

                res.Messages.AssertEqual(
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void Where_Where2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) => i >= 1).Where(x => x < 6)
                );

                res.Messages.AssertEqual(
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void Where_Where3()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Where(x => x > 3).Where((x, i) => i < 2)
                );

                res.Messages.AssertEqual(
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void Where_Where4()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) => i >= 1).Where((x, i) => i < 2)
                );

                res.Messages.AssertEqual(
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void Where_Indexed_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(240, state),
                OnLoad(320, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1),
                OnNext(280, 0),
                OnNext(290, 1),
                OnNext(310, 0),
                OnNext(330, 1),
                OnNext(340, 0),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () =>
                    xs.Where((i, idx) => (i + idx) % 2 == 0).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(330, 1),
                OnNext(340, 0),
                OnCompleted<int>(400)
            );
        }
    }
}
