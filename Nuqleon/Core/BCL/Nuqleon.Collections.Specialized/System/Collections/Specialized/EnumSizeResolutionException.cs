// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Created this type.
//   IG - 2025/12    - Remove CLR serialization support.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1032 // Add other exception constructors. (This exception is only instantiated by the library.)

namespace System.Collections.Specialized
{
    /// <summary>
    /// The exception representing a failure to determine the size needed for an EnumDictionary.
    /// </summary>
    public sealed class EnumSizeResolutionException : Exception
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public EnumSizeResolutionError ErrorCode { get; }

        internal EnumSizeResolutionException(EnumSizeResolutionError errorCode)
            : base()
        {
            ErrorCode = errorCode;
        }
    }
}
