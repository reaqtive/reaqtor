// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class AssemblySlimTests : TestBase
    {
        [TestMethod]
        public void AssemblySlim_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentException>(() => new AssemblySlim(name: null), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => new AssemblySlim(name: ""), ex => Assert.AreEqual("name", ex.ParamName));
        }

        [TestMethod]
        public void AssemblySlim_ToString()
        {
            var assembly = new AssemblySlim("Foo");
            Assert.AreEqual("Foo", assembly.ToString());
        }

        [TestMethod]
        public void AssemblySlim_Equality()
        {
            var foo1 = new AssemblySlim("Foo");
            var foo2 = new AssemblySlim("Foo");

            var bar1 = new AssemblySlim("Bar");
            var bar2 = new AssemblySlim("Bar");

            Assert.IsTrue(default(AssemblySlim) == null);
            Assert.IsFalse(foo1 == null);
            Assert.IsFalse(null == foo1);

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(foo1 == foo1);
            Assert.IsTrue(bar1 == bar1);
#pragma warning restore CS1718 // Comparison made to same variable

            Assert.IsTrue(foo1 == foo2);
            Assert.IsTrue(foo2 == foo1);
            Assert.IsTrue(bar1 == bar2);
            Assert.IsTrue(bar2 == bar1);

            Assert.IsFalse(bar1 == foo1);
            Assert.IsFalse(foo1 == bar1);
        }

        [TestMethod]
        public void AssemblySlim_Inequality()
        {
            var foo1 = new AssemblySlim("Foo");
            var foo2 = new AssemblySlim("Foo");

            var bar1 = new AssemblySlim("Bar");
            var bar2 = new AssemblySlim("Bar");

            Assert.IsFalse(default(AssemblySlim) != null);
            Assert.IsTrue(foo1 != null);
            Assert.IsTrue(null != foo1);

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsFalse(foo1 != foo1);
            Assert.IsFalse(bar1 != bar1);
#pragma warning restore CS1718 // Comparison made to same variable

            Assert.IsFalse(foo1 != foo2);
            Assert.IsFalse(foo2 != foo1);
            Assert.IsFalse(bar1 != bar2);
            Assert.IsFalse(bar2 != bar1);

            Assert.IsTrue(bar1 != foo1);
            Assert.IsTrue(foo1 != bar1);
        }

        [TestMethod]
        public void AssemblySlim_Equals()
        {
            var foo1 = new AssemblySlim("Foo");
            var foo2 = new AssemblySlim("Foo");

            var bar1 = new AssemblySlim("Bar");

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.IsFalse(foo1.Equals(default(object)));
            Assert.IsFalse(foo1.Equals(default(AssemblySlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            Assert.IsTrue(foo1.Equals(foo1));
            Assert.IsTrue(foo2.Equals(foo2));
            Assert.IsTrue(foo1.Equals(foo2));
            Assert.IsTrue(foo2.Equals(foo1));

            Assert.IsFalse(foo1.Equals(bar1));
            Assert.IsFalse(bar1.Equals(foo1));

            Assert.IsTrue(foo1.Equals((object)foo1));
            Assert.IsTrue(foo2.Equals((object)foo2));
            Assert.IsTrue(foo1.Equals((object)foo2));
            Assert.IsTrue(foo2.Equals((object)foo1));

            Assert.IsFalse(foo1.Equals((object)bar1));
            Assert.IsFalse(bar1.Equals((object)foo1));

            Assert.IsFalse(foo1.Equals("Foo"));
            Assert.IsFalse(bar1.Equals("Bar"));
            Assert.IsFalse(bar1.Equals(42));
        }

        [TestMethod]
        public void AssemblySlim_GetHashCode()
        {
            var foo1 = new AssemblySlim("Foo");
            var foo2 = new AssemblySlim("Foo");
            var foo3 = new AssemblySlim("Bar");

            Assert.AreEqual(foo1.GetHashCode(), foo2.GetHashCode());
            Assert.AreNotEqual(foo1.GetHashCode(), foo3.GetHashCode());
        }
    }
}
