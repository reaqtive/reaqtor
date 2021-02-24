// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - October 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel.TypeSystem;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    #region Aliases

    using Expression = ExpressionSlim;
    using MemberInitExpression = MemberInitExpressionSlim;
    using NewExpression = NewExpressionSlim;

    using MemberAssignment = MemberAssignmentSlim;
    using MemberBinding = MemberBindingSlim;
    using MemberListBinding = MemberListBindingSlim;
    using MemberMemberBinding = MemberMemberBindingSlim;

    using Type = TypeSlim;
    using FieldInfo = FieldInfoSlim;
    using PropertyInfo = PropertyInfoSlim;
    using MemberInfo = MemberInfoSlim;

    using TypeSubstitutionExpressionVisitor = System.Linq.CompilerServices.Bonsai.TypeSubstitutionExpressionSlimVisitor;

    using Object = ObjectSlim;

    #endregion

    /// <summary>
    /// Replaces entity types with anonymous or record types.
    /// </summary>
    public abstract class EntityTypeSlimSubstitutor : TypeSubstitutionExpressionVisitor
    {
        #region Fields & constructors

        private readonly EnumAwareTypeSlimSubstitutor _subst;
#if DEBUG
        private readonly HashSet<object> _checked;
#endif

        /// <summary>
        /// Create an entity type substitutor.
        /// </summary>
        /// <param name="typeMap">A map to use to replace types.</param>
        protected EntityTypeSlimSubstitutor(IDictionary<Type, Type> typeMap)
            : base(typeMap)
        {
            TypeMap = typeMap ?? throw new ArgumentNullException(nameof(typeMap));
            ConstantsMap = new Dictionary<object, object>();
            _subst = new EnumAwareTypeSlimSubstitutor(typeMap);
#if DEBUG
            _checked = new HashSet<object>();
#endif
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a reference to the type map so that mappings can be added.
        /// </summary>
        public IDictionary<Type, Type> TypeMap { get; private set; }

        /// <summary>
        /// The parent of this type substitution visitor, which has an `Apply` method
        /// to first check for previously unencountered entity types, and add them to
        /// the type substitution dictionary.
        /// </summary>
        public ExpressionSlimEntityTypeSubstitutor Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a reference to the constants map of converted constant values.
        /// </summary>
        public IDictionary<object, object> ConstantsMap { get; private set; }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Transforms an expression by replacing entity types.
        /// </summary>
        /// <param name="expression">The expression to transform.</param>
        /// <returns>An expression with entity types replaced.</returns>
        public override Expression Apply(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = base.Apply(expression);
            return res;
        }

        #endregion

        #region Expression rewriting

        /// <summary>
        /// Visits new expressions and transforms occurrences of entity types.
        /// </summary>
        /// <param name="node">The expression to transform.</param>
        /// <returns>The new expression with entity types replaced.</returns>
        protected override Expression VisitNew(NewExpression node)
        {
            var oldType = node.Type;
            if (oldType != null)
            {
                if (TypeMap.TryGetValue(oldType, out var newType))
                {
                    if (newType is StructuralTypeSlim newStructuralType)
                    {
                        return VisitNewStructuralDataType(node, newStructuralType);
                    }
                }
            }

            return base.VisitNew(node);
        }

        /// <summary>
        /// Visit new expressions for structural data types and transforms occurrences of entity types.
        /// </summary>
        /// <param name="node">The expression to transform.</param>
        /// <param name="newType">The original type of the new expression.</param>
        /// <returns>The new expression with the original type replaced with the new type.</returns>
        protected virtual Expression VisitNewStructuralDataType(NewExpression node, StructuralTypeSlim newType)
        {
            if (node.Constructor is not DataModelConstructorInfoSlim oldConstructor || oldConstructor.ParameterMappings.Count != node.ArgumentCount)
            {
                throw new InvalidOperationException("Expected a constructor info instance with mapping attributes associated with constructor parameters.");
            }

            var n = oldConstructor.ParameterMappings.Count;
            var memberAssignments = new Dictionary<MemberInfo, Expression>(n);

            for (var i = 0; i < n; ++i)
            {
                var oldParameter = oldConstructor.ParameterMappings[i];
                var oldArgument = node.GetArgument(i);

                var newProperty = newType.Properties.Single(p => p.Name == oldParameter); // TODO: remove allocation

                if (memberAssignments.ContainsKey(newProperty))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Parameter mapping attribute '{0}' on the constructor of '{1}' is used more than once.", oldParameter, newType));
                }

                var newArgument = Visit(oldArgument);
                memberAssignments[newProperty] = newArgument;
            }

            return CreateNewExpression(newType, memberAssignments);
        }

        /// <summary>
        /// Creates a new expression to instantiate the given type using the specified member assignments.
        /// </summary>
        /// <param name="type">Type to instantiate.</param>
        /// <param name="memberAssignments">Member assignments to carry out.</param>
        /// <returns>New expression for the instantiation of the given type using the specified member assignments.</returns>
        protected abstract Expression CreateNewExpression(Type type, IDictionary<MemberInfo, Expression> memberAssignments);

        /// <summary>
        /// Visits member initializer expressions and transforms occurrences of entity types.
        /// </summary>
        /// <param name="node">The expression to transform.</param>
        /// <returns>The new expression with entity types replaced.</returns>
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var oldType = node.NewExpression.Type;
            if (oldType != null)
            {
                if (TypeMap.TryGetValue(oldType, out var newType))
                {
                    if (newType is StructuralTypeSlim newStructuralType)
                    {
                        return VisitMemberInitStructuralDataType(node, newStructuralType);
                    }

                    throw new InvalidOperationException("Only structural types should be rewritten in this visitor.");
                }
            }

            return base.VisitMemberInit(node);
        }

        /// <summary>
        /// Visit member initializer expressions for structural data types and transforms occurrences of entity types.
        /// </summary>
        /// <param name="node">The expression to transform.</param>
        /// <param name="newStructuralType">The new structural type.</param>
        /// <returns>The member initializer expression with the original type replaced with the new type.</returns>
        protected virtual Expression VisitMemberInitStructuralDataType(MemberInitExpression node, StructuralTypeSlim newStructuralType)
        {
            var memberAssignments = default(IDictionary<MemberInfo, Expression>);

            var newExpression = Visit(node.NewExpression);

            switch (newExpression.NodeType)
            {
                case ExpressionType.MemberInit:
                    memberAssignments = GetMemberAssignments((MemberInitExpression)newExpression);
                    break;
                case ExpressionType.New:
                    memberAssignments = GetMemberAssignments((NewExpression)newExpression);
                    break;
            }

            return CreateNewExpressionFromBindings(newStructuralType, node.Bindings, memberAssignments);
        }

        private Expression CreateNewExpressionFromBindings(Type newType, IEnumerable<MemberBinding> bindings, IDictionary<MemberInfo, Expression> memberAssignments)
        {
            foreach (var binding in bindings)
            {
                var oldMember = binding.Member;
                var newMember = VisitMember(oldMember);

                if (memberAssignments.ContainsKey(newMember))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Mapped member '{0}' on type '{1}' has been initialized before through a constructor parameter. Only one assignment is allowed.", Helpers.GetMemberName(oldMember), oldMember.DeclaringType));
                }

                switch (binding.BindingType)
                {
                    case MemberBindingType.Assignment:
                        var assignment = GetMemberAssignment((MemberAssignment)binding);
                        memberAssignments[newMember] = assignment;
                        break;
                    case MemberBindingType.ListBinding:
                        var listBinding = GetMemberListBinding((MemberListBinding)binding, newMember);
                        memberAssignments[newMember] = listBinding;
                        break;
                    case MemberBindingType.MemberBinding:
                        var memberBinding = GetMemberMemberBinding((MemberMemberBinding)binding);
                        memberAssignments[newMember] = memberBinding;
                        break;
                }
            }

            return CreateNewExpression(newType, memberAssignments);
        }

        private Expression GetMemberAssignment(MemberAssignment assignment) => Visit(assignment.Expression);

        private Expression GetMemberListBinding(MemberListBinding listBinding, MemberInfo newMember)
        {
            var memberType = Helpers.GetMemberType(newMember);

            var constructor = memberType.GetConstructor(EmptyReadOnlyCollection<Type>.Instance);

            var initializers = Visit(listBinding.Initializers, VisitElementInit);

            return Expression.ListInit(Expression.New(constructor), initializers);
        }

        private Expression GetMemberMemberBinding(MemberMemberBinding memberBinding)
        {
            var oldMemberType = Helpers.GetMemberType(memberBinding.Member);

            if (!TypeMap.TryGetValue(oldMemberType, out var newMemberType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected member type '{0}' in member binding '{1}'. Only entities can be initialized in member bindings.", newMemberType, memberBinding));
            }

            return CreateNewExpressionFromBindings(newMemberType, memberBinding.Bindings, new Dictionary<MemberInfo, Expression>());
        }

        private static IDictionary<MemberInfo, Expression> GetMemberAssignments(NewExpression node)
        {
            var members = node.Members;

            var n = node.ArgumentCount;
            var memberAssignments = new Dictionary<MemberInfo, Expression>(n);

            for (var i = 0; i < n; i++)
            {
                var argument = node.GetArgument(i);
                var member = members[i];
                memberAssignments[member] = argument;
            }

            return memberAssignments;
        }

        private static IDictionary<MemberInfo, Expression> GetMemberAssignments(MemberInitExpression node)
        {
            var memberAssignments = GetMemberAssignments(node.NewExpression);

            foreach (var mb in node.Bindings)
            {
                memberAssignments[mb.Member] = ((MemberAssignment)mb).Expression; // TODO: Check if we need to support other types of bindings.
            }

            return memberAssignments;
        }

        /// <summary>
        /// Resolves a type for rewriting of the expression tree.
        /// </summary>
        /// <param name="originalType">Original type.</param>
        /// <returns>New type to use.</returns>
        protected override Type ResolveType(Type originalType) => _subst.Rewrite(originalType);

        #endregion

        #region Property resolution

        /// <summary>
        /// Resolves a field after retargeting types.
        /// </summary>
        /// <param name="originalField">Original field.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="fieldType">Retargeted field type.</param>
        /// <returns>New member to use.</returns>
        protected override MemberInfo ResolveField(FieldInfo originalField, Type declaringType, Type fieldType)
        {
            Debug.Assert(originalField != null);
            Debug.Assert(declaringType != null);
            Debug.Assert(fieldType != null);

            if (declaringType is StructuralTypeSlim structuralDeclaringType)
            {
                var candidate = default(PropertyInfo);

                foreach (var p in structuralDeclaringType.Properties)
                {
                    if (p.Name == originalField.Name && Equals(p.PropertyType, fieldType)) // NB: TypeSlim implements IEquatable
                    {
                        AssignSingle(ref candidate, p);
                    }
                }

                if (candidate == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not resolve field '{0}' on structural type '{1}'.", originalField.Name, declaringType));
                }

                return candidate;
            }

            return base.ResolveField(originalField, declaringType, fieldType);
        }

        /// <summary>
        /// Resolves a property after retargeting types.
        /// </summary>
        /// <param name="originalProperty">Original property.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="propertyType">Retargeted property type.</param>
        /// <param name="indexerParameters">Retargeted indexer parameter types.</param>
        /// <returns>New property to use.</returns>
        protected override MemberInfo ResolveProperty(PropertyInfo originalProperty, Type declaringType, Type propertyType, Type[] indexerParameters)
        {
            if (originalProperty == null)
                throw new ArgumentNullException(nameof(originalProperty));
            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            if (declaringType is StructuralTypeSlim structuralDeclaringType)
            {
                var candidate = default(PropertyInfo);

                foreach (var p in structuralDeclaringType.Properties)
                {
                    if (p.Name == originalProperty.Name &&
                        Equals(p.PropertyType, propertyType) &&  // NB: TypeSlim implements IEquatable
                        p.IndexParameterTypes.SequenceEqual(indexerParameters, TypeSlimEqualityComparer.Default))
                    {
                        AssignSingle(ref candidate, p);
                    }
                }

                if (candidate == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not resolve property '{0}' on structural type '{1}'.", originalProperty.Name, declaringType));
                }

                return candidate;
            }

            return base.ResolveProperty(originalProperty, declaringType, propertyType, indexerParameters);
        }

        #endregion

        #region Constant conversion

        // TODO: Reduce duplication between EntityTypeSubstitutor and EntityTypeSlimSubstitutor by extracting this functionality
        //       into a constant converter while retaining the protected virtuals here.

        /// <summary>
        /// Converts a constant of a rewritten type into a new type.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="newType">New type to convert to.</param>
        /// <returns>Converted constant.</returns>
        protected override Object ConvertConstant(Object originalValue, Type newType)
        {
            if (originalValue == null)
            {
                return null;
            }

            var newValueObj = default(object);

            var oldType = originalValue.OriginalType;
            var oldDataType = DataType.FromType(oldType, allowCycles: true);

            if (TryGetCarriedType(newType, out var newTypeCarried))
            {
                var newDataType = DataType.FromType(newTypeCarried, allowCycles: true);
                newValueObj = ConvertConstant(originalValue.Value, oldDataType, newDataType);
            }
            else
            {
                newValueObj = originalValue.Value;
            }

            var newValue = Object.Create(newValueObj, newType, oldType);

#if DEBUG
            CheckConstant(newValue, oldDataType);
#endif

            return newValue;
        }

        /// <summary>
        /// Converts a constant from one data type to another.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <param name="newDataType">New data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstant(object originalValue, DataType oldDataType, DataType newDataType)
        {
            if (originalValue == null)
            {
                return null;
            }

            if (newDataType.Kind != oldDataType.Kind)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Constant conversion is unsupported for mismatched type kinds '{0}' and '{1}'.", oldDataType.Kind, newDataType.Kind));
            }


            if (!ConstantsMap.TryGetValue(originalValue, out var newValue))
            {
                newValue = oldDataType.Kind switch
                {
                    DataTypeKinds.Array => ConvertConstantArray(originalValue, (ArrayDataType)oldDataType, (ArrayDataType)newDataType),
                    DataTypeKinds.Custom => ConvertConstantCustom(originalValue, oldDataType, newDataType),
                    DataTypeKinds.Function => ConvertConstantFunction(originalValue, (FunctionDataType)oldDataType, (FunctionDataType)newDataType),
                    DataTypeKinds.Primitive => ConvertConstantPrimitive(originalValue, (PrimitiveDataType)oldDataType, (PrimitiveDataType)newDataType),
                    DataTypeKinds.Quotation => ConvertConstantQuotation(originalValue, (QuotationDataType)oldDataType, (QuotationDataType)newDataType),
                    DataTypeKinds.Structural => ConvertConstantStructural(originalValue, (StructuralDataType)oldDataType, (StructuralDataType)newDataType),
                    _ => throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Unknown data type kind '{0}' encountered.", oldDataType.Kind)),
                };
                ConstantsMap[originalValue] = newValue;
            }

            return newValue;
        }

        /// <summary>
        /// Converts a constant from one array data type to another.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <param name="newDataType">New data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstantArray(object originalValue, ArrayDataType oldDataType, ArrayDataType newDataType)
        {
            var oldArray = oldDataType.GetList(originalValue);
            var newArray = (IList)newDataType.CreateInstance(oldArray.Count);

            ConstantsMap[originalValue] = newArray; // Allow cycles

            for (var i = 0; i < oldArray.Count; i++)
            {
                var oldElement = oldArray[i];
                var newElement = ConvertConstant(oldElement, oldDataType.ElementType, newDataType.ElementType);

                if (newArray.IsFixedSize)
                {
                    newArray[i] = newElement;
                }
                else
                {
                    newArray.Add(newElement);
                }
            }

            return newArray;
        }

        /// <summary>
        /// Converts a constant from one custom data type to another.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <param name="newDataType">New data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstantCustom(object originalValue, DataType oldDataType, DataType newDataType)
        {
            throw new NotImplementedException("Support for conversion of constants of custom data types should be implemented by derived classes.");
        }

        /// <summary>
        /// Converts a constant from one function data type to another.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <param name="newDataType">New data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstantFunction(object originalValue, FunctionDataType oldDataType, FunctionDataType newDataType)
        {
            if (oldDataType.UnderlyingType == newDataType.UnderlyingType)
            {
                return originalValue;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot convert value '{0}' from function data type '{1}' to '{2}'.", originalValue, oldDataType, newDataType));
        }

        /// <summary>
        /// Converts a constant from one primitive data type to another.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <param name="newDataType">New data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstantPrimitive(object originalValue, PrimitiveDataType oldDataType, PrimitiveDataType newDataType)
        {
            var oldUnderlyingType = oldDataType.UnderlyingType;
            var newUnderlyingType = newDataType.UnderlyingType;

            if (oldUnderlyingType == newUnderlyingType)
            {
                return originalValue;
            }

            if (oldUnderlyingType.IsEnum)
            {
                var enumType = oldUnderlyingType;

                var underlyingEnumType = Enum.GetUnderlyingType(enumType);
                if (newUnderlyingType == underlyingEnumType)
                {
                    var enumValue = Convert.ChangeType(originalValue, newUnderlyingType, CultureInfo.InvariantCulture);
                    return enumValue;
                }
                else if (newUnderlyingType.IsNullableType())
                {
                    var nonNullNewUnderlyingType = newUnderlyingType.GetNonNullType();

                    if (nonNullNewUnderlyingType == underlyingEnumType)
                    {
                        var enumValue = Convert.ChangeType(originalValue, nonNullNewUnderlyingType, CultureInfo.InvariantCulture);
                        return enumValue;
                    }
                }
#if ENUM_AS_STRING
                else if (newUnderlyingType == typeof(string))
                {
                    //
                    // TODO: Flexibility to map to string. This can get tricky with non-nullable uses of
                    //       the enum during type substitution and when dealing with [Flags].
                    //
                    var name = Enum.GetName(enumType, originalValue);
                    var field = enumType.GetField(name);
                    var mapping = field.GetCustomAttribute<MappingAttribute>();
                    return mapping.Uri;
                }
#endif
            }
            else if (oldUnderlyingType.IsNullableType())
            {
                var nonNullOldUnderlyingType = oldUnderlyingType.GetNonNullType();

                if (nonNullOldUnderlyingType.IsEnum)
                {
                    var enumType = nonNullOldUnderlyingType;

                    var underlyingEnumType = Enum.GetUnderlyingType(enumType);

                    if (newUnderlyingType.IsNullableType())
                    {
                        var nonNullNewUnderlyingType = newUnderlyingType.GetNonNullType();

                        if (nonNullNewUnderlyingType == underlyingEnumType)
                        {
                            if (originalValue == null)
                            {
                                return null;
                            }

                            var enumValue = Convert.ChangeType(originalValue, nonNullNewUnderlyingType, CultureInfo.InvariantCulture);
                            return enumValue;
                        }
                    }
#if ENUM_AS_STRING
                    else if (newUnderlyingType == typeof(string))
                    {
                        if (originalValue == null)
                        {
                            return null;
                        }

                        //
                        // TODO: Flexibility to map to string. This can get tricky with non-nullable uses of
                        //       the enum during type substitution and when dealing with [Flags].
                        //
                        var name = Enum.GetName(enumType, originalValue);
                        var field = enumType.GetField(name);
                        var mapping = field.GetCustomAttribute<MappingAttribute>();
                        return mapping.Uri;
                    }
#endif
                }
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot convert value '{0}' from primitive data type '{1}' to '{2}'.", originalValue, oldDataType, newDataType));
        }

        /// <summary>
        /// Converts a constant from one quotation data type to another.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <param name="newDataType">New data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstantQuotation(object originalValue, QuotationDataType oldDataType, QuotationDataType newDataType)
        {
            var oldExpression = originalValue as LambdaExpressionSlim ?? oldDataType.GetExpression(originalValue).ToExpressionSlim();
            var newExpression = Parent.Apply(oldExpression);

            return newDataType.CreateInstance(newExpression);
        }

        /// <summary>
        /// Converts a constant from one structural data type to another data type.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <param name="newDataType">New data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstantStructural(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType)
        {
            if (oldDataType.StructuralKind == newDataType.StructuralKind)
            {
                switch (oldDataType.StructuralKind)
                {
                    case StructuralDataTypeKinds.Anonymous:
                        return ConvertConstantStructuralAnonymous(originalValue, oldDataType, newDataType);
                    case StructuralDataTypeKinds.Tuple:
                        return ConvertConstantStructuralTuple(originalValue, oldDataType, newDataType);
                    case StructuralDataTypeKinds.Record:
                        return ConvertConstantStructuralRecord(originalValue, oldDataType, newDataType);
                }
            }

            return ConvertConstantStructuralCore(originalValue, oldDataType, newDataType);
        }

        /// <summary>
        /// Converts a constant from one anonymous data type to another anonymous data type.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old anonymous data type.</param>
        /// <param name="newDataType">New anonymous data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstantStructuralAnonymous(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType)
        {
            if (oldDataType.Properties.Count != newDataType.Properties.Count)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of properties on '{0}' and '{1}' does not match.", oldDataType, newDataType));
            }

            return ConvertConstantStructuralByPropertyOrderInstantiation(originalValue, oldDataType, newDataType);
        }

        /// <summary>
        /// Converts a constant from one tuple data type to another tuple data type.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old tuple data type.</param>
        /// <param name="newDataType">New tuple data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstantStructuralTuple(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType)
        {
            if (oldDataType.Properties.Count != newDataType.Properties.Count)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of tuple components on '{0}' and '{1}' does not match.", oldDataType, newDataType));
            }

            return ConvertConstantStructuralByPropertyOrderInstantiation(originalValue, oldDataType, newDataType);
        }

        /// <summary>
        /// Converts a constant by instantiating the new data type based on the converted values of the original data type's properties, in declaration order.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <param name="newDataType">New data type.</param>
        /// <returns>Converted constant.</returns>
        /// <remarks>Anonymous types and tuple data types obey to the prerequisite of property ordering according to the constructor parameter list.</remarks>
        protected object ConvertConstantStructuralByPropertyOrderInstantiation(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType)
        {
            var oldProperties = oldDataType.Properties;
            var newProperties = newDataType.Properties;

            var n = newProperties.Count;
            var args = new object[n];

            for (var i = 0; i < n; i++)
            {
                var oldProperty = oldProperties[i];
                var newProperty = newProperties[i];

                var oldValue = oldProperty.GetValue(originalValue);
                var newValue = ConvertConstant(oldValue, oldProperty.Type, newProperty.Type);

                args[i] = newValue;
            }

            var result = newDataType.CreateInstance(args);
            return result;
        }

        /// <summary>
        /// Converts a constant from one record data type to another record data type.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old record data type.</param>
        /// <param name="newDataType">New record data type.</param>
        /// <returns>Converted constant.</returns>
        protected virtual object ConvertConstantStructuralRecord(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType)
        {
            if (oldDataType.Properties.Count != newDataType.Properties.Count)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of record entries on '{0}' and '{1}' does not match.", oldDataType, newDataType));
            }

            var oldProperties = oldDataType.Properties;
            var newProperties = newDataType.Properties;

            var result = newDataType.CreateInstance();

            ConstantsMap[originalValue] = result; // Allow cycles

            var n = newProperties.Count;

            for (var i = 0; i < n; i++)
            {
                var oldProperty = oldProperties[i];
                var newProperty = newProperties[i];

                var oldValue = oldProperty.GetValue(originalValue);
                var newValue = ConvertConstant(oldValue, oldProperty.Type, newProperty.Type);

                newProperty.SetValue(result, newValue);
            }

            return result;
        }

        /// <summary>
        /// Converts a constant from one structural data type to another data type. This method is called when the structural type kind needs to change.
        /// </summary>
        /// <param name="originalValue">Original value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <param name="newDataType">New data type.</param>
        /// <returns>Converted constant.</returns>
        protected abstract object ConvertConstantStructuralCore(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType);

