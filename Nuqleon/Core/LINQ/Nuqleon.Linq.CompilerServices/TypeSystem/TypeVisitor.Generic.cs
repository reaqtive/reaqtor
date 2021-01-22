// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Base class for visitor for CLR types, based on their structure.
    /// </summary>
    /// <typeparam name="TResult">Type of the result of the visit.</typeparam>
    public abstract class TypeVisitor<TResult>
    {
        /// <summary>
        /// Visits a CLR type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result after the visit.</returns>
        public virtual TResult Visit(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsArray)
            {
                return VisitArray(type);
            }

            if (type.IsGenericType)
            {
                return VisitGeneric(type);
            }

            if (type.IsGenericParameter)
            {
                return VisitGenericParameter(type);
            }

            if (type.IsByRef)
            {
                return VisitByRef(type);
            }

            if (type.IsPointer)
            {
                return VisitPointer(type);
            }

            return VisitSimple(type);
        }

        /// <summary>
        /// Visits a sequence of types.
        /// </summary>
        /// <param name="types">Sequence of types to visit.</param>
        /// <returns>Sequence consisting of the result of visiting each type in the original sequence.</returns>
        protected virtual IEnumerable<TResult> Visit(IEnumerable<Type> types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            return types.Select(Visit).ToArray();
        }

        /// <summary>
        /// Visits an array of types.
        /// </summary>
        /// <param name="types">Array of types to visit.</param>
        /// <returns>Array consisting of the result of visiting each type in the original sequence.</returns>
        protected virtual TResult[] Visit(Type[] types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            var n = types.Length;

            //
            // Typically only called with GetGenericArguments parameter, which has > 0 types,
            // so not adding an empty array case here.
            //

            var res = new TResult[n];

            for (var i = 0; i < n; i++)
            {
                res[i] = Visit(types[i]);
            }

            return res;
        }

        /// <summary>
        /// Visits all array types and dispatches into VisitArrayVector or VisitArrayMultidimensional.
        /// </summary>
        /// <param name="type">Array type to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected virtual TResult VisitArray(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsSZArray())
            {
                return VisitArrayVector(type);
            }
            else
            {
                return VisitArrayMultidimensional(type);
            }
        }

        /// <summary>
        /// Visits a single-dimensional array type, also known as a vector.
        /// </summary>
        /// <param name="type">Single-dimensional array type to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected virtual TResult VisitArrayVector(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var elemOld = type.GetElementType();
            var elemNew = Visit(elemOld);

            return MakeArrayType(type, elemNew);
        }

        /// <summary>
        /// Constructs a single-dimensional vector array type from the specified element type.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <param name="elementType">Element type of the array.</param>
        /// <returns>Representation of a single-dimensional vector array type with the given element type.</returns>
        protected abstract TResult MakeArrayType(Type type, TResult elementType);

        /// <summary>
        /// Visits a multi-dimensional array type.
        /// </summary>
        /// <param name="type">Multi-dimensional array type to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected virtual TResult VisitArrayMultidimensional(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var rank = type.GetArrayRank();

            var elemOld = type.GetElementType();
            var elemNew = Visit(elemOld);

            return MakeArrayType(type, elemNew, rank);
        }

        /// <summary>
        /// Constructs a multi-dimensional array type from the specified element type and rank.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <param name="elementType">Element type of the array.</param>
        /// <param name="rank">Rank (the number of dimensions) of the array.</param>
        /// <returns>Representation of a multi-dimensional array type with the given element type and rank.</returns>
        protected abstract TResult MakeArrayType(Type type, TResult elementType, int rank);

        /// <summary>
        /// Visits types that are generic and dispatches into VisitGenericClosed or VisitGenericTypeDefinition.
        /// Notice this method does not dispatch into VisitGenericParameter, because a generic parameter is not categorized as a generic type.
        /// </summary>
        /// <param name="type">Generic type to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected virtual TResult VisitGeneric(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsGenericTypeDefinition)
            {
                return VisitGenericTypeDefinition(type);
            }
            else
            {
                return VisitGenericClosed(type);
            }
        }

        /// <summary>
        /// Visits a generic parameter.
        /// </summary>
        /// <param name="type">Generic parameter to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected abstract TResult VisitGenericParameter(Type type);

        /// <summary>
        /// Visits a closed generic type.
        /// </summary>
        /// <param name="type">Closed generic type to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected virtual TResult VisitGenericClosed(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var oldGenDef = type.GetGenericTypeDefinition();
            var newGenDef = Visit(oldGenDef);

            var oldGenArgs = type.GetGenericArguments();
            var newGenArgs = Visit(oldGenArgs);

            return MakeGenericType(type, newGenDef, newGenArgs);
        }

        /// <summary>
        /// Constructs a closed generic type from the specified generic type definition and type arguments.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <param name="genericTypeDefinition">Generic type definition.</param>
        /// <param name="genericArguments">Generic type arguments.</param>
        /// <returns>Representation of a closed generic type with the given generic type definition and type arguments.</returns>
        protected abstract TResult MakeGenericType(Type type, TResult genericTypeDefinition, params TResult[] genericArguments);

        /// <summary>
        /// Visits a generic type definition.
        /// </summary>
        /// <param name="type">Generic type definition to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected abstract TResult VisitGenericTypeDefinition(Type type);

        /// <summary>
        /// Visits a by ref type.
        /// </summary>
        /// <param name="type">By ref type to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected virtual TResult VisitByRef(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var elemOld = type.GetElementType();
            var elemNew = Visit(elemOld);

            return MakeByRefType(type, elemNew);
        }

        /// <summary>
        /// Constructs a by ref type from the specified element type.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <param name="elementType">Element type of the array.</param>
        /// <returns>Representation of a by ref type with the given element type.</returns>
        protected abstract TResult MakeByRefType(Type type, TResult elementType);

        /// <summary>
        /// Visits a pointer type.
        /// </summary>
        /// <param name="type">Pointer type to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected virtual TResult VisitPointer(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var elemOld = type.GetElementType();
            var elemNew = Visit(elemOld);

            return MakePointerType(type, elemNew);
        }

        /// <summary>
        /// Constructs a pointer type from the specified element type.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <param name="elementType">Element type of the array.</param>
        /// <returns>Representation of a pointer type with the given element type.</returns>
        protected abstract TResult MakePointerType(Type type, TResult elementType);

        /// <summary>
        /// Visits a type with no internal structure.
        /// </summary>
        /// <param name="type">Simple type to visit.</param>
        /// <returns>Result after the visit.</returns>
        protected abstract TResult VisitSimple(Type type);
    }
}
