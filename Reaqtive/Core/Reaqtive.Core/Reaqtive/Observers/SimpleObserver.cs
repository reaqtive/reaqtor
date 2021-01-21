// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive
{
    internal class SimpleObserver<TSource> : ObserverBase<TSource>
    {
        private readonly Action<TSource> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        public SimpleObserver(Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            Debug.Assert(onNext != null);
            Debug.Assert(onError != null);
            Debug.Assert(onCompleted != null);

            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        protected override void OnCompletedCore()
        {
            _onCompleted();
        }

        protected override void OnErrorCore(Exception error)
        {
            _onError(error);
        }

        protected override void OnNextCore(TSource value)
        {
            _onNext(value);
        }
    }
}
