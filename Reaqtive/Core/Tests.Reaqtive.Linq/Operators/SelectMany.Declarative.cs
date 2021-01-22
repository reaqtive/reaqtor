// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

#if GLITCHING
using Reaqtor;
using Reaqtor.TestingFramework;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    public partial class SelectMany : OperatorTestBase
    {
        [TestMethod]
        public void SelectMany_Complete()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

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
                        OnCompleted(900, witness)
                );

                var res = client.Start(() =>
                    xs.SelectMany(x => x)
                );

                res.Messages.AssertEqual(
                    OnNext(310, 102),
                    OnNext(390, 103),
                    OnNext(410, 104),
                    OnNext(490, 105),
                    OnNext(560, 301),
                    OnNext(580, 202),
                    OnNext(590, 203),
                    OnNext(600, 302),
                    OnNext(620, 303),
                    OnNext(740, 106),
                    OnNext(810, 304),
                    OnNext(860, 305),
                    OnNext(930, 401),
                    OnNext(940, 402),
                    OnCompleted<int>(960)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 900));

                xs.ObserverMessages[2].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(300, 760));

                xs.ObserverMessages[3].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(400, 605));

                xs.ObserverMessages[4].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(550, 960));

                xs.ObserverMessages[5].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(750, 790));

                xs.ObserverMessages[6].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(850, 950));
            });
        }

        [TestMethod]
        public void SelectMany_Complete_InnerNotComplete()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

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
                            OnNext(190, 203))),
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
                        OnCompleted(900, witness)
                );

                var res = client.Start(() =>
                    xs.SelectMany(x => x)
                );

                res.Messages.AssertEqual(
                    OnNext(310, 102),
                    OnNext(390, 103),
                    OnNext(410, 104),
                    OnNext(490, 105),
                    OnNext(560, 301),
                    OnNext(580, 202),
                    OnNext(590, 203),
                    OnNext(600, 302),
                    OnNext(620, 303),
                    OnNext(740, 106),
                    OnNext(810, 304),
                    OnNext(860, 305),
                    OnNext(930, 401),
                    OnNext(940, 402)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 900));

                xs.ObserverMessages[2].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(300, 760));

                xs.ObserverMessages[3].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(400, 1000));

                xs.ObserverMessages[4].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(550, 960));

                xs.ObserverMessages[5].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(750, 790));

                xs.ObserverMessages[6].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(850, 950));
            });
        }

        [TestMethod]
        public void SelectMany_Complete_OuterNotComplete()
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
                            OnCompleted<int>(100)))
                );

                var res = client.Start(() =>
                    xs.SelectMany(x => x)
                );

                res.Messages.AssertEqual(
                    OnNext(310, 102),
                    OnNext(390, 103),
                    OnNext(410, 104),
                    OnNext(490, 105),
                    OnNext(560, 301),
                    OnNext(580, 202),
                    OnNext(590, 203),
                    OnNext(600, 302),
                    OnNext(620, 303),
                    OnNext(740, 106),
                    OnNext(810, 304),
                    OnNext(860, 305),
                    OnNext(930, 401),
                    OnNext(940, 402)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000));

                xs.ObserverMessages[2].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(300, 760));

                xs.ObserverMessages[3].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(400, 605));

                xs.ObserverMessages[4].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(550, 960));

                xs.ObserverMessages[5].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(750, 790));

                xs.ObserverMessages[6].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(850, 950));
            });
        }

        [TestMethod]
        public void SelectMany_Error_Outer()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

                var ex = new Exception();

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
                        OnError(900, ex, witness)
                );

                var res = client.Start(() =>
                    xs.SelectMany(x => x)
                );

                res.Messages.AssertEqual(
                    OnNext(310, 102),
                    OnNext(390, 103),
                    OnNext(410, 104),
                    OnNext(490, 105),
                    OnNext(560, 301),
                    OnNext(580, 202),
                    OnNext(590, 203),
                    OnNext(600, 302),
                    OnNext(620, 303),
                    OnNext(740, 106),
                    OnNext(810, 304),
                    OnNext(860, 305),
                    OnError<int>(900, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 900));

                xs.ObserverMessages[2].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(300, 760));

                xs.ObserverMessages[3].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(400, 605));

                xs.ObserverMessages[4].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(550, 900));

                xs.ObserverMessages[5].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(750, 790));

                xs.ObserverMessages[6].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(850, 900));
            });
        }

        [TestMethod]
        public void SelectMany_Error_Inner()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

                var ex = new Exception();

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
                            OnError<int>(460, ex))),
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
                        OnCompleted(900, witness)
                );

                var res = client.Start(() =>
                    xs.SelectMany(x => x)
                );

                res.Messages.AssertEqual(
                    OnNext(310, 102),
                    OnNext(390, 103),
                    OnNext(410, 104),
                    OnNext(490, 105),
                    OnNext(560, 301),
                    OnNext(580, 202),
                    OnNext(590, 203),
                    OnNext(600, 302),
                    OnNext(620, 303),
                    OnNext(740, 106),
                    OnError<int>(760, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 760));

                xs.ObserverMessages[2].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(300, 760));

                xs.ObserverMessages[3].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(400, 605));

                xs.ObserverMessages[4].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(550, 760));

                xs.ObserverMessages[5].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(750, 760));

                xs.ObserverMessages[6].Value.Value.Subscriptions.AssertEqual(
                );
            });
        }

        [TestMethod]
        public void SelectMany_Dispose()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

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
                        OnCompleted(900, witness)
                );

                var res = client.Start(() =>
                    xs.SelectMany(x => x),
                    700
                );

                res.Messages.AssertEqual(
                    OnNext(310, 102),
                    OnNext(390, 103),
                    OnNext(410, 104),
                    OnNext(490, 105),
                    OnNext(560, 301),
                    OnNext(580, 202),
                    OnNext(590, 203),
                    OnNext(600, 302),
                    OnNext(620, 303)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 700));

                xs.ObserverMessages[2].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(300, 700));

                xs.ObserverMessages[3].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(400, 605));

                xs.ObserverMessages[4].Value.Value.Subscriptions.AssertEqual(
                    Subscribe(550, 700));

                xs.ObserverMessages[5].Value.Value.Subscriptions.AssertEqual(
                );

                xs.ObserverMessages[6].Value.Value.Subscriptions.AssertEqual(
                );
            });
        }

        [TestMethod]
        public void SelectMany_UseFunction()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var xs = client.CreateHotObservable(
                    OnNext(210, 4),
                    OnNext(220, 3),
                    OnNext(250, 5),
                    OnNext(270, 1),
                    OnCompleted<int>(290)
                );

                var res = client.Start(() =>
                    xs.SelectMany(x => context.Timer(TimeSpan.FromTicks(10), TimeSpan.FromTicks(10)).Select(_ => x).Take(x))
                );

                res.Messages.AssertEqual(
                    OnNext(220, 4),
                    OnNext(230, 3),
                    OnNext(230, 4),
                    OnNext(240, 3),
                    OnNext(240, 4),
                    OnNext(250, 3),
                    OnNext(250, 4),
                    OnNext(260, 5),
                    OnNext(270, 5),
                    OnNext(280, 1),
                    OnNext(280, 5),
                    OnNext(290, 5),
                    OnNext(300, 5),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 290)
                );
            });
        }

        [TestMethod]
        public void SelectMany_QueryOperator_CompleteOuterFirst()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var xs = client.CreateHotObservable(
                    OnNext(220, 4),
                    OnNext(221, 3),
                    OnNext(222, 2),
                    OnNext(223, 5),
                    OnCompleted<int>(224)
                );

                var res = client.Start(() =>
                    from x in xs
                    from y in context.Timer(TimeSpan.FromTicks(1), TimeSpan.FromTicks(1)).Take(x)
                    select x * 10 + (int)y
                );

                res.Messages.AssertEqual(
                    OnNext(221, 40),
                    OnNext(222, 30),
                    OnNext(222, 41),
                    OnNext(223, 20),
                    OnNext(223, 31),
                    OnNext(223, 42),
                    OnNext(224, 50),
                    OnNext(224, 21),
                    OnNext(224, 32),
                    OnNext(224, 43),
                    OnNext(225, 51),
                    OnNext(226, 52),
                    OnNext(227, 53),
                    OnNext(228, 54),
                    OnCompleted<int>(228)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 224)
                );
            });
        }

        [TestMethod]
        public void SelectMany_QueryOperator_CompleteInnerFirst()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var xs = client.CreateHotObservable(
                    OnNext(220, 4),
                    OnNext(221, 3),
                    OnNext(222, 2),
                    OnNext(223, 5),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    from x in xs
                    from y in context.Timer(TimeSpan.FromTicks(1), TimeSpan.FromTicks(1)).Take(x)
                    select x * 10 + (int)y
                );

                res.Messages.AssertEqual(
                    OnNext(221, 40),
                    OnNext(222, 30),
                    OnNext(222, 41),
                    OnNext(223, 20),
                    OnNext(223, 31),
                    OnNext(223, 42),
                    OnNext(224, 50),
                    OnNext(224, 21),
                    OnNext(224, 32),
                    OnNext(224, 43),
                    OnNext(225, 51),
                    OnNext(226, 52),
                    OnNext(227, 53),
                    OnNext(228, 54),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void SelectMany_QueryOperator_ErrorOuter()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(220, 4),
                    OnNext(221, 3),
                    OnNext(222, 2),
                    OnNext(223, 5),
                    OnError<int>(224, ex)
                );

                var res = client.Start(() =>
                    from x in xs
                    from y in context.Timer(TimeSpan.FromTicks(1), TimeSpan.FromTicks(1)).Take(x)
                    select x * 10 + (int)y
                );

                res.Messages.AssertEqual(
                    OnNext(221, 40),
                    OnNext(222, 30),
                    OnNext(222, 41),
                    OnNext(223, 20),
                    OnNext(223, 31),
                    OnNext(223, 42),
                    OnError<int>(224, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 224)
                );
            });
        }

        [TestMethod]
        public void SelectMany_QueryOperator_ErrorInner()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var xs = client.CreateHotObservable(
                    OnNext(220, 4),
                    OnNext(221, 3),
                    OnNext(222, 2),
                    OnNext(223, 5),
                    OnCompleted<int>(224)
                );

                var ex = new Exception();

                var err = client.CreateColdObservable<long>(
                    OnError<long>(1, ex)
                );

                var res = client.Start(() =>
                    from x in xs
                    from y in x == 2 ? err
                                     : context.Timer(TimeSpan.FromTicks(1), TimeSpan.FromTicks(1)).Take(x)
                    select x * 10 + (int)y
                );

                res.Messages.AssertEqual(
                    OnNext(221, 40),
                    OnNext(222, 30),
                    OnNext(222, 41),
                    OnError<int>(223, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 223)
                );
            });
        }

        [TestMethod]
        public void SelectMany_QueryOperator_Dispose()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var xs = client.CreateHotObservable(
                    OnNext(220, 4),
                    OnNext(221, 3),
                    OnNext(222, 2),
                    OnNext(223, 5),
                    OnCompleted<int>(224)
                );

                var res = client.Start(() =>
                    from x in xs
                    from y in context.Timer(TimeSpan.FromTicks(1), TimeSpan.FromTicks(1)).Take(x)
                    select x * 10 + (int)y,
                    223
                );

                res.Messages.AssertEqual(
                    OnNext(221, 40),
                    OnNext(222, 30),
                    OnNext(222, 41)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 223)
                );
            });
        }

    }
}
