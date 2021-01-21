// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/03/2015 - Created ValueOrError functionality.
//

namespace System.Memory
{
    /// <summary>
    /// Enumeration indicating whether an object represents a value or an error.
    /// </summary>
    public enum ValueOrErrorKind
    {
        /// <summary>
        /// Representation of a value.
        /// </summary>
        Value,

        /// <summary>
        /// Representation of an error.
        /// </summary>
        Error,
    }
}
