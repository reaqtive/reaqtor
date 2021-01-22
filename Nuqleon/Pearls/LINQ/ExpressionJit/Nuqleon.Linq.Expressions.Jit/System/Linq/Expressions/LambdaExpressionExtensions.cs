// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Diagnostics;
using System.Linq.Expressions.Jit;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Provides a set of extension methods for advanced expression tree compilation support.
    /// </summary>
    public static class LambdaExpressionExtensions
    {
        /// <summary>
        /// Compiles the specified expression tree with the specified compilation options.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate represented by the expression tree.</typeparam>
        /// <param name="expression">The expression tree to compile.</param>
        /// <param name="options">Compilation options to influence compilation behavior.</param>
        /// <returns>Delegate that can be used to evaluate the expression tree.</returns>
        public static TDelegate Compile<TDelegate>(this Expression<TDelegate> expression, CompilationOptions options)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            expression = OptimizeIfEnabled(expression, options);

            if ((options & CompilationOptions.EnableJustInTimeCompilation) != 0)
            {
                return (TDelegate)(object)JitCompile(expression, options);
            }

            if ((options & CompilationOptions.PreferInterpretation) != 0)
            {
                return expression.Compile(preferInterpretation: true);
            }

            return expression.Compile();
        }

        /// <summary>
        /// Compiles the specified expression tree with the specified compilation options.
        /// </summary>
        /// <param name="expression">The expression tree to compile.</param>
        /// <param name="options">Compilation options to influence compilation behavior.</param>
        /// <returns>Delegate that can be used to evaluate the expression tree.</returns>
        public static Delegate Compile(this LambdaExpression expression, CompilationOptions options)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            expression = OptimizeIfEnabled(expression, options);

            if ((options & CompilationOptions.EnableJustInTimeCompilation) != 0)
            {
                return JitCompile(expression, options);
            }

            if ((options & CompilationOptions.PreferInterpretation) != 0)
            {
                return expression.Compile(preferInterpretation: true);
            }

            return expression.Compile();
        }

        /// <summary>
        /// Applies expression tree optimization if enabled in the compilation options.
        /// </summary>
        /// <typeparam name="TExpression">The type of the expression to rewrite.</typeparam>
        /// <param name="expression">The expression to rewrite.</param>
        /// <param name="options">The compilation options that were supplied to the Compile method.</param>
        /// <returns>The result of rewriting the expression.</returns>
        private static TExpression OptimizeIfEnabled<TExpression>(TExpression expression, CompilationOptions options)
            where TExpression : LambdaExpression
        {
            if ((options & CompilationOptions.Optimize) != 0)
            {
                //
                // NB: Expressions of type LamdbaExpression don't get rewritten to nodes
                //     of another kind, so the cast below should succeed.
                //
                // CONSIDER: Expose an advanced overload to Compile which provides fine-
                //           grained control over the optimizations to apply.
                //
                return (TExpression)Optimizer.Optimize(expression, Optimizations.All);
            }

            return expression;
        }

        /// <summary>
        /// Compiles the specified expression with support for Just In Time (JIT) compilation.
        /// </summary>
        /// <param name="expression">The lambda expression to compile.</param>
        /// <param name="options">Compilation options to influence compilation behavior.</param>
        /// <returns>Delegate that can be used to evaluate the expression tree.</returns>
        private static Delegate JitCompile(LambdaExpression expression, CompilationOptions options)
        {
            //
            // Check if we want compilation or interpretation.
            //
            var preferInterpretation = (options & CompilationOptions.PreferInterpretation) != 0;
            var tieredCompilation = (options & CompilationOptions.TieredCompilation) != 0;

            //
            // First, reduce the expression in order to get rid of nested lambda
            // expressions in positions where JIT compilation is not supported.
            //
            var reduced = Reducer.Reduce(expression);

            //
            // Create the root parameter of the top-level lambda, representing
            // the method table. Entries in the method table are retrieved to
            // obtain thunks for inner lambdas at runtime.
            //
            var methodTable = Expression.Parameter(typeof(MethodTable), "__mt");

            //
            // Analyze the expression tree to obtain information about scopes
            // which are used to build closures.
            //
            var analysis = Analyzer.Analyze(reduced, methodTable);

            //
            // Create the expression rewriter to prepare the expression tree for
            // JIT compilation support.
            //
            var thunkFactory =
                tieredCompilation ? ThunkFactory.TieredCompilation :
                preferInterpretation ? ThunkFactory.Interpreted : ThunkFactory.Compiled;
            var result = JitCompiler.Prepare(methodTable, analysis, reduced, thunkFactory);

            //
            // Build the top-level lambda which is parameterized by the method
            // table to access thunks for inner lambdas. This will return a
            // lambda expression whose return type is a delegate.
            //
            var lambda = Expression.Lambda(result.Expression, result.MethodTableParameter);
            Debug.Assert(lambda.Body.Type == expression.Type, "Expected compatible delegate type.");

            //
            // Compile the top-level lambda and invoke the resulting delegate with
            // the method table instance. This will prepare the inner lambda(s) for
            // later invocation. Effectively, we've curried the original expression
            // to take an explicit method table parameter, and we're applying the
            // outer lambda here.
            //
            var compiled = lambda.Compile(preferInterpretation);
            return (Delegate)compiled.DynamicInvoke(result.MethodTable);
        }
    }
}
