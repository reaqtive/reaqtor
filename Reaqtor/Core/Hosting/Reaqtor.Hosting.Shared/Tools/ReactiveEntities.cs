// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq.Expressions;

namespace Reaqtor.Hosting.Shared.Tools
{
    #region Aliases

    using ReactiveEntitySet = Dictionary<string, HashSet<IEnumerable<ExpressionSlim>>>;

    #endregion

    /// <summary>
    /// A dictionary used for organizing occurrences of Reactive entities in an expression.
    /// </summary>
    public class ReactiveEntities : Dictionary<ReactiveEntityType, ReactiveEntitySet>
    {
        /// <summary>
        /// Initializes the dictionaries for the Reactive entity types.
        /// </summary>
        public ReactiveEntities()
        {
            Add(ReactiveEntityType.Observable, new ReactiveEntitySet());
            Add(ReactiveEntityType.Observer, new ReactiveEntitySet());
            Add(ReactiveEntityType.Stream, new ReactiveEntitySet());
            Add(ReactiveEntityType.StreamFactory, new ReactiveEntitySet());
            Add(ReactiveEntityType.Subscription, new ReactiveEntitySet());
            Add(ReactiveEntityType.SubscriptionFactory, new ReactiveEntitySet());
        }

        /// <summary>
        /// Gets the dictionary of occurrences of observable entities.
        /// </summary>
        public ReactiveEntitySet Observables => this[ReactiveEntityType.Observable];

        /// <summary>
        /// Gets the dictionary of occurrences of observer entities.
        /// </summary>
        public ReactiveEntitySet Observers => this[ReactiveEntityType.Observer];

        /// <summary>
        /// Gets the dictionary of occurrences of stream entities.
        /// </summary>
        public ReactiveEntitySet Streams => this[ReactiveEntityType.Stream];

        /// <summary>
        /// Gets the dictionary of occurrences of stream factory entities.
        /// </summary>
        public ReactiveEntitySet StreamFactories => this[ReactiveEntityType.StreamFactory];

        /// <summary>
        /// Gets the dictionary of occurrences of subscription entities.
        /// </summary>
        public ReactiveEntitySet Subscriptions => this[ReactiveEntityType.Subscription];

        /// <summary>
        /// Gets the dictionary of occurrences of subscription factory entities.
        /// </summary>
        public ReactiveEntitySet SubscriptionFactories => this[ReactiveEntityType.SubscriptionFactory];
    }
}
