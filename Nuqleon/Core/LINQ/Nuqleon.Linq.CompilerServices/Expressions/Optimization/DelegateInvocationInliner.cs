// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Provides an expression tree optimization by inlining invocations to delegates as method calls.
    /// </summary>
    public static class DelegateInvocationInliner
    {
        /// <summary>
        /// Inlines delegate invocations in the specified expression by using method call expressions.
        /// </summary>
        /// <param name="expression">Expression to inline delegate invocations for.</param>
        /// <param name="inlineNonPublicMethods">Indicates whether to inline non-public methods.</param>
        /// <returns>Optimized expression with delegate invocations inlined as method call expressions.</returns>
        public static Expression Apply(Expression expression, bool inlineNonPublicMethods)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var visitor = inlineNonPublicMethods ? Impl.InlineNonPublicMethods : Impl.NoInlineNonPublicMethods;

            return visitor.Visit(expression);
        }

        private sealed class Impl : ExpressionVisitor
        {
            public static readonly Impl NoInlineNonPublicMethods = new(inlineNonPublicMethods: false);
            public static readonly Impl InlineNonPublicMethods = new(inlineNonPublicMethods: true);

            private readonly bool _inlineNonPublicMethods;

            private Impl(bool inlineNonPublicMethods) => _inlineNonPublicMethods = inlineNonPublicMethods;

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                var expression = Visit(node.Expression);
                var arguments = Visit(node.Arguments);

                if (typeof(Delegate).IsAssignableFrom(expression.Type))
                {
                    if (expression is ConstantExpression constExpr)
                    {
                        var d = (Delegate)constExpr.Value;
                        if (d != null)
                        {
                            var i = d.GetInvocationList();
                            if (i.Length == 1)
                            {
                                var mtd = i[0].Method;

                                if (_inlineNonPublicMethods || mtd.IsPublic)
                                {
                                    var obj = mtd.IsStatic ? default(Expression) : Expression.Constant(d.Target, mtd.DeclaringType);

                                    return Expression.Call(obj, mtd, arguments);
                                }
                            }
                        }
                    }
                }

                return node.Update(expression, arguments);
            }
        }
    }
}
