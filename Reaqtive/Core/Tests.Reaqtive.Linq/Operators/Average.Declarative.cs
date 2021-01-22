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
    public partial class Average : OperatorTestBase
    {
        [TestMethod]
        public void AverageInt64_Overflow1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<long>(10, long.MaxValue - 10),
                    OnNext<long>(20, 2),
                    OnNext<long>(30, 3),
                    OnNext<long>(40, 6),
                    OnCompleted<long>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<double>(240, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        [TestMethod]
        public void AverageInt64_Overflow2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<long>(10, long.MinValue + 10),
                    OnNext<long>(20, -2),
                    OnNext<long>(30, -3),
                    OnNext<long>(40, -6),
                    OnCompleted<long>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<double>(240, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        [TestMethod]
        public void AverageDecimal_Overflow1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<decimal>(10, decimal.MaxValue - 10m),
                    OnNext<decimal>(20, 2m),
                    OnNext<decimal>(30, 3m),
                    OnNext<decimal>(40, 6m),
                    OnCompleted<decimal>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<decimal>(240, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        [TestMethod]
        public void AverageDecimal_Overflow2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<decimal>(10, decimal.MinValue + 10m),
                    OnNext<decimal>(20, -2m),
                    OnNext<decimal>(30, -3m),
                    OnNext<decimal>(40, -6m),
                    OnCompleted<decimal>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<decimal>(240, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }
    }
}
