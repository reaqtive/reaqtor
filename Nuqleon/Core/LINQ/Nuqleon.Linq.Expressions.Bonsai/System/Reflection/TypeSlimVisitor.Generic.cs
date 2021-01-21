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
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.CompilerServices;

namespace System.Reflection
{
    /// <summary>
    /// Visitor over the structure of a lightweight representation of a type.
    /// </summary>
    /// <typeparam name="TType">Type representing a type.</typeparam>
    /// <typeparam name="TSimpleType">Type representing a simple type.</typeparam>
    /// <typeparam name="TArrayType">Type representing an array type.</typeparam>
    /// <typeparam name="TStructuralType">Type representing a structural type.</typeparam>
    /// <typeparam name="TGenericDefinitionType">Type representing an open generic type definition.</typeparam>
    /// <typeparam name="TGenericType">Type representing a closed generic type.</typeparam>
    /// <typeparam name="TGenericParameterType">Type representing a generic parameter type.</typeparam>
    public abstract class TypeSlimVisitor<TType, TSimpleType, TArrayType, TStructuralType, TGenericDefinitionType, TGenericType, TGenericParameterType>
        where TSimpleType : TType
        where TArrayType : TType
        where TStructuralType : TType
        where TGenericDefinitionType : TType
        where TGenericType : TType
        where TGenericParameterType : TType
    {
        /// <summary>
        /// Visits the specified type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        public virtual TType Visit(TypeSlim type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.Kind switch
            {
                TypeSlimKind.Simple => VisitSimple((SimpleTypeSlim)type),
                TypeSlimKind.Array => VisitArray((ArrayTypeSlim)type),
                TypeSlimKind.GenericDefinition => VisitGenericDefinition((GenericDefinitionTypeSlim)type),
                TypeSlimKind.Generic => VisitGeneric((GenericTypeSlim)type),
                TypeSlimKind.GenericParameter => VisitGenericParameter((GenericParameterTypeSlim)type),
                TypeSlimKind.Structural => VisitStructural((StructuralTypeSlim)type),
                _ => throw new NotSupportedException("Unknown type kind."),
            };
        }

        /// <summary>
        /// Visits a simple type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected abstract TSimpleType VisitSimple(SimpleTypeSlim type);

        /// <summary>
        /// Visits a structural type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TStructuralType VisitStructural(StructuralTypeSlim type)
        {
            // NB: The use of lazy enumerable sequences here is required for backwards compatibility
            //     with some derived types (such as the DataModel assemblies) which may override the
            //     MakeStructuralType method in such a way it never triggers enumeration, thus never
            //     causing a visit for the property types. This can be used to prevent stack overflow
            //     when dealing with recursive types.

            var newPropertyTypes = type.Properties.Select(prop => new KeyValuePair<PropertyInfoSlim, TType>(prop, Visit(prop.PropertyType)));
            var newIndexParameterTypes = type.Properties.Select(prop => new KeyValuePair<PropertyInfoSlim, ReadOnlyCollection<TType>>(prop, Visit(prop.IndexParameterTypes)));
            return MakeStructuralType(type, newPropertyTypes, newIndexParameterTypes);
        }

        /// <summary>
        /// Constructs a structural type with the specified member types.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <param name="propertyTypes">Mapping from original properties to new property types.</param>
        /// <param name="propertyIndexParameters">Mapping from original index parameters to new property types.</param>
        /// <returns>Representation of a structural type with the given members.</returns>
        protected abstract TStructuralType MakeStructuralType(StructuralTypeSlim type, IEnumerable<KeyValuePair<PropertyInfoSlim, TType>> propertyTypes, IEnumerable<KeyValuePair<PropertyInfoSlim, ReadOnlyCollection<TType>>> propertyIndexParameters);

        /// <summary>
        /// Visits an array type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TArrayType VisitArray(ArrayTypeSlim type)
        {
            var rank = type.Rank;

            var elemOld = type.ElementType;
            var elemNew = Visit(elemOld);

            return MakeArrayType(type, elemNew, rank);
        }

