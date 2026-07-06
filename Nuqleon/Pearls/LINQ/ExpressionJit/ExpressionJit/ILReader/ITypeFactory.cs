// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Code adapted from https://blogs.msdn.microsoft.com/haibo_luo/2010/04/19/ilvisualizer-2010-solution

#pragma warning disable

using System;
using System.Reflection;

namespace System.Linq.Expressions.Tests;

public interface ITypeFactory
{
    Type FromHandle(IntPtr handle);
    Type MakeArrayType(Type type);
    Type MakeArrayType(Type type, int rank);
    Type MakeByRefType(Type type);
    Type MakePointerType(Type type);
    Type MakeGenericType(Type definition, Type[] arguments);
}

internal class DefaultTypeFactory : ITypeFactory
{
    public static readonly ITypeFactory Instance = new DefaultTypeFactory();

    protected DefaultTypeFactory() { }

#if GETTYPEFROMHANDLEUNSAFE
    private static readonly MethodInfo s_GetTypeFromHandleUnsafe = typeof(Type).GetMethodAssert("GetTypeFromHandleUnsafe");

    public virtual Type FromHandle(IntPtr handle) => (Type)s_GetTypeFromHandleUnsafe.Invoke(obj: null, new object[] { handle });
    public Type MakeGenericType(Type definition, Type[] arguments) => definition.MakeGenericType(arguments);
#else
    public virtual Type FromHandle(IntPtr handle) => typeof(Unknown);
    public Type MakeGenericType(Type definition, Type[] arguments)
    {
        if (definition == UnknownTypeFactory.Unknown)
        {
            definition = UnknownTypeFactory.GetGenericUnknown(arguments.Length);

            if (definition == null)
            {
                return UnknownTypeFactory.Unknown;
            }
        }

        return definition.MakeGenericType(arguments);
    }
#endif
    public Type MakeArrayType(Type type) => type.MakeArrayType();
    public Type MakeArrayType(Type type, int rank) => type.MakeArrayType(rank);
    public Type MakeByRefType(Type type) => type.MakeByRefType();
    public Type MakePointerType(Type type) => type.MakePointerType();
}
