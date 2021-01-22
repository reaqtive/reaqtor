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
    public partial class Sum
    {
        [TestMethod]
        public void SumInt32_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Int32>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int32>(250, 0),
                    OnCompleted<Int32>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt32_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Int32>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int32>(250, 0),
                    OnCompleted<Int32>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt32_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Int32>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int32>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt32_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Int32>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Int32>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt32_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32[]>(50, new Int32[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
                );

                res.Messages.AssertEqual(
                    OnError<Int32>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt32_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int32>(200 + 90, 63),
                    OnCompleted<Int32>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void SumInt32_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int32>(200 + 90, 63),
                    OnCompleted<Int32>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Int32?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int32?>(250, 0),
                    OnCompleted<Int32?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Int32?>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int32?>(250, 0),
                    OnCompleted<Int32?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Int32?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int32?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Int32?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Int32?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32?[]>(50, new Int32?[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
                );

                res.Messages.AssertEqual(
                    OnError<Int32?>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int32?>(200 + 90, 63),
                    OnCompleted<Int32?>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int32?>(200 + 90, 63),
                    OnCompleted<Int32?>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Simple_Null()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int32?>(200 + 120, 63),
                    OnCompleted<Int32?>(200 + 120)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 120)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Selector_Simple_Null()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int32?>(200 + 120, 63),
                    OnCompleted<Int32?>(200 + 120)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 120)
                );
            });
        }

        [TestMethod]
        public void SumInt64_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Int64>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int64>(250, 0),
                    OnCompleted<Int64>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt64_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Int64>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int64>(250, 0),
                    OnCompleted<Int64>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt64_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Int64>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int64>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt64_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Int64>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Int64>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt64_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64[]>(50, new Int64[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
                );

                res.Messages.AssertEqual(
                    OnError<Int64>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumInt64_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int64>(200 + 90, 63L),
                    OnCompleted<Int64>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void SumInt64_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int64>(200 + 90, 63L),
                    OnCompleted<Int64>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Int64?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int64?>(250, 0),
                    OnCompleted<Int64?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Int64?>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int64?>(250, 0),
                    OnCompleted<Int64?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Int64?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int64?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Int64?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnError<Int64?>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64?[]>(50, new Int64?[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
                );

                res.Messages.AssertEqual(
                    OnError<Int64?>(250, ex => ex is IndexOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int64?>(200 + 90, 63L),
                    OnCompleted<Int64?>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int64?>(200 + 90, 63L),
                    OnCompleted<Int64?>(200 + 90)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 90)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Simple_Null()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Int64?>(200 + 120, 63L),
                    OnCompleted<Int64?>(200 + 120)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 120)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Selector_Simple_Null()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Int64?>(200 + 120, 63L),
                    OnCompleted<Int64?>(200 + 120)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 120)
                );
            });
        }

        [TestMethod]
        public void SumSingle_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Single>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Single>(250, 0),
                    OnCompleted<Single>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumSingle_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Single>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Single>(250, 0),
                    OnCompleted<Single>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumSingle_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Single>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
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
        public void SumSingle_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Single>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
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
        public void SumSingle_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single[]>(50, new Single[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
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
        public void SumSingle_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Single>(200 + 50, sum => Math.Abs((Single)(sum - -11.26f)) < 0.0001f),
                    OnCompleted<Single>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumSingle_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Single>(200 + 50, sum => Math.Abs((Single)(sum - -11.26f)) < 0.0001f),
                    OnCompleted<Single>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumNullableSingle_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Single?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(250, 0),
                    OnCompleted<Single?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableSingle_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Single?>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(250, 0),
                    OnCompleted<Single?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableSingle_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Single?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
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
        public void SumNullableSingle_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Single?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
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
        public void SumNullableSingle_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Single?[]>(50, new Single?[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
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
        public void SumNullableSingle_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(200 + 50, sum => Math.Abs((Single)(sum - -11.26f)) < 0.0001f),
                    OnCompleted<Single?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumNullableSingle_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(200 + 50, sum => Math.Abs((Single)(sum - -11.26f)) < 0.0001f),
                    OnCompleted<Single?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumNullableSingle_Simple_Null()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(200 + 80, sum => Math.Abs((Single)(sum - -11.26f)) < 0.0001f),
                    OnCompleted<Single?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

        [TestMethod]
        public void SumNullableSingle_Selector_Simple_Null()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Single?>(200 + 80, sum => Math.Abs((Single)(sum - -11.26f)) < 0.0001f),
                    OnCompleted<Single?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

        [TestMethod]
        public void SumDouble_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Double>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(250, 0),
                    OnCompleted<Double>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumDouble_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Double>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(250, 0),
                    OnCompleted<Double>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumDouble_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Double>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
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
        public void SumDouble_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Double>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
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
        public void SumDouble_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double[]>(50, new Double[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
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
        public void SumDouble_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(200 + 50, sum => Math.Abs((Double)(sum - -11.26d)) < 0.0001d),
                    OnCompleted<Double>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumDouble_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double>(200 + 50, sum => Math.Abs((Double)(sum - -11.26d)) < 0.0001d),
                    OnCompleted<Double>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumNullableDouble_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Double?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(250, 0),
                    OnCompleted<Double?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableDouble_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Double?>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(250, 0),
                    OnCompleted<Double?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableDouble_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Double?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
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
        public void SumNullableDouble_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Double?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
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
        public void SumNullableDouble_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Double?[]>(50, new Double?[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
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
        public void SumNullableDouble_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 50, sum => Math.Abs((Double)(sum - -11.26d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumNullableDouble_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 50, sum => Math.Abs((Double)(sum - -11.26d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumNullableDouble_Simple_Null()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 80, sum => Math.Abs((Double)(sum - -11.26d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

        [TestMethod]
        public void SumNullableDouble_Selector_Simple_Null()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Double?>(200 + 80, sum => Math.Abs((Double)(sum - -11.26d)) < 0.0001d),
                    OnCompleted<Double?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

        [TestMethod]
        public void SumDecimal_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Decimal>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal>(250, 0),
                    OnCompleted<Decimal>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumDecimal_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Decimal>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal>(250, 0),
                    OnCompleted<Decimal>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumDecimal_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Decimal>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
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
        public void SumDecimal_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Decimal>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
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
        public void SumDecimal_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal[]>(50, new Decimal[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
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
        public void SumDecimal_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal>(200 + 50, 640.94m),
                    OnCompleted<Decimal>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumDecimal_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal>(200 + 50, 640.94m),
                    OnCompleted<Decimal>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumNullableDecimal_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Decimal?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(250, 0),
                    OnCompleted<Decimal?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableDecimal_Selector_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<Tuple<Decimal?>>(50)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(250, 0),
                    OnCompleted<Decimal?>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SumNullableDecimal_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Decimal?>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum()
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
        public void SumNullableDecimal_Selector_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<Tuple<Decimal?>>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Sum(x => x.Item1)
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
        public void SumNullableDecimal_SelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal?[]>(50, new Decimal?[0])
                );

                var res = client.Start(() =>
                    xs.Sum(x => x[1])
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
        public void SumNullableDecimal_Simple()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(200 + 50, 640.94m),
                    OnCompleted<Decimal?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumNullableDecimal_Selector_Simple()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(200 + 50, 640.94m),
                    OnCompleted<Decimal?>(200 + 50)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 50)
                );
            });
        }

        [TestMethod]
        public void SumNullableDecimal_Simple_Null()
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
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(200 + 80, 640.94m),
                    OnCompleted<Decimal?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

        [TestMethod]
        public void SumNullableDecimal_Selector_Simple_Null()
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
                    xs.Sum(x => x.Item1)
                );

                res.Messages.AssertEqual(
                    OnNext<Decimal?>(200 + 80, 640.94m),
                    OnCompleted<Decimal?>(200 + 80)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 200 + 80)
                );
            });
        }

    }
}
