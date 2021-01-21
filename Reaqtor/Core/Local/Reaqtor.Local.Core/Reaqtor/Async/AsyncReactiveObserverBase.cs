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
    /// Base class for observers.
    /// </summary>
    /// <typeparam name="T">Type of the data received by the observable.</typeparam>
    public abstract class AsyncReactiveObserverBase<T> : IAsyncReactiveObserver<T>
    {
        #region OnNextAsync

        /// <summary>
        /// Sends a value to the observer.
        /// </summary>
        /// <param name="value">Object to send to the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the observer is undefined.</remarks>
        public Task OnNextAsync(T value, CancellationToken token = default)
        {
            return OnNextAsyncCore(value, token);
        }

        /// <summary>
        /// Sends a value to the observer.
        /// </summary>
        /// <param name="value">Object to send to the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the observer is undefined.</remarks>
        protected abstract Task OnNextAsyncCore(T value, CancellationToken token);

        #endregion

        #region OnErrorAsync

        /// <summary>
        /// Reports an error to the observer.
        /// </summary>
        /// <param name="error">Error to report on the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the observer is undefined.</remarks>
        public Task OnErrorAsync(Exception error, CancellationToken token = default)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return OnErrorAsyncCore(error, token);
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        /// <summary>
        /// Reports an error to the observer.
        /// </summary>
        /// <param name="error">Error to report on the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the observer is undefined.</remarks>
        protected abstract Task OnErrorAsyncCore(Exception error, CancellationToken token = default);

#pragma warning restore CA1716

        #endregion

        #region OnCompletedAsync

        /// <summary>
        /// Reports completion of the observer.
        /// </summary>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the observer is undefined.</remarks>
        public Task OnCompletedAsync(CancellationToken token = default)
        {
            return OnCompletedAsyncCore(token);
        }

        /// <summary>
        /// Reports completion of the observer.
        /// </summary>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the observer is undefined.</remarks>
        protected abstract Task OnCompletedAsyncCore(CancellationToken token);

        #endregion
    }
}
