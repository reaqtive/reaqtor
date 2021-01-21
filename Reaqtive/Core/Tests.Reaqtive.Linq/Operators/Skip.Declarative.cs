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
    public partial class Skip : OperatorTestBase
    {
        #region Skip Count

        [TestMethod]
        public void Skip_Complete_After()
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
                    xs.Skip(20)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(690)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 690)
                );
            });
        }

        [TestMethod]
        public void Skip_Complete_Same()
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
                    xs.Skip(17)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(690)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 690)
                );
            });
        }

        [TestMethod]
        public void Skip_Complete_Before()
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
                    xs.Skip(10)
                );

                res.Messages.AssertEqual(
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
        public void Skip_Complete_Zero()
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
                    xs.Skip(0)
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
        public void Skip_Error_After()
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
                    xs.Skip(20)
                );

                res.Messages.AssertEqual(
                    OnError<int>(690, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 690)
                );
            });
        }

        [TestMethod]
        public void Skip_Error_Same()
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
                    xs.Skip(17)
                );

                res.Messages.AssertEqual(
                    OnError<int>(690, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 690)
                );
            });
        }

        [TestMethod]
        public void Skip_Error_Before()
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
                    xs.Skip(3)
                );

                res.Messages.AssertEqual(
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
        public void Skip_Dispose_Before()
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
                    xs.Skip(3),
                    250
                );

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Skip_Dispose_After()
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
                    xs.Skip(3),
                    400
                );

                res.Messages.AssertEqual(
                    OnNext(280, 1),
                    OnNext(300, -1),
                    OnNext(310, 3),
                    OnNext(340, 8),
                    OnNext(370, 11)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }

        [TestMethod]
        public void Skip_Skip1()
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
                    xs.Skip(3).Skip(2)
                );

                res.Messages.AssertEqual(
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

        #endregion

        #region Skip TimeSpan

        [TestMethod]
        public void Skip_Zero()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.Skip(TimeSpan.Zero)
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
        public void Skip_Some()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.Skip(TimeSpan.FromTicks(15))
                );

                res.Messages.AssertEqual(
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void Skip_Late()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.Skip(TimeSpan.FromTicks(50))
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(230)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void Skip_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                    OnError<int>(210, ex)
                );

                var res = client.Start(() =>
                    xs.Skip(TimeSpan.FromTicks(50))
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
        public void Skip_Never()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                );

                var res = client.Start(() =>
                    xs.Skip(TimeSpan.FromTicks(50))
                );

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void Skip_Twice1()
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
                    xs.Skip(TimeSpan.FromTicks(15)).Skip(TimeSpan.FromTicks(30))
                );

                res.Messages.AssertEqual(
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnCompleted<int>(270)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void Skip_Twice2()
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
                    xs.Skip(TimeSpan.FromTicks(30)).Skip(TimeSpan.FromTicks(15))
                );

                res.Messages.AssertEqual(
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnCompleted<int>(270)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void Skip_Time_None()
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
                    xs.Skip(TimeSpan.Zero)
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
        public void Skip_Time_Simple()
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
                    xs.Skip(new TimeSpan(105))
                );

                res.Messages.AssertEqual(
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
        public void Skip_Time_AfterDispose()
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
                    xs.Skip(new TimeSpan(250))
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }

        #endregion
    }
}
