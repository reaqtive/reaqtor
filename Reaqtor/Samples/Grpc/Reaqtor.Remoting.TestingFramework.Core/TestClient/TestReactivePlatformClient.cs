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
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.Hosting.Shared.Tools;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    internal class TestReactivePlatformClient : ReactivePlatformClient, ITestReactivePlatformClient
    {
        private const string UriBase = "reactor://test/{0}";

        private readonly List<Tuple<Uri, ReactiveEntityType>> _cleanupEntities;
        private readonly bool _cleanupEnvironment;
        private int _uriCounter;

        public TestReactivePlatformClient(IReactivePlatform platform, bool cleanupEnvironment = false)
            : base(platform)
        {
            _cleanupEntities = [];
            _cleanupEnvironment = cleanupEnvironment;
        }

        public ITestScheduler Scheduler => (ITestScheduler)Platform.QueryEvaluators.First().Scheduler;

        public ITestObserver<T> Start<T>(
            Func<IAsyncReactiveQbservable<T>> create,
            long created,
            long subscribed,
            long disposed)
        {
            ArgumentNullException.ThrowIfNull(create);

            ArgumentOutOfRangeException.ThrowIfGreaterThan(created, subscribed);

            ArgumentOutOfRangeException.ThrowIfGreaterThan(subscribed, disposed);

            var source = default(IAsyncReactiveQbservable<T>);
            var subscription = default(IAsyncReactiveQubscription);
            var observer = CreateObserver<T>();

            var agenda = new VirtualTimeAgenda<ReactiveClientContext>
            {
                { created, _ => source = Augment<T>(create()) },
                { subscribed, _ => subscription = source.SubscribeAsync(observer, NextUri(), null, CancellationToken.None).Result },
                // NB: on net10.0 IAsyncReactiveSubscription derives from the BCL System.IAsyncDisposable, so the
                //     DisposeAsync(CancellationToken) extension returns a ValueTask (not the archived Task). Bridge
                //     via .AsTask() before the blocking Wait(), mirroring AsyncDisposableExtensions.Disposable.Dispose.
                { disposed, _ => subscription.DisposeAsync(CancellationToken.None).AsTask().Wait() },
            };

            Helpers.DoScheduling(agenda)(Context, Scheduler).Wait();
            Scheduler.Start();

            return observer;
        }

        private static IAsyncReactiveQbservable<T> Augment<T>(IAsyncReactiveQbservable<T> source)
        {
            return source.StatefulAugmentation();
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0060 // Remove unused parameter (used for type inference)
#pragma warning disable CA1801 // Review unused parameters
        public ITestableQbservable<T> CreateHotObservableWitnessed<T>(T witness, params Recorded<INotification<T>>[] messages)
        {
            return CreateHotObservable(messages);
        }
#pragma warning restore CA1801 // Review unused parameters
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0079 // Remove unnecessary suppression

        public ITestableQbservable<T> CreateHotObservable<T>(params Recorded<INotification<T>>[] messages)
        {
            return CreateHotObservable<T>(NextUri(), messages);
        }

        private ITestableQbservable<T> CreateHotObservable<T>(Uri id, params Recorded<INotification<T>>[] messages)
        {
            var hotObservable = Context.GetObservable<Uri, T>(Constants.Test.HotTimelineObservable.Uri)(id);
            var testQE = Platform.QueryEvaluators.First().GetInstance<TestQueryEvaluatorServiceConnection>();
            var messageList = messages.ToList();
            testQE.EventTimelines.TryAdd(id.ToCanonicalString(), [.. Helpers.SerializeObserverMessages<T>(messageList)]);
            return new TestableQbservable<T>(id, messageList, hotObservable, this);
        }

        public ITestableQbservable<T> CreateColdObservable<T>(params Recorded<INotification<T>>[] messages)
        {
            return CreateColdObservable<T>(NextUri(), messages);
        }

        private ITestableQbservable<T> CreateColdObservable<T>(Uri id, params Recorded<INotification<T>>[] messages)
        {
            var coldObservable = Context.GetObservable<Uri, T>(Constants.Test.ColdTimelineObservable.Uri)(id);
            var testQE = Platform.QueryEvaluators.First().GetInstance<TestQueryEvaluatorServiceConnection>();
            testQE.EventTimelines.TryAdd(id.ToCanonicalString(), [.. Helpers.SerializeObserverMessages<T>([.. messages])]);
            return new TestableQbservable<T>(id, messages, coldObservable, this);
        }

        private TestableQbserver<T> CreateObserver<T>(Uri uri)
        {
            var qbserver = Context.GetObserver<Uri, T>(Constants.Test.TestObserver.Uri)(uri);
            return new TestableQbserver<T>(uri, qbserver, Platform);
        }

        private TestableQbserver<T> CreateObserver<T>()
        {
            return CreateObserver<T>(NextUri());
        }

        #region Platform Cleanup

        public void CleanupEntity(Uri uri, ReactiveEntityType entityType)
        {
            var entity = Tuple.Create(uri, entityType);
            if (!_cleanupEntities.Contains(entity))
            {
                _cleanupEntities.Add(entity);
            }
        }

        private void CleanupQueryEvaluator()
        {
            Platform.QueryEvaluators.First().GetInstance<TestQueryEvaluatorServiceConnection>().TestObservers.Clear();
            Platform.QueryEvaluators.First().GetInstance<TestQueryEvaluatorServiceConnection>().TestSubscriptions.Clear();
        }

        private void CleanupEnvironment()
        {
            Platform.Environment.StateStoreService.GetInstance<IReactiveStateStoreConnection>().Clear();

            var ctx = Platform.CreateClient().Context;

            // NB: on net10.0 DisposeAsync(CancellationToken) returns a ValueTask (the BCL System.IAsyncDisposable
            //     extension), so bridge each to Task via .AsTask() before Task.WaitAll. See remark in Start above.
            Task.WaitAll([.. _cleanupEntities
                .Where(t => t.Item2 == ReactiveEntityType.Subscription)
                .Select(t => ctx.GetSubscription(t.Item1).DisposeAsync(CancellationToken.None).AsTask())]);

            Platform.Environment.KeyValueStoreService.GetInstance<ITransactionalKeyValueStoreConnection>().Clear();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_cleanupEnvironment)
            {
                CleanupQueryEvaluator();
                CleanupEnvironment();
                Platform.Dispose();
            }
        }

        #endregion

        #region Helpers

        private Uri NextUri()
        {
#pragma warning disable CA1863 // Use CompositeFormat: cold path (test-client URI generation). PR #155 measured all format sites as cold and rejected CompositeFormat adoption.
            return new Uri(string.Format(CultureInfo.InvariantCulture, UriBase, ++_uriCounter));
#pragma warning restore CA1863
        }

        #endregion
    }
}
