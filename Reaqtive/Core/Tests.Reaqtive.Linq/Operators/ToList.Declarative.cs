// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

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
    public partial class ToList : OperatorTestBase
    {
        [TestMethod]
        public void ToList_Simple1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.ToList()
                );

                res.Messages.AssertEqual(
                    OnNext<IList<int>>(250, l => l.SequenceEqual(Array.Empty<int>())),
                    OnCompleted<IList<int>>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void ToList_Simple2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 42),
                    OnNext<int>(20, 43),
                    OnNext<int>(30, 44),
                    OnNext<int>(40, 45),
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.ToList()
                );

                res.Messages.AssertEqual(
                    OnNext<IList<int>>(250, l => l.SequenceEqual(new[] { 42, 43, 44, 45 })),
                    OnCompleted<IList<int>>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void ToList_Simple3()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 42),
                    OnNext<int>(20, 43),
                    OnNext<int>(30, 44),
                    OnNext<int>(40, 45),
                    OnError<int>(50, ex)
                );

                var res = client.Start(() =>
                    xs.ToList()
                );

                res.Messages.AssertEqual(
                    OnError<IList<int>>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }
    }
}
