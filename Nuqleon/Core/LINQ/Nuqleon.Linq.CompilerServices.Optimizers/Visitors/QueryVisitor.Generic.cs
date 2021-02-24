// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.ObjectModel;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Query expression visitor to rewrite a query expression tree into a target type.
    /// </summary>
    /// <typeparam name="TQueryTree">Target type for query expressions.</typeparam>
    /// <typeparam name="TMonadMember">Target type for monad member expressions. This type has to derive from TQueryTree.</typeparam>
    /// <typeparam name="TQueryOperator">Target type for query operator expressions. This type has to derive from TMonadMember.</typeparam>
    public abstract class QueryVisitor<TQueryTree, TMonadMember, TQueryOperator>
        where TMonadMember : TQueryTree
        where TQueryOperator : TMonadMember
    {
        /// <summary>
        /// Visits the specified query expression and rewrites it to the target query expression type.
        /// </summary>
        /// <param name="expression">Query expression to visit.</param>
        /// <returns>Result of visiting the query expression.</returns>
        public virtual TQueryTree Visit(QueryTree expression)
        {
            if (expression == null)
            {
                return default;
            }

            return expression.Accept(this);
        }

        /// <summary>
        /// Visits the specified query expression and rewrites it to the specified target query expression type.
        /// </summary>
        /// <typeparam name="TStronglyTypedResult">Type of the result of the rewrite. This type should derive from TQueryTree.</typeparam>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected TStronglyTypedResult VisitAndConvert<TStronglyTypedResult>(QueryTree node)
            where TStronglyTypedResult : TQueryTree
        {
            return (TStronglyTypedResult)Visit(node);
        }

        /// <summary>
        /// Visits the elements in the specified input collection.
        /// </summary>
        /// <typeparam name="TSource">Element type in the input collection.</typeparam>
        /// <typeparam name="TResult">Element type in the result collection.</typeparam>
        /// <param name="nodes">Input collection whose elements to visit.</param>
        /// <param name="visitor">Function to visit elements in the input collection.</param>
        /// <returns>Collection of visited input elements.</returns>
        protected ReadOnlyCollection<TResult> Visit<TSource, TResult>(ReadOnlyCollection<TSource> nodes, Func<TSource, TResult> visitor)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            var n = nodes.Count;

            var res = new TResult[n];

            for (var i = 0; i < n; i++)
            {
                res[i] = visitor(nodes[i]);
            }

            return new TrueReadOnlyCollection<TResult>(/* transfer ownership */ res);
        }

        /// <summary>
        /// Visits a <see cref="FirstOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual TQueryOperator VisitFirst(FirstOperator op)
        {
            var src = VisitAndConvert<TMonadMember>(op.Source);
            return MakeFirst(op, src);
        }

        /// <summary>
        /// Makes a <see cref="FirstOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected abstract TQueryOperator MakeFirst(FirstOperator node, TMonadMember source);

        /// <summary>
        /// Visits a <see cref="FirstPredicateOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual TQueryOperator VisitFirstPredicate(FirstPredicateOperator op)
        {
            var src = VisitAndConvert<TMonadMember>(op.Source);
            var pred = Visit(op.Predicate);
            return MakeFirstPredicate(op, src, pred);
        }

        /// <summary>
        /// Makes a <see cref="FirstPredicateOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="predicate">Predicate query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected abstract TQueryOperator MakeFirstPredicate(FirstPredicateOperator node, TMonadMember source, TQueryTree predicate);

        /// <summary>
        /// Visits a <see cref="LambdaAbstraction" /> node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual TQueryTree VisitLambdaAbstraction(LambdaAbstraction node)
        {
            var args = Visit(node.Parameters, Visit);

            return MakeLambdaAbstraction(node, args);
        }

        /// <summary>
        /// Makes a <see cref="LambdaAbstraction" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="arguments">Argument query expressions.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected abstract TQueryTree MakeLambdaAbstraction(LambdaAbstraction node, ReadOnlyCollection<TQueryTree> arguments);

        /// <summary>
        /// Visits a <see cref="MonadAbstraction" /> node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual TMonadMember VisitMonadAbstraction(MonadAbstraction node)
        {
            var inner = Visit(node.Inner);
            return MakeMonadAbstraction(node, inner);
        }

        /// <summary>
        /// Makes a <see cref="MonadAbstraction" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="inner">Inner query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected abstract TMonadMember MakeMonadAbstraction(MonadAbstraction node, TQueryTree inner);

        /// <summary>
        /// Visits a <see cref="SelectOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual TQueryOperator VisitSelect(SelectOperator op)
        {
            var src = VisitAndConvert<TMonadMember>(op.Source);
            var selector = Visit(op.Selector);
            return MakeSelect(op, src, selector);
        }

        /// <summary>
        /// Makes a <see cref="SelectOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="selector">Selector query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected abstract TQueryOperator MakeSelect(SelectOperator node, TMonadMember source, TQueryTree selector);

        /// <summary>
        /// Visits a <see cref="TakeOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual TQueryOperator VisitTake(TakeOperator op)
        {
            var src = VisitAndConvert<TMonadMember>(op.Source);
            var count = Visit(op.Count);
            return MakeTake(op, src, count);
        }

        /// <summary>
        /// Makes a <see cref="TakeOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="count">Count query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected abstract TQueryOperator MakeTake(TakeOperator node, TMonadMember source, TQueryTree count);

        /// <summary>
        /// Visits a <see cref="WhereOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal virtual TQueryOperator VisitWhere(WhereOperator op)
        {
            var src = VisitAndConvert<TMonadMember>(op.Source);
            var pred = Visit(op.Predicate);
            return MakeWhere(op, src, pred);
        }

        /// <summary>
        /// Makes a <see cref="WhereOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="predicate">Predicate query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected abstract TQueryOperator MakeWhere(WhereOperator node, TMonadMember source, TQueryTree predicate);
    }
}
