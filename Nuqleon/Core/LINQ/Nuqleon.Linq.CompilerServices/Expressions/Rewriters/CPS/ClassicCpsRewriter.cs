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
 * This file provides a simple CPS rewriter that's based on a single Action<T> based callback that will
 * receive the computed value of an asynchronous operation.
 * 
 * BD - 5/8/2013
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /*
     * R(x, cb) --> cb(x)
     * R(e(), cb) --> e'(cb)
     * R(f(e), cb) --> R(e, x => f'(x, cb))
     * R(g(e1, e2), cb) --> R(e1, x => R(e2, y => g'(x, y, cb)))
     */

    /// <summary>
    /// Expression tree rewriter that turns an expression into a Continuation Passing Style (CPS) expression using an Action&lt;T&gt; based callback method.
    /// </summary>
    /// <example>
    /// Examples are provided on the various methods, assuming the following methods are defined:
    /// <code>
    ///     [UseAsyncMethod]
    ///     int Add(int a, int b)
    ///     {
    ///         return a + b;
    ///     }
    ///     
    ///     void Add(int a, int b, Action&lt;int&gt; callback)
    ///     {
    ///         callback(a + b);
    ///     }
    /// </code>
    /// </example>
    public sealed class ClassicCpsRewriter : CpsRewriterBase<Expression>
    {
        #region Applicative forms (supplying continuation)

        #region Non-generic

        /// <summary>
        /// Rewrites the given expression to a CPS expression using the specified callback.
        /// </summary>
        /// <param name="expression">Expression to rewrite.</param>
        /// <param name="continuation">Continuation to pass.</param>
        /// <returns>Rewritten expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     Add(1, Add(Add(2, 3), 4))
        /// </code>
        /// into:
        /// <code>
        ///     Add(2, 3, x0 => Add(x0, 4, x1 => Add(1, x1, continuation)))
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     Add(Foo(1), Add(Add(2, 3), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     Invoke(x0 => Add(2, 3, x1 => Add(x1, 4, x2 => Add(x0, x2, continuation))), Foo(1))
        /// </code>
        /// </example>
        public Expression Rewrite(Expression expression, Expression continuation)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            return base.RewriteCore(expression, continuation);
        }

        #endregion

        #region Action

        /// <summary>
        /// Rewrites the given expression to a CPS expression using the specified callback.
        /// </summary>
        /// <param name="expression">Lambda expression whose body to rewrite.</param>
        /// <param name="continuation">Continuation to pass.</param>
        /// <returns>Rewritten expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     () => Bar(1, 2, 3)
        /// </code>
        /// into:
        /// <code>
        ///     Bar(continuation)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     () => Bar(Foo(1), 2, 3)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     Invoke(x0 => Bar(x0, 2, 3, continuation), Foo(1))
        /// </code>
        /// </example>
        public Expression Rewrite(Expression<Action> expression, Expression<Action> continuation)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            return base.RewriteCore(expression.Body, continuation);
        }

        #endregion

        #region Func

        /// <summary>
        /// Rewrites the given expression to a CPS expression using the specified callback.
        /// </summary>
        /// <typeparam name="TResult">Type of the result produced by the expression.</typeparam>
        /// <param name="expression">Lambda expression whose body to rewrite.</param>
        /// <param name="continuation">Continuation to pass.</param>
        /// <returns>Rewritten expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     () => Add(1, Add(Add(2, 3), 4))
        /// </code>
        /// into:
        /// <code>
        ///     Add(2, 3, x0 => Add(x0, 4, x1 => Add(1, x1, continuation)))
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     () => Add(Foo(1), Add(Add(2, 3), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     Invoke(x0 => Add(2, 3, x1 => Add(x1, 4, x2 => Add(x0, x2, continuation))), Foo(1))
        /// </code>
        /// </example>
        public Expression Rewrite<TResult>(Expression<Func<TResult>> expression, Expression<Action<TResult>> continuation)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            return base.RewriteCore(expression.Body, continuation);
        }

        #endregion

        #endregion

        #region Functional forms (conversions)

        #region Non-generic

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     Add(1, Add(Add(2, 3), 4))
        /// </code>
        /// into:
        /// <code>
        ///     callback => Add(2, 3, x0 => Add(x0, 4, x1 => Add(1, x1, callback)))
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     Add(Foo(1), Add(Add(2, 3), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     callback => Invoke(x0 => Add(2, 3, x1 => Add(x1, 4, x2 => Add(x0, x2, callback))), Foo(1))
        /// </code>
        /// </example>
        public LambdaExpression Rewrite(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.Type == typeof(void))
            {
                var p = Expression.Parameter(typeof(Action), "callback");
                var b = base.RewriteCore(expression, p);
                return Expression.Lambda(typeof(Action<Action>), b, p);
            }
            else
            {
                var p = Expression.Parameter(typeof(Action<>).MakeGenericType(expression.Type), "callback");
                var b = base.RewriteCore(expression, p);
                return Expression.Lambda(typeof(Action<>).MakeGenericType(p.Type), b, p);
            }
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     (a, b, c, d) => Add(a, Add(Add(b, c), d))
        /// </code>
        /// into:
        /// <code>
        ///     (a, b, c, d, callback) => Add(b, c, x0 => Add(x0, d, x1 => Add(a, x1, callback)))
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (a, b, c, d) => Add(Foo(a), Add(Add(b, c), d))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (a, b, c, d, callback) => Invoke(x0 => Add(b, c, x1 => Add(x1, d, x2 => Add(x0, x2, callback))), Foo(a))
        /// </code>
        /// </example>
        public LambdaExpression Rewrite(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.Body.Type == typeof(void))
            {
                var p = Expression.Parameter(typeof(Action), "callback");
                var b = base.RewriteCore(expression.Body, p);
                var f = Expression.GetActionType(expression.Parameters.Select(x => x.Type).Concat(new[] { typeof(Action) }).ToArray());
                return Expression.Lambda(f, b, expression.Parameters.Concat(new[] { p }));
            }
            else
            {
                var p = Expression.Parameter(typeof(Action<>).MakeGenericType(expression.Body.Type), "callback");
                var b = base.RewriteCore(expression.Body, p);
                var f = Expression.GetActionType(expression.Parameters.Select(x => x.Type).Concat(new[] { p.Type }).ToArray());
                return Expression.Lambda(f, b, expression.Parameters.Concat(new[] { p }));
            }
        }

        #endregion

        #region Action

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     () => Bar(1, 2, 3)
        /// </code>
        /// into:
        /// <code>
        ///     callback => Bar(1, 2, 3, callback)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     () => Bar(Foo(1), 2, 3)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     callback => Invoke(x0 => Bar(x0, 2, 3, callback), Foo(1))
        /// </code>
        /// </example>
        public Expression<Action<Action>> Rewrite(Expression<Action> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var p = Expression.Parameter(typeof(Action), "callback");
            var b = base.RewriteCore(expression.Body, p);
            return Expression.Lambda<Action<Action>>(b, p);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <typeparam name="T">Type of the parameter passed to the computation.</typeparam>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     x => Bar(x, 2, 3)
        /// </code>
        /// into:
        /// <code>
        ///     (x, callback) => Bar(x, 2, 3, callback)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     x => Bar(Foo(x), 2, 3)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, callback) => Invoke(x0 => Bar(x0, 2, 3, callback), Foo(x))
        /// </code>
        /// </example>
        public Expression<Action<T, Action>> Rewrite<T>(Expression<Action<T>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var p = Expression.Parameter(typeof(Action), "callback");
            var b = base.RewriteCore(expression.Body, p);
            return Expression.Lambda<Action<T, Action>>(b, expression.Parameters[0], p);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter passed to the computation.</typeparam>
        /// <typeparam name="T2">Type of the second parameter passed to the computation.</typeparam>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     (x, y) => Bar(x, y, 3)
        /// </code>
        /// into:
        /// <code>
        ///     (x, y, callback) => Bar(x, y, 3, callback)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (x, y) => Bar(Foo(x), y, 3)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, y, callback) => Invoke(x0 => Bar(x0, y, 3, callback), Foo(x))
        /// </code>
        /// </example>
        public Expression<Action<T1, T2, Action>> Rewrite<T1, T2>(Expression<Action<T1, T2>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var p = Expression.Parameter(typeof(Action), "callback");
            var b = base.RewriteCore(expression.Body, p);
            return Expression.Lambda<Action<T1, T2, Action>>(b, expression.Parameters[0], expression.Parameters[1], p);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter passed to the computation.</typeparam>
        /// <typeparam name="T2">Type of the second parameter passed to the computation.</typeparam>
        /// <typeparam name="T3">Type of the third parameter passed to the computation.</typeparam>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     (x, y, z) => Bar(x, y, z)
        /// </code>
        /// into:
        /// <code>
        ///     (x, y, z, callback) => Bar(x, y, z, callback)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (x, y, z) => Bar(Foo(x), y, z)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, y, z, callback) => Invoke(x0 => Bar(x0, y, z, callback), Foo(x))
        /// </code>
        /// </example>
        public Expression<Action<T1, T2, T3, Action>> Rewrite<T1, T2, T3>(Expression<Action<T1, T2, T3>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var p = Expression.Parameter(typeof(Action), "callback");
            var b = base.RewriteCore(expression.Body, p);
            return Expression.Lambda<Action<T1, T2, T3, Action>>(b, expression.Parameters[0], expression.Parameters[1], expression.Parameters[2], p);
        }

        #endregion

        #region Func

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <typeparam name="TResult">Type of the result produced by the expression.</typeparam>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     () => Add(1, Add(Add(2, 3), 4))
        /// </code>
        /// into:
        /// <code>
        ///     callback => Add(2, 3, x0 => Add(x0, 4, x1 => Add(1, x1, callback)))
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     () => Add(Foo(1), Add(Add(2, 3), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     callback => Invoke(x0 => Add(2, 3, x1 => Add(x1, 4, x2 => Add(x0, x2, callback))), Foo(1))
        /// </code>
        /// </example>
        public Expression<Action<Action<TResult>>> Rewrite<TResult>(Expression<Func<TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var p = Expression.Parameter(typeof(Action<TResult>), "callback");
            var b = base.RewriteCore(expression.Body, p);
            return Expression.Lambda<Action<Action<TResult>>>(b, p);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <typeparam name="T">Type of the parameter passed to the computation.</typeparam>
        /// <typeparam name="TResult">Type of the result produced by the expression.</typeparam>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     x => Add(1, Add(Add(2, x), 4))
        /// </code>
        /// into:
        /// <code>
        ///     (x, callback) => Add(2, x, x0 => Add(x0, 4, x1 => Add(1, x1, callback)))
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     x => Add(Foo(1), Add(Add(2, x), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, callback) => Invoke(x0 => Add(2, x, x1 => Add(x1, 4, x2 => Add(x0, x2, callback))), Foo(1))
        /// </code>
        /// </example>
        public Expression<Action<T, Action<TResult>>> Rewrite<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var p = Expression.Parameter(typeof(Action<TResult>), "callback");
            var b = base.RewriteCore(expression.Body, p);
            return Expression.Lambda<Action<T, Action<TResult>>>(b, expression.Parameters[0], p);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter passed to the computation.</typeparam>
        /// <typeparam name="T2">Type of the second parameter passed to the computation.</typeparam>
        /// <typeparam name="TResult">Type of the result produced by the expression.</typeparam>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     (x, y) => Add(1, Add(Add(2, x), y))
        /// </code>
        /// into:
        /// <code>
        ///     (x, y, callback) => Add(2, x, x0 => Add(x0, y, x1 => Add(1, x1, callback)))
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (x, y) => Add(Foo(1), Add(Add(2, x), y))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, y, callback) => Invoke(x0 => Add(2, x, x1 => Add(x1, y, x2 => Add(x0, x2, callback))), Foo(1))
        /// </code>
        /// </example>
        public Expression<Action<T1, T2, Action<TResult>>> Rewrite<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var p = Expression.Parameter(typeof(Action<TResult>), "callback");
            var b = base.RewriteCore(expression.Body, p);
            return Expression.Lambda<Action<T1, T2, Action<TResult>>>(b, expression.Parameters[0], expression.Parameters[1], p);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts a callback and executes the expression using CPS.
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter passed to the computation.</typeparam>
        /// <typeparam name="T2">Type of the second parameter passed to the computation.</typeparam>
        /// <typeparam name="T3">Type of the third parameter passed to the computation.</typeparam>
        /// <typeparam name="TResult">Type of the result produced by the expression.</typeparam>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     (x, y, z) => Add(x, Add(Add(2, y), z))
        /// </code>
        /// into:
        /// <code>
        ///     (x, y, z, callback) => Add(2, y, x0 => Add(x0, z, x1 => Add(x, x1, callback)))
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (x, y, z) => Add(Foo(x), Add(Add(2, y), z))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, y, z, callback) => Invoke(x0 => Add(2, y, x1 => Add(x1, z, x2 => Add(x0, x2, callback))), Foo(x))
        /// </code>
        /// </example>
        public Expression<Action<T1, T2, T3, Action<TResult>>> Rewrite<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var p = Expression.Parameter(typeof(Action<TResult>), "callback");
            var b = base.RewriteCore(expression.Body, p);
            return Expression.Lambda<Action<T1, T2, T3, Action<TResult>>>(b, expression.Parameters[0], expression.Parameters[1], expression.Parameters[2], p);
        }

        #endregion

        #endregion

        /// <summary>
        /// Gets the continuation parameter types that are used during method overload resolution by appending those to the parameters type of the synchronous method.
        /// </summary>
        /// <param name="method">Method to find continuation parameters for, e.g. by inspecting its return type and creating the corresponding callback parameter type.</param>
        /// <returns>Continuation parameter types to check for when searching for an asynchronous method overload.</returns>
        protected override IEnumerable<Type> GetContinuationParameters(MethodInfo method)
        {
            Debug.Assert(method != null);

            return new[] {
                method.ReturnType == typeof(void) ? typeof(Action) : typeof(Action<>).MakeGenericType(method.ReturnType)
            };
        }

        /// <summary>
        /// Creates an asynchronous method call with the given continuation.
        /// </summary>
        /// <param name="instance">Instance of the method call.</param>
        /// <param name="method">Method to call.</param>
        /// <param name="arguments">Arguments of the method call.</param>
        /// <param name="continuation">Continuation to supply to the asynchronous method.</param>
        /// <returns>Expression representing an asynchronous method call with the given continuation.</returns>
        protected override Expression MakeAsyncMethodCall(Expression instance, MethodInfo method, IEnumerable<Expression> arguments, Expression continuation)
        {
            Debug.Assert(method != null);

            return Expression.Call(instance, method, arguments.Concat(new[] { continuation }));
        }

        /// <summary>
        /// Creates a new continuation with the given result parameter to run the specified remainder computation.
        /// </summary>
        /// <param name="remainder">Remainder of the expression to evaluate upon invocation of the continuation.</param>
        /// <param name="resultParameter">Parameter with the result of the expression evaluation preceding the invocation of the continuation.</param>
        /// <param name="currentContinuation">Current continuation being processed upon requesting a new continuation.</param>
        /// <returns>Continuation object that will execute the remainder computation, given the specified result parameter.</returns>
        protected override Expression MakeContinuation(Expression remainder, ParameterExpression resultParameter, Expression currentContinuation) => Expression.Lambda(remainder, resultParameter);

        /// <summary>
        /// Creates an invocation of the given continuation.
        /// </summary>
        /// <param name="continuation">Continuation to invoke.</param>
        /// <param name="argument">Argument to pass to the continuation's invocation.</param>
        /// <returns>Expression representing the invocation of the continuation.</returns>
        protected override Expression InvokeContinuation(Expression continuation, Expression argument) => Expression.Invoke(continuation, argument);
    }
}
