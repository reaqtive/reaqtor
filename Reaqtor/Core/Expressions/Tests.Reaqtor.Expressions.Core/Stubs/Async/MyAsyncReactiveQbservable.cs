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
    public sealed class MyAsyncReactiveQbservable<T> : AsyncReactiveQbservableBase<T>
    {
        private readonly Expression _expression;

        public MyAsyncReactiveQbservable(IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            _expression = Expression.Constant(this);
        }

        public MyAsyncReactiveQbservable(IAsyncReactiveQueryProvider provider, Expression expression)
            : base(provider)
        {
            _expression = expression;
        }

        public override Expression Expression => _expression;

        protected override Task<IAsyncReactiveQubscription> SubscribeAsyncCore(IAsyncReactiveQbserver<T> observer, Uri subscriptionUri, object state, CancellationToken token)
        {
            var expr =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<T>, IAsyncReactiveQbserver<T>, Uri, object, IAsyncReactiveQubscription>), "$subscribe"),
                    Expression,
                    observer.Expression,
                    Expression.Constant(subscriptionUri, typeof(Uri)),
                    Expression.Constant(state, typeof(object))
                );

            return Task.FromResult<IAsyncReactiveQubscription>(Provider.CreateQubscription(expr));
        }
    }
}
