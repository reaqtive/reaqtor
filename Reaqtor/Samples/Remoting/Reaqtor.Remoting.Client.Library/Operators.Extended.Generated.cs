// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor
{
    public static partial class Operators
    {
        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.Int32)]
        public static IAsyncReactiveQbservable<Int32> Sum(this IAsyncReactiveQbservable<Int32> source)
        {
            return source.Provider.CreateQbservable<Int32>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.Int32)]
        public static IAsyncReactiveQbservable<Int32> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int32>> selector)
        {
            return source.Provider.CreateQbservable<Int32>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.NullableInt32)]
        public static IAsyncReactiveQbservable<Int32?> Sum(this IAsyncReactiveQbservable<Int32?> source)
        {
            return source.Provider.CreateQbservable<Int32?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.NullableInt32)]
        public static IAsyncReactiveQbservable<Int32?> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int32?>> selector)
        {
            return source.Provider.CreateQbservable<Int32?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.Int64)]
        public static IAsyncReactiveQbservable<Int64> Sum(this IAsyncReactiveQbservable<Int64> source)
        {
            return source.Provider.CreateQbservable<Int64>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.Int64)]
        public static IAsyncReactiveQbservable<Int64> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int64>> selector)
        {
            return source.Provider.CreateQbservable<Int64>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.NullableInt64)]
        public static IAsyncReactiveQbservable<Int64?> Sum(this IAsyncReactiveQbservable<Int64?> source)
        {
            return source.Provider.CreateQbservable<Int64?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.NullableInt64)]
        public static IAsyncReactiveQbservable<Int64?> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int64?>> selector)
        {
            return source.Provider.CreateQbservable<Int64?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.Single)]
        public static IAsyncReactiveQbservable<Single> Sum(this IAsyncReactiveQbservable<Single> source)
        {
            return source.Provider.CreateQbservable<Single>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.Single)]
        public static IAsyncReactiveQbservable<Single> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Single>> selector)
        {
            return source.Provider.CreateQbservable<Single>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.NullableSingle)]
        public static IAsyncReactiveQbservable<Single?> Sum(this IAsyncReactiveQbservable<Single?> source)
        {
            return source.Provider.CreateQbservable<Single?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.NullableSingle)]
        public static IAsyncReactiveQbservable<Single?> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Single?>> selector)
        {
            return source.Provider.CreateQbservable<Single?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.Double)]
        public static IAsyncReactiveQbservable<Double> Sum(this IAsyncReactiveQbservable<Double> source)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.Double)]
        public static IAsyncReactiveQbservable<Double> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Double>> selector)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.NullableDouble)]
        public static IAsyncReactiveQbservable<Double?> Sum(this IAsyncReactiveQbservable<Double?> source)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.NullableDouble)]
        public static IAsyncReactiveQbservable<Double?> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Double?>> selector)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.Decimal)]
        public static IAsyncReactiveQbservable<Decimal> Sum(this IAsyncReactiveQbservable<Decimal> source)
        {
            return source.Provider.CreateQbservable<Decimal>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.Decimal)]
        public static IAsyncReactiveQbservable<Decimal> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Decimal>> selector)
        {
            return source.Provider.CreateQbservable<Decimal>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.NoSelector.NullableDecimal)]
        public static IAsyncReactiveQbservable<Decimal?> Sum(this IAsyncReactiveQbservable<Decimal?> source)
        {
            return source.Provider.CreateQbservable<Decimal?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Sum.Selector.NullableDecimal)]
        public static IAsyncReactiveQbservable<Decimal?> Sum<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Decimal?>> selector)
        {
            return source.Provider.CreateQbservable<Decimal?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.Int32)]
        public static IAsyncReactiveQbservable<Int32> Max(this IAsyncReactiveQbservable<Int32> source)
        {
            return source.Provider.CreateQbservable<Int32>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.Int32)]
        public static IAsyncReactiveQbservable<Int32> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int32>> selector)
        {
            return source.Provider.CreateQbservable<Int32>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.NullableInt32)]
        public static IAsyncReactiveQbservable<Int32?> Max(this IAsyncReactiveQbservable<Int32?> source)
        {
            return source.Provider.CreateQbservable<Int32?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.NullableInt32)]
        public static IAsyncReactiveQbservable<Int32?> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int32?>> selector)
        {
            return source.Provider.CreateQbservable<Int32?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.Int64)]
        public static IAsyncReactiveQbservable<Int64> Max(this IAsyncReactiveQbservable<Int64> source)
        {
            return source.Provider.CreateQbservable<Int64>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.Int64)]
        public static IAsyncReactiveQbservable<Int64> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int64>> selector)
        {
            return source.Provider.CreateQbservable<Int64>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.NullableInt64)]
        public static IAsyncReactiveQbservable<Int64?> Max(this IAsyncReactiveQbservable<Int64?> source)
        {
            return source.Provider.CreateQbservable<Int64?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.NullableInt64)]
        public static IAsyncReactiveQbservable<Int64?> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int64?>> selector)
        {
            return source.Provider.CreateQbservable<Int64?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.Single)]
        public static IAsyncReactiveQbservable<Single> Max(this IAsyncReactiveQbservable<Single> source)
        {
            return source.Provider.CreateQbservable<Single>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.Single)]
        public static IAsyncReactiveQbservable<Single> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Single>> selector)
        {
            return source.Provider.CreateQbservable<Single>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.NullableSingle)]
        public static IAsyncReactiveQbservable<Single?> Max(this IAsyncReactiveQbservable<Single?> source)
        {
            return source.Provider.CreateQbservable<Single?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.NullableSingle)]
        public static IAsyncReactiveQbservable<Single?> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Single?>> selector)
        {
            return source.Provider.CreateQbservable<Single?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.Double)]
        public static IAsyncReactiveQbservable<Double> Max(this IAsyncReactiveQbservable<Double> source)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.Double)]
        public static IAsyncReactiveQbservable<Double> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Double>> selector)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.NullableDouble)]
        public static IAsyncReactiveQbservable<Double?> Max(this IAsyncReactiveQbservable<Double?> source)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.NullableDouble)]
        public static IAsyncReactiveQbservable<Double?> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Double?>> selector)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.Decimal)]
        public static IAsyncReactiveQbservable<Decimal> Max(this IAsyncReactiveQbservable<Decimal> source)
        {
            return source.Provider.CreateQbservable<Decimal>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.Decimal)]
        public static IAsyncReactiveQbservable<Decimal> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Decimal>> selector)
        {
            return source.Provider.CreateQbservable<Decimal>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.NoSelector.NullableDecimal)]
        public static IAsyncReactiveQbservable<Decimal?> Max(this IAsyncReactiveQbservable<Decimal?> source)
        {
            return source.Provider.CreateQbservable<Decimal?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Max.Selector.NullableDecimal)]
        public static IAsyncReactiveQbservable<Decimal?> Max<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Decimal?>> selector)
        {
            return source.Provider.CreateQbservable<Decimal?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.Int32)]
        public static IAsyncReactiveQbservable<Int32> Min(this IAsyncReactiveQbservable<Int32> source)
        {
            return source.Provider.CreateQbservable<Int32>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.Int32)]
        public static IAsyncReactiveQbservable<Int32> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int32>> selector)
        {
            return source.Provider.CreateQbservable<Int32>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.NullableInt32)]
        public static IAsyncReactiveQbservable<Int32?> Min(this IAsyncReactiveQbservable<Int32?> source)
        {
            return source.Provider.CreateQbservable<Int32?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.NullableInt32)]
        public static IAsyncReactiveQbservable<Int32?> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int32?>> selector)
        {
            return source.Provider.CreateQbservable<Int32?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.Int64)]
        public static IAsyncReactiveQbservable<Int64> Min(this IAsyncReactiveQbservable<Int64> source)
        {
            return source.Provider.CreateQbservable<Int64>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.Int64)]
        public static IAsyncReactiveQbservable<Int64> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int64>> selector)
        {
            return source.Provider.CreateQbservable<Int64>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.NullableInt64)]
        public static IAsyncReactiveQbservable<Int64?> Min(this IAsyncReactiveQbservable<Int64?> source)
        {
            return source.Provider.CreateQbservable<Int64?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.NullableInt64)]
        public static IAsyncReactiveQbservable<Int64?> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int64?>> selector)
        {
            return source.Provider.CreateQbservable<Int64?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.Single)]
        public static IAsyncReactiveQbservable<Single> Min(this IAsyncReactiveQbservable<Single> source)
        {
            return source.Provider.CreateQbservable<Single>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.Single)]
        public static IAsyncReactiveQbservable<Single> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Single>> selector)
        {
            return source.Provider.CreateQbservable<Single>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.NullableSingle)]
        public static IAsyncReactiveQbservable<Single?> Min(this IAsyncReactiveQbservable<Single?> source)
        {
            return source.Provider.CreateQbservable<Single?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.NullableSingle)]
        public static IAsyncReactiveQbservable<Single?> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Single?>> selector)
        {
            return source.Provider.CreateQbservable<Single?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.Double)]
        public static IAsyncReactiveQbservable<Double> Min(this IAsyncReactiveQbservable<Double> source)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.Double)]
        public static IAsyncReactiveQbservable<Double> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Double>> selector)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.NullableDouble)]
        public static IAsyncReactiveQbservable<Double?> Min(this IAsyncReactiveQbservable<Double?> source)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.NullableDouble)]
        public static IAsyncReactiveQbservable<Double?> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Double?>> selector)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.Decimal)]
        public static IAsyncReactiveQbservable<Decimal> Min(this IAsyncReactiveQbservable<Decimal> source)
        {
            return source.Provider.CreateQbservable<Decimal>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.Decimal)]
        public static IAsyncReactiveQbservable<Decimal> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Decimal>> selector)
        {
            return source.Provider.CreateQbservable<Decimal>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.NoSelector.NullableDecimal)]
        public static IAsyncReactiveQbservable<Decimal?> Min(this IAsyncReactiveQbservable<Decimal?> source)
        {
            return source.Provider.CreateQbservable<Decimal?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Min.Selector.NullableDecimal)]
        public static IAsyncReactiveQbservable<Decimal?> Min<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Decimal?>> selector)
        {
            return source.Provider.CreateQbservable<Decimal?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.Int32)]
        public static IAsyncReactiveQbservable<Double> Average(this IAsyncReactiveQbservable<Int32> source)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.Int32)]
        public static IAsyncReactiveQbservable<Double> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int32>> selector)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.NullableInt32)]
        public static IAsyncReactiveQbservable<Double?> Average(this IAsyncReactiveQbservable<Int32?> source)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.NullableInt32)]
        public static IAsyncReactiveQbservable<Double?> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int32?>> selector)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.Int64)]
        public static IAsyncReactiveQbservable<Double> Average(this IAsyncReactiveQbservable<Int64> source)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.Int64)]
        public static IAsyncReactiveQbservable<Double> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int64>> selector)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.NullableInt64)]
        public static IAsyncReactiveQbservable<Double?> Average(this IAsyncReactiveQbservable<Int64?> source)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.NullableInt64)]
        public static IAsyncReactiveQbservable<Double?> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Int64?>> selector)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.Single)]
        public static IAsyncReactiveQbservable<Single> Average(this IAsyncReactiveQbservable<Single> source)
        {
            return source.Provider.CreateQbservable<Single>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.Single)]
        public static IAsyncReactiveQbservable<Single> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Single>> selector)
        {
            return source.Provider.CreateQbservable<Single>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.NullableSingle)]
        public static IAsyncReactiveQbservable<Single?> Average(this IAsyncReactiveQbservable<Single?> source)
        {
            return source.Provider.CreateQbservable<Single?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.NullableSingle)]
        public static IAsyncReactiveQbservable<Single?> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Single?>> selector)
        {
            return source.Provider.CreateQbservable<Single?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.Double)]
        public static IAsyncReactiveQbservable<Double> Average(this IAsyncReactiveQbservable<Double> source)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.Double)]
        public static IAsyncReactiveQbservable<Double> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Double>> selector)
        {
            return source.Provider.CreateQbservable<Double>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.NullableDouble)]
        public static IAsyncReactiveQbservable<Double?> Average(this IAsyncReactiveQbservable<Double?> source)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.NullableDouble)]
        public static IAsyncReactiveQbservable<Double?> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Double?>> selector)
        {
            return source.Provider.CreateQbservable<Double?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.Decimal)]
        public static IAsyncReactiveQbservable<Decimal> Average(this IAsyncReactiveQbservable<Decimal> source)
        {
            return source.Provider.CreateQbservable<Decimal>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.Decimal)]
        public static IAsyncReactiveQbservable<Decimal> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Decimal>> selector)
        {
            return source.Provider.CreateQbservable<Decimal>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.NoSelector.NullableDecimal)]
        public static IAsyncReactiveQbservable<Decimal?> Average(this IAsyncReactiveQbservable<Decimal?> source)
        {
            return source.Provider.CreateQbservable<Decimal?>(
                Expression.Call(
                    (MethodInfo)MethodBase.GetCurrentMethod(),
                    source.Expression));
        }

        [KnownResource(Remoting.Client.Constants.Observable.Average.Selector.NullableDecimal)]
        public static IAsyncReactiveQbservable<Decimal?> Average<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, Decimal?>> selector)
        {
            return source.Provider.CreateQbservable<Decimal?>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    selector));
        }

    }
}
