// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;

using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Deployable;
using Reaqtor.Remoting.Deployable.Streams;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;
using Reaqtor.Remoting.Reactor;
using Reaqtor.Remoting.Reactor.Client;
using Reaqtor.Remoting.Reactor.DomainFeeds;
using Reaqtor.Remoting.TestingFramework;
using Reaqtive.TestingFramework;

namespace Reaqtor.Remoting
{
    #region Aliases

    #endregion

    internal class Program
    {
        private static void Main()
        {
            Simple();
            ObserverQuotes();
            TestScheduler();
            SchedulePhysical();
            StreamCreation();
            ForeignFunctions();
            RecoveryFromEnvironment();
            RecoveryFromState();
            ConnectToRunningTcpPlatform();
            RecoveryStress(EnvironmentSetup(1000, 2));
            GroupBySupport();
            SubscriptionFactories();
            Delegation();

            UseAzureMetadata(); // TODO
            ReliableObservable(); // TODO - Definition of artifact removed in a checkin
            RemoteMetadataQuery(); // TODO - Attempt to deserialize metadata into interface type fails
        }

        /// <summary>
        /// Example of GroupBy operator support.
        /// </summary>
        [Demo]
        private static void GroupBySupport()
        {
            Run(CreateInMemory, async platform =>
            {
                Initialize(platform);

                var context = platform.CreateClient().Context;

                var range = context.GetObservable<int, int>(Reactor.Constants.Identifiers.Observable.Range.Uri);
                var cout = context.GetObserver<int>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

                var sub = await range(5).GroupBy(x => x % 2).SelectMany(g => g.Sum()).SubscribeAsync(cout, new Uri("reactor://test/subscription"), null);

                Console.ReadLine();

                await Task.Delay(1000);

                await sub.DisposeAsync();
            });
        }

        /// <summary>
        /// Example of using subscription factories.
        /// </summary>
        [Demo]
        private static void SubscriptionFactories()
        {
            Run(CreateInMemory, async platform =>
            {
                Initialize(platform);

                var context = platform.CreateClient().Context;

                var range = context.GetObservable<int, int>(Reactor.Constants.Identifiers.Observable.Range.Uri);
                var cout = context.GetObserver<int>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

                var coutRangeUri = new Uri("my://subscriptionFactory/coutRange");

                await context.DefineSubscriptionFactoryAsync<int>(coutRangeUri, count => range(count).Subscribe(cout), null, CancellationToken.None);

                var coutRange = context.GetSubscriptionFactory<int>(coutRangeUri);

                var sub1 = await coutRange.CreateAsync(new Uri("reactor://test/subscription/1"), 5, null);
                var sub2 = await coutRange.CreateAsync(new Uri("reactor://test/subscription/2"), 7, null);

                await Task.Delay(1000);

                await sub1.DisposeAsync();
                await sub2.DisposeAsync();
            });
        }

        /// <summary>
        /// Example of simple interactions with a reactive platform.
        /// </summary>
        [Demo]
        private static void Simple()
        {
            Run(CreateInMemory, async platform =>
            {
                Initialize(platform);

                var context = platform.CreateClient().Context;

                var range = context.GetObservable<int, int>(Reactor.Constants.Identifiers.Observable.Range.Uri);
                var cout = context.GetObserver<int>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

                var sub = await range(5).SubscribeAsync(cout, new Uri("reactor://test/subscription"), null);

                await Task.Delay(1000);

                await sub.DisposeAsync();
            });
        }

