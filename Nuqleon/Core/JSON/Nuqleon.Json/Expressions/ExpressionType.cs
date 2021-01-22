// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

#pragma warning disable CA1720 // Identifier 'X' contains type name (for Object and String). By design; models JSON.

namespace Nuqleon.Json.Expressions
{
    /// <summary>
    /// Expression tree types.
    /// </summary>
    public enum ExpressionType
    {
        /// <summary>
        /// JSON object.
        /// </summary>
        Object,

        /// <summary>
        /// JSON array.
        /// </summary>
        Array,

        /// <summary>
        /// JSON Boolean constant value.
        /// </summary>
        Boolean,

        /// <summary>
        /// JSON numeric constant value.
        /// </summary>
        Number,

        /// <summary>
        /// JSON string constant value.
        /// </summary>
        String,

        /// <summary>
        /// JSON null value.
        /// </summary>
        Null,
    }
}
