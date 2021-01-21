// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor;
using Reaqtor.Remoting.Reactor.Client;
using Reaqtor.Remoting.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Constants = Reaqtor.Remoting.Client.Constants;
using PlatformConstants = Reaqtor.Remoting.Platform.Constants;
using ReactorConstants = Reaqtor.Remoting.Reactor.Client.Constants;
using TestConstants = Reaqtor.Remoting.TestingFramework.Constants;

namespace Tests.Reaqtor.Remoting
{
    [TestClass]
    public class RemotingTest : RemotingTestBase
    {
        [TestMethod]
        public async Task Remoting_DefineUndefine_Observable_MetadataState()
        {
            var uri = new Uri("reactor://definetest/observable");

            await RunAsync(
                async (ReactiveClientContext ctx) =>
                {
                    var range = ctx.GetObservable<int, int>(ReactorConstants.Identifiers.Observable.Range.Uri);
                    await ctx.DefineObservableAsync<int>(uri, range(10), null, CancellationToken.None);
                },
                new MetadataState
                {
                    Type = MetadataEntityType.Observable,
                    Uri = uri,
                    Expression = Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<int>, IAsyncReactiveQbservable<int>>), ReactorConstants.Identifiers.Observable.Range.String),
                        Expression.New(
                            typeof(Tuple<int>).GetConstructor(new[] { typeof(int) }),
                            new Expression[] { Expression.Constant(10, typeof(int)) },
                            typeof(Tuple<int>).GetProperty("Item1")
                        )
                    )
                }
            );
        }

        [TestMethod]
        public async Task Remoting_DefineUndefine_Observer_MetadataState()
        {
            var uri = new Uri("reactor://definetest/observer");
            await RunAsync(
                async (ReactiveClientContext ctx) =>
                {
                    var test = ctx.GetObserver<string, int>(TestConstants.Test.TestObserver.Uri);
                    await ctx.DefineObserverAsync<int>(uri, test("foo"), null, CancellationToken.None);
                },
                new MetadataState
                {
                    Type = MetadataEntityType.Observer,
                    Uri = uri,
                    Expression = Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<string>, IAsyncReactiveQbserver<int>>), TestConstants.Test.TestObserver.String),
                        Expression.New(
                            typeof(Tuple<string>).GetConstructor(new[] { typeof(string) }),
                            new Expression[] { Expression.Constant("foo", typeof(string)) },
                            typeof(Tuple<string>).GetProperty("Item1")
                        )
                    )
                }
            );
        }

        [TestMethod]
        public async Task Remoting_DefineUndefine_StreamFactory_MetadataState()
        {
            var uri = new Uri("reactor://definetest/streamfactory");
            var fakeStreamFactoryUri = "reactor://fake/streamfactory";

            await RunAsync(
                async (ReactiveClientContext ctx) =>
                {
                    var test = ctx.GetStreamFactory<string, int, int>(new Uri(fakeStreamFactoryUri));
                    await ctx.DefineStreamFactoryAsync<string, int, int>(uri, test, null, CancellationToken.None);
                },
                new MetadataState
                {
                    Type = MetadataEntityType.StreamFactory,
                    Uri = uri,
                    Expression = Expression.Parameter(typeof(Func<string, IAsyncReactiveQubject<int, int>>), fakeStreamFactoryUri)
                }
            );
        }

        [TestMethod]
        public async Task Remoting_DefineUndefine_SubscriptionFactory_MetadataState()
        {
            var uri = new Uri("reactor://definetest/subscriptionfactory");

            // NB: Remoting's QC implementation logic in InjectCleanupHook prevents a trivial alias for an existing stream factory from working right now,
            //     so we have to use a more complex definition here. Unfortunately, the current implementation reveals this cleanup hook implementation
            //     detail to the user of metadata APIs.

            var tuple = Expression.Parameter(typeof(Tuple<int>));
            var qbservable = Expression.Parameter(typeof(Func<Tuple<int>, IAsyncReactiveQbservable<int>>), "reactor://observable/foo");
            var qbserver = Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), "reactor://observer/bar");
            var cleanup = Expression.Parameter(typeof(Func<Tuple<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>>, IAsyncReactiveQubscription>), "reactor://platform.bing.com/qeh/operators/withCleanupSubscription");

            var expression =
                Expression.Lambda(
                    Expression.Invoke(
                        cleanup,
                        Expression.New(
                            typeof(Tuple<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>>).GetConstructors().Single(),
                            new Expression[]
                            {
                                Expression.Invoke(
                                    qbservable,
                                    Expression.New(
                                        typeof(Tuple<int>).GetConstructors().Single(),
                                        new Expression[]
                                        {
                                            Expression.Property(tuple, "Item1")
                                        },
                                        new MemberInfo[]
                                        {
                                            typeof(Tuple<int>).GetProperty("Item1")
                                        }
                                    )
                                ),
                                qbserver
                            },
                            new MemberInfo[]
                            {
                                typeof(Tuple<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>>).GetProperty("Item1"),
                                typeof(Tuple<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>>).GetProperty("Item2")
                            }
                        )
                    ),
                    tuple
                );

            await RunAsync(
                async (ReactiveClientContext ctx) =>
                {
                    var foo = ctx.GetObservable<int, int>(new Uri("reactor://observable/foo"));
                    var bar = ctx.GetObserver<int>(new Uri("reactor://observer/bar"));

                    await ctx.DefineSubscriptionFactoryAsync<int>(uri, x => Extensions.Subscribe(foo(x), bar), null, CancellationToken.None);
                },
                new MetadataState
                {
                    Type = MetadataEntityType.SubscriptionFactory,
                    Uri = uri,
                    Expression = expression
                }
            );
        }

        [TestMethod]
        public async Task Remoting_SubscribeGetDispose_ObserverState()
        {
            var uri = new Uri("reactor://test/subscription");
            var testObserverName = new Uri("reactor://test/observer");

            await AssertVirtual<ReactiveClientContext, int>(
                async (ctx, _) =>
                {
                    var xs = ctx.GetObservable<int, int>(ReactorConstants.Identifiers.Observable.Range.Uri)(5);
                    var observer = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri)(testObserverName);
                    await xs.SubscribeAsync(observer, uri, null, CancellationToken.None);
                },
                new ObserverState<int>(testObserverName)
                {
                    ObserverMessage.OnNext(0),
                    ObserverMessage.OnNext(1),
                    ObserverMessage.OnNext(2),
                    ObserverMessage.OnNext(3),
                    ObserverMessage.OnNext(4),
                    ObserverMessage.OnCompleted<int>()
                }
            );
        }

        [TestMethod]
        public async Task Remoting_Subscribe_VirtualTime_ObserverState()
        {
            var uri = new Uri("reactor://test/subscription");
            var testObserverName = new Uri("reactor://test/observer");

            await AssertVirtual<ReactiveClientContext, int>(
                (ctx, scheduler) =>
                {
                    var observer = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri)(testObserverName);
                    scheduler.ScheduleAbsolute(200, () => ctx.Empty<int>().StartWith(0, 1, 2, 3, 4).SubscribeAsync(observer, uri, null, CancellationToken.None));

                    var sub = ctx.GetSubscription(uri);
                    scheduler.ScheduleAbsolute(500, () => sub.DisposeAsync(CancellationToken.None));

                    scheduler.ScheduleAbsolute(501, async () =>
                        {
                            try
                            {
                                await sub.DisposeAsync(CancellationToken.None);
                            }
                            catch (AggregateException ex)
                            {
                                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException));
                            }
                        });

                    return Task.FromResult(true);
                },
                new ObserverState<int>(testObserverName)
                {
                    ObserverMessage.OnNext(201, 0),
                    ObserverMessage.OnNext(202, 1),
                    ObserverMessage.OnNext(203, 2),
                    ObserverMessage.OnNext(204, 3),
                    ObserverMessage.OnNext(205, 4),
                    ObserverMessage.OnCompleted<int>(206)
                }
            );
        }

        [TestMethod]
        public async Task Remoting_Range_VirtualTime_ObserverState_Schedule()
        {
            var observer = default(Func<Uri, IAsyncReactiveQbserver<int>>);
            var observable = default(IAsyncReactiveQbservable<int>);
            var testObserverName = new Uri("reactor://test/observer");
            var subscriptionUri = new Uri("reactor://test/subscription");

            await AssertVirtual<ReactiveClientContext, int>(
                new VirtualTimeAgenda<ReactiveClientContext>
                {
                    { 100L, ctx => { observer = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri); } },
                    { 150L, ctx => { observable = ctx.Empty<int>().StartWith(0, 1, 2, 3, 4); } },
                    { 200L, ctx => observable.SubscribeAsync(observer(testObserverName), subscriptionUri, null, CancellationToken.None) }
                },
                new ObserverState<int>(testObserverName)
                {
                    ObserverMessage.OnNext(201, 0),
                    ObserverMessage.OnNext(202, 1),
                    ObserverMessage.OnNext(203, 2),
                    ObserverMessage.OnNext(204, 3),
                    ObserverMessage.OnNext(205, 4),
                    ObserverMessage.OnCompleted<int>(206)
                });
        }

        [TestMethod]
        public async Task Remoting_Subscribe_HigherArityObservable_ObserverState()
        {
            var uri = new Uri("rx://test/observable");
            var subUri = new Uri("rx://test/sub");
            var testObserverName = new Uri("rx://test/observer");

            await AssertVirtual<ReactiveClientContext, int>(
                async (ctx, _) =>
                {
                    var range = ctx.GetObservable<int, int>(ReactorConstants.Identifiers.Observable.Range.Uri);
                    await ctx.DefineObservableAsync<int, int, int, int>(
                        uri,
                        (x, y, z) => range(x + y - z),
                        null,
                        CancellationToken.None
                    );

                    var o = ctx.GetObservable<int, int, int, int>(uri);
                    var observer = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri)(testObserverName);

                    await o(1, 2, 1).SubscribeAsync(observer, subUri, null, CancellationToken.None);

                    await ctx.UndefineObservableAsync(uri, CancellationToken.None);
                },
                new ObserverState<int>(testObserverName)
                {
                    ObserverMessage.OnNext(0),
                    ObserverMessage.OnNext(1),
                    ObserverMessage.OnCompleted<int>()
                }
            );
        }

        [TestMethod]
        public async Task Remoting_DefineObservableAndSubscribe_ClientTypeToClientType()
        {
            var observerName = new Uri("reactor://test/observer");

            await AssertVirtual<ReactiveClientContext, int>(
                async (ctx, _) =>
                {
                    var uri = new Uri("reactor://test/observable");
                    var subUri = new Uri("reactor://test/subscription");

                    await ctx.DefineObservableAsync<ClientPerson>(
                        uri,
                        ctx.Empty<ClientPerson>().StartWith(new ClientPerson { LastName = "Smith", Age = 30 }),
                        null,
                        CancellationToken.None);

                    var observable = ctx.GetObservable<AltClientPerson>(uri);
                    var observerFactory = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri);
                    var observer = observerFactory(observerName);

                    await observable.Select(p => p.YearsSinceBirth).SubscribeAsync(observer, subUri, null, CancellationToken.None);
                },
                new ObserverState<int>(observerName)
                {
                    ObserverMessage.OnNext(30),
                    ObserverMessage.OnCompleted<int>()
                }
            );
        }

        [TestMethod]
        [Ignore, Description("TODO: Fix bug.")]
        public async Task Remoting_DefineObservableAndSubscribe_ClientTypeToClientTypeWithStructuralArray()
        {
            var observerName = new Uri("reactor://test/observer");

            await AssertVirtual<ReactiveClientContext, int>(
                async (ctx, _) =>
                {
                    var uri = new Uri("reactor://test/observable");
                    var suburi = new Uri("reactor://test/subscription");

                    await ctx.UndefineObservableAsync(uri, CancellationToken.None);
                    await ctx.DefineObservableAsync<ClientFamily>(
                        uri,
                        ctx.Empty<ClientFamily>().StartWith(new ClientFamily { Members = { new ClientPerson { LastName = "Smith", Age = 30 } } }),
                        null,
                        CancellationToken.None);

                    var observable = ctx.GetObservable<ClientFamily>(uri);
                    var observerFactory = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri);
                    var observer = observerFactory(observerName);

                    await observable.Select(p => p.Members[0].Age).SubscribeAsync(observer, suburi, null, CancellationToken.None);
                },
                new ObserverState<int>(observerName)
                {
                    ObserverMessage.OnNext(30),
                    ObserverMessage.OnCompleted<int>()
                }
            );
        }

        [TestMethod]
        public async Task Remoting_Subscribe_ClientTypeToKnownType()
        {
            var observerName = new Uri("reactor://test/observer");

            await AssertVirtual<ReactiveClientContext, int>(
                async (ctx, _) =>
                {
                    var uri = new Uri("reactor://test/observable");
                    var subUri = new Uri("reactor://test/subscription");
                    var personObservable = ctx.GetObservable<ClientPersonObservableParameters, ClientPerson>(ReactorConstants.Identifiers.Observable.Person.Uri);

                    var observerFactory = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri);
                    var observer = observerFactory(observerName);

                    await personObservable(new ClientPersonObservableParameters { Age = 20 }).Take(1).Select(p => p.Age).SubscribeAsync(observer, new Uri("reactor://test/subscription"), null, CancellationToken.None);
                },
                new ObserverState<int>(observerName)
                {
                    ObserverMessage.OnNext(20),
                    ObserverMessage.OnCompleted<int>()
                }
            );
        }

        [TestMethod]
        public async Task Remoting_SubscribeWithEnums_ClientTypeToClientType()
        {
            var observerName = new Uri("reactor://test/observer");

            await AssertVirtual<ReactiveClientContext, int>(
                async (ctx, _) =>
                {
                    var uri = new Uri("reactor://test/observable");
                    var subUri = new Uri("reactor://test/subscription");

                    await ctx.DefineObservableAsync<ClientPerson>(
                        uri,
                        ctx.Empty<ClientPerson>().StartWith(new ClientPerson { LastName = "Smith", Age = 30, Occupation = ClientOccupation.ChiefExecutiveOfficer }),
                        null,
                        CancellationToken.None);

                    var observable = ctx.GetObservable<AltClientPerson>(uri);
                    var observerFactory = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri);
                    var observer = observerFactory(observerName);

                    await observable
                        .Where(p => p.Occupation == AltClientOccupation.ChiefExecutiveOfficer)
                        .Select(p => p.YearsSinceBirth)
                        .SubscribeAsync(observer, new Uri("reactor://test/subscription"), null, CancellationToken.None);
                },
                new ObserverState<int>(observerName)
                {
                    ObserverMessage.OnNext(30),
                    ObserverMessage.OnCompleted<int>()
                }
            );
        }

        [TestMethod]
        public async Task Remoting_SubscribeWithEnums_ClientTypeToKnownType()
        {
            var observerName = new Uri("reactor://test/observer");

            await AssertVirtual<ReactiveClientContext, int>(
                async (ctx, _) =>
                {
                    var uri = new Uri("reactor://test/observable");
                    var subUri = new Uri("reactor://test/subscription");
                    var personObservable = ctx.GetObservable<ClientPersonObservableParameters, ClientPerson>(ReactorConstants.Identifiers.Observable.Person.Uri);

                    var observerFactory = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri);
                    var observer = observerFactory(observerName);

                    await personObservable(new ClientPersonObservableParameters { Age = 20, Occupation = ClientOccupation.Unemployed })
                        .Take(1)
                        .Where(p => p.Occupation == ClientOccupation.Unemployed).Select(p => p.Age)
                        .SubscribeAsync(observer, new Uri("reactor://test/subscription"), null, CancellationToken.None);
                },
                new ObserverState<int>(observerName)
                {
                    ObserverMessage.OnNext(20),
                    ObserverMessage.OnCompleted<int>()
                }
            );
        }

        [TestMethod]
        public async Task Remoting_SubscribeDo_AsyncToSyncMethodCalls()
        {
            var testObserverName = new Uri("reactor://test/observer");

            await AssertVirtual<ReactiveClientContext, int>(
                async (ctx, _) =>
                {
                    var uri = new Uri("reactor://test/subscription");

                    var xs = ctx.GetObservable<int, int>(ReactorConstants.Identifiers.Observable.Range.Uri)(3);
                    var observer = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri)(testObserverName);
                    var doObserver = ctx.GetObserver<int>(new Uri(Constants.Observer.Nop));

                    await xs
                        .Where(x => x > 0)
                        .Do(x => doObserver.OnNextAsync(x, CancellationToken.None))
                        .SubscribeAsync(observer, uri, null, CancellationToken.None);
                },
                new ObserverState<int>(testObserverName)
                {
                    ObserverMessage.OnNext(1),
                    ObserverMessage.OnNext(2),
                    ObserverMessage.OnCompleted<int>()
                }
            );
        }

        [TestMethod]
        public async Task Remoting_DistinctUntilChanged_KnownType()
        {
            var controlObserverName = new Uri("reactor://control/observer");
            var testObserverName = new Uri("reactor://test/observer");
            await AssertVirtual<ReactiveClientContext, int>(
                async (ctx, _) =>
                {
                    var controlUri = new Uri("reactor://control/subscription");
                    var testUri = new Uri("reactor://test/subscription");
                    var io = ctx.GetObservable<ClientPersonObservableParameters, ClientPerson>(ReactorConstants.Identifiers.Observable.Person.Uri);
                    var iv = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri);
                    await io(new ClientPersonObservableParameters { Age = 50 }).Take(5).Select(p => p.Age).SubscribeAsync(iv(controlObserverName), controlUri, null, CancellationToken.None);
                    await io(new ClientPersonObservableParameters { Age = 50 }).Take(5).DistinctUntilChanged().Select(p => p.Age).SubscribeAsync(iv(testObserverName), testUri, null, CancellationToken.None);
                },
                new ObserverState<int>(controlObserverName)
                {
                    ObserverMessage.OnNext(50),
                    ObserverMessage.OnNext(50),
                    ObserverMessage.OnNext(50),
                    ObserverMessage.OnNext(50),
                    ObserverMessage.OnNext(50),
                    ObserverMessage.OnCompleted<int>()
                },
                new ObserverState<int>(testObserverName)
                {
                    ObserverMessage.OnNext(50),
                    ObserverMessage.OnCompleted<int>()
                }
            );
        }

        [TestMethod]
        public void Remoting_VirtualTimeTest_Sample()
        {
            using var client = CreateTestClient();

            var xs = client.CreateHotObservable(
                ObserverMessage.OnNext(100, 1),
                ObserverMessage.OnNext(150, 2),
                ObserverMessage.OnNext(200, 3),
                ObserverMessage.OnNext(250, 4));

            var res = client.Start(() => xs.Select(x => x * x), 50, 125, 225);

            res.Messages.AssertEqual(
                ObserverMessage.OnNext(150, 4),
                ObserverMessage.OnNext(200, 9));

            xs.Subscriptions.AssertEqual(
                ReactiveTest.Subscribe(125, 225));
        }

        [TestMethod]
        public async Task Remoting_ContextSwitchOperator_StateChanged_Empty()
        {
            var streamId = new Uri("reactor://test/stream");
            var subscriptionId = new Uri("reactor://test/subscription");
            var testObserverName = new Uri("reactor://test/observer");

            using var client = CreateTestClient();

            var sf = client.Context.GetStreamFactory<int, int>(PlatformConstants.Identifiers.Observable.FireHose.Uri);
            var stream = await sf.CreateAsync(streamId, null, CancellationToken.None);
            var observer = client.Context.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri)(testObserverName);

            client.Scheduler.ScheduleAbsolute(100, () => stream.SubscribeAsync(observer, subscriptionId, null, CancellationToken.None));
            // Cause the ItemProcessor to execute once...
            client.Scheduler.ScheduleAbsolute(110, () => stream.OnNextAsync(1, CancellationToken.None));
            // Full checkpoint and trigger OnStateSaved()...
            client.Scheduler.ScheduleAbsolute(110, () => client.Platform.QueryEvaluators.First().Checkpoint());
            // Cause item to be queued but not processed...
            client.Scheduler.ScheduleAbsolute(110, () => stream.OnNextAsync(2, CancellationToken.None));
            // Cause additional item to be queued but not processed...
            client.Scheduler.ScheduleAbsolute(110, () => stream.OnNextAsync(3, CancellationToken.None));
            // Differential checkpoint and reload from state...
            client.Scheduler.ScheduleAbsolute(110, () =>
            {
                var qe = client.Platform.QueryEvaluators.First();
                qe.Checkpoint();
                qe.Unload();
                qe.Recover();
            });

            client.Scheduler.Start();

            var observerState = new ObserverState<int>(testObserverName)
                {
                    ObserverMessage.OnNext(1),
                    ObserverMessage.OnNext(2),
                    ObserverMessage.OnNext(3),
                    ObserverMessage.OnNext(2),
                    ObserverMessage.OnNext(3),
                };

            AssertObserverState(client.Platform, observerState);
        }

        [TestMethod]
        public async Task Remoting_ContextSwitchOperator_StateChanged_ItemsProcessed()
        {
            var streamId = new Uri("reactor://test/stream");
            var subscriptionId = new Uri("reactor://test/subscription");
            var testObserverName = new Uri("reactor://test/observer");

            using var client = CreateTestClient();

            var ctx = client.Context;
            var scheduler = (ITestScheduler)client.Platform.QueryEvaluators.First().Scheduler;

            var sf = ctx.GetStreamFactory<int, int>(PlatformConstants.Identifiers.Observable.FireHose.Uri);
            var stream = await sf.CreateAsync(streamId, null, CancellationToken.None);
            var observer = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri)(testObserverName);

            scheduler.ScheduleAbsolute(100, () => stream.SubscribeAsync(observer, subscriptionId, null, CancellationToken.None));

            // Cause the ItemProcessor to execute once...
            scheduler.ScheduleAbsolute(110, () => stream.OnNextAsync(1, CancellationToken.None));
            // Cause item to be queued but not processed...
            scheduler.ScheduleAbsolute(110, () => stream.OnNextAsync(2, CancellationToken.None));
            // Full checkpoint and trigger OnStateSaved()...
            scheduler.ScheduleAbsolute(110, () => client.Platform.QueryEvaluators.First().Checkpoint());
            // Cause additional item to be queued but not processed...
            scheduler.ScheduleAbsolute(110, () => stream.OnNextAsync(3, CancellationToken.None));
            // Differential checkpoint and reload from state...
            scheduler.ScheduleAbsolute(112, () =>
            {
                var qe = client.Platform.QueryEvaluators.First();
                qe.Checkpoint();
                qe.Unload();
                qe.Recover();
            });


            scheduler.Start();

            var observerState = new ObserverState<int>(testObserverName)
                {
                    ObserverMessage.OnNext(1),
                    ObserverMessage.OnNext(2),
                    ObserverMessage.OnNext(3),
                };

            AssertObserverState(client.Platform, observerState);
        }

        [TestMethod]
        public async Task Remoting_ContextSwitchOperator_StateChanged_NoneProcessed()
        {
            var streamId = new Uri("reactor://test/stream");
            var subscriptionId = new Uri("reactor://test/subscription");
            var testObserverName = new Uri("reactor://test/observer");

            using var client = CreateTestClient();

            var ctx = client.Context;
            var scheduler = (ITestScheduler)client.Platform.QueryEvaluators.First().Scheduler;

            var sf = ctx.GetStreamFactory<int, int>(PlatformConstants.Identifiers.Observable.FireHose.Uri);
            var stream = await sf.CreateAsync(streamId, null, CancellationToken.None);
            var observer = ctx.GetObserver<Uri, int>(TestConstants.Test.TestObserver.Uri)(testObserverName);

            scheduler.ScheduleAbsolute(100, () => stream.SubscribeAsync(observer, subscriptionId, null, CancellationToken.None));

            // Cause the ItemProcessor to execute once...
            scheduler.ScheduleAbsolute(110, () => stream.OnNextAsync(1, CancellationToken.None));
            // Cause item to be queued but not processed...
            scheduler.ScheduleAbsolute(110, () => stream.OnNextAsync(2, CancellationToken.None));
            // Full checkpoint and trigger OnStateSaved()...
            scheduler.ScheduleAbsolute(110, () => client.Platform.QueryEvaluators.First().Checkpoint());
            // Cause additional item to be queued but not processed...
            scheduler.ScheduleAbsolute(110, () => stream.OnNextAsync(3, CancellationToken.None));
            // Differential checkpoint and reload from state...
            scheduler.ScheduleAbsolute(110, () =>
            {
                var qe = client.Platform.QueryEvaluators.First();
                qe.Checkpoint();
                qe.Unload();
                qe.Recover();
            });

            scheduler.Start();

            var observerState = new ObserverState<int>(testObserverName)
                {
                    ObserverMessage.OnNext(1),
                    ObserverMessage.OnNext(2),
                    ObserverMessage.OnNext(3),
                    ObserverMessage.OnNext(2),
                    ObserverMessage.OnNext(3),
                };

            AssertObserverState(client.Platform, observerState);
        }
    }
}
