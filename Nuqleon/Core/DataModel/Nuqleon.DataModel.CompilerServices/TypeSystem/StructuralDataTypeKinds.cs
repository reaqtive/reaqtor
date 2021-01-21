// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - July 2013 - Created this file.
//

using System;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Enumeration of structural data type kinds.
    /// </summary>
    /// <remarks>This enumeration supports flags in order to allow for checkers that check for a subset of the allowed data model type kinds.</remarks>
    [Flags]
    public enum StructuralDataTypeKinds
    {
        /// <summary>
        /// Anonymous type.
        /// </summary>
        Anonymous = 1,

        /// <summary>
        /// Record type.
        /// </summary>
        Record = 2,

        /// <summary>
        /// User-defined entity type.
        /// </summary>
        Entity = 4,

        /// <summary>
        /// Tuple type.
        /// </summary>
        Tuple = 8,
    }
}
