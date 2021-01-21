// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using Reaqtive.Operators;

namespace Reaqtive
{
    /// <summary>
    /// Provides conversions from <see cref="IObservable{T}"/>.
    /// </summary>
    public static class ObservableExtensions
    {
        //
        // CONSIDER: (Re)move this; other than this, there are no dependencies on IObservable<T>.
        //

        /// <summary>
        /// Converts an observable sequence to a subscribable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to convert to a subscribable sequence.</param>
        /// <returns>Subscribable sequence producing the same notifications as the specified observable sequence.</returns>
        public static ISubscribable<TSource> ToSubscribable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source is ISubscribable<TSource> subscribable)
            {
                return subscribable;
            }

            return new ToSubscribable<TSource>(source);
        }
    }
}
