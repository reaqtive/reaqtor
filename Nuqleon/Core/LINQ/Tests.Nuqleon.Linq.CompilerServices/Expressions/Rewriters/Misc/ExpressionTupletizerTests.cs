// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if USE_SLIM
using System.Linq.CompilerServices.Bonsai;
using System.Reflection;
#endif

#if USE_SLIM
namespace Tests.System.Linq.Expressions.Bonsai
#else
namespace Tests.System.Linq.CompilerServices
#endif
{
    [TestClass]
#if USE_SLIM
    public class ExpressionSlimTupletizerTests
#else
    public class ExpressionTupletizerTests
#endif
    {
        [TestMethod]
        public void ExpressionTupletizer_Pack_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => Pack(default(IEnumerable<Expression>)), ex => Assert.AreEqual("expressions", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => Pack(Enumerable.Empty<Expression>()), ex => Assert.AreEqual("expressions", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => Pack(default(LambdaExpression)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Pack(default(LambdaExpression), typeof(int)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Pack((Expression<Func<int, int>>)(x => x), default(Type)), ex => Assert.AreEqual("voidType", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => Pack(default(Expression), Array.Empty<ParameterExpression>()), ex => Assert.AreEqual("body", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Pack(Expression.Constant(1), default(ParameterExpression[])), ex => Assert.AreEqual("parameters", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Pack(default(Expression), Array.Empty<ParameterExpression>(), typeof(int)), ex => Assert.AreEqual("body", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Pack(Expression.Constant(1), default(ParameterExpression[]), typeof(int)), ex => Assert.AreEqual("parameters", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Pack(Expression.Constant(1), Array.Empty<ParameterExpression>(), default(Type)), ex => Assert.AreEqual("voidType", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ExpressionTupletizer_Pack_TheWorks()
        {
            for (int i = 1; i < 18; i++)
            {
                var t = Enumerable.Range(1, i).Select(x => Expression.Constant(x));
                var e = ExpressionTupletizer.Pack(t);

                var n = 0;
                var u = e;
                while (u != null)
                {
                    var c = u.Type.GetGenericArguments().Length;
                    n += c;

                    var f = (NewExpression)u;

                    if (u.Type.GetGenericTypeDefinition() == typeof(Tuple<,,,,,,,>))
                    {
                        u = f.Arguments.Last();
                        Assert.IsTrue(u.Type.Name.StartsWith("Tuple"));

                        Assert.IsTrue(f.Members.Take(7).Select(m => m.Name).SequenceEqual(Enumerable.Range(1, 7).Select(j => "Item" + j)));
                        Assert.IsTrue(f.Members.Last().Name == "Rest");
                        n--;
                    }
                    else
                    {
                        Assert.IsTrue(f.Members.Select(m => m.Name).SequenceEqual(Enumerable.Range(1, c).Select(j => "Item" + j)));
                        u = null;
                    }
                }

                Assert.AreEqual(i, n);

                var s = e.Evaluate();

                var z = "(" + string.Join(", ", t.Select(c => c.Value)) + ")";
                Assert.AreEqual(z, s.ToString());
            }
        }

        [TestMethod]
        public void ExpressionTupletizer_PackAndUnpack_Lambda_NoVoid()
        {
#if USE_SLIM
            var packs = new Func<LambdaExpression, LambdaExpression>[]
            {
                f => (LambdaExpression)ExpressionSlimTupletizer.Pack((LambdaExpressionSlim)f.ToExpressionSlim()).ToExpression(),
                f => { var s = (LambdaExpressionSlim)f.ToExpressionSlim(); return (LambdaExpression)ExpressionSlimTupletizer.Pack(s.Body, s.Parameters).ToExpression(); },
            };
#else
            var packs = new Func<LambdaExpression, LambdaExpression>[]
            {
                f => ExpressionTupletizer.Pack(f),
                f => ExpressionTupletizer.Pack(f.Body, f.Parameters),
            };
#endif

            foreach (var f in new LambdaExpression[] {
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
                (Expression<Func<int>>)(() => 42),
#pragma warning restore IDE0004 // Remove Unnecessary Cast
                (Expression<Func<string, int>>)(s => s.Length),
                (Expression<Func<string, int, int>>)((s, i) => s.Length + i),
                (Expression<Func<int, int, int, int>>)((a, b, c) => a * b + c),
                (Expression<Func<int, int, int, int, int>>)((a, b, c, d) => a * b + c - d),
                (Expression<Func<int, int, int, int, int, int>>)((a, b, c, d, e) => a * b + c - d / e),
                (Expression<Func<int, int, int, int, int, int, int>>)((a, b, c, d, e, f) => a * b + c - d / e + f),
                (Expression<Func<int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g) => a * b + c - d / e + f * g),
                (Expression<Func<int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h) => a * b + c - d / e + f * g / h),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i) => a * b + c - d / e + f * g / h + i),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j) => a * b + c - d / e + f * g / h + i - j),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k) => a * b + c - d / e + f * g / h + i - j * k),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k, l) => a * b + c - d / e + f * g / h + i - j * k / l),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k, l, m) => a * b + c - d / e + f * g / h + i - j * k / l + m),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a * b + c - d / e + f * g / h + i - j * k / l + m % n),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a * b + c - d / e + f * g / h + i - j * k / l + m % n + o),
                (Expression<Func<string, Uri, int, double, DateTime, float, byte, TimeSpan, long, short, Guid, char, uint, AppDomain, decimal[], List<int>, bool>>)((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => (a + b.ToString() + new string(l, 10) + n.FriendlyName).Length + i * d - e.Year % j + f * g / h.Days + k.ToByteArray().Length + m * p[c] > 0 && o[c] < 12.45m),
            })
            {
                foreach (var pack in packs)
                {
                    var g = pack(f);

                    var n = g.Parameters.Count;
                    Assert.IsTrue(n is 0 or 1, "Parameter count: " + f.ToString());

                    if (n == 1)
                    {
                        Assert.IsTrue(g.Parameters[0].Type.FullName.StartsWith("System.Tuple`"), "Parameter type: " + f.ToString());
                    }

                    var h = ExpressionTupletizer.Unpack(g);

                    var res = new ExpressionEqualityComparer().Equals(f, h);
                    Assert.IsTrue(res, "Equality: " + f.ToString());
                }
            }
        }

        [TestMethod]
        public void ExpressionTupletizer_PackAndUnpack_Lambda_Void()
        {
#if USE_SLIM
            var packs = new Func<LambdaExpression, LambdaExpression>[]
            {
                f => (LambdaExpression)ExpressionSlimTupletizer.Pack((LambdaExpressionSlim)f.ToExpressionSlim(), typeof(object).ToTypeSlim()).ToExpression(),
                f => { var s = (LambdaExpressionSlim)f.ToExpressionSlim(); return (LambdaExpression)ExpressionSlimTupletizer.Pack(s.Body, s.Parameters, typeof(object).ToTypeSlim()).ToExpression(); },
            };
#else
            var packs = new Func<LambdaExpression, LambdaExpression>[]
            {
                f => ExpressionTupletizer.Pack(f, typeof(object)),
                f => ExpressionTupletizer.Pack(f.Body, f.Parameters, typeof(object)),
            };
#endif

            foreach (var f in new LambdaExpression[] {
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
                (Expression<Func<int>>)(() => 42),
#pragma warning restore IDE0004 // Remove Unnecessary Cast
                (Expression<Func<string, int>>)(s => s.Length),
                (Expression<Func<string, int, int>>)((s, i) => s.Length + i),
                (Expression<Func<int, int, int, int>>)((a, b, c) => a * b + c),
                (Expression<Func<int, int, int, int, int>>)((a, b, c, d) => a * b + c - d),
                (Expression<Func<int, int, int, int, int, int>>)((a, b, c, d, e) => a * b + c - d / e),
                (Expression<Func<int, int, int, int, int, int, int>>)((a, b, c, d, e, f) => a * b + c - d / e + f),
                (Expression<Func<int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g) => a * b + c - d / e + f * g),
                (Expression<Func<int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h) => a * b + c - d / e + f * g / h),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i) => a * b + c - d / e + f * g / h + i),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j) => a * b + c - d / e + f * g / h + i - j),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k) => a * b + c - d / e + f * g / h + i - j * k),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k, l) => a * b + c - d / e + f * g / h + i - j * k / l),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k, l, m) => a * b + c - d / e + f * g / h + i - j * k / l + m),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a * b + c - d / e + f * g / h + i - j * k / l + m % n),
                (Expression<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a * b + c - d / e + f * g / h + i - j * k / l + m % n + o),
                (Expression<Func<string, Uri, int, double, DateTime, float, byte, TimeSpan, long, short, Guid, char, uint, AppDomain, decimal[], List<int>, bool>>)((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => (a + b.ToString() + new string(l, 10) + n.FriendlyName).Length + i * d - e.Year % j + f * g / h.Days + k.ToByteArray().Length + m * p[c] > 0 && o[c] < 12.45m),
            })
            {
                foreach (var pack in packs)
                {
                    var g = pack(f);

                    var n = g.Parameters.Count;
                    Assert.IsTrue(n == 1, "Parameter count: " + f.ToString());

                    if (n == 1)
                    {
                        var p = g.Parameters[0];

                        if (f.Parameters.Count == 0)
                        {
                            Assert.AreEqual(typeof(object), p.Type, "Void type");
                        }
                        else
                        {
                            Assert.IsTrue(p.Type.FullName.StartsWith("System.Tuple`"), "Parameter type: " + f.ToString());
                        }
                    }

                    var h = ExpressionTupletizer.Unpack(g, typeof(object));

                    var res = new ExpressionEqualityComparer().Equals(f, h);
                    Assert.IsTrue(res, "Equality: " + f.ToString());
                }
            }
        }

        [TestMethod]
        public void ExpressionTupletizer_PackAndUnpack_Lambda_Random1()
        {
            var g = (Expression<Func<IQueryable<int>, Expression<Func<int, bool>>, IQueryable<string>>>)((xs, f) => xs.Where(f).Select(x => x.ToString()));

            var h = Unpack(Pack(g));

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(g, h));
        }

        [TestMethod]
        public void ExpressionTupletizer_PackAndUnpack_Lambda_Random2()
        {
            var g = (Expression<Func<IQueryable<Tuple<int, int>>, IQueryable<int>>>)(xs => xs.Where(t => t.Item1 > t.Item2).Select(t => t.Item1 * t.Item2));

            var h = Unpack(Pack(g));

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(g, h));
        }

        [TestMethod]
        public void ExpressionTupletizer_PackAndUnpack_Lambda_Random3()
        {
            var g = (Expression<Func<int, Func<int, int>>>)(x => y => x * y);

            var h = Unpack(Pack(g));

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(g, h));
        }

        [TestMethod]
        public void ExpressionTupletizer_PackAndUnpack_Lambda_Random4()
        {
            var g = (Expression<Func<int, Func<int, int>>>)(x => y => x * y);

            var h = Unpack(Pack((LambdaExpression)g.Body));

            Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator()).Equals(g.Body, h));
        }

        [TestMethod]
        public void ExpressionTupletizer_PackAndUnpack_Lambda_Errors()
        {
            Assert.ThrowsException<InvalidOperationException>(() => Unpack((Expression<Func<int, int>>)(x => x)));
            Assert.ThrowsException<InvalidOperationException>(() => Unpack((Expression<Func<Tuple<int>, int, int>>)((t, x) => x)));
            Assert.ThrowsException<InvalidOperationException>(() => Unpack((Expression<Func<object, object>>)(x => x), typeof(object)));
        }

        [TestMethod]
        public void ExpressionTupletizer_Unpack_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => Unpack(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => Unpack(default(LambdaExpression)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Unpack(default(LambdaExpression), typeof(int)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Unpack((Expression<Func<int, int>>)(x => x), default(Type)), ex => Assert.AreEqual("voidType", ex.ParamName));

            var body = default(Expression);
            var parameters = default(IEnumerable<ParameterExpression>);
            AssertEx.ThrowsException<ArgumentNullException>(() => Unpack(default(LambdaExpression), out body, out parameters), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Unpack(default(LambdaExpression), typeof(int), out body, out parameters), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => Unpack((Expression<Func<int, int>>)(x => x), default(Type), out body, out parameters), ex => Assert.AreEqual("voidType", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ExpressionTupletizer_Pack_Unpack_Roundtrip()
        {
            for (int i = 1; i < 18; i++)
            {
                var t = Enumerable.Range(1, i).Select(x => Expression.Constant(x)).ToArray();
                var e = Pack(t);
                var u = Unpack(e);
                Assert.IsTrue(t.SequenceEqual(u, new ExpressionEqualityComparer()));
            }
        }

        [TestMethod]
        public void ExpressionTupletizer_Unpack_Errors()
        {
            Assert.ThrowsException<InvalidOperationException>(() => Unpack(Expression.Constant(1)));
            Assert.ThrowsException<InvalidOperationException>(() => Unpack(Expression.New(typeof(TupleBar))));
            Assert.ThrowsException<InvalidOperationException>(() => Unpack(Expression.New(typeof(List<int>))));
            Assert.ThrowsException<InvalidOperationException>(() => Unpack(((Expression<Func<Tuple<int, int, int, int, int, int, int, int>>>)(() => new Tuple<int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8))).Body));
        }

        [TestMethod]
        public void ExpressionTupletizer_Unpack_NoChange()
        {
            var expression = Expression.Lambda(Expression.Default(typeof(int)));

            Unpack(expression, out var body, out var parameters);
#if !USE_SLIM
            Assert.AreSame(expression.Body, body);
            Assert.AreSame(expression.Parameters, parameters);
#else
            Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator()).Equals(expression.Body, body));
            Assert.AreEqual(0, parameters.Count());
#endif

            Unpack(expression, typeof(void), out body, out parameters);
#if !USE_SLIM
            Assert.AreSame(expression.Body, body);
            Assert.AreSame(expression.Parameters, parameters);
#else
            Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator()).Equals(expression.Body, body));
            Assert.AreEqual(0, parameters.Count());
