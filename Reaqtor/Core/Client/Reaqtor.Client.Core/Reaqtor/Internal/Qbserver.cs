// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    internal class Qbserver<T> : AsyncReactiveQbserverBase<T>
    {
        private int _initializing = 0;
        private volatile IAsyncReactiveObserver<T> _observer;

        public Qbserver(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task OnNextAsyncCore(T value, CancellationToken token)
        {
            return _observer?.OnNextAsync(value, token) ?? OnNextAsyncCoreSlow(value, token);
        }

        private async Task OnNextAsyncCoreSlow(T value, CancellationToken token)
        {
            await EnsureObserver(token).ConfigureAwait(false);

            await _observer.OnNextAsync(value, token).ConfigureAwait(false);
        }

        protected override Task OnErrorAsyncCore(Exception error, CancellationToken token)
        {
            return _observer?.OnErrorAsync(error, token) ?? OnErrorAsyncCoreSlow(error, token);
        }

        private async Task OnErrorAsyncCoreSlow(Exception error, CancellationToken token)
        {
            await EnsureObserver(token).ConfigureAwait(false);

            await _observer.OnErrorAsync(error, token).ConfigureAwait(false);
        }

        protected override Task OnCompletedAsyncCore(CancellationToken token)
        {
            return _observer?.OnCompletedAsync(token) ?? OnCompletedAsyncCoreSlow(token);
        }

        private async Task OnCompletedAsyncCoreSlow(CancellationToken token)
        {
            await EnsureObserver(token).ConfigureAwait(false);

            await _observer.OnCompletedAsync(token).ConfigureAwait(false);
        }

        private async Task EnsureObserver(CancellationToken token)
        {
            if (_observer == null)
            {
                if (Interlocked.CompareExchange(ref _initializing, 1, 0) == 1)
                {
                    throw new InvalidOperationException("Concurrent calls on observer detected.");
                }

                try
                {
                    var observer = ((AsyncReactiveQueryProviderBase)base.Provider).GetObserverAsync<T>(this, token);
                    _observer = await observer.ConfigureAwait(false);
                }
                finally
                {
                    Volatile.Write(ref _initializing, 0);
                }
            }
        }
    }
}
