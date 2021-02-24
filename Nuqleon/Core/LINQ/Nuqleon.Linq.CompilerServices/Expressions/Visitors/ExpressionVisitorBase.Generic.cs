// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.ObjectModel;

#if USE_SLIM
using System.Linq.CompilerServices;
#else
using System.Linq.Expressions;
#endif

#if USE_SLIM
namespace System.Linq.Expressions
#else
namespace System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using BinaryExpressionAlias = BinaryExpressionSlim;
    using BlockExpressionAlias = BlockExpressionSlim;
    using ConditionalExpressionAlias = ConditionalExpressionSlim;
    using ConstantExpressionAlias = ConstantExpressionSlim;
    using DefaultExpressionAlias = DefaultExpressionSlim;
    using ExpressionAlias = ExpressionSlim;
    using GotoExpressionAlias = GotoExpressionSlim;
    using IArgumentProviderAlias = IArgumentProviderSlim;
    using IndexExpressionAlias = IndexExpressionSlim;
    using InvocationExpressionAlias = InvocationExpressionSlim;
    using LabelExpressionAlias = LabelExpressionSlim;
    using LambdaExpressionAlias = LambdaExpressionSlim;
    using ListInitExpressionAlias = ListInitExpressionSlim;
    using LoopExpressionAlias = LoopExpressionSlim;
    using MemberExpressionAlias = MemberExpressionSlim;
    using MemberInitExpressionAlias = MemberInitExpressionSlim;
    using MethodCallExpressionAlias = MethodCallExpressionSlim;
    using NewArrayExpressionAlias = NewArrayExpressionSlim;
    using NewExpressionAlias = NewExpressionSlim;
    using ParameterExpressionAlias = ParameterExpressionSlim;
    using SwitchExpressionAlias = SwitchExpressionSlim;
    using TryExpressionAlias = TryExpressionSlim;
    using TypeBinaryExpressionAlias = TypeBinaryExpressionSlim;
    using UnaryExpressionAlias = UnaryExpressionSlim;

    #endregion
#else
    #region Aliases

    using BinaryExpressionAlias = BinaryExpression;
    using BlockExpressionAlias = BlockExpression;
    using ConditionalExpressionAlias = ConditionalExpression;
    using ConstantExpressionAlias = ConstantExpression;
    using DefaultExpressionAlias = DefaultExpression;
    using ExpressionAlias = Expression;
    using GotoExpressionAlias = GotoExpression;
    using IndexExpressionAlias = IndexExpression;
    using InvocationExpressionAlias = InvocationExpression;
    using LabelExpressionAlias = LabelExpression;
    using LambdaExpressionAlias = LambdaExpression;
    using ListInitExpressionAlias = ListInitExpression;
    using LoopExpressionAlias = LoopExpression;
    using MemberExpressionAlias = MemberExpression;
    using MemberInitExpressionAlias = MemberInitExpression;
    using MethodCallExpressionAlias = MethodCallExpression;
    using NewArrayExpressionAlias = NewArrayExpression;
    using NewExpressionAlias = NewExpression;
    using ParameterExpressionAlias = ParameterExpression;
    using SwitchExpressionAlias = SwitchExpression;
    using TryExpressionAlias = TryExpression;
    using TypeBinaryExpressionAlias = TypeBinaryExpression;
    using UnaryExpressionAlias = UnaryExpression;

    #endregion
#endif

    /// <summary>
    /// Base class for expression visitors that rewrite an expression tree into a target type.
    /// </summary>
    /// <typeparam name="TExpression">Target type for expressions.</typeparam>
#if USE_SLIM
    public abstract class ExpressionSlimVisitor<TExpression>
#else
    public abstract class ExpressionVisitor<TExpression>
#endif
    {
        /// <summary>
        /// Visits the specified expression and rewrites it to the target expression type.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        public virtual TExpression Visit(ExpressionAlias node)
        {
            if (node == null)
            {
                return default;
            }

            switch (node.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AndAssign:
                case ExpressionType.Assign:
                case ExpressionType.DivideAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.OrAssign:
                case ExpressionType.PowerAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                    return VisitBinary((BinaryExpressionAlias)node);

                case ExpressionType.Block:
                    return VisitBlock((BlockExpressionAlias)node);

                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpressionAlias)node);

                case ExpressionType.Conditional:
                    return VisitConditional((ConditionalExpressionAlias)node);

                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpressionAlias)node);

#if !USE_SLIM
                case ExpressionType.DebugInfo:
                    return VisitDebugInfo((DebugInfoExpression)node);
#endif

                case ExpressionType.Default:
                    return VisitDefault((DefaultExpressionAlias)node);

#if !USE_SLIM
                case ExpressionType.Dynamic:
                    return VisitDynamic((DynamicExpression)node);
