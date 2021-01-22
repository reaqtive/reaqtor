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
    /// Void compiled delegate cache that just compiles lambda expressions without storing them.
    /// </summary>
    public class VoidCompiledDelegateCache : ICompiledDelegateCache
    {
        private VoidCompiledDelegateCache()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the void compiled delegate cache.
        /// </summary>
        public static ICompiledDelegateCache Instance { get; } = new VoidCompiledDelegateCache();

        /// <summary>
        /// Gets the number of entries in the cache. Always returns zero.
        /// </summary>
        public int Count => 0;

        /// <summary>
        /// Clears the cache. This operation has no effect.
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Compiles the lambda expression to a delegate and returns the result.
        /// </summary>
        /// <param name="expression">Lambda expression to look up in the cache.</param>
        /// <returns>Compiled delegate to execute the lambda expression.</returns>
        public Delegate GetOrAdd(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return expression.Compile();
        }
    }
}
