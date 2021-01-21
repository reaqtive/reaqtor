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
    public partial class TakeWhile
    {
        [TestMethod]
        public void TakeWhile_Complete_Before_Declarative()
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


                var res = client.Start(() =>
                    xs.TakeWhile(x => IsPrime(x))
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
            });
        }

        [TestMethod]
        public void TakeWhile_Complete_After_Declarative()
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


                var res = client.Start(() =>
                    xs.TakeWhile(x => IsPrime(x))
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
            });
        }

        [TestMethod]
        public void TakeWhile_Error_Before_Declarative()
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

                var res = client.Start(() =>
                    xs.TakeWhile(x => IsPrime(x))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnError<int>(270, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void TakeWhile_Error_After_Declarative()
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

                var res = client.Start(() =>
                    xs.TakeWhile(x => IsPrime(x))
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
            });
        }

        [TestMethod]
        public void TakeWhile_Dispose_Before_Declarative()
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


                var res = client.Start(() =>
                    xs.TakeWhile(x => IsPrime(x)),
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
            });
        }

        [TestMethod]
        public void TakeWhile_Dispose_After_Declarative()
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

                var res = client.Start(() =>
                    xs.TakeWhile(x => IsPrime(x)),
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
            });
        }

        [TestMethod]
        public void TakeWhile_Zero_Declarative()
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

                var res = client.Start(() =>
                    xs.TakeWhile(x => IsPrime(x)),
                    300
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(205)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 205)
                );
            });
        }

        [TestMethod]
        public void TakeWhile_Index1_Declarative()
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

                var res = client.Start(() =>
                    xs.TakeWhile((x, i) => i < 5)
                );

                res.Messages.AssertEqual(
                    OnNext(205, 100),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnCompleted<int>(350)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 350)
                );
            });
        }

        [TestMethod]
        public void TakeWhile_Index2_Declarative()
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
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.TakeWhile((x, i) => i >= 0)
                );

                res.Messages.AssertEqual(
                    OnNext(205, 100),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }

        [TestMethod]
        public void TakeWhile_Index_Throw_Declarative()
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
                    OnError<int>(400, ex)
                );

                var res = client.Start(() =>
                    xs.TakeWhile((x, i) => i >= 0)
                );

                res.Messages.AssertEqual(
                    OnNext(205, 100),
                    OnNext(210, 2),
                    OnNext(260, 5),
                    OnNext(290, 13),
                    OnNext(320, 3),
                    OnNext(350, 7),
                    OnNext(390, 4),
                    OnError<int>(400, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }
    }
}
