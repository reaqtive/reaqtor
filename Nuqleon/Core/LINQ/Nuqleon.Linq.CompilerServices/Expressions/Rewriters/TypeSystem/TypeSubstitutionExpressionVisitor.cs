// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - November 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.
#pragma warning disable IDE0049 // Name can be simplified - the analyzer doesn't know we use Object instead of object to enabling aliasing

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

#if USE_SLIM
using System.Collections.ObjectModel;
#else
using System.Linq.CompilerServices.Reflection;
#endif

#if USE_SLIM
namespace System.Linq.CompilerServices.Bonsai
#else
namespace System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using ConstructorInfo = ConstructorInfoSlim;
    using Expression = ExpressionSlim;
    using ExpressionVisitorWithReflection = ExpressionSlimVisitorWithReflection;
    using FieldInfo = FieldInfoSlim;
    using MemberInfo = MemberInfoSlim;
    using MethodInfo = MethodInfoSlim;
    using Object = ObjectSlim;
    using PropertyInfo = PropertyInfoSlim;
    using Type = TypeSlim;
    using TypeSubstitutor = TypeSlimSubstitutor;

    #endregion
#endif

    /// <summary>
    /// Visitor to retarget types that occur in an expression tree. Subclasses can customize reflection member resolution.
    /// </summary>
#if USE_SLIM
    public class TypeSubstitutionExpressionSlimVisitor : ExpressionVisitorWithReflection
#else
    public class TypeSubstitutionExpressionVisitor : ExpressionVisitorWithReflection
