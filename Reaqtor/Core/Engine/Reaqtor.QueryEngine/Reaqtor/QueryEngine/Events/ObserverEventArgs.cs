// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Event arguments involving an observer.
    /// </summary>
    internal sealed class ObserverEventArgs : ReactiveEntityEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="ObserverEventArgs"/> class for the specified entity.
        /// </summary>
        /// <param name="entity">The entity representing the observer.</param>
        public ObserverEventArgs(IReactiveResource entity)
            : base(entity.Uri, entity, ReactiveEntityKind.Observer)
        {
        }
    }
}
