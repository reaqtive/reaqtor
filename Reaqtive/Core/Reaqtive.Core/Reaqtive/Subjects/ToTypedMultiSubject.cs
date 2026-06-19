// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    internal class ToTypedMultiSubject<TInput, TOutput> : IMultiSubject<TInput, TOutput>
    {
        private readonly IMultiSubject _innerSubject;

        public ToTypedMultiSubject(IMultiSubject innerSubject)
        {
            _innerSubject = innerSubject;
        }

        public IObserver<TInput> CreateObserver()
        {
            return _innerSubject.GetObserver<TInput>();
        }

        public ISubscription Subscribe(IObserver<TOutput> observer)
        {
            return _innerSubject.GetObservable<TOutput>().Subscribe(observer);
        }

        IDisposable IObservable<TOutput>.Subscribe(IObserver<TOutput> observer)
        {
            return Subscribe(observer);
        }

        public void Dispose()
        {
            _innerSubject.Dispose();
        }
    }
}
