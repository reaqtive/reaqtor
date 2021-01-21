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
    public partial class Min : OperatorTestBase
    {
        [TestMethod]
        public void MinComparer_String_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<string>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Min(StringComparer.Ordinal)
                );

                res.Messages.AssertEqual(
                    OnError<string>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_TimeSpan_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<TimeSpan>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Min(Comparer<TimeSpan>.Default)
                );

                res.Messages.AssertEqual(
                    OnError<TimeSpan>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_String_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<string>(50)
                );

                var res = client.Start(() =>
                    xs.Min(StringComparer.Ordinal)
                );

                res.Messages.AssertEqual(
                    OnNext<string>(250, default(string)),
                    OnCompleted<string>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_TimeSpan_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<TimeSpan>(50)
                );

                var res = client.Start(() =>
                    xs.Min(Comparer<TimeSpan>.Default)
                );

                res.Messages.AssertEqual(
                    OnError<TimeSpan>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_String_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<string>(10, "foo"),
                    OnNext<string>(20, "bar"),
                    OnNext<string>(30, "qux"),
                    OnNext<string>(40, "baz"),
                    OnCompleted<string>(50)
                );

                var res = client.Start(() =>
                    xs.Min(StringComparer.Ordinal)
                );

                res.Messages.AssertEqual(
                    OnNext<string>(250, "bar"),
                    OnCompleted<string>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_TimeSpan_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<TimeSpan>(10, TimeSpan.FromSeconds(4)),
                    OnNext<TimeSpan>(20, TimeSpan.FromSeconds(7)),
                    OnNext<TimeSpan>(30, TimeSpan.FromSeconds(2)),
                    OnNext<TimeSpan>(40, TimeSpan.FromSeconds(5)),
                    OnCompleted<TimeSpan>(50)
                );

                var res = client.Start(() =>
                    xs.Min(Comparer<TimeSpan>.Default)
                );

                res.Messages.AssertEqual(
                    OnNext<TimeSpan>(250, TimeSpan.FromSeconds(2)),
                    OnCompleted<TimeSpan>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_String_HasNull()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<string>(10, "foo"),
                    OnNext<string>(20, "bar"),
                    OnNext<string>(30, default(string)),
                    OnNext<string>(40, "baz"),
                    OnCompleted<string>(50)
                );

                var res = client.Start(() =>
                    xs.Min(StringComparer.Ordinal)
                );

                res.Messages.AssertEqual(
                    OnNext<string>(250, "bar"),
                    OnCompleted<string>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_String_AllNull()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<string>(10, default(string)),
                    OnNext<string>(20, default(string)),
                    OnNext<string>(30, default(string)),
                    OnNext<string>(40, default(string)),
                    OnCompleted<string>(50)
                );

                var res = client.Start(() =>
                    xs.Min(StringComparer.Ordinal)
                );

                res.Messages.AssertEqual(
                    OnNext<string>(250, default(string)),
                    OnCompleted<string>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_String_DefaultComparer()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<string>(10, "foo"),
                    OnNext<string>(20, "bar"),
                    OnNext<string>(30, "qux"),
                    OnNext<string>(40, "baz"),
                    OnCompleted<string>(50)
                );

                var res = client.Start(() =>
                    xs.Min()
                );

                res.Messages.AssertEqual(
                    OnNext<string>(250, xs.ObserverMessages.Where(m => m.Value.HasValue).Select(m => m.Value.Value).Min()),
                    OnCompleted<string>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_TimeSpan_DefaultComparer()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<TimeSpan>(10, TimeSpan.FromSeconds(4)),
                    OnNext<TimeSpan>(20, TimeSpan.FromSeconds(7)),
                    OnNext<TimeSpan>(30, TimeSpan.FromSeconds(2)),
                    OnNext<TimeSpan>(40, TimeSpan.FromSeconds(5)),
                    OnCompleted<TimeSpan>(50)
                );

                var res = client.Start(() =>
                    xs.Min()
                );

                res.Messages.AssertEqual(
                    OnNext<TimeSpan>(250, xs.ObserverMessages.Where(m => m.Value.HasValue).Select(m => m.Value.Value).Min()),
                    OnCompleted<TimeSpan>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void MinComparer_String_ComparerThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext<string>(10, "foo"),
                    OnNext<string>(20, "bar"),
                    OnNext<string>(30, "qux"),
                    OnNext<string>(40, "baz"),
                    OnCompleted<string>(50)
                );

                var res = client.Start(() =>
                    xs.Min(new ThrowingComparer<string>(ex))
                );

                res.Messages.AssertEqual(
                    OnError<string>(220, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void MinComparer_TimeSpan_ComparerThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext<TimeSpan>(10, TimeSpan.FromSeconds(4)),
                    OnNext<TimeSpan>(20, TimeSpan.FromSeconds(7)),
                    OnNext<TimeSpan>(30, TimeSpan.FromSeconds(2)),
                    OnNext<TimeSpan>(40, TimeSpan.FromSeconds(5)),
                    OnCompleted<TimeSpan>(50)
                );

                var res = client.Start(() =>
                    xs.Min(new ThrowingComparer<TimeSpan>(ex))
                );

                res.Messages.AssertEqual(
                    OnError<TimeSpan>(220, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        private sealed class ThrowingComparer<T> : IComparer<T>
        {
            private readonly Exception _ex;

            public ThrowingComparer(Exception ex)
            {
                _ex = ex;
            }

            public int Compare(T x, T y)
            {
                throw _ex;
            }
        }
    }
}