        /// <summary>
        /// Example of quotes in custom observer types.
        /// </summary>
        [Demo]
        private static void ObserverQuotes()
        {
            Run(CreateInMemory, async platform =>
            {
                Initialize(platform);

                var context = platform.CreateClient().Context;

                //
                // Define a few trivial observers without parameters. Note the underlying general-purpose "EOP" observer (for EgressObserverProxy)
                // takes in a parameter for the identifier of the observer in a remote system, and a parameter for an extra argument, which we give
                // a default value.
                //

                var xUri = new Uri("my://observer/x");
                var xObserverFactory = (Expression<Func<IAsyncReactiveQbserver<T>>>)(() => new EOP<int, T>("es://observer/x", 2 /* every other OnNext call, there will be a failure */).AsQbserver());
                var xObserver = context.Provider.CreateQbserver<T>(xObserverFactory.Body);
                await context.DefineObserverAsync<T>(xUri, xObserver, null, CancellationToken.None);

                var yUri = new Uri("my://observer/y");
                var yObserverFactory = (Expression<Func<IAsyncReactiveQbserver<T>>>)(() => new EOP<string, T>("es://observer/y", "bar").AsQbserver());
                var yObserver = context.Provider.CreateQbserver<T>(yObserverFactory.Body);
                await context.DefineObserverAsync<T>(yUri, yObserver, null, CancellationToken.None);

                //
                // Define an observer with a trivial parameter type.
                //

                var zUri = new Uri("my://observer/z");
                await context.DefineObserverAsync<int, T>(zUri, i => new EOP<int, T>("es://observer/z", i).AsQbserver(), null, CancellationToken.None);

                //
                // Define an observer parameterized on other observers. Note the use of a tuple to bundle these arguments together.
                //

                var fUri = new Uri("my://observer/f");
                await context.DefineObserverAsync<IAsyncReactiveQbserver<T>, IAsyncReactiveQbserver<T>, T>(fUri, (l, r) => new EOP<Tuple<IObserver<T>, IObserver<T>>, T>("es://observer/f", new Tuple<IObserver<T>, IObserver<T>>(l.To<IAsyncReactiveQbserver<T>, IObserver<T>>(), r.To<IAsyncReactiveQbserver<T>, IObserver<T>>())).AsQbserver(), null, CancellationToken.None);

                //
                // Define an observer parameterized on another observer as well as a lambda expression (which illustrates resolving the challenges
                // encountered when trying to provide a quoted representation at runtime).
                //

                var cfUri = new Uri("my://observer/cf");
                await context.DefineObserverAsync<IAsyncReactiveQbserver<T>, Expression<Func<Exception, IAsyncReactiveQbserver<T>>>, T>(
                    cfUri,
                    (source, selector) => new EOP<Tuple<IObserver<T>, Func<Exception, IObserver<T>>>, T>(
                        "es://observer/cf",
                        new Tuple<IObserver<T>, Func<Exception, IObserver<T>>>(
                            source.To<IAsyncReactiveQbserver<T>, IObserver<T>>(),
                            selector.To<Expression<Func<Exception, IAsyncReactiveQbserver<T>>>, Func<Exception, IObserver<T>>>()
                        )
                    ).AsQbserver(),
                    null,
                    CancellationToken.None);

                //
                // Get proxies to the whole shebang.
                //

                var range = context.GetObservable<int, int>(Reactor.Constants.Identifiers.Observable.Range.Uri);

                var x = context.GetObserver<int>(xUri);
                var y = context.GetObserver<int>(yUri);
                var z = context.GetObserver<int, int>(zUri)(1983);
                var f = context.GetObserver<IAsyncReactiveQbserver<int>, IAsyncReactiveQbserver<int>, int>(fUri);

                //
                // Compose an observer from x, y, z, f, and cf. Because the remoting stack does a poor job at closure elimination right now, we
                // hand-roll the expression tree representing the invocation of cf. The sample below is isomorphic to a more realistic usage of
                //
                //     http(uri1).Or(ftp(uri2)).Catch(ex => pigeon(ex))
                //
                // after substituting x for http, y for ftp, f for Or, cf for Catch, and z for pigeon.
                //

                var o = context.Provider.CreateQbserver<int>(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbserver<int>, Expression<Func<Exception, IAsyncReactiveQbserver<int>>>, IAsyncReactiveQbserver<int>>), cfUri.OriginalString),
                        f(x, y).Expression,
                        Expression.Lambda(
                            z.Expression,
                            Expression.Parameter(typeof(Exception), "_")
                        )
                    )
                );

                //
                // Create the subscription which will illustrate the quotation of the observers at runtime.
                //

                var sub = await range(10).SubscribeAsync(o, new Uri("reactor://test/subscription"), null);

                await Task.Delay(1000);

