// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Reaqtor
{
    public static partial class Operators
    {
        /// <summary>
        /// Aggregates the elements in the source sequence using the specified aggregation function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to aggregate.</param>
        /// <param name="aggregate">Aggregation function used to combine the running aggregation result with each consecutive element.</param>
        /// <returns>Observable sequence containing a single element representing the result of the aggregation. If the source sequence is empty, an error of type <see cref="System.InvalidOperationException"/> is propagated.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Aggregate.Accumulate)]
        public static IAsyncReactiveQbservable<TSource> Aggregate<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TSource, TSource>> aggregate)
        {
            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    aggregate));
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
        [KnownResource(Remoting.Client.Constants.Observable.Aggregate.Seed)]
        public static IAsyncReactiveQbservable<TResult> Aggregate<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, TResult seed, Expression<Func<TResult, TSource, TResult>> aggregate)
        {
            return source.Provider.CreateQbservable<TResult>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    Expression.Constant(seed, typeof(TResult)),
                    aggregate));
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
        [KnownResource(Remoting.Client.Constants.Observable.Aggregate.SeedResult)]
        public static IAsyncReactiveQbservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncReactiveQbservable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> aggregate, Expression<Func<TAccumulate, TResult>> resultSelector)
        {
            return source.Provider.CreateQbservable<TResult>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)),
                    source.Expression,
                    Expression.Constant(seed, typeof(TAccumulate)),
                    aggregate,
                    resultSelector));
        }

        /// <summary>
        /// Produces a sequence with a rolling aggregates of the elements in the source sequence using the specified aggregation function.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to aggregate.</param>
        /// <param name="aggregate">Aggregation function used to combine the running aggregation result with each consecutive element.</param>
        /// <returns>Observable sequence containing elements obtained from applying the rolling aggregate to the source sequence. If the source sequence is empty, an empty sequence is returned.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Scan.Accumulate)]
        public static IAsyncReactiveQbservable<TSource> Scan<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, TSource, TSource>> aggregate)
        {
            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    aggregate));
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
        [KnownResource(Remoting.Client.Constants.Observable.Scan.Seed)]
        public static IAsyncReactiveQbservable<TResult> Scan<TSource, TResult>(this IAsyncReactiveQbservable<TSource> source, TResult seed, Expression<Func<TResult, TSource, TResult>> aggregate)
        {
            return source.Provider.CreateQbservable<TResult>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    Expression.Constant(seed, typeof(TResult)),
                    aggregate));
        }

        /// <summary>
        /// Aggregates the source sequence to determine whether it contains any element.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to check for the presence of elements.</param>
        /// <returns>Observable sequence containing a single element with value <c>true</c> if the source sequence is not empty; otherwise a single element with value <c>false</c>.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Any.NoArgument)]
        public static IAsyncReactiveQbservable<bool> Any<TSource>(this IAsyncReactiveQbservable<TSource> source)
        {
            return source.Provider.CreateQbservable<bool>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Aggregates the source sequence to determine whether any of the elements matches the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to match with the predicate.</param>
        /// <param name="predicate">Predicate function to apply to each element of the source sequence. Upon encountering the first element that does pass the predicate, the aggregation operation will stop.</param>
        /// <returns>Observable sequence containing a single element with value <c>true</c> if any element in the source sequence matches the predicate; otherwise a single element with value <c>false</c>. If the source sequence is empty, an element with value <c>false</c> is returned.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Any.Predicate)]
        public static IAsyncReactiveQbservable<bool> Any<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Provider.CreateQbservable<bool>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Aggregates the source sequence to determine whether all of the elements match the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to match with the predicate.</param>
        /// <param name="predicate">Predicate function to apply to each element of the source sequence. Upon encountering the first element that doesn't pass the predicate, the aggregation operation will stop.</param>
        /// <returns>Observable sequence containing a single element with value <c>true</c> if all elements in the source sequence match the predicate; otherwise a single element with value <c>false</c>. If the source sequence is empty, an element with value <c>true</c> is returned.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.All.Predicate)]
        public static IAsyncReactiveQbservable<bool> All<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Provider.CreateQbservable<bool>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Aggregates the source sequence to determine whether it is empty.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to check for the presence of elements.</param>
        /// <returns>Observable sequence containing a single element with value <c>true</c> if the source sequence is empty; otherwise a single element with value <c>false</c>.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.IsEmpty.NoArgument)]
        public static IAsyncReactiveQbservable<bool> IsEmpty<TSource>(this IAsyncReactiveQbservable<TSource> source)
        {
            return source.Provider.CreateQbservable<bool>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Aggregates the source sequence to count the number of elements. The result is represented as a 32-bit integer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to count.</param>
        /// <returns>Observable sequence containing a single element with an integer value representing the number of elements in the sequence. If the source sequence is empty, an element with value 0 is returned.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Count.NoArgument)]
        public static IAsyncReactiveQbservable<int> Count<TSource>(this IAsyncReactiveQbservable<TSource> source)
        {
            return source.Provider.CreateQbservable<int>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Aggregates the source sequence to count the number of elements that match the specified predicate. The result is represented as a 32-bit integer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to count based on the specified predicate.</param>
        /// <param name="predicate">Predicate function to apply to each element of the source sequence. Elements that match the predicate will be counted.</param>
        /// <returns>Observable sequence containing a single element with an integer value representing the number of elements in the sequence that match the predicate. If the source sequence does not contain any elements matching the predicate, an element with value 0 is returned.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Count.Predicate)]
        public static IAsyncReactiveQbservable<int> Count<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Provider.CreateQbservable<int>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Aggregates the source sequence to count the number of elements. The result is represented as a 64-bit integer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to count.</param>
        /// <returns>Observable sequence containing a single element with an integer value representing the number of elements in the sequence. If the source sequence is empty, an element with value 0 is returned.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.LongCount.NoArgument)]
        public static IAsyncReactiveQbservable<long> LongCount<TSource>(this IAsyncReactiveQbservable<TSource> source)
        {
            return source.Provider.CreateQbservable<long>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Aggregates the source sequence to count the number of elements that match the specified predicate. The result is represented as a 64-bit integer.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to count based on the specified predicate.</param>
        /// <param name="predicate">Predicate function to apply to each element of the source sequence. Elements that match the predicate will be counted.</param>
        /// <returns>Observable sequence containing a single element with an integer value representing the number of elements in the sequence that match the predicate. If the source sequence does not contain any elements matching the predicate, an element with value 0 is returned.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.LongCount.Predicate)]
        public static IAsyncReactiveQbservable<long> LongCount<TSource>(this IAsyncReactiveQbservable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.Provider.CreateQbservable<long>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    predicate));
        }

        /// <summary>
        /// Aggregates the source sequence into a list containing all of its elements.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to aggregate into a list.</param>
        /// <returns>Observable sequence containing a single element with a list containing all of the elements in the sequence. If the source sequence is empty, an empty list is returned.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.ToList.NoArgument)]
        public static IAsyncReactiveQbservable<IList<TSource>> ToList<TSource>(this IAsyncReactiveQbservable<TSource> source)
        {
            return source.Provider.CreateQbservable<IList<TSource>>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression));
        }

        /// <summary>
        /// Returns an observable sequence that produces a single OnNext notification with the specified value.
        /// </summary>
        /// <typeparam name="TResult">Type of the elements in the resulting sequence.</typeparam>
        /// <param name="value">The single value the resulting sequence should produce.</param>
        /// <returns>An observable sequence that produces a single element with the specified value, followed by a completion notification.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Return.Value)]
        public static IAsyncReactiveQbservable<TResult> Return<TResult>(this IReactiveProxy context, TResult value)
        {
            return context.GetObservable<TResult, TResult>(new Uri(Remoting.Client.Constants.Observable.Return.Value))(value);
        }

        /// <summary>
        /// Returns an observable sequence that produces an OnError notification with the specified error.
        /// </summary>
        /// <typeparam name="TResult">Type of the elements in the resulting sequence.</typeparam>
        /// <param name="error">Error to propagate in the resulting sequence.</param>
        /// <returns>An observable sequence that propagates the specified error.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Return.Value)]
        public static IAsyncReactiveQbservable<TResult> Throw<TResult>(this IReactiveProxy context, Exception error)
        {
            return context.GetObservable<Exception, TResult>(new Uri(Remoting.Client.Constants.Observable.Throw.Error))(error);
        }

        /// <summary>
        /// Returns an observable sequence that never produces any notification. Such a sequence can be used to represent an infinite duration.
        /// </summary>
        /// <typeparam name="TResult">Type of the elements in the resulting sequence.</typeparam>
        /// <returns>An observable sequence that never produces any notification.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Never.NoArgument)]
        public static IAsyncReactiveQbservable<TResult> Never<TResult>(this IReactiveProxy context)
        {
            return context.GetObservable<TResult>(new Uri(Remoting.Client.Constants.Observable.Never.NoArgument));
        }

        /// <summary>
        /// Merges the elements in the specified observable sequences.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Array of sequences whose elements should be merged.</param>
        /// <returns>Observable sequence containing the elements of the specified source sequences.</returns>
        [KnownResource(Remoting.Client.Constants.Observable.Merge.N)]
        public static IAsyncReactiveQbservable<TSource> Merge<TSource>(this IReactiveProxy context, params IAsyncReactiveQbservable<TSource>[] sources)
        {
            return context.GetObservable<IAsyncReactiveQbservable<TSource>[], TSource>(new Uri(Remoting.Client.Constants.Observable.Merge.N))(sources);
        }
    }
}
