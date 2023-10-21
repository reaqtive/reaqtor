// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

#if !NET8_0 && !NETSTANDARD2_1

using System.Threading;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Interface for asynchronous disposable of resources.
    /// </summary>
    public interface IAsyncDisposable
    {
        /// <summary>
        /// Disposes the resource asynchronously.
        /// </summary>
        /// <param name="token">Token to observe for cancellation of the disposal request.</param>
        /// <returns>Task representing the eventual completion of the disposal request.</returns>
        Task DisposeAsync(CancellationToken token);
    }
}

#endif
