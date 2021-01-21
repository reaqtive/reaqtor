// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Nuqleon.DataModel.TypeSystem;

using Reaqtive;

using Reaqtor.Expressions.Core;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Platform.Firehose;

namespace Reaqtor.Remoting.Deployable
{
    public partial class Deployable : IDeployable
    {
        private readonly bool _doUndefine;

        public Deployable(bool doUndefine = false)
        {
            _doUndefine = doUndefine;
        }

        public void Execute(ReactiveClientContext context)
        {
            if (_doUndefine)
            {
                UndefineObservables(context).Wait();
                UndefineOperators(context).Wait();
                UndefineObservers(context).Wait();
            }

            DefineObservables(context).Wait();
            DefineOperators(context).Wait();
            DefineObservers(context).Wait();
        }

        protected virtual async Task UndefineOperators(ReactiveClientContext context)
        {
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Aggregate.Accumulate), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Aggregate.Seed), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Aggregate.SeedResult), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.All.Predicate), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Any.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Any.Predicate), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Buffer.TimeDuration), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Buffer.TimeDurationShift), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Buffer.Count), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Buffer.CountSkip), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Buffer.TimeCount), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.CombineLatest.ObservableFunc), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Count.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Count.Predicate), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.DelaySubscription.DateTimeOffset), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.DelaySubscription.TimeSpan), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.DelaySubscription.V1.DateTimeOffset), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.DelaySubscription.V1.TimeSpan), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.DistinctUntilChanged.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.DistinctUntilChanged.Func), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Do.OnNext), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Do.OnNextOnError), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Do.OnNextOnCompleted), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Do.AllActions), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Do.Observer), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Do.ObserverSelector), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Empty.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Finally.Action), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.FirstAsync.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.FirstAsync.Func), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.FirstOrDefaultAsync.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.FirstOrDefaultAsync.Func), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.GroupBy.Key), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.GroupBy.KeyElement), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.IsEmpty.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.LongCount.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.LongCount.Predicate), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Merge.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Merge.Binary), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Merge.N), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Never.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Return.Value), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Retry.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Retry.Count), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sample.TimeSpan), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sample.Observable), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Scan.Accumulate), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Scan.Seed), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Select.Func), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Select.IndexedFunc), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.SelectMany.Func), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.SelectMany.FuncFunc), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.SequenceEqual.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Skip.Int), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Skip.TimeSpan), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.SkipUntil.DateTimeOffset), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.SkipUntil.Observable), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.SkipWhile.Func), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.SkipWhile.IndexedFunc), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.StartWith.Array), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Switch.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Take.Int), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Take.TimeSpan), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.TakeUntil.DateTimeOffset), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.TakeUntil.Observable), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.TakeWhile.Func), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.TakeWhile.IndexedFunc), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Throttle.TimeSpan), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Timer.DateTimeOffset), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Timer.DateTimeOffsetTimeSpan), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Timer.TimeSpan), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Timer.TimeSpanTimeSpan), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.ToList.NoArgument), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Where.Func), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Where.IndexedFunc), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Window.TimeDuration), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Window.TimeDurationShift), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Window.Count), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Window.CountSkip), CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Window.TimeCount), CancellationToken.None));

            await UndefineOperatorsExtension(context);
        }

        protected virtual async Task DefineOperators(ReactiveClientContext context)
        {
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, T, T>>, T>(new Uri(Client.Constants.Observable.Aggregate.Accumulate), (source, accumulate) => source.AsSync().Aggregate(accumulate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, R, Expression<Func<R, T, R>>, R>(new Uri(Client.Constants.Observable.Aggregate.Seed), (source, seed, accumulate) => source.AsSync().Aggregate(seed, accumulate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T1>, T2, Expression<Func<T2, T1, T2>>, Expression<Func<T2, R>>, R>(new Uri(Client.Constants.Observable.Aggregate.SeedResult), (source, seed, accumulate, selector) => source.AsSync().Aggregate(seed, accumulate, selector).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, bool>>, bool>(new Uri(Client.Constants.Observable.All.Predicate), (source, predicate) => source.AsSync().All(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, bool>(new Uri(Client.Constants.Observable.Any.NoArgument), source => source.AsSync().Any().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, bool>>, bool>(new Uri(Client.Constants.Observable.Any.Predicate), (source, predicate) => source.AsSync().Any(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T1>, IAsyncReactiveQbservable<T2>, Expression<Func<T1, T2, R>>, R>(new Uri(Client.Constants.Observable.CombineLatest.ObservableFunc), (source, otherSource, selector) => source.AsSync().CombineLatest(otherSource.AsSync(), selector).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, int>(new Uri(Client.Constants.Observable.Count.NoArgument), source => source.AsSync().Count().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, bool>>, int>(new Uri(Client.Constants.Observable.Count.Predicate), (source, predicate) => source.AsSync().Count(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, DateTimeOffset, T>(new Uri(Client.Constants.Observable.DelaySubscription.DateTimeOffset), (source, dueTime) => source.AsSync().DelaySubscription(dueTime).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, T>(new Uri(Client.Constants.Observable.DelaySubscription.TimeSpan), (source, dueTime) => source.AsSync().DelaySubscription(dueTime).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, DateTimeOffset, T>(new Uri(Client.Constants.Observable.DelaySubscription.V1.DateTimeOffset), (source, dueTime) => ReactiveQbservable.Timer(dueTime).SelectMany(_ => source.AsSync()).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, T>(new Uri(Client.Constants.Observable.DelaySubscription.V1.TimeSpan), (source, dueTime) => ReactiveQbservable.Timer(dueTime).SelectMany(_ => source.AsSync()).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, T>(new Uri(Client.Constants.Observable.DistinctUntilChanged.NoArgument), source => source.AsSync().DistinctUntilChanged(DataTypeEqualityComparer<T>.Default).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, R>>, T>(new Uri(Client.Constants.Observable.DistinctUntilChanged.Func), (source, keySelector) => source.AsSync().DistinctUntilChanged(keySelector, DataTypeEqualityComparer<R>.Default).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Action<T>>, T>(new Uri(Client.Constants.Observable.Do.OnNext), (source, onNext) => source.AsSync().Do(onNext).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Action<T>>, Expression<Action<Exception>>, T>(new Uri(Client.Constants.Observable.Do.OnNextOnError), (source, onNext, onError) => source.AsSync().Do(onNext, onError).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Action<T>>, Expression<Action>, T>(new Uri(Client.Constants.Observable.Do.OnNextOnCompleted), (source, onNext, onCompleted) => source.AsSync().Do(onNext, onCompleted).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Action<T>>, Expression<Action<Exception>>, Expression<Action>, T>(new Uri(Client.Constants.Observable.Do.AllActions), (source, onNext, onError, onCompleted) => source.AsSync().Do(onNext, onError, onCompleted).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, IAsyncReactiveQbserver<T>, T>(new Uri(Client.Constants.Observable.Do.Observer), (source, observer) => source.AsSync().Do(observer.AsSync()).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, R>>, IAsyncReactiveQbserver<R>, T>(new Uri(Client.Constants.Observable.Do.ObserverSelector), (source, selector, observer) => source.AsSync().Do(selector, observer.AsSync()).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Action>, T>(new Uri(Client.Constants.Observable.Finally.Action), (source, action) => source.AsSync().Finally(action).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, T>(new Uri(Client.Constants.Observable.FirstAsync.NoArgument), source => source.AsSync().FirstAsync().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(Client.Constants.Observable.FirstAsync.Func), (source, predicate) => source.AsSync().FirstAsync(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, T>(new Uri(Client.Constants.Observable.FirstOrDefaultAsync.NoArgument), source => source.AsSync().FirstOrDefaultAsync().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(Client.Constants.Observable.FirstOrDefaultAsync.Func), (source, predicate) => source.AsSync().FirstOrDefaultAsync(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, bool>(new Uri(Client.Constants.Observable.IsEmpty.NoArgument), source => source.AsSync().IsEmpty().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, long>(new Uri(Client.Constants.Observable.LongCount.NoArgument), source => source.AsSync().LongCount().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, bool>>, long>(new Uri(Client.Constants.Observable.LongCount.Predicate), (source, predicate) => source.AsSync().LongCount(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, T>(new Uri(Client.Constants.Observable.Retry.NoArgument), source => source.AsSync().Retry().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, T>(new Uri(Client.Constants.Observable.Retry.Count), (source, retryCount) => source.AsSync().Retry(retryCount).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<T, T>(new Uri(Client.Constants.Observable.Return.Value), value => ReactiveQbservable.Return(value).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, T>(new Uri(Client.Constants.Observable.Sample.TimeSpan), (source, interval) => source.AsSync().Sample(interval).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, IAsyncReactiveQbservable<R>, T>(new Uri(Client.Constants.Observable.Sample.Observable), (source, sampler) => source.AsSync().Sample(sampler.AsSync()).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, T, T>>, T>(new Uri(Client.Constants.Observable.Scan.Accumulate), (source, aggregate) => source.AsSync().Scan(aggregate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, R, Expression<Func<R, T, R>>, R>(new Uri(Client.Constants.Observable.Scan.Seed), (source, seed, aggregate) => source.AsSync().Scan(seed, aggregate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, R>>, R>(new Uri(Client.Constants.Observable.Select.Func), (source, selector) => source.AsSync().Select(selector).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, int, R>>, R>(new Uri(Client.Constants.Observable.Select.IndexedFunc), (source, selector) => source.AsSync().Select(selector).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, IAsyncReactiveQbservable<T>, bool>(new Uri(Client.Constants.Observable.SequenceEqual.NoArgument), (left, right) => left.AsSync().SequenceEqual(right.AsSync()).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, T>(new Uri(Client.Constants.Observable.Skip.Int), (source, count) => source.AsSync().Skip(count).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, T>(new Uri(Client.Constants.Observable.Skip.TimeSpan), (source, dueTime) => source.AsSync().Skip(dueTime).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, DateTimeOffset, T>(new Uri(Client.Constants.Observable.SkipUntil.DateTimeOffset), (source, dueTime) => source.AsSync().SkipUntil(dueTime).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T1>, IAsyncReactiveQbservable<T2>, T1>(new Uri(Client.Constants.Observable.SkipUntil.Observable), (source, triggeringSource) => source.AsSync().SkipUntil(triggeringSource.AsSync()).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(Client.Constants.Observable.SkipWhile.Func), (source, predicate) => source.AsSync().SkipWhile(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, int, bool>>, T>(new Uri(Client.Constants.Observable.SkipWhile.IndexedFunc), (source, predicate) => source.AsSync().SkipWhile(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, T[], T>(new Uri(Client.Constants.Observable.StartWith.Array), (source, values) => source.AsSync().StartWith(values).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, T>(new Uri(Client.Constants.Observable.Take.Int), (source, count) => source.AsSync().Take(count).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, T>(new Uri(Client.Constants.Observable.Take.TimeSpan), (source, duration) => source.AsSync().Take(duration).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, DateTimeOffset, T>(new Uri(Client.Constants.Observable.TakeUntil.DateTimeOffset), (source, startTime) => source.AsSync().TakeUntil(startTime).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T1>, IAsyncReactiveQbservable<T2>, T1>(new Uri(Client.Constants.Observable.TakeUntil.Observable), (source, other) => source.AsSync().TakeUntil(other.AsSync()).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(Client.Constants.Observable.TakeWhile.Func), (source, predicate) => source.AsSync().TakeWhile(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, int, bool>>, T>(new Uri(Client.Constants.Observable.TakeWhile.IndexedFunc), (source, predicate) => source.AsSync().TakeWhile(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<DateTimeOffset, long>(new Uri(Client.Constants.Observable.Timer.DateTimeOffset), dueTime => ReactiveQbservable.Timer(dueTime).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<DateTimeOffset, TimeSpan, long>(new Uri(Client.Constants.Observable.Timer.DateTimeOffsetTimeSpan), (dueTime, period) => ReactiveQbservable.Timer(dueTime, period).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<TimeSpan, long>(new Uri(Client.Constants.Observable.Timer.TimeSpan), dueTime => ReactiveQbservable.Timer(dueTime).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<TimeSpan, TimeSpan, long>(new Uri(Client.Constants.Observable.Timer.TimeSpanTimeSpan), (dueTime, period) => ReactiveQbservable.Timer(dueTime, period).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, IList<T>>(new Uri(Client.Constants.Observable.ToList.NoArgument), source => source.AsSync().ToList().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, bool>>, T>(new Uri(Client.Constants.Observable.Where.Func), (source, predicate) => source.AsSync().Where(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, int, bool>>, T>(new Uri(Client.Constants.Observable.Where.IndexedFunc), (source, predicate) => source.AsSync().Where(predicate).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, IAsyncReactiveQbservable<T>, T>(new Uri(Client.Constants.Observable.Merge.Binary), (left, right) => left.To<IAsyncReactiveQbservable<T>, IReactiveQbservable<T>>().Merge(right.To<IAsyncReactiveQbservable<T>, IReactiveQbservable<T>>()).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>[], T>(new Uri(Client.Constants.Observable.Merge.N), sources => ReactiveQbservable.Merge(sources.To<IAsyncReactiveQbservable<T>[], IReactiveQbservable<T>[]>()).AsAsync(), null, CancellationToken.None);

            // Non-parameterized defines
            var emptyFactory = (Expression<Func<IAsyncReactiveQbservable<T>>>)(() => ReactiveQbservable.Empty<T>().AsAsync());
            var emptyObservable = context.Provider.CreateQbservable<T>(emptyFactory.Body);
            await context.DefineObservableAsync<T>(new Uri(Client.Constants.Observable.Empty.NoArgument), emptyObservable, null, CancellationToken.None);

            var neverFactory = (Expression<Func<IAsyncReactiveQbservable<T>>>)(() => ReactiveQbservable.Never<T>().AsAsync());
            var neverObservable = context.Provider.CreateQbservable<T>(neverFactory.Body);
            await context.DefineObservableAsync<T>(new Uri(Client.Constants.Observable.Never.NoArgument), neverObservable, null, CancellationToken.None);

            // Higher order defines
            await context.DefineObservableAsync<IAsyncReactiveQbservable<IAsyncReactiveObservable<T>>, T>(new Uri(Client.Constants.Observable.Merge.NoArgument), source => source.To<IAsyncReactiveQbservable<IAsyncReactiveObservable<T>>, IReactiveQbservable<IReactiveObservable<T>>>().Merge().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<IAsyncReactiveObservable<T>>, T>(new Uri(Client.Constants.Observable.Switch.NoArgument), source => source.To<IAsyncReactiveQbservable<IAsyncReactiveObservable<T>>, IReactiveQbservable<IReactiveObservable<T>>>().Switch().AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, T>(new Uri(Client.Constants.Observable.Throttle.TimeSpan), (source, dueTime) => source.AsSync().Throttle(_ => ReactiveQbservable.Timer(dueTime)).AsAsync(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, IAsyncReactiveQbservable<R>>>, R>(
                new Uri(Client.Constants.Observable.SelectMany.Func),
                (source, selector) => source.AsSync().SelectMany(selector.To<Expression<Func<T, IAsyncReactiveQbservable<R>>>, Expression<Func<T, IReactiveQbservable<R>>>>()).AsAsync(),
                null,
                CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T1>, Expression<Func<T1, IAsyncReactiveQbservable<T2>>>, Expression<Func<T1, T2, R>>, R>(
                new Uri(Client.Constants.Observable.SelectMany.FuncFunc),
                (source, collectionSelector, resultSelector) => source.AsSync().SelectMany(collectionSelector.To<Expression<Func<T1, IAsyncReactiveQbservable<T2>>>, Expression<Func<T1, IReactiveQbservable<T2>>>>(), resultSelector).AsAsync(),
                null,
                CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, IAsyncReactiveQbservable<T>>(new Uri(Client.Constants.Observable.Window.TimeDuration), (source, duration) => source.AsSync().Window(duration).To<IReactiveQbservable<IReactiveQbservable<T>>, IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>>>(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, TimeSpan, IAsyncReactiveQbservable<T>>(new Uri(Client.Constants.Observable.Window.TimeDurationShift), (source, duration, shift) => source.AsSync().Window(duration, shift).To<IReactiveQbservable<IReactiveQbservable<T>>, IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>>>(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, IAsyncReactiveQbservable<T>>(new Uri(Client.Constants.Observable.Window.Count), (source, count) => source.AsSync().Window(count).To<IReactiveQbservable<IReactiveQbservable<T>>, IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>>>(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, int, IAsyncReactiveQbservable<T>>(new Uri(Client.Constants.Observable.Window.CountSkip), (source, count, skip) => source.AsSync().Window(count, skip).To<IReactiveQbservable<IReactiveQbservable<T>>, IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>>>(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, int, IAsyncReactiveQbservable<T>>(new Uri(Client.Constants.Observable.Window.TimeCount), (source, duration, count) => source.AsSync().Window(duration, count).To<IReactiveQbservable<IReactiveQbservable<T>>, IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>>>(), null, CancellationToken.None);

            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, IList<T>>(new Uri(Client.Constants.Observable.Buffer.TimeDuration), (source, duration) => source.AsSync().Buffer(duration).To<IReactiveQbservable<IList<T>>, IAsyncReactiveQbservable<IList<T>>>(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, TimeSpan, IList<T>>(new Uri(Client.Constants.Observable.Buffer.TimeDurationShift), (source, duration, shift) => source.AsSync().Buffer(duration, shift).To<IReactiveQbservable<IList<T>>, IAsyncReactiveQbservable<IList<T>>>(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, IList<T>>(new Uri(Client.Constants.Observable.Buffer.Count), (source, count) => source.AsSync().Buffer(count).To<IReactiveQbservable<IList<T>>, IAsyncReactiveQbservable<IList<T>>>(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, int, IList<T>>(new Uri(Client.Constants.Observable.Buffer.CountSkip), (source, count, skip) => source.AsSync().Buffer(count, skip).To<IReactiveQbservable<IList<T>>, IAsyncReactiveQbservable<IList<T>>>(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, int, IList<T>>(new Uri(Client.Constants.Observable.Buffer.TimeCount), (source, duration, count) => source.AsSync().Buffer(duration, count).To<IReactiveQbservable<IList<T>>, IAsyncReactiveQbservable<IList<T>>>(), null, CancellationToken.None);

            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, T1>>, IAsyncReactiveGroupedQbservable<T1, T>>(new Uri(Client.Constants.Observable.GroupBy.Key), (source, keySelector) => source.AsSync().GroupBy(keySelector).To<IReactiveQbservable<IReactiveGroupedQbservable<T1, T>>, IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<T1, T>>>(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, T1>>, Expression<Func<T, T2>>, IAsyncReactiveGroupedQbservable<T1, T2>>(new Uri(Client.Constants.Observable.GroupBy.KeyElement), (source, keySelector, elementSelector) => source.AsSync().GroupBy(keySelector, elementSelector).To<IReactiveQbservable<IReactiveGroupedQbservable<T1, T2>>, IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<T1, T2>>>(), null, CancellationToken.None);

            // Extension defines
            await DefineOperatorsExtension(context);
        }

        protected virtual async Task UndefineObservables(ReactiveClientContext context)
        {
            await TryUndefine(() => context.UndefineObservableAsync(Platform.Constants.Identifiers.Observable.FireHose.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(Platform.Constants.Identifiers.Observable.GarbageCollector.Uri, CancellationToken.None));
        }

        protected virtual async Task DefineObservables(ReactiveClientContext context)
        {
            await context.DefineObservableAsync<Uri, T>(Platform.Constants.Identifiers.Observable.FireHose.Uri, uri => new FirehoseSubscribable<T>(uri).AsQbservable(), null, CancellationToken.None);
            await context.DefineObservableAsync<IAsyncReactiveQbservable<int>, int>(Platform.Constants.Identifiers.Observable.GarbageCollector.Uri, source => new GarbageCollectorObservable(source.To<IAsyncReactiveQbservable<int>, ISubscribable<int>>()).AsQbservable(), null, CancellationToken.None);
        }

        protected virtual async Task UndefineObservers(ReactiveClientContext context)
        {
            await TryUndefine(() => context.UndefineObserverAsync(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(Platform.Constants.Identifiers.Observer.ConsoleObserverParam.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(Platform.Constants.Identifiers.Observer.TraceObserver.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(Platform.Constants.Identifiers.Observer.TraceObserverParam.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(Platform.Constants.Identifiers.Observer.FireHose.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(Platform.Constants.Identifiers.Observer.Throughput.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(new Uri(Client.Constants.Observer.Nop), CancellationToken.None));
        }

        protected virtual async Task DefineObservers(ReactiveClientContext context)
        {
            var consoleObserverFactory = (Expression<Func<IAsyncReactiveQbserver<T>>>)(() => new ConsoleObserver<T>().AsQbserver());
            var consoleObserver = context.Provider.CreateQbserver<T>(consoleObserverFactory.Body);
            await context.DefineObserverAsync<T>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri, consoleObserver, null, CancellationToken.None);

            await context.DefineObserverAsync<string, T>(Platform.Constants.Identifiers.Observer.ConsoleObserverParam.Uri, prefix => new ConsoleObserver<T>(prefix).AsQbserver(), null, CancellationToken.None);

            var traceObserverFactory = (Expression<Func<IAsyncReactiveQbserver<T>>>)(() => new TraceObserver<T>().AsQbserver());
            var traceObserver = context.Provider.CreateQbserver<T>(traceObserverFactory.Body);
            await context.DefineObserverAsync<T>(Platform.Constants.Identifiers.Observer.TraceObserver.Uri, traceObserver, null, CancellationToken.None);

            await context.DefineObserverAsync<string, T>(Platform.Constants.Identifiers.Observer.TraceObserverParam.Uri, prefix => new TraceObserver<T>(prefix).AsQbserver(), null, CancellationToken.None);

            await context.DefineObserverAsync<Uri, T>(Platform.Constants.Identifiers.Observer.FireHose.Uri, uri => new FirehoseObserver<T>(uri).AsQbserver(), null, CancellationToken.None);

            await context.DefineObserverAsync<string, T>(Platform.Constants.Identifiers.Observer.Throughput.Uri, s => new ThroughputObserver<T>(s).AsQbserver(), null, CancellationToken.None);

            var nopObserverFactory = (Expression<Func<IAsyncReactiveQbserver<T>>>)(() => NopObserver<T>.Instance.AsQbserver());
            var nopObserver = context.Provider.CreateQbserver<T>(nopObserverFactory.Body);
            await context.DefineObserverAsync<T>(new Uri(Client.Constants.Observer.Nop), nopObserver, null, CancellationToken.None);
        }

        protected static async Task TryUndefine(Func<Task> undefine)
        {
            try
            {
                await undefine();
            }
            catch (Exception)
            {

            }
        }
    }
}
