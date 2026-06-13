// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Sink implementation for fused operators to be tied to disposable resources.
//
// BD - October 2014
//

using System;
using System.Threading;

namespace RuntimeLib
{
    //
    // NB: Not CLS compliant due to fields starting with underscore. This type has to be public for runtime code-generation,
    //     but the type isn't targeted by end users.
    //

    [CLSCompliant(false)]
    public class Sink<T> : IDisposable
    {
#pragma warning disable CA1051 // Do not declare visible instance fields. (By design; runtime-generated code uses these fields.)
        protected IObserver<T> _observer;
        protected IDisposable _disposable;
#pragma warning restore CA1051

        public Sink(IObserver<T> observer, IDisposable disposable)
        {
            _observer = observer;
            _disposable = disposable;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var res = Interlocked.Exchange(ref _disposable, Sentinels.HasDisposed);
                if (res != Sentinels.HasDisposed)
                {
                    _observer = NopObserver<T>.Instance;
                    res.Dispose();
                }
            }
        }
    }
}
