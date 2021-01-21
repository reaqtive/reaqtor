// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive;

using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Utility and extension methods for reliable observables.
    /// </summary>
    public static class ReliableObservableExtensions
    {
        /// <summary>
        /// Converts a reliable observable sequence to a subscribable sequence which checkpoints the received elements using their sequence numbers.
        /// </summary>
        /// <typeparam name="T">Type of the elements received by the source sequence.</typeparam>
        /// <param name="source">Reliable observable sequence to convert to a subscribable sequence.</param>
        /// <returns>Subscribable sequence wrapping the specified reliable observable.</returns>
        public static ISubscribable<T> ToSubscribable<T>(this IReliableObservable<T> source) => source.ToSubscribable(switchContext: false);

        /// <summary>
        /// Converts a reliable observable sequence to a subscribable sequence which checkpoints the received elements using their sequence numbers, optionally specifying context switching behavior.
        /// </summary>
        /// <typeparam name="T">Type of the elements received by the source sequence.</typeparam>
        /// <param name="source">Reliable observable sequence to convert to a subscribable sequence.</param>
        /// <param name="switchContext">If set to <c>true</c>, received elements will be produced on the logical scheduler provided to subscription on the resulting subscribable sequence via the SetContext call. Otherwise, elements are produced in a free-threaded manner.</param>
        /// <returns>Subscribable sequence wrapping the specified reliable observable.</returns>
        public static ISubscribable<T> ToSubscribable<T>(this IReliableObservable<T> source, bool switchContext) => new ReliableSubscriptionInput<T>(source, switchContext);
    }
}
