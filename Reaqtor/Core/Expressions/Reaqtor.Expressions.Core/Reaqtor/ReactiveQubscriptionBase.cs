// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Base class for the implementation of subscriptions represented by an expression tree.
    /// </summary>
    public abstract class ReactiveQubscriptionBase : ReactiveSubscriptionBase, IReactiveQubscription
    {
        /// <summary>
        /// Creates a new subscription represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the observable.</param>
        protected ReactiveQubscriptionBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription.
        /// </summary>
        public abstract Expression Expression { get; }
    }
}
