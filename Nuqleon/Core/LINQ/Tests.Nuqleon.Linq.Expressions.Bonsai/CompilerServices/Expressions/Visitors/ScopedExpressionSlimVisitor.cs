// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices.Bonsai;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Bonsai.CompilerServices
{
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
                AssertEx.ThrowsException<ArgumentNullException>(() => base.Push(default(IEnumerable<ParameterExpressionSlim>)), ex => Assert.AreEqual("parameters", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.Push(default(IEnumerable<KeyValuePair<ParameterExpressionSlim, ParameterExpressionSlim>>)), ex => Assert.AreEqual("scope", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => { base.TryLookup(parameter: null, out _); }, ex => Assert.AreEqual("parameter", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
            }

            protected override ParameterExpressionSlim GetState(ParameterExpressionSlim parameter) => parameter;
        }
    }
}
