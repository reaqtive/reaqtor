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
    public class AlphaRenamerTests
    {
        [TestMethod]
        public void SyntaxTrie_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => AlphaRenamer.EliminateNameConflicts(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void SyntaxTrie_Unchanged1()
        {
            Expression<Func<int, Func<int, int>>> f = x => y => x;
            var g = AlphaRenamer.EliminateNameConflicts(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(f, g));
            Assert.AreSame(f, g);
        }

        [TestMethod]
        public void SyntaxTrie_Unchanged2()
        {
            Expression<Func<int, Func<int, int>>> f = x => y => y;
            var g = AlphaRenamer.EliminateNameConflicts(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(f, g));
            Assert.AreSame(f, g);
        }

        [TestMethod]
        public void SyntaxTrie_Unchanged3()
        {
            Expression<Func<int, Func<int, int>>> f = x => y => x;
            var g = AlphaRenamer.EliminateNameConflicts(f.Body);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(f.Body, g));
            Assert.AreSame(f.Body, g);
        }

        [TestMethod]
        public void SyntaxTrie_Unchanged4()
        {
            Expression<Func<int, Func<int, int>>> f = x => y => y;
            var g = AlphaRenamer.EliminateNameConflicts(f.Body);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(f.Body, g));
            Assert.AreSame(f.Body, g);
        }

        [TestMethod]
        public void SyntaxTrie_Unchanged5()
        {
            Expression<Action> f = () => Foo(x => x, x => x, x => x, x => x);
            var g = AlphaRenamer.EliminateNameConflicts(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(f, g));
            Assert.AreSame(f, g);
        }

#pragma warning disable CA1822 // Mark static
#pragma warning disable IDE0060 // Remove unused parameter
        private void Foo(Expression<Func<int, int>> f, Expression<Func<long, long>> g, Func<int, int> h, Func<long, long> i)
        {
        }
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CA1822

        [TestMethod]
        public void SyntaxTrie_Rename1()
        {
            var x = Expression.Parameter(typeof(int), "x");
            var y = Expression.Parameter(typeof(int), "x");

            var f = Expression.Lambda<Func<int, Func<int, int>>>(Expression.Lambda(y, y), x);
            var g = (Expression<Func<int, Func<int, int>>>)AlphaRenamer.EliminateNameConflicts(f);

            Assert.AreEqual(2, f.Compile()(1)(2));
            Assert.AreEqual(2, g.Compile()(1)(2));

            Assert.AreEqual("x => x => x", f.ToString());
            Assert.AreEqual("x => x0 => x0", g.ToString());
        }

        [TestMethod]
        public void SyntaxTrie_Rename2()
        {
            var x = Expression.Parameter(typeof(int), "x");
            var y = Expression.Parameter(typeof(int), "x");

            var f = Expression.Lambda<Func<int, Func<int, int>>>(Expression.Lambda(x, y), x);
            var g = (Expression<Func<int, Func<int, int>>>)AlphaRenamer.EliminateNameConflicts(f);

            Assert.AreEqual(1, f.Compile()(1)(2));
            Assert.AreEqual(1, g.Compile()(1)(2));

            Assert.AreEqual("x => x => x", f.ToString());
            Assert.AreEqual("x => x0 => x", g.ToString());
        }

        [TestMethod]
        public void SyntaxTrie_Rename3()
        {
            var x = Expression.Parameter(typeof(int), "x");
            var y = Expression.Parameter(typeof(int), "x0");
            var z = Expression.Parameter(typeof(int), "x");

            var f = Expression.Lambda(Expression.Lambda(Expression.Lambda(z, z), y), x);
            var g = AlphaRenamer.EliminateNameConflicts(f);

            Assert.AreEqual("x => x0 => x => x", f.ToString());
            Assert.AreEqual("x => x0 => x1 => x1", g.ToString());
        }

        [TestMethod]
        public void SyntaxTrie_Rename4()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));

            var f = Expression.Lambda(Expression.Lambda(Expression.Lambda(z, z), y), x);
            var g = AlphaRenamer.EliminateNameConflicts(f);

            Assert.AreEqual("Param_0 => Param_1 => Param_2 => Param_2", f.ToString());
            Assert.AreEqual("Param_0 => Param_1 => Param_2 => Param_2", g.ToString());
        }
    }
}
