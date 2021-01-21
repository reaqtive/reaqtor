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
 * This file provides a CPS rewriter that provides both an Action<T> and an Action<Exception> callback
 * in order to propagate computed values and possible errors from an asynchronous operation.
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
     * R(x, cb, err) --> Eval(() => x, cb, err)
     * R(e(), cb, err) --> e'(cb, err)
     * R(f(e), cb, err) --> R(e, x => f'(x, cb, err), err)
     * R(g(e1, e2), cb, err) --> R(e1, x => R(e2, y => g'(x, y, cb, err)), err)
     */

    /// <summary>
    /// Expression tree rewriter that turns an expression into a Continuation Passing Style (CPS) expression using an Action&lt;T&gt; based callback method and an Action&lt;Exception&gt; callback for error propagation.
    /// </summary>
    /// <example>
    /// Examples are provided on the various methods, assuming the following methods are defined:
    /// <code>
    ///     [UseAsyncMethod]
    ///     int Div(int a, int b)
    ///     {
    ///         return a / b;
    ///     }
    ///
    ///     void Div(int a, int b, Action&lt;int&gt; success, Action&lt;Exception&gt; error)
    ///     {
    ///         if (b == 0)
    ///             error(new DivisionByZeroException());
    ///
    ///         success(a / b);
    ///     }
    /// </code>
    /// </example>
    public sealed class ClassicCpsRewriterWithErrorPropagation : CpsRewriterBase<SuccessErrorContinuationPair>
    {
        #region Applicative forms (supplying continuation)

        #region Non-generic

        /// <summary>
        /// Rewrites the given expression to a CPS expression using the specified callbacks.
        /// </summary>
        /// <param name="expression">Expression to rewrite.</param>
        /// <param name="success">Continuation to call upon success.</param>
        /// <param name="failure">Continuation to call upon failure.</param>
        /// <returns>Rewritten expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     Div(1, Div(Div(2, 3), 4))
        /// </code>
        /// into:
        /// <code>
        ///     Div(2, 3, x0 => Div(x0, 4, x1 => Div(1, x1, success, failure), failure), failure)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     Div(Foo(1), Div(Div(2, 3), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     Invoke(x0 => Div(2, 3, x1 => Div(x1, 4, x2 => Div(x0, x2, callback, failure), failure), failure), Foo(1))
        /// </code>
        /// </example>
        public Expression Rewrite(Expression expression, Expression success, Expression failure)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (success == null)
                throw new ArgumentNullException(nameof(success));
            if (failure == null)
                throw new ArgumentNullException(nameof(failure));

            return base.RewriteCore(expression, new SuccessErrorContinuationPair { SuccessContinuation = success, ErrorContinuation = failure });
        }

        #endregion

        #region Action

        /// <summary>
        /// Rewrites the given expression to a CPS expression using the specified callbacks.
        /// </summary>
        /// <param name="expression">Lambda expression whose body to rewrite.</param>
        /// <param name="success">Continuation to call upon success.</param>
        /// <param name="failure">Continuation to call upon failure.</param>
        /// <returns>Rewritten expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     () => Bar(1, 2, 3)
        /// </code>
        /// into:
        /// <code>
        ///     Bar(success, failure)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     () => Bar(Foo(1), 2, 3)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     Invoke(x0 => Bar(x0, 2, 3, success, failure), Foo(1))
        /// </code>
        /// </example>
        public Expression Rewrite(Expression<Action> expression, Expression<Action> success, Expression<Action<Exception>> failure)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (success == null)
                throw new ArgumentNullException(nameof(success));
            if (failure == null)
                throw new ArgumentNullException(nameof(failure));

            return base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = success, ErrorContinuation = failure });
        }

        #endregion

        #region Func

        /// <summary>
        /// Rewrites the given expression to a CPS expression using the specified callbacks.
        /// </summary>
        /// <typeparam name="TResult">Type of the result produced by the expression.</typeparam>
        /// <param name="expression">Lambda expression whose body to rewrite.</param>
        /// <param name="success">Continuation to call upon success.</param>
        /// <param name="failure">Continuation to call upon failure.</param>
        /// <returns>Rewritten expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     () => Div(1, Div(Div(2, 3), 4))
        /// </code>
        /// into:
        /// <code>
        ///     Div(2, 3, x0 => Div(x0, 4, x1 => Div(1, x1, success, failure), failure), failure)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     () => Div(Foo(1), Div(Div(2, 3), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     Invoke(x0 => Div(2, 3, x1 => Div(x1, 4, x2 => Div(x0, x2, success, failure), failure), failure), Foo(1))
        /// </code>
        /// </example>
        public Expression Rewrite<TResult>(Expression<Func<TResult>> expression, Expression<Action<TResult>> success, Expression<Action<Exception>> failure)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (success == null)
                throw new ArgumentNullException(nameof(success));
            if (failure == null)
                throw new ArgumentNullException(nameof(failure));

            return base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = success, ErrorContinuation = failure });
        }

        #endregion

        #endregion

        #region Functional forms (conversions)

        #region Non-generic

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
        /// </summary>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a success callback and a failure callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     Div(1, Div(Div(2, 3), 4))
        /// </code>
        /// into:
        /// <code>
        ///     (success, error) => Div(2, 3, x0 => Div(x0, 4, x1 => Div(1, x1, success, error), error), error)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     Div(Foo(1), Div(Div(2, 3), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (success, error) => Invoke(x0 => Div(2, 3, x1 => Div(x1, 4, x2 => Div(x0, x2, success, error), error), error), Foo(1))
        /// </code>
        /// </example>
        public LambdaExpression Rewrite(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.Type == typeof(void))
            {
                var s = Expression.Parameter(typeof(Action), "success");
                var e = Expression.Parameter(typeof(Action<Exception>), "failure");
                var b = base.RewriteCore(expression, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
                return Expression.Lambda(typeof(Action<,>).MakeGenericType(s.Type, e.Type), b, s, e);
            }
            else
            {
                var s = Expression.Parameter(typeof(Action<>).MakeGenericType(expression.Type), "success");
                var e = Expression.Parameter(typeof(Action<Exception>), "failure");
                var b = base.RewriteCore(expression, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
                return Expression.Lambda(typeof(Action<,>).MakeGenericType(s.Type, e.Type), b, s, e);
            }
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
        /// </summary>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a success callback and a failure callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     (a, b, c, d) => Div(a, Div(Div(b, c), d))
        /// </code>
        /// into:
        /// <code>
        ///     (a, b, c, d, success, error) => Div(b, c, x0 => Div(x0, d, x1 => Div(a, x1, success, error), error), error)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (a, b, c, d) => Div(Foo(a), Div(Div(b, c), d))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (a, b, c, d, success, error) => Invoke(x0 => Div(b, c, x1 => Div(x1, d, x2 => Div(x0, x2, success, error), error), error), Foo(a))
        /// </code>
        /// </example>
        public LambdaExpression Rewrite(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.Body.Type == typeof(void))
            {
                var s = Expression.Parameter(typeof(Action), "success");
                var e = Expression.Parameter(typeof(Action<Exception>), "failure");
                var f = Expression.GetActionType(expression.Parameters.Select(x => x.Type).Concat(new[] { s.Type, e.Type }).ToArray());
                var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
                return Expression.Lambda(f, b, expression.Parameters.Concat(new[] { s, e }).ToArray());
            }
            else
            {
                var s = Expression.Parameter(typeof(Action<>).MakeGenericType(expression.Body.Type), "success");
                var e = Expression.Parameter(typeof(Action<Exception>), "failure");
                var f = Expression.GetActionType(expression.Parameters.Select(x => x.Type).Concat(new[] { s.Type, e.Type }).ToArray());
                var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
                return Expression.Lambda(f, b, expression.Parameters.Concat(new[] { s, e }).ToArray());
            }
        }

        #endregion

        #region Action

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
        /// </summary>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a success callback and a failure callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     () => Bar(1, 2, 3)
        /// </code>
        /// into:
        /// <code>
        ///     (success, error) => Bar(1, 2, 3, success, error)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     () => Bar(Foo(1), 2, 3)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (success, error) => Invoke(x0 => Bar(x0, 2, 3, callback, error), Foo(1))
        /// </code>
        /// </example>
        public Expression<Action<Action, Action<Exception>>> Rewrite(Expression<Action> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var s = Expression.Parameter(typeof(Action), "success");
            var e = Expression.Parameter(typeof(Action<Exception>), "failure");
            var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
            return Expression.Lambda<Action<Action, Action<Exception>>>(b, s, e);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
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
        ///     (x, success, failure) => Bar(x, 2, 3, success, failure)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     x => Bar(Foo(x), 2, 3)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, success, failure) => Invoke(x0 => Bar(x0, 2, 3, success, failure), Foo(x))
        /// </code>
        /// </example>
        public Expression<Action<T, Action, Action<Exception>>> Rewrite<T>(Expression<Action<T>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var s = Expression.Parameter(typeof(Action), "success");
            var e = Expression.Parameter(typeof(Action<Exception>), "failure");
            var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
            return Expression.Lambda<Action<T, Action, Action<Exception>>>(b, expression.Parameters[0], s, e);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
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
        ///     (x, y, success, failure) => Bar(x, y, 3, success, failure)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (x, y) => Bar(Foo(x), y, 3)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, y, success, failure) => Invoke(x0 => Bar(x0, y, 3, success, failure), Foo(x))
        /// </code>
        /// </example>
        public Expression<Action<T1, T2, Action, Action<Exception>>> Rewrite<T1, T2>(Expression<Action<T1, T2>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var s = Expression.Parameter(typeof(Action), "success");
            var e = Expression.Parameter(typeof(Action<Exception>), "failure");
            var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
            return Expression.Lambda<Action<T1, T2, Action, Action<Exception>>>(b, expression.Parameters[0], expression.Parameters[1], s, e);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
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
        ///     (x, y, z, success, failure) => Bar(x, y, z, success, failure)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (x, y, z) => Bar(Foo(x), y, z)
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, y, z, success, failure) => Invoke(x0 => Bar(x0, y, z, success, failure), Foo(x))
        /// </code>
        /// </example>
        public Expression<Action<T1, T2, T3, Action, Action<Exception>>> Rewrite<T1, T2, T3>(Expression<Action<T1, T2, T3>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var s = Expression.Parameter(typeof(Action), "success");
            var e = Expression.Parameter(typeof(Action<Exception>), "failure");
            var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
            return Expression.Lambda<Action<T1, T2, T3, Action, Action<Exception>>>(b, expression.Parameters[0], expression.Parameters[1], expression.Parameters[2], s, e);
        }

        #endregion

        #region Func

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
        /// </summary>
        /// <typeparam name="TResult">Type of the result produced by the expression.</typeparam>
        /// <param name="expression">Expression to rewrite.</param>
        /// <returns>Lambda expression that, given a success callback and a failure callback, will execute the expression using CPS.</returns>
        /// <example>
        /// This method will rewrite the following expression:
        /// <code>
        ///     () => Div(1, Div(Div(2, 3), 4))
        /// </code>
        /// into:
        /// <code>
        ///     (success, error) => Div(2, 3, x0 => Div(x0, 4, x1 => Div(1, x1, success, error), error), error)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     () => Div(Foo(1), Div(Div(2, 3), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (success, error) => Invoke(x0 => Div(2, 3, x1 => Div(x1, 4, x2 => Div(x0, x2, callback, error), error), error), Foo(1))
        /// </code>
        /// </example>
        public Expression<Action<Action<TResult>, Action<Exception>>> Rewrite<TResult>(Expression<Func<TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var s = Expression.Parameter(typeof(Action<TResult>), "success");
            var e = Expression.Parameter(typeof(Action<Exception>), "failure");
            var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
            return Expression.Lambda<Action<Action<TResult>, Action<Exception>>>(b, s, e);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
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
        ///     (x, success, failure) => Add(2, x, x0 => Add(x0, 4, x1 => Add(1, x1, success, failure), failure), failure)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     x => Add(Foo(1), Add(Add(2, x), 4))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, success, failure) => Invoke(x0 => Add(2, x, x1 => Add(x1, 4, x2 => Add(x0, x2, success, failure), failure), failure), Foo(1))
        /// </code>
        /// </example>
        public Expression<Action<T, Action<TResult>, Action<Exception>>> Rewrite<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var s = Expression.Parameter(typeof(Action<TResult>), "success");
            var e = Expression.Parameter(typeof(Action<Exception>), "failure");
            var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
            return Expression.Lambda<Action<T, Action<TResult>, Action<Exception>>>(b, expression.Parameters[0], s, e);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
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
        ///     (x, y, success, failure) => Add(2, x, x0 => Add(x0, y, x1 => Add(1, x1, success, failure), failure), failure)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (x, y) => Add(Foo(1), Add(Add(2, x), y))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, y, success, failure) => Invoke(x0 => Add(2, x, x1 => Add(x1, y, x2 => Add(x0, x2, success, failure), failure), failure), Foo(1))
        /// </code>
        /// </example>
        public Expression<Action<T1, T2, Action<TResult>, Action<Exception>>> Rewrite<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var s = Expression.Parameter(typeof(Action<TResult>), "success");
            var e = Expression.Parameter(typeof(Action<Exception>), "failure");
            var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
            return Expression.Lambda<Action<T1, T2, Action<TResult>, Action<Exception>>>(b, expression.Parameters[0], expression.Parameters[1], s, e);
        }

        /// <summary>
        /// Rewrites the given expression to a lambda expression that accepts success and failure callbacks and executes the expression using CPS.
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
        ///     (x, y, z, success, failure) => Add(2, y, x0 => Add(x0, z, x1 => Add(x, x1, success, failure), failure), failure)
        /// </code>
        /// Evaluation order of side-effects is preserved. The following expression:
        /// <code>
        ///     (x, y, z) => Add(Foo(x), Add(Add(2, y), z))
        /// </code>
        /// will be rewritten into:
        /// <code>
        ///     (x, y, z, success, failure) => Invoke(x0 => Add(2, y, x1 => Add(x1, z, x2 => Add(x0, x2, success, failure), failure), failure), Foo(x))
        /// </code>
        /// </example>
        public Expression<Action<T1, T2, T3, Action<TResult>, Action<Exception>>> Rewrite<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var s = Expression.Parameter(typeof(Action<TResult>), "success");
            var e = Expression.Parameter(typeof(Action<Exception>), "failure");
            var b = base.RewriteCore(expression.Body, new SuccessErrorContinuationPair { SuccessContinuation = s, ErrorContinuation = e });
            return Expression.Lambda<Action<T1, T2, T3, Action<TResult>, Action<Exception>>>(b, expression.Parameters[0], expression.Parameters[1], expression.Parameters[2], s, e);
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

            return method.ReturnType == typeof(void)
                ? new[] { typeof(Action), typeof(Action<Exception>) }
                : new[] { typeof(Action<>).MakeGenericType(method.ReturnType), typeof(Action<Exception>) };
        }

        /// <summary>
        /// Creates an asynchronous method call with the given continuation.
        /// </summary>
        /// <param name="instance">Instance of the method call.</param>
        /// <param name="method">Method to call.</param>
        /// <param name="arguments">Arguments of the method call.</param>
        /// <param name="continuation">Continuation to supply to the asynchronous method.</param>
        /// <returns>Expression representing an asynchronous method call with the given continuation.</returns>
        protected override Expression MakeAsyncMethodCall(Expression instance, MethodInfo method, IEnumerable<Expression> arguments, SuccessErrorContinuationPair continuation)
        {
            Debug.Assert(method != null);
            Debug.Assert(continuation != null);

            return Expression.Call(instance, method, arguments.Concat(new[] { continuation.SuccessContinuation, continuation.ErrorContinuation }));
        }

        /// <summary>
        /// Creates a new continuation with the given result parameter to run the specified remainder computation.
        /// </summary>
        /// <param name="remainder">Remainder of the expression to evaluate upon invocation of the continuation.</param>
        /// <param name="resultParameter">Parameter with the result of the expression evaluation preceding the invocation of the continuation.</param>
        /// <param name="currentContinuation">Current continuation being processed upon requesting a new continuation.</param>
        /// <returns>Continuation object that will execute the remainder computation, given the specified result parameter.</returns>
        protected override SuccessErrorContinuationPair MakeContinuation(Expression remainder, ParameterExpression resultParameter, SuccessErrorContinuationPair currentContinuation)
        {
            Debug.Assert(currentContinuation != null);

            return new SuccessErrorContinuationPair
            {
                SuccessContinuation = Expression.Lambda(remainder, resultParameter),
                ErrorContinuation = currentContinuation.ErrorContinuation
            };
        }

        /// <summary>
        /// Creates an invocation of the given continuation.
        /// </summary>
        /// <param name="continuation">Continuation to invoke.</param>
        /// <param name="argument">Argument to pass to the continuation's invocation.</param>
        /// <returns>Expression representing the invocation of the continuation.</returns>
        protected override Expression InvokeContinuation(SuccessErrorContinuationPair continuation, Expression argument)
        {
            Debug.Assert(continuation != null);

            if (argument.NodeType is ExpressionType.Constant or ExpressionType.Parameter or ExpressionType.Default)
            {
                return Expression.Invoke(continuation.SuccessContinuation, argument);
            }

            if (argument.NodeType == ExpressionType.Throw)
            {
                var ex = ((UnaryExpression)argument).Operand;
                return Expression.Invoke(continuation.ErrorContinuation, ex);
            }

            return Expression.Call(
                s_evalFunc.Value.MakeGenericMethod(argument.Type),
                Expression.Lambda(typeof(Func<>).MakeGenericType(argument.Type), argument),
                continuation.SuccessContinuation,
                continuation.ErrorContinuation
            );
        }

        private static readonly Lazy<MethodInfo> s_evalFunc = new(() => ((MethodInfo)ReflectionHelpers.InfoOf(() => Eval<object>(default, default, default))).GetGenericMethodDefinition());

        private static void Eval<R>(Func<R> f, Action<R> success, Action<Exception> fail)
        {
            R res;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Catch more specific exception type. (By design.)

            try
            {
                res = f();
            }
            catch (Exception ex)
            {
                fail(ex);
                return;
            }

#pragma warning restore CA1031
#pragma warning restore IDE0079

            success(res);
        }
    }

    /// <summary>
    /// (Infrastructure) Representation of a pair of success and error continuations.
    /// </summary>
    public sealed class SuccessErrorContinuationPair
    {
        /// <summary>
        /// Gets the expression representing the success continuation.
        /// </summary>
        public Expression SuccessContinuation { get; internal set; }

        /// <summary>
        /// Gets the expression representing the error continuation.
        /// </summary>
        public Expression ErrorContinuation { get; internal set; }
    }
}
