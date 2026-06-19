// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a subscription factory in a QueryEngineRegistry.
    /// </summary>
    internal class SubscriptionFactoryDefinitionEntity : DefinitionEntity, IReactiveSubscriptionFactoryDefinition
    {
        public SubscriptionFactoryDefinitionEntity(Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        public override ReactiveEntityKind Kind => ReactiveEntityKind.SubscriptionFactory;

        public static SubscriptionFactoryDefinitionEntity CreateInvalidInstance(Uri uri) => new InvalidEntity(uri);

        public IReactiveQubscriptionFactory ToSubscriptionFactory() => throw new NotImplementedException();

        public IReactiveQubscriptionFactory<TArgs> ToSubscriptionFactory<TArgs>() => throw new NotImplementedException();

        private sealed class InvalidEntity : SubscriptionFactoryDefinitionEntity
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
