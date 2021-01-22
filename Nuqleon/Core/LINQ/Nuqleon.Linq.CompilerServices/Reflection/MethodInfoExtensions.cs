// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Reflection;

namespace System.Linq.CompilerServices.Reflection
{
    internal static class MethodInfoExtensions
    {
        internal static string ToCSharpString(this MethodInfo method)
        {
            // NOTE: This produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var name = method.Name;
            if (method.IsGenericMethod)
            {
                name = name + "<" + string.Join(", ", method.GetGenericArguments().Select(a => a.ToCSharpString())) + ">";
            }

            var dot = method.IsStatic ? "::" : ".";

            return method.ReturnType.ToCSharpString() + " " + method.DeclaringType.ToCSharpString() + dot + name + "(" + string.Join(", ", method.GetParameters().Select(p => p.ParameterType.ToCSharpString() + " " + p.Name)) + ")";
        }
    }
}
