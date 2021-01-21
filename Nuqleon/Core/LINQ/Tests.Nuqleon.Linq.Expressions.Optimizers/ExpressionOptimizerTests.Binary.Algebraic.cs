// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        private static readonly Dictionary<Type, object[]> s_values = new()
        {
            { typeof(bool), new object[] { false, true } },
            { typeof(sbyte), new object[] { sbyte.MinValue, (sbyte)-1, (sbyte)0, (sbyte)1, (sbyte)42, sbyte.MaxValue } },
            { typeof(short), new object[] { short.MinValue, (short)-1, (short)0, (short)1, (short)42, short.MaxValue } },
            { typeof(int), new object[] { int.MinValue, -1, 0, 1, 42, int.MaxValue } },
            { typeof(long), new object[] { long.MinValue, (long)-1, (long)0, (long)1, (long)42, long.MaxValue } },
            { typeof(byte), new object[] { (byte)0, (byte)1, (byte)42, byte.MaxValue } },
            { typeof(ushort), new object[] { (ushort)0, (ushort)1, (ushort)42, ushort.MaxValue } },
            { typeof(uint), new object[] { (uint)0, (uint)1, (uint)42, uint.MaxValue } },
            { typeof(ulong), new object[] { (ulong)0, (ulong)1, (ulong)42, ulong.MaxValue } },
            { typeof(float), new object[] { (float)0.0, (float)1.0, (float)3.14, float.MinValue, float.MaxValue, float.NaN, float.NegativeInfinity, float.PositiveInfinity } },
            { typeof(double), new object[] { 0.0, 1.0, 3.14, double.MinValue, double.MaxValue, double.NaN, double.NegativeInfinity, double.PositiveInfinity } },
        };

        private static readonly Dictionary<Type, object> s_zeros = new()
        {
            { typeof(bool), false },
            { typeof(byte), (byte)0 },
            { typeof(sbyte), (sbyte)0 },
            { typeof(short), (short)0 },
            { typeof(ushort), (ushort)0 },
            { typeof(int), 0 },
            { typeof(uint), (uint)0 },
            { typeof(long), (long)0 },
            { typeof(ulong), (ulong)0 },
        };

        private static readonly Dictionary<Type, object> s_ones = new()
        {
            { typeof(bool), true },
            { typeof(byte), (byte)1 },
            { typeof(sbyte), (sbyte)1 },
            { typeof(short), (short)1 },
            { typeof(ushort), (ushort)1 },
            { typeof(int), 1 },
            { typeof(uint), (uint)1 },
            { typeof(long), (long)1 },
            { typeof(ulong), (ulong)1 },
        };

        private static readonly Dictionary<Type, object> s_allOnes = new()
        {
            { typeof(bool), true },
            { typeof(byte), unchecked((byte)0xFF) },
            { typeof(sbyte), unchecked((sbyte)0xFF) },
            { typeof(short), unchecked((short)0xFFFF) },
            { typeof(ushort), unchecked((ushort)0xFFFF) },
            { typeof(int), unchecked((int)0xFFFFFFFF) },
            { typeof(uint), unchecked(0xFFFFFFFF) },
            { typeof(long), unchecked((long)0xFFFFFFFFFFFFFFFF) },
            { typeof(ulong), unchecked(0xFFFFFFFFFFFFFFFF) },
        };

        private static readonly Dictionary<Type, object> s_minValues = new()
        {
            { typeof(byte), byte.MinValue },
            { typeof(sbyte), sbyte.MinValue },
            { typeof(short), short.MinValue },
            { typeof(ushort), ushort.MinValue },
            { typeof(int), int.MinValue },
            { typeof(uint), uint.MinValue },
            { typeof(long), long.MinValue },
            { typeof(ulong), ulong.MinValue },
        };

        private static readonly Dictionary<Type, object> s_maxValues = new()
        {
            { typeof(byte), byte.MaxValue },
            { typeof(sbyte), sbyte.MaxValue },
            { typeof(short), short.MaxValue },
            { typeof(ushort), ushort.MaxValue },
            { typeof(int), int.MaxValue },
            { typeof(uint), uint.MaxValue },
            { typeof(long), long.MaxValue },
            { typeof(ulong), ulong.MaxValue },
        };

        private static readonly Type[] s_integerTypes = new[]
        {
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
        };

        private static readonly Type[] s_integerAndBoolTypes = new[]
        {
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(bool),
        };

        private static readonly Type[] s_arithIntegerTypes = new[]
        {
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
        };

        [TestMethod]
        public void Binary_Algebraic_Add_Nop()
        {
            var e =
                Expression.Add(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(1)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_Add_Zero()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.Add(x, c);
                    AssertOptimized(b1, x);

                    var b2 = Expression.Add(c, x);
                    AssertOptimized(b2, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_AddChecked_Nop()
        {
            var e =
                Expression.AddChecked(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(1)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_AddChecked_Zero()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.AddChecked(x, c);
                    AssertOptimized(b1, x);

                    var b2 = Expression.AddChecked(c, x);
                    AssertOptimized(b2, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_Subtract_Nop()
        {
            var e =
                Expression.Subtract(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(1)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_Subtract_Zero()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.Subtract(x, c);
                    AssertOptimized(b1, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_SubtractChecked_Nop()
        {
            var e =
                Expression.SubtractChecked(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(1)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_SubtractChecked_Zero()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.SubtractChecked(x, c);
                    AssertOptimized(b1, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_Multiply_Nop()
        {
            var e =
                Expression.Multiply(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(2)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_Multiply_One()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetOne(t), t);

                    var b1 = Expression.Multiply(x, c);
                    AssertOptimized(b1, x);

                    var b2 = Expression.Multiply(c, x);
                    AssertOptimized(b2, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_MultiplyChecked_Nop()
        {
            var e =
                Expression.MultiplyChecked(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(2)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_MultiplyChecked_One()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetOne(t), t);

                    var b1 = Expression.MultiplyChecked(x, c);
                    AssertOptimized(b1, x);

                    var b2 = Expression.MultiplyChecked(c, x);
                    AssertOptimized(b2, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_Multiply_Zero()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.Multiply(x, c);
                    AssertOptimized(b1, c);

                    var b2 = Expression.Multiply(c, x);
                    AssertOptimized(b2, c);

                    var f0 = Expression.Lambda(c, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }

                foreach (var t in new[] { typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.Multiply(x, c);
                    AssertOptimized(b1, b1);

                    var b2 = Expression.Multiply(c, x);
                    AssertOptimized(b2, b2);
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_MultiplyChecked_Zero()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.MultiplyChecked(x, c);
                    AssertOptimized(b1, c);

                    var b2 = Expression.MultiplyChecked(c, x);
                    AssertOptimized(b2, c);

                    var f0 = Expression.Lambda(c, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }

                foreach (var t in new[] { typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.MultiplyChecked(x, c);
                    AssertOptimized(b1, b1);

                    var b2 = Expression.MultiplyChecked(c, x);
                    AssertOptimized(b2, b2);
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_Divide_Nop()
        {
            var e =
                Expression.Divide(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(2)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_Divide_One()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetOne(t), t);

                    var b1 = Expression.Divide(x, c);
                    AssertOptimized(b1, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_LeftShift_Nop()
        {
            var e =
                Expression.LeftShift(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(1)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_LeftShift_Zero()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(0, typeof(int));

                    var b1 = Expression.LeftShift(x, c);
                    AssertOptimized(b1, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_RightShift_Nop()
        {
            var e =
                Expression.RightShift(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(1)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_RightShift_Zero()
        {
            foreach (var a in s_arithIntegerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(0, typeof(int));

                    var b1 = Expression.RightShift(x, c);
                    AssertOptimized(b1, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_And_Nop()
        {
            var e =
                Expression.And(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(2)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_And_AllOnes()
        {
            foreach (var a in s_integerAndBoolTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetAllOnes(t), t);

                    var b1 = Expression.And(x, c);
                    AssertOptimized(b1, x);

                    var b2 = Expression.And(c, x);
                    AssertOptimized(b2, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_And_AllZeros()
        {
            foreach (var a in s_integerAndBoolTypes)
            {
                foreach (var t in new[] { a })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.And(x, c);
                    AssertOptimized(b1, c);

                    var b2 = Expression.And(c, x);
                    AssertOptimized(b2, c);

                    var f0 = Expression.Lambda(c, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }

                foreach (var t in new[] { typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.And(x, c);
                    AssertOptimized(b1, b1);

                    var b2 = Expression.And(c, x);
                    AssertOptimized(b2, b2);
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_Or_Nop()
        {
            var e =
                Expression.Or(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(2)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_Or_AllZeros()
        {
            foreach (var a in s_integerAndBoolTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.Or(x, c);
                    AssertOptimized(b1, x);

                    var b2 = Expression.Or(c, x);
                    AssertOptimized(b2, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_Or_AllOnes()
        {
            foreach (var a in s_integerAndBoolTypes)
            {
                foreach (var t in new[] { a })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetAllOnes(t), t);

                    var b1 = Expression.Or(x, c);
                    AssertOptimized(b1, c);

                    var b2 = Expression.Or(c, x);
                    AssertOptimized(b2, c);

                    var f0 = Expression.Lambda(c, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }

                foreach (var t in new[] { typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetAllOnes(t), t);

                    var b1 = Expression.Or(x, c);
                    AssertOptimized(b1, b1);

                    var b2 = Expression.Or(c, x);
                    AssertOptimized(b2, b2);
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_ExclusiveOr_Nop()
        {
            var e =
                Expression.ExclusiveOr(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(2)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Algebraic_ExclusiveOr_AllZeros()
        {
            foreach (var a in s_integerAndBoolTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetZero(t), t);

                    var b1 = Expression.ExclusiveOr(x, c);
                    AssertOptimized(b1, x);

                    var b2 = Expression.ExclusiveOr(c, x);
                    AssertOptimized(b2, x);

                    var f0 = Expression.Lambda(x, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_ExclusiveOr_AllOnes()
        {
            foreach (var a in s_integerAndBoolTypes)
            {
                foreach (var t in new[] { a })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetAllOnes(t), t);
                    var e = t == typeof(bool) ? Expression.Not(x) : Expression.OnesComplement(x);

                    var b1 = Expression.ExclusiveOr(x, c);
                    AssertOptimized(b1, e);

                    var b2 = Expression.ExclusiveOr(c, x);
                    AssertOptimized(b2, e);

                    var f0 = Expression.Lambda(e, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }

                foreach (var t in new[] { typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetAllOnes(t), t);

                    var b1 = Expression.ExclusiveOr(x, c);
                    AssertOptimized(b1, b1);

                    var b2 = Expression.ExclusiveOr(c, x);
                    AssertOptimized(b2, b2);
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_ExclusiveOr_Law()
        {
            foreach (var a in s_integerAndBoolTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var y = Expression.Parameter(t);

                    var nx = Expression.Not(x);
                    var ny = Expression.Not(y);

                    var b = Expression.ExclusiveOr(nx, ny);
                    var e = Expression.ExclusiveOr(x, y);

                    AssertOptimized(b, e);

                    var f0 = Expression.Lambda(e, x, y).Compile();
                    var f1 = Expression.Lambda(b, x, y).Compile();

                    foreach (var l in GetValues(t))
                    {
                        foreach (var r in GetValues(t))
                        {
                            var r0 = f0.DynamicInvoke(l, r);
                            var r1 = f1.DynamicInvoke(l, r);

                            Assert.AreEqual(r0, r1);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_LessThan_MinValue_NoLiftToNull()
        {
            foreach (var a in s_integerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetMinValue(t), t);
                    var e = Expression.Constant(false);

                    var b1 = Expression.LessThan(x, c);
                    AssertOptimized(b1, e);

                    var b2 = Expression.GreaterThan(c, x);
                    AssertOptimized(b2, e);

                    var f0 = Expression.Lambda(e, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_LessThan_MinValue_LiftToNull()
        {
            foreach (var a in s_integerTypes)
            {
                foreach (var t in new[] { typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetMinValue(t), t);

                    var b1 = Expression.LessThan(x, c, liftToNull: true, method: null);
                    AssertOptimized(b1, b1);

                    var b2 = Expression.GreaterThan(c, x, liftToNull: true, method: null);
                    AssertOptimized(b2, b2);
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_LessThanOrEqual_MaxValue_NonNullable()
        {
            foreach (var a in s_integerTypes)
            {
                foreach (var t in new[] { a })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetMaxValue(t), t);
                    var e = Expression.Constant(true);

                    var b1 = Expression.LessThanOrEqual(x, c);
                    AssertOptimized(b1, e);

                    var b2 = Expression.GreaterThanOrEqual(c, x);
                    AssertOptimized(b2, e);

                    var f0 = Expression.Lambda(e, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_LessThanOrEqual_MaxValue_Nullable()
        {
            foreach (var a in s_integerTypes)
            {
                foreach (var t in new[] { typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetMaxValue(t), t);

                    foreach (var b in new[] { false, true })
                    {
                        var b1 = Expression.LessThanOrEqual(x, c, b, method: null);
                        AssertOptimized(b1, b1);

                        var b2 = Expression.GreaterThanOrEqual(c, x, b, method: null);
                        AssertOptimized(b2, b2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_GreaterThan_MaxValue_NoLiftToNull()
        {
            foreach (var a in s_integerTypes)
            {
                foreach (var t in new[] { a, typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetMaxValue(t), t);
                    var e = Expression.Constant(false);

                    var b1 = Expression.GreaterThan(x, c);
                    AssertOptimized(b1, e);

                    var b2 = Expression.LessThan(c, x);
                    AssertOptimized(b2, e);

                    var f0 = Expression.Lambda(e, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_GreaterThan_MaxValue_LiftToNull()
        {
            foreach (var a in s_integerTypes)
            {
                foreach (var t in new[] { typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetMaxValue(t), t);

                    var b1 = Expression.GreaterThan(x, c, liftToNull: true, method: null);
                    AssertOptimized(b1, b1);

                    var b2 = Expression.LessThan(c, x, liftToNull: true, method: null);
                    AssertOptimized(b2, b2);
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_GreaterThanOrEqual_MinValue_NonNullable()
        {
            foreach (var a in s_integerTypes)
            {
                foreach (var t in new[] { a })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetMinValue(t), t);
                    var e = Expression.Constant(true);

                    var b1 = Expression.GreaterThanOrEqual(x, c);
                    AssertOptimized(b1, e);

                    var b2 = Expression.LessThanOrEqual(c, x);
                    AssertOptimized(b2, e);

                    var f0 = Expression.Lambda(e, x).Compile();
                    var f1 = Expression.Lambda(b1, x).Compile();
                    var f2 = Expression.Lambda(b2, x).Compile();

                    foreach (var o in GetValues(t))
                    {
                        var r0 = f0.DynamicInvoke(o);
                        var r1 = f1.DynamicInvoke(o);
                        var r2 = f2.DynamicInvoke(o);

                        Assert.AreEqual(r0, r1);
                        Assert.AreEqual(r0, r2);
                    }
                }
            }
        }

        [TestMethod]
        public void Binary_Algebraic_GreaterThanOrEqual_MinValue_Nullable()
        {
            foreach (var a in s_integerTypes)
            {
                foreach (var t in new[] { typeof(Nullable<>).MakeGenericType(a) })
                {
                    var x = Expression.Parameter(t);
                    var c = Expression.Constant(GetMinValue(t), t);

                    foreach (var b in new[] { false, true })
                    {
                        var b1 = Expression.GreaterThanOrEqual(x, c, b, method: null);
                        AssertOptimized(b1, b1);

                        var b2 = Expression.LessThanOrEqual(c, x, b, method: null);
                        AssertOptimized(b2, b2);
                    }
                }
            }
        }

        private static object GetZero(Type type)
        {
            return s_zeros[type.GetNonNullableType()];
        }

        private static object GetOne(Type type)
        {
            return s_ones[type.GetNonNullableType()];
        }

        private static object GetAllOnes(Type type)
        {
            return s_allOnes[type.GetNonNullableType()];
        }

        private static object GetMinValue(Type type)
        {
            return s_minValues[type.GetNonNullableType()];
        }

        private static object GetMaxValue(Type type)
        {
            return s_maxValues[type.GetNonNullableType()];
        }

        private static IEnumerable<object> GetValues(Type type)
        {
            if (type.IsGenericType)
            {
                Debug.Assert(type.GetGenericTypeDefinition() == typeof(Nullable<>));

                return s_values[type.GetNonNullableType()].Concat(new object[] { null });
            }

            return s_values[type];
        }
    }
}
