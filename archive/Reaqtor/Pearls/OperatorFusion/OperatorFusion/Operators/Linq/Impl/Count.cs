// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the Count operator using fusion.
//
// BD - October 2014
//
//
// Remarks:
//
//   Notice we don't have an implementation for Count(IO<T>, Func<T, bool>). Thanks to
//   fusion, the compiled code will be the same as composing Where and Count.
//

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class CountFactory : IFusionOperator
    {
        public Type OutputType => typeof(int);

        public HoistOperations Hoist => HoistOperations.None;

        private FieldInfo countFld;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
            countFld = defineField(typeof(int));
            var count = Expression.Field(state, countFld);

            appendToCtor(
                Expression.Assign(count, Expression.Constant(0, typeof(int)))
            );
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var count = Expression.Field(state, countFld);

                var ret = Expression.Label();
                var ex = Expression.Parameter(typeof(Exception), "ex");

                return
                    Expression.Block(
                        typeof(void),
                        Array.Empty<ParameterExpression>(),
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.AddAssignChecked(count, Expression.Constant(1, typeof(int))),
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
            var count = Expression.Field(state, countFld);

            return
                Expression.Block(
                    createOnNext(count),
                    onCompleted
                );
        }
    }
}
