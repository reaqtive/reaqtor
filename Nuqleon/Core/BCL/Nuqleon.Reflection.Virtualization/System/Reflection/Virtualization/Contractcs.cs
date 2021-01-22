// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2017 - Created this file.
//

using System.Runtime.CompilerServices;

namespace System.Reflection
{
    internal static class Contracts
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NotNull<T>(T provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            return provider;
        }
    }
}
