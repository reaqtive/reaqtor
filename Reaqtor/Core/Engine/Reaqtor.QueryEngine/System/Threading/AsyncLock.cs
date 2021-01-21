// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace System.Threading
{
    internal sealed class AsyncLock : IDisposable
    {
        private readonly SemaphoreSlim _lock;

        public AsyncLock() => _lock = new SemaphoreSlim(1);

        public async Task<Releaser> EnterAsync()
        {
            await _lock.WaitAsync().ConfigureAwait(false);
            return new Releaser(this);
        }

        private void Exit() => _lock.Release();

        public void Dispose() => _lock.Dispose();

        public struct Releaser : IDisposable
        {
            private readonly AsyncLock _lock;
#if DEBUG
            private static readonly object True = true;
            private object _isDisposed;
#endif

            public Releaser(AsyncLock @lock)
            {
                _lock = @lock;
#if DEBUG
                _isDisposed = false;
#endif
            }

            public void Dispose()
            {
#if DEBUG
                if ((bool)Interlocked.Exchange(ref _isDisposed, True))
                    throw new ObjectDisposedException(nameof(AsyncLock));
#endif
                _lock.Exit();
            }
        }
    }
}
