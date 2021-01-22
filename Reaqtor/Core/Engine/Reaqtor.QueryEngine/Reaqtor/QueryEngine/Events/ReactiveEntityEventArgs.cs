// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Metadata;
using System;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Base class for event arguments involving a reactive entity.
    /// </summary>
    public abstract class ReactiveEntityEventArgs : EventArgs, IKnownResource
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ReactiveEntityEventArgs"/> class for the specified entity.
        /// </summary>
        /// <param name="uri">URI of the reactive entity.</param>
        /// <param name="entity">Reactive entity.</param>
        /// <param name="entityType">Kind of the reactive entity.</param>
        protected ReactiveEntityEventArgs(Uri uri, IReactiveResource entity, ReactiveEntityKind entityType)
        {
            Uri = uri;
            Entity = entity;
            EntityType = entityType;
        }

        /// <summary>
        /// Gets the URI of the reactive entity.
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// Gets the kind of the reactive entity.
        /// </summary>
        public ReactiveEntityKind EntityType { get; }

        /// <summary>
        /// Gets the reactive entity.
        /// </summary>
        public IReactiveResource Entity { get; }
    }
}
