// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Interface describing in memory metadata storage.
    /// </summary>
    /// <remarks>Dictionaries exposed on this interface are thread-safe for lookup and add calls.</remarks>
    internal interface IReactiveMetadataInMemory
    {
        /// <summary>
        /// Gets a dictionary providing access to observable definition metadata.
        /// </summary>
        /// <remarks>Note to implementers: This collection should be thread-safe for lookup and add calls.</remarks>
        IDictionary<Uri, IReactiveObservableDefinition> Observables { get; }

        /// <summary>
        /// Gets a dictionary providing access to observer definition metadata.
        /// </summary>
        /// <remarks>Note to implementers: This collection should be thread-safe for lookup and add calls.</remarks>
        IDictionary<Uri, IReactiveObserverDefinition> Observers { get; }

        /// <summary>
        /// Gets a dictionary providing access to stream factory definition metadata.
        /// </summary>
        /// <remarks>Note to implementers: This collection should be thread-safe for lookup and add calls.</remarks>
        IDictionary<Uri, IReactiveStreamFactoryDefinition> StreamFactories { get; }

        /// <summary>
        /// Gets a dictionary providing access to subscription factory definition metadata.
        /// </summary>
        /// <remarks>Note to implementers: This collection should be thread-safe for lookup and add calls.</remarks>
        IDictionary<Uri, IReactiveSubscriptionFactoryDefinition> SubscriptionFactories { get; }

        /// <summary>
        /// Gets a dictionary providing access to stream metadata.
        /// </summary>
        /// <remarks>Note to implementers: This collection should be thread-safe for lookup and add calls.</remarks>
        IDictionary<Uri, IReactiveStreamProcess> Streams { get; }

        /// <summary>
        /// Gets a dictionary providing access to subscription metadata.
        /// </summary>
        /// <remarks>Note to implementers: This collection should be thread-safe for lookup and add calls.</remarks>
        IDictionary<Uri, IReactiveSubscriptionProcess> Subscriptions { get; }
    }
}
