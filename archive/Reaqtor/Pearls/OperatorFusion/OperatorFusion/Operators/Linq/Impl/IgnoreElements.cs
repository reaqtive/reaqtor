// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the IgnoreElements operator using fusion.
//
// BD - October 2014
//

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class IgnoreElementsFactory : IFusionOperator
    {
        public Type OutputType
        {
            get; set;
        }

        public HoistOperations Hoist => HoistOperations.None;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                return Expression.Empty();
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
