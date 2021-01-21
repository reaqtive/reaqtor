// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using Nuqleon.DataModel.CompilerServices;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;

namespace Nuqleon.DataModel.TypeSystem
{
    internal abstract class DataModelTypeVisitorBase<TType> : TypeVisitor<TType>
    {
        // WARNING: Known subtypes of this type implement IClearable. Adding state here is a breaking change and requires implementing IClearable with
        //          a virtual Clear method for subtypes to override.

        private static readonly HashSet<Type> s_primitiveTypes = new()
        {
            typeof(Unit),
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(bool),
            typeof(char),
            typeof(string),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Uri),
        };

        protected static readonly Type s_tupleMax = typeof(Tuple<,,,,,,,>);

        private static readonly HashSet<Type> s_tuples = new()
        {
            typeof(Tuple<>),
            typeof(Tuple<,>),
            typeof(Tuple<,,>),
            typeof(Tuple<,,,>),
            typeof(Tuple<,,,,>),
            typeof(Tuple<,,,,,>),
            typeof(Tuple<,,,,,,>),
            s_tupleMax,
        };

        public override TType Visit(Type type)
        {
            if (TryVisitStructuralType(type, out var res))
            {
                return res;
            }

            if (TryMakeFunctionType(type, out res))
            {
                return res;
            }

            return base.Visit(type);
        }

        protected override TType VisitGenericClosed(Type type)
        {
            var genDef = type.GetGenericTypeDefinition();
            var genArg = type.GetGenericArguments();

            if (genDef == typeof(Nullable<>))
            {
                var value = genArg[0];
                if (s_primitiveTypes.Contains(value))
                {
                    return MakePrimitiveType(type);
                }
                else if (value.IsEnum)
                {
                    return VisitEnumType(type);
                }
                else
                {
                    return FailNullableType(type);
                }
            }
            else if (genDef == typeof(List<>) || genDef == typeof(IList<>) || genDef == typeof(IReadOnlyList<>)) // Important: Keep in sync with ArrayDataType code.
            {
                var elementType = Visit(genArg[0]);
                return MakeArrayType(type, elementType);
            }
            else if (genDef.FindGenericType(typeof(Expression<>)) != null) // NB: Need to check hierarchy due to change in .NET Core.
            {
                var functionType = Visit(genArg[0]);
                return MakeQuotationType(type, functionType);
            }
            else if (s_tuples.Contains(genDef))
            {
                var componentTypes = Visit(genArg);
                return MakeTupleType(type, componentTypes);
            }

            return FailGenericType(type);
        }

        protected override TType MakeGenericType(Type type, TType genericTypeDefinition, params TType[] genericArguments)
        {
            throw new NotImplementedException("Open generic data types are not supported; use VisitGenericClosed for analysis of closed generic data types.");
        }

        protected override TType VisitSimple(Type type)
        {
            if (s_primitiveTypes.Contains(type))
            {
                return MakePrimitiveType(type);
            }
            else if (type.IsEnum)
            {
                return VisitEnumType(type);
            }
            else if (type == typeof(void))
            {
                return MakeVoidType(type);
            }
            else if (typeof(Expression).IsAssignableFrom(type))
            {
                return MakeExpressionType(type);
            }
            else if (type == typeof(ExpandoObject))
            {
                return MakeDynamicType(type);
            }
            else if (type.IsDefined(typeof(TypeWildcardAttribute), inherit: false))
            {
                return MakeOpenGenericParameterType(type);
            }

            return FailSimpleType(type);
        }

        protected virtual bool TryVisitStructuralType(Type type, out TType result)
        {
            if (type.IsAnonymousType())
            {
                result = VisitAnonymousType(type);
                return true;
            }
            else if (type.IsRecordType())
            {
                result = VisitRecordType(type);
                return true;
            }
            else if (DataType.IsStructuralEntityDataType(type))
            {
                result = VisitEntityType(type);
                return true;
            }

            result = default;
            return false;
        }

        protected virtual TType VisitEntityType(Type type)
        {
            var res = true;

            res &= CheckEntityTypePropertiesAndFields(type, out var mappedMembers);
            res &= CheckEntityTypeConstructors(type, mappedMembers);

            // TODO: Check against custom methods (?).

            if (res)
            {
                var entityType = DefineEntityType(type);

                var n = mappedMembers.Count;
                var properties = new DataProperty<TType>[n];

                for (var i = 0; i < n; i++)
                {
                    var mappedMember = mappedMembers[i];

                    var name = mappedMember.Key;
                    var member = mappedMember.Value;

                    var dataProperty = MakeDataProperty(member, name);
                    properties[i] = dataProperty;
                }

                return MakeEntityType(entityType, properties);
            }

            return FailEntityType(type);
        }

