// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - June 2014 - Created this file.
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// An equality comparer based exclusively on object reference, even if the
    /// object has implemented IEquatable or overrides the default Equals method.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    public class ReferenceEqualityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        private ReferenceEqualityComparer() { }

#pragma warning disable CA1000 // Do not declare static members on generic types.

        /// <summary>
        /// The instance of the equality comparer.
        /// </summary>
        public static ReferenceEqualityComparer<T> Instance { get; } = new ReferenceEqualityComparer<T>();

#pragma warning restore CA1000

        /// <summary>
        /// Checks if two objects are reference equal.
        /// </summary>
        /// <param name="x">The left object.</param>
        /// <param name="y">The right object.</param>
        /// <returns>
        /// <b>true</b> if the objects are reference equal, <b>false</b> otherwise.
        /// </returns>
        public bool Equals(T x, T y) => object.ReferenceEquals(x, y);

        /// <summary>
        /// Gets a unique hash code based on the object reference.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The reference-based hash code.</returns>
        public int GetHashCode(T obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
