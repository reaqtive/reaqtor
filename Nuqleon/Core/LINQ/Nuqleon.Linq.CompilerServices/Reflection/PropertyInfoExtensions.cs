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
    internal static class PropertyInfoExtensions
    {
        internal static bool IsStatic(this PropertyInfo property) => GetPropertyAccessor(property).IsStatic;

        internal static bool IsPublic(this PropertyInfo property) => GetPropertyAccessor(property).IsPublic;

        internal static MethodInfo GetPropertyAccessor(PropertyInfo property) => property.GetGetMethod() ?? property.GetSetMethod();

        internal static string ToCSharpString(this PropertyInfo property)
        {
            // NOTE: This produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var hasGet = property.GetGetMethod() != null;
            var hasSet = property.GetSetMethod() != null;

            var accessors = " { ";
            if (hasGet)
                accessors += "get; ";
            if (hasSet)
                accessors += "set; ";
            accessors += "}";

            var dot = IsStatic(property) ? "::" : ".";

            return property.PropertyType.ToCSharpString() + " " + property.DeclaringType.ToCSharpString() + dot + property.Name + accessors;
        }
    }
}
