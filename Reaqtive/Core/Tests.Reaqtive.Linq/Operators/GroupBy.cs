// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;
using Reaqtive.Testing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class GroupBy : OperatorTestBase
    {
        [TestMethod]
        public void GroupBy_ArgumentChecking()
        {
            var ns = default(ISubscribable<int>);
            var xs = DummySubscribable<int>.Instance;
            var nk = default(Func<int, int>);
            var dk = new Func<int, int>(x => x);
            var nc = default(IEqualityComparer<int>);
            var dc = EqualityComparer<int>.Default;
            var ne = default(Func<int, string>);
            var de = new Func<int, string>(x => x.ToString());

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(ns, dk));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(xs, nk));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(ns, dk, dc));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(xs, nk, dc));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(xs, dk, nc));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(ns, dk, de));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(xs, nk, de));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(xs, dk, ne));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(ns, dk, de, dc));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(xs, nk, de, dc));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(xs, dk, ne, dc));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.GroupBy(xs, dk, de, nc));
        }
    }
}
