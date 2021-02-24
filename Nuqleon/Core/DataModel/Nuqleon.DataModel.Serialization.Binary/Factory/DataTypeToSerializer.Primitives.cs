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
    internal partial class DataTypeToSerializer
    {
        private static LambdaExpression VisitUnit(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_UNIT;
            Expression body;
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
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter)
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte)))
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static LambdaExpression VisitByte(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_BYTE;
            Expression body;
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
                    // if (value != null) stream.WriteByte((byte)value);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(streamParameter, s_writeByte.Value, Expression.Convert(valueParameter, typeof(byte)))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteByte(value);
                    Expression.Call(streamParameter, s_writeByte.Value, valueParameter)
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static LambdaExpression VisitString(Type type)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");

            var typeCode = Protocol.TYPE_STRING;
            var body = Expression.Block(
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
                // if (value != null) stream.WriteString(value);
                Expression.IfThen(
                    Expression.NotEqual(valueParameter, s_nullObject.Value),
                    Expression.Call(s_writeString.Value, streamParameter, valueParameter)
                )
            );

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<MethodInfo> s_dtToBinary = new(() => (MethodInfo)ReflectionHelpers.InfoOf((DateTime dt) => dt.ToBinary()));

        private static LambdaExpression VisitDateTime(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_DATETIME;
            Expression body;
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
                    // if (value != null) stream.WriteInt64(((DateTime)value).ToBinary());
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeInt64.Value, streamParameter, Expression.Call(Expression.Convert(valueParameter, typeof(DateTime)), s_dtToBinary.Value))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteInt64(converted.ToBinary());
                    Expression.Call(s_writeInt64.Value, streamParameter, Expression.Call(valueParameter, s_dtToBinary.Value))
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<PropertyInfo> s_dtoDateTime = new(() => (PropertyInfo)ReflectionHelpers.InfoOf((DateTimeOffset dto) => dto.DateTime));
        private static readonly Lazy<PropertyInfo> s_dtoOffset = new(() => (PropertyInfo)ReflectionHelpers.InfoOf((DateTimeOffset dto) => dto.Offset));
        private static readonly Lazy<PropertyInfo> s_tsTotalMinutes = new(() => (PropertyInfo)ReflectionHelpers.InfoOf((TimeSpan ts) => ts.TotalMinutes));

        private static LambdaExpression VisitDateTimeOffset(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_DATETIMEOFFSET;
            Expression body;
            if (isNullable)
            {
                typeCode |= Protocol.TYPE_FLAG_ISNULLABLE;
                var tcParameter = Expression.Parameter(typeof(byte), "tc");
                var nonNullableParameter = Expression.Parameter(typeof(DateTimeOffset), "nonNullable");
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter, nonNullableParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // if (value == null) tc |= Protocol.TYPE_FLAG_NULLVALUE;
                    Expression.IfThen(
                        Expression.Equal(valueParameter, s_nullObject.Value),
                        Expression.OrAssign(tcParameter, s_nullValueFlag.Value)
                    ),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    // if (value != null) {
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Block(
                            // DateTimeOffset nonNullable;
                            new[] { nonNullableParameter },
                            // nonNullable = (DateTimeOffset)value;
                            Expression.Assign(nonNullableParameter, Expression.Convert(valueParameter, typeof(DateTimeOffset))),
                            // stream.WriteInt64(nonNullable.DateTime.ToBinary());
                            Expression.Call(s_writeInt64.Value, streamParameter, Expression.Call(Expression.Property(nonNullableParameter, s_dtoDateTime.Value), s_dtToBinary.Value)),
                            // stream.WriteInt16((short)nonNullable.Offset.TotalMinutes);
                            Expression.Call(s_writeInt16.Value, streamParameter, Expression.ConvertChecked(Expression.Property(Expression.Property(nonNullableParameter, s_dtoOffset.Value), s_tsTotalMinutes.Value), typeof(short)))
                        )
                    )
                // }
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteInt64(value.DateTime.ToBinary());
                    Expression.Call(s_writeInt64.Value, streamParameter, Expression.Call(Expression.Property(valueParameter, s_dtoDateTime.Value), s_dtToBinary.Value)),
                    // stream.WriteInt16((short)value.Offset.TotalMinutes);
                    Expression.Call(s_writeInt16.Value, streamParameter, Expression.ConvertChecked(Expression.Property(Expression.Property(valueParameter, s_dtoOffset.Value), s_tsTotalMinutes.Value), typeof(short)))
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<PropertyInfo> s_tsTicks = new(() => (PropertyInfo)ReflectionHelpers.InfoOf((TimeSpan ts) => ts.Ticks));

        private static LambdaExpression VisitTimeSpan(Type type, bool isNullable)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");

            var typeCode = Protocol.TYPE_TIMESPAN;
            Expression body;
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
                    // if (value != null) stream.WriteInt64(((TimeSpan)value).Ticks);
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        Expression.Call(s_writeInt64.Value, streamParameter, Expression.Property(Expression.Convert(valueParameter, typeof(TimeSpan)), s_tsTicks.Value))
                    )
                );
            }
            else
            {
                body = Expression.Block(
                    // stream.WriteByte(typeCode);
                    Expression.Call(streamParameter, s_writeByte.Value, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteInt64(converted.Ticks);
                    Expression.Call(s_writeInt64.Value, streamParameter, Expression.Property(valueParameter, s_tsTicks.Value))
                );
            }

            return Expression.Lambda(body, streamParameter, valueParameter);
        }

        private static readonly Lazy<PropertyInfo> s_uriOriginalString = new(() => (PropertyInfo)ReflectionHelpers.InfoOf((Uri uri) => uri.OriginalString));
        private static readonly Lazy<PropertyInfo> s_uriIsAbsolute = new(() => (PropertyInfo)ReflectionHelpers.InfoOf((Uri uri) => uri.IsAbsoluteUri));

        private static LambdaExpression VisitUri(Type type)
        {
            var valueParameter = Expression.Parameter(type, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");

            var typeCode = Protocol.TYPE_URI;
            var body = Expression.Block(
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
                // if (value != null)
                Expression.IfThen(
                    Expression.NotEqual(valueParameter, s_nullObject.Value),
                    // {
                    Expression.Block(
                        // stream.WriteString(value.OriginalString);
                        Expression.Call(s_writeString.Value, streamParameter, Expression.Property(valueParameter, s_uriOriginalString.Value)),
                        // stream.WriteBoolean(value.IsAbsoluteUri);
                        Expression.Call(s_writeBoolean.Value, streamParameter, Expression.Property(valueParameter, s_uriIsAbsolute.Value))
                    )
                // }
                )
            );

            return Expression.Lambda(body, streamParameter, valueParameter);
        }
    }
}
