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
    /// Specifies the behavior of thunk types.
    /// </summary>
    internal enum ThunkBehavior
    {
        /// <summary>
        /// Compile expression trees using dynamic IL code generation.
        /// </summary>
        Compiling,

        /// <summary>
        /// Compile expression trees using an interpreter execution target.
        /// </summary>
        Interpreting,

        /// <summary>
        /// Compile expression trees using an interpreter execution target, and recompile using dynamic IL code
        /// generation if sufficient invocations are made.
        /// </summary>
        TieredCompilation,
    }
}
