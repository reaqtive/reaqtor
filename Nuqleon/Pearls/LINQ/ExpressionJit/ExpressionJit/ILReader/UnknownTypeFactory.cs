// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Code adapted from https://blogs.msdn.microsoft.com/haibo_luo/2010/04/19/ilvisualizer-2010-solution

// NB: These types live in the global namespace in order to make their type name as short as
//     possible when printed. They cannot share ITypeFactory.cs now that it uses a file-scoped
//     namespace (which applies to every declaration in its file).

#pragma warning disable

#if !GETTYPEFROMHANDLEUNSAFE

using System;
using System.Reflection;

static class UnknownTypeFactory
{
    public static readonly Type Unknown = typeof(Unknown);
    public static Type GetGenericUnknown(int arity) => Unknown.GetTypeInfo().Assembly.GetType(Unknown.FullName + "`" + arity, throwOnError: false);
}

class Unknown { }
class Unknown<T1> { }
class Unknown<T1, T2> { }
class Unknown<T1, T2, T3> { }
class Unknown<T1, T2, T3, T4> { }
class Unknown<T1, T2, T3, T4, T5> { }
class Unknown<T1, T2, T3, T4, T5, T6> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7, T8> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7, T8, T9> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> { }
class Unknown<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> { }

#endif
