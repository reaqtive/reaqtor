// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Reaqtor.QueryEngine.ReificationFramework;

internal class DeploymentReactiveServiceContext : ReactiveServiceContext
{
    public DeploymentReactiveServiceContext(IReactiveEngineProvider provider)
        : base(new ExpressionServices(), provider)
    {
    }

    private sealed class ExpressionServices : ReactiveExpressionServices
    {
        public ExpressionServices()
            : base(typeof(IReactiveClient))
        {
        }

        public override Expression Normalize(Expression expression)
        {
            var normalized = base.Normalize(expression);
            return ExpressionTupletization.Tupletize(normalized);
        }
    }
}
