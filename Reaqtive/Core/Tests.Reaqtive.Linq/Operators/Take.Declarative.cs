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
    public partial class Take : OperatorTestBase
    {
        #region Take Count

        [TestMethod]
        public void Take_Complete_After()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnCompleted<int>(690)
                );

                var res = client.Start(() =>
                    xs.Take(20)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnCompleted<int>(690)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 690)
                );
            });
        }

        [TestMethod]
        public void Take_Complete_Same()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnCompleted<int>(690)
                );

                var res = client.Start(() =>
                    xs.Take(17)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnCompleted<int>(630)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 630)
                );
            });
        }

        [TestMethod]
        public void Take_Complete_Before()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnCompleted<int>(690)
                );

                var res = client.Start(() =>
                    xs.Take(10)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnCompleted<int>(415)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 415)
                );
            });
        }

        [TestMethod]
        public void Take_Error_After()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnError<int>(690, ex)
                );

                var res = client.Start(() =>
                    xs.Take(20)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnError<int>(690, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 690)
                );
            });
        }

        [TestMethod]
        public void Take_Error_Same()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnError<int>(690, new Exception())
                );

                var res = client.Start(() =>
                    xs.Take(17)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnCompleted<int>(630)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 630)
                );
            });
        }

        [TestMethod]
        public void Take_Error_Before()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10),
                    OnError<int>(690, new Exception())
                );

                var res = client.Start(() =>
                    xs.Take(3)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnCompleted<int>(270)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void Take_Dispose_Before()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10)
                );

                var res = client.Start(() =>
                    xs.Take(3),
                    250
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Take_Dispose_After()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnNext(410, 15),
                    OnNext(415, 16),
                    OnNext(460, 72),
                    OnNext(510, 76),
                    OnNext(560, 32),
                    OnNext(570, -100),
                    OnNext(580, -3),
                    OnNext(590, 5),
                    OnNext(630, 10)
                );

                var res = client.Start(() =>
                    xs.Take(3),
                    400
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnCompleted<int>(270)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void Take_0_DefaultScheduler()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13)
                );

                var res = client.Start(() =>
                    xs.Take(0)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(Increment(200, 1)) // Immediate
                );

                xs.Subscriptions.AssertEqual(
                );
            });
        }

        [TestMethod]
        public void Take_Take1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Take(3).Take(4)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnCompleted<int>(270)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void Take_Take2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Take(4).Take(3)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnCompleted<int>(270)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void Take_None()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Take(0)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(Increment(200, 1))
                );

                xs.Subscriptions.AssertEqual(
                );
            });
        }

        #endregion

        #region Take TimeSpan

        [TestMethod]
        public void Take_Zero()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.Take(TimeSpan.Zero)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(200)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200)
                );
            });
        }

        [TestMethod]
        public void Take_Some()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnCompleted<int>(240)
                );

                var res = client.Start(() =>
                    xs.Take(TimeSpan.FromTicks(25))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(225)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        [TestMethod]
        public void Take_Late()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.Take(TimeSpan.FromTicks(50))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void Take_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                    OnError<int>(210, ex)
                );

                var res = client.Start(() =>
                    xs.Take(TimeSpan.FromTicks(50))
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Take_Never()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                );

                var res = client.Start(() =>
                    xs.Take(TimeSpan.FromTicks(50))
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Take_Twice1()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnCompleted<int>(270)
                );

                var res = client.Start(() =>
                    xs.Take(TimeSpan.FromTicks(55)).Take(TimeSpan.FromTicks(35))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnCompleted<int>(235)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 235)
                );
            });
        }

        [TestMethod]
        public void Take_Twice2()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnCompleted<int>(270)
                );

                var res = client.Start(() =>
                    xs.Take(TimeSpan.FromTicks(35)).Take(TimeSpan.FromTicks(55))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnCompleted<int>(235)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 235)
                );
            });
        }

        [TestMethod]
        public void Take_Time_None()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Take(TimeSpan.Zero)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(200)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200)
                );
            });
        }

        [TestMethod]
        public void Take_Time_All()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Take(new TimeSpan(210))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }

        [TestMethod]
        public void Take_Time_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(70, 6),
                    OnNext(150, 4),
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Take(new TimeSpan(85))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 9),
                    OnNext(230, 13),
                    OnNext(270, 7),
                    OnNext(280, 1),
                    OnCompleted<int>(285)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 285)
                );
            });
        }

        [TestMethod]
        public void Take_TimeSpan_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(100, 1),
                    OnNext(210, 2),
                    OnNext(300, 3),
                    OnNext(400, 4),
                    OnNext(500, 5),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() => xs.Take(new TimeSpan(150)));

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(300, 3),
                    OnCompleted<int>(350)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 350)
                );
            });
        }

        [TestMethod]
        public void Take_TimeSpan_AfterEnd()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(100, 1),
                    OnNext(210, 2),
                    OnNext(300, 3),
                    OnNext(400, 4),
                    OnNext(500, 5),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() => xs.Take(new TimeSpan(450)));

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(300, 3),
                    OnNext(400, 4),
                    OnNext(500, 5),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600)
                );
            });
        }

        #endregion
    }
}
