// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Shebang.Linq
{
    public static class QueryOperators
    {
        [KnownResource("rx://observable/aggregate")]
        public static IAsyncReactiveQbservable<TSource> Aggregate<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TSource, TSource>> aggregate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, aggregate));

        [KnownResource("rx://observable/aggregate/seed")]
        public static IAsyncReactiveQbservable<TResult> Aggregate<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, TResult seed, Expression<Func<TResult, TSource, TResult>> accumulate) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TResult)), accumulate));

        [KnownResource("rx://observable/aggregate/seed/result")]
        public static IAsyncReactiveQbservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncReactiveQbservable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulate, Expression<Func<TAccumulate, TResult>> resultSelector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulate, resultSelector));

        [KnownResource("rx://observable/all")]
        public static IAsyncReactiveQbservable<bool> All<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/any")]
        public static IAsyncReactiveQbservable<bool> Any<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/any/predicate")]
        public static IAsyncReactiveQbservable<bool> Any<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/average/int")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<int> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/long")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<long> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/float")]
        public static IAsyncReactiveQbservable<float> Average(this IAsyncReactiveQbservable<float> source) => source.Provider.CreateQbservable<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/double")]
        public static IAsyncReactiveQbservable<double> Average(this IAsyncReactiveQbservable<double> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/decimal")]
        public static IAsyncReactiveQbservable<decimal> Average(this IAsyncReactiveQbservable<decimal> source) => source.Provider.CreateQbservable<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/nullable_int")]
        public static IAsyncReactiveQbservable<double?> Average(this IAsyncReactiveQbservable<int?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/nullable_long")]
        public static IAsyncReactiveQbservable<double?> Average(this IAsyncReactiveQbservable<long?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Average(this IAsyncReactiveQbservable<float?> source) => source.Provider.CreateQbservable<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Average(this IAsyncReactiveQbservable<double?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Average(this IAsyncReactiveQbservable<decimal?> source) => source.Provider.CreateQbservable<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/average/selector/int")]
        public static IAsyncReactiveQbservable<double> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/average/selector/long")]
        public static IAsyncReactiveQbservable<double> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/average/selector/float")]
        public static IAsyncReactiveQbservable<float> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float>> selector) => source.Provider.CreateQbservable<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/average/selector/double")]
        public static IAsyncReactiveQbservable<double> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/average/selector/decimal")]
        public static IAsyncReactiveQbservable<decimal> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal>> selector) => source.Provider.CreateQbservable<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/average/selector/nullable_int")]
        public static IAsyncReactiveQbservable<double?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/average/selector/nullable_long")]
        public static IAsyncReactiveQbservable<double?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/average/selector/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float?>> selector) => source.Provider.CreateQbservable<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/average/selector/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/average/selector/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Average<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal?>> selector) => source.Provider.CreateQbservable<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/buffer/duration")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan))));

        [KnownResource("rx://observable/buffer/count")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, int count) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));

        [KnownResource("rx://observable/buffer/duration/shift")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan)), Expression.Constant(shift, typeof(TimeSpan))));

        [KnownResource("rx://observable/buffer/count/skip")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, int count, int skip) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int)), Expression.Constant(skip, typeof(int))));

        [KnownResource("rx://observable/buffer/duration/count")]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan)), Expression.Constant(count, typeof(int))));

        [KnownResource("rx://observable/combinelatest/2")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, Expression<Func<TSource1, TSource2, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TResult)), source1.Expression, source2.Expression, selector));

        [KnownResource("rx://observable/combinelatest/3")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, Expression<Func<TSource1, TSource2, TSource3, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, selector));

        [KnownResource("rx://observable/combinelatest/4")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, Expression<Func<TSource1, TSource2, TSource3, TSource4, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TSource4), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, source4.Expression, selector));

        [KnownResource("rx://observable/combinelatest/5")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TSource4), typeof(TSource5), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, source4.Expression, source5.Expression, selector));

        [KnownResource("rx://observable/combinelatest/6")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, IAsyncReactiveQbservable<TSource6> source6, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TSource4), typeof(TSource5), typeof(TSource6), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, source4.Expression, source5.Expression, source6.Expression, selector));

        [KnownResource("rx://observable/combinelatest/7")]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, IAsyncReactiveQbservable<TSource6> source6, IAsyncReactiveQbservable<TSource7> source7, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>> selector) => source1.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TSource3), typeof(TSource4), typeof(TSource5), typeof(TSource6), typeof(TSource7), typeof(TResult)), source1.Expression, source2.Expression, source3.Expression, source4.Expression, source5.Expression, source6.Expression, source7.Expression, selector));

        [KnownResource("rx://observable/count")]
        public static IAsyncReactiveQbservable<int> Count<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/count/predicate")]
        public static IAsyncReactiveQbservable<int> Count<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/delaysubscription/absolute")]
        public static IAsyncReactiveQbservable<TSource> DelaySubscription<TSource>(this IAsyncReactiveQbservable<TSource> source, DateTimeOffset dueTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(dueTime, typeof(DateTimeOffset))));

        [KnownResource("rx://observable/delaysubscription/relative")]
        public static IAsyncReactiveQbservable<TSource> DelaySubscription<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(dueTime, typeof(TimeSpan))));

        [KnownResource("rx://observable/distinctuntilchanged/")]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/distinctuntilchanged/comparer")]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource>(this IAsyncReactiveQbservable<TSource> source, IEqualityComparer<TSource> comparer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));

        [KnownResource("rx://observable/distinctuntilchanged/keySelector")]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));

        [KnownResource("rx://observable/distinctuntilchanged/keySelector/comparer")]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));

        [KnownResource("rx://observable/do/observer")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, IObserver<TSource> observer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(observer, typeof(IObserver<TSource>))));

        [KnownResource("rx://observable/do/onNext")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action<TSource>> onNext) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext));

        [KnownResource("rx://observable/do/onNext/onError")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError));

        [KnownResource("rx://observable/do/onNext/onCompleted")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action> onCompleted) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onCompleted));

        [KnownResource("rx://observable/do/selector/observer")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource, TNotification>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TNotification>> selector, IObserver<TNotification> observer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TNotification)), source.Expression, selector, Expression.Constant(observer, typeof(IObserver<TNotification>))));

        [KnownResource("rx://observable/do/onNext/onError/onCompleted")]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError, Expression<Action> onCompleted) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError, onCompleted));

        [KnownResource("rx://observable/finally")]
        public static IAsyncReactiveQbservable<TSource> Finally<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Action> action) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, action));

        [KnownResource("rx://observable/first")]
        public static IAsyncReactiveQbservable<TSource> FirstAsync<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/first/predicate")]
        public static IAsyncReactiveQbservable<TSource> FirstAsync<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/firstordefault")]
        public static IAsyncReactiveQbservable<TSource> FirstOrDefaultAsync<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/firstordefault/predicate")]
        public static IAsyncReactiveQbservable<TSource> FirstOrDefaultAsync<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/groupby/keySelector")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector) => source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));

        [KnownResource("rx://observable/groupby/keySelector/comparer")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer) => source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));

        [KnownResource("rx://observable/groupby/keySelector/elementSelector")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector) => source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector));

        [KnownResource("rx://observable/groupby/keySelector/elementSelector/comparer")]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer) => source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));

        [KnownResource("rx://observable/isempty")]
        public static IAsyncReactiveQbservable<bool> IsEmpty<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/longcount")]
        public static IAsyncReactiveQbservable<long> LongCount<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/longcount/predicate")]
        public static IAsyncReactiveQbservable<long> LongCount<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/max")]
        public static IAsyncReactiveQbservable<TSource> Max<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/max/int")]
        public static IAsyncReactiveQbservable<int> Max(this IAsyncReactiveQbservable<int> source) => source.Provider.CreateQbservable<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/long")]
        public static IAsyncReactiveQbservable<long> Max(this IAsyncReactiveQbservable<long> source) => source.Provider.CreateQbservable<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/float")]
        public static IAsyncReactiveQbservable<float> Max(this IAsyncReactiveQbservable<float> source) => source.Provider.CreateQbservable<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/double")]
        public static IAsyncReactiveQbservable<double> Max(this IAsyncReactiveQbservable<double> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/decimal")]
        public static IAsyncReactiveQbservable<decimal> Max(this IAsyncReactiveQbservable<decimal> source) => source.Provider.CreateQbservable<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Max(this IAsyncReactiveQbservable<int?> source) => source.Provider.CreateQbservable<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Max(this IAsyncReactiveQbservable<long?> source) => source.Provider.CreateQbservable<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Max(this IAsyncReactiveQbservable<float?> source) => source.Provider.CreateQbservable<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Max(this IAsyncReactiveQbservable<double?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Max(this IAsyncReactiveQbservable<decimal?> source) => source.Provider.CreateQbservable<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/max/comparer")]
        public static IAsyncReactiveQbservable<TSource> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, IComparer<TSource> comparer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>))));

        [KnownResource("rx://observable/max/selector/int")]
        public static IAsyncReactiveQbservable<int> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int>> selector) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/max/selector/long")]
        public static IAsyncReactiveQbservable<long> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long>> selector) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/max/selector/float")]
        public static IAsyncReactiveQbservable<float> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float>> selector) => source.Provider.CreateQbservable<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/max/selector/double")]
        public static IAsyncReactiveQbservable<double> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/max/selector/decimal")]
        public static IAsyncReactiveQbservable<decimal> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal>> selector) => source.Provider.CreateQbservable<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/max/selector/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int?>> selector) => source.Provider.CreateQbservable<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/max/selector/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long?>> selector) => source.Provider.CreateQbservable<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/max/selector/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float?>> selector) => source.Provider.CreateQbservable<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/max/selector/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/max/selector/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Max<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal?>> selector) => source.Provider.CreateQbservable<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/merge")]
        public static IAsyncReactiveQbservable<TSource> Merge<TSource>(this IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> sources) => sources.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), sources.Expression));

        [KnownResource("rx://observable/merge/2")]
        public static IAsyncReactiveQbservable<TSource> Merge<TSource>(this IAsyncReactiveQbservable<TSource> first, IAsyncReactiveQbservable<TSource> second) => first.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, second.Expression));

        [KnownResource("rx://observable/min")]
        public static IAsyncReactiveQbservable<TSource> Min<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/min/int")]
        public static IAsyncReactiveQbservable<int> Min(this IAsyncReactiveQbservable<int> source) => source.Provider.CreateQbservable<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/long")]
        public static IAsyncReactiveQbservable<long> Min(this IAsyncReactiveQbservable<long> source) => source.Provider.CreateQbservable<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/float")]
        public static IAsyncReactiveQbservable<float> Min(this IAsyncReactiveQbservable<float> source) => source.Provider.CreateQbservable<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/double")]
        public static IAsyncReactiveQbservable<double> Min(this IAsyncReactiveQbservable<double> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/decimal")]
        public static IAsyncReactiveQbservable<decimal> Min(this IAsyncReactiveQbservable<decimal> source) => source.Provider.CreateQbservable<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Min(this IAsyncReactiveQbservable<int?> source) => source.Provider.CreateQbservable<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Min(this IAsyncReactiveQbservable<long?> source) => source.Provider.CreateQbservable<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Min(this IAsyncReactiveQbservable<float?> source) => source.Provider.CreateQbservable<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Min(this IAsyncReactiveQbservable<double?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Min(this IAsyncReactiveQbservable<decimal?> source) => source.Provider.CreateQbservable<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/min/comparer")]
        public static IAsyncReactiveQbservable<TSource> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, IComparer<TSource> comparer) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>))));

        [KnownResource("rx://observable/min/selector/int")]
        public static IAsyncReactiveQbservable<int> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int>> selector) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/min/selector/long")]
        public static IAsyncReactiveQbservable<long> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long>> selector) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/min/selector/float")]
        public static IAsyncReactiveQbservable<float> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float>> selector) => source.Provider.CreateQbservable<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/min/selector/double")]
        public static IAsyncReactiveQbservable<double> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/min/selector/decimal")]
        public static IAsyncReactiveQbservable<decimal> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal>> selector) => source.Provider.CreateQbservable<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/min/selector/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int?>> selector) => source.Provider.CreateQbservable<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/min/selector/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long?>> selector) => source.Provider.CreateQbservable<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/min/selector/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float?>> selector) => source.Provider.CreateQbservable<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/min/selector/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/min/selector/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Min<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal?>> selector) => source.Provider.CreateQbservable<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/retry")]
        public static IAsyncReactiveQbservable<TSource> Retry<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/retry/count")]
        public static IAsyncReactiveQbservable<TSource> Retry<TSource>(this IAsyncReactiveQbservable<TSource> source, int retryCount) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(retryCount, typeof(int))));

        [KnownResource("rx://observable/sample/period")]
        public static IAsyncReactiveQbservable<TSource> Sample<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan period) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(period, typeof(TimeSpan))));

        [KnownResource("rx://observable/sample")]
        public static IAsyncReactiveQbservable<TSource> Sample<TSource, TSample>(this IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TSample> sampler) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TSample)), source.Expression, sampler.Expression));

        [KnownResource("rx://observable/scan")]
        public static IAsyncReactiveQbservable<TSource> Scan<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TSource, TSource>> aggregate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, aggregate));

        [KnownResource("rx://observable/scan/seed")]
        public static IAsyncReactiveQbservable<TResult> Scan<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, TResult seed, Expression<Func<TResult, TSource, TResult>> accumulate) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TResult)), accumulate));

        [KnownResource("rx://observable/select")]
        public static IAsyncReactiveQbservable<TResult> Select<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TResult>> selector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));

        [KnownResource("rx://observable/select/indexed")]
        public static IAsyncReactiveQbservable<TResult> Select<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, TResult>> selector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));

        [KnownResource("rx://observable/selectmany")]
        public static IAsyncReactiveQbservable<TResult> SelectMany<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, IAsyncReactiveQbservable<TResult>>> selector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));

        [KnownResource("rx://observable/selectmany/result")]
        public static IAsyncReactiveQbservable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, IAsyncReactiveQbservable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) => source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));

        [KnownResource("rx://observable/sequenceequal")]
        public static IAsyncReactiveQbservable<bool> SequenceEqual<TSource>(this IAsyncReactiveQbservable<TSource> left, IAsyncReactiveQbservable<TSource> right) => left.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), left.Expression, right.Expression));

        [KnownResource("rx://observable/sequenceequal/comparer")]
        public static IAsyncReactiveQbservable<bool> SequenceEqual<TSource>(this IAsyncReactiveQbservable<TSource> left, IAsyncReactiveQbservable<TSource> right, IEqualityComparer<TSource> comparer) => left.Provider.CreateQbservable<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), left.Expression, right.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));

        [KnownResource("rx://observable/skip/count")]
        public static IAsyncReactiveQbservable<TSource> Skip<TSource>(this IAsyncReactiveQbservable<TSource> source, int count) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));

        [KnownResource("rx://observable/skip/dueTime")]
        public static IAsyncReactiveQbservable<TSource> Skip<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(dueTime, typeof(TimeSpan))));

        [KnownResource("rx://observable/skipuntil")]
        public static IAsyncReactiveQbservable<TSource> SkipUntil<TSource, TOther>(this IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> triggeringSource) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)), source.Expression, triggeringSource.Expression));

        [KnownResource("rx://observable/skipuntil/absolute")]
        public static IAsyncReactiveQbservable<TSource> SkipUntil<TSource>(this IAsyncReactiveQbservable<TSource> source, DateTimeOffset startTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(startTime, typeof(DateTimeOffset))));

        [KnownResource("rx://observable/skipwhile")]
        public static IAsyncReactiveQbservable<TSource> SkipWhile<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/skipwhile/indexed")]
        public static IAsyncReactiveQbservable<TSource> SkipWhile<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/startwith")]
        public static IAsyncReactiveQbservable<TSource> StartWith<TSource>(this IAsyncReactiveQbservable<TSource> source, TSource[] values) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(values, typeof(TSource[]))));

        [KnownResource("rx://observable/sum/float")]
        public static IAsyncReactiveQbservable<float> Sum(this IAsyncReactiveQbservable<float> source) => source.Provider.CreateQbservable<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/double")]
        public static IAsyncReactiveQbservable<double> Sum(this IAsyncReactiveQbservable<double> source) => source.Provider.CreateQbservable<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/decimal")]
        public static IAsyncReactiveQbservable<decimal> Sum(this IAsyncReactiveQbservable<decimal> source) => source.Provider.CreateQbservable<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Sum(this IAsyncReactiveQbservable<int?> source) => source.Provider.CreateQbservable<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Sum(this IAsyncReactiveQbservable<long?> source) => source.Provider.CreateQbservable<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Sum(this IAsyncReactiveQbservable<float?> source) => source.Provider.CreateQbservable<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Sum(this IAsyncReactiveQbservable<double?> source) => source.Provider.CreateQbservable<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Sum(this IAsyncReactiveQbservable<decimal?> source) => source.Provider.CreateQbservable<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/int")]
        public static IAsyncReactiveQbservable<int> Sum(this IAsyncReactiveQbservable<int> source) => source.Provider.CreateQbservable<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/long")]
        public static IAsyncReactiveQbservable<long> Sum(this IAsyncReactiveQbservable<long> source) => source.Provider.CreateQbservable<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression));

        [KnownResource("rx://observable/sum/selector/float")]
        public static IAsyncReactiveQbservable<float> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float>> selector) => source.Provider.CreateQbservable<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/sum/selector/double")]
        public static IAsyncReactiveQbservable<double> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double>> selector) => source.Provider.CreateQbservable<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/sum/selector/decimal")]
        public static IAsyncReactiveQbservable<decimal> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal>> selector) => source.Provider.CreateQbservable<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/sum/selector/nullable_int")]
        public static IAsyncReactiveQbservable<int?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int?>> selector) => source.Provider.CreateQbservable<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/sum/selector/nullable_long")]
        public static IAsyncReactiveQbservable<long?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long?>> selector) => source.Provider.CreateQbservable<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/sum/selector/nullable_float")]
        public static IAsyncReactiveQbservable<float?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, float?>> selector) => source.Provider.CreateQbservable<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/sum/selector/nullable_double")]
        public static IAsyncReactiveQbservable<double?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, double?>> selector) => source.Provider.CreateQbservable<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/sum/selector/nullable_decimal")]
        public static IAsyncReactiveQbservable<decimal?> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, decimal?>> selector) => source.Provider.CreateQbservable<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/sum/selector/int")]
        public static IAsyncReactiveQbservable<int> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int>> selector) => source.Provider.CreateQbservable<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/sum/selector/long")]
        public static IAsyncReactiveQbservable<long> Sum<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, long>> selector) => source.Provider.CreateQbservable<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));

        [KnownResource("rx://observable/switch")]
        public static IAsyncReactiveQbservable<TSource> Switch<TSource>(this IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> sources) => sources.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), sources.Expression));

        [KnownResource("rx://observable/take/count")]
        public static IAsyncReactiveQbservable<TSource> Take<TSource>(this IAsyncReactiveQbservable<TSource> source, int count) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));

        [KnownResource("rx://observable/take/dueTime")]
        public static IAsyncReactiveQbservable<TSource> Take<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(dueTime, typeof(TimeSpan))));

        [KnownResource("rx://observable/takeuntil")]
        public static IAsyncReactiveQbservable<TSource> TakeUntil<TSource, TOther>(this IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> triggeringSource) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)), source.Expression, triggeringSource.Expression));

        [KnownResource("rx://observable/takeuntil/absolute")]
        public static IAsyncReactiveQbservable<TSource> TakeUntil<TSource>(this IAsyncReactiveQbservable<TSource> source, DateTimeOffset endTime) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(endTime, typeof(DateTimeOffset))));

        [KnownResource("rx://observable/takewhile")]
        public static IAsyncReactiveQbservable<TSource> TakeWhile<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/takewhile/indexed")]
        public static IAsyncReactiveQbservable<TSource> TakeWhile<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/throttle")]
        public static IAsyncReactiveQbservable<TSource> Throttle<TSource, TThrottle>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, IAsyncReactiveQbservable<TThrottle>>> throttleSelector) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TThrottle)), source.Expression, throttleSelector));

        [KnownResource("rx://observable/throttle/duration")]
        public static IAsyncReactiveQbservable<TSource> Throttle<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan))));

        [KnownResource("rx://observable/tolist")]
        public static IAsyncReactiveQbservable<IList<TSource>> ToList<TSource>(this IAsyncReactiveQbservable<TSource> source) => source.Provider.CreateQbservable<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));

        [KnownResource("rx://observable/where")]
        public static IAsyncReactiveQbservable<TSource> Where<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/where/indexed")]
        public static IAsyncReactiveQbservable<TSource> Where<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, int, bool>> predicate) => source.Provider.CreateQbservable<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));

        [KnownResource("rx://observable/window/duration")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan))));

        [KnownResource("rx://observable/window/count")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, int count) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));

        [KnownResource("rx://observable/window/duration/shift")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan)), Expression.Constant(shift, typeof(TimeSpan))));

        [KnownResource("rx://observable/window/count/skip")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, int count, int skip) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int)), Expression.Constant(skip, typeof(int))));

        [KnownResource("rx://observable/window/duration/count")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count) => source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(duration, typeof(TimeSpan)), Expression.Constant(count, typeof(int))));

    }
}
