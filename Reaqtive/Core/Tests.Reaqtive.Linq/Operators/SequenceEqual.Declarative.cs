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
    public partial class SequenceEqual : OperatorTestBase
    {
        [TestMethod]
        public void SequenceEqual_Simple1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var ys = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.SequenceEqual(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(300, true),
                    OnCompleted<bool>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void SequenceEqual_Simple2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var ys = client.CreateHotObservable(
                    OnNext(310, 1),
                    OnNext(320, 2),
                    OnNext(330, 3),
                    OnNext(340, 4),
                    OnNext(350, 5),
                    OnNext(360, 6),
                    OnNext(370, 7),
                    OnNext(380, 8),
                    OnNext(390, 9),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.SequenceEqual(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(400, true),
                    OnCompleted<bool>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void SequenceEqual_Simple3()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var ys = client.CreateHotObservable(
                    OnNext(310, 1),
                    OnNext(320, 2),
                    OnNext(330, 3),
                    OnNext(340, 4),
                    OnNext(350, 5),
                    OnNext(360, 6),
                    OnNext(370, 7),
                    OnNext(380, 8),
                    OnNext(390, 9),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    ys.SequenceEqual(xs)
                );

                res.Messages.AssertEqual(
                    OnNext(400, true),
                    OnCompleted<bool>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void SequenceEqual_Simple4()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var ys = client.CreateHotObservable(
                    OnNext(310, 1),
                    OnNext(320, 2),
                    OnNext(330, 3),
                    OnNext(340, 4),
                    OnNext(350, -5),
                    OnNext(360, 6),
                    OnNext(370, 7),
                    OnNext(380, 8),
                    OnNext(390, 9),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.SequenceEqual(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(350, false),
                    OnCompleted<bool>(350)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 350));
            });
        }

        [TestMethod]
        public void SequenceEqual_Simple5()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, -5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var ys = client.CreateHotObservable(
                    OnNext(310, 1),
                    OnNext(320, 2),
                    OnNext(330, 3),
                    OnNext(340, 4),
                    OnNext(350, 5),
                    OnNext(360, 6),
                    OnNext(370, 7),
                    OnNext(380, 8),
                    OnNext(390, 9),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.SequenceEqual(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(350, false),
                    OnCompleted<bool>(350)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 350));
            });
        }

        [TestMethod]
        public void SequenceEqual_Simple6()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnCompleted<int>(300)
                );

                var ys = client.CreateHotObservable(
                    OnNext(310, 1),
                    OnNext(320, 2),
                    OnNext(330, 3),
                    OnNext(340, 4),
                    OnNext(350, 5),
                    OnNext(360, 6),
                    OnNext(370, 7),
                    OnNext(380, 8),
                    OnNext(390, 9),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.SequenceEqual(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(350, false),
                    OnCompleted<bool>(350)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 350));
            });
        }

        [TestMethod]
        public void SequenceEqual_Simple7()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var ys = client.CreateHotObservable(
                    OnNext(310, 1),
                    OnNext(320, 2),
                    OnNext(330, 3),
                    OnNext(340, 4),
                    OnNext(350, 5),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.SequenceEqual(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(400, false),
                    OnCompleted<bool>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void SequenceEqual_Error1()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnError<int>(300, ex)
                );

                var ys = client.CreateHotObservable(
                    OnNext(310, 1),
                    OnNext(320, 2),
                    OnNext(330, 3),
                    OnNext(340, 4),
                    OnNext(350, 5),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.SequenceEqual(ys)
                );

                res.Messages.AssertEqual(
                    OnError<bool>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void SequenceEqual_Error2()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var ys = client.CreateHotObservable(
                    OnNext(310, 1),
                    OnNext(320, 2),
                    OnNext(330, 3),
                    OnNext(340, 4),
                    OnNext(350, 5),
                    OnError<int>(400, ex)
                );

                var res = client.Start(() =>
                    xs.SequenceEqual(ys)
                );

                res.Messages.AssertEqual(
                    OnError<bool>(400, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }
    }
}
