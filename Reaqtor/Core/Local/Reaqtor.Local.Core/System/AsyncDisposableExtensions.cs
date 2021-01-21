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
    /// Provides a set of extension methods for IAsyncDisposable.
    /// </summary>
    public static class AsyncDisposableExtensions
    {
#if !NET5_0 && !NETSTANDARD2_1
        /// <summary>
        /// Disposes the resource asynchronously.
        /// </summary>
        /// <param name="disposable">Object to dispose.</param>
        /// <returns>Task representing the eventual completion of the disposal request.</returns>
        public static Task DisposeAsync(this IAsyncDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            return disposable.DisposeAsync(CancellationToken.None);
        }
#else
        /// <summary>
        /// Disposes the resource asynchronously.
        /// </summary>
        /// <param name="disposable">Object to dispose.</param>
        /// <param name="token">Token to observe for cancellation of the disposal request.</param>
        /// <returns>Task representing the eventual completion of the disposal request.</returns>
        public static ValueTask DisposeAsync(this IAsyncDisposable disposable, CancellationToken token)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            _ = token; // NB: By design for compat.

            return disposable.DisposeAsync();
        }
#endif

        /// <summary>
        /// Converts an IAsyncDisposable to a standard IDisposable.
        /// </summary>
        /// <param name="disposable">Disposable to provide a wrapper for.</param>
        /// <returns>Wrapper around the specified disposable, exposing the standard IDisposable interface.</returns>
        public static IDisposable AsDisposable(this IAsyncDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            return new Disposable(disposable);
        }

        private sealed class Disposable : IDisposable
        {
            private readonly IAsyncDisposable _disposable;

            public Disposable(IAsyncDisposable disposable) => _disposable = disposable;

            public void Dispose()
            {
                //
                // No idempotency enforcement. Left to the underlying IAsyncDisposable implementation.
                //
                _disposable.DisposeAsync(CancellationToken.None)
#if NET5_0 || NETSTANDARD2_1
                    .AsTask()
#endif
                    .Wait();
            }
        }
    }
}
