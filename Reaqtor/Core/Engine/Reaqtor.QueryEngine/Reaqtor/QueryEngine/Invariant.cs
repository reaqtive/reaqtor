// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    internal static class Invariant
    {
        internal static void Assert(bool condition, string message)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(condition, message);
#else
            if (!condition)
                throw new InvariantException(message);
#endif
        }
    }
}
