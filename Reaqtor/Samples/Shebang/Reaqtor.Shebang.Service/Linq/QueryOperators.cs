// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices.TypeSystem;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;

namespace Reaqtor.Shebang.Linq
{
    internal static class DeployQueryOperators
    {
        public static async Task DefineAsync(ReactiveClientContext ctx, CancellationToken token = default)
        {
            await ctx.DefineObservableAsync(new Uri("rx://observable/aggregate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TSource, TSource> aggregate) => Subscribable.Aggregate<TSource>(source.AsSubscribable(), aggregate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/aggregate/seed"), (IAsyncReactiveQbservable<TSource> source, TResult seed, Func<TResult, TSource, TResult> accumulate) => Subscribable.Aggregate<TSource, TResult>(source.AsSubscribable(), seed, accumulate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/aggregate/seed/result"), (IAsyncReactiveQbservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulate, Func<TAccumulate, TResult> resultSelector) => Subscribable.Aggregate<TSource, TAccumulate, TResult>(source.AsSubscribable(), seed, accumulate, resultSelector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/all"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.All<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/any"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Any<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/any/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.Any<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/int"), (IAsyncReactiveQbservable<int> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/long"), (IAsyncReactiveQbservable<long> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/float"), (IAsyncReactiveQbservable<float> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/double"), (IAsyncReactiveQbservable<double> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/decimal"), (IAsyncReactiveQbservable<decimal> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/nullable_int"), (IAsyncReactiveQbservable<int?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/nullable_long"), (IAsyncReactiveQbservable<long?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/nullable_float"), (IAsyncReactiveQbservable<float?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/nullable_double"), (IAsyncReactiveQbservable<double?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/nullable_decimal"), (IAsyncReactiveQbservable<decimal?> source) => Subscribable.Average(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/nullable_int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/nullable_long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/nullable_float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/nullable_double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/average/selector/nullable_decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal?> selector) => Subscribable.Average<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/buffer/duration"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => Subscribable.Buffer<TSource>(source.AsSubscribable(), duration).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/buffer/count"), (IAsyncReactiveQbservable<TSource> source, int count) => Subscribable.Buffer<TSource>(source.AsSubscribable(), count).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/buffer/duration/shift"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift) => Subscribable.Buffer<TSource>(source.AsSubscribable(), duration, shift).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/buffer/count/skip"), (IAsyncReactiveQbservable<TSource> source, int count, int skip) => Subscribable.Buffer<TSource>(source.AsSubscribable(), count, skip).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/buffer/duration/count"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count) => Subscribable.Buffer<TSource>(source.AsSubscribable(), duration, count).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/combinelatest/2"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, Func<TSource1, TSource2, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/combinelatest/3"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, Func<TSource1, TSource2, TSource3, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/combinelatest/4"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, Func<TSource1, TSource2, TSource3, TSource4, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), source4.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/combinelatest/5"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), source4.AsSubscribable(), source5.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/combinelatest/6"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, IAsyncReactiveQbservable<TSource6> source6, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), source4.AsSubscribable(), source5.AsSubscribable(), source6.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/combinelatest/7"), (IAsyncReactiveQbservable<TSource1> source1, IAsyncReactiveQbservable<TSource2> source2, IAsyncReactiveQbservable<TSource3> source3, IAsyncReactiveQbservable<TSource4> source4, IAsyncReactiveQbservable<TSource5> source5, IAsyncReactiveQbservable<TSource6> source6, IAsyncReactiveQbservable<TSource7> source7, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> selector) => Subscribable.CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1.AsSubscribable(), source2.AsSubscribable(), source3.AsSubscribable(), source4.AsSubscribable(), source5.AsSubscribable(), source6.AsSubscribable(), source7.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/count"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Count<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/count/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.Count<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/delaysubscription/absolute"), (IAsyncReactiveQbservable<TSource> source, DateTimeOffset dueTime) => Subscribable.DelaySubscription<TSource>(source.AsSubscribable(), dueTime).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/delaysubscription/relative"), (IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => Subscribable.DelaySubscription<TSource>(source.AsSubscribable(), dueTime).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/distinctuntilchanged/"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.DistinctUntilChanged<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/distinctuntilchanged/comparer"), (IAsyncReactiveQbservable<TSource> source, IEqualityComparer<TSource> comparer) => Subscribable.DistinctUntilChanged<TSource>(source.AsSubscribable(), comparer).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/distinctuntilchanged/keySelector"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector) => Subscribable.DistinctUntilChanged<TSource, TKey>(source.AsSubscribable(), keySelector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/distinctuntilchanged/keySelector/comparer"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) => Subscribable.DistinctUntilChanged<TSource, TKey>(source.AsSubscribable(), keySelector, comparer).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/do/observer"), (IAsyncReactiveQbservable<TSource> source, IObserver<TSource> observer) => Subscribable.Do<TSource>(source.AsSubscribable(), observer).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/do/onNext"), (IAsyncReactiveQbservable<TSource> source, Action<TSource> onNext) => Subscribable.Do<TSource>(source.AsSubscribable(), onNext).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/do/onNext/onError"), (IAsyncReactiveQbservable<TSource> source, Action<TSource> onNext, Action<Exception> onError) => Subscribable.Do<TSource>(source.AsSubscribable(), onNext, onError).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/do/onNext/onCompleted"), (IAsyncReactiveQbservable<TSource> source, Action<TSource> onNext, Action onCompleted) => Subscribable.Do<TSource>(source.AsSubscribable(), onNext, onCompleted).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/do/selector/observer"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TNotification> selector, IObserver<TNotification> observer) => Subscribable.Do<TSource, TNotification>(source.AsSubscribable(), selector, observer).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/do/onNext/onError/onCompleted"), (IAsyncReactiveQbservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted) => Subscribable.Do<TSource>(source.AsSubscribable(), onNext, onError, onCompleted).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/finally"), (IAsyncReactiveQbservable<TSource> source, Action action) => Subscribable.Finally<TSource>(source.AsSubscribable(), action).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/first"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.FirstAsync<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/first/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.FirstAsync<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/firstordefault"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.FirstOrDefaultAsync<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/firstordefault/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.FirstOrDefaultAsync<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/groupby/keySelector"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector) => Subscribable.GroupBy<TSource, TKey>(source.AsSubscribable(), keySelector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/groupby/keySelector/comparer"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) => Subscribable.GroupBy<TSource, TKey>(source.AsSubscribable(), keySelector, comparer).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/groupby/keySelector/elementSelector"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) => Subscribable.GroupBy<TSource, TKey, TElement>(source.AsSubscribable(), keySelector, elementSelector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/groupby/keySelector/elementSelector/comparer"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer) => Subscribable.GroupBy<TSource, TKey, TElement>(source.AsSubscribable(), keySelector, elementSelector, comparer).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/isempty"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.IsEmpty<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/longcount"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.LongCount<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/longcount/predicate"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.LongCount<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Max<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/int"), (IAsyncReactiveQbservable<int> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/long"), (IAsyncReactiveQbservable<long> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/float"), (IAsyncReactiveQbservable<float> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/double"), (IAsyncReactiveQbservable<double> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/decimal"), (IAsyncReactiveQbservable<decimal> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/nullable_int"), (IAsyncReactiveQbservable<int?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/nullable_long"), (IAsyncReactiveQbservable<long?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/nullable_float"), (IAsyncReactiveQbservable<float?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/nullable_double"), (IAsyncReactiveQbservable<double?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/nullable_decimal"), (IAsyncReactiveQbservable<decimal?> source) => Subscribable.Max(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/comparer"), (IAsyncReactiveQbservable<TSource> source, IComparer<TSource> comparer) => Subscribable.Max<TSource>(source.AsSubscribable(), comparer).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/nullable_int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/nullable_long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/nullable_float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/nullable_double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/max/selector/nullable_decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal?> selector) => Subscribable.Max<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/merge"), (IAsyncReactiveQbservable<ISubscribable<TSource>> sources) => Subscribable.Merge<TSource>(sources.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/merge/2"), (IAsyncReactiveQbservable<TSource> first, IAsyncReactiveQbservable<TSource> second) => Subscribable.Merge<TSource>(first.AsSubscribable(), second.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Min<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/int"), (IAsyncReactiveQbservable<int> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/long"), (IAsyncReactiveQbservable<long> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/float"), (IAsyncReactiveQbservable<float> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/double"), (IAsyncReactiveQbservable<double> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/decimal"), (IAsyncReactiveQbservable<decimal> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/nullable_int"), (IAsyncReactiveQbservable<int?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/nullable_long"), (IAsyncReactiveQbservable<long?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/nullable_float"), (IAsyncReactiveQbservable<float?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/nullable_double"), (IAsyncReactiveQbservable<double?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/nullable_decimal"), (IAsyncReactiveQbservable<decimal?> source) => Subscribable.Min(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/comparer"), (IAsyncReactiveQbservable<TSource> source, IComparer<TSource> comparer) => Subscribable.Min<TSource>(source.AsSubscribable(), comparer).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/nullable_int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/nullable_long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/nullable_float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/nullable_double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/min/selector/nullable_decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal?> selector) => Subscribable.Min<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/retry"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.Retry<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/retry/count"), (IAsyncReactiveQbservable<TSource> source, int retryCount) => Subscribable.Retry<TSource>(source.AsSubscribable(), retryCount).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sample/period"), (IAsyncReactiveQbservable<TSource> source, TimeSpan period) => Subscribable.Sample<TSource>(source.AsSubscribable(), period).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sample"), (IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TSample> sampler) => Subscribable.Sample<TSource, TSample>(source.AsSubscribable(), sampler.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/scan"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TSource, TSource> aggregate) => Subscribable.Scan<TSource>(source.AsSubscribable(), aggregate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/scan/seed"), (IAsyncReactiveQbservable<TSource> source, TResult seed, Func<TResult, TSource, TResult> accumulate) => Subscribable.Scan<TSource, TResult>(source.AsSubscribable(), seed, accumulate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/select"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, TResult> selector) => Subscribable.Select<TSource, TResult>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/select/indexed"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int, TResult> selector) => Subscribable.Select<TSource, TResult>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/selectmany"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, ISubscribable<TResult>> selector) => Subscribable.SelectMany<TSource, TResult>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/selectmany/result"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, ISubscribable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) => Subscribable.SelectMany<TSource, TCollection, TResult>(source.AsSubscribable(), collectionSelector, resultSelector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sequenceequal"), (IAsyncReactiveQbservable<TSource> left, IAsyncReactiveQbservable<TSource> right) => Subscribable.SequenceEqual<TSource>(left.AsSubscribable(), right.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sequenceequal/comparer"), (IAsyncReactiveQbservable<TSource> left, IAsyncReactiveQbservable<TSource> right, IEqualityComparer<TSource> comparer) => Subscribable.SequenceEqual<TSource>(left.AsSubscribable(), right.AsSubscribable(), comparer).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/skip/count"), (IAsyncReactiveQbservable<TSource> source, int count) => Subscribable.Skip<TSource>(source.AsSubscribable(), count).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/skip/dueTime"), (IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => Subscribable.Skip<TSource>(source.AsSubscribable(), dueTime).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/skipuntil"), (IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> triggeringSource) => Subscribable.SkipUntil<TSource, TOther>(source.AsSubscribable(), triggeringSource.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/skipuntil/absolute"), (IAsyncReactiveQbservable<TSource> source, DateTimeOffset startTime) => Subscribable.SkipUntil<TSource>(source.AsSubscribable(), startTime).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/skipwhile"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.SkipWhile<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/skipwhile/indexed"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int, bool> predicate) => Subscribable.SkipWhile<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/startwith"), (IAsyncReactiveQbservable<TSource> source, TSource[] values) => Subscribable.StartWith<TSource>(source.AsSubscribable(), values).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/float"), (IAsyncReactiveQbservable<float> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/double"), (IAsyncReactiveQbservable<double> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/decimal"), (IAsyncReactiveQbservable<decimal> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/nullable_int"), (IAsyncReactiveQbservable<int?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/nullable_long"), (IAsyncReactiveQbservable<long?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/nullable_float"), (IAsyncReactiveQbservable<float?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/nullable_double"), (IAsyncReactiveQbservable<double?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/nullable_decimal"), (IAsyncReactiveQbservable<decimal?> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/int"), (IAsyncReactiveQbservable<int> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/long"), (IAsyncReactiveQbservable<long> source) => Subscribable.Sum(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_float"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, float?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_double"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, double?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_decimal"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, decimal?> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/int"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/sum/selector/long"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, long> selector) => Subscribable.Sum<TSource>(source.AsSubscribable(), selector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/switch"), (IAsyncReactiveQbservable<ISubscribable<TSource>> sources) => Subscribable.Switch<TSource>(sources.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/take/count"), (IAsyncReactiveQbservable<TSource> source, int count) => Subscribable.Take<TSource>(source.AsSubscribable(), count).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/take/dueTime"), (IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime) => Subscribable.Take<TSource>(source.AsSubscribable(), dueTime).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/takeuntil"), (IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> triggeringSource) => Subscribable.TakeUntil<TSource, TOther>(source.AsSubscribable(), triggeringSource.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/takeuntil/absolute"), (IAsyncReactiveQbservable<TSource> source, DateTimeOffset endTime) => Subscribable.TakeUntil<TSource>(source.AsSubscribable(), endTime).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/takewhile"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.TakeWhile<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/takewhile/indexed"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int, bool> predicate) => Subscribable.TakeWhile<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/throttle"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, ISubscribable<TThrottle>> throttleSelector) => Subscribable.Throttle<TSource, TThrottle>(source.AsSubscribable(), throttleSelector).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/throttle/duration"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => Subscribable.Throttle<TSource>(source.AsSubscribable(), duration).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/tolist"), (IAsyncReactiveQbservable<TSource> source) => Subscribable.ToList<TSource>(source.AsSubscribable()).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/where"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, bool> predicate) => Subscribable.Where<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/where/indexed"), (IAsyncReactiveQbservable<TSource> source, Func<TSource, int, bool> predicate) => Subscribable.Where<TSource>(source.AsSubscribable(), predicate).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/window/duration"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration) => Subscribable.Window<TSource>(source.AsSubscribable(), duration).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/window/count"), (IAsyncReactiveQbservable<TSource> source, int count) => Subscribable.Window<TSource>(source.AsSubscribable(), count).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/window/duration/shift"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift) => Subscribable.Window<TSource>(source.AsSubscribable(), duration, shift).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/window/count/skip"), (IAsyncReactiveQbservable<TSource> source, int count, int skip) => Subscribable.Window<TSource>(source.AsSubscribable(), count, skip).AsAsyncQbservable(), null, token);
            await ctx.DefineObservableAsync(new Uri("rx://observable/window/duration/count"), (IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count) => Subscribable.Window<TSource>(source.AsSubscribable(), duration, count).AsAsyncQbservable(), null, token);
        }

        public static async Task UndefineAsync(ReactiveClientContext ctx, CancellationToken token = default)
        {
            await ctx.UndefineObservableAsync(new Uri("rx://observable/aggregate"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/aggregate/seed"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/aggregate/seed/result"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/all"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/any"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/any/predicate"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/nullable_int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/nullable_long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/nullable_float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/nullable_double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/nullable_decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/nullable_int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/nullable_long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/nullable_float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/nullable_double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/average/selector/nullable_decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/buffer/duration"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/buffer/count"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/buffer/duration/shift"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/buffer/count/skip"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/buffer/duration/count"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/combinelatest/2"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/combinelatest/3"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/combinelatest/4"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/combinelatest/5"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/combinelatest/6"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/combinelatest/7"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/count"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/count/predicate"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/delaysubscription/absolute"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/delaysubscription/relative"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/distinctuntilchanged/"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/distinctuntilchanged/comparer"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/distinctuntilchanged/keySelector"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/distinctuntilchanged/keySelector/comparer"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/do/observer"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/do/onNext"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/do/onNext/onError"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/do/onNext/onCompleted"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/do/selector/observer"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/do/onNext/onError/onCompleted"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/finally"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/first"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/first/predicate"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/firstordefault"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/firstordefault/predicate"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/groupby/keySelector"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/groupby/keySelector/comparer"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/groupby/keySelector/elementSelector"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/groupby/keySelector/elementSelector/comparer"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/isempty"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/longcount"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/longcount/predicate"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/nullable_int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/nullable_long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/nullable_float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/nullable_double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/nullable_decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/comparer"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/nullable_int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/nullable_long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/nullable_float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/nullable_double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/max/selector/nullable_decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/merge"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/merge/2"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/nullable_int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/nullable_long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/nullable_float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/nullable_double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/nullable_decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/comparer"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/nullable_int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/nullable_long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/nullable_float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/nullable_double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/min/selector/nullable_decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/retry"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/retry/count"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sample/period"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sample"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/scan"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/scan/seed"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/select"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/select/indexed"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/selectmany"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/selectmany/result"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sequenceequal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sequenceequal/comparer"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/skip/count"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/skip/dueTime"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/skipuntil"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/skipuntil/absolute"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/skipwhile"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/skipwhile/indexed"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/startwith"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/nullable_int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/nullable_long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/nullable_float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/nullable_double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/nullable_decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_float"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_double"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/nullable_decimal"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/int"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/sum/selector/long"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/switch"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/take/count"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/take/dueTime"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/takeuntil"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/takeuntil/absolute"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/takewhile"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/takewhile/indexed"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/throttle"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/throttle/duration"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/tolist"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/where"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/where/indexed"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/window/duration"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/window/count"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/window/duration/shift"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/window/count/skip"), token);
            await ctx.UndefineObservableAsync(new Uri("rx://observable/window/duration/count"), token);
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
