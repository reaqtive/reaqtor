// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

namespace System.Linq.Expressions.Bonsai
{
    internal static class Invariant
    {
        public static void Assert(bool condition, string message)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(condition, message);
#else
            if (!condition)
                throw new InvalidOperationException(message);
#endif
        }

        public static Exception Unreachable => new InvalidOperationException("Unreachable code.");
    }
}
