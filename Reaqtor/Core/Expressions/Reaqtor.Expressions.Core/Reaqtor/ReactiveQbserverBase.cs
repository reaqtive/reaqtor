// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Base class for the implementation of observers represented by an expression tree.
    /// </summary>
    /// <typeparam name="T">Type of the data received by the observer.</typeparam>
    public abstract class ReactiveQbserverBase<T> : ReactiveObserverBase<T>, IReactiveQbserver<T>
    {
        /// <summary>
        /// Creates a new observer represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the observer.</param>
        protected ReactiveQbserverBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the type of the data received by the observer.
        /// </summary>
        public Type ElementType => typeof(T);

        /// <summary>
        /// Gets the query provider that is associated with the observer.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the observer.
        /// </summary>
        public abstract Expression Expression { get; }
    }
}
