// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Reflection
{
    /// <summary>
    /// An equality comparer for slim representations of CLR types.
    /// </summary>
    public class TypeSlimEqualityComparer : IEqualityComparer<TypeSlim>
    {
        private readonly Func<TypeSlimEqualityComparator> _comparatorFactory;

        /// <summary>
        /// Instantiates the slim type comparer.
        /// </summary>
        public TypeSlimEqualityComparer()
        {
            _comparatorFactory = () => new TypeSlimEqualityComparator();
        }

        /// <summary>
        /// Instantiates the slim type comparer.
        /// </summary>
        /// <param name="comparatorFactory">Generates a comparator to use for equality checks.</param>
        public TypeSlimEqualityComparer(Func<TypeSlimEqualityComparator> comparatorFactory)
        {
            _comparatorFactory = comparatorFactory ?? throw new ArgumentNullException(nameof(comparatorFactory));
        }

        /// <summary>
        /// A default instance of the equality comparer.
        /// </summary>
        public static TypeSlimEqualityComparer Default { get; } = new();

        #region Equals

        /// <summary>
        /// Checks if two type slims are equal.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        public bool Equals(TypeSlim x, TypeSlim y) => object.ReferenceEquals(x, y) || GetComparator().Equals(x, y);

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets a hash code of a type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        public int GetHashCode(TypeSlim obj) => GetComparator().GetHashCode(obj);

        #endregion

        private TypeSlimEqualityComparator GetComparator()
        {
            var comparator = _comparatorFactory();

            if (comparator == null)
            {
                throw new InvalidOperationException("Factory returned null reference.");
            }

            return comparator;
        }
    }
}
