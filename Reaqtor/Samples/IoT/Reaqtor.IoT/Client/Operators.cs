// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if !DEFINE_ALL_OPERATORS
using System;
using System.Linq.Expressions;
using System.Reflection;

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
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));
        }

        [KnownResource("iot://reactor/observables/average/int64")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));
        }

        [KnownResource("iot://reactor/observables/average/double")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));
        }

        [KnownResource("iot://reactor/observables/average/selector/int32")]
        public static IAsyncReactiveQbservable<double> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/average/selector/int64")]
        public static IAsyncReactiveQbservable<double> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/average/selector/double")]
        public static IAsyncReactiveQbservable<double> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/distinct")]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource>(this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
        }

        [KnownResource("iot://reactor/observables/group")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/map")]
        public static IAsyncReactiveQbservable<TResult> Select<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/map/indexed")]
        public static IAsyncReactiveQbservable<TResult> Select<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/bind")]
        public static IAsyncReactiveQbservable<TResult> SelectMany<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, IAsyncReactiveQbservable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        [KnownResource("iot://reactor/observables/take")]
        public static IAsyncReactiveQbservable<TSource> Take<TSource>(this IAsyncReactiveQbservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count)));
        }

        [KnownResource("iot://reactor/observables/filter")]
        public static IAsyncReactiveQbservable<TSource> Where<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
        }

        [KnownResource("iot://reactor/observables/filter/indexed")]
        public static IAsyncReactiveQbservable<TSource> Where<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
        }

        [KnownResource("iot://reactor/observables/window/hopping/count")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count)));
        }

        [KnownResource("iot://reactor/observables/window/hopping/time")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration)));
        }

        [KnownResource("iot://reactor/observables/window/sliding/count")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count), Expression.Constant(skip)));
        }

        [KnownResource("iot://reactor/observables/window/sliding/time")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration), Expression.Constant(shift)));
        }

        [KnownResource("iot://reactor/observables/window/ferry")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration), Expression.Constant(count)));
        }
    }
}
#endif
