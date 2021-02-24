// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/07/2017 - Created this type.
//

using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    /// <summary>
    /// Equality comparer that checks for reference equality.
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    public sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        /// <summary>
        /// The singleton instance of the <see cref="ReferenceEqualityComparer{T}"/> class.
        /// </summary>
        public static readonly ReferenceEqualityComparer<T> Instance = new();

        /// <summary>
        /// Private constructor used to ensure the singleton instancing.
        /// </summary>
        private ReferenceEqualityComparer()
        {
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>true if the specified objects are reference equal; otherwise, false.</returns>
        public bool Equals(T x, T y) => x == y;

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        public int GetHashCode(T obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
