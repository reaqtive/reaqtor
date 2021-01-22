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
    internal partial class DataTypeToDeserializer
    {
        private static LambdaExpression VisitUnit(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_UNIT;
            Expression body;
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
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? null : default(Unit);
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Default(typeof(Unit)), type)
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
                    // return default(Unit)
                    Expression.Default(typeof(Unit))
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static LambdaExpression VisitByte(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_BYTE;
            Expression body;
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
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE)  ? (Byte?)null : (Byte?)stream.ReadByte();
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(ReflectionConstants.ReadByte, streamParameter), type)
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
                    // return stream.ReadByte();
                    Expression.Call(ReflectionConstants.ReadByte, streamParameter)
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

        private static LambdaExpression VisitString(Type type)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");

            var typeCode = Protocol.TYPE_STRING;
            var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
            var body = Expression.Block(
                // byte tc;
                new[] { tcParameter },
                // tc = stream.ReadByte();
                Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                Expression.IfThen(
                    Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                    ReflectionConstants.ThrowUnexpectedTypeCode
                ),
                // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? null : stream.ReadString();
                Expression.Condition(
                    Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                    Expression.Convert(ReflectionConstants.NullObject, type),
                    Expression.Call(ReflectionConstants.ReadString, streamParameter)
                )
            );

            return Expression.Lambda(body, streamParameter);
        }

#pragma warning disable IDE0034 // Simplify 'default' expression
        private static readonly Lazy<MethodInfo> s_dtFromBinary = new(() => (MethodInfo)ReflectionHelpers.InfoOf(() => DateTime.FromBinary(default(long))));
#pragma warning restore IDE0034 // Simplify 'default' expression

        private static LambdaExpression VisitDateTime(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_DATETIME;
            Expression body;
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
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (DateTime?)null : (DateTime?)DateTime.FromBinary(stream.ReadInt64());
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.Call(s_dtFromBinary.Value, Expression.Call(s_readInt64.Value, streamParameter)), type)
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
                    // return DateTime.FromBinary(string.ReadInt64());
                    Expression.Call(s_dtFromBinary.Value, Expression.Call(s_readInt64.Value, streamParameter))
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

#pragma warning disable IDE0034 // Simplify 'default' expression
        private static readonly Lazy<ConstructorInfo> s_dtoCtor = new(() => (ConstructorInfo)ReflectionHelpers.InfoOf(() => new DateTimeOffset(default(DateTime), default(TimeSpan))));
        private static readonly Lazy<ConstructorInfo> s_tsMinuteCtor = new(() => (ConstructorInfo)ReflectionHelpers.InfoOf(() => new TimeSpan(default(int), default(int), default(int))));
#pragma warning restore IDE0034 // Simplify 'default' expression

        private static LambdaExpression VisitDateTimeOffset(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_DATETIMEOFFSET;
            Expression body;
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
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE)
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        // ? (DateTimeOffset?)null; 
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        // : (DateTimeOffset?)new DateTimeOffset(DateTime.FromBinary(stream.ReadInt64()), new TimeSpan(0, (int)stream.ReadInt16(), 0));
                        Expression.Convert(
                            Expression.New(s_dtoCtor.Value,
                                Expression.Call(s_dtFromBinary.Value, Expression.Call(s_readInt64.Value, streamParameter)),
                                Expression.New(s_tsMinuteCtor.Value, ReflectionConstants.ZeroInt32, Expression.Convert(Expression.Call(s_readInt16.Value, streamParameter), typeof(int)), ReflectionConstants.ZeroInt32)
                            ),
                            type
                        )
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
                    // return new DateTimeOffset(DateTime.FromBinary(stream.ReadInt64()), new TimeSpan(0, (int)stream.ReadInt16(), 0));
                    Expression.New(s_dtoCtor.Value,
                        Expression.Call(s_dtFromBinary.Value, Expression.Call(s_readInt64.Value, streamParameter)),
                        Expression.New(s_tsMinuteCtor.Value, ReflectionConstants.ZeroInt32, Expression.Convert(Expression.Call(s_readInt16.Value, streamParameter), typeof(int)), ReflectionConstants.ZeroInt32)
                    )
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

#pragma warning disable IDE0034 // Simplify 'default' expression
        private static readonly Lazy<ConstructorInfo> s_tsCtor = new(() => (ConstructorInfo)ReflectionHelpers.InfoOf(() => new TimeSpan(default(long))));
#pragma warning restore IDE0034 // Simplify 'default' expression

        private static LambdaExpression VisitTimeSpan(Type type, bool isNullable)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_TIMESPAN;
            Expression body;
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
                    // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? (TimeSpan?)null : (TimeSpan?)new TimeSpan(stream.ReadInt64());
                    Expression.Condition(
                        Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                        Expression.Convert(ReflectionConstants.NullObject, type),
                        Expression.Convert(Expression.New(s_tsCtor.Value, Expression.Call(s_readInt64.Value, streamParameter)), type)
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
                    // return new TimeSpan(string.ReadInt64());
                    Expression.New(s_tsCtor.Value, Expression.Call(s_readInt64.Value, streamParameter))
                );
            }

            return Expression.Lambda(body, streamParameter);
        }

#pragma warning disable IDE0034 // Simplify 'default' expression
        private static readonly Lazy<ConstructorInfo> s_uriCtor = new(() => (ConstructorInfo)ReflectionHelpers.InfoOf(() => new Uri(default(string), default(UriKind))));
#pragma warning restore IDE0034 // Simplify 'default' expression

        private static LambdaExpression VisitUri(Type type)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");

            var typeCode = Protocol.TYPE_URI;
            var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
            var body = Expression.Block(
                // byte tc;
                new[] { tcParameter },
                // tc = stream.ReadByte();
                Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                Expression.IfThen(
                    Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                    ReflectionConstants.ThrowUnexpectedTypeCode
                ),
                // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) ? null : new Uri(stream.ReadString(), stream.ReadBoolean() ? UriKind.Absolute : UriKind.Relative);
                Expression.Condition(
                    Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                    Expression.Convert(ReflectionConstants.NullObject, type),
                    Expression.New(
                        s_uriCtor.Value,
                        Expression.Call(ReflectionConstants.ReadString, streamParameter),
                        Expression.Condition(
                            Expression.Call(s_readBoolean.Value, streamParameter),
                            Expression.Constant(UriKind.Absolute, typeof(UriKind)),
                            Expression.Constant(UriKind.Relative, typeof(UriKind))
                        )
                    )
                )
            );

            return Expression.Lambda(body, streamParameter);
        }
    }
}
