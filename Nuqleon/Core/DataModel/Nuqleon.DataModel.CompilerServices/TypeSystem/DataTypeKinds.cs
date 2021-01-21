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
    /// Enumeration of data model type kinds.
    /// </summary>
    /// <remarks>This enumeration supports flags in order to allow for checkers that check for a subset of the allowed data model type kinds.</remarks>
    [Flags]
    public enum DataTypeKinds
    {
        /// <summary>
        /// Primitive data type, i.e. a type treated as an atom by the data model.
        /// </summary>
        Primitive = 1,

        /// <summary>
        /// Array data type, i.e. a single-dimensional array with a data model type as the element type.
        /// </summary>
        Array = 2,

        /// <summary>
        /// Structural data type, i.e. a type with properties typed with data model types.
        /// </summary>
        Structural = 4,

        /// <summary>
        /// Function data type, i.e. akin to a delegate type.
        /// </summary>
        Function = 8,

        /// <summary>
        /// Expression data type, i.e. a code-as-data representation.
        /// </summary>
        Expression = 16,

        /// <summary>
        /// Function quotation data type, i.e. an expression representation of a function.
        /// </summary>
        Quotation = 32,

        /// <summary>
        /// Open generic parameter data type, i.e. a wildcard type.
        /// </summary>
        OpenGenericParameter = 64,

        /// <summary>
        /// Custom data type.
        /// </summary>
        Custom = 1024,
    }
}
