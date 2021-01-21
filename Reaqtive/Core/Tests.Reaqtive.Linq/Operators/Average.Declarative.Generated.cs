// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

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
    public partial class Average
    {
        [TestMethod]
        public void AverageInt32_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Int32>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt32_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Int32>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt32_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Int32>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt32_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Int32>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt32_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32[]>(50, new Int32[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt32_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32>(10, 17),
                    OnNext<Int32>(20, -8),
                    OnNext<Int32>(30, 25),
                    OnNext<Int32>(40, 2),
                    OnNext<Int32>(50, 3),
                    OnNext<Int32>(60, -5),
                    OnNext<Int32>(70, -7),
                    OnNext<Int32>(80, 36),
                    OnCompleted<Int32>(90)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(200 + 90, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void AverageInt32_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Int32>>(10, Tuple.Create<Int32>(17)),
                    OnNext<Tuple<Int32>>(20, Tuple.Create<Int32>(-8)),
                    OnNext<Tuple<Int32>>(30, Tuple.Create<Int32>(25)),
                    OnNext<Tuple<Int32>>(40, Tuple.Create<Int32>(2)),
                    OnNext<Tuple<Int32>>(50, Tuple.Create<Int32>(3)),
                    OnNext<Tuple<Int32>>(60, Tuple.Create<Int32>(-5)),
                    OnNext<Tuple<Int32>>(70, Tuple.Create<Int32>(-7)),
                    OnNext<Tuple<Int32>>(80, Tuple.Create<Int32>(36)),
                    OnCompleted<Tuple<Int32>>(90)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(200 + 90, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageInt32_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32>(10, 17),
                    OnNext<Int32>(20, -8),
                    OnNext<Int32>(30, 25),
                    OnNext<Int32>(40, 2),
                    OnNext<Int32>(50, 3),
                    OnNext<Int32>(60, -5),
                    OnNext<Int32>(70, -7),
                    OnNext<Int32>(80, 36),
                    OnCompleted<Int32>(90)
                );

                var res = client.CreateObserver<Double>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Double>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableInt32_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Int32?>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(250, default(Double?)),
                    OnCompleted<Double?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt32_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Int32?>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(250, default(Double?)),
                    OnCompleted<Double?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt32_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Int32?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Double?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt32_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Int32?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Double?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt32_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32?[]>(50, new Int32?[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Double?>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt32_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32?>(10, 17),
                    OnNext<Int32?>(20, -8),
                    OnNext<Int32?>(30, 25),
                    OnNext<Int32?>(40, 2),
                    OnNext<Int32?>(50, 3),
                    OnNext<Int32?>(60, -5),
                    OnNext<Int32?>(70, -7),
                    OnNext<Int32?>(80, 36),
                    OnCompleted<Int32?>(90)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 90, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt32_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Int32?>>(10, Tuple.Create<Int32?>(17)),
                    OnNext<Tuple<Int32?>>(20, Tuple.Create<Int32?>(-8)),
                    OnNext<Tuple<Int32?>>(30, Tuple.Create<Int32?>(25)),
                    OnNext<Tuple<Int32?>>(40, Tuple.Create<Int32?>(2)),
                    OnNext<Tuple<Int32?>>(50, Tuple.Create<Int32?>(3)),
                    OnNext<Tuple<Int32?>>(60, Tuple.Create<Int32?>(-5)),
                    OnNext<Tuple<Int32?>>(70, Tuple.Create<Int32?>(-7)),
                    OnNext<Tuple<Int32?>>(80, Tuple.Create<Int32?>(36)),
                    OnCompleted<Tuple<Int32?>>(90)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 90, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableInt32_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32?>(10, 17),
                    OnNext<Int32?>(20, -8),
                    OnNext<Int32?>(30, 25),
                    OnNext<Int32?>(40, 2),
                    OnNext<Int32?>(50, 3),
                    OnNext<Int32?>(60, -5),
                    OnNext<Int32?>(70, -7),
                    OnNext<Int32?>(80, 36),
                    OnCompleted<Int32?>(90)
                );

                var res = client.CreateObserver<Double?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Double?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableInt32_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32?>(10, 17),
                    OnNext<Int32?>(20, -8),
                    OnNext<Int32?>(30, default(Int32?)),
                    OnNext<Int32?>(40, 25),
                    OnNext<Int32?>(50, default(Int32?)),
                    OnNext<Int32?>(60, 2),
                    OnNext<Int32?>(70, 3),
                    OnNext<Int32?>(80, default(Int32?)),
                    OnNext<Int32?>(90, -5),
                    OnNext<Int32?>(100, -7),
                    OnNext<Int32?>(110, 36),
                    OnCompleted<Int32?>(120)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 120, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 120)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 120)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt32_Selector_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Int32?>>(10, Tuple.Create<Int32?>(17)),
                    OnNext<Tuple<Int32?>>(20, Tuple.Create<Int32?>(-8)),
                    OnNext<Tuple<Int32?>>(30, Tuple.Create<Int32?>(default(Int32?))),
                    OnNext<Tuple<Int32?>>(40, Tuple.Create<Int32?>(25)),
                    OnNext<Tuple<Int32?>>(50, Tuple.Create<Int32?>(default(Int32?))),
                    OnNext<Tuple<Int32?>>(60, Tuple.Create<Int32?>(2)),
                    OnNext<Tuple<Int32?>>(70, Tuple.Create<Int32?>(3)),
                    OnNext<Tuple<Int32?>>(80, Tuple.Create<Int32?>(default(Int32?))),
                    OnNext<Tuple<Int32?>>(90, Tuple.Create<Int32?>(-5)),
                    OnNext<Tuple<Int32?>>(100, Tuple.Create<Int32?>(-7)),
                    OnNext<Tuple<Int32?>>(110, Tuple.Create<Int32?>(36)),
                    OnCompleted<Tuple<Int32?>>(120)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 120, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 120)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 120)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableInt32_Overflow_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32?>(10, 17),
                    OnNext<Int32?>(20, -8),
                    OnNext<Int32?>(30, 25),
                    OnNext<Int32?>(40, 2),
                    OnNext<Int32?>(50, 3),
                    OnNext<Int32?>(60, -5),
                    OnNext<Int32?>(70, -7),
                    OnNext<Int32?>(80, 36),
                    OnCompleted<Int32?>(90)
                );

                var res = client.CreateObserver<Double?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Double?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageInt64_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Int64>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt64_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Int64>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt64_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Int64>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt64_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Int64>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt64_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64[]>(50, new Int64[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageInt64_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64>(10, 17L),
                    OnNext<Int64>(20, -8L),
                    OnNext<Int64>(30, 25L),
                    OnNext<Int64>(40, 2L),
                    OnNext<Int64>(50, 3L),
                    OnNext<Int64>(60, -5L),
                    OnNext<Int64>(70, -7L),
                    OnNext<Int64>(80, 36L),
                    OnCompleted<Int64>(90)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(200 + 90, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void AverageInt64_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Int64>>(10, Tuple.Create<Int64>(17L)),
                    OnNext<Tuple<Int64>>(20, Tuple.Create<Int64>(-8L)),
                    OnNext<Tuple<Int64>>(30, Tuple.Create<Int64>(25L)),
                    OnNext<Tuple<Int64>>(40, Tuple.Create<Int64>(2L)),
                    OnNext<Tuple<Int64>>(50, Tuple.Create<Int64>(3L)),
                    OnNext<Tuple<Int64>>(60, Tuple.Create<Int64>(-5L)),
                    OnNext<Tuple<Int64>>(70, Tuple.Create<Int64>(-7L)),
                    OnNext<Tuple<Int64>>(80, Tuple.Create<Int64>(36L)),
                    OnCompleted<Tuple<Int64>>(90)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(200 + 90, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageInt64_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64>(10, 17L),
                    OnNext<Int64>(20, -8L),
                    OnNext<Int64>(30, 25L),
                    OnNext<Int64>(40, 2L),
                    OnNext<Int64>(50, 3L),
                    OnNext<Int64>(60, -5L),
                    OnNext<Int64>(70, -7L),
                    OnNext<Int64>(80, 36L),
                    OnCompleted<Int64>(90)
                );

                var res = client.CreateObserver<Double>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Double>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableInt64_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Int64?>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(250, default(Double?)),
                    OnCompleted<Double?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt64_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Int64?>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(250, default(Double?)),
                    OnCompleted<Double?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt64_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Int64?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Double?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt64_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Int64?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Double?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt64_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64?[]>(50, new Int64?[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Double?>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt64_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64?>(10, 17L),
                    OnNext<Int64?>(20, -8L),
                    OnNext<Int64?>(30, 25L),
                    OnNext<Int64?>(40, 2L),
                    OnNext<Int64?>(50, 3L),
                    OnNext<Int64?>(60, -5L),
                    OnNext<Int64?>(70, -7L),
                    OnNext<Int64?>(80, 36L),
                    OnCompleted<Int64?>(90)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 90, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt64_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Int64?>>(10, Tuple.Create<Int64?>(17L)),
                    OnNext<Tuple<Int64?>>(20, Tuple.Create<Int64?>(-8L)),
                    OnNext<Tuple<Int64?>>(30, Tuple.Create<Int64?>(25L)),
                    OnNext<Tuple<Int64?>>(40, Tuple.Create<Int64?>(2L)),
                    OnNext<Tuple<Int64?>>(50, Tuple.Create<Int64?>(3L)),
                    OnNext<Tuple<Int64?>>(60, Tuple.Create<Int64?>(-5L)),
                    OnNext<Tuple<Int64?>>(70, Tuple.Create<Int64?>(-7L)),
                    OnNext<Tuple<Int64?>>(80, Tuple.Create<Int64?>(36L)),
                    OnCompleted<Tuple<Int64?>>(90)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 90, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableInt64_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64?>(10, 17L),
                    OnNext<Int64?>(20, -8L),
                    OnNext<Int64?>(30, 25L),
                    OnNext<Int64?>(40, 2L),
                    OnNext<Int64?>(50, 3L),
                    OnNext<Int64?>(60, -5L),
                    OnNext<Int64?>(70, -7L),
                    OnNext<Int64?>(80, 36L),
                    OnCompleted<Int64?>(90)
                );

                var res = client.CreateObserver<Double?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Double?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableInt64_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64?>(10, 17L),
                    OnNext<Int64?>(20, -8L),
                    OnNext<Int64?>(30, default(Int64?)),
                    OnNext<Int64?>(40, 25L),
                    OnNext<Int64?>(50, default(Int64?)),
                    OnNext<Int64?>(60, 2L),
                    OnNext<Int64?>(70, 3L),
                    OnNext<Int64?>(80, default(Int64?)),
                    OnNext<Int64?>(90, -5L),
                    OnNext<Int64?>(100, -7L),
                    OnNext<Int64?>(110, 36L),
                    OnCompleted<Int64?>(120)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 120, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 120)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 120)
                );
            });
        }

        [TestMethod]
        public void AverageNullableInt64_Selector_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Int64?>>(10, Tuple.Create<Int64?>(17L)),
                    OnNext<Tuple<Int64?>>(20, Tuple.Create<Int64?>(-8L)),
                    OnNext<Tuple<Int64?>>(30, Tuple.Create<Int64?>(default(Int64?))),
                    OnNext<Tuple<Int64?>>(40, Tuple.Create<Int64?>(25L)),
                    OnNext<Tuple<Int64?>>(50, Tuple.Create<Int64?>(default(Int64?))),
                    OnNext<Tuple<Int64?>>(60, Tuple.Create<Int64?>(2L)),
                    OnNext<Tuple<Int64?>>(70, Tuple.Create<Int64?>(3L)),
                    OnNext<Tuple<Int64?>>(80, Tuple.Create<Int64?>(default(Int64?))),
                    OnNext<Tuple<Int64?>>(90, Tuple.Create<Int64?>(-5L)),
                    OnNext<Tuple<Int64?>>(100, Tuple.Create<Int64?>(-7L)),
                    OnNext<Tuple<Int64?>>(110, Tuple.Create<Int64?>(36L)),
                    OnCompleted<Tuple<Int64?>>(120)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 120, avg => Math.Abs((Double)(avg - 7.875d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 120)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 120)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableInt64_Overflow_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64?>(10, 17L),
                    OnNext<Int64?>(20, -8L),
                    OnNext<Int64?>(30, 25L),
                    OnNext<Int64?>(40, 2L),
                    OnNext<Int64?>(50, 3L),
                    OnNext<Int64?>(60, -5L),
                    OnNext<Int64?>(70, -7L),
                    OnNext<Int64?>(80, 36L),
                    OnCompleted<Int64?>(90)
                );

                var res = client.CreateObserver<Double?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Double?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageSingle_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Single>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Single>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageSingle_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Single>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Single>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageSingle_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Single>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Single>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageSingle_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Single>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Single>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageSingle_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single[]>(50, new Single[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Single>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageSingle_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single>(10, 17.8f),
                    OnNext<Single>(20, -25.2f),
                    OnNext<Single>(30, 3.5f),
                    OnNext<Single>(40, -7.36f),
                    OnCompleted<Single>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Single>(200 + 50, avg => Math.Abs((Single)(avg - -2.815001f)) < 0.0001f),
                    OnCompleted<Single>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void AverageSingle_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Single>>(10, Tuple.Create<Single>(17.8f)),
                    OnNext<Tuple<Single>>(20, Tuple.Create<Single>(-25.2f)),
                    OnNext<Tuple<Single>>(30, Tuple.Create<Single>(3.5f)),
                    OnNext<Tuple<Single>>(40, Tuple.Create<Single>(-7.36f)),
                    OnCompleted<Tuple<Single>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Single>(200 + 50, avg => Math.Abs((Single)(avg - -2.815001f)) < 0.0001f),
                    OnCompleted<Single>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageSingle_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single>(10, 17.8f),
                    OnNext<Single>(20, -25.2f),
                    OnNext<Single>(30, 3.5f),
                    OnNext<Single>(40, -7.36f),
                    OnCompleted<Single>(50)
                );

                var res = client.CreateObserver<Single>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Single>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableSingle_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Single?>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(250, default(Single?)),
                    OnCompleted<Single?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableSingle_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Single?>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(250, default(Single?)),
                    OnCompleted<Single?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableSingle_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Single?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Single?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableSingle_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Single?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Single?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableSingle_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single?[]>(50, new Single?[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Single?>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableSingle_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single?>(10, 17.8f),
                    OnNext<Single?>(20, -25.2f),
                    OnNext<Single?>(30, 3.5f),
                    OnNext<Single?>(40, -7.36f),
                    OnCompleted<Single?>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(200 + 50, avg => Math.Abs((Single)(avg - -2.815001f)) < 0.0001f),
                    OnCompleted<Single?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void AverageNullableSingle_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Single?>>(10, Tuple.Create<Single?>(17.8f)),
                    OnNext<Tuple<Single?>>(20, Tuple.Create<Single?>(-25.2f)),
                    OnNext<Tuple<Single?>>(30, Tuple.Create<Single?>(3.5f)),
                    OnNext<Tuple<Single?>>(40, Tuple.Create<Single?>(-7.36f)),
                    OnCompleted<Tuple<Single?>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(200 + 50, avg => Math.Abs((Single)(avg - -2.815001f)) < 0.0001f),
                    OnCompleted<Single?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableSingle_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single?>(10, 17.8f),
                    OnNext<Single?>(20, -25.2f),
                    OnNext<Single?>(30, 3.5f),
                    OnNext<Single?>(40, -7.36f),
                    OnCompleted<Single?>(50)
                );

                var res = client.CreateObserver<Single?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Single?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableSingle_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single?>(10, 17.8f),
                    OnNext<Single?>(20, default(Single?)),
                    OnNext<Single?>(30, default(Single?)),
                    OnNext<Single?>(40, -25.2f),
                    OnNext<Single?>(50, default(Single?)),
                    OnNext<Single?>(60, 3.5f),
                    OnNext<Single?>(70, -7.36f),
                    OnCompleted<Single?>(80)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(200 + 80, avg => Math.Abs((Single)(avg - -2.815001f)) < 0.0001f),
                    OnCompleted<Single?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

        [TestMethod]
        public void AverageNullableSingle_Selector_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Single?>>(10, Tuple.Create<Single?>(17.8f)),
                    OnNext<Tuple<Single?>>(20, Tuple.Create<Single?>(default(Single?))),
                    OnNext<Tuple<Single?>>(30, Tuple.Create<Single?>(default(Single?))),
                    OnNext<Tuple<Single?>>(40, Tuple.Create<Single?>(-25.2f)),
                    OnNext<Tuple<Single?>>(50, Tuple.Create<Single?>(default(Single?))),
                    OnNext<Tuple<Single?>>(60, Tuple.Create<Single?>(3.5f)),
                    OnNext<Tuple<Single?>>(70, Tuple.Create<Single?>(-7.36f)),
                    OnCompleted<Tuple<Single?>>(80)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(200 + 80, avg => Math.Abs((Single)(avg - -2.815001f)) < 0.0001f),
                    OnCompleted<Single?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableSingle_Overflow_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single?>(10, 17.8f),
                    OnNext<Single?>(20, -25.2f),
                    OnNext<Single?>(30, 3.5f),
                    OnNext<Single?>(40, -7.36f),
                    OnCompleted<Single?>(50)
                );

                var res = client.CreateObserver<Single?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Single?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageDouble_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Double>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDouble_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Double>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDouble_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Double>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDouble_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Double>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDouble_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double[]>(50, new Double[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Double>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDouble_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double>(10, 17.8d),
                    OnNext<Double>(20, -25.2d),
                    OnNext<Double>(30, 3.5d),
                    OnNext<Double>(40, -7.36d),
                    OnCompleted<Double>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(200 + 50, avg => Math.Abs((Double)(avg - -2.815d)) < 0.0001d),
                    OnCompleted<Double>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void AverageDouble_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Double>>(10, Tuple.Create<Double>(17.8d)),
                    OnNext<Tuple<Double>>(20, Tuple.Create<Double>(-25.2d)),
                    OnNext<Tuple<Double>>(30, Tuple.Create<Double>(3.5d)),
                    OnNext<Tuple<Double>>(40, Tuple.Create<Double>(-7.36d)),
                    OnCompleted<Tuple<Double>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(200 + 50, avg => Math.Abs((Double)(avg - -2.815d)) < 0.0001d),
                    OnCompleted<Double>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageDouble_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double>(10, 17.8d),
                    OnNext<Double>(20, -25.2d),
                    OnNext<Double>(30, 3.5d),
                    OnNext<Double>(40, -7.36d),
                    OnCompleted<Double>(50)
                );

                var res = client.CreateObserver<Double>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Double>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableDouble_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Double?>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(250, default(Double?)),
                    OnCompleted<Double?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDouble_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Double?>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(250, default(Double?)),
                    OnCompleted<Double?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDouble_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Double?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Double?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDouble_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Double?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Double?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDouble_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double?[]>(50, new Double?[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Double?>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDouble_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double?>(10, 17.8d),
                    OnNext<Double?>(20, -25.2d),
                    OnNext<Double?>(30, 3.5d),
                    OnNext<Double?>(40, -7.36d),
                    OnCompleted<Double?>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 50, avg => Math.Abs((Double)(avg - -2.815d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDouble_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Double?>>(10, Tuple.Create<Double?>(17.8d)),
                    OnNext<Tuple<Double?>>(20, Tuple.Create<Double?>(-25.2d)),
                    OnNext<Tuple<Double?>>(30, Tuple.Create<Double?>(3.5d)),
                    OnNext<Tuple<Double?>>(40, Tuple.Create<Double?>(-7.36d)),
                    OnCompleted<Tuple<Double?>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 50, avg => Math.Abs((Double)(avg - -2.815d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableDouble_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double?>(10, 17.8d),
                    OnNext<Double?>(20, -25.2d),
                    OnNext<Double?>(30, 3.5d),
                    OnNext<Double?>(40, -7.36d),
                    OnCompleted<Double?>(50)
                );

                var res = client.CreateObserver<Double?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Double?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableDouble_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double?>(10, 17.8d),
                    OnNext<Double?>(20, default(Double?)),
                    OnNext<Double?>(30, default(Double?)),
                    OnNext<Double?>(40, -25.2d),
                    OnNext<Double?>(50, default(Double?)),
                    OnNext<Double?>(60, 3.5d),
                    OnNext<Double?>(70, -7.36d),
                    OnCompleted<Double?>(80)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 80, avg => Math.Abs((Double)(avg - -2.815d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDouble_Selector_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Double?>>(10, Tuple.Create<Double?>(17.8d)),
                    OnNext<Tuple<Double?>>(20, Tuple.Create<Double?>(default(Double?))),
                    OnNext<Tuple<Double?>>(30, Tuple.Create<Double?>(default(Double?))),
                    OnNext<Tuple<Double?>>(40, Tuple.Create<Double?>(-25.2d)),
                    OnNext<Tuple<Double?>>(50, Tuple.Create<Double?>(default(Double?))),
                    OnNext<Tuple<Double?>>(60, Tuple.Create<Double?>(3.5d)),
                    OnNext<Tuple<Double?>>(70, Tuple.Create<Double?>(-7.36d)),
                    OnCompleted<Tuple<Double?>>(80)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 80, avg => Math.Abs((Double)(avg - -2.815d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableDouble_Overflow_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double?>(10, 17.8d),
                    OnNext<Double?>(20, -25.2d),
                    OnNext<Double?>(30, 3.5d),
                    OnNext<Double?>(40, -7.36d),
                    OnCompleted<Double?>(50)
                );

                var res = client.CreateObserver<Double?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Double?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageDecimal_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Decimal>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Decimal>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDecimal_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Decimal>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Decimal>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDecimal_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Decimal>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Decimal>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDecimal_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Decimal>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Decimal>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDecimal_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal[]>(50, new Decimal[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Decimal>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageDecimal_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal>(10, 24.95m),
                    OnNext<Decimal>(20, -7m),
                    OnNext<Decimal>(30, 499.99m),
                    OnNext<Decimal>(40, 123m),
                    OnCompleted<Decimal>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal>(200 + 50, 160.235m),
                    OnCompleted<Decimal>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void AverageDecimal_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Decimal>>(10, Tuple.Create<Decimal>(24.95m)),
                    OnNext<Tuple<Decimal>>(20, Tuple.Create<Decimal>(-7m)),
                    OnNext<Tuple<Decimal>>(30, Tuple.Create<Decimal>(499.99m)),
                    OnNext<Tuple<Decimal>>(40, Tuple.Create<Decimal>(123m)),
                    OnCompleted<Tuple<Decimal>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal>(200 + 50, 160.235m),
                    OnCompleted<Decimal>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageDecimal_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal>(10, 24.95m),
                    OnNext<Decimal>(20, -7m),
                    OnNext<Decimal>(30, 499.99m),
                    OnNext<Decimal>(40, 123m),
                    OnCompleted<Decimal>(50)
                );

                var res = client.CreateObserver<Decimal>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Decimal>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableDecimal_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Decimal?>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(250, default(Decimal?)),
                    OnCompleted<Decimal?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDecimal_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Decimal?>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(250, default(Decimal?)),
                    OnCompleted<Decimal?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDecimal_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Decimal?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnError<Decimal?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDecimal_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Decimal?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Decimal?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDecimal_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal?[]>(50, new Decimal?[0])
                );

                var res = client.Start(() =>
                    xs.Average(x => x[0])
                );

                res.Messages.AssertEqual(
                    OnError<Decimal?>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDecimal_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal?>(10, 24.95m),
                    OnNext<Decimal?>(20, -7m),
                    OnNext<Decimal?>(30, 499.99m),
                    OnNext<Decimal?>(40, 123m),
                    OnCompleted<Decimal?>(50)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(200 + 50, 160.235m),
                    OnCompleted<Decimal?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDecimal_Selector_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Decimal?>>(10, Tuple.Create<Decimal?>(24.95m)),
                    OnNext<Tuple<Decimal?>>(20, Tuple.Create<Decimal?>(-7m)),
                    OnNext<Tuple<Decimal?>>(30, Tuple.Create<Decimal?>(499.99m)),
                    OnNext<Tuple<Decimal?>>(40, Tuple.Create<Decimal?>(123m)),
                    OnCompleted<Tuple<Decimal?>>(50)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(200 + 50, 160.235m),
                    OnCompleted<Decimal?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableDecimal_Overflow()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal?>(10, 24.95m),
                    OnNext<Decimal?>(20, -7m),
                    OnNext<Decimal?>(30, 499.99m),
                    OnNext<Decimal?>(40, 123m),
                    OnCompleted<Decimal?>(50)
                );

                var res = client.CreateObserver<Decimal?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Decimal?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

        [TestMethod]
        public void AverageNullableDecimal_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal?>(10, 24.95m),
                    OnNext<Decimal?>(20, default(Decimal?)),
                    OnNext<Decimal?>(30, default(Decimal?)),
                    OnNext<Decimal?>(40, -7m),
                    OnNext<Decimal?>(50, default(Decimal?)),
                    OnNext<Decimal?>(60, 499.99m),
                    OnNext<Decimal?>(70, 123m),
                    OnCompleted<Decimal?>(80)
                );

                var res = client.Start(() =>
                    xs.Average()
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(200 + 80, 160.235m),
                    OnCompleted<Decimal?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

        [TestMethod]
        public void AverageNullableDecimal_Selector_Simple_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Tuple<Decimal?>>(10, Tuple.Create<Decimal?>(24.95m)),
                    OnNext<Tuple<Decimal?>>(20, Tuple.Create<Decimal?>(default(Decimal?))),
                    OnNext<Tuple<Decimal?>>(30, Tuple.Create<Decimal?>(default(Decimal?))),
                    OnNext<Tuple<Decimal?>>(40, Tuple.Create<Decimal?>(-7m)),
                    OnNext<Tuple<Decimal?>>(50, Tuple.Create<Decimal?>(default(Decimal?))),
                    OnNext<Tuple<Decimal?>>(60, Tuple.Create<Decimal?>(499.99m)),
                    OnNext<Tuple<Decimal?>>(70, Tuple.Create<Decimal?>(123m)),
                    OnCompleted<Tuple<Decimal?>>(80)
                );

                var res = client.Start(() =>
                    xs.Average(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(200 + 80, 160.235m),
                    OnCompleted<Decimal?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void AverageNullableDecimal_Overflow_Null()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal?>(10, 24.95m),
                    OnNext<Decimal?>(20, -7m),
                    OnNext<Decimal?>(30, 499.99m),
                    OnNext<Decimal?>(40, 123m),
                    OnCompleted<Decimal?>(50)
                );

                var res = client.CreateObserver<Decimal?>();

                client.ScheduleAbsolute(default(object), 200, (s, _) =>
                {
                    var ys = xs.Average();
                    var sub = ys.Subscribe(res);

                    new SubscriptionInitializeVisitor(sub).Initialize(client.CreateContext());

                    // Yes, dirty. No, I don't care.
                    SubscriptionVisitor.Do<IOperator>(op =>
                    {
                        var top = op.GetType();
                        if (top.DeclaringType == ys.GetType())
                        {
                            var count = top.GetField("_count", BindingFlags.Instance | BindingFlags.NonPublic);
                            count.SetValue(op, long.MaxValue - 2);
                        }
                    }).Apply(sub);
                });

                client.Start();

                res.Messages.AssertEqual(
                    OnError<Decimal?>(230, ex => ex is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
#endif

    }
}
