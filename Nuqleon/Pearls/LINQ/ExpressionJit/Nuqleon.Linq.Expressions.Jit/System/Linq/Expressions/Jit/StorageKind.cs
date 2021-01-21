// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

namespace System.Linq.Expressions.Jit
{
    /// <summary>
    /// Enum for different storage kinds for variables.
    /// Designed to support combinations of storage requirements, e.g. <c>Hoisted | Boxed</c>.
    /// </summary>
    [Flags]
    internal enum StorageKind
    {
        /// <summary>
        /// Store the variable in a local (default).
        /// </summary>
        Local = 0,

        /// <summary>
        /// Hoist the variable for storage in a closure object.
        /// </summary>
        Hoisted = 1,

        /// <summary>
        /// Store the variable in a strong box, for use within a RuntimeVariables or Quote expression.
        /// </summary>
        Boxed = 2,
    }
}
