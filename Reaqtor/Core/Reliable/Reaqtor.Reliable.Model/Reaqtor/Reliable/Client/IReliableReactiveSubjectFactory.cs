// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Reliable.Client
{
    public interface IReliableReactiveSubjectFactory<TInput, TOutput>
    {
        IReliableReactiveMultiSubject<TInput, TOutput> Create(Uri uri, object state = null);
    }

    public interface IReliableReactiveSubjectFactory<TInput, TOutput, TArgs>
    {
        IReliableReactiveMultiSubject<TInput, TOutput> Create(Uri uri, TArgs argument, object state = null);
    }
}
