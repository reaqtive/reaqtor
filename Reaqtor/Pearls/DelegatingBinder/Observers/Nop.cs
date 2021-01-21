// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive;

namespace DelegatingBinder
{
    internal sealed class Nop<T> : ObserverBase<T>
    {
        protected override void OnCompletedCore()
        {
        }

        protected override void OnErrorCore(Exception error)
        {
        }

        protected override void OnNextCore(T value)
        {
        }
    }
}
