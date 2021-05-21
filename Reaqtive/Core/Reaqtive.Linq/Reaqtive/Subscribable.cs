// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive.Operators;

namespace Reaqtive
{
    /// <summary>
    /// Provides a set of query operators for subscribable sequences.
    /// </summary>
    public static partial class Subscribable
    {
        #region Helpers

        private static readonly Action s_nop = () => { };
        private static readonly Action<Exception> s_ignoreError = _ => { };

        #endregion

        #region Aggregates

        #region Aggregate

        /// <summary>
        /// Aggregates the elements in the source sequence using the specified aggregation function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to aggregate.</param>
        /// <param name="aggregate">Aggregation function used to combine the running aggregation result with each consecutive element.</param>
        /// <returns>Observable sequence containing a single element representing the result of the aggregation. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        public static ISubscribable<TSource> Aggregate<TSource>(this ISubscribable<TSource> source, Func<TSource, TSource, TSource> aggregate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            return new Aggregate<TSource>(source, aggregate);
        }

        /// <summary>
        /// Aggregates the elements in the source sequence starting from an initial seed value and using the specified aggregation function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result of the aggregation.</typeparam>
        /// <param name="source">Source sequence whose elements to aggregate.</param>
        /// <param name="seed">Seed value of the aggregation.</param>
        /// <param name="accumulate">Aggregation function used to combine the running aggregation result with each consecutive element.</param>
        /// <returns>Observable sequence containing a single element representing the result of the aggregation.</returns>
        public static ISubscribable<TResult> Aggregate<TSource, TResult>(this ISubscribable<TSource> source, TResult seed, Func<TResult, TSource, TResult> accumulate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulate == null)
                throw new ArgumentNullException(nameof(accumulate));

