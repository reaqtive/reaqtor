// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Factory for thunk types.
    /// </summary>
    internal static class ThunkFactory
    {
        /// <summary>
        /// Thunk factory for thunks that compile expression trees.
        /// </summary>
        public static IThunkFactory Compiled { get; } = new CompilingThunkFactory();

        /// <summary>
        /// Thunk factory for thunks that interpret expression trees.
        /// </summary>
        public static IThunkFactory Interpreted { get; } = new InterpretingThunkFactory();

        /// <summary>
        /// Thunk factory for thunks that first interpret expression trees and recompile expression trees
        /// if invoked frequently.
        /// </summary>
        public static IThunkFactory TieredCompilation { get; } = new TieredCompilationThunkFactory();
    }
}
