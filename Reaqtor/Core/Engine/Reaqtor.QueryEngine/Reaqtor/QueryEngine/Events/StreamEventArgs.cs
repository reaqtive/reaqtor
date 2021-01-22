// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Event arguments involving a stream.
    /// </summary>
    internal sealed class StreamEventArgs : ReactiveEntityEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="StreamEventArgs"/> class for the specified entity.
        /// </summary>
        /// <param name="entity">The entity representing the stream.</param>
        public StreamEventArgs(SubjectEntity entity)
            : base(entity.Uri, entity, ReactiveEntityKind.Stream)
        {
        }

        /// <summary>
        /// Gets the stream entity.
        /// </summary>
        public new IReactiveStreamProcess Entity => (IReactiveStreamProcess)base.Entity;
    }
}
