// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Contains : OperatorTestBase
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
        public void Contains_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Contains<int>(default(ISubscribable<int>), 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Contains<int>(DummySubscribable<int>.Instance, 0, null));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void Contains_Comparer_Never()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 2),
                    OnNext(230, 2),
                    OnNext(240, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Contains(2, new FuncComparer<int>((x, y) => false))
                );

                res.Messages.AssertEqual(
                    OnNext(250, false),
                    OnCompleted<bool>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Contains_Comparer_Always()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Contains(0, new FuncComparer<int>((x, y) => true))
                );

                res.Messages.AssertEqual(
                    OnNext(210, true),
                    OnCompleted<bool>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        private sealed class FuncComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equals;

            public FuncComparer(Func<T, T, bool> equals)
            {
                _equals = equals;
            }

            public bool Equals(T x, T y)
            {
                return _equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        [TestMethod]
        public void Contains_ComparerThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Contains(2, new ThrowComparer<int>(ex))
                );

                res.Messages.AssertEqual(
                    OnError<bool>(210, ex)
                );
            });
        }

        private sealed class ThrowComparer<T> : IEqualityComparer<T>
        {
            private readonly Exception _ex;

            public ThrowComparer(Exception ex)
            {
                _ex = ex;
            }

            public bool Equals(T x, T y)
            {
                throw _ex;
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }
    }
}
