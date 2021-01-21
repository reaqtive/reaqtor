// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace Reaqtor.Expressions.Core
{
    /// <summary>
    /// Signatures of the available operators to be used in operator definitions (DefineObservable).
    /// This class does not contain implementations for the operators. Expressions using the operators
    /// have to be rebound to a perticular implementation (Rx or Subscribable) before they can be executed.
    /// </summary>
    public static partial class ReactiveQbservable
    {
        #region CombineLatest

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, Expression<Func<TSource1, TSource2, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, Expression<Func<TSource1, TSource2, TSource3, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, Expression<Func<TSource1, TSource2, TSource3, TSource4, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="source8">Eighth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, IReactiveQbservable<TSource8> source8, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="source8">Eighth observable input sequence.</param>
        /// <param name="source9">Ninth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, IReactiveQbservable<TSource8> source8, IReactiveQbservable<TSource9> source9, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="source8">Eighth observable input sequence.</param>
        /// <param name="source9">Ninth observable input sequence.</param>
        /// <param name="source10">Tenth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, IReactiveQbservable<TSource8> source8, IReactiveQbservable<TSource9> source9, IReactiveQbservable<TSource10> source10, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="source8">Eighth observable input sequence.</param>
        /// <param name="source9">Ninth observable input sequence.</param>
        /// <param name="source10">Tenth observable input sequence.</param>
        /// <param name="source11">Eleventh observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, IReactiveQbservable<TSource8> source8, IReactiveQbservable<TSource9> source9, IReactiveQbservable<TSource10> source10, IReactiveQbservable<TSource11> source11, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="source8">Eighth observable input sequence.</param>
        /// <param name="source9">Ninth observable input sequence.</param>
        /// <param name="source10">Tenth observable input sequence.</param>
        /// <param name="source11">Eleventh observable input sequence.</param>
        /// <param name="source12">Twelfth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, IReactiveQbservable<TSource8> source8, IReactiveQbservable<TSource9> source9, IReactiveQbservable<TSource10> source10, IReactiveQbservable<TSource11> source11, IReactiveQbservable<TSource12> source12, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="source8">Eighth observable input sequence.</param>
        /// <param name="source9">Ninth observable input sequence.</param>
        /// <param name="source10">Tenth observable input sequence.</param>
        /// <param name="source11">Eleventh observable input sequence.</param>
        /// <param name="source12">Twelfth observable input sequence.</param>
        /// <param name="source13">Thirteenth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, IReactiveQbservable<TSource8> source8, IReactiveQbservable<TSource9> source9, IReactiveQbservable<TSource10> source10, IReactiveQbservable<TSource11> source11, IReactiveQbservable<TSource12> source12, IReactiveQbservable<TSource13> source13, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="source8">Eighth observable input sequence.</param>
        /// <param name="source9">Ninth observable input sequence.</param>
        /// <param name="source10">Tenth observable input sequence.</param>
        /// <param name="source11">Eleventh observable input sequence.</param>
        /// <param name="source12">Twelfth observable input sequence.</param>
        /// <param name="source13">Thirteenth observable input sequence.</param>
        /// <param name="source14">Fourteenth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, IReactiveQbservable<TSource8> source8, IReactiveQbservable<TSource9> source9, IReactiveQbservable<TSource10> source10, IReactiveQbservable<TSource11> source11, IReactiveQbservable<TSource12> source12, IReactiveQbservable<TSource13> source13, IReactiveQbservable<TSource14> source14, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="source8">Eighth observable input sequence.</param>
        /// <param name="source9">Ninth observable input sequence.</param>
        /// <param name="source10">Tenth observable input sequence.</param>
        /// <param name="source11">Eleventh observable input sequence.</param>
        /// <param name="source12">Twelfth observable input sequence.</param>
        /// <param name="source13">Thirteenth observable input sequence.</param>
        /// <param name="source14">Fourteenth observable input sequence.</param>
        /// <param name="source15">Fifteenth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, IReactiveQbservable<TSource8> source8, IReactiveQbservable<TSource9> source9, IReactiveQbservable<TSource10> source10, IReactiveQbservable<TSource11> source11, IReactiveQbservable<TSource12> source12, IReactiveQbservable<TSource13> source13, IReactiveQbservable<TSource14> source14, IReactiveQbservable<TSource15> source15, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="source5">Fifth observable input sequence.</param>
        /// <param name="source6">Sixth observable input sequence.</param>
        /// <param name="source7">Seventh observable input sequence.</param>
        /// <param name="source8">Eighth observable input sequence.</param>
        /// <param name="source9">Ninth observable input sequence.</param>
        /// <param name="source10">Tenth observable input sequence.</param>
        /// <param name="source11">Eleventh observable input sequence.</param>
        /// <param name="source12">Twelfth observable input sequence.</param>
        /// <param name="source13">Thirteenth observable input sequence.</param>
        /// <param name="source14">Fourteenth observable input sequence.</param>
        /// <param name="source15">Fifteenth observable input sequence.</param>
        /// <param name="source16">Sixteenth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static IReactiveQbservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(this IReactiveQbservable<TSource1> source1, IReactiveQbservable<TSource2> source2, IReactiveQbservable<TSource3> source3, IReactiveQbservable<TSource4> source4, IReactiveQbservable<TSource5> source5, IReactiveQbservable<TSource6> source6, IReactiveQbservable<TSource7> source7, IReactiveQbservable<TSource8> source8, IReactiveQbservable<TSource9> source9, IReactiveQbservable<TSource10> source10, IReactiveQbservable<TSource11> source11, IReactiveQbservable<TSource12> source12, IReactiveQbservable<TSource13> source13, IReactiveQbservable<TSource14> source14, IReactiveQbservable<TSource15> source15, IReactiveQbservable<TSource16> source16, Expression<Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Aggregates

        #region Average
        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Average(this IReactiveQbservable<Int32> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int32>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Average(this IReactiveQbservable<Int64> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int64>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Single> Average(this IReactiveQbservable<Single> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Single> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Single>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Average(this IReactiveQbservable<Double> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Double>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal> Average(this IReactiveQbservable<Decimal> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Decimal>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Average(this IReactiveQbservable<Int32?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int32?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Average(this IReactiveQbservable<Int64?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int64?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Single?> Average(this IReactiveQbservable<Single?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Single?> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Single?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Average(this IReactiveQbservable<Double?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Double?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose average of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the average of all values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal?> Average(this IReactiveQbservable<Decimal?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the average of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose average of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the average of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal?> Average<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Decimal?>> selector)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Min
        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int32> Min(this IReactiveQbservable<Int32> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int32> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int32>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int64> Min(this IReactiveQbservable<Int64> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int64> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int64>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Single> Min(this IReactiveQbservable<Single> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Single> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Single>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Min(this IReactiveQbservable<Double> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Double>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal> Min(this IReactiveQbservable<Decimal> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Decimal>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int32?> Min(this IReactiveQbservable<Int32?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int32?> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int32?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int64?> Min(this IReactiveQbservable<Int64?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int64?> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int64?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Single?> Min(this IReactiveQbservable<Single?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Single?> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Single?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Min(this IReactiveQbservable<Double?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Double?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose smallest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal?> Min(this IReactiveQbservable<Decimal?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the smallest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose smallest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the smallest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal?> Min<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Decimal?>> selector)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Max
        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int32> Max(this IReactiveQbservable<Int32> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int32> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int32>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int64> Max(this IReactiveQbservable<Int64> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int64> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int64>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Single> Max(this IReactiveQbservable<Single> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Single> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Single>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Max(this IReactiveQbservable<Double> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Double>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal> Max(this IReactiveQbservable<Decimal> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Decimal>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int32?> Max(this IReactiveQbservable<Int32?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int32?> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int32?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int64?> Max(this IReactiveQbservable<Int64?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int64?> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int64?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Single?> Max(this IReactiveQbservable<Single?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Single?> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Single?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Max(this IReactiveQbservable<Double?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Double?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose largest of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the largest of all values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal?> Max(this IReactiveQbservable<Decimal?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the largest of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose largest of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the largest of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal?> Max<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Decimal?>> selector)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Sum
        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int32> Sum(this IReactiveQbservable<Int32> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int32> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int32>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int64> Sum(this IReactiveQbservable<Int64> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int64> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int64>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Single> Sum(this IReactiveQbservable<Single> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Single> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Single>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Sum(this IReactiveQbservable<Double> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Double>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal> Sum(this IReactiveQbservable<Decimal> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Decimal>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int32?> Sum(this IReactiveQbservable<Int32?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int32?> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int32?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Int64?> Sum(this IReactiveQbservable<Int64?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Int64?> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Int64?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Single?> Sum(this IReactiveQbservable<Single?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Single?> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Single?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Sum(this IReactiveQbservable<Double?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Double?> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Double?>> selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the element values.
        /// </summary>
        /// <param name="source">Source sequence whose sum of elements to obtain.</param>
        /// <returns>Observable sequence containing a single element with the sum of all values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal?> Sum(this IReactiveQbservable<Decimal?> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aggregates the source sequence to return the sum of the values obtained from applying the specified selector function to each element in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose sum of projected elements to obtain.</param>
        /// <param name="selector">Selector function to apply to each element in the source sequence.</param>
        /// <returns>Observable sequence containing a single element with the sum of all projected values in the source sequence.</returns>
        public static IReactiveQbservable<Decimal?> Sum<TSource>(this IReactiveQbservable<TSource> source, Expression<Func<TSource, Decimal?>> selector)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}