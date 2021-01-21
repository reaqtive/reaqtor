// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor.ReificationFramework;
using Reaqtor.TestingFramework;

namespace Reaqtor.QueryEngine.ReificationFramework
{
    public class QueryEngineReificationBinder : IReificationBinder<QueryEngineEnvironment>
    {
        private static readonly PropertyInfo s_metadataCtx = ((PropertyInfo)ReflectionHelpers.InfoOf((QueryEngineEnvironment env) => env.MetadataContext));
        private static readonly PropertyInfo s_engineCtx = ((PropertyInfo)ReflectionHelpers.InfoOf((QueryEngineEnvironment env) => env.EngineContext));

        private readonly bool _templatize;

        public QueryEngineReificationBinder(bool templatize = false)
        {
            _templatize = templatize;
        }

        public Expression<Action<QueryEngineEnvironment>> Bind(ServiceOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var reactiveProxyBinder = new ReactiveServiceOperationBinder();
            var bound = reactiveProxyBinder.Visit(operation);

            var envParam = Expression.Parameter(typeof(QueryEngineEnvironment), "env");
            var contextExpr = operation.Kind is ServiceOperationKind.CreateSubscription or ServiceOperationKind.DeleteSubscription
                ? Expression.MakeMemberAccess(envParam, s_engineCtx)
                : Expression.MakeMemberAccess(envParam, s_metadataCtx);

            var ctxParam = Expression.Parameter(typeof(IReactive), "ctx");
            return Expression.Lambda<Action<QueryEngineEnvironment>>(
                Expression.Block(
                    new[] { ctxParam },
                    Expression.Assign(ctxParam, contextExpr),
                    Expression.Invoke(
                        bound,
                        ctxParam
                    )
                ),
                envParam
            );
        }

        public Expression<Action<QueryEngineEnvironment>> Bind(QueryEngineOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return operation.Kind switch
            {
                QueryEngineOperationKind.DifferentialCheckpoint => env => env.DifferentialCheckpoint(),
                QueryEngineOperationKind.FullCheckpoint => env => env.FullCheckpoint(),
                QueryEngineOperationKind.Recovery => env => env.Recovery(),
                _ => throw new NotImplementedException(),
            };
        }

        public Expression<Action<QueryEngineEnvironment>> Optimize(Expression<Action<QueryEngineEnvironment>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var envParam = expression.Parameters[0];
            var metadataParam = Expression.Parameter(typeof(IReactive), "mctx");
            var engineParam = Expression.Parameter(typeof(IReactive), "ectx");
            var ctxOptimizer = new ContextOptimizer(envParam, metadataParam, engineParam);
            var optimized = (LambdaExpression)ctxOptimizer.Visit(expression);
            if (optimized != expression)
            {
                expression = Expression.Lambda<Action<QueryEngineEnvironment>>(
                    Expression.Block(
                        new[] { metadataParam, engineParam },
                        Expression.Assign(metadataParam, Expression.MakeMemberAccess(envParam, s_metadataCtx)),
                        Expression.Assign(engineParam, Expression.MakeMemberAccess(envParam, s_engineCtx)),
                        optimized.Body
                    ),
                    envParam
                );
            }

            var optimizer = new ObserverOptimizer();
            return (Expression<Action<QueryEngineEnvironment>>)optimizer.Visit(expression);
        }

        public QueryEngineEnvironment CreateEnvironment()
        {
            return new QueryEngineEnvironment(_templatize);
        }

        private class OptimizerBase : ScopedExpressionVisitor<List<OptimizedPair>>
        {
            private readonly Stack<IEnumerable<ParameterExpression>> _environment = new();
            private readonly HashSet<ParameterExpression> _descoped = new();

            protected override Expression VisitBlockCore(BlockExpression node)
            {
                var result = (BlockExpression)base.VisitBlockCore(node);

                var descoping = GetAllDescoping().ToList();
                var descopingVars = descoping.Select(op => op.Variable);
                var descopingAsgns = descoping.Select(op => Expression.Assign(op.Variable, op.Expression));

                if (descoping.Any())
                {
                    return Expression.Block(
                        result.Type,
                        result.Variables.Concat(descopingVars),
                        result.Expressions
                            .TakeWhile(e => e.NodeType == ExpressionType.Assign)
                            .Concat(descopingAsgns)
                            .Concat(result.Expressions
                                .SkipWhile(e => e.NodeType == ExpressionType.Assign))
                    );
                }

                return result;
            }

