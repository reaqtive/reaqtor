// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Reaqtor.TestingFramework;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// A reified operation binder for client environments.
    /// </summary>
    public class ReactiveProxyReificationBinder : IReificationBinder<IReactiveClientEnvironment>
    {
        private static readonly MethodInfo s_waitMethod = (MethodInfo)ReflectionHelpers.InfoOf((Task t) => t.Wait());
        private static readonly PropertyInfo s_getContextProperty = (PropertyInfo)ReflectionHelpers.InfoOf((IReactiveClientEnvironment env) => env.Context);

        private readonly Func<IReactiveClientEnvironment> _clientFactory;

        /// <summary>
        /// Instantiates the binder.
        /// </summary>
        /// <param name="clientFactory">A client environment factory.</param>
        public ReactiveProxyReificationBinder(Func<IReactiveClientEnvironment> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Binds a service operation to the environment.
        /// </summary>
        /// <param name="operation">The operation to bind.</param>
        /// <returns>
        /// A lambda expression that can be evaluated with an environment
        /// instance returned from the `CreateEnvironment` method.
        /// </returns>
        public Expression<Action<IReactiveClientEnvironment>> Bind(ServiceOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var binder = new ReactiveProxyServiceOperationBinder();
            var clientBound = binder.Visit(operation);
            return Expression.Parameter(typeof(IReactiveClientEnvironment), "env").Let(env =>
                Expression.Lambda<Action<IReactiveClientEnvironment>>(
                    Expression.Call(
                        Expression.Invoke(
                            clientBound,
                            Expression.MakeMemberAccess(
                                env,
                                s_getContextProperty
                            )
                        ),
                        s_waitMethod
                    ),
                    env
                )
            ).BetaReduce();
        }

        /// <summary>
        /// Binds a query engine operation to the environment.
        /// </summary>
        /// <param name="operation">The operation to bind.</param>
        /// <returns>
        /// A lambda expression that can be evaluated with an environment
        /// instance returned from the `CreateEnvironment` method.
        /// </returns>
        public Expression<Action<IReactiveClientEnvironment>> Bind(QueryEngineOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return operation.Kind switch
            {
                QueryEngineOperationKind.DifferentialCheckpoint => InlineConstant<IReactiveClientEnvironment, Uri>((env, uri) => env.DifferentialCheckpoint(uri), operation.TargetObjectUri),
                QueryEngineOperationKind.FullCheckpoint => InlineConstant<IReactiveClientEnvironment, Uri>((env, uri) => env.FullCheckpoint(uri), operation.TargetObjectUri),
                QueryEngineOperationKind.Recovery => InlineConstant<IReactiveClientEnvironment, Uri>((env, uri) => env.Recover(uri), operation.TargetObjectUri),
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Optimizes an expression a bound reified operation.
        /// </summary>
        /// <param name="expression">The expression to optimize.</param>
        /// <returns>The optimized expression.</returns>
        /// <remarks>
        /// E.g., an optimization might share resources over successive calls in a loop.
        /// </remarks>
        public Expression<Action<IReactiveClientEnvironment>> Optimize(Expression<Action<IReactiveClientEnvironment>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var envParam = expression.Parameters[0];
            var ctxParam = Expression.Parameter(typeof(IReactiveProxy), "ctx");
            var ctxOptimizer = new ContextOptimizer(envParam, ctxParam);
            var optimized = (LambdaExpression)ctxOptimizer.Visit(expression);
            if (optimized != expression)
            {
                expression = Expression.Lambda<Action<IReactiveClientEnvironment>>(
                    Expression.Block(
                        new[] { ctxParam },
                        Expression.Assign(ctxParam, Expression.MakeMemberAccess(envParam, s_getContextProperty)),
                        optimized.Body
                    ),
                    envParam
                );
            }

            return expression;
        }

        /// <summary>
        /// Creates a fresh instance of the environment.
        /// </summary>
        /// <returns>A fresh instance of the environment.</returns>
        public IReactiveClientEnvironment CreateEnvironment()
        {
            return _clientFactory();
        }

        private static Expression<Action<TParam>> InlineConstant<TParam, TConstant>(Expression<Action<TParam, TConstant>> lambda, TConstant constant)
        {
            var parameter = lambda.Parameters[0];
            return Expression.Lambda<Action<TParam>>(
                Expression.Invoke(
                    lambda,
                    parameter,
                    Expression.Constant(constant, typeof(TConstant))
                ),
                parameter
            ).BetaReduce();
        }

        private sealed class ContextOptimizer : ExpressionVisitor
        {
            private static readonly Expression s_empty = Expression.Empty();

            private readonly ParameterExpression _envParam;
            private readonly ParameterExpression _ctxParam;

            private readonly List<ParameterExpression> _ctxReplacements;

            public ContextOptimizer(ParameterExpression envParam, ParameterExpression ctxParam)
            {
                _envParam = envParam;
                _ctxParam = ctxParam;

                _ctxReplacements = new List<ParameterExpression>();
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                if (node.NodeType == ExpressionType.Assign
                    && node.Left is ParameterExpression leftExpr
                    && node.Right is MemberExpression rightExpr
                    && rightExpr.Expression == _envParam
                    && rightExpr.Member == s_getContextProperty)
                {
                    _ctxReplacements.Add(leftExpr);
                    return s_empty;
                }

                return base.VisitBinary(node);
            }

            protected override Expression VisitBlock(BlockExpression node)
            {
                var result = (BlockExpression)base.VisitBlock(node);

                if (result.Variables.Contains(_ctxParam))
                {
                    return Expression.Block(
                        result.Type,
                        result.Variables.Except(new[] { _ctxParam }),
                        result.Expressions
                    );
                }

                return result;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression == _envParam && node.Member == s_getContextProperty)
                {
                    return _ctxParam;
                }

                return base.VisitMember(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (_ctxReplacements.Contains(node))
                {
                    return _ctxParam;
                }

                return base.VisitParameter(node);
            }
        }

    }
}
