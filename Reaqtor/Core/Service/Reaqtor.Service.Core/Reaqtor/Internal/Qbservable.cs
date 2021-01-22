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
    internal class Qbservable<T> : ReactiveQbservableBase<T>
    {
        public Qbservable(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubscription SubscribeCore(IReactiveQbserver<T> observer, Uri subscriptionUri, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).Subscribe<T>(this, observer, subscriptionUri, state);
        }
    }
}