#endif
        }

        [TestMethod]
        public void ExpressionTupletizer_ManOrBoy()
        {
            var es = new Expression[]
            {
                Expression.Constant(1),
                Expression.Constant(false),
                (Expression<Func<int, bool>>)(x => x % 2 == 0),
                Expression.Parameter(typeof(double)),
                ((Expression<Func<Tuple<string, decimal>>>)(() => new Tuple<string, decimal>("bar", 12.34m))).Body,
                Expression.Constant(value: null),
                (from x in new[] { 1, 2, 3 }.AsQueryable() where x > 0 select x * x).Expression,
                Expression.Constant(2),
                Expression.Constant(3),
                Expression.Constant(4),
            };

            var p = Pack(es);
            var u = Unpack(p);

            Assert.IsTrue(es.SequenceEqual(u, new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator())));
        }

        [TestMethod]
        public void ExpressionTupletizer_VoidShallNotWork()
        {
            AssertEx.ThrowsException<ArgumentException>(() => Pack(new[] { ((Expression<Action<string>>)(s => Console.WriteLine(s))).Body }), ex =>
            {
                Assert.AreEqual("expressions", ex.ParamName);
                Assert.IsTrue(ex.Message.Contains("WriteLine"));
            });
        }

        [TestMethod]
        public void ExpressionTupletizer_IsTuple()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionTupletizer.IsTuple(type: null), ex => Assert.AreEqual("type", ex.ParamName));

            foreach (var t in new[] {
                typeof(Tuple<int>),
                typeof(Tuple<int, int>),
                typeof(Tuple<int, int, int>),
                typeof(Tuple<int, int, int, int>),
                typeof(Tuple<int, int, int, int, int>),
                typeof(Tuple<int, int, int, int, int, int>),
                typeof(Tuple<int, int, int, int, int, int, int>),
                typeof(Tuple<int, int, int, int, int, int, int, Tuple<int>>),
                typeof(Tuple<int, int, int, int, int, int, int, Tuple<int, int>>),
                typeof(Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int>>>),
            })
            {
                Assert.IsTrue(IsTuple(t));
            }

            foreach (var t in new[] {
                typeof(int),
                typeof(List<int>),
                typeof(Tuple<>),
                typeof(Tuple<,>),
                typeof(Tuple<int, int, int, int, int, int, int, /*TRest*/ int>),
            })
            {
                Assert.IsFalse(IsTuple(t));
            }
        }

        private sealed class TupleBar
        {
        }

        private sealed class GlobalParameterSafeComparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }

