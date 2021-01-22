// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Reactive metadata service with three cache levels.
    /// 1. Immutable resources are kept locally across all uses of the cache.
    /// 2. Storage local to the current call context is consulted.
    /// 3. External metadata is queried for cache misses. The result is stored in call context local storage, or in the local cache depending on the resource's properties.
    /// </summary>
    internal class ReactiveMetadataCache : IReactiveMetadataCache
    {
        /// <summary>
        /// Dictionary providing access to observable definition metadata.
        /// </summary>
        private readonly LeveledCacheQueryableDictionary<IReactiveObservableDefinition> _observables;

        /// <summary>
        /// Dictionary providing access to observer definition metadata.
        /// </summary>
        private readonly LeveledCacheQueryableDictionary<IReactiveObserverDefinition> _observers;

        /// <summary>
        /// Dictionary providing access to stream factory definition metadata.
        /// </summary>
        private readonly LeveledCacheQueryableDictionary<IReactiveStreamFactoryDefinition> _streamFactories;

        /// <summary>
        /// Dictionary providing access to subscription factory definition metadata.
        /// </summary>
        private readonly LeveledCacheQueryableDictionary<IReactiveSubscriptionFactoryDefinition> _subscriptionFactories;

        /// <summary>
        /// Dictionary providing access to subscription metadata.
        /// </summary>
        private readonly LeveledCacheQueryableDictionary<IReactiveSubscriptionProcess> _subscriptions;

        /// <summary>
        /// Dictionary providing access to stream metadata.
        /// </summary>
        private readonly LeveledCacheQueryableDictionary<IReactiveStreamProcess> _streams;

        /// <summary>
        /// Creates a new metadata cache using the specified external and local metadata services.
        /// </summary>
        /// <param name="external">External metadata service to fall back to for lookups.</param>
        /// <param name="local">Local metadata service used for caching of immutable resources that can be shared safely across users of the metadata service.</param>
        public ReactiveMetadataCache(IReactiveMetadata external, IReactiveMetadataInMemory local)
        {
            _observables = new LeveledCacheQueryableDictionary<IReactiveObservableDefinition>(external.Observables, local.Observables);
            _observers = new LeveledCacheQueryableDictionary<IReactiveObserverDefinition>(external.Observers, local.Observers);
            _streamFactories = new LeveledCacheQueryableDictionary<IReactiveStreamFactoryDefinition>(external.StreamFactories, local.StreamFactories);
            _subscriptionFactories = new LeveledCacheQueryableDictionary<IReactiveSubscriptionFactoryDefinition>(external.SubscriptionFactories, local.SubscriptionFactories);
            _subscriptions = new LeveledCacheQueryableDictionary<IReactiveSubscriptionProcess>(external.Subscriptions, local.Subscriptions);
            _streams = new LeveledCacheQueryableDictionary<IReactiveStreamProcess>(external.Streams, local.Streams);
        }

        /// <summary>
        /// Gets a dictionary providing access to observable definition metadata.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveObservableDefinition> Observables => _observables;

        /// <summary>
        /// Gets a dictionary providing access to observer definition metadata.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveObserverDefinition> Observers => _observers;

        /// <summary>
        /// Gets a dictionary providing access to stream factory definition metadata.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveStreamFactoryDefinition> StreamFactories => _streamFactories;

        /// <summary>
        /// Gets a dictionary providing access to stream factory definition metadata.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveSubscriptionFactoryDefinition> SubscriptionFactories => _subscriptionFactories;

        /// <summary>
        /// Gets a dictionary providing access to stream metadata.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveStreamProcess> Streams => _streams;

        /// <summary>
        /// Gets a dictionary providing access to subscription metadata.
        /// </summary>
        public IQueryableDictionary<Uri, IReactiveSubscriptionProcess> Subscriptions => _subscriptions;

        /// <summary>
        /// Creates and pushes a new call context local scope.
        /// </summary>
        /// <returns>Disposable used to pop and clean up the call context scope.</returns>
        public IDisposable CreateScope()
        {
            return new CompositeDisposable(
                _observables.CreateScope(),
                _observers.CreateScope(),
                _streamFactories.CreateScope(),
                _subscriptionFactories.CreateScope(),
                _streams.CreateScope(),
                _subscriptions.CreateScope());
        }

        private sealed class CompositeDisposable : IDisposable
        {
            private IDisposable[] _disposables;

            public CompositeDisposable(params IDisposable[] disposables)
            {
                _disposables = disposables;
            }

            public void Dispose()
            {
                var ds = Interlocked.Exchange(ref _disposables, Array.Empty<IDisposable>());
                if (ds != null)
                {
                    foreach (var d in ds)
                    {
                        d.Dispose();
                    }
                }
            }
        }
    }
}
