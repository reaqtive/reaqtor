// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Ad-hoc clone of Observable query operators from Rx.
//
// Can be replaced by using the existing IRP equivalent library.
// Notice this implementation has KnownResourceAttribute annotations and bundles client and service aspects for sake of simplicity.
//
// BD - September 2014
//

using System;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Provides a set of query operators for observable sequences.
    /// </summary>
    internal static class Observable
    {
        /// <summary>
        /// Creates a new observable sequence implementation using the specified subscribe method delegate.
        /// </summary>
        /// <typeparam name="T">Element type of the resulting sequence.</typeparam>
        /// <param name="subscribe">Subscribe method delegate.</param>
        /// <returns>Observable sequence using the specified subscribe method delegate implementation.</returns>
        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe)
        {
            return new AnonymousObservable<T>(subscribe);
        }

        /// <summary>
        /// Filters the elements in the specified source sequence using the specified predicate.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to filter.</param>
        /// <param name="predicate">Predicate function to filter elements in the source sequence.</param>
        /// <returns>Observable sequence with the source sequence elements that passed the specified predicate.</returns>
        [KnownResource("rx://operators/filter")]
        public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            return Create<T>(observer =>
            {
                return source.Subscribe(
                    Observer.Create<T>(
                        x =>
                        {
                            if (predicate(x)) // TODO: exception case omitted; use Rx
                            {
                                observer.OnNext(x);
                            }
                        },
                        observer.OnError,
                        observer.OnCompleted
                    )
                );
            });
        }

        /// <summary>
        /// Projects the elements in the specified source sequence using the specified selector.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="R">Type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to project.</param>
        /// <param name="selector">Selector function to project source sequence elements onto result sequence elements.</param>
        /// <returns>Observable sequence with the source sequence elements projected using the specified selector.</returns>
        [KnownResource("rx://operators/map")]
        public static IObservable<R> Select<T, R>(this IObservable<T> source, Func<T, R> selector)
        {
            return Create<R>(observer =>
            {
                return source.Subscribe(
                    Observer.Create<T>(
                        x =>
                        {
                            var res = selector(x); // TODO: exception case omitted; use Rx
                            observer.OnNext(res);
                        },
                        observer.OnError,
                        observer.OnCompleted
                    )
                );
            });
        }

        /// <summary>
        /// Takes the specified number of elements from the start of the specified source sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take the specified number of elements from.</param>
        /// <param name="count">Number of elements to take.</param>
        /// <returns>Observable sequence with the first <paramref name="count"/> elements from the source sequence, or all of the source sequence's elements if it has less than <paramref name="count"/> elements.</returns>
        [KnownResource("rx://operators/take")]
        public static IObservable<T> Take<T>(this IObservable<T> source, int count)
        {
            // TODO: omitted case of count == 0, replace using Rx

            return Create<T>(observer =>
            {
                var n = count;

                var d = default(IDisposable); // TODO: omitted proper SingleAssignmentDisposable or phasing, replace using Rx or ISubscribable<T>
                d = source.Subscribe(
                    Observer.Create<T>(
                         x =>
                         {
                             // TODO: omitted reentrancy protection, replace using Rx
                             observer.OnNext(x);

                             if (--n == 0)
                             {
                                 observer.OnCompleted();
                                 d.Dispose();
                             }
                         },
                         observer.OnError,
                         observer.OnCompleted
                     )
                 );

                return d;
            });
        }

        /// <summary>
        /// Anonymous implementation of an observable.
        /// </summary>
        /// <typeparam name="T">Type of the elements produced by the observable sequence.</typeparam>
        private class AnonymousObservable<T> : IObservable<T>
        {
            private readonly Func<IObserver<T>, IDisposable> _create;

            public AnonymousObservable(Func<IObserver<T>, IDisposable> create)
            {
                _create = create;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                return _create(observer);
            }
        }
    }
}
