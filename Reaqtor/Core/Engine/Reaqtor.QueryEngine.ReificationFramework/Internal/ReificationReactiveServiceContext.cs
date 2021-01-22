// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.QueryEngine.ReificationFramework
{
    internal class ReificationReactiveServiceContext : ReactiveServiceContext
    {
        public ReificationReactiveServiceContext(IReactiveEngineProvider provider)
            : base(new BetaReducingExpressionServices(), provider)
        {
        }

        private sealed class BetaReducingExpressionServices : ReactiveExpressionServices
        {
            public BetaReducingExpressionServices()
                : base(typeof(IReactiveClient))
            {
            }

            public override Expression Normalize(Expression expression)
            {
                var normalized = base.Normalize(expression);

                return BetaReducer.ReduceEager(
                    normalized,
                    BetaReductionNodeTypes.Unrestricted,
                    BetaReductionRestrictions.None,
                    true);
            }
        }
    }
}
