// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Optimizers;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices.Optimizers
{
    [TestClass]
    public class OptimizerTests
    {
#pragma warning disable CA1825 // Avoid unnecessary zero-length array allocations. (Used in expression trees)
        [TestMethod]
        public void Optimizer_FirstCoalescing()
        {
            var cmp = new ExpressionEqualityComparer();

            {
                var e = Infer(() => new int[0].Where(x => true).First()).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                    Infer(() => new int[0].First(x => true)).Body,
                    o));
            }
        }

        [TestMethod]
        public void Optimizer_SelectCoalescing()
        {
            var cmp = new ExpressionEqualityComparer();

            {
                var e = Infer(() => new int[0].Select(i => i.ToString()).Select(s => int.Parse(s))).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.AreEqual(ExpressionType.Call, o.NodeType);
                var select = (MethodCallExpression)o;
                Assert.AreEqual(ReflectionHelpers.InfoOf((IEnumerable<int> enumerable) => enumerable.Select(default(Func<int, int>))), select.Method);

                var selector = (Expression<Func<int, int>>)select.Arguments[1];

                var x = Expression.Parameter(typeof(int));
                var y = Expression.Parameter(typeof(string));
                var expectedCoalesced =
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Lambda(
                                Expression.Call(
                                    null,
                                    (MethodInfo)ReflectionHelpers.InfoOf((string s) => int.Parse(s)),
                                    y
                                ),
                                y
                            ),
                            Expression.Call(
                                x,
                                (MethodInfo)ReflectionHelpers.InfoOf((int i) => i.ToString())
                            )
                        ),
                        x
                    );

                Assert.IsTrue(cmp.Equals(
                    expectedCoalesced,
                    selector));
            }

            {
                var e = Infer(() => new int[0].Select(i => i.ToString()).Select(i => int.Parse(i))).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                var r =
                    BetaReducer.Reduce(
                        Expression.Invoke(
                            Infer((Func<string, int> f) => new int[0].Select(i => f(i.ToString()))),
                            Infer((string i) => int.Parse(i))
                        ),
                        BetaReductionNodeTypes.Unrestricted,
                        BetaReductionRestrictions.None
                    );

                Assert.IsTrue(cmp.Equals(
                    r,
                    o));
            }

            {
                var e = Infer(() => new int[0].Select(new[] { (Func<int, string>)(i => i.ToString()) }.First()).Select(i => int.Parse(i))).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                var r =
                    BetaReducer.Reduce(
                        Expression.Invoke(
                            Infer((Func<string, int> g, Func<int, string> f) => new int[0].Select(i => g(f(i)))),
                            Infer((string i) => int.Parse(i)),
                            Infer(() => new[] { (Func<int, string>)(i => i.ToString()) }.First()).Body
                        ),
                        BetaReductionNodeTypes.Unrestricted,
                        BetaReductionRestrictions.None
                    );

                Assert.IsTrue(cmp.Equals(
                    r,
                    o));
            }

            {
                var e = Infer(() => new int[0].Select(i => i.ToString()).Select(new[] { (Func<string, int>)(i => int.Parse(i)) }.First())).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                var r =
                    BetaReducer.ReduceEager(
                        Expression.Invoke(
                            Infer((Func<string, int> g, Func<int, string> f) => new int[0].Select(i => g(f(i)))),
                            Infer(() => new[] { (Func<string, int>)(i => int.Parse(i)) }.First()).Body,
                            Infer((int i) => i.ToString())
                        ),
                        BetaReductionNodeTypes.Unrestricted,
                        BetaReductionRestrictions.None,
                        false
                    );

                Assert.IsTrue(cmp.Equals(
                    r,
                    o));
            }
        }

        [TestMethod]
        public void Optimizer_WhereCoalescing()
        {
            var cmp = new ExpressionEqualityComparer();

            {
                var e = Infer(() => new int[0].Where(x => x >= 0).Where(x => x <= 0)).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                    Infer(() => new int[0].Where(x => x >= 0 && x <= 0)).Body,
                    o));
            }

            {
                Expression<Func<Func<int, bool>, Func<int, bool>>> id = _ => _;
                var e =
                    BetaReducer.Reduce(
                        Expression.Invoke(
                            Infer((Func<Func<int, bool>, Func<int, bool>> f) => new int[0].Where(f(x => x >= 0)).Where(x => x <= 0)),
                            id
                        ),
                        BetaReductionNodeTypes.Unrestricted,
                        BetaReductionRestrictions.None
                    );
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                    BetaReducer.Reduce(
                        Expression.Invoke(
                            Infer((Func<Func<int, bool>, Func<int, bool>> f) => new int[0].Where(y => f(x => x >= 0)(y) && y <= 0)),
                            id
                        ),
                        BetaReductionNodeTypes.Unrestricted,
                        BetaReductionRestrictions.None
                    ),
                    o));
            }

            {
                var e = Infer(() => new int[0].Where(Enumerable.Range(0, 1).Select(_ => (Func<int, bool>)(x => x >= 0)).Single()).Where(x => x <= 0)).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                    Infer(() => new int[0].Where(x => Enumerable.Range(0, 1).Select(_ => (Func<int, bool>)(y => y >= 0)).Single()(x) && x <= 0)).Body,
                    o));
            }

            {
                var e = Infer(() => new int[0].Where(x => x >= 0).Where(Enumerable.Range(0, 1).Select(_ => (Func<int, bool>)(x => x <= 0)).Single())).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                   Infer(() => new int[0].Where(x => x >= 0 && Enumerable.Range(0, 1).Select(_ => (Func<int, bool>)(y => y <= 0)).Single()(x))).Body,
                   o));
            }
        }

        [TestMethod]
        public void Optimizer_TakeCoalescing()
        {
            var cmp = new ExpressionEqualityComparer();

            {
                var e = Infer(() => new int[0].Take(100).Take(10)).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                    Infer(() => new int[0].Take(10)).Body,
                    o));
            }

            {
                var e = Infer(() => new int[0].Take(10).Take(100)).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                    Infer(() => new int[0].Take(10)).Body,
                    o));
            }

            {
                var e = Infer(() => new int[0].Take(10).Take(Math.Abs(-100))).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                    Infer(() => new int[0].Take(Math.Min(10, Math.Abs(-100)))).Body,
                    o));
            }

            {
                var e = Infer(() => new int[0].Take(Math.Abs(-100)).Take(10)).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                    Infer(() => new int[0].Take(Math.Min(Math.Abs(-100), 10))).Body,
                    o));
            }

            {
                var e = Infer(() => new int[0].Take(Math.Abs(-1)).Take(Math.Abs(-100))).Body;
                var q = new EnumerableToQueryTreeConverter().Convert(e);
                var c = new CoalescingOptimizer().Optimize(q);
                var o = c.Reduce();

                Assert.IsTrue(cmp.Equals(
                    Infer(() => new int[0].Take(Math.Min(Math.Abs(-1), Math.Abs(-100)))).Body,
                    o));
            }
        }

        [TestMethod]
        public void Optimizer_NoOptimization()
        {
            var cmp = new ExpressionEqualityComparer();
            var e = Infer(() => new int[0].Where(x => x >= 0).Select(_ => _).Take(100).First()).Body;
            var q = new EnumerableToQueryTreeConverter().Convert(e);
            var c = new CoalescingOptimizer().Optimize(q);
            var o = c.Reduce();

            Assert.AreSame(q, c);
            Assert.IsTrue(cmp.Equals(e, o));
        }

        public static Expression<Func<T>> Infer<T>(Expression<Func<T>> expr) => expr;

        public static Expression<Func<T, R>> Infer<T, R>(Expression<Func<T, R>> expr) => expr;

        public static Expression<Func<T1, T2, R>> Infer<T1, T2, R>(Expression<Func<T1, T2, R>> expr) => expr;
#pragma warning restore CA1825
    }
}
