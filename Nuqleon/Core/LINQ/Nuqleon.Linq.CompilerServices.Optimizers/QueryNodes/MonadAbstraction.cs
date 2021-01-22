// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// A representation of a query tree which is a monad member.
    /// </summary>
    public sealed class MonadAbstraction : MonadMember
    {
        /// <summary>
        /// Creates a monad abstraction.
        /// </summary>
        /// <param name="elementType">The element type of the monad.</param>
        /// <param name="inner">The query tree this abstraction wraps.</param>
        internal MonadAbstraction(Type elementType, QueryTree inner)
            : base(elementType)
        {
            Inner = inner;
        }

        /// <summary>
        /// Gets the <see cref="Optimizers.QueryNodeType"/> of the <see cref="QueryTree"/>.
        /// </summary>
        public override QueryNodeType QueryNodeType => QueryNodeType.MonadAbstraction;

        /// <summary>
        /// The query tree this object wraps.
        /// </summary>
        public QueryTree Inner { get; }

        /// <summary>
        /// Creates a monad abstraction query expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="inner">The <see cref="Inner"/> child node of the result.</param>
        /// <returns>This query expression if no children are changed or an expression with the updated children.</returns>
        public MonadAbstraction Update(QueryTree inner)
        {
            if (inner == Inner)
            {
                return this;
            }

            return DefaultQueryExpressionFactory.Instance.MonadAbstraction(ElementType, inner);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks. (Similar to expression tree visitor implementation pattern.)

        /// <summary>
        /// Accepts the query expression tree node in the specified visitor.
        /// </summary>
        /// <typeparam name="TQueryTree">Target type for query expressions.</typeparam>
        /// <typeparam name="TMonadMember">Target type for monad member query expressions. This type has to derive from TQueryTree.</typeparam>
        /// <typeparam name="TQueryOperator">Target type for query operator query expressions. This type has to derive from TMonadMember.</typeparam>
        /// <param name="visitor">Visitor to process the current query expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override TQueryTree Accept<TQueryTree, TMonadMember, TQueryOperator>(QueryVisitor<TQueryTree, TMonadMember, TQueryOperator> visitor)
        {
            return visitor.VisitMonadAbstraction(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079

        /// <summary>
        /// Reduces the current node to an equivalent <see cref="Expression"/>.
        /// </summary>
        /// <returns>This node's equivalent <see cref="Expression"/>.</returns>
        public sealed override Expression Reduce() => Inner.Reduce();
    }
}
