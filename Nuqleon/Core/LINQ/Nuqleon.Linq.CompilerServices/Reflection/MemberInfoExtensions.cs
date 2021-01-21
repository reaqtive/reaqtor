// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using SLCR = System.Linq.CompilerServices.Reflection;

namespace System.Reflection
{
    internal static class MemberInfoExtensions
    {
        internal static string ToCSharpString(this MemberInfo member)
        {
            // NOTE: This produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            return member.MemberType switch
            {
                MemberTypes.Method => SLCR.MethodInfoExtensions.ToCSharpString((MethodInfo)member),
                MemberTypes.Constructor => SLCR.ConstructorInfoExtensions.ToCSharpString((ConstructorInfo)member),
                MemberTypes.Property => SLCR.PropertyInfoExtensions.ToCSharpString((PropertyInfo)member),
                MemberTypes.Field => SLCR.FieldInfoExtensions.ToCSharpString((FieldInfo)member),
                MemberTypes.Event => SLCR.EventInfoExtensions.ToCSharpString((EventInfo)member),
                MemberTypes.NestedType or MemberTypes.TypeInfo => System.TypeExtensions.ToCSharpString((Type)member, useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: false),
                _ => throw new NotSupportedException(),
            };
        }
    }
}
