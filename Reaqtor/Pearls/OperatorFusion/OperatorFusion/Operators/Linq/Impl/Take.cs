// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the Take operator using fusion.
//
// BD - October 2014
//

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class TakeFactory : IFusionOperator
    {
        public Type OutputType { get; set; }

        public HoistOperations Hoist => HoistOperations.OnCompleted;

        public int Count { get; set; }

        private FieldInfo nFld;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
            nFld = defineField(typeof(int));

            var n = Expression.Field(state, nFld);

            appendToCtor(
                Expression.Assign(n, Expression.Constant(Count))
            );
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            // TODO: Take(0) support

            return value =>
            {
                var n = Expression.Field(state, nFld);

                var ret = Expression.Label();

                return
                    Expression.Block(
                        typeof(void),
                        Array.Empty<ParameterExpression>(),
                        Expression.IfThen(
                            Expression.Equal(n, Expression.Constant(0)),
                            Expression.Return(ret)
                        ),
                        Expression.PreDecrementAssign(n),
                        markDirty,
                        createOnNext(value),
                        Expression.IfThen(
                            Expression.Equal(n, Expression.Constant(0)),
                            onCompleted // CHECK: dispose timing
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
