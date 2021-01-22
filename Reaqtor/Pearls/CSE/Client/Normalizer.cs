// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Simple expression visitor to replace method call expressions by known resource invocation expressions.
//
// BD - September 2014
//

using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Normalizer for expressions using identifiers to represent accesses to known resources.
    /// </summary>
    internal class Normalizer : ExpressionVisitor
    {
        // NOTE: property accessor analysis omitted for brevity

        /// <summary>
        /// Analyzes method calls for known resource identifiers.
        /// </summary>
        /// <param name="node">Method call expression to analayze.</param>
        /// <returns>The original expression if the method does not represent a known resource; otherwise, an invocation expression using an unbound parameter expression representing the known resource.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var known = node.Method.GetCustomAttribute<KnownResourceAttribute>();
            if (known != null)
            {
                var type = Expression.GetFuncType(node.Arguments.Select(a => a.Type).Concat(new[] { node.Type }).ToArray());
                return Expression.Invoke(Expression.Parameter(type, known.Id), Visit(node.Arguments));
            }

            return base.VisitMethodCall(node);
        }
    }
}
