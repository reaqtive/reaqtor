// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Base class for visitor for CLR types, based on their structure.
    /// </summary>
    public class TypeVisitor
    {
        /// <summary>
        /// Visits a CLR type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        public virtual Type Visit(Type type)
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
        /// Visits an array of types.
        /// </summary>
        /// <param name="types">Array of types to visit.</param>
        /// <returns>Array consisting of the result of visiting each type in the original sequence.</returns>
        protected Type[] Visit(Type[] types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            var res = default(Type[]);

            var n = types.Length;
            for (int i = 0; i < n; i++)
            {
                var typeOld = types[i];
                var typeNew = Visit(typeOld);

                if (res == null)
                {
                    if (typeOld != typeNew)
                    {
                        res = new Type[n];
                        Array.Copy(types, res, i);
                        res[i] = typeNew;
                    }
                }
                else
                {
                    res[i] = typeNew;
                }
            }

            return res ?? types;
        }

        /// <summary>
        /// Visits all array types and dispatches into VisitArrayVector or VisitArrayMultidimensional.
        /// </summary>
        /// <param name="type">Array type to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitArray(Type type)
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
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitArrayVector(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var elemOld = type.GetElementType();
            var elemNew = Visit(elemOld);

            if (elemOld != elemNew)
            {
                return elemNew.MakeArrayType();
            }
            else
            {
                return type;
            }
        }

        /// <summary>
        /// Visits a multi-dimensional array type.
        /// </summary>
        /// <param name="type">Multi-dimensional array type to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitArrayMultidimensional(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var rank = type.GetArrayRank();

            var elemOld = type.GetElementType();
            var elemNew = Visit(elemOld);

            if (elemOld != elemNew)
            {
                return elemNew.MakeArrayType(rank);
            }
            else
            {
                return type;
            }
        }

        /// <summary>
        /// Visits types that are generic and dispatches into VisitGenericClosed or VisitGenericTypeDefinition.
        /// Notice this method does not dispatch into VisitGenericParameter, because a generic parameter is not categorized as a generic type.
        /// </summary>
        /// <param name="type">Generic type to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitGeneric(Type type)
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
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitGenericParameter(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type;
        }

        /// <summary>
        /// Visits a closed generic type.
        /// </summary>
        /// <param name="type">Closed generic type to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitGenericClosed(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var oldGenDef = type.GetGenericTypeDefinition();
            var newGenDef = Visit(oldGenDef);

            var oldGenArgs = type.GetGenericArguments();
            var newGenArgs = Visit(oldGenArgs);

            if (oldGenDef != newGenDef || oldGenArgs != newGenArgs)
            {
                return newGenDef.MakeGenericType(newGenArgs);
            }
            else
            {
                return type;
            }
        }

        /// <summary>
        /// Visits a generic type definition.
        /// </summary>
        /// <param name="type">Generic type definition to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitGenericTypeDefinition(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type;
        }

        /// <summary>
        /// Visits a by ref type.
        /// </summary>
        /// <param name="type">By ref type to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitByRef(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var elemOld = type.GetElementType();
            var elemNew = Visit(elemOld);

            if (elemOld != elemNew)
            {
                return elemNew.MakeByRefType();
            }
            else
            {
                return type;
            }
        }

        /// <summary>
        /// Visits a pointer type.
        /// </summary>
        /// <param name="type">Pointer type to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitPointer(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var elemOld = type.GetElementType();
            var elemNew = Visit(elemOld);

            if (elemOld != elemNew)
            {
                return elemNew.MakePointerType();
            }
            else
            {
                return type;
            }
        }

        /// <summary>
        /// Visits a type with no internal structure.
        /// </summary>
        /// <param name="type">Simple type to visit.</param>
        /// <returns>Resulting type after the visit.</returns>
        protected virtual Type VisitSimple(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type;
        }
    }
}
