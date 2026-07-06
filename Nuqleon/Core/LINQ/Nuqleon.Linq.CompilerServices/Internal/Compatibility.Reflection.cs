// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2014 - Created this file.
//

// NB: Split out of Compatibility.cs so both files can use a file-scoped namespace (a file can
//     hold only one namespace in that style).

using System.Runtime.CompilerServices;

namespace System.Reflection;

internal static class MemberInfoCompatibilityExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberTypes GetMemberType(this MemberInfo member) => member.MemberType;
}
