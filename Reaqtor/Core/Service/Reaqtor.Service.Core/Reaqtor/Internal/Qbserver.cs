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
    internal class Qbserver<T> : ReactiveQbserverBase<T>
    {
        private readonly Lazy<IReactiveObserver<T>> _observer;

        public Qbserver(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
            _observer = new Lazy<IReactiveObserver<T>>(Resolve);
        }

        public override Expression Expression { get; }

        protected override void OnNextCore(T value) => _observer.Value.OnNext(value);

        protected override void OnErrorCore(Exception error) => _observer.Value.OnError(error);

        protected override void OnCompletedCore() => _observer.Value.OnCompleted();

        private IReactiveObserver<T> Resolve()
        {
            return ((ReactiveQueryProviderBase)base.Provider).GetObserver<T>(this);
        }
    }
}
