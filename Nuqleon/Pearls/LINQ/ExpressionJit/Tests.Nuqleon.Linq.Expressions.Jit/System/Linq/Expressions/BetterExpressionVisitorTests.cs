// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace Tests
{
    [TestClass]
    public class BetterExpressionVisitorTests
    {
        [TestMethod]
        public void IfThenNoClone()
        {
            var visitor = new BetterExpressionVisitor();

            var b = IfThen(Default(typeof(bool)), Empty());
            var a = visitor.Visit(b);

            Assert.AreSame(b, a);
        }

        [TestMethod]
        public void ConditionalClones()
        {
            var visitor = new MyVisitor();

            var v = Throw(Default(typeof(Exception)), typeof(void));

            var b1 = Condition(Constant(true), Default(typeof(int)), Default(typeof(int)));
            var a1 = visitor.Visit(b1);
            Assert.AreNotSame(b1, a1);

            var b2 = Condition(Constant(true), Default(typeof(void)), Default(typeof(void)));
            var a2 = visitor.Visit(b2);
            Assert.AreNotSame(b2, a2);

            var b3 = Condition(Default(typeof(bool)), v, Default(typeof(void)));
            var a3 = visitor.Visit(b3);
            Assert.AreNotSame(b3, a3);

            var b4 = Condition(Default(typeof(bool)), Default(typeof(void)), v);
            var a4 = visitor.Visit(b4);
            Assert.AreNotSame(b4, a4);
        }

        private sealed class MyVisitor : BetterExpressionVisitor
        {
            protected override Expression VisitConstant(ConstantExpression node)
            {
                return Constant(node.Value, node.Type);
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                if (node.NodeType == ExpressionType.Throw)
                {
                    return Throw(Visit(node.Operand));
                }

                return base.VisitUnary(node);
            }
        }
    }
}
