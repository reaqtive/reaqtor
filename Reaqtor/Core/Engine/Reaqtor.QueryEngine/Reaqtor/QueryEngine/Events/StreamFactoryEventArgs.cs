// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Event arguments involving a stream factory.
    /// </summary>
    internal sealed class StreamFactoryEventArgs : ReactiveEntityEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="StreamFactoryEventArgs"/> class for the specified entity.
        /// </summary>
        /// <param name="entity">The entity representing the stream factory.</param>
        public StreamFactoryEventArgs(IReactiveResource entity)
            : base(entity.Uri, entity, ReactiveEntityKind.StreamFactory)
        {
        }
    }
}
