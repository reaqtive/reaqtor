// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void ChangeType_Block()
        {
            var p = Expression.Parameter(typeof(string));

            AssertChangeType(
                Expression.Block(p),
                typeof(object),
                Expression.Block(typeof(object), p)
            );
        }

        [TestMethod]
        public void ChangeType_Conditional()
        {
            var b = Expression.Parameter(typeof(bool));
            var s1 = Expression.Parameter(typeof(string));
            var s2 = Expression.Parameter(typeof(string));

            AssertChangeType(
                Expression.Condition(b, s1, s2),
                typeof(object),
                Expression.Condition(b, s1, s2, typeof(object))
            );
        }

        [TestMethod]
        public void ChangeType_Constant()
        {
            var s = "bar";

            AssertChangeType(
                Expression.Constant(s),
                typeof(object),
                Expression.Constant(s, typeof(object))
            );
        }

        [TestMethod]
        public void ChangeType_Constant_Void()
        {
            AssertChangeType(
                Expression.Constant(42),
                typeof(void),
                Expression.Empty()
            );
        }

        [TestMethod]
        public void ChangeType_Default()
        {
            AssertChangeType(
                Expression.Default(typeof(string)),
                typeof(object),
                Expression.Default(typeof(object))
            );
        }

        [TestMethod]
        public void ChangeType_Goto()
        {
            var target = Expression.Label(typeof(object));
            var s = Expression.Parameter(typeof(string));

            AssertChangeType(
                Expression.Goto(target, s),
                typeof(object),
                Expression.Goto(target, s, typeof(object))
            );
        }

        [TestMethod]
        public void ChangeType_Switch()
        {
            var x = Expression.Parameter(typeof(int));
            var s1 = Expression.Parameter(typeof(string));
            var s2 = Expression.Parameter(typeof(string));
            var c1 = Expression.SwitchCase(s1, Expression.Constant(1));
            var c2 = Expression.SwitchCase(s2, Expression.Constant(2));
            var d = Expression.Parameter(typeof(string));

            AssertChangeType(
                Expression.Switch(x, d, c1, c2),
                typeof(object),
                Expression.Switch(typeof(object), x, d, comparison: null, c1, c2)
            );
        }

        [TestMethod]
        public void ChangeType_Throw()
        {
            var ex = Expression.Parameter(typeof(Expression));

            AssertChangeType(
                Expression.Throw(ex, typeof(string)),
                typeof(object),
                Expression.Throw(ex, typeof(object))
            );
        }

        [TestMethod]
        public void ChangeType_Try()
        {
            var s = Expression.Parameter(typeof(string));

            AssertChangeType(
                Expression.TryFinally(s, Expression.Empty()),
                typeof(object),
                Expression.MakeTry(typeof(object), s, Expression.Empty(), fault: null, handlers: null)
            );
        }

        [TestMethod]
        public void HasExactType()
        {
            var opt = new TypingTestExpressionOptimizer();

            foreach (var e in new Expression[]
            {
                Expression.Constant(42),
                Expression.Default(typeof(int)),

                Expression.NewArrayBounds(typeof(int), Expression.Constant(1)),
                Expression.NewArrayInit(typeof(int), Expression.Constant(1)),

                Expression.New(typeof(string).GetConstructor(new[] { typeof(char), typeof(int) }), Expression.Constant('*'), Expression.Constant(1)),
                Expression.ListInit(Expression.New(typeof(List<int>).GetConstructor(Type.EmptyTypes)), Expression.ElementInit(typeof(List<int>).GetMethod(nameof(List<int>.Add), new[] { typeof(int) }), Expression.Constant(1))),
                Expression.MemberInit(Expression.New(typeof(StrongBox<int>).GetConstructor(Type.EmptyTypes)), Expression.Bind(typeof(StrongBox<int>).GetField(nameof(StrongBox<int>.Value)), Expression.Constant(1))),

                Expression.IsFalse(Expression.Constant(true)),
                Expression.IsTrue(Expression.Constant(true)),

                Expression.TypeEqual(Expression.Constant(42), typeof(int)),
                Expression.TypeIs(Expression.Constant(42), typeof(int)),

                Expression.Call(Expression.Constant(""), typeof(string).GetMethod(nameof(string.ToUpper), Type.EmptyTypes)),

                Expression.Convert(Expression.Default(typeof(object)), typeof(int)),
            })
            {
                Assert.IsTrue(opt.HasExactType(e), e.ToString());
            }

            foreach (var e in new Expression[]
            {
                Expression.Constant(null),
                Expression.Constant("", typeof(object)),

                Expression.Default(typeof(int?)),
                Expression.Default(typeof(string)),

                Expression.Convert(Expression.Default(typeof(string)), typeof(object)),
                Expression.Convert(Expression.Default(typeof(object)), typeof(int?)),
            })
            {
                Assert.IsFalse(opt.HasExactType(e), e.ToString());
            }
        }

        private static void AssertChangeType(Expression expression, Type type, Expression expected)
        {
            var res = new TypingTestExpressionOptimizer().ChangeType(expression, type);
            AreEqual(expected, res);
        }

        private sealed class TypingTestExpressionOptimizer : ExpressionOptimizer
        {
            public TypingTestExpressionOptimizer()
                : base(new DefaultSemanticProvider(), new DefaultEvaluatorFactory())
            {
            }

            public new Expression ChangeType(Expression expression, Type type) => base.ChangeType(expression, type);

            public new bool HasExactType(Expression expression) => base.HasExactType(expression);
        }
    }
}
