// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Linq;

using Reaqtive;
using Reaqtive.Disposables;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    public partial class CombineLatest
    {
        #region Argument checking

        [TestMethod]
        public void CombineLatest_ArgumentChecking_HighArity()
        {
            var xs = DummySubscribable<int>.Instance;
            var ns = default(ISubscribable<int>);

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, (_1, _2) => _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, (_1, _2) => _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, default(Func<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, (_1, _2, _3) => _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, (_1, _2, _3) => _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, (_1, _2, _3) => _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, default(Func<int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, (_1, _2, _3, _4) => _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, (_1, _2, _3, _4) => _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, (_1, _2, _3, _4) => _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, (_1, _2, _3, _4) => _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, default(Func<int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5) => _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5) => _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5) => _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5) => _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5) => _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6) => _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6) => _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6) => _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6) => _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6) => _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6) => _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7) => _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7) => _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7) => _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7) => _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7) => _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7) => _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7) => _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7, _8) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7, _8) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7, _8, _9) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, xs, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, ns, (_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16) => _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15 + _16));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
        }

        #endregion

        #region Checkpointing

        [TestMethod]
        public void CombineLatest_SaveAndReload_2()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1),
                OnNext(550, 10)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, (p0, p1) => p0 + p1).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 20, 2),
                OnNext(550, 11),
                OnNext(650, 3)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_3()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1),
                OnNext(550, 10)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, (p0, p1, p2) => p0 + p1 + p2).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 30, 3),
                OnNext(550, 12),
                OnNext(650, 4)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_4()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1),
                OnNext(550, 10)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, (p0, p1, p2, p3) => p0 + p1 + p2 + p3).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 40, 4),
                OnNext(550, 13),
                OnNext(650, 5)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_5()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1),
                OnNext(550, 10)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, (p0, p1, p2, p3, p4) => p0 + p1 + p2 + p3 + p4).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 50, 5),
                OnNext(550, 14),
                OnNext(650, 6)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_6()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1),
                OnNext(550, 10)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, (p0, p1, p2, p3, p4, p5) => p0 + p1 + p2 + p3 + p4 + p5).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 60, 6),
                OnNext(550, 15),
                OnNext(650, 7)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_7()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1),
                OnNext(550, 10)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, (p0, p1, p2, p3, p4, p5, p6) => p0 + p1 + p2 + p3 + p4 + p5 + p6).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 70, 7),
                OnNext(550, 16),
                OnNext(650, 8)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_8()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1),
                OnNext(550, 10)
            );

            var io7 = Scheduler.CreateHotObservable(
                OnNext(210 + 70, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, io7, (p0, p1, p2, p3, p4, p5, p6, p7) => p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 80, 8),
                OnNext(550, 17),
                OnNext(650, 9)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_9()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1)
            );

            var io7 = Scheduler.CreateHotObservable(
                OnNext(210 + 70, 1),
                OnNext(550, 10)
            );

            var io8 = Scheduler.CreateHotObservable(
                OnNext(210 + 80, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, io7, io8, (p0, p1, p2, p3, p4, p5, p6, p7, p8) => p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 90, 9),
                OnNext(550, 18),
                OnNext(650, 10)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_10()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1)
            );

            var io7 = Scheduler.CreateHotObservable(
                OnNext(210 + 70, 1)
            );

            var io8 = Scheduler.CreateHotObservable(
                OnNext(210 + 80, 1),
                OnNext(550, 10)
            );

            var io9 = Scheduler.CreateHotObservable(
                OnNext(210 + 90, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, io7, io8, io9, (p0, p1, p2, p3, p4, p5, p6, p7, p8, p9) => p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 100, 10),
                OnNext(550, 19),
                OnNext(650, 11)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_11()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1)
            );

            var io7 = Scheduler.CreateHotObservable(
                OnNext(210 + 70, 1)
            );

            var io8 = Scheduler.CreateHotObservable(
                OnNext(210 + 80, 1)
            );

            var io9 = Scheduler.CreateHotObservable(
                OnNext(210 + 90, 1),
                OnNext(550, 10)
            );

            var io10 = Scheduler.CreateHotObservable(
                OnNext(210 + 100, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, io7, io8, io9, io10, (p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 110, 11),
                OnNext(550, 20),
                OnNext(650, 12)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_12()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1)
            );

            var io7 = Scheduler.CreateHotObservable(
                OnNext(210 + 70, 1)
            );

            var io8 = Scheduler.CreateHotObservable(
                OnNext(210 + 80, 1)
            );

            var io9 = Scheduler.CreateHotObservable(
                OnNext(210 + 90, 1)
            );

            var io10 = Scheduler.CreateHotObservable(
                OnNext(210 + 100, 1),
                OnNext(550, 10)
            );

            var io11 = Scheduler.CreateHotObservable(
                OnNext(210 + 110, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, io7, io8, io9, io10, io11, (p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 120, 12),
                OnNext(550, 21),
                OnNext(650, 13)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_13()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1)
            );

            var io7 = Scheduler.CreateHotObservable(
                OnNext(210 + 70, 1)
            );

            var io8 = Scheduler.CreateHotObservable(
                OnNext(210 + 80, 1)
            );

            var io9 = Scheduler.CreateHotObservable(
                OnNext(210 + 90, 1)
            );

            var io10 = Scheduler.CreateHotObservable(
                OnNext(210 + 100, 1)
            );

            var io11 = Scheduler.CreateHotObservable(
                OnNext(210 + 110, 1),
                OnNext(550, 10)
            );

            var io12 = Scheduler.CreateHotObservable(
                OnNext(210 + 120, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, io7, io8, io9, io10, io11, io12, (p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 130, 13),
                OnNext(550, 22),
                OnNext(650, 14)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_14()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1)
            );

            var io7 = Scheduler.CreateHotObservable(
                OnNext(210 + 70, 1)
            );

            var io8 = Scheduler.CreateHotObservable(
                OnNext(210 + 80, 1)
            );

            var io9 = Scheduler.CreateHotObservable(
                OnNext(210 + 90, 1)
            );

            var io10 = Scheduler.CreateHotObservable(
                OnNext(210 + 100, 1)
            );

            var io11 = Scheduler.CreateHotObservable(
                OnNext(210 + 110, 1)
            );

            var io12 = Scheduler.CreateHotObservable(
                OnNext(210 + 120, 1),
                OnNext(550, 10)
            );

            var io13 = Scheduler.CreateHotObservable(
                OnNext(210 + 130, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, io7, io8, io9, io10, io11, io12, io13, (p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 140, 14),
                OnNext(550, 23),
                OnNext(650, 15)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_15()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1)
            );

            var io7 = Scheduler.CreateHotObservable(
                OnNext(210 + 70, 1)
            );

            var io8 = Scheduler.CreateHotObservable(
                OnNext(210 + 80, 1)
            );

            var io9 = Scheduler.CreateHotObservable(
                OnNext(210 + 90, 1)
            );

            var io10 = Scheduler.CreateHotObservable(
                OnNext(210 + 100, 1)
            );

            var io11 = Scheduler.CreateHotObservable(
                OnNext(210 + 110, 1)
            );

            var io12 = Scheduler.CreateHotObservable(
                OnNext(210 + 120, 1)
            );

            var io13 = Scheduler.CreateHotObservable(
                OnNext(210 + 130, 1),
                OnNext(550, 10)
            );

            var io14 = Scheduler.CreateHotObservable(
                OnNext(210 + 140, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, io7, io8, io9, io10, io11, io12, io13, io14, (p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 150, 15),
                OnNext(550, 24),
                OnNext(650, 16)
            );
        }

        [TestMethod]
        public void CombineLatest_SaveAndReload_16()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(500, state),
                OnLoad(600, state),
            };

            var io0 = Scheduler.CreateHotObservable(
                OnNext(210 + 0, 1)
            );

            var io1 = Scheduler.CreateHotObservable(
                OnNext(210 + 10, 1)
            );

            var io2 = Scheduler.CreateHotObservable(
                OnNext(210 + 20, 1)
            );

            var io3 = Scheduler.CreateHotObservable(
                OnNext(210 + 30, 1)
            );

            var io4 = Scheduler.CreateHotObservable(
                OnNext(210 + 40, 1)
            );

            var io5 = Scheduler.CreateHotObservable(
                OnNext(210 + 50, 1)
            );

            var io6 = Scheduler.CreateHotObservable(
                OnNext(210 + 60, 1)
            );

            var io7 = Scheduler.CreateHotObservable(
                OnNext(210 + 70, 1)
            );

            var io8 = Scheduler.CreateHotObservable(
                OnNext(210 + 80, 1)
            );

            var io9 = Scheduler.CreateHotObservable(
                OnNext(210 + 90, 1)
            );

            var io10 = Scheduler.CreateHotObservable(
                OnNext(210 + 100, 1)
            );

            var io11 = Scheduler.CreateHotObservable(
                OnNext(210 + 110, 1)
            );

            var io12 = Scheduler.CreateHotObservable(
                OnNext(210 + 120, 1)
            );

            var io13 = Scheduler.CreateHotObservable(
                OnNext(210 + 130, 1)
            );

            var io14 = Scheduler.CreateHotObservable(
                OnNext(210 + 140, 1),
                OnNext(550, 10)
            );

            var io15 = Scheduler.CreateHotObservable(
                OnNext(210 + 150, 1),
                OnNext(650, 2)
            );

            var res = Scheduler.Start(
                () => io0.CombineLatest(io1, io2, io3, io4, io5, io6, io7, io8, io9, io10, io11, io12, io13, io14, io15, (p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(200 + 160, 16),
                OnNext(550, 25),
                OnNext(650, 17)
            );
        }

        #endregion
    }
}