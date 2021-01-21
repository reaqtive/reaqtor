// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Shebang.App
{
    public interface IReliableSubject<T> : IObservable<(long sequenceId, T item)>, IObserver<(long sequenceId, T item)>
    {
    }
}
