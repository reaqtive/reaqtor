// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Ad-hoc clone of Observer from Rx.
//
// Can be replaced by using the existing IRP equivalent library.
//
// BD - September 2014
//

using System;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Provides a set of methods to construct observers.
    /// </summary>
    internal static class Observer
    {
        /// <summary>
        /// Creates an observer using the specified method implementation delegates.
        /// </summary>
        /// <typeparam name="T">Type of the elements processed by the observer.</typeparam>
        /// <param name="onNext">Implementation of the observer's OnNext method.</param>
        /// <param name="onError">Implementation of the observer's OnError method.</param>
        /// <param name="onCompleted">Implementation of the observer's OnCompleted method.</param>
        /// <returns>Observer implementation using the specified delegates.</returns>
        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            return new AnonymousObserver<T>(onNext, onError, onCompleted);
        }

        /// <summary>
        /// Anonymous implementation of an observer.
        /// </summary>
        /// <typeparam name="T">Type of the elements processed by the observer.</typeparam>
        private class AnonymousObserver<T> : IObserver<T>
        {
            private readonly Action<T> _onNext;
            private readonly Action<Exception> _onError;
            private readonly Action _onCompleted;

            public AnonymousObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }

            public void OnCompleted()
            {
                // TODO: omitted terminal behavior; use Rx

                _onCompleted();
            }

            public void OnError(Exception error)
            {
                // TODO: omitted terminal behavior; use Rx

                _onError(error);
            }

            public void OnNext(T value)
            {
                _onNext(value);
            }
        }
    }
}
