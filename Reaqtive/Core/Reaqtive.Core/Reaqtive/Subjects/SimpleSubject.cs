// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

namespace Reaqtive
{
    /// <summary>
    /// Subject with a single observer.
    /// </summary>
    /// <typeparam name="T">Type of the elements processed by the subject.</typeparam>
    public class SimpleSubject<T> : IMultiSubject<T>, IObserver<T>
    {
        private volatile IObserver<T> _observer = Sentinels<T>.Nop;

        /// <summary>
        /// Obtains the single observer used to push elements into the subject.
        /// </summary>
        /// <returns>Observer used to push elements into the subject.</returns>
        public virtual IObserver<T> CreateObserver()
        {
            return this;
        }

        /// <summary>
        /// Subscribes the specified observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <returns>Subscription handle representing the observer's subscription to the subject.</returns>
        public virtual ISubscription Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            IObserver<T> old, @new;
            do
            {
                old = _observer;

                if (old == Sentinels<T>.Disposed)
                {
                    throw new ObjectDisposedException("this");
                }

                if (old == Sentinels<T>.Nop)
                {
                    @new = observer;
                }
                else
                {
                    if (old is ListObserver<T> multi)
                    {
                        @new = multi.Add(observer);
                    }
                    else
                    {
                        @new = new ListObserver<T>(old, observer);
                    }
                }
            } while (Interlocked.CompareExchange(ref _observer, @new, old) != old);
#pragma warning restore

            return new Subscription(this, observer);
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            return Subscribe(observer);
        }

        /// <summary>
        /// Pushes a completion notification into the subject.
        /// </summary>
        public virtual void OnCompleted()
        {
            _observer.OnCompleted();
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        /// <summary>
        /// Pushes the specified error into the subject.
        /// </summary>
        /// <param name="error">Error to push into the subject.</param>
        public virtual void OnError(Exception error)
        {
            _observer.OnError(error);
        }

#pragma warning restore CA1716

        /// <summary>
        /// Pushes the specified value into the subject.
        /// </summary>
        /// <param name="value">Value to push into the subject.</param>
        public virtual void OnNext(T value)
        {
            _observer.OnNext(value);
        }

        /// <summary>
        /// Disposes the subject.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">true if called from <see cref="IDisposable.Dispose"/>; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _observer = Sentinels<T>.Disposed;
            }
        }

        private void Unsubscribe(IObserver<T> observer)
        {
            IObserver<T> old, @new;
            do
            {
                old = _observer;

                if (old == Sentinels<T>.Disposed)
                {
                    return; // TODO: should we be less tolerant here?
                }

                if (old == Sentinels<T>.Nop)
                {
                    throw new InvalidOperationException("Observer not found.");
                }
                else
                {
                    if (old is ListObserver<T> multi)
                    {
                        @new = multi.Remove(observer);
                    }
                    else
                    {
                        if (old != observer)
                        {
                            throw new InvalidOperationException("Observer not found.");
                        }

                        @new = Sentinels<T>.Nop;
                    }
                }
            } while (Interlocked.CompareExchange(ref _observer, @new, old) != old);
#pragma warning restore
        }

        private sealed class Subscription : ISubscription
        {
            private SimpleSubject<T> _parent;
            private IObserver<T> _observer;

            public Subscription(SimpleSubject<T> parent, IObserver<T> observer)
            {
                _parent = parent;
                _observer = observer;
            }

            public void Accept(ISubscriptionVisitor visitor)
            {
            }

            public void Dispose()
            {
                var parent = Interlocked.Exchange(ref _parent, null);
                if (parent != null)
                {
                    var observer = _observer;
                    _observer = null;

                    parent.Unsubscribe(observer);
                }
            }
        }
    }
}
