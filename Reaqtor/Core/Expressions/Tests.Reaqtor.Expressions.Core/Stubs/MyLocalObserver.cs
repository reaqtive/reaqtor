// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;

using Reaqtor;

namespace Tests.Reaqtor.Expressions.Core
{
    public sealed class MyLocalObserver<T> : ReactiveObserverBase<T>
    {
        protected override void OnNextCore(T value)
        {
            throw new NotImplementedException();
        }

        protected override void OnErrorCore(Exception error)
        {
            throw new NotImplementedException();
        }

        protected override void OnCompletedCore()
        {
            throw new NotImplementedException();
        }
    }
}
