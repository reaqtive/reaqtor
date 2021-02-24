// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;
using Reaqtive.TestingFramework.Mocks;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class SelectMany : OperatorTestBase
    {
        [TestMethod]
        public void SelectMany_ArgumentChecking()
        {
            var ns = default(ISubscribable<int>);
            var xs = DummySubscribable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SelectMany(ns, _ => xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SelectMany(xs, default(Func<int, ISubscribable<string>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SelectMany(ns, _ => xs, (x, y) => x + y));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SelectMany(xs, default(Func<int, ISubscribable<string>>), (x, y) => x + y));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SelectMany(xs, _ => xs, default(Func<int, int, int>)));
        }

        [TestMethod]
        public void SelectMany_Throw()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                        OnNext(5, client.CreateColdObservable(
                            OnError<int>(1, new InvalidOperationException()))),
                        OnNext(105, client.CreateColdObservable(
                            OnError<int>(1, new InvalidOperationException()))),
                        OnNext(300, client.CreateColdObservable(
                            OnNext(10, 102),
                            OnNext(90, 103),
                            OnNext(110, 104),
                            OnNext(190, 105),
                            OnNext(440, 106),
                            OnCompleted<int>(460))),
                        OnNext(400, client.CreateColdObservable(
                            OnNext(180, 202),
                            OnNext(190, 203),
                            OnCompleted<int>(205))),
                        OnNext(550, client.CreateColdObservable(
                            OnNext(10, 301),
                            OnNext(50, 302),
                            OnNext(70, 303),
                            OnNext(260, 304),
                            OnNext(310, 305),
                            OnCompleted<int>(410))),
                        OnNext(750, client.CreateColdObservable(
                            OnCompleted<int>(40))),
                        OnNext(850, client.CreateColdObservable(
                            OnNext(80, 401),
                            OnNext(90, 402),
                            OnCompleted<int>(100))),
                        OnCompleted<ITestableSubscribable<int>>(900)
                );

                var invoked = 0;

                var ex = new Exception();

                var res = client.Start(() =>
                    xs.SelectMany(x =>
                    {
                        invoked++;
                        if (invoked == 3)
                            throw ex;
                        return x;
                    })
                );

                res.Messages.AssertEqual(
                    OnNext(310, 102),
                    OnNext(390, 103),
                    OnNext(410, 104),
                    OnNext(490, 105),
                    OnError<int>(550, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 550));

                xs.ObserverMessages[2].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(300, 550));

                xs.ObserverMessages[3].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(400, 550));

                xs.ObserverMessages[4].Value.Value.Subscriptions.AssertEqual(
                );

                xs.ObserverMessages[5].Value.Value.Subscriptions.AssertEqual(
                );

                xs.ObserverMessages[6].Value.Value.Subscriptions.AssertEqual(
                );

                Assert.AreEqual(3, invoked);
            });
        }

        private static T Throw<T>(Exception ex)
        {
            throw ex;
        }

        [TestMethod]
        public void SelectMany_QueryOperator_ThrowSelector()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(220, 4),
                    OnNext(221, 3),
                    OnNext(222, 2),
                    OnNext(223, 5),
                    OnCompleted<int>(224)
                );

                var ex = new Exception();

                var res = client.Start(() =>
                    from x in xs
                    from y in Throw<ISubscribable<long>>(ex)
                    select x * 10 + (int)y
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void SelectMany_QueryOperator_ThrowResult()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(220, 4),
                    OnNext(221, 3),
                    OnNext(222, 2),
                    OnNext(223, 5),
                    OnCompleted<int>(224)
                );

                var ex = new Exception();

                var res = client.Start(() =>
                    from x in xs
                    from y in Subscribable.Timer(TimeSpan.FromTicks(1), TimeSpan.FromTicks(1)).Take(x)
                    select Throw<int>(ex)
                );

                res.Messages.AssertEqual(
                    OnError<int>(221, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 221)
                );
            });
        }
    }
}