#if USE_SLIM
        private static LambdaExpression Pack(LambdaExpression expression)
        {
            var slimLambda = expression != null ? (LambdaExpressionSlim)expression.ToExpressionSlim() : null;
            return (LambdaExpression)ExpressionSlimTupletizer.Pack(slimLambda).ToExpression();
        }

        private static LambdaExpression Pack(LambdaExpression expression, Type voidType)
        {
            var slimLambda = expression != null ? (LambdaExpressionSlim)expression.ToExpressionSlim() : null;
            var slimVoidType = voidType?.ToTypeSlim();
            return (LambdaExpression)ExpressionSlimTupletizer.Pack(slimLambda, slimVoidType).ToExpression();
        }

        private static Expression Pack(IEnumerable<Expression> expressions)
        {
            var slimExpressions = expressions?.Select(e => e.ToExpressionSlim());
            return ExpressionSlimTupletizer.Pack(slimExpressions).ToExpression();
        }

        private static LambdaExpression Unpack(LambdaExpression expression)
        {
            var slimLambda = expression != null ? (LambdaExpressionSlim)expression.ToExpressionSlim() : null;
            return (LambdaExpression)ExpressionSlimTupletizer.Unpack(slimLambda).ToExpression();
        }

        private static LambdaExpression Unpack(LambdaExpression expression, Type voidType)
        {
            var slimLambda = expression != null ? (LambdaExpressionSlim)expression.ToExpressionSlim() : null;
            var slimVoidType = voidType?.ToTypeSlim();
            return (LambdaExpression)ExpressionSlimTupletizer.Unpack(slimLambda, slimVoidType).ToExpression();
        }

        private static IEnumerable<Expression> Unpack(Expression expression)
        {
            var slimExpression = expression?.ToExpressionSlim();
            return ExpressionSlimTupletizer.Unpack(slimExpression).Select(x => x.ToExpression());
        }

        private static void Unpack(LambdaExpression expression, out Expression body, out IEnumerable<ParameterExpression> parameters)
        {
            var slimLambda = expression != null ? (LambdaExpressionSlim)expression.ToExpressionSlim() : null;
            ExpressionSlimTupletizer.Unpack(slimLambda, out var slimBody, out var slimParameters);
            var fatLambda = (LambdaExpression)ExpressionSlim.Lambda(slimBody, slimParameters).ToExpression();
            body = fatLambda.Body;
            parameters = fatLambda.Parameters;
        }

        private static void Unpack(LambdaExpression expression, Type voidType, out Expression body, out IEnumerable<ParameterExpression> parameters)
        {
            var slimLambda = expression != null ? (LambdaExpressionSlim)expression.ToExpressionSlim() : null;
            var slimVoidType = voidType?.ToTypeSlim();
            ExpressionSlimTupletizer.Unpack(slimLambda, slimVoidType, out var slimBody, out var slimParameters);
            var fatLambda = (LambdaExpression)ExpressionSlim.Lambda(slimBody, slimParameters).ToExpression();
            body = fatLambda.Body;
            parameters = fatLambda.Parameters;
        }

        private static bool IsTuple(Type type)
        {
            return ExpressionSlimTupletizer.IsTuple(type.ToTypeSlim());
        }

        private static Expression Pack(Expression expression, params ParameterExpression[] parameters)
        {
            var slimExpression = expression?.ToExpressionSlim();
            var slimParameters = parameters?.Select(p => (ParameterExpressionSlim)p.ToExpressionSlim()).ToArray();
            ExpressionSlimTupletizer.Pack(slimExpression, slimParameters);

            // This helper should only be used for argument checks...
            throw new Exception();
        }

        private static Expression Pack(Expression expression, IEnumerable<ParameterExpression> parameters, Type voidType)
        {
            var slimExpression = expression?.ToExpressionSlim();
            var slimParameters = parameters?.Select(p => (ParameterExpressionSlim)p.ToExpressionSlim()).ToArray();
            var slimVoidType = voidType?.ToTypeSlim();
            ExpressionSlimTupletizer.Pack(slimExpression, slimParameters, slimVoidType);

            // This helper should only be used for argument checks...
            throw new Exception();
        }
