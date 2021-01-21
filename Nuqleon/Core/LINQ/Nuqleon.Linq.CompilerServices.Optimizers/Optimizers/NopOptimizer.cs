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
    /// An optimizer that does not do any optimization.
    /// </summary>
    public sealed class NopOptimizer : IOptimizer
    {
        private NopOptimizer()
        {
        }

        /// <summary>
        /// Gets an instance of this optimizer.
        /// </summary>
        public static IOptimizer Instance { get; } = new NopOptimizer();

        /// <summary>
        /// Gives back the original tree.
        /// </summary>
        /// <param name="queryTree">The query expression to optimize.</param>
        /// <returns>The original query expression.</returns>
        public QueryTree Optimize(QueryTree queryTree) => queryTree;
    }
}
