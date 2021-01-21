// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor.ReificationFramework;
using Reaqtor.TestingFramework;

namespace Reaqtor.Remoting.ReificationFramework
{
    public class ReactiveServiceConnectionReificationBinder : IReificationBinder<ReactiveServiceConnectionEnvironment>
    {
        private static readonly PropertyInfo s_getQcProperty = (PropertyInfo)ReflectionHelpers.InfoOf((ReactiveServiceConnectionEnvironment env) => env.QueryCoordinator);
        private static readonly PropertyInfo s_getQeProperty = (PropertyInfo)ReflectionHelpers.InfoOf((ReactiveServiceConnectionEnvironment env) => env.QueryEvaluator);

        private readonly bool _subscribeToQueryEvaluator;

        public ReactiveServiceConnectionReificationBinder(bool subscribeToQueryEvaluator = false)
        {
            _subscribeToQueryEvaluator = subscribeToQueryEvaluator;
        }

        public Expression<Action<ReactiveServiceConnectionEnvironment>> Bind(ServiceOperation operation)
        {
            var connectionProperty = s_getQcProperty;
            if ((operation.Kind == ServiceOperationKind.CreateSubscription || operation.Kind == ServiceOperationKind.DeleteSubscription) && _subscribeToQueryEvaluator)
            {
                connectionProperty = s_getQeProperty;
            }

            var binder = new ServiceOperationBinder();
            var clientBound = binder.Visit(operation);

            return Expression.Parameter(typeof(ReactiveServiceConnectionEnvironment), "env").Let(env =>
                Expression.Lambda<Action<ReactiveServiceConnectionEnvironment>>(
                    Expression.Invoke(
                        clientBound,
                        Expression.MakeMemberAccess(
                            env,
                            connectionProperty
                        )
                    ),
                    env
                )
            ).BetaReduce();
        }

        public Expression<Action<ReactiveServiceConnectionEnvironment>> Bind(QueryEngineOperation operation)
        {
            var uri = operation.TargetObjectUri;

            return operation.Kind switch
            {
                QueryEngineOperationKind.DifferentialCheckpoint => InlineClosures(env => env.DifferentialCheckpoint(uri)),
                QueryEngineOperationKind.FullCheckpoint => InlineClosures(env => env.FullCheckpoint(uri)),
                QueryEngineOperationKind.Recovery => InlineClosures(env => env.Recover(uri)),
                _ => throw new NotImplementedException(),
            };
        }

        public Expression<Action<ReactiveServiceConnectionEnvironment>> Optimize(Expression<Action<ReactiveServiceConnectionEnvironment>> expression)
        {
            return expression;
        }

        public ReactiveServiceConnectionEnvironment CreateEnvironment()
        {
            return new ReactiveServiceConnectionEnvironment();
        }

        private static Expression<Action<ReactiveServiceConnectionEnvironment>> InlineClosures(Expression<Action<ReactiveServiceConnectionEnvironment>> lambda)
        {
            return lambda.InlineClosures();
        }
    }
}
