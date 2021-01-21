// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

//
// NB: This file contains a port of Rx code that should eventually get removed.
//

using System;
using System.Collections.Generic;
using System.Threading;
using Reaqtive.Disposables;

namespace Reaqtive.Linq
{
    /// <summary>
    /// Provides a set of factory methods for <see cref="IObservable{T}"/> objects.
    /// </summary>
    public static class Observable
    {
        /// <summary>
        /// Creates an observable from the given <paramref name="subscribe"/> method implementation.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="subscribe">The implementation of the <see cref="IObservable{T}.Subscribe(IObserver{T})"/> method.</param>
        /// <returns>An observable instance.</returns>
        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe) => new AnonymousObservable<T>(subscribe);

        /// <summary>
        /// Materializes the notifications in the specified observable sequence as <see cref="Notification{T}"/> instances.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source observable sequence whose notifications to materialize.</param>
        /// <returns>An observable sequence containing materialized notifications.</returns>
        public static IObservable<Notification<T>> Materialize<T>(this IObservable<T> source) => Create<Notification<T>>(observer => source.Subscribe(
            x => observer.OnNext(Notification.CreateOnNext(x)),
            ex => { observer.OnNext(Notification.CreateOnError<T>(ex)); observer.OnCompleted(); },
            () => { observer.OnNext(Notification.CreateOnCompleted<T>()); observer.OnCompleted(); }
        ));

        /// <summary>
        /// Creates an observable that produces a single <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="value">The single value produced on the sequence.</param>
        /// <returns>An observable instance.</returns>
        public static IObservable<T> Return<T>(T value) => Create<T>(observer =>
        {
            observer.OnNext(value);
            observer.OnCompleted();

            return Disposable.Empty;
        });

        /// <summary>
        /// Converts the specified observable sequence to an enumerable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The enumerable sequence to convert.</param>
        /// <returns>An enumerable sequence yielding the elements produced by the specified observable sequence.</returns>
        public static IEnumerable<T> ToEnumerable<T>(this IObservable<T> source)
        {
            var xs = new List<T>();
            var e = new ManualResetEvent(false);
            var error = default(Exception);

            var d = source.Subscribe(xs.Add, ex => { error = ex; e.Set(); }, () => { e.Set(); });

            e.WaitOne();

            foreach (var x in xs)
            {
                yield return x;
            }

            if (error != null)
            {
                throw error;
            }
        }

        private sealed class AnonymousObservable<T> : IObservable<T>
        {
            private readonly Func<IObserver<T>, IDisposable> _subscribe;

            public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe)
            {
                _subscribe = subscribe;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                try
                {
                    return _subscribe(observer);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return Disposable.Empty;
                }
            }
        }
    }
}
