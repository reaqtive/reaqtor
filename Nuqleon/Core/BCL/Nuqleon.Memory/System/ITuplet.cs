// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Created this type.
//

namespace System
{
    /// <summary>
    /// Interface for tuplets, used to recursively call some functionality on nested tuples without knowing the closed generic type.
    /// </summary>
    internal interface ITuplet
    {
        /// <summary>
        /// Gets the string representation of the tuplet, including the trailing closing parenthesis.
        /// </summary>
        /// <returns>String representation of the tuplet.</returns>
        string ToStringEnd();
    }
}
