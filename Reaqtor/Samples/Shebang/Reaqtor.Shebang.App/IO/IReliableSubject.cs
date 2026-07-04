// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Shebang.App;

#pragma warning disable CA1515 // Consider making public types internal. (Deliberate: the sample notebooks reference the built Reaqtor.Shebang.App assembly and use this type.)
public interface IReliableSubject<T> : IObservable<(long sequenceId, T item)>, IObserver<(long sequenceId, T item)>
{
}
#pragma warning restore CA1515

