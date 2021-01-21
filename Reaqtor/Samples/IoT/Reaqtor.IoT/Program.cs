// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#define DEMO1
//#define DEMO2
//#define DEMO3
//#define DEMO4

using System;
using System.Linq;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Nuqleon.DataModel;
using Reaqtive;
using Reaqtive.Scheduler;

namespace Reaqtor.IoT
{
    public class Program
    {
        public static async Task Main()
        {
            //
            // Query engines host reactive artifacts, e.g. subscriptions, which can be stateful.
            //
            // Query engines are a failover unit. State for all artifacts is persisted via checkpointing.
            //
            // Query engines depend on services from the environment:
            //
            // - A scheduler to process events on:
            //   - There's one physical scheduler per host. Think of it as a thread pool.
            //   - Each engine has a logical scheduler. Think of it as a collection of tasks. The engine suspends/resumes all work for checkpoint/recovery.
            // - A key/value store for state persistence, including:
            //   - A transaction log of create/delete operations for reactive artifacts.
            //   - Periodic checkpoint state, which includes:
            //     - State of reactive artifacts (e.g. sum and count for an Average operator).
            //     - Watermarks for ingress streams, enabling replay of events upon failover.
            //
            // This sample also parameterizes query engines on an ingress/egress manager to receive/send events across the engine/environment boundary.
            //
            using var ps = PhysicalScheduler.Create();

            var scheduler = new LogicalScheduler(ps);
            var store = new InMemoryKeyValueStore();
            var iemgr = new IngressEgressManager();

            //
            // Illustrates the lifecycle of an engine:
            //
            // - Instantiate the object, passing the environment services.
            // - Recover the engine's state from the key/value store.
            // - Use the engine (omitted below).
            // - Checkpoint the engine's state. This is typically done periodically, e.g. once per minute. The interval is a tradeoff between:
            //   - I/O frequency versus I/O size, e.g. due to state growth as events get processed.
            //   - Replay capacity for ingress events and duration of replay, e.g. having to replay up to 1 minute worth of events from a source.
            // - Unloading the engine. This is optional but useful for graceful shutdown. In the Reactor service this is used when a primary moves to another node in the cluster. It allows reactive artifacts to unload resources (e.g. connections).
            //
            Console.WriteLine("Creating brand new engine...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());
                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

            //
            // Illustrates populating the registry of defined artifacts in the engine. This is a one-time step for the environment creating a new engine.
            //
            // - Artifact types that are defined include:
            //   - Observables, e.g. sources of events, or query operators.
            //   - Observers, e.g. sinks for events, or event handlers.
            //   - Stream factories, not shown here. Useful for creation of "subjects" local to the engine.
            //   - Subscription factories, not shown here. Useful for "templates" to create subscriptions with parameters.
            // - All Reactor artifacts use URIs for naming purposes.
            //
            // The key take-away is that Reactor engines are empty by default and have no built-in artifacts whatsoever. The environment controls the registry, which includes standard query operators, specialized query operators, etc.
            //
            // NB: There's an alternative approach to having artifacts defined in and persisted by individual engine instances. The engine can also be parameterized on a queryable external catalog. This is useful for homogeneous environments.
            //
            Console.WriteLine("Defining artifacts in engine...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);
                await ctx.DefineObserverAsync(new Uri("iot://reactor/observers/cout"), ctx.Provider.CreateQbserver<T>(Expression.New(typeof(ConsoleObserver<T>))), null, CancellationToken.None);
                await ctx.DefineObservableAsync<TimeSpan, DateTimeOffset>(new Uri("iot://reactor/observables/timer"), t => new TimerObservable(t).AsAsyncQbservable(), null, CancellationToken.None);

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

#if DEMO1
            //
            // Illustrates the user programming surface of Reactor. The environment should provide an IReactiveProxy "context" object to the user. It provides an API similar to LINQ to SQL and Rx:
            //
            // - Get* to obtain artifacts, using their well-known URIs.
            // - Compose queries over those artifacts.
            // - Submit them to the engine using async operations (e.g. SubscribeAsync in lieu of Subscribe in Rx).
            //
            // The sample below shows the most basic subscription, merely connecting a source (observable) and a sink (observer). The resulting subscription has a name, which can be used to delete it later.
            //
            // A few notes on IReactiveProxy:
            //
            // - APIs are asynchronous to cover I/O:
            //   - In a distributed service, this includes submitting a serialized expression tree across machine boundaries.
            //   - At the engine level (shown here), this includes the transaction log operation for the create operation (enabling replay of the creation operation in the event of engine failure before the next checkpoint).
            // - The ReactorContext type shown below implements this interface:
            //   - Think of it being analogous to DataContext in LINQ to SQL, which has methods like GetTable<T>.
            //   - Derived types can provide friendly accessors for artifacts, just like LINQ to SQL could have a NorthwindDataContext providing a Products property to hide GetTable<Product>("Product").
            //
            Console.WriteLine("Creating a subscription...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

                // *** USER CODE STARTS HERE ***

                var timer = ctx.GetObservable<TimeSpan, DateTimeOffset>(new Uri("iot://reactor/observables/timer"));
                var cout = ctx.GetObserver<DateTimeOffset>(new Uri("iot://reactor/observers/cout"));

                await timer(TimeSpan.FromSeconds(1)).SubscribeAsync(cout, new Uri("iot://reactor/subscriptions/heartbeat"), null, CancellationToken.None);

                // *** USER CODE ENDS HERE ***

                await Task.Delay(TimeSpan.FromSeconds(5));

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

            Console.WriteLine("Engine was unloaded...");
            await Task.Delay(TimeSpan.FromSeconds(5));

            //
            // Illustrates checkpoint/recovery. The subscription is running again after failover.
            //
            Console.WriteLine("Recovering a subscription...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                await Task.Delay(TimeSpan.FromSeconds(5));

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

            //
            // Illustrates user code to dispose an existing subscription.
            //
            // See remarks on IReactiveProxy higher up. The pattern is identical:
            //
            // - Use a Get operation to get a proxy to the artifact, in this case a subscription, using the URI.
            // - Invoke an asynchronous operation to act on it, in this case DisposeAsync to dispose the subscription.
            //
            // The asynchronous nature of the operation is again due to:
            //
            // - The ability to send the operation across machine boundaries.
            // - The transaction log in the engine to persist the deletion operation in the event of failure before the next checkpoint.
            //
            Console.WriteLine("Disposing a subscription...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

                // *** USER CODE STARTS HERE ***

                var heartbeat = ctx.GetSubscription(new Uri("iot://reactor/subscriptions/heartbeat"));
                await heartbeat.DisposeAsync(CancellationToken.None);

                // *** USER CODE ENDS HERE ***

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }
#endif

            //
            // Illustration of defining query operators, similar to defining other artifacts higher up. A few remarks:
            //
            // - No operators are built-in. Below, we define essential operators like Where, Select, and Take. The URI for these is not even prescribed; the environment picks those.
            // - Implementations of the operators are provided in Reaqtive, similar to System.Reactive for classic Rx. The difference is mainly due to support for state persistence, which classic Rx lacks.
            // - Custom operators are as first-class as "standard query operators". That is, the query engine does not have an opinion about the operator surface provided.
            //
            // Some ugly technicalities show up below, but those are entirely irrelevant to the user experience. The code below is part of the one-time setup provided by the environment. In particular:
            //
            // - Define operations are done through IReactiveProxy, but could also be done straight on the engine (though it brings some additional complexity when doing so, see revision history of this file for details).
            // - There's some conversion friction to build expressions that fit through a "queryable" expression-tree based API but eventually bind to types in Reaqtive. That's all the As* stuff below.
            //
            Console.WriteLine("Defining some query operators in engine...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

#if DEFINE_ALL_OPERATORS
                await Operators.DefineAsync(ctx, CancellationToken.None);
#else
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, Func<T, bool>, T>(new Uri("iot://reactor/observables/filter"), (source, predicate) => source.AsSubscribable().Where(predicate).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, Func<T, int, bool>, T>(new Uri("iot://reactor/observables/filter/indexed"), (source, predicate) => source.AsSubscribable().Where(predicate).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, Func<T, R>, R>(new Uri("iot://reactor/observables/map"), (source, selector) => source.AsSubscribable().Select(selector).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, Func<T, int, R>, R>(new Uri("iot://reactor/observables/map/indexed"), (source, selector) => source.AsSubscribable().Select(selector).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, T>(new Uri("iot://reactor/observables/take"), (source, count) => source.AsSubscribable().Take(count).AsAsyncQbservable(), null, CancellationToken.None);
#endif

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

#if DEMO2
            //
            // Illustration of a more sophisticated query expression written by the user, using the operators defined above.
            //
            // Note that all Rx operators can be defined and used with Reactor, so this merely serves as an example of a select subset of those.
            //
            // The fluent experience using extension methods (and hence supporting query expression "LINQ" syntax as well) is introduced through the Operators type with method definitions like this:
            //
            //     [KnownResource("iot://reactor/observables/filter")]
            //     public static IAsyncReactiveQbservable<T> Where<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
            //
            // This is part of the APIs provided by the environment.
            //
            // Note the use of `KnownResource` to refer to the URI of the defined artifact. Any (extension) method besides the standard query operators can use this mechanism to allow fluent formulation of queries.
            // This again serves to show that nothing is built-in in Reactor: Where, Select, etc. aren't any more special than any other "non-standard" query operator.
            //
            Console.WriteLine("Creating a more fancy subscription...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

                // *** USER CODE STARTS HERE ***

                var timer = ctx.GetObservable<TimeSpan, DateTimeOffset>(new Uri("iot://reactor/observables/timer"));
                var cout = ctx.GetObserver<string>(new Uri("iot://reactor/observers/cout"));

                var res = timer(TimeSpan.FromSeconds(0.5)).Where((x, i) => i % 2 == 0).Select(dt => dt.ToString()).Take(8);

                await res.SubscribeAsync(cout, new Uri("iot://reactor/subscriptions/heartbeat/advanced"), null, CancellationToken.None);

                // *** USER CODE ENDS HERE ***

                await Task.Delay(TimeSpan.FromSeconds(5));

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

            Console.WriteLine("Engine was unloaded...");
            await Task.Delay(TimeSpan.FromSeconds(5));

            //
            // Illustrates recovery of a stateful query. The query above has a Take(8), so part of the persistence includes the remaining event count.
            //
            Console.WriteLine("Recovering a subscription...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                await Task.Delay(TimeSpan.FromSeconds(5));

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

            //
            // Illustration of metadata queries on the engine.
            //
            // IReactiveProxy exposes queryable collections such as Observables, Observers, Subscriptions that can be used to enumerate artifacts in the engine, or to formulate queries.
            // Note that LINQ query provider support is limited in the engine today, but the registry can be indexed efficiently (e.g. ContainsKey, SingleOrDefault), and can be enumerated.
            // Work on metadata queries in the engine has been hampered by the lack of IAsyncQueryable<T> support (which is only coming to .NET now, over 5 years later). We could go back to
            // add rich querying support if such a need arises.
            //
            // For the IoT environment, a ContainsKey query could be useful to check whether a query has already been defined, and even to obain its expression tree, e.g. if we wish to do
            // some idempotent Create operation.
            //
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

                // *** USER CODE STARTS HERE ***

                var found = ctx.Subscriptions.ContainsKey(new Uri("iot://reactor/subscriptions/heartbeat/advanced"));
                Console.WriteLine("Found subscription: " + found);
                Console.WriteLine();

                Console.WriteLine("IoT operators defined in engine:");
                foreach (var observable in ctx.Observables.AsEnumerable(/* NB: See remarks above; no rich query support today :-(. */).Where(kv => kv.Key.Scheme == "iot"))
                {
                    Console.WriteLine("  " + observable.Key);
                }

                // *** USER CODE ENDS HERE ***

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

            //
            // Illustration of disposing a subscription, again.
            //
            Console.WriteLine("Disposing a subscription...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

                // *** USER CODE STARTS HERE ***

                var heartbeat = ctx.GetSubscription(new Uri("iot://reactor/subscriptions/heartbeat/advanced"));
                await heartbeat.DisposeAsync(CancellationToken.None);

                // *** USER CODE ENDS HERE ***

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }
#endif

            //
            // Illustration of defining ingress/egress proxies as observable/observer artifacts.
            //
            // Also see the implementation of IngressObservable<T> and EgressObserver<T>, which use the ingress/egress manager to connect to the outside world. The essence is this:
            //
            // - To the query running inside the engine, these look like ordinary Rx artifacts implemented using interfaces base classes provided by Reactor:
            //   - ISubscribable<T> rather than IObservable<T>, to support the richer lifecycle of artifacts in Reactor compared to Rx.
            //   - Load/Save state operations for checkpointing.
            // - The external world communicates with the engine using a variant of the observable/observer interfaces, namely IReliable*<T>:
            //   - Events received and produced have sequence numbers.
            //   - Subscription handles to receive events from the outside world have additional operations:
            //     - Start(long) to replay events from the given sequence number.
            //     - AcknowledgeRange(long) to allow the external service to (optionally) prune events that are no longer needed by the engine.
            // - Proxies in the engine use the sequence number to provide reliability:
            //   - Save persists the latest received sequence number. Load gets it back.
            //   - Upon restart of an ingress proxy, the restored sequence number is used to ask for replay of events.
            //   - Upon a successful checkpoint, the latest received sequence number is acknowledged to the source (allowing pruning).
            //
            // The Reactor service implements such ingress/egress mechanisms using services like EventHub.
            //
            Console.WriteLine("Defining ingress/egress proxies in engine...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);
                await ctx.DefineObserverAsync<string, T>(new Uri("iot://reactor/observers/egress"), stream => new EgressObserver<T>(stream).AsAsyncQbserver(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<string, T>(new Uri("iot://reactor/observables/ingress"), stream => new IngressObservable<T>(stream).AsAsyncQbservable(), null, CancellationToken.None);

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

#if DEMO3
            //
            // Mimic the outside world by creating streams in the ingress/egress manager. In reality, this would come from the environment, e.g. sensor data.
            //
            // We create two streams:
            //
            // - bar will be used to receive events from the outside world and to perform event processing queries on those events;
            // - foo will be used to send events produced by the queries to the outside world.
            //
            // Thus:
            //
            // - the query in the engine will subscribe to bar and emit events into foo;
            // - the outside world will emit events into bar and subscribe to foo.
            //
            // Also note that all events outside the engine boundaries have sequence numbers.
            //
            var stopBarProducer = new CancellationTokenSource();

            Console.WriteLine("Setting up external streams...");
            {
                var bar = iemgr.CreateSubject<int>("bar");
                var foo = iemgr.CreateSubject<int>("foo");

                foo.Subscribe(x => Console.WriteLine("foo> " + x), ex => Console.WriteLine("foo> " + ex.Message), () => Console.WriteLine("foo> Done"));

                _ = Task.Run(async () =>
                {
                    for (int i = 0; !stopBarProducer.IsCancellationRequested; i++)
                    {
                        var e = (i, 10 * i);
                        Console.WriteLine("bar> " + e);
                        bar.OnNext(e);

                        await Task.Delay(250);
                    }
                });
            }

            //
            // Illustrates a simple pass-through query where events are received from external stream bar and forwarded to external stream foo.
            //
            // Note that the observable and observer proxies are parameterized on the external stream name. This can obviously be hidden in a number of ways:
            //
            // - Define non-parameterized observable and observer artifacts for input/output streams, using a descriptive URI, e.g. iot://sensor/temperature.
            // - Create a context derived from ReactorContext that provides properties that provide direct access to those, e.g. ctx.Temperature.
            //
            Console.WriteLine("Creating subscription using ingress/egress...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

                // *** USER CODE STARTS HERE ***

                var input = ctx.GetObservable<string, int>(new Uri("iot://reactor/observables/ingress"));
                var output = ctx.GetObserver<string, int>(new Uri("iot://reactor/observers/egress"));

                await input("bar").SubscribeAsync(output("foo"), new Uri("iot://reactor/subscriptions/in_out"), null, CancellationToken.None);

                // *** USER CODE ENDS HERE ***

                await Task.Delay(TimeSpan.FromSeconds(5));

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

            Console.WriteLine("Engine was unloaded...");
            await Task.Delay(TimeSpan.FromSeconds(5));

            //
            // Illustrates the replay behavior in the face of failover. During the 5 seconds of downtime, events were produced. Upon recovery, these events are replayed.
            //
            Console.WriteLine("Recovering a subscription...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                await Task.Delay(TimeSpan.FromSeconds(5));

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }

            //
            // Get rid of the external producer, for demo purposes (so it doesn't keep printing during subsequent demos).
            //
            stopBarProducer.Cancel();

            //
            // Illustration of disposing a subscription, again.
            //
            Console.WriteLine("Disposing a subscription...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

                // *** USER CODE STARTS HERE ***

                var in_out = ctx.GetSubscription(new Uri("iot://reactor/subscriptions/in_out"));
                await in_out.DisposeAsync(CancellationToken.None);

                // *** USER CODE ENDS HERE ***

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }
#endif

#if !DEFINE_ALL_OPERATORS
            //
            // Illustrates the definition of higher-order operators such as SelectMany and GroupBy which operate on sequences of sequences (IObservable<IObservable<T>>) which is one of the most powerful aspects of Rx.
            //
            Console.WriteLine("Defining advanced query operators in engine...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

                // Average
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<int>, double>(new Uri("iot://reactor/observables/average/int32"), source => source.AsSubscribable().Average().AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<long>, double>(new Uri("iot://reactor/observables/average/int64"), source => source.AsSubscribable().Average().AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<double>, double>(new Uri("iot://reactor/observables/average/double"), source => source.AsSubscribable().Average().AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, Func<T, int>, double>(new Uri("iot://reactor/observables/average/selector/int32"), (source, selector) => source.AsSubscribable().Average(selector).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, Func<T, long>, double>(new Uri("iot://reactor/observables/average/selector/int64"), (source, selector) => source.AsSubscribable().Average(selector).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, Func<T, double>, double>(new Uri("iot://reactor/observables/average/selector/double"), (source, selector) => source.AsSubscribable().Average(selector).AsAsyncQbservable(), null, CancellationToken.None);

                // DistinctUntilChanged
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, T>(new Uri("iot://reactor/observables/distinct"), source => source.AsSubscribable().DistinctUntilChanged().AsAsyncQbservable(), null, CancellationToken.None);

                // SelectMany
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, Func<T, ISubscribable<R>>, R>(new Uri("iot://reactor/observables/bind"), (source, selector) => source.AsSubscribable().SelectMany(selector).AsAsyncQbservable(), null, CancellationToken.None);

                // Window
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, ISubscribable<T>>(new Uri("iot://reactor/observables/window/hopping/time"), (source, duration) => source.AsSubscribable().Window(duration).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, ISubscribable<T>>(new Uri("iot://reactor/observables/window/hopping/count"), (source, count) => source.AsSubscribable().Window(count).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, TimeSpan, ISubscribable<T>>(new Uri("iot://reactor/observables/window/sliding/time"), (source, duration, shift) => source.AsSubscribable().Window(duration, shift).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, int, int, ISubscribable<T>>(new Uri("iot://reactor/observables/window/sliding/count"), (source, count, skip) => source.AsSubscribable().Window(count, skip).AsAsyncQbservable(), null, CancellationToken.None);
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, TimeSpan, int, ISubscribable<T>>(new Uri("iot://reactor/observables/window/ferry"), (source, duration, count) => source.AsSubscribable().Window(duration, count).AsAsyncQbservable(), null, CancellationToken.None);

                // GroupBy
                await ctx.DefineObservableAsync<IAsyncReactiveQbservable<T>, Func<T, R>, IGroupedSubscribable<R, T>>(new Uri("iot://reactor/observables/group"), (source, selector) => source.AsSubscribable().GroupBy(selector).AsAsyncQbservable(), null, CancellationToken.None);

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }
#endif

#if DEMO4
            //
            // Add other streams to connect to the environment, simulating a temperature sensor reading and a feedback channel to control an A/C unit.
            //
            Console.WriteLine("Setting up external streams...");
            {
                var readings = iemgr.CreateSubject<SensorReading>("bart://sensors/home/livingroom/temperature/readings");
                var settings = iemgr.CreateSubject<double?>("bart://sensors/home/livingroom/temperature/settings");

                var rand = new Random();

                //
                // Speed and granularity of simulation.
                //
                var timeStep = TimeSpan.FromMinutes(15);
                var simulationDelay = TimeSpan.FromMilliseconds(250);

                //
                // Absolute value of temperature gain/loss per unit time of the house adjusting to the outside temperature.
                //
                var insulationTemperatureIncrement = 0.1;

                //
                // Absolute value of temperature gain/loss per unit time due to the A/C unit cooling down or heating up.
                //
                var acTemperatureIncrement = 0.2;

                //
                // Temperature sensitivity of the thermostat to trigger turning off the A/C unit, i.e. within this range from target.
                //
                var thermostatSensitivity = 0.5;

                //
                // Configuration of simulation: minimum and maximum temperature outside, and coldest time of day.
                //
                var outsideMin = 55;
                var outsideMax = 85;
                var coldestTime = new TimeSpan(5, 0, 0); // 5AM

                //
                // Scale for the temperature range, to multiply [0..1] by to obtain a temperature value that can be added to the minimum.
                //
                var scale = outsideMax - outsideMin;

                //
                // Offset to the midpoint of the temperature range. Outside temperature will vary as a sine wave around this value.
                //
                var offset = outsideMin + scale / 2;

                //
                // Random initial value inside, within the range of temperatures.
                //
                var inside = outsideMin + rand.NextDouble() * scale;

                //
                // null if A/C unit is off; otherwise, target temperature.
                //
                var target = default(double?);

                //
                // Clock driven by the simulation.
                //
                var time = DateTime.Today;

                //
                // Print commands arriving at thermostat.
                //
                settings.Subscribe(s =>
                {
                    target = s.item;
                    Console.WriteLine($"{time} thermostat> {(target == null ? "OFF" : "ON " + (target > inside ? "heating" : "cooling") + " to " + target)}");
                });

                //
                // Run simulation which adjusts both inside and outside temperature.
                //
                _ = Task.Run(async () =>
                {
                    while (true)
                    {
                        var now = (time.TimeOfDay - coldestTime - TimeSpan.FromHours(6)).TotalSeconds;
                        var secondsPerDay = TimeSpan.FromHours(24).TotalSeconds;

                        var outside = scale * Math.Sin(2 * Math.PI * now / secondsPerDay) / 2 + offset;

                        var environmentEffect = outside < inside ? -insulationTemperatureIncrement : insulationTemperatureIncrement;
                        var acUnitEffect = target != null ? (target < inside ? -acTemperatureIncrement : acTemperatureIncrement) : 0.0;

                        inside += environmentEffect + acUnitEffect;

                        if (target != null && Math.Abs(target.Value - inside) < thermostatSensitivity)
                        {
                            target = null;
                        }

                        Console.WriteLine($"{time} temperature> inside = {inside} outside = {outside} target = {target}");
                        readings.OnNext((Environment.TickCount, new SensorReading { Room = "Hallway", Temperature = inside }));

                        await Task.Delay(simulationDelay);
                        time += timeStep;
                    }
                });
            }

            //
            // Illustration of creating more advanced queries.
            //
            Console.WriteLine("Creating a complex subscription...");
            {
                var engine = new QueryEngine(new Uri("iot://reactor/1"), scheduler, store, iemgr);
                await engine.RecoverAsync(store.GetReader());

                var ctx = new ReactorContext(engine);

                // *** USER CODE STARTS HERE ***

                var input = ctx.GetObservable<string, SensorReading>(new Uri("iot://reactor/observables/ingress"));
                var output = ctx.GetObserver<string, double?>(new Uri("iot://reactor/observers/egress"));

                var readings = input("bart://sensors/home/livingroom/temperature/readings");
                var settings = output("bart://sensors/home/livingroom/temperature/settings");

                await readings.Window(4).SelectMany(w => w.Average(r => r.Temperature)).Select(t => t < 70 || t > 80 ? 75 : default(double?)).DistinctUntilChanged().SubscribeAsync(settings, new Uri("iot://reactor/subscription/BD/livingroom/comfy"), null, CancellationToken.None);

                // *** USER CODE ENDS HERE ***

                await Task.Delay(TimeSpan.FromSeconds(120));

                await engine.CheckpointAsync(store.GetWriter());
                await engine.UnloadAsync();
            }
#endif
        }
    }

    //
    // Reactor Core is built to be flexible with regards to data models, but the default data model that's well-supported originates from
    // a graph database effort in Bing that predates Reactor. The [Mapping] attributes below are the means to annotate properties. These
    // property names are used to normalize entity types in the serialized expression representation, so the query is not dependent on a
    // concrete type in an assembly, thus allowing the structure of data types (here to represent events) to be serialized across machine
    // boundaries without deployment of binaries.
    //

    public class SensorReading
    {
        [Mapping("iot://sensor/reading/room")]
        public string Room { get; set; }

        [Mapping("iot://sensor/reading/temperature")]
        public double Temperature { get; set; }
    }
}
