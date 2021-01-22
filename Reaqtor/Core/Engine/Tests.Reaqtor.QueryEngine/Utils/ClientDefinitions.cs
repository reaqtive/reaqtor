// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;

using Reaqtor;

namespace Tests.Reaqtor.QueryEngine
{
    public static class ClientDefinitions
    {
        #region Builtins
        [KnownResource("rx:/observable/id")]
        public static IReactiveQbservable<T> AsQbservable<T>(this IObservable<T> source)
        {
            // This should never be called
            throw new NotImplementedException();
        }

        [KnownResource("rx:/subscribable/id")]
        public static IReactiveQbservable<T> AsQbservable<T>(this ISubscribable<T> source)
        {
            // This should never be called
            throw new NotImplementedException();
        }

        [KnownResource("rx:/observer/id")]
        public static IReactiveQbserver<T> AsQbserver<T>(this IObserver<T> observer)
        {
            // This should never be called
            throw new NotImplementedException();
        }

        [KnownResource("rx://builtin/id")]
        public static ISubscribable<T> AsSubscribable<T>(this IReactiveQbservable<T> source)
        {
            // This should never be called
            throw new NotImplementedException();
        }

        [KnownResource("rx://builtin/id")]
        public static TOuput To<TInput, TOuput>(this TInput observer)
        {
            // This should never be called
            throw new NotImplementedException();
        }

        public static IReactiveQubject<T, T> CreateStream<T>(this IReactive context, Uri streamUri)
        {
            return context.GetStreamFactory<T, T>(new Uri("rx://subject/create")).Create(streamUri, null);
        }

        /// <summary>
        /// Gets the expression representation for the elements in the given sequence. If an element is
        /// IExpressible, the element's self-describing expression is used; otherwise, a constant
        /// expression containing the value is used.
        /// </summary>
        /// <remarks>Copied and pasted directly from RIPP codebase (specifically Operators.cs).</remarks>
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

        #endregion

        #region Atoms

        [KnownResource("rx://observable/empty")]
        public static IReactiveQbservable<T> Empty<T>(this IReactive service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            return service.GetObservable<T>(new Uri("rx://observable/empty"));
        }

        [KnownResource("rx://observable/never")]
        public static IReactiveQbservable<T> Never<T>(this IReactive service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            return service.GetObservable<T>(new Uri("rx://observable/never"));
        }

        [KnownResource("rx://observers/nop")]
        public static IReactiveQbserver<T> Nop<T>(this IReactive service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            Expression<Func<IReactiveQbserver<T>>> e = () => NopObserver<T>.Instance.AsQbserver();
            return service.Provider.CreateQbserver<T>(e.Body);
        }

        [KnownResource("rx://observable/return")]
        public static IReactiveQbservable<T> Return<T>(this IReactive service, T value)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            return service.GetObservable<T, T>(new Uri("rx://observable/return"))(value);
        }

        #endregion

        #region Filtering

        #region Where
        [KnownResource("rx://observable/filter")]
        public static IReactiveQbservable<T> Where<T>(this IReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        [KnownResource("rx://observable/filter/index")]
        public static IReactiveQbservable<T> Where<T>(this IReactiveQbservable<T> source, Expression<Func<T, int, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        #endregion

        #region Take/Skip
        [KnownResource("rx://observable/take")]
        public static IReactiveQbservable<T> Take<T>(this IReactiveQbservable<T> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

        [KnownResource("rx://observable/take/while")]
        public static IReactiveQbservable<T> TakeWhile<T>(this IReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        [KnownResource("rx://observable/take/while/index")]
        public static IReactiveQbservable<T> TakeWhile<T>(this IReactiveQbservable<T> source, Expression<Func<T, int, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        [KnownResource("rx://observable/take/until")]
        public static IReactiveQbservable<TSource> TakeUntil<TSource, TOther>(this IReactiveQbservable<TSource> source, IReactiveQbservable<TOther> other)
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
                    other.Expression
                )
            );
        }

        [KnownResource("rx://observable/skip")]
        public static IReactiveQbservable<T> Skip<T>(this IReactiveQbservable<T> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

        [KnownResource("rx://observable/skip/until")]
        public static IReactiveQbservable<TSource> SkipUntil<TSource, TOther>(this IReactiveQbservable<TSource> source, IReactiveQbservable<TOther> triggeringSource)
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
                    triggeringSource.Expression
                )
            );
        }

