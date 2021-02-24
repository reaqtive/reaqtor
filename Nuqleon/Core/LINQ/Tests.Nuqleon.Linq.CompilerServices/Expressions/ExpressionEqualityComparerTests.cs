// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - April 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if USE_SLIM
using System.Linq.CompilerServices.Bonsai;
#endif

#if USE_SLIM
namespace Tests.System.Linq.Expressions.Bonsai.CompilerServices
#else
namespace Tests.System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using MemberAssignment = global::System.Linq.Expressions.MemberAssignmentSlim;
    using MemberBinding = global::System.Linq.Expressions.MemberBindingSlim;
    using MemberListBinding = global::System.Linq.Expressions.MemberListBindingSlim;
    using MemberMemberBinding = global::System.Linq.Expressions.MemberMemberBindingSlim;

    #endregion
#endif
    [TestClass]
    public class ExpressionEqualityComparerTests
    {
        [TestMethod]
        public void ExpressionEquality_Equals_Protected_Null_Null()
        {
            var eq = CreateComparator();

            foreach (var equals in eq.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(m => m.Name == "Equals"))
            {
                try
                {
                    Assert.IsTrue((bool)equals.Invoke(eq, new object[] { null, null }), equals.ToString());
                }
                catch (TargetInvocationException ex)
                {
                    // Some methods require derived types to implement semantics.
                    if (ex.InnerException is not NotImplementedException)
                        throw;
                }
            }
        }

        [TestMethod]
        public void ExpressionEquality_GetHashCode_Protected_Null()
        {
            var eq = CreateComparator();

            foreach (var getHashCode in eq.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(m => m.Name == "GetHashCode" && m.GetParameters().Length == 1))
            {
                try
                {
                    var res = (int)getHashCode.Invoke(eq, new object[] { null });
                    Assert.IsTrue(true);
                }
                catch (TargetInvocationException ex)
                {
                    // Some methods require derived types to implement semantics.
                    if (ex.InnerException is not NotImplementedException)
                        throw;
                }
            }
        }

        [TestMethod]
        public void ExpressionEquality_BigTest1()
        {
            var eq = CreateComparator();

            var e1 = (Expression<Func<int, string, bool>>)((x, s) => -x * 2 + ~Math.Abs(1) / s.Length - x % 7 > 0 || x < 0 && !s.Equals(""));
            var e2 = (Expression<Func<int, string, bool>>)((y, s) => -y * 2 + ~Math.Abs(1) / s.Length - y % 7 > 0 || y < 0 && !s.Equals(""));

#if USE_SLIM
            Assert.IsTrue(eq.Equals(e1.ToExpressionSlim(), e2.ToExpressionSlim()));
            Assert.AreEqual(eq.GetHashCode(e1.ToExpressionSlim()), eq.GetHashCode(e2.ToExpressionSlim()));
#else
            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));
#endif
        }

        [TestMethod]
        public void ExpressionEquality_BigTest2()
        {
            var eq = CreateComparator();

            var e1 = (Expression<Func<int, Func<int, int>>>)(x => y => x);
            var e2 = (Expression<Func<int, Func<int, int>>>)(y => x => y);
            var e3 = (Expression<Func<int, Func<int, int>>>)(a => b => b);

#if USE_SLIM
            Assert.IsTrue(eq.Equals(e1.ToExpressionSlim(), e2.ToExpressionSlim()));
            Assert.IsFalse(eq.Equals(e1.ToExpressionSlim(), e3.ToExpressionSlim()));
            Assert.AreEqual(eq.GetHashCode(e1.ToExpressionSlim()), eq.GetHashCode(e2.ToExpressionSlim()));
#else
            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.IsFalse(eq.Equals(e1, e3));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));
#endif
        }

        [TestMethod]
        public void ExpressionEquality_BigTest3()
        {
            var eq = CreateComparator();

            var e1 = (Expression<Func<Bar>>)(() => new Bar { X = 42, Foo = { Qux = { "2", "3", "5" } } });
            var e2 = (Expression<Func<Bar>>)(() => new Bar { X = 42, Foo = { Qux = { "2", "3", "5" } } });

#if USE_SLIM
            Assert.IsTrue(eq.Equals(e1.ToExpressionSlim(), e2.ToExpressionSlim()));
            Assert.AreEqual(eq.GetHashCode(e1.ToExpressionSlim()), eq.GetHashCode(e2.ToExpressionSlim()));
#else
            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));
#endif
        }

        [TestMethod]
        public void ExpressionEquality_BigTest4()
        {
            var eq = CreateComparator();

            var e1 = default(Expression);
            var e2 = default(Expression);

            {
                var to = Expression.Parameter(typeof(int), "to");
                var res = Expression.Variable(typeof(List<int>), "res");
                var n = Expression.Variable(typeof(int), "n");
                var found = Expression.Variable(typeof(bool), "found");
                var d = Expression.Variable(typeof(int), "d");
                var breakOuter = Expression.Label("break_outer");
                var breakInner = Expression.Label("break_inner");
                e1 = GetPrimesStatementExpression(to, res, n, found, d, breakOuter, breakInner);
            }

            {
                var to = Expression.Parameter(typeof(int), "tot");
                var res = Expression.Variable(typeof(List<int>), "res");
                var n = Expression.Variable(typeof(int), "n");
                var found = Expression.Variable(typeof(bool), "gevonden");
                var d = Expression.Variable(typeof(int), "d");
                var breakOuter = Expression.Label("breek_buiten");
                var breakInner = Expression.Label("breek_binnen");
                e2 = GetPrimesStatementExpression(to, res, n, found, d, breakOuter, breakInner);
            }

#if USE_SLIM
            Assert.IsTrue(eq.Equals(e1.ToExpressionSlim(), e2.ToExpressionSlim()));
#else
            Assert.IsTrue(eq.Equals(e1, e2));
#endif
        }

        [TestMethod]
        public void ExpressionEquality_NewStruct()
        {
            var eq = CreateComparator();

            var n1 = Expression.New(typeof(TimeSpan));
            var n2 = Expression.New(typeof(DateTime));

#if USE_SLIM
            Assert.IsFalse(eq.Equals(n1.ToExpressionSlim(), n2.ToExpressionSlim()));
#else
            Assert.IsFalse(eq.Equals(n1, n2));
#endif
        }

        private static Expression GetPrimesStatementExpression(ParameterExpression to, ParameterExpression res, ParameterExpression n, ParameterExpression found, ParameterExpression d, LabelTarget breakOuter, LabelTarget breakInner)
        {
            return
                Expression.Lambda<Func<int, List<int>>>(
                    Expression.Block(
                        new[] { res },
                        Expression.Assign(
                            res,
                            Expression.New(typeof(List<int>))
                        ),
                        Expression.Block(
                            new[] { n },
                            Expression.Assign(
                                n,
                                Expression.Constant(2)
                            ),
                            Expression.Loop(
                                Expression.Block(
                                    Expression.IfThen(
                                        Expression.Not(
                                            Expression.LessThanOrEqual(
                                                n,
                                                to
                                            )
                                        ),
                                        Expression.Break(breakOuter)
                                    ),
                                    Expression.Block(
                                        new[] { found },
                                        Expression.Assign(
                                            found,
                                            Expression.Constant(false)
                                        ),
                                        Expression.Block(
                                            new[] { d },
                                            Expression.Assign(
                                                d,
                                                Expression.Constant(2)
                                            ),
                                            Expression.Loop(
                                                Expression.Block(
                                                    Expression.IfThen(
                                                        Expression.Not(
                                                            Expression.LessThanOrEqual(
                                                                d,
                                                                Expression.Convert(
                                                                    Expression.Call(
                                                                        null,
                                                                        typeof(Math).GetMethod("Sqrt"),
                                                                        Expression.Convert(
                                                                            n,
                                                                            typeof(double)
                                                                        )
                                                                    ),
                                                                    typeof(int)
                                                                )
                                                            )
                                                        ),
                                                        Expression.Break(breakInner)
                                                    ),
                                                    Expression.Block(
                                                        Expression.IfThen(
                                                            Expression.Equal(
                                                                Expression.Modulo(
                                                                    n,
                                                                    d
                                                                ),
                                                                Expression.Constant(0)
                                                            ),
                                                            Expression.Block(
                                                                Expression.Assign(
                                                                    found,
                                                                    Expression.Constant(true)
                                                                ),
                                                                Expression.Break(breakInner)
                                                            )
                                                        )
                                                    ),
                                                    Expression.PostIncrementAssign(d)
                                                ),
                                                breakInner
                                            )
                                        ),
                                        Expression.IfThen(
                                            Expression.Not(found),
                                            Expression.Call(
                                                res,
                                                typeof(List<int>).GetMethod("Add"),
                                                n
                                            )
                                        )
                                    ),
                                    Expression.PostIncrementAssign(n)
                                ),
                                breakOuter
                            )
                        ),
                        res
                    ),
                    to
                );
        }

        private sealed class Bar
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Foo Foo { get; set; }
            public Foo Baz { get; set; }
        }

        private sealed class Foo
        {
            public List<string> Qux { get; set; }
            public List<string> Bar { get; set; }

            public int X { get; set; }
            public int Y { get; set; }
        }

