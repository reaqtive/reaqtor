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
    public sealed class MyReactiveQbserver<T> : ReactiveQbserverBase<T>
    {
        private readonly Expression _expression;

        public MyReactiveQbserver(IReactiveQueryProvider provider)
            : base(provider)
        {
            _expression = Expression.Constant(this);
        }

        public MyReactiveQbserver(IReactiveQueryProvider provider, Expression expression)
            : base(provider)
        {
            _expression = expression;
        }

        public override Expression Expression => _expression;

        protected override void OnNextCore(T value) => throw new NotImplementedException();

        protected override void OnErrorCore(Exception error) => throw new NotImplementedException();

        protected override void OnCompletedCore() => throw new NotImplementedException();
    }
}
