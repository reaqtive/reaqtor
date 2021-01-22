// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// A representation of a take query expression operator.
    /// </summary>
    public class TakeOperator : QueryOperator
    {
        /// <summary>
        /// Creates a representation of a take query expression operator.
        /// </summary>
        /// <param name="elementType">The element type of the resulting monad.</param>
        /// <param name="source">The source to take from.</param>
        /// <param name="count">The number of elements to take.</param>
        protected internal TakeOperator(Type elementType, MonadMember source, QueryTree count)
            : base(elementType)
        {
            Source = source;
            Count = count;
        }

        /// <summary>
        /// Gets the <see cref="OperatorType"/> of the <see cref="QueryOperator"/>.
        /// </summary>
        public override OperatorType NodeType => OperatorType.Take;

        /// <summary>
        /// The source on which this operator acts.
        /// </summary>
        public MonadMember Source { get; }

        /// <summary>
        /// The number of items to take from the source monad.
        /// </summary>
        public QueryTree Count { get; }

        /// <summary>
        /// Creates a take operator query expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="source">The <see cref="Source"/> child node of the result.</param>
        /// <param name="count">The <see cref="Count"/> child node of the result.</param>
        /// <returns>This query expression if no children are changed or an expression with the updated children.</returns>
        public TakeOperator Update(MonadMember source, QueryTree count)
        {
            if (source == Source && count == Count)
            {
                return this;
            }

            return QueryExpressionFactory.Take(ElementType, source, count);
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
            return visitor.VisitTake(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079
    }
}
