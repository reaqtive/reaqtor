// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// A representation of the unknown parts of the query expression tree.
    /// </summary>
    public sealed class LambdaAbstraction : QueryTree
    {
        /// <summary>
        /// Create a representation of the unknown parts of the query expression tree.
        /// </summary>
        /// <param name="body">The unknown parts of the query expression tree.</param>
        /// <param name="parameters">The known sub-parts of the tree.</param>
        internal LambdaAbstraction(LambdaExpression body, ReadOnlyCollection<QueryTree> parameters)
        {
            Body = body;
            Parameters = parameters;
        }

        /// <summary>
        /// The <see cref="Optimizers.QueryNodeType"/> of the <see cref="QueryTree"/>.
        /// </summary>
        public override QueryNodeType QueryNodeType => QueryNodeType.Lambda;

        /// <summary>
        /// The unknown parts of the query expression, abstracted out as a lambda expression.
        /// </summary>
        public LambdaExpression Body { get; }

        /// <summary>
        /// The known sub-parts of the query expression.
        /// </summary>
        public ReadOnlyCollection<QueryTree> Parameters { get; }

        /// <summary>
        /// Creates a lambda abstraction query expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="body">The <see cref="Body"/> child node of the result.</param>
        /// <param name="parameters">The <see cref="Parameters"/> child node of the result.</param>
        /// <returns>This query expression if no children are changed or an expression with the updated children.</returns>
        public LambdaAbstraction Update(LambdaExpression body, IEnumerable<QueryTree> parameters)
        {
            if (body == Body && parameters == Parameters)
            {
                return this;
            }

            return DefaultQueryExpressionFactory.Instance.LambdaAbstraction(body, parameters.ToReadOnly());
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
            return visitor.VisitLambdaAbstraction(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079

        /// <summary>
        /// Reduces the current node to an equivalent <see cref="System.Linq.Expressions.Expression"/>.
        /// </summary>
        /// <returns>This node's equivalent <see cref="System.Linq.Expressions.Expression"/>.</returns>
        public override Expression Reduce()
        {
            var cnt = Parameters.Count;
            var reducedParameters = new Expression[cnt];
            for (var i = 0; i < cnt; i++)
            {
                reducedParameters[i] = Parameters[i].Reduce();
            }

            // The first reduction is to fill in the holes that the converter created
            var reduced = Body;
            for (var i = 0; i < cnt; i++)
            {
                // TODO Make replacement visitor take multiple replacements
                reduced = Expression.Lambda(new ReplacementVisitor(reduced.Parameters[i], reducedParameters[i]).Visit(reduced.Body), reduced.Parameters);
            }

            var body = reduced.Body;

            // After filling in the holes, we might get new opportunities for reductions, so go ahead and do that.
            // TODO this step won't be needed if we maintain the invariant that no LambdaAbstraction nodes have LambdaAbstraction subnodes.
            return BetaReducer.Reduce(body, BetaReductionNodeTypes.Atoms, BetaReductionRestrictions.None);
        }
    }
}
