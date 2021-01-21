// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Interface for expression visitors.
    /// </summary>
    public interface IRecursiveExpressionVisitor
    {
        /// <summary>
        /// Visits the specified expression.
        /// </summary>
        /// <param name="expression">Expression to visit.</param>
        /// <param name="visit">Recursive visit function.</param>
        /// <param name="result">Result of the visit.</param>
        /// <returns>true if the node was visited; otherwise, false.</returns>
        bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result);
    }
}
