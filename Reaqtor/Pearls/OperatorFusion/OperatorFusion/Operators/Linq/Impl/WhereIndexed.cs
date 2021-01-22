// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the Where operator (with index parameter) using fusion.
//
// BD - October 2014
//

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class WhereIndexedFactory : IFusionOperator
    {
        public Type OutputType => Predicate.Parameters[0].Type;

        public HoistOperations Hoist => HoistOperations.OnError;

        public LambdaExpression Predicate { get; set; }

        private FieldInfo indexFld;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
            indexFld = defineField(typeof(int));
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var index = Expression.Field(state, indexFld);

                var ret = Expression.Label();

                var ex = Expression.Parameter(typeof(Exception));
                var p = Expression.Parameter(typeof(bool));

                return
                    Expression.Block(
                        typeof(void),
                        new[] { p },
                        Expression.Assign(p, Expression.Constant(false)),
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(p, Expression.Invoke(Predicate, value, index)),
                                Expression.AddAssignChecked(index, Expression.Constant(1)),
                                markDirty,
                                Expression.Default(typeof(void))
                            ),
                            Expression.MakeCatchBlock(
                                typeof(Exception), ex,
                                Expression.Block(
                                    createOnError(ex),
                                    Expression.Return(ret)
                                ),
                                null
                            )
                        ),
                        Expression.IfThen(
                            p,
                            createOnNext(value)
                        ),
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
            return onCompleted;
        }
    }
}
