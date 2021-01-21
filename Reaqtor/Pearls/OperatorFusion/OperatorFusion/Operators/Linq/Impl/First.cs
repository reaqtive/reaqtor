// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the First[Async] operator using fusion.
//
// BD - October 2014
//

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class FirstFactory : IFusionOperator
    {
        public Type OutputType { get; set; }

        public HoistOperations Hoist => HoistOperations.OnError | HoistOperations.OnCompleted;

        private FieldInfo hasValueFld;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
            hasValueFld = defineField(typeof(bool));

            var hasValue = Expression.Field(state, hasValueFld);

            appendToCtor(
                Expression.Assign(hasValue, Expression.Constant(false))
            );
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var hasValue = Expression.Field(state, hasValueFld);

                var ret = Expression.Label();

                return
                    Expression.Block(
                        typeof(void),
                        Array.Empty<ParameterExpression>(),
                        Expression.IfThen(
                            hasValue,
                            Expression.Return(ret)
                        ),
                        Expression.Assign(hasValue, Expression.Constant(true)),
                        markDirty,
                        createOnNext(value),
                        onCompleted, // CHECK: dispose timing
                        Expression.Label(ret)
                    );
            };
        }

        public Func<Expression, Expression> CreateOnError(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return error =>
            {
                return createOnError(error);
            };
        }

        public Expression CreateOnCompleted(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            var hasValue = Expression.Field(state, hasValueFld);

            // NOTE: FirstOrDefault is tricky; we'd have to call OnNext in a few places, which would result in code duplication.
            //       We really need more intelligence higher up to realize multiple such calls were made and substitute these using a generated helper method.

            return
                Expression.IfThenElse(
                    Expression.Not(hasValue),
                    createOnError(
                        Expression.New(typeof(InvalidOperationException).GetConstructor(new[] { typeof(string) }), Expression.Constant("Sequence is empty."))
                    ),
                    onCompleted
                );
        }
    }
}
