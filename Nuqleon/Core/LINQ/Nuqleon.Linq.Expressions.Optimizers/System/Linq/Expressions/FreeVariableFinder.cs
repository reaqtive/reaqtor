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
    // CONSIDER: Rework the FreeVariableScanner to use this visitor underneath and provide direct access to
    //           the visitor for advanced users (such as the optimizer code).

    /// <summary>
    /// Visitor to find free variables in an expression.
    /// </summary>
    /// <remarks>
    /// This utility differs from <see cref="System.Linq.CompilerServices.FreeVariableScanner"/> because it
    /// provides direct access to the visitor and can be used to visit multiple expressions to collect the
    /// set of all free variables.
    /// </remarks>
    internal sealed class FreeVariableFinder : ExpressionVisitor
    {
        /// <summary>
        /// The set of free variables found in expressions that have been visited.
        /// </summary>
        public readonly HashSet<ParameterExpression> FreeVariables = new();

        /// <summary>
        /// Keeps track of the nested variable scopes. Any variable that occurs in any of these scopes is
        /// considered to be a bound variable.
        /// </summary>
        private readonly Stack<IEnumerable<ParameterExpression>> _environment = new();

        /// <summary>
        /// Visits the specified block expression's child expressions with the block expression's variables
        /// (if any) brought in scope.
        /// </summary>
        /// <param name="node">The block expression to visit.</param>
        /// <returns>The original expression.</returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            if (node.Variables.Count > 0)
            {
                _environment.Push(node.Variables);
            }

            Visit(node.Expressions);

            if (node.Variables.Count > 0)
            {
                _environment.Pop();
            }

            return node;
        }

        /// <summary>
        /// Visits the specified catch block's child expressions with the catch block's variable (if any)
        /// brought in scope.
        /// </summary>
        /// <param name="node">The block expression to visit.</param>
        /// <returns>The original node.</returns>
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            if (node.Variable != null)
            {
                _environment.Push(new[] { node.Variable });
            }

            Visit(node.Filter);
            Visit(node.Body);

            if (node.Variable != null)
            {
                _environment.Pop();
            }

            return node;
        }

        /// <summary>
        /// Visits the specified lambda expression's body with the lambda expression's variables (if any)
        /// brought in scope.
        /// </summary>
        /// <typeparam name="T">The type of the delegate represented by the lambda expression.</typeparam>
        /// <param name="node">The lambda expression to visit.</param>
        /// <returns>The original node.</returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (node.Parameters.Count > 0)
            {
                _environment.Push(node.Parameters);
            }

            Visit(node.Body);

            if (node.Parameters.Count > 0)
            {
                _environment.Pop();
            }

            return node;
        }

        /// <summary>
        /// Visits the specified parameter expression use site to determine whether the parameter occurs
        /// as a free variable or is bound by any of the nested scopes tracked by the visitor.
        /// </summary>
        /// <param name="node">The parameter expression to visit.</param>
        /// <returns>The original node.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            foreach (var scope in _environment)
            {
                foreach (var variable in scope)
                {
                    if (variable == node)
                    {
                        return node;
                    }
                }
            }

            FreeVariables.Add(node);

            return node;
        }
    }
}
