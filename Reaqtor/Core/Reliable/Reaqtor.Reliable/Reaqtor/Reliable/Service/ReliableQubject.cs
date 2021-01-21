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
    public class ReliableQubject<TInput, TOutput> : ReliableQubjectBase<TInput, TOutput>
    {
        public ReliableQubject(Expression expression, IReliableQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReliableQubscription SubscribeCore(IReliableQbserver<TOutput> observer, Uri subscriptionUri, object state) => ((ReliableQueryProviderBase)Provider).Subscribe(this, observer, subscriptionUri, state);

        protected override IReliableQbserver<TInput> CreateQbserverCore() => ((ReliableQueryProviderBase)Provider).CreateObserver(this);

        protected override void DisposeCore() => ((ReliableQueryProviderBase)Provider).DeleteStream(this);
    }
}
