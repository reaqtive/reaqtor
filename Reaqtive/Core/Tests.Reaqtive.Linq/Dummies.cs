// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Test.Reaqtive
{
    internal static class DummyFunc<T>
    {
        public static readonly Func<T> Instance = () => { throw new NotImplementedException(); };
    }

    internal static class DummyFunc<T, U>
    {
        public static readonly Func<T, U> Instance = t => { throw new NotImplementedException(); };
    }

    internal static class DummyFunc<T, U, V>
    {
        public static readonly Func<T, U, V> Instance = (t, u) => { throw new NotImplementedException(); };
    }

    internal static class DummyAction
    {
        public static readonly Action Instance = () => { throw new NotImplementedException(); };
    }

    internal static class DummyAction<T>
    {
        public static readonly Action<T> Instance = t => { throw new NotImplementedException(); };
    }

    internal static class DummyAction<T, U>
    {
        public static readonly Action<T, U> Instance = (t, u) => { throw new NotImplementedException(); };
    }

    internal sealed class DummyObserver<T> : IObserver<T>
    {
        public static readonly DummyObserver<T> Instance = new();

        private DummyObserver()
        {
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            throw new NotImplementedException();
        }
    }

    internal sealed class DummySubscribable<T> : ISubscribable<T>
    {
        public static readonly DummySubscribable<T> Instance = new();

        private DummySubscribable()
        {
        }

        public ISubscription Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}
