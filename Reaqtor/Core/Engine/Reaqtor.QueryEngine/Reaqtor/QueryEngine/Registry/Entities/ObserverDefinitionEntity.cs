// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents an observer in a QueryEngineRegistry.
    /// </summary>
    internal class ObserverDefinitionEntity : DefinitionEntity, IReactiveObserverDefinition
    {
        public ObserverDefinitionEntity(Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        public override ReactiveEntityKind Kind => ReactiveEntityKind.Observer;

        public static ObserverDefinitionEntity CreateInvalidInstance(Uri uri) => new InvalidEntity(uri);

        public static ObserverDefinitionEntity FromSubject(SubjectEntity subject) => new FromSubjectEntity(subject);

        public IReactiveQbserver<T> ToObserver<T>() => throw new NotImplementedException();

        public Func<TArgs, IReactiveQbserver<TResult>> ToObserver<TArgs, TResult>() => throw new NotImplementedException();

        private sealed class InvalidEntity : ObserverDefinitionEntity
        {
            private static readonly Expression s_invalidExpression = Expression.Default(typeof(object));

            public InvalidEntity(Uri uri)
                : base(uri, s_invalidExpression, state: null)
            {
                AdvanceState(TransactionState.Active);
            }

            public override bool IsInitialized => false;
        }

        private sealed class FromSubjectEntity : ObserverDefinitionEntity
        {
            public FromSubjectEntity(SubjectEntity subject)
                : this(subject, subject.IsInitialized)
            {
            }

            /// <summary>
            /// This constructor is needed to ensure ordering between the checks
            /// for `IsInitialized`, and the creation of the constant expression.
            /// If the order were reversed, we could get into a state where the
            /// constant expression contains a null value, but the entity appears
            /// to be initialized.
            /// </summary>
            /// <param name="entity">The subject.</param>
            /// <param name="isInitialized">
            /// <b>true</b> if the subject is initialized at the time the
            /// constructor is called, <b>false</b> otherwise.
            /// </param>
            private FromSubjectEntity(SubjectEntity entity, bool isInitialized)
                : base(entity.Uri, Expression.Constant(entity.Instance), entity.State)
            {
                IsInitialized = isInitialized;
            }

            public override bool IsInitialized { get; }
        }
    }
}
