// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Reliable
{
    public interface IReliableMultiSubject : IDisposable
    {
    }

    public interface IReliableMultiSubject<in TInput, out TOutput> : IReliableMultiSubject, IReliableObservable<TOutput>
    {
        IReliableObserver<TInput> CreateObserver();
    }

    public interface IReliableMultiSubject<T> : IReliableMultiSubject<T, T>
    {
    }
}
