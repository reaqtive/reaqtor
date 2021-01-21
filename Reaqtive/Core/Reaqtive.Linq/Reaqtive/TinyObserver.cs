// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    internal abstract class TinyObserver<T> : ObserverBase<T>
    {
        protected override void OnCompletedCore()
        {
            throw new InvalidOperationException();
        }

        protected override void OnErrorCore(Exception error)
        {
            throw new InvalidOperationException();
        }
    }
}
