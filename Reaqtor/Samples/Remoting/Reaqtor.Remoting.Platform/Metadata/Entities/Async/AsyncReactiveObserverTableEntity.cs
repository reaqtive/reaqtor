// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Table entity representing an observer.
    /// </summary>
    public class AsyncReactiveObserverTableEntity : AsyncReactiveDefinedResourceTableEntity, IAsyncReactiveObserverDefinition
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        public AsyncReactiveObserverTableEntity()
        {
        }

        /// <summary>
        /// Creates a new table entity representing a observer with the specified URI and expression representation.
        /// </summary>
        /// <param name="uri">URI identifying the observer represented by the table entity.</param>
        /// <param name="expression">Expression representation of the observer.</param>
        /// <param name="state">The state.</param>
        public AsyncReactiveObserverTableEntity(Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        /// <summary>
        /// Gets the observer defined by the definition.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <returns>Representation of the observer defined by the definition.</returns>
        /// <remarks>
        /// This method can be used in isolation from the client library. It's allowed to return an object that encapsulates
        /// the definition using an expression tree, to be used for composition and delegation to other systems. The object
        /// does not have to provide data operations, as is expected from the client library proxy objects.
        /// </remarks>
        public IAsyncReactiveQbserver<T> ToObserver<T>()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on table entities.");
        }

        /// <summary>
        /// Gets the parameterized observer defined by the definition.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <returns>Representation of the parameterized observer defined by the definition.</returns>
        /// <remarks>
        /// This method can be used in isolation from the client library. It's allowed to return an object that encapsulates
        /// the definition using an expression tree, to be used for composition and delegation to other systems. The object
        /// does not have to provide data operations, as is expected from the client library proxy objects.
        /// </remarks>
        public Func<TArgs, IAsyncReactiveQbserver<TResult>> ToObserver<TArgs, TResult>()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on table entities.");
        }
    }
}
