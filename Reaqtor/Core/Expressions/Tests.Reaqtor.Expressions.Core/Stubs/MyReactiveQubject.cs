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
    public sealed class MyReactiveQubject<TInput, TOutput> : ReactiveQubjectBase<TInput, TOutput>
    {
        private readonly Expression _expression;

        public MyReactiveQubject(IReactiveQueryProvider provider)
            : base(provider)
        {
            _expression = Expression.Constant(this);
        }

        public MyReactiveQubject(IReactiveQueryProvider provider, Expression expression)
            : base(provider)
        {
            _expression = expression;
        }

        public override Expression Expression => _expression;

        protected override IReactiveQubscription SubscribeCore(IReactiveQbserver<TOutput> observer, Uri subscriptionUri, object state)
        {
            var expr =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<IReactiveQbservable<TOutput>, IReactiveQbserver<TOutput>, Uri, object, IReactiveQubscription>), "$subscribe"),
                    Expression,
                    observer.Expression,
                    Expression.Constant(subscriptionUri, typeof(Uri)),
                    Expression.Constant(state, typeof(object))
                );

            return Provider.CreateQubscription(expr);
        }

        protected override void OnNextCore(TInput value) => throw new NotImplementedException();

        protected override void OnErrorCore(Exception error) => throw new NotImplementedException();

        protected override void OnCompletedCore() => throw new NotImplementedException();

        protected override void DisposeCore() => throw new NotImplementedException();
    }
}
