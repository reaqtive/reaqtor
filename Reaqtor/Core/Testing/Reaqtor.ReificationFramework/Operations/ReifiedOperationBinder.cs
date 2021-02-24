// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.TestingFramework;

namespace Reaqtor.ReificationFramework
{
    internal class ReifiedOperationBinder<TEnvironment> : ReifiedOperationVisitor<Expression<Action<TEnvironment>>>
    {
        private static readonly Expression<Func<IWildcardGenerator, Uri>> s_generateExpr = wcg => wcg.Generate();
        private static readonly MethodInfo s_taskRunMethod = (MethodInfo)ReflectionHelpers.InfoOf(() => Task.Run(default(Action), CancellationToken.None));
        private static readonly PropertyInfo s_IsCancellationRequestedProperty = (PropertyInfo)ReflectionHelpers.InfoOf((CancellationToken token) => token.IsCancellationRequested);

        private readonly IReificationBinder<TEnvironment> _platform;

        public ReifiedOperationBinder(IReificationBinder<TEnvironment> platform)
        {
            _platform = platform;
        }

        protected override Expression<Action<TEnvironment>> MakeAsync(Expression<Action<TEnvironment>> operation, Action<Task> onStart, CancellationToken token)
        {
            var parameter = operation.Parameters[0];
            return Expression.Lambda<Action<TEnvironment>>(
                Expression.Invoke(
                    Expression.Constant(onStart),
                    Expression.Call(
                        s_taskRunMethod,
                        Expression.Lambda<Action>(operation.Body),
                        Expression.Constant(token, typeof(CancellationToken))
                    )
                ),
                parameter
            );
        }

        protected override Expression<Action<TEnvironment>> MakeCatch<T>(Expression<Action<TEnvironment>> operation, Action<T> handler)
        {
            var parameter = operation.Parameters[0];
            var catchParameter = Expression.Parameter(typeof(T));
            return Expression.Lambda<Action<TEnvironment>>(
                Expression.TryCatch(
                    operation.Body,
                    Expression.Catch(
                        catchParameter,
                        Expression.Invoke(
                            Expression.Constant(handler, typeof(Action<T>)),
                            catchParameter
                        )
                    )
                ),
                parameter
            );
        }

        protected override Expression<Action<TEnvironment>> MakeChain(Expression<Action<TEnvironment>> first, IEnumerable<Expression<Action<TEnvironment>>> rest)
        {
            var parameter = first.Parameters[0];
            var bodies = rest.StartWith(first).Select(o => Expression.Invoke(o, parameter).BetaReduce());
            return Expression.Lambda<Action<TEnvironment>>(
                Expression.Block(bodies),
                parameter
            );
        }

        protected override Expression<Action<TEnvironment>> MakeInstrument(Expression<Action<TEnvironment>> operation, Action onEnter, Action onExit)
        {
            var parameter = operation.Parameters[0];
            return Expression.Lambda<Action<TEnvironment>>(
                Expression.Block(
                    Expression.Invoke(Expression.Constant(onEnter)),
                    Expression.TryFinally(
                        operation.Body,
                        Expression.Invoke(Expression.Constant(onExit))
                    )
                ),
                parameter
            );
        }

        protected override Expression<Action<TEnvironment>> MakeLiftWildcards(Expression<Action<TEnvironment>> operation, IWildcardGenerator generator)
        {
            var parameter = operation.Parameters[0];
            var wildcardSubstitutor = new WildcardParameterSubstitutor();
            var substitutedOperation = wildcardSubstitutor.Visit(operation.Body);
            var generateExpr = Expression.Invoke(s_generateExpr, Expression.Constant(generator, typeof(IWildcardGenerator))).BetaReduce();
            var assignments = wildcardSubstitutor.Wildcards.Select(p => Expression.Assign(p, generateExpr));
            return Expression.Lambda<Action<TEnvironment>>(
                Expression.Block(
                    wildcardSubstitutor.Wildcards,
                    assignments.EndWith(substitutedOperation)
                ),
                parameter
            );
        }

        protected override Expression<Action<TEnvironment>> VisitQueryEngineOperation(QueryEngineOperation operation)
        {
            return _platform.Bind(operation);
        }

        protected override Expression<Action<TEnvironment>> MakeRepeat(Expression<Action<TEnvironment>> operation, long count)
        {
            var @break = Expression.Label();
            var param = Expression.Parameter(typeof(long), "i");

            var loop = Expression.Block(
                new[] { param },
                Expression.Assign(
                    param,
                    Expression.Constant(0L, typeof(long))
                ),
                Expression.Loop(
                    Expression.Block(
                        Expression.IfThenElse(
                            Expression.Equal(param, Expression.Constant(count, typeof(long))),
                            Expression.Break(@break),
                            Expression.Default(typeof(bool)) // something that's not typeof(void) to avoid bug in `ConditionalExpression.Update`
                        ),
                        operation.Body,
                        Expression.PostIncrementAssign(param)
                    ),
                    @break
                )
            );

            var parameter = operation.Parameters[0];
            return Expression.Lambda<Action<TEnvironment>>(loop, parameter);
        }

        protected override Expression<Action<TEnvironment>> MakeRepeatUntil(Expression<Action<TEnvironment>> operation, CancellationToken token)
        {
            var @break = Expression.Label();

            var loop = Expression.Block(
                Expression.Loop(
                    Expression.Block(
                        Expression.IfThenElse(
                            Expression.MakeMemberAccess(Expression.Constant(token, typeof(CancellationToken)), s_IsCancellationRequestedProperty),
                            Expression.Break(@break),
                            Expression.Default(typeof(bool)) // something that's not typeof(void) to avoid bug in `ConditionalExpression.Update`
                        ),
                        operation.Body
                    ),
                    @break
                )
            );

            var parameter = operation.Parameters[0];
            return Expression.Lambda<Action<TEnvironment>>(loop, parameter);
        }


        protected override Expression<Action<TEnvironment>> VisitServiceOperation(ServiceOperation operation)
        {
            return _platform.Bind(operation);
        }

        protected override Expression<Action<TEnvironment>> VisitExtension(ReifiedOperation operation)
        {
            throw new NotImplementedException();
        }

        private sealed class WildcardParameterSubstitutor : ExpressionVisitor
        {
            private readonly IDictionary<Uri, ParameterExpression> _wildcards =
                new Dictionary<Uri, ParameterExpression>();

            public IEnumerable<ParameterExpression> Wildcards => _wildcards.Values;

            protected override Expression VisitConstant(ConstantExpression node)
            {

                if (node.Type == typeof(Uri))
                {
                    var uri = (Uri)node.Value;
                    if (_wildcards.TryGetValue(uri, out var parameter))
                    {
                        return parameter;
                    }
                    else if (uri.IsWildcard())
                    {
                        parameter = Expression.Parameter(typeof(Uri), uri.ToCanonicalString());
                        _wildcards.Add(uri, parameter);
                        return parameter;
                    }
                }

                return base.VisitConstant(node);
            }
        }
    }
}
