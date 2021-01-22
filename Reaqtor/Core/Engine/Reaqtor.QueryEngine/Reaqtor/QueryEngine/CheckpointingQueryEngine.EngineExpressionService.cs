// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        /// <summary>
        /// Simple expression services to simplify expressions coming in to the engine.
        /// </summary>
        /// <remarks>
        /// Currently only applies beta reduction. We could possibly plug in optimizers here as well, at the expense
        /// of diagnostics ("WYSIWYG" for expressions going in through Create/Define and coming out from the registry).
        /// </remarks>
        private sealed class EngineExpressionService : ReactiveExpressionServices
        {
            public EngineExpressionService()
                : base(typeof(IReactiveClient)) // REVIEW: What is the right type to use here?
            {
            }

            public override Expression Normalize(Expression expression)
            {
                var res = base.Normalize(expression);

                res = BetaReducer.ReduceEager(res, BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None, throwOnCycle: false);

                return res;
            }
        }
    }
}
