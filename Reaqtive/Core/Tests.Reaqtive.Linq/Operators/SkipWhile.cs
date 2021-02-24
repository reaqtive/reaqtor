﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class SkipWhile : OperatorTestBase
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
        public void SkipWhile_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).SkipWhile(DummyFunc<int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.SkipWhile(default(Func<int, bool>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.SkipWhile(DummyFunc<int, bool>.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).SkipWhile((x, i) => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.SkipWhile(default(Func<int, int, bool>)));
        }

        [TestMethod]
        public void SkipWhile_Complete_Before()
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
                    xs.SkipWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(330)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 330)
                );

                Assert.AreEqual(4, invoked);
            });
        }

        [TestMethod]
        public void SkipWhile_Complete_After()
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
                    xs.SkipWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600)
                );

                Assert.AreEqual(6, invoked);
            });
        }

        [TestMethod]
        public void SkipWhile_Error_Before()
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
                    xs.SkipWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnError<int>(270, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );

                Assert.AreEqual(2, invoked);
            });
        }

        [TestMethod]
        public void SkipWhile_Error_After()
        {
            Run(client =>
            {
                var ex = new Exception();

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
                    OnError<int>(600, ex)
                );

                var invoked = 0;

                var res = client.Start(() =>
                    xs.SkipWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8),
                    OnNext(500, 23),
                    OnError<int>(600, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600)
                );

                Assert.AreEqual(6, invoked);
            });
        }

        [TestMethod]
        public void SkipWhile_Dispose_Before()
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
                    xs.SkipWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    }),
                    300
                );

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );

                Assert.AreEqual(3, invoked);
            });
        }

        [TestMethod]
        public void SkipWhile_Dispose_After()
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
                    xs.SkipWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    }),
                    470
                );

                res.Messages.AssertEqual(
                    OnNext(390, 4),
                    OnNext(410, 17),
                    OnNext(450, 8)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 470)
                );

                Assert.AreEqual(6, invoked);
            });
        }

        [TestMethod]
        public void SkipWhile_Zero()
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
                    xs.SkipWhile(x =>
                    {
                        invoked++;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
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

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600)
                );

                Assert.AreEqual(1, invoked);
            });
        }

        [TestMethod]
        public void SkipWhile_Throw()
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
                    xs.SkipWhile(x =>
                    {
                        invoked++;
                        if (invoked == 3)
                            throw ex;
                        return IsPrime(x);
                    })
                );

                res.Messages.AssertEqual(
                    OnError<int>(290, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 290)
                );

                Assert.AreEqual(3, invoked);
            });
        }

        [TestMethod]
        public void SkipWhile_Index_SelectorThrows()
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
                    xs.SkipWhile((x, i) => { if (i < 5) return true; throw ex; })
                );

                res.Messages.AssertEqual(
                    OnError<int>(350, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 350)
                );
            });
        }

        [TestMethod]
        public void SkipWhile_Indexless_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(275, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1), // skipped
                OnNext(270, 1), // skipped
                                // state saved @275
                OnNext(280, 2), //
                                // state loaded @305
                OnNext(310, 1), // skipped
                OnNext(315, 2),
                OnNext(320, 3),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () =>
                    xs.SkipWhile(i => i != 2).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(280, 2),
                OnNext(315, 2),
                OnNext(320, 3),
                OnCompleted<int>(400)
            );
        }

        [TestMethod]
        public void SkipWhile_Indexful_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(275, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1), // skipped
                                // state saved @275
                OnNext(280, 1),
                OnNext(290, 2), //
                                // state loaded @305
                OnNext(310, 4), // skipped
                OnNext(315, 0),
                OnNext(320, 3),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () =>
                    xs.SkipWhile((i, idx) => (i + idx) != 2).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(280, 1),
                OnNext(290, 2),
                OnNext(315, 0),
                OnNext(320, 3),
                OnCompleted<int>(400)
            );
        }
    }
}
