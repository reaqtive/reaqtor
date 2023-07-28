// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

using Nuqleon.DataModel.TypeSystem;

namespace Nuqleon.DataModel.Serialization.Binary
{
    internal partial class DataTypeToDeserializer : DataTypeVisitor<LambdaExpression, Tuple<DataProperty, LambdaExpression>>
    {
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

        public DataTypeToDeserializer()
        {
            _visited = new HashSet<Type>();
        }

        protected override LambdaExpression MakeArray(ArrayDataType type, LambdaExpression elementType)
        {
            if (!type.UnderlyingType.IsArray)
            {
                return MakeList(type, elementType);
            }

            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var resultParameter = Expression.Parameter(type.UnderlyingType, "result");
            var lengthParameter = Expression.Parameter(typeof(int), "length");
            var loopParameter = Expression.Parameter(typeof(int), "i");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");

            var serializerParameter = default(ParameterExpression);
            var innerInvoke = MakeInnerDeserialize(elementType, streamParameter, ref serializerParameter);

            var typeCode = Protocol.TYPE_ARRAY;
            var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
            var body = Expression.Block(
                // byte tc;
                // T[] result;
                new[] { tcParameter, resultParameter },
                // tc = stream.ReadByte();
                Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                Expression.IfThen(
                    Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                    ReflectionConstants.ThrowUnexpectedTypeCode
                ),
                // if (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) return null;
                Expression.Condition(
                    Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                    Expression.Convert(ReflectionConstants.NullObject, type.UnderlyingType),
                    // else {
                    Expression.Block(
                        // int length, i;
                        new[] { lengthParameter, loopParameter },
                        // length = (int)stream.ReadUInt32Compact();
                        Expression.Assign(lengthParameter, Expression.ConvertChecked(Expression.Call(ReflectionConstants.ReadIntCompact, streamParameter), typeof(int))),
                        // result = new T[length];
                        Expression.Assign(resultParameter, Expression.NewArrayBounds(type.ElementType.UnderlyingType, lengthParameter)),
                        // var i = 0; while (true) { if (i < length) { result[i] = /* ... deserialize inner ... */; i++; } else { break; } }
                        ExpressionHelpers.For(
                            Expression.Assign(loopParameter, ReflectionConstants.ZeroInt32),
                            Expression.LessThan(loopParameter, lengthParameter),
                            Expression.PreIncrementAssign(loopParameter),
                            Expression.Assign(Expression.ArrayAccess(resultParameter, loopParameter), innerInvoke)
                        ),
                        // return result;
                        resultParameter
                    )
                // }
                )
            );

            return serializerParameter == null
                ? Expression.Lambda(body, streamParameter)
                : Expression.Lambda(body, serializerParameter, streamParameter);
        }

        private static LambdaExpression MakeList(ArrayDataType type, LambdaExpression elementType)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var resultParameter = Expression.Parameter(type.UnderlyingType, "result");
            var lengthParameter = Expression.Parameter(typeof(int), "length");
            var loopParameter = Expression.Parameter(typeof(int), "i");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");

            var listCtor = type.UnderlyingType.GetConstructor(ReflectionConstants.ListCtorArgs);
            var listAdd = type.UnderlyingType.GetMethod("Add", new[] { type.ElementType.UnderlyingType } /* pool? */);

            var serializerParameter = default(ParameterExpression);
            var innerInvoke = MakeInnerDeserialize(elementType, streamParameter, ref serializerParameter);

            var typeCode = Protocol.TYPE_ARRAY;
            var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
            var body = Expression.Block(
                // byte tc;
                // T[] result;
                new[] { tcParameter, resultParameter },
                // tc = stream.ReadByte();
                Expression.Assign(tcParameter, Expression.Call(ReflectionConstants.ReadByte, streamParameter)),
                // if (tc & typeCode != typeCode) throw new InvalidDataException("Unexpected type code.");
                Expression.IfThen(
                    Expression.NotEqual(Expression.And(tcParameter, typeCodeConstant), typeCodeConstant),
                    ReflectionConstants.ThrowUnexpectedTypeCode
                ),
                // if (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE) return null;
                Expression.Condition(
                    Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                    Expression.Convert(ReflectionConstants.NullObject, type.UnderlyingType),
                    // else {
                    Expression.Block(
                        // int length, i;
                        new[] { lengthParameter, loopParameter },
                        // length = (int)stream.ReadUInt32Compact();
                        Expression.Assign(lengthParameter, Expression.ConvertChecked(Expression.Call(ReflectionConstants.ReadIntCompact, streamParameter), typeof(int))),
                        // result = new List<T>(length);
                        Expression.Assign(resultParameter, Expression.New(listCtor, lengthParameter)),
                        // var i = 0; while (true) { if (i < length) { result.Add(/* ... inner deserialize ... */); i++; } else { break; } }
                        ExpressionHelpers.For(
                            Expression.Assign(loopParameter, ReflectionConstants.ZeroInt32),
                            Expression.LessThan(loopParameter, lengthParameter),
                            Expression.PreIncrementAssign(loopParameter),
                            Expression.Call(resultParameter, listAdd, innerInvoke)
                        ),
                        // return result;
                        resultParameter
                    )
                // }
                )
            );

