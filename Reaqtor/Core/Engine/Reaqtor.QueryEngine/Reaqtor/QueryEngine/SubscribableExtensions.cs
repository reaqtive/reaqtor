// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.QueryEngine
{
    internal static class SubscribableExtensions
    {
        /// <summary>
        /// Decorates the specified <paramref name="source"/> with a cleanup for the specified <paramref name="edges"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="source">The source for which associated edges should be deleted upon termination (OnError, OnCompleted, or Dispose).</param>
        /// <param name="edges">The edges to clean up.</param>
        /// <returns>Wrapper arond the specified <paramref name="source"/>.</returns>
        public static ISubscribable<T> EdgeCleanup<T>(this ISubscribable<T> source, EdgeDescription[] edges) => new EdgeCleanup<T>(source, edges);

        /// <summary>
        /// Target for binding rx://builtin/subscribe. Supports visiting the subscription as well as the observer (e.g. for purposes
        /// of state persistence).
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="subscribable">The observable to subscribe to.</param>
        /// <param name="observer">The observer to use for the subscription.</param>
        /// <returns>The subscription object which supports traversal using subscription visitors.</returns>
        public static ISubscription SubscribeRoot<T>(this ISubscribable<T> subscribable, IObserver<T> observer)
        {
            var subscription = subscribable.Subscribe(observer);

            //
            // NB: Subscription is a bit of a misnomer; it's merely a mechanism to visit nodes in a query tree. As such, an observer
            //     can implement `ISubscription` and be reachable by visitors (e.g. to save/load state, to support IUnloadable, etc.).
            //

            if (observer is ISubscription o)
            {
                subscription = StaticCompositeSubscription.Create(subscription, o);
            }

            return subscription;
        }
    }
}
