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

namespace Reaqtor
{
    internal class QubjectFactory<TInput, TOutput> : ReactiveQubjectFactoryBase<TInput, TOutput>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg argument, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, argument, streamUri, state);
        }
    }
}
