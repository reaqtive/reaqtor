// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using Reaqtive.Scheduler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Test.Reaqtive.Scheduler.Tasks;

namespace Test.Reaqtive.Scheduler
{
    [TestClass]
    public class YieldTokenTests
    {
        [TestMethod]
        public void YieldToken_None()
        {
            Assert.IsFalse(YieldToken.None.IsYieldRequested);

            Assert.AreEqual(0, YieldToken.None.GetHashCode());

            var t1 = YieldToken.None;
            var t2 = YieldToken.None;

            Assert.IsTrue(t1.Equals((object)t2));
            Assert.IsFalse(t1.Equals("bar"));

            Assert.IsTrue(t1.Equals(t2));
            Assert.IsTrue(t1 == t2);
            Assert.IsFalse(t1 != t2);
        }

        [TestMethod]
        public void YieldToken_Basics()
        {
            var s = new MyYieldTokenSource();

            var t = new YieldToken(s);

            foreach (var token in new[]
            {
                t,
                s.Token,
            })
            {
                s.IsYieldRequested = false;
                Assert.IsFalse(token.IsYieldRequested);

                s.IsYieldRequested = true;
                Assert.IsTrue(token.IsYieldRequested);
            }

            var t1 = t;
            var t2 = t;

            Assert.IsTrue(t1.Equals((object)t2));
            Assert.IsFalse(t1.Equals("bar"));

            Assert.IsTrue(t1.Equals(t2));
            Assert.IsTrue(t1 == t2);
            Assert.IsFalse(t1 != t2);

            Assert.IsFalse(t == YieldToken.None);
            Assert.IsFalse(YieldToken.None == t);
            Assert.IsFalse(YieldToken.None.Equals(t));
            Assert.IsFalse(YieldToken.None.Equals((object)t));
            Assert.IsFalse(t.Equals(YieldToken.None));
            Assert.IsFalse(t.Equals((object)YieldToken.None));

            Assert.AreNotEqual(0, t.GetHashCode());
        }

        private sealed class MyYieldTokenSource : IYieldTokenSource
        {
            public YieldToken Token => new YieldToken(this);

            public bool IsYieldRequested { get; set; }
        }
    }
}
