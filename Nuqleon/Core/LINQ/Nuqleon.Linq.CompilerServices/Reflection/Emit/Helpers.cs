// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2018 - Created this file.
//

//
// NB: This file contains a late bound accessor for System.Reflection.Emit types which are unavailable on .NET Standard 2.0.
//

using System;
using System.Reflection;

namespace Nuqleon.Reflection.Emit
{
    internal static class Helpers
    {
        public static Type Unwrap(this Type type) => type is TypeBuilder tb ? tb._type : type;

        public static Type[] Unwrap(this Type[] types)
        {
            var res = default(Type[]);

            var n = types.Length;

            for (var i = 0; i < n; i++)
            {
                var type = types[i];
                var unwrapped = type.Unwrap();

                if (res == null)
                {
                    if (type != unwrapped)
                    {
                        res = new Type[n];

                        for (var j = 0; j < i; j++)
                        {
                            res[j] = types[j];
                        }

                        res[i] = unwrapped;
                    }
                }
                else
                {
                    res[i] = unwrapped;
                }
            }

            return res ?? types;
        }

        public static ConstructorInfo Unwrap(this ConstructorInfo constructor) => constructor is ConstructorBuilder cb ? cb._ctor : constructor;

        public static MethodInfo Unwrap(this MethodInfo method) => method is MethodBuilder mb ? mb._method : method;

        public static FieldInfo Unwrap(this FieldInfo field) => field is FieldBuilder fb ? fb._field : field;

        public static PropertyInfo Unwrap(this PropertyInfo property) => property is PropertyBuilder pb ? pb._property : property;
    }
}
