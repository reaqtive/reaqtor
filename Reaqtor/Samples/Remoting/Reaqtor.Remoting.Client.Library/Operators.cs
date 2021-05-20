// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    public static partial class Operators
    {
        /// <summary>
        /// Converts an IReactiveQbservable to an IAsyncReactiveQbservable for define operations.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">An observable.</param>
        /// <returns>Always throws exception; should only be used in definition expressions.</returns>
        /// <exception cref="System.NotImplementedException">Always thrown; see remarks.</exception>
        /// <remarks>
        /// This method is only used for definition purposes.  It rewrites the the method
        /// call expression into an invocation of the known resource URI with the source
        /// parameter.  The URI maps to a built-in operator.  This method should only be
        /// needed to define operators in terms of their synchronous implementations.
        /// Any other operators defined as combinations of other operators should be able
        /// to use the asynchronous variants without any issues.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Identity)]
        public static IAsyncReactiveQbservable<TSource> AsAsync<TSource>(
            this IReactiveQbservable<TSource> source)
        {
            // This should never be called.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts an IAsyncReativeQbservable to an IReactiveQbservable for define operations.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">An observable.</param>
        /// <returns>Always throws exception; should only be used in definition expressions.</returns>
        /// <exception cref="System.NotImplementedException">Always thrown; see remarks.</exception>
        /// <remarks>
        /// This method is only used for definition purposes.  It rewrites the the method
        /// call expression into an invocation of the known resource URI with the source
        /// parameter.  The URI maps to a built-in operator.  This method should only be
        /// needed to define operators in terms of their synchronous implementations.
        /// Any other operators defined as combinations of other operators should be able
        /// to use the asynchronous variants without any issues.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Identity)]
        public static IReactiveQbservable<TSource> AsSync<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            // This should never be called.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts an IAsyncReativeQbserver to an IReactiveQbserver for define operations.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">An observer.</param>
        /// <returns>Always throws exception; should only be used in definition expressions.</returns>
        /// <exception cref="System.NotImplementedException">Always thrown; see remarks.</exception>
        /// <remarks>
        /// This method is only used for definition purposes.  It rewrites the the method
        /// call expression into an invocation of the known resource URI with the source
        /// parameter.  The URI maps to a built-in operator.  This method should only be
        /// needed to define operators in terms of their synchronous implementations.
        /// Any other operators defined as combinations of other operators should be able
        /// to use the asynchronous variants without any issues.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Identity)]
        public static IReactiveQbserver<TSource> AsSync<TSource>(
            this IAsyncReactiveQbserver<TSource> source)
        {
            // This should never be called.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts an IReativeQbserver to an IAsyncReactiveQbserver for define operations.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">An observer.</param>
        /// <returns>Always throws exception; should only be used in definition expressions.</returns>
        /// <exception cref="System.NotImplementedException">Always thrown; see remarks.</exception>
        /// <remarks>
        /// This method is only used for definition purposes.  It rewrites the the method
        /// call expression into an invocation of the known resource URI with the source
        /// parameter.  The URI maps to a built-in operator.  This method should only be
        /// needed to define operators in terms of their synchronous implementations.
        /// Any other operators defined as combinations of other operators should be able
        /// to use the asynchronous variants without any issues.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Identity)]
        public static IAsyncReactiveQbserver<TSource> AsAsync<TSource>(
            this IReactiveQbserver<TSource> source)
        {
            // This should never be called.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Partitions the source sequence into buffers based on the specified buffer duration.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="duration">Duration of a buffer.</param>
        /// <returns>Observable sequence containing buffers of the specified duration over the source sequence.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Buffer.TimeDuration)]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            return source.Provider.CreateQbservable<IList<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(duration, typeof(TimeSpan))
                )
            );
        }

        /// <summary>
        /// Partitions the source sequence into buffers based on the specified buffer duration and time shift.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="duration">Duration of a buffer.</param>
        /// <param name="shift">Time shift between one buffer and the next.</param>
        /// <returns>Observable sequence containing buffers of the specified duration over the source sequence.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Buffer.TimeDurationShift)]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            if (shift <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(shift));
            }

            return source.Provider.CreateQbservable<IList<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(duration, typeof(TimeSpan)),
                    Expression.Constant(shift, typeof(TimeSpan))
                )
            );
        }

        /// <summary>
        /// Partitions the source sequence into buffers based on the specified buffer length.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="count">Number of elements per buffer.</param>
        /// <returns>Observable sequence containing buffers of the specified length over the source sequence.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Buffer.Count)]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return source.Provider.CreateQbservable<IList<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

        /// <summary>
        /// Partitions the source sequence into buffers based on the specified buffer length and element skip count.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="count">Number of elements per buffer.</param>
        /// <param name="skip">Offset of the number of elements between the start of one buffer and the next.</param>
        /// <returns>Observable sequence containing buffers of the specified length over the source sequence.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Buffer.CountSkip)]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, int count, int skip)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (skip <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }

            return source.Provider.CreateQbservable<IList<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count, typeof(int)),
                    Expression.Constant(skip, typeof(int))
                )
            );
        }

        /// <summary>
        /// Partitions the source sequence into buffers that are sent out when they're full or a given amount of time has elapsed.
        /// A useful real-world analogy of this overload is the behavior of a ferry leaving the dock when all seats are taken, or at the scheduled time of departure, whichever event occurs first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate buffers for.</param>
        /// <param name="duration">Maximum duration of a buffer.</param>
        /// <param name="count">Maximum number of elements per buffer.</param>
        /// <returns>Observable sequence containing buffers over the source sequence.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Buffer.TimeCount)]
        public static IAsyncReactiveQbservable<IList<TSource>> Buffer<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return source.Provider.CreateQbservable<IList<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(duration, typeof(TimeSpan)),
                    Expression.Constant(count, typeof(int))
                )
            );
        }

        /// <summary>
        /// Merges two queryable observable sequences into one queryable
        /// observable sequence by using the selector function whenever one of
        /// the queryable observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the second sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result sequence.</typeparam>
        /// <param name="first">The first queryable observable source.</param>
        /// <param name="second">The second queryable observable source.</param>
        /// <param name="selector">
        /// The function to invoke whenever either of the sources produces an
        /// element.</param>
        /// <returns>
        /// A queryable observable sequence containing the result of combining
        /// elements of both sources using the specified result selector
        /// function.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>first</c> or <c>second</c> or <c>selector</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.CombineLatest.ObservableFunc)]
        public static IAsyncReactiveQbservable<TResult> CombineLatest<TFirst, TSecond, TResult>(
            this IAsyncReactiveQbservable<TFirst> first,
            IAsyncReactiveQbservable<TSecond> second,
            Expression<Func<TFirst, TSecond, TResult>> selector)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return first.Provider.CreateQbservable<TResult>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TResult)),
                    first.Expression,
                    GetSourceExpression(second),
                    selector));
        }

        /// <summary>
        /// Time shifts the query observable sequence by delaying the
        /// subscription to the specified absolute time.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay subscription for.</param>
        /// <param name="dueTime">Absolute time to perform the subscription at.</param>
        /// <returns>
        /// Time-shifted sequence
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><c>source</c> is null</exception>
        /// <remarks>
        ///   <para>
        /// This operator is more efficient than Delay but postpones all
        /// side-effects of subscription and affects error propagation timing.
        ///   </para><para>
        /// The side-effects of subscribing to the source sequence will be run
        /// on the default scheduler. Observer callbacks will not be affected.
        ///   </para>
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.DelaySubscription.V1.DateTimeOffset)]
        public static IAsyncReactiveQbservable<TSource> DelaySubscription<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            DateTimeOffset dueTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(dueTime)));
        }

        /// <summary>
        /// Time shifts the query observable sequence by delaying the
        /// subscription to the specified relative time.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay subscription for.</param>
        /// <param name="dueTime">Relative time to perform the subscription at.</param>
        /// <returns>
        /// Time-shifted sequence
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><c>source</c> is null</exception>
        /// <remarks>A
        ///   <para>
        /// This operator is more efficient than Delay but postpones all
        /// side-effects of subscription and affects error propagation timing.
        ///   </para>
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.DelaySubscription.V1.TimeSpan)]
        public static IAsyncReactiveQbservable<TSource> DelaySubscription<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            TimeSpan dueTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(dueTime)));
        }

        /// <summary>
        /// Returns a query observable sequence that contains only distinct
        /// elements.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">
        /// A queryable observable sequence to retain distinct elements for.
        /// </param>
        /// <returns>
        /// A queryable observable sequence only containing the distinct
        /// elements from the source query observable sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Distinct.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> Distinct<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Returns a query observable sequence that contains only distinct
        /// elements according to the keySelector.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the discriminator key computed for each element in the
        /// source sequence.
        /// </typeparam>
        /// <param name="source">
        /// A queryable observable sequence to retain distinct elements for,
        /// based on a computed key value.
        /// </param>
        /// <param name="keySelector">
        /// A function to compute the comparison key for each element.
        /// </param>
        /// <returns>
        /// A queryable observable sequence only containing the distinct
        /// elements, based on a computed key value, from the source
        /// query observable sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>keySelector</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Distinct.Func)]
        public static IAsyncReactiveQbservable<TSource> Distinct<TSource, TKey>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector));
        }

        /// <summary>
        /// Returns a query observable sequence that contains only distinct
        /// contiguous elements.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">
        /// A queryable observable sequence to retain distinct contiguous
        /// elements for.
        /// </param>
        /// <returns>
        /// A queryable observable sequence only containing the distinct
        /// contiguous elements from the source query observable sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.DistinctUntilChanged.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Returns a query observable sequence that contains only distinct
        /// contiguous elements according to the keySelector.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the discriminator key computed for each element in the
        /// source sequence.
        /// </typeparam>
        /// <param name="source">
        /// A queryable observable sequence to retain distinct contiguous
        /// elements for, based on a computed key value.
        /// </param>
        /// <param name="keySelector">
        /// A function to compute the comparison key for each element.
        /// </param>
        /// <returns>
        /// A queryable observable sequence only containing the distinct
        /// contiguous elements, based on a computed key value, from the source
        /// query observable sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>keySelector</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.DistinctUntilChanged.Func)]
        public static IAsyncReactiveQbservable<TSource> DistinctUntilChanged<TSource, TKey>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector));
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, and
        /// propagates all observer messages through the result sequence. This
        /// method can be used for debugging, logging, etc. of query behavior
        /// by intercepting the message stream to run arbitrary actions for
        /// messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="observer">
        /// An observer to invoke actions on as they propagate through to
        /// the result sequence.
        /// </param>
        /// <returns>
        /// The source query observerable sequence with the side-effecting
        /// behavior applied.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>observer</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Do.Observer)]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            IAsyncReactiveQbserver<TSource> observer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    observer.Expression));
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, and
        /// propagates all observer messages through the result sequence. This
        /// method can be used for debugging, logging, etc. of query behavior
        /// by intercepting the message stream to run arbitrary actions for
        /// messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="onNext">
        /// Action to invoke for each element in the query observable sequence.
        /// </param>
        /// <returns>
        /// The source query observerable sequence with the side-effecting
        /// behavior applied.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>onNext</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Do.OnNext)]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Action<TSource>> onNext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    onNext));
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, and
        /// propagates all observer messages through the result sequence. This
        /// method can be used for debugging, logging, etc. of query behavior
        /// by intercepting the message stream to run arbitrary actions for
        /// messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// The type of the elements in the source observable sequence.
        /// </typeparam>
        /// <typeparam name="TNotification">
        /// The type of the notifications sent to the observer.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">
        /// Selector to project source elements to a notification object
        /// used to pass to the observer.
        /// </param>
        /// <param name="observer">
        /// An observer to invoke actions on as they propagate through to
        /// the result sequence.
        /// </param>
        /// <returns>
        /// The source query observerable sequence with the side-effecting
        /// The source query observable sequence with the side-effecting
        /// behavior applied.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>observer</c> is null.
        /// <c>source</c> or <c>selector</c> or <c>observer</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Do.ObserverSelector)]
        public static IAsyncReactiveQbservable<TSource> Do<TSource, TNotification>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, TNotification>> selector,
            IAsyncReactiveQbserver<TNotification> observer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TNotification)),
                    source.Expression,
                    selector,
                    observer.Expression));
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, and
        /// propagates all observer messages through the result sequence. This
        /// method can be used for debugging, logging, etc. of query behavior
        /// by intercepting the message stream to run arbitrary actions for
        /// messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="onNext">
        /// Action to invoke for each element in the query observable sequence.
        /// </param>
        /// <param name="onError">
        /// Action to invoke if an error is encountered in the query observable
        /// sequence.
        /// </param>
        /// <returns>
        /// The source query observerable sequence with the side-effecting
        /// behavior applied.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>onNext</c> or <c>onError</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Do.OnNextOnError)]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Action<TSource>> onNext,
            Expression<Action<Exception>> onError)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    onNext,
                    onError));
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, and
        /// propagates all observer messages through the result sequence. This
        /// method can be used for debugging, logging, etc. of query behavior
        /// by intercepting the message stream to run arbitrary actions for
        /// messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="onNext">
        /// Action to invoke for each element in the query observable sequence.
        /// </param>
        /// <param name="onCompleted">
        /// Action to invoke if the query observable sequence completes.
        /// </param>
        /// <returns>
        /// The source query observerable sequence with the side-effecting
        /// behavior applied.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>onNext</c> or <c>onCompleted</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Do.OnNextOnCompleted)]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Action<TSource>> onNext,
            Expression<Action> onCompleted)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    onNext,
                    onCompleted));
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, and
        /// propagates all observer messages through the result sequence. This
        /// method can be used for debugging, logging, etc. of query behavior
        /// by intercepting the message stream to run arbitrary actions for
        /// messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="onNext">
        /// Action to invoke for each element in the query observable sequence.
        /// </param>
        /// <param name="onError">
        /// Action to invoke if an error is encountered in the query observable
        /// sequence.
        /// </param>
        /// <param name="onCompleted">
        /// Action to invoke if the query observable sequence completes.
        /// </param>
        /// <returns>
        /// The source query observerable sequence with the side-effecting
        /// behavior applied.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>onNext</c> or <c>onError</c> or <c>onCompleted</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Do.AllActions)]
        public static IAsyncReactiveQbservable<TSource> Do<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Action<TSource>> onNext,
            Expression<Action<Exception>> onError,
            Expression<Action> onCompleted)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    onNext,
                    onError,
                    onCompleted));
        }

        /// <summary>
        /// Returns a query observable sequence that has no elements.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of empty sequence to be produced.
        /// </typeparam>
        /// <param name="context">
        /// Context used to construct the IAsyncReactiveQbservable data source.
        /// </param>
        /// <returns>
        /// A query observable sequence that completes when subscribed to.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>context</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.Empty.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> Empty<TSource>(this IReactiveProxy context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // TODO: Code-gen will emit this in the future.
            return context.GetObservable<TSource>(new Uri(Remoting.Client.Constants.Observable.Empty.NoArgument));
        }

        /// <summary>
        /// Runs the specified finally action upon termination or cancellation of each subscription
        /// to the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">The source sequence to run a finally action for.</param>
        /// <param name="finallyAction">
        /// The action to run upon termination of cancellation of source subscriptions.
        /// </param>
        /// <returns>The sequence with the finally action policy applied.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null, or <c>finallyAction</c> is null
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Finally.Action)]
        public static IAsyncReactiveQbservable<TSource> Finally<TSource>(
            this IAsyncReactiveQbservable<TSource> source, Expression<Action> finallyAction)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (finallyAction == null)
            {
                throw new ArgumentNullException(nameof(finallyAction));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    finallyAction));
        }

        /// <summary>
        /// First returns the first value from a sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <returns>
        /// A queryable observable sequence which contains only the first element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, an InvalidOperationException is propagated to signal the
        /// stream was empty.
        /// </returns>
        /// <remarks>
        /// This operator is semantically close to Take(1), modulo the
        /// exception behavior.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.FirstAsync.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> First<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// FirstAsync returns the first value from a sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <returns>
        /// A queryable observable sequence which contains only the first element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, an InvalidOperationException is propagated to signal the
        /// stream was empty.
        /// </returns>
        /// <remarks>
        /// This operator is semantically close to Take(1), modulo the
        /// exception behavior.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.FirstAsync.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> FirstAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// First returns the first value from a sequence that passes the
        /// filter predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition.
        /// </param>
        /// <returns>
        /// A queryable observable sequence which contains only the first element
        /// of the source sequence after applying the filter predicate. If the
        /// source sequence does not contain any elements, an InvalidOperationException
        /// is propagated to signal the stream was empty.
        /// </returns>
        /// <remarks>
        /// This operator is semantically close to Take(1), modulo the
        /// exception behavior.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.FirstAsync.Func)]
        public static IAsyncReactiveQbservable<TSource> First<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// FirstAsync returns the first value from a sequence that passes the
        /// filter predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition.
        /// </param>
        /// <returns>
        /// A queryable observable sequence which contains only the first element
        /// of the source sequence after applying the filter predicate. If the
        /// source sequence does not contain any elements, an InvalidOperationException
        /// is propagated to signal the stream was empty.
        /// </returns>
        /// <remarks>
        /// This operator is semantically close to Take(1), modulo the
        /// exception behavior.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.FirstAsync.Func)]
        public static IAsyncReactiveQbservable<TSource> FirstAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// FirstOrDefault returns the first value from a sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <returns>
        /// A queryable observable sequence which contains only the first element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, a default value is propagated.
        /// </returns>
        /// <remarks>
        /// This operator is semantically close to Take(1), modulo the
        /// default value propagation behavior.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.FirstOrDefaultAsync.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> FirstOrDefault<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// FirstOrDefaultAsync returns the first value from a sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <returns>
        /// A queryable observable sequence which contains only the first element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, a default value is propagated.
        /// </returns>
        /// <remarks>
        /// This operator is semantically close to Take(1), modulo the
        /// default value propagation behavior.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.FirstOrDefaultAsync.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> FirstOrDefaultAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// FirstOrDefault returns the first value from a sequence that passes
        /// the filter predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition.
        /// </param>
        /// <returns>
        /// A queryable observable sequence which contains only the first element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, a default value is propagated.
        /// </returns>
        /// <remarks>
        /// This operator is semantically close to Take(1), modulo the
        /// default value propagation behavior.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.FirstOrDefaultAsync.Func)]
        public static IAsyncReactiveQbservable<TSource> FirstOrDefault<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// FirstOrDefaultAsync returns the first value from a sequence that passes
        /// the filter predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition.
        /// </param>
        /// <returns>
        /// A queryable observable sequence which contains only the first element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, a default value is propagated.
        /// </returns>
        /// <remarks>
        /// This operator is semantically close to Take(1), modulo the
        /// default value propagation behavior.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.FirstOrDefaultAsync.Func)]
        public static IAsyncReactiveQbservable<TSource> FirstOrDefaultAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Groups the elements in the observable sequence by a computed key.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the keys to group by.</typeparam>
        /// <param name="source">Observable sequence whose elements to group.</param>
        /// <param name="keySelector">Key selector function to obtain a key for each element.</param>
        /// <returns>Observable sequence of groups, which are observable sequences themselves.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.GroupBy.Key)]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector));
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
        [KnownResource(Remoting.Client.Constants.Observable.GroupBy.KeyElement)]
        public static IAsyncReactiveQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (elementSelector == null)
            {
                throw new ArgumentNullException(nameof(elementSelector));
            }

            return source.Provider.CreateQbservable<IAsyncReactiveGroupedQbservable<TKey, TElement>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)),
                    source.Expression,
                    keySelector,
                    elementSelector));
        }

        /// <summary>
        /// Suppresses all elements of the source observable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence whose elements to suppress.</param>
        /// <returns>Observable sequence with no element.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.IgnoreElements.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> IgnoreElements<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Flattens an observable sequence of observable sequences into a
        /// single observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="sources">
        /// The observable sequence of inner observable sequences.</param>
        /// <returns>
        /// The flattened observable sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>sources</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.Merge.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> Merge<TSource>(
            this IAsyncReactiveQbservable<IAsyncReactiveObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return sources.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    sources.Expression));
        }

        /// <summary>
        /// Merges the elements in the specified observable sequences.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="first">The first observable sequence whose elements should be merged.</param>
        /// <param name="second">The second observable sequence whose elements should be merged.</param>
        /// <returns>Observable sequence containing the elements of the specified source sequences.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>first</c> is null or <c>second</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.Merge.Binary)]
        public static IAsyncReactiveQbservable<TSource> Merge<TSource>(
            this IAsyncReactiveQbservable<TSource> first, IAsyncReactiveQbservable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    first.Expression,
                    second.Expression));
        }

        /// <summary>
        /// Unsubscribes from and resubscribes to the source upon receiving an
        /// OnError message.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">
        /// The source observable sequence.
        /// </param>
        /// <returns>
        /// Observable sequence with retry behavior applied.
        /// </returns>
        [KnownResource(Remoting.Client.Constants.Observable.Retry.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> Retry<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Unsubscribes from and resubscribes to the source upon receiving an
        /// OnError message, with the specified maximum retry count.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">
        /// The source observable sequence.
        /// </param>
        /// <param name="retryCount">
        /// Maximum number of times to retry.
        /// </param>
        /// <returns>
        /// Observable sequence with retry behavior applied.
        /// </returns>
        [KnownResource(Remoting.Client.Constants.Observable.Retry.Count)]
        public static IAsyncReactiveQbservable<TSource> Retry<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            int retryCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(retryCount, typeof(int))));
        }

        /// <summary>
        /// Samples the observable sequence at each interval. Upon each
        /// sampling tick, the latest element (if any) in the source sequence
        /// during the last sampling interval is sent to the resulting sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">The source sequence to sample.</param>
        /// <param name="interval">
        /// Interval at which to sample. If this value is equal to TimeSpan.Zero,
        /// the scheduler will continuously sample the stream.
        /// </param>
        /// <returns>
        /// The sampled observable sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>interval</c> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for interval doesn't guarantee all
        /// source sequence elements will be preserved. This is a side-effect
        /// of the asynchrony introduced by the scheduler, where the sampling
        /// action may not execute immediately, despite the TimeSpan.Zero due
        /// time.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Sample.TimeSpan)]
        public static IAsyncReactiveQbservable<TSource> Sample<TSource>(
            this IAsyncReactiveQbservable<TSource> source, TimeSpan interval)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (interval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(interval)));
        }

        /// <summary>
        /// Samples the observable sequence when signaled. Upon each sampling
        /// signal, the latest element (if any) in the source sequence during
        /// the last sampling interval is sent to the resulting sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TOther">
        /// The type of the elements in the sampler sequence.
        /// </typeparam>
        /// <param name="source">The source sequence to sample.</param>
        /// <param name="sampler">
        /// A sequence that triggers when to sample from the source sequence.
        /// </param>
        /// <returns>
        /// The sampled observable sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>sampler</c> is null
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Sample.Observable)]
        public static IAsyncReactiveQbservable<TSource> Sample<TSource, TOther>(
            this IAsyncReactiveQbservable<TSource> source,
            IAsyncReactiveQbservable<TOther> sampler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (sampler == null)
            {
                throw new ArgumentOutOfRangeException(nameof(sampler));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)),
                    source.Expression,
                    sampler.Expression));
        }

        /// <summary>
        /// Projects each element of an observable sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements in the result query obervable sequence,
        /// obtained by running the selector function for each element in the
        /// source query observable sequence.
        /// </typeparam>
        /// <param name="source">
        /// A query observable sequence of elements to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each source element.
        /// </param>
        /// <returns>
        /// A query observable sequence whose elements are the result of
        /// invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>selector</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Select.Func)]
        public static IAsyncReactiveQbservable<TResult> Select<TSource, TResult>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Provider.CreateQbservable<TResult>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    selector));
        }

        /// <summary>
        /// Projects each element of an observable sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements in the result query obervable sequence,
        /// obtained by running the selector function for each element in the
        /// source query observable sequence.
        /// </typeparam>
        /// <param name="source">
        /// A query observable sequence of elements to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        /// A transform function to apply to each source element that receives a
        /// sequence number as a parameter.
        /// </param>
        /// <returns>
        /// A query observable sequence whose elements are the result of
        /// invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>selector</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Select.IndexedFunc)]
        public static IAsyncReactiveQbservable<TResult> Select<TSource, TResult>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, int, TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Provider.CreateQbservable<TResult>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    selector));
        }

        /// <summary>
        /// Projects each element of an observable sequence to an observable
        /// sequence and merges the resulting observable sequences into one
        /// observable sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements in the projected inner sequences and the
        /// the elements in the merged result sequence.</typeparam>
        /// <param name="source">
        /// An observable sequence of elements to project.</param>
        /// <param name="selector">
        /// A transform function to apply to each element.</param>
        /// <returns>
        /// An observable sequence whose elements are the result of invoking
        /// the one-to-many transform function on each element of the input
        /// sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>selector</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SelectMany.Func)]
        public static IAsyncReactiveQbservable<TResult> SelectMany<TSource, TResult>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, IAsyncReactiveObservable<TResult>>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Provider.CreateQbservable<TResult>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    selector));
        }

        /// <summary>
        /// Projects each element of a query observable sequence to an
        /// enumerable sequence, invokes the result selector for the source
        /// element and each of the corresponding inner sequence's elements,
        /// and merges the results into one query observable sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <typeparam name="TCollection">
        /// The type of the elements in the projected intermediate enumerable
        /// sequences.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements in the result sequence, obtained by using
        /// the selector to combine source sequence elements with their
        /// corresponding intermediate sequence elements.
        /// </typeparam>
        /// <param name="source">
        /// A query observable sequence of elements to project.
        /// </param>
        /// <param name="collectionSelector">
        /// A transform function to apply to each element of the input query
        /// observable sequence.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to each element of the intermediate
        /// query observable sequence.
        /// </param>
        /// <returns>
        /// A query observable sequence whose elements are the result of
        /// invoking the one-to-many transform function collectionSelector on
        /// each element of the input sequence and then mapping each of those
        /// sequence elements and their corresponding source element to a
        /// result element.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>collectionSelector</c> or <c>resultSelector</c>
        /// is null.
        /// </exception>
        /// <remarks>
        /// The projected sequences are enumerated synchronously within the
        /// OnNext call of the source sequence. In order to do a concurrent,
        /// non-blocking merge, change the selector to return an observable
        /// sequence obtained using the ToObservable conversion.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.SelectMany.FuncFunc)]
        public static IAsyncReactiveQbservable<TResult> SelectMany<TSource, TCollection, TResult>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, IAsyncReactiveObservable<TCollection>>> collectionSelector,
            Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (collectionSelector == null)
            {
                throw new ArgumentNullException(nameof(collectionSelector));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return source.Provider.CreateQbservable<TResult>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)),
                    source.Expression,
                    collectionSelector,
                    resultSelector));
        }

        /// <summary>
        /// Compares two observable sequences for pairwise equality of elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
        /// <param name="left">Left sequence whose elements to compare.</param>
        /// <param name="right">Right sequence whose elements to compare.</param>
        /// <returns>Observable sequence containing a single Boolean value indicating whether the two sequences are equal in length and have pairwise equality of elements.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.SequenceEqual.NoArgument)]
        public static IAsyncReactiveQbservable<bool> SequenceEqual<TSource>(
            this IAsyncReactiveQbservable<TSource> left,
            IAsyncReactiveQbservable<TSource> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return left.Provider.CreateQbservable<bool>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    left.Expression,
                    GetSourceExpression(right)));
        }

        /// <summary>
        /// Single returns the only value in a sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <returns>
        /// A queryable observable sequence which contains the only element
        /// of the source sequence. If the source sequence does not contain any
        /// elements or if the source sequence contains more than one element,
        /// an InvalidOperationException is propagated to signal the stream
        /// was not containing exactly one single element.
        /// </returns>
        /// <remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.SingleAsync.NoArgument)]
