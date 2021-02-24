// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    internal /* <DontEventThinkAbout> */ /* public */ /* </DontEventThinkAbout> */ static class ReliableSentinels<T>
    {
        public static readonly IReliableObserver<T> Disposed = new ReliableFaultObserver<T>(() => new ObjectDisposedException("this"));

        private sealed class ReliableFaultObserver<TSource> : IReliableObserver<TSource>
        {
            private readonly Func<Exception> _getFault;

            public ReliableFaultObserver(Func<Exception> getFault)
            {
                Debug.Assert(getFault != null);
                _getFault = getFault;
            }

            public void OnCompleted() => throw _getFault();

            public void OnError(Exception error) => throw _getFault();

            public void OnNext(TSource value, long sequenceId) => throw _getFault();

            public Uri ResubscribeUri => throw _getFault();

            public void OnStarted() => throw _getFault();
        }
    }
}