            return new Aggregate<TSource, TResult>(source, seed, accumulate);
        }

        /// <summary>
        /// Aggregates the elements in the source sequence starting from an initial seed value, using the specified aggregation function, and using a selector function to project the result.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TAccumulate">Type of the intermediate aggregation values.</typeparam>
        /// <typeparam name="TResult">Type of the result of the aggregation.</typeparam>
        /// <param name="source">Source sequence whose elements to aggregate.</param>
        /// <param name="seed">Seed value of the aggregation.</param>
        /// <param name="accumulate">Aggregation function used to combine the running aggregation result with each consecutive element.</param>
        /// <param name="resultSelector">Selector function to apply to the resulting intermediate aggregation value.</param>
        /// <returns>Observable sequence containing a single element representing the result of the aggregation.</returns>
        public static ISubscribable<TResult> Aggregate<TSource, TAccumulate, TResult>(this ISubscribable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulate, Func<TAccumulate, TResult> resultSelector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Aggregate(seed, accumulate).Select(resultSelector);
        }

        #endregion

        #region All

        /// <summary>
        /// Aggregates the source sequence to determine whether all of the elements match the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to match with the predicate.</param>
        /// <param name="predicate">Predicate function to apply to each element of the source sequence. Upon encountering the first element that doesn't pass the predicate, the aggregation operation will stop.</param>
        /// <returns>Observable sequence containing a single element with value <c>true</c> if all elements in the source sequence match the predicate; otherwise a single element with value <c>false</c>. If the source sequence is empty, an element with value <c>true</c> is returned.</returns>
        public static ISubscribable<bool> All<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new All<TSource>(source, predicate);
        }

        #endregion

        #region Any

        /// <summary>
        /// Aggregates the source sequence to determine whether it contains any element.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to check for the presence of elements.</param>
        /// <returns>Observable sequence containing a single element with value <c>true</c> if the source sequence is not empty; otherwise a single element with value <c>false</c>.</returns>
        public static ISubscribable<bool> Any<TSource>(this ISubscribable<TSource> source)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Any(_ => true);
        }

        /// <summary>
        /// Aggregates the source sequence to determine whether any of the elements matches the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to match with the predicate.</param>
        /// <param name="predicate">Predicate function to apply to each element of the source sequence. Upon encountering the first element that does pass the predicate, the aggregation operation will stop.</param>
        /// <returns>Observable sequence containing a single element with value <c>true</c> if any element in the source sequence matches the predicate; otherwise a single element with value <c>false</c>. If the source sequence is empty, an element with value <c>false</c> is returned.</returns>
        public static ISubscribable<bool> Any<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new Any<TSource>(source, predicate);
        }

        #endregion

        #region Count

        /// <summary>
        /// Aggregates the source sequence to count the number of elements. The result is represented as a 32-bit integer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to count.</param>
        /// <returns>Observable sequence containing a single element with an integer value representing the number of elements in the sequence. If the source sequence is empty, an element with value 0 is returned.</returns>
        public static ISubscribable<int> Count<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new Count<TSource>(source);
        }

        /// <summary>
        /// Aggregates the source sequence to count the number of elements that match the specified predicate. The result is represented as a 32-bit integer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to count based on the specified predicate.</param>
        /// <param name="predicate">Predicate function to apply to each element of the source sequence. Elements that match the predicate will be counted.</param>
        /// <returns>Observable sequence containing a single element with an integer value representing the number of elements in the sequence that match the predicate. If the source sequence does not contain any elements matching the predicate, an element with value 0 is returned.</returns>
        public static ISubscribable<int> Count<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Where(predicate).Count();
        }

        #endregion

        #region IsEmpty

        /// <summary>
        /// Aggregates the source sequence to determine whether it is empty.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to check for the presence of elements.</param>
        /// <returns>Observable sequence containing a single element with value <c>true</c> if the source sequence is empty; otherwise a single element with value <c>false</c>.</returns>
        public static ISubscribable<bool> IsEmpty<TSource>(this ISubscribable<TSource> source)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Any().Select(b => !b);
        }

        #endregion

        #region LongCount

        /// <summary>
        /// Aggregates the source sequence to count the number of elements. The result is represented as a 64-bit integer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to count.</param>
        /// <returns>Observable sequence containing a single element with an integer value representing the number of elements in the sequence. If the source sequence is empty, an element with value 0 is returned.</returns>
        public static ISubscribable<long> LongCount<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new LongCount<TSource>(source);
        }

        /// <summary>
        /// Aggregates the source sequence to count the number of elements that match the specified predicate. The result is represented as a 64-bit integer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to count based on the specified predicate.</param>
        /// <param name="predicate">Predicate function to apply to each element of the source sequence. Elements that match the predicate will be counted.</param>
        /// <returns>Observable sequence containing a single element with an integer value representing the number of elements in the sequence that match the predicate. If the source sequence does not contain any elements matching the predicate, an element with value 0 is returned.</returns>
        public static ISubscribable<long> LongCount<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return source.Where(predicate).LongCount();
        }

        #endregion

        #region Scan

        /// <summary>
        /// Produces a sequence with a rolling aggregates of the elements in the source sequence using the specified aggregation function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to aggregate.</param>
        /// <param name="aggregate">Aggregation function used to combine the running aggregation result with each consecutive element.</param>
        /// <returns>Observable sequence containing elements obtained from applying the rolling aggregate to the source sequence. If the source sequence is empty, an empty sequence is returned.</returns>
        public static ISubscribable<TSource> Scan<TSource>(this ISubscribable<TSource> source, Func<TSource, TSource, TSource> aggregate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            return new Scan<TSource>(source, aggregate);
        }

        /// <summary>
        /// Produces a sequence with a rolling aggregates of the elements in the source sequence starting from an initial seed value and using the specified aggregation function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the result of the aggregation.</typeparam>
        /// <param name="source">Source sequence whose elements to aggregate.</param>
        /// <param name="seed">Seed value of the aggregation.</param>
        /// <param name="accumulate">Aggregation function used to combine the running aggregation result with each consecutive element.</param>
        /// <returns>Observable sequence containing elements obtained from applying the rolling aggregate to the source sequence. If the source sequence is empty, an empty sequence is returned.</returns>
        public static ISubscribable<TResult> Scan<TSource, TResult>(this ISubscribable<TSource> source, TResult seed, Func<TResult, TSource, TResult> accumulate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulate == null)
                throw new ArgumentNullException(nameof(accumulate));

            return new Scan<TSource, TResult>(source, seed, accumulate);
        }

        #endregion

        #region Max

        /// <summary>
        /// Aggregates the source sequence to return the largest element's value determined using the default comparer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        public static ISubscribable<TSource> Max<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new Max<TSource>(source, Comparer<TSource>.Default);
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest element's value determined using the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest element to obtain.</param>
        /// <param name="comparer">Comparer used to determine the element with the largest value.</param>
        /// <returns>Observable sequence containing a single element with the largest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        public static ISubscribable<TSource> Max<TSource>(this ISubscribable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new Max<TSource>(source, comparer);
        }

        #endregion

        #region Min

        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value determined using the default comparer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        public static ISubscribable<TSource> Min<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new Min<TSource>(source, Comparer<TSource>.Default);
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest element's value determined using the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest element to obtain.</param>
        /// <param name="comparer">Comparer used to determine the element with the smallest value.</param>
        /// <returns>Observable sequence containing a single element with the smallest value found in the source sequence. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        public static ISubscribable<TSource> Min<TSource>(this ISubscribable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new Min<TSource>(source, comparer);
        }

        #endregion

        #region ToList

        /// <summary>
        /// Aggregates the source sequence into a list containing all of its elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to aggregate into a list.</param>
        /// <returns>Observable sequence containing a single element with a list containing all of the elements in the sequence. If the source sequence is empty, an empty list is returned.</returns>
        public static ISubscribable<IList<TSource>> ToList<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new ToList<TSource>(source);
        }

        #endregion

        #endregion

        #region Atoms

        #region Empty

        /// <summary>
        /// Returns an observable sequence representing an empty sequence by producing an OnCompleted notification.
        /// </summary>
        /// <typeparam name="TResult">Type of the elements in the resulting sequence.</typeparam>
        /// <returns>An empty observable sequence.</returns>
        public static ISubscribable<TResult> Empty<TResult>()
        {
            return new Empty<TResult>();
        }

        #endregion

        #region Never

        /// <summary>
        /// Returns an observable sequence that never produces any notification. Such a sequence can be used to represent an infinite duration.
        /// </summary>
        /// <typeparam name="TResult">Type of the elements in the resulting sequence.</typeparam>
        /// <returns>An observable sequence that never produces any notification.</returns>
        public static ISubscribable<TResult> Never<TResult>()
        {
            return new Never<TResult>();
        }

        #endregion

        #region Return

        /// <summary>
        /// Returns an observable sequence that produces a single OnNext notification with the specified value.
        /// </summary>
        /// <typeparam name="TResult">Type of the elements in the resulting sequence.</typeparam>
        /// <param name="value">The single value the resulting sequence should produce.</param>
        /// <returns>An observable sequence that produces a single element with the specified value, followed by a completion notification.</returns>
        public static ISubscribable<TResult> Return<TResult>(TResult value)
        {
            return new Return<TResult>(value);
        }

        #endregion

        #region Throw

        /// <summary>
        /// Returns an observable sequence that produces an OnError notification with the specified error.
        /// </summary>
        /// <typeparam name="TResult">Type of the elements in the resulting sequence.</typeparam>
        /// <param name="error">Error to propagate in the resulting sequence.</param>
        /// <returns>An observable sequence that propagates the specified error.</returns>
        public static ISubscribable<TResult> Throw<TResult>(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return new Throw<TResult>(error);
        }

        #endregion

        #endregion

        #region Higher order

        #region Merge

        /// <summary>
        /// Merges the elements in the inner sequences produced by the specified higher-order sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence producing inner sequences whose elements should be merged.</param>
        /// <returns>Observable sequence containing the elements of the source sequences produced by the specified higher-order sequence.</returns>
        public static ISubscribable<TSource> Merge<TSource>(this ISubscribable<ISubscribable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return SelectMany(sources, xs => xs);
        }

        /// <summary>
        /// Merges the elements in the specified sequences.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
        /// <param name="first">The first observable sequence whose elements should be merged.</param>
        /// <param name="second">The second observable sequence whose elements should be merged.</param>
        /// <returns>Observable sequence containing the elements of the specified source sequences.</returns>
        public static ISubscribable<TSource> Merge<TSource>(this ISubscribable<TSource> first, ISubscribable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new Merge<TSource>(first, second);
        }

        /// <summary>
        /// Merges the elements in the specified sequences.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Array of sequences whose elements should be merged.</param>
        /// <returns>Observable sequence containing the elements of the specified source sequences.</returns>
        public static ISubscribable<TSource> Merge<TSource>(params ISubscribable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new Merge<TSource>(sources);
        }

        #endregion

        #region Switch

        /// <summary>
        /// Switches to the latest inner sequence produced by the specified higher-order sequence and propagates the latest inner sequence's elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence producing inner sequences that should be switched over.</param>
        /// <returns>Observable sequence containing the elements of the latest sequence produced by the specified higher-order sequence.</returns>
        public static ISubscribable<TSource> Switch<TSource>(this ISubscribable<ISubscribable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new Switch<TSource>(sources);
        }

        #endregion

        #endregion

        #region Imperative

        #region Do

        /// <summary>
        /// Observes the source sequence's notifications using the specified observer and propagates the source's notifications to the resulting observable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to observe notifications for.</param>
        /// <param name="observer">Observer used to observe the source's notifications.</param>
        /// <returns>Observable sequence propagating the source's notifications after they've been fed to the observer.</returns>
        public static ISubscribable<TSource> Do<TSource>(this ISubscribable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return new Do<TSource, TSource>(source, x => x, observer);
        }

        /// <summary>
        /// Observes the source sequence's elements using the specified action and propagates the source's notifications to the resulting observable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to observe notifications for.</param>
        /// <param name="onNext">Action to process elements in the source sequence.</param>
        /// <returns>Observable sequence propagating the source's notifications after they've been observed by the specified actions.</returns>
        public static ISubscribable<TSource> Do<TSource>(this ISubscribable<TSource> source, Action<TSource> onNext)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return Do(source, onNext, s_ignoreError, s_nop);
        }

        /// <summary>
        /// Observes the source sequence's elements and error notifications using the specified actions and propagates the source's notifications to the resulting observable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to observe notifications for.</param>
        /// <param name="onNext">Action to process elements in the source sequence.</param>
        /// <param name="onError">Action to process error notifications in the source sequence.</param>
        /// <returns>Observable sequence propagating the source's notifications after they've been observed by the specified actions.</returns>
        public static ISubscribable<TSource> Do<TSource>(this ISubscribable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return Do(source, onNext, onError, s_nop);
        }

        /// <summary>
        /// Observes the source sequence's elements and completion notifications using the specified actions and propagates the source's notifications to the resulting observable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to observe notifications for.</param>
        /// <param name="onNext">Action to process elements in the source sequence.</param>
        /// <param name="onCompleted">Action to process completion notifications in the source sequence.</param>
        /// <returns>Observable sequence propagating the source's notifications after they've been observed by the specified actions.</returns>
        public static ISubscribable<TSource> Do<TSource>(this ISubscribable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return Do(source, onNext, s_ignoreError, onCompleted);
        }

        /// <summary>
        /// Observes the source sequence's notifications using the specified actions and propagates the source's notifications to the resulting observable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to observe notifications for.</param>
        /// <param name="onNext">Action to process elements in the source sequence.</param>
        /// <param name="onError">Action to process error notifications in the source sequence.</param>
        /// <param name="onCompleted">Action to process completion notifications in the source sequence.</param>
        /// <returns>Observable sequence propagating the source's notifications after they've been observed by the specified actions.</returns>
        public static ISubscribable<TSource> Do<TSource>(this ISubscribable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Do(source, Observer.Create<TSource>(onNext, onError, onCompleted));
        }

        /// <summary>
        /// Observes the source sequence's notifications - after projection of its elements - using the specified observer and propagates the source's notifications to the resulting observable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TNotification">Type of the result of the projection.</typeparam>
        /// <param name="source">Source sequence to observe notifications for.</param>
        /// <param name="selector">Selector function used to project the source sequence's elements.</param>
        /// <param name="observer">Observer used to observe the source's notifications after projection.</param>
        /// <returns>Observable sequence propagating the source's notifications after they've been projected and fed to the observer.</returns>
        public static ISubscribable<TSource> Do<TSource, TNotification>(this ISubscribable<TSource> source, Func<TSource, TNotification> selector, IObserver<TNotification> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return new Do<TSource, TNotification>(source, selector, observer);
        }

        #endregion

        #region Finally

        /// <summary>
        /// Invokes the specified action upon exceptional or successful termination of the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to apply the finally behavior to.</param>
        /// <param name="action">Action to invoke upon termination of the source sequence.</param>
        /// <returns>Observable sequence propagating the notification of the source sequence and invoking the specified action upon the sequence's termination.</returns>
        public static ISubscribable<TSource> Finally<TSource>(this ISubscribable<TSource> source, Action action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new Finally<TSource>(source, action);
        }

        #endregion

        #endregion

        #region Reactive Operators

        #region DefaultIfEmpty

        /// <summary>
        /// Propagates all elements from the source sequence, or a default item if the source sequence emits nothing.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate.</param>
        /// <returns>Observable sequence containing all elements from the source sequence, or a default item.</returns>
        public static ISubscribable<TSource> DefaultIfEmpty<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new DefaultIfEmpty<TSource>(source);
        }

        #endregion

        #region Distinct
        /// <summary>
        /// Propagates distinct elements from the source sequence using the default comparer for elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate.</param>
        /// <returns>Observable sequence containing distinct elements from the source sequence.</returns>
        public static ISubscribable<TSource> Distinct<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new Distinct<TSource, TSource>(source, x => x, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Propagates distinct elements from the source sequence using the specified comparer for elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate.</param>
        /// <param name="comparer">Comparer used to compare whether an element is the same as any previously propagated elements.</param>
        /// <returns>Observable sequence containing distinct elements from the source sequence.</returns>
        public static ISubscribable<TSource> Distinct<TSource>(this ISubscribable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return Distinct(source, x => x, comparer);
        }

        /// <summary>
        /// Propagates distinct elements from the source sequence using the default comparer for elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the keys to compare elements by.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate.</param>
        /// <param name="keySelector">Selector function to apply to each element to obtain a key used for equality comparison between elements.</param>
        /// <returns>Observable sequence containing distinct elements from the source sequence.</returns>
        public static ISubscribable<TSource> Distinct<TSource, TKey>(this ISubscribable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return Distinct(source, keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Propagates distinct elements from the source sequence using the specified comparer for elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the keys to compare elements by.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate.</param>
        /// <param name="keySelector">Selector function to apply to each element to obtain a key used for equality comparison between elements.</param>
        /// <param name="comparer">Comparer used to compare whether an element is the same as any previously propagated elements.</param>
        /// <returns>Observable sequence containing distinct elements from the source sequence.</returns>
        public static ISubscribable<TSource> Distinct<TSource, TKey>(this ISubscribable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new Distinct<TSource, TKey>(source, keySelector, comparer);
        }
        #endregion

        #region DistinctUntilChanged

        /// <summary>
        /// Propagates adjacent distinct elements from the source sequence using the default comparer for elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate.</param>
        /// <returns>Observable sequence propagating the source sequence's elements provided the current element is different from the immediately preceding element.</returns>
        public static ISubscribable<TSource> DistinctUntilChanged<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return new DistinctUntilChanged<TSource, TSource>(source, x => x, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Propagates adjacent distinct elements from the source sequence using the specified comparer for elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate.</param>
        /// <param name="comparer">Comparer used to compare whether an element is the same as the preceding element.</param>
        /// <returns>Observable sequence propagating the source sequence's elements provided the current element is different from the immediately preceding element.</returns>
        public static ISubscribable<TSource> DistinctUntilChanged<TSource>(this ISubscribable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return DistinctUntilChanged(source, x => x, comparer);
        }

        /// <summary>
        /// Propagates adjacent distinct elements from the source sequence based on the comparison of a projected key and using the default comparer for keys.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the keys to compare elements by.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate.</param>
        /// <param name="keySelector">Selector function to apply to each element to obtain a key used for equality comparison between elements.</param>
        /// <returns>Observable sequence propagating the source sequence's elements provided the current element is different from the immediately preceding element.</returns>
        public static ISubscribable<TSource> DistinctUntilChanged<TSource, TKey>(this ISubscribable<TSource> source, Func<TSource, TKey> keySelector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return DistinctUntilChanged(source, keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Propagates adjacent distinct elements from the source sequence based on the comparison of a projected key and using the specified comparer for keys.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the keys to compare elements by.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate.</param>
        /// <param name="keySelector">Selector function to apply to each element to obtain a key used for equality comparison between elements.</param>
        /// <param name="comparer">Comparer used to compare whether an element is the same as the preceding element based on the keys obtained for these elements.</param>
        /// <returns>Observable sequence propagating the source sequence's elements provided the current element is different from the immediately preceding element.</returns>
        public static ISubscribable<TSource> DistinctUntilChanged<TSource, TKey>(this ISubscribable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new DistinctUntilChanged<TSource, TKey>(source, keySelector, comparer);
        }

        #endregion

        #region Sample

        /// <summary>
        /// Samples the elements in the source sequence using the specified sampling sequence. For each sampling period, the last element (if any) observed in the sampling period will be propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TSample">Type of the elements in the sampling sequence.</typeparam>
        /// <param name="source">Source sequence to sample.</param>
        /// <param name="sampler">Sampling sequence whose elements cause samples elements from the source sequence to be propagated to the result sequence.</param>
        /// <returns>Observable sequence containing elements of the source sequence obtained from sampling using the specified sampling sequence.</returns>
        public static ISubscribable<TSource> Sample<TSource, TSample>(this ISubscribable<TSource> source, ISubscribable<TSample> sampler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (sampler == null)
                throw new ArgumentNullException(nameof(sampler));

            return new Sample<TSource, TSample>(source, sampler);
        }

        #endregion

        #region SkipUntil

        /// <summary>
        /// Skips elements in the source sequence until the triggering sequence produces an element.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TOther">Type of the elements in the triggering sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to skip until the triggering sequence produces an element.</param>
        /// <param name="triggeringSource">Triggering sequence to produce a signal to start propagating the source sequence's elements.</param>
        /// <returns>Observable sequence propagating the source sequence's elements after the triggering sequence produces an element.</returns>
        public static ISubscribable<TSource> SkipUntil<TSource, TOther>(this ISubscribable<TSource> source, ISubscribable<TOther> triggeringSource)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (triggeringSource == null)
            {
                throw new ArgumentNullException(nameof(triggeringSource));
            }

            return new SkipUntil<TSource, TOther>(source, triggeringSource);
        }

        #endregion

        #region TakeUntil

        /// <summary>
        /// Takes elements from the source sequence until the triggering sequence produces an element.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TOther">Type of the elements in the triggering sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to take until the triggering sequence produces an element.</param>
        /// <param name="triggeringSource">Triggering sequence to produce a signal to stop propagating the source sequence's elements.</param>
        /// <returns>Observable sequence propagating the source sequence's elements until the triggering sequence produces an element.</returns>
        public static ISubscribable<TSource> TakeUntil<TSource, TOther>(this ISubscribable<TSource> source, ISubscribable<TOther> triggeringSource)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (triggeringSource == null)
            {
                throw new ArgumentNullException(nameof(triggeringSource));
            }

            return new TakeUntil<TSource, TOther>(source, triggeringSource);
        }

        #endregion

        #region Throttle

        /// <summary>
        /// Throttles the input sequence based on throttling duration sequences obtained using the specified selector. If an element is received in before the throttling sequence obtained from the previous element has produced an element, the throttling selector is applied to the newly received element and the previous element is dropped.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TThrottle">Type of the elements in the throttling sequences.</typeparam>
        /// <param name="source">Source sequence whose elements to throttle.</param>
        /// <param name="throttleSelector">Selector function to obtain throttling duration sequences for source elements.</param>
        /// <returns>Observable sequence with the throttled elements from the source sequence.</returns>
        public static ISubscribable<TSource> Throttle<TSource, TThrottle>(this ISubscribable<TSource> source, Func<TSource, ISubscribable<TThrottle>> throttleSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (throttleSelector == null)
                throw new ArgumentNullException(nameof(throttleSelector));

            return new Throttle<TSource, TThrottle>(source, throttleSelector);
        }

        #endregion

        #endregion

        #region Sequencing

        #region Retry

        /// <summary>
        /// Retries receiving elements from the source sequence upon encountering an error notification by resubscribing to it.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to apply retry behavior to.</param>
        /// <returns>Observable sequence propagating elements from the source sequence subscription(s).</returns>
        public static ISubscribable<TSource> Retry<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new Retry<TSource>(source);
        }

        /// <summary>
        /// Retries receiving elements from the source sequence upon encountering an error notification by resubscribing to it for a specified maximum number of times.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to apply retry behavior to.</param>
        /// <param name="retryCount">Maximum number of times to subscribe to the source sequence; a value of 1 indicates no retry.</param>
        /// <returns>Observable sequence propagating elements from the source sequence subscription(s).</returns>
        public static ISubscribable<TSource> Retry<TSource>(this ISubscribable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount));

            return new Retry<TSource>(source, retryCount);
        }

        #endregion

        #region StartWith

        /// <summary>
        /// Prepends the source sequence with the specified values.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to subscribe to and propagate elements for after the specified values have been produced in the resulting sequence.</param>
        /// <param name="values">Values to prepend the source sequence with.</param>
        /// <returns>Observable sequence producing the specified values, followed by the elements of the source sequence.</returns>
        public static ISubscribable<TSource> StartWith<TSource>(this ISubscribable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return new StartWith<TSource>(source, values);
        }

        #endregion

        #endregion

        #region Standard Query Operators

        #region Contains

        /// <summary>
        /// Propagates a boolean value determining whether the source sequence contains specified element or not using default comparer for elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence in which to locate the element.</param>
        /// <param name="element">The element to locate in the source sequence.</param>
        /// <returns>Observable sequence propagating a boolean value which determines whether the source sequence contains specified element or not.</returns>
        public static ISubscribable<bool> Contains<TSource>(this ISubscribable<TSource> source, TSource element)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new Contains<TSource>(source, element, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Propagates a boolean value determining whether the source sequence contains specified element or not using specified comparer for elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence in which to locate the element.</param>
        /// <param name="element">The element to locate in the source sequence.</param>
        /// <param name="comparer">Comparer used to compare whether an element is the same as the element being located.</param>
        /// <returns>Observable sequence propagating a boolean value which determines whether the source sequence contains specified element or not.</returns>
        public static ISubscribable<bool> Contains<TSource>(this ISubscribable<TSource> source, TSource element, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new Contains<TSource>(source, element, comparer);
        }

        #endregion

        #region ElementAt

        /// <summary>
        /// Returns a sequence propagating the element at specified index. If the source sequence does not propagate element at specified index, an ArgumentOutOfRangeException error is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence in which to retrieve the element.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>Observable sequence propagating the element at specified index.</returns>
        public static ISubscribable<TSource> ElementAt<TSource>(this ISubscribable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return new ElementAt<TSource>(source, index, throwIfNotFound: true);
        }

        #endregion

        #region ElementAtOrDefault

        /// <summary>
        /// Returns a sequence propagating the element at specified index. If the source sequence does not propagate element at specified index, a default item is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence in which to retrieve the element.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>Observable sequence propagating the element at specified index.</returns>
        public static ISubscribable<TSource> ElementAtOrDefault<TSource>(this ISubscribable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return new ElementAt<TSource>(source, index, throwIfNotFound: false);
        }

        #endregion

        #region FirstAsync

        /// <summary>
        /// Returns a sequence propagating the first element of the source sequence. If the source sequence is empty, an InvalidOperatingException error is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the first element for.</param>
        /// <returns>Observable sequence propagating the first element of the source sequence.</returns>
        public static ISubscribable<TSource> FirstAsync<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new FirstAsync<TSource>(source, predicate: null, throwOnEmpty: true);
        }

        /// <summary>
        /// Returns a sequence propagating the first element of the source sequence that matches the specified predicate. If the source sequence has no element matching the predicate, an InvalidOperatingException error is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the first element matching the predicate for.</param>
        /// <param name="predicate">Predicate to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence propagating the first element of the source sequence that matches the specified predicate.</returns>
        public static ISubscribable<TSource> FirstAsync<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new FirstAsync<TSource>(source, predicate, throwOnEmpty: true);
        }

        #endregion

        #region FirstOrDefaultAsync

        /// <summary>
        /// Returns a sequence propagating the first element of the source sequence. If the source sequence is empty, a default value is produced.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the first element for.</param>
        /// <returns>Observable sequence propagating the first element of the source sequence.</returns>
        public static ISubscribable<TSource> FirstOrDefaultAsync<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new FirstAsync<TSource>(source, predicate: null, throwOnEmpty: false);
        }

        /// <summary>
        /// Returns a sequence propagating the first element of the source sequence that matches the specified predicate. If the source sequence has no element matching the predicate, a default value is produced.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the first element matching the predicate for.</param>
        /// <param name="predicate">Predicate to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence propagating the first element of the source sequence that matches the specified predicate.</returns>
        public static ISubscribable<TSource> FirstOrDefaultAsync<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new FirstAsync<TSource>(source, predicate, throwOnEmpty: false);
        }

        #endregion

        #region GroupBy

        /// <summary>
        /// Groups the elements in the observable sequence by a computed key.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the keys to group by.</typeparam>
        /// <param name="source">Observable sequence whose elements to group.</param>
        /// <param name="keySelector">Key selector function to obtain a key for each element.</param>
        /// <returns>Observable sequence of groups, which are observable sequences themselves.</returns>
        public static ISubscribable<IGroupedSubscribable<TKey, TSource>> GroupBy<TSource, TKey>(this ISubscribable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupBy<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Groups the elements in the observable sequence by a computed key which gets compared to other keys using the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the keys to group by.</typeparam>
        /// <param name="source">Observable sequence whose elements to group.</param>
        /// <param name="keySelector">Key selector function to obtain a key for each element.</param>
        /// <param name="comparer">Comparer used to compare keys for equality.</param>
        /// <returns>Observable sequence of groups, which are observable sequences themselves.</returns>
        public static ISubscribable<IGroupedSubscribable<TKey, TSource>> GroupBy<TSource, TKey>(this ISubscribable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupBy<TSource, TKey>(source, keySelector, comparer);
        }

        /// <summary>
        /// Groups the elements in the observable sequence by a computed key.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the keys to group by.</typeparam>
        /// <typeparam name="TElement">Type of the elements produced in the groups.</typeparam>
        /// <param name="source">Observable sequence whose elements to group.</param>
        /// <param name="keySelector">Key selector function to obtain a key for each element.</param>
        /// <param name="elementSelector">Element selector function to obtain a group element for each input element.</param>
        /// <returns>Observable sequence of groups, which are observable sequences themselves.</returns>
        public static ISubscribable<IGroupedSubscribable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this ISubscribable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return new GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Groups the elements in the observable sequence by a computed key which gets compared to other keys using the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the keys to group by.</typeparam>
        /// <typeparam name="TElement">Type of the elements produced in the groups.</typeparam>
        /// <param name="source">Observable sequence whose elements to group.</param>
        /// <param name="keySelector">Key selector function to obtain a key for each element.</param>
        /// <param name="elementSelector">Element selector function to obtain a group element for each input element.</param>
        /// <param name="comparer">Comparer used to compare keys for equality.</param>
        /// <returns>Observable sequence of groups, which are observable sequences themselves.</returns>
        public static ISubscribable<IGroupedSubscribable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this ISubscribable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        #endregion

        #region IgnoreElements

        /// <summary>
        /// Suppresses all of the elements propagated by the source sequence, but still propagates its OnCompleted or OnError notification.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to suppress.</param>
        /// <returns>Observable sequence propagating only an OnCompleted or an OnError notification.</returns>
        public static ISubscribable<TSource> IgnoreElements<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new IgnoreElements<TSource>(source);
        }

        #endregion

        #region LastAsync

        /// <summary>
        /// Returns a sequence propagating the last element of the source sequence. If the source sequence is empty, an InvalidOperatingException error is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the last element for.</param>
        /// <returns>Observable sequence propagating the last element of the source sequence.</returns>
        public static ISubscribable<TSource> LastAsync<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new LastAsync<TSource>(source, predicate: null, throwOnEmpty: true);
        }

        /// <summary>
        /// Returns a sequence propagating the last element of the source sequence that matches the specified predicate. If the source sequence has no element matching the predicate, an InvalidOperatingException error is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the last element matching the predicate for.</param>
        /// <param name="predicate">Predicate to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence propagating the last element of the source sequence that matches the specified predicate.</returns>
        public static ISubscribable<TSource> LastAsync<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new LastAsync<TSource>(source, predicate, throwOnEmpty: true);
        }

        #endregion

        #region LastOrDefaultAsync

        /// <summary>
        /// Returns a sequence propagating the last element of the source sequence. If the source sequence is empty, a default value is produced.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the last element for.</param>
        /// <returns>Observable sequence propagating the last element of the source sequence.</returns>
        public static ISubscribable<TSource> LastOrDefaultAsync<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new LastAsync<TSource>(source, predicate: null, throwOnEmpty: false);
        }

        /// <summary>
        /// Returns a sequence propagating the last element of the source sequence that matches the specified predicate. If the source sequence has no element matching the predicate, a default value is produced.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the last element matching the predicate for.</param>
        /// <param name="predicate">Predicate to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence propagating the last element of the source sequence that matches the specified predicate.</returns>
        public static ISubscribable<TSource> LastOrDefaultAsync<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new LastAsync<TSource>(source, predicate, throwOnEmpty: false);
        }

        #endregion

        #region Select

        /// <summary>
        /// Projects elements in the source sequence using the specified selector function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to project.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence with the projected elements from the source sequence using the specified selector function.</returns>
        public static ISubscribable<TResult> Select<TSource, TResult>(this ISubscribable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new Select<TSource, TResult>(source, selector);
        }

        //
        // TODO: overload with TimeSpan based index.
        //

        /// <summary>
        /// Projects elements in the source sequence using the specified selector function that takes in an element index.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to project.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence with the projected elements from the source sequence using the specified selector function.</returns>
        public static ISubscribable<TResult> Select<TSource, TResult>(this ISubscribable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new SelectIndexed<TSource, TResult>(source, selector);
        }

        #endregion

        #region SelectMany

        /// <summary>
        /// Merges the inner sequences obtained by projecting the source sequence's elements using the specified selector function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">Type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to project onto inner sequences.</param>
        /// <param name="selector">Selector to apply to element in the source sequence in order to obtain an inner sequence.</param>
        /// <returns>Observable sequence containing the merged elements from the inner sequences.</returns>
        public static ISubscribable<TResult> SelectMany<TSource, TResult>(this ISubscribable<TSource> source, Func<TSource, ISubscribable<TResult>> selector)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return SelectMany<TSource, TResult, TResult>(source, selector, (_, result) => result);
        }

        /// <summary>
        /// Merges the inner sequences obtained by projecting the source sequence's elements using the specified collection selector function and by applying the specified result selector function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">Type of the elements in the inner sequences.</typeparam>
        /// <typeparam name="TResult">Type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to project onto inner sequences.</param>
        /// <param name="collectionSelector">Selector to apply to element in the source sequence in order to obtain an inner sequence.</param>
        /// <param name="resultSelector">Result selector to combine an outer source sequence's element and an inner sequence's element in order to produce a result element.</param>
        /// <returns>Observable sequence containing the elements obtained from applying the result selector to pairs of outer and inner sequence elements.</returns>
        public static ISubscribable<TResult> SelectMany<TSource, TCollection, TResult>(this ISubscribable<TSource> source, Func<TSource, ISubscribable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return new SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        #endregion

        #region SequenceEqual

        /// <summary>
        /// Compares two observable sequences for pairwise equality of elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
        /// <param name="left">Left sequence whose elements to compare.</param>
        /// <param name="right">Right sequence whose elements to compare.</param>
        /// <returns>Observable sequence containing a single Boolean value indicating whether the two sequences are equal in length and have pairwise equality of elements.</returns>
        public static ISubscribable<bool> SequenceEqual<TSource>(this ISubscribable<TSource> left, ISubscribable<TSource> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new SequenceEqual<TSource>(left, right, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Compares two observable sequences for pairwise equality of elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
        /// <param name="left">Left sequence whose elements to compare.</param>
        /// <param name="right">Right sequence whose elements to compare.</param>
        /// <param name="comparer">Equality comparer used to compare elements from both sequences.</param>
        /// <returns>Observable sequence containing a single Boolean value indicating whether the two sequences are equal in length and have pairwise equality of elements.</returns>
        public static ISubscribable<bool> SequenceEqual<TSource>(this ISubscribable<TSource> left, ISubscribable<TSource> right, IEqualityComparer<TSource> comparer)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new SequenceEqual<TSource>(left, right, comparer);
        }

        #endregion

        #region SingleAsync

        /// <summary>
        /// Returns a sequence propagating the only element in the source sequence. If source sequence is empty or is containing more than one element, an InvalidOperationException error is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the only element for.</param>
        /// <returns>Observable sequence propagating the only element in the source sequence.</returns>
        public static ISubscribable<TSource> SingleAsync<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SingleAsync<TSource>(source, predicate: null, throwOnEmpty: true);
        }

        /// <summary>
        /// Returns a sequence propagating the only element in the source sequence that matches the specified predicate. If the source sequence has no element matching the predicate or has more than one element matching the predicate, an InvalidOperationException error is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the only element matching the predicate for.</param>
        /// <param name="predicate">Predicate to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence propagating the only element in the source sequence that matches the specified predicate.</returns>
        public static ISubscribable<TSource> SingleAsync<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SingleAsync<TSource>(source, predicate, throwOnEmpty: true);
        }

        #endregion

        #region SingleOrDefaultAsync

        /// <summary>
        /// Returns a sequence propagating the only element in the source sequence. If source sequence is empty, then a default value is produced. If source sequence contains more than one element, then an InvalidOperationException error is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the only element for.</param>
        /// <returns>Observable sequence propagating the only element in the source sequence.</returns>
        public static ISubscribable<TSource> SingleOrDefaultAsync<TSource>(this ISubscribable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SingleAsync<TSource>(source, predicate: null, throwOnEmpty: false);
        }

        /// <summary>
        /// Returns a sequence propagating the only element in the source sequence that matches the specified predicate. If the source sequence has no element matching the predicate, then default value is produced. If the source sequence has more than one element matching the predicate, then an InvalidOperationException error is propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to propagate the only element for.</param>
        /// <param name="predicate">Predicate to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence propagating the only element in the source sequence that matches the specified predicate.</returns>
        public static ISubscribable<TSource> SingleOrDefaultAsync<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SingleAsync<TSource>(source, predicate, throwOnEmpty: false);
        }

        #endregion

        #region Skip

        /// <summary>
        /// Skips the specified number of elements from the start of the source sequence. If the sequence contains less elements than the number specified, an empty sequence is returned.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate to the result sequence.</param>
        /// <param name="count">Number of elements to skip from the start of the source sequence.</param>
        /// <returns>Observable sequence skipping the specified number of elements from the start of the source sequence.</returns>
        public static ISubscribable<TSource> Skip<TSource>(this ISubscribable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return new Skip<TSource>(source, count);
        }

        #endregion

        #region SkipWhile

        /// <summary>
        /// Skips elements from the source sequence until the specified predicate holds for an element.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate to the result sequence.</param>
        /// <param name="predicate">Predicate function to apply to each element in the sequence to determine when to start propagating elements.</param>
        /// <returns>Observable sequence containing the source sequence's elements starting from the first element matching the predicate.</returns>
        public static ISubscribable<TSource> SkipWhile<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SkipWhile<TSource>(source, predicate);
        }

        /// <summary>
        /// Skips elements from the source sequence until the specified indexed predicate holds for an element.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate to the result sequence.</param>
        /// <param name="predicate">Predicate function to apply to each element in the sequence to determine when to start propagating elements.</param>
        /// <returns>Observable sequence containing the source sequence's elements starting from the first element matching the predicate.</returns>
        public static ISubscribable<TSource> SkipWhile<TSource>(this ISubscribable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new SkipWhileIndexed<TSource>(source, predicate);
        }

        #endregion

        #region Take

        /// <summary>
        /// Takes the specified number of elements from the start of the source sequence. If the sequence contains less elements than the number specified, all elements are returned.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate to the result sequence.</param>
        /// <param name="count">Number of elements to propagate from the start of the source sequence.</param>
        /// <returns>Observable sequence containing the specified number of elements from the start of the source sequence.</returns>
        public static ISubscribable<TSource> Take<TSource>(this ISubscribable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return Empty<TSource>();
            }

            return new Take<TSource>(source, count);
        }

        #endregion

        #region TakeWhile

        /// <summary>
        /// Takes elements from the source sequence as long as the specified predicate holds for its elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate to the result sequence.</param>
        /// <param name="predicate">Predicate function to apply to each element in the sequence to determine when to stop propagating elements.</param>
        /// <returns>Observable sequence containing the source sequence's elements until the predicate doesn't match an element.</returns>
        public static ISubscribable<TSource> TakeWhile<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new TakeWhile<TSource>(source, predicate);
        }

        /// <summary>
        /// Takes elements from the source sequence as long as the specified indexed predicate holds for its elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to propagate to the result sequence.</param>
        /// <param name="predicate">Predicate function to apply to each element in the sequence to determine when to stop propagating elements.</param>
        /// <returns>Observable sequence containing the source sequence's elements until the predicate doesn't match an element.</returns>
        public static ISubscribable<TSource> TakeWhile<TSource>(this ISubscribable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new TakeWhileIndexed<TSource>(source, predicate);
        }

        #endregion

        #region Where

        /// <summary>
        /// Filters elements from the source sequence using the specified predicate function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to filter.</param>
        /// <param name="predicate">Predicate function used to determine whether an element should be included in the result sequence.</param>
        /// <returns>Observable sequence containing the elements from the source sequence that match the predicate.</returns>
        public static ISubscribable<TSource> Where<TSource>(this ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new Where<TSource>(source, predicate);
        }

        //
        // TODO: overload with TimeSpan based index.
        //

        /// <summary>
        /// Filters elements from the source sequence using the specified predicate function that takes in an element index.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to filter.</param>
        /// <param name="predicate">Predicate function used to determine whether an element should be included in the result sequence.</param>
        /// <returns>Observable sequence containing the elements from the source sequence that match the predicate.</returns>
        public static ISubscribable<TSource> Where<TSource>(this ISubscribable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new WhereIndexed<TSource>(source, predicate);
        }

        #endregion

        #endregion

        #region Time

        #region Buffer

        /// <summary>
        /// Partitions the source sequence into buffers based on the specified buffer duration.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="duration">Duration of a buffer.</param>
        /// <returns>Observable sequence containing buffers of the specified duration over the source sequence.</returns>
        public static ISubscribable<IList<TSource>> Buffer<TSource>(this ISubscribable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration <= TimeSpan.FromTicks(0))
                throw new ArgumentOutOfRangeException(nameof(duration));

            return new BufferDuration<TSource>(source, duration);
        }

        /// <summary>
        /// Partitions the source sequence into buffers based on the specified buffer duration and time shift.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="duration">Duration of a buffer.</param>
        /// <param name="shift">Time shift between one buffer and the next.</param>
        /// <returns>Observable sequence containing buffers of the specified duration over the source sequence.</returns>
        public static ISubscribable<IList<TSource>> Buffer<TSource>(this ISubscribable<TSource> source, TimeSpan duration, TimeSpan shift)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration <= TimeSpan.FromTicks(0))
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (shift <= TimeSpan.FromTicks(0))
                throw new ArgumentOutOfRangeException(nameof(shift));

            return new BufferDurationShift<TSource>(source, duration, shift);
        }

        /// <summary>
        /// Partitions the source sequence into buffers based on the specified buffer length.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="count">Number of elements per buffer.</param>
        /// <returns>Observable sequence containing buffers of the specified length over the source sequence.</returns>
        public static ISubscribable<IList<TSource>> Buffer<TSource>(this ISubscribable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return new BufferCount<TSource>(source, count);
        }

        /// <summary>
        /// Partitions the source sequence into buffers based on the specified buffer length and element skip count.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="count">Number of elements per buffer.</param>
        /// <param name="skip">Offset of the number of elements between the start of one buffer and the next.</param>
        /// <returns>Observable sequence containing buffers of the specified length over the source sequence.</returns>
        public static ISubscribable<IList<TSource>> Buffer<TSource>(this ISubscribable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (skip <= 0)
                throw new ArgumentOutOfRangeException(nameof(skip));

            return new BufferCountSkip<TSource>(source, count, skip);
        }

        /// <summary>
        /// Partitions the source sequence into buffers that are sent out when they're full or a given amount of time has elapsed.
        /// A useful real-world analogy of this overload is the behavior of a ferry leaving the dock when all seats are taken, or at the scheduled time of departure, whichever event occurs first.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="duration">Maximum duration of a buffer.</param>
        /// <param name="count">Maximum number of elements per buffer.</param>
        /// <returns>Observable sequence containing buffers over the source sequence.</returns>
        public static ISubscribable<IList<TSource>> Buffer<TSource>(this ISubscribable<TSource> source, TimeSpan duration, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration <= TimeSpan.FromTicks(0))
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return new BufferDurationCount<TSource>(source, duration, count);
        }

        #endregion

        #region DelaySubscription

        /// <summary>
        /// Delays the subscription to the source sequence until the specified absolute due time.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay subscription to.</param>
        /// <param name="dueTime">Time when a subscription to the source sequence should be made.</param>
        /// <returns>Observable sequence propagating the source sequence's notifications after it has been subscribed to at the specified due time.</returns>
        public static ISubscribable<TSource> DelaySubscription<TSource>(this ISubscribable<TSource> source, DateTimeOffset dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new DelaySubscription<TSource>(source, dueTime);
        }

        /// <summary>
        /// Delays the subscription to the source sequence until the specified relative due time.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay subscription to. This time is relative to the time of subscription.</param>
        /// <param name="dueTime">Time when a subscription to the source sequence should be made.</param>
        /// <returns>Observable sequence propagating the source sequence's notifications after it has been subscribed to at the specified due time.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the due time is less than zero.</exception>
        public static ISubscribable<TSource> DelaySubscription<TSource>(this ISubscribable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.FromTicks(0))
                throw new ArgumentOutOfRangeException(nameof(dueTime));

            return new DelaySubscription<TSource>(source, dueTime);
        }

        #endregion

        #region Sample

        /// <summary>
        /// Samples the elements in the source sequence using the specified period. For each sampling period, the last element (if any) observed in the sampling period will be propagated.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to sample.</param>
        /// <param name="period">Sampling period.</param>
        /// <returns>Observable sequence containing elements of the source sequence obtained from sampling with the specified period.</returns>
        public static ISubscribable<TSource> Sample<TSource>(this ISubscribable<TSource> source, TimeSpan period)
        {
            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return Sample<TSource, long>(source, Timer(period, period));
        }

        #endregion

        #region Skip

        /// <summary>
        /// Skips the elements in the source sequence for the specified duration.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to skip elements for.</param>
        /// <param name="dueTime">Time when elements in the source sequence should be start to propagated to the resulting sequence. This time is relative to the time of subscription.</param>
        /// <returns>Observable sequence propagating the elements of the source sequence after the specified relative due time.</returns>
        public static ISubscribable<TSource> Skip<TSource>(this ISubscribable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));

            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return new SkipUntil<TSource, long>(source, Timer(dueTime), terminateEarly: true);
        }

        #endregion

        #region SkipUntil

        /// <summary>
        /// Skips the elements in the source sequence until the specified start time.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to skip elements for.</param>
        /// <param name="startTime">Time when elements in the source sequence should start to be propagated to the resulting sequence.</param>
        /// <returns>Observable sequence propagating the elements of the source sequence after the specified absolute start time.</returns>
        public static ISubscribable<TSource> SkipUntil<TSource>(this ISubscribable<TSource> source, DateTimeOffset startTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return new SkipUntil<TSource, long>(source, Timer(startTime), terminateEarly: true);
        }

        #endregion

        #region Take

        /// <summary>
        /// Takes the elements from the source sequence for the specified duration.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="dueTime">Time when elements in the source sequence should be stop to propagated to the resulting sequence. This time is relative to the time of subscription.</param>
        /// <returns>Observable sequence propagating the elements of the source sequence for the specified relative due time.</returns>
        public static ISubscribable<TSource> Take<TSource>(this ISubscribable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));

            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return new TakeUntil<TSource, long>(source, Timer(dueTime));
        }

        #endregion

        #region TakeUntil

        /// <summary>
        /// Takes the elements from the source sequence until the specified end time.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="endTime">Time when elements in the source sequence should stop to be propagated to the resulting sequence.</param>
        /// <returns>Observable sequence propagating the elements of the source sequence until the specified absolute end time.</returns>
        public static ISubscribable<TSource> TakeUntil<TSource>(this ISubscribable<TSource> source, DateTimeOffset endTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return new TakeUntil<TSource, long>(source, Timer(endTime));
        }

        #endregion

        #region Throttle

        /// <summary>
        /// Throttles the elements in the source sequence using the specified throttling duration. If an element is received in less than the throttling duration from the previous element, the throttling timer is reset and the previous element is dropped.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to throttle elements for.</param>
        /// <param name="duration">Duration of a throttling period.</param>
        /// <returns>Observable sequence with the throttled elements from the source sequence.</returns>
        public static ISubscribable<TSource> Throttle<TSource>(this ISubscribable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            //
            // WARNING: Don't unfold the composition blindly; recovery of existing state won't be compatible.
            //
            return new Throttle<TSource, long>(source, value => Timer(duration));
        }

        #endregion

        #region Timer

        /// <summary>
        /// Returns an observable sequence that generates an element at the specified relative due time.
        /// </summary>
        /// <param name="dueTime">Time when the timer is due. This time is relative to the time of subscription.</param>
        /// <returns>Observable sequence producing a single element at the specified relative due time.</returns>
        public static ISubscribable<long> Timer(TimeSpan dueTime)
        {
            return new Timer(dueTime, period: null);
        }

        /// <summary>
        /// Returns an observable sequence that generates elements for each timer tick specified by the relative due time and period.
        /// </summary>
        /// <param name="dueTime">Time when the first timer tick is due. This time is relative to the time of subscription.</param>
        /// <param name="period">Period of the timer.</param>
        /// <returns>Observable sequence producing an element for each tick of the timer conform the specified relative due time and period.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the period is less than zero.</exception>
        public static ISubscribable<long> Timer(TimeSpan dueTime, TimeSpan period)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));

            return new Timer(dueTime, period);
        }

        /// <summary>
        /// Returns an observable sequence that generates an element at the specified absolute due time.
        /// </summary>
        /// <param name="dueTime">Absolute time when the timer is due.</param>
        /// <returns>Observable sequence producing a single element at the specified absolute due time.</returns>
        public static ISubscribable<long> Timer(DateTimeOffset dueTime)
        {
            return new Timer(dueTime, period: null);
        }

        /// <summary>
        /// Returns an observable sequence that generates elements for each timer tick specified by the absolute due time and period.
        /// </summary>
        /// <param name="dueTime">Absolute time when the first timer tick is due.</param>
        /// <param name="period">Period of the timer.</param>
        /// <returns>Observable sequence producing an element for each tick of the timer conform the specified absolute due time and period.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the period is less than zero.</exception>
        public static ISubscribable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));

            return new Timer(dueTime, period);
        }

        #endregion

        #region Window

        /// <summary>
        /// Partitions the source sequence into windows based on the specified window duration.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="duration">Duration of a window.</param>
        /// <returns>Observable sequence containing windows of the specified duration over the source sequence.</returns>
        public static ISubscribable<ISubscribable<TSource>> Window<TSource>(this ISubscribable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration <= TimeSpan.FromTicks(0))
                throw new ArgumentOutOfRangeException(nameof(duration));

            return new WindowDuration<TSource>(source, duration);
        }

        /// <summary>
        /// Partitions the source sequence into windows based on the specified window duration and time shift.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="duration">Duration of a window.</param>
        /// <param name="shift">Time shift between one window and the next.</param>
        /// <returns>Observable sequence containing windows of the specified duration over the source sequence.</returns>
        public static ISubscribable<ISubscribable<TSource>> Window<TSource>(this ISubscribable<TSource> source, TimeSpan duration, TimeSpan shift)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration <= TimeSpan.FromTicks(0))
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (shift <= TimeSpan.FromTicks(0))
                throw new ArgumentOutOfRangeException(nameof(shift));

            return new WindowDurationShift<TSource>(source, duration, shift);
        }

        /// <summary>
        /// Partitions the source sequence into windows based on the specified window length.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="count">Number of elements per window.</param>
        /// <returns>Observable sequence containing windows of the specified length over the source sequence.</returns>
        public static ISubscribable<ISubscribable<TSource>> Window<TSource>(this ISubscribable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return new WindowCount<TSource>(source, count);
        }

        /// <summary>
        /// Partitions the source sequence into windows based on the specified window length and element skip count.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="count">Number of elements per window.</param>
        /// <param name="skip">Offset of the number of elements between the start of one window and the next.</param>
        /// <returns>Observable sequence containing windows of the specified length over the source sequence.</returns>
        public static ISubscribable<ISubscribable<TSource>> Window<TSource>(this ISubscribable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (skip <= 0)
                throw new ArgumentOutOfRangeException(nameof(skip));

            return new WindowCountSkip<TSource>(source, count, skip);
        }

        /// <summary>
        /// Partitions the source sequence into windows that are closed when they're full or a given amount of time has elapsed.
        /// A useful real-world analogy of this overload is the behavior of a ferry leaving the dock when all seats are taken, or at the scheduled time of departure, whichever event occurs first.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="duration">Maximum duration of a window.</param>
        /// <param name="count">Maximum number of elements per window.</param>
        /// <returns>Observable sequence containing windows over the source sequence.</returns>
        public static ISubscribable<ISubscribable<TSource>> Window<TSource>(this ISubscribable<TSource> source, TimeSpan duration, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration <= TimeSpan.FromTicks(0))
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return new WindowDurationCount<TSource>(source, duration, count);
        }

        #endregion

        #endregion

        #region ToSubscribable

        //
        // NB: This is kept on the Subscribable type (but no longer as an extension method) for backwards compatibility
        //     reasons. Checkpointed state in production contains references to the method defined here.
        //

        /// <summary>
        /// Converts an observable sequence to a subscribable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to convert to a subscribable sequence.</param>
        /// <returns>Subscribable sequence producing the same notifications as the specified observable sequence.</returns>
        public static ISubscribable<TSource> ToSubscribable<TSource>(IObservable<TSource> source) => source.ToSubscribable();

        #endregion
    }
}
