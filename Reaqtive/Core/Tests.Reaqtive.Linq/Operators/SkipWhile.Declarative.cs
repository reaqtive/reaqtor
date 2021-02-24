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
    public partial class SkipWhile : OperatorTestBase
    {
        [TestMethod]
        public void SkipWhile_Complete_Before_Declarative()
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
                    xs.SkipWhile(x => IsPrime(x))
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(330)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 330)
                );
            });
        }

        [TestMethod]
        public void SkipWhile_Complete_After_Declarative()
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
                    xs.SkipWhile(x => IsPrime(x))
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
            });
        }

        [TestMethod]
        public void SkipWhile_Error_Before_Declarative()
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
                    xs.SkipWhile(x => IsPrime(x))
                );

                res.Messages.AssertEqual(
                    OnError<int>(270, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void SkipWhile_Error_After_Declarative()
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

                var res = client.Start(() =>
                    xs.SkipWhile(x => IsPrime(x))
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
            });
        }

        [TestMethod]
        public void SkipWhile_Dispose_Before_Declarative()
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
                    xs.SkipWhile(x => IsPrime(x)),
                    300
                );

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void SkipWhile_Dispose_After_Declarative()
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
                    xs.SkipWhile(x => IsPrime(x)),
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
            });
        }

        [TestMethod]
        public void SkipWhile_Zero_Declarative()
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
                    xs.SkipWhile(x => IsPrime(x))
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
            });
        }

        [TestMethod]
        public void SkipWhile_Index()
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
                    xs.SkipWhile((x, i) => i < 5)
                );

                res.Messages.AssertEqual(
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
            });
        }

        [TestMethod]
        public void SkipWhile_Index_Throw()
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
                    xs.SkipWhile((x, i) => i < 5)
                );

                res.Messages.AssertEqual(
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