#endif

                case ExpressionType.Extension:
                    return VisitExtension(node);

                case ExpressionType.Goto:
                    return VisitGoto((GotoExpressionAlias)node);

                case ExpressionType.Index:
                    return VisitIndex((IndexExpressionAlias)node);

                case ExpressionType.Invoke:
                    return VisitInvocation((InvocationExpressionAlias)node);

                case ExpressionType.Label:
                    return VisitLabel((LabelExpressionAlias)node);

                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpressionAlias)node);

                case ExpressionType.ListInit:
                    return VisitListInit((ListInitExpressionAlias)node);

                case ExpressionType.Loop:
                    return VisitLoop((LoopExpressionAlias)node);

                case ExpressionType.MemberAccess:
                    return VisitMember((MemberExpressionAlias)node);

                case ExpressionType.MemberInit:
                    return VisitMemberInit((MemberInitExpressionAlias)node);

                case ExpressionType.New:
                    return VisitNew((NewExpressionAlias)node);

                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                    return VisitNewArray((NewArrayExpressionAlias)node);

                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpressionAlias)node);

#if !USE_SLIM
                case ExpressionType.RuntimeVariables:
                    return VisitRuntimeVariables((RuntimeVariablesExpression)node);
#endif

                case ExpressionType.Switch:
                    return VisitSwitch((SwitchExpressionAlias)node);

                case ExpressionType.Try:
                    return VisitTry((TryExpressionAlias)node);

                case ExpressionType.TypeEqual:
                case ExpressionType.TypeIs:
                    return VisitTypeBinary((TypeBinaryExpressionAlias)node);

                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.OnesComplement:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.Quote:
                case ExpressionType.Throw:
                case ExpressionType.TypeAs:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Unbox:
                    return VisitUnary((UnaryExpressionAlias)node);

                default:
                    throw new NotSupportedException();
            }
        }

#if !USE_SLIM
        private sealed class LambdaDispatch : ExpressionVisitor
        {
            private readonly ExpressionVisitor<TExpression> _parent;

            public LambdaDispatch(ExpressionVisitor<TExpression> parent)
            {
                _parent = parent;
            }

            protected override ExpressionAlias VisitLambda<T>(Expression<T> node)
            {
                return new ExpressionHolder { Result = _parent.VisitLambda(node) };
            }
        }

        private sealed class ExpressionHolder : ExpressionAlias
        {
            public TExpression Result;
        }

        private LambdaDispatch _dispatch;

        private TExpression VisitLambda(LambdaExpressionAlias node)
        {
            if (_dispatch == null)
            {
                _dispatch = new LambdaDispatch(this);
            }

            return ((ExpressionHolder)_dispatch.Visit(node)).Result;
        }
#endif

        /// <summary>
        /// Visits the collection of expression nodes and rewrites them to the target expression type.
        /// </summary>
        /// <typeparam name="T">Type of the expressions to rewrite. This type should derive from Expression.</typeparam>
        /// <param name="nodes">Expression nodes to rewrite.</param>
        /// <returns>Collection of rewritten expression nodes.</returns>
        protected ReadOnlyCollection<TExpression> Visit<T>(ReadOnlyCollection<T> nodes)
            where T : ExpressionAlias
        {
            var n = nodes.Count;

            if (n == 0)
            {
                return EmptyReadOnlyCollection<TExpression>.Instance;
            }

            var res = new TExpression[n];

            for (var i = 0; i < n; i++)
            {
                var node = Visit(nodes[i]);
                res[i] = node;
            }

            return new TrueReadOnlyCollection<TExpression>(/* transfer ownership */ res);
        }

#if USE_SLIM
        /// <summary>
        /// Visits the arguments in an argument provider.
        /// </summary>
        /// <param name="nodes">The argument provider whose arguments to visit.</param>
        /// <returns>The rewritten arguments.</returns>
        protected internal ReadOnlyCollection<TExpression> VisitArguments(IArgumentProviderAlias nodes)
        {
            var n = nodes.ArgumentCount;

            var res = new TExpression[n];

            for (var i = 0; i < n; i++)
            {
                var argument = nodes.GetArgument(i);
                var expression = Visit(argument);
                res[i] = expression;
            }

            return new TrueReadOnlyCollection<TExpression>(/* transfer ownership */ res);
        }
