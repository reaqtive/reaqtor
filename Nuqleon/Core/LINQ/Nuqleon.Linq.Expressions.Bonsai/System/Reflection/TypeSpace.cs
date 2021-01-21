// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks. (Protected methods are supposed to be called with non-null arguments.)

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.CompilerServices;

namespace System.Reflection
{
    /// <summary>
    /// A container for mappings from types to slim types and members to slim members.
    /// </summary>
    public class TypeSpace
    {
        #region Fields

        private readonly Dictionary<PropertyInfo, PropertyInfoSlim> _properties;
        private readonly Dictionary<FieldInfo, FieldInfoSlim> _fields;
        private readonly Dictionary<MethodInfo, MethodInfoSlim> _methods;
        private readonly Dictionary<ConstructorInfo, ConstructorInfoSlim> _constructors;
        private readonly TypeToTypeSlimConverter _typeConverter;

        private static readonly ReadOnlyCollection<TypeSlim> s_emptyTypes = new TrueReadOnlyCollection<TypeSlim>(Array.Empty<TypeSlim>());

        #endregion

        #region Constructors

        /// <summary>
        /// Creates the type space.
        /// </summary>
        public TypeSpace()
        {
            // TODO: equality comparers modulo ReflectedType
            _properties = new Dictionary<PropertyInfo, PropertyInfoSlim>();
            _fields = new Dictionary<FieldInfo, FieldInfoSlim>();
            _methods = new Dictionary<MethodInfo, MethodInfoSlim>();
            _constructors = new Dictionary<ConstructorInfo, ConstructorInfoSlim>();
            _typeConverter = new TypeToTypeSlimConverter();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type conversion instance used for this type space.
        /// </summary>
        protected virtual TypeToTypeSlimConverter TypeConverter => _typeConverter;

        #endregion

        #region Methods

        /// <summary>
        /// Manually adds mapping from a CLR type to a slim type
        /// </summary>
        /// <param name="type">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        public virtual void MapType(Type type, TypeSlim typeSlim)
        {
            TypeConverter.MapType(type, typeSlim);
        }

        /// <summary>
        /// Gets a slim type from a CLR type.
        /// </summary>
        /// <param name="type">The CLR type.</param>
        /// <returns>A slim representation of the CLR type.</returns>
        public virtual TypeSlim ConvertType(Type type)
        {
            return TypeConverter.Visit(type);
        }

        /// <summary>
        /// Gets a slim version of a member.
        /// </summary>
        /// <param name="member">The original member.</param>
        /// <returns>The slim representation of the member.</returns>
        public MemberInfoSlim GetMember(MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            var memberType = member.GetMemberType();

            return memberType switch
            {
                MemberTypes.Constructor => GetConstructor((ConstructorInfo)member),
                MemberTypes.Field => GetField((FieldInfo)member),
                MemberTypes.Method => GetMethod((MethodInfo)member),
                MemberTypes.Property => GetProperty((PropertyInfo)member),
                _ => throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Member type '{0}' is not supported.", memberType)),
            };
        }


        /// <summary>
        /// Gets a slim version of a constructor.
        /// </summary>
        /// <param name="constructor">The original constructor.</param>
        /// <returns>The slim representation of the constructor.</returns>
        public ConstructorInfoSlim GetConstructor(ConstructorInfo constructor)
        {
            if (constructor == null)
            {
                throw new ArgumentNullException(nameof(constructor));
            }

            if (_constructors.TryGetValue(constructor, out ConstructorInfoSlim res))
            {
                return res;
            }

            var declaringType = ConvertType(constructor.DeclaringType);

            var parameterTypes = ConvertParameterTypes(constructor);

            res = GetConstructorCore(constructor, declaringType, parameterTypes);

            _constructors[constructor] = res;
            return res;
        }

        /// <summary>
        /// Creates a slim constructor from the original constructor.
        /// </summary>
        /// <param name="originalConstructor">The original constructor.</param>
        /// <param name="declaringTypeSlim">The slim version of the declaring type.</param>
        /// <param name="parameterTypeSlims">The slim versions of the constructor parameter types.</param>
        /// <returns>The slim representation of the constructor.</returns>
        protected virtual ConstructorInfoSlim GetConstructorCore(ConstructorInfo originalConstructor, TypeSlim declaringTypeSlim, ReadOnlyCollection<TypeSlim> parameterTypeSlims)
        {
            return declaringTypeSlim.GetConstructor(parameterTypeSlims);
        }

        /// <summary>
        /// Gets a slim version of a method.
        /// </summary>
        /// <param name="method">The original method.</param>
        /// <returns>The slim representation of the method.</returns>
        public MethodInfoSlim GetMethod(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (_methods.TryGetValue(method, out MethodInfoSlim res))
            {
                return res;
            }

            if (method.IsGenericMethod)
            {
                if (method.IsGenericMethodDefinition)
                {
                    res = GetGenericDefinitionMethod(method);
                }
                else
                {
                    res = GetGenericMethod(method);
                }
            }
            else
            {
                res = GetSimpleMethod(method);
            }

            _methods[method] = res;
            return res;
        }

        private MethodInfoSlim GetSimpleMethod(MethodInfo method)
        {
            var declaringType = ConvertType(method.DeclaringType);
            var parameterTypes = ConvertParameterTypes(method);
            var returnType = ConvertType(method.ReturnType);
            return GetSimpleMethodCore(method, declaringType, parameterTypes, returnType);
        }

        /// <summary>
        /// Creates a slim simple method from the original simple method.
        /// </summary>
        /// <param name="originalMethod">The original method.</param>
        /// <param name="declaringTypeSlim">The slim version of the declaring type.</param>
        /// <param name="parameterTypeSlims">The slim versions of the method parameter types.</param>
        /// <param name="returnTypeSlim">The slim version of the return type.</param>
        /// <returns>The slim representation of the method.</returns>
        protected virtual MethodInfoSlim GetSimpleMethodCore(MethodInfo originalMethod, TypeSlim declaringTypeSlim, ReadOnlyCollection<TypeSlim> parameterTypeSlims, TypeSlim returnTypeSlim)
        {
            return declaringTypeSlim.GetSimpleMethod(originalMethod.Name, parameterTypeSlims, returnTypeSlim);
        }

        private MethodInfoSlim GetGenericDefinitionMethod(MethodInfo method)
        {
            var declaringType = ConvertType(method.DeclaringType);

            var genArgs = method.GetGenericArguments();
            var n = genArgs.Length;
            var genericParameterMap = new Dictionary<Type, TypeSlim>(n);
            var argsList = new TypeSlim[n];

            for (var i = 0; i < n; i++)
            {
                var genArg = genArgs[i];
                var genArgSlim = (TypeSlim)TypeSlim.GenericParameter(genArg.Name);
                argsList[i] = genArgSlim;
                genericParameterMap[genArg] = genArgSlim;
            }

            var args = new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ argsList);

            TypeConverter.Push(genericParameterMap);

            var parameterTypes = ConvertParameterTypes(method);
            var returnType = ConvertType(method.ReturnType);

            TypeConverter.Pop();

            return GetGenericDefinitionMethodCore(method, declaringType, args, parameterTypes, returnType);
        }

