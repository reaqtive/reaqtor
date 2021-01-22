// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    internal class Qubscription : AsyncReactiveQubscriptionBase
    {
        public Qubscription(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        protected override Task DisposeAsyncCore(CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).DeleteSubscriptionAsync(this, token);
        }

        public override Expression Expression { get; }
    }
}
