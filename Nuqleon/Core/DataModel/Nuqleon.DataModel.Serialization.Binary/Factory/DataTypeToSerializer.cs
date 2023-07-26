// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel.TypeSystem;

namespace Nuqleon.DataModel.Serialization.Binary
{
    internal partial class DataTypeToSerializer : DataTypeVisitor<LambdaExpression, Tuple<DataProperty, LambdaExpression>>
    {
#pragma warning disable IDE0034 // Simplify 'default' expression
        private static readonly Lazy<MethodInfo> s_write = new(() => (MethodInfo)ReflectionHelpers.InfoOf((Stream s) => s.Write(default(byte[]), default(int), default(int))));
        private static readonly Lazy<MethodInfo> s_writeIntCompact = new(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteUInt32Compact(default(Stream), default(uint))));
        private static readonly Lazy<MethodInfo> s_writeByte = new(() => (MethodInfo)ReflectionHelpers.InfoOf((Stream s) => s.WriteByte(default(byte))));
        private static readonly Lazy<MethodInfo> s_writeString = new(() => (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.WriteString(default(Stream), default(string))));
        private static readonly Lazy<MethodInfo> s_getMemoryStream = new(() => (MethodInfo)ReflectionHelpers.InfoOf(() => CustomMemoryStreamPool.Allocate()));

        private static readonly Lazy<MethodInfo> s_getBuffer = new(() => (MethodInfo)ReflectionHelpers.InfoOf((PooledMemoryStream s) => s.GetBuffer()));
        private static readonly Lazy<MethodInfo> s_free = new(() => (MethodInfo)ReflectionHelpers.InfoOf((PooledMemoryStream s) => s.Free()));
        private static readonly Lazy<MethodInfo> s_serialize = new(() => (MethodInfo)ReflectionHelpers.InfoOf((DataTypeBinarySerializer f) => f.SerializeRecursive(default(Type), default(Stream), default(object))));
        private static readonly Lazy<MethodInfo> s_expressionSerialize = new(() => (MethodInfo)ReflectionHelpers.InfoOf((IExpressionSerializer s) => s.Serialize(default(Stream), default(Expression))));

        private static readonly Lazy<PropertyInfo> s_count = new(() => (PropertyInfo)ReflectionHelpers.InfoOf((IList l) => l.Count));
        private static readonly Lazy<PropertyInfo> s_expressionSerializer = new(() => (PropertyInfo)ReflectionHelpers.InfoOf((DataTypeBinarySerializer f) => f.ExpressionSerializer));

        private static readonly Lazy<Expression> s_nullObject = new(new Func<Expression>(() => Expression.Constant(value: null, typeof(object))));
        private static readonly Lazy<Expression> s_zeroInt32 = new(new Func<Expression>(() => Expression.Constant(0, typeof(int))));
        private static readonly Lazy<Expression> s_nullValueFlag = new(new Func<Expression>(() => Expression.Constant(Protocol.TYPE_FLAG_NULLVALUE, typeof(byte))));
#pragma warning restore IDE0034 // Simplify 'default' expression

        private static readonly IReadOnlyDictionary<Type, byte> s_discriminators =
            new ReadOnlyDictionary<Type, byte>(
                new Dictionary<Type, byte>
                    {
                        { typeof(Unit), Protocol.TYPE_UNIT },
                        { typeof(sbyte), Protocol.TYPE_SBYTE },
                        { typeof(byte), Protocol.TYPE_BYTE },
                        { typeof(short), Protocol.TYPE_INT16 },
                        { typeof(ushort), Protocol.TYPE_UINT16 },
                        { typeof(int), Protocol.TYPE_INT32 },
                        { typeof(uint), Protocol.TYPE_UINT32 },
                        { typeof(long), Protocol.TYPE_INT64 },
                        { typeof(ulong), Protocol.TYPE_UINT64 },
                        { typeof(float), Protocol.TYPE_SINGLE },
                        { typeof(double), Protocol.TYPE_DOUBLE },
                        { typeof(decimal), Protocol.TYPE_DECIMAL },
                        { typeof(bool), Protocol.TYPE_BOOLEAN },
                        { typeof(char), Protocol.TYPE_CHAR },
                        { typeof(string), Protocol.TYPE_STRING },
                        { typeof(DateTime), Protocol.TYPE_DATETIME },
                        { typeof(DateTimeOffset), Protocol.TYPE_DATETIMEOFFSET },
                        { typeof(TimeSpan), Protocol.TYPE_TIMESPAN },
                        { typeof(Guid), Protocol.TYPE_GUID },
                        { typeof(Uri), Protocol.TYPE_URI },
                    });

