// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace System.Reflection
{
    /// <summary>
    /// Enum describing the kind of a method.
    /// </summary>
    public enum MethodInfoSlimKind
    {
        /// <summary>
        /// Simple method, i.e. not generic.
        /// </summary>
        Simple,

        /// <summary>
        /// Open generic method definition.
        /// </summary>
        GenericDefinition,

        /// <summary>
        /// Closed generic method, consisting of a method definition and type arguments.
        /// </summary>
        Generic,
    }
}
