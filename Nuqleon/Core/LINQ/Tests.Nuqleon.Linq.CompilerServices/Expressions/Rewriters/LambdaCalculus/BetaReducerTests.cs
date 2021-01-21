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
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class BetaReducerTests
    {
        [TestMethod]
        public void BetaReducer_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => BetaReducer.Reduce(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => BetaReducer.Reduce(expression: null, BetaReductionNodeTypes.Atoms, BetaReductionRestrictions.None), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => BetaReducer.ReduceEager(expression: null, BetaReductionNodeTypes.Atoms, BetaReductionRestrictions.None, throwOnCycle: false), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void BetaReducer_Defaults_None1()
        {
            var e = Expression.Constant(42);
            var r = BetaReducer.Reduce(e);

            Assert.AreSame(e, r);
        }

        [TestMethod]
        public void BetaReducer_Defaults_None2()
        {
            var e = Expression.Invoke(Expression.Constant(new Func<int, int>(x => x)), Expression.Constant(42));
            var r = BetaReducer.Reduce(e);

            Assert.AreSame(e, r);
        }

        [TestMethod]
        public void BetaReducer_Defaults_Constant_Identity()
        {
            var e = Expression.Constant(42);
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(x, x), e);
            var r = BetaReducer.Reduce(f);

            Assert.AreSame(e, r);
        }

        [TestMethod]
        public void BetaReducer_Defaults_Constant_MultipleUses()
        {
            var e = Expression.Constant(42);
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Add(x, x), x), e);
            var r = BetaReducer.Reduce(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Add(e, e), r));
        }

        [TestMethod]
        public void BetaReducer_Defaults_Constant_Nested()
        {
            var e = Expression.Constant(42);
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Lambda(x, y), x), e);
            var r = BetaReducer.Reduce(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Lambda(e, y), r));
        }

        [TestMethod]
        public void BetaReducer_Defaults_Constant_Nested_Complex()
        {
            var e = Expression.Constant(42);
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Lambda(x, x), x), e);
            var r = BetaReducer.Reduce(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Lambda(x, x), r));
        }

        [TestMethod]
        public void BetaReducer_Defaults_Default_Identity()
        {
            var e = Expression.Default(typeof(int));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(x, x), e);
            var r = BetaReducer.Reduce(f);

            Assert.AreSame(e, r);
        }

        [TestMethod]
        public void BetaReducer_Defaults_Default_MultipleUses()
        {
            var e = Expression.Default(typeof(int));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Add(x, x), x), e);
            var r = BetaReducer.Reduce(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Add(e, e), r));
        }

        [TestMethod]
        public void BetaReducer_Defaults_Default_Nested()
        {
            var e = Expression.Default(typeof(int));
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Lambda(x, y), x), e);
            var r = BetaReducer.Reduce(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Lambda(e, y), r));
        }

        [TestMethod]
        public void BetaReducer_Defaults_Parameter_Identity()
        {
            var e = Expression.Parameter(typeof(int));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(x, x), e);
            var r = BetaReducer.Reduce(f);

            Assert.AreSame(e, r);
        }

        [TestMethod]
        public void BetaReducer_Defaults_Parameter_MultipleUses()
        {
            var e = Expression.Parameter(typeof(int));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Add(x, x), x), e);
            var r = BetaReducer.Reduce(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Add(e, e), r));
        }

        [TestMethod]
        public void BetaReducer_Defaults_Parameter_Nested()
        {
            var e = Expression.Parameter(typeof(int));
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Lambda(x, y), x), e);
            var r = BetaReducer.Reduce(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Lambda(e, y), r));
        }

        [TestMethod]
        public void BetaReducer_Defaults_Quote_Identity()
        {
            var e = Expression.Quote(Expression.Lambda(Expression.Constant("foo")));
            var x = Expression.Parameter(typeof(Expression));
            var f = Expression.Invoke(Expression.Lambda(x, x), e);
            var r = BetaReducer.Reduce(f);

            Assert.AreSame(e, r);
        }

        [TestMethod]
        public void BetaReducer_Defaults_Quote_Nested()
        {
            var e = Expression.Quote(Expression.Lambda(Expression.Constant("foo")));
            var x = Expression.Parameter(typeof(Expression));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Lambda(x, y), x), e);
            var r = BetaReducer.Reduce(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Lambda<Func<int, Expression>>(e, y), r));
        }

        [TestMethod]
        public void BetaReducer_Defaults_NonAtom()
        {
            var e = Expression.Negate(Expression.Constant(42));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(x, x), e);
            var r = BetaReducer.Reduce(f);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(f, r));
        }

        [TestMethod]
        public void BetaReducer_Advanced_AllNodes()
        {
            var e = Expression.Negate(Expression.Constant(42));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(x, x), e);
            var r = BetaReducer.Reduce(f, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(e, r));
        }

        [TestMethod]
        public void BetaReducer_Advanced_AllNodes_Discard()
        {
            var e = Expression.Negate(Expression.Constant(42));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Constant(1), x), e);
            var r = BetaReducer.Reduce(f, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Constant(1), r));
        }

        [TestMethod]
        public void BetaReducer_Advanced_AllNodes_NoDiscard()
        {
            var e = Expression.Negate(Expression.Constant(42));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Constant(1), x), e);

            Assert.ThrowsException<InvalidOperationException>(() => BetaReducer.Reduce(f, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.DisallowDiscard));
        }

        [TestMethod]
        public void BetaReducer_Advanced_AllNodes_MoreThanOnce()
        {
            var e = Expression.Negate(Expression.Constant(42));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Add(x, x), x), e);
            var r = BetaReducer.Reduce(f, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(Expression.Add(e, e), r));
        }

        [TestMethod]
        public void BetaReducer_Advanced_AllNodes_ExactlyOnce()
        {
            var e = Expression.Negate(Expression.Constant(42));
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Invoke(Expression.Lambda(Expression.Add(x, x), x), e);

            Assert.ThrowsException<InvalidOperationException>(() => BetaReducer.Reduce(f, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.ExactlyOnce));
        }

        [TestMethod]
        public void BetaReducer_MathTest()
        {
            var f = (Expression<Func<int, int, int>>)((x, y) => x * y + Enumerable.Range(0, x).Select(a => a + y).Sum());

            var g = f.Compile();

            foreach (var (x, y) in new[]
            {
                ( x : 0, y : 0 ),
                ( x : 3, y : 4 ),
                ( x : 7, y : 3 ),
                ( x : 4, y : 9 ),
            })
            {
                var red = BetaReducer.Reduce(Expression.Invoke(f, Expression.Constant(x), Expression.Constant(y)));
                Assert.AreEqual(ExpressionType.Add, red.NodeType);

                var res1 = g(x, y);
                var res2 = Expression.Lambda<Func<int>>(red).Compile()();

                Assert.AreEqual(res1, res2);
            }
        }

        [TestMethod]
        public void BetaReducer_AvoidCapture()
        {
            var f = (Expression<Func<int, Func<int, int>>>)(y => x => x + y);

            var px = ((LambdaExpression)f.Body).Parameters[0];

            var g = Expression.Lambda<Func<int, Func<int, int>>>(Expression.Invoke(f, px), px);
            var h = (Expression<Func<int, Func<int, int>>>)BetaReducer.Reduce(g);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(g, h));

            var a = g.Compile()(2)(3);
            var b = h.Compile()(2)(3);
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void BetaReducer_Eager1()
        {
            var x = Expression.Parameter(typeof(int), "x");
            var y = Expression.Parameter(typeof(int), "y");

            var f = Expression.Lambda<Func<int, int>>(Expression.Add(x, y), y);
            var g = Expression.Invoke(f, Expression.Constant(1));
            var h = Expression.Lambda<Func<int, int>>(g, x);
            var i = Expression.Invoke(h, Expression.Constant(2));

            var r = BetaReducer.ReduceEager(i, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None, throwOnCycle: false);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(r, Expression.Add(Expression.Constant(2), Expression.Constant(1))));
        }

        private delegate D D(D d);

        [TestMethod]
        public void BetaReducer_Eager2()
        {
            var a = (Expression<D>)((D d) => d(d));
            var b = Expression.Invoke(a, a);

            var r = BetaReducer.ReduceEager(b, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None, throwOnCycle: false);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(r, b));
        }

        [TestMethod]
        public void BetaReducer_Eager3()
        {
            var a = (Expression<D>)((D d) => d(d));
            var b = Expression.Invoke(a, a);

            Assert.ThrowsException<InvalidOperationException>(() => BetaReducer.ReduceEager(b, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None, throwOnCycle: true));
        }

        [TestMethod]
        public void BetaReducer_Regression_AlphaEquivalence()
        {
            //
            // This test catches regressions for a bug where the same parameter was used in many nested scoped
            // for Invoke(Lambda(body, parameters), arguments) constructs. When any of the parameters ended up
            // being excluded (due to the argument being non-inlinable), they did not end up in the scope table
            // causing an earlier entry (from a higher reducible Invoke/Lambda pair) to be picked up instead,
            // thus violating the scoping rules.
            //

            var f = (Func<int, int, int>)((i, j) => i + j);
            var g = (Func<int, int>)(i => i + 1);

            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Lambda(
                                Expression.Invoke(
                                    Expression.Constant(f),
                                    x, // the bug caused Expression.Constant(1) to be inlined here
                                    x  // and here, because the scope below was not tracked
                                ),
                                x // this scope entry got lost in the environment, so the previous binding of 'x' was taken
                            ),
                            Expression.Invoke(Expression.Constant(g), x) // this cannot be reduced (atoms only configuration)
                        ),
                        x // this scope entry was preserved, with a reducible binding to Expression.Constant(1)
                    ),
                    Expression.Constant(1) // but this can
                );

            var r = BetaReducer.Reduce(e);

            var rc = Expression.Lambda<Func<int>>(r).Compile();
            var ec = Expression.Lambda<Func<int>>(e).Compile();

            var res1 = rc();
            var res2 = ec();

            Assert.AreEqual(res1, res2);
        }
    }
}
