// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks. (Protected methods are supposed to be called with non-null arguments.)

using System.Collections.Generic;
using System.Globalization;

namespace System.Reflection
{
    /// <summary>
    /// A container for mappings from slim types to types and slim members to members.
    /// </summary>
    public class InvertedTypeSpace
    {
        #region Fields

        private readonly Dictionary<PropertyInfoSlim, PropertyInfo> _properties;
        private readonly Dictionary<FieldInfoSlim, FieldInfo> _fields;
        private readonly Dictionary<MethodInfoSlim, MethodInfo> _methods;
        private readonly Dictionary<ConstructorInfoSlim, ConstructorInfo> _constructors;
        private readonly TypeSlimToTypeConverter _typeConverter;
        private readonly IReflectionProvider _provider;

        private static readonly Type[] s_emptyTypes = Array.Empty<Type>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates the type space.
        /// </summary>
        public InvertedTypeSpace()
            : this(DefaultReflectionProvider.Instance)
        {
        }

        /// <summary>
        /// Creates the type space.
        /// </summary>
        /// <param name="provider">The reflection provider to use.</param>
        public InvertedTypeSpace(IReflectionProvider provider)
        {
            _properties = new Dictionary<PropertyInfoSlim, PropertyInfo>();
            _fields = new Dictionary<FieldInfoSlim, FieldInfo>();
            _methods = new Dictionary<MethodInfoSlim, MethodInfo>();
            _constructors = new Dictionary<ConstructorInfoSlim, ConstructorInfo>();
            _typeConverter = new TypeSlimToTypeConverter(provider);
            _provider = provider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Manually adds a mapping from a slim type to a CLR type.
        /// </summary>
        /// <param name="typeSlim">The slim type.</param>
        /// <param name="type">The CLR type.</param>
        public void MapType(TypeSlim typeSlim, Type type)
        {
            _typeConverter.MapType(typeSlim, type);
        }

        /// <summary>
        /// Gets a CLR type from a slim type.
        /// </summary>
        /// <param name="type">The slim type.</param>
        /// <returns>The CLR type represented by the slim type.</returns>
        public Type ConvertType(TypeSlim type)
        {
            return _typeConverter.Visit(type);
        }

        /// <summary>
        /// Gets a member from a slim member.
        /// </summary>
        /// <param name="memberSlim">The slim member.</param>
        /// <returns>The member represented by the slim member.</returns>
        public MemberInfo GetMember(MemberInfoSlim memberSlim)
        {
            if (memberSlim == null)
                throw new ArgumentNullException(nameof(memberSlim));

            return memberSlim.MemberType switch
            {
                MemberTypes.Constructor => GetConstructor((ConstructorInfoSlim)memberSlim),
                MemberTypes.Field => GetField((FieldInfoSlim)memberSlim),
                MemberTypes.Method => GetMethod((MethodInfoSlim)memberSlim),
                MemberTypes.Property => GetProperty((PropertyInfoSlim)memberSlim),
                _ => throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Member type '{0}' not supported.", memberSlim.MemberType)),
            };
        }

        /// <summary>
        /// Gets a constructor from a slim constructor.
        /// </summary>
        /// <param name="constructorSlim">The slim constructor.</param>
        /// <returns>The constructor represented by the slim constructor.</returns>
        public ConstructorInfo GetConstructor(ConstructorInfoSlim constructorSlim)
        {
            if (constructorSlim == null)
                throw new ArgumentNullException(nameof(constructorSlim));

            if (_constructors.TryGetValue(constructorSlim, out ConstructorInfo res))
            {
                return res;
            }

            var declaringType = ConvertType(constructorSlim.DeclaringType);
            var parameterTypes = ConvertTypes(constructorSlim.ParameterTypes);

            res = GetConstructorCore(constructorSlim, declaringType, parameterTypes);

            _constructors[constructorSlim] = res;
            return res;
        }

        /// <summary>
        /// Gets a constructor from a CLR type.
        /// </summary>
        /// <param name="constructorSlim">The slim constructor.</param>
        /// <param name="declaringType">The declaring CLR type.</param>
        /// <param name="parameterTypes">The constructor parameter CLR types.</param>
        /// <returns>The declaring type constructor with given parameter types.</returns>
        protected virtual ConstructorInfo GetConstructorCore(ConstructorInfoSlim constructorSlim, Type declaringType, Type[] parameterTypes)
        {
            var constructor = _provider.GetConstructor(declaringType, parameterTypes);
            if (constructor == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find a constructor on type '{0}' with parameters '{1}'.", declaringType, string.Join<Type>(",", parameterTypes)));

            return constructor;
        }

        /// <summary>
        /// Gets a method from a slim method.
        /// </summary>
        /// <param name="methodSlim">The slim method.</param>
        /// <returns>The method represented by the slim method.</returns>
        public MethodInfo GetMethod(MethodInfoSlim methodSlim)
        {
            if (methodSlim == null)
                throw new ArgumentNullException(nameof(methodSlim));

            if (_methods.TryGetValue(methodSlim, out MethodInfo res))
            {
                return res;
            }

            res = methodSlim.Kind switch
            {
                MethodInfoSlimKind.Simple => GetSimpleMethod((SimpleMethodInfoSlim)methodSlim),
                MethodInfoSlimKind.GenericDefinition => GetGenericDefinitionMethod((GenericDefinitionMethodInfoSlim)methodSlim),
                MethodInfoSlimKind.Generic => GetGenericMethod((GenericMethodInfoSlim)methodSlim),
                _ => throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Method type '{0}' is not supported.", methodSlim.Kind)),
            };
            _methods[methodSlim] = res;
            return res;
        }

        private MethodInfo GetSimpleMethod(SimpleMethodInfoSlim methodSlim)
        {
            var declaringType = ConvertType(methodSlim.DeclaringType);
            var parameterTypes = ConvertTypes(methodSlim.ParameterTypes);
            var returnType = methodSlim.ReturnType != null ? ConvertType(methodSlim.ReturnType) : null;

            return GetSimpleMethodCore(methodSlim, declaringType, parameterTypes, returnType);
        }

        /// <summary>
        /// Gets a method on a CLR type.
        /// </summary>
        /// <param name="methodSlim">The slim method.</param>
        /// <param name="declaringType">The declaring CLR type.</param>
        /// <param name="parameterTypes">The parameter CLR types.</param>
        /// <param name="returnType">The return CLR type.</param>
        /// <returns>The method on the declaring type.</returns>
        protected virtual MethodInfo GetSimpleMethodCore(SimpleMethodInfoSlim methodSlim, Type declaringType, Type[] parameterTypes, Type returnType)
        {
            var parameterCount = parameterTypes.Length;

            var failed = false;
            var method = default(MethodInfo);

            var candidates = _provider.GetMethods(declaringType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var candidate in candidates)
            {
                //
                // Name should match.
                //
                if (candidate.Name != methodSlim.Name)
                {
                    continue;
                }

                //
                // Only simple methods are considered.
                //
                if (candidate.IsGenericMethod)
                {
                    continue;
                }

                //
                // The return type should match, if specified.
                //
                if (returnType != null && candidate.ReturnType != returnType)
                {
                    continue;
                }

                //
                // Number of parameters should match.
                //
                var candidateParameters = _provider.GetParameters(candidate);

                if (candidateParameters.Count != parameterTypes.Length)
                {
                    continue;
                }

                //
                // Signature should match.
                //
                var mismatch = false;

                for (var i = 0; i < parameterCount; i++)
                {
                    var requestedParameterType = parameterTypes[i];
                    var candidateParameterType = candidateParameters[i].ParameterType;

                    if (candidateParameterType != requestedParameterType)
                    {
                        mismatch = true;
                        break;
                    }
                }

                if (mismatch)
                {
                    continue;
                }

                //
                // Ensure there's only one match.
                //
                if (method == null)
                {
                    method = candidate;
                }
                else
                {
                    failed = true;
                }
            }

            failed |= method == null;

            if (failed)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find simple method '{0}' with parameters '{1}' and return type '{2}' on type '{3}'.", methodSlim.Name, parameterTypes, returnType, declaringType));

            return method;
        }

        private MethodInfo GetGenericDefinitionMethod(GenericDefinitionMethodInfoSlim method)
        {
            var declaringType = ConvertType(method.DeclaringType);
            return GetGenericDefinitionMethodCore(method, declaringType);
        }

        /// <summary>
        /// Gets a generic definition method from a slim generic definition method.
        /// </summary>
        /// <param name="methodSlim">The slim method.</param>
        /// <param name="declaringType">The declaring CLR type.</param>
        /// <returns>The method represented by the slim method.</returns>
        protected virtual MethodInfo GetGenericDefinitionMethodCore(GenericDefinitionMethodInfoSlim methodSlim, Type declaringType)
        {
            var slimGenericParameterTypes = methodSlim.GenericParameterTypes;
            var slimParameterTypes = methodSlim.ParameterTypes;

            var arity = slimGenericParameterTypes.Count;
            var parameterCount = slimParameterTypes.Count;

            var failed = false;
            var method = default(MethodInfo);
            var genericParameterMap = default(Dictionary<TypeSlim, Type>);

            var candidates = _provider.GetMethods(declaringType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var candidate in candidates)
            {
                //
                // Name should match. Notice that the name of a generic method does not contain an arity indicator (a la Func`1).
                //
                if (candidate.Name != methodSlim.Name)
                {
                    continue;
                }

                //
                // Only open generic methods are considered.
                //
                if (!candidate.IsGenericMethodDefinition)
                {
                    continue;
                }

                //
                // Parameter count should match.
                //
                var candidateParameters = _provider.GetParameters(candidate);

                if (candidateParameters.Count != slimParameterTypes.Count)
                {
                    continue;
                }

                //
                // Generic arity should match.
                //
                var candidateGenericArgumentTypes = candidate.GetGenericArguments();

                if (candidateGenericArgumentTypes.Length != slimGenericParameterTypes.Count)
                {
                    continue;
                }

                //
                // Signature should match.
                //
                {
                    //
                    // First, create a map of slim type and CLR type representations of generic arguments.
                    //
                    genericParameterMap ??= new Dictionary<TypeSlim, Type>(arity);

                    for (var i = 0; i < arity; i++)
                    {
                        var slimGenericParameterType = slimGenericParameterTypes[i];
                        var candidateTypeGenericParameterType = candidateGenericArgumentTypes[i];

                        genericParameterMap[slimGenericParameterType] = candidateTypeGenericParameterType;
                    }

                    //
                    // Second, push the type map to the converter. This enables us to convert return type and parameter types using the candidate's generic arguments.
                    //
                    _typeConverter.Push(genericParameterMap);

                    try
                    {
                        //
                        // Return type should match.
                        //
                        var returnType = ConvertType(methodSlim.ReturnType);

                        if (candidate.ReturnType != returnType)
                        {
                            continue;
                        }

                        //
                        // Parameter types should match.
                        //
                        var mismatch = false;

                        for (var i = 0; i < parameterCount; i++)
                        {
                            var slimParameterType = methodSlim.ParameterTypes[i];
                            var typeParameterType = ConvertType(slimParameterType);

                            if (candidateParameters[i].ParameterType != typeParameterType)
                            {
                                mismatch = true;
                                break;
                            }
                        }

                        if (mismatch)
                        {
                            continue;
                        }
                    }
                    finally
                    {
                        //
                        // Finally, pop the generic argument context.
                        //
                        _typeConverter.Pop();
                    }
                }

                //
                // Ensure there's only one match.
                //
                if (method == null)
                {
                    method = candidate;
                }
                else
                {
                    failed = true;
                }
            }

            failed |= method == null;

            if (failed)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find generic definition method '{0}' on declaring type '{1}'.", methodSlim.Name, declaringType));

            return method;
        }

        private MethodInfo GetGenericMethod(GenericMethodInfoSlim methodSlim)
        {
            var methodDefinition = GetGenericDefinitionMethod(methodSlim.GenericMethodDefinition);
            var arguments = ConvertTypes(methodSlim.GenericArguments);
            return GetGenericMethodCore(methodSlim, methodDefinition, arguments);
        }

        /// <summary>
        /// Creates a generic method from the generic method definition.
        /// </summary>
        /// <param name="methodSlim">The slim method.</param>
        /// <param name="genericMethodDefinition">The generic method definition.</param>
        /// <param name="typeArguments">The generic argument types.</param>
        /// <returns>The generic method.</returns>
        protected virtual MethodInfo GetGenericMethodCore(GenericMethodInfoSlim methodSlim, MethodInfo genericMethodDefinition, params Type[] typeArguments)
        {
            return _provider.MakeGenericMethod(genericMethodDefinition, typeArguments);
        }

        /// <summary>
        /// Gets a property from a slim property.
        /// </summary>
        /// <param name="propertySlim">The slim property.</param>
        /// <returns>The property represented by the slim property.</returns>
        public PropertyInfo GetProperty(PropertyInfoSlim propertySlim)
        {
            if (propertySlim == null)
                throw new ArgumentNullException(nameof(propertySlim));

            if (_properties.TryGetValue(propertySlim, out PropertyInfo res))
            {
                return res;
            }

            var declaringType = ConvertType(propertySlim.DeclaringType);
            var propertyType = (propertySlim.PropertyType != null) ? ConvertType(propertySlim.PropertyType) : null;
            var indexParameterTypes = (propertySlim.IndexParameterTypes != null) ? ConvertTypes(propertySlim.IndexParameterTypes) : null;

            res = GetPropertyCore(propertySlim, declaringType, propertyType, indexParameterTypes);

            _properties[propertySlim] = res;
            return res;
        }

        /// <summary>
        /// Resolves a property on a CLR type.
        /// </summary>
        /// <param name="propertySlim">The slim property.</param>
        /// <param name="declaringType">The declaring CLR type.</param>
        /// <param name="propertyType">The property CLR type.</param>
        /// <param name="indexParameterTypes">The index parameter CLR types.</param>
        /// <returns>The property from the declaring type.</returns>
        protected virtual PropertyInfo GetPropertyCore(PropertyInfoSlim propertySlim, Type declaringType, Type propertyType, Type[] indexParameterTypes)
        {
            var failed = false;
            var property = default(PropertyInfo);

            var candidates = _provider.GetProperties(declaringType);

            foreach (var candidate in candidates)
            {
                //
                // Name should match.
                //
                if (candidate.Name != propertySlim.Name)
                {
                    continue;
                }

                //
                // The property type should match, if specified.
                //
                if (propertyType != null && candidate.PropertyType != propertyType)
                {
                    continue;
                }

                //
                // Indexer parameter types should match, if specified.
                //
                if (indexParameterTypes != null)
                {
                    var candidateIndexParameters = _provider.GetIndexParameters(candidate);

                    if (candidateIndexParameters.Count != indexParameterTypes.Length)
                    {
                        continue;
                    }

                    var n = indexParameterTypes.Length;

                    var mismatch = false;

                    for (var i = 0; i < n; i++)
                    {
                        if (candidateIndexParameters[i].ParameterType != indexParameterTypes[i])
                        {
                            mismatch = true;
                            break;
                        }
                    }

                    if (mismatch)
                    {
                        continue;
                    }
                }

                //
                // Ensure there's only one match.
                //
                if (property == null)
                {
                    property = candidate;
                }
                else
                {
                    failed = true;
                }
            }

            failed |= property == null;

            if (failed)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Property '{0}' from type '{1}' has no match on type '{2}'.", propertySlim.Name, propertySlim.DeclaringType.ToString(), declaringType.FullName));

            return property;
        }

        /// <summary>
        /// Gets a field from a slim field.
        /// </summary>
        /// <param name="fieldSlim">The slim field.</param>
        /// <returns>The field represented by the slim field.</returns>
        public FieldInfo GetField(FieldInfoSlim fieldSlim)
        {
            if (fieldSlim == null)
                throw new ArgumentNullException(nameof(fieldSlim));

            if (_fields.TryGetValue(fieldSlim, out FieldInfo res))
            {
                return res;
            }

            var declaringType = ConvertType(fieldSlim.DeclaringType);

            res = GetFieldCore(fieldSlim, declaringType);

            _fields[fieldSlim] = res;
            return res;
        }

        /// <summary>
        /// Resolves a field on a CLR type.
        /// </summary>
        /// <param name="fieldSlim">The slim field.</param>
        /// <param name="declaringType">The declaring CLR type.</param>
        /// <returns>The field from the declaring type.</returns>
        protected virtual FieldInfo GetFieldCore(FieldInfoSlim fieldSlim, Type declaringType)
        {
            var field = _provider.GetField(declaringType, fieldSlim.Name);
            if (field == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Field '{0}' from type '{1}' has no match on type '{2}'.", fieldSlim.Name, fieldSlim.DeclaringType.ToString(), declaringType.FullName));

            return field;
        }

        /// <summary>
        /// Converts the slim types to their CLR type representation.
        /// </summary>
        /// <param name="types">Slim type to obtain CLR type representations for.</param>
        /// <returns>The list of CLR types of the specified slim types.</returns>
        private Type[] ConvertTypes(IList<TypeSlim> types)
        {
            var n = types.Count;

            if (n == 0)
            {
                return s_emptyTypes;
            }

            var res = new Type[n];

            for (var i = 0; i < n; i++)
            {
                res[i] = ConvertType(types[i]);
            }

            return res;
        }

        #endregion
    }
}
