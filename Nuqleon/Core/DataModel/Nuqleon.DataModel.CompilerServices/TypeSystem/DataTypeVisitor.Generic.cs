// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.
#pragma warning disable CA1716 // Reserved language keyword 'property'.

using System;
using System.Collections.ObjectModel;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Visitor of the structure of a data model type.
    /// </summary>
    /// <typeparam name="TType">Type of the result of visiting a data type.</typeparam>
    /// <typeparam name="TProperty">Type of the result of visiting a property.</typeparam>
    public abstract class DataTypeVisitor<TType, TProperty>
    {
        // WARNING: Known subtypes of this type implement IClearable. Adding state here is a breaking change and requires implementing IClearable with
        //          a virtual Clear method for subtypes to override.

        /// <summary>
        /// Visits a data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        public virtual TType Visit(DataType type)
        {
            if (type == null)
            {
                return default;
            }

            return type.Kind switch
            {
                DataTypeKinds.Array => VisitArray((ArrayDataType)type),
                DataTypeKinds.Expression => VisitExpression((ExpressionDataType)type),
                DataTypeKinds.Function => VisitFunction((FunctionDataType)type),
                DataTypeKinds.Primitive => VisitPrimitive((PrimitiveDataType)type),
                DataTypeKinds.Quotation => VisitQuotation((QuotationDataType)type),
                DataTypeKinds.Structural => VisitStructural((StructuralDataType)type),
                DataTypeKinds.OpenGenericParameter => VisitOpenGenericParameter((OpenGenericParameterDataType)type),
                DataTypeKinds.Custom => VisitCustom(type),
                _ => throw new NotSupportedException(),
            };
        }

        /// <summary>
        /// Visits an array data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected virtual TType VisitArray(ArrayDataType type)
        {
            var oldElementType = type.ElementType;
            var newElementType = Visit(oldElementType);
            return MakeArray(type, newElementType);
        }

        /// <summary>
        /// Makes an array type representation.
        /// </summary>
        /// <param name="type">Original array data type.</param>
        /// <param name="elementType">Element type of the array.</param>
        /// <returns>Representation of an array type.</returns>
        protected abstract TType MakeArray(ArrayDataType type, TType elementType);

        /// <summary>
        /// Visits an expression data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected abstract TType VisitExpression(ExpressionDataType type);

        /// <summary>
        /// Visits a function data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected virtual TType VisitFunction(FunctionDataType type)
        {
            var oldParameterTypes = type.ParameterTypes;
            var count = oldParameterTypes.Count;
            var newParameterTypes = new TType[count];

            for (var i = 0; i < count; ++i)
            {
                newParameterTypes[i] = Visit(oldParameterTypes[i]);
            }

            var oldReturnType = type.ReturnType;
            var newReturnType = Visit(oldReturnType);

            return MakeFunction(type, new ReadOnlyCollection<TType>(newParameterTypes), newReturnType);
        }

        /// <summary>
        /// Makes an function type representation.
        /// </summary>
        /// <param name="type">Original function data type.</param>
        /// <param name="parameterTypes">Parameter types of the function.</param>
        /// <param name="returnType">Return type of the function.</param>
        /// <returns>Representation of a function type.</returns>
        protected abstract TType MakeFunction(FunctionDataType type, ReadOnlyCollection<TType> parameterTypes, TType returnType);

        /// <summary>
        /// Visits an open generic parameter data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected abstract TType VisitOpenGenericParameter(OpenGenericParameterDataType type);

        /// <summary>
        /// Visits a primitive data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected abstract TType VisitPrimitive(PrimitiveDataType type);

        /// <summary>
        /// Visits a quotation data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected virtual TType VisitQuotation(QuotationDataType type)
        {
            var oldFunctionType = type.Function;
            var newFunctionType = Visit(oldFunctionType);

            return MakeQuotation(type, newFunctionType);
        }

        /// <summary>
        /// Makes a quotation type representation.
        /// </summary>
        /// <param name="type">Original quotation data type.</param>
        /// <param name="functionType">Type of the function.</param>
        /// <returns>Representation of a quotation type.</returns>
        protected abstract TType MakeQuotation(QuotationDataType type, TType functionType);

        /// <summary>
        /// Visits a structural data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected virtual TType VisitStructural(StructuralDataType type)
        {
            var oldProperties = type.Properties;
            var count = oldProperties.Count;
            var newProperties = new TProperty[count];

            for (var i = 0; i < count; ++i)
            {
                newProperties[i] = VisitProperty(oldProperties[i]);
            }

            return MakeStructural(type, new ReadOnlyCollection<TProperty>(newProperties));
        }

        /// <summary>
        /// Makes a structural type representation.
        /// </summary>
        /// <param name="type">Original structural data type.</param>
        /// <param name="properties">Properties of the structural type.</param>
        /// <returns>Representation of a structural type.</returns>
        protected abstract TType MakeStructural(StructuralDataType type, ReadOnlyCollection<TProperty> properties);

        /// <summary>
        /// Visits a custom data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type.</returns>
        protected abstract TType VisitCustom(DataType type);

        /// <summary>
        /// Visits a data model property.
        /// </summary>
        /// <param name="property">Property to visit.</param>
        /// <returns>Result of visiting the property.</returns>
        protected virtual TProperty VisitProperty(DataProperty property)
        {
            if (property == null)
            {
                return default;
            }

            var oldType = property.Type;
            var newType = Visit(oldType);

            return MakeProperty(property, newType);
        }

        /// <summary>
        /// Makes a property representation.
        /// </summary>
        /// <param name="property">Original property.</param>
        /// <param name="propertyType">Property type.</param>
        /// <returns>Representation of a property.</returns>
        protected abstract TProperty MakeProperty(DataProperty property, TType propertyType);
    }
}
