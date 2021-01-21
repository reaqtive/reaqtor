// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/20/2014 - Centralized error handling.
//

namespace System
{
    internal static class Errors
    {
        public static void ThrowArgumentNull<T>(T value, string paramName)
            where T : class
        {
            if (value is null)
                throw new ArgumentNullException(paramName);
        }

        public static void ThrowArgumentOutOfRangeIf(bool isError, string paramName)
        {
            if (isError)
                throw new ArgumentOutOfRangeException(paramName);
        }
    }
}
