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
    /// The method table which is used as the first parameter to a compiled
    /// expression with JIT compilation support.
    /// </summary>
    public sealed class MethodTable
    {
#pragma warning disable CA1051 // Do not declare visible instance field. (Usage of field is by design.)

        /// <summary>
        /// Array containing the thunks for all the inner lambda expressions
        /// that can be JIT compiled.
        /// </summary>
        public readonly object[] Thunks;

#pragma warning restore CA1051

        /// <summary>
        /// Creates a new method table with the specified <paramref name="thunks"/>.
        /// </summary>
        /// <param name="thunks">Array containing the thunks for all the inner lambda expressions that can be JIT compiled.</param>
        public MethodTable(object[] thunks)
        {
            Thunks = thunks;
        }
    }
}
