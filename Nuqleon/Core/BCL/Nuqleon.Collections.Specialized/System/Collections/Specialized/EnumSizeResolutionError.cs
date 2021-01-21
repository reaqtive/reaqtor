// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Created this type.
//

namespace System.Collections.Specialized
{
    /// <summary>
    /// The reasons for being unable to find the size of an EnumDictionary.
    /// </summary>
    public enum EnumSizeResolutionError
    {
        /// <summary>
        /// Unknown failure.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The type given was not an enum.
        /// </summary>
        TypeIsNotEnum = 1,

        /// <summary>
        /// The underlying type is not an int, short, unsigned short, sbyte, or byte.
        /// </summary>
        UnderlyingTypeIsNotIntOrSmaller = 2,

        /// <summary>
        /// The enum contains negative values.
        /// </summary>
        EnumContainsNegativeValues = 3,

        /// <summary>
        /// The enum is a flags enum.
        /// </summary>
        EnumHasFlagAttribute = 4,
    }
}
