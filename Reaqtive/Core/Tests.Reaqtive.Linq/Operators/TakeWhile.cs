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
    public partial class TakeWhile : OperatorTestBase
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
        public void TakeWhile_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).TakeWhile(DummyFunc<int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.TakeWhile(default(Func<int, bool>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.TakeWhile(DummyFunc<int, bool>.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).TakeWhile((x, i) => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.TakeWhile(default(Func<int, int, bool>)));
        }

        [TestMethod]
        public void TakeWhile_Complete_Before()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(90, -1),
                    OnNext(110, -1),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnCompleted<int>(330),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23),
                    OnCompleted<int>(600)
                );

                var invoked = 0;

                var res = client.Start(() =>
                    xs.TakeWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnCompleted<int>(330)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 330)
                );

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void TakeWhile_Complete_After()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(90, -1),
                    OnNext(110, -1),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23),
                    OnCompleted<int>(600)
                );

                var invoked = 0;

                var res = client.Start(() =>
                    xs.TakeWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnCompleted<int>(390)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 390)
                );

                Assert.AreEqual(6, invoked);
            });
        }

        [TestMethod]
        public void TakeWhile_Error_Before()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(90, -1),
                    OnNext(110, -1),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnError<int>(270, ex),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23)
                );

                var invoked = 0;

                var res = client.Start(() =>
                    xs.TakeWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnError<int>(270, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );

                Assert.AreEqual(2, invoked);
            });
        }

        [TestMethod]
        public void TakeWhile_Error_After()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(90, -1),
                    OnNext(110, -1),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23),
                    OnError<int>(600, new Exception())
                );

                var invoked = 0;

                var res = client.Start(() =>
                    xs.TakeWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnCompleted<int>(390)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 390)
                );

                Assert.AreEqual(6, invoked);
            });
        }

        [TestMethod]
        public void TakeWhile_Dispose_Before()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(90, -1),
                    OnNext(110, -1),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23),
                    OnCompleted<int>(600)
                );

                var invoked = 0;

                var res = client.Start(() =>
                    xs.TakeWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    }),
                    300
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );

                Assert.AreEqual(3, invoked);
            });
        }

        [TestMethod]
        public void TakeWhile_Dispose_After()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(90, -1),
                    OnNext(110, -1),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23),
                    OnCompleted<int>(600)
                );

                var invoked = 0;

                var res = client.Start(() =>
                    xs.TakeWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    }),
                    400
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnCompleted<int>(390)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 390)
                );

                Assert.AreEqual(6, invoked);
            });
        }

        [TestMethod]
        public void TakeWhile_Zero()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(90, -1),
                    OnNext(110, -1),
                    OnNext(205, 100),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23),
                    OnCompleted<int>(600)
                );

                var invoked = 0;

                var res = client.Start(() =>
                    xs.TakeWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    }),
                    300
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(205)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 205)
                );

                Assert.AreEqual(1, invoked);
            });
        }

        [TestMethod]
        public void TakeWhile_Throw()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(90, -1),
                    OnNext(110, -1),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23),
                    OnCompleted<int>(600)
                );

                var invoked = 0;
                var ex = new Exception();

                var res = client.Start(() =>
                    xs.TakeWhile(x =>
                    {
                        invoked++;
                        if (invoked == 3)
                            throw ex;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnError<int>(290, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 290)
                );

                Assert.AreEqual(3, invoked);
            });
        }

        [TestMethod]
        public void TakeWhile_Index_SelectorThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(90, -1),
                    OnNext(110, -1),
                    OnNext(205, 100),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.TakeWhile((x, i) => { if (i < 5) return true; throw ex; })
                );

                res.Messages.AssertEqual(
                    OnNext(205, 100),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnError<int>(350, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 350)
                );
            });
        }

        [TestMethod]
        public void TakeWhile_Index_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(290, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(270, 5),
                OnNext(280, 7),
                // state saved @290
                OnNext(300, 11),
                // state loaded @305
                OnNext(310, 13),
                OnNext(340, 17),
                OnNext(370, 19),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(() =>
                xs.TakeWhile((x, i) => i < 6).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),           // i = 0
                OnNext(230, 3),           // i = 1
                OnNext(270, 5),           // i = 2
                OnNext(280, 7),           // i = 3
                                          // state saved @290
                OnNext(300, 11),          // i = 4
                                          // state reloaded @305
                OnNext(310, 13),          // i = 4
                OnNext(340, 17),          // i = 5
                OnCompleted<int>(370)
            );
        }
    }
}
