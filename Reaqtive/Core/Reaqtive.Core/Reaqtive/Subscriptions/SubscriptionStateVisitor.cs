// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Visitor to act on operator state of a subscription.
    /// </summary>
    public class SubscriptionStateVisitor
    {
        private readonly ISubscription _subscription;

        /// <summary>
        /// Creates a new subscription state visitor for the specified subscription.
        /// </summary>
        /// <param name="subscription">Subscription visited by this visitor.</param>
        public SubscriptionStateVisitor(ISubscription subscription)
        {
            _subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
        }

        /// <summary>
        /// Determines whether the state of any of the operators in the subscription has changed.
        /// </summary>
        /// <returns><c>true</c> if any operator has state changes; otherwise, <c>false</c>.</returns>
        public bool HasStateChanged() => HasStateChangedCore(_subscription);

        /// <summary>
        /// Determines whether the state of any of the operators in the <paramref name="subscription"/> has changed.
        /// </summary>
        /// <param name="subscription">The subscription to check for state changes.</param>
        /// <returns><c>true</c> if any operator has state changes; otherwise, <c>false</c>.</returns>
        public static bool HasStateChanged(ISubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            return HasStateChangedCore(subscription);
        }

        private static bool HasStateChangedCore(ISubscription subscription)
        {
            var visitor = new HasStateChangedVisitor();

            subscription.Accept(visitor);

            return visitor.Changed;
        }

        /// <summary>
        /// Saves the state of operators in the subscription to state writers obtained by the specified state writer factory.
        /// </summary>
        /// <param name="factory">State writer factory to obtain state writers for operators from.</param>
        public void SaveState(IOperatorStateWriterFactory factory) => SaveStateCore(_subscription, factory);

        /// <summary>
        /// Saves the state of operators in the <paramref name="subscription"/> to state writers obtained by the specified state writer factory.
        /// </summary>
        /// <param name="subscription">The subscription to save state for.</param>
        /// <param name="factory">State writer factory to obtain state writers for operators from.</param>
        public static void SaveState(ISubscription subscription, IOperatorStateWriterFactory factory)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            SaveStateCore(subscription, factory);
        }

        internal static void SaveStateCore(ISubscription subscription, IOperatorStateWriterFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            subscription.Accept(new SaveStateVisitor(factory));
        }

        /// <summary>
        /// Loads the state of operators in the subscription from state readers obtained by the specified state reader factory.
        /// </summary>
        /// <param name="factory">State reader factory to obtain state readers for operators from.</param>
        public void LoadState(IOperatorStateReaderFactory factory) => LoadStateCore(_subscription, factory);

        /// <summary>
        /// Loads the state of operators in the <paramref name="subscription"/> from state readers obtained by the specified state reader factory.
        /// </summary>
        /// <param name="subscription">The subscription to save state for.</param>
        /// <param name="factory">State reader factory to obtain state readers for operators from.</param>
        public static void LoadState(ISubscription subscription, IOperatorStateReaderFactory factory)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            LoadStateCore(subscription, factory);
        }

        internal static void LoadStateCore(ISubscription subscription, IOperatorStateReaderFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            subscription.Accept(new LoadStateVisitor(factory));
        }

        /// <summary>
        /// Propagates state save notifications to stateful operators in the subscription.
        /// </summary>
        public void OnStateSaved() => OnStateSavedCore(_subscription);

        /// <summary>
        /// Propagates state save notifications to stateful operators in the <paramref name="subscription"/>.
        /// </summary>
        /// <param name="subscription">The subscription to send state save notifications to.</param>
        public static void OnStateSaved(ISubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            OnStateSavedCore(subscription);
        }

        private static void OnStateSavedCore(ISubscription subscription)
        {
            subscription.Accept(OnStateSavedVisitor.Instance);
        }

        private sealed class HasStateChangedVisitor : SubscriptionVisitor
        {
            public bool Changed = false;

            public override void Visit(IOperator node)
            {
                if (Changed)
                {
                    return;
                }

                base.Visit(node);
            }

            protected override void VisitCore(IOperator node)
            {
                if (node is IStatefulOperator s)
                {
                    Changed |= s.StateChanged;
                }
            }
        }

        private sealed class SaveStateVisitor : SubscriptionVisitor
        {
            private readonly IOperatorStateWriterFactory _factory;

            public SaveStateVisitor(IOperatorStateWriterFactory factory)
            {
                _factory = factory;
            }

            protected override void VisitCore(IOperator node)
            {
                if (node is IStatefulOperator s)
                {
                    _factory.SaveState(s);
                }
            }
        }

        private sealed class LoadStateVisitor : SubscriptionVisitor
        {
            private readonly IOperatorStateReaderFactory _factory;

            public LoadStateVisitor(IOperatorStateReaderFactory factory)
            {
                _factory = factory;
            }

            protected override void VisitCore(IOperator node)
            {
                if (node is IStatefulOperator s)
                {
                    _factory.LoadState(s);
                }
            }
        }

        private sealed class OnStateSavedVisitor : SubscriptionVisitor
        {
            public static readonly OnStateSavedVisitor Instance = new();

            private OnStateSavedVisitor() { }

            protected override void VisitCore(IOperator node)
            {
                if (node is IStatefulOperator s)
                {
                    s.OnStateSaved();
                }
            }
        }
    }
}
