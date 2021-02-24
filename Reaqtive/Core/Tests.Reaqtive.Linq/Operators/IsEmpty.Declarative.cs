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
    public partial class IsEmpty : OperatorTestBase
    {
        [TestMethod]
        public void IsEmpty_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.IsEmpty()
                );

                res.Messages.AssertEqual(
                    OnNext<bool>(250, true),
                    OnCompleted<bool>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void IsEmpty_NonEmpty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 3),
                    OnNext<int>(30, 5),
                    OnNext<int>(40, 7),
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.IsEmpty()
                );

                res.Messages.AssertEqual(
                    OnNext<bool>(210, false),
                    OnCompleted<bool>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void IsEmpty_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<int>(50, ex)
                );

                var res = client.Start(() =>
                    xs.IsEmpty()
                );

                res.Messages.AssertEqual(
                    OnError<bool>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }
    }
}
