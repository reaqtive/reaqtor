// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class ReferenceEqualityComparerTests
    {
        [TestMethod]
        public void ReferenceEqualityComparer_Basics()
        {
            var o1 = new object();
            var o2 = new object();

            Assert.IsTrue(ReferenceEqualityComparer<object>.Instance.Equals(o1, o1));
            Assert.IsTrue(ReferenceEqualityComparer<object>.Instance.Equals(o2, o2));

            Assert.IsFalse(ReferenceEqualityComparer<object>.Instance.Equals(o1, o2));
            Assert.IsFalse(ReferenceEqualityComparer<object>.Instance.Equals(o2, o1));

            Assert.AreEqual(o1.GetHashCode(), ReferenceEqualityComparer<object>.Instance.GetHashCode(o1));
            Assert.AreEqual(o2.GetHashCode(), ReferenceEqualityComparer<object>.Instance.GetHashCode(o2));
        }
    }
}
