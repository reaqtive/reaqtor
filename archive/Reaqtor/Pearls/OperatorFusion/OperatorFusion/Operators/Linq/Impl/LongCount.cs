// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the LongCount operator using fusion.
//
// BD - October 2014
//
//
// Remarks:
//
//   Notice we don't have an implementation for LongCount(IO<T>, Func<T, bool>). Thanks to
//   fusion, the compiled code will be the same as composing Where and Count.
//

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class LongCountFactory : IFusionOperator
    {
        public Type OutputType => typeof(long);

        public HoistOperations Hoist => HoistOperations.None; // TODO: error handling for checked arithmetic?

        private FieldInfo countFld;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
            countFld = defineField(typeof(long));
            var count = Expression.Field(state, countFld);

            appendToCtor(
                Expression.Assign(count, Expression.Constant(0L, typeof(long)))
            );
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var count = Expression.Field(state, countFld);

                return
                    Expression.Block(
                        typeof(void),
                        Array.Empty<ParameterExpression>(),
                        Expression.AddAssignChecked(count, Expression.Constant(1L, typeof(long))),
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
