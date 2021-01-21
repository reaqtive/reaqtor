// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Reactive metadata service with local, in-memory storage.
    /// </summary>
    internal class ReactiveMetadataInMemory : IReactiveMetadataInMemory
    {
        /// <summary>
        /// Creates an in-memory store for reactive metadata.
        /// </summary>
        public ReactiveMetadataInMemory()
        {
            Observables = new ConcurrentDictionary<Uri, IReactiveObservableDefinition>();
            Observers = new ConcurrentDictionary<Uri, IReactiveObserverDefinition>();
            StreamFactories = new ConcurrentDictionary<Uri, IReactiveStreamFactoryDefinition>();
            SubscriptionFactories = new ConcurrentDictionary<Uri, IReactiveSubscriptionFactoryDefinition>();
            Streams = new ConcurrentDictionary<Uri, IReactiveStreamProcess>();
            Subscriptions = new ConcurrentDictionary<Uri, IReactiveSubscriptionProcess>();
        }

        /// <summary>
        /// Gets a dictionary providing access to observable definition metadata.
        /// </summary>
        /// <remarks>This collection is thread-safe for lookup and add calls.</remarks>
        public IDictionary<Uri, IReactiveObservableDefinition> Observables { get; }

        /// <summary>
        /// Gets a dictionary providing access to observer definition metadata.
        /// </summary>
        /// <remarks>This collection is thread-safe for lookup and add calls.</remarks>
        public IDictionary<Uri, IReactiveObserverDefinition> Observers { get; }

        /// <summary>
        /// Gets a dictionary providing access to stream factory definition metadata.
        /// </summary>
        /// <remarks>This collection is thread-safe for lookup and add calls.</remarks>
        public IDictionary<Uri, IReactiveStreamFactoryDefinition> StreamFactories { get; }

        /// <summary>
        /// Gets a dictionary providing access to subscription factory definition metadata.
        /// </summary>
        /// <remarks>This collection is thread-safe for lookup and add calls.</remarks>
        public IDictionary<Uri, IReactiveSubscriptionFactoryDefinition> SubscriptionFactories { get; }

        /// <summary>
        /// Gets a dictionary providing access to stream metadata.
        /// </summary>
        /// <remarks>This collection is thread-safe for lookup and add calls.</remarks>
        public IDictionary<Uri, IReactiveStreamProcess> Streams { get; }

        /// <summary>
        /// Gets a dictionary providing access to subscription metadata.
        /// </summary>
        /// <remarks>This collection is thread-safe for lookup and add calls.</remarks>
        public IDictionary<Uri, IReactiveSubscriptionProcess> Subscriptions { get; }
    }
}
