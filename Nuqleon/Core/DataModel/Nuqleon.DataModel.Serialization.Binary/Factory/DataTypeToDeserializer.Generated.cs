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
    partial class DataTypeToDeserializer
    {
        private static readonly Lazy<MethodInfo> s_readSByte = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadSByte(default(Stream))));

        private static LambdaExpression VisitSByte(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_SBYTE;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (SByte?)null : (SByte?)stream.ReadSByte();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readSByte.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadSByte();
                    Expression.Call(s_readSByte.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readInt16 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadInt16(default(Stream))));

        private static LambdaExpression VisitInt16(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_INT16;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (Int16?)null : (Int16?)stream.ReadInt16();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readInt16.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadInt16();
                    Expression.Call(s_readInt16.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readUInt16 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadUInt16(default(Stream))));

        private static LambdaExpression VisitUInt16(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_UINT16;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (UInt16?)null : (UInt16?)stream.ReadUInt16();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readUInt16.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadUInt16();
                    Expression.Call(s_readUInt16.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readInt32 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadInt32(default(Stream))));

        private static LambdaExpression VisitInt32(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_INT32;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (Int32?)null : (Int32?)stream.ReadInt32();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readInt32.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadInt32();
                    Expression.Call(s_readInt32.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readUInt32 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadUInt32(default(Stream))));

        private static LambdaExpression VisitUInt32(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_UINT32;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (UInt32?)null : (UInt32?)stream.ReadUInt32();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readUInt32.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadUInt32();
                    Expression.Call(s_readUInt32.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readInt64 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadInt64(default(Stream))));

        private static LambdaExpression VisitInt64(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_INT64;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (Int64?)null : (Int64?)stream.ReadInt64();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readInt64.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadInt64();
                    Expression.Call(s_readInt64.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readUInt64 = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadUInt64(default(Stream))));

        private static LambdaExpression VisitUInt64(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_UINT64;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (UInt64?)null : (UInt64?)stream.ReadUInt64();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readUInt64.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadUInt64();
                    Expression.Call(s_readUInt64.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readSingle = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadSingle(default(Stream))));

        private static LambdaExpression VisitSingle(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_SINGLE;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (Single?)null : (Single?)stream.ReadSingle();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readSingle.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadSingle();
                    Expression.Call(s_readSingle.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readDouble = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadDouble(default(Stream))));

        private static LambdaExpression VisitDouble(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_DOUBLE;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (Double?)null : (Double?)stream.ReadDouble();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readDouble.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadDouble();
                    Expression.Call(s_readDouble.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readDecimal = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadDecimal(default(Stream))));

        private static LambdaExpression VisitDecimal(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_DECIMAL;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (Decimal?)null : (Decimal?)stream.ReadDecimal();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readDecimal.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadDecimal();
                    Expression.Call(s_readDecimal.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readChar = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadChar(default(Stream))));

        private static LambdaExpression VisitChar(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_CHAR;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (Char?)null : (Char?)stream.ReadChar();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readChar.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadChar();
                    Expression.Call(s_readChar.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readBoolean = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadBoolean(default(Stream))));

        private static LambdaExpression VisitBoolean(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_BOOLEAN;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (Boolean?)null : (Boolean?)stream.ReadBoolean();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readBoolean.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadBoolean();
                    Expression.Call(s_readBoolean.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static readonly Lazy<MethodInfo> s_readGuid = new Lazy<MethodInfo>(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadGuid(default(Stream))));

        private static LambdaExpression VisitGuid(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_GUID;
            var body = default(Expression);
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = stream.ReadByte();
                    Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                    // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (Guid?)null : (Guid?)stream.ReadGuid();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_readGuid.Value, streamParameter), type)
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // if (stream.ReadByte() != typeCode) throw new InvalidDataException("Unexpected type code.");
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Call(ReflectionConstants.ReadByte, streamParameter), Expression.Constant(typeCode, typeof(byte))),
                        ReflectionConstants.ThrowUnexpectedTypeCode
                    ),
                    // return stream.ReadGuid();
                    Expression.Call(s_readGuid.Value, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

    }
}
