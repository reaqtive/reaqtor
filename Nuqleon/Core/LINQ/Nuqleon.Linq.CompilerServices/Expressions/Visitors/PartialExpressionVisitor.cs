// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Globalization;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Base class for partial expression visitors that only support specific expression tree nodes.
    /// </summary>
    /// <typeparam name="TExpression">Target type for expressions.</typeparam>
    public class PartialExpressionVisitor<TExpression> : ExpressionVisitor<TExpression>
    {
        /// <summary>
        /// Visits the children of the BinaryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitBinary(BinaryExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the BlockExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitBlock(BlockExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the ConditionalExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitConditional(ConditionalExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the ConstantExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitConstant(ConstantExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the DebugInfoExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitDebugInfo(DebugInfoExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the DefaultExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitDefault(DefaultExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the DynamicExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitDynamic(DynamicExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the extension expression node.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitExtension(Expression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the GotoExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitGoto(GotoExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the IndexExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitIndex(IndexExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the InvocationExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitInvocation(InvocationExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the LabelExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitLabel(LabelExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the LambdaExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitLambda<TDelegate>(Expression<TDelegate> node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the ListInitExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitListInit(ListInitExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the LoopExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitLoop(LoopExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the MemberExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitMember(MemberExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the MemberInitExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitMemberInit(MemberInitExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the MethodCallExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitMethodCall(MethodCallExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the NewExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitNew(NewExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the NewArrayExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitNewArray(NewArrayExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the ParameterExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitParameter(ParameterExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the RuntimeVariablesExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitRuntimeVariables(RuntimeVariablesExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the SwitchExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitSwitch(SwitchExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the TryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitTry(TryExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the TypeBinaryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitTypeBinary(TypeBinaryExpression node) => throw NotSupported(node);

        /// <summary>
        /// Visits the children of the UnaryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override TExpression VisitUnary(UnaryExpression node) => throw NotSupported(node);

        /// <summary>
        /// Gets the exception thrown when a non-supported node is encountered.
        /// </summary>
        /// <param name="node">Node not handled by the visitor.</param>
        /// <returns>Exception that will be thrown in the visit method for the unhandled node.</returns>
        protected virtual Exception NotSupported(Expression node) => new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Expression of type '{0}' is not supported by this visitor.", node.NodeType));
    }
}
