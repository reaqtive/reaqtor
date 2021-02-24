// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Unifier for CLR types. Nuqleon only has it for slim types.
//
// BD - September 2014
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices.TypeSystem;
using System.Reflection;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Provides a unification mechanism for wildcards and types.
    /// </summary>
    internal class TypeUnifier
    {
        /// <summary>
        /// Gets the dictionary containing the bindings of wildcards to types.
        /// </summary>
        public IDictionary<Type, Type> Bindings { get; } = new Dictionary<Type, Type>();

        /// <summary>
        /// Unifies the two given types.
        /// </summary>
        /// <param name="type1">First type to unify.</param>
        /// <param name="type2">Second type to unify.</param>
        /// <exception cref="InvalidOperationException">Thrown when the structure of the types doesn't match or unification fails due to binding conflicts.</exception>
        public void Unify(Type type1, Type type2)
        {
            if (type1 == type2)
            {
                return;
            }

            if (type1.IsArray)
            {
                if (!type2.IsArray)
                {
                    throw new InvalidOperationException("Both types should be arrays.");
                }

                Unify(type1.GetElementType(), type2.GetElementType());
                return;
            }

            if (type1.IsGenericType && !type1.IsGenericTypeDefinition)
            {
                if (!(type2.IsGenericType && !type2.IsGenericTypeDefinition))
                {
                    throw new InvalidOperationException("Both types should be closed generics.");
                }

                var g1 = type1.GetGenericTypeDefinition();
                var g2 = type2.GetGenericTypeDefinition();

                if (g1 != g2)
                {
                    throw new InvalidOperationException("Generic type definitions should match.");
                }

                var a1 = type1.GetGenericArguments();
                var a2 = type2.GetGenericArguments();

                for (var i = 0; i < a1.Length; i++)
                {
                    Unify(a1[i], a2[i]);
                }

                return;
            }

            var w1 = type1.IsDefined(typeof(TypeWildcardAttribute));
            var w2 = type2.IsDefined(typeof(TypeWildcardAttribute));

            if (!(w1 ^ w2))
            {
                throw new InvalidOperationException("Only one wildcard allowed.");
            }

            var w = default(Type);
            var t = default(Type);

            if (w1)
            {
                w = type1;
                t = type2;
            }
            else if (w2)
            {
                w = type2;
                t = type1;
            }

            if (Bindings.TryGetValue(w, out var r))
            {
                if (r != t)
                {
                    throw new InvalidOperationException("Wildcard already bound.");
                }
            }

            Bindings[w] = t;
        }
    }
}
