// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class CompilerGeneratedNameEliminatorTests
    {
        [TestMethod]
        public void CompilerGeneratedNameEliminator_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => CompilerGeneratedNameEliminator.Prettify(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void CompilerGeneratedNameEliminator_Simple1()
        {
            var q = (from x in new[] { 1, 2, 3 }.AsQueryable() let y = x + 1 let z = x + y select x * y + z).Expression;
            var p = CompilerGeneratedNameEliminator.Prettify(q);
            new CheckVariableNames().Visit(p);
        }

        [TestMethod]
        public void CompilerGeneratedNameEliminator_Simple2()
        {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1806 // Use of Any() claimed to be discarded. Seems like an analyzer bug but failing to repro in isolation.

            var q = (from x in new[] { 1, 2, 3 }.AsQueryable() let y = x + 1 where (from z in new[] { 4, 5 } let a = z + y select z).Any(b => b > 0) select x * y).Expression;
            var p = CompilerGeneratedNameEliminator.Prettify(q);
            new CheckVariableNames().Visit(p);

#pragma warning restore CA1806
#pragma warning restore IDE0079
        }

        [TestMethod]
        public void CompilerGeneratedNameEliminator_Simple3()
        {
            var p = Expression.Parameter(typeof(int));
            var q = CompilerGeneratedNameEliminator.Prettify(p);
            Assert.AreSame(p, q);
        }

        [TestMethod]
        public void CompilerGeneratedNameEliminator_Simple4()
        {
            var p = (from x in new[] { 1, 2, 3 }.AsQueryable() where x > 0 select x * x).Expression;
            var q = CompilerGeneratedNameEliminator.Prettify(p);
            Assert.AreSame(p, q);
        }

        [TestMethod]
        public void CompilerGeneratedNameEliminator_Simple5()
        {
            var p = Expression.Parameter(typeof(int), "x");
            var q = CompilerGeneratedNameEliminator.Prettify(p);
            Assert.AreSame(p, q);
        }

        [TestMethod]
        public void CompilerGeneratedNameEliminator_Simple6()
        {
            var p = Expression.Parameter(typeof(int), "<>h__TransparentIdentifier123");
            var q = CompilerGeneratedNameEliminator.Prettify(p);
            Assert.AreEqual(p.Type, q.Type);
            Assert.AreEqual("t", ((ParameterExpression)q).Name);
        }

        [TestMethod]
        public void CompilerGeneratedNameEliminator_Simple7()
        {
            var x = Expression.Parameter(typeof(int), "<>h__TransparentIdentifier1");
            var y = Expression.Parameter(typeof(int), "<>h__TransparentIdentifier2");
            var p = Expression.Lambda<Func<int, Func<int, int>>>(Expression.Lambda<Func<int, int>>(x, y), x);
            var q = (Expression<Func<int, Func<int, int>>>)CompilerGeneratedNameEliminator.Prettify(p);
            Assert.AreEqual("t => t0 => t", q.ToString());
            Assert.AreEqual(2, p.Compile()(2)(3));
            Assert.AreEqual(2, q.Compile()(2)(3));
        }

        private sealed class CheckVariableNames : ExpressionVisitor
        {
            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node.Name != null && node.Name.StartsWith("<>"))
                {
                    Assert.Fail();
                }

                return base.VisitParameter(node);
            }
        }
    }
}
