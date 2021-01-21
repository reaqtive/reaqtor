// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace ExpressionDebugger
{
    public class ExpressionEvaluationTracer : ExpressionInstrumenter
    {
        protected override bool ShouldInstrument(Expression expression, out InstrumentationKind kind)
        {
            kind = InstrumentationKind.TryCatchFinally;
            return true;
        }

        protected override bool TryGetEnterExpression(Expression expression, out Expression enterExpression)
        {
            return base.TryGetEnterExpression(expression, out enterExpression);
        }

        protected override bool TryGetExitExpression(Expression expression, out Expression exitExpression)
        {
            return base.TryGetExitExpression(expression, out exitExpression);
        }

        protected override bool TryGetCatchBlocks(Expression expression, out CatchBlock[] catchBlock)
        {
            return base.TryGetCatchBlocks(expression, out catchBlock);
        }
    }
}
