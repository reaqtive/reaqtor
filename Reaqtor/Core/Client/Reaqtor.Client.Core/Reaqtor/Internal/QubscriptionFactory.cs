// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - Feburary 2016 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    internal class QubscriptionFactory : AsyncReactiveQubscriptionFactoryBase
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArgs> : AsyncReactiveQubscriptionFactoryBase<TArgs>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArgs argument, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, argument, subscriptionUri, state, token);
        }
    }
}
