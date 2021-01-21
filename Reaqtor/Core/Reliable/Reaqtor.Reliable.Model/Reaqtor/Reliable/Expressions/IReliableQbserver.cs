// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Reliable.Client;

namespace Reaqtor.Reliable.Expressions
{
    public interface IReliableQbserver : IReliableReactiveExpressible
    {
        Type ElementType { get; }
    }

    public interface IReliableQbserver<in T> : IReliableReactiveObserver<T>, IReliableQbserver
    {
    }
}
