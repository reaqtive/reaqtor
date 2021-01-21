// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Ad-hoc clone of Subject<T> from Rx.
//
// Can be replaced by using the existing IRP equivalent library.
//
// BD - September 2014
//

#define NO_LOG

using System;
using System.Collections.Generic;
using System.Threading;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Simple stateless fire-and-forget subject implementation.
    /// </summary>
    /// <typeparam name="T">Type of elements processed by this subject.</typeparam>
    internal class Subject<T> : IObservable<T>, IObserver<T>
    {
        // TODO: omitted locking; should use the well-known immutable list design (or use Rx's subject as-is)

        /// <summary>
        /// List of observers subscribed to the subject.
        /// </summary>
        private readonly List<IObserver<T>> _observers = new();

        /// <summary>
        /// Identifier of te subject.
        /// </summary>
        private readonly string _id;

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        /// <param name="id">Optional identifier used for logging.</param>
        public Subject(string id = "")
        {
            _id = id;
        }

        /// <summary>
        /// Returns a friendly string representation of the subject.
        /// </summary>
        /// <returns>String representation of the subject.</returns>
        public override string ToString()
        {
            return "Subject<" + typeof(T).Name + ">(" + _id + ")";
        }

        /// <summary>
        /// Subscribes the specified observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to receive the subject's elements on.</param>
        /// <returns>Disposable resource used to cancel the subscription.</returns>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            _observers.Add(observer);

            LogSubCount();

            return new Disposable(this, observer);
        }

        /// <summary>
        /// Disposable resource used to unsubscribe an observer from the parent subject.
        /// </summary>
        private class Disposable : IDisposable
        {
            /// <summary>
            /// Subject owning the subscription using the specified observer.
            /// </summary>
            private Subject<T> _subject;

            /// <summary>
            /// Observer subscribed to the subject.
            /// </summary>
            private IObserver<T> _observer;

            /// <summary>
            /// Creates a new disposable resource that can be used to unsubscribe the specified observer from the specified subject.
            /// </summary>
            /// <param name="subject">Subject owning the subscription using the specified observer.</param>
            /// <param name="observer">Observer subscribed to the subject.</param>
            public Disposable(Subject<T> subject, IObserver<T> observer)
            {
                _subject = subject;
                _observer = observer;
            }

            /// <summary>
            /// Disposes the subscription of the observer to the subject.
            /// </summary>
            public void Dispose()
            {
                var s = Interlocked.Exchange(ref _subject, null);
                if (s != null)
                {
                    s.Remove(_observer);
                    _observer = null;
                }
            }
        }

        /// <summary>
        /// Helper method to remove the last occurrence of the specified observer from the observers list.
        /// </summary>
        /// <param name="observer">Observer to remove.</param>
        private void Remove(IObserver<T> observer)
        {
            var i = _observers.LastIndexOf(observer);
            if (i >= 0)
            {
                _observers.RemoveAt(i);
            }

            LogSubCount();
        }

        /// <summary>
        /// Logs the number of subscriptions currently active on the subject.
        /// </summary>
        private void LogSubCount()
        {
            var n = _observers.Count;

#if DEBUG && !NO_LOG
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(ToString() + ".SubscriptionCount = " + n);
            Console.ResetColor();
#endif
        }

        /// <summary>
        /// Publishes a completion notification into the subject.
        /// </summary>
        public void OnCompleted()
        {
            // TODO: omitted terminal behavior

            foreach (var o in _observers)
            {
                o.OnCompleted();
            }
        }

        /// <summary>
        /// Publishes an error into the subject.
        /// </summary>
        /// <param name="error">Error to publish.</param>
        public void OnError(Exception error)
        {
            // TODO: omitted terminal behavior

            foreach (var o in _observers)
            {
                o.OnError(error);
            }
        }

        /// <summary>
        /// Publishes an element into the subject.
        /// </summary>
        /// <param name="value">Element to publish.</param>
        public void OnNext(T value)
        {
            foreach (var o in _observers)
            {
                o.OnNext(value);
            }
        }
    }
}
