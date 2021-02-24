// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Window : OperatorTestBase
    {
        [TestMethod]
        public void Window_ArgumentChecking()
        {
            var ns = default(ISubscribable<int>);
            var xs = DummySubscribable<int>.Instance;
            var n = 1;
            var t = TimeSpan.FromSeconds(1);
            var u = -t;

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Window(ns, n));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Window(ns, n, n));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Window(ns, t, n));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Window(xs, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Window(xs, 0, n));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Window(xs, n, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Window(xs, t, 0));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Window(ns, t));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Window(ns, t, t));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Window(xs, u));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Window(xs, u, t));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Window(xs, t, u));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Window(xs, u, n));
        }
    }
}
