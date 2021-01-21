// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - April 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;

#if USE_SLIM
namespace System.Linq.CompilerServices.Bonsai
#else
namespace System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using Expression = ExpressionSlim;

    using ExpressionEqualityComparator = ExpressionSlimEqualityComparator;

    #endregion
#endif

    /// <summary>
    /// Customizable equality comparer for expression trees. Default behavior matches trees in a structural fashion.
    /// </summary>
#if USE_SLIM
    public class ExpressionSlimEqualityComparer
#else
    public class ExpressionEqualityComparer
#endif
        : IEqualityComparer<Expression>
    {
        private readonly Func<ExpressionEqualityComparator> _comparatorFactory;

        /// <summary>
        /// Creates a new expression equality comparer with structural matching behavior.
        /// </summary>
#if USE_SLIM
        public ExpressionSlimEqualityComparer()
#else
        public ExpressionEqualityComparer()
#endif
        {
            _comparatorFactory = () => new ExpressionEqualityComparator();
        }

        /// <summary>
        /// Creates a new expression equality comparer with custom matching behavior implemented on the specified comparator.
        /// </summary>
        /// <param name="comparatorFactory">Factory for comparators that define custom matching behavior.</param>
#if USE_SLIM
        public ExpressionSlimEqualityComparer(Func<ExpressionEqualityComparator> comparatorFactory)
#else
        public ExpressionEqualityComparer(Func<ExpressionEqualityComparator> comparatorFactory)
#endif
        {
            _comparatorFactory = comparatorFactory ?? throw new ArgumentNullException(nameof(comparatorFactory));
        }

        /// <summary>
        /// Checks whether the two given expressions are equal.
        /// </summary>
        /// <param name="x">First expression.</param>
        /// <param name="y">Second expression.</param>
        /// <returns>true if both expressions are equal; otherwise, false.</returns>
        public bool Equals(Expression x, Expression y) => GetComparator().Equals(x, y);

        /// <summary>
        /// Gets a hash code for the given expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code for the given expression.</returns>
        public int GetHashCode(Expression obj) => GetComparator().GetHashCode(obj);

        private ExpressionEqualityComparator GetComparator()
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
