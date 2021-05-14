// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - July 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks. (Protected methods are supposed to be called with non-null arguments.)

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;

namespace System.Reflection
{
    /// <summary>
    /// Pairwise checker for unification of type slims to types.
    /// </summary>
    public class TypeUnifier
    {
        private readonly Dictionary<TypeSlim, Type> _entries;
        private readonly Dictionary<Type, TypeSlim> _wildcards;
        private readonly bool _safe;

        /// <summary>
        /// Instantiates a type unifier with safe flag set to false.
        /// </summary>
        public TypeUnifier()
            : this(safe: false)
        {
        }

        /// <summary>
        /// Instantiates a type unifier.
        /// </summary>
        /// <param name="safe">true if no exceptions should be thrown, false otherwise.</param>
        public TypeUnifier(bool safe)
        {
            _safe = safe;
            _entries = new Dictionary<TypeSlim, Type>();
            _wildcards = new Dictionary<Type, TypeSlim>();
        }

        /// <summary>
        /// Set of types unified as a result of calling Unify.
        /// </summary>
        public ReadOnlyDictionary<TypeSlim, Type> Entries => new(_entries);

        /// <summary>
        /// Attempts to unify a CLR type and a slim type.
        /// </summary>
        /// <param name="typeRich">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        /// <returns>true if the unification was successful, false or exception thrown if safe is true or false, respectively.</returns>
        public bool Unify(Type typeRich, TypeSlim typeSlim)
        {
            if (typeRich == null)
                throw new ArgumentNullException(nameof(typeRich));
            if (typeSlim == null)
                throw new ArgumentNullException(nameof(typeSlim));

            if (_entries.TryGetValue(typeSlim, out Type mappedType))
            {
                var equalityComparer = new StructuralTypeEqualityComparer();
                if (equalityComparer.Equals(typeRich, mappedType))
                {
                    return true;
                }
                else
                {
                    return FailUnify(string.Format(CultureInfo.InvariantCulture, "Unification failed between '{0}' and '{1}'.  Slim type '{1}' already mapped to '{2}'.", typeRich.ToCSharpStringPretty(), typeSlim, mappedType.ToCSharpStringPretty()));
                }
            }

            if (typeRich.IsDefined(typeof(TypeWildcardAttribute), inherit: false))
            {
                if (_wildcards.TryGetValue(typeRich, out TypeSlim wildcardMatch))
                {
                    if (typeSlim == wildcardMatch)
                    {
                        return true;
                    }
                    else
                    {
                        return FailUnify(string.Format(CultureInfo.InvariantCulture, "Unification failed between '{0}' and '{1}'.  Wildcard '{0}' already mapped to '{2}'.", typeRich.ToCSharpStringPretty(), typeSlim, wildcardMatch));
                    }
                }
                else
                {
                    _wildcards[typeRich] = typeSlim;
                    return true;
                }
            }

            var res = typeSlim.Kind switch
            {
                TypeSlimKind.Simple => UnifySimple(typeRich, (SimpleTypeSlim)typeSlim),
                TypeSlimKind.Array => UnifyArray(typeRich, (ArrayTypeSlim)typeSlim),
                TypeSlimKind.Structural => UnifyStructural(typeRich, (StructuralTypeSlim)typeSlim),
                TypeSlimKind.GenericDefinition => UnifyGenericDefinition(typeRich, (GenericDefinitionTypeSlim)typeSlim),
                TypeSlimKind.Generic => UnifyGeneric(typeRich, (GenericTypeSlim)typeSlim),
                TypeSlimKind.GenericParameter => UnifyGenericParameter(typeRich, (GenericParameterTypeSlim)typeSlim),
                _ => throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Cannot unify slim types of kind '{0}'.", typeSlim.Kind)),
            };

            if (!res)
            {
                return FailUnify(string.Format(CultureInfo.InvariantCulture, "Unification failed between '{0}' and '{1}'.", typeRich.ToCSharpStringPretty(), typeSlim));
            }

            return res;
        }

        /// <summary>
        /// Attempts to unify a CLR type and a simple slim type.
        /// </summary>
        /// <param name="typeRich">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        /// <returns>true if the unification was successful, false or exception thrown if safe is true or false, respectively.</returns>
        protected virtual bool UnifySimple(Type typeRich, SimpleTypeSlim typeSlim)
        {
            return UnifySimpleBase(typeRich, typeSlim);
        }

        private static bool UnifySimpleBase(Type typeRich, SimpleTypeSlimBase typeSlim)
        {
            return typeRich.Assembly.FullName == typeSlim.Assembly.Name && typeRich.FullName == typeSlim.Name;
        }

        /// <summary>
        /// Attempts to unify a CLR type and a slim array type.
        /// </summary>
        /// <param name="typeRich">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        /// <returns>true if the unification was successful, false or exception thrown if safe is true or false, respectively.</returns>
        protected virtual bool UnifyArray(Type typeRich, ArrayTypeSlim typeSlim)
        {
            if (!typeRich.IsArray)
            {
                return false;
            }

            var typeSlimRank = typeSlim.Rank != null ? typeSlim.Rank.Value : 1;

            return Unify(typeRich.GetElementType(), typeSlim.ElementType) && typeRich.GetArrayRank() == typeSlimRank;
        }

