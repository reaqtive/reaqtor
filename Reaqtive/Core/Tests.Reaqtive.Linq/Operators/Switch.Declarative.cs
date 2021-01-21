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
    public partial class Switch : OperatorTestBase
    {
        [TestMethod]
        public void Switch_Data()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

                var ys1 = client.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(110, 103),
                    OnNext(120, 104),
                    OnNext(210, 105),
                    OnNext(220, 106),
                    OnCompleted<int>(230)
                );

                var ys2 = client.CreateColdObservable(
                    OnNext(10, 201),
                    OnNext(20, 202),
                    OnNext(30, 203),
                    OnNext(40, 204),
                    OnCompleted<int>(50)
                );

                var ys3 = client.CreateColdObservable(
                    OnNext(10, 301),
                    OnNext(20, 302),
                    OnNext(30, 303),
                    OnNext(40, 304),
                    OnCompleted<int>(150)
                );

                var xs = client.CreateHotObservable(
                    OnNext(300, ys1),
                    OnNext(400, ys2),
                    OnNext(500, ys3),
                    OnCompleted(600, witness)
                );

                var res = client.Start(() =>
                    xs.Switch()
                );

                res.Messages.AssertEqual(
                    OnNext(310, 101),
                    OnNext(320, 102),
                    OnNext(410, 201),
                    OnNext(420, 202),
                    OnNext(430, 203),
                    OnNext(440, 204),
                    OnNext(510, 301),
                    OnNext(520, 302),
                    OnNext(530, 303),
                    OnNext(540, 304),
                    OnCompleted<int>(650)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600)
                );

                ys1.Subscriptions.AssertEqual(
                    Subscribe(300, 400)
                );

                ys2.Subscriptions.AssertEqual(
                    Subscribe(400, 450)
                );

                ys3.Subscriptions.AssertEqual(
                    Subscribe(500, 650)
                );
            });
        }

        [TestMethod]
        public void Switch_InnerThrows()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

                var ex = new Exception();

                var ys1 = client.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(110, 103),
                    OnNext(120, 104),
                    OnNext(210, 105),
                    OnNext(220, 106),
                    OnCompleted<int>(230)
                );

                var ys2 = client.CreateColdObservable(
                    OnNext(10, 201),
                    OnNext(20, 202),
                    OnNext(30, 203),
                    OnNext(40, 204),
                    OnError<int>(50, ex)
                );

                var ys3 = client.CreateColdObservable(
                    OnNext(10, 301),
                    OnNext(20, 302),
                    OnNext(30, 303),
                    OnNext(40, 304),
                    OnCompleted<int>(150)
                );

                var xs = client.CreateHotObservable(
                    OnNext(300, ys1),
                    OnNext(400, ys2),
                    OnNext(500, ys3),
                    OnCompleted(600, witness)
                );

                var res = client.Start(() =>
                    xs.Switch()
                );

                res.Messages.AssertEqual(
                    OnNext(310, 101),
                    OnNext(320, 102),
                    OnNext(410, 201),
                    OnNext(420, 202),
                    OnNext(430, 203),
                    OnNext(440, 204),
                    OnError<int>(450, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 450)
                );

                ys1.Subscriptions.AssertEqual(
                    Subscribe(300, 400)
                );

                ys2.Subscriptions.AssertEqual(
                    Subscribe(400, 450)
                );

                ys3.Subscriptions.AssertEqual(
                );
            });
        }

        [TestMethod]
        public void Switch_OuterThrows()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

                var ex = new Exception();

                var ys1 = client.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(110, 103),
                    OnNext(120, 104),
                    OnNext(210, 105),
                    OnNext(220, 106),
                    OnCompleted<int>(230)
                );

                var ys2 = client.CreateColdObservable(
                    OnNext(10, 201),
                    OnNext(20, 202),
                    OnNext(30, 203),
                    OnNext(40, 204),
                    OnCompleted<int>(50)
                );

                var xs = client.CreateHotObservable(
                    OnNext(300, ys1),
                    OnNext(400, ys2),
                    OnError(500, ex, witness)
                );

                var res = client.Start(() =>
                    xs.Switch()
                );

                res.Messages.AssertEqual(
                    OnNext(310, 101),
                    OnNext(320, 102),
                    OnNext(410, 201),
                    OnNext(420, 202),
                    OnNext(430, 203),
                    OnNext(440, 204),
                    OnError<int>(500, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 500)
                );

                ys1.Subscriptions.AssertEqual(
                    Subscribe(300, 400)
                );

                ys2.Subscriptions.AssertEqual(
                    Subscribe(400, 450)
                );
            });
        }

        [TestMethod]
        public void Switch_NoInner()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

                var xs = client.CreateHotObservable(
                    OnCompleted(500, witness)
                );

                var res = client.Start(() =>
                    xs.Switch()
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 500)
                );
            });
        }

        [TestMethod]
        public void Switch_InnerCompletes()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

                var ys1 = client.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
                    OnNext(110, 103),
                    OnNext(120, 104),
                    OnNext(210, 105),
                    OnNext(220, 106),
                    OnCompleted<int>(230)
                );

                var xs = client.CreateHotObservable(
                    OnNext(300, ys1),
                    OnCompleted(540, witness)
                );

                var res = client.Start(() =>
                    xs.Switch()
                );

                res.Messages.AssertEqual(
                    OnNext(310, 101),
                    OnNext(320, 102),
                    OnNext(410, 103),
                    OnNext(420, 104),
                    OnNext(510, 105),
                    OnNext(520, 106),
                    OnCompleted<int>(540)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 540)
                );

                ys1.Subscriptions.AssertEqual(
                    Subscribe(300, 530)
                );
            });
        }
    }
}
