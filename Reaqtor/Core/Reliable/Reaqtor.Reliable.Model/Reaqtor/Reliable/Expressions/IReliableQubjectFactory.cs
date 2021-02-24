// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Reliable.Client;
using System;

namespace Reaqtor.Reliable.Expressions
{
    public interface IReliableQubjectFactory<TInput, TOutput> : IReliableReactiveSubjectFactory<TInput, TOutput>, IReliableReactiveExpressible
    {
        new IReliableMultiQubject<TInput, TOutput> Create(Uri streamUri, object state = null);
    }

    public interface IReliableQubjectFactory<TInput, TOutput, TArgs> : IReliableReactiveSubjectFactory<TInput, TOutput, TArgs>, IReliableReactiveExpressible
    {
        new IReliableMultiQubject<TInput, TOutput> Create(Uri streamUri, TArgs argument, object state = null);
    }
}
