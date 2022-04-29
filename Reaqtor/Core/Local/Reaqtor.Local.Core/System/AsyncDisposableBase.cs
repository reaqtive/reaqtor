// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Threading;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Base class for asynchronous resource disposables.
    /// </summary>
    public abstract class AsyncDisposableBase : IAsyncDisposable
    {
#pragma warning disable IDE0079 // Remove unnecessary suppressions.
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize. (Analyzer does not know about pre-netstandard2.1 IAsyncDisposable.)

#if !NET6_0 && !NETSTANDARD2_1
        /// <summary>
        /// Disposes the resource asynchronously.
        /// </summary>
        /// <param name="token">Token to observe for cancellation of the disposal request.</param>
        /// <returns>Task representing the eventual completion of the disposal request.</returns>
        public async Task DisposeAsync(CancellationToken token)
        {
            //
            // TODO: Enforce idempotent behavior? What about transient errors, cancellations, etc?
            //       Should be allow retry but no concurrent calls? Do we rely on DisposeAsyncCore
            //       being idempotent itself?
            //
            await DisposeAsyncCore(token).ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }
#else
        /// <summary>
        /// Disposes the resource asynchronously.
        /// </summary>
        /// <returns>Task representing the eventual completion of the disposal request.</returns>
        public async ValueTask DisposeAsync()
        {
            //
            // TODO: Enforce idempotent behavior? What about transient errors, cancellations, etc?
            //       Should be allow retry but no concurrent calls? Do we rely on DisposeAsyncCore
            //       being idempotent itself?
            //
            await DisposeAsyncCore(default).ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }
#endif

        /// <summary>
        /// Disposes the resource asynchronously.
        /// </summary>
        /// <param name="token">Token to observe for cancellation of the disposal request.</param>
        /// <returns>Task representing the eventual completion of the disposal request.</returns>
        protected abstract Task DisposeAsyncCore(CancellationToken token);
    }
}
