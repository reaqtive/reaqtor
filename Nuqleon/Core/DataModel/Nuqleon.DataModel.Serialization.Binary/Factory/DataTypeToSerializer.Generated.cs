// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Nuqleon.DataModel.Serialization.Binary
{
    partial class DataTypeToSerializer
    {
        private static readonly Lazy<MethodInfo> s_writeSByte = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteSByte(default(Stream), default(SByte))));

        private static LambdaExpression VisitSByte(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_SBYTE;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteSByte((SByte)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeSByte.Value, streamParameter, Expression.Convert(valueParameter, typeof(SByte)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteSByte(value);
                    Expression.Call(s_writeSByte.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeInt16 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteInt16(default(Stream), default(Int16))));

        private static LambdaExpression VisitInt16(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_INT16;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteInt16((Int16)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeInt16.Value, streamParameter, Expression.Convert(valueParameter, typeof(Int16)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteInt16(value);
                    Expression.Call(s_writeInt16.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeUInt16 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteUInt16(default(Stream), default(UInt16))));

        private static LambdaExpression VisitUInt16(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_UINT16;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteUInt16((UInt16)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeUInt16.Value, streamParameter, Expression.Convert(valueParameter, typeof(UInt16)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteUInt16(value);
                    Expression.Call(s_writeUInt16.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeInt32 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteInt32(default(Stream), default(Int32))));

        private static LambdaExpression VisitInt32(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_INT32;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteInt32((Int32)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeInt32.Value, streamParameter, Expression.Convert(valueParameter, typeof(Int32)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteInt32(value);
                    Expression.Call(s_writeInt32.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeUInt32 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteUInt32(default(Stream), default(UInt32))));

        private static LambdaExpression VisitUInt32(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_UINT32;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteUInt32((UInt32)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeUInt32.Value, streamParameter, Expression.Convert(valueParameter, typeof(UInt32)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteUInt32(value);
                    Expression.Call(s_writeUInt32.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeInt64 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteInt64(default(Stream), default(Int64))));

        private static LambdaExpression VisitInt64(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_INT64;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteInt64((Int64)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeInt64.Value, streamParameter, Expression.Convert(valueParameter, typeof(Int64)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteInt64(value);
                    Expression.Call(s_writeInt64.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeUInt64 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteUInt64(default(Stream), default(UInt64))));

        private static LambdaExpression VisitUInt64(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_UINT64;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteUInt64((UInt64)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeUInt64.Value, streamParameter, Expression.Convert(valueParameter, typeof(UInt64)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteUInt64(value);
                    Expression.Call(s_writeUInt64.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeSingle = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteSingle(default(Stream), default(Single))));

        private static LambdaExpression VisitSingle(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_SINGLE;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteSingle((Single)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeSingle.Value, streamParameter, Expression.Convert(valueParameter, typeof(Single)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteSingle(value);
                    Expression.Call(s_writeSingle.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeDouble = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteDouble(default(Stream), default(Double))));

        private static LambdaExpression VisitDouble(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_DOUBLE;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteDouble((Double)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeDouble.Value, streamParameter, Expression.Convert(valueParameter, typeof(Double)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteDouble(value);
                    Expression.Call(s_writeDouble.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeDecimal = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteDecimal(default(Stream), default(Decimal))));

        private static LambdaExpression VisitDecimal(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_DECIMAL;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteDecimal((Decimal)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeDecimal.Value, streamParameter, Expression.Convert(valueParameter, typeof(Decimal)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteDecimal(value);
                    Expression.Call(s_writeDecimal.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeChar = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteChar(default(Stream), default(Char))));

        private static LambdaExpression VisitChar(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_CHAR;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteChar((Char)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeChar.Value, streamParameter, Expression.Convert(valueParameter, typeof(Char)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteChar(value);
                    Expression.Call(s_writeChar.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeBoolean = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteBoolean(default(Stream), default(Boolean))));

        private static LambdaExpression VisitBoolean(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_BOOLEAN;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteBoolean((Boolean)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeBoolean.Value, streamParameter, Expression.Convert(valueParameter, typeof(Boolean)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteBoolean(value);
                    Expression.Call(s_writeBoolean.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_writeGuid = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteGuid(default(Stream), default(Guid))));

        private static LambdaExpression VisitGuid(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_GUID;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) stream.WriteGuid((Guid)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeGuid.Value, streamParameter, Expression.Convert(valueParameter, typeof(Guid)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteGuid(value);
                    Expression.Call(s_writeGuid.Value, streamParameter, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

    }
}
