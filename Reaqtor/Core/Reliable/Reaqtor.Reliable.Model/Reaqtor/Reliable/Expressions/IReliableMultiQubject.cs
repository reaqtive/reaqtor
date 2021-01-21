// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Reliable.Client;

namespace Reaqtor.Reliable.Expressions
{
    public interface IReliableMultiQubject : IReliableReactiveExpressible, IReliableQbservable
    {
    }

    public interface IReliableMultiQubject<in TInput, out TOutput> : IReliableReactiveMultiSubject<TInput, TOutput>, IReliableQbservable<TOutput>, IReliableMultiQubject
    {
        new IReliableQbserver<TInput> CreateObserver();
    }
}
