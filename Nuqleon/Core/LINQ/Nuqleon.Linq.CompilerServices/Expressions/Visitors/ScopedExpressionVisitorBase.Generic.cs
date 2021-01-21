// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;

#if USE_SLIM
namespace System.Linq.CompilerServices.Bonsai
#else
namespace System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using BlockExpression = BlockExpressionSlim;
    using CatchBlock = CatchBlockSlim;
    using LambdaExpression = LambdaExpressionSlim;
    using ParameterExpression = ParameterExpressionSlim;

    #endregion
#endif

    /// <summary>
    /// Base class for expression visitors with scope tracking facilities.
    /// </summary>
    /// <typeparam name="TExpression">Target type for expressions.</typeparam>
    /// <typeparam name="TLambdaExpression">Target type for lambda expressions. This type has to derive from TExpression.</typeparam>
    /// <typeparam name="TParameterExpression">Target type for parameter expressions. This type has to derive from TExpression.</typeparam>
    /// <typeparam name="TNewExpression">Target type for new expressions. This type has to derive from TExpression.</typeparam>
    /// <typeparam name="TElementInit">Target type for element initializers.</typeparam>
    /// <typeparam name="TMemberBinding">Target type for member bindings.</typeparam>
    /// <typeparam name="TMemberAssignment">Target type for member assignments. This type has to derive from TMemberBinding.</typeparam>
    /// <typeparam name="TMemberListBinding">Target type for member list bindings. This type has to derive from TMemberBinding.</typeparam>
    /// <typeparam name="TMemberMemberBinding">Target type for member member bindings. This type has to derive from TMemberBinding.</typeparam>
    /// <typeparam name="TCatchBlock">Target type for catch blocks.</typeparam>
    /// <typeparam name="TSwitchCase">Target type for switch cases.</typeparam>
    /// <typeparam name="TLabelTarget">Target type for label targets.</typeparam>
#if USE_SLIM
    public abstract class ScopedExpressionSlimVisitorBase<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> : ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>
#else
    public abstract class ScopedExpressionVisitorBase<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> : ExpressionVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>
#endif
        where TLambdaExpression : TExpression
        where TParameterExpression : TExpression
        where TNewExpression : TExpression
        where TMemberAssignment : TMemberBinding
        where TMemberListBinding : TMemberBinding
        where TMemberMemberBinding : TMemberBinding
    {
        /// <summary>
        /// Visits a block expression, applying scope tracking to the block's declared variables.
        /// To visit blocks in derived classes, use <see cref="VisitBlockCore"/>.
        /// </summary>
        /// <param name="node">Block expression to visit.</param>
        /// <returns>Result of visiting the block expression.</returns>
#if USE_SLIM
        protected internal sealed override TExpression VisitBlock(BlockExpression node)
#else
        protected sealed override TExpression VisitBlock(BlockExpression node)
#endif        
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            Push(node.Variables);

            var res = VisitBlockCore(node);

            Pop();

            return res;
        }

        /// <summary>
        /// Visits a block expression. During the call, the block's variables are mapped in the tracked scope symbol table.
        /// </summary>
        /// <param name="node">Block expression to visit.</param>
        /// <returns>Result of visiting the block expression.</returns>
        protected virtual TExpression VisitBlockCore(BlockExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var v = VisitAndConvert<ParameterExpression, TParameterExpression>(node.Variables);
            var e = Visit(node.Expressions);
            return MakeBlock(node, v, e);
        }

        /// <summary>
        /// Visits a catch block, applying scope tracking to the block's declared variable (if any).
        /// To visit catch blocks in derived classes, use <see cref="VisitCatchBlockCore"/>.
        /// </summary>
        /// <param name="node">Catch block to visit.</param>
        /// <returns>Result of visiting the catch block.</returns>
#if USE_SLIM
        protected internal sealed override TCatchBlock VisitCatchBlock(CatchBlock node)
#else
        protected sealed override TCatchBlock VisitCatchBlock(CatchBlock node)
#endif        
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node.Variable != null)
            {
                Push(new[] { node.Variable });
            }

            var res = VisitCatchBlockCore(node);

            if (node.Variable != null)
            {
                Pop();
            }

            return res;
        }

        /// <summary>
        /// Visits a catch block. During the call, the block's variables are mapped in the tracked scope symbol table.
        /// </summary>
        /// <param name="node">Catch block to visit.</param>
        /// <returns>Result of visiting the catch block.</returns>
        protected virtual TCatchBlock VisitCatchBlockCore(CatchBlock node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var v = VisitAndConvert<TParameterExpression>(node.Variable);
            var b = Visit(node.Body);
            var f = Visit(node.Filter);
            return MakeCatchBlock(node, v, b, f);
        }

#if USE_SLIM
        /// <summary>
        /// Visits a lambda expression, applying scope tracking to the lambda's declared parameters.
        /// To visit lambda expressions in derived classes, use <see cref="VisitLambdaCore"/>.
        /// </summary>
        /// <param name="node">Lambda expression to visit.</param>
        /// <returns>Result of visiting the lambda expression.</returns>
        protected internal sealed override TExpression VisitLambda(LambdaExpression node)
#else
        /// <summary>
        /// Visits a lambda expression, applying scope tracking to the lambda's declared parameters.
        /// To visit lambda expressions in derived classes, use <see cref="VisitLambdaCore"/>.
        /// </summary>
        /// <typeparam name="T">Type of the delegate.</typeparam>
        /// <param name="node">Lambda expression to visit.</param>
        /// <returns>Result of visiting the lambda expression.</returns>
        protected sealed override TExpression VisitLambda<T>(Expression<T> node)
#endif
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            Push(node.Parameters);

#if USE_SLIM
            var res = VisitLambdaCore(node);
#else
            var res = VisitLambdaCore<T>(node);
#endif

            Pop();

            return res;
        }

#if USE_SLIM
        /// <summary>
        /// Visits a lambda expression. During the call, the block's variables are mapped in the tracked scope symbol table.
        /// </summary>
        /// <param name="node">Lambda expression to visit.</param>
        /// <returns>Result of visiting the lambda expression.</returns>
        protected virtual TExpression VisitLambdaCore(LambdaExpression node)
#else
        /// <summary>
        /// Visits a lambda expression. During the call, the block's variables are mapped in the tracked scope symbol table.
        /// </summary>
        /// <typeparam name="T">Type of the delegate.</typeparam>
        /// <param name="node">Lambda expression to visit.</param>
        /// <returns>Result of visiting the lambda expression.</returns>
        protected virtual TExpression VisitLambdaCore<T>(Expression<T> node)
#endif
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var b = Visit(node.Body);
            var p = VisitAndConvert<ParameterExpression, TParameterExpression>(node.Parameters);
#if USE_SLIM
            return MakeLambda(node, b, p);
#else
            return MakeLambda<T>(node, b, p);
#endif
        }

        /// <summary>
        /// Pushes the parameters of a new declaration site into a new scope.
        /// </summary>
        /// <param name="parameters">Parameters of the declaration site.</param>
        protected abstract void Push(IEnumerable<ParameterExpression> parameters);

        /// <summary>
        /// Pops a scope.
        /// </summary>
        protected abstract void Pop();
    }
}
