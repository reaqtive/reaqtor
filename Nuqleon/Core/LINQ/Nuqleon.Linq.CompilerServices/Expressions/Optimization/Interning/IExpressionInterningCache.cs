// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - January 2014 - Created this file.
//

namespace System.Linq.Expressions
{
    /// <summary>
    /// Interface for expression interning cache.
    /// </summary>
    public interface IExpressionInterningCache
    {
        /// <summary>
        /// Gets the number of entries in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Rewrites the given expression tree with nodes from the cache.
        /// </summary>
        /// <param name="expression">The expression to rewrite.</param>
        /// <returns>
        /// An equivalent expression with pre-cached nodes replacing nodes in the given tree.
        /// </returns>
        /// <remarks>
        /// As a side-effect, adds each new node to the cache.
        /// </remarks>
        Expression GetOrAdd(Expression expression);
    }
}