#endif
    {
        #region Constructor & fields

        private readonly TypeSubstitutor _subst;

        /// <summary>
        /// Creates a new type substitution expression visitor with the specified type substitutor.
        /// </summary>
        /// <param name="typeSubstitutor">Type substitutor to map source types onto target types.</param>
#if USE_SLIM
        public TypeSubstitutionExpressionSlimVisitor(TypeSubstitutor typeSubstitutor)
#else
        public TypeSubstitutionExpressionVisitor(TypeSubstitutor typeSubstitutor)
#endif
        {
            _subst = typeSubstitutor ?? throw new ArgumentNullException(nameof(typeSubstitutor));
        }

        /// <summary>
        /// Creates a new type substitution expression visitor with the specified type map.
        /// </summary>
        /// <param name="typeMap">Dictionary to map source types onto target types.</param>
#if USE_SLIM
        public TypeSubstitutionExpressionSlimVisitor(IDictionary<Type, Type> typeMap)
#else
        public TypeSubstitutionExpressionVisitor(IDictionary<Type, Type> typeMap)
#endif
        {
            if (typeMap == null)
                throw new ArgumentNullException(nameof(typeMap));

            _subst = new TypeSubstitutor(typeMap);
        }

        #endregion

        #region Apply

        /// <summary>
        /// Applies the type substitution to the specified expression.
        /// </summary>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Expression with nodes with source types replaced by nodes with the corresponding target types.</returns>
        public virtual Expression Apply(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return Visit(expression);
        }

        #endregion

        #region Visit

        /// <summary>
        /// Visits a member. When overridden, other visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="member">Member to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected override MemberInfo VisitMember(MemberInfo member)
        {
            var memberType = member.MemberType;

            Debug.Assert(memberType is MemberTypes.Field or MemberTypes.Property);
            var newMember = member; // see Debug.Assert

            switch (memberType)
            {
                case MemberTypes.Property:
                    newMember = VisitProperty((PropertyInfo)member);
                    break;
                case MemberTypes.Field:
                    newMember = VisitField((FieldInfo)member);
                    break;
            }

            return newMember;
        }

        /// <summary>
        /// Visits a constructor. When overridden, other visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="constructor">Constructor to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected override ConstructorInfo VisitConstructor(ConstructorInfo constructor)
        {
            var newConstructor = constructor;

            if (constructor != null)
            {
                var hasChanged = false;

                var oldDeclaringType = constructor.DeclaringType;
                var newDeclaringType = VisitType(oldDeclaringType);
                hasChanged |= oldDeclaringType != newDeclaringType;

#if USE_SLIM
                var oldParameterTypes = constructor.ParameterTypes;
                var newParameterTypes = Visit(oldParameterTypes, VisitType);
                hasChanged |= oldParameterTypes != newParameterTypes;
#else
                hasChanged |= TryChangeParameterTypes(constructor.GetParameters(), hasChanged, out Type[] newParameterTypes);
#endif

                if (hasChanged)
                {
                    newConstructor = ResolveConstructor(constructor, newDeclaringType, newParameterTypes.AsArray());
                }
            }

            return newConstructor;
        }

        /// <summary>
        /// Visits a property. When overridden, other visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="property">Property to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected override MemberInfo VisitProperty(PropertyInfo property)
        {
            var newMember = (MemberInfo)property;

            if (property != null)
            {
                var hasChanged = false;

                var oldDeclaringType = property.DeclaringType;
                var newDeclaringType = VisitType(oldDeclaringType);
                hasChanged |= oldDeclaringType != newDeclaringType;

                var oldPropertyType = property.PropertyType;
                var newPropertyType = VisitType(oldPropertyType);
                hasChanged |= oldPropertyType != newPropertyType;

#if USE_SLIM
                var oldIndexParameterTypes = property.IndexParameterTypes;
                var newIndexParameterTypes = Visit(oldIndexParameterTypes, VisitType);
                hasChanged |= oldIndexParameterTypes != newIndexParameterTypes;
#else
                hasChanged |= TryChangeParameterTypes(property.GetIndexParameters(), hasChanged, out Type[] newIndexParameterTypes);
#endif

                if (hasChanged)
                {
                    newMember = ResolveProperty(property, newDeclaringType, newPropertyType, newIndexParameterTypes.AsArray());
                }
            }

            return newMember;
        }

        /// <summary>
        /// Visits a field. When overridden, other visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="field">Field to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfo VisitField(FieldInfo field)
        {
            var newMember = (MemberInfo)field;

            if (field != null)
            {
                var hasChanged = false;

                var oldDeclaringType = field.DeclaringType;
                var newDeclaringType = VisitType(oldDeclaringType);
                hasChanged |= oldDeclaringType != newDeclaringType;

                var oldFieldType = field.FieldType;
                var newFieldType = VisitType(oldFieldType);
                hasChanged |= oldFieldType != newFieldType;

                if (hasChanged)
                {
                    newMember = ResolveField(field, newDeclaringType, newFieldType);
                }
            }

            return newMember;
        }

        /// <summary>
        /// Visits a method. When overridden, other visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="method">Method to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected override MethodInfo VisitMethod(MethodInfo method)
        {
            var newMethod = method;

            if (method != null)
            {
                var hasChanged = false;

                var oldDeclaringType = method.DeclaringType;
                var newDeclaringType = VisitType(oldDeclaringType);
                hasChanged |= oldDeclaringType != newDeclaringType;

                var oldReturnType = method.ReturnType;
                var newReturnType = VisitType(oldReturnType);
                hasChanged |= oldReturnType != newReturnType;

#if USE_SLIM
                var oldGenericArguments = default(ReadOnlyCollection<Type>);
                var newGenericArguments = oldGenericArguments;

                switch (method.Kind)
                {
                    case MethodInfoSlimKind.Simple:
                        break;
                    case MethodInfoSlimKind.GenericDefinition:
                        oldGenericArguments = ((GenericDefinitionMethodInfoSlim)method).GenericParameterTypes;
                        newGenericArguments = Visit(oldGenericArguments, VisitType);
                        break;
                    case MethodInfoSlimKind.Generic:
                        oldGenericArguments = ((GenericMethodInfoSlim)method).GenericArguments;
                        newGenericArguments = Visit(oldGenericArguments, VisitType);
                        break;
                    default:
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected method type for '{0}'.", method));
                }
#else
                var oldGenericArguments = default(Type[]);
                var newGenericArguments = oldGenericArguments;

                if (method.IsGenericMethod)
                {
                    oldGenericArguments = method.GetGenericArguments();
                    newGenericArguments = Visit(oldGenericArguments, VisitType);
                }
#endif

                hasChanged |= oldGenericArguments != newGenericArguments;

#if USE_SLIM
                var oldParameterTypes = method.ParameterTypes;
                var newParameterTypes = Visit(oldParameterTypes, VisitType);
                hasChanged |= oldParameterTypes != newParameterTypes;
#else
                hasChanged |= TryChangeParameterTypes(method.GetParameters(), hasChanged, out Type[] newParameterTypes);
#endif

                if (hasChanged)
                {
                    newMethod = ResolveMethod(method, newDeclaringType, newGenericArguments.AsArray(), newParameterTypes.AsArray(), newReturnType);
                }
            }

            return newMethod;
        }

#if !USE_SLIM
        /// <summary>
        /// Visits a dynamic expression in order to rewrite the type while retaining the binder.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected override Expression VisitDynamic(DynamicExpression node)
        {
            //
            // NB: See remarks in the Visit method for .NET Core compatibility.
            //

            var type = VisitType(node.DelegateType);
            var args = Visit(node.Arguments);

            if (type == node.DelegateType && args == node.Arguments)
            {
                return node;
            }

            return Expression.MakeDynamic(type, node.Binder, args);
        }
#endif

        /// <summary>
        /// Visits a type. When overridden, other visit methods should be overridden appropriately to ensure consistent typing.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected override Type VisitType(Type type) => ResolveType(type);

        #endregion

        #region ConvertConstant

        /// <summary>
        /// Converts an expression whose type has changed.
        /// </summary>
        /// <param name="originalExpression">Original expression.</param>
        /// <param name="resultExpression">Rewritten expression which does not conform to the expected type.</param>
        /// <param name="newType">Resolved target type for the expression.</param>
        /// <returns>Expression with the same value as the rewritten expression, but using the specified resolved target type.</returns>
        protected virtual Expression ConvertExpression(Expression originalExpression, Expression resultExpression, Type newType)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Result of rewriting '{0}' into '{1}' has a type '{2}' that requires substitution.", originalExpression, resultExpression, newType));
        }

        /// <summary>
        /// Converts a constant value whose constant expression node's type has changed.
        /// </summary>
        /// <param name="originalValue">Original value of the constant expression.</param>
        /// <param name="newType">Resolved target type for the constant.</param>
        /// <returns>Object representing the same value as the original value, but using the specified resolved target type.</returns>
        protected virtual Object ConvertConstant(Object originalValue, Type newType)
        {
#if USE_SLIM
            if (originalValue.CanReduce)
            {
                return originalValue.Update(originalValue.Value, newType);
            }
            else
            {
                return Object.Create(originalValue.Value, newType, originalValue.OriginalType);
            }
#else
            if (originalValue == null && (!newType.IsValueType || newType.IsNullableType()))
            {
                return null;
            }

            if (originalValue != null && newType.IsAssignableFrom(originalValue.GetType()))
            {
                return originalValue;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable conversion of constant '{0}' to type '{1}' found.", originalValue, newType));
#endif
        }

        #endregion

        #region Resolve

        /// <summary>
        /// Resolves a constructor after retargeting types.
        /// </summary>
        /// <param name="originalConstructor">Original constructor.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="parameters">Retargeted parameter types.</param>
        /// <returns>New constructor to use.</returns>
        protected virtual ConstructorInfo ResolveConstructor(ConstructorInfo originalConstructor, Type declaringType, Type[] parameters)
        {
            if (originalConstructor == null)
                throw new ArgumentNullException(nameof(originalConstructor));
            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

#if USE_SLIM
            var newConstructor = new ConstructorInfo(declaringType, parameters.ToReadOnly());
#else
            var flags = GetFlags(originalConstructor);

            var newConstructor = declaringType.GetConstructor(flags, null, parameters, null);
            if (newConstructor == null)
            {
                return FailResolveConstructor(originalConstructor, declaringType, parameters);
            }
#endif

            return newConstructor;
        }

#if !USE_SLIM
        /// <summary>
        /// Reports failure of resolving a constructor after retargeting types, offering a last chance opportunity to resolve the constructor.
        /// </summary>
        /// <param name="originalConstructor">Original constructor.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="parameters">Retargeted parameter types.</param>
        /// <returns>New constructor to use. By default, this method throws an exception to report the resolution failure.</returns>
        /// <exception cref="InvalidOperationException">Thrown to report the resolution failure.</exception>
        protected virtual ConstructorInfo FailResolveConstructor(ConstructorInfo originalConstructor, Type declaringType, Type[] parameters)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable constructor on declaring type '{0}' with parameter types ({1}) found.", declaringType, string.Join(", ", parameters.Select(p => p.ToString()))));
        }
