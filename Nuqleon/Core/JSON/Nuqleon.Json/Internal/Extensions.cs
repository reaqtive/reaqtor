// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

using System;
using System.Collections.Generic;

namespace Nuqleon.Json.Internal
{
    /// <summary>
    /// Extension methods for internal use.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Checks whether a type is a nullable value type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if the type is a nullable value type (Nullable&lt;T&gt;); false otherwise.</returns>
        public static bool IsNullable(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        /// <summary>
        /// Advances the enumerator to the next element of the collection, producing the element as an output parameter if the enumerator was advanced.
        /// </summary>
        /// <typeparam name="T">The type of objects to enumerate.</typeparam>
        /// <param name="enumerator">Enumerator to consume input from.</param>
        /// <param name="value">Value of the enumerator's Current property in case the MoveNext operation succeeded.</param>
        /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        public static bool TryMoveNext<T>(this IEnumerator<T> enumerator, out T value)
        {
            if (enumerator.MoveNext())
            {
                value = enumerator.Current;
                return true;
            }

            value = default;
            return false;
        }
    }
}
