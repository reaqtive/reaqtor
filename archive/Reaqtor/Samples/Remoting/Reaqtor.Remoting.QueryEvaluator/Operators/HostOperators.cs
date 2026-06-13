// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Exposes a set of query operators that are host-aware.
    /// </summary>
    internal static class HostOperators
    {
        /// <summary>
        /// Subscription cleanup operator. This operator should only be injected in the final position before a Subscribe call
        /// in order to defer cleanup until completion of the entire query.
        /// </summary>
        /// <typeparam name="T">Type of the elements processed by the operator.</typeparam>
        /// <param name="source">Sequence to provide subscription cleanup for.</param>
        /// <returns>Sequence with the subscription cleanup operator applied.</returns>
        public static ISubscribable<T> CleanupSubscription<T>(this ISubscribable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new SubscriptionCleanup<T>(source);
        }

        /// <summary>
        /// Subscription cleanup operator. This operator should only be injected in the final position before a Subscribe call
        /// in order to defer cleanup until completion of the entire query.
        /// </summary>
        /// <typeparam name="T">Type of the elements processed by the operator.</typeparam>
        /// <param name="source">Sequence to provide subscription cleanup for.</param>
        /// <returns>Sequence with the subscription cleanup operator applied.</returns>
        [KnownResource(HostOperatorConstants.CleanupSubscriptionUri)]
        public static IReactiveQbservable<T> CleanupSubscription<T>(this IReactiveQbservable<T> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dummy function that "converts" to `Subscribable`. Is removed during beta reductions during the normalization process.
        /// </summary>
        /// <exception cref="System.NotImplementedException">Always thrown.</exception>
        /// <typeparam name="T">Type emitted by our `Subscribable`.</typeparam>
        /// <param name="source">Expression to convert to `Subscribable`.</param>
        /// <returns>Should never return.</returns>
        /// <remarks>Throws exception because it should be normalized away and thus never called.</remarks>
        [KnownResource(Constants.IdentityFunctionUri)]
        public static ISubscribable<T> AsSubscribable<T>(this IReactiveQbservable<T> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dummy function "converts" to `IReactiveQbservable`. Is removed during beta reductions during normalization process.
        /// </summary>
        /// <exception cref="System.NotImplementedException">Always thrown.</exception>
        /// <typeparam name="T">Type emitted by our `IReactiveQbservable`.</typeparam>
        /// <param name="source">Expression to convert to `IReactiveQbservable`.</param>
        /// <returns>Should never return.</returns>
        /// <remarks>Throws exception because it should be normalized away and thus never called.</remarks>
        [KnownResource(Constants.IdentityFunctionUri)]
        public static IReactiveQbservable<T> AsQbservable<T>(this ISubscribable<T> source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dummy function to bind to the built-in Subscribe method.
        /// </summary>
        /// <exception cref="System.NotImplementedException">Always thrown.</exception>
        /// <typeparam name="T">Element type processed by the subscription.</typeparam>
        /// <param name="observable">The observable used in the subscription.</param>
        /// <param name="observer">The observer used in the subscription.</param>
        /// <returns>Should never return.</returns>
        /// <remarks>Throws exception because it should be normalized away and thus never called.</remarks>
        [KnownResource(Constants.SubscribeUri)]
        public static IReactiveQubscription QuotedSubscribe<T>(this IReactiveQbservable<T> observable, IReactiveQbserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}
