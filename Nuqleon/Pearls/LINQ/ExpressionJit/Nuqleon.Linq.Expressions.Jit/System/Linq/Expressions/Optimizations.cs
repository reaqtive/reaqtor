// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

namespace System.Linq.Expressions
{
    /// <summary>
    /// Flags to select optimizations to apply.
    /// </summary>
    [Flags]
    internal enum Optimizations
    {
        /// <summary>
        /// No optimizations are applied.
        /// </summary>
        None = 0,

        /// <summary>
        /// Flattens nested blocks in order to reduce the number of nested scopes.
        /// </summary>
        BlockFlattening = 1,

        /// <summary>
        /// Inlines the invocation of a lambda expression.
        /// </summary>
        InvocationInlining = 2,

        /// <summary>
        /// Enables all optimizations.
        /// </summary>
        All = BlockFlattening | InvocationInlining,
    }
}
