// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Shebang.Extensions
{
    //
    // Example of a stateless observer, just using the Rx IObserver<T> interface.
    //

    internal sealed class ConsoleObserver<T> : IObserver<T>
    {
        public void OnCompleted() => Console.WriteLine("OnCompleted()");

        public void OnError(Exception error) => Console.WriteLine($"OnError({error})");

        public void OnNext(T value) => Console.WriteLine($"OnNext({value})");
    }
}