        /// <summary>
        /// Creates a slim generic definition method from the original method.
        /// </summary>
        /// <param name="originalMethod">The original method.</param>
        /// <param name="declaringTypeSlim">The slim version of the declaring type.</param>
        /// <param name="genericParameterTypeSlims">The slim versions of the generic parameter types.</param>
        /// <param name="parameterTypeSlims">The slim versions of the method parameter types.</param>
        /// <param name="returnTypeSlim">The slim version of the return type.</param>
        /// <returns>The slim representation of the method.</returns>
        protected virtual MethodInfoSlim GetGenericDefinitionMethodCore(MethodInfo originalMethod, TypeSlim declaringTypeSlim, ReadOnlyCollection<TypeSlim> genericParameterTypeSlims, ReadOnlyCollection<TypeSlim> parameterTypeSlims, TypeSlim returnTypeSlim)
        {
            return declaringTypeSlim.GetGenericDefinitionMethod(originalMethod.Name, genericParameterTypeSlims, parameterTypeSlims, returnTypeSlim);
        }

        private MethodInfoSlim GetGenericMethod(MethodInfo method)
        {
            var md = method.GetGenericMethodDefinition();
            var methodDefinition = (GenericDefinitionMethodInfoSlim)GetMethod(md);

            var genArgs = method.GetGenericArguments();
            var n = genArgs.Length;
            var argsList = new TypeSlim[n];

            for (var i = 0; i < n; i++)
            {
                argsList[i] = ConvertType(genArgs[i]);
            }

            var args = new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ argsList);

            return GetGenericMethodCore(method, methodDefinition, args);
        }

