// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;
using Reaqtive.TestingFramework;

using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;
using Reaqtor.Remoting.Reactor.Client;
using Reaqtor.Remoting.Reactor.DomainFeeds;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtor.Remoting.TestingFramework
{
    #region Platform Configuration
#if APPDOMAINPLATFORM
    using TestReactiveEnvironment = AppDomainReactiveEnvironment;
    using TestReactivePlatform = AppDomainTestPlatform;
#else
    using TestReactiveEnvironment = InMemoryReactiveEnvironment;
    using TestReactivePlatform = InMemoryTestPlatform;
#endif
    #endregion

    public abstract class RemotingTestBase
    {
        #region Environment

        private static readonly Lazy<TestReactiveEnvironment> s_environment = new(() =>
        {
            var env = new TestReactiveEnvironment();
            env.StartAsync(CancellationToken.None).Wait();
            var platform = new TestReactivePlatform(env);
            platform.StartAsync(CancellationToken.None).Wait();
            new ReactivePlatformDeployer(platform, new TestDeployable(), new Reactor.Deployable(), new DomainFeedsDeployable()).Deploy();
            platform.StopAsync(CancellationToken.None).Wait();
            return env;
        });

        public static IReactiveEnvironment Environment => s_environment.Value;

        #endregion

        #region Assertions

        #region Assertion Signatures

        protected static async Task RunAsync<TContext>(Func<TContext, Task> task)
            where TContext : ReactiveClientContext
        {
            await RunTask(task, (_, __) => { });
        }

        protected static async Task RunAsync<TContext, TObserver>(Func<TContext, Task> task, params ObserverState<TObserver>[] observers)
            where TContext : ReactiveClientContext
        {
            await RunTask(task, (client, platform) =>
            {
                AssertObserverState(platform, observers);
            });
        }

        protected static async Task RunAsync<TContext>(Func<TContext, Task> task, params MetadataState[] metadataState)
            where TContext : ReactiveClientContext
        {
            await RunTask(task, (client, platform) =>
            {
                AssertMetadataState(client.MetadataProxy, metadataState);
            });
        }

        #endregion

        #region Virtual Time Assertions

        #region Lambda Assertions

        protected static async Task AssertVirtual<TContext>(Func<TContext, ITestScheduler, Task> task)
            where TContext : ReactiveClientContext
        {
            await RunVirtualTask(task, (_, __) => { });
        }

        protected static async Task AssertVirtual<TContext, TObserver>(Func<TContext, ITestScheduler, Task> task, params ObserverState<TObserver>[] observers)
            where TContext : ReactiveClientContext
        {
            await RunVirtualTask(task, (client, platform) =>
            {
                AssertObserverState(platform, observers);
            });
        }

        protected static async Task AssertVirtual<TContext>(Func<TContext, ITestScheduler, Task> task, params MetadataState[] metadataState)
            where TContext : ReactiveClientContext
        {
            await RunVirtualTask(task, (client, platform) =>
            {
                AssertMetadataState(client.MetadataProxy, metadataState);
            });
        }

        #endregion

        #region Schedule Assertions

        protected static async Task AssertVirtual<TContext>(VirtualTimeAgenda<TContext> schedule)
            where TContext : ReactiveClientContext
        {
            await AssertVirtual<TContext>(Helpers.DoScheduling<TContext>(schedule));
        }

        protected static async Task AssertVirtual<TContext, TObserver>(VirtualTimeAgenda<TContext> schedule, params ObserverState<TObserver>[] observers)
            where TContext : ReactiveClientContext
        {
            await AssertVirtual<TContext, TObserver>(Helpers.DoScheduling<TContext>(schedule), observers);
        }

        protected static async Task AssertVirtual<TContext>(VirtualTimeAgenda<TContext> schedule, params MetadataState[] metadataState)
            where TContext : ReactiveClientContext
        {
            await AssertVirtual<TContext>(Helpers.DoScheduling<TContext>(schedule), metadataState);
        }

        #endregion

        #endregion

        #region Assert Observer State

        public static void AssertObserverState<TObserver>(IReactivePlatform platform, params ObserverState<TObserver>[] observers)
        {
            foreach (var observer in observers)
            {
                var testQE = platform.QueryEvaluators.First().GetInstance<TestQueryEvaluatorServiceConnection>();
                testQE.TestObservers.TryGetValue(observer.Uri.ToCanonicalString(), out var testObserver);
                var deserializedMessages = Helpers.DeserializeObserverMessages<TObserver>(testObserver.Messages);
                Assert.IsTrue(observer.SequenceEqual(deserializedMessages, RecordedNotificationEqualityComparer<TObserver>.Default));
            }
        }

        #endregion

        #region Assert Metadata State

        private static void AssertMetadataState(IReactiveMetadataProxy metadata, IEnumerable<MetadataState> states)
        {
            foreach (var state in states)
            {
                switch (state.Type)
                {
                    case MetadataEntityType.Observable:
                        AssertObservableDefined(metadata, state.Uri, state.Expression);
                        break;
                    case MetadataEntityType.Observer:
                        AssertObserverDefined(metadata, state.Uri, state.Expression);
                        break;
                    case MetadataEntityType.StreamFactory:
                        AssertStreamFactoryDefined(metadata, state.Uri, state.Expression);
                        break;
                    case MetadataEntityType.SubscriptionFactory:
                        AssertSubscriptionFactoryDefined(metadata, state.Uri, state.Expression);
                        break;
                }
            }
        }

        private static void AssertObservableDefined(IReactiveMetadataProxy metadata, Uri uri, Expression expected)
        {
            Assert.IsTrue(metadata.Observables.TryGetValue(uri, out var observable));
            AssertExpressionEquals(expected, observable.Expression);
        }

        private static void AssertObserverDefined(IReactiveMetadataProxy metadata, Uri uri, Expression expected)
        {
            Assert.IsTrue(metadata.Observers.TryGetValue(uri, out var observer));
            AssertExpressionEquals(expected, observer.Expression);
        }

        private static void AssertStreamFactoryDefined(IReactiveMetadataProxy metadata, Uri uri, Expression expected)
        {
            Assert.IsTrue(metadata.StreamFactories.TryGetValue(uri, out var streamFactory));
            AssertExpressionEquals(expected, streamFactory.Expression);
        }

        private static void AssertSubscriptionFactoryDefined(IReactiveMetadataProxy metadata, Uri uri, Expression expected)
        {
            Assert.IsTrue(metadata.SubscriptionFactories.TryGetValue(uri, out var subscriptionFactory));
            AssertExpressionEquals(expected, subscriptionFactory.Expression);
        }

        private static void AssertExpressionEquals(Expression expected, Expression actual)
        {
            var comparer = new ExpressionEqualityComparer(() => new Comparator(new StructuralTypeEqualityComparator()));
            Assert.IsTrue(comparer.Equals(expected, actual), string.Format("Expected expression: '{0}'. Actual expression: '{1}'", expected.ToTraceString(), actual.ToTraceString()));
        }

        #endregion

        #region Test Runner & Other Methods

        private static async Task RunVirtualTask<TContext>(Func<TContext, ITestScheduler, Task> task, Action<IReactivePlatformClient, TestReactivePlatform> callback)
            where TContext : ReactiveClientContext
        {
            using var environment = new TestReactiveEnvironment();
            using var platform = new TestReactivePlatform(environment);

            await environment.StartAsync(CancellationToken.None);

            platform.Configuration.SchedulerType = SchedulerType.Test;

            await platform.StartAsync(CancellationToken.None);

            new ReactivePlatformDeployer(platform, new TestDeployable(), new Reactor.Deployable(), new DomainFeedsDeployable()).Deploy();

            var scheduler = (ITestScheduler)platform.QueryEvaluators.First().Scheduler;
            var client = new AgentsClient(platform);

            Debug.Assert(client.Context is TContext);

            await task((TContext)client.Context, scheduler);

            scheduler.Start();

            callback(client, platform);
        }

        private static async Task RunTask<TContext>(Func<TContext, Task> task, Action<IReactivePlatformClient, TestReactivePlatform> callback)
            where TContext : ReactiveClientContext
        {
            using var environment = new TestReactiveEnvironment();
            using var platform = new TestReactivePlatform(environment);

            await environment.StartAsync(CancellationToken.None);
            await platform.StartAsync(CancellationToken.None);

            new ReactivePlatformDeployer(platform, new TestDeployable(), new Reactor.Deployable(), new DomainFeedsDeployable()).Deploy();

            var client = new AgentsClient(platform);

            Debug.Assert(client.Context is TContext);

            await task((TContext)client.Context);

            callback(client, platform);
        }

        #endregion

        #endregion

        #region Test Clients

        private static readonly Action<IReactivePlatformConfiguration> DefaultConfiguration = c => c.SchedulerType = SchedulerType.Test;

        public ITestReactivePlatformClient CreateTestClient()
        {
            var platform = new InMemoryTestPlatform();
            DefaultConfiguration(platform.Configuration);
            return CreateTestClient(platform, sharedEnvironment: false);
        }

        public ITestReactivePlatformClient CreateTestClient(IReactiveEnvironment environment, Action<IReactivePlatformConfiguration> configure)
        {
            var platform = new InMemoryTestPlatform(environment);
            configure(platform.Configuration);
            return CreateTestClient(platform, sharedEnvironment: true);
        }

        private ITestReactivePlatformClient CreateTestClient(IReactivePlatform platform, bool sharedEnvironment)
        {
            platform.StartAsync(CancellationToken.None).Wait();
            return CreateTestClientCore(platform, sharedEnvironment);
        }

        protected virtual ITestReactivePlatformClient CreateTestClientCore(IReactivePlatform platform, bool sharedEnvironment)
        {
            return new TestReactivePlatformClient(platform, sharedEnvironment);
        }

        #endregion

        #region Helper Classes

        private sealed class Comparator : ExpressionEqualityComparator
        {
            public Comparator(StructuralTypeEqualityComparator typeComparer)
                : base(typeComparer, typeComparer.MemberComparer, EqualityComparer<object>.Default, EqualityComparer<CallSiteBinder>.Default)
            {
            }

            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }

        #endregion
    }
}