                await sub.DisposeAsync();
            });
        }

        /// <summary>
        /// Example of how to schedule remote actions on a test scheduler.
        /// </summary>
        [Demo]
        private static void TestScheduler()
        {
            using var environment = new AppDomainReactiveEnvironment();
            using var platform = new AppDomainReactivePlatform(environment);

            platform.Configuration.SchedulerType = SchedulerType.Test;

            Initialize(environment, platform);

            var scheduler = (ITestScheduler)platform.QueryEvaluators.First().Scheduler;
            scheduler.ScheduleAbsolute(100, () => { Console.WriteLine("foo"); });
            scheduler.ScheduleAbsolute(150, () => { Console.WriteLine("bar"); });
            scheduler.Start();
        }

        /// <summary>
        /// Example of how to schedule remote actions on a regular scheduler.
        /// </summary>
        [Demo]
        private static void SchedulePhysical()
        {
            using var environment = new AppDomainReactiveEnvironment();
            using var platform = new AppDomainReactivePlatform(environment);

            Initialize(environment, platform);

            var scheduler = platform.QueryEvaluators.First().Scheduler;

            scheduler.Schedule(() => { Console.WriteLine("bar@{0}", DateTimeOffset.Now); });
            scheduler.Schedule(new TimeSpan(0, 0, 5), () => { Console.WriteLine("foo@{0}", DateTimeOffset.Now); });
            var dto = DateTimeOffset.Now + TimeSpan.FromSeconds(10);
            scheduler.Schedule(dto, () => { Console.WriteLine("qux@{0}", DateTimeOffset.Now); });

            Task.Delay(15000).Wait();
        }

        /// <summary>
        /// Example of how to create a stream and subscribe to it.
        /// </summary>
        [Demo]
        private static void StreamCreation()
        {
            using var environment = new TcpReactiveEnvironment();
            using var platform = new TcpReactivePlatform(environment);

            Initialize(environment, platform);

            var ctx = platform.CreateClient().Context;

            var streamFactory = ctx.GetStreamFactory<Person, Person>(Platform.Constants.Identifiers.Observable.FireHose.Uri);
            var stream = streamFactory.CreateAsync(new Uri("reactor://stream/foo"), null).Result;

            var cout = ctx.GetObserver<Person>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

            var subscription = ctx.GetObservable<Person>(new Uri("reactor://stream/foo")).SubscribeAsync(cout, new Uri("reactor://subscription"), null).Result;

            var observer = ctx.GetObserver<Person>(new Uri("reactor://stream/foo"));

            observer.OnNextAsync(new Person { FirstName = "John", LastName = "Smith", Location = "Home" }).Wait();
            observer.OnNextAsync(new Person { FirstName = "Bob", LastName = "Smith", Location = "Home" }).Wait();
            observer.OnNextAsync(new Person { FirstName = "Joe", LastName = "Smith", Location = "Home" }).Wait();

            Task.Delay(10000).Wait();
        }

        /// <summary>
        /// Example of how foreign functions work.
        /// </summary>
        [Demo]
        private static void ForeignFunctions()
        {
            ForeignFunctions1();
            ForeignFunctions2();

            static void ForeignFunctions1()
            {
                using var environment = new InMemoryReactiveEnvironment();
                using var platform = new InMemoryReactivePlatform(environment);

                Initialize(environment, platform);

                var ctx = platform.CreateClient().Context;

                var range = ctx.GetObservable<int, int>(Reactor.Constants.Identifiers.Observable.Range.Uri);
                var cout = ctx.GetObserver<string>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

                Console.WriteLine("Press ENTER to dispose...");

                var sub = range(5)
                    .Select(value => GenerateLanguage(value, "en-us"))
                    .SubscribeAsync(cout, new Uri("reactor://test/subscription"), null).Result;

                Task.Delay(1000).Wait();

                sub.DisposeAsync().Wait();
            }

            static void ForeignFunctions2()
            {
                using var environment = new InMemoryReactiveEnvironment();
                using var platform = new InMemoryReactivePlatform(environment);

                Initialize(environment, platform);

                var ctx = platform.CreateClient().Context;

                Console.WriteLine("Creating stream...");

                var streamFactory = ctx.GetStreamFactory<Person, Person>(Platform.Constants.Identifiers.Observable.FireHose.Uri);
                var stream = streamFactory.CreateAsync(new Uri("reactor://stream/foo"), null).Result;

                var people = ctx.GetObservable<Person>(new Uri("reactor://stream/foo"));
                var cout = ctx.GetObserver<string>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

                Console.WriteLine("Creating subscription...");

                var sub = people
                    .Select(p => Json.Serialize(p)) // NB: fast serialization doesn't work at the moment; no support for enums yet
                    .SubscribeAsync(cout, new Uri("reactor://test/subscription"), null).Result;

                Console.WriteLine("Publishing events...");

                var observer = ctx.GetObserver<Person>(new Uri("reactor://stream/foo"));

                observer.OnNextAsync(new Person { FirstName = "John", LastName = "Smith", Location = "Home" }).Wait();
                observer.OnNextAsync(new Person { FirstName = "Bob", LastName = "Smith", Location = "Home" }).Wait();
                observer.OnNextAsync(new Person { FirstName = "Joe", LastName = "Smith", Location = "Home" }).Wait();

                Task.Delay(10000).Wait();

                sub.DisposeAsync().Wait();
            }
        }

        /// <summary>
        /// Generates language describing an object.
        /// </summary>
        /// <typeparam name="TSource">The type of object.</typeparam>
        /// <param name="instance">The object to describe.</param>
        /// <returns>The description of the object.</returns>
        [KnownResource(Platform.Constants.Identifiers.GenerateLanguage)]
        public static string GenerateLanguage<TSource>(TSource instance, string lang)
        {
            throw new NotImplementedException("This operator should only be used in expressions.");
        }

        /// <summary>
        /// Example of recovering a platform from an existing environment.
        /// </summary>
        [Demo]
        private static void RecoveryFromEnvironment()
        {
            Console.Write("Loading environment... ");

            using var environment = new AppDomainReactiveEnvironment();

            Console.WriteLine("Done.");

            Console.Write("Loading reactive platform... ");

            using (var platform = new AppDomainReactivePlatform(environment))
            {
                Console.WriteLine("Done.");

                Console.Write("Initializing reactive platform... ");
                Initialize(environment, platform);
                Console.WriteLine("Done.");

                var context = platform.CreateClient().Context;

                var cout = context.GetObserver<long>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

                context.Timer(DateTime.Now, TimeSpan.FromSeconds(1))
                    .SubscribeAsync(cout, new Uri("rctr://sub/foo"), null).Wait();

                Task.Delay(3000).Wait();

                Console.Write("Checkpointing... ");
                platform.QueryEvaluators.First().Checkpoint();
                Console.WriteLine("Done.");

                Task.Delay(3000).Wait();
            }

            Console.WriteLine("Crash");

            Console.Write("Loading reactive platform... ");

            using (var platform = new AppDomainReactivePlatform(environment))
            {
                Console.WriteLine("Done.");

                Console.Write("Recovering reactive platform... ");
                platform.StartAsync(CancellationToken.None).Wait();
                Console.WriteLine("Done.");

                Task.Delay(3000).Wait();
            }
        }

        /// <summary>
        /// Example of recovering a platform from a serialized state store.
        /// </summary>
        [Demo]
        private static void RecoveryFromState()
        {
            var stateStoreStream = default(byte[]);
            var kvsStream = default(byte[]);

            Console.Write("Loading environment... ");
            var environment = new AppDomainReactiveEnvironment();
            Console.WriteLine("Done.");

            Console.Write("Loading reactive platform... ");

            using (var platform = new AppDomainReactivePlatform(environment))
            {
                Console.WriteLine("Done.");

                Console.Write("Initializing reactive platform... ");
                Initialize(environment, platform);
                Console.WriteLine("Done.");

                var context = platform.CreateClient().Context;

                var cout = context.GetObserver<long>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

                context.Timer(DateTime.Now, TimeSpan.FromSeconds(1))
                    .SubscribeAsync(cout, new Uri("rctr://sub/basic"), null).Wait();

                context.Timer(DateTime.Now, TimeSpan.FromSeconds(1))
                    .SubscribeAsync(cout, new Uri("rctr://sub/deleteAfterCheckpoint"), null).Wait();

                Task.Delay(3000).Wait();

                Console.Write("Checkpointing... ");
                platform.QueryEvaluators.First().Checkpoint();
                stateStoreStream = environment.StateStoreService.GetInstance<IReactiveStateStoreConnection>().SerializeStateStore();
                Console.WriteLine("Done.");

                context.GetSubscription(new Uri("rctr://sub/deleteAfterCheckpoint")).DisposeAsync().Wait();

                context.Timer(DateTime.Now, TimeSpan.FromSeconds(1))
                    .SubscribeAsync(cout, new Uri("rctr://sub/createAfterCheckpoint"), null).Wait();

                kvsStream = environment.KeyValueStoreService.GetInstance<ITransactionalKeyValueStoreConnection>().SerializeStore();

                Task.Delay(3000).Wait();
            }

            Console.WriteLine("Crash");
            Console.WriteLine("Press any key to load a new platform.");
            Console.Read();

            Console.Write("Loading environment... ");
            var newEnvironment = new ReactiveEnvironment(environment.MetadataService, environment.MessagingService, new AppDomainStateStoreService(), new AppDomainKeyValueStoreService());
            newEnvironment.StateStoreService.StartAsync(CancellationToken.None).Wait();
            newEnvironment.KeyValueStoreService.StartAsync(CancellationToken.None).Wait();
            Console.WriteLine("Done.");

            Console.Write("Loading reactive platform... ");

            using (var platform = new AppDomainReactivePlatform(newEnvironment))
            {
                Console.WriteLine("Done.");

                Console.Write("Recovering reactive platform... ");
                newEnvironment.StateStoreService.GetInstance<IReactiveStateStoreConnection>().DeserializeStateStore(stateStoreStream);
                newEnvironment.KeyValueStoreService.GetInstance<ITransactionalKeyValueStoreConnection>().DeserializeStore(kvsStream);
                platform.StartAsync(CancellationToken.None).Wait();
                Console.WriteLine("Done.");

                Task.Delay(3000).Wait();
            }

            newEnvironment.StateStoreService.StopAsync(CancellationToken.None);
            environment.Dispose();
        }

        /// <summary>
        /// Example of using Azure dev storage instead of remoting storage.
        /// </summary>
        [Demo]
        private static void UseAzureMetadata()
        {
            Run(
                () => new InMemoryReactiveEnvironment("UseDevelopmentStorage=true"),
                e => new InMemoryReactivePlatform(e),
                async (e, platform) =>
                {
                    var context = platform.CreateClient().Context;

                    var range = context.GetObservable<int, int>(Reactor.Constants.Identifiers.Observable.Range.Uri);
                    var cout = context.GetObserver<int>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

                    var sub = await range(5).SubscribeAsync(cout, new Uri("reactor://test/subscription"), null);

                    await Task.Delay(1000);

                    await sub.DisposeAsync();
                });
        }

        /// <summary>
        /// Example of subscribing against an existing TCP Reactive platform.
        /// </summary>
        private static void ConnectToRunningTcpPlatform()
        {
            var ctx = new RemotingClientContext("tcp://127.0.0.1:8080/QueryCoordinator");
            try
            {
                ctx.GetSubscription(new Uri("rctr://test/sub")).DisposeAsync(CancellationToken.None).Wait();
            }
            finally
            {
                ctx.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)).Take(5).SubscribeAsync(ctx.GetObserver<long>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri), new Uri("rctr://test/sub"), null);
            }
        }

        private static void ReliableObservable()
        {
            Run(CreateInMemory, async platform =>
            {
                Initialize(platform);

                var context = platform.CreateClient().Context;

                var reliableStartWith = context.GetObservable<string, int[], int>(new Uri("reactor://platform.bing.com/qeh/operators/toReliableObservable"));
                var cout = context.GetObserver<int>(Platform.Constants.Identifiers.Observer.ConsoleObserver.Uri);

                var sub = await reliableStartWith("foo", new[] { 1, 2, 3 }).SubscribeAsync(cout, new Uri("reactor://test/subscription"), null);

                await Task.Delay(1000);

                await sub.DisposeAsync();
            });
        }

        private static IReactiveEnvironment[] EnvironmentSetup(int subscriptionCount = 60000, int qeCount = 1)
        {
            IAsyncReactiveQbservable<TrafficInfo> queryFactory(IReactiveProxy context)
            {
                return
                    context
                        .Never<TrafficInfo>()
                        .TakeUntil(DateTimeOffset.Now.AddDays(100))
                        .DelaySubscription(DateTimeOffset.Now)
                        .Select(_ => context.Timer(TimeSpan.FromMinutes(10)).Select(__ => new TrafficInfo
                        {
                            FlowInfo = new TrafficFlowInfo { DelayInSeconds = 300, HovDelayInSeconds = 150 },
                            SubscriptionId = "test://sub/",
                            TimestampUTC = DateTime.Now,
                        }))
                        .StartWith(context.Timer(TimeSpan.FromMinutes(5)).Select(_ => new TrafficInfo
                        {
                            FlowInfo = new TrafficFlowInfo { DelayInSeconds = 0, HovDelayInSeconds = 0 },
                            SubscriptionId = "test://sub/",
                            TimestampUTC = DateTime.Now,
                        }))
                        .Switch()
                        .Take(2);
            }

            var environment = new InMemoryReactiveEnvironment();
            environment.StartAsync(CancellationToken.None).Wait();

            var sw = Stopwatch.StartNew();
            Console.Write("Deploying to environment... ");
            using (var platform = new InMemoryReactivePlatform(environment))
            {
                platform.StartAsync(CancellationToken.None).Wait();
                new ReactivePlatformDeployer(platform, new TestDeployable(), new Reactor.Deployable(), new DomainFeedsDeployable()).Deploy();
            }
            Console.WriteLine("Done in {0} ms.", sw.ElapsedMilliseconds);

            sw.Restart();
            Console.Write("Starting {0} QEs... ", qeCount);
            var environments = Enumerable.Repeat(0, qeCount).Select(_ => new InMemoryReactiveEnvironment(environment.MetadataService, environment.MessagingService)).ToArray();
            Parallel.ForEach(environments, e => e.StartAsync(CancellationToken.None).Wait());

            var platforms = environments.Select(e => new InMemoryTestPlatform(e)).ToList();
            Parallel.ForEach(platforms, p => { p.Configuration.SchedulerType = SchedulerType.Static; p.StartAsync(CancellationToken.None).Wait(); });
            Console.WriteLine("Done in {0} ms.", sw.ElapsedMilliseconds);

            var contexts = platforms.Select(p => p.CreateClient().Context).ToArray();

            sw.Restart();
            Console.Write("Creating {0} subscriptions... ", subscriptionCount);
            Parallel.ForEach(
                Enumerable.Range(1, subscriptionCount),
                i =>
                {
                    var context = contexts[i % qeCount];
                    var query = queryFactory(context);
                    query.SubscribeAsync(new Uri("test://subscription/" + i)).Wait();
                });
            Console.WriteLine("Done in {0} ms.", sw.ElapsedMilliseconds);

            sw.Restart();
            Console.Write("Checkpointing {0} QEs... ", qeCount);
            Parallel.ForEach(platforms, p => p.QueryEvaluators.First().Checkpoint());
            Console.WriteLine("Done in {0} ms.", sw.ElapsedMilliseconds);

            sw.Restart();
            Console.Write("Failing {0} QEs... ", qeCount);
            Parallel.ForEach(platforms, p => p.Dispose());
            Console.WriteLine("Done in {0} ms.", sw.ElapsedMilliseconds);

            return environments;
        }

        private static void RecoveryStress(params IReactiveEnvironment[] environments)
        {
            Console.Write("Recovering {0} QEs... ", environments.Length);
            var platforms = environments.Select(e => new InMemoryReactivePlatform(e)).ToList();
            var tasks = new Task[platforms.Count];
            var recoveries = new TimeSpan[platforms.Count];
            ForEach(platforms, (p, i) => tasks[i] = Task.Factory.StartNew(() =>
            {
                p.Configuration.SchedulerType = SchedulerType.Static;
                var recoveryTimer = Stopwatch.StartNew();
                p.StartAsync(CancellationToken.None).Wait();
                recoveries[i] = recoveryTimer.Elapsed;
            }));
            Task.WaitAll(tasks);
            Console.WriteLine("Done in {0} ms.", recoveries.Max().TotalMilliseconds);
            Console.WriteLine(string.Join(",", recoveries.Select(t => t.TotalMilliseconds)));

            var sw = Stopwatch.StartNew();
            Console.Write("Failing {0} QEs... ", environments.Length);
            Parallel.ForEach(platforms, p => p.Dispose());
            Console.WriteLine("Done in {0} ms.", sw.ElapsedMilliseconds);
        }

        private static void ForEach<T>(List<T> list, Action<T, int> body)
        {
            list.Select((x, i) => (x, i)).ToList().ForEach(a => body(a.x, a.i));
        }

        private static void RemoteMetadataQuery()
        {
            Run(CreateInMemory, platform =>
            {
                Initialize(platform);
                var ctx = platform.CreateClient().Context;

                Console.WriteLine("Filtered expressions...");

                foreach (var observable in ctx.Observables.Where(kv => kv.Key.Scheme != "rx" && kv.Key.Scheme != "reactor").Select(kv => kv.Value))
                {
                    Console.WriteLine(observable.Expression.ToTraceString());
                }

                Console.WriteLine("All expressions...");

                foreach (var observable in ctx.Observables)
                {
                    Console.WriteLine(observable.Value.Expression.ToTraceString());
                }

                return Task.FromResult(true);
            });
        }

        [Demo]
        private static void Delegation()
        {
            const int numUsers = 100;
            const int numNotifications = 1000;
            var users = Enumerable.Range(0, numUsers).Select(_ => Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())).ToArray();
            var rand = new Random(0);
            var notifications = Enumerable.Range(0, numNotifications).Select(_ =>
            {
                var user = users[rand.Next(users.Length)];
                return new Geocoordinate
                {
                    RequestId = Guid.NewGuid().ToString(),
                    UserId = user.Item1,
                    UserIdHashcode = rand.Next(0, int.MaxValue),
                    DeviceId = Guid.NewGuid().ToString(),
                    DataCenter = "bn2",
                    UserSignal = new UserSignal
                    {
                        Type = "com.bing.merino.device.geolocation.geocoordinate",
                        Timestamp = DateTime.UtcNow,
                        ClientRequestid = "00000000-0000-0000-0000-000000000000",
                        AgentInstanceId = user.Item2,
                        Value = new GeocoordinateValue
                        {
                            Status = null,
                            Accuracy = rand.Next(0, 1000).ToString(),
                            Altitude = null,
                            AltitudeAccuracy = null,
                            Heading = null,
                            Latitude = rand.NextDouble(),
                            Longtitude = rand.NextDouble(),
                            PositionSource = null,
                            Speed = null,
                        }
                    }
                };
            }).ToArray();

            var disposables = new IAsyncDisposable[numUsers];
            Stress<Geocoordinate, Geocoordinate>(async (ctx, stream) =>
            {
                Program.Count = 0;
                Program.MaxCount = numNotifications;
                Program.RunDone = new TaskCompletionSource<bool>();

                for (var i = 0; i < disposables.Length; i++)
                {
                    var userId = users[i].Item1;
                    var agentId = users[i].Item2;

                    var filter = (Expression<Func<string, string, Func<Geocoordinate, bool>>>)((s, t) => signal => signal.UserId == s && string.Equals(signal.UserSignal.AgentInstanceId, t, StringComparison.Ordinal) && true);
                    var f = (Expression<Func<Geocoordinate, bool>>)BetaReducer.Reduce(Expression.Invoke(filter, Expression.Constant(userId), Expression.Constant(agentId)));

                    disposables[i] = await stream.Where(f).SubscribeAsync(ctx.GetObserver<Geocoordinate>(new Uri("custom:break")), new Uri("custom:sub" + i), null, CancellationToken.None);
                }

                var sw = Stopwatch.StartNew();

                foreach (var n in notifications)
                    await stream.OnNextAsync(n, CancellationToken.None);

                await RunDone.Task;

                Console.WriteLine(sw.ElapsedMilliseconds);
                Console.WriteLine(GC.GetTotalMemory(forceFullCollection: true));

                for (var i = 0; i < disposables.Length; i++)
                {
                    await disposables[i].DisposeAsync();
                }

                Console.WriteLine(GC.GetTotalMemory(forceFullCollection: true));
            }, usePartitioned: true);
        }

        private static void Stress<TInput, TOutput>(Func<ReactiveClientContext, IAsyncReactiveQubject<TInput, TOutput>, Task> action, bool usePartitioned)
        {
            if (usePartitioned)
            {
                RunPartitioned(action);
            }
            else
            {
                RunFirehose(action);
            }

            static void RunFirehose(Func<ReactiveClientContext, IAsyncReactiveQubject<TInput, TOutput>, Task> action)
            {
                Run(CreateInMemory, async platform =>
                {
                    Initialize(platform);

                    var ctx = platform.CreateClient().Context;

                    var streamFactory = ctx.GetStreamFactory<TInput, TOutput>(Platform.Constants.Identifiers.Observable.FireHose.Uri);
                    var stream = await streamFactory.CreateAsync(new Uri("reactor://stream/foo"), null, CancellationToken.None);

                    var brk = (Expression<Func<IAsyncReactiveQbserver<T>>>)(() => new MyObserver<T>().To<IObserver<T>, IAsyncReactiveQbserver<T>>());
                    await ctx.DefineObserverAsync<T>(new Uri("custom:break"), ctx.Provider.CreateQbserver<T>(brk.Body), null, CancellationToken.None);

                    var watch = Stopwatch.StartNew();

                    await action(ctx, stream);

                    Console.WriteLine("Normal stream: {0} ms.", watch.ElapsedMilliseconds);
                });
            }

            static void RunPartitioned(Func<ReactiveClientContext, IAsyncReactiveQubject<TInput, TOutput>, Task> action)
            {
                Run(CreateInMemory, async platform =>
                {
                    Initialize(platform);

                    var ctx = platform.CreateClient().Context;

                    var sf = ctx.Provider.CreateQubjectFactory<T, T>((Expression<Func<IMultiSubject>>)(() => new PMS()));
                    await ctx.DefineStreamFactoryAsync<T, T>(new Uri("custom:partitionedFactory"), sf, null, CancellationToken.None);

                    var brk = (Expression<Func<IAsyncReactiveQbserver<T>>>)(() => new MyObserver<T>().To<IObserver<T>, IAsyncReactiveQbserver<T>>());
                    await ctx.DefineObserverAsync<T>(new Uri("custom:break"), ctx.Provider.CreateQbserver<T>(brk.Body), null, CancellationToken.None);

                    Expression<Func<ISubscribable<T>, Expression, IEqualityComparer<R>, R, IAsyncReactiveQbservable<T>>> def = (s, f, e, k) => s.Partition(f, e, k).To<ISubscribable<T>, IAsyncReactiveQbservable<T>>();
                    await ctx.DefineObservableAsync<ISubscribable<T>, Expression, IEqualityComparer<R>, R, T>(new Uri("custom:partition"), def, null, CancellationToken.None);

                    var sfUseSite = ctx.Provider.CreateQubjectFactory<TInput, TOutput>(Expression.Parameter(typeof(Func<IMultiSubject>), "custom:partitionedFactory"));
                    var stream = await sfUseSite.CreateAsync(new Uri("custom:partitioned"), null, CancellationToken.None);

                    var watch = Stopwatch.StartNew();

                    await action(ctx, stream);

                    Console.WriteLine("Partitioned stream: {0} ms.", watch.ElapsedMilliseconds);
                });
            }
        }

        public static int Count;
        public static int MaxCount;
        public static TaskCompletionSource<bool> RunDone;

        private class MyObserver<T> : IObserver<T>
        {
            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(T value)
            {
                Interlocked.Increment(ref Count);
                if (Volatile.Read(ref Count) >= MaxCount)
                    RunDone.TrySetResult(true);
            }
        }

        private static void Initialize(IReactivePlatform platform)
        {
            platform.StartAsync(CancellationToken.None).Wait();
            var deployer = new ReactivePlatformDeployer(platform, new Deployable.Deployable(), new Reactor.Deployable(), new DomainFeedsDeployable());
            deployer.Deploy();
        }

        private static void Initialize(IReactiveEnvironment environment, IReactivePlatform platform)
        {
            environment.StartAsync(CancellationToken.None).Wait();
            Initialize(platform);
        }

        private static IReactivePlatform CreateInMemory()
        {
            return new InMemoryReactivePlatform();
        }

