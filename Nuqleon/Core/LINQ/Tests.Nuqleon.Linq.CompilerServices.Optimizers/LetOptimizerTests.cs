// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Optimizers;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.CompilerServices.Optimizers
{
    [TestClass]
    public partial class LetOptimizerTests
    {
        [TestMethod]
        public void LetCoalescer_Simple()
        {
            {
                var e =
                    Infer(() =>
                        from i in Enumerable.Range(0, 1)
                        let x = 42
                        let y = i
                        let z = 100
                        let a = x
                        let b = i.ToString()
                        let c = 2
                        let d = i + 1
                        let f = i * 3
                        select i
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(0, r.Funcletize<IEnumerable<int>>().Compile()().First());
            }

            {
                var e =
                    Infer(() =>
                        from i in Enumerable.Range(0, 1)
                        let x = 42
                        let y = i
                        let z = 100
                        let a = i
                        let b = i.ToString()
                        let c = 2
                        let d = "b = " + b
                        let f = i * 3
                        select i
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(3, counter.Count); // two for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<int>>().Compile()().First(), r.Funcletize<IEnumerable<int>>().Compile()().First());
            }

            {
                var e =
                    Infer(() =>
                        from i in Enumerable.Range(0, 1)
                        let x = 42
                        let y = i
                        let z = 100
                        let a = x
                        let b = i.ToString()
                        let c = 2
                        let d = i + 1
                        let f = i - 3
                        select string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", i, x, y, z, a, b, c, d, f)
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() =>
                        from p in Enumerable.Repeat(new Person { Name = "Bob", Age = 25 }, 1)
                        let n = p.Name
                        let a = p.Age
                        select n + " is " + a
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() =>
                        from p in Enumerable.Repeat(new Person { Name = "Alice", Age = 30 }, 1)
                        let n = p.Name
                        let s = n.Length
                        select n + ".Length = " + s
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(3, counter.Count); // The let bindings can't be coalesced since the second one refers to the first
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }
        }

        [TestMethod]
        public void LetCoalescer_VariableMasking()
        {
            var select = ((MethodInfo)ReflectionHelpers.InfoOf((IEnumerable<int> xs) => xs.Select(x => x))).GetGenericMethodDefinition();

            {
                //Enumerable
                //    .Range(0, 1)
                //    .Select(x => new { i = 1 })
                //    .Select(y => new { A = y, b = (('a y) => y.i)(new { i = 2 }) })

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

                var a = new { i = 1 };
                var b = new { A = a, b = 1 };
                var t = a.GetType();
                var u = b.GetType();

#pragma warning restore IDE0050

                var s = Infer(() => Enumerable.Range(0, 1)).Body;

                var x = Expression.Parameter(typeof(int));
                var y = Expression.Parameter(t);
                var e =
                    Expression.Call(
                        select.MakeGenericMethod(new[] { t, u }),
                        Expression.Call(
                            select.MakeGenericMethod(new[] { typeof(int), t }),
                            s,
                            Expression.Lambda(
                                Expression.New(
                                    t.GetConstructors().Single(),
                                    new[] { Expression.Constant(1) },
                                    t.GetMember("i").Single()
                                ),
                                x
                            )
                        ),
                        Expression.Lambda(
                            Expression.New(
                                u.GetConstructors().Single(),
                                new Expression[]
                                {
                                    y,
                                    Expression.Invoke(
                                        Expression.Lambda(Expression.MakeMemberAccess(y, t.GetMember("i").Single()), y),
                                        Expression.New(
                                            t.GetConstructors().Single(),
                                            new[] { Expression.Constant(2) },
                                            t.GetMember("i").Single()
                                        )
                                    )
                                },
                                u.GetMember("A").Single(),
                                u.GetMember("b").Single()
                            ),
                            y
                        )
                    );

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(1, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<object>>().Compile()().First(), r.Funcletize<IEnumerable<object>>().Compile()().First());
            }

            {
                //Enumerable
                //    .Range(0, 1)
                //    .Select(x => new { i = 1 })
                //    .Select(y => new { A = y, b = (('a z) => z.i)(new { i = 2 }) })

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

                var a = new { i = 1 };
                var b = new { A = a, b = 1 };
                var t = a.GetType();
                var u = b.GetType();

#pragma warning restore IDE0050

                var s = Infer(() => Enumerable.Range(0, 1)).Body;

                var x = Expression.Parameter(typeof(int));
                var y = Expression.Parameter(t);
                var z = Expression.Parameter(t);
                var e =
                    Expression.Call(
                        select.MakeGenericMethod(new[] { t, u }),
                        Expression.Call(
                            select.MakeGenericMethod(new[] { typeof(int), t }),
                            s,
                            Expression.Lambda(
                                Expression.New(
                                    t.GetConstructors().Single(),
                                    new[] { Expression.Constant(1) },
                                    t.GetMember("i").Single()
                                ),
                                x
                            )
                        ),
                        Expression.Lambda(
                            Expression.New(
                                u.GetConstructors().Single(),
                                new Expression[]
                                {
                                    y,
                                    Expression.Invoke(
                                        Expression.Lambda(Expression.MakeMemberAccess(z, t.GetMember("i").Single()), z),
                                        Expression.New(
                                            t.GetConstructors().Single(),
                                            new[] { Expression.Constant(2) },
                                            t.GetMember("i").Single()
                                        )
                                    )
                                },
                                u.GetMember("A").Single(),
                                u.GetMember("b").Single()
                            ),
                            y
                        )
                    );

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(1, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<object>>().Compile()().First(), r.Funcletize<IEnumerable<object>>().Compile()().First());
            }

            {
                //Enumerable
                //    .Range(0, 1)
                //    .Select(x => new { i = 1 })
                //    .Select(y => (('a y) => y.i)(new { i = y.i + 1 }))
                var a = new { i = 1 };
                var t = a.GetType();

                var s = Infer(() => Enumerable.Range(0, 1)).Body;

                var x = Expression.Parameter(typeof(int));
                var y = Expression.Parameter(t);
                var e =
                    Expression.Call(
                        select.MakeGenericMethod(new[] { t, typeof(int) }),
                        Expression.Call(
                            select.MakeGenericMethod(new[] { typeof(int), t }),
                            s,
                            Expression.Lambda(
                                Expression.New(
                                    t.GetConstructors().Single(),
                                    new[] { Expression.Constant(1) },
                                    t.GetMember("i").Single()
                                ),
                                x
                            )
                        ),
                        Expression.Lambda(
                            Expression.Invoke(
                                Expression.Lambda(Expression.MakeMemberAccess(y, t.GetMember("i").Single()), y),
                                Expression.New(
                                    t.GetConstructors().Single(),
                                    new[] { Expression.Add(Expression.MakeMemberAccess(y, t.GetMember("i").Single()), Expression.Constant(1)) },
                                    t.GetMember("i").Single()
                                )
                            ),
                            y
                        )
                    );

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<int>>().Compile()().First(), r.Funcletize<IEnumerable<int>>().Compile()().First());
            }
        }

        public static Expression<Func<T>> Infer<T>(Expression<Func<T>> f) => f;

        private sealed class SelectCountingVisitor : ExpressionVisitor
        {
            private static readonly MethodInfo s_select = ((MethodInfo)ReflectionHelpers.InfoOf((IEnumerable<int> xs) => xs.Select(default(Func<int, int>)))).GetGenericMethodDefinition();

            public int Count { get; private set; }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.IsGenericMethod && node.Method.GetGenericMethodDefinition() == s_select)
                    Count++;

                return base.VisitMethodCall(node);
            }
        }

        private sealed class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }
    }
}
