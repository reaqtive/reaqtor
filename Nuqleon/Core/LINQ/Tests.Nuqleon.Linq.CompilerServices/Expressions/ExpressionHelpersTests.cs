// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionHelpersTests
    {
        #region StripQuotes

        [TestMethod]
        public void StripQuotes_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionHelpers.StripQuotes(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void StripQuotes_NoLambda()
        {
            var c = Expression.Constant(42);
            var r = ExpressionHelpers.StripQuotes(c);
            Assert.AreSame(c, r);
        }

        [TestMethod]
        public void StripQuotes_Lambda()
        {
            var f = (Expression<Func<int, int>>)(x => x);
            var r = ExpressionHelpers.StripQuotes(f);
            Assert.AreSame(f, r);
        }

        [TestMethod]
        public void StripQuotes_QuotedLambda()
        {
            var f = (Expression<Action>)(() => QuoteMe(x => x));
            var e = ((MethodCallExpression)f.Body).Arguments[0];
            var r = ExpressionHelpers.StripQuotes(e);
            Assert.AreSame(((UnaryExpression)e).Operand, r);
        }

        #endregion

        #region Unquote

        [TestMethod]
        public void Unquote_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionHelpers.Unquote(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void Unquote_NoLambda()
        {
            var c = Expression.Constant(42);
            Assert.ThrowsException<InvalidCastException>(() => ExpressionHelpers.Unquote(c));
        }

        [TestMethod]
        public void Unquote_Lambda()
        {
            var f = (Expression<Func<int, int>>)(x => x);
            var r = ExpressionHelpers.Unquote(f);
            Assert.AreSame(f, r);
        }

        [TestMethod]
        public void Unquote_QuotedLambda()
        {
            var f = (Expression<Action>)(() => QuoteMe(x => x));
            var e = ((MethodCallExpression)f.Body).Arguments[0];
            var r = ExpressionHelpers.Unquote(e);
            Assert.AreSame(((UnaryExpression)e).Operand, r);
        }

        #endregion

        #region Helpers

#pragma warning disable IDE0060 // Remove unused parameter
        private static void QuoteMe(Expression<Func<int, int>> e)
        {
        }
#pragma warning restore IDE0060 // Remove unused parameter

        #endregion
    }
}