        internal virtual bool CheckEntityTypePropertiesAndFields(Type type, out IList<KeyValuePair<string, MemberInfo>> mappedMembers)
        {
            var res = true;

            var props = (IEnumerable<MemberInfo>)type.GetProperties(); // TODO: Check against statics.
            var fields = (IEnumerable<MemberInfo>)type.GetFields();
            var members = props.Concat(fields);

            using (var phs = Helpers.NewHashSetOfString())
            {
                var mappings = phs.HashSet;
                mappedMembers = new List<KeyValuePair<string, MemberInfo>>();

                foreach (var member in members)
                {
                    var valid = true;

                    var mapping = member.GetCustomAttribute<MappingAttribute>(inherit: true);

                    if (mapping == null || string.IsNullOrWhiteSpace(mapping.Uri))
                    {
                        FailEntityTypePropertyMissingMapping(member);
                        valid = false;
                    }
                    else if (!mappings.Add(mapping.Uri))
                    {
                        FailEntityTypePropertyDuplicateMapping(member, mapping.Uri);
                        valid = false;
                    }

                    if (member is PropertyInfo property)
                    {
                        if (property.GetGetMethod(/* only public */) == null)
                        {
                            FailEntityTypePropertyNotReadable(property);
                            valid = false;
                        }
                    }

                    if (valid)
                    {
                        var propName = mapping.Uri;
                        mappedMembers.Add(new KeyValuePair<string, MemberInfo>(propName, member));
                    }

                    res = res && valid;
                }
            }

            return res;
        }

        internal virtual bool CheckEntityTypeConstructors(Type type, IList<KeyValuePair<string, MemberInfo>> mappedMembers)
        {
            var res = true;

            var constructors = type.GetConstructors();
            var propertiesByMappingUri = mappedMembers.ToDictionary(kv => kv.Key, kv => kv.Value);

            foreach (var constructor in constructors)
            {
                var valid = true;

                var parameters = constructor.GetParameters();

                foreach (var parameter in parameters)
                {
                    // TODO: Check against out/ref parameters.

                    var mapping = parameter.GetCustomAttribute<MappingAttribute>(inherit: false);
                    if (mapping == null || string.IsNullOrWhiteSpace(mapping.Uri))
                    {
                        FailEntityTypeConstructorParameterMissingMapping(constructor, parameter);
                        valid = false;
                    }
                    else
                    {
                        var mappingUri = mapping.Uri;

                        if (!propertiesByMappingUri.TryGetValue(mapping.Uri, out var property))
                        {
                            FailEntityTypeConstructorParameterInvalidMapping(constructor, parameter, mappingUri);
                            valid = false;
                        }
                        else
                        {
                            if (Helpers.GetMemberType(property) != parameter.ParameterType)
                            {
                                FailEntityTypeConstructorParameterAndPropertyTypeMismatch(constructor, parameter, mappingUri, property);
                                valid = false;
                            }
                        }
                    }
                }

                res = res && valid;
            }

            return res;
        }

        protected virtual bool TryMakeFunctionType(Type type, out TType result)
        {
            if (typeof(Delegate).IsAssignableFrom(type) && type != typeof(Delegate) && type != typeof(MulticastDelegate))
            {
                var invoke = type.GetMethod("Invoke");

                var returnType = Visit(invoke.ReturnType);

                var invokeParameters = invoke.GetParameters();
                var parameterCount = invokeParameters.Length;
                var parameterTypes = new TType[parameterCount];
                for (var i = 0; i < parameterCount; i++)
                {
                    parameterTypes[i] = Visit(invokeParameters[i].ParameterType);
                }

                result = MakeFunctionType(type, parameterTypes, returnType);

                return true;
            }

            result = default;
            return false;
        }

        protected abstract TType MakeVoidType(Type type);

        protected abstract TType MakePrimitiveType(Type type);

        protected virtual TType VisitEnumType(Type type)
        {
            if (DataType.IsEntityEnumDataType(type))
            {
                return VisitEntityEnumType(type);
            }
            else
            {
                return MakeEnumType(type);
            }
        }

