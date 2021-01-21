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
using System.Linq.Expressions;
using System.Reflection;
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

            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Constant(value: null, t), ex => Assert.AreEqual("value", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Constant(v, type: null), ex => Assert.AreEqual("type", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => new ConstantExpressionSlim(value: null), ex => Assert.AreEqual("value", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TypedConstantExpressionSlim(value: null, t), ex => Assert.AreEqual("value", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TypedConstantExpressionSlim(v, type: null), ex => Assert.AreEqual("type", ex.ParamName));
        }
    }
}
