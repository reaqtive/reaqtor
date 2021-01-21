// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Applies eta conversion on lambda expressions (abstractions) over invocations (applications) of a function.
    /// </summary>
    /// <example>
    /// Consider the following expression:
    /// <code>
    /// Expression.Lambda(Expression.Invoke(f, x, y), x, y)
    /// </code>
    /// After eta conversion, the resulting expression is:
    /// <code>
    /// f
    /// </code>
    /// </example>
    public static class EtaConverter
    {
        /// <summary>
        /// Applies eta conversion on lambda expressions in the given expression.
        /// </summary>
        /// <param name="expression">Expression to apply eta conversions on.</param>
        /// <returns>Expression after applying eta conversions.</returns>
        public static Expression Convert(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new Impl().Visit(expression);
        }

        private sealed class Impl : ExpressionVisitor
        {
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var body = Visit(node.Body);

                if (body is InvocationExpression invoke)
                {
                    if (invoke.Expression.Type == node.Type)
                    {
                        if (invoke.Arguments.SequenceEqual(node.Parameters))
                        {
                            return invoke.Expression;
                        }
                    }
                }

                return node.Update(body, node.Parameters);
            }
        }
    }
}
