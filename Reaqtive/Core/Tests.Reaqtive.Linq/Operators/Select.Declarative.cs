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
    public partial class Select : OperatorTestBase
    {
        [TestMethod]
        public void Select_Completed_Declarative()
        {
            Run(client =>
            {

                var xs = client.CreateHotObservable(
                    OnNext(180, 1),
                    OnNext(210, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(350, 5),
                    OnCompleted<int>(400),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var res = client.Start(() =>
                    xs.Select(x => x + 1)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 3),
                    OnNext(240, 4),
                    OnNext(290, 5),
                    OnNext(350, 6),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void Select_NotCompleted_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(180, 1),
                    OnNext(210, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(350, 5)
                );

                var res = client.Start(() =>
                    xs.Select(x => x + 1)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 3),
                    OnNext(240, 4),
                    OnNext(290, 5),
                    OnNext(350, 6)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void Select_Error_Declarative()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(180, 1),
                    OnNext(210, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(350, 5),
                    OnError<int>(400, ex),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var res = client.Start(() =>
                    xs.Select(x => x + 1)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 3),
                    OnNext(240, 4),
                    OnNext(290, 5),
                    OnNext(350, 6),
                    OnError<int>(400, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void SelectWithIndex_Completed_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnCompleted<int>(400),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var res = client.Start(() =>
                    xs.Select((x, index) => (x + 1) + (index * 10))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 5),
                    OnNext(240, 14),
                    OnNext(290, 23),
                    OnNext(350, 32),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void SelectWithIndex_NotCompleted_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1)
                );

                var res = client.Start(() =>
                    xs.Select((x, index) => (x + 1) + (index * 10))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 5),
                    OnNext(240, 14),
                    OnNext(290, 23),
                    OnNext(350, 32)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void SelectWithIndex_Error_Declarative()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnError<int>(400, ex),
                    OnNext(410, -1),
                    OnCompleted<int>(420),
                    OnError<int>(430, new Exception())
                );

                var res = client.Start(() =>
                    xs.Select((x, index) => (x + 1) + (index * 10))
                );

                res.Messages.AssertEqual(
                    OnNext(210, 5),
                    OnNext(240, 14),
                    OnNext(290, 23),
                    OnNext(350, 32),
                    OnError<int>(400, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void Select_Select1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Select(x => x + 1).Select(x => x - 2)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 4 + 1 - 2),
                    OnNext(240, 3 + 1 - 2),
                    OnNext(290, 2 + 1 - 2),
                    OnNext(350, 1 + 1 - 2),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void Select_Select2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Select((x, i) => x + i).Select(x => x - 2)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 4 + 0 - 2),
                    OnNext(240, 3 + 1 - 2),
                    OnNext(290, 2 + 2 - 2),
                    OnNext(350, 1 + 3 - 2),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }

        [TestMethod]
        public void Select_Select3()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Select(x => x + 1).Select((x, i) => x - i)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 4 + 1 - 0),
                    OnNext(240, 3 + 1 - 1),
                    OnNext(290, 2 + 1 - 2),
                    OnNext(350, 1 + 1 - 3),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void Select_Select4()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(180, 5),
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Select((x, i) => x + i).Select((x, i) => x - i)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 4),
                    OnNext(240, 3),
                    OnNext(290, 2),
                    OnNext(350, 1),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }
    }
}
