// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Visits a catch block to perform optimization steps.
        /// </summary>
        /// <param name="node">The catch block to visit.</param>
        /// <returns>The result of optimizing the catch block.</returns>
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            var res = base.VisitCatchBlock(node);

            res = RemoveUnusedVariables(res);

            var filter = res.Filter;

            if (filter != null && HasConstantValue(filter))
            {
                var filterValue = (bool)GetConstantValue(filter);

                if (filterValue)
                {
                    return res.Update(res.Variable, filter: null, res.Body);
                }

                // NB: Filter evaluating to false has to be dealt with higher up.
            }

            return res;
        }

        /// <summary>
        /// Removes the variable used for a catch block if it's not used.
        /// </summary>
        /// <param name="catchBlock">The catch block to remove unused exception variables from.</param>
        /// <returns>The result of rewriting the catch block.</returns>
        private static CatchBlock RemoveUnusedVariables(CatchBlock catchBlock)
        {
            var variable = catchBlock.Variable;

            if (variable != null)
            {
                // REVIEW: This does a lot of repeated scanning of the same child nodes.
                //         We could consider storing these results for later reuse.

                var finder = new FreeVariableFinder();

                finder.Visit(catchBlock.Filter);
                finder.Visit(catchBlock.Body);

                var freeVariables = finder.FreeVariables;

                if (!freeVariables.Contains(variable))
                {
                    return catchBlock.Update(variable: null, catchBlock.Filter, catchBlock.Body);
                }
            }

            return catchBlock;
        }
    }
}
