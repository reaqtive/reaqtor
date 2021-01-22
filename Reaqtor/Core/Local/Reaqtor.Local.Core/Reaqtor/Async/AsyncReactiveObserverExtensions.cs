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
    /// Provides a set of extension methods for IAsyncReactiveObserver&lt;T&gt;.
    /// </summary>
    public static class AsyncReactiveObserverExtensions
    {
        #region OnNextAsync

        /// <summary>
        /// Sends a value to the stream.
        /// </summary>
        /// <param name="observer">Observer to send the notification on.</param>
        /// <param name="value">Object to send to the stream.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the order of events in the stream as observed by the server is undefined.</remarks>
        public static Task OnNextAsync<T>(this IAsyncReactiveObserver<T> observer, T value)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return observer.OnNextAsync(value, CancellationToken.None);
        }

        #endregion

        #region OnErrorAsync

        /// <summary>
        /// Reports an error to the stream.
        /// </summary>
        /// <param name="observer">Observer to send the notification on.</param>
        /// <param name="error">Error to report on the stream.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the order of events in the stream as observed by the server is undefined.</remarks>
        public static Task OnErrorAsync<T>(this IAsyncReactiveObserver<T> observer, Exception error)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return observer.OnErrorAsync(error, CancellationToken.None);
        }

        #endregion

        #region OnCompletedAsync

        /// <summary>
        /// Reports completion of the stream.
        /// </summary>
        /// <param name="observer">Observer to send the notification on.</param>
        /// <returns>Task to await the acknowledgement of the submission of the event, or an exception.</returns>
        /// <remarks>If observer calls are not awaited in a sequential manner, the order of events in the stream as observed by the server is undefined.</remarks>
        public static Task OnCompletedAsync<T>(this IAsyncReactiveObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return observer.OnCompletedAsync(CancellationToken.None);
        }

        #endregion

        #region ToObserver

        /// <summary>
        /// Converts an IAsyncReactiveObserver&lt;T&gt; to an IReactiveObserver&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="observer">Observer to provide a wrapper for.</param>
        /// <returns>Wrapper around the specified observer, exposing the IReactiveObserver&lt;T&gt; interface.</returns>
        public static IReactiveObserver<T> ToObserver<T>(this IAsyncReactiveObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return new Observer<T>(observer);
        }

        private sealed class Observer<T> : IReactiveObserver<T>
        {
            private readonly IAsyncReactiveObserver<T> _observer;

            public Observer(IAsyncReactiveObserver<T> observer) => _observer = observer;

            public void OnNext(T value) => _observer.OnNextAsync(value).Wait();

            public void OnError(Exception error) => _observer.OnErrorAsync(error).Wait();

            public void OnCompleted() => _observer.OnCompletedAsync().Wait();
        }

        #endregion
    }
}
