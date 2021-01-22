// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Client;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public class ReliableQbserver<T> : ReliableQbserverBase<T>
    {
        private readonly Lazy<IReliableReactiveObserver<T>> _observer;

        public ReliableQbserver(Expression expression, IReliableQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
            _observer = new Lazy<IReliableReactiveObserver<T>>(Resolve);
        }

        public override Expression Expression { get; }

        public override Uri ResubscribeUri => _observer.Value.ResubscribeUri;

        protected override void OnNextCore(T item, long sequenceId) => _observer.Value.OnNext(item, sequenceId);

        protected override void OnStartedCore() => _observer.Value.OnStarted();

        protected override void OnErrorCore(Exception error) => _observer.Value.OnError(error);

        protected override void OnCompletedCore() => _observer.Value.OnCompleted();

        private IReliableReactiveObserver<T> Resolve() => ((ReliableQueryProviderBase)Provider).GetObserver(this);
    }
}
