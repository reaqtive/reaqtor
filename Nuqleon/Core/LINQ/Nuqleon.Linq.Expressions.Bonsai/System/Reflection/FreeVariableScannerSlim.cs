// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Reflection
{
    /// <summary>
    /// Visits an expression tree to find all the free variables.
    /// </summary>
    public static class FreeVariableScannerSlim
    {
        /// <summary>
        /// Finds all the free variables in an expression.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <returns>The set of free variables.</returns>
        public static HashSet<ParameterExpressionSlim> Find(ExpressionSlim expression)
        {
            var impl = new Impl();
            impl.Visit(expression);
            return impl.Globals;
        }

        private sealed class Impl : ExpressionSlimVisitor
        {
            public readonly HashSet<ParameterExpressionSlim> Globals;

            // PERF: Consider the use of other collection types for small environment frames
            //       and measure impact. A possible choice is ICollection<T> and use a simple
            //       array for VisitCatchBlock. Cost of virtual method calls has to be assessed.

            private readonly Stack<HashSet<ParameterExpressionSlim>> _environment;

            public Impl()
            {
                Globals = new HashSet<ParameterExpressionSlim>();

                _environment = new Stack<HashSet<ParameterExpressionSlim>>();
                _environment.Push(Globals);
            }

            protected internal override ExpressionSlim VisitBlock(BlockExpressionSlim node)
            {
                if (node.Variables.Count > 0)
                {
                    var frame = new HashSet<ParameterExpressionSlim>(node.Variables);
                    _environment.Push(frame);
                }

                var res = base.VisitBlock(node);

                if (node.Variables.Count > 0)
                {
                    _environment.Pop();
                }

                return res;
            }

            protected internal override CatchBlockSlim VisitCatchBlock(CatchBlockSlim node)
            {
                if (node.Variable != null)
                {
                    _environment.Push(new HashSet<ParameterExpressionSlim> { node.Variable });
                }

                var res = base.VisitCatchBlock(node);

                if (node.Variable != null)
                {
                    _environment.Pop();
                }

                return res;
            }

            protected internal override ExpressionSlim VisitLambda(LambdaExpressionSlim node)
            {
                if (node.Parameters.Count > 0)
                {
                    var frame = new HashSet<ParameterExpressionSlim>(node.Parameters);
                    _environment.Push(frame);
                }

                var res = base.VisitLambda(node);

                if (node.Parameters.Count > 0)
                {
                    _environment.Pop();
                }

                return res;
            }

            protected internal override ExpressionSlim VisitParameter(ParameterExpressionSlim node)
            {
                foreach (var frame in _environment)
                {
                    if (frame.Contains(node))
                    {
                        return node;
                    }
                }

                Globals.Add(node);

                return node;
            }
        }
    }
}