            protected override Expression VisitLambdaCore<T>(Expression<T> node)
            {
                var result = (Expression<T>)base.VisitLambdaCore<T>(node);

                var descoping = GetAllDescoping().ToList();
                var descopingVars = descoping.Select(op => op.Variable);
                var descopingAsgns = descoping.Select(op => Expression.Assign(op.Variable, op.Expression));
                var blockExprs = new Expression[descoping.Count + 1];
                blockExprs[descoping.Count] = result.Body;
                for (var i = 0; i < descoping.Count; ++i)
                {
                    var op = descoping[i];
                    blockExprs[i] = Expression.Assign(op.Variable, op.Expression);
                }

                if (descoping.Any())
                {
                    return Expression.Lambda<T>(
                        Expression.Block(
                            descopingVars,
                            blockExprs
                        ),
                        result.Name,
                        result.TailCall,
                        result.Parameters
                    );
                }

                return result;
            }

            protected override void Push(IEnumerable<ParameterExpression> parameters)
            {
                base.Push(parameters);
                _environment.Push(parameters);
            }

            protected override void Pop()
            {
                base.Pop();
                _environment.Pop();
            }

            protected override List<OptimizedPair> GetState(ParameterExpression parameter)
            {
                return new List<OptimizedPair>();
            }

            protected bool TryAdd(Expression expression, out ParameterExpression parameter)
            {
                var fvs = FreeVariableScanner.Scan(expression);
                parameter = Expression.Parameter(expression.Type);

                var optimizedPairs = default(List<OptimizedPair>);
                if (fvs.Any(p => !TryLookup(p, out optimizedPairs)))
                {
                    parameter = null;
                    return false;
                }

                var op = new OptimizedPair(parameter, expression);
                foreach (var fv in fvs)
                {
                    TryLookup(fv, out optimizedPairs);
                    optimizedPairs.Add(op);
                }

                return true;
            }

            private IEnumerable<OptimizedPair> GetAllDescoping()
            {
                var currentEnvironment = _environment.Peek();
                var descoping = currentEnvironment.SelectMany(GetDescoping);

                foreach (var descope in descoping)
                {
                    if (!_descoped.Contains(descope.Variable))
                    {
                        _descoped.Add(descope.Variable);
                        yield return descope;
                    }
                }
            }

            private IEnumerable<OptimizedPair> GetDescoping(ParameterExpression expression)
            {
                if (TryLookup(expression, out var list))
                {
                    return list;
                }

                return Enumerable.Empty<OptimizedPair>();
            }
        }

        private sealed class ObserverOptimizer : OptimizerBase
        {
            private static readonly MethodInfo s_getObv = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive ctx) => ctx.GetObserver<object>(null))).GetGenericMethodDefinition();

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.IsGenericMethod
                    && node.Method.GetGenericMethodDefinition() == s_getObv
                    && node.Arguments[0].NodeType == ExpressionType.Constant
                    && TryAdd(node, out var result))
                {
                    return result;
                }

                return base.VisitMethodCall(node);
            }
        }

        private sealed class ContextOptimizer : ExpressionVisitor
        {
            private static readonly Expression s_empty = Expression.Empty();

            private readonly ParameterExpression _envParam;
            private readonly ParameterExpression _metadataParam;
            private readonly ParameterExpression _engineParam;

            private readonly List<ParameterExpression> _metadataReplacements;
            private readonly List<ParameterExpression> _engineReplacements;

            public ContextOptimizer(ParameterExpression envParam, ParameterExpression metadataParam, ParameterExpression engineParam)
            {
                _envParam = envParam;
                _metadataParam = metadataParam;
                _engineParam = engineParam;

                _metadataReplacements = new List<ParameterExpression>();
                _engineReplacements = new List<ParameterExpression>();
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                if (node.NodeType == ExpressionType.Assign
                    && node.Left is ParameterExpression leftExpr
                    && node.Right is MemberExpression rightExpr
                    && rightExpr.Expression == _envParam)
                {
                    if (rightExpr.Member == s_metadataCtx)
                    {
                        _metadataReplacements.Add(leftExpr);
                        return s_empty;
                    }
                    else if (rightExpr.Member == s_engineCtx)
                    {
                        _engineReplacements.Add(leftExpr);
                        return s_empty;
                    }
                }

                return base.VisitBinary(node);
            }

            protected override Expression VisitBlock(BlockExpression node)
            {
                var result = (BlockExpression)base.VisitBlock(node);

                if (result.Variables.Contains(_metadataParam) || result.Variables.Contains(_engineParam))
                {
                    return Expression.Block(
                        result.Type,
                        result.Variables.Except(new[] { _metadataParam, _engineParam }),
                        result.Expressions
                    );
                }

                return result;
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                return base.VisitLambda<T>(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (_metadataReplacements.Contains(node))
                {
                    return _metadataParam;
                }
                else if (_engineReplacements.Contains(node))
                {
                    return _engineParam;
                }

                return base.VisitParameter(node);
            }
        }

        private sealed class OptimizedPair
        {
            public OptimizedPair(ParameterExpression variable, Expression expression)
            {
                Variable = variable;
                Expression = expression;
            }

            public ParameterExpression Variable { get; }

            public Expression Expression { get; }
        }
    }
}
