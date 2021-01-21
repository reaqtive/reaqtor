// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.Remoting.Tasks
{
    /// <summary>
    /// Base class with helper methods to convert a remote request into a local task.
    /// </summary>
    public class RemoteProxyBase
    {
        /// <summary>
        /// Invokes a remote operation.
        /// </summary>
        /// <typeparam name="T">The expected return type of the operation.</typeparam>
        /// <param name="invokeRemote">The remote operation.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task to await the completion of the remote operation.</returns>
        protected static async Task<T> Invoke<T>(Func<IObserver<T>, IDisposable> invokeRemote, CancellationToken token)
        {
            if (invokeRemote == null)
                throw new ArgumentNullException(nameof(invokeRemote));

            var tcs = new TaskCompletionSource<T>();

            var cancel = invokeRemote(new Reply<T>(tcs));

            using var ctr = token.Register(() =>
            {
                tcs.TrySetCanceled();
                cancel.Dispose();
            });

            return await tcs.Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Invokes a remote operation.
        /// </summary>
        /// <typeparam name="T">The expected return type of the operation.</typeparam>
        /// <param name="invokeRemote">The remote operation.</param>
        /// <returns>A task to await the completion of the remote operation.</returns>
        protected static Task<T> Invoke<T>(Action<IObserver<T>> invokeRemote)
        {
            if (invokeRemote == null)
                throw new ArgumentNullException(nameof(invokeRemote));

            var tcs = new TaskCompletionSource<T>();

            invokeRemote(new Reply<T>(tcs));

            return tcs.Task;
        }
    }
}
