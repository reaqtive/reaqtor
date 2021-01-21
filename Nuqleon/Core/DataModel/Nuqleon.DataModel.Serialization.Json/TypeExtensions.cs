// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Nuqleon.DataModel.Serialization.Json
{
    /// <summary>
    /// Type extensions.
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified type is a Tuple.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is a tuple; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTuple(this Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            Type genericType = type.GetGenericTypeDefinition();
            return genericType == typeof(Tuple<>)
                 || genericType == typeof(Tuple<,>)
                 || genericType == typeof(Tuple<,,>)
                 || genericType == typeof(Tuple<,,,>)
                 || genericType == typeof(Tuple<,,,,>)
                 || genericType == typeof(Tuple<,,,,,>)
                 || genericType == typeof(Tuple<,,,,,,>)
                 || genericType == typeof(Tuple<,,,,,,,>);
        }

        /// <summary>
        /// Returns the class inheritance chain for the type including the type itself.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Class inheritance chain.</returns>
        public static IEnumerable<Type> InheritanceChain(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
