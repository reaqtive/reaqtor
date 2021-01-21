// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;

namespace Reaqtive
{
    internal static class Invariant
    {
        public static void Assert(bool value, string message)
        {
            if (!value)
                throw new InvalidOperationException(message);
        }
    }
}
