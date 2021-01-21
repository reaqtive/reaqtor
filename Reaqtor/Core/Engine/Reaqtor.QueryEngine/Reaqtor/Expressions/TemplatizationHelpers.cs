// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.Expressions
{
    /// <summary>
    /// A set of utilities for templatizing query engine artifacts.
    /// </summary>
    internal static class TemplatizationHelpers
    {
        /// <summary>
        /// The template identifier base.
        /// </summary>
        public const string TemplateBase = "rx://template/";

        /// <summary>
        /// Checks if an expression is templatized.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// <b>true</b> if the expression is templatized, <b>false</b> otherwise.
        /// </returns>
        public static bool IsTemplatized(this Expression expression) => expression.IsTemplatized(out _, out _);

        /// <summary>
        /// Checks if an expression is templatized.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="template">The template parameter.</param>
        /// <param name="argument">The template argument.</param>
        /// <returns>
        /// <b>true</b> if the expression is templatized, <b>false</b> otherwise.
        /// </returns>
        public static bool IsTemplatized(this Expression expression, out ParameterExpression template, out Expression argument)
        {
            if (expression is InvocationExpression invoke)
            {
                template = invoke.Expression as ParameterExpression;
                if (template != null && template.Name != null && template.Name.StartsWith(TemplateBase, StringComparison.Ordinal) && invoke.Arguments.Count <= 1)
                {
                    argument = invoke.Arguments.SingleOrDefault();
                    return true;
                }
            }

            template = null;
            argument = null;
            return false;
        }

        /// <summary>
        /// Substitutes a template expression with a new identifier.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="replacement">The replacement identifier.</param>
        /// <returns>A template expression with the identifier replaced.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Throws an invalid operation exception if the expression is not templatized.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Throws an argument null exception if the replacement identifier is null.
        /// </exception>
        public static Expression SubstituteTemplateId(this Expression expression, string replacement)
        {
            if (replacement == null)
            {
                throw new ArgumentNullException(nameof(replacement));
            }

            if (expression.IsTemplatized(out var parameter, out _))
            {
                var replacementParameter = Expression.Parameter(parameter.Type, replacement);
                return new TemplateSubstitutor(parameter, replacementParameter).Visit(expression);
            }

            throw new InvalidOperationException(
                string.Format(CultureInfo.InvariantCulture, "Expression '{0}' is not templatized.", expression.ToTraceString()));
        }

        /// <summary>
        /// Try to templatize an expression, only if it has constants.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// The expression with a template parameter substituted, the template, and the name of the parameter.
        /// </returns>
        public static AppliedExpressionTemplate TemplatizeAndIdentify(this Expression expression)
        {
            if (!expression.IsTemplatized())
            {
                var templatized = expression.Templatize();

                var templateId = TemplateBase + Guid.NewGuid().ToString("D");
                var template = templatized.Template;

                Expression result;
                if (templatized.Argument != null)
                {
                    result = Expression.Invoke(
                        Expression.Parameter(
                            typeof(Func<,>).MakeGenericType(templatized.Argument.Type, expression.Type), templateId),
                            templatized.Argument);
                }
                else
                {
                    result = Expression.Invoke(Expression.Parameter(typeof(Func<>).MakeGenericType(expression.Type), templateId));
                }

                return new AppliedExpressionTemplate
                {
                    Expression = result,
                    Template = template,
                    TemplateId = templateId,
                };
            }

            return new AppliedExpressionTemplate
            {
                Expression = expression,
            };
        }

        private sealed class TemplateSubstitutor : ScopedExpressionVisitor<ParameterExpression>
        {
            private readonly ParameterExpression _search;
            private readonly ParameterExpression _replacement;

            public TemplateSubstitutor(ParameterExpression search, ParameterExpression replacement)
            {
                _search = search;
                _replacement = replacement;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (!TryLookup(node, out _) && node == _search)
                {
                    return _replacement;
                }

                return node;
            }

            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;
        }
    }

    internal struct AppliedExpressionTemplate
    {
        public Expression Expression;
        public LambdaExpression Template;
        public string TemplateId;
    }
}
