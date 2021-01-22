// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

namespace System.Reflection
{
    /// <summary>
    /// Enum describing the kind of a type.
    /// </summary>
    public enum TypeSlimKind
    {
        /// <summary>
        /// Simple type, i.e. not constructed from other types.
        /// </summary>
        Simple,

        /// <summary>
        /// Array type, consisting of an element type and a rank.
        /// </summary>
        Array,

        /// <summary>
        /// Structural type, consisting of a set of named members.
        /// </summary>
        Structural,

        /// <summary>
        /// Open generic type definition.
        /// </summary>
        GenericDefinition,

        /// <summary>
        /// Closed generic type, consisting of a type definition and type arguments.
        /// </summary>
        Generic,

        /// <summary>
        /// Generic type parameter.
        /// </summary>
        GenericParameter,
    }
}
