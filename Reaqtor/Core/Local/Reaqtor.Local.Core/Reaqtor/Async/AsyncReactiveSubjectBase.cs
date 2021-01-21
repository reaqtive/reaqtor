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
    /// Base class for subjects.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
    public abstract class AsyncReactiveSubjectBase<TInput, TOutput> : AsyncDisposableBase, IAsyncReactiveSubject<TInput, TOutput>
    {
        #region Observer

        #region OnNextAsync

        /// <summary>
        /// Sends a value to the subject.
        /// </summary>
        /// <param name="value">Object to send to the subject.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the subject is undefined.</remarks>
        public Task OnNextAsync(TInput value, CancellationToken token)
        {
            return OnNextAsyncCore(value, token);
        }

        /// <summary>
        /// Sends a value to the subject.
        /// </summary>
        /// <param name="value">Object to send to the subject.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the subject is undefined.</remarks>
        protected abstract Task OnNextAsyncCore(TInput value, CancellationToken token);

        #endregion

        #region OnErrorAsync

        /// <summary>
        /// Reports an error to the subject.
        /// </summary>
        /// <param name="error">Error to report on the subject.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the subject is undefined.</remarks>
        public Task OnErrorAsync(Exception error, CancellationToken token)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return OnErrorAsyncCore(error, token);
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        /// <summary>
        /// Reports an error to the subject.
        /// </summary>
        /// <param name="error">Error to report on the subject.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the subject is undefined.</remarks>
        protected abstract Task OnErrorAsyncCore(Exception error, CancellationToken token);

#pragma warning restore CA1716

        #endregion

        #region OnCompletedAsync

        /// <summary>
        /// Reports completion of the subject.
        /// </summary>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the subject is undefined.</remarks>
        public Task OnCompletedAsync(CancellationToken token)
        {
            return OnCompletedAsyncCore(token);
        }

        /// <summary>
        /// Reports completion of the subject.
        /// </summary>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the processing order of events in the subject is undefined.</remarks>
        protected abstract Task OnCompletedAsyncCore(CancellationToken token);

        #endregion

        #endregion

        #region Observable

        #region SubscribeAsync

        /// <summary>
        /// Subscribes to the subject using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the subject's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        public Task<IAsyncReactiveSubscription> SubscribeAsync(IAsyncReactiveObserver<TOutput> observer, Uri subscriptionUri, object state = null, CancellationToken token = default)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return SubscribeAsyncCore(observer, subscriptionUri, state, token);
        }

        /// <summary>
        /// Subscribes to the subject using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the subject's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        protected abstract Task<IAsyncReactiveSubscription> SubscribeAsyncCore(IAsyncReactiveObserver<TOutput> observer, Uri subscriptionUri, object state, CancellationToken token);

        #endregion

        #endregion
    }
}
