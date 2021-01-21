// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Reliable
{
    public interface IReliableSubjectFactory<TInput, TOutput>
    {
        IReliableMultiSubject<TInput, TOutput> Create();
    }

    public interface IReliableSubjectFactory<TInput, TOutput, TArgs>
    {
        IReliableMultiSubject<TInput, TOutput> Create(TArgs argument);
    }
}
