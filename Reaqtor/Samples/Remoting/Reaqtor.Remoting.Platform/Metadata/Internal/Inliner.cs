// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Poor man's inliner, used in lieu of the BetaReducer because WCF Data Services doesn't like it when a regular expression visitor traverses its trees due to the use of a custom node type.
    /// </summary>
    internal class Inliner : ExpressionVisitor
    {
        /// <summary>
        /// Parameter to replace.
        /// </summary>
        private readonly ParameterExpression _parameter;

        /// <summary>
        /// Argument to use for the replacement of the parameter.
        /// </summary>
        private readonly Expression _argument;

        /// <summary>
        /// Creates a new inliner that will replace occurrences of the specified parameter for the specified argument expression.
        /// </summary>
        /// <param name="parameter">Parameter whose occurrences should be replaces.</param>
        /// <param name="argument">Argument used to replace the parameter.</param>
        public Inliner(ParameterExpression parameter, Expression argument)
        {
            _parameter = parameter;
            _argument = argument;
        }

        /// <summary>
        /// Checks whether the parameter is to be replaced and returns the replacement if so.
        /// </summary>
        /// <param name="node">Parameter expression to check and optionally replace.</param>
        /// <returns>Argument replacing the parameter; otherwise, the original parameter.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node == _parameter)
            {
                return _argument;
            }

            return base.VisitParameter(node);
        }
    }
}
