// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//


using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace Tests.System.Linq.Expressions.Optimizers
{
    partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void Unary_Decrement_Eval_Int16()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((short)-32768, typeof(short)))
                /* 32767 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short)-1, typeof(short)))
                /* -2 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short)0, typeof(short)))
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short)1, typeof(short)))
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short)42, typeof(short)))
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short)32767, typeof(short)))
                /* 32766 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_NullableInt16()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((short?)null, typeof(short?)))
                /* null */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short?)-32768, typeof(short?)))//,
                /* 32767 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short?)-1, typeof(short?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short?)0, typeof(short?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short?)1, typeof(short?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short?)42, typeof(short?)))//,
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((short?)32767, typeof(short?)))//,
                /* 32766 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_UInt16()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((ushort)0, typeof(ushort)))
                /* 65535 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ushort)1, typeof(ushort)))
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ushort)42, typeof(ushort)))
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ushort)65535, typeof(ushort)))
                /* 65534 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_NullableUInt16()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((ushort?)null, typeof(ushort?)))
                /* null */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ushort?)0, typeof(ushort?)))//,
                /* 65535 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ushort?)1, typeof(ushort?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ushort?)42, typeof(ushort?)))//,
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ushort?)65535, typeof(ushort?)))//,
                /* 65534 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_Int32()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((int)-2147483648, typeof(int)))
                /* 2147483647 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int)-1, typeof(int)))
                /* -2 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int)0, typeof(int)))
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int)1, typeof(int)))
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int)42, typeof(int)))
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int)2147483647, typeof(int)))
                /* 2147483646 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_NullableInt32()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((int?)null, typeof(int?)))
                /* null */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int?)-2147483648, typeof(int?)))//,
                /* 2147483647 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int?)-1, typeof(int?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int?)0, typeof(int?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int?)1, typeof(int?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int?)42, typeof(int?)))//,
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((int?)2147483647, typeof(int?)))//,
                /* 2147483646 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_UInt32()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((uint)0, typeof(uint)))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((uint)1, typeof(uint)))
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((uint)42, typeof(uint)))
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((uint)4294967295, typeof(uint)))
                /* 4294967294 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_NullableUInt32()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((uint?)null, typeof(uint?)))
                /* null */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((uint?)0, typeof(uint?)))//,
                /* 4294967295 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((uint?)1, typeof(uint?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((uint?)42, typeof(uint?)))//,
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((uint?)4294967295, typeof(uint?)))//,
                /* 4294967294 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_Int64()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((long)-9223372036854775808, typeof(long)))
                /* 9223372036854775807 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long)-1, typeof(long)))
                /* -2 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long)0, typeof(long)))
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long)1, typeof(long)))
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long)42, typeof(long)))
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long)9223372036854775807, typeof(long)))
                /* 9223372036854775806 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_NullableInt64()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((long?)null, typeof(long?)))
                /* null */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long?)-9223372036854775808, typeof(long?)))//,
                /* 9223372036854775807 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long?)-1, typeof(long?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long?)0, typeof(long?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long?)1, typeof(long?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long?)42, typeof(long?)))//,
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((long?)9223372036854775807, typeof(long?)))//,
                /* 9223372036854775806 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_UInt64()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((ulong)0, typeof(ulong)))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ulong)1, typeof(ulong)))
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ulong)42, typeof(ulong)))
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ulong)18446744073709551615, typeof(ulong)))
                /* 18446744073709551614 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_NullableUInt64()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((ulong?)null, typeof(ulong?)))
                /* null */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ulong?)0, typeof(ulong?)))//,
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ulong?)1, typeof(ulong?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ulong?)42, typeof(ulong?)))//,
                /* 41 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)))//,
                /* 18446744073709551614 */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_Single()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((float)0, typeof(float)))
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float)1, typeof(float)))
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float)3.14, typeof(float)))
                /* 2.14 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float)float.MinValue, typeof(float)))
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float)float.MaxValue, typeof(float)))
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float)float.NaN, typeof(float)))
                /* nan */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float)float.NegativeInfinity, typeof(float)))
                /* -∞ */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float)float.PositiveInfinity, typeof(float)))
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_NullableSingle()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((float?)null, typeof(float?)))
                /* null */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float?)0, typeof(float?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float?)1, typeof(float?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float?)3.14, typeof(float?)))//,
                /* 2.14 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float?)float.MinValue, typeof(float?)))//,
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float?)float.MaxValue, typeof(float?)))//,
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float?)float.NaN, typeof(float?)))//,
                /* nan */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float?)float.NegativeInfinity, typeof(float?)))//,
                /* -∞ */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((float?)float.PositiveInfinity, typeof(float?)))//,
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_Double()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((double)0, typeof(double)))
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double)1, typeof(double)))
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double)3.14, typeof(double)))
                /* 2.14 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double)double.MinValue, typeof(double)))
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double)double.MaxValue, typeof(double)))
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double)double.NaN, typeof(double)))
                /* nan */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double)double.NegativeInfinity, typeof(double)))
                /* -∞ */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double)double.PositiveInfinity, typeof(double)))
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_Decrement_Eval_NullableDouble()
        {
            AssertEval(
                Expression.Decrement(Expression.Constant((double?)null, typeof(double?)))
                /* null */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double?)0, typeof(double?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double?)1, typeof(double?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double?)3.14, typeof(double?)))//,
                /* 2.14 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double?)double.MinValue, typeof(double?)))//,
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double?)double.MaxValue, typeof(double?)))//,
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double?)double.NaN, typeof(double?)))//,
                /* nan */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double?)double.NegativeInfinity, typeof(double?)))//,
                /* -∞ */
            );
            AssertEval(
                Expression.Decrement(Expression.Constant((double?)double.PositiveInfinity, typeof(double?)))//,
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_Int16()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((short)-32768, typeof(short)))
                /* -32767 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short)-1, typeof(short)))
                /* 0 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short)0, typeof(short)))
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short)1, typeof(short)))
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short)42, typeof(short)))
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short)32767, typeof(short)))
                /* -32768 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_NullableInt16()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((short?)null, typeof(short?)))
                /* null */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short?)-32768, typeof(short?)))//,
                /* -32767 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short?)-1, typeof(short?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short?)0, typeof(short?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short?)1, typeof(short?)))//,
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short?)42, typeof(short?)))//,
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((short?)32767, typeof(short?)))//,
                /* -32768 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_UInt16()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((ushort)0, typeof(ushort)))
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ushort)1, typeof(ushort)))
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ushort)42, typeof(ushort)))
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ushort)65535, typeof(ushort)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_NullableUInt16()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((ushort?)null, typeof(ushort?)))
                /* null */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ushort?)0, typeof(ushort?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ushort?)1, typeof(ushort?)))//,
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ushort?)42, typeof(ushort?)))//,
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ushort?)65535, typeof(ushort?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_Int32()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((int)-2147483648, typeof(int)))
                /* -2147483647 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int)-1, typeof(int)))
                /* 0 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int)0, typeof(int)))
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int)1, typeof(int)))
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int)42, typeof(int)))
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int)2147483647, typeof(int)))
                /* -2147483648 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_NullableInt32()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((int?)null, typeof(int?)))
                /* null */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int?)-2147483648, typeof(int?)))//,
                /* -2147483647 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int?)-1, typeof(int?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int?)0, typeof(int?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int?)1, typeof(int?)))//,
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int?)42, typeof(int?)))//,
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((int?)2147483647, typeof(int?)))//,
                /* -2147483648 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_UInt32()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((uint)0, typeof(uint)))
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((uint)1, typeof(uint)))
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((uint)42, typeof(uint)))
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((uint)4294967295, typeof(uint)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_NullableUInt32()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((uint?)null, typeof(uint?)))
                /* null */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((uint?)0, typeof(uint?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((uint?)1, typeof(uint?)))//,
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((uint?)42, typeof(uint?)))//,
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((uint?)4294967295, typeof(uint?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_Int64()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((long)-9223372036854775808, typeof(long)))
                /* -9223372036854775807 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long)-1, typeof(long)))
                /* 0 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long)0, typeof(long)))
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long)1, typeof(long)))
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long)42, typeof(long)))
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long)9223372036854775807, typeof(long)))
                /* -9223372036854775808 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_NullableInt64()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((long?)null, typeof(long?)))
                /* null */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long?)-9223372036854775808, typeof(long?)))//,
                /* -9223372036854775807 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long?)-1, typeof(long?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long?)0, typeof(long?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long?)1, typeof(long?)))//,
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long?)42, typeof(long?)))//,
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((long?)9223372036854775807, typeof(long?)))//,
                /* -9223372036854775808 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_UInt64()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((ulong)0, typeof(ulong)))
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ulong)1, typeof(ulong)))
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ulong)42, typeof(ulong)))
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ulong)18446744073709551615, typeof(ulong)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_NullableUInt64()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((ulong?)null, typeof(ulong?)))
                /* null */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ulong?)0, typeof(ulong?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ulong?)1, typeof(ulong?)))//,
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ulong?)42, typeof(ulong?)))//,
                /* 43 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_Single()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((float)0, typeof(float)))
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float)1, typeof(float)))
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float)3.14, typeof(float)))
                /* 4.14 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float)float.MinValue, typeof(float)))
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float)float.MaxValue, typeof(float)))
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float)float.NaN, typeof(float)))
                /* nan */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float)float.NegativeInfinity, typeof(float)))
                /* -∞ */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float)float.PositiveInfinity, typeof(float)))
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_NullableSingle()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((float?)null, typeof(float?)))
                /* null */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float?)0, typeof(float?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float?)1, typeof(float?)))//,
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float?)3.14, typeof(float?)))//,
                /* 4.14 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float?)float.MinValue, typeof(float?)))//,
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float?)float.MaxValue, typeof(float?)))//,
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float?)float.NaN, typeof(float?)))//,
                /* nan */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float?)float.NegativeInfinity, typeof(float?)))//,
                /* -∞ */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((float?)float.PositiveInfinity, typeof(float?)))//,
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_Double()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((double)0, typeof(double)))
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double)1, typeof(double)))
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double)3.14, typeof(double)))
                /* 4.14 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double)double.MinValue, typeof(double)))
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double)double.MaxValue, typeof(double)))
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double)double.NaN, typeof(double)))
                /* nan */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double)double.NegativeInfinity, typeof(double)))
                /* -∞ */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double)double.PositiveInfinity, typeof(double)))
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_Increment_Eval_NullableDouble()
        {
            AssertEval(
                Expression.Increment(Expression.Constant((double?)null, typeof(double?)))
                /* null */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double?)0, typeof(double?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double?)1, typeof(double?)))//,
                /* 2 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double?)3.14, typeof(double?)))//,
                /* 4.14 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double?)double.MinValue, typeof(double?)))//,
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double?)double.MaxValue, typeof(double?)))//,
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double?)double.NaN, typeof(double?)))//,
                /* nan */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double?)double.NegativeInfinity, typeof(double?)))//,
                /* -∞ */
            );
            AssertEval(
                Expression.Increment(Expression.Constant((double?)double.PositiveInfinity, typeof(double?)))//,
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_IsFalse_Eval_Boolean()
        {
            AssertEval(
                Expression.IsFalse(Expression.Constant((bool)false, typeof(bool)))
                /* true */
            );
            AssertEval(
                Expression.IsFalse(Expression.Constant((bool)true, typeof(bool)))
                /* false */
            );
        }

        [TestMethod]
        public void Unary_IsFalse_Eval_NullableBoolean()
        {
            AssertEval(
                Expression.IsFalse(Expression.Constant((bool?)null, typeof(bool?)))
                /* null */
            );
            AssertEval(
                Expression.IsFalse(Expression.Constant((bool?)false, typeof(bool?)))//,
                /* true */
            );
            AssertEval(
                Expression.IsFalse(Expression.Constant((bool?)true, typeof(bool?)))//,
                /* false */
            );
        }

        [TestMethod]
        public void Unary_IsTrue_Eval_Boolean()
        {
            AssertEval(
                Expression.IsTrue(Expression.Constant((bool)false, typeof(bool)))
                /* false */
            );
            AssertEval(
                Expression.IsTrue(Expression.Constant((bool)true, typeof(bool)))
                /* true */
            );
        }

        [TestMethod]
        public void Unary_IsTrue_Eval_NullableBoolean()
        {
            AssertEval(
                Expression.IsTrue(Expression.Constant((bool?)null, typeof(bool?)))
                /* null */
            );
            AssertEval(
                Expression.IsTrue(Expression.Constant((bool?)false, typeof(bool?)))//,
                /* false */
            );
            AssertEval(
                Expression.IsTrue(Expression.Constant((bool?)true, typeof(bool?)))//,
                /* true */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_Int16()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((short)-32768, typeof(short)))
                /* -32768 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short)-1, typeof(short)))
                /* 1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short)0, typeof(short)))
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short)1, typeof(short)))
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short)42, typeof(short)))
                /* -42 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short)32767, typeof(short)))
                /* -32767 */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_NullableInt16()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((short?)null, typeof(short?)))
                /* null */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short?)-32768, typeof(short?)))//,
                /* -32768 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short?)-1, typeof(short?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short?)0, typeof(short?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short?)1, typeof(short?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short?)42, typeof(short?)))//,
                /* -42 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((short?)32767, typeof(short?)))//,
                /* -32767 */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_Int32()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((int)-2147483648, typeof(int)))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int)-1, typeof(int)))
                /* 1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int)0, typeof(int)))
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int)1, typeof(int)))
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int)42, typeof(int)))
                /* -42 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int)2147483647, typeof(int)))
                /* -2147483647 */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_NullableInt32()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((int?)null, typeof(int?)))
                /* null */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int?)-2147483648, typeof(int?)))//,
                /* -2147483648 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int?)-1, typeof(int?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int?)0, typeof(int?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int?)1, typeof(int?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int?)42, typeof(int?)))//,
                /* -42 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((int?)2147483647, typeof(int?)))//,
                /* -2147483647 */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_Int64()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((long)-9223372036854775808, typeof(long)))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long)-1, typeof(long)))
                /* 1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long)0, typeof(long)))
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long)1, typeof(long)))
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long)42, typeof(long)))
                /* -42 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long)9223372036854775807, typeof(long)))
                /* -9223372036854775807 */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_NullableInt64()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((long?)null, typeof(long?)))
                /* null */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long?)-9223372036854775808, typeof(long?)))//,
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long?)-1, typeof(long?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long?)0, typeof(long?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long?)1, typeof(long?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long?)42, typeof(long?)))//,
                /* -42 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((long?)9223372036854775807, typeof(long?)))//,
                /* -9223372036854775807 */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_Single()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((float)0, typeof(float)))
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float)1, typeof(float)))
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float)3.14, typeof(float)))
                /* -3.14 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float)float.MinValue, typeof(float)))
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float)float.MaxValue, typeof(float)))
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float)float.NaN, typeof(float)))
                /* nan */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float)float.NegativeInfinity, typeof(float)))
                /* ∞ */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float)float.PositiveInfinity, typeof(float)))
                /* -∞ */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_NullableSingle()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((float?)null, typeof(float?)))
                /* null */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float?)0, typeof(float?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float?)1, typeof(float?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float?)3.14, typeof(float?)))//,
                /* -3.14 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float?)float.MinValue, typeof(float?)))//,
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float?)float.MaxValue, typeof(float?)))//,
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float?)float.NaN, typeof(float?)))//,
                /* nan */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float?)float.NegativeInfinity, typeof(float?)))//,
                /* ∞ */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((float?)float.PositiveInfinity, typeof(float?)))//,
                /* -∞ */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_Double()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((double)0, typeof(double)))
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double)1, typeof(double)))
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double)3.14, typeof(double)))
                /* -3.14 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double)double.MinValue, typeof(double)))
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double)double.MaxValue, typeof(double)))
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double)double.NaN, typeof(double)))
                /* nan */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double)double.NegativeInfinity, typeof(double)))
                /* ∞ */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double)double.PositiveInfinity, typeof(double)))
                /* -∞ */
            );
        }

        [TestMethod]
        public void Unary_Negate_Eval_NullableDouble()
        {
            AssertEval(
                Expression.Negate(Expression.Constant((double?)null, typeof(double?)))
                /* null */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double?)0, typeof(double?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double?)1, typeof(double?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double?)3.14, typeof(double?)))//,
                /* -3.14 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double?)double.MinValue, typeof(double?)))//,
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double?)double.MaxValue, typeof(double?)))//,
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double?)double.NaN, typeof(double?)))//,
                /* nan */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double?)double.NegativeInfinity, typeof(double?)))//,
                /* ∞ */
            );
            AssertEval(
                Expression.Negate(Expression.Constant((double?)double.PositiveInfinity, typeof(double?)))//,
                /* -∞ */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_Int16()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short)-32768, typeof(short)))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short)-1, typeof(short)))
                /* 1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short)0, typeof(short)))
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short)1, typeof(short)))
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short)42, typeof(short)))
                /* -42 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short)32767, typeof(short)))
                /* -32767 */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_NullableInt16()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short?)null, typeof(short?)))
                /* null */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short?)-32768, typeof(short?)))//,
                /* typeof(global::System.OverflowException) */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short?)-1, typeof(short?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short?)0, typeof(short?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short?)1, typeof(short?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short?)42, typeof(short?)))//,
                /* -42 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((short?)32767, typeof(short?)))//,
                /* -32767 */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_Int32()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int)-2147483648, typeof(int)))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int)-1, typeof(int)))
                /* 1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int)0, typeof(int)))
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int)1, typeof(int)))
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int)42, typeof(int)))
                /* -42 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int)2147483647, typeof(int)))
                /* -2147483647 */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_NullableInt32()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int?)null, typeof(int?)))
                /* null */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int?)-2147483648, typeof(int?)))//,
                /* typeof(global::System.OverflowException) */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int?)-1, typeof(int?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int?)0, typeof(int?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int?)1, typeof(int?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int?)42, typeof(int?)))//,
                /* -42 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((int?)2147483647, typeof(int?)))//,
                /* -2147483647 */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_Int64()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long)-9223372036854775808, typeof(long)))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long)-1, typeof(long)))
                /* 1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long)0, typeof(long)))
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long)1, typeof(long)))
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long)42, typeof(long)))
                /* -42 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long)9223372036854775807, typeof(long)))
                /* -9223372036854775807 */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_NullableInt64()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long?)null, typeof(long?)))
                /* null */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)))//,
                /* typeof(global::System.OverflowException) */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long?)-1, typeof(long?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long?)0, typeof(long?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long?)1, typeof(long?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long?)42, typeof(long?)))//,
                /* -42 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)))//,
                /* -9223372036854775807 */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_Single()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float)0, typeof(float)))
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float)1, typeof(float)))
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float)3.14, typeof(float)))
                /* -3.14 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float)float.MinValue, typeof(float)))
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float)float.MaxValue, typeof(float)))
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float)float.NaN, typeof(float)))
                /* nan */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float)float.NegativeInfinity, typeof(float)))
                /* ∞ */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float)float.PositiveInfinity, typeof(float)))
                /* -∞ */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_NullableSingle()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float?)null, typeof(float?)))
                /* null */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float?)0, typeof(float?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float?)1, typeof(float?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float?)3.14, typeof(float?)))//,
                /* -3.14 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float?)float.MinValue, typeof(float?)))//,
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float?)float.MaxValue, typeof(float?)))//,
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float?)float.NaN, typeof(float?)))//,
                /* nan */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float?)float.NegativeInfinity, typeof(float?)))//,
                /* ∞ */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((float?)float.PositiveInfinity, typeof(float?)))//,
                /* -∞ */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_Double()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double)0, typeof(double)))
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double)1, typeof(double)))
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double)3.14, typeof(double)))
                /* -3.14 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double)double.MinValue, typeof(double)))
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double)double.MaxValue, typeof(double)))
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double)double.NaN, typeof(double)))
                /* nan */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double)double.NegativeInfinity, typeof(double)))
                /* ∞ */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double)double.PositiveInfinity, typeof(double)))
                /* -∞ */
            );
        }

        [TestMethod]
        public void Unary_NegateChecked_Eval_NullableDouble()
        {
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double?)null, typeof(double?)))
                /* null */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double?)0, typeof(double?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double?)1, typeof(double?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double?)3.14, typeof(double?)))//,
                /* -3.14 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double?)double.MinValue, typeof(double?)))//,
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double?)double.MaxValue, typeof(double?)))//,
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double?)double.NaN, typeof(double?)))//,
                /* nan */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double?)double.NegativeInfinity, typeof(double?)))//,
                /* ∞ */
            );
            AssertEval(
                Expression.NegateChecked(Expression.Constant((double?)double.PositiveInfinity, typeof(double?)))//,
                /* -∞ */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_Boolean()
        {
            AssertEval(
                Expression.Not(Expression.Constant((bool)false, typeof(bool)))
                /* true */
            );
            AssertEval(
                Expression.Not(Expression.Constant((bool)true, typeof(bool)))
                /* false */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_NullableBoolean()
        {
            AssertEval(
                Expression.Not(Expression.Constant((bool?)null, typeof(bool?)))
                /* null */
            );
            AssertEval(
                Expression.Not(Expression.Constant((bool?)false, typeof(bool?)))//,
                /* true */
            );
            AssertEval(
                Expression.Not(Expression.Constant((bool?)true, typeof(bool?)))//,
                /* false */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_SByte()
        {
            AssertEval(
                Expression.Not(Expression.Constant((sbyte)-128, typeof(sbyte)))
                /* 127 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte)-1, typeof(sbyte)))
                /* 0 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte)0, typeof(sbyte)))
                /* -1 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte)1, typeof(sbyte)))
                /* -2 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte)42, typeof(sbyte)))
                /* -43 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte)127, typeof(sbyte)))
                /* -128 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_NullableSByte()
        {
            AssertEval(
                Expression.Not(Expression.Constant((sbyte?)null, typeof(sbyte?)))
                /* null */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte?)-128, typeof(sbyte?)))//,
                /* 127 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte?)-1, typeof(sbyte?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte?)0, typeof(sbyte?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte?)1, typeof(sbyte?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte?)42, typeof(sbyte?)))//,
                /* -43 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((sbyte?)127, typeof(sbyte?)))//,
                /* -128 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_Byte()
        {
            AssertEval(
                Expression.Not(Expression.Constant((byte)0, typeof(byte)))
                /* 255 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((byte)1, typeof(byte)))
                /* 254 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((byte)42, typeof(byte)))
                /* 213 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((byte)255, typeof(byte)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_NullableByte()
        {
            AssertEval(
                Expression.Not(Expression.Constant((byte?)null, typeof(byte?)))
                /* null */
            );
            AssertEval(
                Expression.Not(Expression.Constant((byte?)0, typeof(byte?)))//,
                /* 255 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((byte?)1, typeof(byte?)))//,
                /* 254 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((byte?)42, typeof(byte?)))//,
                /* 213 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((byte?)255, typeof(byte?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_Int16()
        {
            AssertEval(
                Expression.Not(Expression.Constant((short)-32768, typeof(short)))
                /* 32767 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short)-1, typeof(short)))
                /* 0 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short)0, typeof(short)))
                /* -1 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short)1, typeof(short)))
                /* -2 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short)42, typeof(short)))
                /* -43 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short)32767, typeof(short)))
                /* -32768 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_NullableInt16()
        {
            AssertEval(
                Expression.Not(Expression.Constant((short?)null, typeof(short?)))
                /* null */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short?)-32768, typeof(short?)))//,
                /* 32767 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short?)-1, typeof(short?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short?)0, typeof(short?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short?)1, typeof(short?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short?)42, typeof(short?)))//,
                /* -43 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((short?)32767, typeof(short?)))//,
                /* -32768 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_UInt16()
        {
            AssertEval(
                Expression.Not(Expression.Constant((ushort)0, typeof(ushort)))
                /* 65535 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ushort)1, typeof(ushort)))
                /* 65534 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ushort)42, typeof(ushort)))
                /* 65493 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ushort)65535, typeof(ushort)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_NullableUInt16()
        {
            AssertEval(
                Expression.Not(Expression.Constant((ushort?)null, typeof(ushort?)))
                /* null */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ushort?)0, typeof(ushort?)))//,
                /* 65535 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ushort?)1, typeof(ushort?)))//,
                /* 65534 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ushort?)42, typeof(ushort?)))//,
                /* 65493 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ushort?)65535, typeof(ushort?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_Int32()
        {
            AssertEval(
                Expression.Not(Expression.Constant((int)-2147483648, typeof(int)))
                /* 2147483647 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int)-1, typeof(int)))
                /* 0 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int)0, typeof(int)))
                /* -1 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int)1, typeof(int)))
                /* -2 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int)42, typeof(int)))
                /* -43 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int)2147483647, typeof(int)))
                /* -2147483648 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_NullableInt32()
        {
            AssertEval(
                Expression.Not(Expression.Constant((int?)null, typeof(int?)))
                /* null */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int?)-2147483648, typeof(int?)))//,
                /* 2147483647 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int?)-1, typeof(int?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int?)0, typeof(int?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int?)1, typeof(int?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int?)42, typeof(int?)))//,
                /* -43 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((int?)2147483647, typeof(int?)))//,
                /* -2147483648 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_UInt32()
        {
            AssertEval(
                Expression.Not(Expression.Constant((uint)0, typeof(uint)))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((uint)1, typeof(uint)))
                /* 4294967294 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((uint)42, typeof(uint)))
                /* 4294967253 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((uint)4294967295, typeof(uint)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_NullableUInt32()
        {
            AssertEval(
                Expression.Not(Expression.Constant((uint?)null, typeof(uint?)))
                /* null */
            );
            AssertEval(
                Expression.Not(Expression.Constant((uint?)0, typeof(uint?)))//,
                /* 4294967295 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((uint?)1, typeof(uint?)))//,
                /* 4294967294 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((uint?)42, typeof(uint?)))//,
                /* 4294967253 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((uint?)4294967295, typeof(uint?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_Int64()
        {
            AssertEval(
                Expression.Not(Expression.Constant((long)-9223372036854775808, typeof(long)))
                /* 9223372036854775807 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long)-1, typeof(long)))
                /* 0 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long)0, typeof(long)))
                /* -1 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long)1, typeof(long)))
                /* -2 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long)42, typeof(long)))
                /* -43 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long)9223372036854775807, typeof(long)))
                /* -9223372036854775808 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_NullableInt64()
        {
            AssertEval(
                Expression.Not(Expression.Constant((long?)null, typeof(long?)))
                /* null */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long?)-9223372036854775808, typeof(long?)))//,
                /* 9223372036854775807 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long?)-1, typeof(long?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long?)0, typeof(long?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long?)1, typeof(long?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long?)42, typeof(long?)))//,
                /* -43 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((long?)9223372036854775807, typeof(long?)))//,
                /* -9223372036854775808 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_UInt64()
        {
            AssertEval(
                Expression.Not(Expression.Constant((ulong)0, typeof(ulong)))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ulong)1, typeof(ulong)))
                /* 18446744073709551614 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ulong)42, typeof(ulong)))
                /* 18446744073709551573 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ulong)18446744073709551615, typeof(ulong)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_Not_Eval_NullableUInt64()
        {
            AssertEval(
                Expression.Not(Expression.Constant((ulong?)null, typeof(ulong?)))
                /* null */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ulong?)0, typeof(ulong?)))//,
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ulong?)1, typeof(ulong?)))//,
                /* 18446744073709551614 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ulong?)42, typeof(ulong?)))//,
                /* 18446744073709551573 */
            );
            AssertEval(
                Expression.Not(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_SByte()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte)-128, typeof(sbyte)))
                /* 127 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte)-1, typeof(sbyte)))
                /* 0 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte)0, typeof(sbyte)))
                /* -1 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte)1, typeof(sbyte)))
                /* -2 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte)42, typeof(sbyte)))
                /* -43 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte)127, typeof(sbyte)))
                /* -128 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_NullableSByte()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte?)null, typeof(sbyte?)))
                /* null */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte?)-128, typeof(sbyte?)))//,
                /* 127 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte?)-1, typeof(sbyte?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte?)0, typeof(sbyte?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte?)1, typeof(sbyte?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte?)42, typeof(sbyte?)))//,
                /* -43 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((sbyte?)127, typeof(sbyte?)))//,
                /* -128 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_Byte()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((byte)0, typeof(byte)))
                /* 255 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((byte)1, typeof(byte)))
                /* 254 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((byte)42, typeof(byte)))
                /* 213 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((byte)255, typeof(byte)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_NullableByte()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((byte?)null, typeof(byte?)))
                /* null */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((byte?)0, typeof(byte?)))//,
                /* 255 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((byte?)1, typeof(byte?)))//,
                /* 254 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((byte?)42, typeof(byte?)))//,
                /* 213 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((byte?)255, typeof(byte?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_Int16()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short)-32768, typeof(short)))
                /* 32767 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short)-1, typeof(short)))
                /* 0 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short)0, typeof(short)))
                /* -1 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short)1, typeof(short)))
                /* -2 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short)42, typeof(short)))
                /* -43 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short)32767, typeof(short)))
                /* -32768 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_NullableInt16()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short?)null, typeof(short?)))
                /* null */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short?)-32768, typeof(short?)))//,
                /* 32767 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short?)-1, typeof(short?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short?)0, typeof(short?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short?)1, typeof(short?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short?)42, typeof(short?)))//,
                /* -43 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((short?)32767, typeof(short?)))//,
                /* -32768 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_UInt16()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ushort)0, typeof(ushort)))
                /* 65535 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ushort)1, typeof(ushort)))
                /* 65534 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ushort)42, typeof(ushort)))
                /* 65493 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ushort)65535, typeof(ushort)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_NullableUInt16()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ushort?)null, typeof(ushort?)))
                /* null */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ushort?)0, typeof(ushort?)))//,
                /* 65535 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ushort?)1, typeof(ushort?)))//,
                /* 65534 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ushort?)42, typeof(ushort?)))//,
                /* 65493 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ushort?)65535, typeof(ushort?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_Int32()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int)-2147483648, typeof(int)))
                /* 2147483647 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int)-1, typeof(int)))
                /* 0 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int)0, typeof(int)))
                /* -1 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int)1, typeof(int)))
                /* -2 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int)42, typeof(int)))
                /* -43 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int)2147483647, typeof(int)))
                /* -2147483648 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_NullableInt32()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int?)null, typeof(int?)))
                /* null */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int?)-2147483648, typeof(int?)))//,
                /* 2147483647 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int?)-1, typeof(int?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int?)0, typeof(int?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int?)1, typeof(int?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int?)42, typeof(int?)))//,
                /* -43 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((int?)2147483647, typeof(int?)))//,
                /* -2147483648 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_UInt32()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((uint)0, typeof(uint)))
                /* 4294967295 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((uint)1, typeof(uint)))
                /* 4294967294 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((uint)42, typeof(uint)))
                /* 4294967253 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((uint)4294967295, typeof(uint)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_NullableUInt32()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((uint?)null, typeof(uint?)))
                /* null */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((uint?)0, typeof(uint?)))//,
                /* 4294967295 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((uint?)1, typeof(uint?)))//,
                /* 4294967294 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((uint?)42, typeof(uint?)))//,
                /* 4294967253 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((uint?)4294967295, typeof(uint?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_Int64()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long)-9223372036854775808, typeof(long)))
                /* 9223372036854775807 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long)-1, typeof(long)))
                /* 0 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long)0, typeof(long)))
                /* -1 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long)1, typeof(long)))
                /* -2 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long)42, typeof(long)))
                /* -43 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long)9223372036854775807, typeof(long)))
                /* -9223372036854775808 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_NullableInt64()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long?)null, typeof(long?)))
                /* null */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long?)-9223372036854775808, typeof(long?)))//,
                /* 9223372036854775807 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long?)-1, typeof(long?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long?)0, typeof(long?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long?)1, typeof(long?)))//,
                /* -2 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long?)42, typeof(long?)))//,
                /* -43 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((long?)9223372036854775807, typeof(long?)))//,
                /* -9223372036854775808 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_UInt64()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ulong)0, typeof(ulong)))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ulong)1, typeof(ulong)))
                /* 18446744073709551614 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ulong)42, typeof(ulong)))
                /* 18446744073709551573 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ulong)18446744073709551615, typeof(ulong)))
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_OnesComplement_Eval_NullableUInt64()
        {
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ulong?)null, typeof(ulong?)))
                /* null */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ulong?)0, typeof(ulong?)))//,
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ulong?)1, typeof(ulong?)))//,
                /* 18446744073709551614 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ulong?)42, typeof(ulong?)))//,
                /* 18446744073709551573 */
            );
            AssertEval(
                Expression.OnesComplement(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)))//,
                /* 0 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_Int16()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short)-32768, typeof(short)))
                /* -32768 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short)-1, typeof(short)))
                /* -1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short)0, typeof(short)))
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short)1, typeof(short)))
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short)42, typeof(short)))
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short)32767, typeof(short)))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_NullableInt16()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short?)null, typeof(short?)))
                /* null */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short?)-32768, typeof(short?)))//,
                /* -32768 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short?)-1, typeof(short?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short?)0, typeof(short?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short?)1, typeof(short?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short?)42, typeof(short?)))//,
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((short?)32767, typeof(short?)))//,
                /* 32767 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_UInt16()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ushort)0, typeof(ushort)))
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ushort)1, typeof(ushort)))
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ushort)42, typeof(ushort)))
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ushort)65535, typeof(ushort)))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_NullableUInt16()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ushort?)null, typeof(ushort?)))
                /* null */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ushort?)0, typeof(ushort?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ushort?)1, typeof(ushort?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ushort?)42, typeof(ushort?)))//,
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ushort?)65535, typeof(ushort?)))//,
                /* 65535 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_Int32()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int)-2147483648, typeof(int)))
                /* -2147483648 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int)-1, typeof(int)))
                /* -1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int)0, typeof(int)))
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int)1, typeof(int)))
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int)42, typeof(int)))
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int)2147483647, typeof(int)))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_NullableInt32()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int?)null, typeof(int?)))
                /* null */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int?)-2147483648, typeof(int?)))//,
                /* -2147483648 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int?)-1, typeof(int?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int?)0, typeof(int?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int?)1, typeof(int?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int?)42, typeof(int?)))//,
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((int?)2147483647, typeof(int?)))//,
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_UInt32()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((uint)0, typeof(uint)))
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((uint)1, typeof(uint)))
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((uint)42, typeof(uint)))
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((uint)4294967295, typeof(uint)))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_NullableUInt32()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((uint?)null, typeof(uint?)))
                /* null */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((uint?)0, typeof(uint?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((uint?)1, typeof(uint?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((uint?)42, typeof(uint?)))//,
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((uint?)4294967295, typeof(uint?)))//,
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_Int64()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long)-9223372036854775808, typeof(long)))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long)-1, typeof(long)))
                /* -1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long)0, typeof(long)))
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long)1, typeof(long)))
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long)42, typeof(long)))
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long)9223372036854775807, typeof(long)))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_NullableInt64()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long?)null, typeof(long?)))
                /* null */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long?)-9223372036854775808, typeof(long?)))//,
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long?)-1, typeof(long?)))//,
                /* -1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long?)0, typeof(long?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long?)1, typeof(long?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long?)42, typeof(long?)))//,
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((long?)9223372036854775807, typeof(long?)))//,
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_UInt64()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ulong)0, typeof(ulong)))
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ulong)1, typeof(ulong)))
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ulong)42, typeof(ulong)))
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ulong)18446744073709551615, typeof(ulong)))
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_NullableUInt64()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ulong?)null, typeof(ulong?)))
                /* null */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ulong?)0, typeof(ulong?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ulong?)1, typeof(ulong?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ulong?)42, typeof(ulong?)))//,
                /* 42 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)))//,
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_Single()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float)0, typeof(float)))
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float)1, typeof(float)))
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float)3.14, typeof(float)))
                /* 3.14 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float)float.MinValue, typeof(float)))
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float)float.MaxValue, typeof(float)))
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float)float.NaN, typeof(float)))
                /* nan */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float)float.NegativeInfinity, typeof(float)))
                /* -∞ */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float)float.PositiveInfinity, typeof(float)))
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_NullableSingle()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float?)null, typeof(float?)))
                /* null */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float?)0, typeof(float?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float?)1, typeof(float?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float?)3.14, typeof(float?)))//,
                /* 3.14 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float?)float.MinValue, typeof(float?)))//,
                /* -3.402823e+38 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float?)float.MaxValue, typeof(float?)))//,
                /* 3.402823e+38 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float?)float.NaN, typeof(float?)))//,
                /* nan */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float?)float.NegativeInfinity, typeof(float?)))//,
                /* -∞ */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((float?)float.PositiveInfinity, typeof(float?)))//,
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_Double()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double)0, typeof(double)))
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double)1, typeof(double)))
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double)3.14, typeof(double)))
                /* 3.14 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double)double.MinValue, typeof(double)))
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double)double.MaxValue, typeof(double)))
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double)double.NaN, typeof(double)))
                /* nan */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double)double.NegativeInfinity, typeof(double)))
                /* -∞ */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double)double.PositiveInfinity, typeof(double)))
                /* ∞ */
            );
        }

        [TestMethod]
        public void Unary_UnaryPlus_Eval_NullableDouble()
        {
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double?)null, typeof(double?)))
                /* null */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double?)0, typeof(double?)))//,
                /* 0 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double?)1, typeof(double?)))//,
                /* 1 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double?)3.14, typeof(double?)))//,
                /* 3.14 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double?)double.MinValue, typeof(double?)))//,
                /* -1.79769313486232e+308 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double?)double.MaxValue, typeof(double?)))//,
                /* 1.79769313486232e+308 */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double?)double.NaN, typeof(double?)))//,
                /* nan */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double?)double.NegativeInfinity, typeof(double?)))//,
                /* -∞ */
            );
            AssertEval(
                Expression.UnaryPlus(Expression.Constant((double?)double.PositiveInfinity, typeof(double?)))//,
                /* ∞ */
            );
        }

    }
}