        protected virtual TType VisitEntityEnumType(Type type)
        {
#if DEBUG
            var enumType = type;

            if (!type.IsEnum)
            {
                enumType = Nullable.GetUnderlyingType(type);
            }

            System.Diagnostics.Debug.Assert(enumType.IsEnum);
#endif

            if (CheckEntityEnumFields(type, out var mappedFields))
            {
                var n = mappedFields.Count;

                var enumValues = new DataEnumValue[n];

                for (var i = 0; i < n; i++)
                {
                    var mappedField = mappedFields[i];

                    var name = mappedField.Key;
                    var field = mappedField.Value;

                    var enumValue = MakeDataEnumValue(field, name);
                    enumValues[i] = enumValue;
                }

                return MakeEntityEnumType(type, enumValues);
            }

            return FailEntityEnumType(type);
        }

        private bool CheckEntityEnumFields(Type type, out IList<KeyValuePair<string, FieldInfo>> mappedFields)
        {
            var res = true;

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            using (var phs = Helpers.NewHashSetOfString())
            {
                var mappings = phs.HashSet;
                mappedFields = new List<KeyValuePair<string, FieldInfo>>();

                foreach (var field in fields)
                {
                    var valid = true;

                    var mapping = field.GetCustomAttribute<MappingAttribute>(inherit: true);

                    if (mapping == null || string.IsNullOrWhiteSpace(mapping.Uri))
                    {
                        FailEntityEnumTypeFieldMissingMapping(field);
                        valid = false;
                    }
                    else if (!mappings.Add(mapping.Uri))
                    {
                        FailEntityEnumTypeFieldDuplicateMapping(field, mapping.Uri);
                        valid = false;
                    }

                    if (valid)
                    {
                        var fieldName = mapping.Uri;
                        mappedFields.Add(new KeyValuePair<string, FieldInfo>(fieldName, field));
                    }

                    res = res && valid;
                }
            }

            return res;
        }

        protected abstract TType MakeEnumType(Type type);

        protected abstract TType MakeEntityEnumType(Type type, IEnumerable<DataEnumValue> values);

        protected abstract TType MakeExpressionType(Type type);

        protected abstract TType MakeQuotationType(Type type, TType functionType);

        protected abstract TType MakeTupleType(Type type, IEnumerable<TType> components);

        protected abstract TType MakeDynamicType(Type type);

        protected abstract TType MakeOpenGenericParameterType(Type type);

        protected virtual TType VisitAnonymousType(Type type)
        {
            var anonType = DefineAnonymousType(type);

            var anonCtor = type.GetConstructors().Single();

            var parameters = anonCtor.GetParameters();
            var n = parameters.Length;

            var props = new DataProperty<TType>[n];

            for (var i = 0; i < n; i++)
            {
                var parameter = parameters[i];
                var prop = type.GetProperty(parameter.Name);
                props[i] = MakeDataProperty(prop);
            }

            return MakeAnonymousType(anonType, props);
        }

        protected abstract TType DefineAnonymousType(Type type);

        protected abstract TType MakeAnonymousType(TType type, IEnumerable<DataProperty<TType>> properties);

        protected virtual TType VisitRecordType(Type type)
        {
            var recordType = DefineRecordType(type);

            var properties = type.GetProperties();
            var n = properties.Length;

            var props = new DataProperty<TType>[n];

            for (var i = 0; i < n; i++)
            {
                var prop = properties[i];
                props[i] = MakeDataProperty(prop);
            }

            return MakeRecordType(recordType, props);
        }

        protected abstract TType DefineRecordType(Type type);

        protected abstract TType MakeRecordType(TType type, IEnumerable<DataProperty<TType>> entries);

        protected abstract TType DefineEntityType(Type type);

        protected abstract TType MakeEntityType(TType type, IEnumerable<DataProperty<TType>> properties);

        protected abstract TType MakeFunctionType(Type type, TType[] parameterTypes, TType returnType);

        private DataProperty<TType> MakeDataProperty(MemberInfo property) => MakeDataProperty(property, property.Name);

        protected virtual DataProperty<TType> MakeDataProperty(MemberInfo property, string name)
        {
            var type = Visit(Helpers.GetMemberType(property));
            return new DataProperty<TType>(property, name, type);
        }

        protected virtual DataEnumValue MakeDataEnumValue(FieldInfo field, string name)
        {
            var value = field.GetValue(obj: null);
            var rawValue = Convert.ChangeType(value, field.DeclaringType.GetEnumUnderlyingType(), CultureInfo.InvariantCulture);
            return new DataEnumValue(name, rawValue);
        }

