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
    public partial class SingleAsync : OperatorTestBase
    {
        [TestMethod]
        public void SingleAsync_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleAsync()
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, e => e is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SingleAsync_One()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleAsync()
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
        public void SingleAsync_Many()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleAsync()
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
        public void SingleAsync_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(210, ex)
                );

                var res = client.Start(() =>
                    xs.SingleAsync()
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
        public void SingleAsync_Predicate_One()
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
                    xs.SingleAsync(x => x == 4)
                );

                res.Messages.AssertEqual(
                    OnNext(250, 4),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SingleAsync_Predicate_Many()
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
                    xs.SingleAsync(x => x % 2 == 1)
                );

                res.Messages.AssertEqual(
                    OnError<int>(240, e => e is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        [TestMethod]
        public void SingleAsync_Predicate_None()
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
                    xs.SingleAsync(x => x > 10)
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, e => e is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SingleAsync_Predicate_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    xs.SingleAsync(x => x % 2 == 1)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }
    }
}
