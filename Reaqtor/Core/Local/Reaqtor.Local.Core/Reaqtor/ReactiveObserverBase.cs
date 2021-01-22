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
    /// Base class for observers.
    /// </summary>
    /// <typeparam name="T">Type of the data received by the observable.</typeparam>
    public abstract class ReactiveObserverBase<T> : IReactiveObserver<T>
    {
        #region OnNext

        /// <summary>
        /// Sends a value to the observer.
        /// </summary>
        /// <param name="value">Object to send to the observer.</param>
        public void OnNext(T value)
        {
            OnNextCore(value);
        }

        /// <summary>
        /// Sends a value to the observer.
        /// </summary>
        /// <param name="value">Object to send to the observer.</param>
        protected abstract void OnNextCore(T value);

        #endregion

        #region OnError

        /// <summary>
        /// Reports an error to the observer.
        /// </summary>
        /// <param name="error">Error to report on the observer.</param>
        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            OnErrorCore(error);
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        /// <summary>
        /// Reports an error to the observer.
        /// </summary>
        /// <param name="error">Error to report on the observer.</param>
        protected abstract void OnErrorCore(Exception error);

#pragma warning restore CA1716

        #endregion

        #region OnCompleted

        /// <summary>
        /// Reports completion of the observer.
        /// </summary>
        public void OnCompleted()
        {
            OnCompletedCore();
        }

        /// <summary>
        /// Reports completion of the observer.
        /// </summary>
        protected abstract void OnCompletedCore();

        #endregion
    }
}
