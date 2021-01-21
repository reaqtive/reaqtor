// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Singleton boxed <c>false</c> value.
        /// </summary>
        private static readonly object s_false = false;

        /// <summary>
        /// Singleton boxed <c>true</c> value.
        /// </summary>
        private static readonly object s_true = true;

        /// <summary>
        /// Singleton boxed <see cref="int"/> values.
        /// </summary>
        private static readonly object[] s_int32 = new object[] { 0, 1, 2, 3 };

        /// <summary>
        /// Creates an expression of type <see cref="ExpressionType.Constant"/> representing a Boolean <paramref name="value"/>.
        /// </summary>
        /// <param name="original">The original expression.</param>
        /// <param name="value">The value of the constant to create.</param>
        /// <returns>An expression representing the specified Boolean <paramref name="value"/>.</returns>
        private Expression Constant(Expression original, bool value) => Constant(original, Box(value), typeof(bool));

        /// <summary>
        /// Boxes the specified Boolean <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to box.</param>
        /// <returns>A boxed representation of the specified <paramref name="value"/>.</returns>
        private static object Box(bool value) => value ? s_true : s_false;

        /// <summary>
        /// Creates an expression of type <see cref="ExpressionType.Constant"/> representing an integer <paramref name="value"/>.
        /// </summary>
        /// <param name="original">The original expression.</param>
        /// <param name="value">The value of the constant to create.</param>
        /// <returns>An expression representing the specified integer <paramref name="value"/>.</returns>
        private Expression Constant(Expression original, int value) => Constant(original, Box(value), typeof(int));

        /// <summary>
        /// Boxes the specified Int32 <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to box.</param>
        /// <returns>A boxed representation of the specified <paramref name="value"/>.</returns>
        private static object Box(int value) => value >= 0 && value < s_int32.Length ? s_int32[value] : value;

        /// <summary>
        /// Creates an expression of type <see cref="ExpressionType.Constant"/> with the specified <paramref name="value"/>
        /// and <paramref name="type"/>. The <paramref name="original"/> expression is passed as well in order to allow
        /// overridden methods to suppress optimizations that produce an <see cref="ExpressionType.Constant"/> node.
        /// </summary>
        /// <param name="original">The original expression.</param>
        /// <param name="value">The value of the constant to create.</param>
        /// <param name="type">The static type of the node to return.</param>
        /// <returns>An expression representing the specified <paramref name="value"/> of the specified <paramref name="type"/>.</returns>
        protected virtual Expression Constant(Expression original, object value, Type type)
        {
            return Utils.Constant(value, type);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Reserved language keyword 'throw'.

        /// <summary>
        /// Creates an expression of type <see cref="ExpressionType.Throw"/> with the specified <paramref name="type"/>
        /// used to throw the specified <paramref name="exception"/> object obtained during evaluation of a pure member
        /// at compile time. The <paramref name="original"/> expression is passed as well in order to allow overridden
        /// methods to suppress optimizations that produce an <see cref="ExpressionType.Throw"/> node.
        /// </summary>
        /// <param name="original">The original expression.</param>
        /// <param name="exception">The exception that was caught during evaluation of the expression.</param>
        /// <param name="type">The static type of the node to return.</param>
        /// <returns>An expression representing throwing the specified <paramref name="exception"/>.</returns>
        protected virtual Expression Throw(Expression original, Exception exception, Type type)
        {
            // REVIEW: Throwing the same instance of an exception may be problematic.

            return Throw(original, Expression.Constant(exception), type);
        }

        /// <summary>
        /// Creates an expression of type <see cref="ExpressionType.Throw"/> with the specified <paramref name="type"/>
        /// used to throw the specified <paramref name="exception"/> expression. The <paramref name="original"/>
        /// expression is passed as well in order to allow overridden methods to suppress optimizations that produce an
        /// <see cref="ExpressionType.Throw"/> node.
        /// </summary>
        /// <param name="original">The original expression.</param>
        /// <param name="exception">The expression representing the exception to throw.</param>
        /// <param name="type">The static type of the node to return.</param>
        /// <returns>An expression representing throwing the specified <paramref name="exception"/> expression.</returns>
        /// <remarks>
        /// The introduction of <see cref="ExpressionType.Throw"/> nodes may result in making the optimized expression
        /// tree bigger. However, if the exception gets propagated by the optimizer, it has the potential of pruning out
        /// a lot of nodes from the overall tree. Derived types can control the behavior by overriding this method as
        /// well as <see cref="AlwaysThrows"/>.
        /// </remarks>
        protected virtual Expression Throw(Expression original, Expression exception, Type type)
        {
            return Expression.Throw(exception, type);
        }

#pragma warning restore CA1716
#pragma warning restore IDE0079

        /// <summary>
        /// Creates an expression representing a <c>null</c> value of the specified <paramref name="type"/>. The
        /// <paramref name="original"/> expression is passed as well in order to allow overridden methods to suppress
        /// optimizations that produce an <see cref="ExpressionType.Default"/> node.
        /// </summary>
        /// <param name="original">The original expression.</param>
        /// <param name="type">The type of the <c>null</c> value to return.</param>
        /// <returns>An expression representing <c>null</c>.</returns>
        protected virtual Expression Null(Expression original, Type type)
        {
            return Expression.Default(type);
        }
    }
}
