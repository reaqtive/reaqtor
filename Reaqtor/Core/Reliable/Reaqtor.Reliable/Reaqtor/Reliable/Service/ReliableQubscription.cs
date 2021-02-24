// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtive;

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public class ReliableQubscription : ReliableQubscriptionBase
    {
        public ReliableQubscription(Expression expression, IReliableQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        public override Uri ResubscribeUri => ((ReliableQueryProviderBase)Provider).GetSubscriptionResubscribeUri(this);

        protected override void StartCore(long sequenceId) => ((ReliableQueryProviderBase)Provider).StartSubscription(this, sequenceId);

        protected override void AcknowledgeRangeCore(long sequenceId) => ((ReliableQueryProviderBase)Provider).AcknowledgeRange(this, sequenceId);

        protected override void DisposeCore() => ((ReliableQueryProviderBase)Provider).DeleteSubscription(this);

        public override void Accept(ISubscriptionVisitor visitor)
        {
        }
    }
}
