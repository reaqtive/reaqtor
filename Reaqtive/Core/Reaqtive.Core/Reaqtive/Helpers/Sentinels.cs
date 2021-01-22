// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    internal /* <DontEventThinkAbout> */ /* public */ /* </DontEventThinkAbout> */ static class Sentinels<T>
    {
        public static readonly IObserver<T> Disposed = new FaultObserver<T>(() => new ObjectDisposedException("this"));
        public static readonly IObserver<T> Nop = new NopObserver<T>();
    }
}
