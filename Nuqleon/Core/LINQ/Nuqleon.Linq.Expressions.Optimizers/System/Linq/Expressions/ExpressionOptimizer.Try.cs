// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        // NB: We never take out empty try blocks that have a finally or fault handler, because
        //     these handlers provide the guarantee of not being interrupted by asynchronous
        //     exceptions and are often used for concurrency-safe programming.

        /// <summary>
        /// Visits a try expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The try expression to visit.</param>
        /// <returns>The result of optimizing the try expression.</returns>
        protected override Expression VisitTry(TryExpression node)
        {
            var res = (TryExpression)base.VisitTry(node);

            AssertTypes(node, res);

            var opt = SimplifyTry(res);

            AssertTypes(node, opt);

            if (opt.NodeType != ExpressionType.Try)
            {
                return opt;
            }

            var optimizedTry = (TryExpression)opt;

            var body = optimizedTry.Body;
            var @finally = optimizedTry.Finally;

            if (IsPure(body))
            {
                if (@finally == null || HasConstantValue(@finally))
                {
                    return ChangeType(body, optimizedTry.Type);
                }
                else
                {
                    return Expression.MakeTry(optimizedTry.Type, body, @finally, fault: null, handlers: null);
                }
            }
            else if (AlwaysThrows(body))
            {
                // NB: We can possibly evaluate pure catch blocks ahead of time if we know the
                //     body throws. However, we'd need to know the exact runtime type of the
                //     exception being thrown.

                return EvaluateTryThrow(optimizedTry);
            }

            return optimizedTry;
        }

        /// <summary>
        /// Simplifies the specified try expression by removing unreachable handlers.
        /// </summary>
        /// <param name="node">The try expression to simplify.</param>
        /// <returns>The result of simplifying the specified try expression.</returns>
        private Expression SimplifyTry(TryExpression node)
        {
            var oldHandlers = node.Handlers;
            var newHandlers = (IList<CatchBlock>)oldHandlers;

            var handlerCount = oldHandlers.Count;

            if (handlerCount > 0)
            {
                var newHandlerList = default(List<CatchBlock>);

                for (var i = 0; i < handlerCount; i++)
                {
                    var handler = oldHandlers[i];

                    if (HasUnreachableFilter(handler) || IsRethrow(handler))
                    {
                        if (newHandlerList == null)
                        {
                            newHandlerList = new List<CatchBlock>(handlerCount);

                            for (var j = 0; j < i; j++)
                            {
                                newHandlerList.Add(oldHandlers[j]);
                            }
                        }
                    }
                    else
                    {
                        newHandlerList?.Add(handler);
                    }
                }

                if (newHandlerList != null)
                {
                    newHandlers = newHandlerList;
                }
            }

            var oldFinally = node.Finally;
            var newFinally = NullIfEmpty(oldFinally);

            var oldFault = node.Fault;
            var newFault = NullIfEmpty(oldFault);

            return Update(node, node.Body, newHandlers, newFinally, newFault);
        }

        /// <summary>
        /// Updates the specified try expression using the specified handlers, finally, and fault blocks.
        /// This method protects against invalid updates of try expressions and can return the body of the
        /// original try expression if no handlers or finally or fault blocks remain.
        /// </summary>
        /// <param name="node">The node to update.</param>
        /// <param name="body">The new body.</param>
        /// <param name="handlers">The new catch handlers.</param>
        /// <param name="finally">The new finally block.</param>
        /// <param name="fault">The new fault block.</param>
        /// <returns>The updated expression or the original expression if nothing changed.</returns>
        private Expression Update(TryExpression node, Expression body, IList<CatchBlock> handlers, Expression @finally, Expression fault)
        {
            if ((handlers == null || handlers.Count == 0) && @finally == null && fault == null)
            {
                return ChangeType(body, node.Type);
            }

            return node.Update(body, handlers, @finally, fault);
        }

        /// <summary>
        /// Returns <c>null</c> if the specified <paramref name="expression"/> represents an empty statement.
        /// </summary>
        /// <param name="expression">The expression to check for the empty statement.</param>
        /// <returns>The original <paramref name="expression"/> if it's not an empty statement, or <c>null</c> if it is an empty statement.</returns>
        private static Expression NullIfEmpty(Expression expression)
        {
            if (expression != null && expression.Type == typeof(void) && expression.NodeType == ExpressionType.Default)
            {
                return null;
            }

            return expression;
        }

        /// <summary>
        /// Checks if the specified catch <paramref name="block"/> in unreachable due to a constant filter
        /// expression that always returns <c>false</c>.
        /// </summary>
        /// <param name="block">The catch block to check.</param>
        /// <returns><c>true</c> if the catch block is unreachable; otherwise, <c>false</c>.</returns>
        private bool HasUnreachableFilter(CatchBlock block)
        {
            var filter = block.Filter;

            return filter != null && HasConstantValue(filter) && !(bool)GetConstantValue(filter);
        }

        /// <summary>
        /// Checks if the specified catch <paramref name="block"/> contains an unconditional rethrow statement.
        /// </summary>
        /// <param name="block">The catch block to check.</param>
        /// <returns><c>true</c> if the catch block has an unconditional rethrow; otherwise, <c>false</c>.</returns>
        private static bool IsRethrow(CatchBlock block)
        {
            if (block.Filter == null)
            {
                if (block.Body.NodeType == ExpressionType.Throw)
                {
                    var @throw = (UnaryExpression)block.Body;
                    if (@throw.Operand == null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Evaluates a try expression containing a throw expression in its body at compile time.
        /// </summary>
        /// <param name="node">The try expression to evaluate.</param>
        /// <returns>The original expression if no evaluation took place; otherwise, the result of partial evaluation.</returns>
        private Expression EvaluateTryThrow(TryExpression node)
        {
            var body = node.Body;

            while (AlwaysThrows(body))
            {
                var handlers = node.Handlers;

                var handlerCount = handlers.Count;
                if (handlerCount == 0)
                    break;

                var @throw = (UnaryExpression)body;
                var exception = @throw.Operand;

                var exceptionType = GetRuntimeExceptionType(exception);
                if (exceptionType == null)
                    break;

                var caught = default(CatchBlock);
                var handlerIndex = -1;

                for (var i = 0; i < handlerCount; i++)
                {
                    var handler = handlers[i];

                    if (handler.Filter != null)
                    {
                        break; // NB: An earlier rewrite should have taken constant filters out.
                    }

                    if (handler.Test.IsAssignableFrom(exceptionType))
                    {
                        caught = handler;
                        handlerIndex = i;
                        break;
                    }
                }

                if (caught == null)
                    break;

                var newBody = caught.Body;

                var variable = caught.Variable;
                if (variable != null)
                {
                    // REVIEW: Does this make the new expression potentially much bigger? Alternatively,
                    //         we could just prune unreachable handlers and retain a try..catch.. block.

                    newBody =
                        Expression.Block(
                            new[] { variable },
                            Expression.Assign(variable, exception),
                            newBody
                        );
                }

                var newHandlers = GetRemainingHandlers(handlers, handlerIndex);

                var updated = Update(node, newBody, newHandlers, node.Finally, node.Fault);
                if (updated.NodeType != ExpressionType.Try)
                {
                    return updated;
                }

                node = (TryExpression)updated;

                body = newBody;
            }

            return node;
        }

        /// <summary>
        /// Gets the remaining catch block handlers from the specified <paramref name="handlers"/> collection with
        /// an index strictly greater than the specified <paramref name="catchingHandlerIndex"/>.
        /// </summary>
        /// <param name="handlers">The handlers from which to get the remaining handlers.</param>
        /// <param name="catchingHandlerIndex">The index of the last handler to remove.</param>
        /// <returns>A list with the remaining handlers, or <c>null</c> if no handlers are remaining.</returns>
        private static List<CatchBlock> GetRemainingHandlers(ReadOnlyCollection<CatchBlock> handlers, int catchingHandlerIndex)
        {
            var handlerCount = handlers.Count;
            var remainingHandlerCount = handlerCount - catchingHandlerIndex - 1;

            var remainingHandlers = default(List<CatchBlock>);

            if (remainingHandlerCount > 0)
            {
                remainingHandlers = new List<CatchBlock>(remainingHandlerCount);

                for (var i = catchingHandlerIndex + 1; i < handlerCount; i++)
                {
                    remainingHandlers.Add(handlers[i]);
                }
            }

            return remainingHandlers;
        }

        /// <summary>
        /// Gets the runtime exception type of the result of evaluating the specified <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The expression whose runtime type to get.</param>
        /// <returns>The runtime type of the exception, if statically known; otherwise, <c>null</c>.</returns>
        private Type GetRuntimeExceptionType(Expression expression)
        {
            // NB: When throwing non-Exception objects, behavior of wrapping these
            //     depends on configuration, so we can't predict the type that will
            //     be observed by catch blocks.

            var res = default(Type);

            if (typeof(Exception).IsAssignableFrom(expression.Type))
            {
                // NB: If the exception object is null, the exception type is
                //     changed to NullReferenceException.

                if (IsAlwaysNull(expression))
                {
                    return typeof(NullReferenceException);
                }

                res = GetRuntimeType(expression);
            }

            return res;
        }
    }
}
