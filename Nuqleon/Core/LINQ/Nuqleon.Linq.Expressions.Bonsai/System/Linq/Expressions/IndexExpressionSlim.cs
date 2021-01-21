// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    // PERF: Consider introducing specialized layouts for common arities of 1 and 2 arguments.

    /// <summary>
    /// Lightweight representation of index expression tree nodes.
    /// </summary>
    public sealed class IndexExpressionSlim : ExpressionSlim, IArgumentProviderSlim
    {
        internal IndexExpressionSlim(ExpressionSlim instance, PropertyInfoSlim indexer, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            Indexer = indexer;
            Object = instance;
            Arguments = arguments;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Index;

        /// <summary>
        /// Gets the index arguments.
        /// </summary>
        public ReadOnlyCollection<ExpressionSlim> Arguments { get; }

        /// <summary>
        /// Gets the indexed property info.
        /// </summary>
        public PropertyInfoSlim Indexer { get; }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1720 // Identifier 'object' contains type name. (Mirroring expression tree APIs.)

        /// <summary>
        /// Gets the object to access indexed property.
        /// </summary>
        public ExpressionSlim Object { get; }

#pragma warning restore CA1720
#pragma warning restore IDE0079

        /// <summary>
        /// Gets the number of arguments.
        /// </summary>
        public int ArgumentCount => Arguments.Count;

        /// <summary>
        /// Gets the argument at the specified index.
        /// </summary>
        /// <param name="index">The index of the argument to retrieve.</param>
        /// <returns>The argument at the specified index.</returns>
        public ExpressionSlim GetArgument(int index) => Arguments[index];

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="instance">The <see cref="Object"/> child node of the result.</param>
        /// <param name="arguments">The <see cref="Arguments"/> child nodes of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public IndexExpressionSlim Update(ExpressionSlim instance, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            if (instance == Object && arguments == Arguments)
            {
                return this;
            }

            return new IndexExpressionSlim(instance, Indexer, arguments);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitIndex(this);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <typeparam name="TExpression">Target type for expressions.</typeparam>
        /// <typeparam name="TLambdaExpression">Target type for lambda expressions. This type has to derive from TExpression.</typeparam>
        /// <typeparam name="TParameterExpression">Target type for parameter expressions. This type has to derive from TExpression.</typeparam>
        /// <typeparam name="TNewExpression">Target type for new expressions. This type has to derive from TExpression.</typeparam>
        /// <typeparam name="TElementInit">Target type for element initializers.</typeparam>
        /// <typeparam name="TMemberBinding">Target type for member bindings.</typeparam>
        /// <typeparam name="TMemberAssignment">Target type for member assignments. This type has to derive from TMemberBinding.</typeparam>
        /// <typeparam name="TMemberListBinding">Target type for member list bindings. This type has to derive from TMemberBinding.</typeparam>
        /// <typeparam name="TMemberMemberBinding">Target type for member member bindings. This type has to derive from TMemberBinding.</typeparam>
        /// <typeparam name="TCatchBlock">Target type for catch blocks.</typeparam>
        /// <typeparam name="TSwitchCase">Target type for switch cases.</typeparam>
        /// <typeparam name="TLabelTarget">Target type for label targets.</typeparam>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override TExpression Accept<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>(ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> visitor)
        {
            return visitor.VisitIndex(this);
        }
    }
}
