// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2016 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Provides a set of extension methods for IAsyncSubscriptionFactory and IAsyncSubscriptionFactory&lt;TArgs&gt;.
    /// </summary>
    public static partial class AsyncReactiveSubscriptionFactoryExtensions
    {
        #region CreateAsync

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionFactory">Factory used to create the subscription.</param>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>Task returning a subscription object, or an exception if the creation request was unsuccessful.</returns>
        public static Task<IAsyncReactiveSubscription> CreateAsync(this IAsyncReactiveSubscriptionFactory subscriptionFactory, Uri subscriptionUri, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return subscriptionFactory.CreateAsync(subscriptionUri, state, CancellationToken.None);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription UR
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>I.
        /// </summary>
        /// <param name="subscriptionFactory">Factory used to create the subscription.</param>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="argument">Parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>Task returning a subscription object, or an exception if the creation request was unsuccessful.</returns>
        public static Task<IAsyncReactiveSubscription> CreateAsync<TArgs>(this IAsyncReactiveSubscriptionFactory<TArgs> subscriptionFactory, Uri subscriptionUri, TArgs argument, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return subscriptionFactory.CreateAsync(subscriptionUri, argument, state, CancellationToken.None);
        }

        #endregion
    }
}
