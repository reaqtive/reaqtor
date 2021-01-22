// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Unbound variable scanner used to detect use sites of variables without any surrounding declaration sites.
    /// </summary>
    /// <remarks>
    /// Instances of this type are stateful and should not be reused.
    /// </remarks>
    internal sealed class UnboundVariableScanner : BetterExpressionVisitor
    {
        /// <summary>
        /// Keeps an environment of nested scopes that declare one or more variables.
        /// </summary>
        private readonly Stack<HashSet<ParameterExpression>> _environment = new();

        /// <summary>
        /// Gets the unbound variables found in the expression tree.
        /// </summary>
        public readonly HashSet<ParameterExpression> UnboundVariables = new();

        /// <summary>
        /// Visits block expressions in order to push a scope of declared variables.
        /// </summary>
        /// <param name="node">The block expression to visit.</param>
        /// <returns>The original expression.</returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            var variables = node.Variables;

            if (variables.Count > 0)
            {
                _environment.Push(new HashSet<ParameterExpression>(variables));
            }

            Visit(node.Expressions);

            if (variables.Count > 0)
            {
                _environment.Pop();
            }

            return node;
        }

        /// <summary>
        /// Visits catch blocks in order to push a scope of declared variables.
        /// </summary>
        /// <param name="node">The catch block to visit.</param>
        /// <returns>The original node.</returns>
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            var variable = node.Variable;

            if (variable != null)
            {
                _environment.Push(new HashSet<ParameterExpression>(new[] { variable }));
            }

            Visit(node.Filter);
            Visit(node.Body);

            if (variable != null)
            {
                _environment.Pop();
            }

            return node;
        }

        /// <summary>
        /// Visits lambda expressions in order to push a scope of declared parameters.
        /// </summary>
        /// <typeparam name="T">The type of the delegate represented by the lambda expression.</typeparam>
        /// <param name="node">The lambda expression to visit.</param>
        /// <returns>The original expression.</returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = node.Parameters;

            if (parameters.Count > 0)
            {
                _environment.Push(new HashSet<ParameterExpression>(parameters));
            }

            Visit(node.Body);

            if (parameters.Count > 0)
            {
                _environment.Pop();
            }

            return node;
        }

        /// <summary>
        /// Visits parameter expressions to perform binding steps used to detect whether a variable is unbound.
        /// </summary>
        /// <param name="node">The parameter expression to bind to a surrounding scope, if any.</param>
        /// <returns>The original expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (!UnboundVariables.Contains(node))
            {
                var isBound = false;

                foreach (var frame in _environment)
                {
                    if (frame.Contains(node))
                    {
                        isBound = true;
                        break;
                    }
                }

                if (!isBound)
                {
                    UnboundVariables.Add(node);
                }
            }

            return node;
        }
    }
}
