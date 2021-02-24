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
    public class Sink<T> : IDisposable
    {
        protected IObserver<T> _observer;
        protected IDisposable _disposable;

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
