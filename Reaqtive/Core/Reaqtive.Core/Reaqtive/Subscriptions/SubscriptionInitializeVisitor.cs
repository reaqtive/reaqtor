// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Visitor to initialize subscriptions.
    /// </summary>
    public class SubscriptionInitializeVisitor
    {
        private readonly ISubscription _subscription;

        /// <summary>
        /// Creates a new subscription initialization visitor for the specified subscription.
        /// </summary>
        /// <param name="subscription">Subscription visited by this visitor.</param>
        public SubscriptionInitializeVisitor(ISubscription subscription)
        {
            _subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
        }

        /// <summary>
        /// Initializes the operators in the subscription by setting the specified operator context and starting the operators.
        /// </summary>
        /// <param name="context">Operator context to set on the operators in the subscription.</param>
        public void Initialize(IOperatorContext context) => InitializeCore(_subscription, context);

        /// <summary>
        /// Initializes the operators in the <paramref name="subscription"/> by setting the specified operator context and starting the operators.
        /// </summary>
        /// <param name="subscription">The subscription to initialize and start.</param>
        /// <param name="context">Operator context to set on the operators in the subscription.</param>
        public static void Initialize(ISubscription subscription, IOperatorContext context)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            InitializeCore(subscription, context);
        }

        private static void InitializeCore(ISubscription subscription, IOperatorContext context)
        {
            SubscribeCore(subscription);

            SetContextCore(subscription, context);

            StartCore(subscription);
        }

        /// <summary>
        /// Initializes the operators in the subscription using the specified operator context, optionally applying state to them.
        /// </summary>
        /// <param name="context">Operator context to set on the operators in the subscription.</param>
        /// <param name="state">State reader factory to read operator state from. This parameter can be left <c>null</c>.</param>
        public void Initialize(IOperatorContext context, IOperatorStateReaderFactory state)
        {
            Subscribe();
            SetContext(context);

            if (state != null)
            {
                LoadState(state);
            }

            Start();
        }

        /// <summary>
        /// Initializes the operators in the subscription by obtaining the inputs that constitute the operator tree.
        /// </summary>
        public void Subscribe() => SubscribeCore(_subscription);

        /// <summary>
        /// Initializes the operators in the <paramref name="subscription"/> by obtaining the inputs that constitute the operator tree.
        /// </summary>
        /// <param name="subscription">The subscription to initialize.</param>
        public static void Subscribe(ISubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            SubscribeCore(subscription);
        }

        private static void SubscribeCore(ISubscription subscription)
        {
            subscription.Accept(SubscribeVisitor.Instance);
        }

        /// <summary>
        /// Sets the specified operator context to operators in the subscription.
        /// </summary>
        /// <param name="context">Operator context to set on the operators in the subscription.</param>
        public void SetContext(IOperatorContext context) => SetContextCore(_subscription, context);

        /// <summary>
        /// Sets the specified operator context to operators in the <paramref name="subscription"/>.
        /// </summary>
        /// <param name="subscription">The subscription to pass operator context to.</param>
        /// <param name="context">Operator context to set on the operators in the subscription.</param>
        public static void SetContext(ISubscription subscription, IOperatorContext context)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            SetContextCore(subscription, context);
        }

        private static void SetContextCore(ISubscription subscription, IOperatorContext context)
        {
            subscription.Accept(new SetContextVisitor(context));
        }

        /// <summary>
        /// Loads the state of operators in the subscription using the specified operator state reader facotry.
        /// </summary>
        /// <param name="factory">State reader factory to read operator state from.</param>
        public void LoadState(IOperatorStateReaderFactory factory)
        {
            SubscriptionStateVisitor.LoadStateCore(_subscription, factory);
        }

        /// <summary>
        /// Starts the operators in the subscription.
        /// </summary>
        public void Start() => StartCore(_subscription);

        /// <summary>
        /// Starts the operators in the <paramref name="subscription"/>.
        /// </summary>
        /// <param name="subscription">The subscription to start.</param>
        public static void Start(ISubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            StartCore(subscription);
        }

        private static void StartCore(ISubscription subscription)
        {
            subscription.Accept(StartVisitor.Instance);
        }

        private sealed class SubscribeVisitor : SubscriptionVisitor
        {
            public static readonly SubscribeVisitor Instance = new();

            private SubscribeVisitor() { }

            protected override void VisitCore(IOperator node) => node.Subscribe();
        }

        private sealed class SetContextVisitor : SubscriptionVisitor
        {
            private readonly IOperatorContext _context;

            public SetContextVisitor(IOperatorContext context)
            {
                _context = context;
            }

            protected override void VisitCore(IOperator node) => node.SetContext(_context);
        }

        private sealed class StartVisitor : SubscriptionVisitor
        {
            public static readonly StartVisitor Instance = new();

            private StartVisitor() { }

            protected override void VisitCore(IOperator node) => node.Start();
        }
    }
}
