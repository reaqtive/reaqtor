// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Linq.Expressions
{
    [TestClass]
    public class ExpressionToExpressionSlimConverterTests : TestBase
    {
        [TestMethod]
        public void ExpressionToExpressionSlimConverter_NullArgument()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new ExpressionToExpressionSlimConverter(typeSpace: null), ex => Assert.AreEqual("typeSpace", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new ExpressionToExpressionSlimConverter(new TypeSpace(), factory: null), ex => Assert.AreEqual("factory", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionToExpressionSlimConverter_BaseClass()
        {
            new MyConverter().TestBase();
        }

        [TestMethod]
        public void ExpressionToExpressionSlimConverter_Binary()
        {
            var c1 = Expression.Constant(1);
            var c2 = Expression.Constant(2);
            var p3 = Expression.Parameter(typeof(int));

            var d1 = Expression.Constant(1.0);
            var d2 = Expression.Constant(2.0);
            var d3 = Expression.Parameter(typeof(double));

            var b1 = Expression.Constant(false);
            var b2 = Expression.Constant(true);

            var s1 = Expression.Constant("bar");
            var s2 = Expression.Constant("foo");

            var B1 = Expression.Constant(new BigInteger(42));
            var B2 = Expression.Constant(new BigInteger(43));

            foreach (var b in new[]
            {
                Expression.Add(c1, c2),
                Expression.AddChecked(c1, c2),
                Expression.Divide(c1, c2),
                Expression.Modulo(c1, c2),
                Expression.Multiply(c1, c2),
                Expression.MultiplyChecked(c1, c2),
                Expression.Power(d1, d2),
                Expression.Subtract(c1, c2),
                Expression.SubtractChecked(c1, c2),

                Expression.Add(B1, B2),
                Expression.AddChecked(B1, B2),
                Expression.Divide(B1, B2),
                Expression.Modulo(B1, B2),
                Expression.Multiply(B1, B2),
                Expression.MultiplyChecked(B1, B2),
                Expression.Subtract(B1, B2),
                Expression.SubtractChecked(B1, B2),

                Expression.And(c1, c2),
                Expression.ExclusiveOr(c1, c2),
                Expression.Or(c1, c2),

                Expression.And(B1, B2),
                Expression.ExclusiveOr(B1, B2),
                Expression.Or(B1, B2),

                Expression.AndAlso(b1, b2),
                Expression.OrElse(b1, b2),

                Expression.LeftShift(c1, c2),
                Expression.RightShift(c1, c2),

                Expression.LeftShift(B1, c2),
                Expression.RightShift(B1, c2),

                Expression.GreaterThan(c1, c2),
                Expression.GreaterThanOrEqual(c1, c2),
                Expression.LessThan(c1, c2),
                Expression.LessThanOrEqual(c1, c2),

                Expression.GreaterThan(B1, B2),
                Expression.GreaterThanOrEqual(B1, B2),
                Expression.LessThan(B1, B2),
                Expression.LessThanOrEqual(B1, B2),

                Expression.Equal(c1, c2),
                Expression.NotEqual(c1, c2),

                Expression.Equal(B1, B2),
                Expression.NotEqual(B1, B2),

                Expression.Equal(s1, s2),
                Expression.NotEqual(s1, s2),

                Expression.ReferenceEqual(s1, s2),
                Expression.ReferenceNotEqual(s1, s2),

                Expression.AddAssign(p3, c2),
                Expression.AddAssignChecked(p3, c2),
                Expression.DivideAssign(p3, c2),
                Expression.ModuloAssign(p3, c2),
                Expression.MultiplyAssign(p3, c2),
                Expression.MultiplyAssignChecked(p3, c2),
                Expression.PowerAssign(d3, d2),
                Expression.SubtractAssign(p3, c2),
                Expression.SubtractAssignChecked(p3, c2),

                Expression.AndAssign(p3, c2),
                Expression.ExclusiveOrAssign(p3, c2),
                Expression.OrAssign(p3, c2),

                Expression.LeftShiftAssign(p3, c2),
                Expression.RightShiftAssign(p3, c2),

                Expression.Assign(p3, c2),
            })
            {
                AssertRoundtrip(b);
            }
        }

        [TestMethod]
        public void ExpressionToExpressionSlimConverter_Unary()
        {
            var c1 = Expression.Constant(1);
            var p3 = Expression.Parameter(typeof(int));

            var b1 = Expression.Constant(false);

            var s1 = Expression.Constant("bar");

            var B1 = Expression.Constant(new BigInteger(42));

            var er = Expression.Constant(new Exception());

            var o1 = Expression.Constant(42, typeof(object));

            var arr = Expression.Constant(new[] { 1, 2, 3 });

            foreach (var b in new[]
            {
                Expression.Negate(c1),
                Expression.NegateChecked(c1),
                Expression.OnesComplement(c1),
                Expression.UnaryPlus(c1),

                Expression.Negate(B1),
                Expression.NegateChecked(B1),
                Expression.OnesComplement(B1),
                Expression.UnaryPlus(B1),

                Expression.Increment(c1),
                Expression.Decrement(c1),

                Expression.Increment(B1),
                Expression.Decrement(B1),

                Expression.Not(b1),

                Expression.ArrayLength(arr),

                Expression.Convert(c1, typeof(long)),
                Expression.ConvertChecked(c1, typeof(long)),
                Expression.TypeAs(s1, typeof(string)),

                Expression.PostDecrementAssign(p3),
                Expression.PostIncrementAssign(p3),
                Expression.PreDecrementAssign(p3),
                Expression.PreIncrementAssign(p3),

                Expression.Rethrow(),
                Expression.Rethrow(typeof(Exception)),

                Expression.Throw(er),
                Expression.Throw(er, typeof(Exception)),

                Expression.Unbox(o1, typeof(int)),
            })
            {
                AssertRoundtrip(b);
            }
        }

        [TestMethod]
        public void ExpressionToExpressionSlimConverter_Loop()
        {
            var counter = Expression.Variable(typeof(int), "counter");
            var @break = Expression.Label(typeof(int));

            var loop =
                Expression.Loop(
                    Expression.Block(
                        typeof(int),
                        new[] { counter },
                        Expression.AddAssign(counter, Expression.Constant(2)),
                        Expression.Condition(
                            Expression.GreaterThanOrEqual(
                                counter,
                                Expression.Constant(1337)
                            ),
                            Expression.Break(@break, counter, typeof(int)),
                            Expression.Constant(42)
                        )
                    ),
                    @break
                );

            AssertRoundtrip(loop);
        }

        [TestMethod]
        public void ExpressionToExpressionSlimConverter_Block()
        {
            var p = Expression.Variable(typeof(int));
            var l =
                Expression.Lambda<Func<int, int>>(
                    Expression.Block(
                        new[] { p },
                        Expression.Assign(p, Expression.Constant(1))
                    ),
                    p
                );

            AssertRoundtrip(l);

            var e =
                Expression.Block(
                    new[] { p },
                    Expression.Block(
                        new[] { p },
                        p
                    )
                );

            AssertRoundtrip(e);

            var t = Expression.Block(typeof(void), Expression.Constant(1));
            AssertRoundtrip(t);
        }

        private static void AssertRoundtrip(Expression e)
        {
            var toSlim = new ExpressionToExpressionSlimConverter();
            var toLinq = new ExpressionSlimToExpressionConverter();
            var eqComp = new ExpressionEqualityComparer(() => new Comparator());

            var s = toSlim.Visit(e);
            var r = toLinq.Visit(s);

            Assert.IsTrue(eqComp.Equals(e, r), e.ToString());
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }

            protected override int GetHashCodeGlobalParameter(ParameterExpression obj) => 0;
        }

        private sealed class MyConverter : ExpressionToExpressionSlimConverter
        {
            public void TestBase()
            {
                Assert.IsNotNull(base.TypeSpace);
            }
        }
    }
}
