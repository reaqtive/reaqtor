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
    public partial class Contains : OperatorTestBase
    {
        [TestMethod]
        public void Contains_Never()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>();

                var res = client.Start(() =>
                    xs.Contains(2)
                );

                res.Messages.AssertEqual(
                );
            });
        }

        [TestMethod]
        public void Contains_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Contains(2)
                );

                res.Messages.AssertEqual(
                    OnNext(250, false),
                    OnCompleted<bool>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Contains_One_Matching()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Contains(2)
                );

                res.Messages.AssertEqual(
                    OnNext(210, true),
                    OnCompleted<bool>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Contains_One_NotMatching()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 3),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Contains(2)
                );

                res.Messages.AssertEqual(
                    OnNext(250, false),
                    OnCompleted<bool>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Contains_AllMatching()
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
                    xs.Contains(2)
                );

                res.Messages.AssertEqual(
                    OnNext(210, true),
                    OnCompleted<bool>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Contains_NoneMatching()
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
                    xs.Contains(6)
                );

                res.Messages.AssertEqual(
                    OnNext(250, false),
                    OnCompleted<bool>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Contains_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(210, ex)
                );

                var res = client.Start(() =>
                    xs.Contains(2)
                );

                res.Messages.AssertEqual(
                    OnError<bool>(210, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Contains_Throw_AfterMatch()
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
                    xs.Contains(2)
                );

                res.Messages.AssertEqual(
                    OnNext(210, true),
                    OnCompleted<bool>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }
    }
}