        #region Unsupported type kinds

        protected override TType MakeArrayType(Type type, TType elementType, int rank) => FailArrayType(type, elementType, rank);

        protected override TType MakeByRefType(Type type, TType elementType) => FailByRefType(type, elementType);

        protected override TType MakePointerType(Type type, TType elementType) => FailPointerType(type, elementType);

        protected override TType VisitGenericParameter(Type type) => FailGenericParameter(type);

        protected override TType VisitGenericTypeDefinition(Type type) => FailGenericTypeDefinition(type);

        #endregion

        #region Failure reporting

        //
        // Important: if any failure cases are added, reconsider the TypeToDataTypeConverter overrides.
        //

        protected virtual TType FailSimpleType(Type type) => throw new NotSupportedException(GetFailSimpleTypeError(type));

        protected static string GetFailSimpleTypeError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is not supported by the data model.", type.ToCSharpStringPretty());

        protected virtual TType FailGenericType(Type type) => throw new NotSupportedException(GetFailGenericTypeError(type));

        protected static string GetFailGenericTypeError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is a generic type using type definition '{1}', which is not supported by the data model.", type.ToCSharpStringPretty(), type.GetGenericTypeDefinition().ToCSharpStringPretty());

        protected virtual TType FailNullableType(Type type) => throw new NotSupportedException(GetFailNullableTypeError(type));

        protected static string GetFailNullableTypeError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is a nullable type of a value type '{1}', which is not supported by the data model.", type.ToCSharpStringPretty(), type.GetGenericArguments()[0].ToCSharpStringPretty());

        protected virtual TType FailArrayType(Type type, TType elementType, int rank) => throw new NotSupportedException(GetFailArrayTypeError(type));

        protected static string GetFailArrayTypeError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is a multi-dimensional array, which is not supported by the data model.", type.ToCSharpStringPretty());

        protected virtual TType FailByRefType(Type type, TType elementType) => throw new NotSupportedException(GetFailByRefTypeError(type));

        protected static string GetFailByRefTypeError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is a by ref type, which is not supported by the data model.", type.ToCSharpStringPretty());

        protected virtual TType FailPointerType(Type type, TType elementType) => throw new NotSupportedException(GetFailPointerTypeError(type));

        protected static string GetFailPointerTypeError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is a pointer type, which is not supported by the data model.", type.ToCSharpStringPretty());

        protected virtual TType FailGenericParameter(Type type) => throw new NotSupportedException(GetFailGenericParameterError(type));

        protected static string GetFailGenericParameterError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is a generic parameter, which is not supported by the data model.", type.ToCSharpStringPretty());

        protected virtual TType FailGenericTypeDefinition(Type type) => throw new NotSupportedException(GetFailGenericTypeDefinitionError(type));

        protected static string GetFailGenericTypeDefinitionError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is an open generic type definition, which is not supported by the data model.", type.ToCSharpStringPretty());

        protected virtual void FailEntityTypePropertyMissingMapping(MemberInfo property) => throw new InvalidOperationException(GetFailEntityTypePropertyMissingMappingError(property));

        protected static string GetFailEntityTypePropertyMissingMappingError(MemberInfo property) => string.Format(CultureInfo.InvariantCulture, "Property or field '{0}' on type '{1}' does not have a mapping attribute, or its value is invalid. In order for type '{1}' to be valid data model entity type, all of its properties should be annotated with mapping attributes.", property.Name, property.DeclaringType.ToCSharpStringPretty());

        protected virtual void FailEntityEnumTypeFieldMissingMapping(FieldInfo field) => throw new InvalidOperationException(GetFailEntityEnumTypeFieldMissingMappingError(field));

        protected static string GetFailEntityEnumTypeFieldMissingMappingError(FieldInfo field) => string.Format(CultureInfo.InvariantCulture, "Field '{0}' on type '{1}' does not have a mapping attribute, or its value is invalid. In order for type '{1}' to be valid data model entity enum type, all of its fields should be annotated with mapping attributes.", field.Name, field.DeclaringType.ToCSharpStringPretty());

        protected virtual void FailEntityTypePropertyDuplicateMapping(MemberInfo property, string mappingUri) => throw new InvalidOperationException(GetFailEntityTypePropertyDuplicateMappingError(property, mappingUri));

