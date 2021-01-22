// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if !DEFINE_ALL_OPERATORS
using System;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;

namespace Reaqtor.IoT
{
    //
    // Provides a set of extension methods that comprise the query operator programming surface used by end-users. This is
    // equivalent to Queryable.* in classic LINQ, with the difference being that there's no such thing as a "standard query
    // operators" set. That is, the environment chooses which operators to expose, and the way they get mapped to operator
    // implementations inside the engine is through KnownResource identifiers.
    //
    // In particular, none of the components in the IRP stack has direct knowledge of operators like Where and Select; these
    // are merely reactive artifacts that happen to be defined using some URI that happens to be used in a KnownResource
    // attribute on some extension method named Where or Select.
    //

    public static class Operators
    {
        [KnownResource("iot://reactor/observables/average/int32")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<int> source)
        {
            return source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));
        }

        [KnownResource("iot://reactor/observables/average/int64")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<long> source)
        {
            return source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));
        }

        [KnownResource("iot://reactor/observables/average/double")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<double> source)
        {
            return source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));
        }

        [KnownResource("iot://reactor/observables/average/selector/int32")]
        public static IAsyncReactiveQbservable<double> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, int>> selector)
        {
            return source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/average/selector/int64")]
        public static IAsyncReactiveQbservable<double> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, long>> selector)
        {
            return source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/average/selector/double")]
        public static IAsyncReactiveQbservable<double> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, double>> selector)
        {
            return source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/distinct")]
        public static IAsyncReactiveQbservable<T> DistinctUntilChanged<T>(this IAsyncReactiveQbservable<T> source)
        {
            return source.Provider.CreateQbservable<T>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression));
        }

        [KnownResource("iot://reactor/observables/group")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<K, T>> GroupBy<T, K>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, K>> selector)
        {
            return source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<K, T>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(K)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/map")]
        public static IAsyncReactiveQbservable<R> Select<T, R>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, R>> selector)
        {
            return source.Provider.CreateQbservable<R>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/map/indexed")]
        public static IAsyncReactiveQbservable<R> Select<T, R>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, int, R>> selector)
        {
            return source.Provider.CreateQbservable<R>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/bind")]
        public static IAsyncReactiveQbservable<R> SelectMany<T, R>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, IAsyncReactiveQbservable<R>>> selector)
        {
            return source.Provider.CreateQbservable<R>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/take")]
        public static IAsyncReactiveQbservable<T> Take<T>(this IAsyncReactiveQbservable<T> source, int count)
        {
            return source.Provider.CreateQbservable<T>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, Expression.Constant(count)));
        }

        [KnownResource("iot://reactor/observables/filter")]
        public static IAsyncReactiveQbservable<T> Where<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Provider.CreateQbservable<T>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, predicate));
        }

        [KnownResource("iot://reactor/observables/filter/indexed")]
        public static IAsyncReactiveQbservable<T> Where<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, int, bool>> predicate)
        {
            return source.Provider.CreateQbservable<T>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, predicate));
        }

        [KnownResource("iot://reactor/observables/window/hopping/count")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>> Window<T>(this IAsyncReactiveQbservable<T> source, int count)
        {
            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<T>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, Expression.Constant(count)));
        }

        [KnownResource("iot://reactor/observables/window/hopping/time")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>> Window<T>(this IAsyncReactiveQbservable<T> source, TimeSpan duration)
        {
            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<T>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, Expression.Constant(duration)));
        }

        [KnownResource("iot://reactor/observables/window/sliding/count")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>> Window<T>(this IAsyncReactiveQbservable<T> source, int count, int skip)
        {
            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<T>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, Expression.Constant(count), Expression.Constant(skip)));
        }

        [KnownResource("iot://reactor/observables/window/sliding/time")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>> Window<T>(this IAsyncReactiveQbservable<T> source, TimeSpan duration, TimeSpan shift)
        {
            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<T>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, Expression.Constant(duration), Expression.Constant(shift)));
        }

        [KnownResource("iot://reactor/observables/window/ferry")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>> Window<T>(this IAsyncReactiveQbservable<T> source, TimeSpan duration, int count)
        {
            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<T>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)), source.Expression, Expression.Constant(duration), Expression.Constant(count)));
        }
    }
}
#endif
