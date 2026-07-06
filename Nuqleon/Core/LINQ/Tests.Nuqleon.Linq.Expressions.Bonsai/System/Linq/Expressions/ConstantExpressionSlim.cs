// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Linq.Expressions
{
    [TestClass]
    public class ConstantExpressionSlimTests : TestBase
    {
        [TestMethod]
        public void ConstantExpressionSlim_ArgumentChecks()
        {
            var t = typeof(string).ToTypeSlim();
            var v = ObjectSlim.Create(value: null, t, typeof(string));

            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => ExpressionSlim.Constant(value: null, t));
            Assert.AreEqual("value", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => ExpressionSlim.Constant(v, type: null));
            Assert.AreEqual("type", ex2.ParamName);

            var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => new ConstantExpressionSlim(value: null));
            Assert.AreEqual("value", ex3.ParamName);
            var ex4 = Assert.ThrowsExactly<ArgumentNullException>(() => new TypedConstantExpressionSlim(value: null, t));
            Assert.AreEqual("value", ex4.ParamName);
            var ex5 = Assert.ThrowsExactly<ArgumentNullException>(() => new TypedConstantExpressionSlim(v, type: null));
            Assert.AreEqual("type", ex5.ParamName);
        }
    }
}
