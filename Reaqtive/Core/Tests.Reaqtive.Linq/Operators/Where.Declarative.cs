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
    public partial class Where : OperatorTestBase
    {
        [TestMethod]
        public void Where_Complete_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where(x => IsPrime(x))
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(340, 5),
                    OnNext(390, 7),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }

        [TestMethod]
        public void Where_True_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where(x => true)
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }

        [TestMethod]
        public void Where_False_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where(x => false)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }

        [TestMethod]
        public void Where_Dispose_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where(x => IsPrime(x)),
                    400
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(340, 5),
                    OnNext(390, 7)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void Where_Error_Declarative()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnError<int>(600, ex),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where(x => IsPrime(x))
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(340, 5),
                    OnNext(390, 7),
                    OnNext(580, 11),
                    OnError<int>(600, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }

        [TestMethod]
        public void WhereWhereOptimization_Regular()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where(x => x > 3).Where(x => x % 2 == 0)
                );

                res.Messages.AssertEqual(
                    OnNext(270, 4),
                    OnNext(380, 6),
                    OnNext(450, 8),
                    OnNext(560, 10),
                    OnCompleted<int>(600)
                );
            });
        }

        [TestMethod]
        public void WhereIndex_Complete_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) => IsPrime(x + i * 10))
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(390, 7),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }

        [TestMethod]
        public void WhereIndex_True_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) => true)
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }

        [TestMethod]
        public void WhereIndex_False_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) => false)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }

        [TestMethod]
        public void WhereIndex_Dispose_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnCompleted<int>(600)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) => IsPrime(x + i * 10)),
                    400
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(390, 7)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void WhereIndex_Error_Declarative()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(110, 1),
                    OnNext(180, 2),
                    OnNext(230, 3),
                    OnNext(270, 4),
                    OnNext(340, 5),
                    OnNext(380, 6),
                    OnNext(390, 7),
                    OnNext(450, 8),
                    OnNext(470, 9),
                    OnNext(560, 10),
                    OnNext(580, 11),
                    OnError<int>(600, ex),
                    OnNext(610, 12),
                    OnError<int>(620, new Exception()),
                    OnCompleted<int>(630)
                );

                var res = client.Start(() =>
                    xs.Where((x, i) => IsPrime(x + i * 10))
                );

                res.Messages.AssertEqual(
                    OnNext(230, 3),
                    OnNext(390, 7),
                    OnError<int>(600, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }
    }
}
