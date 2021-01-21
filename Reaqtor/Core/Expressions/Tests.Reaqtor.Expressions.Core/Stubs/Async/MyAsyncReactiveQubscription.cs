// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;

namespace Tests.Reaqtor.Expressions.Core
{
    public sealed class MyAsyncReactiveQubscription : AsyncReactiveQubscriptionBase
    {
        private readonly Expression _expression;

        public MyAsyncReactiveQubscription(IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            _expression = Expression.Constant(this);
        }

        public MyAsyncReactiveQubscription(IAsyncReactiveQueryProvider provider, Expression expression)
            : base(provider)
        {
            _expression = expression;
        }

        public override Expression Expression => _expression;

        protected override Task DisposeAsyncCore(CancellationToken token) => throw new NotImplementedException();
    }
}
