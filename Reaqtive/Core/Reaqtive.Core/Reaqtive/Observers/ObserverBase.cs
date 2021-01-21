// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

namespace Reaqtive
{
    /// <summary>
    /// Base class for observer implementations.
    /// </summary>
    /// <typeparam name="T">Type of the elements received by the subject.</typeparam>
    public abstract class ObserverBase<T> : IObserver<T>
    {
        private int _stopped = 0;

        /// <summary>
        /// Pushes a completion notification into the observer.
        /// </summary>
        public void OnCompleted()
        {
            if (Interlocked.Exchange(ref _stopped, 1) == 0)
            {
                OnCompletedCore();
            }
        }

        /// <summary>
        /// Pushes a completion notification into the observer.
        /// </summary>
        protected abstract void OnCompletedCore();

        /// <summary>
        /// Pushes the specified error into the observer.
        /// </summary>
        /// <param name="error">Error to push into the observer.</param>
        public void OnError(Exception error)
        {
            if (Interlocked.Exchange(ref _stopped, 1) == 0)
            {
                OnErrorCore(error);
            }
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        /// <summary>
        /// Pushes the specified error into the observer.
        /// </summary>
        /// <param name="error">Error to push into the observer.</param>
        protected abstract void OnErrorCore(Exception error);

#pragma warning restore CA1716

        /// <summary>
        /// Pushes the specified value into the observer.
        /// </summary>
        /// <param name="value">Value to push into the observer.</param>
        public void OnNext(T value)
        {
            OnNextCore(value);
        }

        /// <summary>
        /// Pushes the specified value into the observer.
        /// </summary>
        /// <param name="value">Value to push into the observer.</param>
        protected abstract void OnNextCore(T value);
    }
}
