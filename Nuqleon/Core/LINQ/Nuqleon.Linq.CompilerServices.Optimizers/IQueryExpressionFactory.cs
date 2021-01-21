// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Reserved language keyword 'Select'. (Nature of LINQ methods.)

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Interface for query expression factories.
    /// </summary>
    public interface IQueryExpressionFactory
    {
        /// <summary>
        /// Creates a <see cref="FirstOperator" /> that represents a first operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad from which to get the first element.</param>
        /// <returns>A <see cref="FirstOperator" /> that has the given source.</returns>
        FirstOperator First(Type elementType, MonadMember source);

        /// <summary>
        /// Creates a <see cref="FirstPredicateOperator" /> that represents a first operator with a predicate.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad from which to get the first element satisfying a condition.</param>
        /// <param name="predicate">The function to test each source element for a condition.</param>
        /// <returns>A <see cref="FirstPredicateOperator" /> that has the given source.</returns>
        FirstPredicateOperator First(Type elementType, MonadMember source, QueryTree predicate);

        /// <summary>
        /// Creates a <see cref="Optimizers.LambdaAbstraction" /> that abstracts over unknown query nodes and non-query nodes.
        /// </summary>
        /// <param name="body">The <see cref="LambdaExpression" /> which represents the unknown parts of a query with holes for known sub-parts.</param>
        /// <param name="parameters">The known sub-parts of a query.</param>
        /// <returns>A <see cref="Optimizers.LambdaAbstraction" /> with holes and parameters to fill the holes.</returns>
        LambdaAbstraction LambdaAbstraction(LambdaExpression body, IEnumerable<QueryTree> parameters);

        /// <summary>
        /// Creates a <see cref="Optimizers.MonadAbstraction" /> that abstracts over a monad member such as a source or unknown operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="inner">The inner monad to abstract over.</param>
        /// <returns>A <see cref="Optimizers.MonadAbstraction" /> that has the given monadic type.</returns>
        MonadAbstraction MonadAbstraction(Type elementType, QueryTree inner);

        /// <summary>
        /// Creates a <see cref="SelectOperator" /> that represents a select operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="inputElementType">The <see cref="Type" /> of the elements in the source monad.</param>
        /// <param name="source">The monad to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        /// <returns>A <see cref="SelectOperator" /> that has the given source.</returns>
        SelectOperator Select(Type elementType, Type inputElementType, MonadMember source, QueryTree selector);

        /// <summary>
        /// Creates a <see cref="FirstOperator" /> that represents a take operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad to take elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>A <see cref="FirstOperator" /> that has the given source.</returns>
        TakeOperator Take(Type elementType, MonadMember source, QueryTree count);

        /// <summary>
        /// Creates a <see cref="WhereOperator" /> that represents a where operator.
        /// </summary>
        /// <param name="elementType">The <see cref="Type" /> of the elements in the resulting monad.</param>
        /// <param name="source">The monad whose elements to filter.</param>
        /// <param name="predicate">The function to test each source element for a condition.</param>
        /// <returns>A <see cref="WhereOperator" /> that has the given source.</returns>
        WhereOperator Where(Type elementType, MonadMember source, QueryTree predicate);
    }
}
