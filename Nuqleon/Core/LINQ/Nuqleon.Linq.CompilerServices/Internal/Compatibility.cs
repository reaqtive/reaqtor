// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2014 - Created this file.
//

using System.Runtime.CompilerServices;

namespace System
{
    internal static class TypeCompatibilityExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGenericType(this Type type) => type.IsGenericType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGenericTypeDefinition(this Type type) => type.IsGenericTypeDefinition;
    }

    namespace Reflection
    {
        internal static class MemberInfoCompatibilityExtensions
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static MemberTypes GetMemberType(this MemberInfo member) => member.MemberType;
        }
    }
}
