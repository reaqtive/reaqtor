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
    public partial class IgnoreElements : OperatorTestBase
    {
        [TestMethod]
        public void IgnoreElements_Complete()
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
                    xs.IgnoreElements()
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(600)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }

        [TestMethod]
        public void IgnoreElements_Dispose()
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
                    xs.IgnoreElements(),
                    400
                );

                res.Messages.AssertEqual(
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 400));
            });
        }

        [TestMethod]
        public void IgnoreElements_Error()
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
                    xs.IgnoreElements()
                );

                res.Messages.AssertEqual(
                    OnError<int>(600, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 600));
            });
        }
    }
}
