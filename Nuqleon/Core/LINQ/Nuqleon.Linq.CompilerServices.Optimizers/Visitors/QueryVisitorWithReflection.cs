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
using System.Linq.Expressions;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Query expression visitor with visit methods for reflection objects.
    /// </summary>
    public class QueryVisitorWithReflection : QueryVisitor
    {
        /// <summary>
        /// Visits a <see cref="FirstOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitFirst(FirstOperator op)
        {
            var elementType = VisitType(op.ElementType);
            var src = VisitAndConvert<MonadMember>(op.Source);

            if (src != op.Source || elementType != op.ElementType)
            {
                return MakeFirst(op, elementType, src);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="FirstOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="elementType">Element type for the resulting operator.</param>
        /// <param name="source">Source query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected virtual QueryOperator MakeFirst(FirstOperator node, Type elementType, MonadMember source)
        {
            return node.QueryExpressionFactory.First(elementType, source);
        }

        /// <summary>
        /// Visits a <see cref="FirstPredicateOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitFirstPredicate(FirstPredicateOperator op)
        {
            var elementType = VisitType(op.ElementType);
            var src = VisitAndConvert<MonadMember>(op.Source);
            var pred = Visit(op.Predicate);

            if (src != op.Source || pred != op.Predicate || elementType != op.ElementType)
            {
                return MakeFirstPredicate(op, elementType, src, pred);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="FirstPredicateOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="elementType">Element type for the resulting operator.</param>
        /// <param name="predicate">Predicate query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected virtual QueryOperator MakeFirstPredicate(FirstPredicateOperator node, Type elementType, MonadMember source, QueryTree predicate)
        {
            return node.QueryExpressionFactory.First(elementType, source, predicate);
        }

        /// <summary>
        /// Visits a <see cref="LambdaAbstraction" /> node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryTree VisitLambdaAbstraction(LambdaAbstraction node)
        {
            var body = VisitLambdaAbstractionBody(node.Body);
            var args = Visit(node.Parameters);

            if (body != node.Body || args != node.Parameters)
            {
                return MakeLambdaAbstraction(node, body, args);
            }

            return node;
        }

        /// <summary>
        /// Makes a <see cref="LambdaAbstraction" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="body">Body expression.</param>
        /// <param name="arguments">Argument query expressions.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected virtual QueryTree MakeLambdaAbstraction(LambdaAbstraction node, LambdaExpression body, ReadOnlyCollection<QueryTree> arguments)
        {
            return DefaultQueryExpressionFactory.Instance.LambdaAbstraction(body, arguments);
        }

        /// <summary>
        /// Visits a <see cref="MonadAbstraction" /> node.
        /// </summary>
        /// <param name="node">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override MonadMember VisitMonadAbstraction(MonadAbstraction node)
        {
            var elementType = VisitType(node.ElementType);
            var inner = Visit(node.Inner);

            if (inner != node.Inner || elementType != node.ElementType)
            {
                return MakeMonadAbstraction(node, elementType, inner);
            }

            return node;
        }

        /// <summary>
        /// Makes a <see cref="MonadAbstraction" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="elementType">Element type for the resulting operator.</param>
        /// <param name="inner">Inner query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected virtual MonadMember MakeMonadAbstraction(MonadAbstraction node, Type elementType, QueryTree inner)
        {
            return DefaultQueryExpressionFactory.Instance.MonadAbstraction(elementType, inner);
        }

        /// <summary>
        /// Visits a <see cref="SelectOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitSelect(SelectOperator op)
        {
            var elementType = VisitType(op.ElementType);
            var inputElementType = VisitType(op.InputElementType);
            var src = VisitAndConvert<MonadMember>(op.Source);
            var selector = Visit(op.Selector);

            if (src != op.Source || selector != op.Selector || elementType != op.ElementType || inputElementType != op.InputElementType)
            {
                return MakeSelect(op, elementType, inputElementType, src, selector);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="SelectOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="elementType">Element type for the resulting operator.</param>
        /// <param name="inputElementType">Input element type for the resulting operator.</param>
        /// <param name="selector">Selector query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected virtual QueryOperator MakeSelect(SelectOperator node, Type elementType, Type inputElementType, MonadMember source, QueryTree selector)
        {
            return node.QueryExpressionFactory.Select(elementType, inputElementType, source, selector);
        }

        /// <summary>
        /// Visits a <see cref="TakeOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitTake(TakeOperator op)
        {
            var elementType = VisitType(op.ElementType);
            var src = VisitAndConvert<MonadMember>(op.Source);
            var count = Visit(op.Count);

            if (src != op.Source || count != op.Count || elementType != op.ElementType)
            {
                return MakeTake(op, elementType, src, count);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="TakeOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="elementType">Element type for the resulting operator.</param>
        /// <param name="count">Count query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected virtual QueryOperator MakeTake(TakeOperator node, Type elementType, MonadMember source, QueryTree count)
        {
            return node.QueryExpressionFactory.Take(elementType, source, count);
        }

        /// <summary>
        /// Visits a <see cref="WhereOperator" /> node.
        /// </summary>
        /// <param name="op">Node to visit.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override QueryOperator VisitWhere(WhereOperator op)
        {
            var elementType = VisitType(op.ElementType);
            var src = VisitAndConvert<MonadMember>(op.Source);
            var pred = Visit(op.Predicate);

            if (src != op.Source || pred != op.Predicate || elementType != op.ElementType)
            {
                return MakeWhere(op, elementType, src, pred);
            }

            return op;
        }

        /// <summary>
        /// Makes a <see cref="WhereOperator" /> with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="elementType">Element type for the resulting operator.</param>
        /// <param name="predicate">Predicate query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected virtual QueryOperator MakeWhere(WhereOperator node, Type elementType, MonadMember source, QueryTree predicate)
        {
            return node.QueryExpressionFactory.Where(elementType, source, predicate);
        }

        /// <summary>
        /// Visits an expression which is the body of a lambda abstraction.
        /// </summary>
        /// <param name="body">The expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected virtual LambdaExpression VisitLambdaAbstractionBody(LambdaExpression body) => body;

        /// <summary>
        /// Visits a type.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual Type VisitType(Type type) => type;
    }
}
