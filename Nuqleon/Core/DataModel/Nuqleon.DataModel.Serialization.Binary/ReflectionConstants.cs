// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Nuqleon.DataModel.Serialization.Binary
{
    internal static class ReflectionConstants
    {
#pragma warning disable IDE0034 // Simplify 'default' expression
        public static readonly MethodInfo ReadIntCompact = (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadUInt32Compact(default(Stream)));
        public static readonly MethodInfo ReadByte = (MethodInfo)ReflectionHelpers.InfoOf((Stream s) => StreamHelpers.ReadByte(default(Stream)));
        public static readonly MethodInfo ReadString = (MethodInfo)ReflectionHelpers.InfoOf(() => StreamHelpers.ReadString(default(Stream)));
        public static readonly MethodInfo Deserialize = (MethodInfo)ReflectionHelpers.InfoOf((DataTypeBinarySerializer f) => f.Deserialize(default(Type), default(Stream)));
        public static readonly MethodInfo ExpressionDeserialize = (MethodInfo)ReflectionHelpers.InfoOf((IExpressionSerializer s) => s.Deserialize(default(Stream)));
        public static readonly MethodInfo HashSetAdd = (MethodInfo)ReflectionHelpers.InfoOf((HashSet<object> h) => h.Add(default(object)));
        public static readonly MethodInfo HashSetRemove = (MethodInfo)ReflectionHelpers.InfoOf((HashSet<object> h) => h.Remove(default(object)));

        public static readonly ConstructorInfo InvalidDataException = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new InvalidDataException(default(string)));
#pragma warning restore IDE0034 // Simplify 'default' expression

        public static readonly PropertyInfo ExpressionSerializer = (PropertyInfo)ReflectionHelpers.InfoOf((DataTypeBinarySerializer f) => f.ExpressionSerializer);
        public static readonly PropertyInfo ListCount = (PropertyInfo)ReflectionHelpers.InfoOf((IList l) => l.Count);

        public static readonly Type[] ListCtorArgs = new[] { typeof(int) };

        public static readonly Expression NullObject = Expression.Constant(value: null, typeof(object));
        public static readonly Expression ZeroInt32 = Expression.Constant(value: 0, typeof(int));
        public static readonly Expression NullValueFlag = Expression.Constant(Protocol.TYPE_FLAG_NULLVALUE, typeof(byte));
        public static readonly Expression ThrowUnexpectedTypeCode = Expression.Throw(Expression.New(InvalidDataException, Expression.Constant("Unexpected type code.", typeof(string))));
    }
}