        private readonly HashSet<Type> _visited;

        public DataTypeToSerializer()
        {
            _visited = new HashSet<Type>();
        }

        protected override LambdaExpression MakeArray(ArrayDataType type, LambdaExpression elementType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var valueParameter = Expression.Parameter(type.UnderlyingType, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var lengthParameter = Expression.Parameter(typeof(int), "length");
            var loopParameter = Expression.Parameter(typeof(int), "i");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");

            var accessExpression = type.UnderlyingType.IsArray
                ? (Expression)Expression.ArrayIndex(valueParameter, loopParameter)
                : Expression.Call(valueParameter, type.UnderlyingType.GetMethod("get_Item", new[] { typeof(int) }), loopParameter);

            var serializerParameter = default(ParameterExpression);
            var innerInvoke = MakeInnerSerialize(elementType, streamParameter, accessExpression, ref serializerParameter);

            var typeCode = Protocol.TYPE_ARRAY;
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
                        // int length, i;
                        new[] { lengthParameter, loopParameter },
                        // length = value.Length;
                        Expression.Assign(lengthParameter, Expression.Property(valueParameter, s_count.Value)),
                        // stream.WriteUInt32Compact((uint)length);
                        Expression.Call(s_writeIntCompact.Value, streamParameter, Expression.Convert(lengthParameter, typeof(uint))),
                        // var i = 0; while (true) { if (i < length) { inner(value[i]); i++; } else { break; } }
                        ExpressionHelpers.For(
                            Expression.Assign(loopParameter, Expression.Constant(0, typeof(int))),
                            Expression.LessThan(loopParameter, lengthParameter),
                            Expression.PreIncrementAssign(loopParameter),
                            innerInvoke
                        )
                    )
                // }
                )
            );

            return serializerParameter == null
                ? Expression.Lambda(body, streamParameter, valueParameter)
                : Expression.Lambda(body, serializerParameter, streamParameter, valueParameter);
        }

        protected override LambdaExpression MakeFunction(FunctionDataType type, ReadOnlyCollection<LambdaExpression> parameterTypes, LambdaExpression returnType)
        {
            throw new NotSupportedException("Functions cannot be serialized.");
        }

        protected override Tuple<DataProperty, LambdaExpression> MakeProperty(DataProperty property, LambdaExpression propertyType)
        {
            return Tuple.Create(property, propertyType);
        }

        protected override LambdaExpression VisitQuotation(QuotationDataType type)
        {
            // No need to recurse into function type, this will be serialized as an expression anyway.
            return MakeQuotation(type, functionType: null);
        }

        protected override LambdaExpression MakeQuotation(QuotationDataType type, LambdaExpression functionType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Debug.Assert(functionType == null);
            return MakeExpressionCore(type.UnderlyingType);
        }

        protected override LambdaExpression VisitStructural(StructuralDataType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (!_visited.Add(type.UnderlyingType))
            {
                return Expression.Parameter(typeof(DataTypeBinarySerializer), "serializer").Let(serializer =>
                    Expression.Parameter(typeof(Stream), "stream").Let(stream =>
                        Expression.Parameter(type.UnderlyingType, "value").Let(value =>
                            Expression.Lambda(
                                Expression.Call(serializer, s_serialize.Value, Expression.Constant(type.UnderlyingType), stream, value),
                                serializer,
                                stream,
                                value
                            )
                        )
                    )
                );
            }

            try
            {
                return base.VisitStructural(type);
            }
            finally
            {
                _visited.Remove(type.UnderlyingType);
            }
        }

        protected override LambdaExpression MakeStructural(StructuralDataType type, ReadOnlyCollection<Tuple<DataProperty, LambdaExpression>> properties)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            switch (type.StructuralKind)
            {
                case StructuralDataTypeKinds.Anonymous:
                case StructuralDataTypeKinds.Entity:
                case StructuralDataTypeKinds.Record:
                case StructuralDataTypeKinds.Tuple:
                    break;
                default:
                    throw new NotSupportedException("Only record, anonymous, entity, and tuples are supported structural types.");
            }

            var valueParameter = Expression.Parameter(type.UnderlyingType, "value");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var countParameter = Expression.Parameter(typeof(int), "count");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");
            var memoryStreamParameter = Expression.Parameter(typeof(PooledMemoryStream), "memoryStream");
            var bufferLengthParameter = Expression.Parameter(typeof(int), "bufferLength");
            var bufferParameter = Expression.Parameter(typeof(byte[]), "buffer");

            var typeCode = Protocol.TYPE_STRUCTURAL;
            var propertyCount = properties.Count;
            var serializeBody = new Expression[propertyCount * 8 + 2];
            // count = propertyCount;
            serializeBody[0] = Expression.Assign(countParameter, Expression.Constant(propertyCount, typeof(int)));
            // stream.WriteUInt32Compact((uint)count);
            serializeBody[1] = Expression.Call(s_writeIntCompact.Value, streamParameter, Expression.Convert(countParameter, typeof(uint)));

            var serializerParameter = default(ParameterExpression);
            for (var i = 0; i < propertyCount; ++i)
            {
                var property = properties[i];
                var innerInvoke = MakeInnerSerialize(property.Item2, memoryStreamParameter, Expression.MakeMemberAccess(valueParameter, property.Item1.Property), ref serializerParameter);

                // stream.WriteString(propertyName);
                serializeBody[i * 8 + 0 + 2] = Expression.Call(s_writeString.Value, streamParameter, Expression.Constant(properties[i].Item1.Name, typeof(string)));
                // memoryStream = PooledMemoryStream.GetInstance();
                serializeBody[i * 8 + 1 + 2] = Expression.Assign(memoryStreamParameter, Expression.Call(s_getMemoryStream.Value));
                // /* ... do inner serialization ... */
                serializeBody[i * 8 + 2 + 2] = innerInvoke;
                // bufferLength = (int)memoryStream.Length;
                serializeBody[i * 8 + 3 + 2] = Expression.Assign(bufferLengthParameter, Expression.ConvertChecked(Expression.Property(memoryStreamParameter, "Length"), typeof(int)));
                // stream.WriteUInt32Compact((uint)bufferLength);
                serializeBody[i * 8 + 4 + 2] = Expression.Call(s_writeIntCompact.Value, streamParameter, Expression.Convert(bufferLengthParameter, typeof(uint)));
                // buffer = memoryStream.GetBuffer();
                serializeBody[i * 8 + 5 + 2] = Expression.Assign(bufferParameter, Expression.Call(memoryStreamParameter, s_getBuffer.Value));
                // stream.Write(buffer, 0, bufferLength);
                serializeBody[i * 8 + 6 + 2] = Expression.Call(streamParameter, s_write.Value, bufferParameter, s_zeroInt32.Value, bufferLengthParameter);
                // memoryStream.Free();
                serializeBody[i * 8 + 7 + 2] = Expression.Call(memoryStreamParameter, s_free.Value);
            }

            BlockExpression body;

            //type has no null value
            if (valueParameter.Type.IsValueType && Nullable.GetUnderlyingType(valueParameter.Type) == null)
            {
                body = Expression.Block(
                    // byte tc;
                    new[] { tcParameter },
                    // tc = typeCode;
                    Expression.Assign(tcParameter, Expression.Constant(typeCode, typeof(byte))),
                    // stream.WriteByte(tc);
                    Expression.Call(streamParameter, s_writeByte.Value, tcParameter),
                    Expression.Block(
                        // int count, bufferLength;
                        // PooledMemoryStream memoryStream;
                        // byte[] buffer;
                        new[] { countParameter, bufferLengthParameter, memoryStreamParameter, bufferParameter },
                        serializeBody
                    )
                );
            }
            else
            {
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
                    // if (value != null)
                    Expression.IfThen(
                        Expression.NotEqual(valueParameter, s_nullObject.Value),
                        // {
                        Expression.Block(
                            // int count, bufferLength;
                            // PooledMemoryStream memoryStream;
                            // byte[] buffer;
                            new[] { countParameter, bufferLengthParameter, memoryStreamParameter, bufferParameter },
                            serializeBody
                        )
                    // }
                    )
                );
            }

            return serializerParameter == null
                ? Expression.Lambda(body, streamParameter, valueParameter)
                : Expression.Lambda(body, serializerParameter, streamParameter, valueParameter);
        }

        protected override LambdaExpression VisitCustom(DataType type) => throw new NotSupportedException("Custom data types are not supported.");

        protected override LambdaExpression VisitExpression(ExpressionDataType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return MakeExpressionCore(type.UnderlyingType);
        }

        protected override LambdaExpression VisitOpenGenericParameter(OpenGenericParameterDataType type) => throw new NotSupportedException("Open generic parameter types cannot have instances, let alone be serialized.");

        protected override LambdaExpression VisitPrimitive(PrimitiveDataType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.PrimitiveKind switch
            {
                PrimitiveDataTypeKinds.Atom => VisitPrimitiveAtom(type),
                PrimitiveDataTypeKinds.EntityEnum or PrimitiveDataTypeKinds.Enum => VisitPrimitiveEnum(type),
                _ => throw new NotSupportedException("Only atoms and enums are supported."),
            };
        }

        private static LambdaExpression MakeExpressionCore(Type type)
        {
            var valueParameter = Expression.Parameter(type, "expression");
            var valueExpression = (type != typeof(Expression))
                ? (Expression)valueParameter
                : Expression.Convert(valueParameter, typeof(Expression));

            return Expression.Parameter(typeof(DataTypeBinarySerializer), "serializer").Let(serializerParameter =>
                Expression.Parameter(typeof(Stream), "stream").Let(streamParameter =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.MakeMemberAccess(serializerParameter, s_expressionSerializer.Value),
                            s_expressionSerialize.Value,
                            streamParameter,
                            valueExpression
                        ),
                        serializerParameter,
                        streamParameter,
                        valueParameter
                    )
                )
            );
        }

        private static Expression MakeInnerSerialize(LambdaExpression innerSerialize, ParameterExpression streamParameter, Expression valueExpression, ref ParameterExpression serializerParameter)
        {
            if (innerSerialize.Parameters.Count == 2)
            {
                return Expression.Invoke(innerSerialize, streamParameter, valueExpression);
            }
            else
            {
                Debug.Assert(innerSerialize.Parameters.Count == 3);
                serializerParameter ??= Expression.Parameter(typeof(DataTypeBinarySerializer), "serializer");

                return Expression.Invoke(innerSerialize, serializerParameter, streamParameter, valueExpression);
            }
        }

        private static LambdaExpression VisitPrimitiveAtom(PrimitiveDataType type)
        {
            var underlyingType = type.UnderlyingType;
            if (type.IsNullable && underlyingType.IsValueType)
            {
                underlyingType = Nullable.GetUnderlyingType(underlyingType);
            }
            if (!s_discriminators.TryGetValue(underlyingType, out var discriminator))
            {
                throw new NotSupportedException("Primitive type not supported.");
            }

            return VisitPrimitiveCore(type.UnderlyingType, type.IsNullable, discriminator);
        }

        private static LambdaExpression VisitPrimitiveEnum(PrimitiveDataType type)
        {
            var enumType = type.UnderlyingType;
            if (type.IsNullable && enumType.IsValueType)
            {
                enumType = Nullable.GetUnderlyingType(enumType);
            }
            var underlyingType = Enum.GetUnderlyingType(enumType);
            if (!s_discriminators.TryGetValue(underlyingType, out var discriminator))
            {
                throw new NotSupportedException("Primitive type not supported.");
            }
            if (type.IsNullable)
            {
                underlyingType = typeof(Nullable<>).MakeGenericType(underlyingType);
            }

            var inner = VisitPrimitiveCore(underlyingType, type.IsNullable, discriminator);
            var enumValueParameter = Expression.Parameter(type.UnderlyingType, "enumValue");
            if (type.IsNullable)
            {
                return Expression.Lambda(
                    // {
                    Expression.Block(
                        new[] { inner.Parameters[1] },
                        Expression.Condition(
                            Expression.Equal(enumValueParameter, s_nullObject.Value),
                            Expression.Assign(inner.Parameters[1], Expression.Convert(s_nullObject.Value, underlyingType)),
                            Expression.Assign(inner.Parameters[1], Expression.Convert(enumValueParameter, underlyingType))
                        ),
                        inner.Body
                    ),
                    inner.Parameters[0],
                    enumValueParameter
                );
            }
            else
            {
                return Expression.Lambda(
                    Expression.Block(
                        new[] { inner.Parameters[1] },
                        Expression.Assign(inner.Parameters[1], Expression.Convert(enumValueParameter, underlyingType)),
                        inner.Body
                    ),
                    inner.Parameters[0],
                    enumValueParameter
                );
            }
        }

        private static LambdaExpression VisitPrimitiveCore(Type type, bool isNullable, byte discriminator)
        {
            return discriminator switch
            {
                Protocol.TYPE_UNIT => VisitUnit(type, isNullable),
                Protocol.TYPE_SBYTE => VisitSByte(type, isNullable),
                Protocol.TYPE_BYTE => VisitByte(type, isNullable),
                Protocol.TYPE_INT16 => VisitInt16(type, isNullable),
                Protocol.TYPE_UINT16 => VisitUInt16(type, isNullable),
                Protocol.TYPE_INT32 => VisitInt32(type, isNullable),
                Protocol.TYPE_UINT32 => VisitUInt32(type, isNullable),
                Protocol.TYPE_INT64 => VisitInt64(type, isNullable),
                Protocol.TYPE_UINT64 => VisitUInt64(type, isNullable),
                Protocol.TYPE_SINGLE => VisitSingle(type, isNullable),
                Protocol.TYPE_DOUBLE => VisitDouble(type, isNullable),
                Protocol.TYPE_DECIMAL => VisitDecimal(type, isNullable),
                Protocol.TYPE_BOOLEAN => VisitBoolean(type, isNullable),
                Protocol.TYPE_CHAR => VisitChar(type, isNullable),
                Protocol.TYPE_STRING => VisitString(type),
                Protocol.TYPE_DATETIME => VisitDateTime(type, isNullable),
                Protocol.TYPE_DATETIMEOFFSET => VisitDateTimeOffset(type, isNullable),
                Protocol.TYPE_TIMESPAN => VisitTimeSpan(type, isNullable),
                Protocol.TYPE_GUID => VisitGuid(type, isNullable),
                Protocol.TYPE_URI => VisitUri(type),
                _ => throw new NotSupportedException("Primitive type not supported."),
            };
        }
    }
}
