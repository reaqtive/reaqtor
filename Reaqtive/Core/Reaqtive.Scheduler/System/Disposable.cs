// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;

namespace System
{
    internal static class Disposable
    {
        public static IDisposable Create(Action dispose)
        {
            return new AnonymousDisposable(dispose);
        }

        private sealed class AnonymousDisposable : IDisposable
        {
            private Action _dispose;

            public AnonymousDisposable(Action dispose)
            {
                _dispose = dispose;
            }

            public void Dispose()
            {
                Interlocked.Exchange(ref _dispose, null)?.Invoke();
            }
        }
    }
}
