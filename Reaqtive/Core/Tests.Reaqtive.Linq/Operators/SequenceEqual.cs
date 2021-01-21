// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class SequenceEqual : OperatorTestBase
    {
        [TestMethod]
        public void SequenceEqual_ArgumentChecking()
        {
            var ns = default(ISubscribable<int>);
            var xs = DummySubscribable<int>.Instance;
            var nc = default(IEqualityComparer<int>);
            var ic = EqualityComparer<int>.Default;

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SequenceEqual(ns, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SequenceEqual(xs, ns));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SequenceEqual(ns, xs, ic));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SequenceEqual(xs, ns, ic));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SequenceEqual(xs, xs, nc));
        }

        [TestMethod]
        public void SequenceEqual_Settings_MaxQueueSize()
        {
            Run(client =>
            {
                var ctx = client.CreateContext(settings: new Dictionary<string, object>
                {
                    { "rx://operators/sequenceEqual/settings/maxQueueSize", 5 },
                });

                var xs = client.CreateHotObservable<int>(
                );

                var ys = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6)
                );

                var res = client.Start(ctx, () =>
                    xs.SequenceEqual(ys),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnError<bool>(260, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 260)
                );
            });
        }

        [TestMethod]
        public void SequenceEqual_ComparerThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1)
                );

                var ys = client.CreateHotObservable<int>(
                    OnNext(220, 1)
                );

                var res = client.Start(() =>
                    xs.SequenceEqual(ys, new ThrowComparer<int>(ex)),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnError<bool>(220, ex)
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
