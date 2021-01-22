// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Interface for observers of event streams.
    /// </summary>
    /// <typeparam name="T">Type of the data received by the observer.</typeparam>
    public interface IAsyncReactiveObserver<in T>
    {
        /// <summary>
        /// Sends a value to the stream.
        /// </summary>
        /// <param name="value">Object to send to the stream.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the order of events in the stream as observed by the server is undefined.</remarks>
        Task OnNextAsync(T value, CancellationToken token);

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        /// <summary>
        /// Reports an error to the stream.
        /// </summary>
        /// <param name="error">Error to report on the stream.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the order of events in the stream as observed by the server is undefined.</remarks>
        Task OnErrorAsync(Exception error, CancellationToken token);

#pragma warning restore CA1716

        /// <summary>
        /// Reports completion of the stream.
        /// </summary>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the order of events in the stream as observed by the server is undefined.</remarks>
        Task OnCompletedAsync(CancellationToken token);
    }
}
