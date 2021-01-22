// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitting null checks for these. Will be non-null when moving to #nullable enable.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Builder for subscription visitors performing actions on each operator node in a subscription tree.
    /// </summary>
    /// <typeparam name="T1">Type of the operators processed by the visitor's action. This type should implement IOperator.</typeparam>
    public class SubscriptionVisitorBuilder<T1>
        where T1 : class, IOperator
    {
        private readonly Action<T1> _visit1;

        /// <summary>
        /// Creates a new builder for subscription visitors with the specified action to visit operator nodes.
        /// </summary>
        /// <param name="visit1">Action applied to each operator node of the specified type.</param>
        public SubscriptionVisitorBuilder(Action<T1> visit1)
        {
            _visit1 = visit1;
        }

        /// <summary>
        /// Creates a new subscription visitor that applies the specified action to each operator node of the specified type in a subscription tree, in addition to the current visitor's action.
        /// </summary>
        /// <typeparam name="T2">Type of the operators processed by the specified action. This type should implement IOperator.</typeparam>
        /// <param name="visit2">Action applied to each operator node of the specified type.</param>
        /// <returns>Subscription visitor that will apply the current visitor's action and the specified action to each matching operator node.</returns>
        public SubscriptionVisitorBuilder<T1, T2> Do<T2>(Action<T2> visit2)
            where T2 : class, IOperator
        {
            return new SubscriptionVisitorBuilder<T1, T2>(_visit1, visit2);
        }

        /// <summary>
        /// Applies the visitor to the specified subscription.
        /// </summary>
        /// <param name="subscription">Subscription to apply the visitor to.</param>
        public void Apply(ISubscription subscription)
        {
            subscription.Accept(new SubscriptionVisitor<T1>(_visit1));
        }
    }

    /// <summary>
    /// Builder for subscription visitors performing actions on each operator node in a subscription tree.
    /// </summary>
    /// <typeparam name="T1">Type of the operators processed by the visitor's first action. This type should implement IOperator.</typeparam>
    /// <typeparam name="T2">Type of the operators processed by the visitor's second action. This type should implement IOperator.</typeparam>
    public class SubscriptionVisitorBuilder<T1, T2>
        where T1 : class, IOperator
        where T2 : class, IOperator
    {
        private readonly Action<T1> _visit1;
        private readonly Action<T2> _visit2;

        /// <summary>
        /// Creates a new builder for subscription visitors with the specified actions to visit operator nodes.
        /// </summary>
        /// <param name="visit1">Action applied to each operator node of type <typeparamref name="T1"/>.</param>
        /// <param name="visit2">Action applied to each operator node of type <typeparamref name="T2"/>.</param>
        public SubscriptionVisitorBuilder(Action<T1> visit1, Action<T2> visit2)
        {
            _visit1 = visit1;
            _visit2 = visit2;
        }

        /// <summary>
        /// Applies the visitor to the specified subscription.
        /// </summary>
        /// <param name="subscription">Subscription to apply the visitor to.</param>
        public void Apply(ISubscription subscription)
        {
            subscription.Accept(new SubscriptionVisitor<T1, T2>(_visit1, _visit2));
        }
    }
}
