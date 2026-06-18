// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.IoT
{
    // Avoids taking dependency on Rx

    public static class MiniRx
    {
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
        {
            return source.Subscribe(new AnonymousObserver<T>(onNext, _ => { }, () => { }));
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            return source.Subscribe(new AnonymousObserver<T>(onNext, onError, onCompleted));
        }

        private sealed class AnonymousObserver<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted) : IObserver<T>
        {
            private readonly Action<T> _onNext = onNext;
            private readonly Action<Exception> _onError = onError;
            private readonly Action _onCompleted = onCompleted;

            public void OnCompleted() => _onCompleted();

            public void OnError(Exception error) => _onError(error);

            public void OnNext(T value) => _onNext(value);
        }
    }
}
