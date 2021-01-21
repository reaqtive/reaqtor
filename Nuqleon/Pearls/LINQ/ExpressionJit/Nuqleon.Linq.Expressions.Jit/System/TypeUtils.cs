// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Reflection;

namespace System
{
    /// <summary>
    /// Provides various convenience helpers to interact with the type system.
    /// </summary>
    internal static class TypeUtils
    {
        /// <summary>
        /// Checks whether the specified type is a closed generic nullable type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is a closed generic nullable type; otherwise, false.</returns>
        public static bool IsNullableType(this Type type)
        {
            var info = type.GetTypeInfo();

            return info.IsConstructedGenericType
                && info.GetGenericTypeDefinition() == typeof(Nullable<>)
                ;
        }

        /// <summary>
        /// Returns the non-nullable variant of the specified type, if it exists.
        /// </summary>
        /// <param name="type">The type to get the non-nullable variant for.</param>
        /// <returns>The non-nullable variant of the specified type if it exists; otherwise, the original type.</returns>
        public static Type GetNonNullableType(this Type type)
        {
            if (type.IsNullableType())
            {
                return type.GetGenericArguments()[0];
            }

            return type;
        }

        /// <summary>
        /// Checks whether two types are equivalent.
        /// </summary>
        /// <param name="first">The first type to check for equivalence.</param>
        /// <param name="second">The second type to check for equivalence.</param>
        /// <returns>true if both types are equivalent; otherwise, false.</returns>
        public static bool AreEquivalent(Type first, Type second)
        {
            return first == second
                || first.GetTypeInfo().IsEquivalentTo(second.GetTypeInfo())
                ;
        }
    }
}
