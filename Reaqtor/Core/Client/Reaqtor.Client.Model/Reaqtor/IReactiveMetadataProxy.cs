// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq;

namespace Reaqtor
{
    using Metadata;

    /// <summary>
    /// Interface for a client-side proxy for the metadata discovery operations on reactive processing services.
    /// </summary>
    /// <remarks>This interface provides essential information for reactive processing services that perform delegation. Implementers can extend the provided metadata.</remarks>
    public interface IReactiveMetadataProxy
    {
        /// <summary>
        /// Gets a queryable dictionary of stream factory definition objects.
        /// </summary>
        IQueryableDictionary<Uri, IAsyncReactiveStreamFactoryDefinition> StreamFactories { get; }

        /// <summary>
        /// Gets a queryable dictionary of stream objects.
        /// </summary>
        IQueryableDictionary<Uri, IAsyncReactiveStreamProcess> Streams { get; }

        /// <summary>
        /// Gets a queryable dictionary of observable definition objects.
        /// </summary>
        IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition> Observables { get; }

        /// <summary>
        /// Gets a queryable dictionary of observer definition objects.
        /// </summary>
        IQueryableDictionary<Uri, IAsyncReactiveObserverDefinition> Observers { get; }

        /// <summary>
        /// Gets a queryable dictionary of subscription factory definition objects.
        /// </summary>
        IQueryableDictionary<Uri, IAsyncReactiveSubscriptionFactoryDefinition> SubscriptionFactories { get; }

        /// <summary>
        /// Gets a queryable dictionary of subscription objects.
        /// </summary>
        IQueryableDictionary<Uri, IAsyncReactiveSubscriptionProcess> Subscriptions { get; }
    }
}
