// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Reaqtor.Metadata
{
    /// <summary>
    /// Interface representing an active resource in a reactive processing service. Processes are hot entities.
    /// </summary>
    /// <remarks>The IAsyncDisposable implementation will stop the process, e.g. cancelling a subscription or deleting a subject.</remarks>
    public interface IAsyncReactiveProcessResource : IAsyncReactiveResource, IAsyncDisposable
    {
        /// <summary>
        /// Gets the state that was passed during creation of the resource.
        /// </summary>
        /// <remarks>Implementers can provide statically typed accessors in derived types.</remarks>
        object State { get; }

        /// <summary>
        /// Gets the date and time when the process was created.
        /// </summary>
        DateTimeOffset CreationTime { get; }
    }
}
