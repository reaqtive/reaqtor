// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Client;

namespace Reaqtor.Reliable.Expressions
{
    /// <summary>
    /// Creates a new observer represented by an expression tree, using the specified associated query provider.
    /// </summary>
    /// <param name="provider">Query provider associated with the observer.</param>
    public abstract class ReliableQbserverBase<T>(IReliableQueryProvider provider) : ReliableReactiveObserverBase<T>, IReliableQbserver<T>
    {

        /// <summary>
        /// Gets the type of the data received by the observer.
        /// </summary>
        public Type ElementType => typeof(T);

        /// <summary>
        /// Gets the query provider that is associated with the observer.
        /// </summary>
        public IReliableQueryProvider Provider { get; } = provider;

        /// <summary>
        /// Gets the expression tree representing the observer.
        /// </summary>
        public abstract Expression Expression { get; }
    }
}
