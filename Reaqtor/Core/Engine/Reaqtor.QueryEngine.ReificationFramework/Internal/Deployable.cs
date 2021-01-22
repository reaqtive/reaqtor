// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if FALSE // NB: Disabled until reification framework host is ported and artifacts are refactored out of remoting into separate libraries.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;

using Nuqleon.DataModel.TypeSystem;
using Reaqtor.Expressions.Core;
using Reaqtor.QueryEngine.Mocks;
using Reaqtor.Remoting.Deployable;

using ReactiveConstants = Reaqtor.Remoting.Client.Constants;
using PlatformConstants = Reaqtor.Remoting.Platform.Constants;

namespace Reaqtor.QueryEngine.ReificationFramework
{
    static partial class Deployable
    {

        public static void Deploy(ReactiveServiceContext context)
        {
            DefineOperators(context);
            DefineObservers(context);
        }

        public static void DefineStreamFactories(ReactiveServiceContext context, SubjectManager manager)
        {
            Expression<Func<IReactiveQubject>> untypedExpr = () => manager.CreateUntyped().To<IMultiSubject, IReactiveQubject>();
            var untypedFactory = context.Provider.CreateQubjectFactory<T, T>(untypedExpr);
            context.DefineStreamFactory(PlatformConstants.Identifiers.Observable.FireHose.Uri, untypedFactory, null);
        }

