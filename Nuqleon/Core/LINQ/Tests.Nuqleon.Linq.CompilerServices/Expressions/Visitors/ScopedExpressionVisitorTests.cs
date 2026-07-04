// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices;

[TestClass]
public class ScopedExpressionVisitorTests
{
    [TestMethod]
    public void ScopedExpressionVisitor_ProtectedNulls()
    {
        new ProtectedNullVisitor().Do();
    }

    private sealed class ProtectedNullVisitor : ScopedExpressionVisitor<ParameterExpression>
    {
        public void Do()
        {
            Assert.IsNull(base.VisitBlock(node: null));
            Assert.IsNull(base.VisitBlockCore(node: null));
            Assert.IsNull(base.VisitCatchBlock(node: null));
            Assert.IsNull(base.VisitCatchBlockCore(node: null));
            Assert.IsNull(base.VisitLambda<Action>(node: null));
            Assert.IsNull(base.VisitLambdaCore<Action>(node: null));

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => base.Push(default(IEnumerable<ParameterExpression>)));
            Assert.AreEqual("parameters", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => base.Push(default(IEnumerable<KeyValuePair<ParameterExpression, ParameterExpression>>)));
            Assert.AreEqual("scope", ex2.ParamName);
            var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => { base.TryLookup(parameter: null, out _); });
            Assert.AreEqual("parameter", ex3.ParamName);
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        protected override ParameterExpression GetState(ParameterExpression parameter)
        {
            return parameter;
        }
    }
}