        protected static string GetFailEntityTypePropertyDuplicateMappingError(MemberInfo property, string mappingUri) => string.Format(CultureInfo.InvariantCulture, "Property or field '{0}' on type '{1}' has a mapping attribute whose value '{2}' is already used for another property. Mappings should be unique for data model entity types.", property.Name, property.DeclaringType.ToCSharpStringPretty(), mappingUri);

        protected virtual void FailEntityEnumTypeFieldDuplicateMapping(FieldInfo field, string mappingUri) => throw new InvalidOperationException(GetFailEntityEnumTypeFieldDuplicateMappingError(field, mappingUri));

        protected static string GetFailEntityEnumTypeFieldDuplicateMappingError(FieldInfo field, string mappingUri) => string.Format(CultureInfo.InvariantCulture, "Field '{0}' on type '{1}' has a mapping attribute whose value '{2}' is already used for another field. Mappings should be unique for data model entity enum types.", field.Name, field.DeclaringType.ToCSharpStringPretty(), mappingUri);

        protected virtual void FailEntityTypePropertyNotReadable(PropertyInfo property) => throw new InvalidOperationException(GetFailEntityTypePropertyNotReadableError(property));

        protected static string GetFailEntityTypePropertyNotReadableError(PropertyInfo property) => string.Format(CultureInfo.InvariantCulture, "Property '{0}' on type '{1}' is not readable, which is required by the data model.", property.Name, property.DeclaringType.ToCSharpStringPretty());

        protected virtual void FailEntityTypeConstructorParameterMissingMapping(ConstructorInfo constructor, ParameterInfo parameter) => throw new InvalidOperationException(GetFailEntityTypeConstructorParameterMissingMappingError(constructor, parameter));

        protected static string GetFailEntityTypeConstructorParameterMissingMappingError(ConstructorInfo constructor, ParameterInfo parameter) => string.Format(CultureInfo.InvariantCulture, "Parameter '{0}' on the constructor of '{1}' does not have a mapping attribute, or its value is invalid. In order for type '{1}' to be a valid data model entity type, all of its constructors' parameters should be annotated with mapping attributes.", parameter.Name, constructor.DeclaringType.ToCSharpStringPretty());

        protected virtual void FailEntityTypeConstructorParameterInvalidMapping(ConstructorInfo constructor, ParameterInfo parameter, string mappingUri) => throw new InvalidOperationException(GetFailEntityTypeConstructorParameterInvalidMappingError(constructor, parameter, mappingUri));

        protected static string GetFailEntityTypeConstructorParameterInvalidMappingError(ConstructorInfo constructor, ParameterInfo parameter, string mappingUri) => string.Format(CultureInfo.InvariantCulture, "Parameter '{0}' on the constructor of '{1}' has a mapping attribute '{2}' with no corresponding property declaration.", parameter.Name, constructor.DeclaringType.ToCSharpStringPretty(), mappingUri);

        protected virtual void FailEntityTypeConstructorParameterAndPropertyTypeMismatch(ConstructorInfo constructor, ParameterInfo parameter, string mappingUri, MemberInfo property) => throw new InvalidOperationException(GetFailEntityTypeConstructorParameterAndPropertyTypeMismatchError(constructor, parameter, mappingUri, property));

        protected static string GetFailEntityTypeConstructorParameterAndPropertyTypeMismatchError(ConstructorInfo constructor, ParameterInfo parameter, string mappingUri, MemberInfo property) => string.Format(CultureInfo.InvariantCulture, "Parameter '{0}' on the constructor of '{1}' has a different type than property or field '{2}' with the same mapping attribute '{3}'.", parameter.Name, constructor.DeclaringType.ToCSharpStringPretty(), property.Name, mappingUri);

        protected virtual TType FailEntityType(Type type) => throw new InvalidOperationException(GetFailEntityTypeError(type));

        protected static string GetFailEntityTypeError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is an invalid entity type. Please make sure all properties are properly annotated with mapping attributes.", type.ToCSharpStringPretty());

        protected virtual TType FailEntityEnumType(Type type) => throw new InvalidOperationException(GetFailEntityEnumTypeError(type));

        protected static string GetFailEntityEnumTypeError(Type type) => string.Format(CultureInfo.InvariantCulture, "Type '{0}' is an invalid entity enum type. Please make sure all fields are properly annotated with mapping attributes.", type.ToCSharpStringPretty());

        #endregion
    }
}
