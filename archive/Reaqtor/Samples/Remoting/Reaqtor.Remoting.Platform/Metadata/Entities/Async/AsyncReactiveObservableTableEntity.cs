// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Table entity representing an observable.
    /// </summary>
    public class AsyncReactiveObservableTableEntity : AsyncReactiveDefinedResourceTableEntity, IAsyncReactiveObservableDefinition
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        public AsyncReactiveObservableTableEntity()
        {
        }

        /// <summary>
        /// Creates a new table entity representing an observable with the specified URI and expression representation.
        /// </summary>
        /// <param name="uri">URI identifying the observable represented by the table entity.</param>
        /// <param name="expression">Expression representation of the observable.</param>
        /// <param name="state">The state.</param>
        public AsyncReactiveObservableTableEntity(System.Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        /// <summary>
        /// Gets an observable object that can be used to interact with the active observable.
        /// </summary>
        /// <typeparam name="T">The type produced by the observable.</typeparam>
        /// <returns>Observable object to interact with the observable.</returns>
        public IAsyncReactiveQbservable<T> ToObservable<T>()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on table entities.");
        }

        /// <summary>
        /// Gets an observable object that can be used to interact with the active observable.
        /// </summary>
        /// <typeparam name="TArgs">The input parameter to the observable.</typeparam>
        /// <typeparam name="TResult">The type produced by the observable.</typeparam>
        /// <returns>Observable object to interact with the observable.</returns>
        public Func<TArgs, IAsyncReactiveQbservable<TResult>> ToObservable<TArgs, TResult>()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on table entities.");
        }
    }
}