#if USE_SLIM
        [TestMethod]
        public void ExpressionEquality_Null()
        {
            var eqc = new ExpressionSlimEqualityComparer(CreateComparator);

#pragma warning disable IDE0034 // Simplify 'default' expression. (Documents signature.)
            Assert.IsFalse(eqc.Equals(default(ExpressionSlim), Expression.Constant(1).ToExpressionSlim()));
            Assert.IsFalse(eqc.Equals(Expression.Constant(1).ToExpressionSlim(), default(ExpressionSlim)));
            Assert.IsTrue(eqc.Equals(default(ExpressionSlim), default(ExpressionSlim)));

            var eqr = CreateComparator();

            Assert.IsFalse(eqr.Equals(default(ExpressionSlim), Expression.Constant(1).ToExpressionSlim()));
            Assert.IsFalse(eqr.Equals(Expression.Constant(1).ToExpressionSlim(), default(ExpressionSlim)));
            Assert.IsTrue(eqr.Equals(default(ExpressionSlim), default(ExpressionSlim)));
#pragma warning restore IDE0034
        }
#else
        [TestMethod]
        public void ExpressionEquality_Null()
        {
            var eqc = new ExpressionEqualityComparer(CreateComparator);

            Assert.IsFalse(eqc.Equals(null, Expression.Constant(1)));
            Assert.IsFalse(eqc.Equals(Expression.Constant(1), null));
            Assert.IsTrue(eqc.Equals(null, null));

            var eqr = CreateComparator();

            Assert.IsFalse(eqr.Equals(null, Expression.Constant(1)));
            Assert.IsFalse(eqr.Equals(Expression.Constant(1), null));
            Assert.IsTrue(eqr.Equals(default(Expression), null));
        }
#endif

        [TestMethod]
        public void ExpressionEquality_Binary()
        {
            var e1 = Expression.Add(Expression.Constant(42), Expression.Constant(43));
            var e2 = Expression.Add(Expression.Constant(42), Expression.Constant(43));
            var e3 = Expression.Add(Expression.Constant(42), Expression.Constant(44));
            var e4 = Expression.Add(Expression.Constant(44), Expression.Constant(43));
            var e5 = Expression.AddChecked(Expression.Constant(42), Expression.Constant(43));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);
            AssertNotEqual(e1, e5);

