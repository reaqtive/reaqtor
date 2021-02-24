// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Reaqtor
{
    /// <summary>
    /// Interface for subjects representing event streams that can produce and receive data.
    /// </summary>
    /// <typeparam name="T">Type of the data received and produced by the subject.</typeparam>
    public interface IAsyncReactiveSubject<T> : IAsyncReactiveSubject<T, T>
    {
    }

    /// <summary>
    /// Interface for subjects representing event streams that can produce and receive data.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
    public interface IAsyncReactiveSubject<in TInput, out TOutput> : IAsyncReactiveObserver<TInput>, IAsyncReactiveObservable<TOutput>, IAsyncDisposable
    {
    }
}
