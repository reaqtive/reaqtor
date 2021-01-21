// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Binder for known resources to their service-side implementation.
//
// BD - September 2014
//

using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Binder for known resources that occur as unbound parameter expressions in an expression.
    /// </summary>
    internal class Binder : ExpressionVisitor
    {
        /// <summary>
        /// Registry used to look up known resources.
        /// </summary>
        private readonly Registry _registry;

        /// <summary>
        /// Creates a new binder that uses the specified registry to look up known resources.
        /// </summary>
        /// <param name="registry">Registry used to look up known resources.</param>
        public Binder(Registry registry)
        {
            _registry = registry;
        }

        /// <summary>
        /// Analyzes invocation expressions for the typical pattern of invoking a known resource. If a known resource is found, it gets bound using the definition in the registry.
        /// </summary>
        /// <param name="node">Expression to analyze.</param>
        /// <returns>Original expression if the invocation doesn't use a known resource; otherwise, the bound equivalent of the original expression.</returns>
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            if (node.Expression is ParameterExpression p) // omitted unbound parameter check
            {
                var e = _registry[p.Name].Expression;

                var u = new TypeUnifier();
                u.Unify(p.Type, e.Type);

                if (u.Bindings.Count != 0)
                {
                    // assuming wildcards are on the left
                    e = new TypeSubstitutionExpressionVisitor(u.Bindings).Visit(e);
                }

                return Expression.Invoke(e, Visit(node.Arguments)); // omited beta reduction
            }

            return base.VisitInvocation(node);
        }
    }
}
