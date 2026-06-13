// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Trivial disposable to reveal Dispose calls.
//
// BD - October 2014
//

using System;

namespace OperatorFusion
{
    internal sealed class Disposable : IDisposable
    {
        private readonly bool _mute;

        public Disposable(bool mute)
        {
            _mute = mute;
        }

        public void Dispose()
        {
            if (!_mute)
                Console.WriteLine("Disposed!");
        }
    }
}
