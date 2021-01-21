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
    /// Represents a subject (a.k.a. stream) in a QueryEngineRegistry.
    /// </summary>
    internal class SubjectEntity : RuntimeEntity<IDisposable>, IReactiveStreamProcess
    {
        public SubjectEntity(Uri uri, Expression expression, object state, IDisposable instance = null)
            : base(uri, expression, state)
        {
            if (instance != null)
            {
                Instance = instance;
            }
        }

        public override ReactiveEntityKind Kind => ReactiveEntityKind.Stream;

        public override bool IsInitialized => Instance != null;

        public static SubjectEntity CreateInvalidInstance(Uri uri) => new InvalidEntity(uri);

        protected override void StartCore(IDisposable instance, params object[] args)
        {
            Debug.Assert(args.Length == 0);

            if (instance is IOperator op)
            {
                op.Start();
            }
        }

        public IReactiveQubject<TInput, TOutput> ToStream<TInput, TOutput>() => throw new NotImplementedException();

        private sealed class InvalidEntity : SubjectEntity
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