        /// <summary>
        /// Creates a slim generic method from the original method.
        /// </summary>
        /// <param name="originalMethod">The original method.</param>
        /// <param name="methodDefinitionSlim">The slim version of the generic method definition.</param>
        /// <param name="genericParameterTypeSlims">The slim versions of the generic parameter types.</param>
        /// <returns>The slim representation of the method.</returns>
        protected virtual MethodInfoSlim GetGenericMethodCore(MethodInfo originalMethod, GenericDefinitionMethodInfoSlim methodDefinitionSlim, ReadOnlyCollection<TypeSlim> genericParameterTypeSlims)
        {
            if (methodDefinitionSlim == null)
            {
                throw new ArgumentNullException(nameof(methodDefinitionSlim));
            }

            return methodDefinitionSlim.DeclaringType.GetGenericMethod(methodDefinitionSlim, genericParameterTypeSlims);
        }

        /// <summary>
        /// Gets a slim version of a property.
        /// </summary>
        /// <param name="property">The original property.</param>
        /// <returns>The slim representation of the property.</returns>
        public PropertyInfoSlim GetProperty(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (_properties.TryGetValue(property, out PropertyInfoSlim res))
            {
                return res;
            }

            var declaringType = ConvertType(property.DeclaringType);
            var propertyType = ConvertType(property.PropertyType);
            var indexParameterTypes = ConvertParameterTypes(property.GetIndexParameters());
            res = GetPropertyCore(property, declaringType, propertyType, indexParameterTypes);

            _properties[property] = res;

            return res;
        }

        /// <summary>
        /// Creates a slim property from the original property.
        /// </summary>
        /// <param name="originalProperty">The original property.</param>
        /// <param name="declaringTypeSlim">The slim version of the declaring type.</param>
        /// <param name="propertyTypeSlim">The slim version of the property type.</param>
        /// <param name="indexParameterTypeSlims">The slim versions of the index parameter types.</param>
        /// <returns>The slim representation of the property.</returns>
        protected virtual PropertyInfoSlim GetPropertyCore(PropertyInfo originalProperty, TypeSlim declaringTypeSlim, TypeSlim propertyTypeSlim, ReadOnlyCollection<TypeSlim> indexParameterTypeSlims)
        {
            return declaringTypeSlim.GetProperty(originalProperty.Name, propertyTypeSlim, indexParameterTypeSlims, originalProperty.CanWrite);
        }

        /// <summary>
        /// Gets a slim version of a field.
        /// </summary>
        /// <param name="field">The original field.</param>
        /// <returns>The slim representation of the field.</returns>
        public FieldInfoSlim GetField(FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (_fields.TryGetValue(field, out FieldInfoSlim res))
            {
                return res;
            }

            var declaringType = ConvertType(field.DeclaringType);
            var fieldType = ConvertType(field.FieldType);
            res = GetFieldCore(field, declaringType, fieldType);

            _fields[field] = res;

            return res;
        }

        /// <summary>
        /// Creates a slim field from the original field.
        /// </summary>
        /// <param name="originalField">The original field.</param>
        /// <param name="declaringTypeSlim">The slim version of the declaring type.</param>
        /// <param name="fieldTypeSlim">The slim version of the field type.</param>
        /// <returns>The slim representation of the field.</returns>
        protected virtual FieldInfoSlim GetFieldCore(FieldInfo originalField, TypeSlim declaringTypeSlim, TypeSlim fieldTypeSlim)
        {
            return declaringTypeSlim.GetField(originalField.Name, fieldTypeSlim);
        }

        /// <summary>
        /// Converts the parameter types of the specified method to a slim representation.
        /// </summary>
        /// <param name="method">Method to obtain slim parameter types for.</param>
        /// <returns>The list of slim parameter types of the specified method.</returns>
        private ReadOnlyCollection<TypeSlim> ConvertParameterTypes(MethodBase method)
        {
            return ConvertParameterTypes(method.GetParameters());
        }

        /// <summary>
        /// Converts the parameter types to a slim representation.
        /// </summary>
        /// <param name="parameters">Parameters to obtain slim type representations for.</param>
        /// <returns>The list of slim types of the specified parameters.</returns>
        private ReadOnlyCollection<TypeSlim> ConvertParameterTypes(ParameterInfo[] parameters)
        {
            var n = parameters.Length;

            if (n == 0)
            {
                return s_emptyTypes;
            }

            var types = new TypeSlim[n];

            for (var i = 0; i < n; i++)
            {
                types[i] = ConvertType(parameters[i].ParameterType);
            }

            return new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ types);
        }

        #endregion
    }
}
