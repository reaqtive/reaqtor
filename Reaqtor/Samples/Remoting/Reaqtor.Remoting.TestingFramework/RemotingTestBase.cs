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
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.TestingFramework;

using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;
using Reaqtor.Remoting.Reactor.Client;
using Reaqtor.Remoting.Reactor.DomainFeeds;

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
            Debug.Assert(task != null);

            await RunTask(task, (_, __) => { }).ConfigureAwait(false);
        }

        protected static async Task RunAsync<TContext, TObserver>(Func<TContext, Task> task, params ObserverState<TObserver>[] observers)
            where TContext : ReactiveClientContext
        {
            Debug.Assert(task != null);

            await RunTask(task, (client, platform) =>
            {
                AssertObserverState(platform, observers);
            }).ConfigureAwait(false);
        }

        protected static async Task RunAsync<TContext>(Func<TContext, Task> task, params MetadataState[] metadataState)
            where TContext : ReactiveClientContext
        {
            Debug.Assert(task != null);

            await RunTask(task, (client, platform) =>
            {
                AssertMetadataState(client.MetadataProxy, metadataState);
            }).ConfigureAwait(false);
        }

        #endregion

        #region Virtual Time Assertions

        #region Lambda Assertions

        protected static async Task AssertVirtual<TContext>(Func<TContext, ITestScheduler, Task> task)
            where TContext : ReactiveClientContext
        {
            Debug.Assert(task != null);

            await RunVirtualTask(task, (_, __) => { }).ConfigureAwait(false);
        }

        protected static async Task AssertVirtual<TContext, TObserver>(Func<TContext, ITestScheduler, Task> task, params ObserverState<TObserver>[] observers)
            where TContext : ReactiveClientContext
        {
            Debug.Assert(task != null);

            await RunVirtualTask(task, (client, platform) =>
            {
                AssertObserverState(platform, observers);
            }).ConfigureAwait(false);
        }

        protected static async Task AssertVirtual<TContext>(Func<TContext, ITestScheduler, Task> task, params MetadataState[] metadataState)
            where TContext : ReactiveClientContext
        {
            Debug.Assert(task != null);

            await RunVirtualTask(task, (client, platform) =>
            {
                AssertMetadataState(client.MetadataProxy, metadataState);
            }).ConfigureAwait(false);
        }

        #endregion

        #region Schedule Assertions

        protected static async Task AssertVirtual<TContext>(VirtualTimeAgenda<TContext> schedule)
            where TContext : ReactiveClientContext
        {
            Debug.Assert(schedule != null);

            await AssertVirtual<TContext>(Helpers.DoScheduling<TContext>(schedule)).ConfigureAwait(false);
        }

        protected static async Task AssertVirtual<TContext, TObserver>(VirtualTimeAgenda<TContext> schedule, params ObserverState<TObserver>[] observers)
            where TContext : ReactiveClientContext
        {
            Debug.Assert(schedule != null);

            await AssertVirtual<TContext, TObserver>(Helpers.DoScheduling<TContext>(schedule), observers).ConfigureAwait(false);
        }

        protected static async Task AssertVirtual<TContext>(VirtualTimeAgenda<TContext> schedule, params MetadataState[] metadataState)
            where TContext : ReactiveClientContext
        {
            Debug.Assert(schedule != null);

            await AssertVirtual<TContext>(Helpers.DoScheduling<TContext>(schedule), metadataState).ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region Assert Observer State

        public static void AssertObserverState<TObserver>(IReactivePlatform platform, params ObserverState<TObserver>[] observers)
        {
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));
            if (observers == null)
                throw new ArgumentNullException(nameof(observers));

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
            Assert.IsTrue(comparer.Equals(expected, actual), string.Format(CultureInfo.InvariantCulture, "Expected expression: '{0}'. Actual expression: '{1}'", expected.ToTraceString(), actual.ToTraceString()));
        }

        #endregion

        #region Test Runner & Other Methods

        private static async Task RunVirtualTask<TContext>(Func<TContext, ITestScheduler, Task> task, Action<IReactivePlatformClient, TestReactivePlatform> callback)
            where TContext : ReactiveClientContext
        {
            using var environment = new TestReactiveEnvironment();
            using var platform = new TestReactivePlatform(environment);

            await environment.StartAsync(CancellationToken.None).ConfigureAwait(false);

            platform.Configuration.SchedulerType = SchedulerType.Test;

            await platform.StartAsync(CancellationToken.None).ConfigureAwait(false);

            new ReactivePlatformDeployer(platform, new TestDeployable(), new Reactor.Deployable(), new DomainFeedsDeployable()).Deploy();

            var scheduler = (ITestScheduler)platform.QueryEvaluators.First().Scheduler;
            var client = new AgentsClient(platform);

            Debug.Assert(client.Context is TContext);

            await task((TContext)client.Context, scheduler).ConfigureAwait(false);

            scheduler.Start();

            callback(client, platform);
        }

        private static async Task RunTask<TContext>(Func<TContext, Task> task, Action<IReactivePlatformClient, TestReactivePlatform> callback)
            where TContext : ReactiveClientContext
        {
            using var environment = new TestReactiveEnvironment();
            using var platform = new TestReactivePlatform(environment);

            await environment.StartAsync(CancellationToken.None).ConfigureAwait(false);
            await platform.StartAsync(CancellationToken.None).ConfigureAwait(false);

            new ReactivePlatformDeployer(platform, new TestDeployable(), new Reactor.Deployable(), new DomainFeedsDeployable()).Deploy();

            var client = new AgentsClient(platform);

            Debug.Assert(client.Context is TContext);

            await task((TContext)client.Context).ConfigureAwait(false);

            callback(client, platform);
        }

        #endregion

        #endregion

        #region Test Clients

        private static readonly Action<IReactivePlatformConfiguration> DefaultConfiguration = c => c.SchedulerType = SchedulerType.Test;

#if !NET472_OR_GREATER
#pragma warning disable CA2000 // Dispose objects before losing scope. (Client manages lifetime of the platform.)
#endif

        public ITestReactivePlatformClient CreateTestClient()
        {
            var platform = new InMemoryTestPlatform();
            DefaultConfiguration(platform.Configuration);
            return CreateTestClient(platform, sharedEnvironment: false);
        }

        public ITestReactivePlatformClient CreateTestClient(IReactiveEnvironment environment, Action<IReactivePlatformConfiguration> configure)
        {
            if (environment == null)
                throw new ArgumentNullException(nameof(environment));
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            var platform = new InMemoryTestPlatform(environment);
            configure(platform.Configuration);
            return CreateTestClient(platform, sharedEnvironment: true);
        }

#if !NET472_OR_GREATER
#pragma warning restore CA2000
#endif

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
