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
    /// Eliminates compiler-generated names from expression trees (e.g. C# transparent identifier range variables).
    /// </summary>
    public static class CompilerGeneratedNameEliminator
    {
        /// <summary>
        /// Prettifies the specified expression by eliminating compiler-generated names.
        /// </summary>
        /// <param name="expression">Expression to prettify.</param>
        /// <returns>Expression tree with compiler-generated names substituted for prettier names.</returns>
        public static Expression Prettify(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Impl().Visit(expression);
            res = AlphaRenamer.EliminateNameConflicts(res);
            return res;
        }

        private sealed class Impl : ScopedExpressionVisitor<ParameterExpression>
        {
            protected override ParameterExpression GetState(ParameterExpression parameter)
            {
                if (!string.IsNullOrEmpty(parameter.Name) && parameter.Name.StartsWith(Constants.CS_TRANSPARENTIDENTIFIER_PREFIX, StringComparison.Ordinal))
                {
                    return Expression.Parameter(parameter.Type, "t");
                }

                return parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (!TryLookup(node, out ParameterExpression res))
                {
                    res = GetState(node);
                    GlobalScope.Add(node, res);
                }

                return res;
            }
        }
    }
}
