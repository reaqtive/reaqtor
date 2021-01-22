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
            _cleanupEntities = new List<Tuple<Uri, ReactiveEntityType>>();
            _cleanupEnvironment = cleanupEnvironment;
        }

        public ITestScheduler Scheduler => (ITestScheduler)Platform.QueryEvaluators.First().Scheduler;

        public ITestObserver<T> Start<T>(
            Func<IAsyncReactiveQbservable<T>> create,
            long created,
            long subscribed,
            long disposed)
        {
            if (create == null)
            {
                throw new ArgumentNullException(nameof(create));
            }

            if (created > subscribed)
            {
                throw new ArgumentOutOfRangeException(nameof(created));
            }

            if (subscribed > disposed)
            {
                throw new ArgumentOutOfRangeException(nameof(subscribed));
            }

            var source = default(IAsyncReactiveQbservable<T>);
            var subscription = default(IAsyncReactiveQubscription);
            var observer = CreateObserver<T>();

            var agenda = new VirtualTimeAgenda<ReactiveClientContext>
            {
                { created, _ => source = Augment<T>(create()) },
                { subscribed, _ => subscription = source.SubscribeAsync(observer, NextUri(), null, CancellationToken.None).Result },
                { disposed, _ => subscription.DisposeAsync(CancellationToken.None).Wait() },
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
        public ITestableQbservable<T> CreateHotObservableWitnessed<T>(T witness, params Recorded<INotification<T>>[] messages)
        {
            return CreateHotObservable(messages);
        }
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
            testQE.EventTimelines.TryAdd(id.ToCanonicalString(), Helpers.SerializeObserverMessages<T>(messageList).ToList());
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
            testQE.EventTimelines.TryAdd(id.ToCanonicalString(), Helpers.SerializeObserverMessages<T>(messages.ToList()).ToList());
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

            Task.WaitAll(_cleanupEntities
                .Where(t => t.Item2 == ReactiveEntityType.Subscription)
                .Select(t => ctx.GetSubscription(t.Item1).DisposeAsync(CancellationToken.None)).ToArray());

            Platform.Environment.KeyValueStoreService.GetInstance<ITransactionalKeyValueStoreConnection>().Clear();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            CleanupQueryEvaluator();
            CleanupEnvironment();
            Platform.Dispose();
        }

        #endregion

        #region Helpers

        private Uri NextUri()
        {
            return new Uri(string.Format(CultureInfo.InvariantCulture, UriBase, ++_uriCounter));
        }

        #endregion
    }
}
