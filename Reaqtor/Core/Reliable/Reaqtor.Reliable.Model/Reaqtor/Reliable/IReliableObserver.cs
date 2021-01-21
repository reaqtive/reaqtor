// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Reliable
{
    public interface IReliableObserver<in T>
    {
        Uri ResubscribeUri { get; }

        void OnNext(T item, long sequenceId);
        void OnStarted();

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)
        void OnError(Exception error);
#pragma warning restore CA1716

        void OnCompleted();
    }
}