#endif

        /// <summary>
        /// Resolves a property after retargeting types.
        /// </summary>
        /// <param name="originalProperty">Original property.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="propertyType">Retargeted property type.</param>
        /// <param name="indexerParameters">Retargeted indexer parameter types.</param>
        /// <returns>New property to use.</returns>
        protected virtual MemberInfo ResolveProperty(PropertyInfo originalProperty, Type declaringType, Type propertyType, Type[] indexerParameters)
        {
            if (originalProperty == null)
                throw new ArgumentNullException(nameof(originalProperty));
            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));
            if (indexerParameters == null)
                throw new ArgumentNullException(nameof(indexerParameters));

#if USE_SLIM
            var newProperty = declaringType.GetProperty(originalProperty.Name, propertyType, indexerParameters.ToReadOnly(), originalProperty.CanWrite);
#else
            var flags = GetFlags(originalProperty.IsPublic(), originalProperty.IsStatic());

            var newProperty = declaringType.GetProperties(flags).SingleOrDefault(p => p.Name == originalProperty.Name && p.PropertyType == propertyType && p.GetIndexParameters().Select(q => q.ParameterType).SequenceEqual(indexerParameters));
            if (newProperty == null)
            {
                return FailResolveProperty(originalProperty, declaringType, propertyType, indexerParameters);
            }
