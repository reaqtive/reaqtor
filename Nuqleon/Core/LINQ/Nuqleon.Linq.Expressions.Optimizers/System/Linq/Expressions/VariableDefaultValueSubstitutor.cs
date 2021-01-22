// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Utility to substitute occurrences of unassigned variables by default values.
    /// </summary>
    internal sealed class VariableDefaultValueSubstitutor : ExpressionVisitor
    {
        /// <summary>
        /// The set of unassigned variables to substitute by default value expressions.
        /// </summary>
        private readonly HashSet<ParameterExpression> _unassigned;

        /// <summary>
        /// Environment to track binding of variables, which is needed because unassigned
        /// variables may be shadowed by variable declaration in inner scopes.
        /// </summary>
        private readonly Stack<IEnumerable<ParameterExpression>> _environment = new();

        /// <summary>
        /// Creates a new variable substitutor to substitute the specified <paramref name="unassigned"/>
        /// variables by default value expressions.
        /// </summary>
        /// <param name="unassigned">The unassigned variables to substitute.</param>
        public VariableDefaultValueSubstitutor(HashSet<ParameterExpression> unassigned)
        {
            _unassigned = unassigned;
        }

        /// <summary>
        /// Visits a block expression to perform variable declaration scope tracking.
        /// </summary>
        /// <param name="node">The block expression to visit.</param>
        /// <returns>The result of visiting the block expression.</returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            if (node.Variables.Count > 0)
            {
                _environment.Push(node.Variables);
            }

            var expressions = Visit(node.Expressions);

            if (node.Variables.Count > 0)
            {
                _environment.Pop();
            }

            return node.Update(node.Variables, expressions);
        }

        /// <summary>
        /// Visits a catch block node to perform variable declaration scope tracking.
        /// </summary>
        /// <param name="node">The catch block to visit.</param>
        /// <returns>The result of visiting the catch block.</returns>
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            if (node.Variable != null)
            {
                _environment.Push(new[] { node.Variable });
            }

            var filter = Visit(node.Filter);
            var body = Visit(node.Body);

            if (node.Variable != null)
            {
                _environment.Pop();
            }

            return node.Update(node.Variable, filter, body);
        }

        /// <summary>
        /// Visits a lambda expression to perform variable declaration scope tracking.
        /// </summary>
        /// <typeparam name="T">The type of the delegate represented by the lambda expression.</typeparam>
        /// <param name="node">The lambda expression to visit.</param>
        /// <returns>The result of visiting the lambda expression.</returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (node.Parameters.Count > 0)
            {
                _environment.Push(node.Parameters);
            }

            var body = Visit(node.Body);

            if (node.Parameters.Count > 0)
            {
                _environment.Pop();
            }

            return node.Update(body, node.Parameters);
        }

        /// <summary>
        /// Visits a parameter expression to analyze whether it needs to be substituted
        /// for a default value expression.
        /// </summary>
        /// <param name="node">The parameter expression to visit.</param>
        /// <returns>The original node or a default expression if the variable is unassigned.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            foreach (var scope in _environment)
            {
                foreach (var variable in scope)
                {
                    if (node == variable)
                    {
                        return node;
                    }
                }
            }

            if (_unassigned.Contains(node))
            {
                return Expression.Default(node.Type);
            }

            return node;
        }
    }
}
