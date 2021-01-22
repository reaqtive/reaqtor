// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a stream factory (a.k.a. subject factory) in a QueryEngineRegistry.
    /// </summary>
    internal class StreamFactoryDefinitionEntity : DefinitionEntity, IReactiveStreamFactoryDefinition
    {
        public StreamFactoryDefinitionEntity(Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        public override ReactiveEntityKind Kind => ReactiveEntityKind.StreamFactory;

        public static StreamFactoryDefinitionEntity CreateInvalidInstance(Uri uri) => new InvalidEntity(uri);

        public IReactiveQubjectFactory<TInput, TOutput> ToStreamFactory<TInput, TOutput>() => throw new NotImplementedException();

        public IReactiveQubjectFactory<TInput, TOutput, TArgs> ToStreamFactory<TArgs, TInput, TOutput>() => throw new NotImplementedException();

        private sealed class InvalidEntity : StreamFactoryDefinitionEntity
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
