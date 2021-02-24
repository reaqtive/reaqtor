// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleTrees.Arithmetic;
using SampleTrees.Numerical;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class BottomUpExpressionRewriterTests
    {
        [TestMethod]
        public void BottomUpExpressionRewriter_ArgumentChecks()
        {
            var burs = new BottomUpExpressionRewriter<ArithExpr>();

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Leaves.Add(default(Expression<Func<ConstantExpression, ArithExpr>>), 1), ex => Assert.AreEqual("convert", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Leaves.Add((ConstantExpression ce) => new Const(1), -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Leaves.Add(default(Expression<Func<ConstantExpression, ArithExpr>>), _ => true, 1), ex => Assert.AreEqual("convert", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Leaves.Add((ConstantExpression ce) => new Const(1), default(Expression<Func<ConstantExpression, bool>>), 1), ex => Assert.AreEqual("predicate", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Leaves.Add((ConstantExpression ce) => new Const(1), _ => true, -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(default(Expression<Func<int>>), () => new Const(1), 1), ex => Assert.AreEqual("pattern", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(default(Expression<Func<int, int>>), (x) => new Const(1), 1), ex => Assert.AreEqual("pattern", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(default(Expression<Func<int, int, int>>), (x, y) => new Const(1), 1), ex => Assert.AreEqual("pattern", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(default(Expression<Func<int, int, int, int>>), (x, y, z) => new Const(1), 1), ex => Assert.AreEqual("pattern", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(default(Expression<Func<int, int, int, int, int>>), (x, y, z, a) => new Const(1), 1), ex => Assert.AreEqual("pattern", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(() => 1, default(Expression<Func<ArithExpr>>), 1), ex => Assert.AreEqual("goal", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add((int x) => x + 1, default(Expression<Func<ArithExpr, ArithExpr>>), 1), ex => Assert.AreEqual("goal", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add((int x, int y) => x + y + 1, default(Expression<Func<ArithExpr, ArithExpr, ArithExpr>>), 1), ex => Assert.AreEqual("goal", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add((int x, int y, int z) => x + y + z + 1, default(Expression<Func<ArithExpr, ArithExpr, ArithExpr, ArithExpr>>), 1), ex => Assert.AreEqual("goal", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add((int x, int y, int z, int a) => x + y + z + a + 1, default(Expression<Func<ArithExpr, ArithExpr, ArithExpr, ArithExpr, ArithExpr>>), 1), ex => Assert.AreEqual("goal", ex.ParamName));

            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Rules.Add(() => 1, () => new Const(1), -1), ex => Assert.AreEqual("cost", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Rules.Add((int x) => x + 1, (x) => new Const(1), -1), ex => Assert.AreEqual("cost", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Rules.Add((int x, int y) => x + y + 1, (x, y) => new Const(1), -1), ex => Assert.AreEqual("cost", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Rules.Add((int x, int y, int z) => x + y + z + 1, (x, y, z) => new Const(1), -1), ex => Assert.AreEqual("cost", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Rules.Add((int x, int y, int z, int a) => x + y + z + a + 1, (x, y, z, a) => new Const(1), -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Fallbacks.Add(default(Expression<Func<Expression, ArithExpr>>), 1), ex => Assert.AreEqual("convert", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Fallbacks.Add((Expression e) => new Const(1), -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Fallbacks.Add(default(Expression<Func<Expression, ArithExpr>>), _ => true, 1), ex => Assert.AreEqual("convert", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Fallbacks.Add((Expression e) => new Const(1), default(Expression<Func<Expression, bool>>), 1), ex => Assert.AreEqual("predicate", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Fallbacks.Add((Expression e) => new Const(1), _ => true, -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rewrite(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Wildcards()
        {
            var p = Expression.Parameter(typeof(int), "x");

            var f = new ExpressionTreeWildcardFactory();
            var w = f.CreateWildcard(p);

            Assert.AreEqual(0, w.Children.Count);

            Assert.AreEqual(ExpressionTreeNodeType.Expression, w.Value.NodeType);
            var n = (ExpressionExpressionTreeNode)w.Value;

            Assert.AreSame(p, n.Expression);

            Assert.AreEqual("(int)*", w.ToString());
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Simple1()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<ArithExpr>
            {
                Leaves =
                {
                    { (ConstantExpression c) => new Const((int)c.Value), c => c.Type == typeof(int), 1 },
                },

                Rules =
                {
                    { (int x) => -x, (x) => new Neg(x), 1 },
                    { (int x) => Math.Abs(x), (x) => new Abs(x), 1 },
                    { (int x, int y) => x + y, (x, y) => new Add(x, y), 2 },
                    { (int x, int y) => x * y, (x, y) => new Mul(x, y), 3 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 1);
            AssertCount(burs.Rules, 4);
            AssertCount(burs.Fallbacks, 0);

            var e1 =
                Expression.Add(
                    Expression.Multiply(
                        Expression.Constant(1),
                        Expression.Call((MethodInfo)ReflectionHelpers.InfoOf((int x) => Math.Abs(x)),
                            Expression.Constant(-2)
                        )
                    ),
                    Expression.Negate(
                        Expression.Constant(3)
                    )
                );

            var a1 = burs.Rewrite(e1);

            Assert.AreEqual("Add(Mul(Const(1), Abs(Const(-2))), Neg(Const(3)))", a1.ToString());
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Simple2()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<NumExpr>
            {
                Leaves =
                {
                    { (ConstantExpression c) => new Val((int)c.Value), c => c.Type == typeof(int), 1 },
                },

                Rules =
                {
                    { (int x) => x + 1, (x) => new Inc(x), 1 },
                    { (int x) => 1 + x, (x) => new Inc(x), 1 },
                    { (int x, int y) => x + y, (x, y) => new Plus(x, y), 2 },
                    { (int x, int y) => x * y, (x, y) => new Times(x, y), 3 },
                    { (int x, int y, int z) => x * y + z, (x, y, z) => new TimesPlus(x, y, z), 4 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 1);
            AssertCount(burs.Rules, 5);
            AssertCount(burs.Fallbacks, 0);

            var e1 =
                Expression.Add(
                    Expression.Multiply(
                        Expression.Add(
                            Expression.Multiply(
                                Expression.Constant(2),
                                Expression.Constant(3)
                            ),
                            Expression.Constant(1)
                        ),
                        Expression.Multiply(
                            Expression.Constant(4),
                            Expression.Constant(5)
                        )
                    ),
                    Expression.Constant(6)
                );

            var a1 = burs.Rewrite(e1);

            Assert.AreEqual("TimesPlus(Inc(Times(2, 3)), Times(4, 5), 6)", a1.ToString());

            var e2 =
                Expression.Add(
                    Expression.Multiply(
                        Expression.Add(
                            Expression.Constant(1),
                            Expression.Multiply(
                                Expression.Constant(2),
                                Expression.Constant(3)
                            )
                        ),
                        Expression.Multiply(
                            Expression.Constant(4),
                            Expression.Constant(5)
                        )
                    ),
                    Expression.Constant(6)
                );

            var a2 = burs.Rewrite(e2);

            Assert.AreEqual("TimesPlus(Inc(Times(2, 3)), Times(4, 5), 6)", a2.ToString());
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Simple3()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<NumExpr>
            {
                Leaves =
                {
                    { (DefaultExpression d) => Val.Zero, 1 },
                    { (ConstantExpression c) => new Val((int)c.Value), c => c.Type == typeof(int), 1 },
                },

                Rules =
                {
                    { () => 1, () => Val.One, 1 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 2);
            AssertCount(burs.Rules, 1);
            AssertCount(burs.Fallbacks, 0);

            var e1 = Expression.Default(typeof(int));
            var a1 = burs.Rewrite(e1);
            Assert.AreSame(Val.Zero, a1);
            Assert.AreEqual("0", a1.ToString());

            var e2 = Expression.Constant(0, typeof(int));
            var a2 = burs.Rewrite(e2);
            Assert.AreNotSame(Val.Zero, a2);
            Assert.AreEqual("0", a2.ToString());

            var e3 = Expression.Constant(1, typeof(int));
            var a3 = burs.Rewrite(e3);
            Assert.AreSame(Val.One, a3);
            Assert.AreEqual("1", a3.ToString());

            var e4 = Expression.Constant(2, typeof(int));
            var a4 = burs.Rewrite(e4);
            Assert.AreEqual("2", a4.ToString());
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Simple4()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<ArithExpr>
            {
                Leaves =
                {
                    { (ConstantExpression c) => new Const((int)c.Value), c => c.Type == typeof(int), 1 },
                },

                Rules =
                {
                    { (int x, int y) => x + y, (x, y) => new Add(x, y), 2 },
                    { (int x, int y) => x * y, (x, y) => new Mul(x, y), 3 },
                },

                Fallbacks =
                {
                    { (Expression e) => new Lazy(e.Funcletize<int>()), 9 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 1);
            AssertCount(burs.Rules, 2);
            AssertCount(burs.Fallbacks, 1);

            var e1 =
                Expression.Add(
                    Expression.Multiply(
                        Expression.Constant(1),
                        Expression.Constant(2)
                    ),
                    Expression.Modulo(
                        Expression.Constant(3),
                        Expression.Constant(4)
                    )
                );

            var a1 = burs.Rewrite(e1);

            Assert.AreEqual("Add(Mul(Const(1), Const(2)), Lazy(() => (3 % 4)))", a1.ToString());
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Simple5()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<ArithExpr>
            {
                Leaves =
                {
                    { (ConstantExpression c) => new Const((int)c.Value), c => c.Type == typeof(int), 1 },
                },

                Rules =
                {
                    { (int x, int y) => x + y, (x, y) => new Add(x, y), 2 },
                    { (int x, int y) => x * y, (x, y) => new Mul(x, y), 3 },
                    { (int a, int b, int c, int d) => a * b + c * d, (a, b, c, d) => new Add(new Mul(c, d), new Mul(a, b)), 7 },
                },

                Fallbacks =
                {
                    { (Expression e) => new Lazy(e.Funcletize<int>()), e => e.NodeType != ExpressionType.Modulo, 9 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 1);
            AssertCount(burs.Rules, 3);
            AssertCount(burs.Fallbacks, 1);

            var e1 =
                Expression.Add(
                    Expression.Multiply(
                        Expression.Constant(1),
                        Expression.Constant(2)
                    ),
                    Expression.Modulo(
                        Expression.Constant(3),
                        Expression.Constant(4)
                    )
                );

            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e1));

            var e2 =
                Expression.Add(
                    Expression.Multiply(
                        Expression.Constant(1),
                        Expression.Constant(2)
                    ),
                    Expression.Divide(
                        Expression.Constant(3),
                        Expression.Constant(4)
                    )
                );

            var a2 = burs.Rewrite(e2);

            Assert.AreEqual("Add(Mul(Const(1), Const(2)), Lazy(() => (3 / 4)))", a2.ToString());

            var e3 =
                Expression.Add(
                    Expression.Multiply(
                        Expression.Constant(1),
                        Expression.Constant(2)
                    ),
                    Expression.Multiply(
                        Expression.Constant(3),
                        Expression.Constant(4)
                    )
                );

            var a3 = burs.Rewrite(e3);

            Assert.AreEqual("Add(Mul(Const(3), Const(4)), Mul(Const(1), Const(2)))", a3.ToString());
        }

        public static void BottomUpExpressionRewriter_MixedTypes()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<ArithExpr>
            {
                Leaves =
                {
                    { (ConstantExpression c) => new Const((int)c.Value), c => c.Type == typeof(int), 1 },
                    { (ConstantExpression c) => new Const(42), c => c.Type == typeof(long), 1 },
                },

                Rules =
                {
                    { (int x, int y) => x + y, (x, y) => new Add(x, y), 3 },
                    { (long x, long y) => x + y, (x, y) => new Mul(x, y), 2 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 2);
            AssertCount(burs.Rules, 2);

            var e1 =
                Expression.Add(
                    Expression.Constant(1),
                    Expression.Constant(2)
                );

            var a1 = burs.Rewrite(e1);

            Assert.AreEqual("Add(Const(1), Const(2))", a1.ToString());

            var e2 =
                Expression.Add(
                    Expression.Constant(1L),
                    Expression.Constant(2L)
                );

            var a2 = burs.Rewrite(e2);

            Assert.AreEqual("Mul(Const(42), Const(42))", a2.ToString());
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_ElementInit()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<ArithExpr>
            {
                Leaves =
                {
                    { (ConstantExpression c) => new Const((int)c.Value), c => c.Type == typeof(int), 1 },
                },

                Rules =
                {
                    { (int x, int y) => new List<int> { x, 1, y, 2 }, (x, y) => new Add(new Const(3), new Mul(x, y)), 2 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 1);
            AssertCount(burs.Rules, 1);
            AssertCount(burs.Fallbacks, 0);

            var e = ((Expression<Func<List<int>>>)(() => new List<int> { 3, 1, 4, 2 })).Body;
            var a = burs.Rewrite(e);
            Assert.AreEqual("Add(Const(3), Mul(Const(3), Const(4)))", a.ToString());
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_MemberBinding1()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<ArithExpr>
            {
                Leaves =
                {
                    { (ConstantExpression c) => new Const((int)c.Value), c => c.Type == typeof(int), 1 },
                },

                Rules =
                {
                    { (int x, int y) => new Adder { x = x, Y = y }, (x, y) => new Add(x, y), 2 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 1);
            AssertCount(burs.Rules, 1);
            AssertCount(burs.Fallbacks, 0);

            var e1 = ((Expression<Func<Adder>>)(() => new Adder { x = 2, Y = 3 })).Body;
            var a1 = burs.Rewrite(e1);
            Assert.AreEqual("Add(Const(2), Const(3))", a1.ToString());

            var e2 = ((Expression<Func<Adder>>)(() => new Adder(2) { x = 2, Y = 3 })).Body;
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e2));

            var e3 = ((Expression<Func<Adder>>)(() => new Adder { x = 2 })).Body;
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e3));

            var e4 = ((Expression<Func<Muller>>)(() => new Muller { x = 2, Y = 3 })).Body;
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e4));

            var e5 = ((Expression<Func<Adder>>)(() => new Adder { Y = 3, x = 2 })).Body; // Order matters due to side-effects
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e5));

            var e6 = ((Expression<Func<Adder>>)(() => new Adder { x = 2, Y = 3, z = 4 })).Body;
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e6));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_MemberBinding2()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<NumExpr>
            {
                Leaves =
                {
                    { (ConstantExpression c) => new Val((int)c.Value), c => c.Type == typeof(int), 1 },
                },

                Rules =
                {
                    { (int x, int y, int z) => new MullerAdder { M = { x = x, Y = y }, z = z }, (x, y, z) => new TimesPlus(x, y, z), 2 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 1);
            AssertCount(burs.Rules, 1);
            AssertCount(burs.Fallbacks, 0);

            var e1 = ((Expression<Func<MullerAdder>>)(() => new MullerAdder { M = { x = 2, Y = 3 }, z = 4 })).Body;
            var a1 = burs.Rewrite(e1);
            Assert.AreEqual("TimesPlus(2, 3, 4)", a1.ToString());

            var e2 = ((Expression<Func<MullerAdder>>)(() => new MullerAdder { z = 4, M = { x = 2, Y = 3 } })).Body;
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e2));

            var e3 = ((Expression<Func<MullerAdder>>)(() => new MullerAdder { M = { Y = 3, x = 2 }, z = 4 })).Body;
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e3));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_MemberBinding3()
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<NumExpr>
            {
                Leaves =
                {
                    { (ConstantExpression c) => new Val((int)c.Value), c => c.Type == typeof(int), 1 },
                },

                Rules =
                {
                    { (int x, int y, int z) => new Summer { Xs = { x, y, z } }, (x, y, z) => new Plus(new Plus(x, y), z), 2 },
                },

                Log = log
            };

            AssertCount(burs.Leaves, 1);
            AssertCount(burs.Rules, 1);
            AssertCount(burs.Fallbacks, 0);

            var e1 = ((Expression<Func<Summer>>)(() => new Summer { Xs = { 1, 2, 3 } })).Body;
            var a1 = burs.Rewrite(e1);
            Assert.AreEqual("Plus(Plus(1, 2), 3)", a1.ToString());

            var e2 = ((Expression<Func<Summer>>)(() => new Summer { })).Body;
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e2));

            var e3 = ((Expression<Func<Summer>>)(() => new Summer { Xs = { 1, 2 } })).Body;
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e3));

            var e4 = ((Expression<Func<Summer>>)(() => new Summer { Xs = { 1, 2, 3, 4 } })).Body;
            Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(e4));
        }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable 0649
        private sealed class Adder
        {
            public Adder()
            {
            }

            public Adder(int foo)
            {
            }

            public int x;
            public int Y { get; set; }
            public int z;
        }

        private sealed class Muller
        {
            public int x;
            public int Y { get; set; }
        }

        private sealed class Summer
        {
            public List<int> Xs { get; private set; }
        }

        private sealed class MullerAdder
        {
            public Muller M { get; private set; }
            public int z;
        }
#pragma warning restore 0649
#pragma warning restore IDE0060 // Remove unused parameter

        [TestMethod]
        public void BottomUpExpressionRewriter_Binary()
        {
            var assert = GetAsserter((int x, int y) => x + y);

            assert(true)(Expression.Add(Expression.Constant(1), Expression.Constant(2)));
            assert(false)(Expression.Add(Expression.Constant(1.0), Expression.Constant(2.0)));
            assert(false)(Expression.Multiply(Expression.Constant(1), Expression.Constant(2)));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Binary_Custom()
        {
            var assert = GetAsserter((DateTime dt, TimeSpan ts) => dt + ts);

            assert(true)(Expression.Add(Expression.Constant(DateTime.Now), Expression.Constant(TimeSpan.Zero)));
            assert(false)(Expression.Subtract(Expression.Constant(DateTime.Now), Expression.Constant(TimeSpan.Zero)));
            assert(false)(Expression.Add(Expression.Constant(DateTimeOffset.Now), Expression.Constant(TimeSpan.Zero)));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Conditional()
        {
            var assert = GetAsserter((bool b, int x) => b ? x : 0);

            assert(true)(Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Constant(0)));
            assert(false)(((Expression<Func<int>>)(() => F(true, 1, 0))).Body);
        }

        private static int F(bool x, int y, int z)
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Constant()
        {
            var assert = GetAsserter(() => 1);

            assert(true)(Expression.Constant(1));
            assert(false)(Expression.Constant(2));
            assert(false)(Expression.Constant(1.0));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Invocation()
        {
            var assert = GetAsserter((Func<int, int> f, int x) => f(x));

            assert(true)(Expression.Invoke(Expression.Constant(new Func<int, int>(x => x)), Expression.Constant(42)));
            assert(false)(Expression.Invoke(Expression.Constant(new Func<int, int, int>((x, y) => x)), Expression.Constant(2), Expression.Constant(3)));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Member_Static()
        {
            var assert = GetAsserter(() => DateTime.Now);

            Expression<Func<DateTime>> f = () => DateTime.Now;
            Expression<Func<DateTime>> g = () => DateTime.UtcNow;
            Expression<Func<DateTimeOffset>> h = () => DateTimeOffset.Now;

            assert(true)(f.Body);
            assert(false)(g.Body);
            assert(false)(h.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Member_Instance()
        {
            var assert = GetAsserter((TimeSpan t) => t.Days);

            Expression<Func<int>> f = () => TimeSpan.Zero.Days;
            Expression<Func<int>> g = () => TimeSpan.Zero.Hours;

            assert(true)(f.Body);
            assert(false)(g.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_New()
        {
            var assert = GetAsserter((long t) => new TimeSpan(t));

            Expression<Func<TimeSpan>> f = () => new TimeSpan(1L);
            Expression<Func<TimeSpan>> g = () => new TimeSpan(1, 2, 3);
            Expression<Func<DateTime>> h = () => new DateTime(1, 2, 3);
#pragma warning disable CA1825 // Avoid unnecessary zero-length array allocations. (Used in expression tree)
            Expression<Func<string>> i = () => new string(new char[0]);
#pragma warning restore CA1825

            assert(true)(f.Body);
            assert(false)(g.Body);
            assert(false)(h.Body);
            assert(false)(i.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_NewArray_Bounds()
        {
            var assert = GetAsserter((int x) => new int[x]);

            Expression<Func<int[]>> f = () => new int[2];
            Expression<Func<int[]>> g = () => new int[] { 2 };
            Expression<Func<long[]>> h = () => new long[2];

            assert(true)(f.Body);
            assert(false)(g.Body);
            assert(false)(h.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_NewArray_Init()
        {
            var assert = GetAsserter((int x) => new int[] { x });

            Expression<Func<int[]>> f = () => new int[] { 2 };
            Expression<Func<int[]>> g = () => new int[2];
            Expression<Func<long[]>> h = () => new long[] { 2L };

            assert(true)(f.Body);
            assert(false)(g.Body);
            assert(false)(h.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_TypeBinary()
        {
            var assert = GetAsserter((object o) => o is string);

            assert(true)(Expression.TypeIs(Expression.Constant(value: null, typeof(object)), typeof(string)));
            assert(false)(Expression.TypeIs(Expression.Constant(value: null, typeof(object)), typeof(int)));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Unary()
        {
            var assert1 = GetAsserter((int x) => -x);

            assert1(true)(Expression.Negate(Expression.Constant(1)));
            assert1(false)(Expression.NegateChecked(Expression.Constant(1)));
            assert1(false)(Expression.Add(Expression.Constant(1), Expression.Constant(2)));

            var assert2 = GetAsserter((int x) => checked(-x));

            assert2(true)(Expression.NegateChecked(Expression.Constant(1)));
            assert2(false)(Expression.Negate(Expression.Constant(1)));
            assert2(false)(Expression.Add(Expression.Constant(1), Expression.Constant(2)));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Unary_Custom()
        {
            var assert1 = GetAsserter((Bar b) => !b);

            Expression<Func<Bar, Bar>> f1 = (Bar b) => !b;
            assert1(true)(f1.Body);

            Expression<Func<Bar, Bar>> f2 = (Bar b) => -b;
            assert1(false)(f2.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Unary_Convert()
        {
            var assert = GetAsserter((long x) => (int)x);

            assert(true)(Expression.Convert(Expression.Constant(1L, typeof(long)), typeof(int)));
            assert(false)(Expression.Convert(Expression.Constant(1L, typeof(long)), typeof(double)));
            assert(false)(Expression.Convert(Expression.Constant(1U, typeof(uint)), typeof(int)));
            assert(false)(Expression.ConvertChecked(Expression.Constant(1L, typeof(long)), typeof(int)));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Unary_Convert_Custom()
        {
            var assert1 = GetAsserter<DateTime, DateTimeOffset>((DateTime x) => x);

            Expression<Func<DateTimeOffset>> f1 = () => DateTime.Now;
            assert1(true)(f1.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Constant_WhiteBox()
        {
            var setnec = ShallowExpressionTreeNodeEqualityComparer.Instance;

            var et1 = Expression.Constant(1).ToExpressionTree();
            var et2 = Expression.Constant(1.0).ToExpressionTree();
            var et3 = Expression.Constant(123).ToExpressionTree();
            var et4 = Expression.Constant(1).ToExpressionTree();

            Assert.IsFalse(setnec.Equals(et1.Value, et2.Value));
            Assert.IsFalse(setnec.Equals(et1.Value, et3.Value));
            Assert.IsTrue(setnec.Equals(et1.Value, et4.Value));
            Assert.AreEqual(setnec.GetHashCode(et1.Value), setnec.GetHashCode(et4.Value));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Default_WhiteBox()
        {
            var setnec = ShallowExpressionTreeNodeEqualityComparer.Instance;

            var et1 = Expression.Default(typeof(int)).ToExpressionTree();
            var et2 = Expression.Default(typeof(bool)).ToExpressionTree();
            var et3 = Expression.Default(typeof(int)).ToExpressionTree();

            Assert.IsFalse(setnec.Equals(et1.Value, et2.Value));
            Assert.IsTrue(setnec.Equals(et1.Value, et3.Value));
            Assert.AreEqual(setnec.GetHashCode(et1.Value), setnec.GetHashCode(et3.Value));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Unary_WhiteBox()
        {
            var setnec = ShallowExpressionTreeNodeEqualityComparer.Instance;

            var et1 = Expression.Convert(Expression.Constant(1), typeof(double)).ToExpressionTree();
            var et2 = Expression.Convert(Expression.Constant(1), typeof(float)).ToExpressionTree();
            var et3 = Expression.Convert(Expression.Constant(1), typeof(double)).ToExpressionTree();

            Assert.IsFalse(setnec.Equals(et1.Value, et2.Value));
            Assert.IsTrue(setnec.Equals(et1.Value, et3.Value));
            Assert.AreEqual(setnec.GetHashCode(et1.Value), setnec.GetHashCode(et3.Value));

            var et4 = Expression.Not(Expression.Constant(new Bar())).ToExpressionTree();
            var et5 = Expression.Negate(Expression.Constant(new Bar())).ToExpressionTree();
            var et6 = Expression.Not(Expression.Constant(new Bar())).ToExpressionTree();

            Assert.IsFalse(setnec.Equals(et4.Value, et5.Value));
            Assert.IsTrue(setnec.Equals(et4.Value, et6.Value));
            Assert.AreEqual(setnec.GetHashCode(et4.Value), setnec.GetHashCode(et6.Value));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Comparer_WhiteBox()
        {
            var setnec = ShallowExpressionTreeNodeEqualityComparer.Instance;

            var et = Expression.Constant(2).ToExpressionTree();
            var ei = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(1)).ToExpressionTree();
            Assert.IsFalse(setnec.Equals(et.Value, ei.Value));

            Assert.ThrowsException<NotImplementedException>(() => setnec.Equals(new FakeNode(), new FakeNode()));
            Assert.ThrowsException<NotImplementedException>(() => setnec.GetHashCode(new FakeNode()));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Lambda1()
        {
            var assert = GetAsserter((int x) => I(y => y, x));

            var e1 = (Expression<Func<int>>)(() => I(x => x, 42));
            assert(true)(e1.Body);

            var e2 = (Expression<Func<int>>)(() => I(x => 1, 42));
            assert(false)(e2.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Lambda2()
        {
            var assert = GetAsserter((int x, int y) => K(a => b => a, x, y));

            var e1 = (Expression<Func<int>>)(() => K(x => y => x, 1, 2));
            assert(true)(e1.Body);

            var e2 = (Expression<Func<int>>)(() => K(x => y => y, 1, 2));
            assert(false)(e2.Body);

            var e3 = (Expression<Func<int>>)(() => K(x => y => 0, 1, 2));
            assert(false)(e3.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Lambda3()
        {
            var p = Expression.Parameter(typeof(int), "x");
            var f = Expression.Lambda<Func<Func<int, Func<int, int>>>>(Expression.Lambda(Expression.Lambda(p, p), p));

            var assert = GetAsserter(f);

            var e1 = (Expression<Func<Func<int, Func<int, int>>>>)(() => x => y => y);
            assert(true)(e1.Body);

            var e2 = (Expression<Func<Func<int, Func<int, int>>>>)(() => x => y => x);
            assert(false)(e2.Body);

            assert(true)(f.Body);
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Parameter()
        {
            var p = Expression.Parameter(typeof(int), "global");
            Expression<Func<int>> f = Expression.Lambda<Func<int>>(p);

            var assert = GetAsserter(f);

            assert(true)(p);
            assert(false)(Expression.Parameter(typeof(int), "global"));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_Parameter_WhiteBox()
        {
            var setnec = ShallowExpressionTreeNodeEqualityComparer.Instance;

            var et1 = Expression.Parameter(typeof(int)).ToExpressionTree();
            var et2 = Expression.Parameter(typeof(bool)).ToExpressionTree();
            var et3 = Expression.Parameter(typeof(int)).ToExpressionTree();

            Assert.IsFalse(setnec.Equals(et1.Value, et2.Value));
            Assert.IsFalse(setnec.Equals(et1.Value, et3.Value));
            Assert.IsTrue(setnec.Equals(et1.Value, et1.Value));
            Assert.AreEqual(setnec.GetHashCode(et1.Value), setnec.GetHashCode(et1.Value));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_DeBruijn_WhiteBox1()
        {
            var etcwdb = new ExpressionTreeConversionWithDeBruijn();

            var e1 = (Expression<Func<int, Func<int, int>>>)(x => y => x + y);
            var et1 = etcwdb.Visit(e1);

            var e2 = (Expression<Func<long, Func<long, long>>>)(x => y => x + y);
            var et2 = etcwdb.Visit(e2);

            var f1 = (ExpressionTree<LambdaExpression>)et1;
            var g1 = (ExpressionTree<LambdaExpression>)f1.Children[0];
            var h1 = g1.Children[0];
            var px = f1.Children[1];
            var py = g1.Children[1];

            var f2 = (ExpressionTree<LambdaExpression>)et2;
            var g2 = (ExpressionTree<LambdaExpression>)f2.Children[0];
            var h2 = g2.Children[0];
            var pa = f2.Children[1];
            var pb = g2.Children[1];

            var pxd = px as ParameterDeclaration;
            Assert.IsNotNull(pxd);

            var pyd = py as ParameterDeclaration;
            Assert.IsNotNull(pyd);

            var pad = pa as ParameterDeclaration;
            Assert.IsNotNull(pad);

            var pbd = pb as ParameterDeclaration;
            Assert.IsNotNull(pbd);

            Assert.IsTrue(pxd.Equals(pyd)); // same type suffices
            Assert.IsFalse(pxd.Equals(pad));
            Assert.IsFalse(pxd.Equals(42));

            var pxr = h1.Children[0] as DeBruijnParameter;
            Assert.IsNotNull(pxr);

            var pyr = h1.Children[1] as DeBruijnParameter;
            Assert.IsNotNull(pyr);

            var par = h2.Children[0] as DeBruijnParameter;
            Assert.IsNotNull(par);

            var pbr = h2.Children[1] as DeBruijnParameter;
            Assert.IsNotNull(pbr);

            Assert.IsFalse(pxd.Equals(pxr));
            Assert.IsFalse(pxr.Equals(pyr));
            Assert.IsFalse(pxr.Equals(par));
            Assert.IsFalse(pxr.Equals(42));

            var cet = Expression.Constant(42).ToExpressionTree();

            Assert.IsFalse(pxd.Equals(cet));
            Assert.IsFalse(pxr.Equals(cet));
        }

        [TestMethod]
        public void BottomUpExpressionRewriter_DeBruijn_WhiteBox2()
        {
            var p1 = new DeBruijnParameter(Expression.Parameter(typeof(int)), 0, 0);
            var p2 = new DeBruijnParameter(Expression.Parameter(typeof(int)), 0, 1);
            var p3 = new DeBruijnParameter(Expression.Parameter(typeof(int)), 1, 0);
            var p4 = new DeBruijnParameter(Expression.Parameter(typeof(long)), 0, 0);

            Assert.IsFalse(p1.Equals(p2));
            Assert.IsFalse(p1.Equals(p3));
            Assert.IsFalse(p1.Equals(p4));
            Assert.IsFalse(p2.Equals(p3));
            Assert.IsFalse(p2.Equals(p4));
            Assert.IsFalse(p3.Equals(p4));

            var p5 = new DeBruijnParameter(Expression.Parameter(typeof(int)), 0, 0);
            Assert.AreEqual(p1.GetHashCode(), p5.GetHashCode());
            Assert.IsTrue(p1.Equals(p5));
        }

        private static int I(Func<int, int> f, int x)
        {
            throw new NotImplementedException();
        }

        private static int K(Func<int, Func<int, int>> f, int x, int y)
        {
            throw new NotImplementedException();
        }

        private sealed class FakeNode : ExpressionTreeNode
        {
            public FakeNode()
                : base((ExpressionTreeNodeType)12345)
            {
            }

            protected override bool EqualsCore(ExpressionTreeNode other)
            {
                throw new NotImplementedException();
            }

            public override bool Equals(object obj)
            {
                throw new NotImplementedException();
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Bar
        {
            public static Bar operator !(Bar _) => throw new NotImplementedException();
            public static Bar operator -(Bar _) => throw new NotImplementedException();
        }

        private static Func<bool, Action<Expression>> GetAsserter<R>(Expression<Func<R>> rule)
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<NumExpr>
            {
                Rules =
                {
                    { rule, () => Val.One, 1 },
                },

                Log = log
            };

            return succeed => subject =>
            {
                if (succeed)
                {
                    var res = burs.Rewrite(subject);
                    Assert.AreEqual("1", res.ToString());
                }
                else
                {
                    Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(subject));
                }
            };
        }

        private static Func<bool, Action<Expression>> GetAsserter<T, R>(Expression<Func<T, R>> rule)
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<NumExpr>
            {
                Leaves =
                {
                    { (Expression e) => Val.Zero, 1 },
                },

                Rules =
                {
                    { rule, x => Val.One, 1 },
                },

                Log = log
            };

            return succeed => subject =>
            {
                if (succeed)
                {
                    var res = burs.Rewrite(subject);
                    Assert.AreEqual("1", res.ToString());
                }
                else
                {
                    Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(subject));
                }
            };
        }

        private static Func<bool, Action<Expression>> GetAsserter<T1, T2, R>(Expression<Func<T1, T2, R>> rule)
        {
            var log = new StringWriter();

            var burs = new BottomUpExpressionRewriter<NumExpr>
            {
                Leaves =
                {
                    { (Expression e) => Val.Zero, 1 },
                },

                Rules =
                {
                    { rule, (x, y) => Val.One, 1 },
                },

                Log = log
            };

            return succeed => subject =>
            {
                if (succeed)
                {
                    var res = burs.Rewrite(subject);
                    Assert.AreEqual("1", res.ToString());
                }
                else
                {
                    Assert.ThrowsException<InvalidOperationException>(() => burs.Rewrite(subject));
                }
            };
        }

        private static void AssertCount<T>(IEnumerable<T> e, int n)
        {
            var e1 = e.GetEnumerator();
            var e2 = ((IEnumerable)e).GetEnumerator();

            var n1 = 0;
            while (e1.MoveNext())
                n1++;

            var n2 = 0;
            while (e2.MoveNext())
                n2++;

            Assert.AreEqual(n, n1);
            Assert.AreEqual(n, n2);
        }
    }
}
