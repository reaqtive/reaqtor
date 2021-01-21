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
    public partial class TakeUntil : OperatorTestBase
    {
        #region TakeUntil Triggering Source

        [TestMethod]
        public void TakeUntil_Preempt_SomeData_Next()
        {
            Run(client =>
            {
                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(225, 99),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnCompleted<int>(225)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        [TestMethod]
        public void TakeUntil_Preempt_SomeData_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(225, ex)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnError<int>(225, ex)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_SomeData_Empty()
        {
            Run(client =>
            {
                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(225)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_SomeData_Never()
        {
            Run(client =>
            {
                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void TakeUntil_Preempt_Never_Next()
        {
            Run(client =>
            {
                var l = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(225, 2), //!
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(225)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        [TestMethod]
        public void TakeUntil_Preempt_Never_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var l = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(225, ex)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                    OnError<int>(225, ex)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_Never_Empty()
        {
            Run(client =>
            {
                var l = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(225)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 1000 /* can't dispose prematurely, could be in flight to dispatch OnError */)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_Never_Never()
        {
            Run(client =>
            {
                var l = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void TakeUntil_Preempt_BeforeFirstProduced()
        {
            Run(client =>
            {
                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(230, 2),
                    OnCompleted<int>(240)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2), //!
                    OnCompleted<int>(220)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(210)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void TakeUntil_Error_Some()
        {
            Run(client =>
            {
                var ex = new Exception();

                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(225, ex)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext<int>(240, 2)
                );

                var res = client.Start(() =>
                    l.TakeUntil(r)
                );

                res.Messages.AssertEqual(
                    OnError<int>(225, ex)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        #endregion

        #region TakeUntil DateTimeOffset

        [TestMethod]
        public void TakeUntil_Zero()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.TakeUntil(new DateTimeOffset())
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
        public void TakeUntil_Some()
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
                    xs.TakeUntil(new DateTimeOffset(225, TimeSpan.Zero))
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
        public void TakeUntil_Late()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero))
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
        public void TakeUntil_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                    OnError<int>(210, ex)
                );

                var res = client.Start(() =>
                    xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero))
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
        public void TakeUntil_Never()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                );

                var res = client.Start(() =>
                    xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero))
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
        public void TakeUntil_Twice1()
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
                    xs.TakeUntil(new DateTimeOffset(255, TimeSpan.Zero)).TakeUntil(new DateTimeOffset(235, TimeSpan.Zero))
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
        public void TakeUntil_Twice2()
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
                    xs.TakeUntil(new DateTimeOffset(235, TimeSpan.Zero)).TakeUntil(new DateTimeOffset(255, TimeSpan.Zero))
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
        public void TakeUntil_DateTimeOffset_Simple()
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

                var res = client.Start(() =>
                    xs.TakeUntil(new DateTimeOffset(350, TimeSpan.Zero))
                );

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
        public void TakeUntil_DateTimeOffset_AfterEnd()
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

                var res = client.Start(() =>
                    xs.TakeUntil(new DateTimeOffset(700, TimeSpan.Zero))
                );

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
