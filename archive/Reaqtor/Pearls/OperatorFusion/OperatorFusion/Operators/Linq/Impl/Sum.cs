// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the Sum operator using fusion.
//
// BD - October 2014
//

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class SumFactory : IFusionOperator
    {
        public SumFactory(Type resultType) => OutputType = resultType;

        public Type OutputType { get; }

        public HoistOperations Hoist => HoistOperations.OnError;

        private FieldInfo sumFld;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
            sumFld = defineField(OutputType);
            var sum = Expression.Field(state, sumFld);

            appendToCtor(
                Expression.Assign(sum, Expression.Default(OutputType))
            );
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var sum = Expression.Field(state, sumFld);

                var ret = Expression.Label();
                var ex = Expression.Parameter(typeof(Exception), "ex");

                return
                    Expression.Block(
                        typeof(void),
                        Array.Empty<ParameterExpression>(),
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.AddAssignChecked(sum, value),
                                Expression.Empty()
                            ),
                            Expression.Catch(
                                ex,
                                Expression.Block(
                                    createOnError(ex),
                                    Expression.Return(ret)
                                )
                            )
                        ),
                        markDirty,
                        Expression.Empty(),
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
            var sum = Expression.Field(state, sumFld);

            return
                Expression.Block(
                    createOnNext(sum),
                    onCompleted
                );
        }
    }
}
