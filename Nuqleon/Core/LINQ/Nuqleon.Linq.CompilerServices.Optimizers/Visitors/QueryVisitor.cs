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
    /// Query expression visitor to rewrite a query expression tree.
    /// </summary>
    public class QueryVisitor : QueryVisitor<QueryTree, MonadMember, QueryOperator>
    {
        /// <summary>
        /// Visits a <see cref="FirstOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitFirst(FirstOperator op)
        {
            var src = VisitAndConvert<MonadMember>(op.Source);
            if (src != op.Source)
            {
                return MakeFirst(op, src);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="FirstOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected sealed override QueryOperator MakeFirst(FirstOperator node, MonadMember source)
        {
            return node.QueryExpressionFactory.First(node.ElementType, source);
        }


        /// <summary>
        /// Visits a <see cref="FirstPredicateOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitFirstPredicate(FirstPredicateOperator op)
        {
            var src = VisitAndConvert<MonadMember>(op.Source);
            var pred = Visit(op.Predicate);

            if (src != op.Source || pred != op.Predicate)
            {
                return MakeFirstPredicate(op, src, pred);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="FirstPredicateOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="predicate">Predicate query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected sealed override QueryOperator MakeFirstPredicate(FirstPredicateOperator node, MonadMember source, QueryTree predicate)
        {
            return node.QueryExpressionFactory.First(node.ElementType, source, predicate);
        }

        /// <summary>
        /// Visits a <see cref="LambdaAbstraction" /> node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryTree VisitLambdaAbstraction(LambdaAbstraction node)
        {
            // This is overridden so we can use the nongeneric Visit function on ReadOnlyCollection which
            // will perform an element-wise equality check to see if the collection has changed
            var args = Visit(node.Parameters);

            if (args != node.Parameters)
            {
                return MakeLambdaAbstraction(node, args);
            }

            return node;
        }

        /// <summary>
        /// Makes a <see cref="LambdaAbstraction" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="arguments">Argument query expressions.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected sealed override QueryTree MakeLambdaAbstraction(LambdaAbstraction node, ReadOnlyCollection<QueryTree> arguments)
        {
            return DefaultQueryExpressionFactory.Instance.LambdaAbstraction(node.Body, arguments);
        }

        /// <summary>
        /// Visits a <see cref="MonadAbstraction" /> node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override MonadMember VisitMonadAbstraction(MonadAbstraction node)
        {
            var inner = Visit(node.Inner);

            if (inner != node.Inner)
            {
                return MakeMonadAbstraction(node, inner);
            }

            return node;
        }

        /// <summary>
        /// Makes a <see cref="MonadAbstraction" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="inner">Inner query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected sealed override MonadMember MakeMonadAbstraction(MonadAbstraction node, QueryTree inner)
        {
            return DefaultQueryExpressionFactory.Instance.MonadAbstraction(node.ElementType, inner);
        }

        /// <summary>
        /// Visits a <see cref="TakeOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitTake(TakeOperator op)
        {
            var src = VisitAndConvert<MonadMember>(op.Source);
            var count = Visit(op.Count);

            if (src != op.Source || count != op.Count)
            {
                return MakeTake(op, src, count);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="TakeOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="count">Count query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected sealed override QueryOperator MakeTake(TakeOperator node, MonadMember source, QueryTree count)
        {
            return node.QueryExpressionFactory.Take(node.ElementType, source, count);
        }

        /// <summary>
        /// Visits a <see cref="SelectOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitSelect(SelectOperator op)
        {
            var src = VisitAndConvert<MonadMember>(op.Source);
            var selector = Visit(op.Selector);

            if (src != op.Source || selector != op.Selector)
            {
                return MakeSelect(op, src, selector);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="SelectOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="selector">Selector query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected sealed override QueryOperator MakeSelect(SelectOperator node, MonadMember source, QueryTree selector)
        {
            return node.QueryExpressionFactory.Select(node.ElementType, node.InputElementType, source, selector);
        }

        /// <summary>
        /// Visits a <see cref="WhereOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitWhere(WhereOperator op)
        {
            var src = VisitAndConvert<MonadMember>(op.Source);
            var pred = Visit(op.Predicate);

            if (src != op.Source || pred != op.Predicate)
            {
                return MakeWhere(op, src, pred);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="WhereOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="predicate">Predicate query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected sealed override QueryOperator MakeWhere(WhereOperator node, MonadMember source, QueryTree predicate)
        {
            return node.QueryExpressionFactory.Where(node.ElementType, source, predicate);
        }

        /// <summary>
        /// Visits the elements in the specified input collection.
        /// </summary>
        /// <param name="nodes">Input collection whose elements to visit.</param>
        /// <returns>Collection of visited input elements.</returns>
        protected ReadOnlyCollection<QueryTree> Visit(ReadOnlyCollection<QueryTree> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            var res = default(QueryTree[]);

            var n = nodes.Count;
            for (int i = 0; i < n; i++)
            {
                var oldNode = nodes[i];
                var newNode = Visit(oldNode);

                if (res != null)
                {
                    res[i] = newNode;
                }
                else
                {
                    if (!object.ReferenceEquals(oldNode, newNode))
                    {
                        res = new QueryTree[n];

                        for (int j = 0; j < i; j++)
                        {
                            res[j] = nodes[j];
                        }

                        res[i] = newNode;
                    }
                }
            }

            if (res != null)
            {
                return new TrueReadOnlyCollection<QueryTree>(/* transfer ownership */ res);
            }

            return nodes;
        }
    }
}
