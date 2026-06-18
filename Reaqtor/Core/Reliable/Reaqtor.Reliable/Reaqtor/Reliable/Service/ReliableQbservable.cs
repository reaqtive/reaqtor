// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks for protected methods.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public class ReliableQbservable<T>(Expression expression, IReliableQueryProvider provider) : ReliableQbservableBase<T>(provider)
    {
        public override Expression Expression { get; } = expression;

        protected override IReliableQubscription SubscribeCore(IReliableQbserver<T> observer, Uri subscriptionUri, object state) => ((ReliableQueryProviderBase)Provider).Subscribe(this, observer, subscriptionUri, state);
    }
}
