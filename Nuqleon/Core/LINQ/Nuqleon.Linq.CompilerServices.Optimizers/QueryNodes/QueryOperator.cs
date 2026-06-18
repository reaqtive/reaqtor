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
    /// A representation of a query expression operator.
    /// </summary>
    /// <remarks>
    /// Creates a representation of a query expression operator.
    /// </remarks>
    /// <param name="elementType">The element type of the resulting monad.</param>
    public abstract class QueryOperator(Type elementType) : MonadMember(elementType)
    {

        /// <summary>
        /// Gets the <see cref="Optimizers.QueryNodeType"/> of the <see cref="QueryTree"/>.
        /// </summary>
        public sealed override QueryNodeType QueryNodeType => QueryNodeType.Operator;

        /// <summary>
        /// Gets the <see cref="OperatorType"/> of the <see cref="QueryOperator"/>.
        /// </summary>
        public abstract OperatorType NodeType { get; }

        /// <summary>
        /// The default query expression factory to use for query operators.
        /// </summary>
        public virtual IQueryExpressionFactory QueryExpressionFactory => DefaultQueryExpressionFactory.Instance;

        /// <summary>
        /// Reduces the current node to an equivalent <see cref="Expression"/>.
        /// </summary>
        /// <returns>This node's equivalent <see cref="Expression"/>.</returns>
        public override Expression Reduce()
        {
            throw new NotSupportedException("This operator does not reduce to an expression.");
        }
    }
}
