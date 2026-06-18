// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2016 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    internal class QubscriptionFactory(Expression expression, IReactiveQueryProvider provider) : ReactiveQubscriptionFactoryBase(provider)
    {
        public override Expression Expression { get; } = expression;

        protected override IReactiveQubscription CreateCore(Uri subscriptionUri, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateSubscription(this, subscriptionUri, state);
        }
    }

    internal class QubscriptionFactory<TArg>(Expression expression, IReactiveQueryProvider provider) : ReactiveQubscriptionFactoryBase<TArg>(provider)
    {
        public override Expression Expression { get; } = expression;

        protected override IReactiveQubscription CreateCore(Uri subscriptionUri, TArg argument, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateSubscription(this, argument, subscriptionUri, state);
        }
    }
}