        [KnownResource("rx://observable/skip/until/datetimeoffset")]
        public static IReactiveQbservable<TSource> TakeUntil<TSource>(
            this IReactiveQbservable<TSource> source, DateTimeOffset endTime)
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

        #endregion

        #region DistinctUntilChanged
        [KnownResource("rx://observable/distinctuntilchanged")]
        public static IReactiveQbservable<T> DistinctUntilChanged<T, TKey>(this IReactiveQbservable<T> source, Expression<Func<T, TKey>> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(TKey)),
                        source.Expression,
                        keySelector));
        }
        #endregion

        #region FirstAsync/FirstOrDefaultAsync

        [KnownResource("rx://observable/firstasync")]
        public static IReactiveQbservable<T> FirstAsync<T>(this IReactiveQbservable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression));
        }

        [KnownResource("rx://observable/firstasync/filter")]
        public static IReactiveQbservable<T> FirstAsync<T>(this IReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
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
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression,
                        predicate));
        }

        [KnownResource("rx://observable/firstordefaultasync")]
        public static IReactiveQbservable<T> FirstOrDefaultAsync<T>(this IReactiveQbservable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression));
        }

        [KnownResource("rx://observable/firstordefaultasync/filter")]
        public static IReactiveQbservable<T> FirstOrDefaultAsync<T>(this IReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
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
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression,
                        predicate));
        }

        #endregion

        #region Throttle
        [KnownResource("rx://observable/throttle")]
        public static IReactiveQbservable<TSource> Throttle<TSource, TThrottleResult>(
            this IReactiveQbservable<TSource> source,
            Expression<Func<TSource, IReactiveQbservable<TThrottleResult>>> throttleSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (throttleSelector == null)
            {
                throw new ArgumentNullException(nameof(throttleSelector));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TThrottleResult)),
                        source.Expression,
                        throttleSelector));
        }
        #endregion

        #endregion

        #region Sequencing

        [KnownResource("rx://observable/retry")]
        public static IReactiveQbservable<TSource> Retry<TSource>(
            this IReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression));
        }

        [KnownResource("rx://observable/retry/count")]
        public static IReactiveQbservable<TSource> Retry<TSource>(
            this IReactiveQbservable<TSource> source,
            int retryCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (retryCount < 0)
            {
                throw new ArgumentNullException(nameof(retryCount));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(retryCount, typeof(int))));
        }

        [KnownResource("rx://observable/startwith")]
        public static IReactiveQbservable<TSource> StartWith<TSource>(
            this IReactiveQbservable<TSource> source,
            params TSource[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.NewArrayInit(typeof(TSource), GetExpressions(values))));
        }

        #endregion

        #region Projections

        #region Do

        [KnownResource("rx://observable/do")]
        public static IReactiveQbservable<T> Do<T>(this IReactiveQbservable<T> source, IReactiveQbserver<T> observer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression,
                        observer.Expression));
        }

        #endregion

        #region Select
        [KnownResource("rx://observable/select")]
        public static IReactiveQbservable<R> Select<T, R>(this IReactiveQbservable<T> source, Expression<Func<T, R>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return
                source.Provider.CreateQbservable<R>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)),
                        source.Expression,
                        selector));
        }
        #endregion

        #region SelectMany
        [KnownResource("rx://observable/selectmany")]
        public static IReactiveQbservable<R> SelectMany<T, C, R>(this IReactiveQbservable<T> source, Expression<Func<T, IReactiveObservable<C>>> collectionSelector, Expression<Func<T, C, R>> resultSelector)
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

            return
                source.Provider.CreateQbservable<R>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(C), typeof(R)),
                        source.Expression,
                        collectionSelector,
                        resultSelector));
        }
        #endregion

        #region Switch
        [KnownResource("rx://observable/switch")]
        public static IReactiveQbservable<TInputValue> Switch<TInputValue>(this IReactiveQbservable<IReactiveQbservable<TInputValue>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return
                sources.Provider.CreateQbservable<TInputValue>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TInputValue)),
                        sources.Expression));
        }
        #endregion

        #region CombineLatest
        [KnownResource("rx://observable/combineLatest")]
        public static IReactiveQbservable<R> CombineLatest<T1, T2, R>(this IReactiveQbservable<T1> left, IReactiveQbservable<T2> right, Expression<Func<T1, T2, R>> selector)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return
                left.Provider.CreateQbservable<R>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T1), typeof(T2), typeof(R)),
                        left.Expression,
                        right.Expression,
                        selector));
        }
        #endregion

        #endregion

        #region Scheduling

        [KnownResource("rx://observable/delaySubscription/absoluteTime")]
        public static IReactiveQbservable<TSource> DelaySubscription<TSource>(this IReactiveQbservable<TSource> source, DateTimeOffset dueTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(dueTime, typeof(DateTimeOffset))));
        }

        [KnownResource("rx://observable/delaySubscription/relativeTime")]
        public static IReactiveQbservable<TSource> DelaySubscription<TSource>(this IReactiveQbservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dueTime < TimeSpan.FromTicks(0))
            {
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(dueTime, typeof(TimeSpan))));
        }

        /// <summary>
        /// Creates a due time timer.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>Observable timer.</returns>
        [KnownResource("rx://observable/relativetimer")]
        public static IReactiveQbservable<long> Timer(this IReactive service, TimeSpan dueTime)
        {
            return Timer_(service, dueTime);
        }

        /// <summary>
        /// Creates a due time timer with a period.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns>Observable timer.</returns>
        [KnownResource("rx://observable/relativetimer/period")]
        public static IReactiveQbservable<long> Timer(this IReactive service, TimeSpan dueTime, TimeSpan period)
        {
            return Timer_(service, dueTime, period);
        }

        /// <summary>
        /// Creates a due timer.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>Observable timer.</returns>
        [KnownResource("rx://observable/absolutetimer")]
        public static IReactiveQbservable<long> Timer(this IReactive service, DateTimeOffset dueTime)
        {
            return Timer_(service, dueTime);
        }

        /// <summary>
        /// Creates a due timer with a period.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns>Observable timer.</returns>
        [KnownResource("rx://observable/absolutetimer/period")]
        public static IReactiveQbservable<long> Timer(this IReactive service, DateTimeOffset dueTime, TimeSpan period)
        {
            return Timer_(service, dueTime, period);
        }

        /// <summary>
        /// Implementation of the relative timer.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>Observable timer.</returns>
        private static IReactiveQbservable<long> Timer_(IReactive service, TimeSpan dueTime)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            MethodInfo method = (MethodInfo)ReflectionHelpers.InfoOf(() => ClientDefinitions.TimerRelative(default));
            return service.Provider.CreateQbservable<long>(
                Expression.Call(
                    method,
                    Expression.Constant(dueTime, typeof(TimeSpan))));
        }

        /// <summary>
        /// Implementation of the relative timer with period.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns>Observable timer.</returns>
        private static IReactiveQbservable<long> Timer_(IReactive service, TimeSpan dueTime, TimeSpan period)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            MethodInfo method = (MethodInfo)ReflectionHelpers.InfoOf(() => ClientDefinitions.TimerRelativePeriod(default, default));
            return service.Provider.CreateQbservable<long>(
                Expression.Call(
                    method,
                    Expression.Constant(dueTime, typeof(TimeSpan)),
                    Expression.Constant(period, typeof(TimeSpan))));
        }

        /// <summary>
        /// Implementation of the absolute timer.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>Observable timer.</returns>
        private static IReactiveQbservable<long> Timer_(IReactive service, DateTimeOffset dueTime)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            MethodInfo method = (MethodInfo)ReflectionHelpers.InfoOf(() => ClientDefinitions.TimerAbsolute(default));
            return service.Provider.CreateQbservable<long>(
                Expression.Call(
                    method,
                    Expression.Constant(dueTime, typeof(DateTimeOffset))));
        }

        /// <summary>
        /// Implementation of the absolute timer with period.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns>Observable timer.</returns>
        private static IReactiveQbservable<long> Timer_(IReactive service, DateTimeOffset dueTime, TimeSpan period)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            MethodInfo method = (MethodInfo)ReflectionHelpers.InfoOf(() => ClientDefinitions.TimerAbsolutePeriod(default, default));
            return service.Provider.CreateQbservable<long>(
                Expression.Call(
                    method,
                    Expression.Constant(dueTime, typeof(DateTimeOffset)),
                    Expression.Constant(period, typeof(TimeSpan))));
        }

        [KnownResource("rx://observable/absolutetimer")]
        private static IReactiveQbservable<long> TimerAbsolute(DateTimeOffset dueTime)
        {
            throw new NotImplementedException();
        }

        [KnownResource("rx://observable/absolutetimer/period")]
        private static IReactiveQbservable<long> TimerAbsolutePeriod(DateTimeOffset dueTime, TimeSpan period)
        {
            throw new NotImplementedException();
        }

        [KnownResource("rx://observable/relativetimer")]
        private static IReactiveQbservable<long> TimerRelative(TimeSpan dueTime)
        {
            throw new NotImplementedException();
        }

        [KnownResource("rx://observable/relativetimer/period")]
        private static IReactiveQbservable<long> TimerRelativePeriod(TimeSpan dueTime, TimeSpan period)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Window

        [KnownResource("rx://operators/window/count")]
        public static IReactiveQbservable<IReactiveQbservable<T>> Window<T>(this IReactiveQbservable<T> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<IReactiveQbservable<T>>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression,
                        Expression.Constant(count)));
        }

        #endregion

        #region Aggregates

        [KnownResource("rx://operators/sum")]
        public static IReactiveQbservable<int> Sum(this IReactiveQbservable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<int>(
                    Expression.Call(
                        (MethodInfo)MethodInfo.GetCurrentMethod(),
                        source.Expression));
        }

        #endregion

        #region Testing

        [KnownResource("test://await/subscribe")]
        public static IReactiveQbservable<TSource> AwaitSubscribe<TSource>(this IReactiveQbservable<TSource> source, string lockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (lockName == null)
            {
                throw new ArgumentNullException(nameof(lockName));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(lockName)));
        }

        [KnownResource("test://block/subscribe")]
        public static IReactiveQbservable<TSource> BlockSubscribe<TSource>(this IReactiveQbservable<TSource> source, string lockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (lockName == null)
            {
                throw new ArgumentNullException(nameof(lockName));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(lockName)));
        }

        public static IReactiveQbservable<TSource> AwaitDo<TSource>(this IReactiveQbservable<TSource> source, string onNextLockName)
        {
            return AwaitDo<TSource>(source, onNextLockName, null, null);
        }

        public static IReactiveQbservable<TSource> AwaitDo<TSource>(this IReactiveQbservable<TSource> source, string onNextLockName, string onErrorLockName)
        {
            return AwaitDo<TSource>(source, onNextLockName, onErrorLockName, null);
        }

        [KnownResource("test://await/do")]
        public static IReactiveQbservable<TSource> AwaitDo<TSource>(this IReactiveQbservable<TSource> source, string onNextLockName, string onErrorLockName, string onCompletedLockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(onNextLockName, typeof(string)),
                        Expression.Constant(onErrorLockName, typeof(string)),
                        Expression.Constant(onCompletedLockName, typeof(string))));
        }

        public static IReactiveQbservable<TSource> BlockDo<TSource>(this IReactiveQbservable<TSource> source, string onNextLockName)
        {
            return BlockDo<TSource>(source, onNextLockName, null, null);
        }

        public static IReactiveQbservable<TSource> BlockDo<TSource>(this IReactiveQbservable<TSource> source, string onNextLockName, string onErrorLockName)
        {
            return BlockDo<TSource>(source, onNextLockName, onErrorLockName, null);
        }

        [KnownResource("test://block/do")]
        public static IReactiveQbservable<TSource> BlockDo<TSource>(this IReactiveQbservable<TSource> source, string onNextLockName, string onErrorLockName, string onCompletedLockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(onNextLockName, typeof(string)),
                        Expression.Constant(onErrorLockName, typeof(string)),
                        Expression.Constant(onCompletedLockName, typeof(string))));
        }

        [KnownResource("test://await/dispose")]
        public static IReactiveQbservable<TSource> AwaitDispose<TSource>(this IReactiveQbservable<TSource> source, string lockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (lockName == null)
            {
                throw new ArgumentNullException(nameof(lockName));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(lockName)));
        }

        #endregion
    }

    public static class AsyncClientDefinitions
    {
        #region Builtins

        public static Task<IAsyncReactiveQubject<T, T>> CreateStream<T>(this IReactiveProxy context, Uri streamUri)
        {
            return context.GetStreamFactory<T, T>(new Uri("rx://subject/create")).CreateAsync(streamUri, null, CancellationToken.None);
        }

        /// <summary>
        /// Gets the expression representation for the elements in the given sequence. If an element is
        /// IExpressible, the element's self-describing expression is used; otherwise, a constant
        /// expression containing the value is used.
        /// </summary>
        /// <remarks>Copied and pasted directly from RIPP codebase (specifically Operators.cs).</remarks>
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

        #endregion

        #region Atoms

        [KnownResource("rx://observable/empty")]
        public static IAsyncReactiveQbservable<T> Empty<T>(this IReactiveProxy service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            return service.GetObservable<T>(new Uri("rx://observable/empty"));
        }

        [KnownResource("rx://observable/never")]
        public static IAsyncReactiveQbservable<T> Never<T>(this IReactiveProxy service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            return service.GetObservable<T>(new Uri("rx://observable/never"));
        }

        [KnownResource("rx://observers/nop")]
        public static IAsyncReactiveQbserver<T> Nop<T>(this IReactiveProxy service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            Expression<Func<IAsyncReactiveQbserver<T>>> e = () => NopObserver<T>.Instance.AsQbserver().To<IReactiveQbserver<T>, IAsyncReactiveQbserver<T>>();
            return service.Provider.CreateQbserver<T>(e.Body);
        }

        [KnownResource("rx://observable/return")]
        public static IAsyncReactiveQbservable<T> Return<T>(this IReactiveProxy service, T value)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            return service.GetObservable<T, T>(new Uri("rx://observable/return"))(value);
        }

        #endregion

        #region Filtering

        #region Where
        [KnownResource("rx://observable/filter")]
        public static IAsyncReactiveQbservable<T> Where<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        [KnownResource("rx://observable/filter/index")]
        public static IAsyncReactiveQbservable<T> Where<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, int, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        #endregion

        #region Take/Skip
        [KnownResource("rx://observable/take")]
        public static IAsyncReactiveQbservable<T> Take<T>(this IAsyncReactiveQbservable<T> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

        [KnownResource("rx://observable/take/while")]
        public static IAsyncReactiveQbservable<T> TakeWhile<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        [KnownResource("rx://observable/take/while/index")]
        public static IAsyncReactiveQbservable<T> TakeWhile<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, int, bool>> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        [KnownResource("rx://observable/take/until")]
        public static IAsyncReactiveQbservable<TSource> TakeUntil<TSource, TOther>(this IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> other)
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
                    other.Expression
                )
            );
        }

        [KnownResource("rx://observable/skip")]
        public static IAsyncReactiveQbservable<T> Skip<T>(this IAsyncReactiveQbservable<T> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

        [KnownResource("rx://observable/skip/until")]
        public static IAsyncReactiveQbservable<TSource> SkipUntil<TSource, TOther>(this IAsyncReactiveQbservable<TSource> source, IAsyncReactiveQbservable<TOther> triggeringSource)
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
                    triggeringSource.Expression
                )
            );
        }

        [KnownResource("rx://observable/skip/until/datetimeoffset")]
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

        #endregion

        #region DistinctUntilChanged
        [KnownResource("rx://observable/distinctuntilchanged")]
        public static IAsyncReactiveQbservable<T> DistinctUntilChanged<T, TKey>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, TKey>> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(TKey)),
                        source.Expression,
                        keySelector));
        }
        #endregion

        #region FirstAsync/FirstOrDefaultAsync

        [KnownResource("rx://observable/firstasync")]
        public static IAsyncReactiveQbservable<T> FirstAsync<T>(this IAsyncReactiveQbservable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression));
        }

        [KnownResource("rx://observable/firstasync/filter")]
        public static IAsyncReactiveQbservable<T> FirstAsync<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
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
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression,
                        predicate));
        }

        [KnownResource("rx://observable/firstordefaultasync")]
        public static IAsyncReactiveQbservable<T> FirstOrDefaultAsync<T>(this IAsyncReactiveQbservable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression));
        }

        [KnownResource("rx://observable/firstordefaultasync/filter")]
        public static IAsyncReactiveQbservable<T> FirstOrDefaultAsync<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
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
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression,
                        predicate));
        }

        #endregion

        #region Throttle
        [KnownResource("rx://observable/throttle")]
        public static IAsyncReactiveQbservable<TSource> Throttle<TSource, TThrottleResult>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, IAsyncReactiveQbservable<TThrottleResult>>> throttleSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (throttleSelector == null)
            {
                throw new ArgumentNullException(nameof(throttleSelector));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TThrottleResult)),
                        source.Expression,
                        throttleSelector));
        }
        #endregion

        #endregion

        #region Sequencing

        [KnownResource("rx://observable/retry")]
        public static IAsyncReactiveQbservable<TSource> Retry<TSource>(
            this IAsyncReactiveQbservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression));
        }

        [KnownResource("rx://observable/retry/count")]
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
                throw new ArgumentNullException(nameof(retryCount));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(retryCount, typeof(int))));
        }

        [KnownResource("rx://observable/startwith")]
        public static IAsyncReactiveQbservable<TSource> StartWith<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            params TSource[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.NewArrayInit(typeof(TSource), GetExpressions(values))));
        }

        #endregion

        #region Projections

        #region Do

        [KnownResource("rx://observable/do")]
        public static IAsyncReactiveQbservable<T> Do<T>(this IAsyncReactiveQbservable<T> source, IAsyncReactiveQbserver<T> observer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return
                source.Provider.CreateQbservable<T>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression,
                        observer.Expression));
        }

        #endregion

        #region Select
        [KnownResource("rx://observable/select")]
        public static IAsyncReactiveQbservable<R> Select<T, R>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, R>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return
                source.Provider.CreateQbservable<R>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)),
                        source.Expression,
                        selector));
        }
        #endregion

        #region SelectMany
        [KnownResource("rx://observable/selectmany")]
        public static IAsyncReactiveQbservable<R> SelectMany<T, C, R>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, IAsyncReactiveObservable<C>>> collectionSelector, Expression<Func<T, C, R>> resultSelector)
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

            return
                source.Provider.CreateQbservable<R>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(C), typeof(R)),
                        source.Expression,
                        collectionSelector,
                        resultSelector));
        }
        #endregion

        #region Switch
        [KnownResource("rx://observable/switch")]
        public static IAsyncReactiveQbservable<TInputValue> Switch<TInputValue>(this IAsyncReactiveQbservable<IAsyncReactiveQbservable<TInputValue>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return
                sources.Provider.CreateQbservable<TInputValue>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TInputValue)),
                        sources.Expression));
        }
        #endregion

        #region CombineLatest
        [KnownResource("rx://observable/combineLatest")]
        public static IAsyncReactiveQbservable<R> CombineLatest<T1, T2, R>(this IAsyncReactiveQbservable<T1> left, IAsyncReactiveQbservable<T2> right, Expression<Func<T1, T2, R>> selector)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return
                left.Provider.CreateQbservable<R>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T1), typeof(T2), typeof(R)),
                        left.Expression,
                        right.Expression,
                        selector));
        }
        #endregion

        #endregion

        #region Scheduling

        [KnownResource("rx://observable/delaySubscription/absoluteTime")]
        public static IAsyncReactiveQbservable<TSource> DelaySubscription<TSource>(this IAsyncReactiveQbservable<TSource> source, DateTimeOffset dueTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(dueTime, typeof(DateTimeOffset))));
        }

        [KnownResource("rx://observable/delaySubscription/relativeTime")]
        public static IAsyncReactiveQbservable<TSource> DelaySubscription<TSource>(this IAsyncReactiveQbservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dueTime < TimeSpan.FromTicks(0))
            {
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(dueTime, typeof(TimeSpan))));
        }

        /// <summary>
        /// Creates a due time timer.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>Observable timer.</returns>
        [KnownResource("rx://observable/relativetimer")]
        public static IAsyncReactiveQbservable<long> Timer(this IReactiveProxy service, TimeSpan dueTime)
        {
            return Timer_(service, dueTime);
        }

        /// <summary>
        /// Creates a due time timer with a period.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns>Observable timer.</returns>
        [KnownResource("rx://observable/relativetimer/period")]
        public static IAsyncReactiveQbservable<long> Timer(this IReactiveProxy service, TimeSpan dueTime, TimeSpan period)
        {
            return Timer_(service, dueTime, period);
        }

        /// <summary>
        /// Creates a due timer.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>Observable timer.</returns>
        [KnownResource("rx://observable/absolutetimer")]
        public static IAsyncReactiveQbservable<long> Timer(this IReactiveProxy service, DateTimeOffset dueTime)
        {
            return Timer_(service, dueTime);
        }

        /// <summary>
        /// Creates a due timer with a period.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns>Observable timer.</returns>
        [KnownResource("rx://observable/absolutetimer/period")]
        public static IAsyncReactiveQbservable<long> Timer(this IReactiveProxy service, DateTimeOffset dueTime, TimeSpan period)
        {
            return Timer_(service, dueTime, period);
        }

        /// <summary>
        /// Implementation of the relative timer.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>Observable timer.</returns>
        private static IAsyncReactiveQbservable<long> Timer_(IReactiveProxy service, TimeSpan dueTime)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            MethodInfo method = (MethodInfo)ReflectionHelpers.InfoOf(() => AsyncClientDefinitions.TimerRelative(default));
            return service.Provider.CreateQbservable<long>(
                Expression.Call(
                    method,
                    Expression.Constant(dueTime, typeof(TimeSpan))));
        }

        /// <summary>
        /// Implementation of the relative timer with period.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns>Observable timer.</returns>
        private static IAsyncReactiveQbservable<long> Timer_(IReactiveProxy service, TimeSpan dueTime, TimeSpan period)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            MethodInfo method = (MethodInfo)ReflectionHelpers.InfoOf(() => AsyncClientDefinitions.TimerRelativePeriod(default, default));
            return service.Provider.CreateQbservable<long>(
                Expression.Call(
                    method,
                    Expression.Constant(dueTime, typeof(TimeSpan)),
                    Expression.Constant(period, typeof(TimeSpan))));
        }

        /// <summary>
        /// Implementation of the absolute timer.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>Observable timer.</returns>
        private static IAsyncReactiveQbservable<long> Timer_(IReactiveProxy service, DateTimeOffset dueTime)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            MethodInfo method = (MethodInfo)ReflectionHelpers.InfoOf(() => AsyncClientDefinitions.TimerAbsolute(default));
            return service.Provider.CreateQbservable<long>(
                Expression.Call(
                    method,
                    Expression.Constant(dueTime, typeof(DateTimeOffset))));
        }

        /// <summary>
        /// Implementation of the absolute timer with period.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns>Observable timer.</returns>
        private static IAsyncReactiveQbservable<long> Timer_(IReactiveProxy service, DateTimeOffset dueTime, TimeSpan period)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            MethodInfo method = (MethodInfo)ReflectionHelpers.InfoOf(() => AsyncClientDefinitions.TimerAbsolutePeriod(default, default));
            return service.Provider.CreateQbservable<long>(
                Expression.Call(
                    method,
                    Expression.Constant(dueTime, typeof(DateTimeOffset)),
                    Expression.Constant(period, typeof(TimeSpan))));
        }

        [KnownResource("rx://observable/absolutetimer")]
        private static IAsyncReactiveQbservable<long> TimerAbsolute(DateTimeOffset dueTime)
        {
            throw new NotImplementedException();
        }

        [KnownResource("rx://observable/absolutetimer/period")]
        private static IAsyncReactiveQbservable<long> TimerAbsolutePeriod(DateTimeOffset dueTime, TimeSpan period)
        {
            throw new NotImplementedException();
        }

        [KnownResource("rx://observable/relativetimer")]
        private static IAsyncReactiveQbservable<long> TimerRelative(TimeSpan dueTime)
        {
            throw new NotImplementedException();
        }

        [KnownResource("rx://observable/relativetimer/period")]
        private static IAsyncReactiveQbservable<long> TimerRelativePeriod(TimeSpan dueTime, TimeSpan period)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Window

        [KnownResource("rx://operators/window/count")]
        public static IAsyncReactiveQbservable<IAsyncReactiveQbservable<T>> Window<T>(this IAsyncReactiveQbservable<T> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<IAsyncReactiveQbservable<T>>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                        source.Expression,
                        Expression.Constant(count)));
        }

        #endregion

        #region Aggregates

        [KnownResource("rx://operators/sum")]
        public static IAsyncReactiveQbservable<int> Sum(this IAsyncReactiveQbservable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<int>(
                    Expression.Call(
                        (MethodInfo)MethodInfo.GetCurrentMethod(),
                        source.Expression));
        }

        #endregion

        #region Testing

        [KnownResource("test://await/subscribe")]
        public static IAsyncReactiveQbservable<TSource> AwaitSubscribe<TSource>(this IAsyncReactiveQbservable<TSource> source, string lockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (lockName == null)
            {
                throw new ArgumentNullException(nameof(lockName));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(lockName)));
        }

        [KnownResource("test://block/subscribe")]
        public static IAsyncReactiveQbservable<TSource> BlockSubscribe<TSource>(this IAsyncReactiveQbservable<TSource> source, string lockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (lockName == null)
            {
                throw new ArgumentNullException(nameof(lockName));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(lockName)));
        }

        public static IAsyncReactiveQbservable<TSource> AwaitDo<TSource>(this IAsyncReactiveQbservable<TSource> source, string onNextLockName)
        {
            return AwaitDo<TSource>(source, onNextLockName, null, null);
        }

        public static IAsyncReactiveQbservable<TSource> AwaitDo<TSource>(this IAsyncReactiveQbservable<TSource> source, string onNextLockName, string onErrorLockName)
        {
            return AwaitDo<TSource>(source, onNextLockName, onErrorLockName, null);
        }

        [KnownResource("test://await/do")]
        public static IAsyncReactiveQbservable<TSource> AwaitDo<TSource>(this IAsyncReactiveQbservable<TSource> source, string onNextLockName, string onErrorLockName, string onCompletedLockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(onNextLockName, typeof(string)),
                        Expression.Constant(onErrorLockName, typeof(string)),
                        Expression.Constant(onCompletedLockName, typeof(string))));
        }

        public static IAsyncReactiveQbservable<TSource> BlockDo<TSource>(this IAsyncReactiveQbservable<TSource> source, string onNextLockName)
        {
            return BlockDo<TSource>(source, onNextLockName, null, null);
        }

        public static IAsyncReactiveQbservable<TSource> BlockDo<TSource>(this IAsyncReactiveQbservable<TSource> source, string onNextLockName, string onErrorLockName)
        {
            return BlockDo<TSource>(source, onNextLockName, onErrorLockName, null);
        }

        [KnownResource("test://block/do")]
        public static IAsyncReactiveQbservable<TSource> BlockDo<TSource>(this IAsyncReactiveQbservable<TSource> source, string onNextLockName, string onErrorLockName, string onCompletedLockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(onNextLockName, typeof(string)),
                        Expression.Constant(onErrorLockName, typeof(string)),
                        Expression.Constant(onCompletedLockName, typeof(string))));
        }

        [KnownResource("test://await/dispose")]
        public static IAsyncReactiveQbservable<TSource> AwaitDispose<TSource>(this IAsyncReactiveQbservable<TSource> source, string lockName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (lockName == null)
            {
                throw new ArgumentNullException(nameof(lockName));
            }

            return
                source.Provider.CreateQbservable<TSource>(
                    Expression.Call(
                        ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                        source.Expression,
                        Expression.Constant(lockName)));
        }

        #endregion
    }
}
