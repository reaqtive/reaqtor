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
    internal class Qubject<TInput, TOutput> : AsyncReactiveQubjectBase<TInput, TOutput>
    {
        private readonly IAsyncReactiveQbserver<TInput> _observer;

        public Qubject(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
            _observer = provider.CreateQbserver<TInput>(expression);
        }

        public override Expression Expression { get; }

        #region Observer

        protected override Task OnNextAsyncCore(TInput value, CancellationToken token) => _observer.OnNextAsync(value, token);

        protected override Task OnErrorAsyncCore(Exception error, CancellationToken token) => _observer.OnErrorAsync(error, token);

        protected override Task OnCompletedAsyncCore(CancellationToken token) => _observer.OnCompletedAsync(token);

        #endregion

        #region Observable

        protected override Task<IAsyncReactiveQubscription> SubscribeAsyncCore(IAsyncReactiveQbserver<TOutput> observer, Uri subscriptionUri, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).SubscribeAsync<TOutput>(this, observer, subscriptionUri, state, token);
        }

        #endregion

        #region Delete

        protected override Task DisposeAsyncCore(CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).DeleteStreamAsync<TInput, TOutput>(this, token);
        }

        #endregion
    }
}
