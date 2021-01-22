// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor;
using Reaqtor.Expressions.Core;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;
using Reaqtor.QueryEngine.Mocks;
using Reaqtor.Reliable;

namespace Tests.Reaqtor.QueryEngine
{
    public abstract class QueryEngineTest
    {
        protected static readonly Uri EmptyObservableUri = new("rx://observable/empty");
        protected static readonly Uri NeverObservableUri = new("rx://observable/never");
        protected static readonly Uri ReturnObservableUri = new("rx://observable/return");

        private MockReactiveServiceResolver _resolver;

        protected IReactive Context { get; private set; }

        protected IReactiveServiceResolver Resolver => _resolver;

        protected void TestInitialize()
        {
            _resolver = new MockReactiveServiceResolver();
            Context = CreateContext();
            AddCommonDefinitions(Context);
        }

        protected void TestCleanup()
        {
            _resolver.Dispose();
            _resolver = null;
            Context = null;
        }

        protected virtual IReactive CreateContext()
        {
            var provider = new MockLazyReactiveEngineProvider();
            return new MockReactiveServiceContext(provider);
        }

        protected virtual void AddCommonDefinitions(IReactive ctx)
        {
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, Expression<Func<T, bool>>>, T>(
                new Uri("rx://observable/filter"),
                t => ReactiveQbservable.Where(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, int>, T>(
                new Uri("rx://observable/take"),
                t => ReactiveQbservable.Take(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, Expression<Func<T, R>>>, T>(
                new Uri("rx://observable/distinctuntilchanged"),
                t => ReactiveQbservable.DistinctUntilChanged(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T1>, IReactiveQbservable<T2>>, T1>(
                new Uri("rx://observable/skip/until"),
                t => ReactiveQbservable.SkipUntil(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T1>, DateTimeOffset>, T1>(
                new Uri("rx://observable/skip/until/datetimeoffset"),
                t => ReactiveQbservable.SkipUntil(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T1>, IReactiveQbservable<T2>>, T1>(
                new Uri("rx://observable/take/until"),
                t => ReactiveQbservable.TakeUntil(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, Expression<Func<T, bool>>>, T>(
                new Uri("rx://observable/take/while"),
                t => ReactiveQbservable.TakeWhile(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>>, T>(
                new Uri("rx://observable/firstasync"),
                t => ReactiveQbservable.FirstAsync(t.Item1),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T1>, Expression<Func<T1, IReactiveObservable<T2>>>>, T1>(
                new Uri("rx://observable/throttle"),
                t => ReactiveQbservable.Throttle(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<IReactiveQbservable<T>>>, T>(
                new Uri("rx://observable/switch"),
                t => ReactiveQbservable.Switch(t.Item1),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, T[]>, T>(
                new Uri("rx://observable/startwith"),
                t => ReactiveQbservable.StartWith<T>(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<T>(
                EmptyObservableUri,
                ctx.Provider.CreateQbservable<T>(((Expression<Func<IReactiveQbservable<T>>>)(() => Subscribable.Empty<T>().AsQbservable())).Body),
                null);
            ctx.DefineObservable<T>(
                NeverObservableUri,
                ctx.Provider.CreateQbservable<T>(((Expression<Func<IReactiveQbservable<T>>>)(() => Subscribable.Never<T>().AsQbservable())).Body),
                null);
            ctx.DefineObservable<Tuple<T>, T>(
                ReturnObservableUri,
                t => Subscribable.Return(t.Item1).AsQbservable(),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T1>, Expression<Func<T1, T2>>>, T2>(
                new Uri("rx://observable/select"),
                t => ReactiveQbservable.Select(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T1>, IReactiveQbservable<T2>, Expression<Func<T1, T2, R>>>, R>(
                new Uri("rx://observable/combineLatest"),
                t => ReactiveQbservable.CombineLatest(t.Item1, t.Item2, t.Item3),
                null);

            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, DateTimeOffset>, T>(
                new Uri("rx://observable/delaySubscription/absoluteTime"),
                t => ReactiveQbservable.Timer(t.Item2).SelectMany(_ => t.Item1),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, TimeSpan>, T>(
                new Uri("rx://observable/delaySubscription/relativeTime"),
                t => ReactiveQbservable.Timer(t.Item2).SelectMany(_ => t.Item1),
                null);

            ctx.DefineObservable<Tuple<DateTimeOffset>, long>(
                new Uri("rx://observable/absolutetimer"),
                t => ReactiveQbservable.Timer(t.Item1),
                null);
            ctx.DefineObservable<Tuple<DateTimeOffset, TimeSpan>, long>(
                new Uri("rx://observable/absolutetimer/period"),
                t => ReactiveQbservable.Timer(t.Item1, t.Item2),
                null);
            ctx.DefineObservable<Tuple<TimeSpan>, long>(
                new Uri("rx://observable/relativetimer"),
                t => ReactiveQbservable.Timer(t.Item1),
                null);
            ctx.DefineObservable<Tuple<TimeSpan, TimeSpan>, long>(
                new Uri("rx://observable/relativetimer/period"),
                t => ReactiveQbservable.Timer(t.Item1, t.Item2),
                null);

            ctx.DefineObservable<Tuple<IReactiveQbservable<T1>, Expression<Func<T1, IReactiveQbservable<T2>>>, Expression<Func<T1, T2, R>>>, R>(
                new Uri("rx://observable/selectmany"),
                t => ReactiveQbservable.SelectMany(t.Item1, t.Item2, t.Item3),
                null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, IReactiveQbserver<T>>, T>(
                new Uri("rx://observable/do"),
                t => ReactiveQbservable.Do(t.Item1, t.Item2),
                null);

            ctx.DefineStreamFactory<T, T>(
                new Uri("rx://subject/create"),
                ctx.Provider.MakeQubjectFactory(() => ReactiveQubjectFactory.GetSubjectFactory<T>()),
                null);

            ctx.DefineObserver<T>(
                new Uri("rx://observer/nop"),
                ctx.Provider.CreateQbserver<T>(((Expression<Func<IReactiveQbserver<T>>>)(() => NopObserver<T>.Instance.AsQbserver())).Body),
                null);

            ctx.DefineObservable<Tuple<IReactiveQbservable<int>>, int>(
                new Uri("rx://operators/sum"),
                t => ReactiveQbservable.Sum(t.Item1),
                null);

            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, int>, IReactiveQbservable<T>>(
                new Uri("rx://operators/window/count"),
                t => ReactiveQbservable.Window(t.Item1, t.Item2),
                null);
        }

        /// <summary>
        /// Recovers query engine from checkpointed memory store.
        /// </summary>
        /// <param name="qe">Query engine to put recovered state in.</param>
        /// <param name="chkpt">Checkpoint from which to recover.</param>
        protected static void Recover(ICheckpointingQueryEngine qe, InMemoryStateStore chkpt)
        {
            using var stateReader = new InMemoryStateReader(chkpt);

            qe.RecoverAsync(stateReader).Wait();
        }

        /// <summary>
        /// Dispose all subscriptions passed in; remove all query engines form service;
        /// clear all mock observables and mock observers.
        /// </summary>
        /// <param name="subscriptions">List of subscriptions to dispose.</param>
        protected void Crash(params ISubscription[] subscriptions)
        {
            foreach (var sub in subscriptions)
            {
                sub.Dispose();
            }

            RemoveAllQueryEngines();
            MockObservable.Clear();
            MockObserver.Clear();
        }

        protected static InMemoryStateStore Checkpoint(ICheckpointingQueryEngine qe, CheckpointKind kind = CheckpointKind.Full, Action onCommit = null)
        {
            var chkpt = new InMemoryStateStore(Guid.NewGuid().ToString("D"));
            Checkpoint(qe, chkpt, kind, onCommit);
            return chkpt;
        }

        protected static void Checkpoint(ICheckpointingQueryEngine qe, InMemoryStateStore chkpt, CheckpointKind kind = CheckpointKind.Full, Action onCommit = null)
        {
            using var stateWriter = new InMemoryStateWriter(chkpt, kind, onCommit);

            qe.CheckpointAsync(stateWriter).Wait();
        }

        protected static TraceSource CreateTraceSource(Action<string> onWrite, SourceLevels sourceLevels = SourceLevels.All)
        {
            var traceSource = new TraceSource("TestTraceSource", sourceLevels);
            traceSource.Listeners.Add(new TestTraceListener(onWrite));
            return traceSource;
        }

        protected CheckpointingQueryEngine CreateQueryEngine(TraceSource traceSource = null)
        {
            return CreateQueryEngine("qe:/" + Guid.NewGuid(), GetScheduler(), traceSource: traceSource);
        }

        protected CheckpointingQueryEngine CreateQueryEngine(string id, TraceSource traceSource = null)
        {
            return CreateQueryEngine(id, GetScheduler(), traceSource: traceSource);
        }

        protected CheckpointingQueryEngine CreateQueryEngine(IKeyValueStore keyValueStore)
        {
            return CreateQueryEngine("qe:/" + Guid.NewGuid(), GetScheduler(), keyValueStore: keyValueStore);
        }

        protected CheckpointingQueryEngine CreateQueryEngine(string id, IScheduler scheduler, ISerializationPolicy serializationPolicy = null, TraceSource traceSource = null, IKeyValueStore keyValueStore = null)
        {
            traceSource ??= new TraceSource("TestTraceSource");

            var map = QuotedTypeConversionTargets.From(new Dictionary<Type, Type>
            {
                { typeof(ReactiveQbservable),            typeof(Subscribable)               },
                { typeof(ReactiveQbserver),              typeof(Observer)                   },
                { typeof(ReactiveQubjectFactory),        typeof(ReliableSubjectFactory)     },
            });

            CheckpointingQueryEngine qe = new CheckpointingQueryEngine(new Uri(id), Resolver,
                scheduler.CreateChildScheduler(), Context, keyValueStore ?? new InMemoryKeyValueStore(), serializationPolicy ?? SerializationPolicy.Default, map, traceSource);

            qe.Options.ForeignFunctionBinder = Bind;

            _resolver.AddQueryEngine(qe);

            return qe;
        }

        private static Expression Bind(string s)
        {
            if (s == "function://int/square")
            {
                return (Expression<Func<int, int>>)(x => x * x);
            }

            return null;
        }

        protected void RemoveAllQueryEngines()
        {
            for (var i = 0; i < _resolver.QueryEngines.Count; i++)
            {
                RemoveQueryEngine(_resolver.QueryEngines[i]);
            }
        }

        protected void RemoveQueryEngine(ICheckpointingQueryEngine qe)
        {
            ((CheckpointingQueryEngine)qe).Scheduler.Dispose();
            _resolver.RemoveQueryEngine(qe);
        }

        protected static ReactiveServiceContext GetQueryEngineReactiveService(ICheckpointingQueryEngine qe)
        {
            return new TupletizingContext(qe.ReactiveService);
        }

        protected static ReactiveClientContext GetQueryEngineAsyncReactiveService(CheckpointingQueryEngine qe)
        {
            return new TupletizingClientContext(qe.ServiceProvider);
        }

        protected abstract IScheduler GetScheduler();

        private sealed class TestTraceListener : TraceListener
        {
            private readonly Action<string> _onWrite;

            public TestTraceListener(Action<string> onWrite)
            {
                _onWrite = onWrite;
            }

            public override void Write(string message)
            {
                _onWrite(message);
            }

            public override void WriteLine(string message)
            {
                _onWrite(message);
            }
        }
    }
}
