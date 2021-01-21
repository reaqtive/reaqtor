// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine
{
    internal static class SerializerHelpers
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0060 // Remove unused parameter (used for type inference).
#pragma warning disable CA1801 // See above.
        public static TOutput To<TInput, TOutput>(this TInput input, TOutput witness)
        {
            return (TOutput)(object)input;
        }
#pragma warning restore CA1801
#pragma warning restore IDE0060
#pragma warning restore IDE0079

        public static bool TryGetSize(this Type type, out int size)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    size = sizeof(bool);
                    return true;
                case TypeCode.Byte:
                    size = sizeof(byte);
                    return true;
                case TypeCode.Char:
                    size = sizeof(char);
                    return true;
                case TypeCode.Double:
                    size = sizeof(double);
                    return true;
                case TypeCode.Int16:
                    size = sizeof(short);
                    return true;
                case TypeCode.Int32:
                    size = sizeof(int);
                    return true;
                case TypeCode.Int64:
                    size = sizeof(long);
                    return true;
                case TypeCode.SByte:
                    size = sizeof(sbyte);
                    return true;
                case TypeCode.Single:
                    size = sizeof(float);
                    return true;
                case TypeCode.UInt16:
                    size = sizeof(ushort);
                    return true;
                case TypeCode.UInt32:
                    size = sizeof(uint);
                    return true;
                case TypeCode.UInt64:
                    size = sizeof(ulong);
                    return true;
                case TypeCode.DBNull:
                case TypeCode.DateTime:
                case TypeCode.Decimal:
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.String:
                default:
                    size = -1;
                    return false;
            }
        }
    }
}
