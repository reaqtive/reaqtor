// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Enumeration of primitive data type kinds.
    /// </summary>
    /// <remarks>This enumeration supports flags in order to allow for checkers that check for a subset of the allowed data model type kinds.</remarks>
    [Flags]
    public enum PrimitiveDataTypeKinds
    {
        /// <summary>
        /// Atom type.
        /// </summary>
        Atom = 1,

        /// <summary>
        /// Enumeration type.
        /// </summary>
        Enum = 2,

        /// <summary>
        /// User-defined enumeration type.
        /// </summary>
        EntityEnum = 4,
    }
}
