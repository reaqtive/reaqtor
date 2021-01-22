// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices
{
    internal static class Helpers
    {
        private static readonly TypeSlim ListType = typeof(List<>).ToTypeSlim();
        private static readonly HashSet<TypeSlim> TupleTypes = new(TypeSlimEqualityComparer.Default)
        {
            typeof(Tuple<>).ToTypeSlim(),
            typeof(Tuple<,>).ToTypeSlim(),
            typeof(Tuple<,,>).ToTypeSlim(),
            typeof(Tuple<,,,>).ToTypeSlim(),
            typeof(Tuple<,,,,>).ToTypeSlim(),
            typeof(Tuple<,,,,,>).ToTypeSlim(),
            typeof(Tuple<,,,,,,>).ToTypeSlim(),
            typeof(Tuple<,,,,,,,>).ToTypeSlim(),
        };

        public static Type GetMemberType(MemberInfo member)
        {
            Debug.Assert(member.MemberType is MemberTypes.Field or MemberTypes.Property);

            if (member is PropertyInfo property)
            {
                return property.PropertyType;
            }
            else
            {
                var field = (FieldInfo)member;
                return field.FieldType;
            }
        }

        public static Type GetUnderlyingEnumType(Type type)
        {
            if (type.IsEnum)
            {
                return Enum.GetUnderlyingType(type);
            }
            else
            {
                var nullableType = Nullable.GetUnderlyingType(type);
                if (nullableType != null && nullableType.IsEnum)
                {
                    var underlyingType = Enum.GetUnderlyingType(nullableType);
                    return typeof(Nullable<>).MakeGenericType(underlyingType);
                }
            }

            return null;
        }

        public static TypeSlim GetMemberType(MemberInfoSlim member)
        {
            Debug.Assert(member.MemberType is MemberTypes.Field or MemberTypes.Property);

            if (member is PropertyInfoSlim property)
            {
                return property.PropertyType;
            }
            else
            {
                var field = (FieldInfoSlim)member;
                return field.FieldType;
            }
        }

        public static string GetMemberName(MemberInfoSlim member)
        {
            Debug.Assert(member.MemberType is MemberTypes.Field or MemberTypes.Property);

            if (member is PropertyInfoSlim property)
            {
                return property.Name;
            }
            else
            {
                var field = (FieldInfoSlim)member;
                return field.Name;
            }
        }

        public static bool TryGetElementType(TypeSlim type, out TypeSlim elementType)
        {
            if (type is ArrayTypeSlim arrayType)
            {
                elementType = arrayType.ElementType;
                return true;
            }
            else
            {
                if (type is GenericTypeSlim genericType && Equals(genericType.GenericTypeDefinition, ListType)) // NB: TypeSlim implements IEquatable
                {
                    elementType = genericType.GetGenericArgument(0);
                    return true;
                }
            }

            elementType = default;
            return false;
        }

        public static bool TryGetStructuralPropertyTypes(TypeSlim type, out ReadOnlyCollection<TypeSlim> propertyTypes)
        {
            if (type is StructuralTypeSlim structuralType)
            {
                var properties = structuralType.Properties;
                var n = properties.Count;

                var propertyTypeArray = new TypeSlim[n];
                for (var i = 0; i < n; ++i)
                {
                    propertyTypeArray[i] = properties[i].PropertyType;
                }

                propertyTypes = AsReadOnly(propertyTypeArray);
                return true;
            }
            else
            {
                if (type is GenericTypeSlim genericType && TupleTypes.Contains(genericType.GenericTypeDefinition, TypeSlimEqualityComparer.Default))
                {
                    propertyTypes = genericType.GenericArguments;
                    return true;
                }
            }

            propertyTypes = default;
            return false;
        }

        // NB: The default HashSet<T> pool has a maximum capacity configuration to drop the HashSet<T> if it grew too big.

        private static readonly HashSetPool<string> s_stringHashSetPool = HashSetPool<string>.Create(Environment.ProcessorCount * 2);

        public static PooledHashSetHolder<string> NewHashSetOfString() => s_stringHashSetPool.New();

        public static ReadOnlyCollection<T> AsReadOnly<T>(T[] array) => new(array); // TODO: Use TrueReadOnlyCollection<T> with ownership transfer.
    }
}
