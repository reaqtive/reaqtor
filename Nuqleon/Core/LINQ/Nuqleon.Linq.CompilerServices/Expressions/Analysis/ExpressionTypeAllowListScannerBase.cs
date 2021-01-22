// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Base class for expression visitors used to check uses of type members against a list of allowed types.
    /// </summary>
    public abstract class ExpressionTypeAllowListScannerBase : ExpressionVisitor
    {
        /// <summary>
        /// Checks whether the expression's type is on the list of allowed types.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        public override Expression Visit(Expression node)
        {
            if (node != null)
            {
                if (!Check(node.Type))
                {
                    return ResolveExpression(node, node.Type, base.Visit);
                }
            }

            return base.Visit(node);
        }

        /// <summary>
        /// Checks whether the specified type is supported.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if the type is supported; otherwise, false.</returns>
        protected abstract bool Check(Type type);

        /// <summary>
        /// Resolves an expression whose type was rejected by the checker.
        /// </summary>
        /// <typeparam name="T">Type the expression to resolve.</typeparam>
        /// <param name="expression">Expression to resolve.</param>
        /// <param name="type">Type that was rejected.</param>
        /// <param name="visit">Function to continue the visit of the node.</param>
        /// <returns>Resolved expression, or an exception. By default, the method throws a NotSupportedException.</returns>
        /// <exception cref="NotSupportedException">The specified expression was rejected.</exception>
        protected virtual Expression ResolveExpression<T>(T expression, Type type, Func<T, Expression> visit)
            where T : Expression
        {
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Expression '{0}' uses '{1}' which is not allowed.", expression.ToCSharpString(), type.ToCSharpString()));
        }
    }
}
