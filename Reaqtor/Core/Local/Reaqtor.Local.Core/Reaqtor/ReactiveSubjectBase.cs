// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Reaqtor
{
    /// <summary>
    /// Base class for subjects.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
    public abstract class ReactiveSubjectBase<TInput, TOutput> : IReactiveSubject<TInput, TOutput>
    {
        #region Observer

        #region OnNext

        /// <summary>
        /// Sends a value to the subject.
        /// </summary>
        /// <param name="value">Object to send to the subject.</param>
        public void OnNext(TInput value)
        {
            OnNextCore(value);
        }

        /// <summary>
        /// Sends a value to the subject.
        /// </summary>
        /// <param name="value">Object to send to the subject.</param>
        protected abstract void OnNextCore(TInput value);

        #endregion

        #region OnError

        /// <summary>
        /// Reports an error to the subject.
        /// </summary>
        /// <param name="error">Error to report on the subject.</param>
        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            OnErrorCore(error);
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        /// <summary>
        /// Reports an error to the subject.
        /// </summary>
        /// <param name="error">Error to report on the subject.</param>
        protected abstract void OnErrorCore(Exception error);

#pragma warning restore CA1716

        #endregion

        #region OnCompleted

        /// <summary>
        /// Reports completion of the subject.
        /// </summary>
        public void OnCompleted()
        {
            OnCompletedCore();
        }

        /// <summary>
        /// Reports completion of the subject.
        /// </summary>
        protected abstract void OnCompletedCore();

        #endregion

        #endregion

        #region Observable

        #region Subscribe

        /// <summary>
        /// Subscribes to the subject using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the subject's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        public IReactiveSubscription Subscribe(IReactiveObserver<TOutput> observer, Uri subscriptionUri, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return SubscribeCore(observer, subscriptionUri, state);
        }

        /// <summary>
        /// Subscribes to the subject using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the subject's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        protected abstract IReactiveSubscription SubscribeCore(IReactiveObserver<TOutput> observer, Uri subscriptionUri, object state);

        #endregion

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes the subject.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">true if called from <see cref="IDisposable.Dispose"/>; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCore();
            }
        }

        /// <summary>
        /// Disposes the subject.
        /// </summary>
        protected abstract void DisposeCore();

        #endregion
    }
}
