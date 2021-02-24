// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq.Expressions;

using Reaqtive;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a subscription in a QueryEngineRegistry.
    /// </summary>
    internal class SubscriptionEntity : RuntimeEntity<ISubscription>, IReactiveSubscriptionProcess
    {
        public SubscriptionEntity(Uri uri, Expression expression, object state, ISubscription instance = null)
            : base(uri, expression, state)
        {
            if (instance != null)
            {
                Instance = instance;
            }
        }

        public override ReactiveEntityKind Kind => ReactiveEntityKind.Subscription;

        public override bool IsInitialized => Instance != null;

        public static SubscriptionEntity CreateInvalidInstance(Uri uri) => new InvalidEntity(uri);

        protected override void StartCore(ISubscription instance, params object[] args)
        {
            Debug.Assert(args.Length == 0);

            SubscriptionInitializeVisitor.Start(instance);
        }

        public IReactiveQubscription ToSubscription() => throw new NotImplementedException();

        private sealed class InvalidEntity : SubscriptionEntity
        {
            private static readonly Expression s_invalidExpression = Expression.Default(typeof(object));

            public InvalidEntity(Uri uri)
                : base(uri, s_invalidExpression, state: null)
            {
                AdvanceState(TransactionState.Active);
            }

            public override bool IsInitialized => false;
        }
    }
}
