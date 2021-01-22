// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//


using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void Convert_Boolean_Boolean()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(bool))
                /* false */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(bool))
                /* true */
            );
        }

        [TestMethod]
        public void Convert_Boolean_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(sbyte))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(byte))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(short))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(ushort))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(int))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(uint))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(long))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(ulong))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(float))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(double))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableBoolean()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(bool?))
                /* false */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(bool?))
                /* true */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(sbyte?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(byte?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(short?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(ushort?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(int?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(uint?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(long?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(ulong?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(float?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_Boolean_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((bool)false, typeof(bool)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool)true, typeof(bool)), typeof(double?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_SByte_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(sbyte))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(sbyte))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(byte))
                /* 128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(byte))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(byte))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(short))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(short))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(ushort))
                /* 65408 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(ushort))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(ushort))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(int))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(int))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(uint))
                /* 4294967168 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(uint))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(uint))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(long))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(long))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(ulong))
                /* 18446744073709551488 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(ulong))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(ulong))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(float))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(float))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(double))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(double))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(sbyte?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(sbyte?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(byte?))
                /* 128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(byte?))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(byte?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(short?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(short?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(ushort?))
                /* 65408 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(ushort?))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(ushort?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(int?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(int?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(uint?))
                /* 4294967168 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(uint?))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(uint?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(long?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(long?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(ulong?))
                /* 18446744073709551488 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(ulong?))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(ulong?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(float?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(float?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_SByte_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(double?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(double?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_Byte_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Byte_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(short))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(ushort))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(int))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(uint))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(long))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(ulong))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(float))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(double))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(short?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(ushort?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(int?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(uint?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(long?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(ulong?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(float?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Byte_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((byte)0, typeof(byte)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)1, typeof(byte)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)42, typeof(byte)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte)255, typeof(byte)), typeof(double?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Int16_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int16_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(byte))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Int16_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(short))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(short))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(ushort))
                /* 32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(ushort))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(ushort))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(int))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(int))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(uint))
                /* 4294934528 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(uint))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(uint))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(long))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(long))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(ulong))
                /* 18446744073709518848 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(ulong))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(ulong))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(float))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(float))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(double))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(double))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(byte?))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(short?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(short?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(ushort?))
                /* 32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(ushort?))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(ushort?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(int?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(int?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(uint?))
                /* 4294934528 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(uint?))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(uint?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(long?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(long?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(ulong?))
                /* 18446744073709518848 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(ulong?))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(ulong?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(float?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(float?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_Int16_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((short)-32768, typeof(short)), typeof(double?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)-1, typeof(short)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)0, typeof(short)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)1, typeof(short)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)42, typeof(short)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short)32767, typeof(short)), typeof(double?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(int))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(uint))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(long))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(ulong))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(float))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(double))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(int?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(uint?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(long?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(ulong?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(float?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt16_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)0, typeof(ushort)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)1, typeof(ushort)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)42, typeof(ushort)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort)65535, typeof(ushort)), typeof(double?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_Int32_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int32_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(byte))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Int32_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int32_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(ushort))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_Int32_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(int))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(int))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_Int32_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(uint))
                /* 2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(uint))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(uint))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_Int32_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(long))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(long))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_Int32_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(ulong))
                /* 18446744071562067968 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(ulong))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(ulong))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_Int32_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(float))
                /* -2.147484e+09 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(float))
                /* 2.147484e+09 */
            );
        }

        [TestMethod]
        public void Convert_Int32_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(double))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(double))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(byte?))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(ushort?))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(int?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(int?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(uint?))
                /* 2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(uint?))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(uint?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(long?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(long?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(ulong?))
                /* 18446744071562067968 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(ulong?))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(ulong?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(float?))
                /* -2.147484e+09 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(float?))
                /* 2.147484e+09 */
            );
        }

        [TestMethod]
        public void Convert_Int32_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((int)-2147483648, typeof(int)), typeof(double?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)-1, typeof(int)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)0, typeof(int)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)1, typeof(int)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)42, typeof(int)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int)2147483647, typeof(int)), typeof(double?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(int))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(uint))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(long))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(ulong))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(float))
                /* 4.294967e+09 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(double))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(int?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(uint?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(long?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(ulong?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(float?))
                /* 4.294967e+09 */
            );
        }

        [TestMethod]
        public void Convert_UInt32_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((uint)0, typeof(uint)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)1, typeof(uint)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)42, typeof(uint)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint)4294967295, typeof(uint)), typeof(double?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_Int64_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int64_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(byte))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Int64_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int64_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(ushort))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_Int64_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(int))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int64_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(uint))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(uint))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_Int64_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(long))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(long))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Convert_Int64_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(ulong))
                /* 9223372036854775808 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(ulong))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(ulong))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Convert_Int64_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(float))
                /* -9.223372e+18 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(float))
                /* 9.223372e+18 */
            );
        }

        [TestMethod]
        public void Convert_Int64_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(double))
                /* -9.22337203685478e+18 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(double))
                /* 9.22337203685478e+18 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(byte?))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(ushort?))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(int?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(uint?))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(uint?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(long?))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(long?))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(ulong?))
                /* 9223372036854775808 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(ulong?))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(ulong?))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(float?))
                /* -9.223372e+18 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(float?))
                /* 9.223372e+18 */
            );
        }

        [TestMethod]
        public void Convert_Int64_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(double?))
                /* -9.22337203685478e+18 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)-1, typeof(long)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)0, typeof(long)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)1, typeof(long)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)42, typeof(long)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(double?))
                /* 9.22337203685478e+18 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(int))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(uint))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(long))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(ulong))
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(float))
                /* 1.844674e+19 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(double))
                /* 1.84467440737096e+19 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(int?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(uint?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(long?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(ulong?))
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(float?))
                /* 1.844674e+19 */
            );
        }

        [TestMethod]
        public void Convert_UInt64_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)0, typeof(ulong)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)1, typeof(ulong)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)42, typeof(ulong)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(double?))
                /* 1.84467440737096e+19 */
            );
        }

        [TestMethod]
        public void Convert_Single_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(sbyte))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(byte))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(short))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(ushort))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(int))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(uint))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(long))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(ulong))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(float))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_Single_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(double))
                /* 3.14000010490417 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(sbyte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(byte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(short?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(ushort?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(int?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(uint?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(long?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(ulong?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(float?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_Single_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((float)0, typeof(float)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)1, typeof(float)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float)3.14, typeof(float)), typeof(double?))
                /* 3.14000010490417 */
            );
        }

        [TestMethod]
        public void Convert_Double_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(sbyte))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(byte))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(short))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(ushort))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(int))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(uint))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(long))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(ulong))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(float))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_Double_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(double))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(sbyte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(byte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(short?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(ushort?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(int?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(uint?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(long?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(ulong?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(float?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_Double_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant((double)0, typeof(double)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)1, typeof(double)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double)3.14, typeof(double)), typeof(double?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_Boolean()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(bool))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(bool))
                /* false */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(bool))
                /* true */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(sbyte))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(byte))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(short))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(ushort))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(int))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(uint))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(long))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(ulong))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(float))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(double))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableBoolean()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(bool?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(bool?))
                /* false */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(bool?))
                /* true */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(sbyte?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(byte?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(short?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(ushort?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(int?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(uint?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(long?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(ulong?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(float?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableBoolean_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(bool?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)false, typeof(bool?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((bool?)true, typeof(bool?)), typeof(double?))
                /* 1 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(sbyte))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(sbyte))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(byte))
                /* 128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(byte))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(byte))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(short))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(short))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(ushort))
                /* 65408 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(ushort))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(ushort))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(int))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(int))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(uint))
                /* 4294967168 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(uint))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(uint))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(long))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(long))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(ulong))
                /* 18446744073709551488 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(ulong))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(ulong))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(float))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(float))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(double))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(double))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(sbyte?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(sbyte?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(byte?))
                /* 128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(byte?))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(byte?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(short?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(short?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(ushort?))
                /* 65408 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(ushort?))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(ushort?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(int?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(int?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(uint?))
                /* 4294967168 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(uint?))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(uint?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(long?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(long?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(ulong?))
                /* 18446744073709551488 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(ulong?))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(ulong?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(float?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(float?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableSByte_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(sbyte?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(double?))
                /* -128 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(double?))
                /* 127 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(short))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(ushort))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(int))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(uint))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(long))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(ulong))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(float))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(double))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(short?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(ushort?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(int?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(uint?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(long?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(ulong?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(float?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableByte_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(byte?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)0, typeof(byte?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)1, typeof(byte?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)42, typeof(byte?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((byte?)255, typeof(byte?)), typeof(double?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(byte))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(short))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(short))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(ushort))
                /* 32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(ushort))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(ushort))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(int))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(int))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(uint))
                /* 4294934528 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(uint))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(uint))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(long))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(long))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(ulong))
                /* 18446744073709518848 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(ulong))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(ulong))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(float))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(float))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(double))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(double))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(byte?))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(short?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(short?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(ushort?))
                /* 32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(ushort?))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(ushort?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(int?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(int?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(uint?))
                /* 4294934528 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(uint?))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(uint?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(long?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(long?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(ulong?))
                /* 18446744073709518848 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(ulong?))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(ulong?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(float?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(float?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt16_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(short?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-32768, typeof(short?)), typeof(double?))
                /* -32768 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)-1, typeof(short?)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)0, typeof(short?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)1, typeof(short?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)42, typeof(short?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((short?)32767, typeof(short?)), typeof(double?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(int))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(uint))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(long))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(ulong))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(float))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(double))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(int?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(uint?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(long?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(ulong?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(float?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt16_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ushort?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(double?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(byte))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(ushort))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(int))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(int))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(uint))
                /* 2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(uint))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(uint))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(long))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(long))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(ulong))
                /* 18446744071562067968 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(ulong))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(ulong))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(float))
                /* -2.147484e+09 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(float))
                /* 2.147484e+09 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(double))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(double))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(byte?))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(ushort?))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(int?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(int?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(uint?))
                /* 2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(uint?))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(uint?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(long?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(long?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(ulong?))
                /* 18446744071562067968 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(ulong?))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(ulong?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(float?))
                /* -2.147484e+09 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(float?))
                /* 2.147484e+09 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt32_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(int?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(double?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)-1, typeof(int?)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)0, typeof(int?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)1, typeof(int?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)42, typeof(int?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((int?)2147483647, typeof(int?)), typeof(double?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(int))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(uint))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(long))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(ulong))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(float))
                /* 4.294967e+09 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(double))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(int?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(uint?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(long?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(ulong?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(float?))
                /* 4.294967e+09 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt32_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(uint?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)0, typeof(uint?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)1, typeof(uint?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)42, typeof(uint?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(double?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(byte))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(ushort))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(int))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(uint))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(uint))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(long))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(long))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(ulong))
                /* 9223372036854775808 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(ulong))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(ulong))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(float))
                /* -9.223372e+18 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(float))
                /* 9.223372e+18 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(double))
                /* -9.22337203685478e+18 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(double))
                /* 9.22337203685478e+18 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(byte?))
                /* 255 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(ushort?))
                /* 65535 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(int?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(uint?))
                /* 4294967295 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(uint?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(long?))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(long?))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(ulong?))
                /* 9223372036854775808 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(ulong?))
                /* 18446744073709551615 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(ulong?))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(float?))
                /* -9.223372e+18 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(float?))
                /* 9.223372e+18 */
            );
        }

        [TestMethod]
        public void Convert_NullableInt64_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(long?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(double?))
                /* -9.22337203685478e+18 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)-1, typeof(long?)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)0, typeof(long?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)1, typeof(long?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)42, typeof(long?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(double?))
                /* 9.22337203685478e+18 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(sbyte))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(short))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(int))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(uint))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(long))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(ulong))
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(float))
                /* 1.844674e+19 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(double))
                /* 1.84467440737096e+19 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(sbyte?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(short?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(int?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(uint?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(long?))
                /* -1 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(ulong?))
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(float?))
                /* 1.844674e+19 */
            );
        }

        [TestMethod]
        public void Convert_NullableUInt64_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(ulong?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(double?))
                /* 1.84467440737096e+19 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(sbyte))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(byte))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(short))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(ushort))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(int))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(uint))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(long))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(ulong))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(float))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(double))
                /* 3.14000010490417 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(sbyte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(byte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(short?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(ushort?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(int?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(uint?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(long?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(ulong?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(float?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_NullableSingle_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(float?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)0, typeof(float?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)1, typeof(float?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((float?)3.14, typeof(float?)), typeof(double?))
                /* 3.14000010490417 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_SByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(sbyte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(sbyte))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_Byte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(byte))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(byte))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_Int16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(short))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(short))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_UInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(ushort))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(ushort))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_Int32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(int))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(int))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_UInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(uint))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(uint))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_Int64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(long))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(long))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_UInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(ulong))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(ulong))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_Single()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(float))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(float))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_Double()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(double))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(double))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableSByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(sbyte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableByte()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(byte?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(byte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(short?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(short?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableUInt16()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(ushort?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(ushort?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(int?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(int?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableUInt32()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(uint?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(uint?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(long?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(long?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableUInt64()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(ulong?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(ulong?))
                /* 3 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableSingle()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(float?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(float?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void Convert_NullableDouble_NullableDouble()
        {
            AssertEval(
                Expression.Convert(Expression.Constant(value: null, typeof(double?)), typeof(double?))
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)0, typeof(double?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)1, typeof(double?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.Convert(Expression.Constant((double?)3.14, typeof(double?)), typeof(double?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_Boolean()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(bool))
                /* false */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(bool))
                /* true */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(sbyte))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(byte))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(short))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(ushort))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(int))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(uint))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(long))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(ulong))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(float))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(double))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableBoolean()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(bool?))
                /* false */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(bool?))
                /* true */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(sbyte?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(byte?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(short?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(ushort?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(int?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(uint?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(long?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(ulong?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(float?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Boolean_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)false, typeof(bool)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool)true, typeof(bool)), typeof(double?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(sbyte))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(sbyte))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(byte))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(short))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(short))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(ushort))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(int))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(int))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(uint))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(long))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(long))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(ulong))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(float))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(float))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(double))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(double))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(sbyte?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(sbyte?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(byte?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(short?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(short?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(ushort?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(int?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(int?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(uint?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(long?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(long?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(ulong?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(float?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(float?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_SByte_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-128, typeof(sbyte)), typeof(double?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)-1, typeof(sbyte)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)0, typeof(sbyte)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)1, typeof(sbyte)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)42, typeof(sbyte)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte)127, typeof(sbyte)), typeof(double?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(short))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(ushort))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(int))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(uint))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(long))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(ulong))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(float))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(double))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(short?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(ushort?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(int?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(uint?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(long?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(ulong?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(float?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Byte_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)0, typeof(byte)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)1, typeof(byte)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)42, typeof(byte)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte)255, typeof(byte)), typeof(double?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(short))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(short))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(ushort))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(int))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(int))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(uint))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(long))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(long))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(ulong))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(float))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(float))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(double))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(double))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(short?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(short?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(ushort?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(int?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(int?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(uint?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(long?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(long?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(ulong?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(float?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(float?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int16_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-32768, typeof(short)), typeof(double?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)-1, typeof(short)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)0, typeof(short)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)1, typeof(short)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)42, typeof(short)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short)32767, typeof(short)), typeof(double?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(int))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(uint))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(long))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(ulong))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(float))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(double))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(int?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(uint?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(long?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(ulong?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(float?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt16_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)0, typeof(ushort)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)1, typeof(ushort)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)42, typeof(ushort)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort)65535, typeof(ushort)), typeof(double?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(int))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(int))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(uint))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(long))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(long))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(ulong))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(float))
                /* -2.147484e+09 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(float))
                /* 2.147484e+09 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(double))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(double))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(int?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(int?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(uint?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(long?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(long?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(ulong?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(float?))
                /* -2.147484e+09 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(float?))
                /* 2.147484e+09 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int32_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-2147483648, typeof(int)), typeof(double?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)-1, typeof(int)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)0, typeof(int)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)1, typeof(int)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)42, typeof(int)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int)2147483647, typeof(int)), typeof(double?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(int))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(uint))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(long))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(ulong))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(float))
                /* 4.294967e+09 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(double))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(int?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(uint?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(long?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(ulong?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(float?))
                /* 4.294967e+09 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt32_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)0, typeof(uint)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)1, typeof(uint)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)42, typeof(uint)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint)4294967295, typeof(uint)), typeof(double?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(int))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(int))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(long))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(long))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(ulong))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(float))
                /* -9.223372e+18 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(float))
                /* 9.223372e+18 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(double))
                /* -9.22337203685478e+18 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(double))
                /* 9.22337203685478e+18 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(int?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(int?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(long?))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(long?))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(ulong?))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(float?))
                /* -9.223372e+18 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(float?))
                /* 9.223372e+18 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Int64_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-9223372036854775808, typeof(long)), typeof(double?))
                /* -9.22337203685478e+18 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)-1, typeof(long)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)0, typeof(long)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)1, typeof(long)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)42, typeof(long)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long)9223372036854775807, typeof(long)), typeof(double?))
                /* 9.22337203685478e+18 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(int))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(long))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(ulong))
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(float))
                /* 1.844674e+19 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(double))
                /* 1.84467440737096e+19 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(int?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(long?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(ulong?))
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(float?))
                /* 1.844674e+19 */
            );
        }

        [TestMethod]
        public void ConvertChecked_UInt64_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)0, typeof(ulong)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)1, typeof(ulong)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)42, typeof(ulong)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong)18446744073709551615, typeof(ulong)), typeof(double?))
                /* 1.84467440737096e+19 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(sbyte))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(byte))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(short))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(ushort))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(int))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(uint))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(long))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(ulong))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(float))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(double))
                /* 3.14000010490417 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(sbyte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(byte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(short?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(ushort?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(int?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(uint?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(long?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(ulong?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(float?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Single_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)0, typeof(float)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)1, typeof(float)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float)3.14, typeof(float)), typeof(double?))
                /* 3.14000010490417 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(sbyte))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(byte))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(short))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(ushort))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(int))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(uint))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(long))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(ulong))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(float))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(double))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(sbyte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(byte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(short?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(ushort?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(int?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(uint?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(long?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(ulong?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(float?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_Double_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)0, typeof(double)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)1, typeof(double)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double)3.14, typeof(double)), typeof(double?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_Boolean()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(bool))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(bool))
                /* false */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(bool))
                /* true */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(sbyte))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(byte))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(short))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(ushort))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(int))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(uint))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(long))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(ulong))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(float))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(double))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableBoolean()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(bool?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(bool?))
                /* false */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(bool?))
                /* true */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(sbyte?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(byte?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(short?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(ushort?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(int?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(uint?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(long?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(ulong?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(float?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableBoolean_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(bool?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)false, typeof(bool?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((bool?)true, typeof(bool?)), typeof(double?))
                /* 1 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(sbyte))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(sbyte))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(byte))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(short))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(short))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(ushort))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(int))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(int))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(uint))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(long))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(long))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(ulong))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(float))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(float))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(double))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(double))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(sbyte?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(sbyte?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(byte?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(short?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(short?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(ushort?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(int?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(int?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(uint?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(long?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(long?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(ulong?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(float?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(float?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSByte_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(sbyte?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-128, typeof(sbyte?)), typeof(double?))
                /* -128 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)-1, typeof(sbyte?)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)0, typeof(sbyte?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)1, typeof(sbyte?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)42, typeof(sbyte?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((sbyte?)127, typeof(sbyte?)), typeof(double?))
                /* 127 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(byte))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(short))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(ushort))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(int))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(uint))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(long))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(ulong))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(float))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(double))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(byte?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(short?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(ushort?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(int?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(uint?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(long?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(ulong?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(float?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableByte_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(byte?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)0, typeof(byte?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)1, typeof(byte?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)42, typeof(byte?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((byte?)255, typeof(byte?)), typeof(double?))
                /* 255 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(short))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(short))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(ushort))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(int))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(int))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(uint))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(long))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(long))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(ulong))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(float))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(float))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(double))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(double))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(short?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(short?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(ushort?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(int?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(int?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(uint?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(long?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(long?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(ulong?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(float?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(float?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt16_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(short?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-32768, typeof(short?)), typeof(double?))
                /* -32768 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)-1, typeof(short?)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)0, typeof(short?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)1, typeof(short?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)42, typeof(short?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((short?)32767, typeof(short?)), typeof(double?))
                /* 32767 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(ushort))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(int))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(uint))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(long))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(ulong))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(float))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(double))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(ushort?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(int?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(uint?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(long?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(ulong?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(float?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt16_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ushort?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)0, typeof(ushort?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)1, typeof(ushort?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)42, typeof(ushort?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ushort?)65535, typeof(ushort?)), typeof(double?))
                /* 65535 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(int))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(int))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(uint))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(long))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(long))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(ulong))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(float))
                /* -2.147484e+09 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(float))
                /* 2.147484e+09 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(double))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(double))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(int?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(int?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(uint?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(long?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(long?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(ulong?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(float?))
                /* -2.147484e+09 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(float?))
                /* 2.147484e+09 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt32_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(int?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-2147483648, typeof(int?)), typeof(double?))
                /* -2147483648 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)-1, typeof(int?)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)0, typeof(int?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)1, typeof(int?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)42, typeof(int?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((int?)2147483647, typeof(int?)), typeof(double?))
                /* 2147483647 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(int))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(uint))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(long))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(ulong))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(float))
                /* 4.294967e+09 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(double))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(int?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(uint?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(long?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(ulong?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(float?))
                /* 4.294967e+09 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt32_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(uint?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)0, typeof(uint?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)1, typeof(uint?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)42, typeof(uint?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((uint?)4294967295, typeof(uint?)), typeof(double?))
                /* 4294967295 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(sbyte))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(short))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(int))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(int))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(int))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(long))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(long))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(long))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(ulong))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(ulong))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(float))
                /* -9.223372e+18 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(float))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(float))
                /* 9.223372e+18 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(double))
                /* -9.22337203685478e+18 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(double))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(double))
                /* 9.22337203685478e+18 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(sbyte?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(short?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(int?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(int?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(int?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(long?))
                /* -9223372036854775808 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(long?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(long?))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(ulong?))
                /* typeof(global::System.OverflowException */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(ulong?))
                /* 9223372036854775807 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(float?))
                /* -9.223372e+18 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(float?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(float?))
                /* 9.223372e+18 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableInt64_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(long?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-9223372036854775808, typeof(long?)), typeof(double?))
                /* -9.22337203685478e+18 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)-1, typeof(long?)), typeof(double?))
                /* -1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)0, typeof(long?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)1, typeof(long?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)42, typeof(long?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((long?)9223372036854775807, typeof(long?)), typeof(double?))
                /* 9.22337203685478e+18 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(sbyte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(sbyte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(byte))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(byte))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(short))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(short))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(ushort))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(ushort))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(int))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(int))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(uint))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(uint))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(long))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(long))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(ulong))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(ulong))
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(float))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(float))
                /* 1.844674e+19 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(double))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(double))
                /* 1.84467440737096e+19 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(sbyte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(sbyte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(byte?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(byte?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(short?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(short?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(ushort?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(ushort?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(int?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(int?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(uint?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(uint?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(long?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(long?))
                /* typeof(global::System.OverflowException */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(ulong?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(ulong?))
                /* 18446744073709551615 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(float?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(float?))
                /* 1.844674e+19 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableUInt64_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(ulong?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)0, typeof(ulong?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)1, typeof(ulong?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)42, typeof(ulong?)), typeof(double?))
                /* 42 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((ulong?)18446744073709551615, typeof(ulong?)), typeof(double?))
                /* 1.84467440737096e+19 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(sbyte))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(byte))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(short))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(ushort))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(int))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(uint))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(long))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(ulong))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(float))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(double))
                /* 3.14000010490417 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(sbyte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(byte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(short?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(ushort?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(int?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(uint?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(long?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(ulong?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(float?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableSingle_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(float?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)0, typeof(float?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)1, typeof(float?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((float?)3.14, typeof(float?)), typeof(double?))
                /* 3.14000010490417 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_SByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(sbyte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(sbyte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(sbyte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(sbyte))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_Byte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(byte))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(byte))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(byte))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(byte))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_Int16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(short))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(short))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(short))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(short))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_UInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(ushort))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(ushort))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(ushort))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(ushort))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_Int32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(int))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(int))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(int))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(int))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_UInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(uint))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(uint))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(uint))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(uint))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_Int64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(long))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(long))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(long))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(long))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_UInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(ulong))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(ulong))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(ulong))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(ulong))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_Single()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(float))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(float))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(float))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(float))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_Double()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(double))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(double))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(double))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(double))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableSByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(sbyte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(sbyte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(sbyte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(sbyte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableByte()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(byte?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(byte?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(byte?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(byte?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(short?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(short?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(short?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(short?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableUInt16()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(ushort?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(ushort?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(ushort?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(ushort?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(int?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(int?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(int?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(int?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableUInt32()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(uint?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(uint?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(uint?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(uint?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(long?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(long?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(long?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(long?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableUInt64()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(ulong?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(ulong?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(ulong?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(ulong?))
                /* 3 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableSingle()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(float?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(float?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(float?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(float?))
                /* 3.14 */
            );
        }

        [TestMethod]
        public void ConvertChecked_NullableDouble_NullableDouble()
        {
            AssertEval(
                Expression.ConvertChecked(Expression.Constant(value: null, typeof(double?)), typeof(double?))
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)0, typeof(double?)), typeof(double?))
                /* 0 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)1, typeof(double?)), typeof(double?))
                /* 1 */
            );
            AssertEval(
                Expression.ConvertChecked(Expression.Constant((double?)3.14, typeof(double?)), typeof(double?))
                /* 3.14 */
            );
        }

    }
}
