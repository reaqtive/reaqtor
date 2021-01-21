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
    public partial class Merge : OperatorTestBase
    {
        [TestMethod]
        public void Merge_ObservableOfObservable_Data_Declarative()
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
                    OnNext(120, 305),
                    OnCompleted<int>(150)
                );

                var xs = client.CreateHotObservable(
                    OnNext(300, ys1),
                    OnNext(400, ys2),
                    OnNext(500, ys3),
                    OnCompleted(600, witness)
                );

                var res = client.Start(() =>
                    xs.Merge()
                );

                res.Messages.AssertEqual(
                    OnNext(310, 101),
                    OnNext(320, 102),
                    OnNext(410, 103),
                    OnNext(410, 201),
                    OnNext(420, 104),
                    OnNext(420, 202),
                    OnNext(430, 203),
                    OnNext(440, 204),
                    OnNext(510, 105),
                    OnNext(510, 301),
                    OnNext(520, 106),
                    OnNext(520, 302),
                    OnNext(530, 303),
                    OnNext(540, 304),
                    OnNext(620, 305),
                    OnCompleted<int>(650)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600)
                );

                ys1.Subscriptions.AssertEqual(
                    Subscribe(300, 530)
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
        public void Merge_ObservableOfObservable_Data_NonOverlapped_Declarative()
        {
            Run(client =>
            {
                var witness = GetObservableWitness<int>();

                var ys1 = client.CreateColdObservable(
                    OnNext(10, 101),
                    OnNext(20, 102),
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
                    OnCompleted<int>(50)
                );

                var xs = client.CreateHotObservable(
                    OnNext(300, ys1),
                    OnNext(400, ys2),
                    OnNext(500, ys3),
                    OnCompleted(600, witness)
                );

                var res = client.Start(() =>
                    xs.Merge()
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
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600)
                );

                ys1.Subscriptions.AssertEqual(
                    Subscribe(300, 530)
                );

                ys2.Subscriptions.AssertEqual(
                    Subscribe(400, 450)
                );

                ys3.Subscriptions.AssertEqual(
                    Subscribe(500, 550)
                );
            });
        }

        [TestMethod]
        public void Merge_ObservableOfObservable_InnerThrows_Declarative()
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
                    xs.Merge()
                );

                res.Messages.AssertEqual(
                    OnNext(310, 101),
                    OnNext(320, 102),
                    OnNext(410, 103),
                    OnNext(410, 201),
                    OnNext(420, 104),
                    OnNext(420, 202),
                    OnNext(430, 203),
                    OnNext(440, 204),
                    OnError<int>(450, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 450)
                );

                ys1.Subscriptions.AssertEqual(
                    Subscribe(300, 450)
                );

                ys2.Subscriptions.AssertEqual(
                    Subscribe(400, 450)
                );

                ys3.Subscriptions.AssertEqual(
                );
            });
        }

        [TestMethod]
        public void Merge_ObservableOfObservable_OuterThrows_Declarative()
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
                    xs.Merge()
                );

                res.Messages.AssertEqual(
                    OnNext(310, 101),
                    OnNext(320, 102),
                    OnNext(410, 103),
                    OnNext(410, 201),
                    OnNext(420, 104),
                    OnNext(420, 202),
                    OnNext(430, 203),
                    OnNext(440, 204),
                    OnError<int>(500, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 500)
                );

                ys1.Subscriptions.AssertEqual(
                    Subscribe(300, 500)
                );

                ys2.Subscriptions.AssertEqual(
                    Subscribe(400, 450)
                );
            });
        }

        [TestMethod]
        public void Merge_Binary_Data_Declarative()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 101),
                    OnNext(230, 102),
                    OnNext(310, 103),
                    OnNext(320, 104),
                    OnNext(410, 105),
                    OnNext(420, 106),
                    OnCompleted<int>(430)
                );

                var ys = client.CreateHotObservable(
                    OnNext(220, 201),
                    OnNext(240, 202),
                    OnNext(340, 203),
                    OnCompleted<int>(450)
                );

                var res = client.Start(() =>
                    xs.Merge(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 101),
                    OnNext(220, 201),
                    OnNext(230, 102),
                    OnNext(240, 202),
                    OnNext(310, 103),
                    OnNext(320, 104),
                    OnNext(340, 203),
                    OnNext(410, 105),
                    OnNext(420, 106),
                    OnCompleted<int>(450)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 430)
                );

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 450)
                );
            });
        }

        [TestMethod]
        public void Merge_Binary_Error_Declarative()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(210, 101),
                    OnError<int>(230, ex)
                );

                var ys = client.CreateHotObservable(
                    OnNext(220, 201),
                    OnNext(240, 202),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Merge(ys)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 101),
                    OnNext(220, 201),
                    OnError<int>(230, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );

                ys.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
    }
}
