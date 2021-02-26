// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2013 - Created this file.
//

using System.Collections.Concurrent;
using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Simple compiled delegate cache with unbounded size.
    /// </summary>
    public class SimpleCompiledDelegateCache : ICompiledDelegateCache
    {
        private readonly ConcurrentDictionary<LambdaExpression, Delegate> _cache;

        /// <summary>
        /// Creates a new delegate cache.
        /// </summary>
        public SimpleCompiledDelegateCache() => _cache = new ConcurrentDictionary<LambdaExpression, Delegate>(new ExpressionEqualityComparer());

        /// <summary>
        /// Gets the number of entries in the cache.
        /// </summary>
        public int Count => _cache.Count;

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear() => _cache.Clear();

        /// <summary>
        /// Gets a compiled delegate from the cache if the specified lambda expression already has been compiled.
        /// Otherwise, compiles the lambda expression to a delegate and stores the result.
        /// </summary>
        /// <param name="expression">Lambda expression to look up in the cache.</param>
        /// <returns>Compiled delegate to execute the lambda expression.</returns>
        public Delegate GetOrAdd(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            //
            // PERF: On .NET Framework, this causes multiple calls to GetHashCode, which is expensive for our comparer. However,
            //       since the following commit in .NET Core, this path has been optimized:
            //
            //         https://github.com/dotnet/runtime/commit/b4a76eed426f18d087f27edbe6d2bc63590bf914
            //

            return _cache.GetOrAdd(expression, l => l.Compile());
        }
    }
}
