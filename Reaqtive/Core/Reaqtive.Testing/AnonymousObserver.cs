// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

//
// NB: This file contains a port of Rx code that should eventually get removed.
//

using System;

namespace Reaqtive
{
    internal sealed class AnonymousObserver<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted) : IObserver<T>
    {
        private readonly Action<T> _onNext = onNext;
        private readonly Action<Exception> _onError = onError;
        private readonly Action _onCompleted = onCompleted;

        public void OnCompleted() => _onCompleted();

        public void OnError(Exception error) => _onError(error);

        public void OnNext(T value) => _onNext(value);
    }
}