#endif

            return newProperty;
        }

#if !USE_SLIM
        /// <summary>
        /// Reports failure of resolving a property after retargeting types, offering a last chance opportunity to resolve the property.
        /// </summary>
        /// <param name="originalProperty">Original property.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="propertyType">Retargeted property type.</param>
        /// <param name="indexerParameters">Retargeted indexer parameter types.</param>
        /// <returns>New property to use. By default, this method throws an exception to report the resolution failure.</returns>
        /// <exception cref="InvalidOperationException">Thrown to report the resolution failure.</exception>
        protected virtual MemberInfo FailResolveProperty(PropertyInfo originalProperty, Type declaringType, Type propertyType, Type[] indexerParameters)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable property '{0}' of type '{1}' on declaring type '{2}' with parameter types ({3}) found.", originalProperty.Name, propertyType, declaringType, string.Join(", ", indexerParameters.Select(p => p.ToString()))));
        }
#endif

        /// <summary>
        /// Resolves a field after retargeting types.
        /// </summary>
        /// <param name="originalField">Original field.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="fieldType">Retargeted field type.</param>
        /// <returns>New field to use.</returns>
        protected virtual MemberInfo ResolveField(FieldInfo originalField, Type declaringType, Type fieldType)
        {
            if (originalField == null)
                throw new ArgumentNullException(nameof(originalField));
            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));
            if (fieldType == null)
                throw new ArgumentNullException(nameof(fieldType));

#if USE_SLIM
            var newField = new FieldInfo(declaringType, originalField.Name, fieldType);
#else
            var flags = GetFlags(originalField.IsPublic, originalField.IsStatic);

            var newField = declaringType.GetFields(flags).SingleOrDefault(f => f.Name == originalField.Name && f.FieldType == fieldType);
            if (newField == null)
            {
                return FailResolveField(originalField, declaringType, fieldType);
            }
#endif

            return newField;
        }

#if !USE_SLIM
        /// <summary>
        /// Reports failure of resolving a field after retargeting types, offering a last chance opportunity to resolve the field.
        /// </summary>
        /// <param name="originalField">Original field.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="fieldType">Retargeted field type.</param>
        /// <returns>New field to use. By default, this method throws an exception to report the resolution failure.</returns>
        /// <exception cref="InvalidOperationException">Thrown to report the resolution failure.</exception>
        protected virtual MemberInfo FailResolveField(FieldInfo originalField, Type declaringType, Type fieldType)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable field '{0}' of type '{1}' on declaring type '{2}' found.", originalField.Name, fieldType, declaringType));
        }