#endif

        /// <summary>
        /// Visits the specified expression and rewrites it to the specified target expression type.
        /// </summary>
        /// <typeparam name="TStronglyTypedResult">Type of the result of the rewrite. This type should derive from TExpression.</typeparam>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected TStronglyTypedResult VisitAndConvert<TStronglyTypedResult>(ExpressionAlias node)
            where TStronglyTypedResult : TExpression
        {
            return (TStronglyTypedResult)Visit(node);
        }

        /// <summary>
        /// Visits the collection of expression nodes and rewrites them to the specified target expression type.
        /// </summary>
        /// <typeparam name="T">Type of the expressions to rewrite. This type should derive from Expression.</typeparam>
        /// <typeparam name="TStronglyTypedResult">Type of the results of the rewrite. This type should derive from TExpression.</typeparam>
        /// <param name="nodes">Expression nodes to rewrite.</param>
        /// <returns>Collection of rewritten expression nodes.</returns>
        protected ReadOnlyCollection<TStronglyTypedResult> VisitAndConvert<T, TStronglyTypedResult>(ReadOnlyCollection<T> nodes)
            where T : ExpressionAlias
            where TStronglyTypedResult : TExpression
        {
            var n = nodes.Count;

            if (n == 0)
            {
                return EmptyReadOnlyCollection<TStronglyTypedResult>.Instance;
            }

            var res = new TStronglyTypedResult[n];

            for (var i = 0; i < n; i++)
            {
                var node = VisitAndConvert<TStronglyTypedResult>(nodes[i]);
                res[i] = node;
            }

            return new TrueReadOnlyCollection<TStronglyTypedResult>(/* transfer ownership */ res);
        }

        /// <summary>
        /// Visits the specified node using the specified visitor function if it's not null.
        /// </summary>
        /// <typeparam name="T">Type of the node to visit.</typeparam>
        /// <typeparam name="TResult">Type of the result of visiting the node.</typeparam>
        /// <param name="node">Node to visit.</param>
        /// <param name="nodeVisitor">Visitor function to apply to the node.</param>
        /// <returns>Result of applying the visitor function to the node.</returns>
        protected static TResult VisitIfNotNull<T, TResult>(T node, Func<T, TResult> nodeVisitor)
        {
            if (nodeVisitor == null)
                throw new ArgumentNullException(nameof(nodeVisitor));

            if (node != null)
            {
                return nodeVisitor(node);
            }

            return default;
        }

        /// <summary>
        /// Visits the children of the BinaryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
# endif
        abstract TExpression VisitBinary(BinaryExpressionAlias node);

        /// <summary>
        /// Visits the children of the BlockExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitBlock(BlockExpressionAlias node);

        /// <summary>
        /// Visits the children of the ConditionalExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitConditional(ConditionalExpressionAlias node);

        /// <summary>
        /// Visits the children of the ConstantExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitConstant(ConstantExpressionAlias node);

#if !USE_SLIM
        /// <summary>
        /// Visits the children of the DebugInfoExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected abstract TExpression VisitDebugInfo(DebugInfoExpression node);
#endif

        /// <summary>
        /// Visits the children of the DefaultExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitDefault(DefaultExpressionAlias node);

#if !USE_SLIM
        /// <summary>
        /// Visits the children of the DynamicExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected abstract TExpression VisitDynamic(DynamicExpression node);
#endif

        /// <summary>
        /// Visits the children of the extension expression node.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitExtension(ExpressionAlias node);

        /// <summary>
        /// Visits the children of the GotoExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitGoto(GotoExpressionAlias node);

        /// <summary>
        /// Visits the children of the IndexExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitIndex(IndexExpressionAlias node);

        /// <summary>
        /// Visits the children of the InvocationExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitInvocation(InvocationExpressionAlias node);

        /// <summary>
        /// Visits the children of the LabelExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitLabel(LabelExpressionAlias node);

#if USE_SLIM
        /// <summary>
        /// Visits the children of the LambdaExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected internal abstract TExpression VisitLambda(LambdaExpressionAlias node);
#else
        /// <summary>
        /// Visits the children of the LambdaExpression.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected abstract TExpression VisitLambda<T>(Expression<T> node);
#endif

        /// <summary>
        /// Visits the children of the ListInitExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitListInit(ListInitExpressionAlias node);

        /// <summary>
        /// Visits the children of the LoopExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitLoop(LoopExpressionAlias node);

        /// <summary>
        /// Visits the children of the MemberExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitMember(MemberExpressionAlias node);

        /// <summary>
        /// Visits the children of the MemberInitExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitMemberInit(MemberInitExpressionAlias node);

        /// <summary>
        /// Visits the children of the MethodCallExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitMethodCall(MethodCallExpressionAlias node);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Replace New suffix. (Name of expression tree node.)

        /// <summary>
        /// Visits the children of the NewExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitNew(NewExpressionAlias node);

#pragma warning restore CA1711
#pragma warning restore IDE0079

        /// <summary>
        /// Visits the children of the NewArrayExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitNewArray(NewArrayExpressionAlias node);

        /// <summary>
        /// Visits the children of the ParameterExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitParameter(ParameterExpressionAlias node);

#if !USE_SLIM
        /// <summary>
        /// Visits the children of the RuntimeVariablesExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected abstract TExpression VisitRuntimeVariables(RuntimeVariablesExpression node);
#endif

        /// <summary>
        /// Visits the children of the SwitchExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitSwitch(SwitchExpressionAlias node);

        /// <summary>
        /// Visits the children of the TryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitTry(TryExpressionAlias node);

        /// <summary>
        /// Visits the children of the TypeBinaryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitTypeBinary(TypeBinaryExpressionAlias node);

        /// <summary>
        /// Visits the children of the UnaryExpression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
#if USE_SLIM
        protected internal
#else
        protected
#endif
        abstract TExpression VisitUnary(UnaryExpressionAlias node);
    }
}
