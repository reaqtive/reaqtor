// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.Operators;
using System;

namespace Reaqtive
{
    public static partial class Subscribable
    {

        #region CombineLatest2

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, Func<TSource1, TSource2, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TResult>(source1, source2, selector);
        }

        #endregion

        #region CombineLatest3

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, Func<TSource1, TSource2, TSource3, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, selector);
        }

        #endregion

        #region CombineLatest4

        /// <summary>
        /// Combines the latest observed element of each input sequence using the specified selector function upon receiving any element on any of the input sequences.
        /// </summary>
        /// <param name="source1">First observable input sequence.</param>
        /// <param name="source2">Second observable input sequence.</param>
        /// <param name="source3">Third observable input sequence.</param>
        /// <param name="source4">Fourth observable input sequence.</param>
        /// <param name="selector">Selector function to apply to the latest observer element of each input sequence.</param>
        /// <returns>Observable sequence containing elements obtained from applying the selector function to the latest value of each input sequence whenever any input sequence receives an element.</returns>
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, Func<TSource1, TSource2, TSource3, TSource4, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, selector);
        }

        #endregion

        #region CombineLatest5

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1, source2, source3, source4, source5, selector);
        }

        #endregion

        #region CombineLatest6

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1, source2, source3, source4, source5, source6, selector);
        }

        #endregion

        #region CombineLatest7

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1, source2, source3, source4, source5, source6, source7, selector);
        }

        #endregion

        #region CombineLatest8

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, ISubscribable<TSource8> source8, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (source8 == null)
            {
                throw new ArgumentNullException(nameof(source8));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, selector);
        }

        #endregion

        #region CombineLatest9

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, ISubscribable<TSource8> source8, ISubscribable<TSource9> source9, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (source8 == null)
            {
                throw new ArgumentNullException(nameof(source8));
            }

            if (source9 == null)
            {
                throw new ArgumentNullException(nameof(source9));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, selector);
        }

        #endregion

        #region CombineLatest10

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, ISubscribable<TSource8> source8, ISubscribable<TSource9> source9, ISubscribable<TSource10> source10, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (source8 == null)
            {
                throw new ArgumentNullException(nameof(source8));
            }

            if (source9 == null)
            {
                throw new ArgumentNullException(nameof(source9));
            }

            if (source10 == null)
            {
                throw new ArgumentNullException(nameof(source10));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, selector);
        }

        #endregion

        #region CombineLatest11

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, ISubscribable<TSource8> source8, ISubscribable<TSource9> source9, ISubscribable<TSource10> source10, ISubscribable<TSource11> source11, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (source8 == null)
            {
                throw new ArgumentNullException(nameof(source8));
            }

            if (source9 == null)
            {
                throw new ArgumentNullException(nameof(source9));
            }

            if (source10 == null)
            {
                throw new ArgumentNullException(nameof(source10));
            }

            if (source11 == null)
            {
                throw new ArgumentNullException(nameof(source11));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, selector);
        }

        #endregion

        #region CombineLatest12

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, ISubscribable<TSource8> source8, ISubscribable<TSource9> source9, ISubscribable<TSource10> source10, ISubscribable<TSource11> source11, ISubscribable<TSource12> source12, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (source8 == null)
            {
                throw new ArgumentNullException(nameof(source8));
            }

            if (source9 == null)
            {
                throw new ArgumentNullException(nameof(source9));
            }

            if (source10 == null)
            {
                throw new ArgumentNullException(nameof(source10));
            }

            if (source11 == null)
            {
                throw new ArgumentNullException(nameof(source11));
            }

            if (source12 == null)
            {
                throw new ArgumentNullException(nameof(source12));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, selector);
        }

        #endregion

        #region CombineLatest13

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, ISubscribable<TSource8> source8, ISubscribable<TSource9> source9, ISubscribable<TSource10> source10, ISubscribable<TSource11> source11, ISubscribable<TSource12> source12, ISubscribable<TSource13> source13, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (source8 == null)
            {
                throw new ArgumentNullException(nameof(source8));
            }

            if (source9 == null)
            {
                throw new ArgumentNullException(nameof(source9));
            }

            if (source10 == null)
            {
                throw new ArgumentNullException(nameof(source10));
            }

            if (source11 == null)
            {
                throw new ArgumentNullException(nameof(source11));
            }

            if (source12 == null)
            {
                throw new ArgumentNullException(nameof(source12));
            }

            if (source13 == null)
            {
                throw new ArgumentNullException(nameof(source13));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, selector);
        }

        #endregion

        #region CombineLatest14

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, ISubscribable<TSource8> source8, ISubscribable<TSource9> source9, ISubscribable<TSource10> source10, ISubscribable<TSource11> source11, ISubscribable<TSource12> source12, ISubscribable<TSource13> source13, ISubscribable<TSource14> source14, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (source8 == null)
            {
                throw new ArgumentNullException(nameof(source8));
            }

            if (source9 == null)
            {
                throw new ArgumentNullException(nameof(source9));
            }

            if (source10 == null)
            {
                throw new ArgumentNullException(nameof(source10));
            }

            if (source11 == null)
            {
                throw new ArgumentNullException(nameof(source11));
            }

            if (source12 == null)
            {
                throw new ArgumentNullException(nameof(source12));
            }

            if (source13 == null)
            {
                throw new ArgumentNullException(nameof(source13));
            }

            if (source14 == null)
            {
                throw new ArgumentNullException(nameof(source14));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, selector);
        }

        #endregion

        #region CombineLatest15

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, ISubscribable<TSource8> source8, ISubscribable<TSource9> source9, ISubscribable<TSource10> source10, ISubscribable<TSource11> source11, ISubscribable<TSource12> source12, ISubscribable<TSource13> source13, ISubscribable<TSource14> source14, ISubscribable<TSource15> source15, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (source8 == null)
            {
                throw new ArgumentNullException(nameof(source8));
            }

            if (source9 == null)
            {
                throw new ArgumentNullException(nameof(source9));
            }

            if (source10 == null)
            {
                throw new ArgumentNullException(nameof(source10));
            }

            if (source11 == null)
            {
                throw new ArgumentNullException(nameof(source11));
            }

            if (source12 == null)
            {
                throw new ArgumentNullException(nameof(source12));
            }

            if (source13 == null)
            {
                throw new ArgumentNullException(nameof(source13));
            }

            if (source14 == null)
            {
                throw new ArgumentNullException(nameof(source14));
            }

            if (source15 == null)
            {
                throw new ArgumentNullException(nameof(source15));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, selector);
        }

        #endregion

        #region CombineLatest16

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
        public static ISubscribable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(this ISubscribable<TSource1> source1, ISubscribable<TSource2> source2, ISubscribable<TSource3> source3, ISubscribable<TSource4> source4, ISubscribable<TSource5> source5, ISubscribable<TSource6> source6, ISubscribable<TSource7> source7, ISubscribable<TSource8> source8, ISubscribable<TSource9> source9, ISubscribable<TSource10> source10, ISubscribable<TSource11> source11, ISubscribable<TSource12> source12, ISubscribable<TSource13> source13, ISubscribable<TSource14> source14, ISubscribable<TSource15> source15, ISubscribable<TSource16> source16, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> selector)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (source3 == null)
            {
                throw new ArgumentNullException(nameof(source3));
            }

            if (source4 == null)
            {
                throw new ArgumentNullException(nameof(source4));
            }

            if (source5 == null)
            {
                throw new ArgumentNullException(nameof(source5));
            }

            if (source6 == null)
            {
                throw new ArgumentNullException(nameof(source6));
            }

            if (source7 == null)
            {
                throw new ArgumentNullException(nameof(source7));
            }

            if (source8 == null)
            {
                throw new ArgumentNullException(nameof(source8));
            }

            if (source9 == null)
            {
                throw new ArgumentNullException(nameof(source9));
            }

            if (source10 == null)
            {
                throw new ArgumentNullException(nameof(source10));
            }

            if (source11 == null)
            {
                throw new ArgumentNullException(nameof(source11));
            }

            if (source12 == null)
            {
                throw new ArgumentNullException(nameof(source12));
            }

            if (source13 == null)
            {
                throw new ArgumentNullException(nameof(source13));
            }

            if (source14 == null)
            {
                throw new ArgumentNullException(nameof(source14));
            }

            if (source15 == null)
            {
                throw new ArgumentNullException(nameof(source15));
            }

            if (source16 == null)
            {
                throw new ArgumentNullException(nameof(source16));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, source16, selector);
        }

        #endregion
    }
}
