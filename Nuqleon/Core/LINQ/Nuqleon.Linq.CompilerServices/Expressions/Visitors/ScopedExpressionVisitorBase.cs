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

    using Expression = System.Linq.Expressions.ExpressionSlim;
    using BlockExpression = System.Linq.Expressions.BlockExpressionSlim;
    using CatchBlock = System.Linq.Expressions.CatchBlockSlim;
    using LambdaExpression = System.Linq.Expressions.LambdaExpressionSlim;
    using ParameterExpression = System.Linq.Expressions.ParameterExpressionSlim;

    using ExpressionVisitor = ExpressionSlimVisitor;

    #endregion
#endif

    /// <summary>
    /// Base class for expression visitors with scope tracking facilities.
    /// </summary>
#if USE_SLIM
    public abstract class ScopedExpressionSlimVisitorBase : ExpressionVisitor
#else
    public abstract class ScopedExpressionVisitorBase : ExpressionVisitor
#endif
    {
        /// <summary>
        /// Visits a block expression, applying scope tracking to the block's declared variables.
        /// To visit blocks in derived classes, use <see cref="VisitBlockCore"/>.
        /// </summary>
        /// <param name="node">Block expression to visit.</param>
        /// <returns>Result of visiting the block expression.</returns>
#if USE_SLIM
        protected internal sealed override Expression VisitBlock(BlockExpression node)
#else
        protected sealed override Expression VisitBlock(BlockExpression node)
#endif
        {
            if (node == null)
            {
                return null;
            }

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
        protected virtual Expression VisitBlockCore(BlockExpression node)
        {
            if (node == null)
            {
                return null;
            }

            var variables = VisitAndConvert(node.Variables, nameof(VisitBlockCore));
            var expressions = Visit(node.Expressions);

            return node.Update(variables, expressions);
        }

        /// <summary>
        /// Visits a catch block, applying scope tracking to the block's declared variable (if any).
        /// To visit catch blocks in derived classes, use <see cref="VisitCatchBlockCore"/>.
        /// </summary>
        /// <param name="node">Catch block to visit.</param>
        /// <returns>Result of visiting the catch block.</returns>
#if USE_SLIM
        protected internal sealed override CatchBlock VisitCatchBlock(CatchBlock node)
#else
        protected sealed override CatchBlock VisitCatchBlock(CatchBlock node)
#endif
        {
            if (node == null)
            {
                return null;
            }

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
        protected virtual CatchBlock VisitCatchBlockCore(CatchBlock node)
        {
            if (node == null)
            {
                return null;
            }

            var variable = VisitAndConvert(node.Variable, "VisitCatchBlockCore");
            var filter = Visit(node.Filter);
            var body = Visit(node.Body);

            return node.Update(variable, filter, body);
        }

#if USE_SLIM
        /// <summary>
        /// Visits a lambda expression, applying scope tracking to the lambda's declared parameters.
        /// To visit lambda expressions in derived classes, use <see cref="VisitLambdaCore"/>.
        /// </summary>
        /// <param name="node">Lambda expression to visit.</param>
        /// <returns>Result of visiting the lambda expression.</returns>
        protected internal sealed override Expression VisitLambda(LambdaExpression node)
#else
        /// <summary>
        /// Visits a lambda expression, applying scope tracking to the lambda's declared parameters.
        /// To visit lambda expressions in derived classes, use <see cref="VisitLambdaCore"/>.
        /// </summary>
        /// <typeparam name="T">Type of the delegate.</typeparam>
        /// <param name="node">Lambda expression to visit.</param>
        /// <returns>Result of visiting the lambda expression.</returns>
        protected sealed override Expression VisitLambda<T>(Expression<T> node)
#endif
        {
            if (node == null)
            {
                return null;
            }

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
        protected virtual Expression VisitLambdaCore(LambdaExpression node)
#else
        /// <summary>
        /// Visits a lambda expression. During the call, the block's variables are mapped in the tracked scope symbol table.
        /// </summary>
        /// <typeparam name="T">Type of the delegate.</typeparam>
        /// <param name="node">Lambda expression to visit.</param>
        /// <returns>Result of visiting the lambda expression.</returns>
        protected virtual Expression VisitLambdaCore<T>(Expression<T> node)
#endif
        {
            if (node == null)
            {
                return null;
            }

            var body = Visit(node.Body);
            var parameters = VisitAndConvert(node.Parameters, nameof(VisitLambdaCore));

            return node.Update(body, parameters);
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
