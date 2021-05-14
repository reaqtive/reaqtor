// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

using Reaqtor.Reliable.Client;

namespace Reaqtor.Reliable.Expressions
{
    public abstract class ReliableQubscriptionBase : ReliableReactiveSubscriptionBase, IReliableQubscription
    {
        protected ReliableQubscriptionBase(IReliableQueryProvider provider) => Provider = provider;

        public IReliableQueryProvider Provider { get; }

        public abstract Expression Expression { get; }
    }
}
