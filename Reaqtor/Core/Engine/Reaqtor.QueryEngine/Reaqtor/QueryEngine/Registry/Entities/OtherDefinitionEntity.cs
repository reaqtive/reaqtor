// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents an arbitrary definition in a QueryEngineRegistry.
    /// </summary>
    internal class OtherDefinitionEntity : DefinitionEntity
    {
        public OtherDefinitionEntity(Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        public override ReactiveEntityKind Kind => ReactiveEntityKind.Other;

        public static OtherDefinitionEntity CreateInvalidInstance(Uri uri) => new InvalidEntity(uri);

        private sealed class InvalidEntity : OtherDefinitionEntity
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
