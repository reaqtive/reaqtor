// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using the word delegate to reflect the concept of delegation.)

using System.Linq.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Indicates that a class can analyze and possibly rewrite an expression tree containing itself. The expression tree
    /// will map the delegation target as a parameter expression.
    /// </summary>
    public interface IDelegationTarget
    {
        /// <summary>
        /// Checks whether the tree should be rewritten.
        /// </summary>
        /// <param name="node">The parameter expression representing this delegation target.</param>
        /// <param name="expression">The whole subexpression on which to perform the analysis.</param>
        /// <returns>True if the tree should be rewritten, false otherwise.</returns>
        bool CanDelegate(ParameterExpression node, Expression expression);

        /// <summary>
        /// Rewrites a tree.
        /// </summary>
        /// <param name="node">The parameter expression representing this delegation target.</param>
        /// <param name="expression">The whole subexpression on which to perform the rewrite.</param>
        /// <returns>The rewritten tree.</returns>
        Expression Delegate(ParameterExpression node, Expression expression);
    }
}
