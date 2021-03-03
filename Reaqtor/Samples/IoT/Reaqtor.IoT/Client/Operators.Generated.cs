// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if DEFINE_ALL_OPERATORS
using System;
using System.Collections.Generic;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;

namespace Microsoft.IoT.Reactor
{
    public static class Operators
    {
        [KnownResource("iot://reactor/observables/aggregate")]
        public static IAsyncReactiveQbservable<TSource> Aggregate<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TSource, TSource>> aggregate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, aggregate));

        [KnownResource("iot://reactor/observables/aggregate/seed")]
        public static IAsyncReactiveQbservable<TResult> Aggregate<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, TResult seed, Expression<Func<TResult, TSource, TResult>> accumulate) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TResult)), accumulate));

        [KnownResource("iot://reactor/observables/aggregate/seed/result")]
        public static IAsyncReactiveQbservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncReactiveQbservable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulate, Expression<Func<TAccumulate, TResult>> resultSelector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulate, resultSelector));

        [KnownResource("iot://reactor/observables/all")]
        public static IAsyncReactiveQbservable<bool> All<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/any")]
        public static IAsyncReactiveQbservable<bool> Any<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/any/predicate")]
        public static IAsyncReactiveQbservable<bool> Any<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/average/int")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<int> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/long")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<long> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/float")]
        public static IAsyncReactiveQbservable<float> Average(this IAsyncReactiveQbservable<float> source) => source.Provider.CreateQbservable<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/double")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<double> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/decimal")]
        public static IAsyncReactiveQbservable<decimal> Average(this IAsyncReactiveQbservable<decimal> source) => source.Provider.CreateQbservable<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/nullable_int")]
        public static IAsyncReactiveQbservable<double?> Average(this IAsyncReactiveQbservable<int?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/nullable_long")]
        public static IAsyncReactiveQbservable<double?> Average(this IAsyncReactiveQbservable<long?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Average(this IAsyncReactiveQbservable<float?> source) => source.Provider.CreateQbservable<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Average(this IAsyncReactiveQbservable<double?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Average(this IAsyncReactiveQbservable<decimal?> source) => source.Provider.CreateQbservable<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/average/selector/int")]
        public static IAsyncReactiveQbservable<double> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/average/selector/long")]
        public static IAsyncReactiveQbservable<double> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/average/selector/float")]
        public static IAsyncReactiveQbservable<float> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float>> selector) => source.Provider.CreateQbservable<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/average/selector/double")]
        public static IAsyncReactiveQbservable<double> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/average/selector/decimal")]
        public static IAsyncReactiveQbservable<decimal> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal>> selector) => source.Provider.CreateQbservable<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/average/selector/nullable_int")]
        public static IAsyncReactiveQbservable<double?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/average/selector/nullable_long")]
        public static IAsyncReactiveQbservable<double?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/average/selector/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float?>> selector) => source.Provider.CreateQbservable<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/average/selector/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/average/selector/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal?>> selector) => source.Provider.CreateQbservable<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/buffer/duration")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan))));

        [KnownResource("iot://reactor/observables/buffer/count")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, int count) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));

        [KnownResource("iot://reactor/observables/buffer/duration/shift")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan)), Expression.Constant(shift, typeof(TimeSpan))));

        [KnownResource("iot://reactor/observables/buffer/count/skip")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, int count, int skip) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int)), Expression.Constant(skip, typeof(int))));

        [KnownResource("iot://reactor/observables/buffer/duration/count")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan)), Expression.Constant(count, typeof(int))));

        [KnownResource("iot://reactor/observables/combinelatest/2")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, Expression<Func<TSource1, TSource2, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TResult)), source1.Expression, source2.Expression, selector));

        [KnownResource("iot://reactor/observables/combinelatest/3")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, Expression<Func<TSource1, TSource2, TSource3, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, selector));

        [KnownResource("iot://reactor/observables/combinelatest/4")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, Expression<Func<TSource1, TSource2, TSource3, TSource4, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TSource4), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, source4.Expression, selector));

        [KnownResource("iot://reactor/observables/combinelatest/5")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TSource4), typeof(TSource5), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, source4.Expression, source5.Expression, selector));

        [KnownResource("iot://reactor/observables/combinelatest/6")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, IAsyncReactiveQbservable<TSource6> source6, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TSource4), typeof(TSource5), typeof(TSource6), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, source4.Expression, source5.Expression, source6.Expression, selector));

        [KnownResource("iot://reactor/observables/combinelatest/7")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, IAsyncReactiveQbservable<TSource6> source6, IAsyncReactiveQbservable<TSource7> source7, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TSource4), typeof(TSource5), typeof(TSource6), typeof(TSource7), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, source4.Expression, source5.Expression, source6.Expression, source7.Expression, selector));

        [KnownResource("iot://reactor/observables/count")]
        public static IAsyncReactiveQbservable<int> Count<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/count/predicate")]
        public static IAsyncReactiveQbservable<int> Count<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/delaysubscription/absolute")]
        public static IAsyncReactiveQbservable<TSource> DelaySubscription<TSource>(this IAsyncReactiveQbservable<TSource> source, DateTimeOffset dueTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(dueTime, typeof(DateTimeOffset))));

        [KnownResource("iot://reactor/observables/delaysubscription/relative")]
        public static IAsyncReactiveQbservable<TSource> DelaySubscription<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(dueTime, typeof(TimeSpan))));

        [KnownResource("iot://reactor/observables/distinctuntilchanged/")]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/distinctuntilchanged/comparer")]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource>(this IAsyncReactiveQbservable<TSource> source, IEqualityComparer<TSource> comparer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));

        [KnownResource("iot://reactor/observables/distinctuntilchanged/keySelector")]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));

        [KnownResource("iot://reactor/observables/distinctuntilchanged/keySelector/comparer")]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));

        [KnownResource("iot://reactor/observables/do/observer")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, IObserver<TSource> observer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(observer, typeof(IObserver<TSource>))));

        [KnownResource("iot://reactor/observables/do/onNext")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action<TSource>> onNext) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext));

        [KnownResource("iot://reactor/observables/do/onNext/onError")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError));

        [KnownResource("iot://reactor/observables/do/onNext/onCompleted")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action> onCompleted) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onCompleted));

        [KnownResource("iot://reactor/observables/do/selector/observer")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource, TNotification>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TNotification>> selector, IObserver<TNotification> observer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TNotification)), source.Expression, selector, Expression.Constant(observer, typeof(IObserver<TNotification>))));

        [KnownResource("iot://reactor/observables/do/onNext/onError/onCompleted")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError, Expression<Action> onCompleted) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError, onCompleted));

        [KnownResource("iot://reactor/observables/finally")]
        public static IAsyncReactiveQbservable<TSource> Finally<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action> action) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, action));

        [KnownResource("iot://reactor/observables/first")]
        public static IAsyncReactiveQbservable<TSource> FirstAsync<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/first/predicate")]
        public static IAsyncReactiveQbservable<TSource> FirstAsync<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/firstordefault")]
        public static IAsyncReactiveQbservable<TSource> FirstOrDefaultAsync<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/firstordefault/predicate")]
        public static IAsyncReactiveQbservable<TSource> FirstOrDefaultAsync<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/groupby/keySelector")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector) => source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));

        [KnownResource("iot://reactor/observables/groupby/keySelector/comparer")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer) => source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));

        [KnownResource("iot://reactor/observables/groupby/keySelector/elementSelector")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector) => source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector));

        [KnownResource("iot://reactor/observables/groupby/keySelector/elementSelector/comparer")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer) => source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));

        [KnownResource("iot://reactor/observables/isempty")]
        public static IAsyncReactiveQbservable<bool> IsEmpty<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/longcount")]
        public static IAsyncReactiveQbservable<long> LongCount<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/longcount/predicate")]
        public static IAsyncReactiveQbservable<long> LongCount<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/max")]
        public static IAsyncReactiveQbservable<TSource> Max<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/max/int")]
        public static IAsyncReactiveQbservable<int> Max(this IAsyncReactiveQbservable<int> source) => source.Provider.CreateQbservable<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/long")]
        public static IAsyncReactiveQbservable<long> Max(this IAsyncReactiveQbservable<long> source) => source.Provider.CreateQbservable<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/float")]
        public static IAsyncReactiveQbservable<float> Max(this IAsyncReactiveQbservable<float> source) => source.Provider.CreateQbservable<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/double")]
        public static IAsyncReactiveQbservable<double> Max(this IAsyncReactiveQbservable<double> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/decimal")]
        public static IAsyncReactiveQbservable<decimal> Max(this IAsyncReactiveQbservable<decimal> source) => source.Provider.CreateQbservable<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Max(this IAsyncReactiveQbservable<int?> source) => source.Provider.CreateQbservable<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Max(this IAsyncReactiveQbservable<long?> source) => source.Provider.CreateQbservable<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Max(this IAsyncReactiveQbservable<float?> source) => source.Provider.CreateQbservable<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Max(this IAsyncReactiveQbservable<double?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Max(this IAsyncReactiveQbservable<decimal?> source) => source.Provider.CreateQbservable<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/max/comparer")]
        public static IAsyncReactiveQbservable<TSource> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, IComparer<TSource> comparer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>))));

        [KnownResource("iot://reactor/observables/max/selector/int")]
        public static IAsyncReactiveQbservable<int> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int>> selector) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/max/selector/long")]
        public static IAsyncReactiveQbservable<long> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long>> selector) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/max/selector/float")]
        public static IAsyncReactiveQbservable<float> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float>> selector) => source.Provider.CreateQbservable<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/max/selector/double")]
        public static IAsyncReactiveQbservable<double> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/max/selector/decimal")]
        public static IAsyncReactiveQbservable<decimal> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal>> selector) => source.Provider.CreateQbservable<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/max/selector/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int?>> selector) => source.Provider.CreateQbservable<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/max/selector/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long?>> selector) => source.Provider.CreateQbservable<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/max/selector/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float?>> selector) => source.Provider.CreateQbservable<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/max/selector/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/max/selector/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal?>> selector) => source.Provider.CreateQbservable<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/merge")]
        public static IAsyncReactiveQbservable<TSource> Merge<TSource>(this IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> sources) => sources.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), sources.Expression));

        [KnownResource("iot://reactor/observables/merge/2")]
        public static IAsyncReactiveQbservable<TSource> Merge<TSource>(this IAsyncReactiveQbservable<TSource> first, IAsyncReactiveQbservable<TSource> second) => first.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, second.Expression));

        [KnownResource("iot://reactor/observables/min")]
        public static IAsyncReactiveQbservable<TSource> Min<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/min/int")]
        public static IAsyncReactiveQbservable<int> Min(this IAsyncReactiveQbservable<int> source) => source.Provider.CreateQbservable<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/long")]
        public static IAsyncReactiveQbservable<long> Min(this IAsyncReactiveQbservable<long> source) => source.Provider.CreateQbservable<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/float")]
        public static IAsyncReactiveQbservable<float> Min(this IAsyncReactiveQbservable<float> source) => source.Provider.CreateQbservable<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/double")]
        public static IAsyncReactiveQbservable<double> Min(this IAsyncReactiveQbservable<double> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/decimal")]
        public static IAsyncReactiveQbservable<decimal> Min(this IAsyncReactiveQbservable<decimal> source) => source.Provider.CreateQbservable<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Min(this IAsyncReactiveQbservable<int?> source) => source.Provider.CreateQbservable<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Min(this IAsyncReactiveQbservable<long?> source) => source.Provider.CreateQbservable<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Min(this IAsyncReactiveQbservable<float?> source) => source.Provider.CreateQbservable<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Min(this IAsyncReactiveQbservable<double?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Min(this IAsyncReactiveQbservable<decimal?> source) => source.Provider.CreateQbservable<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/min/comparer")]
        public static IAsyncReactiveQbservable<TSource> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, IComparer<TSource> comparer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>))));

        [KnownResource("iot://reactor/observables/min/selector/int")]
        public static IAsyncReactiveQbservable<int> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int>> selector) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/min/selector/long")]
        public static IAsyncReactiveQbservable<long> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long>> selector) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/min/selector/float")]
        public static IAsyncReactiveQbservable<float> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float>> selector) => source.Provider.CreateQbservable<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/min/selector/double")]
        public static IAsyncReactiveQbservable<double> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/min/selector/decimal")]
        public static IAsyncReactiveQbservable<decimal> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal>> selector) => source.Provider.CreateQbservable<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/min/selector/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int?>> selector) => source.Provider.CreateQbservable<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/min/selector/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long?>> selector) => source.Provider.CreateQbservable<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/min/selector/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float?>> selector) => source.Provider.CreateQbservable<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/min/selector/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/min/selector/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal?>> selector) => source.Provider.CreateQbservable<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/retry")]
        public static IAsyncReactiveQbservable<TSource> Retry<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/retry/count")]
        public static IAsyncReactiveQbservable<TSource> Retry<TSource>(this IAsyncReactiveQbservable<TSource> source, int retryCount) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(retryCount, typeof(int))));

        [KnownResource("iot://reactor/observables/sample/period")]
        public static IAsyncReactiveQbservable<TSource> Sample<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan period) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(period, typeof(TimeSpan))));

        [KnownResource("iot://reactor/observables/sample")]
        public static IAsyncReactiveQbservable<TSource> Sample<TSource, TSample>(this IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TSample> sampler) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TSample)), source.Expression, sampler.Expression));

        [KnownResource("iot://reactor/observables/scan")]
        public static IAsyncReactiveQbservable<TSource> Scan<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TSource, TSource>> aggregate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, aggregate));

        [KnownResource("iot://reactor/observables/scan/seed")]
        public static IAsyncReactiveQbservable<TResult> Scan<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, TResult seed, Expression<Func<TResult, TSource, TResult>> accumulate) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TResult)), accumulate));

        [KnownResource("iot://reactor/observables/select")]
        public static IAsyncReactiveQbservable<TResult> Select<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TResult>> selector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/select/indexed")]
        public static IAsyncReactiveQbservable<TResult> Select<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, TResult>> selector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/selectmany")]
        public static IAsyncReactiveQbservable<TResult> SelectMany<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, IAsyncReactiveQbservable<TResult>>> selector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/selectmany/result")]
        public static IAsyncReactiveQbservable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, IAsyncReactiveQbservable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));

        [KnownResource("iot://reactor/observables/sequenceequal")]
        public static IAsyncReactiveQbservable<bool> SequenceEqual<TSource>(this IAsyncReactiveQbservable<TSource> left, IAsyncReactiveQbservable<TSource> right) => left.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), left.Expression, right.Expression));

        [KnownResource("iot://reactor/observables/sequenceequal/comparer")]
        public static IAsyncReactiveQbservable<bool> SequenceEqual<TSource>(this IAsyncReactiveQbservable<TSource> left, IAsyncReactiveQbservable<TSource> right, IEqualityComparer<TSource> comparer) => left.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), left.Expression, right.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));

        [KnownResource("iot://reactor/observables/skip/count")]
        public static IAsyncReactiveQbservable<TSource> Skip<TSource>(this IAsyncReactiveQbservable<TSource> source, int count) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));

        [KnownResource("iot://reactor/observables/skip/dueTime")]
        public static IAsyncReactiveQbservable<TSource> Skip<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(dueTime, typeof(TimeSpan))));

        [KnownResource("iot://reactor/observables/skipuntil")]
        public static IAsyncReactiveQbservable<TSource> SkipUntil<TSource, TOther>(this IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> triggeringSource) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)), source.Expression, triggeringSource.Expression));

        [KnownResource("iot://reactor/observables/skipuntil/absolute")]
        public static IAsyncReactiveQbservable<TSource> SkipUntil<TSource>(this IAsyncReactiveQbservable<TSource> source, DateTimeOffset startTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(startTime, typeof(DateTimeOffset))));

        [KnownResource("iot://reactor/observables/skipwhile")]
        public static IAsyncReactiveQbservable<TSource> SkipWhile<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/skipwhile/indexed")]
        public static IAsyncReactiveQbservable<TSource> SkipWhile<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/startwith")]
        public static IAsyncReactiveQbservable<TSource> StartWith<TSource>(this IAsyncReactiveQbservable<TSource> source, TSource[] values) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(values, typeof(TSource[]))));

        [KnownResource("iot://reactor/observables/sum/float")]
        public static IAsyncReactiveQbservable<float> Sum(this IAsyncReactiveQbservable<float> source) => source.Provider.CreateQbservable<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/double")]
        public static IAsyncReactiveQbservable<double> Sum(this IAsyncReactiveQbservable<double> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/decimal")]
        public static IAsyncReactiveQbservable<decimal> Sum(this IAsyncReactiveQbservable<decimal> source) => source.Provider.CreateQbservable<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Sum(this IAsyncReactiveQbservable<int?> source) => source.Provider.CreateQbservable<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Sum(this IAsyncReactiveQbservable<long?> source) => source.Provider.CreateQbservable<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Sum(this IAsyncReactiveQbservable<float?> source) => source.Provider.CreateQbservable<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Sum(this IAsyncReactiveQbservable<double?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Sum(this IAsyncReactiveQbservable<decimal?> source) => source.Provider.CreateQbservable<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/int")]
        public static IAsyncReactiveQbservable<int> Sum(this IAsyncReactiveQbservable<int> source) => source.Provider.CreateQbservable<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/long")]
        public static IAsyncReactiveQbservable<long> Sum(this IAsyncReactiveQbservable<long> source) => source.Provider.CreateQbservable<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("iot://reactor/observables/sum/selector/float")]
        public static IAsyncReactiveQbservable<float> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float>> selector) => source.Provider.CreateQbservable<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/sum/selector/double")]
        public static IAsyncReactiveQbservable<double> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/sum/selector/decimal")]
        public static IAsyncReactiveQbservable<decimal> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal>> selector) => source.Provider.CreateQbservable<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/sum/selector/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int?>> selector) => source.Provider.CreateQbservable<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/sum/selector/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long?>> selector) => source.Provider.CreateQbservable<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/sum/selector/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float?>> selector) => source.Provider.CreateQbservable<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/sum/selector/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/sum/selector/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal?>> selector) => source.Provider.CreateQbservable<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/sum/selector/int")]
        public static IAsyncReactiveQbservable<int> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int>> selector) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/sum/selector/long")]
        public static IAsyncReactiveQbservable<long> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long>> selector) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("iot://reactor/observables/switch")]
        public static IAsyncReactiveQbservable<TSource> Switch<TSource>(this IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> sources) => sources.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), sources.Expression));

        [KnownResource("iot://reactor/observables/take/count")]
        public static IAsyncReactiveQbservable<TSource> Take<TSource>(this IAsyncReactiveQbservable<TSource> source, int count) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));

        [KnownResource("iot://reactor/observables/take/dueTime")]
        public static IAsyncReactiveQbservable<TSource> Take<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(dueTime, typeof(TimeSpan))));

        [KnownResource("iot://reactor/observables/takeuntil")]
        public static IAsyncReactiveQbservable<TSource> TakeUntil<TSource, TOther>(this IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> triggeringSource) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)), source.Expression, triggeringSource.Expression));

        [KnownResource("iot://reactor/observables/takeuntil/absolute")]
        public static IAsyncReactiveQbservable<TSource> TakeUntil<TSource>(this IAsyncReactiveQbservable<TSource> source, DateTimeOffset endTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(endTime, typeof(DateTimeOffset))));

        [KnownResource("iot://reactor/observables/takewhile")]
        public static IAsyncReactiveQbservable<TSource> TakeWhile<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/takewhile/indexed")]
        public static IAsyncReactiveQbservable<TSource> TakeWhile<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/throttle")]
        public static IAsyncReactiveQbservable<TSource> Throttle<TSource, TThrottle>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, IAsyncReactiveQbservable<TThrottle>>> throttleSelector) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TThrottle)), source.Expression, throttleSelector));

        [KnownResource("iot://reactor/observables/throttle/duration")]
        public static IAsyncReactiveQbservable<TSource> Throttle<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan))));

        [KnownResource("iot://reactor/observables/tolist")]
        public static IAsyncReactiveQbservable<IList<TSource>> ToList<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("iot://reactor/observables/where")]
        public static IAsyncReactiveQbservable<TSource> Where<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/where/indexed")]
        public static IAsyncReactiveQbservable<TSource> Where<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("iot://reactor/observables/window/duration")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan))));

        [KnownResource("iot://reactor/observables/window/count")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, int count) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));

        [KnownResource("iot://reactor/observables/window/duration/shift")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan)), Expression.Constant(shift, typeof(TimeSpan))));

        [KnownResource("iot://reactor/observables/window/count/skip")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, int count, int skip) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int)), Expression.Constant(skip, typeof(int))));

        [KnownResource("iot://reactor/observables/window/duration/count")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan)), Expression.Constant(count, typeof(int))));

        internal static async Task DefineAsync(ReactiveClientContext ctx, CancellationToken token)
        {
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/aggregate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TSource, TSource> aggregate) => Subscribable.Aggregate<TSource>(source.AsSubscribable(), aggregate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/aggregate/seed"), (IAsyncReactiveQbservable<TSource> source, TResult seed, Func<TResult, TSource, TResult> accumulate) => Subscribable.Aggregate<TSource, TResult>(source.AsSubscribable(), seed, accumulate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/aggregate/seed/result"), (IAsyncReactiveQbservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulate, Func<TAccumulate, TResult> resultSelector) => Subscribable.Aggregate<TSource, TAccumulate, TResult>(source.AsSubscribable(), seed, accumulate, resultSelector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/all"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.All<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/any"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Any<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/any/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.Any<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/int"), (IAsyncReactiveQbservable<int> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/long"), (IAsyncReactiveQbservable<long> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/float"), (IAsyncReactiveQbservable<float> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/double"), (IAsyncReactiveQbservable<double> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/decimal"), (IAsyncReactiveQbservable<decimal> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_int"), (IAsyncReactiveQbservable<int?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_long"), (IAsyncReactiveQbservable<long?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_float"), (IAsyncReactiveQbservable<float?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_double"), (IAsyncReactiveQbservable<double?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_decimal"), (IAsyncReactiveQbservable<decimal?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/buffer/duration"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => Subscribable.Buffer<TSource>(source.AsSubscribable(), duration).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/buffer/count"), (IAsyncReactiveQbservable<TSource> source, int count) => Subscribable.Buffer<TSource>(source.AsSubscribable(), count).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/buffer/duration/shift"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift) => Subscribable.Buffer<TSource>(source.AsSubscribable(), duration, shift).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/buffer/count/skip"), (IAsyncReactiveQbservable<TSource> source, int count, int skip) => Subscribable.Buffer<TSource>(source.AsSubscribable(), count, skip).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/buffer/duration/count"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count) => Subscribable.Buffer<TSource>(source.AsSubscribable(), duration, count).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/2"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, Func<TSource1, TSource2, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/3"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, Func<TSource1, TSource2, TSource3, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/4"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, Func<TSource1, TSource2, TSource3, TSource4, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), source4.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/5"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), source4.AsSubscribable(), source5.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/6"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, IAsyncReactiveQbservable<TSource6> source6, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), source4.AsSubscribable(), source5.AsSubscribable(), source6.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/7"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, IAsyncReactiveQbservable<TSource6> source6, IAsyncReactiveQbservable<TSource7> source7, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), source4.AsSubscribable(), source5.AsSubscribable(), source6.AsSubscribable(), source7.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/count"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Count<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/count/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.Count<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/delaysubscription/absolute"), (IAsyncReactiveQbservable<TSource> source, DateTimeOffset dueTime) => Subscribable.DelaySubscription<TSource>(source.AsSubscribable(), dueTime).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/delaysubscription/relative"), (IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => Subscribable.DelaySubscription<TSource>(source.AsSubscribable(), dueTime).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/distinctuntilchanged/"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.DistinctUntilChanged<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/distinctuntilchanged/comparer"), (IAsyncReactiveQbservable<TSource> source, IEqualityComparer<TSource> comparer) => Subscribable.DistinctUntilChanged<TSource>(source.AsSubscribable(), comparer).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/distinctuntilchanged/keySelector"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector) => Subscribable.DistinctUntilChanged<TSource, TKey>(source.AsSubscribable(), keySelector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/distinctuntilchanged/keySelector/comparer"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) => Subscribable.DistinctUntilChanged<TSource, TKey>(source.AsSubscribable(), keySelector, comparer).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/do/observer"), (IAsyncReactiveQbservable<TSource> source, IObserver<TSource> observer) => Subscribable.Do<TSource>(source.AsSubscribable(), observer).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/do/onNext"), (IAsyncReactiveQbservable<TSource> source, Action<TSource> onNext) => Subscribable.Do<TSource>(source.AsSubscribable(), onNext).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/do/onNext/onError"), (IAsyncReactiveQbservable<TSource> source, Action<TSource> onNext, Action<Exception> onError) => Subscribable.Do<TSource>(source.AsSubscribable(), onNext, onError).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/do/onNext/onCompleted"), (IAsyncReactiveQbservable<TSource> source, Action<TSource> onNext, Action onCompleted) => Subscribable.Do<TSource>(source.AsSubscribable(), onNext, onCompleted).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/do/selector/observer"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TNotification> selector, IObserver<TNotification> observer) => Subscribable.Do<TSource, TNotification>(source.AsSubscribable(), selector, observer).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/do/onNext/onError/onCompleted"), (IAsyncReactiveQbservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted) => Subscribable.Do<TSource>(source.AsSubscribable(), onNext, onError, onCompleted).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/finally"), (IAsyncReactiveQbservable<TSource> source, Action action) => Subscribable.Finally<TSource>(source.AsSubscribable(), action).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/first"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.FirstAsync<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/first/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.FirstAsync<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/firstordefault"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.FirstOrDefaultAsync<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/firstordefault/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.FirstOrDefaultAsync<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/groupby/keySelector"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector) => Subscribable.GroupBy<TSource, TKey>(source.AsSubscribable(), keySelector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/groupby/keySelector/comparer"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) => Subscribable.GroupBy<TSource, TKey>(source.AsSubscribable(), keySelector, comparer).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/groupby/keySelector/elementSelector"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) => Subscribable.GroupBy<TSource, TKey, TElement>(source.AsSubscribable(), keySelector, elementSelector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/groupby/keySelector/elementSelector/comparer"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer) => Subscribable.GroupBy<TSource, TKey, TElement>(source.AsSubscribable(), keySelector, elementSelector, comparer).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/isempty"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.IsEmpty<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/longcount"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.LongCount<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/longcount/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.LongCount<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Max<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/int"), (IAsyncReactiveQbservable<int> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/long"), (IAsyncReactiveQbservable<long> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/float"), (IAsyncReactiveQbservable<float> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/double"), (IAsyncReactiveQbservable<double> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/decimal"), (IAsyncReactiveQbservable<decimal> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_int"), (IAsyncReactiveQbservable<int?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_long"), (IAsyncReactiveQbservable<long?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_float"), (IAsyncReactiveQbservable<float?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_double"), (IAsyncReactiveQbservable<double?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_decimal"), (IAsyncReactiveQbservable<decimal?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/comparer"), (IAsyncReactiveQbservable<TSource> source, IComparer<TSource> comparer) => Subscribable.Max<TSource>(source.AsSubscribable(), comparer).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/merge"), (IAsyncReactiveQbservable<ISubscribable<TSource>> sources) => Subscribable.Merge<TSource>(sources.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/merge/2"), (IAsyncReactiveQbservable<TSource> first, IAsyncReactiveQbservable<TSource> second) => Subscribable.Merge<TSource>(first.AsSubscribable(), second.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Min<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/int"), (IAsyncReactiveQbservable<int> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/long"), (IAsyncReactiveQbservable<long> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/float"), (IAsyncReactiveQbservable<float> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/double"), (IAsyncReactiveQbservable<double> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/decimal"), (IAsyncReactiveQbservable<decimal> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_int"), (IAsyncReactiveQbservable<int?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_long"), (IAsyncReactiveQbservable<long?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_float"), (IAsyncReactiveQbservable<float?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_double"), (IAsyncReactiveQbservable<double?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_decimal"), (IAsyncReactiveQbservable<decimal?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/comparer"), (IAsyncReactiveQbservable<TSource> source, IComparer<TSource> comparer) => Subscribable.Min<TSource>(source.AsSubscribable(), comparer).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/retry"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Retry<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/retry/count"), (IAsyncReactiveQbservable<TSource> source, int retryCount) => Subscribable.Retry<TSource>(source.AsSubscribable(), retryCount).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sample/period"), (IAsyncReactiveQbservable<TSource> source, TimeSpan period) => Subscribable.Sample<TSource>(source.AsSubscribable(), period).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sample"), (IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TSample> sampler) => Subscribable.Sample<TSource, TSample>(source.AsSubscribable(), sampler.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/scan"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TSource, TSource> aggregate) => Subscribable.Scan<TSource>(source.AsSubscribable(), aggregate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/scan/seed"), (IAsyncReactiveQbservable<TSource> source, TResult seed, Func<TResult, TSource, TResult> accumulate) => Subscribable.Scan<TSource, TResult>(source.AsSubscribable(), seed, accumulate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/select"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TResult> selector) => Subscribable.Select<TSource, TResult>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/select/indexed"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int, TResult> selector) => Subscribable.Select<TSource, TResult>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/selectmany"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, ISubscribable<TResult>> selector) => Subscribable.SelectMany<TSource, TResult>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/selectmany/result"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, ISubscribable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) => Subscribable.SelectMany<TSource, TCollection, TResult>(source.AsSubscribable(), collectionSelector, resultSelector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sequenceequal"), (IAsyncReactiveQbservable<TSource> left, IAsyncReactiveQbservable<TSource> right) => Subscribable.SequenceEqual<TSource>(left.AsSubscribable(), right.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sequenceequal/comparer"), (IAsyncReactiveQbservable<TSource> left, IAsyncReactiveQbservable<TSource> right, IEqualityComparer<TSource> comparer) => Subscribable.SequenceEqual<TSource>(left.AsSubscribable(), right.AsSubscribable(), comparer).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/skip/count"), (IAsyncReactiveQbservable<TSource> source, int count) => Subscribable.Skip<TSource>(source.AsSubscribable(), count).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/skip/dueTime"), (IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => Subscribable.Skip<TSource>(source.AsSubscribable(), dueTime).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/skipuntil"), (IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> triggeringSource) => Subscribable.SkipUntil<TSource, TOther>(source.AsSubscribable(), triggeringSource.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/skipuntil/absolute"), (IAsyncReactiveQbservable<TSource> source, DateTimeOffset startTime) => Subscribable.SkipUntil<TSource>(source.AsSubscribable(), startTime).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/skipwhile"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.SkipWhile<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/skipwhile/indexed"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int, bool> predicate) => Subscribable.SkipWhile<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/startwith"), (IAsyncReactiveQbservable<TSource> source, TSource[] values) => Subscribable.StartWith<TSource>(source.AsSubscribable(), values).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/float"), (IAsyncReactiveQbservable<float> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/double"), (IAsyncReactiveQbservable<double> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/decimal"), (IAsyncReactiveQbservable<decimal> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_int"), (IAsyncReactiveQbservable<int?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_long"), (IAsyncReactiveQbservable<long?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_float"), (IAsyncReactiveQbservable<float?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_double"), (IAsyncReactiveQbservable<double?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_decimal"), (IAsyncReactiveQbservable<decimal?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/int"), (IAsyncReactiveQbservable<int> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/long"), (IAsyncReactiveQbservable<long> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/switch"), (IAsyncReactiveQbservable<ISubscribable<TSource>> sources) => Subscribable.Switch<TSource>(sources.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/take/count"), (IAsyncReactiveQbservable<TSource> source, int count) => Subscribable.Take<TSource>(source.AsSubscribable(), count).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/take/dueTime"), (IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => Subscribable.Take<TSource>(source.AsSubscribable(), dueTime).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/takeuntil"), (IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> triggeringSource) => Subscribable.TakeUntil<TSource, TOther>(source.AsSubscribable(), triggeringSource.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/takeuntil/absolute"), (IAsyncReactiveQbservable<TSource> source, DateTimeOffset endTime) => Subscribable.TakeUntil<TSource>(source.AsSubscribable(), endTime).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/takewhile"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.TakeWhile<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/takewhile/indexed"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int, bool> predicate) => Subscribable.TakeWhile<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/throttle"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, ISubscribable<TThrottle>> throttleSelector) => Subscribable.Throttle<TSource, TThrottle>(source.AsSubscribable(), throttleSelector).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/throttle/duration"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => Subscribable.Throttle<TSource>(source.AsSubscribable(), duration).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/tolist"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.ToList<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/where"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.Where<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/where/indexed"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int, bool> predicate) => Subscribable.Where<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/window/duration"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => Subscribable.Window<TSource>(source.AsSubscribable(), duration).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/window/count"), (IAsyncReactiveQbservable<TSource> source, int count) => Subscribable.Window<TSource>(source.AsSubscribable(), count).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/window/duration/shift"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift) => Subscribable.Window<TSource>(source.AsSubscribable(), duration, shift).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/window/count/skip"), (IAsyncReactiveQbservable<TSource> source, int count, int skip) => Subscribable.Window<TSource>(source.AsSubscribable(), count, skip).AsAsyncQbservable(), null, token).ConfigureAwait(false);
            await ctx.DefineObservableAsync(new Uri("iot://reactor/observables/window/duration/count"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count) => Subscribable.Window<TSource>(source.AsSubscribable(), duration, count).AsAsyncQbservable(), null, token).ConfigureAwait(false);
        }

        internal static async Task UndefineAsync(ReactiveClientContext ctx, CancellationToken token)
        {
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/aggregate"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/aggregate/seed"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/aggregate/seed/result"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/all"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/any"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/any/predicate"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/nullable_decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/average/selector/nullable_decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/buffer/duration"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/buffer/count"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/buffer/duration/shift"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/buffer/count/skip"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/buffer/duration/count"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/2"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/3"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/4"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/5"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/6"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/combinelatest/7"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/count"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/count/predicate"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/delaysubscription/absolute"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/delaysubscription/relative"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/distinctuntilchanged/"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/distinctuntilchanged/comparer"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/distinctuntilchanged/keySelector"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/distinctuntilchanged/keySelector/comparer"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/do/observer"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/do/onNext"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/do/onNext/onError"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/do/onNext/onCompleted"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/do/selector/observer"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/do/onNext/onError/onCompleted"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/finally"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/first"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/first/predicate"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/firstordefault"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/firstordefault/predicate"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/groupby/keySelector"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/groupby/keySelector/comparer"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/groupby/keySelector/elementSelector"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/groupby/keySelector/elementSelector/comparer"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/isempty"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/longcount"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/longcount/predicate"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/nullable_decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/comparer"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/max/selector/nullable_decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/merge"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/merge/2"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/nullable_decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/comparer"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/min/selector/nullable_decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/retry"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/retry/count"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sample/period"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sample"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/scan"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/scan/seed"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/select"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/select/indexed"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/selectmany"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/selectmany/result"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sequenceequal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sequenceequal/comparer"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/skip/count"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/skip/dueTime"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/skipuntil"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/skipuntil/absolute"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/skipwhile"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/skipwhile/indexed"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/startwith"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/nullable_decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_float"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_double"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/nullable_decimal"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/int"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/sum/selector/long"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/switch"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/take/count"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/take/dueTime"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/takeuntil"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/takeuntil/absolute"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/takewhile"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/takewhile/indexed"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/throttle"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/throttle/duration"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/tolist"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/where"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/where/indexed"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/window/duration"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/window/count"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/window/duration/shift"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/window/count/skip"), token).ConfigureAwait(false);
            await ctx.UndefineObservableAsync(new Uri("iot://reactor/observables/window/duration/count"), token).ConfigureAwait(false);
        }
    }

    [TypeWildcard] internal sealed class TSource { }
    [TypeWildcard] internal sealed class TResult { }
    [TypeWildcard] internal sealed class TAccumulate { }
    [TypeWildcard] internal sealed class TSource1 { }
    [TypeWildcard] internal sealed class TSource2 { }
    [TypeWildcard] internal sealed class TSource3 { }
    [TypeWildcard] internal sealed class TSource4 { }
    [TypeWildcard] internal sealed class TSource5 { }
    [TypeWildcard] internal sealed class TSource6 { }
    [TypeWildcard] internal sealed class TSource7 { }
    [TypeWildcard] internal sealed class TKey { }
    [TypeWildcard] internal sealed class TNotification { }
    [TypeWildcard] internal sealed class TElement { }
    [TypeWildcard] internal sealed class TSample { }
    [TypeWildcard] internal sealed class TCollection { }
    [TypeWildcard] internal sealed class TOther { }
    [TypeWildcard] internal sealed class TThrottle { }
}
#endif
