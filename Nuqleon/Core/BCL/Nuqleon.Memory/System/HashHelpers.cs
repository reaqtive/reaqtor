﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

namespace System
{
    /// <summary>
    /// Provides a set of helpers to create stable hash codes.
    /// </summary>
    internal static partial class HashHelpers
    {
        /// <summary>
        /// Combines two hash values.
        /// </summary>
        /// <param name="a">The first hash value to combine.</param>
        /// <param name="b">The second hash value to combine.</param>
        /// <returns>A combination of the two hash codes.</returns>
        public static int Combine(int a, int b)
        {
            // NB: RyuJIT optimizes this to use the ROL instruction.
            //     This code is borrowed from the corefx implementation, see https://github.com/dotnet/corefx/blob/src/Common/src/System/Numerics/Hashing/HashHelpers.cs

            uint rol5 = ((uint)a << 5) | ((uint)a >> 27);
            return ((int)rol5 + a) ^ b;
        }
    }
}
