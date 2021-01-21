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
    /// Provides various utilities for expression trees.
    /// </summary>
    public static class ExpressionHelpers
    {
        /// <summary>
        /// Strips top-level UnaryExpression expression tree nodes of type Quote from the given expression and returns the resulting LambdaExpression.
        /// </summary>
        /// <param name="expression">Expression to strip quotes from.</param>
        /// <returns>Lambda expression after unquoting. An exception occurs if the specified expression was not a (quoted) LambdaExpression.</returns>
        public static LambdaExpression Unquote(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return (LambdaExpression)StripQuotesImpl(expression);
        }

        /// <summary>
        /// Strips top-level UnaryExpression expression tree nodes of type Quote, if any, from the given expression.
        /// </summary>
        /// <param name="expression">Expression to strip quotes from.</param>
        /// <returns>Expression after unquoting.</returns>
        public static Expression StripQuotes(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return StripQuotesImpl(expression);
        }

        private static Expression StripQuotesImpl(Expression expression)
        {
            while (expression.NodeType == ExpressionType.Quote)
                expression = ((UnaryExpression)expression).Operand;

            return expression;
        }
    }
}
