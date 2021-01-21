// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Implementation of the DistinctUntilChanged operator using fusion.
//
// BD - October 2014
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OperatorFusion
{
    internal sealed class DistinctUntilChangedFactory : IFusionOperator
    {
        public Type OutputType { get; set; }

        public HoistOperations Hoist => HoistOperations.OnError;

        private FieldInfo hasValueFld;
        private FieldInfo lastValueFld;
        private Func<Expression, Expression, Expression> createCompare;

        public void Initialize(ParameterExpression state, Func<Type, FieldInfo> defineField, Action<Expression> appendToCtor, Expression result, Expression disposable)
        {
            hasValueFld = defineField(typeof(bool));
            lastValueFld = defineField(OutputType);

            createCompare = CreateEqualsCheck(OutputType, t => Expression.Field(state, defineField(t)), appendToCtor);
        }

        public Func<Expression, Expression> CreateOnNext(Func<Expression, Expression> createOnNext, Func<Expression, Expression> createOnError, Expression onCompleted, ParameterExpression state, Expression markDirty)
        {
            return value =>
            {
                var hasValue = Expression.Field(state, hasValueFld);
                var lastValue = Expression.Field(state, lastValueFld);

                var ret = Expression.Label();

                var ex = Expression.Parameter(typeof(Exception));
                var comparerEquals = Expression.Parameter(typeof(bool));

                return
                    Expression.Block(
                        typeof(void),
                        new[] { comparerEquals },
                        Expression.Assign(comparerEquals, Expression.Constant(false)),
                        Expression.IfThen(
                            hasValue,
                            Expression.Block(
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.Assign(
                                            comparerEquals,
                                            createCompare(lastValue, value)
                                        ),
                                        Expression.Empty()
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
                                Expression.Empty()
                            )
                        ),
                        Expression.IfThen(
                            Expression.OrElse(
                                Expression.Not(hasValue),
                                Expression.Not(comparerEquals)
                            ),
                            Expression.Block(
                                Expression.Assign(hasValue, Expression.Constant(true)),
                                Expression.Assign(lastValue, value),
                                markDirty,
                                createOnNext(value),
                                Expression.Empty()
                            )
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

        private static Func<Expression, Expression, Expression> CreateEqualsCheck(Type type, Func<Type, Expression> createField, Action<Expression> appendToCtor)
        {
            if (type == typeof(int)) // TODO: IEquatable<T> etc.
            {
                return (l, r) =>
                {
                    return Expression.Equal(l, r);
                };
            }
            else
            {
                var comparer = createField(typeof(IEqualityComparer<>).MakeGenericType(type));

                appendToCtor(
                    Expression.Assign(
                        comparer,
                        Expression.Property(null, typeof(EqualityComparer<>).MakeGenericType(type).GetProperty("Default"))
                    )
                );

                var equalsMethod = typeof(IEqualityComparer<>).MakeGenericType(type).GetMethods().Single(m => m.Name == "Equals" && !m.IsStatic && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { type, type }));

                return (l, r) =>
                {
                    return Expression.Call(comparer, equalsMethod, l, r);
                };
            }
        }
    }
}
