// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    internal class QubjectFactory<TInput, TOutput> : AsyncReactiveQubjectFactoryBase<TInput, TOutput>
    {
        public QubjectFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateStreamAsync(this, streamUri, state, token);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArgs> : AsyncReactiveQubjectFactoryBase<TInput, TOutput, TArgs>
    {
        public QubjectFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArgs argument, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateStreamAsync(this, argument, streamUri, state, token);
        }
    }
}
