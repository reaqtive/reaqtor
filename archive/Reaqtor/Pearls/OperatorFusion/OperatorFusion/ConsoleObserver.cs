// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Trivial console observer.
//
// BD - October 2014
//

using System;

namespace OperatorFusion
{
    internal sealed class ConsoleObserver<T> : IObserver<T>
    {
        private readonly bool _mute;

        public ConsoleObserver(bool mute)
        {
            _mute = mute;
        }

        public void OnCompleted()
        {
            if (!_mute)
                Console.WriteLine("Done");
        }

        public void OnError(Exception error)
        {
            if (!_mute)
                Console.WriteLine(error.Message);
        }

        public void OnNext(T value)
        {
            if (!_mute)
                Console.WriteLine(value);
        }
    }
}