#else
        private static LambdaExpression Pack(LambdaExpression expression)
        {
            return ExpressionTupletizer.Pack(expression);
        }

        private static LambdaExpression Pack(LambdaExpression expression, Type voidType)
        {
            return ExpressionTupletizer.Pack(expression, voidType);
        }

        private static Expression Pack(IEnumerable<Expression> expressions)
        {
            return ExpressionTupletizer.Pack(expressions);
        }

        private static LambdaExpression Unpack(LambdaExpression expression)
        {
            return ExpressionTupletizer.Unpack(expression);
        }

        private static LambdaExpression Unpack(LambdaExpression expression, Type voidType)
        {
            return ExpressionTupletizer.Unpack(expression, voidType);
        }

        private static IEnumerable<Expression> Unpack(Expression expression)
        {
            return ExpressionTupletizer.Unpack(expression);
        }

        private static void Unpack(LambdaExpression expression, out Expression body, out IEnumerable<ParameterExpression> parameters)
        {
            ExpressionTupletizer.Unpack(expression, out body, out parameters);
        }

        private static void Unpack(LambdaExpression expression, Type voidType, out Expression body, out IEnumerable<ParameterExpression> parameters)
        {
            ExpressionTupletizer.Unpack(expression, voidType, out body, out parameters);
        }

        private static bool IsTuple(Type type)
        {
            return ExpressionTupletizer.IsTuple(type);
        }

        private static Expression Pack(Expression expression, params ParameterExpression[] parameters)
        {
            return ExpressionTupletizer.Pack(expression, parameters);
        }

        private static Expression Pack(Expression expression, IEnumerable<ParameterExpression> parameters, Type voidType)
        {
            return ExpressionTupletizer.Pack(expression, parameters, voidType);
        }
#endif
    }
}
