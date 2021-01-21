// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive;

#pragma warning disable CA1303 // Do not pass literals as localized parameters. (No localization in sample code.)

namespace DelegatingBinder
{
    internal sealed class Cout<T> : ObserverBase<T>
    {
        protected override void OnCompletedCore()
        {
            Console.WriteLine("OnCompleted");
        }

        protected override void OnErrorCore(Exception error)
        {
            Console.WriteLine("OnError({0})", error);
        }

        protected override void OnNextCore(T value)
        {
            Console.WriteLine("OnNext({0})", value);
        }
    }
}
