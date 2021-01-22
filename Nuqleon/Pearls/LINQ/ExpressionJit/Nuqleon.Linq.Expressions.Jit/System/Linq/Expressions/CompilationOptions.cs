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
    /// Provides various expression tree compilation options.
    /// </summary>
    [Flags]
    public enum CompilationOptions
    {
        /// <summary>
        /// No compilation options. Platform defaults are used.
        /// </summary>
        None = 0,

        /// <summary>
        /// Prefer evaluation of the resulting compiled expression tree using an interpreter.
        /// </summary>
        /// <remarks>
        /// This option is currently ignored.
        /// </remarks>
        PreferInterpretation = 1,

        /// <summary>
        /// Apply optimization steps prior to compilation.
        /// </summary>
        Optimize = 2,

        /// <summary>
        /// Enable Just In Time (JIT) compilation of lambda expressions.
        /// </summary>
        EnableJustInTimeCompilation = 4,

        /// <summary>
        /// Supports compilation of expression trees using an interpreter first, with the potential
        /// to recompile using dynamic IL code generation if the expression gets invoked frequently.
        /// </summary>
        TieredCompilation = 8,
    }
}