#if DEBUG
        /// <summary>
        /// Converts a constant from one data type to another.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstant(Object newValue, DataType oldDataType)
        {
            if (newValue.Value == null)
            {
                return;
            }

            if (!_checked.Add(newValue.Value))
            {
                return;
            }

            switch (oldDataType.Kind)
            {
                case DataTypeKinds.Array:
                    CheckConstantArray(newValue, (ArrayDataType)oldDataType);
                    break;
                case DataTypeKinds.Custom:
                    CheckConstantCustom(newValue, oldDataType);
                    break;
                case DataTypeKinds.Function:
                    CheckConstantFunction(newValue, (FunctionDataType)oldDataType);
                    break;
                case DataTypeKinds.Primitive:
                    CheckConstantPrimitive(newValue, (PrimitiveDataType)oldDataType);
                    break;
                case DataTypeKinds.Quotation:
                    CheckConstantQuotation(newValue, (QuotationDataType)oldDataType);
                    break;
                case DataTypeKinds.Structural:
                    CheckConstantStructural(newValue, (StructuralDataType)oldDataType);
                    break;
                default:
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Unknown data type kind '{0}' encountered.", oldDataType.Kind));
            }
        }

        /// <summary>
        /// Checks if a constant can be converted from one array data type to another.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstantArray(Object newValue, ArrayDataType oldDataType)
        {
            var oldArray = oldDataType.GetList(newValue.Value);

            if (!Helpers.TryGetElementType(newValue.TypeSlim, out var newElementType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot convert constant of type '{0}' to type '{1}'.", oldDataType.UnderlyingType.ToCSharpStringPretty(), newValue.TypeSlim));
            }

            for (var i = 0; i < oldArray.Count; i++)
            {
                var oldElement = Object.Create(oldArray[i], newElementType, oldDataType.UnderlyingType);
                CheckConstant(oldElement, oldDataType.ElementType);
            }
        }

        /// <summary>
        /// Checks if a constant can be converted from one custom data type to another.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstantCustom(Object newValue, DataType oldDataType)
        {
            throw new NotImplementedException("Support for conversion of constants of custom data types should be implemented by derived classes.");
        }

        /// <summary>
        /// Checks if a constant can be converted from one function data type to another.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstantFunction(Object newValue, FunctionDataType oldDataType)
        {
            if (!Equals(newValue.TypeSlim, oldDataType.UnderlyingType.ToTypeSlim())) // NB: TypeSlim implements IEquatable
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot convert value '{0}' from function data type '{1}' to '{2}'.", newValue.Value, oldDataType, newValue.TypeSlim));
            }
        }

        /// <summary>
        /// Checks if a constant can be converted from one primitive data type to another.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstantPrimitive(Object newValue, PrimitiveDataType oldDataType)
        {
            if (oldDataType.UnderlyingType.IsEnum)
            {
                var enumType = oldDataType.UnderlyingType;
                var underlyingEnumType = Enum.GetUnderlyingType(enumType);

                if (!Equals(newValue.TypeSlim, underlyingEnumType.ToTypeSlim())) // NB: TypeSlim implements IEquatable
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot convert value '{0}' from primitive data type '{1}' to '{2}'.", newValue.Value, oldDataType, newValue.TypeSlim));
                }
            }
        }

        /// <summary>
        /// Checks if a constant can be converted from one quotation data type to another.
        /// </summary>
        /// <param name="newValue">Converted value.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstantQuotation(Object newValue, QuotationDataType oldDataType)
        {
        }

        /// <summary>
        /// Checks if a constant can be converted from one structural data type to another data type.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstantStructural(Object newValue, StructuralDataType oldDataType)
        {
            switch (oldDataType.StructuralKind)
            {
                case StructuralDataTypeKinds.Anonymous:
                    CheckConstantStructuralAnonymous(newValue, oldDataType);
                    return;
                case StructuralDataTypeKinds.Tuple:
                    CheckConstantStructuralTuple(newValue, oldDataType);
                    return;
                case StructuralDataTypeKinds.Record:
                    CheckConstantStructuralRecord(newValue, oldDataType);
                    return;
            }

            CheckConstantStructuralCore(newValue, oldDataType);
        }

        /// <summary>
        /// Checks if a constant can be converted from one anonymous data type to another anonymous data type.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstantStructuralAnonymous(Object newValue, StructuralDataType oldDataType)
        {
            var newStructuralType = newValue.TypeSlim as StructuralTypeSlim;
            if (newStructuralType == null || oldDataType.Properties.Count != newStructuralType.Properties.Count)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of properties on '{0}' and '{1}' does not match.", oldDataType, newStructuralType));
            }

            CheckConstantStructuralByPropertyOrderInstantiation(newValue, oldDataType);
        }

        /// <summary>
        /// Checks if a constant can be converted from one tuple data type to another tuple data type.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstantStructuralTuple(Object newValue, StructuralDataType oldDataType)
        {
            var newTupleType = newValue.TypeSlim as GenericTypeSlim;
            if (newTupleType == null || oldDataType.Properties.Count != newTupleType.GenericArgumentCount)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of tuple components on '{0}' and '{1}' does not match.", oldDataType, newTupleType));
            }

            CheckConstantStructuralByPropertyOrderInstantiation(newValue, oldDataType);
        }

        /// <summary>
        /// Checks if a constant can be converted based on the converted values of the original data type's properties, in declaration order.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        /// <remarks>Anonymous types and tuple data types obey to the prerequisite of property ordering according to the constructor parameter list.</remarks>
        protected void CheckConstantStructuralByPropertyOrderInstantiation(Object newValue, StructuralDataType oldDataType)
        {
            var oldProperties = oldDataType.Properties;
            if (!Helpers.TryGetStructuralPropertyTypes(newValue.TypeSlim, out var newPropertyTypes))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot convert object of type '{0}' to type '{1}'.", oldDataType.UnderlyingType, newValue.TypeSlim));
            }

            var n = newPropertyTypes.Count;

            for (var i = 0; i < n; i++)
            {
                var oldProperty = oldProperties[i];
                var newPropertyType = newPropertyTypes[i];

                var oldValue = oldProperty.GetValue(newValue.Value);
                var newPropertyValue = Object.Create(oldValue, newPropertyType, oldProperty.Type.UnderlyingType);
                CheckConstant(newPropertyValue, oldProperty.Type);
            }
        }

        /// <summary>
        /// Checks if a constant can be converted from one record data type to another record data type.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected virtual void CheckConstantStructuralRecord(Object newValue, StructuralDataType oldDataType)
        {
            var newStructuralType = newValue.TypeSlim as StructuralTypeSlim;
            if (newStructuralType == null || oldDataType.Properties.Count != newStructuralType.Properties.Count)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of record entries on '{0}' and '{1}' does not match.", oldDataType, newValue.TypeSlim));
            }

            var oldProperties = oldDataType.Properties;
            if (!Helpers.TryGetStructuralPropertyTypes(newValue.TypeSlim, out var newPropertyTypes))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot convert object of type '{0}' to type '{1}'.", oldDataType.UnderlyingType, newValue.TypeSlim));
            }

            var n = newPropertyTypes.Count;

            for (var i = 0; i < n; i++)
            {
                var oldProperty = oldProperties[i];
                var newPropertyType = newPropertyTypes[i];

                var oldValue = oldProperty.GetValue(newValue.Value);
                var newPropertyValue = Object.Create(oldValue, newPropertyType, oldProperty.Type.UnderlyingType);
                CheckConstant(newPropertyValue, oldProperty.Type);
            }
        }

        /// <summary>
        /// Checks if a constant can be converted from one structural data type to another data type. This method is called when the structural type kind needs to change.
        /// </summary>
        /// <param name="newValue">Value to convert.</param>
        /// <param name="oldDataType">Old data type.</param>
        protected abstract void CheckConstantStructuralCore(Object newValue, StructuralDataType oldDataType);

