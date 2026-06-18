// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2013 - Created this file.
//

namespace System.Linq.Expressions
{
    /// <summary>
    /// Event arguments for a cache event.
    /// </summary>
    /// <remarks>
    /// Creates a new event argument associated with the specified cache entry.
    /// </remarks>
    /// <param name="expression">Lambda expression used as the key of the cache entry.</param>
    /// <param name="delegate">Delegate used as the value of the cache entry.</param>
    public class CacheEventArgs(LambdaExpression expression, Delegate @delegate) : EventArgs
    {

        /// <summary>
        /// Gets the lambda expression used as the key of the cache entry.
        /// </summary>
        public LambdaExpression Lambda { get; } = expression;

        /// <summary>
        /// Gets the delegate used as the value of the cache entry.
        /// </summary>
        public Delegate Delegate { get; } = @delegate;
    }
}
