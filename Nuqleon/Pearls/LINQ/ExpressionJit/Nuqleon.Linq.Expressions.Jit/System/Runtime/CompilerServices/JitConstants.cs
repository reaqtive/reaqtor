﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Configuration constants to influence JIT behavior.
    /// </summary>
    internal static class JitConstants
    {
        /// <summary>
        /// Number of invocations after which tiered compilation switches from interpretation to compilation.
        /// </summary>
        public const int TieredCompilationThreshold = 4;
    }
}