#if UNUSED // NB: Can be used to switch over demos to other remoting implementations.
        private static IReactivePlatform CreateAppDomain()
        {
            return new AppDomainReactivePlatform();
        }

        private static IReactivePlatform CreateTcp()
        {
            return new TcpReactivePlatform();
        }
#endif

        private static void Run(Func<IReactivePlatform> createPlatform, Func<IReactivePlatform, Task> action)
        {
            using var platform = createPlatform();

            action(platform).Wait();
        }

#if UNUSED // NB: Kept as a reference implementation for client config.
        private static void InitializeClient()
        {
            var clientProvider = new System.Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider();
            var serverProvider = new System.Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider { TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full };
            var props = new System.Collections.Hashtable
            {
                { "port", 0 },
                { "name", System.Guid.NewGuid().ToString() },
                { "typeFilterLevel", System.Runtime.Serialization.Formatters.TypeFilterLevel.Full }
            };
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(new System.Runtime.Remoting.Channels.Tcp.TcpChannel(props, clientProvider, serverProvider), ensureSecurity: false);
        }
#endif

        private static void Run(Func<IReactiveEnvironment> getEnvironment, Func<IReactiveEnvironment, IReactivePlatform> getPlatform, Func<IReactiveEnvironment, IReactivePlatform, Task> action)
        {
            using var environment = getEnvironment();
            using var platform = getPlatform(environment);

            Initialize(environment, platform);

            action(environment, platform).Wait();
        }
    }
}
