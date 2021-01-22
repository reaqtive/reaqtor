// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Interface for an optimizer for query expression trees.
    /// </summary>
    public interface IOptimizer
    {
        /// <summary>
        /// Optimizes the given query expression.
        /// </summary>
        /// <param name="queryTree">The query expression to optimize.</param>
        /// <returns>The optimized query tree.</returns>
        QueryTree Optimize(QueryTree queryTree);
    }
}
