// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Globalization;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Base class for funclet expressions.
    /// </summary>
    public abstract class FuncletExpression : Expression
    {
        internal FuncletExpression()
        {
        }

        /// <summary>
        /// Creates a new funclet to evaluate the specified expression.
        /// </summary>
        /// <param name="expression">Expression to evaluate by the funclet.</param>
        /// <returns>Funclet expression that will evaluate the specified expression upon reduction.</returns>
        public static FuncletExpression Create(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var create = typeof(FuncletExpression<>).MakeGenericType(expression.Type).GetMethod(nameof(FuncletExpression<int>.CreateNew), BindingFlags.Static | BindingFlags.NonPublic);
            return (FuncletExpression)create.Invoke(obj: null, new object[] { expression });
        }

        /// <summary>
        /// Creates a new funclet to evaluate the specified expression.
        /// </summary>
        /// <typeparam name="T">Result type of the funclet.</typeparam>
        /// <param name="expression">Expression to evaluate by the funclet.</param>
        /// <returns>Funclet expression that will evaluate the specified expression upon reduction.</returns>
        public static FuncletExpression<T> Create<T>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return FuncletExpression<T>.CreateNew(expression);
        }

        /// <summary>
        /// Always returns true.
        /// </summary>
        public override bool CanReduce => true;

        /// <summary>
        /// Returns ExpressionType.Extension.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Extension;
    }

    /// <summary>
    /// Represents a funclet which reduces an expression by evaluating it and returning a constant expression upon reduction.
    /// </summary>
    /// <typeparam name="T">Type of the result of the funclet.</typeparam>
    public sealed class FuncletExpression<T> : FuncletExpression
    {
        /// <summary>
        /// Function to evaluate the expression.
        /// </summary>
        private readonly Func<T> _funclet;

        /// <summary>
        /// Friendly string representation of the funclet's expression.
        /// </summary>
        private readonly string _string;

        /// <summary>
        /// Creates a new funclet that will evaluate the specified expression upon reduction.
        /// </summary>
        /// <param name="expression">Expression to evaluate by the funclet.</param>
        /// <returns>Funclet expression that will evaluate the specified expression upon reduction.</returns>
        internal static FuncletExpression<T> CreateNew(Expression expression)
        {
            var funclet = expression.Funcletize<T>().Compile();
            var toString = expression.ToString();
            return new FuncletExpression<T>(funclet, toString);
        }

        private FuncletExpression(Func<T> funclet, string toString)
        {
            _funclet = funclet;
            _string = toString;
        }

        /// <summary>
        /// Gets the type of the result of the funclet.
        /// </summary>
        public override Type Type => typeof(T);

        /// <summary>
        /// Reduces the funclet to a constant.
        /// </summary>
        /// <returns>Constant expression containing the result of evaluating the funclet.</returns>
        public override Expression Reduce() => Expression.Constant(_funclet(), Type);

        /// <summary>
        /// Returns a friendly string representation of the funclet.
        /// </summary>
        /// <returns>Friendly string representation of the funclet.</returns>
        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Eval({0})", _string);
    }
}
