// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.Collections.ObjectModel;

namespace System.Linq.Expressions.Bonsai.Serialization.Binary
{
#if USE_SLIM
    using LabelTarget = System.Linq.Expressions.LabelTargetSlim;
    using ParameterExpression = System.Linq.Expressions.ParameterExpressionSlim;
#endif

    internal struct TypedParameter
    {
        public uint Type;
        public ParameterExpression Parameter;
    }

    internal struct TypedLabel
    {
        public uint Type;
        public LabelTarget Label;
    }

    internal enum MemberType : byte
    {
        None = 0x00,
        Field = 0x01,
        Property = 0x02,
        Constructor = 0x04,
        Method = 0x08,
        GenericMethod = 0x10,
        GenericMethodDefinition = 0x20,
    }

    internal static class EmptyArray<T>
    {
        public static readonly T[] Instance = Array.Empty<T>();
    }

    internal static class EmptyReadOnlyCollection<T>
    {
        public static readonly ReadOnlyCollection<T> Instance = new(EmptyArray<T>.Instance);
    }
}

#if USE_SLIM
namespace System.Reflection
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions.Bonsai.Serialization.Binary;

    internal static class TypeSlimExtensions
    {
        private static readonly Dictionary<TypeSlim, TypeCode> s_typeCodes = new(PooledTypeSlimEqualityComparer.Instance)
        {
            { Types.Byte, TypeCode.Byte },
            { Types.SByte, TypeCode.SByte },
            { Types.Int16, TypeCode.Int16 },
            { Types.UInt16, TypeCode.UInt16 },
            { Types.Int32, TypeCode.Int32 },
            { Types.UInt32, TypeCode.UInt32 },
            { Types.Int64, TypeCode.Int64 },
            { Types.UInt64, TypeCode.UInt64 },
            { Types.Single, TypeCode.Single },
            { Types.Double, TypeCode.Double },
            { Types.Decimal, TypeCode.Decimal },
            { Types.Boolean, TypeCode.Boolean },
            { Types.Char, TypeCode.Char },
            { Types.String, TypeCode.String },
            { Types.DateTime, TypeCode.DateTime },
        };

        public static int GetArrayRank(this TypeSlim type) => ((ArrayTypeSlim)type).Rank ?? 1; // REVIEW

        public static AssemblySlim GetAssembly(this TypeSlim type)
        {
            return type switch
            {
                SimpleTypeSlimBase simple => simple.Assembly,
                _ => null,
            };
        }

        public static void GetAssemblyAndName(this TypeSlim type, out AssemblySlim assembly, out string name)
        {
            var def = (SimpleTypeSlimBase)type;
            assembly = def.Assembly;
            name = def.Name;
        }

        public static TypeSlim GetElementType(this TypeSlim type) => ((ArrayTypeSlim)type).ElementType;

        public static ReadOnlyCollection<TypeSlim> GetGenericArguments(this TypeSlim type)
        {
            // PERF: Avoid accessing GenericArguments; use GetGenericArgument instead.

            return type switch
            {
                GenericTypeSlim gen => gen.GenericArguments,
                _ => throw new InvalidOperationException(),
            };
        }

        public static TypeSlim GetGenericTypeDefinition(this TypeSlim type)
        {
            return type switch
            {
                GenericTypeSlim gen => gen.GenericTypeDefinition,
                GenericDefinitionTypeSlim def => def,
                _ => throw new InvalidOperationException(),
            };
        }

        public static string GetName(this TypeSlim type)
        {
            return type switch
            {
                SimpleTypeSlimBase simple => simple.Name,
                GenericParameterTypeSlim parameter => parameter.Name,
                _ => throw new InvalidOperationException(),
            };
        }

        public static TypeCode GetTypeCode(this TypeSlim type)
        {
            if (s_typeCodes.TryGetValue(type, out var code))
            {
                return code;
            }

            return TypeCode.Object;
        }

        public static bool IsArray(this TypeSlim type) => type.Kind == TypeSlimKind.Array;
        public static bool IsGenericParameter(this TypeSlim type) => type.Kind == TypeSlimKind.GenericParameter;
        public static bool IsGenericType(this TypeSlim type) => type.Kind is TypeSlimKind.GenericDefinition or TypeSlimKind.Generic;
        public static bool IsGenericTypeDefinition(this TypeSlim type) => type.Kind == TypeSlimKind.GenericDefinition;

        public static TypeSlim MakeArrayType(this TypeSlim type) => TypeSlim.Array(type);
        public static TypeSlim MakeArrayType(this TypeSlim type, int rank) => TypeSlim.Array(type, rank);
        public static TypeSlim MakeGenericType(this TypeSlim definition, params TypeSlim[] arguments) => TypeSlim.Generic((GenericDefinitionTypeSlim)definition, arguments);
    }

    internal static class MethodInfoSlimExtensions
    {
        public static ReadOnlyCollection<TypeSlim> GetGenericArguments(this MethodInfoSlim method)
        {
            return method switch
            {
                GenericMethodInfoSlim gen => gen.GenericArguments,
                GenericDefinitionMethodInfoSlim def => def.GenericParameterTypes,
                _ => throw new InvalidOperationException(),
            };
        }

        public static MethodInfoSlim GetGenericMethodDefinition(this MethodInfoSlim method) => ((GenericMethodInfoSlim)method).GenericMethodDefinition;

        public static string GetName(this MethodInfoSlim method) => ((SimpleMethodInfoSlimBase)method).Name;

        public static bool IsGenericMethodDefinition(this MethodInfoSlim method) => method is GenericDefinitionMethodInfoSlim;

        public static MethodInfoSlim MakeGenericMethod(this MemberInfoSlim definition, params TypeSlim[] arguments)
        {
            var def = (GenericDefinitionMethodInfoSlim)definition;
            return def.DeclaringType.GetGenericMethod(def, new ReadOnlyCollection<TypeSlim>(arguments));
        }
    }
}
#else
namespace System
{
    using System.Reflection;

    internal static class TypeExtensions
    {
        public static Assembly GetAssembly(this Type type) => type.Assembly;

        public static void GetAssemblyAndName(this Type type, out Assembly assembly, out string name)
        {
            assembly = type.Assembly;
            name = type.FullName;
        }

        public static string GetName(this Type type) => type.Name;

        public static TypeCode GetTypeCode(this Type type) => Type.GetTypeCode(type);

        public static bool IsArray(this Type type) => type.IsArray;
        public static bool IsGenericParameter(this Type type) => type.IsGenericParameter;
        public static bool IsGenericType(this Type type) => type.IsGenericType;
        public static bool IsGenericTypeDefinition(this Type type) => type.IsGenericTypeDefinition;
    }

    namespace Reflection
    {
        internal static class MethodInfoExtensions
        {
            public static string GetName(this MethodInfo method) => method.Name;

            public static bool IsGenericMethodDefinition(this MethodInfo method) => method.IsGenericMethodDefinition;
        }
    }
}
#endif
