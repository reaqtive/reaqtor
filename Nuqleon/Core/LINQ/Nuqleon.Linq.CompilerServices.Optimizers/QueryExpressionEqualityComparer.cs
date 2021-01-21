// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Customizable equality comparer for query expression trees. Default behavior matches trees in a structural fashion.
    /// </summary>
    public class QueryExpressionEqualityComparer : IEqualityComparer<QueryTree>
    {
        private readonly Func<QueryExpressionEqualityComparator> _comparatorFactory;

        /// <summary>
        /// Creates a new query expression equality comparer with structural matching behavior.
        /// </summary>
        public QueryExpressionEqualityComparer()
        {
            // TODO Add ctor that takes a pool

            _comparatorFactory = () => new QueryExpressionEqualityComparator();
        }

        /// <summary>
        /// Creates a new query expression equality comparer with custom matching behavior implemented on the specified comparator.
        /// </summary>
        /// <param name="comparatorFactory">Factory for comparators that define custom matching behavior.</param>
        public QueryExpressionEqualityComparer(Func<QueryExpressionEqualityComparator> comparatorFactory)
        {
            _comparatorFactory = comparatorFactory ?? throw new ArgumentNullException(nameof(comparatorFactory));
        }

        /// <summary>
        /// Checks whether the two given query expressions are equal.
        /// </summary>
        /// <param name="x">First query expression.</param>
        /// <param name="y">Second query expression.</param>
        /// <returns>true if both query expressions are equal; otherwise, false.</returns>
        public bool Equals(QueryTree x, QueryTree y) => GetComparator().Equals(x, y);

        /// <summary>
        /// Gets a hash code for the given query expression.
        /// </summary>
        /// <param name="obj">Query expression to compute a hash code for.</param>
        /// <returns>Hash code for the given query expression.</returns>
        public int GetHashCode(QueryTree obj) => GetComparator().GetHashCode(obj);

        private QueryExpressionEqualityComparator GetComparator()
        {
            return _comparatorFactory() ?? throw new InvalidOperationException("Factory returned null reference.");
        }
    }
}
