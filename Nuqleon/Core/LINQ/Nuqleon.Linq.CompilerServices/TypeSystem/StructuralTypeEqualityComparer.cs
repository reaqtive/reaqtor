// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Equality comparer for structural types.
    /// </summary>
    public class StructuralTypeEqualityComparer : IEqualityComparer<Type>
    {
        private static StructuralTypeEqualityComparer _instance;

        private readonly Func<StructuralTypeEqualityComparator> _comparatorFactory;

        /// <summary>
        /// Instantiates an equality comparer for structural types, using the default structural type comparator
        /// </summary>
        public StructuralTypeEqualityComparer()
            : this(() => new StructuralTypeEqualityComparator())
        {
        }

        /// <summary>
        /// Instantiates an equality comparer for structural types.
        /// </summary>
        /// <param name="comparatorFactory">A factory to get structural type comparator instances.</param>
        public StructuralTypeEqualityComparer(Func<StructuralTypeEqualityComparator> comparatorFactory) => _comparatorFactory = comparatorFactory ?? throw new ArgumentNullException(nameof(comparatorFactory));

        /// <summary>
        /// A default instance of the equality comparer.
        /// </summary>
        public static StructuralTypeEqualityComparer Default => _instance ??= new StructuralTypeEqualityComparer();

        /// <summary>
        /// Checks whether two given types are equal.
        /// </summary>
        /// <param name="x">The left type.</param>
        /// <param name="y">The right type.</param>
        /// <returns>true if the given types are equal, false otherwise.</returns>
        public bool Equals(Type x, Type y) => _comparatorFactory().Equals(x, y);

        /// <summary>
        /// Returns a hash code for the specified type.
        /// </summary>
        /// <param name="obj">The type for which a hash code is to be returned.</param>
        /// <returns>The hash code of the type.</returns>
        public int GetHashCode(Type obj) => _comparatorFactory().GetHashCode(obj);
    }
}
