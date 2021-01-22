// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections;
using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Equality comparer for non-generic trees.
    /// </summary>
    public class TreeEqualityComparer : IEqualityComparer<ITree>
    {
        private readonly IEqualityComparer _comparer;

        /// <summary>
        /// Creates a new tree equality comparer using the default comparer for tree node values.
        /// </summary>
        public TreeEqualityComparer() => _comparer = EqualityComparer<object>.Default;

        /// <summary>
        /// Creates a new tree equality comparer using the specified comparer for tree node values.
        /// </summary>
        /// <param name="comparer">Equality comparer for tree node values.</param>
        public TreeEqualityComparer(IEqualityComparer comparer) => _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

        /// <summary>
        /// Checks whether two trees are equal.
        /// </summary>
        /// <param name="x">First tree to compare.</param>
        /// <param name="y">Second tree to compare.</param>
        /// <returns>true if both trees are equal; otherwise, false.</returns>
        public bool Equals(ITree x, ITree y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return _comparer.Equals(x.Value, y.Value) && x.Children.SequenceEqual(y.Children, this);
        }

        /// <summary>
        /// Gets a hash code representation of the specified tree.
        /// </summary>
        /// <param name="obj">Tree to get a hash code representation for.</param>
        /// <returns>Hash code for the specified tree.</returns>
        public int GetHashCode(ITree obj) => obj == null ? 1979 : obj.Children.Aggregate(_comparer.GetHashCode(obj.Value), (h, c) => h * 23 + GetHashCode(c));
    }
}
