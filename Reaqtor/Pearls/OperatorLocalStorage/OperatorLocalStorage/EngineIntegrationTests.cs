// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

#define USE_OLS // flag to control using operator local storage operator variants

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;
using Reaqtive.Operators;
using Reaqtive.Scheduler;

using Reaqtor;
using Reaqtor.Expressions.Core;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.KeyValueStore.InMemory;

using Engine;

using Utilities;

namespace Playground
{
    internal static partial class EngineIntegrationTests
    {
        private static readonly IQuotedTypeConversionTargets s_map = QuotedTypeConversionTargets.From(new Dictionary<Type, Type>
        {
            { typeof(ReactiveQbservable),            typeof(Subscribable)               }, // NB: We use ReactivQbservable.Select for definitions.
            { typeof(ReactiveQbserver),              typeof(Observer)                   }, // NB: Just here for completeness.
        });

        public static async Task RunAsync()
        {
            //
            // Define a stream manager that will be made available to Source<T> and Sink<T> through the operator context.
            // The state of the stream manager itself is not persisted as part of a checkpoint, so we keep the instance alive across engine lifetimes.
            //

            var sm = new StreamManager();

            //
            // Create two streams: xs will be used by Source<T> to send events into the computation, and ys will be used by Sink<T> to receive results.
            //

            var xs = sm.CreateSubject<int>("xs");
            var ys = sm.CreateSubject<string>("ys");

            //
            // For diagnostic purposes, we'll always listen to the sink. Regardless of engine failover, we should see events here.
            //

            ys.Subscribe(Console.WriteLine);

            //
            // We'll reuse a few more things across engine failovers:
            //
            // - a physical scheduler to obtain logical schedulers from
            // - an in-memory key/value store for the create/delete transactions (note we could save/load this one, but it doesn't intersect with operator local storage functionality)
            //

            var physicalScheduler = PhysicalScheduler.Create();
            var kvs = new InMemoryKeyValueStore();

            //
            // We'll have two generations of the same engine, similating a checkpoint/recover transition. The only piece of shared state will be the in-memory checkpoint store.
            //

            var checkpointStore = new Store();

            //
            // First generation of the engine.
            //

            {
                //
                // Instantiate an engine with:
                //
                // - a no-op resolver; we're never going to rely on binding to artifacts defined outside this engine
                // - a no-op registry; we'll define all artifacts inside the engine rather than relying on a central catalog
                // - a shared in-memory key/value store for transactions (see above)
                // - a logical scheduler, no trace source, and the simplest of compiled delegate caches available
                // - an operator context element added for Source<T> and Sink<T> to get access to the stream manager
                //

                var engine = new Engine.CheckpointingQueryEngine(new Uri("qe://demo/1"), new Resolver(), new LogicalScheduler(physicalScheduler), new Registry(), kvs, s_map, traceSource: null, new SimpleCompiledDelegateCache())
                {
                    OperatorContextElements =
                    {
                        { "StreamManager", sm }
                    }
                };

                //
                // Get an IReactive wrapper around the engine for programming ergonomics below.
                //

                var ctx = new QueryEngineContext(engine.ReactiveService);

                //
                // For the first instance of the engine, we'll define a bunch of artifacts which will be checkpointed, including our custom buffer operator using operator local storage.
                //

                {
                    ctx.DefineObservable(
                        new Uri("observable://source"),
                        CastVisitor.Apply((Expression<Func<string, IReactiveQbservable<T>>>)(stream => new Source<T>(stream).ToQbservable())),
                        null);

                    ctx.DefineObserver(
                        new Uri("observer://sink"),
                        CastVisitor.Apply((Expression<Func<string, IReactiveQbserver<T>>>)(stream => new Sink<T>(stream).ToQbserver())),
                        null);

                    ctx.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, R>>, R>(
                        new Uri("rx://observable/map"),
                        (source, selector) => ReactiveQbservable.Select(source, selector),
                        null);

                    ctx.DefineObservable<IReactiveQbservable<T>, int, int, IList<T>>(
                        new Uri("rx://observable/buffer"),
#if USE_OLS
                        CastVisitor.Apply((Expression<Func<IReactiveQbservable<T>, int, int, IReactiveQbservable<IList<T>>>>)((source, count, skip) => new BufferCountSkip<T>(source.ToSubscribable(), count, skip).ToQbservable())),
#else
                        (source, count, skip) => ReactiveQbservable.Buffer(source, count, skip),
#endif
                        null);
                }

                //
                // For the first instance of the engine, we'll also define our standing query which will be checkpointed.
                //

                {
                    var source = ctx.GetObservable<string, int>(new Uri("observable://source"))("xs");

                    var select = ctx.GetObservable<IReactiveQbservable<IList<int>>, Expression<Func<IList<int>, string>>, string>(new Uri("rx://observable/map"));
                    var buffer = ctx.GetObservable<IReactiveQbservable<int>, int, int, IList<int>>(new Uri("rx://observable/buffer"));

                    var sink = ctx.GetObserver<string, string>(new Uri("observer://sink"))("ys");

                    select(buffer(source, 5, 3), list => string.Join(", ", list)).Subscribe(sink, new Uri("subscription://demo"), null);
                }

                //
                // We'll send events in range [0..9] which should emit two buffers ([0..4] and [3..7]) and have two buffers pending ([6..10] and [9..13]).
                //
                //                                      Failure
                //                                         |
                //   0,  1,  2,  3,  4,  5,  6,  7,  8,  9,|10, 11, 12, 13, 14, 15, 16, 17, 18, 19
                // [------------------]                    |
                //             [------------------]        |
                //                         [---------------|--]
                //                                     [---|--------------]
                //                                         |       [------------------]
                //                                         |                   [------------------]
                //                                         |
                //
                // In order to wait for the delivery of the first two buffers, we'll subscribe to ys outside the engine ourselves to obtain a task to await on.
                // This is needed because event processing in the engine happens asynchronously on scheduler threads.
                //

                var afterTwoEvents = ys.Take(2).ToTask();

                for (var i = 0; i <= 9; i++)
                {
                    xs.OnNext(i);
                }

                await afterTwoEvents;

                //
                // Now it's time to checkpoint the engine.
                //
                // NB: We have to use the "long" overload here because it's the only one we supply in our extension to CheckpointingQueryEngine.
                //

                await engine.CheckpointAsync(new Writer(checkpointStore, CheckpointKind.Differential), CancellationToken.None, progress: null);

                //
                // To make our testing easier, we'll call UnloadAsync, which enables our Source<T> to detach from the stream manager.
                //

                await engine.UnloadAsync();
            }

            //
            // Print the store.
            //

            checkpointStore.Print();

            //
            // Second generation of the engine.
            //

            {
                //
                // Re-instantiate the engine with parameters similar to the ones used the first time around.
                //

                var engine = new Engine.CheckpointingQueryEngine(new Uri("qe://demo/1"), new Resolver(), new LogicalScheduler(physicalScheduler), new Registry(), kvs, s_map, traceSource: null, new SimpleCompiledDelegateCache())
                {
                    OperatorContextElements =
                    {
                        { "StreamManager", sm }
                    }
                };

                //
                // Recover the engine from state.
                //
                // NB: We have to use the "long" overload here because it's the only one we supply in our extension to CheckpointingQueryEngine.
                //

                await engine.RecoverAsync(new Reader(checkpointStore), CancellationToken.None, progress: null);

                //
                // We'll continue to send events in range [10..19] which should emit the two buffers that were pending ([6..10] and [9..13]) and two new buffers ([12..16] and [15..19]).
                //
                //                                      Failure
                //                                         |
                //   0,  1,  2,  3,  4,  5,  6,  7,  8,  9,|10, 11, 12, 13, 14, 15, 16, 17, 18, 19
                // [------------------]                    |
                //             [------------------]        |
                //                         [---------------|--]
                //                                     [---|--------------]
                //                                         |       [------------------]
                //                                         |                   [------------------]
                //                                         |
                //
                // In order to wait for the delivery of the next four buffers, we'll subscribe to ys outside the engine ourselves to obtain a task to await on.
                // This is needed because event processing in the engine happens asynchronously on scheduler threads.
                //

                var afterFourEvents = ys.Take(4).ToTask();

                for (var i = 10; i <= 19; i++)
                {
                    xs.OnNext(i);
                }

                await afterFourEvents;
            }
        }
    }
}
