// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks for protected methods.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public class ReliableQubjectFactory<TInput, TOutput>(Expression expression, IReliableQueryProvider provider) : ReliableQubjectFactoryBase<TInput, TOutput>(provider)
    {
        public override Expression Expression { get; } = expression;

        protected override IReliableMultiQubject<TInput, TOutput> CreateCore(Uri streamUri, object state) => ((ReliableQueryProviderBase)Provider).CreateStream(this, streamUri, state);
    }

    public class ReliableQubjectFactory<TInput, TOutput, TArg>(Expression expression, IReliableQueryProvider provider) : ReliableQubjectFactoryBase<TInput, TOutput, TArg>(provider)
    {
        public override Expression Expression { get; } = expression;

        protected override IReliableMultiQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg argument, object state) => ((ReliableQueryProviderBase)Provider).CreateStream(this, argument, streamUri, state);
    }
}
