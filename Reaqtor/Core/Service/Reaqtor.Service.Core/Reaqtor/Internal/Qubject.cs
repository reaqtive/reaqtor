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

namespace Reaqtor
{
    internal class Qubject<TInput, TOutput> : ReactiveQubjectBase<TInput, TOutput>
    {
        private readonly Lazy<IReactiveObserver<TInput>> _observer;

        public Qubject(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
            _observer = new Lazy<IReactiveObserver<TInput>>(Resolve);
        }

        public override Expression Expression { get; }

        #region Observer

        protected override void OnNextCore(TInput value) => _observer.Value.OnNext(value);

        protected override void OnErrorCore(Exception error) => _observer.Value.OnError(error);

        protected override void OnCompletedCore() => _observer.Value.OnCompleted();

        private IReactiveObserver<TInput> Resolve()
        {
            return ((ReactiveQueryProviderBase)base.Provider).GetObserver<TInput>(this);
        }

        #endregion

        #region Observable

        protected override IReactiveQubscription SubscribeCore(IReactiveQbserver<TOutput> observer, Uri subscriptionUri, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).Subscribe<TOutput>(this, observer, subscriptionUri, state);
        }

        #endregion

        #region Delete

        protected override void DisposeCore()
        {
            ((ReactiveQueryProviderBase)base.Provider).DeleteStream<TInput, TOutput>(this);
        }

        #endregion
    }
}
