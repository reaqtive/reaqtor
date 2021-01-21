// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices.Expressions.Rewriters.Misc
{
    [TestClass]
    public class BindingTests
    {
        [TestMethod]
        public void Binding_Equals()
        {
            var p1 = Expression.Parameter(typeof(int));
            var p2 = Expression.Parameter(typeof(string));
            var e1 = Expression.Default(typeof(int));
            var e2 = Expression.Default(typeof(string));
            var b1 = new Binding(p1, e1);
            var b2 = new Binding(p2, e2);
            var b3 = new Binding(p1, e1);

            Assert.IsTrue(b1.Equals(b3));
            Assert.IsTrue(b1.Equals((object)b3));
            Assert.IsFalse(b1.Equals(b2));
            Assert.IsFalse(b1.Equals(null));
            Assert.IsFalse(b1.Equals(new object()));
            Assert.IsTrue(b1 == b3);
            Assert.IsTrue(b1 != b2);
            Assert.AreEqual(b1.GetHashCode(), b3.GetHashCode());
        }
    }
}
