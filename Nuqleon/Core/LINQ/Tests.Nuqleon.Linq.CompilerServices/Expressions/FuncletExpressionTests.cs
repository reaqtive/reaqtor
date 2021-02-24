// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class FuncletExpressionTests
    {
        [TestMethod]
        public void FuncletExpression_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => FuncletExpression.Create(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => FuncletExpression.Create<int>(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void FuncletExpression_Basics1()
        {
            var c = Expression.Constant(42);
            var f = FuncletExpression.Create(c);

            Assert.IsTrue(f.CanReduce);
            Assert.AreEqual(ExpressionType.Extension, f.NodeType);
            Assert.AreEqual(typeof(int), f.Type);
            Assert.AreEqual("Eval(42)", f.ToString());

            Assert.AreEqual(42, f.Reduce().Evaluate<int>());
            Assert.AreEqual(42, f.ReduceAndCheck().Evaluate<int>());
            Assert.AreEqual(42, f.ReduceExtensions().Evaluate<int>());

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(c, f.Reduce()));
            Assert.IsTrue(eq.Equals(c, f.ReduceAndCheck()));
            Assert.IsTrue(eq.Equals(c, f.ReduceExtensions()));
        }

        [TestMethod]
        public void FuncletExpression_Basics2()
        {
            var a = new Foo();

            var c = Expression.Constant(a);
            var f = FuncletExpression.Create(c);

            a.x = 42;

            Assert.AreEqual(42, f.Reduce().Evaluate<Foo>().x);

            a.x = 43;

            Assert.AreEqual(43, f.ReduceAndCheck().Evaluate<Foo>().x);

            a.x = 44;

            Assert.AreEqual(44, f.ReduceExtensions().Evaluate<Foo>().x);
        }

        private sealed class Foo
        {
            public int x;
        }

        [TestMethod]
        public void FuncletExpression_Basics3()
        {
            var c = Expression.Constant("bar");
            var f = FuncletExpression.Create<string>(c);

            var r = new NoOpVisitor().Visit(f);

            var eq = new ExpressionEqualityComparator();
            Assert.IsTrue(eq.Equals(Expression.Constant("bar"), r));
        }

        private sealed class NoOpVisitor : ExpressionVisitor
        {
        }

        private string s1 = "FOO";

        [TestMethod]
        public void FuncletExpression_ManOrBoyTest()
        {
            var fws = new FuncletizingAllowListScanner
            {
                DeclaringTypes =
                {
                    typeof(string)
                }
            };

            var s2 = "bar";

            var e = (Expression<Func<string>>)(() => s1.ToLower() + s2.ToUpper() + Environment.MachineName);

            var f = fws.Visit(e.Body);

            Assert.AreEqual("fooBAR" + Environment.MachineName, f.Evaluate<string>());

            s1 = "BAR";
            s2 = "qux";

            Assert.AreEqual("barQUX" + Environment.MachineName, f.Evaluate<string>());
        }

        private sealed class FuncletizingAllowListScanner : ExpressionMemberAllowListScanner
        {
            protected override Expression ResolveExpression<T>(T expression, MemberInfo member, Func<T, Expression> visit)
            {
                return FuncletExpression.Create(expression);
            }
        }
    }
}
