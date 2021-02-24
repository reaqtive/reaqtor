// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks. (Protected methods are supposed to be called with non-null arguments.)

using Nuqleon.DataModel.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    /// <summary>
    /// Type space to convert CLR reflection objects into the
    /// corresponding slim representation of reflection objects with data
    /// model awareness.
    /// </summary>
    public class DataModelTypeSpace : TypeSpace
    {
        /// <summary>
        /// Creates the type space.
        /// </summary>
        public DataModelTypeSpace() => TypeConverter = new TypeCarryingTypeToTypeSlimConverter();

        /// <summary>
        /// Gets the converter used to create slim representations of types.
        /// </summary>
        protected override TypeToTypeSlimConverter TypeConverter { get; }

        /// <summary>
        /// Creates a slim representation of a type property.
        /// </summary>
        /// <param name="originalProperty">The original property.</param>
        /// <param name="declaringTypeSlim">The slim representation of the declaring type.</param>
        /// <param name="propertyTypeSlim">The slim representation of the property type.</param>
        /// <param name="indexParameterTypeSlims">The slim representations of the index parameter types.</param>
        /// <returns>The slim representation of the property.</returns>
        protected override PropertyInfoSlim GetPropertyCore(PropertyInfo originalProperty, TypeSlim declaringTypeSlim, TypeSlim propertyTypeSlim, ReadOnlyCollection<TypeSlim> indexParameterTypeSlims)
        {
            if (!originalProperty.DeclaringType.IsDefined(typeof(KnownTypeAttribute), inherit: false))
            {
                var propertyMapping = originalProperty.GetCustomAttribute<MappingAttribute>(inherit: false);
                if (propertyMapping != null)
                {
                    return declaringTypeSlim.GetProperty(propertyMapping.Uri, propertyTypeSlim, indexParameterTypeSlims, originalProperty.CanWrite);
                }
            }
            return base.GetPropertyCore(originalProperty, declaringTypeSlim, propertyTypeSlim, indexParameterTypeSlims);
        }

        /// <summary>
        /// Creates a slim representation of a type field.
        /// </summary>
        /// <param name="originalField">The original field.</param>
        /// <param name="declaringTypeSlim">The slim representation of the declaring type.</param>
        /// <param name="fieldTypeSlim">The slim representation of the field type.</param>
        /// <returns>The slim representation of the field.</returns>
        protected override FieldInfoSlim GetFieldCore(FieldInfo originalField, TypeSlim declaringTypeSlim, TypeSlim fieldTypeSlim)
        {
            if (!originalField.DeclaringType.IsDefined(typeof(KnownTypeAttribute), inherit: false))
            {
                var fieldMapping = originalField.GetCustomAttribute<MappingAttribute>(inherit: false);
                if (fieldMapping != null)
                {
                    return declaringTypeSlim.GetField(fieldMapping.Uri, fieldTypeSlim);
                }
            }
            return base.GetFieldCore(originalField, declaringTypeSlim, fieldTypeSlim);
        }

        /// <summary>
        /// Creates a slim representation of a type constructor.
        /// </summary>
        /// <param name="originalConstructor">The original constructor.</param>
        /// <param name="declaringTypeSlim">The slim representation of the declaring type.</param>
        /// <param name="parameterTypeSlims">The slim representations of the constructor parameter types.</param>
        /// <returns>The slim representation of the constructor.</returns>
        protected override ConstructorInfoSlim GetConstructorCore(ConstructorInfo originalConstructor, TypeSlim declaringTypeSlim, ReadOnlyCollection<TypeSlim> parameterTypeSlims)
        {
            var slimConstructor = base.GetConstructorCore(originalConstructor, declaringTypeSlim, parameterTypeSlims);
            var parameters = originalConstructor.GetParameters();

            DataTypeHelpers.TryFromTypeCached(originalConstructor.DeclaringType, allowCycles: true, out var res);

            if (res is StructuralDataType dataType)
            {
                var parameterMappings = EmptyReadOnlyCollection<string>.Instance;

                if (dataType.StructuralKind == StructuralDataTypeKinds.Entity)
                {
                    var mappings = new List<string>(parameters.Length);
                    var mappedCount = 0;
                    for (var i = 0; i < parameters.Length; ++i)
                    {
                        var mapping = parameters[i].GetCustomAttribute<MappingAttribute>(inherit: false);

                        if (mapping != null)
                        {
                            mappedCount++;
                            mappings.Add(mapping.Uri);
                        }
                    }

                    if (mappedCount != parameters.Length)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "There are '{0}' parameters in the constructor of '{1}' that do not have a mapping attributes. In order for type '{1}' to be a valid data model entity type, all of its constructors' parameters should be annotated with mapping attributes.", parameters.Length - mappedCount, originalConstructor.DeclaringType.ToCSharpStringPretty()));
                    }

                    if (mappings.Count > 0 && mappings[0] != null)
                    {
                        parameterMappings = mappings.AsReadOnly();
                    }
                }

                return new DataModelConstructorInfoSlim(slimConstructor.DeclaringType, slimConstructor.ParameterTypes, parameterMappings);
            }

            return slimConstructor;
        }
    }
}
