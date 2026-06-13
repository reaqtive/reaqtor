// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the Last[Async] operator using fusion.
//
// BD - October 2014
//

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class LastFactory : IFusionOperator
    {
        public Type OutputType { get; set; }

        public HoistOperations Hoist => HoistOperations.OnError;

        private FieldInfo hasValueFld;
        private FieldInfo lastValueFld;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
            hasValueFld = defineField(typeof(bool));
            lastValueFld = defineField(OutputType);

            var hasValue = Expression.Field(state, hasValueFld);
            var lastValue = Expression.Field(state, lastValueFld);

            appendToCtor(
                Expression.Assign(hasValue, Expression.Constant(false))
            );

            appendToCtor(
                Expression.Assign(lastValue, Expression.Default(OutputType))
            );
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var hasValue = Expression.Field(state, hasValueFld);
                var lastValue = Expression.Field(state, lastValueFld);

                return
                    Expression.Block(
                        typeof(void),
                        Array.Empty<ParameterExpression>(),
                        Expression.Assign(hasValue, Expression.Constant(true)),
                        Expression.Assign(lastValue, value),
                        markDirty,
                        Expression.Empty()
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
            var lastValue = Expression.Field(state, lastValueFld);

            return
                Expression.IfThenElse(
                    Expression.Not(hasValue),
                    createOnError(
                        Expression.New(typeof(InvalidOperationException).GetConstructor(new[] { typeof(string) }), Expression.Constant("Sequence is empty."))
                    ),
                    Expression.Block(
                        createOnNext(lastValue),
                        onCompleted
                    )
                );
        }
    }
}
