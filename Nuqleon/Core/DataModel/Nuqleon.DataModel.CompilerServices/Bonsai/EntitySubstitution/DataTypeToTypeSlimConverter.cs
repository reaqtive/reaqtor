// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using Nuqleon.DataModel.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    /// <summary>
    /// Data type visitor to create slim type representations from data types.
    /// </summary>
    internal abstract class DataTypeToTypeSlimConverter : DataTypeVisitor<TypeSlim, PropertyDataSlim>
    {
        private readonly Dictionary<StructuralDataType, StructuralTypeSlimReference> _structuralTypes;

        /// <summary>
        /// Initializes the visitor.
        /// </summary>
        protected DataTypeToTypeSlimConverter() => _structuralTypes = new Dictionary<StructuralDataType, StructuralTypeSlimReference>();

        /// <summary>
        /// Makes an array type representation.
        /// </summary>
        /// <param name="type">Original array data type.</param>
        /// <param name="elementType">Element type of the array.</param>
        /// <returns>Representation of an array type.</returns>
        /// <remarks>
        /// Currently, the data model only supports either arrays or lists. If
        /// the data model changes in terms of the array types supported, this
        /// method must be revisited to support the extended data model.
        /// </remarks>
        protected override TypeSlim MakeArray(ArrayDataType type, TypeSlim elementType)
        {
            if (type.UnderlyingType.IsArray)
            {
                return TypeSlim.Array(elementType);
            }
            else
            {
                Debug.Assert(type.UnderlyingType.IsGenericType);

                return TypeSlim.Generic(
                    (GenericDefinitionTypeSlim)type.UnderlyingType.GetGenericTypeDefinition().ToTypeSlim(),
                    elementType);
            }
        }

        /// <summary>
        /// Visits an expression data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected override TypeSlim VisitExpression(ExpressionDataType type) => type.UnderlyingType.ToTypeSlim();

        /// <summary>
        /// Makes an function type representation.
        /// </summary>
        /// <param name="type">Original function data type.</param>
        /// <param name="parameterTypes">Parameter types of the function.</param>
        /// <param name="returnType">Return type of the function.</param>
        /// <returns>Representation of a function type.</returns>
        protected override TypeSlim MakeFunction(FunctionDataType type, ReadOnlyCollection<TypeSlim> parameterTypes, TypeSlim returnType)
        {
            var count = parameterTypes.Count;

            var genericArguments = new TypeSlim[count + 1];
            for (var i = 0; i < count; ++i)
            {
                genericArguments[i] = parameterTypes[i];
            }

            genericArguments[count] = returnType;

            return TypeSlim.Generic(
                (GenericDefinitionTypeSlim)type.UnderlyingType.GetGenericTypeDefinition().ToTypeSlim(),
                Helpers.AsReadOnly(genericArguments));
        }

        /// <summary>
        /// Visits an open generic parameter data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected override TypeSlim VisitOpenGenericParameter(OpenGenericParameterDataType type) => type.UnderlyingType.ToTypeSlim();

        /// <summary>
        /// Visits a primitive data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected override TypeSlim VisitPrimitive(PrimitiveDataType type)
        {
            var underlyingType = type.UnderlyingType;

            if (DataType.IsEntityEnumDataType(underlyingType))
            {
                underlyingType = Helpers.GetUnderlyingEnumType(underlyingType);
            }

            return underlyingType.ToTypeSlim();
        }

        /// <summary>
        /// Makes a quotation type representation.
        /// </summary>
        /// <param name="type">Original quotation data type.</param>
        /// <param name="functionType">Type of the function.</param>
        /// <returns>Representation of a quotation type.</returns>
        protected override TypeSlim MakeQuotation(QuotationDataType type, TypeSlim functionType)
        {
            return TypeSlim.Generic(
                (GenericDefinitionTypeSlim)type.UnderlyingType.GetGenericTypeDefinition().ToTypeSlim(),
                functionType);
        }

        /// <summary>
        /// Visits a structural type representation.
        /// </summary>
        /// <param name="type">Original structural data type.</param>
        /// <returns>Representation of a structural type.</returns>
        protected override TypeSlim VisitStructural(StructuralDataType type)
        {
            if (type.UnderlyingType.IsDefined(typeof(KnownTypeAttribute)))
            {
                return type.UnderlyingType.ToTypeSlim();
            }

            if (_structuralTypes.TryGetValue(type, out var slimType))
            {
                return slimType;
            }

            try
            {
                _structuralTypes.Add(type, GetTypeSlimBuilder());
                return base.VisitStructural(type);
            }
            finally
            {
                _structuralTypes.Remove(type);
            }
        }

        /// <summary>
        /// Makes a structural type representation.
        /// </summary>
        /// <param name="type">Original structural data type.</param>
        /// <param name="properties">Properties of the structural type.</param>
        /// <returns>Representation of a structural type.</returns>
        protected override TypeSlim MakeStructural(StructuralDataType type, ReadOnlyCollection<PropertyDataSlim> properties)
        {
            return type.StructuralKind switch
            {
                StructuralDataTypeKinds.Anonymous or
                StructuralDataTypeKinds.Record or
                StructuralDataTypeKinds.Entity => MakeStructuralEntity(type, properties),

                StructuralDataTypeKinds.Tuple => MakeStructuralTuple(type, properties),

                _ => throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Unexpected structural data type kind '{0}'.", type.StructuralKind)),
            };
        }

        private TypeSlim MakeStructuralEntity(StructuralDataType type, ReadOnlyCollection<PropertyDataSlim> properties)
        {
            var builder = _structuralTypes[type];

            foreach (var property in properties)
            {
                AddProperty(builder, property);
            }

            return builder;
        }

        private static TypeSlim MakeStructuralTuple(StructuralDataType type, ReadOnlyCollection<PropertyDataSlim> properties)
        {
            var count = properties.Count;

            var genericArguments = new TypeSlim[count];
            for (var i = 0; i < count; ++i)
            {
                genericArguments[i] = properties[i].Type;
            }

            return TypeSlim.Generic(
                (GenericDefinitionTypeSlim)type.UnderlyingType.GetGenericTypeDefinition().ToTypeSlim(),
                Helpers.AsReadOnly(genericArguments));
        }

        /// <summary>
        /// Visits a custom data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected override TypeSlim VisitCustom(DataType type)
        {
            throw new NotImplementedException("This method should be implemented by derived classes.");
        }

        /// <summary>
        /// Makes a property representation.
        /// </summary>
        /// <param name="property">Original property.</param>
        /// <param name="propertyType">Property type.</param>
        /// <returns>Representation of a property.</returns>
        protected override PropertyDataSlim MakeProperty(DataProperty property, TypeSlim propertyType)
        {
            return new PropertyDataSlim(property.Name, propertyType);
        }

        /// <summary>
        /// Gets a structural type slim reference to use in building a structural type slim.
        /// </summary>
        /// <returns>The structural type slim reference.</returns>
        protected abstract StructuralTypeSlimReference GetTypeSlimBuilder();

        /// <summary>
        /// Adds a property to a structural type slim reference.
        /// </summary>
        /// <param name="builder">The structural type.</param>
        /// <param name="property">The property.</param>
        protected abstract void AddProperty(StructuralTypeSlimReference builder, PropertyDataSlim property);
    }
}
