// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void Unary_Algebraic_IsFalse_Boolean()
        {
            var b = Expression.Parameter(typeof(bool));

            var e =
                Expression.IsFalse(b);

            var r =
                Expression.Not(b);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_IsFalse_NullableBoolean()
        {
            var b = Expression.Parameter(typeof(bool?));

            var e =
                Expression.IsFalse(b);

            var r =
                Expression.Not(b);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_IsFalse_Method()
        {
            var m = typeof(ExpressionOptimizerTests).GetMethod(nameof(UnaryBoolMethod));

            var b = Expression.Parameter(typeof(bool));

            var e =
                Expression.IsFalse(b, m);

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Algebraic_IsTrue_Boolean()
        {
            var b = Expression.Parameter(typeof(bool));

            var e =
                Expression.IsTrue(b);

            var r =
                b;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_IsTrue_NullableBoolean()
        {
            var b = Expression.Parameter(typeof(bool?));

            var e =
                Expression.IsTrue(b);

            var r =
                b;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_IsTrue_Method()
        {
            var m = typeof(ExpressionOptimizerTests).GetMethod(nameof(UnaryBoolMethod));

            var b = Expression.Parameter(typeof(bool));

            var e =
                Expression.IsTrue(b, m);

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Algebraic_Not_IsFalse()
        {
            var b = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.IsFalse(b)
                );

            var r =
                b;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_Not_Not_Boolean()
        {
            var b = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.Not(b)
                );

            var r =
                b;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_Not_Not_NullableBoolean()
        {
            var b = Expression.Parameter(typeof(bool?));

            var e =
                Expression.Not(
                    Expression.Not(b)
                );

            var r =
                b;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_Not_Not_Boolean_Method1()
        {
            var m = typeof(ExpressionOptimizerTests).GetMethod(nameof(UnaryBoolMethod));

            var b = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.Not(b, m)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Algebraic_Not_Not_Boolean_Method2()
        {
            var m = typeof(ExpressionOptimizerTests).GetMethod(nameof(UnaryBoolMethod));

            var b = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.Not(b),
                    m
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Algebraic_UnaryPlus()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.UnaryPlus(
                    x
                );

            var r =
                x;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_Law_Byte()
        {
            var x = Expression.Parameter(typeof(byte));
            var o = Expression.OnesComplement(Expression.OnesComplement(x));
            var f = Expression.Lambda<Func<byte, byte>>(o, x);
            var d = f.Compile();

            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                var b = (byte)i;

                Assert.AreEqual(b, d(b));
            }
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_Law_SByte()
        {
            var x = Expression.Parameter(typeof(sbyte));
            var o = Expression.OnesComplement(Expression.OnesComplement(x));
            var f = Expression.Lambda<Func<sbyte, sbyte>>(o, x);
            var d = f.Compile();

            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                var b = (sbyte)i;

                Assert.AreEqual(b, d(b));
            }
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_Law_Int16()
        {
            var x = Expression.Parameter(typeof(short));
            var o = Expression.OnesComplement(Expression.OnesComplement(x));
            var f = Expression.Lambda<Func<short, short>>(o, x);
            var d = f.Compile();

            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                var b = (short)i;

                Assert.AreEqual(b, d(b));
            }
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_Law_UInt16()
        {
            var x = Expression.Parameter(typeof(ushort));
            var o = Expression.OnesComplement(Expression.OnesComplement(x));
            var f = Expression.Lambda<Func<ushort, ushort>>(o, x);
            var d = f.Compile();

            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                var b = (ushort)i;

                Assert.AreEqual(b, d(b));
            }
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_Law_Int32()
        {
            var x = Expression.Parameter(typeof(int));
            var o = Expression.OnesComplement(Expression.OnesComplement(x));
            var f = Expression.Lambda<Func<int, int>>(o, x);
            var d = f.Compile();

            for (long i = int.MinValue; i <= int.MinValue + 128; i++)
            {
                var b = (int)i;

                Assert.AreEqual(b, d(b));
            }

            for (long i = -128; i <= 128; i++)
            {
                var b = (int)i;

                Assert.AreEqual(b, d(b));
            }

            for (long i = int.MaxValue - 128; i <= int.MaxValue; i++)
            {
                var b = (int)i;

                Assert.AreEqual(b, d(b));
            }
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_Law_UInt32()
        {
            var x = Expression.Parameter(typeof(uint));
            var o = Expression.OnesComplement(Expression.OnesComplement(x));
            var f = Expression.Lambda<Func<uint, uint>>(o, x);
            var d = f.Compile();

            for (ulong i = uint.MinValue; i <= uint.MinValue + 128; i++)
            {
                var b = (uint)i;

                Assert.AreEqual(b, d(b));
            }

            for (ulong i = uint.MaxValue - 128; i <= uint.MaxValue; i++)
            {
                var b = (uint)i;

                Assert.AreEqual(b, d(b));
            }
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_Law_Int64()
        {
            var x = Expression.Parameter(typeof(long));
            var o = Expression.OnesComplement(Expression.OnesComplement(x));
            var f = Expression.Lambda<Func<long, long>>(o, x);
            var d = f.Compile();

            for (long i = long.MinValue; i <= long.MinValue + 128; i++)
            {
                Assert.AreEqual(i, d(i));
            }

            for (long i = -128; i <= 128; i++)
            {
                Assert.AreEqual(i, d(i));
            }

            for (long i = long.MaxValue - 128; i < long.MaxValue; i++)
            {
                Assert.AreEqual(i, d(i));
            }

            Assert.AreEqual(long.MaxValue, d(long.MaxValue));
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_Law_UInt64()
        {
            var x = Expression.Parameter(typeof(ulong));
            var o = Expression.OnesComplement(Expression.OnesComplement(x));
            var f = Expression.Lambda<Func<ulong, ulong>>(o, x);
            var d = f.Compile();

            for (ulong i = ulong.MinValue; i <= ulong.MinValue + 128; i++)
            {
                Assert.AreEqual(i, d(i));
            }

            for (ulong i = ulong.MaxValue - 128; i < ulong.MaxValue; i++)
            {
                Assert.AreEqual(i, d(i));
            }

            Assert.AreEqual(ulong.MaxValue, d(ulong.MaxValue));
        }

        [TestMethod]
        public void Unary_Algebraic_Not_Int32()
        {
            var i = Expression.Parameter(typeof(int));

            var e =
                Expression.Not(i);

            // REVIEW: This merely substitutes Not for OnesComplement for better intuition and to
            //         unblock "double ones complement" optimization.

            var r =
                Expression.OnesComplement(i);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_OnesComplement_Int32()
        {
            var i = Expression.Parameter(typeof(int));

            var e =
                Expression.OnesComplement(
                    Expression.OnesComplement(i)
                );

            var r =
                i;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_OnesComplement_NullableInt32()
        {
            var i = Expression.Parameter(typeof(int?));

            var e =
                Expression.OnesComplement(
                    Expression.OnesComplement(i)
                );

            var r =
                i;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_OnesComplement_Not_Int32()
        {
            var i = Expression.Parameter(typeof(int));

            var e =
                Expression.OnesComplement(
                    Expression.Not(i)
                );

            var r =
                i;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_Not_OnesComplement_Int32()
        {
            var i = Expression.Parameter(typeof(int));

            var e =
                Expression.Not(
                    Expression.OnesComplement(i)
                );

            var r =
                i;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_Negate_Law_Int16()
        {
            var x = Expression.Parameter(typeof(short));
            var o = Expression.Negate(Expression.Negate(x));
            var f = Expression.Lambda<Func<short, short>>(o, x);
            var d = f.Compile();

            for (int i = short.MinValue; i <= short.MaxValue; i++)
            {
                var b = (short)i;

                Assert.AreEqual(b, d(b));
            }
        }

        [TestMethod]
        public void Unary_Algebraic_Negate_Law_Int32()
        {
            var x = Expression.Parameter(typeof(int));
            var o = Expression.Negate(Expression.Negate(x));
            var f = Expression.Lambda<Func<int, int>>(o, x);
            var d = f.Compile();

            for (long i = int.MinValue; i <= int.MinValue + 128; i++)
            {
                var b = (int)i;

                Assert.AreEqual(b, d(b));
            }

            for (long i = -128; i <= 128; i++)
            {
                var b = (int)i;

                Assert.AreEqual(b, d(b));
            }

            for (long i = int.MaxValue - 128; i <= int.MaxValue; i++)
            {
                var b = (int)i;

                Assert.AreEqual(b, d(b));
            }
        }

        [TestMethod]
        public void Unary_Algebraic_Negate_Law_Int64()
        {
            var x = Expression.Parameter(typeof(long));
            var o = Expression.Negate(Expression.Negate(x));
            var f = Expression.Lambda<Func<long, long>>(o, x);
            var d = f.Compile();

            for (long i = long.MinValue; i <= long.MinValue + 128; i++)
            {
                Assert.AreEqual(i, d(i));
            }

            for (long i = -128; i <= 128; i++)
            {
                Assert.AreEqual(i, d(i));
            }

            for (long i = long.MaxValue - 128; i < long.MaxValue; i++)
            {
                Assert.AreEqual(i, d(i));
            }

            Assert.AreEqual(long.MaxValue, d(long.MaxValue));
        }

        [TestMethod]
        public void Unary_Algebraic_Negate_Negate_Int32()
        {
            var i = Expression.Parameter(typeof(int));

            var e =
                Expression.Negate(
                    Expression.Negate(i)
                );

            var r =
                i;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_And_Boolean()
        {
            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.And(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var r =
                Expression.Or(
                    x,
                    y
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_Or_Boolean()
        {
            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.Or(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var r =
                Expression.And(
                    x,
                    y
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_AndAlso_Boolean()
        {
            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.AndAlso(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var r =
                Expression.OrElse(
                    x,
                    y
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_OrElse_Boolean()
        {
            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.OrElse(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var r =
                Expression.AndAlso(
                    x,
                    y
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_And_NullableBoolean()
        {
            var x = Expression.Parameter(typeof(bool?));
            var y = Expression.Parameter(typeof(bool?));

            var e =
                Expression.Not(
                    Expression.And(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var r =
                Expression.Or(
                    x,
                    y
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_Or_NullableBoolean()
        {
            var x = Expression.Parameter(typeof(bool?));
            var y = Expression.Parameter(typeof(bool?));

            var e =
                Expression.Not(
                    Expression.Or(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var r =
                Expression.And(
                    x,
                    y
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_AndAlso_NullableBoolean()
        {
            var x = Expression.Parameter(typeof(bool?));
            var y = Expression.Parameter(typeof(bool?));

            var e =
                Expression.Not(
                    Expression.AndAlso(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var r =
                Expression.OrElse(
                    x,
                    y
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_OrElse_NullableBoolean()
        {
            var x = Expression.Parameter(typeof(bool?));
            var y = Expression.Parameter(typeof(bool?));

            var e =
                Expression.Not(
                    Expression.OrElse(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var r =
                Expression.AndAlso(
                    x,
                    y
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_And_Nop1()
        {
            var u = typeof(ExpressionOptimizerTests).GetMethod(nameof(UnaryBoolMethod));

            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.And(
                        Expression.Not(x),
                        Expression.Not(y)
                    ),
                    u
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_And_Nop2()
        {
            var u = typeof(ExpressionOptimizerTests).GetMethod(nameof(UnaryBoolMethod));

            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.And(
                        Expression.Not(x, u),
                        Expression.Not(y)
                    )
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_And_Nop3()
        {
            var u = typeof(ExpressionOptimizerTests).GetMethod(nameof(UnaryBoolMethod));

            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.And(
                        Expression.Not(x),
                        Expression.Not(y, u)
                    )
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_And_Nop4()
        {
            var b = typeof(ExpressionOptimizerTests).GetMethod(nameof(BinaryBoolMethod));

            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.And(
                        Expression.Not(x),
                        Expression.Not(y),
                        b
                    )
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_Xor()
        {
            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.ExclusiveOr(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var r =
                Expression.Not(
                    Expression.ExclusiveOr(
                        x,
                        y
                    )
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_And_Law_Boolean()
        {
            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.And(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var f = Expression.Lambda<Func<bool, bool, bool>>(e, x, y);
            var o = GetOptimizer().VisitAndConvert(f, "Test");

            var fun = f.Compile();
            var opt = o.Compile();

            foreach (var b1 in new[] { false, true })
            {
                foreach (var b2 in new[] { false, true })
                {
                    Assert.AreEqual(opt(b1, b2), fun(b1, b2));
                }
            }
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_Or_Law_Boolean()
        {
            var x = Expression.Parameter(typeof(bool));
            var y = Expression.Parameter(typeof(bool));

            var e =
                Expression.Not(
                    Expression.Or(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var f = Expression.Lambda<Func<bool, bool, bool>>(e, x, y);
            var o = GetOptimizer().VisitAndConvert(f, "Test");

            var fun = f.Compile();
            var opt = o.Compile();

            foreach (var b1 in new[] { false, true })
            {
                foreach (var b2 in new[] { false, true })
                {
                    Assert.AreEqual(opt(b1, b2), fun(b1, b2));
                }
            }
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_And_Law_NullableBoolean()
        {
            var x = Expression.Parameter(typeof(bool?));
            var y = Expression.Parameter(typeof(bool?));

            var e =
                Expression.Not(
                    Expression.And(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var f = Expression.Lambda<Func<bool?, bool?, bool?>>(e, x, y);
            var o = GetOptimizer().VisitAndConvert(f, "Test");

            var fun = f.Compile();
            var opt = o.Compile();

            foreach (var b1 in new bool?[] { null, false, true })
            {
                foreach (var b2 in new bool?[] { null, false, true })
                {
                    Assert.AreEqual(opt(b1, b2), fun(b1, b2));
                }
            }
        }

        [TestMethod]
        public void Unary_Algebraic_DeMorgan_Or_Law_NullableBoolean()
        {
            var x = Expression.Parameter(typeof(bool?));
            var y = Expression.Parameter(typeof(bool?));

            var e =
                Expression.Not(
                    Expression.Or(
                        Expression.Not(x),
                        Expression.Not(y)
                    )
                );

            var f = Expression.Lambda<Func<bool?, bool?, bool?>>(e, x, y);
            var o = GetOptimizer().VisitAndConvert(f, "Test");

            var fun = f.Compile();
            var opt = o.Compile();

            foreach (var b1 in new bool?[] { null, false, true })
            {
                foreach (var b2 in new bool?[] { null, false, true })
                {
                    Assert.AreEqual(opt(b1, b2), fun(b1, b2));
                }
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Comparison_NonNullable_Laws()
        {
            var original = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.Not(Expression.LessThan(l, r)),
                (l, r) => Expression.Not(Expression.LessThanOrEqual(l, r)),
                (l, r) => Expression.Not(Expression.GreaterThan(l, r)),
                (l, r) => Expression.Not(Expression.GreaterThanOrEqual(l, r)),
            };

            var alternative = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.GreaterThanOrEqual(l, r),
                (l, r) => Expression.GreaterThan(l, r),
                (l, r) => Expression.LessThanOrEqual(l, r),
                (l, r) => Expression.LessThan(l, r),
            };

            for (var i = 0; i < original.Length; i++)
            {
                var o = original[i];
                var a = alternative[i];

                AssertAlgebraicLaw<sbyte, bool>(o, a, new sbyte[] { sbyte.MinValue, -1, 0, 1, sbyte.MaxValue });
                AssertAlgebraicLaw<byte, bool>(o, a, new byte[] { 0, 1, byte.MaxValue });
                AssertAlgebraicLaw<short, bool>(o, a, new short[] { short.MinValue, -1, 0, 1, short.MaxValue });
                AssertAlgebraicLaw<ushort, bool>(o, a, new ushort[] { 0, 1, ushort.MaxValue });
                AssertAlgebraicLaw<int, bool>(o, a, new int[] { int.MinValue, -1, 0, 1, int.MaxValue });
                AssertAlgebraicLaw<uint, bool>(o, a, new uint[] { 0, 1, uint.MaxValue });
                AssertAlgebraicLaw<long, bool>(o, a, new long[] { long.MinValue, -1, 0, 1, long.MaxValue });
                AssertAlgebraicLaw<ulong, bool>(o, a, new ulong[] { 0, 1, ulong.MaxValue });

                /*
                 * NB: These are not valid for e.g. !(0 < NaN) --> (0 >= NaN)
                 *
                AssertAlgebraicLaw<float, bool>(o, a, new float[] { 0.0f, 0.1f, 0.3f, 1.0f, float.MinValue, float.MaxValue, float.NaN, float.NegativeInfinity, float.PositiveInfinity });
                AssertAlgebraicLaw<double, bool>(o, a, new double[] { 0.0d, 0.1d, 0.3d, 1.0d, double.MinValue, double.MaxValue, double.NaN, double.NegativeInfinity, double.PositiveInfinity });
                 */
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Comparison_Nullable_NotLifted_Laws()
        {
            var original = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.Not(Expression.LessThan(l, r, liftToNull: false, method: null)),
                (l, r) => Expression.Not(Expression.LessThanOrEqual(l, r, liftToNull: false, method: null)),
                (l, r) => Expression.Not(Expression.GreaterThan(l, r, liftToNull: false, method: null)),
                (l, r) => Expression.Not(Expression.GreaterThanOrEqual(l, r, liftToNull: false, method: null)),
            };

            var alternative = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.GreaterThanOrEqual(l, r, liftToNull: false, method: null),
                (l, r) => Expression.GreaterThan(l, r, liftToNull: false, method: null),
                (l, r) => Expression.LessThanOrEqual(l, r, liftToNull: false, method: null),
                (l, r) => Expression.LessThan(l, r, liftToNull: false, method: null),
            };

            for (var i = 0; i < original.Length; i++)
            {
                var o = original[i];
                var a = alternative[i];

                // WARNING: The sample values *exclude* null! The algebraic law doesn't hold with null operands.

                AssertAlgebraicLaw<sbyte?, bool>(o, a, new sbyte?[] { sbyte.MinValue, -1, 0, 1, sbyte.MaxValue });
                AssertAlgebraicLaw<byte?, bool>(o, a, new byte?[] { 0, 1, byte.MaxValue });
                AssertAlgebraicLaw<short?, bool>(o, a, new short?[] { short.MinValue, -1, 0, 1, short.MaxValue });
                AssertAlgebraicLaw<ushort?, bool>(o, a, new ushort?[] { 0, 1, ushort.MaxValue });
                AssertAlgebraicLaw<int?, bool>(o, a, new int?[] { int.MinValue, -1, 0, 1, int.MaxValue });
                AssertAlgebraicLaw<uint?, bool>(o, a, new uint?[] { 0, 1, uint.MaxValue });
                AssertAlgebraicLaw<long?, bool>(o, a, new long?[] { long.MinValue, -1, 0, 1, long.MaxValue });
                AssertAlgebraicLaw<ulong?, bool>(o, a, new ulong?[] { 0, 1, ulong.MaxValue });

                /*
                 * NB: These are not valid for e.g. !(0 < NaN) --> (0 >= NaN)
                 *
                AssertAlgebraicLaw<float?, bool>(o, a, new float?[] { null, 0.0f, 0.1f, 0.3f, 1.0f, float.MinValue, float.MaxValue, float.NaN, float.NegativeInfinity, float.PositiveInfinity });
                AssertAlgebraicLaw<double?, bool>(o, a, new double?[] { null, 0.0d, 0.1d, 0.3d, 1.0d, double.MinValue, double.MaxValue, double.NaN, double.NegativeInfinity, double.PositiveInfinity });
                 */
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Comparison_Nullable_Lifted_Laws()
        {
            var original = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.Not(Expression.LessThan(l, r, liftToNull: true, method: null)),
                (l, r) => Expression.Not(Expression.LessThanOrEqual(l, r, liftToNull: true, method: null)),
                (l, r) => Expression.Not(Expression.GreaterThan(l, r, liftToNull: true, method: null)),
                (l, r) => Expression.Not(Expression.GreaterThanOrEqual(l, r, liftToNull: true, method: null)),
            };

            var alternative = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.GreaterThanOrEqual(l, r, liftToNull: true, method: null),
                (l, r) => Expression.GreaterThan(l, r, liftToNull: true, method: null),
                (l, r) => Expression.LessThanOrEqual(l, r, liftToNull: true, method: null),
                (l, r) => Expression.LessThan(l, r, liftToNull: true, method: null),
            };

            for (var i = 0; i < original.Length; i++)
            {
                var o = original[i];
                var a = alternative[i];

                AssertAlgebraicLaw<sbyte?, bool?>(o, a, new sbyte?[] { null, sbyte.MinValue, -1, 0, 1, sbyte.MaxValue });
                AssertAlgebraicLaw<byte?, bool?>(o, a, new byte?[] { null, 0, 1, byte.MaxValue });
                AssertAlgebraicLaw<short?, bool?>(o, a, new short?[] { null, short.MinValue, -1, 0, 1, short.MaxValue });
                AssertAlgebraicLaw<ushort?, bool?>(o, a, new ushort?[] { null, 0, 1, ushort.MaxValue });
                AssertAlgebraicLaw<int?, bool?>(o, a, new int?[] { null, int.MinValue, -1, 0, 1, int.MaxValue });
                AssertAlgebraicLaw<uint?, bool?>(o, a, new uint?[] { null, 0, 1, uint.MaxValue });
                AssertAlgebraicLaw<long?, bool?>(o, a, new long?[] { null, long.MinValue, -1, 0, 1, long.MaxValue });
                AssertAlgebraicLaw<ulong?, bool?>(o, a, new ulong?[] { null, 0, 1, ulong.MaxValue });

                /*
                 * NB: These are not valid for e.g. !(0 < NaN) --> (0 >= NaN)
                 *
                AssertAlgebraicLaw<float?, bool?>(o, a, new float?[] { null, 0.0f, 0.1f, 0.3f, 1.0f, float.MinValue, float.MaxValue, float.NaN, float.NegativeInfinity, float.PositiveInfinity });
                AssertAlgebraicLaw<double?, bool?>(o, a, new double?[] { null, 0.0d, 0.1d, 0.3d, 1.0d, double.MinValue, double.MaxValue, double.NaN, double.NegativeInfinity, double.PositiveInfinity });
                 */
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Equality_NonNullable_Laws()
        {
            var original = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.Not(Expression.Equal(l, r)),
                (l, r) => Expression.Not(Expression.NotEqual(l, r)),
            };

            var alternative = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.NotEqual(l, r),
                (l, r) => Expression.Equal(l, r),
            };

            for (var i = 0; i < original.Length; i++)
            {
                var o = original[i];
                var a = alternative[i];

                AssertAlgebraicLaw<sbyte, bool>(o, a, new sbyte[] { sbyte.MinValue, -1, 0, 1, sbyte.MaxValue });
                AssertAlgebraicLaw<byte, bool>(o, a, new byte[] { 0, 1, byte.MaxValue });
                AssertAlgebraicLaw<short, bool>(o, a, new short[] { short.MinValue, -1, 0, 1, short.MaxValue });
                AssertAlgebraicLaw<ushort, bool>(o, a, new ushort[] { 0, 1, ushort.MaxValue });
                AssertAlgebraicLaw<int, bool>(o, a, new int[] { int.MinValue, -1, 0, 1, int.MaxValue });
                AssertAlgebraicLaw<uint, bool>(o, a, new uint[] { 0, 1, uint.MaxValue });
                AssertAlgebraicLaw<long, bool>(o, a, new long[] { long.MinValue, -1, 0, 1, long.MaxValue });
                AssertAlgebraicLaw<ulong, bool>(o, a, new ulong[] { 0, 1, ulong.MaxValue });
                AssertAlgebraicLaw<float, bool>(o, a, new float[] { 0.0f, 0.1f, 0.3f, 1.0f, float.MinValue, float.MaxValue, float.NaN, float.NegativeInfinity, float.PositiveInfinity });
                AssertAlgebraicLaw<double, bool>(o, a, new double[] { 0.0d, 0.1d, 0.3d, 1.0d, double.MinValue, double.MaxValue, double.NaN, double.NegativeInfinity, double.PositiveInfinity });
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Equality_Nullable_NotLifted_Laws()
        {
            var original = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.Not(Expression.Equal(l, r, liftToNull: false, method: null)),
                (l, r) => Expression.Not(Expression.NotEqual(l, r, liftToNull: false, method: null)),
            };

            var alternative = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.NotEqual(l, r, liftToNull: false, method: null),
                (l, r) => Expression.Equal(l, r, liftToNull: false, method: null),
            };

            for (var i = 0; i < original.Length; i++)
            {
                var o = original[i];
                var a = alternative[i];

                AssertAlgebraicLaw<sbyte?, bool>(o, a, new sbyte?[] { null, sbyte.MinValue, -1, 0, 1, sbyte.MaxValue });
                AssertAlgebraicLaw<byte?, bool>(o, a, new byte?[] { null, 0, 1, byte.MaxValue });
                AssertAlgebraicLaw<short?, bool>(o, a, new short?[] { null, short.MinValue, -1, 0, 1, short.MaxValue });
                AssertAlgebraicLaw<ushort?, bool>(o, a, new ushort?[] { null, 0, 1, ushort.MaxValue });
                AssertAlgebraicLaw<int?, bool>(o, a, new int?[] { null, int.MinValue, -1, 0, 1, int.MaxValue });
                AssertAlgebraicLaw<uint?, bool>(o, a, new uint?[] { null, 0, 1, uint.MaxValue });
                AssertAlgebraicLaw<long?, bool>(o, a, new long?[] { null, long.MinValue, -1, 0, 1, long.MaxValue });
                AssertAlgebraicLaw<ulong?, bool>(o, a, new ulong?[] { null, 0, 1, ulong.MaxValue });
                AssertAlgebraicLaw<float?, bool>(o, a, new float?[] { null, 0.0f, 0.1f, 0.3f, 1.0f, float.MinValue, float.MaxValue, float.NaN, float.NegativeInfinity, float.PositiveInfinity });
                AssertAlgebraicLaw<double?, bool>(o, a, new double?[] { null, 0.0d, 0.1d, 0.3d, 1.0d, double.MinValue, double.MaxValue, double.NaN, double.NegativeInfinity, double.PositiveInfinity });
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Equality_Nullable_Lifted_Laws()
        {
            var original = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.Not(Expression.Equal(l, r, liftToNull: true, method: null)),
                (l, r) => Expression.Not(Expression.NotEqual(l, r, liftToNull: true, method: null)),
            };

            var alternative = new Func<Expression, Expression, Expression>[]
            {
                (l, r) => Expression.NotEqual(l, r, liftToNull: true, method: null),
                (l, r) => Expression.Equal(l, r, liftToNull: true, method: null),
            };

            for (var i = 0; i < original.Length; i++)
            {
                var o = original[i];
                var a = alternative[i];

                AssertAlgebraicLaw<sbyte?, bool?>(o, a, new sbyte?[] { null, sbyte.MinValue, -1, 0, 1, sbyte.MaxValue });
                AssertAlgebraicLaw<byte?, bool?>(o, a, new byte?[] { null, 0, 1, byte.MaxValue });
                AssertAlgebraicLaw<short?, bool?>(o, a, new short?[] { null, short.MinValue, -1, 0, 1, short.MaxValue });
                AssertAlgebraicLaw<ushort?, bool?>(o, a, new ushort?[] { null, 0, 1, ushort.MaxValue });
                AssertAlgebraicLaw<int?, bool?>(o, a, new int?[] { null, int.MinValue, -1, 0, 1, int.MaxValue });
                AssertAlgebraicLaw<uint?, bool?>(o, a, new uint?[] { null, 0, 1, uint.MaxValue });
                AssertAlgebraicLaw<long?, bool?>(o, a, new long?[] { null, long.MinValue, -1, 0, 1, long.MaxValue });
                AssertAlgebraicLaw<ulong?, bool?>(o, a, new ulong?[] { null, 0, 1, ulong.MaxValue });
                AssertAlgebraicLaw<float?, bool?>(o, a, new float?[] { null, 0.0f, 0.1f, 0.3f, 1.0f, float.MinValue, float.MaxValue, float.NaN, float.NegativeInfinity, float.PositiveInfinity });
                AssertAlgebraicLaw<double?, bool?>(o, a, new double?[] { null, 0.0d, 0.1d, 0.3d, 1.0d, double.MinValue, double.MaxValue, double.NaN, double.NegativeInfinity, double.PositiveInfinity });
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Comparison_NonNullable()
        {
            var opt = new[]
            {
                Tuple.Create(ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan),
                Tuple.Create(ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan),
            };

            var nop = new[]
            {
                ExpressionType.GreaterThan,
                ExpressionType.LessThan,
            };

            foreach (var t in new[] { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) })
            {
                var x = Expression.Parameter(t);
                var y = Expression.Parameter(t);

                foreach (var o in opt)
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                o.Item1,
                                x,
                                y
                            )
                        );

                    var r =
                        Expression.MakeBinary(
                            o.Item2,
                            x,
                            y
                        );

                    AssertOptimized(e, r);
                }

                foreach (var n in nop)
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                n,
                                x,
                                y
                            )
                        );

                    AssertOptimized(e, e);
                }
            }

            foreach (var t in new[] { typeof(float), typeof(double), typeof(decimal) /* NB: decimal uses a method */ })
            {
                var x = Expression.Parameter(t);
                var y = Expression.Parameter(t);

                foreach (var n in opt.Select(o => o.Item1).Concat(nop))
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                n,
                                x,
                                y
                            )
                        );

                    AssertOptimized(e, e);
                }
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Comparison_Nullable_Lifted()
        {
            var opt = new[]
            {
                Tuple.Create(ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan),
                Tuple.Create(ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan),
            };

            var nop = new[]
            {
                ExpressionType.GreaterThan,
                ExpressionType.LessThan,
            };

            foreach (var t in new[] { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) })
            {
                var x = Expression.Parameter(typeof(Nullable<>).MakeGenericType(t));
                var y = Expression.Parameter(typeof(Nullable<>).MakeGenericType(t));

                foreach (var o in opt)
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                o.Item1,
                                x,
                                y,
                                true,
                                null
                            )
                        );

                    var r =
                        Expression.MakeBinary(
                            o.Item2,
                            x,
                            y,
                            true,
                            null
                        );

                    AssertOptimized(e, r);
                }

                foreach (var n in nop)
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                n,
                                x,
                                y,
                                true,
                                null
                            )
                        );

                    AssertOptimized(e, e);
                }
            }

            foreach (var t in new[] { typeof(float), typeof(double), typeof(decimal) /* NB: decimal uses a method */ })
            {
                var x = Expression.Parameter(t);
                var y = Expression.Parameter(t);

                foreach (var n in opt.Select(o => o.Item1).Concat(nop))
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                n,
                                x,
                                y,
                                true,
                                null
                            )
                        );

                    AssertOptimized(e, e);
                }
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Comparison_Nullable_NonLifted()
        {
            var nop = new[]
            {
                ExpressionType.GreaterThan,
                ExpressionType.GreaterThanOrEqual,
                ExpressionType.LessThan,
                ExpressionType.LessThanOrEqual,
            };

            foreach (var t in new[] { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) /* NB: decimal uses a method */ })
            {
                var x = Expression.Parameter(typeof(Nullable<>).MakeGenericType(t));
                var y = Expression.Parameter(typeof(Nullable<>).MakeGenericType(t));

                foreach (var n in nop)
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                n,
                                x,
                                y,
                                false,
                                null
                            )
                        );

                    AssertOptimized(e, e);
                }
            }
        }

        [TestMethod]
        public void Unary_Algebraic_NotOf_Comparison_Nullable_NonLifted_NonNullOperands()
        {
            var opt = new[]
            {
                Tuple.Create(ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan),
                Tuple.Create(ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan),
            };

            var nop = new[]
            {
                ExpressionType.GreaterThan,
                ExpressionType.LessThan,
            };

            foreach (var t in new[] { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) })
            {
                var x = Expression.New(typeof(Nullable<>).MakeGenericType(t).GetConstructor(new[] { t }), Expression.Parameter(t));
                var y = Expression.New(typeof(Nullable<>).MakeGenericType(t).GetConstructor(new[] { t }), Expression.Parameter(t));

                foreach (var o in opt)
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                o.Item1,
                                x,
                                y,
                                false,
                                null
                            )
                        );

                    var r =
                        Expression.MakeBinary(
                            o.Item2,
                            x,
                            y,
                            false,
                            null
                        );

                    AssertOptimized(e, r);
                }

                foreach (var n in nop)
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                n,
                                x,
                                y,
                                false,
                                null
                            )
                        );

                    AssertOptimized(e, e);
                }
            }

            foreach (var t in new[] { typeof(float), typeof(double), typeof(decimal) /* NB: decimal uses a method */ })
            {
                var x = Expression.Parameter(t);
                var y = Expression.Parameter(t);

                foreach (var n in opt.Select(o => o.Item1).Concat(nop))
                {
                    var e =
                        Expression.Not(
                            Expression.MakeBinary(
                                n,
                                x,
                                y,
                                false,
                                null
                            )
                        );

                    AssertOptimized(e, e);
                }
            }
        }

        private static void AssertAlgebraicLaw<T, R>(Func<Expression, Expression, Expression> original, Func<Expression, Expression, Expression> alternative, T[] values)
        {
            var l = Expression.Parameter(typeof(T), "x");
            var r = Expression.Parameter(typeof(T), "y");

            var o = Expression.Lambda<Func<T, T, R>>(original(l, r), l, r);
            var a = Expression.Lambda<Func<T, T, R>>(alternative(l, r), l, r);

            AssertAlgebraicLaw<T, R>(o, a, values);

            var z = GetOptimizer().VisitAndConvert(o, "Test");

            AssertAlgebraicLaw<T, R>(o, z, values);
        }

        private static void AssertAlgebraicLaw<T, R>(Expression<Func<T, T, R>> original, Expression<Func<T, T, R>> alternative, T[] values)
        {
            var o = original.Compile();
            var a = alternative.Compile();

            foreach (var x in values)
            {
                foreach (var y in values)
                {
                    Assert.AreEqual(o(x, y), a(x, y), $"Failed for ({x},{y}) - {original.Body} != {alternative.Body}");
                }
            }
        }

        public static bool UnaryBoolMethod(bool b) => b;
        public static bool BinaryBoolMethod(bool b1, bool b2) => b1 & b2;
    }
}
