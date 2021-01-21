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
    /// Default factory to create query expressions.
    /// </summary>
    public class DefaultQueryExpressionFactory : QueryExpressionFactoryBase, IQueryExpressionFactory
    {
        /// <summary>
        /// Constructs a default query expression factory.
        /// </summary>
        protected DefaultQueryExpressionFactory()
        {
        }

        /// <summary>
        /// Gets an instance of the default query expression factory.
        /// </summary>
        public static DefaultQueryExpressionFactory Instance { get; } = new DefaultQueryExpressionFactory();

        /// <summary>
        /// Creates a <see cref="FirstOperator" /> that represents a first operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad from which to get the first element.</param>
        /// <returns>A <see cref="FirstOperator" /> that has the given source.</returns>
        public override FirstOperator First(Type elementType, MonadMember source)
        {
            RequireNotNull(elementType, nameof(elementType));
            RequireNotNull(source, nameof(source));

            RequireTypesAssignable(elementType, source.ElementType, nameof(elementType));

            return MakeFirst(elementType, source);
        }

        /// <summary>
        /// Creates a <see cref="FirstOperator" /> that represents a first operator using type inference where needed.
        /// </summary>
        /// <param name="source">The monad from which to get the first element.</param>
        /// <returns>A <see cref="FirstOperator" /> that has the given source.</returns>
        public FirstOperator First(MonadMember source)
        {
            RequireNotNull(source, nameof(source));

            return First(source.ElementType, source);
        }

        /// <summary>
        /// Creates a <see cref="FirstOperator" /> that represents a first operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad from which to get the first element.</param>
        /// <returns>A <see cref="FirstOperator" /> that has the given source.</returns>
        protected virtual FirstOperator MakeFirst(Type elementType, MonadMember source)
        {
            return new FirstOperator(elementType, source);
        }

        /// <summary>
        /// Creates a <see cref="FirstPredicateOperator" /> that represents a first operator with a predicate.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad from which to get the first element satisfying a condition.</param>
        /// <param name="predicate">The function to test each source element for a condition.</param>
        /// <returns>A <see cref="FirstPredicateOperator" /> that has the given source.</returns>
        public override FirstPredicateOperator First(Type elementType, MonadMember source, QueryTree predicate)
        {
            RequireNotNull(elementType, nameof(elementType));
            RequireNotNull(source, nameof(source));
            RequireNotNull(predicate, nameof(predicate));

            RequireTypesAssignable(elementType, source.ElementType, nameof(elementType));

            return MakeFirstPredicate(elementType, source, predicate);
        }

        /// <summary>
        /// Creates a <see cref="FirstPredicateOperator" /> that represents a first operator with a predicate using type inference where needed.
        /// </summary>
        /// <param name="source">The monad from which to get the first element satisfying a condition.</param>
        /// <param name="predicate">The function to test each source element for a condition.</param>
        /// <returns>A <see cref="FirstPredicateOperator" /> that has the given source.</returns>
        public FirstPredicateOperator First(MonadMember source, QueryTree predicate)
        {
            RequireNotNull(source, nameof(source));

            return First(source.ElementType, source, predicate);
        }

        /// <summary>
        /// Creates a <see cref="FirstPredicateOperator" /> that represents a first operator with a predicate.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad from which to get the first element satisfying a condition.</param>
        /// <param name="predicate">The function to test each source element for a condition.</param>
        /// <returns>A <see cref="FirstPredicateOperator" /> that has the given source.</returns>
        protected virtual FirstPredicateOperator MakeFirstPredicate(Type elementType, MonadMember source, QueryTree predicate)
        {
            return new FirstPredicateOperator(elementType, source, predicate);
        }

        /// <summary>
        /// Creates a <see cref="SelectOperator" /> that represents a select operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="inputElementType">The <see cref="Type" /> of the elements in the source monad.</param>
        /// <param name="source">The monad to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        /// <returns>A <see cref="SelectOperator" /> that has the given source.</returns>
        public override SelectOperator Select(Type elementType, Type inputElementType, MonadMember source, QueryTree selector)
        {
            RequireNotNull(elementType, nameof(elementType));
            RequireNotNull(inputElementType, nameof(inputElementType));
            RequireNotNull(source, nameof(source));
            RequireNotNull(selector, nameof(selector));

            RequireTypesAssignable(inputElementType, source.ElementType, nameof(elementType));

            return MakeSelect(elementType, inputElementType, source, selector);
        }

        /// <summary>
        /// Creates a <see cref="SelectOperator" /> that represents a select operator using type inference where needed.
        /// </summary>
        /// <param name="source">The monad to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        /// <returns>A <see cref="SelectOperator" /> that has the given source.</returns>
        public SelectOperator Select(MonadMember source, QueryTree selector)
        {
            RequireNotNull(source, nameof(source));
            RequireNotNull(selector, nameof(selector));

            if (selector.QueryNodeType != QueryNodeType.Lambda)
                throw new ArgumentException("Cannot infer selector's return type. Type inference is only supported when the selector is a lambda abstraction.");

            return Select(((LambdaAbstraction)selector).Body.ReturnType, source.ElementType, source, selector);
        }

        /// <summary>
        /// Creates a <see cref="SelectOperator" /> that represents a select operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="inputElementType">The <see cref="Type" /> of the elements in the source monad.</param>
        /// <param name="source">The monad to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        /// <returns>A <see cref="SelectOperator" /> that has the given source.</returns>
        protected virtual SelectOperator MakeSelect(Type elementType, Type inputElementType, MonadMember source, QueryTree selector)
        {
            return new SelectOperator(elementType, inputElementType, source, selector);
        }

        /// <summary>
        /// Creates a <see cref="FirstOperator" /> that represents a take operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad to take elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>A <see cref="FirstOperator" /> that has the given source.</returns>
        public override TakeOperator Take(Type elementType, MonadMember source, QueryTree count)
        {
            RequireNotNull(elementType, nameof(elementType));
            RequireNotNull(source, nameof(source));
            RequireNotNull(count, nameof(count));

            RequireTypesAssignable(elementType, source.ElementType, nameof(elementType));

            return MakeTake(elementType, source, count);
        }

        /// <summary>
        /// Creates a <see cref="FirstOperator" /> that represents a take operator using type inference where needed.
        /// </summary>
        /// <param name="source">The monad to take elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>A <see cref="FirstOperator" /> that has the given source.</returns>
        public TakeOperator Take(MonadMember source, QueryTree count)
        {
            RequireNotNull(source, nameof(source));

            return Take(source.ElementType, source, count);
        }

        /// <summary>
        /// Creates a <see cref="FirstOperator" /> that represents a take operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad to take elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>A <see cref="FirstOperator" /> that has the given source.</returns>
        protected virtual TakeOperator MakeTake(Type elementType, MonadMember source, QueryTree count)
        {
            return new TakeOperator(elementType, source, count);
        }

        /// <summary>
        /// Creates a <see cref="WhereOperator" /> that represents a where operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad whose elements to filter.</param>
        /// <param name="predicate">The function to test each source element for a condition.</param>
        /// <returns>A <see cref="WhereOperator" /> that has the given source.</returns>
        public override WhereOperator Where(Type elementType, MonadMember source, QueryTree predicate)
        {
            RequireNotNull(elementType, nameof(elementType));
            RequireNotNull(source, nameof(source));
            RequireNotNull(predicate, nameof(predicate));

            RequireTypesAssignable(elementType, source.ElementType, nameof(elementType));

            return MakeWhere(elementType, source, predicate);
        }

        /// <summary>
        /// Creates a <see cref="WhereOperator" /> that represents a where operator using type inference where needed.
        /// </summary>
        /// <param name="source">The monad whose elements to filter.</param>
        /// <param name="predicate">The function to test each source element for a condition.</param>
        /// <returns>A <see cref="WhereOperator" /> that has the given source.</returns>
        public WhereOperator Where(MonadMember source, QueryTree predicate)
        {
            RequireNotNull(source, nameof(source));

            return Where(source.ElementType, source, predicate);
        }

        /// <summary>
        /// Creates a <see cref="WhereOperator" /> that represents a where operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad whose elements to filter.</param>
        /// <param name="predicate">The function to test each source element for a condition.</param>
        /// <returns>A <see cref="WhereOperator" /> that has the given source.</returns>
        protected virtual WhereOperator MakeWhere(Type elementType, MonadMember source, QueryTree predicate)
        {
            return new WhereOperator(elementType, source, predicate);
        }
    }
}
