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
    public partial class Throttle : OperatorTestBase
    {
        [TestMethod]
        public void Throttle_TimeSpan_AllPass()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 0),
                    OnNext(210, 1),
                    OnNext(240, 2),
                    OnNext(270, 3),
                    OnNext(300, 4),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Throttle(TimeSpan.FromTicks(20))
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1),
                    OnNext(260, 2),
                    OnNext(290, 3),
                    OnNext(320, 4),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }

        [TestMethod]
        public void Throttle_TimeSpan_AllPass_ErrorEnd()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 0),
                    OnNext(210, 1),
                    OnNext(240, 2),
                    OnNext(270, 3),
                    OnNext(300, 4),
                    OnError<int>(400, ex)
                );

                var res = client.Start(() =>
                    xs.Throttle(TimeSpan.FromTicks(20))
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1),
                    OnNext(260, 2),
                    OnNext(290, 3),
                    OnNext(320, 4),
                    OnError<int>(400, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }

        [TestMethod]
        public void Throttle_TimeSpan_AllDrop()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 0),
                    OnNext(210, 1),
                    OnNext(240, 2),
                    OnNext(270, 3),
                    OnNext(300, 4),
                    OnNext(330, 5),
                    OnNext(360, 6),
                    OnNext(390, 7),
                    OnCompleted<int>(400)
                );

                var res = client.Start(() =>
                    xs.Throttle(TimeSpan.FromTicks(40))
                );

                res.Messages.AssertEqual(
                    OnNext(400, 7),
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }

        [TestMethod]
        public void Throttle_TimeSpan_AllDrop_ErrorEnd()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 0),
                    OnNext(210, 1),
                    OnNext(240, 2),
                    OnNext(270, 3),
                    OnNext(300, 4),
                    OnNext(330, 5),
                    OnNext(360, 6),
                    OnNext(390, 7),
                    OnError<int>(400, ex)
                );

                var res = client.Start(() =>
                    xs.Throttle(TimeSpan.FromTicks(40))
                );

                res.Messages.AssertEqual(
                    OnError<int>(400, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );
            });
        }

        [TestMethod]
        public void Throttle_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 0),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Throttle(TimeSpan.FromTicks(10))
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void Throttle_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 0),
                    OnError<int>(300, ex)
                );

                var res = client.Start(() =>
                    xs.Throttle(TimeSpan.FromTicks(10))
                );

                res.Messages.AssertEqual(
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void Throttle_Never()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 0)
                );

                var res = client.Start(() =>
                    xs.Throttle(TimeSpan.FromTicks(10))
                );

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void Throttle_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 0),
                    OnNext(210, 1),
                    OnNext(240, 2),
                    OnNext(250, 3),
                    OnNext(280, 4),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Throttle(TimeSpan.FromTicks(20))
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1),
                    OnNext(270, 3),
                    OnNext(300, 4),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }
    }
}