        private static void DefineOperators(ReactiveServiceContext context)
        {
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, T, T>>, T>(new Uri(ReactiveConstants.Observable.Aggregate.Accumulate), (source, accumulate) => source.Aggregate(accumulate), null);
            context.DefineObservable<IReactiveQbservable<T>, R, Expression<Func<R, T, R>>, R>(new Uri(ReactiveConstants.Observable.Aggregate.Seed), (source, seed, accumulate) => source.Aggregate(seed, accumulate), null);
            context.DefineObservable<IReactiveQbservable<T1>, T2, Expression<Func<T2, T1, T2>>, Expression<Func<T2, R>>, R>(new Uri(ReactiveConstants.Observable.Aggregate.SeedResult), (source, seed, accumulate, selector) => source.Aggregate(seed, accumulate, selector), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, bool>>, bool>(new Uri(ReactiveConstants.Observable.All.Predicate), (source, predicate) => source.All(predicate), null);
            context.DefineObservable<IReactiveQbservable<T>, bool>(new Uri(ReactiveConstants.Observable.Any.NoArgument), source => source.Any(), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, bool>>, bool>(new Uri(ReactiveConstants.Observable.Any.Predicate), (source, predicate) => source.Any(predicate), null);
            context.DefineObservable<IReactiveQbservable<T1>, IReactiveQbservable<T2>, Expression<Func<T1, T2, R>>, R>(new Uri(ReactiveConstants.Observable.CombineLatest.ObservableFunc), (source, otherSource, selector) => source.CombineLatest(otherSource, selector), null);
            context.DefineObservable<IReactiveQbservable<T>, int>(new Uri(ReactiveConstants.Observable.Count.NoArgument), source => source.Count(), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, bool>>, int>(new Uri(ReactiveConstants.Observable.Count.Predicate), (source, predicate) => source.Count(predicate), null);
            context.DefineObservable<IReactiveQbservable<T>, DateTimeOffset, T>(new Uri(ReactiveConstants.Observable.DelaySubscription.DateTimeOffset), (source, dueTime) => source.DelaySubscription(dueTime), null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, T>(new Uri(ReactiveConstants.Observable.DelaySubscription.TimeSpan), (source, dueTime) => source.DelaySubscription(dueTime), null);
            context.DefineObservable<IReactiveQbservable<T>, T>(new Uri(ReactiveConstants.Observable.DistinctUntilChanged.NoArgument), source => source.DistinctUntilChanged(DataTypeEqualityComparer<T>.Default), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, R>>, T>(new Uri(ReactiveConstants.Observable.DistinctUntilChanged.Func), (source, keySelector) => source.DistinctUntilChanged(keySelector, DataTypeEqualityComparer<R>.Default), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Action<T>>, T>(new Uri(ReactiveConstants.Observable.Do.OnNext), (source, onNext) => source.Do(onNext), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Action<T>>, Expression<Action<Exception>>, T>(new Uri(ReactiveConstants.Observable.Do.OnNextOnError), (source, onNext, onError) => source.Do(onNext, onError), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Action<T>>, Expression<Action>, T>(new Uri(ReactiveConstants.Observable.Do.OnNextOnCompleted), (source, onNext, onCompleted) => source.Do(onNext, onCompleted), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Action<T>>, Expression<Action<Exception>>, Expression<Action>, T>(new Uri(ReactiveConstants.Observable.Do.AllActions), (source, onNext, onError, onCompleted) => source.Do(onNext, onError, onCompleted), null);
            context.DefineObservable<IReactiveQbservable<T>, IReactiveQbserver<T>, T>(new Uri(ReactiveConstants.Observable.Do.Observer), (source, observer) => source.Do(observer), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, R>>, IReactiveQbserver<R>, T>(new Uri(ReactiveConstants.Observable.Do.ObserverSelector), (source, selector, observer) => source.Do(selector, observer), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Action>, T>(new Uri(ReactiveConstants.Observable.Finally.Action), (source, action) => source.Finally(action), null);
            context.DefineObservable<IReactiveQbservable<T>, T>(new Uri(ReactiveConstants.Observable.FirstAsync.NoArgument), source => source.FirstAsync(), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(ReactiveConstants.Observable.FirstAsync.Func), (source, predicate) => source.FirstAsync(predicate), null);
            context.DefineObservable<IReactiveQbservable<T>, T>(new Uri(ReactiveConstants.Observable.FirstOrDefaultAsync.NoArgument), source => source.FirstOrDefaultAsync(), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(ReactiveConstants.Observable.FirstOrDefaultAsync.Func), (source, predicate) => source.FirstOrDefaultAsync(predicate), null);
            context.DefineObservable<IReactiveQbservable<T>, bool>(new Uri(ReactiveConstants.Observable.IsEmpty.NoArgument), source => source.IsEmpty(), null);
            context.DefineObservable<IReactiveQbservable<T>, long>(new Uri(ReactiveConstants.Observable.LongCount.NoArgument), source => source.LongCount(), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, bool>>, long>(new Uri(ReactiveConstants.Observable.LongCount.Predicate), (source, predicate) => source.LongCount(predicate), null);
            context.DefineObservable<IReactiveQbservable<T>, T>(new Uri(ReactiveConstants.Observable.Retry.NoArgument), source => source.Retry(), null);
            context.DefineObservable<IReactiveQbservable<T>, int, T>(new Uri(ReactiveConstants.Observable.Retry.Count), (source, retryCount) => source.Retry(retryCount), null);
            context.DefineObservable<T, T>(new Uri(ReactiveConstants.Observable.Return.Value), value => ReactiveQbservable.Return(value), null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, T>(new Uri(ReactiveConstants.Observable.Sample.TimeSpan), (source, interval) => source.Sample(interval), null);
            context.DefineObservable<IReactiveQbservable<T>, IReactiveQbservable<R>, T>(new Uri(ReactiveConstants.Observable.Sample.Observable), (source, sampler) => source.Sample(sampler), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, T, T>>, T>(new Uri(ReactiveConstants.Observable.Scan.Accumulate), (source, aggregate) => source.Scan(aggregate), null);
            context.DefineObservable<IReactiveQbservable<T>, R, Expression<Func<R, T, R>>, R>(new Uri(ReactiveConstants.Observable.Scan.Seed), (source, seed, aggregate) => source.Scan(seed, aggregate), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, R>>, R>(new Uri(ReactiveConstants.Observable.Select.Func), (source, selector) => source.Select(selector), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, int, R>>, R>(new Uri(ReactiveConstants.Observable.Select.IndexedFunc), (source, selector) => source.Select(selector), null);
            context.DefineObservable<IReactiveQbservable<T>, IReactiveQbservable<T>, bool>(new Uri(ReactiveConstants.Observable.SequenceEqual.NoArgument), (left, right) => left.SequenceEqual(right), null);
            context.DefineObservable<IReactiveQbservable<T>, int, T>(new Uri(ReactiveConstants.Observable.Skip.Int), (source, count) => source.Skip(count), null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, T>(new Uri(ReactiveConstants.Observable.Skip.TimeSpan), (source, dueTime) => source.Skip(dueTime), null);
            context.DefineObservable<IReactiveQbservable<T>, DateTimeOffset, T>(new Uri(ReactiveConstants.Observable.SkipUntil.DateTimeOffset), (source, dueTime) => source.SkipUntil(dueTime), null);
            context.DefineObservable<IReactiveQbservable<T1>, IReactiveQbservable<T2>, T1>(new Uri(ReactiveConstants.Observable.SkipUntil.Observable), (source, triggeringSource) => source.SkipUntil(triggeringSource), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(ReactiveConstants.Observable.SkipWhile.Func), (source, predicate) => source.SkipWhile(predicate), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, int, bool>>, T>(new Uri(ReactiveConstants.Observable.SkipWhile.IndexedFunc), (source, predicate) => source.SkipWhile(predicate), null);
            context.DefineObservable<IReactiveQbservable<T>, T[], T>(new Uri(ReactiveConstants.Observable.StartWith.Array), (source, values) => source.StartWith(values), null);
            context.DefineObservable<IReactiveQbservable<T>, int, T>(new Uri(ReactiveConstants.Observable.Take.Int), (source, count) => source.Take(count), null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, T>(new Uri(ReactiveConstants.Observable.Take.TimeSpan), (source, duration) => source.Take(duration), null);
            context.DefineObservable<IReactiveQbservable<T>, DateTimeOffset, T>(new Uri(ReactiveConstants.Observable.TakeUntil.DateTimeOffset), (source, startTime) => source.TakeUntil(startTime), null);
            context.DefineObservable<IReactiveQbservable<T1>, IReactiveQbservable<T2>, T1>(new Uri(ReactiveConstants.Observable.TakeUntil.Observable), (source, other) => source.TakeUntil(other), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(ReactiveConstants.Observable.TakeWhile.Func), (source, predicate) => source.TakeWhile(predicate), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, int, bool>>, T>(new Uri(ReactiveConstants.Observable.TakeWhile.IndexedFunc), (source, predicate) => source.TakeWhile(predicate), null);
            context.DefineObservable<DateTimeOffset, long>(new Uri(ReactiveConstants.Observable.Timer.DateTimeOffset), dueTime => ReactiveQbservable.Timer(dueTime), null);
            context.DefineObservable<DateTimeOffset, TimeSpan, long>(new Uri(ReactiveConstants.Observable.Timer.DateTimeOffsetTimeSpan), (dueTime, period) => ReactiveQbservable.Timer(dueTime, period), null);
            context.DefineObservable<TimeSpan, long>(new Uri(ReactiveConstants.Observable.Timer.TimeSpan), dueTime => ReactiveQbservable.Timer(dueTime), null);
            context.DefineObservable<TimeSpan, TimeSpan, long>(new Uri(ReactiveConstants.Observable.Timer.TimeSpanTimeSpan), (dueTime, period) => ReactiveQbservable.Timer(dueTime, period), null);
            context.DefineObservable<IReactiveQbservable<T>, IList<T>>(new Uri(ReactiveConstants.Observable.ToList.NoArgument), source => source.ToList(), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(ReactiveConstants.Observable.Where.Func), (source, predicate) => source.Where(predicate), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, int, bool>>, T>(new Uri(ReactiveConstants.Observable.Where.IndexedFunc), (source, predicate) => source.Where(predicate), null);

            // Non-parameterized defines
            var emptyFactory = (Expression<Func<IReactiveQbservable<T>>>)(() => ReactiveQbservable.Empty<T>());
            var emptyObservable = context.Provider.CreateQbservable<T>(emptyFactory.Body);
            context.DefineObservable<T>(new Uri(ReactiveConstants.Observable.Empty.NoArgument), emptyObservable, null);

            var neverFactory = (Expression<Func<IReactiveQbservable<T>>>)(() => ReactiveQbservable.Never<T>());
            var neverObservable = context.Provider.CreateQbservable<T>(neverFactory.Body);
            context.DefineObservable<T>(new Uri(ReactiveConstants.Observable.Never.NoArgument), neverObservable, null);

            // Higher order defines
            context.DefineObservable<IReactiveQbservable<IReactiveObservable<T>>, T>(new Uri(ReactiveConstants.Observable.Merge.NoArgument), source => source.To<IReactiveQbservable<IReactiveObservable<T>>, IReactiveQbservable<IReactiveObservable<T>>>().Merge(), null);
            context.DefineObservable<IReactiveQbservable<IReactiveObservable<T>>, T>(new Uri(ReactiveConstants.Observable.Switch.NoArgument), source => source.To<IReactiveQbservable<IReactiveObservable<T>>, IReactiveQbservable<IReactiveObservable<T>>>().Switch(), null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, T>(new Uri(ReactiveConstants.Observable.Throttle.TimeSpan), (source, dueTime) => source.Throttle(_ => ReactiveQbservable.Timer(dueTime)), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, IReactiveQbservable<R>>>, R>(
                new Uri(ReactiveConstants.Observable.SelectMany.Func),
                (source, selector) => source.SelectMany(selector.To<Expression<Func<T, IReactiveQbservable<R>>>, Expression<Func<T, IReactiveQbservable<R>>>>()),
                null);
            context.DefineObservable<IReactiveQbservable<T1>, Expression<Func<T1, IReactiveQbservable<T2>>>, Expression<Func<T1, T2, R>>, R>(
                new Uri(ReactiveConstants.Observable.SelectMany.FuncFunc),
                (source, collectionSelector, resultSelector) => source.SelectMany(collectionSelector.To<Expression<Func<T1, IReactiveQbservable<T2>>>, Expression<Func<T1, IReactiveQbservable<T2>>>>(), resultSelector),
                null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, IReactiveQbservable<T>>(new Uri(ReactiveConstants.Observable.Window.TimeDuration), (source, duration) => source.Window(duration).To<IReactiveQbservable<IReactiveQbservable<T>>, IReactiveQbservable<IReactiveQbservable<T>>>(), null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, TimeSpan, IReactiveQbservable<T>>(new Uri(ReactiveConstants.Observable.Window.TimeDurationShift), (source, duration, shift) => source.Window(duration, shift).To<IReactiveQbservable<IReactiveQbservable<T>>, IReactiveQbservable<IReactiveQbservable<T>>>(), null);
            context.DefineObservable<IReactiveQbservable<T>, int, IReactiveQbservable<T>>(new Uri(ReactiveConstants.Observable.Window.Count), (source, count) => source.Window(count).To<IReactiveQbservable<IReactiveQbservable<T>>, IReactiveQbservable<IReactiveQbservable<T>>>(), null);
            context.DefineObservable<IReactiveQbservable<T>, int, int, IReactiveQbservable<T>>(new Uri(ReactiveConstants.Observable.Window.CountSkip), (source, count, skip) => source.Window(count, skip).To<IReactiveQbservable<IReactiveQbservable<T>>, IReactiveQbservable<IReactiveQbservable<T>>>(), null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, int, IReactiveQbservable<T>>(new Uri(ReactiveConstants.Observable.Window.TimeCount), (source, duration, count) => source.Window(duration, count).To<IReactiveQbservable<IReactiveQbservable<T>>, IReactiveQbservable<IReactiveQbservable<T>>>(), null);

            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, IList<T>>(new Uri(ReactiveConstants.Observable.Buffer.TimeDuration), (source, duration) => source.Buffer(duration).To<IReactiveQbservable<IList<T>>, IReactiveQbservable<IList<T>>>(), null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, TimeSpan, IList<T>>(new Uri(ReactiveConstants.Observable.Buffer.TimeDurationShift), (source, duration, shift) => source.Buffer(duration, shift).To<IReactiveQbservable<IList<T>>, IReactiveQbservable<IList<T>>>(), null);
            context.DefineObservable<IReactiveQbservable<T>, int, IList<T>>(new Uri(ReactiveConstants.Observable.Buffer.Count), (source, count) => source.Buffer(count).To<IReactiveQbservable<IList<T>>, IReactiveQbservable<IList<T>>>(), null);
            context.DefineObservable<IReactiveQbservable<T>, int, int, IList<T>>(new Uri(ReactiveConstants.Observable.Buffer.CountSkip), (source, count, skip) => source.Buffer(count, skip).To<IReactiveQbservable<IList<T>>, IReactiveQbservable<IList<T>>>(), null);
            context.DefineObservable<IReactiveQbservable<T>, TimeSpan, int, IList<T>>(new Uri(ReactiveConstants.Observable.Buffer.TimeCount), (source, duration, count) => source.Buffer(duration, count).To<IReactiveQbservable<IList<T>>, IReactiveQbservable<IList<T>>>(), null);

            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, T1>>, IReactiveGroupedQbservable<T1, T>>(new Uri(ReactiveConstants.Observable.GroupBy.Key), (source, keySelector) => source.GroupBy(keySelector), null);
            context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, T1>>, Expression<Func<T, T2>>, IReactiveGroupedQbservable<T1, T2>>(new Uri(ReactiveConstants.Observable.GroupBy.KeyElement), (source, keySelector, elementSelector) => source.GroupBy(keySelector, elementSelector), null);

            // Extension defines
            DefineOperatorsExtension(context);
        }

        private static void DefineObservers(ReactiveServiceContext context)
        {
            var consoleObserverFactory = (Expression<Func<IReactiveQbserver<T>>>)(() => new ConsoleObserver<T>().AsReactiveQbserver());
            var consoleObserver = context.Provider.CreateQbserver<T>(consoleObserverFactory.Body);
            context.DefineObserver<T>(PlatformConstants.Identifiers.Observer.ConsoleObserver.Uri, consoleObserver, null);

            context.DefineObserver<string, T>(PlatformConstants.Identifiers.Observer.ConsoleObserverParam.Uri, prefix => new ConsoleObserver<T>(prefix).AsReactiveQbserver(), null);

            var traceObserverFactory = (Expression<Func<IReactiveQbserver<T>>>)(() => new TraceObserver<T>().AsReactiveQbserver());
            var traceObserver = context.Provider.CreateQbserver<T>(traceObserverFactory.Body);
            context.DefineObserver<T>(PlatformConstants.Identifiers.Observer.TraceObserver.Uri, traceObserver, null);

            context.DefineObserver<string, T>(PlatformConstants.Identifiers.Observer.TraceObserverParam.Uri, prefix => new TraceObserver<T>(prefix).AsReactiveQbserver(), null);

            context.DefineObserver<string, T>(PlatformConstants.Identifiers.Observer.Throughput.Uri, s => new ThroughputObserver<T>(s).AsReactiveQbserver(), null);

            var nopObserverFactory = (Expression<Func<IReactiveQbserver<T>>>)(() => NopObserver<T>.Instance.AsReactiveQbserver());
            var nopObserver = context.Provider.CreateQbserver<T>(nopObserverFactory.Body);
            context.DefineObserver<T>(new Uri(ReactiveConstants.Observer.Nop), nopObserver, null);
        }

        [KnownResource(Constants.IdentityFunctionUri)]
        private static IReactiveQbserver<T> AsReactiveQbserver<T>(this IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}

#endif
