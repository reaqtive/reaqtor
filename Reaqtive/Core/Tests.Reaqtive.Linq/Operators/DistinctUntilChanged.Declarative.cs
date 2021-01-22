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
    public partial class DistinctUntilChanged : OperatorTestBase
    {
        [TestMethod]
        public void DistinctUntilChanged_Never()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>();

                var res = client.Start(() =>
                    xs.DistinctUntilChanged()
                );

                res.Messages.AssertEqual(
                );
            });
        }

        [TestMethod]
        public void DistinctUntilChanged_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.DistinctUntilChanged()
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
        public void DistinctUntilChanged_Return()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.DistinctUntilChanged()
                );

                res.Messages.AssertEqual(
                    OnNext(220, 2),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void DistinctUntilChanged_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(250, ex)
                );

                var res = client.Start(() =>
                    xs.DistinctUntilChanged()
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void DistinctUntilChanged_AllChanges()
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
                    xs.DistinctUntilChanged()
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void DistinctUntilChanged_AllSame()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 2),
                    OnNext(230, 2),
                    OnNext(240, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.DistinctUntilChanged()
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void DistinctUntilChanged_SomeChanges()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2), //*
                    OnNext(215, 3), //*
                    OnNext(220, 3),
                    OnNext(225, 2), //*
                    OnNext(230, 2),
                    OnNext(235, 1), //*
                    OnNext(240, 2), //*
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.DistinctUntilChanged()
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(215, 3),
                    OnNext(225, 2),
                    OnNext(235, 1),
                    OnNext(240, 2),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void DistinctUntilChanged_KeySelector_Div2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2), //*
                    OnNext(220, 4),
                    OnNext(230, 3), //*
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.DistinctUntilChanged(x => x % 2)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(230, 3),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }
    }
}
