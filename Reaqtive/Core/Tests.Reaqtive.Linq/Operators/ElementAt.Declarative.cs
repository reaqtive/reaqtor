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
    public partial class ElementAt : OperatorTestBase
    {
        [TestMethod]
        public void ElementAt_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAt(0)
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, e => e is ArgumentOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void ElementAt_One_InRange()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAt(0)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnCompleted<int>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void ElementAt_One_OutOfRange()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAt(1)
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, e => e is ArgumentOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void ElementAt_Many_InRange()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAt(2)
                );

                res.Messages.AssertEqual(
                    OnNext(230, 4),
                    OnCompleted<int>(230)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void ElementAt_Many_OutOfRange()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAt(10)
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, e => e is ArgumentOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void ElementAt_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnError<int>(240, ex)
                );

                var res = client.Start(() =>
                    xs.ElementAt(10)
                );

                res.Messages.AssertEqual(
                    OnError<int>(240, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        [TestMethod]
        public void ElementAt_Throw_AfterMatchingIndex()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnError<int>(240, ex)
                );

                var res = client.Start(() =>
                    xs.ElementAt(1)
                );

                res.Messages.AssertEqual(
                    OnNext(220, 3),
                    OnCompleted<int>(220)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void ElementAtOrDefault_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAtOrDefault(0)
                );

                res.Messages.AssertEqual(
                    OnNext(250, default(int)),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void ElementAtOrDefault_One_InRange()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAtOrDefault(0)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnCompleted<int>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void ElementAtOrDefault_One_OutOfRange()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAtOrDefault(1)
                );

                res.Messages.AssertEqual(
                    OnNext(250, default(int)),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void ElementAtOrDefault_Many_InRange()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAtOrDefault(2)
                );

                res.Messages.AssertEqual(
                    OnNext(230, 4),
                    OnCompleted<int>(230)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void ElementAtOrDefault_Many_OutOfRange()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.ElementAtOrDefault(10)
                );

                res.Messages.AssertEqual(
                    OnNext(250, default(int)),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void ElementAtOrDefault_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnError<int>(240, ex)
                );

                var res = client.Start(() =>
                    xs.ElementAtOrDefault(10)
                );

                res.Messages.AssertEqual(
                    OnError<int>(240, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        [TestMethod]
        public void ElementAtOrDefault_Throw_AfterMatchingIndex()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnError<int>(240, ex)
                );

                var res = client.Start(() =>
                    xs.ElementAtOrDefault(1)
                );

                res.Messages.AssertEqual(
                    OnNext(220, 3),
                    OnCompleted<int>(220)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }
    }
}
