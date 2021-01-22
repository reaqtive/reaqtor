// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the Select operator using fusion.
//
// BD - October 2014
//

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class SelectFactory : IFusionOperator
    {
        public Type OutputType => Selector.Body.Type;

        public HoistOperations Hoist => HoistOperations.OnError;

        public LambdaExpression Selector { get; set; }

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var res = Expression.Parameter(OutputType);
                var ex = Expression.Parameter(typeof(Exception));

                var ret = Expression.Label();

                return
                    Expression.Block(
                        typeof(void),
                        new[] { res },
                        Expression.Assign(res, Expression.Default(OutputType)),
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(res, Expression.Invoke(Selector, value)),
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
                        createOnNext(res),
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
