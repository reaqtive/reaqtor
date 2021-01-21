// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Reflection;

namespace System.Linq.Expressions.Jit
{
    /// <summary>
    /// Represents information about the closure field storage for a hoisted local.
    /// </summary>
    internal struct ClosureFieldInfo
    {
        /// <summary>
        /// The field in the closure holding the hoisted local.
        /// </summary>
        public FieldInfo Field;

        /// <summary>
        /// The storage kind of the hoisted local.
        /// </summary>
        public StorageKind Kind;
    }
}
