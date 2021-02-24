// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Memory;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nuqleon.DataModel.TypeSystem
{
    internal sealed class TypeToDataTypeConverter
    {
        // NB: We have two static singleton instances to avoid creation of many (stateless) TypeToDataTypeConverter instances.

        private static readonly TypeToDataTypeConverter s_allowCycles = new(allowCycles: true);
        private static readonly TypeToDataTypeConverter s_noCycles = new(allowCycles: false);

        // NB: The implementation of the Convert methods relies on stateful implementations which are quite heavy to allocate, so we pool those.

        private readonly ObjectPool<Impl> _implPool;
        private readonly ObjectPool<SafeImpl> _safeImplPool;
        private readonly ObjectPool<KnownTypeImpl> _knownTypeImplPool;

        // NB: Conversion methods are pure functions, so their result can be cached. We use weak caches in order to avoid rooting collectible types.
        //     Note that the cache has an upper bound that is proportional in the number of Type instances in the AppDomain. We could reduce this using
        //     some LRU strategy (cf. memoization caches) but consider that this is the same upper bound as reflection.

        private readonly ConditionalWeakTable<Type, DataType> _convertCache = new();
        private readonly ConditionalWeakTable<Type, DataType> _tryConvertCache = new();
        private readonly ConditionalWeakTable<Type, DataType> _convertKnownTypeCache = new();

        private TypeToDataTypeConverter(bool allowCycles)
        {
            _implPool = new ObjectPool<Impl>(() => new Impl(allowCycles));
            _safeImplPool = new ObjectPool<SafeImpl>(() => new SafeImpl(allowCycles));
            _knownTypeImplPool = new ObjectPool<KnownTypeImpl>(() => new KnownTypeImpl(allowCycles));
        }

        public static TypeToDataTypeConverter Create(bool allowCycles) => allowCycles ? s_allowCycles : s_noCycles;

        public DataType Convert(Type type) => _convertCache.GetValue(type, t => ConvertCore(t));

        private DataType ConvertCore(Type type)
        {
            using var impl = _implPool.New();

            var res = impl.Object.Visit(type);

            return res;
        }

        public bool TryConvert(Type type, out DataType result)
        {
            result = _tryConvertCache.GetValue(type, t => TryConvertCore(t));
            return result != null;
        }

        private DataType TryConvertCore(Type type)
        {
            using var impl = _safeImplPool.New();

            var result = impl.Object.Visit(type);

            return !impl.Object.HasFailed ? result : null;
        }

        public DataType ConvertKnownType(Type type)
        {
            return _convertKnownTypeCache.GetValue(type, t => ConvertKnownTypeCore(t));
        }

        private DataType ConvertKnownTypeCore(Type type)
        {
            using var impl = _knownTypeImplPool.New();

            var res = impl.Object.Visit(type);

            return res;
        }

        private class Impl : DataModelTypeVisitorBase<DataType>, IClearable
        {
            // WARNING: This type implements IClearable. If state is added here, make sure to reset it in the Clear method.

            private readonly bool _allowCycles;
            private readonly IDictionary<Type, DataType> _convertedTypes;

            private const int TupleItemNamesCount = 7;

            private static readonly string[] s_tupleItemNames = new string[]
            {
                "Item1",
                "Item2",
                "Item3",
                "Item4",
                "Item5",
                "Item6",
                "Item7",
            };

            public Impl(bool allowCycles)
            {
                _convertedTypes = new Dictionary<Type, DataType>();
                _allowCycles = allowCycles;
            }

            public override DataType Visit(Type type)
            {
                if (_convertedTypes.TryGetValue(type, out var res))
                {
                    if (!_allowCycles && res == RecursionCanary.Instance)
                    {
                        res = FailCycle(type);
                    }

                    return res;
                }
                else
                {
                    if (!_allowCycles)
                    {
                        _convertedTypes[type] = RecursionCanary.Instance;
                    }
                }

                res = base.Visit(type);

                _convertedTypes[type] = res;

                return res;
            }

            protected virtual DataType FailCycle(Type type) => throw new InvalidOperationException(GetFailCycleError(type));

            protected static string GetFailCycleError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' has a cycle. Recursive type definitions are not supported by the data model.", type);

            private sealed class RecursionCanary : DataType
            {
                // WARNING: The singleton instance of a RecursionCanary should never be returned to the outside world.

                public static readonly RecursionCanary Instance = new();

                private RecursionCanary()
                    : base(typeof(object))
                {
                }

                public override DataTypeKinds Kind => DataTypeKinds.Custom;

                public override object CreateInstance(params object[] arguments) => throw new NotImplementedException();
            }

            protected override DataType MakeVoidType(Type type) => new PrimitiveDataType(typeof(Unit), PrimitiveDataTypeKinds.Atom);

            protected override DataType MakePrimitiveType(Type type) => new PrimitiveDataType(type, PrimitiveDataTypeKinds.Atom);

            protected override DataType MakeEnumType(Type type) => new PrimitiveDataType(type, PrimitiveDataTypeKinds.Enum);

            protected override DataType MakeEntityEnumType(Type type, IEnumerable<DataEnumValue> values) => new PrimitiveDataType(type, PrimitiveDataTypeKinds.EntityEnum);

            protected override DataType MakeExpressionType(Type type) => new ExpressionDataType(type);

            protected override DataType MakeQuotationType(Type type, DataType functionType) => new QuotationDataType(type, (FunctionDataType)functionType);

            protected override DataType MakeDynamicType(Type type) => throw new NotSupportedException(GetFailDynamicTypeError(type));

            protected static string GetFailDynamicTypeError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is a dynamic type, which is not supported by the data model (yet).", type.ToCSharpStringPretty());

            protected override DataType DefineAnonymousType(Type type) => DefineStructuralType(type, StructuralDataTypeKinds.Anonymous);

            protected override DataType MakeAnonymousType(DataType type, IEnumerable<DataProperty<DataType>> properties) => FixStructuralType(type, properties);

            protected override DataType DefineRecordType(Type type) => DefineStructuralType(type, StructuralDataTypeKinds.Record);

            protected override DataType MakeRecordType(DataType type, IEnumerable<DataProperty<DataType>> entries) => FixStructuralType(type, entries);

            protected override DataType DefineEntityType(Type type) => DefineStructuralType(type, StructuralDataTypeKinds.Entity);

            protected override DataType MakeEntityType(DataType type, IEnumerable<DataProperty<DataType>> properties) => FixStructuralType(type, properties);

            private DataType DefineStructuralType(Type type, StructuralDataTypeKinds kind)
            {
                DataType res;

                if (_allowCycles)
                {
                    if (!_convertedTypes.TryGetValue(type, out res))
                    {
                        res = new StructuralDataTypeReference(type, kind);
                        _convertedTypes[type] = res;
                    }
                }
                else
                {
                    return new StructuralDataTypeReference(type, kind);
                }

                return res;
            }

            private static DataType FixStructuralType(DataType type, IEnumerable<DataProperty<DataType>> properties)
            {
                var structuralType = (StructuralDataTypeReference)type;
                structuralType.Properties.AddRange(properties.Select(p => new DataProperty(p.Property, p.Name, p.Type)));
                structuralType.Freeze();
                return structuralType;
            }

            protected override DataType MakeFunctionType(Type type, DataType[] parameterTypes, DataType returnType) => new FunctionDataType(type, parameterTypes.ToReadOnly(), returnType);

            protected override DataType MakeArrayType(Type type, DataType elementType) => new ArrayDataType(type, elementType);

            protected override DataType MakeTupleType(Type type, IEnumerable<DataType> components)
            {
                var hasOverflow = (type.GetGenericTypeDefinition() == s_tupleMax);

                var comps = components.ToArray();
                var n = comps.Length;
                var properties = new DataProperty[n];

                for (var i = 0; i < n; i++)
                {
                    var name = GetTupleItemName(i);

                    if (i == n - 1 && hasOverflow)
                    {
                        name = "Rest";
                    }

                    var property = type.GetProperty(name);
                    var propertyType = comps[i];
                    properties[i] = new DataProperty(property, name, propertyType);
                }

                return new StructuralDataType(type, new ReadOnlyCollection<DataProperty>(properties), StructuralDataTypeKinds.Tuple);
            }

            private static string GetTupleItemName(int i)
            {
                if (i < TupleItemNamesCount)
                {
                    return s_tupleItemNames[i];
                }

                return "Item" + (i + 1);
            }

            protected override DataType MakeOpenGenericParameterType(Type type) => new OpenGenericParameterDataType(type);

            public virtual void Clear() => _convertedTypes.Clear();
        }

        private sealed class SafeImpl : Impl
        {
            private readonly IDictionary<Type, List<string>> _entityFailures;
            private readonly IDictionary<Type, List<string>> _entityEnumFailures;

            public SafeImpl(bool allowCycles)
                : base(allowCycles)
            {
                HasFailed = false;
                _entityFailures = new Dictionary<Type, List<string>>();
                _entityEnumFailures = new Dictionary<Type, List<string>>();
            }

            public bool HasFailed { get; private set; }

            protected override DataType FailCycle(Type type) => Fail(type, GetFailCycleError);

            protected override DataType MakeDynamicType(Type type) => Fail(type, GetFailDynamicTypeError);

            protected override DataType FailArrayType(Type type, DataType elementType, int rank) => Fail(type, GetFailArrayTypeError);

            protected override DataType FailByRefType(Type type, DataType elementType) => Fail(type, GetFailByRefTypeError);

            protected override DataType FailEntityType(Type type)
            {
                return Fail(type, t =>
                {
                    using var psb = PooledStringBuilder.New();

                    var error = psb.StringBuilder;

                    var generalError = GetFailEntityTypeError(t);
                    error.AppendLine(generalError);

                    if (_entityFailures.TryGetValue(t, out var detailedErrors))
                    {
                        error.AppendLine();

                        foreach (var detailedError in detailedErrors)
                        {
                            error.AppendLine("- " + detailedError);
                        }
                    }

                    return error.ToString();
                });
            }

            protected override void FailEntityTypePropertyDuplicateMapping(MemberInfo property, string mappingUri) => FailEntity(property.DeclaringType, GetFailEntityTypePropertyDuplicateMappingError(property, mappingUri));

            protected override void FailEntityTypePropertyMissingMapping(MemberInfo property) => FailEntity(property.DeclaringType, GetFailEntityTypePropertyMissingMappingError(property));

            protected override void FailEntityTypePropertyNotReadable(PropertyInfo property) => FailEntity(property.DeclaringType, GetFailEntityTypePropertyNotReadableError(property));

            protected override void FailEntityTypeConstructorParameterAndPropertyTypeMismatch(ConstructorInfo constructor, ParameterInfo parameter, string mappingUri, MemberInfo property) => FailEntity(constructor.DeclaringType, GetFailEntityTypeConstructorParameterAndPropertyTypeMismatchError(constructor, parameter, mappingUri, property));

            protected override void FailEntityTypeConstructorParameterInvalidMapping(ConstructorInfo constructor, ParameterInfo parameter, string mappingUri) => FailEntity(constructor.DeclaringType, GetFailEntityTypeConstructorParameterInvalidMappingError(constructor, parameter, mappingUri));

            protected override void FailEntityTypeConstructorParameterMissingMapping(ConstructorInfo constructor, ParameterInfo parameter) => FailEntity(constructor.DeclaringType, GetFailEntityTypeConstructorParameterMissingMappingError(constructor, parameter));

            private void FailEntity(Type type, string error)
            {
                if (!_entityFailures.TryGetValue(type, out var entityErrors))
                {
                    entityErrors = new List<string>();
                    _entityFailures[type] = entityErrors;
                }

                entityErrors.Add(error);
            }

            protected override DataType FailEntityEnumType(Type type)
            {
                return Fail(type, t =>
                {
                    using var psb = PooledStringBuilder.New();

                    var error = psb.StringBuilder;

                    var generalError = GetFailEntityEnumTypeError(t);
                    error.AppendLine(generalError);

                    if (_entityEnumFailures.TryGetValue(t, out var detailedErrors))
                    {
                        error.AppendLine();

                        foreach (var detailedError in detailedErrors)
                        {
                            error.AppendLine("- " + detailedError);
                        }
                    }

                    return error.ToString();
                });
            }

            protected override void FailEntityEnumTypeFieldDuplicateMapping(FieldInfo field, string mappingUri) => FailEntityEnum(field.DeclaringType, GetFailEntityEnumTypeFieldDuplicateMappingError(field, mappingUri));

            protected override void FailEntityEnumTypeFieldMissingMapping(FieldInfo field) => FailEntityEnum(field.DeclaringType, GetFailEntityEnumTypeFieldMissingMappingError(field));

            private void FailEntityEnum(Type type, string error)
            {
                if (!_entityEnumFailures.TryGetValue(type, out var entityErrors))
                {
                    entityErrors = new List<string>();
                    _entityEnumFailures[type] = entityErrors;
                }

                entityErrors.Add(error);
            }

            protected override DataType FailGenericParameter(Type type) => Fail(type, GetFailGenericParameterError);

            protected override DataType FailGenericType(Type type) => Fail(type, GetFailGenericTypeError);

            protected override DataType FailGenericTypeDefinition(Type type) => Fail(type, GetFailGenericTypeDefinitionError);

            protected override DataType FailNullableType(Type type) => Fail(type, GetFailNullableTypeError);

            protected override DataType FailPointerType(Type type, DataType elementType) => Fail(type, GetFailPointerTypeError);

            protected override DataType FailSimpleType(Type type) => Fail(type, GetFailSimpleTypeError);

            private DataType Fail(Type type, Func<Type, string> getErrorMessage)
            {
                HasFailed = true;
                var error = getErrorMessage(type);
                return new InvalidDataType(type, error);
            }

            public override void Clear()
            {
                base.Clear();

                HasFailed = false;
                _entityFailures.Clear();
                _entityEnumFailures.Clear();
            }

            private sealed class InvalidDataType : DataType
            {
                private readonly string _error;

                public InvalidDataType(Type type, string error)
                    : base(type)
                {
                    _error = error;
                }

                public override DataTypeKinds Kind => DataTypeKinds.Custom;

                public override DataType Reduce() => throw new InvalidOperationException("Invalid data type node detected. Error: " + _error);

                public override string ToString() => "<invalid data type>{" + _error + "}";

                public override object CreateInstance(params object[] arguments) => throw new NotImplementedException();
            }
        }

        private sealed class KnownTypeImpl : Impl
        {
            // WARNING: Objects of this type are pooled; if state is added here, don't forget to override the Clear method!

            public KnownTypeImpl(bool allowCycles)
                : base(allowCycles)
            {
            }

            internal override bool CheckEntityTypePropertiesAndFields(Type type, out IList<KeyValuePair<string, MemberInfo>> mappedMembers)
            {
                base.CheckEntityTypePropertiesAndFields(type, out mappedMembers);
                return true;
            }

            internal override bool CheckEntityTypeConstructors(Type type, IList<KeyValuePair<string, MemberInfo>> mappedMembers)
            {
                base.CheckEntityTypeConstructors(type, mappedMembers);
                return true;
            }

            protected override void FailEntityTypePropertyMissingMapping(MemberInfo property)
            {
            }

            protected override void FailEntityTypePropertyNotReadable(PropertyInfo property)
            {
            }

            protected override void FailEntityTypeConstructorParameterMissingMapping(ConstructorInfo constructor, ParameterInfo parameter)
            {
            }
        }
    }
}
