// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Event arguments involving a subscription factory.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of <see cref="SubscriptionFactoryEventArgs"/> class for the specified entity.
    /// </remarks>
    /// <param name="entity">The entity representing the subscription factory.</param>
    internal sealed class SubscriptionFactoryEventArgs(IReactiveResource entity) : ReactiveEntityEventArgs(entity.Uri, entity, ReactiveEntityKind.SubscriptionFactory)
    {
    }
}
