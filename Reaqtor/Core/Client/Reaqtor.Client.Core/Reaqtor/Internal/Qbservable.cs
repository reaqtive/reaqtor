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
    internal class Qbservable<T> : AsyncReactiveQbservableBase<T>
    {
        public Qbservable(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> SubscribeAsyncCore(IAsyncReactiveQbserver<T> observer, Uri subscriptionUri, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).SubscribeAsync<T>(this, observer, subscriptionUri, state, token);
        }
    }
}
