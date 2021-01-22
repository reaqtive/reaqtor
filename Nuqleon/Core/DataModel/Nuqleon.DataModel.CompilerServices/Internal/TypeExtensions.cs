// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;

namespace Nuqleon.DataModel
{
    internal static class TypeExtensions
    {
        public static string ToCSharpStringPretty(this Type type) => System.TypeExtensions.ToCSharpString(type, useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: false);

        public static bool IsNullableType(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static Type GetNonNullType(this Type type) => type.GetGenericArguments()[0];
    }
}