            return serializerParameter == null
                ? Expression.Lambda(body, streamParameter)
                : Expression.Lambda(body, serializerParameter, streamParameter);
        }

        protected override LambdaExpression MakeFunction(FunctionDataType type, ReadOnlyCollection<LambdaExpression> parameterTypes, LambdaExpression returnType)
        {
            throw new NotSupportedException("Arbitrary functions cannot be serialized.");
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
            Debug.Assert(functionType == null);

            return MakeExpressionCore(type.UnderlyingType);
        }

        protected override LambdaExpression VisitStructural(StructuralDataType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (!_visited.Add(type.UnderlyingType))
            {
                return Expression.Parameter(typeof(DataTypeBinarySerializer), "serializer").Let(serializerParameter =>
                    Expression.Parameter(typeof(Stream), "stream").Let(streamParameter =>
                        Expression.Lambda(
                            Expression.Convert(
                                Expression.Call(serializerParameter, ReflectionConstants.Deserialize, Expression.Constant(type.UnderlyingType), streamParameter),
                                type.UnderlyingType
                            ),
                            serializerParameter,
                            streamParameter
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
            return type.StructuralKind switch
            {
                StructuralDataTypeKinds.Anonymous or StructuralDataTypeKinds.Tuple => MakeAnonymousOrTuple(type, properties),
                StructuralDataTypeKinds.Entity or StructuralDataTypeKinds.Record => MakeEntityOrRecord(type, properties),
                _ => throw new NotSupportedException("Only record, anonymous, entity, and tuples are supported structural types."),
            };
        }

        private static LambdaExpression MakeEntityOrRecord(StructuralDataType type, ReadOnlyCollection<Tuple<DataProperty, LambdaExpression>> properties)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var resultParameter = Expression.Parameter(type.UnderlyingType, "result");
            var countParameter = Expression.Parameter(typeof(int), "count");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");
            var nameParameter = Expression.Parameter(typeof(string), "name");
            var loopParameter = Expression.Parameter(typeof(int), "i");
            var skipParameter = Expression.Parameter(typeof(long), "skip");

            var tmpValueCnt = 0;
            var tmpValueParameters = properties
                .Select(p => p.Item1.Type.UnderlyingType)
                .Distinct()
                .ToDictionary(t => t, t => Expression.Parameter(t, "tmp" + (tmpValueCnt++)));

            var propertyCount = properties.Count;

            var serializerParameter = default(ParameterExpression);
            var switchCases = new SwitchCase[propertyCount];
            for (var i = 0; i < propertyCount; ++i)
            {
                var property = properties[i];
                var tmpValueParameter = tmpValueParameters[property.Item1.Type.UnderlyingType];
                var innerInvoke = MakeInnerDeserialize(property.Item2, streamParameter, ref serializerParameter);
                // case propertyName:
                switchCases[i] = Expression.SwitchCase(
                    // {
                    Expression.Block(
                        typeof(void),
                        // stream.ReadUInt32Compact(); /* ignore */
                        Expression.Call(ReflectionConstants.ReadIntCompact, streamParameter),
                        //
                        // The reason we're doing tmp assignment
                        //     tmp = deserialize(stream); result.member = tmp;
                        // instead of result.member = deserialize(stream)
                        // is to prevent potential stackspilling when result is a value type and when deserialize contains
                        // one of the these expressions (loop try throw goto) which requires a balance stack upon entry
                        // and a value type member assignment requires the value type to be passed in on the stack.
                        //
                        // tmp = deserialize(stream);
                        Expression.Assign(tmpValueParameter, innerInvoke),
                        // result.Property = tmp;
                        Expression.Assign(Expression.MakeMemberAccess(resultParameter, property.Item1.Property), tmpValueParameter)
                    ),
                    // }
                    Expression.Constant(property.Item1.Name, typeof(string))
                );
            }

            // default:
            // {
            var defaultBody = Expression.Block(
                typeof(void),
                // long skip;
                new[] { skipParameter },
                // skip = (long)stream.ReadUInt32Compact();
                Expression.Assign(skipParameter, Expression.Convert(Expression.Call(ReflectionConstants.ReadIntCompact, streamParameter), typeof(long))),
                // stream.Position += skip;
                Expression.AddAssign(Expression.Property(streamParameter, "Position"), skipParameter)
            );
            // }

            var typeCode = Protocol.TYPE_STRUCTURAL;
            var typeCodeConstant = Expression.Constant(typeCode, typeof(byte));
            var body = Expression.Block(
                // byte tc;
                // T[] result;
                new[] { tcParameter, resultParameter },
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
                    // ? null
                    Expression.Convert(ReflectionConstants.NullObject, type.UnderlyingType),
                    // : {
                    Expression.Block(
                        // int count, i;
                        // T result;
                        // TTmp0 tmp0; TTmp1 tmp1; ...
                        (new[] { countParameter, loopParameter, resultParameter }).Concat(tmpValueParameters.Values),
                        // result = new T();
                        Expression.Assign(resultParameter, Expression.New(type.UnderlyingType)),
                        // count = (int)stream.ReadUInt32Compact();
                        Expression.Assign(countParameter, Expression.ConvertChecked(Expression.Call(ReflectionConstants.ReadIntCompact, streamParameter), typeof(int))),
                        // var i = 0; while (true) { if (i < length) { string name; name = stream.ReadString(); switch (name) { ... } ++i; } else { break; } }
                        ExpressionHelpers.For(
                            Expression.Assign(loopParameter, ReflectionConstants.ZeroInt32),
                            Expression.LessThan(loopParameter, countParameter),
                            Expression.PreIncrementAssign(loopParameter),
                            Expression.Block(
                                new[] { nameParameter },
                                Expression.Assign(nameParameter, Expression.Call(ReflectionConstants.ReadString, streamParameter)),
                                Expression.Switch(nameParameter, defaultBody, switchCases)
                            )
                        ),
                        // return result;
                        resultParameter
                    )
                // }
                )
            );

            return serializerParameter == null
                ? Expression.Lambda(body, streamParameter)
                : Expression.Lambda(body, serializerParameter, streamParameter);
        }

        private static LambdaExpression MakeAnonymousOrTuple(StructuralDataType type, ReadOnlyCollection<Tuple<DataProperty, LambdaExpression>> properties)
        {
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            var countParameter = Expression.Parameter(typeof(int), "count");
            var tcParameter = Expression.Parameter(typeof(byte), "tc");
            var nameParameter = Expression.Parameter(typeof(string), "name");
            var loopParameter = Expression.Parameter(typeof(int), "i");
            var skipParameter = Expression.Parameter(typeof(long), "skip");

            var propertyCount = properties.Count;

            var propertyTypes = new Type[propertyCount];
            var blockParameters = new ParameterExpression[propertyCount + 2];
            blockParameters[0] = countParameter;
            blockParameters[1] = loopParameter;

            var serializerParameter = default(ParameterExpression);
            var switchCases = new SwitchCase[propertyCount];
            for (var i = 0; i < propertyCount; ++i)
            {
                var property = properties[i];
                var propertyType = property.Item1.Type.UnderlyingType;
                propertyTypes[i] = propertyType;
                var innerInvoke = MakeInnerDeserialize(property.Item2, streamParameter, ref serializerParameter);

                // TProperty{i} item{i};
                var tupleParameter = Expression.Parameter(propertyType, "item" + i);
                blockParameters[i + 2] = tupleParameter;
                // case propertyName:
                switchCases[i] = Expression.SwitchCase(
                    // {
                    Expression.Block(
                        typeof(void),
                        // stream.ReadUInt32Compact(); /* ignore */
                        Expression.Call(ReflectionConstants.ReadIntCompact, streamParameter),
                        // itemi = (TProperty{i})deserialize(stream);
                        Expression.Assign(tupleParameter, Expression.Convert(innerInvoke, propertyType))
                    ),
                    // }
                    Expression.Constant(property.Item1.Name, typeof(string))
                );
            }

            // default:
            // {
            var defaultBody = Expression.Block(
                typeof(void),
                // long skip;
                new[] { skipParameter },
                // skip = (long)stream.ReadUInt32Compact();
                Expression.Assign(skipParameter, Expression.Convert(Expression.Call(ReflectionConstants.ReadIntCompact, streamParameter), typeof(long))),
                // stream.Position += skip;
                Expression.AddAssign(Expression.Property(streamParameter, "Position"), skipParameter)
            );
            // }

            var typeCtor = type.UnderlyingType.GetConstructor(propertyTypes);

            var typeCode = Protocol.TYPE_STRUCTURAL;
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
                // return (tc & Protocol.TYPE_FLAG_NULLVALUE == Protocol.TYPE_FLAG_NULLVALUE)
                Expression.Condition(
                    Expression.Equal(Expression.And(tcParameter, ReflectionConstants.NullValueFlag), ReflectionConstants.NullValueFlag),
                    // ? null
                    Expression.Convert(ReflectionConstants.NullObject, type.UnderlyingType),
                    // : {
                    Expression.Block(
                        // int count, i;
                        // TProperty0 item0; TProperty1 item1; ...
                        blockParameters,
                        // count = (int)stream.ReadInt32();
                        Expression.Assign(countParameter, Expression.ConvertChecked(Expression.Call(ReflectionConstants.ReadIntCompact, streamParameter), typeof(int))),
                        // var i = 0; while (true) { if (i < length) { name = stream.ReadString(); switch (name) { ... } ++i; } else { break; } }
                        ExpressionHelpers.For(
                            Expression.Assign(loopParameter, ReflectionConstants.ZeroInt32),
                            Expression.LessThan(loopParameter, countParameter),
                            Expression.PreIncrementAssign(loopParameter),
                            Expression.Block(
                                new[] { nameParameter },
                                Expression.Assign(nameParameter, Expression.Call(ReflectionConstants.ReadString, streamParameter)),
                                Expression.Switch(nameParameter, defaultBody, switchCases)
                            )
                        ),
                        // return .ctor(...);
                        Expression.New(typeCtor, blockParameters.Skip(2))
                    )
                // }
                )
            );

            return serializerParameter == null
                ? Expression.Lambda(body, streamParameter)
                : Expression.Lambda(body, serializerParameter, streamParameter);
        }

        protected override LambdaExpression VisitCustom(DataType type)
        {
            throw new NotSupportedException("Custom data types are not supported.");
        }

        protected override LambdaExpression VisitExpression(ExpressionDataType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return MakeExpressionCore(type.UnderlyingType);
        }

        protected override LambdaExpression VisitOpenGenericParameter(OpenGenericParameterDataType type)
        {
            throw new NotSupportedException("Open generic parameter types cannot have instances, let alone be serialized.");
        }

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
            if (type.IsNullable)
            {
                var primitiveParameter = Expression.Parameter(underlyingType, "primitive");
                var convertedResultParameter = Expression.Parameter(enumType, "convertedResult");
                return Expression.Lambda(
                    // {
                    Expression.Block(
                        new[] { primitiveParameter, convertedResultParameter },
                        Expression.Assign(primitiveParameter, inner.Body),
                        Expression.Condition(
                            Expression.Equal(primitiveParameter, ReflectionConstants.NullObject),
                            Expression.Convert(primitiveParameter, type.UnderlyingType),
                            Expression.Convert(Expression.Convert(primitiveParameter, enumType), type.UnderlyingType)
                        )
                    ),
                    inner.Parameters
                );
            }
            else
            {
                return Expression.Lambda(
                    Expression.Convert(inner.Body, type.UnderlyingType),
                    inner.Parameters
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

        private static LambdaExpression MakeExpressionCore(Type type)
        {
            var serializerParameter = Expression.Parameter(typeof(DataTypeBinarySerializer), "serializer");
            var streamParameter = Expression.Parameter(typeof(Stream), "stream");
            Expression body = Expression.Call(
                Expression.MakeMemberAccess(serializerParameter, ReflectionConstants.ExpressionSerializer),
                ReflectionConstants.ExpressionDeserialize,
                streamParameter
            );

            if (type != typeof(Expression))
            {
                body = Expression.Convert(body, type);
            }

            return Expression.Lambda(body, serializerParameter, streamParameter);
        }

        private static Expression MakeInnerDeserialize(LambdaExpression innerDeserialize, ParameterExpression streamParameter, ref ParameterExpression serializerParameter)
        {
            if (innerDeserialize.Parameters.Count == 1)
            {
                return Expression.Invoke(innerDeserialize, streamParameter);
            }
            else
            {
                Debug.Assert(innerDeserialize.Parameters.Count == 2);
                serializerParameter ??= Expression.Parameter(typeof(DataTypeBinarySerializer), "serializer");

                return Expression.Invoke(innerDeserialize, serializerParameter, streamParameter);
            }
        }
    }
}
