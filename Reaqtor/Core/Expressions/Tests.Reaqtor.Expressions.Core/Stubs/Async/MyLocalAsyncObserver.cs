// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;

namespace Tests.Reaqtor.Expressions.Core
{
    public sealed class MyLocalAsyncObserver<T> : AsyncReactiveObserverBase<T>
    {
        protected override Task OnNextAsyncCore(T value, CancellationToken token) => throw new NotImplementedException();

        protected override Task OnErrorAsyncCore(Exception error, CancellationToken token) => throw new NotImplementedException();

        protected override Task OnCompletedAsyncCore(CancellationToken token) => throw new NotImplementedException();
    }
}