        /// <summary>
        /// Attempts to unify a CLR type and a structural slim type.
        /// </summary>
        /// <param name="typeRich">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        /// <returns>true if the unification was successful, false or exception thrown if safe is true or false, respectively.</returns>
        protected virtual bool UnifyStructural(Type typeRich, StructuralTypeSlim typeSlim)
        {
            _entries[typeSlim] = typeRich;

            foreach (var propertySlim in typeSlim.Properties)
            {
                var propertyRich = ResolveProperty(typeRich, propertySlim);
                if (propertyRich == null || !Unify(propertyRich.PropertyType, propertySlim.PropertyType))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Attempts to get the property info from the given CLR type that corresponds to the given slim property info.
        /// </summary>
        /// <param name="declaringTypeRich">The CLR type declaring the property.</param>
        /// <param name="propertySlim">The slim property.</param>
        /// <returns>The property info from the CLR type or null if not found.</returns>
        protected virtual PropertyInfo ResolveProperty(Type declaringTypeRich, PropertyInfoSlim propertySlim)
        {
            return declaringTypeRich.GetProperty(propertySlim.Name);
        }

        /// <summary>
        /// Attempts to unify a CLR type and a slim generic definition type.
        /// </summary>
        /// <param name="typeRich">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        /// <returns>true if the unification was successful, false or exception thrown if safe is true or false, respectively.</returns>
        protected virtual bool UnifyGenericDefinition(Type typeRich, GenericDefinitionTypeSlim typeSlim)
        {
            if (!typeRich.IsGenericTypeDefinition)
            {
                return false;
            }

            return UnifySimpleBase(typeRich, typeSlim);
        }

        /// <summary>
        /// Attempts to unify a CLR type and a slim generic type.
        /// </summary>
        /// <param name="typeRich">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        /// <returns>true if the unification was successful, false or exception thrown if safe is true or false, respectively.</returns>
        protected virtual bool UnifyGeneric(Type typeRich, GenericTypeSlim typeSlim)
        {
            if (!typeRich.IsGenericType || typeRich.IsGenericTypeDefinition)
            {
                return false;
            }

            var defRich = typeRich.GetGenericTypeDefinition();
            var defSlim = typeSlim.GenericTypeDefinition;

            if (!Unify(defRich, defSlim))
            {
                return false;
            }

            var argsRich = typeRich.GetGenericArguments();

            return UnifyGenericArguments(argsRich, typeSlim);
        }

        /// <summary>
        /// Attempts to unify the CLR types and slim types representing generic arguments.
        /// </summary>
        /// <param name="argsRich">The CLR types representing the generic arguments.</param>
        /// <param name="argsSlim">The slim generic type containing the generic arguments.</param>
        /// <returns>true if each of the pairwise unifications are successful, false otherwise.</returns>
        private bool UnifyGenericArguments(Type[] argsRich, GenericTypeSlim argsSlim)
        {
            var n = argsRich.Length;

            if (n != argsSlim.GenericArgumentCount)
            {
                return false;
            }

            for (var i = 0; i < n; i++)
            {
                var typeRich = argsRich[i];
                var typeSlim = argsSlim.GetGenericArgument(i);

                if (!Unify(typeRich, typeSlim))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Attempts to unify a CLR type and a slim generic parameter type.
        /// </summary>
        /// <param name="typeRich">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        /// <returns>Always throws exception; see comment.</returns>
        /// <exception cref="NotSupportedException">
        /// The base type unifier does not support unification of a slim generic parameter type and a CLR type.
        /// </exception>
        protected virtual bool UnifyGenericParameter(Type typeRich, GenericParameterTypeSlim typeSlim)
        {
            throw new NotSupportedException("Cannot unify open generic parameters.");
        }

        /// <summary>
        /// Attempts to unify a collection of CLR types with a collection of slim types.
        /// </summary>
        /// <param name="typeRiches">The collection of CLR types.</param>
        /// <param name="typeSlims">The collection of slim types.</param>
        /// <returns>true if each of the pairwise unifications are successful, false otherwise.</returns>
        protected bool Unify(ReadOnlyCollection<Type> typeRiches, ReadOnlyCollection<TypeSlim> typeSlims)
        {
            if (typeRiches.Count != typeSlims.Count)
            {
                return false;
            }

            var n = typeRiches.Count;

            for (var i = 0; i < n; i++)
            {
                var typeRich = typeRiches[i];
                var typeSlim = typeSlims[i];

                if (!Unify(typeRich, typeSlim))
                {
                    return false;
                }
            }

            return true;
        }

        private bool FailUnify(string exceptionMessage)
        {
            if (!_safe)
                throw new InvalidOperationException(exceptionMessage);

            return false;
        }
    }
}