#pragma warning disable CA1720 // Identifier contains type name
        public static IAsyncReactiveQbservable<TSource> Single<TSource>(
#pragma warning restore CA1720 // Identifier contains type name
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// SingleAsync returns the only value in a sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <returns>
        /// A queryable observable sequence which contains the only element
        /// of the source sequence. If the source sequence does not contain any
        /// elements or if the source sequence contains more than one element,
        /// an InvalidOperationException is propagated to signal the stream
        /// was not containing exactly one single element.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SingleAsync.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> SingleAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Single returns the only value in a sequence that passes the
        /// filter predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition.
        /// </param>
        /// <returns>
        /// A queryable observable sequence which contains the only element
        /// of the source sequence after applying the filter predicate. If the
        /// source sequence does not contain any elements or if the source
        /// contains more than one element, an InvalidOperationException
        /// is propagated to signal the stream was not containing exactly one
        /// single element.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SingleAsync.Func)]
#pragma warning disable CA1720 // Identifier contains type name
        public static IAsyncReactiveQbservable<TSource> Single<TSource>(
#pragma warning restore CA1720 // Identifier contains type name
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// SingleAsync returns the only value in a sequence that passes the
        /// filter predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition.
        /// </param>
        /// <returns>
        /// A queryable observable sequence which contains the only element
        /// of the source sequence after applying the filter predicate. If the
        /// source sequence does not contain any elements or if the source
        /// contains more than one element, an InvalidOperationException
        /// is propagated to signal the stream was not containing exactly one
        /// single element.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SingleAsync.Func)]
        public static IAsyncReactiveQbservable<TSource> SingleAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// SingleOrDefault returns the only value in a sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <returns>
        /// A queryable observable sequence which contains the only element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, then a default value is propagated. If the source
        /// contains more than one element, then an InvalidOperationException
        /// is propagated to signal the stream contained more than one
        /// element.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SingleOrDefaultAsync.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> SingleOrDefault<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// SingleOrDefaultAsync returns the only value in a sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <returns>
        /// A queryable observable sequence which contains the only element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, then a default value is propagated. If the source
        /// contains more than one element, then an InvalidOperationException
        /// is propagated to signal the stream contained more than one
        /// element.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SingleOrDefaultAsync.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> SingleOrDefaultAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// SingleOrDefault returns the only value in a sequence that passes
        /// the filter predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition.
        /// </param>
        /// <returns>
        /// A queryable observable sequence which contains the only element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, then a default value is propagated. If the source
        /// sequence contains more than one element, then an
        /// InvalidOperationException is propagated to signal the stream
        /// contained more than one element.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SingleOrDefaultAsync.Func)]
        public static IAsyncReactiveQbservable<TSource> SingleOrDefault<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// SingleOrDefaultAsync returns the only value in a sequence that passes
        /// the filter predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The source query observable sequence.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition.
        /// </param>
        /// <returns>
        /// A queryable observable sequence which contains the only element
        /// of the source sequence. If the source sequence does not contain any
        /// elements, then a default value is propagated. If the source
        /// sequence contains more than one element, then an
        /// InvalidOperationException is propagated to signal the stream
        /// contained more than one element.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SingleOrDefaultAsync.Func)]
        public static IAsyncReactiveQbservable<TSource> SingleOrDefaultAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Bypasses a specified number of values in an observable sequence and
        /// then returns the remaining values.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The sequence to take elements from.</param>
        /// <param name="duration">
        /// Duration for skipping elements from the start of the sequence.
        /// </param>
        /// <returns>
        /// A queryable observable sequence that contains the elements that
        /// occur after the specified duration from the start of the source
        /// sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>duration</c> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// Specifying a TimeSpan.Zero value for duration doesn't guarantee
        /// that no elements will be dropped from the start of the source
        /// sequence. This is a side-effect of the asynchrony introduced by
        /// the scheduler, where the action that causes callbacks from the
        /// source sequence to be forwarded may not execute immediately,
        /// despite the TimeSpan.Zero due time.
        /// </para><para>
        /// Errors produced by the source sequence are always forwarded to the
        /// result sequence, even if the error occurs before the duration.
        /// </para>
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Skip.TimeSpan)]
        public static IAsyncReactiveQbservable<TSource> Skip<TSource>(
            this IAsyncReactiveQbservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (duration < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(duration)));
        }

        /// <summary>
        /// Bypasses a specified number of values in an observable sequence and
        /// then returns the remaining values.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">The sequence to take elements from.</param>
        /// <param name="count">
        /// Number of elements to skip from the start of the sequence.
        /// </param>
        /// <returns>
        /// A queryable observable sequence that contains the elements that
        /// occur after a specific number of elements from the start of the
        /// source sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>count</c> is less than zero.</exception>
        [KnownResource(Remoting.Client.Constants.Observable.Skip.Count)]
        public static IAsyncReactiveQbservable<TSource> Skip<TSource>(
            this IAsyncReactiveQbservable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count)));
        }

        /// <summary>
        /// Returns the values from the source queryable observable sequence
        /// only after the start time.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence to propogate the elements for.</param>
        /// <param name="startTime">
        /// Time to start taking elements from the source sequence. If this
        /// value is less than or equal to DateTimeOffset.UtcNow, no elements
        /// will be skipped.</param>
        /// <returns>
        /// A queryable observable sequence containing the elements of the
        /// source sequence starting from the start time.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null</exception>
        /// <remarks>
        /// Errors produced by the source sequence are always forwarded to the
        /// result sequence, even if the error occurs before the <c>startTime</c>.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.SkipUntil.DateTimeOffset)]
        public static IAsyncReactiveQbservable<TSource> SkipUntil<TSource>(
            this IAsyncReactiveQbservable<TSource> source, DateTimeOffset startTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(startTime)));
        }

        /// <summary>
        /// Returns the values from the source queryable observable sequence
        /// only after the triggering sequence has signaled.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TOther">The type of the elements in the triggering source sequence.</typeparam>
        /// <param name="source">The source sequence to propogate the elements for.</param>
        /// <param name="triggeringSource">
        /// The triggering sequence; elements are skipped from the source sequence until
        /// the first element from the triggering sequence is received.</param>
        /// <returns>
        /// A queryable observable sequence containing the elements of the
        /// source sequence starting after the triggering source signals.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>triggeringSource</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.SkipUntil.Observable)]
        public static IAsyncReactiveQbservable<TSource> SkipUntil<TSource, TOther>(
            this IAsyncReactiveQbservable<TSource> source,
            IAsyncReactiveQbservable<TOther> triggeringSource)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (triggeringSource == null)
            {
                throw new ArgumentNullException(nameof(triggeringSource));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)),
                    source.Expression,
                    triggeringSource.Expression));
        }

        /// <summary>
        /// Bypasses elements in a query observable sequence as long as a
        /// specified condition is true and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observabl sequence.
        /// </typeparam>
        /// <param name="source">
        /// A query observable sequence to return elements from.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition.
        /// </param>
        /// <returns>
        /// A query observable sequence that contains the elements from the
        /// input sequence starting at the first element in the linear series
        /// that does not pass the test specified by predicate.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SkipWhile.Func)]
        public static IAsyncReactiveQbservable<TSource> SkipWhile<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Bypasses elements in a query observable sequence as long as a
        /// specified condition is true and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observabl sequence.
        /// </typeparam>
        /// <param name="source">
        /// A query observable sequence to return elements from.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition that receives
        /// a sequence counter for the number of elements encountered.
        /// </param>
        /// <returns>
        /// A query observable sequence that contains the elements from the
        /// input sequence starting at the first element in the linear series
        /// that does not pass the test specified by predicate.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null.
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.SkipWhile.IndexedFunc)]
        public static IAsyncReactiveQbservable<TSource> SkipWhile<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Prepends a sequence of values to an observable sequence with the
        /// specified source and values.
        /// </summary>
        /// <typeparam name="TSource">The type of the source</typeparam>
        /// <param name="source">The source sequence to prepend values to.</param>
        /// <param name="values">The values to prepend to the specified sequence.</param>
        /// <returns>The source sequence prepended with the specified values.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>values</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.StartWith.Array)]
        public static IAsyncReactiveQbservable<TSource> StartWith<TSource>(
            this IAsyncReactiveQbservable<TSource> source, params TSource[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.NewArrayInit(typeof(TSource), GetExpressions(values))));
        }

        /// <summary>
        /// Transforms an observable sequence of observable sequences into an
        /// observable sequence producing values only from the most recent
        /// observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="sources">
        /// The observable sequence of inner observable sequences.</param>
        /// <returns>
        /// The observable sequence that at any point in time produces the
        /// elements of the most recent inner observable sequence that has been
        /// received.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>sources</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.Switch.NoArgument)]
        public static IAsyncReactiveQbservable<TSource> Switch<TSource>(
            this IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return sources.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    sources.Expression));
        }

        /// <summary>
        /// Evaluates the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to subscribe to.</param>
        /// <param name="subscriptionId">The subscription id.</param>
        /// <returns>
        /// Task returning a subscription object that can be used to cancel the
        /// subscription, or an exception if the submission was unsuccessful.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><c>source</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observer.Nop)]
        public static Task<IAsyncReactiveQubscription> SubscribeAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Uri subscriptionId)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            var nop = source.Provider.CreateQbserver<TSource>(Expression.Parameter(typeof(IAsyncReactiveQbserver<TSource>), Remoting.Client.Constants.Observer.Nop));
            return source.SubscribeAsync(nop, subscriptionId, null, CancellationToken.None);
        }

        /// <summary>
        /// Evaluates the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to subscribe to.</param>
        /// <param name="subscriptionId">The subscription id.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// Task returning a subscription object that can be used to cancel the
        /// subscription, or an exception if the submission was unsuccessful.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><c>source</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observer.Nop)]
        public static Task<IAsyncReactiveQubscription> SubscribeAsync<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Uri subscriptionId,
            object state)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            var nop = source.Provider.CreateQbserver<TSource>(Expression.Parameter(typeof(IAsyncReactiveQbserver<TSource>), Remoting.Client.Constants.Observer.Nop));
            return source.SubscribeAsync(nop, subscriptionId, state, CancellationToken.None);
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start
        /// of a queryable observable sequence
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">The sequence to take elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// A queryable observable sequence that contains the specified number
        /// of elements from the start of the input sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>count</c> is less than zero.</exception>
        [KnownResource(Remoting.Client.Constants.Observable.Take.Count)]
        public static IAsyncReactiveQbservable<TSource> Take<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(count, typeof(int))));
        }

        /// <summary>
        /// Takes elements for the specified duration from the start of the
        /// query observable source sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">The sequence to take elements from.</param>
        /// <param name="duration">
        /// The duration for taking elements from the start of the sequence.
        /// </param>
        /// <returns>
        /// A query observable sequence with the elements taken during the
        /// specified duration from the start of the source sequence.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>duration</c> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for duration doesn't guarantee an
        /// empty sequence will be returned. This is a side-effect of the
        /// asynchrony introduced by the scheduler, where the action that stops
        /// forwarding callbacks from the source sequence may not execute
        /// immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Take.TimeSpan)]
        public static IAsyncReactiveQbservable<TSource> Take<TSource>(
            this IAsyncReactiveQbservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (duration < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(duration)));
        }

        /// <summary>
        /// Returns the values from the source queryable observable sequence
        /// until the end time.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">The source sequence to propagate elements for.</param>
        /// <param name="endTime">
        /// Time to stop taking elements from the source sequence. If this
        /// value is less than or equal to DateTimeOffset.UtcNow, the result
        /// stream will complete immediately.
        /// </param>
        /// <returns>
        /// A queryable observable sequence containing the elements of the
        /// source sequence taken until the specified endtime.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.TakeUntil.DateTimeOffset)]
        public static IAsyncReactiveQbservable<TSource> TakeUntil<TSource>(
            this IAsyncReactiveQbservable<TSource> source, DateTimeOffset endTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(endTime)));
        }

        /// <summary>
        /// Returns the values from the source queryable observable sequence
        /// until the the queryable observable sequence produces a value.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <typeparam name="TOther">
        /// The type of the elements in the other observable sequence.
        /// </typeparam>
        /// <param name="source">
        /// The source query observable sequence to propagate elements for.
        /// </param>
        /// <param name="other">
        /// The query observable sequence that terminates propogation of
        /// elements of the source sequence.
        /// </param>
        /// <returns>
        /// A queryable observable sequence containing the elements of the
        /// source sequence up to the point the other sequence interrupted
        /// further propagation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>other</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.TakeUntil.Observable)]
        public static IAsyncReactiveQbservable<TSource> TakeUntil<TSource, TOther>(
            this IAsyncReactiveQbservable<TSource> source,
            IAsyncReactiveQbservable<TOther> other)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)),
                    source.Expression,
                    GetSourceExpression(other)));
        }

        /// <summary>
        /// Returns the values from the source queryable observable sequence
        /// as long as a specified condition has not been met.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">
        /// The source query observable sequence to propagate elements for.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition.
        /// </param>
        /// <returns>
        /// A queryable observable sequence containing the elements of the
        /// source sequence up to the point the provided condition is met.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.TakeWhile.Func)]
        public static IAsyncReactiveQbservable<TSource> TakeWhile<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Returns the values from the source queryable observable sequence
        /// as long as a specified condition has not been met.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source query observable sequence.
        /// </typeparam>
        /// <param name="source">
        /// The source query observable sequence to propagate elements for.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition that receives
        /// a counter parameter for the number of items encountered.
        /// </param>
        /// <returns>
        /// A queryable observable sequence containing the elements of the
        /// source sequence up to the point the provided condition is met.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.TakeWhile.IndexedFunc)]
        public static IAsyncReactiveQbservable<TSource> TakeWhile<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Ignores elements from an observable sequence which are followed by
        /// another element within a specified relative time duration.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">The source sequence to throttle.</param>
        /// <param name="dueTime">
        /// The throttling duration for each element.
        /// </param>
        /// <returns>The throttled sequence.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> is null
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>dueTime</c> is less than TimeSpan.Zero.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This operator throttles the source sequence by holding on to each
        /// element for the duration specified in dueTime. If another element
        /// is produced within this time window, the element is dropped and a
        /// new timer is started for the current element, repeating this whole
        /// process. For streams that never have gaps larger than or equal to
        /// dueTime between elements, the resulting stream won't produce any
        /// elements. In order to reduce the volume of a stream whilst
        /// guaranteeing the periodic production of elements, consider using
        /// the Observable.Sample set of operators.
        /// </para><para>
        /// Specifying a TimeSpan.Zero value for dueTime is not recommended but
        /// supported, causing throttling timers to be scheduled that are due
        /// immediately. However, this doesn't guarantee all elements will be
        /// retained in the result sequence. This is a side-effect of the
        /// asynchrony introduced by the scheduler, where the action to forward
        /// the current element may not execute immediately, despite the
        /// TimeSpan.Zero due time. In such cases, the next element may arrive
        /// before the scheduler gets a chance to run the throttling action.
        /// </para>
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Throttle.TimeSpan)]
        public static IAsyncReactiveQbservable<TSource> Throttle<TSource>(
            this IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dueTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(dueTime)));
        }

        /// <summary>
        /// Returns a query observable sequence that produces a single
        /// value at the specified absolute due time.
        /// </summary>
        /// <param name="context">
        /// Context used to construct the IAsyncReactiveQbservable
        /// data source.</param>
        /// <param name="dueTime">
        /// Absolute time at which to produce the value. If this value is less
        /// than or equal to DateTimeOffset.UtcNow, the timer will fire as soon
        /// as possible.</param>
        /// <returns>
        /// A query observable sequence that produces a value after the due
        /// time has elapsed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>context</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.Timer.DateTimeOffset)]
        public static IAsyncReactiveQbservable<long> Timer(this IReactiveProxy context, DateTimeOffset dueTime)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // TODO: Code-gen will emit this in the future.
            return context.GetObservable<DateTimeOffset, long>(new Uri(Remoting.Client.Constants.Observable.Timer.DateTimeOffset))(dueTime);
        }

        /// <summary>
        /// Returns a query observable sequence that produces a single
        /// value at the specified relative due time.
        /// </summary>
        /// <param name="context">
        /// Context used to construct the IAsyncReactiveQbservable
        /// data source.</param>
        /// <param name="dueTime">
        /// Relative time at which to produce the value. If this value is less
        /// than or equal to TimeSpan.Zero, the timer will fire as soon
        /// as possible.</param>
        /// <returns>
        /// A query observable sequence that produces a value after the due
        /// time has elapsed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>context</c> is null</exception>
        [KnownResource(Remoting.Client.Constants.Observable.Timer.TimeSpan)]
        public static IAsyncReactiveQbservable<long> Timer(this IReactiveProxy context, TimeSpan dueTime)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // TODO: Code-gen will emit this in the future.
            return context.GetObservable<TimeSpan, long>(new Uri(Remoting.Client.Constants.Observable.Timer.TimeSpan))(dueTime);
        }

        /// <summary>
        /// Returns a query observable sequence that periodically produces a
        /// value starting at the specified initial absolute due time.
        /// </summary>
        /// <param name="context">
        /// Context used to construct the IAsyncReactiveQbservable
        /// data source.</param>
        /// <param name="dueTime">
        /// Absolute time at which to produce the first value. If this value is
        /// less than or equal to DateTimeOffset.UtcNow, the timer will fire as
        /// soon as possible.</param>
        /// <param name="period">
        /// Period to produce subsequent values. If this value is equal to
        /// TimeSpan.Zero, the timer will recur as fast as possible.</param>
        /// <returns>
        /// A query observable sequence that produces a value at due time and
        /// then after each period.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>provider</c> is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>period</c> is less than TimeSpan.Zero.</exception>
        [KnownResource(Remoting.Client.Constants.Observable.Timer.DateTimeOffsetTimeSpan)]
        public static IAsyncReactiveQbservable<long> Timer(
            this ReactiveClientContext context,
            DateTimeOffset dueTime,
            TimeSpan period)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            // TODO: Code-gen will emit this in the future.
            return context.GetObservable<DateTimeOffset, TimeSpan, long>(
                new Uri(Remoting.Client.Constants.Observable.Timer.DateTimeOffsetTimeSpan))(dueTime, period);
        }

        /// <summary>
        /// Returns an observable sequence that produces a value after due time
        /// has elapsed and then after each period.
        /// </summary>
        /// <param name="context">
        /// Context used to construct the IAsyncReactiveQbservable
        /// data source.
        /// </param>
        /// <param name="dueTime">
        /// The relative time at which to produce the first value. If this
        /// value is less than or equal to TimeSpan.Zero, the timer will fire
        /// as soon as possible.
        /// </param>
        /// <param name="period">The period to produce subsequent values. If
        /// this value is equal to TimeSpan.Zero, the timer will recur as fast
        /// as possible.
        /// </param>
        /// <returns>
        /// A query observable sequence that produces a value after due time
        /// has elapsed and then after each period.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>provider</c> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>period</c> is less than TimeSpan.Zero.
        /// </exception>
        /// <remarks>
        /// The returned type parameter is always of type long. In Rx, there's
        /// no interpretation other than it being guaranteed to monotonically
        /// increase. The Timer and Interval methods are often used in Rx to
        /// provide pulses. If one wants to associate a time stamp with it, one
        /// would use the Timestamp operator to compose it in, providing a time
        /// source based on an IScheduler. This followed our design point of
        /// sequences not carrying time info unless composed in explicitly. We
        /// can do something different for RIPP, but time is a complex animal
        /// (whose clock is being used to provide such values, especially when
        /// distributed across machines?).
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Timer.TimeSpanTimeSpan)]
        public static IAsyncReactiveQbservable<long> Timer(
            this ReactiveClientContext context,
            TimeSpan dueTime,
            TimeSpan period)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            // TODO: Code-gen will emit this in the future.
            return context.GetObservable<TimeSpan, TimeSpan, long>(
                new Uri(Remoting.Client.Constants.Observable.Timer.TimeSpanTimeSpan))(dueTime, period);
        }

        /// <summary>
        /// Casts an instance from one type to another.
        /// </summary>
        /// <typeparam name="TInput">The input type.</typeparam>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="input">The instance to cast.</param>
        /// <returns>The casted instance.</returns>
        /// <remarks>
        /// This operator performs an unsafe cast from <c>TInput</c> to
        /// <c>System.Object</c> to <c>TOutput</c>.  It is intended to
        /// be used for definitions of operators, which calls to it will be
        /// rewritten to an invocation of the known resource URI.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Identity)]
        public static TOutput To<TInput, TOutput>(this TInput input)
        {
            return (TOutput)(object)input;
        }

        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">An observable sequence whose elements to filter.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition.
        /// </param>
        /// <returns>
        /// An observable sequence that contains elements from the input
        /// sequence that satisfy the condition.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Where.Func)]
        public static IAsyncReactiveQbservable<TSource> Where<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        predicate));
        }

        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">An observable sequence whose elements to filter.</param>
        /// <param name="predicate">
        /// A function to test each source element for a condition that receives a
        /// counter tracking the number of elements that have been seen.
        /// </param>
        /// <returns>
        /// An observable sequence that contains elements from the input
        /// sequence that satisfy the condition.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>predicate</c> is null
        /// </exception>
        [KnownResource(Remoting.Client.Constants.Observable.Where.IndexedFunc)]
        public static IAsyncReactiveQbservable<TSource> Where<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        predicate));
        }

        /// <summary>
        /// Partitions the source sequence into windows based on the specified window duration.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="duration">Duration of a window.</param>
        /// <returns>Observable sequence containing windows of the specified duration over the source sequence.</returns>
        /// <remarks>
        /// Windows in the resulting sequence should be subscribed to immediately when they are produced in order to avoid time gaps.
        /// The current implementation of the window operators guards against invalid subscription timing on produced windows. For
        /// example, when writing xs.Window(t).SelectMany(w => w.DelaySubscription(u)) an error will occur because the window is not
        /// subscribed to as soon as it gets produced. The alternative of xs.Window(t).SelectMany(w => w.SkipUntil(u)) does work.
        /// This behavior is put in place to avoid typical mistakes in writing queries as observed in classic Rx.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Window.TimeDuration)]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(duration, typeof(TimeSpan))
                )
            );
        }

        /// <summary>
        /// Partitions the source sequence into windows based on the specified window duration and time shift.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="duration">Duration of a window.</param>
        /// <param name="shift">Time shift between one window and the next.</param>
        /// <returns>Observable sequence containing windows of the specified duration over the source sequence.</returns>
        /// <remarks>
        /// Windows in the resulting sequence should be subscribed to immediately when they are produced in order to avoid time gaps.
        /// The current implementation of the window operators guards against invalid subscription timing on produced windows. For
        /// example, when writing xs.Window(t, s).SelectMany(w => w.DelaySubscription(u)) an error will occur because the window is not
        /// subscribed to as soon as it gets produced. The alternative of xs.Window(t, s).SelectMany(w => w.SkipUntil(u)) does work.
        /// This behavior is put in place to avoid typical mistakes in writing queries as observed in classic Rx.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Window.TimeDurationShift)]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, TimeSpan shift)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            if (shift <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(shift));
            }

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(duration, typeof(TimeSpan)),
                    Expression.Constant(shift, typeof(TimeSpan))
                )
            );
        }

        /// <summary>
        /// Partitions the source sequence into windows based on the specified window length.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="count">Number of elements per window.</param>
        /// <returns>Observable sequence containing windows of the specified length over the source sequence.</returns>
        /// <remarks>
        /// Windows in the resulting sequence should be subscribed to immediately when they are produced in order to avoid time gaps.
        /// The current implementation of the window operators guards against invalid subscription timing on produced windows. For
        /// example, when writing xs.Window(n).SelectMany(w => w.DelaySubscription(u)) an error will occur because the window is not
        /// subscribed to as soon as it gets produced. The alternative of xs.Window(n).SelectMany(w => w.SkipUntil(u)) does work.
        /// This behavior is put in place to avoid typical mistakes in writing queries as observed in classic Rx.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Window.Count)]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

        /// <summary>
        /// Partitions the source sequence into windows based on the specified window length and element skip count.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="count">Number of elements per window.</param>
        /// <param name="skip">Offset of the number of elements between the start of one window and the next.</param>
        /// <returns>Observable sequence containing windows of the specified length over the source sequence.</returns>
        /// <remarks>
        /// Windows in the resulting sequence should be subscribed to immediately when they are produced in order to avoid time gaps.
        /// The current implementation of the window operators guards against invalid subscription timing on produced windows. For
        /// example, when writing xs.Window(n, s).SelectMany(w => w.DelaySubscription(u)) an error will occur because the window is not
        /// subscribed to as soon as it gets produced. The alternative of xs.Window(n, s).SelectMany(w => w.SkipUntil(u)) does work.
        /// This behavior is put in place to avoid typical mistakes in writing queries as observed in classic Rx.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Window.CountSkip)]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, int count, int skip)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (skip <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count, typeof(int)),
                    Expression.Constant(skip, typeof(int))
                )
            );
        }

        /// <summary>
        /// Partitions the source sequence into windows that are closed when they're full or a given amount of time has elapsed.
        /// A useful real-world analogy of this overload is the behavior of a ferry leaving the dock when all seats are taken, or at the scheduled time of departure, whichever event occurs first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to generate windows for.</param>
        /// <param name="duration">Maximum duration of a window.</param>
        /// <param name="count">Maximum number of elements per window.</param>
        /// <returns>Observable sequence containing windows over the source sequence.</returns>
        /// <remarks>
        /// Windows in the resulting sequence should be subscribed to immediately when they are produced in order to avoid time gaps.
        /// The current implementation of the window operators guards against invalid subscription timing on produced windows. For
        /// example, when writing xs.Window(n, s).SelectMany(w => w.DelaySubscription(u)) an error will occur because the window is not
        /// subscribed to as soon as it gets produced. The alternative of xs.Window(n, s).SelectMany(w => w.SkipUntil(u)) does work.
        /// This behavior is put in place to avoid typical mistakes in writing queries as observed in classic Rx.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Window.TimeCount)]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<TSource>> Window<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan duration, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(duration, typeof(TimeSpan)),
                    Expression.Constant(count, typeof(int))
                )
            );
        }

        /// <summary>
        /// Converts an IObservable to an IAsyncReactiveQbservable for define operations.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">An observable.</param>
        /// <returns>Always throws exception; should only be used in definition expressions.</returns>
        /// <exception cref="System.NotImplementedException">Always thrown; see remarks.</exception>
        /// <remarks>
        /// This method is only used for definition purposes.  It rewrites the the method
        /// call expression into an invocation of the known resource URI with the source
        /// parameter.  The URI maps to a built-in operator.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Observable.Identity.Observable)]
        public static IAsyncReactiveQbservable<TSource> AsQbservable<TSource>(
            this IObservable<TSource> source)
        {
            // This should never be called.
            throw new NotImplementedException("This operator should only be used in the context of an expression in a call to DefineObservableAsync.");
        }

        /// <summary>
        /// Converts an IObserver to an IAsyncReactiveQbserver for define operations.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="observer">An observer.</param>
        /// <returns>Always throws exception; should only be used in definition expressions.</returns>
        /// <exception cref="System.NotImplementedException">Always thrown; see remarks.</exception>
        /// <remarks>
        /// This method is only used for definition purposes.  It rewrites the the method
        /// call expression into an invocation of the known resource URI with the source
        /// parameter.  The URI maps to a built-in operator.
        /// </remarks>
        [KnownResource(Remoting.Client.Constants.Identity)]
        public static IAsyncReactiveQbserver<TSource> AsQbserver<TSource>(
            this IObserver<TSource> observer)
        {
            // This should never be called.
            throw new NotImplementedException("This operator should only be used in the context of an expression in a call to DefineObserverAsync.");
        }

        /// <summary>
        /// Gets the expressions.
        /// </summary>
        /// <typeparam name="TSource">the type of the source</typeparam>
        /// <param name="values">The values.</param>
        /// <returns>the expressions</returns>
        private static IEnumerable<Expression> GetExpressions<TSource>(IEnumerable<TSource> values)
        {
            foreach (var value in values)
            {
                if (value is IExpressible expr)
                {
                    yield return expr.Expression;
                }
                else
                {
                    yield return Expression.Constant(value, typeof(TSource));
                }
            }
        }

        /// <summary>
        /// Gets an expression representing the source sequence.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">The source.</param>
        /// <returns>an expression representing the source sequence</returns>
        private static Expression GetSourceExpression<TSource>(
            IAsyncReactiveObservable<TSource> source)
        {
            return
                source is IAsyncReactiveQbservable<TSource> q
                ? q.Expression
                : Expression.Constant(source, typeof(IAsyncReactiveObservable<TSource>));
        }
    }
}