#if USE_SLIM
            AssertProtectedNull((BinaryExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Call_Static()
        {
            var m1 = (MethodInfo)ReflectionHelpers.InfoOf(() => Console.WriteLine(default(string)));
            var m2 = (MethodInfo)ReflectionHelpers.InfoOf(() => Process.Start(default(string)));

            var e1 = Expression.Call(m1, Expression.Constant("x"));
            var e2 = Expression.Call(m1, Expression.Constant("x"));
            var e3 = Expression.Call(m2, Expression.Constant("x"));
            var e4 = Expression.Call(m1, Expression.Constant("y"));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((MethodCallExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Call_Instance()
        {
            var m1 = (MethodInfo)ReflectionHelpers.InfoOf((string s) => s.ToLower());
            var m2 = (MethodInfo)ReflectionHelpers.InfoOf((string s) => s.ToUpper());

            var e1 = Expression.Call(Expression.Constant("x"), m1);
            var e2 = Expression.Call(Expression.Constant("x"), m1);
            var e3 = Expression.Call(Expression.Constant("x"), m2);
            var e4 = Expression.Call(Expression.Constant("y"), m2);

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((MethodCallExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Conditional()
        {
            var e1 = Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Constant(2));
            var e2 = Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Constant(2));
            var e3 = Expression.Condition(Expression.Constant(false), Expression.Constant(1), Expression.Constant(2));
            var e4 = Expression.Condition(Expression.Constant(true), Expression.Constant(3), Expression.Constant(2));
            var e5 = Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Constant(4));
            var e6 = Expression.Constant("");

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);
            AssertNotEqual(e1, e5);

#if USE_SLIM
            AssertProtectedNull((ConditionalExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Constant()
        {
            var e1 = Expression.Constant(42);
            var e2 = Expression.Constant(42);
            var e3 = Expression.Constant(43);
            var e4 = Expression.Constant("");

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((ConstantExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Default()
        {
            var e1 = Expression.Default(typeof(int));
            var e2 = Expression.Default(typeof(int));
            var e3 = Expression.Default(typeof(long));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);

#if USE_SLIM
            AssertProtectedNull((DefaultExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Invocation()
        {
            var f = (Expression<Func<int, int>>)(x => x);
            var g = (Expression<Func<int, int>>)(y => y);
            var h = (Expression<Func<int, int>>)(y => 0);

            var e1 = Expression.Invoke(f, Expression.Constant(42));
            var e2 = Expression.Invoke(g, Expression.Constant(42));
            var e3 = Expression.Invoke(h, Expression.Constant(42));
            var e4 = Expression.Invoke(f, Expression.Constant(43));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((InvocationExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Lambda()
        {
            var e1 = (Expression<Func<int, int>>)(x => x);
            var e2 = (Expression<Func<int, int>>)(y => y);
            var e3 = (Expression<Func<int, int>>)(y => 0);

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);

#if USE_SLIM
            AssertProtectedNull((LambdaExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull((LambdaExpression)e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_ListInit()
        {
            var e1 = (ListInitExpression)((Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 })).Body;
            var e2 = (ListInitExpression)((Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 })).Body;
            var e3 = (ListInitExpression)((Expression<Func<List<int>>>)(() => new List<int> { 2, 4, 5 })).Body;
            var e4 = (ListInitExpression)((Expression<Func<HashSet<int>>>)(() => new HashSet<int> { 2, 3, 5 })).Body;

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((ListInitExpressionSlim)ExpressionSlimExtensions.ToExpressionSlim(e1));
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Member_Static()
        {
            var m1 = (PropertyInfo)ReflectionHelpers.InfoOf(() => Console.ForegroundColor);
            var m2 = (PropertyInfo)ReflectionHelpers.InfoOf(() => Console.BackgroundColor);

            var e1 = Expression.MakeMemberAccess(null, m1);
            var e2 = Expression.MakeMemberAccess(null, m1);
            var e3 = Expression.MakeMemberAccess(null, m2);

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);

#if USE_SLIM
            AssertProtectedNull((MemberExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Member_Instance()
        {
            var m1 = (PropertyInfo)ReflectionHelpers.InfoOf((TimeSpan s) => s.Days);
            var m2 = (PropertyInfo)ReflectionHelpers.InfoOf((TimeSpan s) => s.Hours);

            var e1 = Expression.MakeMemberAccess(Expression.Constant(TimeSpan.Zero), m1);
            var e2 = Expression.MakeMemberAccess(Expression.Constant(TimeSpan.Zero), m1);
            var e3 = Expression.MakeMemberAccess(Expression.Constant(TimeSpan.Zero), m2);
            var e4 = Expression.MakeMemberAccess(Expression.Constant(TimeSpan.FromSeconds(1)), m2);

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((MemberExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_MemberInit()
        {
            var c1 = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new ADS());
            var c2 = (ConstructorInfo)ReflectionHelpers.InfoOf((string c) => new ADS(c));
            var m1 = (PropertyInfo)ReflectionHelpers.InfoOf((ADS s) => s.Foo);
            var m2 = (PropertyInfo)ReflectionHelpers.InfoOf((ADS s) => s.Bar);

            var e1 = Expression.MemberInit(Expression.New(c1), Expression.Bind(m1, Expression.Constant("foo")));
            var e2 = Expression.MemberInit(Expression.New(c1), Expression.Bind(m1, Expression.Constant("foo")));
            var e3 = Expression.MemberInit(Expression.New(c1), Expression.Bind(m1, Expression.Constant("bar")));
            var e4 = Expression.MemberInit(Expression.New(c2, Expression.Constant(value: null, typeof(string))), Expression.Bind(m2, Expression.Constant("foo")));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull(e1.ToExpressionSlimExt());
#else
            AssertProtectedNull(e1);
#endif
        }

        private sealed class ADS
        {
            public ADS()
            {
            }

            public ADS(string foo)
            {
                Foo = foo;
            }

            public string Foo { get; set; }
            public string Bar { get; set; }
        }

        [TestMethod]
        public void ExpressionEquality_New()
        {
            var c1 = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new TimeSpan(0, 0, 0));
            var c2 = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new TimeSpan(0, 0, 0, 0));

            var e1 = Expression.New(c1, Expression.Constant(1), Expression.Constant(2), Expression.Constant(3));
            var e2 = Expression.New(c1, Expression.Constant(1), Expression.Constant(2), Expression.Constant(3));
            var e3 = Expression.New(c1, Expression.Constant(1), Expression.Constant(5), Expression.Constant(3));
            var e4 = Expression.New(c2, Expression.Constant(1), Expression.Constant(2), Expression.Constant(3), Expression.Constant(4));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((NewExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_New_Anonymous()
        {
#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

            var e1 = (NewExpression)GetExpression(() => new { a = 42, b = "foo" });
            var e2 = (NewExpression)GetExpression(() => new { a = 42, b = "foo" });
            var e3 = (NewExpression)GetExpression(() => new { a = 42, b = "bar" });
            var e4 = (NewExpression)GetExpression(() => new { a = 42, b = "foo", c = 49.95m });

#pragma warning restore IDE0050

            var e5 = Expression.New(e1.Constructor, Expression.Constant(42), Expression.Constant("foo"));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);
            AssertNotEqual(e1, e5);
            AssertNotEqual(e5, e1);

#if USE_SLIM
            AssertProtectedNull((NewExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        private static Expression GetExpression<T>(Expression<Func<T>> f)
        {
            return f.Body;
        }

        [TestMethod]
        public void ExpressionEquality_NewArray()
        {
            var e1 = Expression.NewArrayBounds(typeof(int), Expression.Constant(1));
            var e2 = Expression.NewArrayBounds(typeof(int), Expression.Constant(1));
            var e3 = Expression.NewArrayBounds(typeof(long), Expression.Constant(1));
            var e4 = Expression.NewArrayBounds(typeof(int), Expression.Constant(2));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

            var e5 = Expression.NewArrayInit(typeof(int), Expression.Constant(1));
            var e6 = Expression.NewArrayInit(typeof(int), Expression.Constant(1));
            var e7 = Expression.NewArrayInit(typeof(long), Expression.Constant(1L));
            var e8 = Expression.NewArrayInit(typeof(int), Expression.Constant(2));

            AssertEqual(e5, e6);
            AssertNotEqual(e5, e7);
            AssertNotEqual(e5, e8);

            AssertNotEqual(e1, e5);

#if USE_SLIM
            AssertProtectedNull((NewArrayExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_TypeIs()
        {
            var e1 = Expression.TypeIs(Expression.Constant(1), typeof(int));
            var e2 = Expression.TypeIs(Expression.Constant(1), typeof(int));
            var e3 = Expression.TypeIs(Expression.Constant(2), typeof(int));
            var e4 = Expression.TypeIs(Expression.Constant(1), typeof(long));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((TypeBinaryExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Parameter()
        {
            var e1 = Expression.Parameter(typeof(int));
            var e2 = Expression.Parameter(typeof(int), "x");
            var e3 = Expression.Parameter(typeof(int), "y");

            AssertEqual(e1, e1);

            var l1 = Expression.Lambda(e2, e2);
            var l2 = Expression.Lambda(e3, e3);

            AssertEqual(l1, l2);

            var l3 = Expression.Lambda(e2, e3);

            AssertNotEqual(l1, l3);
            AssertNotEqual(l3, l1);

#if USE_SLIM
            AssertProtectedNull((ParameterExpressionSlim)e1.ToExpressionSlim(), "Parameter");
#else
            AssertProtectedNull(e1, "Parameter");
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Unary()
        {
            var e1 = Expression.Negate(Expression.Constant(42));
            var e2 = Expression.Negate(Expression.Constant(42));
            var e3 = Expression.Negate(Expression.Constant(43));
            var e4 = Expression.NegateChecked(Expression.Constant(42));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((UnaryExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Unary_Convert()
        {
            var e1 = Expression.Convert(Expression.Constant(42, typeof(int)), typeof(long));
            var e2 = Expression.Convert(Expression.Constant(42, typeof(int)), typeof(long));
            var e3 = Expression.Convert(Expression.Constant(43, typeof(int)), typeof(double));
            var e4 = Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(long));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);
        }

        [TestMethod]
        public void ExpressionEquality_Block()
        {
            var p1 = Expression.Parameter(typeof(int), "x");
            var p2 = Expression.Parameter(typeof(int), "y");
            var p3 = Expression.Parameter(typeof(long), "x");

            var e1 = Expression.Block(new[] { p1 }, Expression.Add(Expression.Constant(1), p1));
            var e2 = Expression.Block(new[] { p1 }, Expression.Add(Expression.Constant(1), p1));
            var e3 = Expression.Block(new[] { p2 }, Expression.Add(Expression.Constant(1), p2));
            var e4 = Expression.Block(new[] { p3 }, Expression.Add(Expression.Constant(1L), p3));
            var e5 = Expression.Block(new[] { p1 }, Expression.Add(Expression.Constant(1), p2));
            var e6 = Expression.Block(new[] { p1 }, Expression.Add(p1, Expression.Constant(1)));

            AssertEqual(e1, e2);
            AssertEqual(e1, e3);
            AssertNotEqual(e1, e4);
            AssertNotEqual(e1, e5);
            AssertNotEqual(e1, e6);

#if USE_SLIM
            AssertProtectedNull((BlockExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Switch()
        {
            var e1 = Expression.Switch(
                Expression.Constant(42),
                Expression.Constant("a"),
                Expression.SwitchCase(
                    Expression.Constant("b"),
                    Expression.Constant(2),
                    Expression.Constant(3)
                )
            );

            var e2 = Expression.Switch(
                Expression.Constant(42),
                Expression.Constant("a"),
                Expression.SwitchCase(
                    Expression.Constant("b"),
                    Expression.Constant(2),
                    Expression.Constant(3)
                )
            );

            var e3 = Expression.Switch(
                Expression.Constant(43),
                Expression.Constant("a"),
                Expression.SwitchCase(
                    Expression.Constant("b"),
                    Expression.Constant(2),
                    Expression.Constant(3)
                )
            );

            var e4 = Expression.Switch(
                Expression.Constant(42),
                Expression.Constant("c"),
                Expression.SwitchCase(
                    Expression.Constant("b"),
                    Expression.Constant(2),
                    Expression.Constant(3)
                )
            );

            var e5 = Expression.Switch(
                Expression.Constant(42),
                Expression.Constant("a"),
                Expression.SwitchCase(
                    Expression.Constant("c"),
                    Expression.Constant(2),
                    Expression.Constant(3)
                )
            );

            var e6 = Expression.Switch(
                Expression.Constant(42),
                Expression.Constant("a"),
                Expression.SwitchCase(
                    Expression.Constant("b"),
                    Expression.Constant(5),
                    Expression.Constant(3)
                )
            );

            var e7 = Expression.Switch(
                Expression.Constant(42),
                Expression.Constant("a"),
                Expression.SwitchCase(
                    Expression.Constant("b"),
                    Expression.Constant(2),
                    Expression.Constant(3)
                ),
                Expression.SwitchCase(
                    Expression.Constant("c"),
                    Expression.Constant(4),
                    Expression.Constant(5)
                )
            );

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);
            AssertNotEqual(e1, e5);
            AssertNotEqual(e1, e6);
            AssertNotEqual(e1, e7);

#if USE_SLIM
            AssertProtectedNull((SwitchExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_SwitchCase()
        {
            var eq = new ExpressionEqualityComparator();

            var e1 = Expression.SwitchCase(
                Expression.Constant("b"),
                Expression.Constant(2),
                Expression.Constant(3)
            );

            var e2 = Expression.SwitchCase(
                Expression.Constant("b"),
                Expression.Constant(2),
                Expression.Constant(3)
            );

            var e3 = Expression.SwitchCase(
                Expression.Constant("c"),
                Expression.Constant(2),
                Expression.Constant(3)
            );

            var e4 = Expression.SwitchCase(
                Expression.Constant("b"),
                Expression.Constant(4),
                Expression.Constant(5)
            );

            var e5 = Expression.SwitchCase(
                Expression.Constant("b"),
                Expression.Constant(2)
            );

            Assert.IsTrue(eq.Equals(e1, e1));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e1));

            Assert.IsFalse(eq.Equals(e1, null));
            Assert.AreNotEqual(eq.GetHashCode(e1), eq.GetHashCode(default(SwitchCase)));

            Assert.IsFalse(eq.Equals(null, e1));
            Assert.IsTrue(eq.Equals(default(SwitchCase), null));

            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));

            Assert.IsFalse(eq.Equals(e1, e3));
            Assert.AreNotEqual(eq.GetHashCode(e1), eq.GetHashCode(e3));
            Assert.IsFalse(eq.Equals(e1, e4));
            Assert.AreNotEqual(eq.GetHashCode(e1), eq.GetHashCode(e4));
            Assert.IsFalse(eq.Equals(e1, e5));
            Assert.AreNotEqual(eq.GetHashCode(e1), eq.GetHashCode(e5));
        }

        [TestMethod]
        public void ExpressionEquality_TryFinally()
        {
            var f1 = (Expression<Action>)(() => Console.WriteLine("foo"));
            var f2 = (Expression<Action>)(() => Console.WriteLine("bar"));

            var e1 = Expression.TryFinally(Expression.Constant(42), f1.Body);
            var e2 = Expression.TryFinally(Expression.Constant(42), f1.Body);
            var e3 = Expression.TryFinally(Expression.Constant(43), f1.Body);
            var e4 = Expression.TryFinally(Expression.Constant(42), f2.Body);

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((TryExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_TryFault()
        {
            var f1 = (Expression<Action>)(() => Console.WriteLine("foo"));
            var f2 = (Expression<Action>)(() => Console.WriteLine("bar"));

            var e1 = Expression.TryFault(Expression.Constant(42), f1.Body);
            var e2 = Expression.TryFault(Expression.Constant(42), f1.Body);
            var e3 = Expression.TryFault(Expression.Constant(43), f1.Body);
            var e4 = Expression.TryFault(Expression.Constant(42), f2.Body);

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((TryExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_TryCatch()
        {
            var a1 = (Expression<Action>)(() => Console.WriteLine("foo"));
            var a2 = (Expression<Action>)(() => Console.WriteLine("bar"));

            var f1 = (Expression<Action<InvalidOperationException>>)(ex => Console.WriteLine(ex.Message));
            var f2 = (Expression<Action<InvalidOperationException>>)(err => Console.WriteLine(err.Message));
            var f3 = (Expression<Action<NotSupportedException>>)(ex => Console.WriteLine(ex.Message));
            var f4 = (Expression<Action<InvalidOperationException>>)(err => Console.WriteLine("Oops! " + err.Message));

            var p1 = f1.Parameters[0];
            var p2 = f2.Parameters[0];
            var p3 = f3.Parameters[0];
            var p4 = f4.Parameters[0];

            var e1 = Expression.TryCatch(a1.Body, Expression.Catch(p1, f1.Body));
            var e2 = Expression.TryCatch(a1.Body, Expression.Catch(p2, f2.Body));
            var e3 = Expression.TryCatch(a1.Body, Expression.Catch(p3, f3.Body));
            var e4 = Expression.TryCatch(a2.Body, Expression.Catch(p1, f1.Body));
            var e5 = Expression.TryCatch(a1.Body, Expression.Catch(p4, f4.Body));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);
            AssertNotEqual(e1, e5);

#if USE_SLIM
            AssertProtectedNull((TryExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_CatchBlock()
        {
            var eq = new ExpressionEqualityComparator();

            var f1 = (Expression<Action<InvalidOperationException>>)(ex => Console.WriteLine(ex.Message));
            var f2 = (Expression<Action<InvalidOperationException>>)(err => Console.WriteLine(err.Message));
            var f3 = (Expression<Action<NotSupportedException>>)(ex => Console.WriteLine(ex.Message));

            var p1 = f1.Parameters[0];
            var p2 = f2.Parameters[0];
            var p3 = f3.Parameters[0];

            var e1 = Expression.Catch(p1, f1.Body);
            var e2 = Expression.Catch(p2, f2.Body);
            var e3 = Expression.Catch(p3, f3.Body);
            var e4 = Expression.Catch(p1, f1.Body, Expression.Constant(true));
            var e5 = Expression.Catch(p1, f1.Body, Expression.Constant(true));

            Assert.IsTrue(eq.Equals(e1, e1));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e1));

            Assert.IsFalse(eq.Equals(e1, null));
            Assert.AreNotEqual(eq.GetHashCode(e1), eq.GetHashCode(default(CatchBlock)));

            Assert.IsFalse(eq.Equals(null, e1));
            Assert.IsTrue(eq.Equals(default(CatchBlock), null));

            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));

            Assert.IsFalse(eq.Equals(e1, e3));
            Assert.AreNotEqual(eq.GetHashCode(e1), eq.GetHashCode(e3));

            Assert.IsFalse(eq.Equals(e1, e4));
            Assert.AreNotEqual(eq.GetHashCode(e1), eq.GetHashCode(e4));

            Assert.IsTrue(eq.Equals(e4, e5));
            Assert.AreEqual(eq.GetHashCode(e4), eq.GetHashCode(e5));
        }

        [TestMethod]
        public void ExpressionEquality_Try_Mixed()
        {
            var f1 = (Expression<Action>)(() => Console.WriteLine("foo"));
            var f2 = (Expression<Action>)(() => Console.WriteLine("bar"));

            var e1 = Expression.TryFault(Expression.Constant(42), f1.Body);
            var e2 = Expression.TryFinally(Expression.Constant(42), f1.Body);

            AssertNotEqual(e1, e2);
        }

        [TestMethod]
        public void ExpressionEquality_Loop()
        {
            var lt1 = Expression.Label("l1");
            var lt2 = Expression.Label("l2");
            var lt3 = Expression.Label("l3");

            var l1 = Expression.Label(lt1);
            var l2 = Expression.Label(lt2);
            var l3 = Expression.Label(lt3);

            var e1 = Expression.Loop(Expression.Constant(42), lt1, lt2);
            var e2 = Expression.Loop(Expression.Constant(42), lt1, lt2);
            var e3 = Expression.Loop(Expression.Constant(43), lt1, lt2);

            var e4 = Expression.Loop(Expression.Goto(lt1), lt1, lt2);
            var e5 = Expression.Loop(Expression.Goto(lt1), lt1, lt2);
            var e6 = Expression.Loop(Expression.Goto(lt2), lt1, lt2);
            var e7 = Expression.Loop(Expression.Goto(lt3), lt1, lt2);
            var e8 = Expression.Loop(Expression.Goto(lt1), lt1);

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

            AssertEqual(e4, e5);
            AssertNotEqual(e4, e6);
            AssertNotEqual(e4, e7);
            AssertNotEqual(e4, e8);

#if USE_SLIM
            AssertProtectedNull((LoopExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Index()
        {
            var c1 = Expression.Constant(new List<int>());
            var c2 = Expression.Constant(new Dictionary<string, int>());

            var e1 = Expression.MakeIndex(c1, c1.Type.GetProperty("Item"), new Expression[] { Expression.Constant(1) });
            var e2 = Expression.MakeIndex(c1, c1.Type.GetProperty("Item"), new Expression[] { Expression.Constant(1) });
            var e3 = Expression.MakeIndex(c2, c2.Type.GetProperty("Item"), new Expression[] { Expression.Constant("") });
            var e4 = Expression.MakeIndex(c1, c1.Type.GetProperty("Item"), new Expression[] { Expression.Constant(2) });

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e4);

#if USE_SLIM
            AssertProtectedNull((IndexExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

#if !USE_SLIM
        [TestMethod]
        public void ExpressionEquality_Dynamic()
        {
            var add = Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(
                Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags.None,
                ExpressionType.Add,
                typeof(ExpressionEqualityComparerTests),
                new[]
                {
                    Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null),
                    Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null)
                }
            );

            var e1 = Expression.Dynamic(
                add,
                typeof(object),
                Expression.Constant(1),
                Expression.Constant(2)
            );

            var e2 = Expression.Dynamic(
                add,
                typeof(object),
                Expression.Constant(1),
                Expression.Constant(2)
            );

            var e3 = Expression.Dynamic(
                add,
                typeof(object),
                Expression.Constant(1),
                Expression.Constant(3)
            );

            AssertEqual(e1, e1);
            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);

            AssertProtectedNull<DynamicExpression>(e1);
        }
#endif

        [TestMethod]
        public void ExpressionEquality_Goto()
        {
            var lt = Expression.Label();

            var e1 = Expression.Goto(lt);
            var e2 = Expression.Goto(lt);
            var e3 = Expression.Return(lt, Expression.Constant(42));
            var e4 = Expression.Return(lt, Expression.Constant(42));
            var e5 = Expression.Return(lt, Expression.Constant(43));

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);
            AssertEqual(e3, e4);
            AssertNotEqual(e3, e5);

#if USE_SLIM
            AssertProtectedNull((GotoExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Label()
        {
            var l1 = Expression.Label();

            var e1 = Expression.Label(l1);
            var e2 = Expression.Label(l1);

            var e3 = Expression.Label(l1, Expression.Constant(1));
            var e4 = Expression.Label(l1, Expression.Constant(1));
            var e5 = Expression.Label(l1, Expression.Constant(2));

            AssertEqual(e1, e2);
            AssertEqual(e3, e4);
            AssertNotEqual(e1, e3);
            AssertNotEqual(e1, e5);

#if USE_SLIM
            AssertProtectedNull((LabelExpressionSlim)e1.ToExpressionSlim());
#else
            AssertProtectedNull(e1);
#endif
        }

        [TestMethod]
        public void ExpressionEquality_Block_WithLabels()
        {
            var l1 = Expression.Label();
            var l2 = Expression.Label();

            var e1 = Expression.Block(Expression.Label(l1), Expression.Goto(l1));
            var e2 = Expression.Block(Expression.Label(l1), Expression.Goto(l1));

            AssertEqual(e1, e2);

            var e3 = Expression.Block(Expression.Goto(l1), Expression.Label(l1));
            var e4 = Expression.Block(Expression.Goto(l1), Expression.Label(l1));

            AssertEqual(e3, e4);
            AssertNotEqual(e1, e3);

            var e5 = Expression.Block(Expression.Goto(l1));

            AssertNotEqual(e1, e5);
            AssertNotEqual(e3, e5);

            var e6 = Expression.Block(Expression.Label(l1), Expression.Label(l2), Expression.Goto(l1));
            var e7 = Expression.Block(Expression.Label(l1), Expression.Label(l2), Expression.Goto(l2));

            AssertNotEqual(e6, e7);
        }

        [TestMethod]
        public void ExpressionEquality_MemberBinding()
        {
            var eq = CreateComparator();

            var f1 = (Expression<Func<Bar>>)(() => new Bar { X = 42 });
            var f2 = (Expression<Func<Bar>>)(() => new Bar { Foo = { X = 42 } });
            var f3 = (Expression<Func<Foo>>)(() => new Foo { Qux = { "b" } });

            var e1 = (MemberAssignment)((MemberInitExpression)f1.Body).ToExpressionSlimExt().Bindings[0];
            var e2 = (MemberMemberBinding)((MemberInitExpression)f2.Body).ToExpressionSlimExt().Bindings[0];
            var e3 = (MemberListBinding)((MemberInitExpression)f3.Body).ToExpressionSlimExt().Bindings[0];

            Assert.IsFalse(eq.Equals(e1, e2));
            Assert.IsFalse(eq.Equals(e1, e3));
            Assert.IsFalse(eq.Equals(e2, e3));

            AssertProtectedNull<MemberBinding>(e1);
        }

        [TestMethod]
        public void ExpressionEquality_MemberAssignment()
        {
            var f1 = (Expression<Func<Bar>>)(() => new Bar { X = 42 });
            var f2 = (Expression<Func<Bar>>)(() => new Bar { X = 42 });
            var f3 = (Expression<Func<Bar>>)(() => new Bar { X = 43 });
            var f4 = (Expression<Func<Bar>>)(() => new Bar { Y = 42 });

            var e1 = (MemberAssignment)((MemberInitExpression)f1.Body).ToExpressionSlimExt().Bindings[0];
            var e2 = (MemberAssignment)((MemberInitExpression)f2.Body).ToExpressionSlimExt().Bindings[0];
            var e3 = (MemberAssignment)((MemberInitExpression)f3.Body).ToExpressionSlimExt().Bindings[0];
            var e4 = (MemberAssignment)((MemberInitExpression)f4.Body).ToExpressionSlimExt().Bindings[0];

            var eq = CreateComparator();

            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));
            Assert.IsFalse(eq.Equals(e1, e3));
            Assert.IsFalse(eq.Equals(e1, e4));

            AssertProtectedNull<MemberAssignment>(e1);
        }

        [TestMethod]
        public void ExpressionEquality_MemberMemberBinding()
        {
            var f1 = (Expression<Func<Bar>>)(() => new Bar { Foo = { X = 42 } });
            var f2 = (Expression<Func<Bar>>)(() => new Bar { Foo = { X = 42 } });
            var f3 = (Expression<Func<Bar>>)(() => new Bar { Foo = { X = 43 } });
            var f4 = (Expression<Func<Bar>>)(() => new Bar { Foo = { Y = 42 } });

            var e1 = (MemberMemberBinding)((MemberInitExpression)f1.Body).ToExpressionSlimExt().Bindings[0];
            var e2 = (MemberMemberBinding)((MemberInitExpression)f2.Body).ToExpressionSlimExt().Bindings[0];
            var e3 = (MemberMemberBinding)((MemberInitExpression)f3.Body).ToExpressionSlimExt().Bindings[0];
            var e4 = (MemberMemberBinding)((MemberInitExpression)f4.Body).ToExpressionSlimExt().Bindings[0];

            var eq = CreateComparator();

            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));
            Assert.IsFalse(eq.Equals(e1, e3));
            Assert.IsFalse(eq.Equals(e1, e4));

            AssertProtectedNull<MemberMemberBinding>(e1);
        }

        [TestMethod]
        public void ExpressionEquality_MemberListBinding()
        {
            var f1 = (Expression<Func<Foo>>)(() => new Foo { Qux = { "a" } });
            var f2 = (Expression<Func<Foo>>)(() => new Foo { Qux = { "a" } });
            var f3 = (Expression<Func<Foo>>)(() => new Foo { Qux = { "b" } });
            var f4 = (Expression<Func<Foo>>)(() => new Foo { Bar = { "a" } });

            var e1 = (MemberListBinding)((MemberInitExpression)f1.Body).ToExpressionSlimExt().Bindings[0];
            var e2 = (MemberListBinding)((MemberInitExpression)f2.Body).ToExpressionSlimExt().Bindings[0];
            var e3 = (MemberListBinding)((MemberInitExpression)f3.Body).ToExpressionSlimExt().Bindings[0];
            var e4 = (MemberListBinding)((MemberInitExpression)f4.Body).ToExpressionSlimExt().Bindings[0];

            var eq = CreateComparator();

            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));
            Assert.IsFalse(eq.Equals(e1, e3));
            Assert.IsFalse(eq.Equals(e1, e4));

            AssertProtectedNull<MemberListBinding>(e1);
        }

        [TestMethod]
        public void ExpressionEquality_ElementInit()
        {
            var f1 = (Expression<Func<List<int>>>)(() => new List<int> { 2 });
            var f2 = (Expression<Func<List<int>>>)(() => new List<int> { 2 });
            var f3 = (Expression<Func<List<int>>>)(() => new List<int> { 3 });
            var f4 = (Expression<Func<List<long>>>)(() => new List<long> { 2L });

            var e1 = ((ListInitExpression)f1.Body).ToExpressionSlimExt().Initializers[0];
            var e2 = ((ListInitExpression)f2.Body).ToExpressionSlimExt().Initializers[0];
            var e3 = ((ListInitExpression)f3.Body).ToExpressionSlimExt().Initializers[0];
            var e4 = ((ListInitExpression)f4.Body).ToExpressionSlimExt().Initializers[0];

            var eq = CreateComparator();

            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));
            Assert.IsFalse(eq.Equals(e1, e3));
            Assert.IsFalse(eq.Equals(e1, e4));

            AssertProtectedNull(e1);
        }

#if !USE_SLIM
        [TestMethod]
        public void ExpressionEquality_EnumerableHelpers_Null()
        {
            AssertProtectedNull(new Expression[] { Expression.Constant(42) }.ToReadOnly());
            AssertProtectedNull(new MemberBinding[] { Expression.Bind(ReflectionHelpers.InfoOf(() => Console.ForegroundColor), Expression.Constant(ConsoleColor.Red)) }.ToReadOnly());
            AssertProtectedNull(new ElementInit[] { Expression.ElementInit((MethodInfo)ReflectionHelpers.InfoOf((List<int> xs) => xs.Add(0)), Expression.Constant(1)) }.ToReadOnly());
            AssertProtectedNull(new CatchBlock[] { Expression.Catch(Expression.Parameter(typeof(Expression)), Expression.Constant(1)) }.ToReadOnly());
            AssertProtectedNull(new SwitchCase[] { Expression.SwitchCase(Expression.Constant(1), Expression.Constant(2)) }.ToReadOnly());
        }
#endif

        [TestMethod]
        public void ExpressionEquality_Extension()
        {
            var eq = CreateComparator();

            Assert.ThrowsException<NotImplementedException>(() => eq.Equals(new MyExt(), new MyExt()));
            Assert.ThrowsException<NotImplementedException>(() => eq.GetHashCode(new MyExt()));
        }

        [TestMethod]
        public void ExpressionEquality_Extension_Custom()
        {
            var eq = new ExpressionEqualityComparatorWithExt();

            var e1 = new MyExt(42);
            var e2 = new MyExt(42);
            var e3 = new MyExt(43);

            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.IsFalse(eq.Equals(e1, e3));

            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));
            Assert.AreNotEqual(eq.GetHashCode(e1), eq.GetHashCode(e3));
        }

        private sealed class MyExt
#if USE_SLIM
            : ExpressionSlim
#else
            : Expression
#endif
        {
            public MyExt()
                : this(0)
            {
            }

            public MyExt(int x)
            {
                X = x;
            }

            public int X
            {
                get;
                private set;
            }

            public override ExpressionType NodeType => ExpressionType.Extension;

#if USE_SLIM
            protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
            {
                throw new NotImplementedException();
            }

            protected internal override TExpression Accept<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>(ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> visitor)
            {
                throw new NotImplementedException();
            }
#endif
        }

        private sealed class ExpressionEqualityComparatorWithExt
#if USE_SLIM
            : ExpressionSlimEqualityComparator
#else
            : ExpressionEqualityComparator
#endif

        {
#if USE_SLIM
            protected override bool EqualsExtension(ExpressionSlim x, ExpressionSlim y)
#else
            protected override bool EqualsExtension(Expression x, Expression y)
#endif
            {
                if (x is MyExt ex && y is MyExt ey)
                {
                    return ex.X == ey.X;
                }

                return x == y;
            }

#if USE_SLIM
            protected override int GetHashCodeExtension(ExpressionSlim obj)
#else
            protected override int GetHashCodeExtension(Expression obj)
#endif
            {
                return obj is MyExt e ? e.X.GetHashCode() : obj.GetHashCode();
            }
        }

#if !USE_SLIM
        [TestMethod]
        public void ExpressionEquality_DebugInfo()
        {
            var eq = new ExpressionEqualityComparator();

            var e1 = Expression.DebugInfo(Expression.SymbolDocument("foo"), 1, 1, 1, 1);
            var e2 = Expression.DebugInfo(Expression.SymbolDocument("foo"), 1, 1, 1, 1);

            Assert.ThrowsException<NotImplementedException>(() => eq.Equals(e1, e2));
            Assert.ThrowsException<NotImplementedException>(() => eq.GetHashCode(e1));
        }

        [TestMethod]
        public void ExpressionEquality_DebugInfo_Custom()
        {
            var eq = new ExpressionEqualityComparatorWithSymbolInfo();

            var e1 = Expression.DebugInfo(Expression.SymbolDocument("foo"), 1, 1, 1, 1);
            var e2 = Expression.DebugInfo(Expression.SymbolDocument("bar"), 1, 1, 1, 1);

            Assert.IsTrue(eq.Equals(e1, e2));
            Assert.AreEqual(eq.GetHashCode(e1), eq.GetHashCode(e2));
        }

        private sealed class ExpressionEqualityComparatorWithSymbolInfo : ExpressionEqualityComparator
        {
            protected override bool EqualsDebugInfo(DebugInfoExpression x, DebugInfoExpression y)
            {
                return true;
            }

            protected override int GetHashCodeDebugInfo(DebugInfoExpression obj)
            {
                return 42;
            }
        }
#endif

#if !USE_SLIM
        [TestMethod]
        public void ExpressionEquality_RuntimeVariables()
        {
            var p1 = Expression.Parameter(typeof(int), "x");
            var p2 = Expression.Parameter(typeof(int), "y");
            var p3 = Expression.Parameter(typeof(long));

            var e1 = Expression.RuntimeVariables(p1);
            var e2 = Expression.RuntimeVariables(p1);
            var e3 = Expression.RuntimeVariables(p3);

            AssertEqual(e1, e2);
            AssertNotEqual(e1, e3);

            var e4 = Expression.Lambda(Expression.RuntimeVariables(p1), p1);
            var e5 = Expression.Lambda(Expression.RuntimeVariables(p2), p2);
            var e6 = Expression.Lambda(Expression.RuntimeVariables(p3), p3);

            AssertEqual(e4, e5);
            AssertNotEqual(e4, e6);

            AssertProtectedNull<RuntimeVariablesExpression>(e1);
        }
#endif

#if USE_SLIM
        [TestMethod]
        public void ExpressionEquality_ArgumentChecks()
        {
            var e = Expression.Constant(42).ToExpressionSlim();

            Assert.ThrowsException<ArgumentNullException>(() => _ = new ExpressionSlimEqualityComparer(null));
            Assert.ThrowsException<InvalidOperationException>(() => _ = new ExpressionSlimEqualityComparer(() => null).Equals(e, e));
            Assert.ThrowsException<InvalidOperationException>(() => _ = new ExpressionSlimEqualityComparer(() => null).GetHashCode(e));
        }
#else
        [TestMethod]
        public void ExpressionEquality_ArgumentChecks()
        {
            var e = Expression.Constant(42);

            Assert.ThrowsException<ArgumentNullException>(() => _ = new ExpressionEqualityComparer(null));
            Assert.ThrowsException<InvalidOperationException>(() => _ = new ExpressionEqualityComparer(() => null).Equals(e, e));
            Assert.ThrowsException<InvalidOperationException>(() => _ = new ExpressionEqualityComparer(() => null).GetHashCode(e));
        }
#endif

#if USE_SLIM
        [TestMethod]
        public void ExpressionEquality_Customized()
        {
            var eq = new ExpressionSlimEqualityComparer(() => new CustomComparator());

            Assert.IsTrue(eq.Equals(Expression.Constant(42).ToExpressionSlim(), Expression.Constant(43).ToExpressionSlim()));
        }

        private sealed class CustomComparator : ExpressionSlimEqualityComparator
        {
            protected override bool EqualsConstant(ConstantExpressionSlim x, ConstantExpressionSlim y)
            {
                return true;
            }
        }
#else
        [TestMethod]
        public void ExpressionEquality_Customized()
        {
            var eq = new ExpressionEqualityComparer(() => new CustomComparator());

            Assert.IsTrue(eq.Equals(Expression.Constant(42), Expression.Constant(43)));
        }

        private sealed class CustomComparator : ExpressionEqualityComparator
        {
            protected override bool EqualsConstant(ConstantExpression x, ConstantExpression y)
            {
                return true;
            }
        }
#endif

        [TestMethod]
        public void ExpressionEquality_GlobalParameters()
        {
#if USE_SLIM
            var eqd = new ExpressionSlimEqualityComparer();
            var eqc = new ExpressionSlimEqualityComparer(() => new GlobalParameterComparator());
#else
            var eqd = new ExpressionEqualityComparer();
            var eqc = new ExpressionEqualityComparer(() => new GlobalParameterComparator());
#endif

            var p1 = Expression.Parameter(typeof(int), "x").ToExpressionSlimExt();
            var p2 = Expression.Parameter(typeof(int), "x").ToExpressionSlimExt();
            var p3 = Expression.Parameter(typeof(string), "x").ToExpressionSlimExt();
            var p4 = Expression.Parameter(typeof(int), "y").ToExpressionSlimExt();

            var p5 = Expression.Parameter(typeof(int)).ToExpressionSlimExt();
            var p6 = Expression.Parameter(typeof(int)).ToExpressionSlimExt();

            var eq = new[]
            {
                new[] { p1, p2 },
                new[] { p2, p1 },
                new[] { p5, p6 },
                new[] { p6, p5 },

                new[] { p1, p1 },
                new[] { p2, p2 },
                new[] { p3, p3 },
                new[] { p4, p4 },
                new[] { p5, p5 },
                new[] { p6, p6 },
            };

            foreach (var e in eq)
            {
                if (object.ReferenceEquals(e[0], e[1]))
                {
                    Assert.IsTrue(eqd.Equals(e[0], e[1]));
                    Assert.AreEqual(eqd.GetHashCode(e[0]), eqd.GetHashCode(e[1]));
                }

                Assert.IsTrue(eqc.Equals(e[0], e[1]));
                Assert.AreEqual(eqc.GetHashCode(e[0]), eqc.GetHashCode(e[1]));
            }

            var neq = new[]
            {
                new[] { p1, p3 },
                new[] { p3, p1 },
                new[] { p2, p3 },
                new[] { p3, p2 },
                new[] { p1, p4 },
                new[] { p4, p1 },
            };

            foreach (var e in neq)
            {
                Assert.IsFalse(eqd.Equals(e[0], e[1]));
                Assert.IsFalse(eqc.Equals(e[0], e[1]));
            }
        }

        private sealed class GlobalParameterComparator
#if USE_SLIM
            : ExpressionSlimEqualityComparator
#else
            : ExpressionEqualityComparator
#endif
        {
#if USE_SLIM
            protected override bool EqualsGlobalParameter(ParameterExpressionSlim x, ParameterExpressionSlim y)
#else
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
#endif
            {
#if USE_SLIM
                return EqualityComparer<TypeSlim>.Default.Equals(x.Type, y.Type)
#else
                return EqualityComparer<Type>.Default.Equals(x.Type, y.Type)
#endif
                    && EqualityComparer<string>.Default.Equals(x.Name, y.Name);
            }

#if USE_SLIM
            protected override int GetHashCodeGlobalParameter(ParameterExpressionSlim obj)
#else
            protected override int GetHashCodeGlobalParameter(ParameterExpression obj)
#endif
            {
                return
#if USE_SLIM
                    EqualityComparer<TypeSlim>.Default.GetHashCode(obj.Type) ^
#else
                    EqualityComparer<Type>.Default.GetHashCode(obj.Type) ^
#endif
                    EqualityComparer<string>.Default.GetHashCode(obj.Name);
            }
        }

        private static void AssertEqual(Expression x, Expression y)
        {
#if USE_SLIM
            var t = Expression.Block(x, y).ToExpressionSlimExt();
            var a = t.Expressions[0];
            var b = t.Expressions[1];
            var eq = new ExpressionSlimEqualityComparer(CreateComparator);
#else
            var a = x;
            var b = y;
            var eq = new ExpressionEqualityComparer();
#endif

            Assert.IsTrue(eq.Equals(a, b));
            Assert.IsTrue(eq.Equals(b, a));
            Assert.AreEqual(eq.GetHashCode(a), eq.GetHashCode(b));
        }

        private static void AssertNotEqual(Expression x, Expression y)
        {
#if USE_SLIM
            var t = Expression.Block(x, y).ToExpressionSlimExt();
            var a = t.Expressions[0];
            var b = t.Expressions[1];
            var eq = new ExpressionSlimEqualityComparer(CreateComparator);
#else
            var a = x;
            var b = y;
            var eq = new ExpressionEqualityComparer();
#endif

            Assert.IsFalse(eq.Equals(a, b));
            Assert.IsFalse(eq.Equals(b, a));
            Assert.AreNotEqual(eq.GetHashCode(a), eq.GetHashCode(b));
        }

        private static void AssertProtectedNull<T>(T e)
        {
            var eq = CreateComparator();

            var equals = eq.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Single(m => m.Name.StartsWith("Equals") && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType == typeof(T));
            Assert.IsTrue((bool)equals.Invoke(eq, new object[] { null, null }));
            Assert.IsFalse((bool)equals.Invoke(eq, new object[] { e, null }));
            Assert.IsFalse((bool)equals.Invoke(eq, new object[] { null, e }));

            var getHashCode = eq.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Single(m => m.Name.StartsWith("GetHashCode") && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(T));
            Assert.AreNotEqual(0, (int)getHashCode.Invoke(eq, new object[] { null }));
        }

        private static void AssertProtectedNull<T>(T e, string methodDiscriminator)
        {
            var eq = CreateComparator();

            var equals = eq.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Single(m => m.Name.StartsWith(string.Format("Equals{0}", methodDiscriminator)) && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType == typeof(T));
            Assert.IsTrue((bool)equals.Invoke(eq, new object[] { null, null }));
            Assert.IsFalse((bool)equals.Invoke(eq, new object[] { e, null }));
            Assert.IsFalse((bool)equals.Invoke(eq, new object[] { null, e }));

            var getHashCode = eq.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Single(m => m.Name.StartsWith(string.Format("GetHashCode{0}", methodDiscriminator)) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(T));
            Assert.AreNotEqual(0, (int)getHashCode.Invoke(eq, new object[] { null }));
        }

#if USE_SLIM
        private static ExpressionSlimEqualityComparator CreateComparator()
        {
            return new ExpressionSlimEqualityComparator(
                EqualityComparer<TypeSlim>.Default,
                EqualityComparer<MemberInfoSlim>.Default,
                new ObjectSlimComparer(),
                EqualityComparer<global::System.Runtime.CompilerServices.CallSiteBinder>.Default);
        }

#else
        private static ExpressionEqualityComparator CreateComparator()
        {
            return new ExpressionEqualityComparator();
        }
#endif

#if USE_SLIM
        private sealed class ObjectSlimComparer : IEqualityComparer<ObjectSlim>
        {
            public bool Equals(ObjectSlim x, ObjectSlim y)
            {
                return
                    EqualityComparer<TypeSlim>.Default.Equals(x.TypeSlim, y.TypeSlim) &&
                    EqualityComparer<object>.Default.Equals(x.Value, y.Value);
            }

            public int GetHashCode(ObjectSlim obj)
            {
                if (obj.Value == null)
                    return 42;

                return obj.Value.GetHashCode();
            }
        }
#endif
    }

    internal static class Extensions
    {
#if USE_SLIM
        internal static ListInitExpressionSlim ToExpressionSlimExt(this ListInitExpression e) => (ListInitExpressionSlim)ExpressionSlimExtensions.ToExpressionSlim(e);
        internal static MemberInitExpressionSlim ToExpressionSlimExt(this MemberInitExpression e) => (MemberInitExpressionSlim)ExpressionSlimExtensions.ToExpressionSlim(e);
        internal static ParameterExpressionSlim ToExpressionSlimExt(this ParameterExpression e) => (ParameterExpressionSlim)ExpressionSlimExtensions.ToExpressionSlim(e);
        internal static BlockExpressionSlim ToExpressionSlimExt(this BlockExpression e) => (BlockExpressionSlim)ExpressionSlimExtensions.ToExpressionSlim(e);
#else
        internal static T ToExpressionSlimExt<T>(this T e) => e;
#endif
    }
}
