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
    public partial class Sample : OperatorTestBase
    {
        [TestMethod]
        public void Sample_Regular()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(230, 3),
                    OnNext(260, 4),
                    OnNext(300, 5),
                    OnNext(350, 6),
                    OnNext(380, 7),
                    OnCompleted<int>(390)
                );

                var res = client.Start(() =>
                    xs.Sample(TimeSpan.FromTicks(50))
                );

                res.Messages.AssertEqual(
                    OnNext(250, 3),
                    OnNext(300, 5), /* CHECK: boundary of sampling */
                    OnNext(350, 6),
                    OnNext(400, 7), /* Sample in last bucket */
                    OnCompleted<int>(400)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 390)
                );
            });
        }

        [TestMethod]
        public void Sample_ErrorInFlight()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(230, 3),
                    OnNext(260, 4),
                    OnNext(300, 5),
                    OnNext(310, 6),
                    OnError<int>(330, ex)
                );

                var res = client.Start(() =>
                    xs.Sample(TimeSpan.FromTicks(50))
                );

                res.Messages.AssertEqual(
                    OnNext(250, 3),
                    OnNext(300, 5), /* CHECK: boundary of sampling */
                    OnError<int>(330, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 330)
                );
            });
        }

        [TestMethod]
        public void Sample_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Sample(TimeSpan.FromTicks(50))
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
        public void Sample_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(300, ex)
                );

                var res = client.Start(() =>
                    xs.Sample(TimeSpan.FromTicks(50))
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
        public void Sample_Never()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var res = client.Start(() =>
                    xs.Sample(TimeSpan.FromTicks(50))
                );

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void Sample_Sampler_Simple1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(300, 5),
                    OnNext(310, 6),
                    OnCompleted<int>(400)
                );

                var ys = client.CreateHotObservable(
                    OnNext(150, ""),
                    OnNext(210, "bar"),
                    OnNext(250, "foo"),
                    OnNext(260, "qux"),
                    OnNext(320, "baz"),
                    OnCompleted<string>(500)
                );

                var res = client.Start(() =>
                    xs.Sample(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(250, 3),
                    OnNext(320, 6),
                    OnCompleted<int>(500 /* on sampling boundaries only */)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 500)
                );
            });
        }

        [TestMethod]
        public void Sample_Sampler_Simple2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(300, 5),
                    OnNext(310, 6),
                    OnNext(360, 7),
                    OnCompleted<int>(400)
                );

                var ys = client.CreateHotObservable(
                    OnNext(150, ""),
                    OnNext(210, "bar"),
                    OnNext(250, "foo"),
                    OnNext(260, "qux"),
                    OnNext(320, "baz"),
                    OnCompleted<string>(500)
                );

                var res = client.Start(() =>
                    xs.Sample(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(250, 3),
                    OnNext(320, 6),
                    OnNext(500, 7),
                    OnCompleted<int>(500 /* on sampling boundaries only */)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400)
                );

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 500)
                );
            });
        }

        [TestMethod]
        public void Sample_Sampler_Simple3()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnCompleted<int>(300)
                );

                var ys = client.CreateHotObservable(
                    OnNext(150, ""),
                    OnNext(210, "bar"),
                    OnNext(250, "foo"),
                    OnNext(260, "qux"),
                    OnNext(320, "baz"),
                    OnCompleted<string>(500)
                );

                var res = client.Start(() =>
                    xs.Sample(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(250, 3),
                    OnNext(320, 4),
                    OnCompleted<int>(320 /* on sampling boundaries only */)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 320)
                );
            });
        }

        [TestMethod]
        public void Sample_Sampler_SourceThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(300, 5),
                    OnNext(310, 6),
                    OnError<int>(320, ex)
                );

                var ys = client.CreateHotObservable(
                    OnNext(150, ""),
                    OnNext(210, "bar"),
                    OnNext(250, "foo"),
                    OnNext(260, "qux"),
                    OnNext(330, "baz"),
                    OnCompleted<string>(400)
                );

                var res = client.Start(() =>
                    xs.Sample(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(250, 3),
                    OnError<int>(320, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 320)
                );

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 320)
                );
            });
        }

        [TestMethod]
        public void Sample_Sampler_SamplerThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 2),
                    OnNext(240, 3),
                    OnNext(290, 4),
                    OnNext(300, 5),
                    OnNext(310, 6),
                    OnCompleted<int>(400)
                );

                var ys = client.CreateHotObservable(
                    OnNext(150, ""),
                    OnNext(210, "bar"),
                    OnNext(250, "foo"),
                    OnNext(260, "qux"),
                    OnError<string>(320, ex)
                );

                var res = client.Start(() =>
                    xs.Sample(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(250, 3),
                    OnError<int>(320, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 320)
                );

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 320)
                );
            });
        }
    }
}
