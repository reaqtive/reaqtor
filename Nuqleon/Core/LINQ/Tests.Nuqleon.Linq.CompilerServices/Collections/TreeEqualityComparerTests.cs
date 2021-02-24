// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Linq.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TreeEqualityComparerTests
    {
        [TestMethod]
        public void TreeEqualityComparer_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new TreeEqualityComparer(comparer: null), ex => Assert.AreEqual("comparer", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TreeEqualityComparer<int>(comparer: null), ex => Assert.AreEqual("comparer", ex.ParamName));
        }

        [TestMethod]
        public void TreeEqualityComparer_Nulls()
        {
            var eq = new TreeEqualityComparer();

            var t = new Tree<int>(42);

            Assert.IsTrue(eq.Equals(null, null));
            Assert.IsFalse(eq.Equals(t, null));
            Assert.IsFalse(eq.Equals(null, t));

            Assert.AreNotEqual(0, eq.GetHashCode(null));
        }

        [TestMethod]
        public void TreeEqualityComparer_Generic_Nulls()
        {
            var eq = new TreeEqualityComparer<int>();

            var t = new Tree<int>(42);

            Assert.IsTrue(eq.Equals(null, null));
            Assert.IsFalse(eq.Equals(t, null));
            Assert.IsFalse(eq.Equals(null, t));

            Assert.AreNotEqual(0, eq.GetHashCode(null));
        }

        [TestMethod]
        public void TreeEqualityComparer_Nullary()
        {
            var eq = new TreeEqualityComparer();

            var t1 = new Tree<int>(42);
            var t2 = new Tree<int>(43);
            var t3 = new Tree<int>(42);

            Assert.IsTrue(eq.Equals(t1, t1));
            Assert.IsTrue(eq.Equals(t2, t2));
            Assert.IsTrue(eq.Equals(t3, t3));
            Assert.IsTrue(eq.Equals(t1, t3));
            Assert.IsTrue(eq.Equals(t3, t1));
            Assert.IsFalse(eq.Equals(t1, t2));
            Assert.IsFalse(eq.Equals(t2, t1));
            Assert.IsFalse(eq.Equals(t2, t3));
            Assert.IsFalse(eq.Equals(t3, t2));

            var h1 = eq.GetHashCode(t1);
            var h3 = eq.GetHashCode(t3);

            Assert.AreEqual(h1, h3);
        }

        [TestMethod]
        public void TreeEqualityComparer_Generic_Nullary()
        {
            var eq = new TreeEqualityComparer<int>();

            var t1 = new Tree<int>(42);
            var t2 = new Tree<int>(43);
            var t3 = new Tree<int>(42);

            Assert.IsTrue(eq.Equals(t1, t1));
            Assert.IsTrue(eq.Equals(t2, t2));
            Assert.IsTrue(eq.Equals(t3, t3));
            Assert.IsTrue(eq.Equals(t1, t3));
            Assert.IsTrue(eq.Equals(t3, t1));
            Assert.IsFalse(eq.Equals(t1, t2));
            Assert.IsFalse(eq.Equals(t2, t1));
            Assert.IsFalse(eq.Equals(t2, t3));
            Assert.IsFalse(eq.Equals(t3, t2));

            var h1 = eq.GetHashCode(t1);
            var h3 = eq.GetHashCode(t3);

            Assert.AreEqual(h1, h3);
        }

        [TestMethod]
        public void TreeEqualityComparer_Structural()
        {
            var eq = new TreeEqualityComparer();

            var t1 = new Tree<int>(42, new Tree<int>(43), new Tree<int>(44));
            var t2 = new Tree<int>(42, new Tree<int>(43));
            var t3 = new Tree<int>(42, new Tree<int>(43), new Tree<int>(44));

            Assert.IsTrue(eq.Equals(t1, t1));
            Assert.IsTrue(eq.Equals(t2, t2));
            Assert.IsTrue(eq.Equals(t3, t3));
            Assert.IsTrue(eq.Equals(t1, t3));
            Assert.IsTrue(eq.Equals(t3, t1));
            Assert.IsFalse(eq.Equals(t1, t2));
            Assert.IsFalse(eq.Equals(t2, t1));
            Assert.IsFalse(eq.Equals(t2, t3));
            Assert.IsFalse(eq.Equals(t3, t2));

            var h1 = eq.GetHashCode(t1);
            var h3 = eq.GetHashCode(t3);

            Assert.AreEqual(h1, h3);
        }

        [TestMethod]
        public void TreeEqualityComparer_Generic_Structural()
        {
            var eq = new TreeEqualityComparer<int>();

            var t1 = new Tree<int>(42, new Tree<int>(43), new Tree<int>(44));
            var t2 = new Tree<int>(42, new Tree<int>(43));
            var t3 = new Tree<int>(42, new Tree<int>(43), new Tree<int>(44));

            Assert.IsTrue(eq.Equals(t1, t1));
            Assert.IsTrue(eq.Equals(t2, t2));
            Assert.IsTrue(eq.Equals(t3, t3));
            Assert.IsTrue(eq.Equals(t1, t3));
            Assert.IsTrue(eq.Equals(t3, t1));
            Assert.IsFalse(eq.Equals(t1, t2));
            Assert.IsFalse(eq.Equals(t2, t1));
            Assert.IsFalse(eq.Equals(t2, t3));
            Assert.IsFalse(eq.Equals(t3, t2));

            var h1 = eq.GetHashCode(t1);
            var h3 = eq.GetHashCode(t3);

            Assert.AreEqual(h1, h3);
        }

        [TestMethod]
        public void TreeEqualityComparer_CustomComparer()
        {
            var eq = new TreeEqualityComparer(StringComparer.OrdinalIgnoreCase);

            var t1 = new Tree<string>("Foo", new Tree<string>("bAr"), new Tree<string>("QUX"));
            var t2 = new Tree<string>("foo", new Tree<string>("baz"), new Tree<string>("qux"));
            var t3 = new Tree<string>("foO", new Tree<string>("bar"), new Tree<string>("qux"));

            Assert.IsTrue(eq.Equals(t1, t1));
            Assert.IsTrue(eq.Equals(t2, t2));
            Assert.IsTrue(eq.Equals(t3, t3));
            Assert.IsTrue(eq.Equals(t1, t3));
            Assert.IsTrue(eq.Equals(t3, t1));
            Assert.IsFalse(eq.Equals(t1, t2));
            Assert.IsFalse(eq.Equals(t2, t1));
            Assert.IsFalse(eq.Equals(t2, t3));
            Assert.IsFalse(eq.Equals(t3, t2));

            var h1 = eq.GetHashCode(t1);
            var h3 = eq.GetHashCode(t3);

            Assert.AreEqual(h1, h3);
        }

        [TestMethod]
        public void TreeEqualityComparer_Generic_CustomComparer()
        {
            var eq = new TreeEqualityComparer<string>(StringComparer.OrdinalIgnoreCase);

            var t1 = new Tree<string>("Foo", new Tree<string>("bAr"), new Tree<string>("QUX"));
            var t2 = new Tree<string>("foo", new Tree<string>("baz"), new Tree<string>("qux"));
            var t3 = new Tree<string>("foO", new Tree<string>("bar"), new Tree<string>("qux"));

            Assert.IsTrue(eq.Equals(t1, t1));
            Assert.IsTrue(eq.Equals(t2, t2));
            Assert.IsTrue(eq.Equals(t3, t3));
            Assert.IsTrue(eq.Equals(t1, t3));
            Assert.IsTrue(eq.Equals(t3, t1));
            Assert.IsFalse(eq.Equals(t1, t2));
            Assert.IsFalse(eq.Equals(t2, t1));
            Assert.IsFalse(eq.Equals(t2, t3));
            Assert.IsFalse(eq.Equals(t3, t2));

            var h1 = eq.GetHashCode(t1);
            var h3 = eq.GetHashCode(t3);

            Assert.AreEqual(h1, h3);
        }
    }
}