#endif

        /// <summary>
        /// Resolves a method after retargeting types.
        /// </summary>
        /// <param name="originalMethod">Original method.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="genericArguments">Retargeted generic argument types.</param>
        /// <param name="parameters">Retargeted parameter types.</param>
        /// <param name="returnType">Retargeted return type.</param>
        /// <returns>New method to use.</returns>
        protected virtual MethodInfo ResolveMethod(MethodInfo originalMethod, Type declaringType, Type[] genericArguments, Type[] parameters, Type returnType)
        {
            if (originalMethod == null)
                throw new ArgumentNullException(nameof(originalMethod));
            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));
            if (genericArguments == null)
                throw new ArgumentNullException(nameof(genericArguments));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));

#if USE_SLIM
            var newMethod = default(MethodInfo);

            switch (originalMethod.Kind)
            {
                case MethodInfoSlimKind.Simple:
                    var simpleMethod = (SimpleMethodInfoSlim)originalMethod;
                    newMethod = declaringType.GetSimpleMethod(simpleMethod.Name, parameters.ToReadOnly(), returnType);
                    break;
                case MethodInfoSlimKind.GenericDefinition:
                    var genericDefinitionMethod = (GenericDefinitionMethodInfoSlim)originalMethod;
                    newMethod = declaringType.GetGenericDefinitionMethod(genericDefinitionMethod.Name, genericDefinitionMethod.GenericParameterTypes, parameters.ToReadOnly(), returnType);
                    break;
                case MethodInfoSlimKind.Generic:
                    var genericMethod = (GenericMethodInfoSlim)originalMethod;
                    newMethod = declaringType.GetGenericMethod(genericMethod.GenericMethodDefinition, genericArguments.ToReadOnly());
                    break;
                default:
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected method type '{0}'.", originalMethod.Kind));
            }
#else
            var flags = GetFlags(originalMethod);

            var candidates = declaringType.GetMethods(flags).Where(m => m.Name == originalMethod.Name);

            if (originalMethod.IsGenericMethod)
            {
                candidates = candidates.Where(m => m.IsGenericMethod && m.GetGenericArguments().Length == genericArguments.Length).Select(m => m.MakeGenericMethod(genericArguments));
            }

            candidates = candidates.Where(m => m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameters) && m.ReturnType == returnType);

            var newMethod = candidates.SingleOrDefault();
            if (newMethod == null)
            {
                return FailResolveMethod(originalMethod, declaringType, genericArguments, parameters, returnType);
            }
#endif

            return newMethod;
        }

#if !USE_SLIM
        /// <summary>
        /// Reports failure of resolving a method after retargeting types, offering a last chance opportunity to resolve the method.
        /// </summary>
        /// <param name="originalMethod">Original method.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="genericArguments">Retargeted generic argument types.</param>
        /// <param name="parameters">Retargeted parameter types.</param>
        /// <param name="returnType">Retargeted return type.</param>
        /// <returns>New method to use. By default, this method throws an exception to report the resolution failure.</returns>
        /// <exception cref="InvalidOperationException">Thrown to report the resolution failure.</exception>
        protected virtual MethodInfo FailResolveMethod(MethodInfo originalMethod, Type declaringType, Type[] genericArguments, Type[] parameters, Type returnType)
        {
            var gen = originalMethod.IsGenericMethod ? string.Format(CultureInfo.InvariantCulture, "generic argument types <{0}>, ", string.Join(", ", genericArguments.Select(p => p.ToString()))) : "";
            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable method named '{0}' on declaring type '{1}' with {4}parameter types ({2}), and return type '{3}' found.", originalMethod.Name, declaringType, string.Join(", ", parameters.Select(p => p.ToString())), returnType, gen));
        }