        /// <summary>
        /// Constructs an array type with the specified element type and rank.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <param name="elementType">Element type of the array.</param>
        /// <param name="rank">Rank of the array, i.e. the number of dimensions. If the rank is null, the array is single-dimensional.</param>
        /// <returns>Representation of an array type with the given element type and rank.</returns>
        protected abstract TArrayType MakeArrayType(ArrayTypeSlim type, TType elementType, int? rank);

        /// <summary>
        /// Visits an open generic type definition.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TGenericDefinitionType VisitGenericDefinition(GenericDefinitionTypeSlim type)
        {
            return MakeGenericDefinition(type);
        }

        /// <summary>
        /// Constructs an open generic type definition with the specified parameter types.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <returns>Representation of an open generic type definition with the given parameter types.</returns>
        protected abstract TGenericDefinitionType MakeGenericDefinition(GenericDefinitionTypeSlim type);

        /// <summary>
        /// Visits a close generic type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TGenericType VisitGeneric(GenericTypeSlim type)
        {
            var typeDefinition = VisitAndConvert<TGenericDefinitionType>(type.GenericTypeDefinition);
            var arguments = VisitGenericTypeArguments(type);
            return MakeGeneric(type, typeDefinition, arguments);
        }

        /// <summary>
        /// Constructs a closed generic type with the specified type definition and type arguments.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <param name="typeDefinition">Generic type definition.</param>
        /// <param name="arguments">Generic type arguments.</param>
        /// <returns>Representation of a closed generic type with the given type definition and type parameters.</returns>
        protected abstract TGenericType MakeGeneric(GenericTypeSlim type, TGenericDefinitionType typeDefinition, ReadOnlyCollection<TType> arguments);

        /// <summary>
        /// Visits a generic parameter type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected abstract TGenericParameterType VisitGenericParameter(GenericParameterTypeSlim type);

        /// <summary>
        /// Visits and converts a type.
        /// </summary>
        /// <typeparam name="TResult">Type representing the kind of type to convert to.</typeparam>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of visiting and converting the type.</returns>
        public TResult VisitAndConvert<TResult>(TypeSlim type)
            where TResult : TType
        {
            var newType = Visit(type);

            if (newType is not TResult)
                throw new InvalidOperationException("Type must rewrite to the same kind.");

            return (TResult)newType;
        }

        /// <summary>
        /// Visits a collection of types.
        /// </summary>
        /// <param name="types">Types to visit.</param>
        /// <returns>Result of visiting the types.</returns>
        public ReadOnlyCollection<TType> Visit(ReadOnlyCollection<TypeSlim> types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            var n = types.Count;

            var res = new TType[n];

            for (var i = 0; i < n; i++)
            {
                res[i] = Visit(types[i]);
            }

            return new TrueReadOnlyCollection<TType>(/* transfer ownership */res);
        }

        /// <summary>
        /// Visits the generic type arguments of a generic type.
        /// </summary>
        /// <param name="genericType">The generic type whose type arguments to visit.</param>
        /// <returns>Result of visiting the generic type arguments.</returns>
        public ReadOnlyCollection<TType> VisitGenericTypeArguments(GenericTypeSlim genericType)
        {
            if (genericType == null)
                throw new ArgumentNullException(nameof(genericType));

            var n = genericType.GenericArgumentCount;

            var res = new TType[n];

            for (var i = 0; i < n; i++)
            {
                res[i] = Visit(genericType.GetGenericArgument(i));
            }

            return new TrueReadOnlyCollection<TType>(/* transfer ownership */res);
        }

        /// <summary>
        /// Visits a collection of types.
        /// </summary>
        /// <param name="types">Types to visit.</param>
        /// <returns>Result of visiting the types.</returns>
        public ReadOnlyCollection<TResult> VisitAndConvert<TResult>(ReadOnlyCollection<TypeSlim> types)
            where TResult : TType
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            var n = types.Count;

            var res = new TResult[n];

            for (var i = 0; i < n; i++)
            {
                res[i] = (TResult)Visit(types[i]);
            }

            return new TrueReadOnlyCollection<TResult>(/* transfer ownership */res);
        }
    }
}
