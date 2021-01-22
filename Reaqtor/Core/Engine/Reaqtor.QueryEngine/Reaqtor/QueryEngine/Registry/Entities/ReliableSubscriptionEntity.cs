// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq.Expressions;

using Reaqtor.Metadata;
using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a reliable subscription in a QueryEngineRegistry.
    /// </summary>
    internal class ReliableSubscriptionEntity : RuntimeEntity<IReliableSubscription>, IReactiveSubscriptionProcess // TODO: metadata in reliable space
    {
        public ReliableSubscriptionEntity(Uri uri, Expression expression, object state, IReliableSubscription instance = null)
            : base(uri, expression, state)
        {
            if (instance != null)
            {
                Instance = instance;
            }
        }

        public override ReactiveEntityKind Kind => ReactiveEntityKind.ReliableSubscription;

        public override bool IsInitialized => Instance != null;

        public static ReliableSubscriptionEntity CreateInvalidInstance(Uri uri) => new InvalidEntity(uri);

        protected override void StartCore(IReliableSubscription instance, params object[] args)
        {
            Debug.Assert(args.Length == 1);

            instance.Start((long)args[0]);
        }

        public IReactiveQubscription ToSubscription() => throw new NotImplementedException();

        private sealed class InvalidEntity : ReliableSubscriptionEntity
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
