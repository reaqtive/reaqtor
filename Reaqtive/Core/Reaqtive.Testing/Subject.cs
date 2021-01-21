// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// NB: This file contains a port of Rx code that should eventually get removed.
//

using System;
using System.Collections.Generic;
using Reaqtive.Disposables;

namespace Reaqtive.Subjects
{
    /// <summary>
    /// Subject to produce and consume events by zero or more observers.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public sealed class Subject<T> : IObservable<T>, IObserver<T>, IDisposable
    {
        private readonly object _lock = new();
        private readonly List<IObserver<T>> _observers = new();
        private bool _done;
        private Exception _error;
        private bool _disposed;

        /// <summary>
        /// Disposes the subject.
        /// </summary>
        public void Dispose()
        {
            lock (_lock)
            {
                _disposed = true;

                _observers.Clear();
            }
        }

        /// <summary>
        /// Sends an <see cref="IObserver{T}.OnCompleted"/> notification to all subscribed observers.
        /// </summary>
        public void OnCompleted()
        {
            lock (_lock)
            {
                CheckDisposed();

                try
                {
                    foreach (var observer in _observers.ToArray()) // NB: Snapshot because reentrant lock can mutate collection (e.g. due to dispose).
                    {
                        observer.OnCompleted();
                    }
                }
                finally
                {
                    _observers.Clear();
                    _done = true;
                }
            }
        }

        /// <summary>
        /// Sends an <see cref="IObserver{T}.OnError(Exception)"/> notification to all subscribed observers.
        /// </summary>
        /// <param name="error">The error to send to all observers.</param>
        public void OnError(Exception error)
        {
            lock (_lock)
            {
                CheckDisposed();

                try
                {
                    foreach (var observer in _observers.ToArray()) // NB: Snapshot because reentrant lock can mutate collection (e.g. due to dispose).
                    {
                        observer.OnError(error);
                    }
                }
                finally
                {
                    _observers.Clear();
                    _error = error;
                    _done = true;
                }
            }
        }

        /// <summary>
        /// Sends an <see cref="IObserver{T}.OnNext(T)"/> notification to all subscribed observers.
        /// </summary>
        /// <param name="value">The value to send to all observers.</param>
        public void OnNext(T value)
        {
            lock (_lock)
            {
                CheckDisposed();

                foreach (var observer in _observers.ToArray()) // NB: Snapshot because reentrant lock can mutate collection (e.g. due to dispose).
                {
                    observer.OnNext(value);
                }
            }
        }

        /// <summary>
        /// Subscribes the given <paramref name="observer"/> to the subject.
        /// </summary>
        /// <param name="observer">The observer used to receive notification from the subject.</param>
        /// <returns>A disposable object used to unsubscribe the observer from the subject.</returns>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            lock (_lock)
            {
                CheckDisposed();

                if (_done)
                {
                    if (_error != null)
                    {
                        observer.OnError(_error);
                    }
                    else
                    {
                        observer.OnCompleted();
                    }

                    return Disposable.Empty;
                }

                _observers.Add(observer);

                return Disposable.Create(() =>
                {
                    lock (_lock)
                    {
                        var i = _observers.LastIndexOf(observer);
                        if (i >= 0)
                        {
                            _observers.RemoveAt(i);
                        }
                    }
                });
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("this");
        }
    }
}
