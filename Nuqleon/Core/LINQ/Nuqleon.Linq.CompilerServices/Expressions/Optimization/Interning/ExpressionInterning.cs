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
    /// Static helper class for expression interning with default cache.
    /// </summary>
    public static class ExpressionInterning
    {
        private static readonly ExpressionInterningCache s_cache = new();

        /// <summary>
        /// Rewrites the expression tree using pre-existing cached nodes.
        /// </summary>
        /// <typeparam name="TExpression">The type of expression to rewrite.</typeparam>
        /// <param name="expression">The expression to rewrite.</param>
        /// <returns>The expression with nodes replaced by potentially pre-cached expression values.</returns>
        /// <remarks>
        /// This extension method uses a static default cache with a default expression equality comparer.
        /// For more control over equality checks for expressions, construct a new instance of the
        /// <code cref="System.Linq.Expressions.ExpressionInterningCache">ExpressionInterningCache</code>.
        /// </remarks>
        public static TExpression Intern<TExpression>(this TExpression expression)
            where TExpression : Expression
        {
            return expression.Intern(s_cache);
        }

        /// <summary>
        /// Rewrites the expression tree using pre-existing cached nodes.
        /// </summary>
        /// <typeparam name="TExpression">The type of expression to rewrite.</typeparam>
        /// <param name="expression">The expression to rewrite.</param>
        /// <param name="cache">The expression cache to use for the rewrite.</param>
        /// <returns>The expression with nodes replaced by potentially pre-cached expression values.</returns>
        public static TExpression Intern<TExpression>(this TExpression expression, IExpressionInterningCache cache)
            where TExpression : Expression
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return (TExpression)cache.GetOrAdd(expression);
        }

        /// <summary>
        /// Clears the static interning cache used by the `Intern` extension method.
        /// </summary>
        public static void ClearInternCache() => s_cache.Clear();
    }
}
