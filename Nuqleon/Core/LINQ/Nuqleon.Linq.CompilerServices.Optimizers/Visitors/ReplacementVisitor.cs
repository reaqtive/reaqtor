// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Visitor to replace a parameter in an expression.
    /// </summary>
    internal class ReplacementVisitor : ExpressionVisitor
    {
        private readonly Expression _replacement;
        private readonly ParameterExpression _variableToReplace;

        private int scopes;

        /// <summary>
        /// Creates a visitor to repace a parameter in an expression.
        /// </summary>
        /// <param name="variableToReplace">The variable to replace.</param>
        /// <param name="replacement">The expression to replace the variable with.</param>
        public ReplacementVisitor(ParameterExpression variableToReplace, Expression replacement)
        {
            _variableToReplace = variableToReplace;
            _replacement = replacement;
        }

        /// <summary>
        /// Visits the lambda to perform scope tracking.
        /// </summary>
        /// <typeparam name="T">The delegate type of the lambda.</typeparam>
        /// <param name="node">The lambda expression node to visit.</param>
        /// <returns>The visited expression.</returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var paramsContainsVariableToReplace = node.Parameters.Contains(_variableToReplace);

            if (paramsContainsVariableToReplace)
            {
                scopes++;
            }

            var body = Visit(node.Body);

            if (paramsContainsVariableToReplace)
            {
                scopes--;
            }

            return node.Update(body, node.Parameters);
        }

        /// <summary>
        /// Visits the parameter and replaces it if it matches the variable to be replaced and is in scope.
        /// </summary>
        /// <param name="node">The parameter expression node to visit.</param>
        /// <returns>The visited expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (scopes == 0 && _variableToReplace == node)
            {
                return _replacement;
            }

            return node;
        }
    }
}
