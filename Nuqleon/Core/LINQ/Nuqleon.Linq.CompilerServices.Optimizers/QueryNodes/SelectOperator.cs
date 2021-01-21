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
    /// A representation of a select query expression operator.
    /// </summary>
    public class SelectOperator : QueryOperator
    {
        /// <summary>
        /// Creates a representation of a select query expression operator.
        /// </summary>
        /// <param name="elementType">The element type of the resulting monad.</param>
        /// <param name="inputElementType">The input type of the source monad.</param>
        /// <param name="source">The source to map from.</param>
        /// <param name="selector">The projection function used to map from the source.</param>
        protected internal SelectOperator(Type elementType, Type inputElementType, MonadMember source, QueryTree selector)
            : base(elementType)
        {
            InputElementType = inputElementType;
            Source = source;
            Selector = selector;
        }

        /// <summary>
        /// Gets the <see cref="OperatorType"/> of the <see cref="QueryOperator"/>.
        /// </summary>
        public override OperatorType NodeType => OperatorType.Select;

        /// <summary>
        /// The source on which this operator acts.
        /// </summary>
        public MonadMember Source { get; }

        /// <summary>
        /// The projection function used to obtain elements from the source monad.
        /// </summary>
        public QueryTree Selector { get; }

        /// <summary>
        /// The element type of the source monad.
        /// </summary>
        public Type InputElementType { get; }

        /// <summary>
        /// Creates a select operator query expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="source">The <see cref="Source"/> child node of the result.</param>
        /// <param name="selector">The <see cref="Selector"/> child node of the result.</param>
        /// <returns>This query expression if no children are changed or an expression with the updated children.</returns>
        public SelectOperator Update(MonadMember source, QueryTree selector)
        {
            if (source == Source && selector == Selector)
            {
                return this;
            }

            return QueryExpressionFactory.Select(ElementType, InputElementType, source, selector);
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
            return visitor.VisitSelect(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079
    }
}
