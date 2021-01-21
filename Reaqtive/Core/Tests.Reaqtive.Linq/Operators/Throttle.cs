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
    public partial class Throttle : OperatorTestBase
    {
        [TestMethod]
        public void Throttle_ArgumentChecking()
        {
            var someObservable = Subscribable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Throttle(default(ISubscribable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Throttle(someObservable, TimeSpan.FromSeconds(-1)));
        }

        [TestMethod]
        public void Throttle_Duration_ArgumentChecking()
        {
            var someObservable = DummySubscribable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Throttle(default(ISubscribable<int>), x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Throttle(someObservable, default(Func<int, ISubscribable<string>>)));
        }

        [TestMethod]
        public void Throttle_Duration_DelayBehavior()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, -1),
                    OnNext(250, 0),
                    OnNext(280, 1),
                    OnNext(310, 2),
                    OnNext(350, 3),
                    OnNext(400, 4),
                    OnCompleted<int>(550)
                );

                var ys = new[] {
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                };

                var res = client.Start(() =>
                    xs.Throttle(x => ys[x])
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250 + 20, 0),
                    OnNext<int>(280 + 20, 1),
                    OnNext<int>(310 + 20, 2),
                    OnNext<int>(350 + 20, 3),
                    OnNext<int>(400 + 20, 4),
                    OnCompleted<int>(550)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 550)
                );

                ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
                ys[1].Subscriptions.AssertEqual(Subscribe(280, 280 + 20));
                ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
                ys[3].Subscriptions.AssertEqual(Subscribe(350, 350 + 20));
                ys[4].Subscriptions.AssertEqual(Subscribe(400, 400 + 20));
            });
        }

        [TestMethod]
        public void Throttle_Duration_ThrottleBehavior()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, -1),
                    OnNext(250, 0),
                    OnNext(280, 1),
                    OnNext(310, 2),
                    OnNext(350, 3),
                    OnNext(400, 4),
                    OnCompleted<int>(550)
                );

                var ys = new[] {
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(40, 42),
                        OnNext(45, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(60, 42),
                        OnNext(65, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                };

                var res = client.Start(() =>
                    xs.Throttle(x => ys[x])
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250 + 20, 0),
                    OnNext<int>(310 + 20, 2),
                    OnNext<int>(400 + 20, 4),
                    OnCompleted<int>(550)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 550)
                );

                ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
                ys[1].Subscriptions.AssertEqual(Subscribe(280, 310));
                ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
                ys[3].Subscriptions.AssertEqual(Subscribe(350, 400));
                ys[4].Subscriptions.AssertEqual(Subscribe(400, 400 + 20));
            });
        }

        [TestMethod]
        public void Throttle_Duration_EarlyCompletion()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, -1),
                    OnNext(250, 0),
                    OnNext(280, 1),
                    OnNext(310, 2),
                    OnNext(350, 3),
                    OnNext(400, 4),
                    OnCompleted<int>(410)
                );

                var ys = new[] {
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(40, 42),
                        OnNext(45, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(60, 42),
                        OnNext(65, 99)
                    ),
                    client.CreateColdObservable(
                        OnNext(20, 42),
                        OnNext(25, 99)
                    ),
                };

                var res = client.Start(() =>
                    xs.Throttle(x => ys[x])
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250 + 20, 0),
                    OnNext<int>(310 + 20, 2),
                    OnNext<int>(410, 4),
                    OnCompleted<int>(410)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 410)
                );

                ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
                ys[1].Subscriptions.AssertEqual(Subscribe(280, 310));
                ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
                ys[3].Subscriptions.AssertEqual(Subscribe(350, 400));
                ys[4].Subscriptions.AssertEqual(Subscribe(400, 410));
            });
        }

        [TestMethod]
        public void Throttle_Duration_InnerError()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(250, 2),
                    OnNext(350, 3),
                    OnNext(450, 4),
                    OnCompleted<int>(550)
                );

                var ex = new Exception();

                var res = client.Start(() =>
                    xs.Throttle(x =>
                        x < 4 ? client.CreateColdObservable(
                                    OnNext(x * 10, "Ignore"),
                                    OnNext(x * 10 + 5, "Aargh!")
                                )
                              : client.CreateColdObservable(
                                    OnError<string>(x * 10, ex)
                                )
                    )
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250 + 2 * 10, 2),
                    OnNext<int>(350 + 3 * 10, 3),
                    OnError<int>(450 + 4 * 10, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 490)
                );
            });
        }

        [TestMethod]
        public void Throttle_Duration_OuterError()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(250, 2),
                    OnNext(350, 3),
                    OnNext(450, 4),
                    OnError<int>(460, ex)
                );

                var res = client.Start(() =>
                    xs.Throttle(x =>
                        client.CreateColdObservable(
                            OnNext(x * 10, "Ignore"),
                            OnNext(x * 10 + 5, "Aargh!")
                        )
                    )
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250 + 2 * 10, 2),
                    OnNext<int>(350 + 3 * 10, 3),
                    OnError<int>(460, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 460)
                );
            });
        }

        [TestMethod]
        public void Throttle_Duration_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(250, 2),
                    OnNext(350, 3),
                    OnNext(450, 4),
                    OnCompleted<int>(550)
                );

                var ex = new Exception();

                var res = client.Start(() =>
                    xs.Throttle(x =>
                    {
                        if (x < 4)
                        {
                            return client.CreateColdObservable(
                                    OnNext(x * 10, "Ignore"),
                                    OnNext(x * 10 + 5, "Aargh!")
                                );
                        }
                        else
                            throw ex;
                    })
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250 + 2 * 10, 2),
                    OnNext<int>(350 + 3 * 10, 3),
                    OnError<int>(450, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 450)
                );
            });
        }

        [TestMethod]
        public void Throttle_Duration_InnerDone_DelayBehavior()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(250, 2),
                    OnNext(350, 3),
                    OnNext(450, 4),
                    OnCompleted<int>(550)
                );

                var ex = new Exception();

                var res = client.Start(() =>
                    xs.Throttle(x =>
                        client.CreateColdObservable(
                            OnCompleted<string>(x * 10)
                        )
                    )
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250 + 2 * 10, 2),
                    OnNext<int>(350 + 3 * 10, 3),
                    OnNext<int>(450 + 4 * 10, 4),
                    OnCompleted<int>(550)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 550)
                );
            });
        }

        [TestMethod]
        public void Throttle_Duration_InnerDone_ThrottleBehavior()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(250, 2),
                    OnNext(280, 3),
                    OnNext(300, 4),
                    OnNext(400, 5),
                    OnNext(410, 6),
                    OnCompleted<int>(550)
                );

                var ex = new Exception();

                var res = client.Start(() =>
                    xs.Throttle(x =>
                        client.CreateColdObservable(
                            OnCompleted<string>(x * 10)
                        )
                    )
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250 + 2 * 10, 2),
                    OnNext<int>(300 + 4 * 10, 4),
                    OnNext<int>(410 + 6 * 10, 6),
                    OnCompleted<int>(550)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 550)
                );
            });
        }
    }
}
