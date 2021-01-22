// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Diagnostics;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// A representation of a query expression as a tree.
    /// </summary>
    [DebuggerDisplay("{new QueryDebugger().Visit(this)}")]
    public abstract class QueryTree
    {
        /// <summary>
        /// Gets the <see cref="Optimizers.QueryNodeType"/> of the <see cref="QueryTree"/>.
        /// </summary>
        public abstract QueryNodeType QueryNodeType { get; }

        /// <summary>
        /// Accepts the query expression tree node in the specified visitor.
        /// </summary>
        /// <typeparam name="TQueryTree">Target type for query expressions.</typeparam>
        /// <typeparam name="TMonadMember">Target type for monad member query expressions. This type has to derive from TQueryTree.</typeparam>
        /// <typeparam name="TQueryOperator">Target type for query operator query expressions. This type has to derive from TMonadMember.</typeparam>
        /// <param name="visitor">Visitor to process the current query expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal abstract TQueryTree Accept<TQueryTree, TMonadMember, TQueryOperator>(QueryVisitor<TQueryTree, TMonadMember, TQueryOperator> visitor)
            where TMonadMember : TQueryTree
            where TQueryOperator : TMonadMember;

        /// <summary>
        /// Reduces the current node to an equivalent <see cref="Expression"/>.
        /// </summary>
        /// <returns>This node's equivalent <see cref="Expression"/>.</returns>
        public abstract Expression Reduce();
    }
}
