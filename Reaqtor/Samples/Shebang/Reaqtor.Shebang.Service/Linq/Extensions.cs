// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.Shebang.Linq
{
    //
    // Provides a set of extension methods used to define reactive artifacts in the engine. These methods are effectively
    // conversions between interfaces used within the engine (to define operators etc.) and interfaces used in the "quoted"
    // expression tree space. When defining artifcts, a conversion is needed to squeeze the expressions through the define
    // operations which are statically typed to use the quoted interfaces (akin to IQueryable<T> in classic LINQ).
    //
    // All this does is provide a fluent way to define artifacts as As* extension methods rather than ugly casts. The known
    // resource identifier refers to a magic "rx://builtin/id" operation that's well-known to all components of IRP. It
    // stands for an identity conversion. After rewriting of the expression tree inside the query engine, all uses of the
    // quoted types (IReactiveQ*<T>) get turned into the corresponding non-quoted types (e.g. ISubscribable<T>) and all
    // stars align to erase all of these "rx://builtin/id" conversions which have become T-to-T identity conversions.
    //
    // Users never see these methods. They are only used within the environment to define "primitive" operations in the
    // engine (or custom artifacts provided by the environment).
    //

    public static class Extensions
    {
#pragma warning disable IDE0060 // Remove unused parameter (these are used in expression trees)
        [KnownResource("rx://builtin/id")]
        public static IReactiveQbservable<T> AsQbservable<T>(this ISubscribable<T> observable) => throw new NotImplementedException();

        [KnownResource("rx://builtin/id")]
        public static IAsyncReactiveQbservable<T> AsAsyncQbservable<T>(this ISubscribable<T> observable) => throw new NotImplementedException();

        [KnownResource("rx://builtin/id")]
        public static IReactiveQbserver<T> AsQbserver<T>(this IObserver<T> observer) => throw new NotImplementedException();

        [KnownResource("rx://builtin/id")]
        public static IAsyncReactiveQbserver<T> AsAsyncQbserver<T>(this IObserver<T> observer) => throw new NotImplementedException();

        [KnownResource("rx://builtin/id")]
        public static ISubscribable<T> AsSubscribable<T>(this IReactiveQbservable<T> observable) => throw new NotImplementedException();

        [KnownResource("rx://builtin/id")]
        public static ISubscribable<T> AsSubscribable<T>(this IAsyncReactiveQbservable<T> observable) => throw new NotImplementedException();
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
