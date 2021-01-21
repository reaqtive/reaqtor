// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

/*
 * Example of a simple continuation passing style (CPS) transformation, in order to rewrite synchronous
 * query execution plan expression trees into asynchronous variants, without any significant changes to
 * existing rewriters that assume the synchronous model.
 *
 * This file provides the base class for all CPS rewriters, performing the common logic required to scan
 * expression trees and maintain continuation objects.
 *
 * BD - 5/8/2013
 */

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Base class for expression tree rewriters to turn expressions into a Continuation Passing Style (CPS).
    /// </summary>
    /// <typeparam name="TContinuationState">Type of the state object threaded through the CPS transformation.</typeparam>
    public abstract class CpsRewriterBase<TContinuationState>
    {
        /// <summary>
        /// Rewrites the given expression into a CPS-based expression using the specified initial CPS rewrite state.
        /// </summary>
        /// <param name="expression">Expression to rewrite to a CPS-based expression.</param>
        /// <param name="initialState">Initial CPS rewrite state.</param>
        /// <returns>Rewritten expression in CPS style.</returns>
        protected Expression RewriteCore(Expression expression, TContinuationState initialState) => RewriteCore(expression, initialState, inline: true);

        /// <summary>
        /// Rewrites the given expression into a CPS-based expression using the specified initial CPS rewrite state.
        /// </summary>
        /// <param name="expression">Expression to rewrite to a CPS-based expression.</param>
        /// <param name="initialState">Initial CPS rewrite state.</param>
        /// <param name="inline">Indicates whether or not to inline invocations when possible. This optimization may defeat evaluation order for specific subclasses, so it can be disabled.</param>
        /// <returns>Rewritten expression in CPS style.</returns>
        protected Expression RewriteCore(Expression expression, TContinuationState initialState, bool inline)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Impl(this, initialState).Visit(expression);
            if (inline)
            {
                res = BetaReducer.Reduce(res);
            }

            res = new LambdaParameterRenamer().Visit(res);

            return res;
        }

        private sealed class LambdaParameterRenamer : ExpressionVisitor
        {
            private readonly Stack<Dictionary<ParameterExpression, ParameterExpression>> _environment = new();
            private int index = 0;

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var ps = node.Parameters;
                var n = ps.Count;

                var parameters = new ParameterExpression[n];
                var map = new Dictionary<ParameterExpression, ParameterExpression>();

                for (var i = 0; i < n; i++)
                {
                    var p = ps[i];

                    if (p.Name.StartsWith(CpsConstants.PREFIX, StringComparison.OrdinalIgnoreCase))
                    {
                        var r = Expression.Parameter(p.Type, "x" + index++);
                        map[p] = r;
                        parameters[i] = r;
                    }
                    else
                    {
                        map[p] = p;
                        parameters[i] = p;
                    }
                }

                _environment.Push(map);

                var body = Visit(node.Body);

                _environment.Pop();

                return Expression.Lambda<T>(body, parameters);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                foreach (var frame in _environment)
                {
                    if (frame.TryGetValue(node, out ParameterExpression res))
                    {
                        return res;
                    }
                }

                return node;
            }
        }

        /// <summary>
        /// Finds the asynchronous method to rewrite the given method to.
        /// The specified method has the UseAsyncMethodAttribute applied, but resolution of the method can be overridden by subclasses.
        /// By default, the GetContinuationParameters method is used to supply additional parameters that appear on the asynchronous method, used to find the right overload of a method with the same name as the synchronous one.
        /// </summary>
        /// <param name="method">Method to find the target asynchronous method for.</param>
        /// <returns>Asynchronous method to rewrite the call site for the specified method.</returns>
        protected virtual MethodInfo FindAsyncMethod(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            var ops = method.GetParameters().Select(p => p.ParameterType);
            var nps = ops.Concat(GetContinuationParameters(method)).ToArray();
            var flg = (!method.IsPublic ? BindingFlags.NonPublic : BindingFlags.Public) | BindingFlags.Instance | BindingFlags.Static;
            var nam = GetAsyncMethodName(method);

            var tgt = default(MethodInfo);
            if (method.IsGenericMethod)
            {
                var gas = method.GetGenericArguments();
                var nga = gas.Length;
                var cds = method.DeclaringType.GetMethods(flg).Where(m => m.Name == nam && m.IsGenericMethod && m.GetGenericArguments().Length == nga).ToArray();
                foreach (var c in cds)
                {
                    var clm = c.MakeGenericMethod(gas);
                    if (clm.GetParameters().Select(p => p.ParameterType).SequenceEqual(nps))
                    {
                        tgt = clm;
                        break;
                    }
                }
            }
            else
            {
                tgt = method.DeclaringType.GetMethod(nam, flg, binder: null, nps, modifiers: null);
            }

            return tgt;
        }

        /// <summary>
        /// Gets the asynchronous method name for the given synchronous method.
        /// By default, the name is the same as the synchronous method. The GetContinuationParameters method will be called to find additional parameters that designate the asynchronous method overload to look for.
        /// </summary>
        /// <param name="method">Method to find the name of the corresponding asynchronous method for.</param>
        /// <returns>Name of the asynchronous method to search for.</returns>
        protected virtual string GetAsyncMethodName(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            return method.Name;
        }

        /// <summary>
        /// Checks whether the discovered asynchronous method matches the specified synchronous method.
        /// </summary>
        /// <param name="synchronousMethod">Synchronous method to match.</param>
        /// <param name="asynchronousMethod">Asynchronous method; null if not found.</param>
        protected virtual void CheckAsyncMethod(MethodInfo synchronousMethod, MethodInfo asynchronousMethod)
        {
            if (synchronousMethod == null)
                throw new ArgumentNullException(nameof(synchronousMethod));

            if (asynchronousMethod == null)
                throw new InvalidOperationException("Asynchronous counterpart method for '" + synchronousMethod.Name + "' not found.");
        }

        /// <summary>
        /// Gets the continuation parameter types that are used during method overload resolution by appending those to the parameters type of the synchronous method.
        /// </summary>
        /// <param name="method">Method to find continuation parameters for, e.g. by inspecting its return type and creating the corresponding callback parameter type.</param>
        /// <returns>Continuation parameter types to check for when searching for an asynchronous method overload.</returns>
        protected abstract IEnumerable<Type> GetContinuationParameters(MethodInfo method);

        /// <summary>
        /// Creates an asynchronous method call with the given continuation.
        /// </summary>
        /// <param name="instance">Instance of the method call.</param>
        /// <param name="method">Method to call.</param>
        /// <param name="arguments">Arguments of the method call.</param>
        /// <param name="continuation">Continuation to supply to the asynchronous method.</param>
        /// <returns>Expression representing an asynchronous method call with the given continuation.</returns>
        protected abstract Expression MakeAsyncMethodCall(Expression instance, MethodInfo method, IEnumerable<Expression> arguments, TContinuationState continuation);

        /// <summary>
        /// Creates a new continuation with the given result parameter to run the specified remainder computation.
        /// </summary>
        /// <param name="remainder">Remainder of the expression to evaluate upon invocation of the continuation.</param>
        /// <param name="resultParameter">Parameter with the result of the expression evaluation preceding the invocation of the continuation.</param>
        /// <param name="currentContinuation">Current continuation being processed upon requesting a new continuation.</param>
        /// <returns>Continuation object that will execute the remainder computation, given the specified result parameter.</returns>
        protected abstract TContinuationState MakeContinuation(Expression remainder, ParameterExpression resultParameter, TContinuationState currentContinuation);

        /// <summary>
        /// Creates an invocation of the given continuation.
        /// </summary>
        /// <param name="continuation">Continuation to invoke.</param>
        /// <param name="argument">Argument to pass to the continuation's invocation.</param>
        /// <returns>Expression representing the invocation of the continuation.</returns>
        protected abstract Expression InvokeContinuation(TContinuationState continuation, Expression argument);

        private sealed class Impl : ExpressionVisitor
        {
            private readonly CpsRewriterBase<TContinuationState> _parent;
            private readonly Stack<TContinuationState> _continuation;

            public Impl(CpsRewriterBase<TContinuationState> parent, TContinuationState initialState)
            {
                _parent = parent;
                _continuation = new Stack<TContinuationState>();
                _continuation.Push(initialState);
            }

            public override Expression Visit(Expression node)
            {
                var res = default(Expression);

                if (node != null)
                {
                    if (!TryApplyAsyncRewrite(node, out res))
                    {
                        res = _parent.InvokeContinuation(_continuation.Pop(), node);
                    }
                }

                return res;
            }

            private bool TryApplyAsyncRewrite(Expression expression, out Expression result)
            {
                if (expression is MethodCallExpression mce)
                {
                    var mtd = mce.Method;
                    if (mtd.IsDefined(typeof(UseAsyncMethodAttribute), inherit: false))
                    {
                        var tgt = _parent.FindAsyncMethod(mtd);
                        _parent.CheckAsyncMethod(mtd, tgt);

                        var cont = _continuation.Pop();

                        var args = default(Expression[]);
                        var ps = default(ParameterExpression[]);
                        var call = default(Expression);
                        if (mce.Object != null)
                        {
                            args = new[] { mce.Object }.Concat(mce.Arguments).ToArray();
                            ps = args.Select((e, i) => Expression.Parameter(e.Type, CpsConstants.PREFIX + i)).ToArray();
                            call = _parent.MakeAsyncMethodCall(ps[0], tgt, ps.Skip(1).Cast<Expression>(), cont);
                        }
                        else
                        {
                            args = mce.Arguments.ToArray();
                            ps = args.Select((e, i) => Expression.Parameter(e.Type, CpsConstants.PREFIX + i)).ToArray();
                            call = _parent.MakeAsyncMethodCall(instance: null, tgt, ps.Cast<Expression>(), cont);
                        }

                        var ctn = default(TContinuationState);
                        var cur = call;

                        for (int i = args.Length - 1; i >= 0; i--)
                        {
                            var par = ps[i];

                            ctn = _parent.MakeContinuation(cur, par, cont);
                            _continuation.Push(ctn);

                            var arg = args[i];
                            cur = Visit(arg);
                        }

                        result = cur;
                        return true;
                    }
                }

                result = null;
                return false;
            }
        }
    }

    internal static class CpsConstants
    {
        public const string PREFIX = "<>__x";
    }
}
