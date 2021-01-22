// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Provides a set of extension methods for expressions and lightweight expressions.
    /// </summary>
    public static class ExpressionSlimExtensions
    {
        /// <summary>
        /// Converts the specified expression to a lightweight representation.
        /// </summary>
        /// <param name="expression">Expression to convert.</param>
        /// <returns>Lightweight representation of the specified expression.</returns>
        public static ExpressionSlim ToExpressionSlim(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new ExpressionToExpressionSlimConverter().Visit(expression);
        }

        /// <summary>
        /// Converts the specified expression to a lightweight representation using the specified expression factory.
        /// </summary>
        /// <param name="expression">Expression to convert.</param>
        /// <param name="factory">The expression factory to use.</param>
        /// <returns>Lightweight representation of the specified expression.</returns>
        public static ExpressionSlim ToExpressionSlim(this Expression expression, IExpressionSlimFactory factory)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new ExpressionToExpressionSlimConverter(new TypeSpace(), factory).Visit(expression);
        }

        // TODO: Introduce symmetric reflection provider concept for the slim space.

        /// <summary>
        /// Converts the lightweight representation of an expression to an expression.
        /// </summary>
        /// <param name="expression">Slim expression to convert.</param>
        /// <returns>Expression represented by the slim expression.</returns>
        public static Expression ToExpression(this ExpressionSlim expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new ExpressionSlimToExpressionConverter().Visit(expression);
        }

        /// <summary>
        /// Converts the lightweight representation of an expression to an expression using the specified expression factory.
        /// </summary>
        /// <param name="expression">Slim expression to convert.</param>
        /// <param name="factory">The expression factory to use.</param>
        /// <returns>Expression represented by the slim expression.</returns>
        public static Expression ToExpression(this ExpressionSlim expression, IExpressionFactory factory)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new ExpressionSlimToExpressionConverter(new InvertedTypeSpace(), factory).Visit(expression);
        }

        /// <summary>
        /// Converts the lightweight representation of an expression to an expression using the specified expression factory and reflection provider.
        /// </summary>
        /// <param name="expression">Slim expression to convert.</param>
        /// <param name="factory">The expression factory to use.</param>
        /// <param name="provider">The reflection provider to use.</param>
        /// <returns>Expression represented by the slim expression.</returns>
        public static Expression ToExpression(this ExpressionSlim expression, IExpressionFactory factory, IReflectionProvider provider)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            return new ExpressionSlimToExpressionConverter(new InvertedTypeSpace(provider), factory).Visit(expression);
        }
    }
}
