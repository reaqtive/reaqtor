// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of a Sink operator used to transition between fused segments and
// the outside world.
//
// BD - October 2014
//

using System;
using System.Linq.Expressions;
using System.Reflection;

using RuntimeLib;

namespace OperatorFusion
{
    internal sealed class SinkFactory : IFusionOperator
    {
        public Type OutputType { get; set; }

        public HoistOperations Hoist => HoistOperations.None;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var snk = Expression.Field(state, typeof(Sink<>).MakeGenericType(OutputType).GetField("_observer", BindingFlags.NonPublic | BindingFlags.Instance));

                return Expression.Call(snk, typeof(IObserver<>).MakeGenericType(OutputType).GetMethod("OnNext"), value);
            };
        }

        public Func<Expression, Expression> CreateOnError(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return error =>
            {
                var snk = Expression.Field(state, typeof(Sink<>).MakeGenericType(OutputType).GetField("_observer", BindingFlags.NonPublic | BindingFlags.Instance));

                return
                    Expression.Block(
                        Expression.Call(snk, typeof(IObserver<>).MakeGenericType(OutputType).GetMethod("OnError"), error),
                        Expression.Call(state, typeof(IDisposable).GetMethod("Dispose")),
                        Expression.Empty()
                    );
            };
        }

        public Expression CreateOnCompleted(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            var snk = Expression.Field(state, typeof(Sink<>).MakeGenericType(OutputType).GetField("_observer", BindingFlags.NonPublic | BindingFlags.Instance));

            return
                Expression.Block(
                    Expression.Call(snk, typeof(IObserver<>).MakeGenericType(OutputType).GetMethod("OnCompleted")),
                    Expression.Call(state, typeof(IDisposable).GetMethod("Dispose")),
                    Expression.Empty()
                );
        }
    }
}
