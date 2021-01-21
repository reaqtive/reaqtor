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

using Reaqtor;

namespace Tests.Reaqtor.Expressions.Core
{
    public sealed class MyReactiveQubscription : ReactiveQubscriptionBase
    {
        private readonly Expression _expression;

        public MyReactiveQubscription(IReactiveQueryProvider provider)
            : base(provider)
        {
            _expression = Expression.Constant(this);
        }

        public MyReactiveQubscription(IReactiveQueryProvider provider, Expression expression)
            : base(provider)
        {
            _expression = expression;
        }

        public override Expression Expression => _expression;

        protected override void DisposeCore() => throw new NotImplementedException();
    }
}
