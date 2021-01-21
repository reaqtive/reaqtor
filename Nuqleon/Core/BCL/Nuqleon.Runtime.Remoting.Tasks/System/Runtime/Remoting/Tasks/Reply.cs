// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System.Threading.Tasks;

namespace System.Runtime.Remoting.Tasks
{
    /// <summary>
    /// A marshalable observer used by the client to receive notifications for remote operations.
    /// </summary>
    /// <typeparam name="T">The expected result type of the remote operation.</typeparam>
    public class Reply<T> : MarshalByRefObject, IObserver<T>
    {
        private readonly TaskCompletionSource<T> _taskCompletionSource;

        /// <summary>
        /// Instantiates the reply observer.
        /// </summary>
        /// <param name="taskCompletionSource">
        /// The task completion source that the observer should push notifications to.
        /// </param>
        public Reply(TaskCompletionSource<T> taskCompletionSource)
        {
            _taskCompletionSource = taskCompletionSource ?? throw new ArgumentNullException(nameof(taskCompletionSource));
        }

        /// <summary>
        /// Used to indicate the lifetime of the remote service.
        /// </summary>
        /// <returns>Always returns null.</returns>
        public override object InitializeLifetimeService() => null;

        /// <summary>
        /// This observer channel should not be used.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Always thrown.</exception>
        public void OnCompleted()
        {
            throw new InvalidOperationException("This observer channel should not be used.");
        }

        /// <summary>
        /// When an error occurs during remote execution of an operation, sends the exception that was thrown.
        /// </summary>
        /// <param name="error">The exception that was thrown.</param>
        public void OnError(Exception error)
        {
            if (error is OperationCanceledException)
            {
                _taskCompletionSource.TrySetCanceled();
            }
            else
            {
                _taskCompletionSource.TrySetException(error);
            }
        }

        /// <summary>
        /// When the remote execution of an operation completes, sends the result.
        /// </summary>
        /// <param name="value">The result.</param>
        public void OnNext(T value)
        {
            _taskCompletionSource.TrySetResult(value);
        }
    }
}