#endif

        #endregion

        #region Helpers

        private static void AssignSingle<T>(ref T target, T value)
            where T : class
        {
            if (target != null)
                throw new InvalidOperationException("The target has already been assigned.");

            target = value;
        }

        private static bool TryGetCarriedType(Type type, out System.Type carriedType)
        {
            carriedType = TypeSlimToCarriedType.Instance.Visit(type);
            return carriedType != null;
        }

        private sealed class TypeSlimToCarriedType : TypeSlimVisitor<System.Type, System.Type, System.Type, System.Type, System.Type, System.Type, System.Type>
        {
            public static readonly TypeSlimToCarriedType Instance = new();

            private TypeSlimToCarriedType()
            {
            }

            public override System.Type Visit(Type type)
            {
                var res = GetCarriedType(type);
                if (res != null)
                {
                    return res;
                }

                return base.Visit(type);
            }

            protected override System.Type MakeArrayType(ArrayTypeSlim type, System.Type elementType, int? rank)
            {
                var res = default(System.Type);

                if (elementType != null)
                {
                    if (rank == null)
                    {
                        res = elementType.MakeArrayType();
                    }
                    else
                    {
                        res = elementType.MakeArrayType(rank.Value);
                    }
                }

                return res;
            }

            protected override System.Type MakeGeneric(GenericTypeSlim type, System.Type typeDefinition, ReadOnlyCollection<System.Type> arguments)
            {
                var res = default(System.Type);

                if (typeDefinition != null && arguments.All(a => a != null))
                {
                    res = typeDefinition.MakeGenericType(arguments.ToArray());
                }

                return res;
            }

            protected override System.Type MakeGenericDefinition(GenericDefinitionTypeSlim type) => GetCarriedType(type);

            protected override System.Type MakeStructuralType(StructuralTypeSlim type, IEnumerable<KeyValuePair<PropertyInfo, System.Type>> propertyTypes, IEnumerable<KeyValuePair<PropertyInfo, ReadOnlyCollection<System.Type>>> propertyIndexParameters)
            {
                // NB: If no carried type is found, we can't create a new structural type here in a cheap manner.
                return null;
            }

            protected override System.Type VisitGenericParameter(GenericParameterTypeSlim type) => GetCarriedType(type);

            protected override System.Type VisitSimple(SimpleTypeSlim type) => GetCarriedType(type);

            private static System.Type GetCarriedType(Type type)
            {
                if (type.TryGetCarriedType(out var res))
                {
                    return res;
                }

                return null;
            }
        }

        #endregion

        #endregion
    }
}
