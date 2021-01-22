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
    internal static class FieldInfoExtensions
    {
        internal static string ToCSharpString(this FieldInfo field)
        {
            // NOTE: This produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var dot = field.IsStatic ? "::" : ".";

            return field.FieldType.ToCSharpString() + " " + field.DeclaringType.ToCSharpString() + dot + field.Name;
        }
    }
}
