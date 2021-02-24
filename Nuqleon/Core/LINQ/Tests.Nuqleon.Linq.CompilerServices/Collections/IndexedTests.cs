// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class IndexedTests
    {
        [TestMethod]
        public void Indexed_Equality()
        {
            var i1 = new Indexed<string>("bar", 0);
            var i2 = new Indexed<string>("bar", 0);
            var i3 = new Indexed<string>("bar", 1);
            var i4 = new Indexed<string>("foo", 0);

            AssertEqual(i1, i2);
            AssertNotEqual(i1, i3);
            AssertNotEqual(i2, i3);
            AssertNotEqual(i1, i4);
            AssertNotEqual(i2, i4);

            Assert.AreNotEqual(i1, null);
            Assert.AreNotEqual(i1, "foo");
        }

        [TestMethod]
        public void Indexed_ToString()
        {
            var i1 = new Indexed<string>("bar", 0);
            var i2 = new Indexed<string>("foo", 1);

            Assert.AreEqual("[0] bar", i1.ToString());
            Assert.AreEqual("[1] foo", i2.ToString());
        }

        private static void AssertEqual<T>(Indexed<T> first, Indexed<T> second)
        {
            Assert.IsTrue(first == second);
            Assert.IsTrue(second == first);
            Assert.IsFalse(first != second);
            Assert.IsFalse(second != first);
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
            Assert.IsTrue(object.Equals(first, second));
            Assert.IsTrue(object.Equals(second, first));
            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
        }

        private static void AssertNotEqual<T>(Indexed<T> first, Indexed<T> second)
        {
            Assert.IsFalse(first == second);
            Assert.IsFalse(second == first);
            Assert.IsTrue(first != second);
            Assert.IsTrue(second != first);
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
            Assert.IsFalse(object.Equals(first, second));
            Assert.IsFalse(object.Equals(second, first));
            Assert.AreNotEqual(first.GetHashCode(), second.GetHashCode());
        }
    }
}
