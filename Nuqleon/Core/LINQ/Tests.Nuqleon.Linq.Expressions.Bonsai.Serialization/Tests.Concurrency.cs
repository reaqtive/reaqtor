// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    public partial class Tests
    {
        [TestMethod]
        public void Bonsai_Concurrency()
        {
            var p = Expression.Parameter(typeof(int), "p");
            var p1 = Expression.Parameter(typeof(int), "p1");
            var p2 = Expression.Parameter(typeof(int), "p2");
            var lp1 = Expression.Parameter(typeof(MyNumeric?), "p1");
            var lp2 = Expression.Parameter(typeof(MyNumeric?), "p2");
            var bp1 = Expression.Parameter(typeof(bool), "p1");
            var bp2 = Expression.Parameter(typeof(bool), "p2");
            var c1 = Expression.Constant(DateTime.Now);
            var c2 = Expression.Constant(TimeSpan.FromSeconds(1));

            var baseCases = new Expression[]
            {
                #region Constant
                Expression.Constant((sbyte)42),
                Expression.Constant((byte)42),
                Expression.Constant((short)42),
                Expression.Constant((ushort)42),
                Expression.Constant(42),
                Expression.Constant((uint)42),
                Expression.Constant((long)42),
                Expression.Constant((ulong)42),
                Expression.Constant((float)42),
                Expression.Constant((double)42),
                //Not supported: typeof(Decimal)
                //Issue: inadequate constant converter
                //Expression.Constant((decimal)42),
                Expression.Constant(true),
                Expression.Constant("Hello"),
                //Not supported: typeof(Char)
                //Issue: inadequate constant converter
                //Expression.Constant('c'),
                #endregion

                #region Unary
                Expression.Negate(p),
                Expression.NegateChecked(p),
                Expression.UnaryPlus(p),
                Expression.OnesComplement(p),
                Expression.Decrement(p),
                Expression.Increment(p),
                Expression.Not(p),
                Expression.ArrayLength(Expression.Parameter(typeof(int[]))),
                Expression.TypeAs(p, typeof(string)),
                Expression.MakeUnary(ExpressionType.Increment, p, type: null, typeof(CustomUnary).GetMethod("Increment")),
                Expression.MakeUnary(ExpressionType.UnaryPlus, p, type: null, typeof(CustomUnary).GetMethod("Plus")),
                Expression.MakeUnary(ExpressionType.Negate, p, type: null, typeof(CustomUnary).GetMethod("Negate")),
                Expression.MakeUnary(ExpressionType.NegateChecked, p, type: null, typeof(CustomUnary).GetMethod("Negate")),
                #endregion

                #region Binary
                Expression.Add(p1, p2),
                Expression.AddChecked(p1, p2),
                Expression.Subtract(p1, p2),
                Expression.SubtractChecked(p1, p2),
                Expression.Multiply(p1, p2),
                Expression.MultiplyChecked(p1, p2),
                Expression.Divide(p1, p2),
                Expression.Modulo(p1, p2),

                Expression.LeftShift(p1, p2),
                Expression.RightShift(p1, p2),

                Expression.And(p1, p2),
                Expression.Or(p1, p2),
                Expression.ExclusiveOr(p1, p2),
                Expression.Power(Expression.Parameter(typeof(double)), Expression.Parameter(typeof(double))),

                Expression.Add(lp1, lp2),
                Expression.AddChecked(lp1, lp2),
                Expression.Subtract(lp1, lp2),
                Expression.SubtractChecked(lp1, lp2),
                Expression.Multiply(lp1, lp2),
                Expression.MultiplyChecked(lp1, lp2),
                Expression.Divide(lp1, lp2),
                Expression.Modulo(lp1, lp2),

                Expression.And(lp1, lp2),
                Expression.Or(lp1, lp2),
                Expression.ExclusiveOr(lp1, lp2),
                Expression.Add(c1, c2),
                Expression.AddChecked(c1, c2),
                Expression.Subtract(c1, c2),
                Expression.SubtractChecked(c1, c2),

                Expression.LessThan(p1, p2),
                Expression.LessThanOrEqual(p1, p2),
                Expression.GreaterThan(p1, p2),
                Expression.GreaterThanOrEqual(p1, p2),
                Expression.Equal(p1, p2),
                Expression.NotEqual(p1, p2),

                Expression.And(bp1, bp2),
                Expression.Or(bp1, bp2),
                Expression.ExclusiveOr(bp1, bp2),
                Expression.AndAlso(bp1, bp2),
                Expression.OrElse(bp1, bp2),

                (Expression<Func<Persoon2, bool>>)(x => x.Geslacht == Sex.Male),
                (Expression<Func<string, string>>)(s => s ?? "null"),
                Expression.Coalesce(Expression.Parameter(typeof(string)), Expression.Constant(0), (Expression<Func<string, int>>)(s => int.Parse(s))),
                #endregion

                #region Conversion
                (Expression<Func<int, BigInteger>>)(i => i),
                (Expression<Func<BigInteger, int>>)(i => (int)i),
                Expression.ConvertChecked(Expression.Parameter(typeof(BigInteger)), typeof(int)),
                #endregion

                #region Lambda
                Expression.Lambda<Func<int, int>>(p, p),
                (Expression<Action<Nopper>>)(n => n.Nop()),
                Expression.Lambda<MyFunc>(p, p),
                Expression.Parameter(typeof(int), "p").Let(x => Expression.Lambda<Func<int, Func<int, int>>>(Expression.Lambda(p, p), x)),
                Expression.Parameter(typeof(int)).Let(x => Expression.Lambda<Func<int, int>>(x, x)),
                (Expression<Rec<int, int>>)(fib => n => n > 1 ? fib(fib)(n - 1) + fib(fib)(n - 2) : n),
                (Expression<Func<int>>)(() => Q.Foo(() => new Foo())),
                #endregion

                #region Calls
                Expression.Call(instance: null, typeof(X).GetMethod("A")),
                Expression.Call(instance: null, typeof(X).GetMethod("B"), Expression.Constant(2)),
                Expression.Call(instance: null, typeof(X).GetMethod("C")),
                Expression.Call(instance: null, typeof(X).GetMethod("D"), Expression.Constant(42)),
                Expression.Call(instance: null, typeof(X).GetMethod("I").MakeGenericMethod(typeof(int)), Expression.Constant(42)),
                (Expression<Action<Adder, int>>)((a, x) => a.Add(x)),
                (Expression<Action<Adder, string>>)((a, x) => a.Add(x)),
                (Expression<Action<Adder, Answer>>)((a, x) => a.Add(x)),
                (Expression<Func<Adder, int, int>>)((a, x) => a.AddAndRet(x)),
                #endregion

                #region Invocation
                (Expression<Func<Func<int, int>, int, int>>)((f, x) => f(x)),
                #endregion

                #region Members
                (Expression<Func<Instancy, string>>)(i => i.Value),
                (Expression<Func<Instancy, string>>)(i => i.Value),
                (Expression<Func<Instancy, int>>)(i => i._field),
                (Expression<Func<string>>)(() => Staticy.Value),
                (Expression<Func<int>>)(() => Staticy._field),
                (Expression<Func<string[], int, string>>)((ss, i) => /* ArrayIndex */ ss[i]),
                #endregion

                #region Indexer
                (Expression<Func<string[,], int, int, string>>)((ss, i, j) => ss[i, j]),
                (Expression<Func<Indexy, string, int>>)((i, s) => i[s]),
                #endregion

                #region Default
                Expression.Default(typeof(string)),
                #endregion

                #region Conditional
                Expression.Default(typeof(string)),
                Expression.Condition(Expression.Constant(true, typeof(bool)), Expression.New(typeof(B)), Expression.New(typeof(C)), typeof(A)),
                #endregion

                #region Object creation and initialization
                (Expression<Func<int[]>>)(() => new[] { 1, 2, 3 }),
                (Expression<Func<List<int>>>)(() => new List<int> { 1, 2, 3 }),
                (Expression<Func<Qux>>)(() => new Qux(42) { Baz = "Hello", Bar = { Zuz = 24 }, Foos = { 1, 2, 3 } }),
                Expression.Lambda(Expression.New(typeof(int))),
                Expression.NewArrayBounds(typeof(int), Expression.Constant(1)),
                #endregion

                #region Man or Boy
                (Expression<Func<List<int>, List<int>, List<int>, List<int>>>)((x, y, z) => Operators.Concat(Operators.Concat(x, y, z), x, 0)),
                #endregion
            };

            var r = new Random(42);
            var testCases = Enumerable.Repeat(baseCases, 1000).SelectMany(x => x).OrderBy(_ => r.Next());

            var serializer = GetSerializer();
            Parallel.ForEach(
                testCases,
                testCase => AssertRoundtrip(testCase, serializer));
        }

        private static void AssertRoundtrip(Expression expected, IExpressionSerializer serializer)
        {
            var actual = Roundtrip(expected, serializer);
            var comparer = new ExpressionEqualityComparer(() => new SimpleComparator());
            Assert.IsTrue(comparer.Equals(expected, actual), "Expected: {0}\nActual: {1}", expected.ToString(), actual.ToString());
        }

        private static Expression Roundtrip(Expression expr, IExpressionSerializer serializer)
        {
            var slim = serializer.Lift(expr);
            var bonsai = serializer.Serialize(slim);
            var roundtrippedSlim = serializer.Deserialize(bonsai);
            return serializer.Reduce(roundtrippedSlim);
        }

        private static IExpressionSerializer GetSerializer() => new SimpleExpressionSerializer();

        private sealed class SimpleComparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }
    }
}
