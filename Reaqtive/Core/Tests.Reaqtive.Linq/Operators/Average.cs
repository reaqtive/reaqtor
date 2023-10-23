// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Average : OperatorTestBase
    {
        [TestInitialize]
        public void Initialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        public void AverageInt32_Overflow1()
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

                var res = client.CreateObserver<double>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    InitializeSubscription(sub, client);

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_sum", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 8);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<double>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void AverageInt32_Overflow2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, -2),
                    OnNext<int>(20, -3),
                    OnNext<int>(30, -5),
                    OnNext<int>(40, -7),
                    OnCompleted<int>(50)
                );

                var res = client.CreateObserver<double>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    InitializeSubscription(sub, client);

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_sum", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MinValue + 8);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<double>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
    }
}
