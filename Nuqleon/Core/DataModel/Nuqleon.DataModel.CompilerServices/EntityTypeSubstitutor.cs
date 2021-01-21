// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - June 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using Nuqleon.DataModel.TypeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices
{
    /// <summary>
    /// Replaces entity types with anonymous or record types.
    /// </summary>
    public abstract class EntityTypeSubstitutor : TypeSubstitutionExpressionVisitor
    {
        #region Fields & constructors

        private readonly EnumAwareTypeSubstitutor _subst;

        /// <summary>
        /// Create an entity type substitutor.
        /// </summary>
        /// <param name="typeMap">A map to use to replace types.</param>
        protected EntityTypeSubstitutor(IDictionary<Type, Type> typeMap)
            : base(typeMap)
        {
            TypeMap = typeMap ?? throw new ArgumentNullException(nameof(typeMap));
            ConstantsMap = new Dictionary<object, object>();
            _subst = new EnumAwareTypeSubstitutor(typeMap);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a reference to the type map so that mappings can be added.
        /// </summary>
        public IDictionary<Type, Type> TypeMap { get; }

        /// <summary>
        /// The parent of this type substitution visitor, which has an `Apply` method
        /// to first check for previously unencountered entity types, and add them to
        /// the type substitution dictionary.
        /// </summary>
        public ExpressionEntityTypeSubstitutor Parent { get; set; }

        /// <summary>
        /// Gets a reference to the constants map of converted constant values.
        /// </summary>
        public IDictionary<object, object> ConstantsMap { get; }

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
            res = new UnassignedExpressionReducer().Visit(res);
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

            if (TypeMap.TryGetValue(oldType, out var newType))
            {
                var newDataType = DataType.FromType(newType, allowCycles: true /* for full generality, we allow cycles here; users should protect against this higher up (cf. DataType.Check) */);
                return VisitNewStructuralDataType(node, oldType, (StructuralDataType)newDataType);
            }

            return base.VisitNew(node);
        }

        /// <summary>
        /// Visit new expressions for structural data types and transforms occurrences of entity types.
        /// </summary>
        /// <param name="node">The expression to transform.</param>
        /// <param name="oldType">The original type of the new expression.</param>
        /// <param name="newDataType">The new data type of the new expression.</param>
        /// <returns>The new expression with the original type replaced with the new type.</returns>
        protected virtual Expression VisitNewStructuralDataType(NewExpression node, Type oldType, StructuralDataType newDataType)
        {
            var newType = newDataType.UnderlyingType;

            var oldConstructor = node.Constructor;
            var oldParameters = oldConstructor.GetParameters();
            var count = oldParameters.Length;
            var oldArguments = node.Arguments;

            var memberAssignments = new Dictionary<MemberInfo, Expression>(count);

            for (var i = 0; i < count; i++)
            {
                var oldParameter = oldParameters[i];
                var oldArgument = oldArguments[i];

                var mapping = oldParameter.GetCustomAttribute<MappingAttribute>(inherit: false);
                Debug.Assert(mapping != null);

                var newProperty = newType.GetProperty(mapping.Uri);
                Debug.Assert(newProperty != null);

                if (memberAssignments.ContainsKey(newProperty))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Parameter '{0}' on the constructor of '{1}' has the same mapping attribute '{2}' of another parameter.", oldParameter.Name, oldType, mapping.Uri));
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
            var oldType = node.Type;

            if (TypeMap.TryGetValue(oldType, out var newType))
            {
                var newDataType = DataType.FromType(newType, allowCycles: true /* for full generality, we allow cycles here; users should protect against this higher up (cf. DataType.Check) */);
                return VisitMemberInitStructuralDataType(node, oldType, (StructuralDataType)newDataType);
            }

            return base.VisitMemberInit(node);
        }

        /// <summary>
        /// Visit member initializer expressions for structural data types and transforms occurrences of entity types.
        /// </summary>
        /// <param name="node">The expression to transform.</param>
        /// <param name="oldType">The original type of the member initializer expression.</param>
        /// <param name="newDataType">The new data type of the member initializer expression.</param>
        /// <returns>The member initializer expression with the original type replaced with the new type.</returns>
        protected virtual Expression VisitMemberInitStructuralDataType(MemberInitExpression node, Type oldType, StructuralDataType newDataType)
        {
            var newType = newDataType.UnderlyingType;

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

            return CreateNewExpressionFromBindings(newType, node.Bindings, memberAssignments);
        }

        private Expression CreateNewExpressionFromBindings(Type newType, IEnumerable<MemberBinding> bindings, IDictionary<MemberInfo, Expression> memberAssignments)
        {
            foreach (var binding in bindings)
            {
                var oldMember = binding.Member;
                var newMember = VisitMember(oldMember);

                if (memberAssignments.ContainsKey(newMember))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Member '{0}' on type '{1}' mapped as '{2}' has been initialized before through a constructor parameter. Only one assignment is allowed.", oldMember.Name, oldMember.DeclaringType, newMember.Name));
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

        private Expression GetMemberAssignment(MemberAssignment assignment)
        {
            return Visit(assignment.Expression);
        }

        private Expression GetMemberListBinding(MemberListBinding listBinding, MemberInfo newMember)
        {
            var memberType = Helpers.GetMemberType(newMember);

            var constructor = memberType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find a constructor for type '{0}' used in list binding '{1}'.", memberType.ToCSharpStringPretty(), listBinding));
            }

            var initializers = Visit(listBinding.Initializers, VisitElementInit);

            return Expression.ListInit(Expression.New(constructor), initializers);
        }

        private Expression GetMemberMemberBinding(MemberMemberBinding memberBinding)
        {
            var oldMemberType = Helpers.GetMemberType(memberBinding.Member);

            if (!TypeMap.TryGetValue(oldMemberType, out var newMemberType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected member type '{0}' in member binding '{1}'. Only entities can be initialized in member bindings.", newMemberType.ToCSharpStringPretty(), memberBinding));
            }

            return CreateNewExpressionFromBindings(newMemberType, memberBinding.Bindings, new Dictionary<MemberInfo, Expression>());
        }

        private static IDictionary<MemberInfo, Expression> GetMemberAssignments(NewExpression node)
        {
            var arguments = node.Arguments;
            var members = node.Members;

            var memberAssignments = new Dictionary<MemberInfo, Expression>();

            for (var i = 0; i < arguments.Count; i++)
            {
                var argument = arguments[i];
                var member = members[i];

                if (argument is not UnassignedExpression)
                {
                    memberAssignments[member] = argument;
                }
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

        #endregion

        #region Property resolution

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

            var mapping = originalProperty.GetCustomAttribute<MappingAttribute>(inherit: false);
            if (mapping != null)
            {
                var res = GetProperty(declaringType, propertyType, mapping.Uri, indexerParameters) ?? GetField(declaringType, propertyType, mapping.Uri);

                if (res != null)
                {
                    return res;
                }
            }

            return base.ResolveProperty(originalProperty, declaringType, propertyType, indexerParameters);
        }

        /// <summary>
        /// Resolves a field after retargeting types.
        /// </summary>
        /// <param name="originalField">Original field.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="fieldType">Retargeted field type.</param>
        /// <returns>New field to use.</returns>
        protected override MemberInfo ResolveField(FieldInfo originalField, Type declaringType, Type fieldType)
        {
            if (originalField == null)
                throw new ArgumentNullException(nameof(originalField));
            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));
            if (fieldType == null)
                throw new ArgumentNullException(nameof(fieldType));

            var mapping = originalField.GetCustomAttribute<MappingAttribute>(inherit: false);
            if (mapping != null)
            {
                var res = GetField(declaringType, fieldType, mapping.Uri) ?? GetProperty(declaringType, fieldType, mapping.Uri);

                if (res != null)
                {
                    return res;
                }
            }

            return base.ResolveField(originalField, declaringType, fieldType);
        }

        private static MemberInfo GetField(Type declaringType, Type fieldType, string name)
        {
            return declaringType.GetFields().SingleOrDefault(f => f.Name == name && f.FieldType == fieldType);
        }

        private static MemberInfo GetProperty(Type declaringType, Type propertyType, string name, params Type[] indexerParameters)
        {
            return declaringType.GetProperty(name, propertyType, indexerParameters);
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
        protected override object ConvertConstant(object originalValue, Type newType)
        {
            if (originalValue == null)
            {
                return null;
            }

            var newDataType = DataType.FromType(newType, allowCycles: true);
            var oldDataType = DataType.FromType(originalValue.GetType(), allowCycles: true);

            return ConvertConstant(originalValue, oldDataType, newDataType);
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
            var oldExpression = oldDataType.GetExpression(originalValue);
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

        #endregion

        #region Error reporting

        /// <summary>
        /// Reports failure of resolving a constructor after retargeting types, offering a last chance opportunity to resolve the constructor.
        /// </summary>
        /// <param name="originalConstructor">Original constructor.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="parameters">Retargeted parameter types.</param>
        /// <returns>New constructor to use. By default, this method throws an exception to report the resolution failure.</returns>
        /// <exception cref="InvalidOperationException">Thrown to report the resolution failure.</exception>
        protected override ConstructorInfo FailResolveConstructor(ConstructorInfo originalConstructor, Type declaringType, Type[] parameters)
        {
            var searchMismatches = new TypeMismatchSearcher();

            searchMismatches.Equals(originalConstructor.DeclaringType, declaringType);
            CheckParameters(searchMismatches, originalConstructor.GetParameters(), parameters);

            var ts = searchMismatches._results.Keys.Select(t => t.ToString()).ToArray();

            var err = string.Format(CultureInfo.InvariantCulture, "Usage of type(s) {0} in the signature of constructor '{1}' are not allowed during entity type erasure. Did you declare a constructor that may not be known by the target system and relies on entity types?", string.Join(", ", ts), originalConstructor);
            throw new InvalidOperationException(err);
        }

        /// <summary>
        /// Reports failure of resolving a field after retargeting types, offering a last chance opportunity to resolve the field.
        /// </summary>
        /// <param name="originalField">Original field.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="fieldType">Retargeted field type.</param>
        /// <returns>New field to use. By default, this method throws an exception to report the resolution failure.</returns>
        /// <exception cref="InvalidOperationException">Thrown to report the resolution failure.</exception>
        protected override MemberInfo FailResolveField(FieldInfo originalField, Type declaringType, Type fieldType)
        {
            var searchMismatches = new TypeMismatchSearcher();

            searchMismatches.Equals(originalField.DeclaringType, declaringType);
            searchMismatches.Equals(originalField.FieldType, fieldType);

            var ts = searchMismatches._results.Keys.Select(t => t.ToString()).ToArray();

            var err = string.Format(CultureInfo.InvariantCulture, "Usage of type(s) {0} in the declaration of field '{1}' are not allowed during entity type erasure. Did you declare a field that may not be known by the target system and relies on entity types?", string.Join(", ", ts), originalField);
            throw new InvalidOperationException(err);
        }

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
        protected override MethodInfo FailResolveMethod(MethodInfo originalMethod, Type declaringType, Type[] genericArguments, Type[] parameters, Type returnType)
        {
            var searchMismatches = new TypeMismatchSearcher();

            searchMismatches.Equals(originalMethod.DeclaringType, declaringType);
            searchMismatches.Equals(originalMethod.ReturnType, returnType);
            CheckParameters(searchMismatches, originalMethod.GetParameters(), parameters);

            var ts = searchMismatches._results.Keys.Select(t => t.ToString()).ToArray();

            var err = string.Format(CultureInfo.InvariantCulture, "Usage of type(s) {0} in the signature of method '{1}' are not allowed during entity type erasure. Did you declare a method that may not be known by the target system and relies on entity types?", string.Join(", ", ts), originalMethod);
            throw new InvalidOperationException(err);
        }

        /// <summary>
        /// Reports failure of resolving a property after retargeting types, offering a last chance opportunity to resolve the property.
        /// </summary>
        /// <param name="originalProperty">Original property.</param>
        /// <param name="declaringType">Retargeted declaring type.</param>
        /// <param name="propertyType">Retargeted property type.</param>
        /// <param name="indexerParameters">Retargeted indexer parameter types.</param>
        /// <returns>New property to use. By default, this method throws an exception to report the resolution failure.</returns>
        /// <exception cref="InvalidOperationException">Thrown to report the resolution failure.</exception>
        protected override MemberInfo FailResolveProperty(PropertyInfo originalProperty, Type declaringType, Type propertyType, Type[] indexerParameters)
        {
            var searchMismatches = new TypeMismatchSearcher();

            searchMismatches.Equals(originalProperty.DeclaringType, declaringType);
            searchMismatches.Equals(originalProperty.PropertyType, propertyType);

            if (indexerParameters != null)
            {
                CheckParameters(searchMismatches, originalProperty.GetIndexParameters(), indexerParameters);
            }

            var ts = searchMismatches._results.Keys.Select(t => t.ToString()).ToArray();

            var err = string.Format(CultureInfo.InvariantCulture, "Usage of type(s) {0} in the declaration of property '{1}' are not allowed during entity type erasure. Did you declare a property that may not be known by the target system and relies on entity types?", string.Join(", ", ts), originalProperty);
            throw new InvalidOperationException(err);
        }

        private static void CheckParameters(TypeMismatchSearcher searcher, IEnumerable<ParameterInfo> ps, Type[] ts)
        {
            foreach (var _ in ps.Select(p => p.ParameterType).Zip(ts, searcher.Equals))
            {
                //
                // NB: Evaluated for the side-effect of adding mismatches to the TypeMismatchSearcher instance.
                //
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Resolves a type for rewriting of the expression tree.
        /// </summary>
        /// <param name="originalType">Original type.</param>
        /// <returns>New type to use.</returns>
        protected override Type ResolveType(Type originalType) => _subst.Rewrite(originalType);

        #region Helpers

        private sealed class TypeMismatchSearcher : TypeEqualityComparer
        {
            public readonly Dictionary<Type, Type> _results = new();

            public override bool Equals(Type x, Type y)
            {
                if (!base.Equals(x, y))
                {
                    _results[x] = y;
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Sentinel expression type which can be used to indicate absence of assignment of a member in a structural type.
        /// </summary>
        protected class UnassignedExpression : Expression
        {
            /// <summary>
            /// Creates a new sentinel expression with the specified underlying type.
            /// </summary>
            /// <param name="type">Underlying type of the expression.</param>
            public UnassignedExpression(Type type)
            {
                Type = type;
            }

            /// <summary>
            /// Returns Extension.
            /// </summary>
            public override ExpressionType NodeType => ExpressionType.Extension;

            /// <summary>
            /// Gets the underlying type.
            /// </summary>
            public override Type Type { get; }

            /// <summary>
            /// Always returns true.
            /// </summary>
            public override bool CanReduce => true;

            /// <summary>
            /// Reduces to default expressions.
            /// </summary>
            /// <returns>DefaultExpression instance of the same underlying type as the sentinel.</returns>
            public override Expression Reduce() => Expression.Default(Type);
        }

        private sealed class UnassignedExpressionReducer : ExpressionVisitor
        {
            //
            // No implementation needed - will have the side-effect of calling Reduce on our nodes.
            //
        }

        #endregion
    }
}
