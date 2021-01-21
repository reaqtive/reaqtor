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
    public partial class SkipUntil : OperatorTestBase
    {
        #region SkipUntil Triggering Source

        [TestMethod]
        public void SkipUntil_SomeData_Next()
        {
            Run(client =>
            {
                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4), //!
                    OnNext(240, 5), //!
                    OnCompleted<int>(250)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(225, 99),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    l.SkipUntil(r)
                );

                res.Messages.AssertEqual(
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
        public void SkipUntil_SomeData_Error()
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
                    l.SkipUntil(r)
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
        public void SkipUntil_Error_SomeData()
        {
            Run(client =>
            {
                var ex = new Exception();

                var l = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnError<int>(220, ex)
                );

                var r = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(230, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    l.SkipUntil(r)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void SkipUntil_SomeData_Empty()
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
                    l.SkipUntil(r)
                );

                res.Messages.AssertEqual(
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
        public void SkipUntil_Never_Next()
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
                    l.SkipUntil(r)
                );

                res.Messages.AssertEqual(
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        [TestMethod]
        public void SkipUntil_Never_Error1()
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
                    l.SkipUntil(r)
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
        public void SkipUntil_SomeData_Error2()
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
                    OnError<int>(300, ex)
                );

                var res = client.Start(() =>
                    l.SkipUntil(r)
                );

                res.Messages.AssertEqual(
                    OnError<int>(300, ex)
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void SkipUntil_SomeData_Never()
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
                    l.SkipUntil(r)
                );

                res.Messages.AssertEqual(
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 1000 /* can't dispose prematurely, could be in flight to dispatch OnError */)
                );
            });
        }

        [TestMethod]
        public void SkipUntil_Never_Empty()
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
                    l.SkipUntil(r)
                );

                res.Messages.AssertEqual(
                );

                l.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );

                r.Subscriptions.AssertEqual(
                    Subscribe(200, 225)
                );
            });
        }

        [TestMethod]
        public void SkipUntil_Never_Never()
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
                    l.SkipUntil(r)
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

        #endregion

        #region SkipUntil DateTimeOffset

        [TestMethod]
        public void SkipUntil_Zero()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.SkipUntil(new DateTimeOffset())
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
        public void SkipUntil_Some()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.SkipUntil(new DateTimeOffset(215, TimeSpan.Zero))
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
        public void SkipUntil_Late()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(230)
                );

                var res = client.Start(() =>
                    xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero))
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
        public void SkipUntil_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                    OnError<int>(210, ex)
                );

                var res = client.Start(() =>
                    xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero))
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
        public void SkipUntil_Never()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                );

                var res = client.Start(() =>
                    xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero))
                );

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void SkipUntil_Twice1()
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
                    xs.SkipUntil(new DateTimeOffset(215, TimeSpan.Zero)).SkipUntil(new DateTimeOffset(230, TimeSpan.Zero))
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
        public void SkipUntil_Twice2()
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
                    xs.SkipUntil(new DateTimeOffset(230, TimeSpan.Zero)).SkipUntil(new DateTimeOffset(215, TimeSpan.Zero))
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
        public void SkipUntil_DateTimeOffset_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(100, 1),
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 4),
                    OnNext(500, 5),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.SkipUntil(new DateTimeOffset(350, TimeSpan.Zero))
                );

                res.Messages.AssertEqual(
                    OnNext(400, 4),
                    OnNext(500, 5),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600)
                );
            });
        }

        [TestMethod]
        public void SkipUntil_DateTimeOffset_AfterEnd()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(100, 1),
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 4),
                    OnNext(500, 5),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.SkipUntil(new DateTimeOffset(650, TimeSpan.Zero))
                );

                res.Messages.AssertEqual(
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
