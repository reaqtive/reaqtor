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
using System.Linq.CompilerServices;

namespace System.Reflection
{
    /// <summary>
    /// Visitor over the structure of a lightweight representation of a type.
    /// </summary>
    public class TypeSlimVisitor
    {
        /// <summary>
        /// Dictionary to keep track of types that have already been visited in order to break recursion
        /// for cyclic type references.
        /// </summary>
        private readonly IDictionary<TypeSlim, TypeSlim> _types;

        /// <summary>
        /// Instantiates a visitor for slim types.
        /// </summary>
        public TypeSlimVisitor()
        {
            _types = new Dictionary<TypeSlim, TypeSlim>(ReferenceEqualityComparer<TypeSlim>.Instance);
        }

        /// <summary>
        /// Visits the specified type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        public virtual TypeSlim Visit(TypeSlim type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (_types.TryGetValue(type, out TypeSlim res))
            {
                return res;
            }

            res = type.Kind switch
            {
                TypeSlimKind.Simple => VisitSimple((SimpleTypeSlim)type),
                TypeSlimKind.Array => VisitArray((ArrayTypeSlim)type),
                TypeSlimKind.Structural => VisitStructural((StructuralTypeSlim)type),
                TypeSlimKind.GenericDefinition => VisitGenericDefinition((GenericDefinitionTypeSlim)type),
                TypeSlimKind.Generic => VisitGeneric((GenericTypeSlim)type),
                TypeSlimKind.GenericParameter => VisitGenericParameter((GenericParameterTypeSlim)type),
                _ => throw new NotSupportedException("Unknown type kind."),
            };

            _types[type] = res;
            return res;
        }

        /// <summary>
        /// Visits a simple type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TypeSlim VisitSimple(SimpleTypeSlim type)
        {
            return type;
        }

        /// <summary>
        /// Visits an array type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TypeSlim VisitArray(ArrayTypeSlim type)
        {
            var elementType = Visit(type.ElementType);

            return type.Update(elementType);
        }

        /// <summary>
        /// Visits a structural type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TypeSlim VisitStructural(StructuralTypeSlim type)
        {
            var properties = type.Properties;

            var newType = StructuralTypeSlimReference.Create(type.HasValueEqualitySemantics, type.StructuralKind, properties.Count);
            _types[type] = newType;

            var changed = false;
            foreach (var property in properties)
            {
                var oldPropertyType = property.PropertyType;
                var newPropertyType = oldPropertyType != null ? Visit(oldPropertyType) : null;

                if (Changed(oldPropertyType, newPropertyType))
                {
                    changed = true;
                }

                var oldIndexParameterTypes = property.IndexParameterTypes;
                var newIndexParameterTypes = Visit(oldIndexParameterTypes);

                for (int i = 0, n = oldIndexParameterTypes.Count; i < n; i++)
                {
                    if (Changed(oldIndexParameterTypes[i], newIndexParameterTypes[i]))
                    {
                        changed = true;
                        break;
                    }
                }

                var newProperty = newType.GetProperty(property.Name, newPropertyType, newIndexParameterTypes, property.CanWrite);
                newType.AddProperty(newProperty);
            }

            if (!changed)
            {
                _types[type] = type;
                return type;
            }
            else
            {
                newType.Freeze();
                return newType;
            }
        }

        private static bool Changed(TypeSlim oldType, TypeSlim newType)
        {
            return oldType != newType;
        }

        /// <summary>
        /// Visits an open generic type definition.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TypeSlim VisitGenericDefinition(GenericDefinitionTypeSlim type)
        {
            return type;
        }

        /// <summary>
        /// Visits a close generic type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TypeSlim VisitGeneric(GenericTypeSlim type)
        {
            var typeDefinition = VisitAndConvert(type.GenericTypeDefinition);
            var arguments = VisitGenericTypeArguments(type);

            // PERF: The == operator below will dispatch into the optimized Equals method for SimpeTypeBase,
            //       which avoids allocation of a TypeSlimEqualityComparator.

            if (typeDefinition == type.GenericTypeDefinition && arguments == null)
            {
                return type;
            }
            else
            {
                return type.Rewrite(typeDefinition, arguments);
            }
        }

        /// <summary>
        /// Visits a generic parameter type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TypeSlim VisitGenericParameter(GenericParameterTypeSlim type)
        {
            return type;
        }

        /// <summary>
        /// Visits and converts a type.
        /// </summary>
        /// <typeparam name="T">Type representing the kind of type to convert to.</typeparam>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of visiting and converting the type.</returns>
        public T VisitAndConvert<T>(T type)
            where T : TypeSlim
        {
            var newType = Visit(type) as T;

            if (newType == null)
                throw new InvalidOperationException("Type '{0}' must rewrite to the same kind.");

            return newType;
        }

        /// <summary>
        /// Visits a collection of types.
        /// </summary>
        /// <param name="types">Types to visit.</param>
        /// <returns>Result of visiting the types.</returns>
        public ReadOnlyCollection<TypeSlim> Visit(ReadOnlyCollection<TypeSlim> types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            var res = default(TypeSlim[]);

            var n = types.Count;
            for (int i = 0; i < n; i++)
            {
                var oldNode = types[i];
                var newNode = Visit(oldNode);

                if (res != null)
                {
                    res[i] = newNode;
                }
                else
                {
                    if ((object)oldNode != newNode)
                    {
                        res = new TypeSlim[n];

                        for (int j = 0; j < i; j++)
                        {
                            res[j] = types[j];
                        }

                        res[i] = newNode;
                    }
                }
            }

            if (res != null)
            {
                return new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ res);
            }

            return types;
        }

        /// <summary>
        /// Visits the generic type arguments of a generic type.
        /// </summary>
        /// <param name="genericType">The generic type whose type arguments to visit.</param>
        /// <returns>Result of visiting the generic type arguments; null if no types changed.</returns>
        protected TypeSlim[] VisitGenericTypeArguments(GenericTypeSlim genericType)
        {
            var res = default(TypeSlim[]);

            var n = genericType.GenericArgumentCount;
            for (int i = 0; i < n; i++)
            {
                var oldNode = genericType.GetGenericArgument(i);
                var newNode = Visit(oldNode);

                if (res != null)
                {
                    res[i] = newNode;
                }
                else
                {
                    if ((object)oldNode != newNode)
                    {
                        res = new TypeSlim[n];

                        for (int j = 0; j < i; j++)
                        {
                            res[j] = genericType.GetGenericArgument(j);
                        }

                        res[i] = newNode;
                    }
                }
            }

            return res;
        }
    }
}
