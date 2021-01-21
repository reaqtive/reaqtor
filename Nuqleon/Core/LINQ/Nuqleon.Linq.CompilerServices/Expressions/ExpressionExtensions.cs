// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Provides a set of extension methods for System.Linq.Expressions.Expression.
    /// </summary>
    public static class ExpressionExtensions
    {
        #region Evaluate

        /// <summary>
        /// Evaluates the specified expression tree.
        /// If the specified expression has unbound parameters, an exception will be thrown.
        /// </summary>
        /// <typeparam name="TResult">Type of the evaluation result.</typeparam>
        /// <param name="expression">Expression to evaluate.</param>
        /// <returns>Result of evaluating the expression tree.</returns>
        public static TResult Evaluate<TResult>(this Expression<Func<TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return EvalImpl(expression, cache: null);
        }

        /// <summary>
        /// Evaluates the specified expression tree.
        /// If the specified expression has unbound parameters, an exception will be thrown.
        /// </summary>
        /// <typeparam name="TResult">Type of the evaluation result.</typeparam>
        /// <param name="expression">Expression to evaluate.</param>
        /// <returns>Result of evaluating the expression tree.</returns>
        public static TResult Evaluate<TResult>(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return EvalImpl<TResult>(expression, cache: null);
        }

        /// <summary>
        /// Evaluates the specified expression tree.
        /// If the specified expression has unbound parameters, an exception will be thrown.
        /// </summary>
        /// <param name="expression">Expression to evaluate.</param>
        /// <returns>Result of evaluating the expression tree.</returns>
        public static object Evaluate(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return EvalImpl<object>(expression, cache: null);
        }

        /// <summary>
        /// Evaluates the specified expression tree.
        /// If the specified expression has unbound parameters, an exception will be thrown.
        /// </summary>
        /// <typeparam name="TResult">Type of the evaluation result.</typeparam>
        /// <param name="expression">Expression to evaluate.</param>
        /// <param name="cache">Compiled delegate cache.</param>
        /// <returns>Result of evaluating the expression tree.</returns>
        public static TResult Evaluate<TResult>(this Expression expression, ICompiledDelegateCache cache)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return EvalImpl<TResult>(expression, cache);
        }

        /// <summary>
        /// Evaluates the specified expression tree.
        /// If the specified expression has unbound parameters, an exception will be thrown.
        /// </summary>
        /// <typeparam name="TResult">Type of the evaluation result.</typeparam>
        /// <param name="expression">Expression to evaluate.</param>
        /// <param name="cache">Compiled delegate cache.</param>
        /// <returns>Result of evaluating the expression tree.</returns>
        public static TResult Evaluate<TResult>(this Expression<Func<TResult>> expression, ICompiledDelegateCache cache)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return EvalImpl(expression, cache);
        }

        /// <summary>
        /// Evaluates the specified expression tree.
        /// If the specified expression has unbound parameters, an exception will be thrown.
        /// </summary>
        /// <param name="expression">Expression to evaluate.</param>
        /// <param name="cache">Compiled delegate cache.</param>
        /// <returns>Result of evaluating the expression tree.</returns>
        public static object Evaluate(this Expression expression, ICompiledDelegateCache cache)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return EvalImpl<object>(expression, cache);
        }

        #endregion

        #region Funcletize

        /// <summary>
        /// Creates a funclet expression for the specified expression tree. This funclet can be used to evaluate the expression tree.
        /// If the specified expression has unbound parameters, an exception will be thrown.
        /// </summary>
        /// <typeparam name="TResult">Result type of the expression.</typeparam>
        /// <param name="expression">Expression to funcletize.</param>
        /// <returns>Funclet to evaluate the expression tree.</returns>
        public static Expression<Func<TResult>> Funcletize<TResult>(this Expression<Func<TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return FuncletizeImpl(expression);
        }

        /// <summary>
        /// Creates a funclet expression for the specified expression tree. This funclet can be used to evaluate the expression tree.
        /// If the specified expression has unbound parameters, an exception will be thrown.
        /// </summary>
        /// <typeparam name="TResult">Result type of the expression.</typeparam>
        /// <param name="expression">Expression to funcletize.</param>
        /// <returns>Funclet to evaluate the expression tree.</returns>
        public static Expression<Func<TResult>> Funcletize<TResult>(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return FuncletizeImpl<TResult>(expression);
        }

        /// <summary>
        /// Creates a funclet expression for the specified expression tree. This funclet can be used to evaluate the expression tree.
        /// If the specified expression has unbound parameters, an exception will be thrown.
        /// </summary>
        /// <param name="expression">Expression to funcletize.</param>
        /// <returns>Funclet to evaluate the expression tree.</returns>
        public static Expression<Func<object>> Funcletize(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return FuncletizeImpl<object>(expression);
        }

        #endregion

        #region ToExpressionTree

        /// <summary>
        /// Converts an expression tree to an ITree representation that can be used with rewriter tools.
        /// </summary>
        /// <param name="expression">Expression tree to convert.</param>
        /// <returns>ITree representation of the expression tree.</returns>
        public static ExpressionTree ToExpressionTree(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var visitor = new ExpressionTreeConversion();
            return visitor.Visit(expression);
        }

        /// <summary>
        /// Converts an element initializer to an ITree representation that can be used with rewriter tools.
        /// </summary>
        /// <param name="elementInitializer">Element initializer to convert.</param>
        /// <returns>ITree representation of the element initializer.</returns>
        public static ElementInitExpressionTree ToExpressionTree(this ElementInit elementInitializer)
        {
            if (elementInitializer == null)
                throw new ArgumentNullException(nameof(elementInitializer));

            var visitor = new ExpressionTreeConversion();
            return visitor.Visit(elementInitializer);
        }

        /// <summary>
        /// Converts a member binding to an ITree representation that can be used with rewriter tools.
        /// </summary>
        /// <param name="memberBinding">Member binding to convert.</param>
        /// <returns>ITree representation of the member binding.</returns>
        public static MemberBindingExpressionTree ToExpressionTree(this MemberBinding memberBinding)
        {
            if (memberBinding == null)
                throw new ArgumentNullException(nameof(memberBinding));

            var visitor = new ExpressionTreeConversion();
            return visitor.Visit(memberBinding);
        }

        #endregion

        #region ToCSharpString

        /// <summary>
        /// Gets a string representation of the specified expression tree using C# syntax.
        /// The resulting string is not guaranteed to be semantically equivalent and should be used for diagnostic purposes only.
        /// </summary>
        /// <param name="expression">Expression to provide a C# code fragment for.</param>
        /// <returns>String representation of the specified expression tree using C# syntax.</returns>
        public static string ToCSharpString(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return ToCSharpStringImpl(expression, allowCompilerGeneratedNames: false).Code;
        }

        /// <summary>
        /// Gets a string representation of the specified expression tree using C# syntax, with configurable exposure of compiler-generated names.
        /// The resulting string is not guaranteed to be semantically equivalent and should be used for diagnostic purposes only.
        /// </summary>
        /// <param name="expression">Expression to provide a C# code fragment for.</param>
        /// <param name="allowCompilerGeneratedNames">Indicates whether compiler-generated names are allowed in the output. If enabled, the resulting C# syntax may not be syntactically valid.</param>
        /// <returns>String representation of the specified expression tree using C# syntax.</returns>
        public static string ToCSharpString(this Expression expression, bool allowCompilerGeneratedNames)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return ToCSharpStringImpl(expression, allowCompilerGeneratedNames).Code;
        }

        /// <summary>
        /// Converts the specified expression tree to a representation using C# syntax, and acquires constants and globals that occur in the expression tree.
        /// </summary>
        /// <param name="expression">Expression to provide a C# representation for.</param>
        /// <returns>C# representation of the specified expression tree.</returns>
        public static CSharpExpression ToCSharp(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return ToCSharpStringImpl(expression, allowCompilerGeneratedNames: false);
        }

        /// <summary>
        /// Converts the specified expression tree to a representation using C# syntax, with configurable exposure of compiler-generated names, and acquires constants and globals that occur in the expression tree.
        /// </summary>
        /// <param name="expression">Expression to provide a C# representation for.</param>
        /// <param name="allowCompilerGeneratedNames">Indicates whether compiler-generated names are allowed in the output. If enabled, the resulting C# syntax may not be syntactically valid.</param>
        /// <returns>C# representation of the specified expression tree.</returns>
        public static CSharpExpression ToCSharp(this Expression expression, bool allowCompilerGeneratedNames)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return ToCSharpStringImpl(expression, allowCompilerGeneratedNames);
        }

        #endregion

        #region Private implementation

        private static TResult EvalImpl<TResult>(Expression expression, ICompiledDelegateCache cache)
        {
            // Adding the case for ExpressionType.Default turns out to be a bit tricky
            // when dealing with value types that don't have a default constructor (e.g.
            // the decimal type). See EmitDefault in ILGen.

            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    if (typeof(TResult).IsAssignableFrom(expression.Type))
                    {
                        return (TResult)((ConstantExpression)expression).Value;
                    }
                    break;
            }

            var funcletized = FuncletizeImpl<TResult>(expression);
            return cache != null ? funcletized.Compile(cache)() : funcletized.Compile()();
        }

        private static TResult EvalImpl<TResult>(Expression<Func<TResult>> expression, ICompiledDelegateCache cache)
        {
            var funcletized = FuncletizeImpl<TResult>(expression);
            return cache != null ? funcletized.Compile(cache)() : funcletized.Compile()();
        }

        private static Expression<Func<TResult>> FuncletizeImpl<TResult>(Expression expression)
        {
            CheckOpenParameters(expression);

            if (expression.Type != typeof(TResult))
            {
                expression = Expression.Convert(expression, typeof(TResult));
            }

            return Expression.Lambda<Func<TResult>>(expression);
        }

        private static Expression<Func<TResult>> FuncletizeImpl<TResult>(Expression<Func<TResult>> expression)
        {
            CheckOpenParameters(expression);

            return expression;
        }

        private static void CheckOpenParameters(Expression expression)
        {
            UnboundParameterException.ThrowIfOpen(expression, "The specified expression contains unbound parameters and cannot be funcletized or evaluated.");
        }

        private static CSharpExpression ToCSharpStringImpl(Expression expression, bool allowCompilerGeneratedNames)
        {
            // TODO: add usings as input parameter
            var prn = new ExpressionCSharpPrinter(allowCompilerGeneratedNames);

            var res = prn.Visit(expression);

            return new CSharpExpression(res, prn.Constants, prn.Globals);
        }

        #endregion
    }
}