#endif

        /// <summary>
        /// Resolves a type for rewriting of the expression tree.
        /// </summary>
        /// <param name="originalType">Original type.</param>
        /// <returns>New type to use.</returns>
        protected virtual Type ResolveType(Type originalType) => _subst.Visit(originalType);

        /// <summary>
        /// Visits the expression to rewrite types.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Expression with types rewritten.</returns>
        public override Expression Visit(Expression node)
        {
#if !USE_SLIM
            //
            // NB: In .NET Core, DynamicExpression is an extension node, causing reduction upon calling Visit. We don't want this reduction
            //     to take place, because it introduces an opaque Constant node containing a call site object for which we can't substitute
            //     types. As such, we need to type check here and dispatch ourselves.
            //

            if (node is DynamicExpression d)
            {
                return VisitDynamic(d);
            }
#endif

            var res = base.Visit(node);

            if (res != null)
            {
#if USE_SLIM
                var derivationVisitor = new TypeSlimDerivationVisitor();
                var resType = derivationVisitor.Visit(res);
                var newType = resType != null ? VisitType(resType) : null;
#else
                var newType = VisitType(res.Type);
                var resType = res.Type;
#endif

                if (newType != resType)
                {
                    res = ConvertExpression(node, res, newType);
                }
            }

            return res;
        }

        /// <summary>
        /// Creates a constant expression.
        /// </summary>
        /// <param name="value">Value of the constant expression.</param>
        /// <param name="type">Type of the constant expression.</param>
        /// <returns>New constant expression node.</returns>
        protected override Expression MakeConstant(Object value, Type type)
        {
            var newValue = ConvertConstant(value, type);
            return base.MakeConstant(newValue, type);
        }

        #endregion

        #region Private implementation

#if !USE_SLIM
        private static BindingFlags GetFlags(MethodBase method) => GetFlags(method.IsPublic, method.IsStatic);

        private static BindingFlags GetFlags(bool isPublic, bool isStatic)
        {
            var flags = default(BindingFlags);
            flags |= isPublic ? BindingFlags.Public : BindingFlags.NonPublic;
            flags |= isStatic ? BindingFlags.Static : BindingFlags.Instance;
            return flags;
        }

        /// <summary>
        /// Tries to change the types of the specified parameter collection using the VisitType method.
        /// A resulting types array gets allocated if and only if any of the parameter types changes, or when the <paramref name="force"/> parameter is set.
        /// </summary>
        /// <param name="parameters">Array of parameters whose types to change.</param>
        /// <param name="force">Indicates whether the <paramref name="types"/> parameter needs to be populated with parameter types regardless of whether changes are found.</param>
        /// <param name="types">Array containing the types of the parameters after applying the VisitType method.</param>
        /// <returns>true if an array with parameter types is returned in <paramref name="types"/>; otherwise, false.</returns>
        private bool TryChangeParameterTypes(ParameterInfo[] parameters, bool force, out Type[] types)
        {
            types = null;

            var n = parameters.Length;

            if (force)
            {
                types = new Type[n];
            }

            for (var i = 0; i < n; i++)
            {
                var parameter = parameters[i];

                var oldType = parameter.ParameterType;
                var newType = VisitType(oldType);

                if (types != null)
                {
                    types[i] = newType;
                }
                else
                {
                    if (oldType != newType)
                    {
                        types = new Type[n];

                        for (var j = 0; j < i; j++)
                        {
                            types[j] = parameters[j].ParameterType;
                        }

                        types[i] = newType;
                    }
                }
            }

            return types != null;
        }

        /// <summary>
        /// Visits the objects in the array using the specified visitor delegate.
        /// If each element's visit returns the original element, the original array is returned.
        /// </summary>
        /// <typeparam name="T">Type of the objects in the array.</typeparam>
        /// <param name="objects">Array with objects to visit.</param>
        /// <param name="visit">Visitor delegate to apply to each element in the array.</param>
        /// <returns>The original array if no elements changed due to the visit; otherwise, a new array containing the results of the calls to the visitor delegate for each element.</returns>
        private static T[] Visit<T>(T[] objects, Func<T, T> visit)
        {
            if (objects != null)
            {
                var res = default(T[]);

                for (int i = 0; i < objects.Length; i++)
                {
                    var m = objects[i];
                    var n = visit(m);

                    if (res != null)
                    {
                        res[i] = n;
                    }
                    else
                    {
                        if (!object.Equals(m, n))
                        {
                            res = new T[objects.Length];
                            Array.Copy(objects, res, i);
                            res[i] = n;
                        }
                    }
                }

                if (res != null)
                {
                    return res;
                }
            }

            return objects;
        }
#endif

        #endregion
    }
}
