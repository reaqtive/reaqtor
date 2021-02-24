// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

namespace System
{
    internal static class TypeUtils
    {
        public static Type FindGenericType(Type definition, Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && AreEquivalent(type.GetGenericTypeDefinition(), definition))
                {
                    return type;
                }

                if (definition.IsInterface)
                {
                    var interfaces = type.GetInterfaces();

                    for (int i = 0; i < interfaces.Length; i++)
                    {
                        var ifType = interfaces[i];
                        var res = FindGenericType(definition, ifType);
                        if (res != null)
                        {
                            return res;
                        }
                    }
                }

                type = type.BaseType;
            }

            return null;
        }

        public static bool AreEquivalent(Type t1, Type t2)
        {
            return t1 == t2 || t1.IsEquivalentTo(t2);
        }

        internal static bool HasPrimitiveConversion(Type source, Type dest)
        {
            return IsConvertible(source) && IsConvertible(dest);
        }

        internal static bool IsConvertible(Type type)
        {
            type = type.GetNonNullableType();

            if (type.IsEnum)
            {
                return true;
            }

            return Type.GetTypeCode(type)
                is TypeCode.Boolean
                or TypeCode.Char
                or TypeCode.SByte
                or TypeCode.Byte
                or TypeCode.Int16
                or TypeCode.UInt16
                or TypeCode.Int32
                or TypeCode.UInt32
                or TypeCode.Int64
                or TypeCode.UInt64
                or TypeCode.Single
                or TypeCode.Double;
        }

        internal static bool IsInteger(Type type)
        {
            type = type.GetNonNullableType();

            if (type.IsEnum)
            {
                return false;
            }

            return Type.GetTypeCode(type)
                is TypeCode.SByte
                or TypeCode.Byte
                or TypeCode.Int16
                or TypeCode.UInt16
                or TypeCode.Int32
                or TypeCode.UInt32
                or TypeCode.Int64
                or TypeCode.UInt64;
        }
    }
}
