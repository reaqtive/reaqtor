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
    /// Interface describing a cache holding compiled delegates.
    /// </summary>
    public interface ICompiledDelegateCache
    {
        /// <summary>
        /// Gets the number of entries in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets a compiled delegate from the cache if the specified lambda expression already has been compiled.
        /// Otherwise, compiles the lambda expression to a delegate and stores the result.
        /// </summary>
        /// <param name="expression">Lambda expression to look up in the cache.</param>
        /// <returns>Compiled delegate to execute the lambda expression.</returns>
        Delegate GetOrAdd(LambdaExpression expression);
    }
}
