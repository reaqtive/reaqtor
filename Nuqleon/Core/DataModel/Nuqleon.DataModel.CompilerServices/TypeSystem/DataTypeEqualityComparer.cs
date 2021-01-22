// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Collections.Generic;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Equality comparer for data model complaint types.
    /// </summary>
    [KnownType]
    public class DataTypeEqualityComparer<T> : IEqualityComparer<T>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types. (Mirrors EqualityComparer<T>.Default.)

        /// <summary>
        /// Gets a default instance for the equality comparer.
        /// </summary>
        public static DataTypeEqualityComparer<T> Default { get; } = new DataTypeEqualityComparer<T>();

#pragma warning restore CA1000

        /// <summary>
        /// Checks for value equality of two objects with data model-compliant types.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        public bool Equals(T x, T y) => DataTypeObjectEqualityComparer.Default.Equals(x, y);

        /// <summary>
        /// Gets the hash code of an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The hash code of the object.</returns>
        public int GetHashCode(T obj) => DataTypeObjectEqualityComparer.Default.GetHashCode(obj);
    }
}
