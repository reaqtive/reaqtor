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
using System.Reflection;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Visitor of the structure of a data model type.
    /// </summary>
    public class DataTypeVisitor
    {
        /// <summary>
        /// Visits a data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type, or the original data type if nothing changed.</returns>
        public virtual DataType Visit(DataType type)
        {
            if (type == null)
            {
                return null;
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
        /// <returns>Result of visiting the data type, or the original data type if nothing changed.</returns>
        protected virtual DataType VisitArray(ArrayDataType type)
        {
            var oldElementType = type.ElementType;
            var newElementType = Visit(oldElementType);

            if (oldElementType != newElementType)
            {
                var newUnderlyingType = ChangeUnderlyingType(type.UnderlyingType);
                return new ArrayDataType(newUnderlyingType, newElementType);
            }

            return type;
        }

        /// <summary>
        /// Visits an expression data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type, or the original data type if nothing changed.</returns>
        protected virtual DataType VisitExpression(ExpressionDataType type) => type;

        /// <summary>
        /// Visits a function data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type, or the original data type if nothing changed.</returns>
        protected virtual DataType VisitFunction(FunctionDataType type)
        {
            var oldParameterTypes = type.ParameterTypes;
            var newParameterTypes = Visit(oldParameterTypes, Visit);

            var oldReturnType = type.ReturnType;
            var newReturnType = Visit(oldReturnType);

            if (oldParameterTypes != newParameterTypes || oldReturnType != newReturnType)
            {
                var newUnderlyingType = ChangeUnderlyingType(type.UnderlyingType);
                return new FunctionDataType(newUnderlyingType, newParameterTypes, newReturnType);
            }

            return type;
        }

        /// <summary>
        /// Visits an open generic parameter type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type, or the original data type if nothing changed.</returns>
        protected virtual DataType VisitOpenGenericParameter(OpenGenericParameterDataType type) => type;

        /// <summary>
        /// Visits a primitive data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type, or the original data type if nothing changed.</returns>
        protected virtual DataType VisitPrimitive(PrimitiveDataType type) => type;

        /// <summary>
        /// Visits a quotation data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type, or the original data type if nothing changed.</returns>
        protected virtual DataType VisitQuotation(QuotationDataType type)
        {
            var oldFunctionType = type.Function;
            var newFunctionType = VisitAndConvert<FunctionDataType>(oldFunctionType);

            if (oldFunctionType != newFunctionType)
            {
                var newUnderlyingType = ChangeUnderlyingType(type.UnderlyingType);
                return new QuotationDataType(newUnderlyingType, newFunctionType);
            }

            return type;
        }

        /// <summary>
        /// Visits a structural data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type, or the original data type if nothing changed.</returns>
        protected virtual DataType VisitStructural(StructuralDataType type)
        {
            var oldProperties = type.Properties;
            var newProperties = Visit(oldProperties, VisitProperty);

            if (oldProperties != newProperties)
            {
                var newUnderlyingType = ChangeUnderlyingType(type.UnderlyingType);
                return new StructuralDataType(newUnderlyingType, newProperties, type.StructuralKind);
            }

            return type;
        }

        /// <summary>
        /// Visits a custom data type.
        /// </summary>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting the data type, or the original data type if nothing changed.</returns>
        protected virtual DataType VisitCustom(DataType type) => throw new NotImplementedException("Derived types should implement VisitCustom.");

        /// <summary>
        /// Visits a data model property.
        /// </summary>
        /// <param name="property">Property to visit.</param>
        /// <returns>Result of visiting the property, or the original property if nothing changed.</returns>
        protected virtual DataProperty VisitProperty(DataProperty property)
        {
            var oldType = property.Type;
            var newType = Visit(oldType);

            if (oldType != newType)
            {
                MemberInfo newProperty;
                if (property.Property is PropertyInfo oldProperty)
                {
                    newProperty = ChangeProperty(oldProperty);
                }
                else
                {
                    var oldField = (FieldInfo)property.Property;
                    newProperty = ChangeField(oldField);
                }

                return new DataProperty(newProperty, property.Name, newType);
            }

            return property;
        }

        /// <summary>
        /// Visits and converts a data type.
        /// </summary>
        /// <typeparam name="T">Type of the data type to convert to.</typeparam>
        /// <param name="type">Data type to visit.</param>
        /// <returns>Result of visiting and converting the type.</returns>
        protected T VisitAndConvert<T>(DataType type)
            where T : DataType
        {
            if (Visit(type) is not T newType)
            {
                throw new InvalidOperationException("Data type must rewrite to the same kind.");
            }

            return newType;
        }

        /// <summary>
        /// Changes the underlying CLR type of a data type during a rewrite of data types.
        /// </summary>
        /// <param name="type">CLR type to change.</param>
        /// <returns>Result of changing the CLR type.</returns>
        protected virtual Type ChangeUnderlyingType(Type type) => throw new NotImplementedException("Derived class should provide a means of converting the underlying type of a data type to a new data type.");

        /// <summary>
        /// Changes the CLR property during a rewrite of properties.
        /// </summary>
        /// <param name="property">CLR property to change.</param>
        /// <returns>Result of changing the CLR property.</returns>
        protected virtual MemberInfo ChangeProperty(PropertyInfo property) => throw new NotImplementedException("Derived class should provide a means of converting the property of a structural data type to a new property.");

        /// <summary>
        /// Changes the CLR field during a rewrite of properties.
        /// </summary>
        /// <param name="field">CLR field to change.</param>
        /// <returns>Result of changing the CLR field.</returns>
        protected virtual MemberInfo ChangeField(FieldInfo field) => throw new NotImplementedException("Derived class should provide a means of converting the field of a structural data type to a new field.");

        /// <summary>
        /// Visits a collection of objects using the specified visit function.
        /// </summary>
        /// <typeparam name="T">Type of the objects in the collection.</typeparam>
        /// <param name="nodes">Nodes to visit.</param>
        /// <param name="elementVisitor">Function to visit each of the elements in the collection.</param>
        /// <returns>Result of visiting the nodes.</returns>
        public static ReadOnlyCollection<T> Visit<T>(ReadOnlyCollection<T> nodes, Func<T, T> elementVisitor)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));
            if (elementVisitor == null)
                throw new ArgumentNullException(nameof(elementVisitor));

            var res = default(T[]);

            var n = nodes.Count;
            for (int i = 0; i < n; i++)
            {
                var oldNode = nodes[i];
                var newNode = elementVisitor(oldNode);

                if (res != null)
                {
                    res[i] = newNode;
                }
                else
                {
                    if (!object.ReferenceEquals(oldNode, newNode))
                    {
                        res = new T[n];

                        for (int j = 0; j < i; j++)
                        {
                            res[j] = nodes[j];
                        }

                        res[i] = newNode;
                    }
                }
            }

            if (res != null)
            {
                return new ReadOnlyCollection<T>(res);
            }

            return nodes;
        }
    }
}
