// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Buffer : OperatorTestBase
    {
        [TestMethod]
        public void Buffer_ArgumentChecking()
        {
            var ns = default(ISubscribable<int>);
            var xs = DummySubscribable<int>.Instance;
            var n = 1;
            var t = TimeSpan.FromSeconds(1);
            var u = -t;

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Buffer(ns, n));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Buffer(ns, n, n));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Buffer(ns, t, n));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Buffer(xs, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Buffer(xs, 0, n));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Buffer(xs, n, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Buffer(xs, t, 0));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Buffer(ns, t));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Buffer(ns, t, t));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Buffer(xs, u));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Buffer(xs, u, t));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Buffer(xs, t, u));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Buffer(xs, u, n));
        }

        [TestMethod]
        public void Buffer_Settings_MaxBufferCount()
        {
            Run(client =>
            {
                var ctx = client.CreateContext(settings: new Dictionary<string, object>
                {
                    { "rx://operators/buffer/settings/maxBufferCount", 5 },
                });

                var xs = client.CreateHotObservable<int>(
                );

                var res = client.Start(ctx, () =>
                    xs.Buffer(TimeSpan.FromTicks(10), TimeSpan.FromTicks(1)).Select(x => x.Sum()),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnError<int>(205, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 205)
                );
            });
        }

        [TestMethod]
        public void Buffer_Settings_MaxBufferSize1()
        {
            Run(client =>
            {
                var ctx = client.CreateContext(settings: new Dictionary<string, object>
                {
                    { "rx://operators/buffer/settings/maxBufferSize", 5 },
                });

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6)
                );

                var res = client.Start(ctx, () =>
                    xs.Buffer(TimeSpan.FromTicks(100)).Select(x => x.Sum()),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnError<int>(260, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 260)
                );
            });
        }

        [TestMethod]
        public void Buffer_Settings_MaxBufferSize2()
        {
            Run(client =>
            {
                var ctx = client.CreateContext(settings: new Dictionary<string, object>
                {
                    { "rx://operators/buffer/settings/maxBufferSize", 5 },
                });

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6)
                );

                var res = client.Start(ctx, () =>
                    xs.Buffer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100)).Select(x => x.Sum()),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnError<int>(260, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 260)
                );
            });
        }
    }
}
