// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.Remoting.Tasks
{
    /// <summary>
    /// Base class for implementing marshalable remote services.
    /// </summary>
    public class RemoteServiceBase : MarshalByRefObject, ICancellationProvider
    {
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _operations = new();

        /// <summary>
        /// Used to indicate the lifetime of the remote service.
        /// </summary>
        /// <returns>Always returns null.</returns>
        public override object InitializeLifetimeService() => null;

        /// <summary>
        /// Starts a local asynchronous operation and returns a disposable that can be used to cancel the operation.
        /// </summary>
        /// <typeparam name="T">The expected return type of the operation.</typeparam>
        /// <param name="invokeTask">The local asynchronous operation.</param>
        /// <param name="reply">The observer used to notify when the task completes or faults.</param>
        /// <returns>A disposable handle that can be used to cancel the operation.</returns>
        protected IDisposable Invoke<T>(Func<CancellationToken, Task<T>> invokeTask, IObserver<T> reply)
        {
            if (invokeTask == null)
                throw new ArgumentNullException(nameof(invokeTask));
            if (reply == null)
                throw new ArgumentNullException(nameof(reply));

            var guid = Guid.NewGuid();
            var cts = new CancellationTokenSource();
            _operations.TryAdd(guid, cts);

            invokeTask(cts.Token).ContinueWith(t =>
            {
                _operations.TryRemove(guid, out _);

                if (t.IsFaulted)
                {
                    reply.OnError(t.Exception);
                }
                else if (t.IsCanceled)
                {
                    reply.OnError(new OperationCanceledException());
                }
                else if (t.IsCompleted)
                {
                    reply.OnNext(t.Result);
                }
            });

            return new RemoteCancellationDisposable(this, guid);
        }

        /// <summary>
        /// Cancels an operation with the given GUID.
        /// </summary>
        /// <param name="identifier">The GUID.</param>
        public void Cancel(Guid identifier)
        {
            if (_operations.TryRemove(identifier, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
    }
}
