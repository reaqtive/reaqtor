// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Linq.CompilerServices.Bonsai;
using System.Linq.Expressions;

namespace Tests.System.Linq.Expressions.Bonsai.CompilerServices;

[TestClass]
public class ScopedExpressionSlimVisitorTests : TestBase
{
    [TestMethod]
    public void ScopedExpressionSlimVisitor_ProtectedNulls()
    {
        new ProtectedNullVisitor().Do();
    }

    private sealed class ProtectedNullVisitor : ScopedExpressionSlimVisitor<ParameterExpressionSlim>
    {
        public void Do()
        {
            Assert.IsNull(base.VisitLambda(node: null));
            Assert.IsNull(base.VisitLambdaCore(node: null));

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => base.Push(default(IEnumerable<ParameterExpressionSlim>)));
            Assert.AreEqual("parameters", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => base.Push(default(IEnumerable<KeyValuePair<ParameterExpressionSlim, ParameterExpressionSlim>>)));
            Assert.AreEqual("scope", ex2.ParamName);
            var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => { base.TryLookup(parameter: null, out _); });
            Assert.AreEqual("parameter", ex3.ParamName);
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        protected override ParameterExpressionSlim GetState(ParameterExpressionSlim parameter) => parameter;
    }
}
