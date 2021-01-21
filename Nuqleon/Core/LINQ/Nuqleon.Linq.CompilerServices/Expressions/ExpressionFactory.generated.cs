// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Factory for expression trees that uses the default factory methods on <see cref="T:System.Linq.Expressions.Expression" />.
    /// </summary>
    public sealed class ExpressionFactory : IExpressionFactory
    {
        /// <summary>
        /// Gets the singleton instance of the factory.
        /// </summary>
        public static readonly IExpressionFactory Instance = new ExpressionFactory();

        private ExpressionFactory() {}

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic addition operation that does not have overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Add" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Add(Expression left, Expression right) => Expression.Add(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic addition operation that does not have overflow checking. The implementing method can be specified.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Add" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Add(Expression left, Expression right, MethodInfo method) => Expression.Add(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an addition assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression AddAssign(Expression left, Expression right) => Expression.AddAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an addition assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression AddAssign(Expression left, Expression right, MethodInfo method) => Expression.AddAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an addition assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression AddAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.AddAssign(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an addition assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression AddAssignChecked(Expression left, Expression right) => Expression.AddAssignChecked(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an addition assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression AddAssignChecked(Expression left, Expression right, MethodInfo method) => Expression.AddAssignChecked(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an addition assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression AddAssignChecked(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.AddAssignChecked(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic addition operation that has overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression AddChecked(Expression left, Expression right) => Expression.AddChecked(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic addition operation that has overflow checking. The implementing method can be specified.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression AddChecked(Expression left, Expression right, MethodInfo method) => Expression.AddChecked(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise <see langword="AND" /> operation.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.And" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The bitwise <see langword="AND" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression And(Expression left, Expression right) => Expression.And(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise <see langword="AND" /> operation. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.And" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the bitwise <see langword="AND" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression And(Expression left, Expression right, MethodInfo method) => Expression.And(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a conditional <see langword="AND" /> operation that evaluates the second operand only if the first operand evaluates to <see langword="true" />.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAlso" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The bitwise <see langword="AND" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
        public BinaryExpression AndAlso(Expression left, Expression right) => Expression.AndAlso(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a conditional <see langword="AND" /> operation that evaluates the second operand only if the first operand is resolved to true. The implementing method can be specified.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAlso" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the bitwise <see langword="AND" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="method" /> is <see langword="null" /> and <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
        public BinaryExpression AndAlso(Expression left, Expression right, MethodInfo method) => Expression.AndAlso(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise AND assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression AndAssign(Expression left, Expression right) => Expression.AndAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise AND assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression AndAssign(Expression left, Expression right, MethodInfo method) => Expression.AndAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise AND assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression AndAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.AndAssign(left, right, method, conversion);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpression" /> to access an array.</summary>
        /// <param name="array">An expression representing the array to index.</param>
        /// <param name="indexes">An array that contains expressions used to index the array.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpression" />.</returns>
        public IndexExpression ArrayAccess(Expression array, Expression[] indexes) => Expression.ArrayAccess(array, indexes);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpression" /> to access a multidimensional array.</summary>
        /// <param name="array">An expression that represents the multidimensional array.</param>
        /// <param name="indexes">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing expressions used to index the array.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpression" />.</returns>
        public IndexExpression ArrayAccess(Expression array, IEnumerable<Expression> indexes) => Expression.ArrayAccess(array, indexes);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents applying an array index operator to a multidimensional array.</summary>
        /// <param name="array">An array of <see cref="T:System.Linq.Expressions.Expression" /> instances - indexes for the array index operation.</param>
        /// <param name="indexes">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> or <paramref name="indexes" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" />.Type does not represent an array type.-or-The rank of <paramref name="array" />.Type does not match the number of elements in <paramref name="indexes" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="indexes" /> does not represent the <see cref="T:System.Int32" /> type.</exception>
        public MethodCallExpression ArrayIndex(Expression array, Expression[] indexes) => Expression.ArrayIndex(array, indexes);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents applying an array index operator to an array of rank more than one.</summary>
        /// <param name="array">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> property equal to.</param>
        /// <param name="indexes">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> or <paramref name="indexes" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" />.Type does not represent an array type.-or-The rank of <paramref name="array" />.Type does not match the number of elements in <paramref name="indexes" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="indexes" /> does not represent the <see cref="T:System.Int32" /> type.</exception>
        public MethodCallExpression ArrayIndex(Expression array, IEnumerable<Expression> indexes) => Expression.ArrayIndex(array, indexes);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents applying an array index operator to an array of rank one.</summary>
        /// <param name="array">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="index">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ArrayIndex" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> or <paramref name="index" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" />.Type does not represent an array type.-or-
        ///               <paramref name="array" />.Type represents an array type whose rank is not 1.-or-
        ///               <paramref name="index" />.Type does not represent the <see cref="T:System.Int32" /> type.</exception>
        public BinaryExpression ArrayIndex(Expression array, Expression index) => Expression.ArrayIndex(array, index);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an expression for obtaining the length of a one-dimensional array.</summary>
        /// <param name="array">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ArrayLength" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to <paramref name="array" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" />.Type does not represent an array type.</exception>
        public UnaryExpression ArrayLength(Expression array) => Expression.ArrayLength(array);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Assign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression Assign(Expression left, Expression right) => Expression.Assign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberAssignment" /> that represents the initialization of a field or property.</summary>
        /// <param name="member">A <see cref="T:System.Reflection.MemberInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberAssignment.Expression" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberAssignment" /> that has <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> equal to <see cref="F:System.Linq.Expressions.MemberBindingType.Assignment" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberAssignment.Expression" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> or <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The property represented by <paramref name="member" /> does not have a <see langword="set" /> accessor.-or-
        ///               <paramref name="expression" />.Type is not assignable to the type of the field or property that <paramref name="member" /> represents.</exception>
        public MemberAssignment Bind(MemberInfo member, Expression expression) => Expression.Bind(member, expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberAssignment" /> that represents the initialization of a member by using a property accessor method.</summary>
        /// <param name="propertyAccessor">A <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberAssignment.Expression" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberAssignment" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.Assignment" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and the <see cref="P:System.Linq.Expressions.MemberAssignment.Expression" /> property set to <paramref name="expression" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="propertyAccessor" /> or <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The property accessed by <paramref name="propertyAccessor" /> does not have a <see langword="set" /> accessor.-or-
        ///               <paramref name="expression" />.Type is not assignable to the type of the field or property that <paramref name="propertyAccessor" /> represents.</exception>
        public MemberAssignment Bind(MethodInfo propertyAccessor, Expression expression) => Expression.Bind(propertyAccessor, expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains the given expressions and has no variables.</summary>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(Expression[] expressions) => Expression.Block(expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains the given expressions and has no variables.</summary>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(IEnumerable<Expression> expressions) => Expression.Block(expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains two expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(Expression arg0, Expression arg1) => Expression.Block(arg0, arg1);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains the given expressions, has no variables and has specific result type.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(Type type, Expression[] expressions) => Expression.Block(type, expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains the given expressions, has no variables and has specific result type.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(Type type, IEnumerable<Expression> expressions) => Expression.Block(type, expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains the given variables and expressions.</summary>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(IEnumerable<ParameterExpression> variables, Expression[] expressions) => Expression.Block(variables, expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains the given variables and expressions.</summary>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions) => Expression.Block(variables, expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains three expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <param name="arg2">The third expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(Expression arg0, Expression arg1, Expression arg2) => Expression.Block(arg0, arg1, arg2);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains the given variables and expressions.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(Type type, IEnumerable<ParameterExpression> variables, Expression[] expressions) => Expression.Block(type, variables, expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains the given variables and expressions.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(Type type, IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions) => Expression.Block(type, variables, expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains four expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <param name="arg2">The third expression in the block.</param>
        /// <param name="arg3">The fourth expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(Expression arg0, Expression arg1, Expression arg2, Expression arg3) => Expression.Block(arg0, arg1, arg2, arg3);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpression" /> that contains five expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <param name="arg2">The third expression in the block.</param>
        /// <param name="arg3">The fourth expression in the block.</param>
        /// <param name="arg4">The fifth expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpression" />.</returns>
        public BlockExpression Block(Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4) => Expression.Block(arg0, arg1, arg2, arg3, arg4);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a break statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpression Break(LabelTarget target) => Expression.Break(target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a break statement. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpression Break(LabelTarget target, Expression value) => Expression.Break(target, value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a break statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, and the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to <paramref name="type" />.</returns>
        public GotoExpression Break(LabelTarget target, Type type) => Expression.Break(target, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a break statement with the specified type. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpression Break(LabelTarget target, Expression value, Type type) => Expression.Break(target, value, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method that takes one argument.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the first argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpression Call(MethodInfo method, Expression arg0) => Expression.Call(method, arg0);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method that has arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in <paramref name="arguments" /> does not equal the number of parameters for the method represented by <paramref name="method" />.-or-One or more of the elements of <paramref name="arguments" /> is not assignable to the corresponding parameter for the method represented by <paramref name="method" />.</exception>
        public MethodCallExpression Call(MethodInfo method, Expression[] arguments) => Expression.Call(method, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a static (Shared in Visual Basic) method.</summary>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> that represents the target method.</param>
        /// <param name="arguments">A collection of <see cref="T:System.Linq.Expressions.Expression" /> that represents the call arguments.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
        public MethodCallExpression Call(MethodInfo method, IEnumerable<Expression> arguments) => Expression.Call(method, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a method that takes no arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> that specifies the instance for an instance method call (pass <see langword="null" /> for a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method).</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is <see langword="null" />.-or-
        ///               <paramref name="instance" /> is <see langword="null" /> and <paramref name="method" /> represents an instance method.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="instance" />.Type is not assignable to the declaring type of the method represented by <paramref name="method" />.</exception>
        public MethodCallExpression Call(Expression instance, MethodInfo method) => Expression.Call(instance, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a static method that takes two arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the second argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1) => Expression.Call(method, arg0, arg1);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a method that takes arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> that specifies the instance for an instance method call (pass <see langword="null" /> for a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method).</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" />, <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" />, and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is <see langword="null" />.-or-
        ///               <paramref name="instance" /> is <see langword="null" /> and <paramref name="method" /> represents an instance method.-or-
        ///               <paramref name="arguments" /> is not <see langword="null" /> and one or more of its elements is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="instance" />.Type is not assignable to the declaring type of the method represented by <paramref name="method" />.-or-The number of elements in <paramref name="arguments" /> does not equal the number of parameters for the method represented by <paramref name="method" />.-or-One or more of the elements of <paramref name="arguments" /> is not assignable to the corresponding parameter for the method represented by <paramref name="method" />.</exception>
        public MethodCallExpression Call(Expression instance, MethodInfo method, Expression[] arguments) => Expression.Call(instance, method, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a method that takes arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> property equal to (pass <see langword="null" /> for a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method).</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" />, <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" />, and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is <see langword="null" />.-or-
        ///               <paramref name="instance" /> is <see langword="null" /> and <paramref name="method" /> represents an instance method.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="instance" />.Type is not assignable to the declaring type of the method represented by <paramref name="method" />.-or-The number of elements in <paramref name="arguments" /> does not equal the number of parameters for the method represented by <paramref name="method" />.-or-One or more of the elements of <paramref name="arguments" /> is not assignable to the corresponding parameter for the method represented by <paramref name="method" />.</exception>
        public MethodCallExpression Call(Expression instance, MethodInfo method, IEnumerable<Expression> arguments) => Expression.Call(instance, method, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a static method that takes three arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the third argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2) => Expression.Call(method, arg0, arg1, arg2);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a method that takes two arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> that specifies the instance for an instance call. (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> that represents the target method.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the second argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
        public MethodCallExpression Call(Expression instance, MethodInfo method, Expression arg0, Expression arg1) => Expression.Call(instance, method, arg0, arg1);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a method by calling the appropriate factory method.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> whose <see cref="P:System.Linq.Expressions.Expression.Type" /> property value will be searched for a specific method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="typeArguments">An array of <see cref="T:System.Type" /> objects that specify the type parameters of the generic method. This argument should be null when methodName specifies a non-generic method.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects that represents the arguments to the method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" />, the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> property equal to <paramref name="instance" />, <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> set to the <see cref="T:System.Reflection.MethodInfo" /> that represents the specified instance method, and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> set to the specified arguments.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="instance" /> or <paramref name="methodName" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">No method whose name is <paramref name="methodName" />, whose type parameters match <paramref name="typeArguments" />, and whose parameter types match <paramref name="arguments" /> is found in <paramref name="instance" />.Type or its base types.-or-More than one method whose name is <paramref name="methodName" />, whose type parameters match <paramref name="typeArguments" />, and whose parameter types match <paramref name="arguments" /> is found in <paramref name="instance" />.Type or its base types.</exception>
        public MethodCallExpression Call(Expression instance, String methodName, Type[] typeArguments, Expression[] arguments) => Expression.Call(instance, methodName, typeArguments, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method by calling the appropriate factory method.</summary>
        /// <param name="type">The <see cref="T:System.Type" /> that specifies the type that contains the specified <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="typeArguments">An array of <see cref="T:System.Type" /> objects that specify the type parameters of the generic method. This argument should be null when methodName specifies a non-generic method.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects that represent the arguments to the method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" />, the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property set to the <see cref="T:System.Reflection.MethodInfo" /> that represents the specified <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method, and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> property set to the specified arguments.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="methodName" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">No method whose name is <paramref name="methodName" />, whose type parameters match <paramref name="typeArguments" />, and whose parameter types match <paramref name="arguments" /> is found in <paramref name="type" /> or its base types.-or-More than one method whose name is <paramref name="methodName" />, whose type parameters match <paramref name="typeArguments" />, and whose parameter types match <paramref name="arguments" /> is found in <paramref name="type" /> or its base types.</exception>
        public MethodCallExpression Call(Type type, String methodName, Type[] typeArguments, Expression[] arguments) => Expression.Call(type, methodName, typeArguments, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a static method that takes four arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the third argument.</param>
        /// <param name="arg3">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the fourth argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3) => Expression.Call(method, arg0, arg1, arg2, arg3);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a method that takes three arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> that specifies the instance for an instance call. (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> that represents the target method.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the third argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
        public MethodCallExpression Call(Expression instance, MethodInfo method, Expression arg0, Expression arg1, Expression arg2) => Expression.Call(instance, method, arg0, arg1, arg2);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a static method that takes five arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the third argument.</param>
        /// <param name="arg3">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the fourth argument.</param>
        /// <param name="arg4">The <see cref="T:System.Linq.Expressions.Expression" /> that represents the fifth argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4) => Expression.Call(method, arg0, arg1, arg2, arg3, arg4);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlock" /> representing a catch statement.</summary>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.Expression.Type" /> of <see cref="T:System.Exception" /> this <see cref="T:System.Linq.Expressions.CatchBlock" /> will handle.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlock" />.</returns>
        public CatchBlock Catch(Type type, Expression body) => Expression.Catch(type, body);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlock" /> representing a catch statement with a reference to the caught <see cref="T:System.Exception" /> object for use in the handler body.</summary>
        /// <param name="variable">A <see cref="T:System.Linq.Expressions.ParameterExpression" /> representing a reference to the <see cref="T:System.Exception" /> object caught by this handler.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlock" />.</returns>
        public CatchBlock Catch(ParameterExpression variable, Expression body) => Expression.Catch(variable, body);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlock" /> representing a catch statement with an <see cref="T:System.Exception" /> filter but no reference to the caught <see cref="T:System.Exception" /> object.</summary>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.Expression.Type" /> of <see cref="T:System.Exception" /> this <see cref="T:System.Linq.Expressions.CatchBlock" /> will handle.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <param name="filter">The body of the <see cref="T:System.Exception" /> filter.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlock" />.</returns>
        public CatchBlock Catch(Type type, Expression body, Expression filter) => Expression.Catch(type, body, filter);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlock" /> representing a catch statement with an <see cref="T:System.Exception" /> filter and a reference to the caught <see cref="T:System.Exception" /> object.</summary>
        /// <param name="variable">A <see cref="T:System.Linq.Expressions.ParameterExpression" /> representing a reference to the <see cref="T:System.Exception" /> object caught by this handler.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <param name="filter">The body of the <see cref="T:System.Exception" /> filter.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlock" />.</returns>
        public CatchBlock Catch(ParameterExpression variable, Expression body, Expression filter) => Expression.Catch(variable, body, filter);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DebugInfoExpression" /> for clearing a sequence point.</summary>
        /// <param name="document">The <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" /> that represents the source file.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.DebugInfoExpression" /> for clearning a sequence point.</returns>
        public DebugInfoExpression ClearDebugInfo(SymbolDocumentInfo document) => Expression.ClearDebugInfo(document);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a coalescing operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of <paramref name="left" /> does not represent a reference type or a nullable value type.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="left" />.Type and <paramref name="right" />.Type are not convertible to each other.</exception>
        public BinaryExpression Coalesce(Expression left, Expression right) => Expression.Coalesce(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a coalescing operation, given a conversion function.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="left" />.Type and <paramref name="right" />.Type are not convertible to each other.-or-
        ///               <paramref name="conversion" /> is not <see langword="null" /> and <paramref name="conversion" />.Type is a delegate type that does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of <paramref name="left" /> does not represent a reference type or a nullable value type.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of <paramref name="left" /> represents a type that is not assignable to the parameter type of the delegate type <paramref name="conversion" />.Type.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of <paramref name="right" /> is not equal to the return type of the delegate type <paramref name="conversion" />.Type.</exception>
        public BinaryExpression Coalesce(Expression left, Expression right, LambdaExpression conversion) => Expression.Coalesce(left, right, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpression" /> that represents a conditional statement.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.IfFalse" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpression.IfFalse" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="test" /> or <paramref name="ifTrue" /> or <paramref name="ifFalse" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="test" />.Type is not <see cref="T:System.Boolean" />.-or-
        ///               <paramref name="ifTrue" />.Type is not equal to <paramref name="ifFalse" />.Type.</exception>
        public ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse) => Expression.Condition(test, ifTrue, ifFalse);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpression" /> that represents a conditional statement.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.IfFalse" /> property equal to.</param>
        /// <param name="type">A <see cref="P:System.Linq.Expressions.Expression.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpression.IfFalse" /> properties set to the specified values.</returns>
        public ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse, Type type) => Expression.Condition(test, ifTrue, ifFalse, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConstantExpression" /> that has the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> property set to the specified value.</summary>
        /// <param name="value">An <see cref="T:System.Object" /> to set the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConstantExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Constant" /> and the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> property set to the specified value.</returns>
        public ConstantExpression Constant(Object value) => Expression.Constant(value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConstantExpression" /> that has the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</summary>
        /// <param name="value">An <see cref="T:System.Object" /> to set the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConstantExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Constant" /> and the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="value" /> is not <see langword="null" /> and <paramref name="type" /> is not assignable from the dynamic type of <paramref name="value" />.</exception>
        public ConstantExpression Constant(Object value, Type type) => Expression.Constant(value, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a continue statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpression Continue(LabelTarget target) => Expression.Continue(target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a continue statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to <paramref name="type" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpression Continue(LabelTarget target, Type type) => Expression.Continue(target, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a type conversion operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Convert" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.</exception>
        public UnaryExpression Convert(Expression expression, Type type) => Expression.Convert(expression, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a conversion operation for which the implementing method is specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Convert" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" />, <see cref="P:System.Linq.Expressions.Expression.Type" />, and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.-or-
        ///               <paramref name="expression" />.Type is not assignable to the argument type of the method represented by <paramref name="method" />.-or-The return type of the method represented by <paramref name="method" /> is not assignable to <paramref name="type" />.-or-
        ///               <paramref name="expression" />.Type or <paramref name="type" /> is a nullable value type and the corresponding non-nullable value type does not equal the argument type or the return type, respectively, of the method represented by <paramref name="method" />.</exception>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one method that matches the <paramref name="method" /> description was found.</exception>
        public UnaryExpression Convert(Expression expression, Type type, MethodInfo method) => Expression.Convert(expression, type, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a conversion operation that throws an exception if the target type is overflowed.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ConvertChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.</exception>
        public UnaryExpression ConvertChecked(Expression expression, Type type) => Expression.ConvertChecked(expression, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a conversion operation that throws an exception if the target type is overflowed and for which the implementing method is specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ConvertChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" />, <see cref="P:System.Linq.Expressions.Expression.Type" />, and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.-or-
        ///               <paramref name="expression" />.Type is not assignable to the argument type of the method represented by <paramref name="method" />.-or-The return type of the method represented by <paramref name="method" /> is not assignable to <paramref name="type" />.-or-
        ///               <paramref name="expression" />.Type or <paramref name="type" /> is a nullable value type and the corresponding non-nullable value type does not equal the argument type or the return type, respectively, of the method represented by <paramref name="method" />.</exception>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one method that matches the <paramref name="method" /> description was found.</exception>
        public UnaryExpression ConvertChecked(Expression expression, Type type, MethodInfo method) => Expression.ConvertChecked(expression, type, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DebugInfoExpression" /> with the specified span.</summary>
        /// <param name="document">The <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" /> that represents the source file.</param>
        /// <param name="startLine">The start line of this <see cref="T:System.Linq.Expressions.DebugInfoExpression" />. Must be greater than 0.</param>
        /// <param name="startColumn">The start column of this <see cref="T:System.Linq.Expressions.DebugInfoExpression" />. Must be greater than 0.</param>
        /// <param name="endLine">The end line of this <see cref="T:System.Linq.Expressions.DebugInfoExpression" />. Must be greater or equal than the start line.</param>
        /// <param name="endColumn">The end column of this <see cref="T:System.Linq.Expressions.DebugInfoExpression" />. If the end line is the same as the start line, it must be greater or equal than the start column. In any case, must be greater than 0.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.DebugInfoExpression" />.</returns>
        public DebugInfoExpression DebugInfo(SymbolDocumentInfo document, Int32 startLine, Int32 startColumn, Int32 endLine, Int32 endColumn) => Expression.DebugInfo(document, startLine, startColumn, endLine, endColumn);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the decrementing of the expression by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to decrement.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the decremented expression.</returns>
        public UnaryExpression Decrement(Expression expression) => Expression.Decrement(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the decrementing of the expression by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to decrement.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the decremented expression.</returns>
        public UnaryExpression Decrement(Expression expression, MethodInfo method) => Expression.Decrement(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DefaultExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to the specified type.</summary>
        /// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DefaultExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Default" /> and the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to the specified type.</returns>
        public DefaultExpression Default(Type type) => Expression.Default(type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic division operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Divide" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The division operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Divide(Expression left, Expression right) => Expression.Divide(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic division operation. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Divide" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the division operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Divide(Expression left, Expression right, MethodInfo method) => Expression.Divide(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a division assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.DivideAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression DivideAssign(Expression left, Expression right) => Expression.DivideAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a division assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.DivideAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression DivideAssign(Expression left, Expression right, MethodInfo method) => Expression.DivideAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a division assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.DivideAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression DivideAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.DivideAssign(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" />.</summary>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="returnType">The result type of the dynamic expression.</param>
        /// <param name="arguments">The arguments to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" /> and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression[] arguments) => Expression.Dynamic(binder, returnType, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" />.</summary>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="returnType">The result type of the dynamic expression.</param>
        /// <param name="arg0">The first argument to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" /> and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0) => Expression.Dynamic(binder, returnType, arg0);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" />.</summary>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="returnType">The result type of the dynamic expression.</param>
        /// <param name="arguments">The arguments to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" /> and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, IEnumerable<Expression> arguments) => Expression.Dynamic(binder, returnType, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" />.</summary>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="returnType">The result type of the dynamic expression.</param>
        /// <param name="arg0">The first argument to the dynamic operation.</param>
        /// <param name="arg1">The second argument to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" /> and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1) => Expression.Dynamic(binder, returnType, arg0, arg1);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" />.</summary>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="returnType">The result type of the dynamic expression.</param>
        /// <param name="arg0">The first argument to the dynamic operation.</param>
        /// <param name="arg1">The second argument to the dynamic operation.</param>
        /// <param name="arg2">The third argument to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" /> and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2) => Expression.Dynamic(binder, returnType, arg0, arg1, arg2);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" />.</summary>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="returnType">The result type of the dynamic expression.</param>
        /// <param name="arg0">The first argument to the dynamic operation.</param>
        /// <param name="arg1">The second argument to the dynamic operation.</param>
        /// <param name="arg2">The third argument to the dynamic operation.</param>
        /// <param name="arg3">The fourth argument to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" /> and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2, Expression arg3) => Expression.Dynamic(binder, returnType, arg0, arg1, arg2, arg3);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.ElementInit" />, given an array of values as the second argument.</summary>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.ElementInit.AddMethod" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to set the <see cref="P:System.Linq.Expressions.ElementInit.Arguments" /> property equal to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.ElementInit" /> that has the <see cref="P:System.Linq.Expressions.ElementInit.AddMethod" /> and <see cref="P:System.Linq.Expressions.ElementInit.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="addMethod" /> or <paramref name="arguments" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The method that addMethod represents is not named "Add" (case insensitive).-or-The method that addMethod represents is not an instance method.-or-arguments does not contain the same number of elements as the number of parameters for the method that addMethod represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the method that <paramref name="addMethod" /> represents.</exception>
        public ElementInit ElementInit(MethodInfo addMethod, Expression[] arguments) => Expression.ElementInit(addMethod, arguments);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.ElementInit" />, given an <see cref="T:System.Collections.Generic.IEnumerable`1" /> as the second argument.</summary>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.ElementInit.AddMethod" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to set the <see cref="P:System.Linq.Expressions.ElementInit.Arguments" /> property equal to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.ElementInit" /> that has the <see cref="P:System.Linq.Expressions.ElementInit.AddMethod" /> and <see cref="P:System.Linq.Expressions.ElementInit.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="addMethod" /> or <paramref name="arguments" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The method that <paramref name="addMethod" /> represents is not named "Add" (case insensitive).-or-The method that <paramref name="addMethod" /> represents is not an instance method.-or-
        ///               <paramref name="arguments" /> does not contain the same number of elements as the number of parameters for the method that <paramref name="addMethod" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the method that <paramref name="addMethod" /> represents.</exception>
        public ElementInit ElementInit(MethodInfo addMethod, IEnumerable<Expression> arguments) => Expression.ElementInit(addMethod, arguments);

        /// <summary>Creates an empty expression that has <see cref="T:System.Void" /> type.</summary>
        /// <returns>A <see cref="T:System.Linq.Expressions.DefaultExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Default" /> and the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to <see cref="T:System.Void" />.</returns>
        public DefaultExpression Empty() => Expression.Empty();

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an equality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The equality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Equal(Expression left, Expression right) => Expression.Equal(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an equality comparison. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the equality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Equal(Expression left, Expression right, Boolean liftToNull, MethodInfo method) => Expression.Equal(left, right, liftToNull, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise <see langword="XOR" /> operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOr" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see langword="XOR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression ExclusiveOr(Expression left, Expression right) => Expression.ExclusiveOr(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise <see langword="XOR" /> operation, using op_ExclusiveOr for user-defined types. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOr" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the <see langword="XOR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression ExclusiveOr(Expression left, Expression right, MethodInfo method) => Expression.ExclusiveOr(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise XOR assignment operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression ExclusiveOrAssign(Expression left, Expression right) => Expression.ExclusiveOrAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise XOR assignment operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression ExclusiveOrAssign(Expression left, Expression right, MethodInfo method) => Expression.ExclusiveOrAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise XOR assignment operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression ExclusiveOrAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.ExclusiveOrAssign(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a field.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property equal to. For <see langword="static" /> (<see langword="Shared" /> in Visual Basic), <paramref name="expression" /> must be <see langword="null" />.</param>
        /// <param name="field">The <see cref="T:System.Reflection.FieldInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" /> and the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> and <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="field" /> is <see langword="null" />.-or-The field represented by <paramref name="field" /> is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic) and <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="expression" />.Type is not assignable to the declaring type of the field represented by <paramref name="field" />.</exception>
        public MemberExpression Field(Expression expression, FieldInfo field) => Expression.Field(expression, field);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a field given the name of the field.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> whose <see cref="P:System.Linq.Expressions.Expression.Type" /> contains a field named <paramref name="fieldName" />. This can be null for static fields.</param>
        /// <param name="fieldName">The name of a field to be accessed.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" />, the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property set to <paramref name="expression" />, and the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property set to the <see cref="T:System.Reflection.FieldInfo" /> that represents the field denoted by <paramref name="fieldName" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="fieldName" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">No field named <paramref name="fieldName" /> is defined in <paramref name="expression" />.Type or its base types.</exception>
        public MemberExpression Field(Expression expression, String fieldName) => Expression.Field(expression, fieldName);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a field.</summary>
        /// <param name="expression">The containing object of the field. This can be null for static fields.</param>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.Expression.Type" /> that contains the field.</param>
        /// <param name="fieldName">The field to be accessed.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.MemberExpression" />.</returns>
        public MemberExpression Field(Expression expression, Type type, String fieldName) => Expression.Field(expression, type, fieldName);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a "go to" statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to the specified value, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpression Goto(LabelTarget target) => Expression.Goto(target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a "go to" statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to the specified value, the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to <paramref name="type" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpression Goto(LabelTarget target, Type type) => Expression.Goto(target, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a "go to" statement. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpression Goto(LabelTarget target, Expression value) => Expression.Goto(target, value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a "go to" statement with the specified type. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpression Goto(LabelTarget target, Expression value, Type type) => Expression.Goto(target, value, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "greater than" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The "greater than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression GreaterThan(Expression left, Expression right) => Expression.GreaterThan(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "greater than" numeric comparison. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the "greater than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression GreaterThan(Expression left, Expression right, Boolean liftToNull, MethodInfo method) => Expression.GreaterThan(left, right, liftToNull, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "greater than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The "greater than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression GreaterThanOrEqual(Expression left, Expression right) => Expression.GreaterThanOrEqual(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "greater than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the "greater than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression GreaterThanOrEqual(Expression left, Expression right, Boolean liftToNull, MethodInfo method) => Expression.GreaterThanOrEqual(left, right, liftToNull, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpression" /> that represents a conditional block with an <see langword="if" /> statement.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" />, properties set to the specified values. The <see cref="P:System.Linq.Expressions.ConditionalExpression.IfFalse" /> property is set to default expression and the type of the resulting <see cref="T:System.Linq.Expressions.ConditionalExpression" /> returned by this method is <see cref="T:System.Void" />.</returns>
        public ConditionalExpression IfThen(Expression test, Expression ifTrue) => Expression.IfThen(test, ifTrue);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpression" /> that represents a conditional block with <see langword="if" /> and <see langword="else" /> statements.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.IfFalse" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpression.IfFalse" /> properties set to the specified values. The type of the resulting <see cref="T:System.Linq.Expressions.ConditionalExpression" /> returned by this method is <see cref="T:System.Void" />.</returns>
        public ConditionalExpression IfThenElse(Expression test, Expression ifTrue, Expression ifFalse) => Expression.IfThenElse(test, ifTrue, ifFalse);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the incrementing of the expression value by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to increment.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the incremented expression.</returns>
        public UnaryExpression Increment(Expression expression) => Expression.Increment(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the incrementing of the expression by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to increment.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the incremented expression.</returns>
        public UnaryExpression Increment(Expression expression, MethodInfo method) => Expression.Increment(expression, method);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.InvocationExpression" /> that applies a delegate or lambda expression to a list of argument expressions.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the delegate or lambda expression to be applied.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects that represent the arguments that the delegate or lambda expression is applied to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.InvocationExpression" /> that applies the specified delegate or lambda expression to the provided arguments.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="expression" />.Type does not represent a delegate type or an <see cref="T:System.Linq.Expressions.Expression`1" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the delegate represented by <paramref name="expression" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="arguments" /> does not contain the same number of elements as the list of parameters for the delegate represented by <paramref name="expression" />.</exception>
        public InvocationExpression Invoke(Expression expression, Expression[] arguments) => Expression.Invoke(expression, arguments);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.InvocationExpression" /> that applies a delegate or lambda expression to a list of argument expressions.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the delegate or lambda expression to be applied to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects that represent the arguments that the delegate or lambda expression is applied to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.InvocationExpression" /> that applies the specified delegate or lambda expression to the provided arguments.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="expression" />.Type does not represent a delegate type or an <see cref="T:System.Linq.Expressions.Expression`1" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the delegate represented by <paramref name="expression" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="arguments" /> does not contain the same number of elements as the list of parameters for the delegate represented by <paramref name="expression" />.</exception>
        public InvocationExpression Invoke(Expression expression, IEnumerable<Expression> arguments) => Expression.Invoke(expression, arguments);

        /// <summary>Returns whether the expression evaluates to false.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to evaluate.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpression" />.</returns>
        public UnaryExpression IsFalse(Expression expression) => Expression.IsFalse(expression);

        /// <summary>Returns whether the expression evaluates to false.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to evaluate.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpression" />.</returns>
        public UnaryExpression IsFalse(Expression expression, MethodInfo method) => Expression.IsFalse(expression, method);

        /// <summary>Returns whether the expression evaluates to true.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to evaluate.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpression" />.</returns>
        public UnaryExpression IsTrue(Expression expression) => Expression.IsTrue(expression);

        /// <summary>Returns whether the expression evaluates to true.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to evaluate.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpression" />.</returns>
        public UnaryExpression IsTrue(Expression expression, MethodInfo method) => Expression.IsTrue(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTarget" /> representing a label with void type and no name.</summary>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTarget" />.</returns>
        public LabelTarget Label() => Expression.Label();

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelExpression" /> representing a label without a default value.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> which this <see cref="T:System.Linq.Expressions.LabelExpression" /> will be associated with.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LabelExpression" /> without a default value.</returns>
        public LabelExpression Label(LabelTarget target) => Expression.Label(target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTarget" /> representing a label with void type and the given name.</summary>
        /// <param name="name">The name of the label.</param>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTarget" />.</returns>
        public LabelTarget Label(String name) => Expression.Label(name);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTarget" /> representing a label with the given type.</summary>
        /// <param name="type">The type of value that is passed when jumping to the label.</param>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTarget" />.</returns>
        public LabelTarget Label(Type type) => Expression.Label(type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelExpression" /> representing a label with the given default value.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> which this <see cref="T:System.Linq.Expressions.LabelExpression" /> will be associated with.</param>
        /// <param name="defaultValue">The value of this <see cref="T:System.Linq.Expressions.LabelExpression" /> when the label is reached through regular control flow.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LabelExpression" /> with the given default value.</returns>
        public LabelExpression Label(LabelTarget target, Expression defaultValue) => Expression.Label(target, defaultValue);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTarget" /> representing a label with the given type and name.</summary>
        /// <param name="type">The type of value that is passed when jumping to the label.</param>
        /// <param name="name">The name of the label.</param>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTarget" />.</returns>
        public LabelTarget Label(Type type, String name) => Expression.Label(type, name);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.Expression`1" /> where the delegate type is known at compile time.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">A delegate type.</typeparam>
        /// <returns>An <see cref="T:System.Linq.Expressions.Expression`1" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="body" /> is <see langword="null" />.-or-One or more elements in <paramref name="parameters" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <typeparamref name="TDelegate" /> is not a delegate type.-or-
        ///               <paramref name="body" />.Type represents a type that is not assignable to the return type of <typeparamref name="TDelegate" />.-or-
        ///               <paramref name="parameters" /> does not contain the same number of elements as the list of parameters for <typeparamref name="TDelegate" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of <typeparamref name="TDelegate" />.</exception>
        public Expression<TDelegate> Lambda<TDelegate>(Expression body, ParameterExpression[] parameters) => Expression.Lambda<TDelegate>(body, parameters);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.Expression`1" /> where the delegate type is known at compile time.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">A delegate type.</typeparam>
        /// <returns>An <see cref="T:System.Linq.Expressions.Expression`1" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="body" /> is <see langword="null" />.-or-One or more elements in <paramref name="parameters" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <typeparamref name="TDelegate" /> is not a delegate type.-or-
        ///               <paramref name="body" />.Type represents a type that is not assignable to the return type of <typeparamref name="TDelegate" />.-or-
        ///               <paramref name="parameters" /> does not contain the same number of elements as the list of parameters for <typeparamref name="TDelegate" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of <typeparamref name="TDelegate" />.</exception>
        public Expression<TDelegate> Lambda<TDelegate>(Expression body, IEnumerable<ParameterExpression> parameters) => Expression.Lambda<TDelegate>(body, parameters);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpression" /> by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="body" /> is <see langword="null" />.-or-One or more elements of <paramref name="parameters" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="parameters" /> contains more than sixteen elements.</exception>
        public LambdaExpression Lambda(Expression body, ParameterExpression[] parameters) => Expression.Lambda(body, parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpression Lambda(Expression body, IEnumerable<ParameterExpression> parameters) => Expression.Lambda(body, parameters);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.Expression`1" /> where the delegate type is known at compile time.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="T:System.Boolean" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An array that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">The delegate type. </typeparam>
        /// <returns>An <see cref="T:System.Linq.Expressions.Expression`1" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public Expression<TDelegate> Lambda<TDelegate>(Expression body, Boolean tailCall, ParameterExpression[] parameters) => Expression.Lambda<TDelegate>(body, tailCall, parameters);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.Expression`1" /> where the delegate type is known at compile time.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="T:System.Boolean" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">The delegate type. </typeparam>
        /// <returns>An <see cref="T:System.Linq.Expressions.Expression`1" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public Expression<TDelegate> Lambda<TDelegate>(Expression body, Boolean tailCall, IEnumerable<ParameterExpression> parameters) => Expression.Lambda<TDelegate>(body, tailCall, parameters);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.Expression`1" /> where the delegate type is known at compile time.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name of the lambda. Used for generating debugging information.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">The delegate type. </typeparam>
        /// <returns>An <see cref="T:System.Linq.Expressions.Expression`1" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public Expression<TDelegate> Lambda<TDelegate>(Expression body, String name, IEnumerable<ParameterExpression> parameters) => Expression.Lambda<TDelegate>(body, name, parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="T:System.Boolean" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An array that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpression Lambda(Expression body, Boolean tailCall, ParameterExpression[] parameters) => Expression.Lambda(body, tailCall, parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="T:System.Boolean" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpression Lambda(Expression body, Boolean tailCall, IEnumerable<ParameterExpression> parameters) => Expression.Lambda(body, tailCall, parameters);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpression" /> by first constructing a delegate type. It can be used when the delegate type is not known at compile time.</summary>
        /// <param name="delegateType">A <see cref="T:System.Type" /> that represents a delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>An object that represents a lambda expression which has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="delegateType" /> or <paramref name="body" /> is <see langword="null" />.-or-One or more elements in <paramref name="parameters" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="delegateType" /> does not represent a delegate type.-or-
        ///               <paramref name="body" />.Type represents a type that is not assignable to the return type of the delegate type represented by <paramref name="delegateType" />.-or-
        ///               <paramref name="parameters" /> does not contain the same number of elements as the list of parameters for the delegate type represented by <paramref name="delegateType" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of the delegate type represented by <paramref name="delegateType" />.</exception>
        public LambdaExpression Lambda(Type delegateType, Expression body, ParameterExpression[] parameters) => Expression.Lambda(delegateType, body, parameters);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpression" /> by first constructing a delegate type. It can be used when the delegate type is not known at compile time.</summary>
        /// <param name="delegateType">A <see cref="T:System.Type" /> that represents a delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>An object that represents a lambda expression which has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="delegateType" /> or <paramref name="body" /> is <see langword="null" />.-or-One or more elements in <paramref name="parameters" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="delegateType" /> does not represent a delegate type.-or-
        ///               <paramref name="body" />.Type represents a type that is not assignable to the return type of the delegate type represented by <paramref name="delegateType" />.-or-
        ///               <paramref name="parameters" /> does not contain the same number of elements as the list of parameters for the delegate type represented by <paramref name="delegateType" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of the delegate type represented by <paramref name="delegateType" />.</exception>
        public LambdaExpression Lambda(Type delegateType, Expression body, IEnumerable<ParameterExpression> parameters) => Expression.Lambda(delegateType, body, parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name for the lambda. Used for emitting debug information.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpression Lambda(Expression body, String name, IEnumerable<ParameterExpression> parameters) => Expression.Lambda(body, name, parameters);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.Expression`1" /> where the delegate type is known at compile time.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name of the lambda. Used for generating debugging info.</param>
        /// <param name="tailCall">A <see cref="T:System.Boolean" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">The delegate type. </typeparam>
        /// <returns>An <see cref="T:System.Linq.Expressions.Expression`1" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public Expression<TDelegate> Lambda<TDelegate>(Expression body, String name, Boolean tailCall, IEnumerable<ParameterExpression> parameters) => Expression.Lambda<TDelegate>(body, name, tailCall, parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="delegateType">A <see cref="P:System.Linq.Expressions.Expression.Type" /> representing the delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="T:System.Boolean" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An array that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpression Lambda(Type delegateType, Expression body, Boolean tailCall, ParameterExpression[] parameters) => Expression.Lambda(delegateType, body, tailCall, parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="delegateType">A <see cref="P:System.Linq.Expressions.Expression.Type" /> representing the delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="T:System.Boolean" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpression Lambda(Type delegateType, Expression body, Boolean tailCall, IEnumerable<ParameterExpression> parameters) => Expression.Lambda(delegateType, body, tailCall, parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name for the lambda. Used for emitting debug information.</param>
        /// <param name="tailCall">A <see cref="T:System.Boolean" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpression Lambda(Expression body, String name, Boolean tailCall, IEnumerable<ParameterExpression> parameters) => Expression.Lambda(body, name, tailCall, parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="delegateType">A <see cref="P:System.Linq.Expressions.Expression.Type" /> representing the delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name for the lambda. Used for emitting debug information.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpression Lambda(Type delegateType, Expression body, String name, IEnumerable<ParameterExpression> parameters) => Expression.Lambda(delegateType, body, name, parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="delegateType">A <see cref="P:System.Linq.Expressions.Expression.Type" /> representing the delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to. </param>
        /// <param name="name">The name for the lambda. Used for emitting debug information.</param>
        /// <param name="tailCall">A <see cref="T:System.Boolean" /> that indicates if tail call optimization will be applied when compiling the created expression. </param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection. </param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpression Lambda(Type delegateType, Expression body, String name, Boolean tailCall, IEnumerable<ParameterExpression> parameters) => Expression.Lambda(delegateType, body, name, tailCall, parameters);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise left-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The left-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression LeftShift(Expression left, Expression right) => Expression.LeftShift(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise left-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the left-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression LeftShift(Expression left, Expression right, MethodInfo method) => Expression.LeftShift(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise left-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression LeftShiftAssign(Expression left, Expression right) => Expression.LeftShiftAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise left-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression LeftShiftAssign(Expression left, Expression right, MethodInfo method) => Expression.LeftShiftAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise left-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression LeftShiftAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.LeftShiftAssign(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "less than" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The "less than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression LessThan(Expression left, Expression right) => Expression.LessThan(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "less than" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the "less than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression LessThan(Expression left, Expression right, Boolean liftToNull, MethodInfo method) => Expression.LessThan(left, right, liftToNull, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a " less than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The "less than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression LessThanOrEqual(Expression left, Expression right) => Expression.LessThanOrEqual(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "less than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the "less than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression LessThanOrEqual(Expression left, Expression right, Boolean liftToNull, MethodInfo method) => Expression.LessThanOrEqual(left, right, liftToNull, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBinding" /> where the member is a field or property.</summary>
        /// <param name="member">A <see cref="T:System.Reflection.MemberInfo" /> that represents a field or property to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberListBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> is <see langword="null" />. -or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Reflection.FieldInfo.FieldType" /> or <see cref="P:System.Reflection.PropertyInfo.PropertyType" /> of the field or property that <paramref name="member" /> represents does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public MemberListBinding ListBind(MemberInfo member, ElementInit[] initializers) => Expression.ListBind(member, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBinding" /> where the member is a field or property.</summary>
        /// <param name="member">A <see cref="T:System.Reflection.MemberInfo" /> that represents a field or property to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberListBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> is <see langword="null" />. -or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Reflection.FieldInfo.FieldType" /> or <see cref="P:System.Reflection.PropertyInfo.PropertyType" /> of the field or property that <paramref name="member" /> represents does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public MemberListBinding ListBind(MemberInfo member, IEnumerable<ElementInit> initializers) => Expression.ListBind(member, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBinding" /> object based on a specified property accessor method.</summary>
        /// <param name="propertyAccessor">A <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberListBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.MemberInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> populated with the elements of <paramref name="initializers" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="propertyAccessor" /> is <see langword="null" />. -or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The <see cref="P:System.Reflection.PropertyInfo.PropertyType" /> of the property that the method represented by <paramref name="propertyAccessor" /> accesses does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public MemberListBinding ListBind(MethodInfo propertyAccessor, ElementInit[] initializers) => Expression.ListBind(propertyAccessor, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBinding" /> based on a specified property accessor method.</summary>
        /// <param name="propertyAccessor">A <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberListBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.MemberInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> populated with the elements of <paramref name="initializers" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="propertyAccessor" /> is <see langword="null" />. -or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The <see cref="P:System.Reflection.PropertyInfo.PropertyType" /> of the property that the method represented by <paramref name="propertyAccessor" /> accesses does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public MemberListBinding ListBind(MethodInfo propertyAccessor, IEnumerable<ElementInit> initializers) => Expression.ListBind(propertyAccessor, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses specified <see cref="T:System.Linq.Expressions.ElementInit" /> objects to initialize a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> and <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public ListInitExpression ListInit(NewExpression newExpression, ElementInit[] initializers) => Expression.ListInit(newExpression, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses specified <see cref="T:System.Linq.Expressions.ElementInit" /> objects to initialize a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> and <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public ListInitExpression ListInit(NewExpression newExpression, IEnumerable<ElementInit> initializers) => Expression.ListInit(newExpression, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses a method named "Add" to add elements to a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">There is no instance method named "Add" (case insensitive) declared in <paramref name="newExpression" />.Type or its base type.-or-The add method on <paramref name="newExpression" />.Type or its base type does not take exactly one argument.-or-The type represented by the <see cref="P:System.Linq.Expressions.Expression.Type" /> property of the first element of <paramref name="initializers" /> is not assignable to the argument type of the add method on <paramref name="newExpression" />.Type or its base type.-or-More than one argument-compatible method named "Add" (case-insensitive) exists on <paramref name="newExpression" />.Type and/or its base type.</exception>
        public ListInitExpression ListInit(NewExpression newExpression, Expression[] initializers) => Expression.ListInit(newExpression, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses a method named "Add" to add elements to a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">There is no instance method named "Add" (case insensitive) declared in <paramref name="newExpression" />.Type or its base type.-or-The add method on <paramref name="newExpression" />.Type or its base type does not take exactly one argument.-or-The type represented by the <see cref="P:System.Linq.Expressions.Expression.Type" /> property of the first element of <paramref name="initializers" /> is not assignable to the argument type of the add method on <paramref name="newExpression" />.Type or its base type.-or-More than one argument-compatible method named "Add" (case-insensitive) exists on <paramref name="newExpression" />.Type and/or its base type.</exception>
        public ListInitExpression ListInit(NewExpression newExpression, IEnumerable<Expression> initializers) => Expression.ListInit(newExpression, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses a specified method to add elements to a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfo" /> that represents an instance method that takes one argument, that adds an element to a collection.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.-or-
        ///               <paramref name="addMethod" /> is not <see langword="null" /> and it does not represent an instance method named "Add" (case insensitive) that takes exactly one argument.-or-
        ///               <paramref name="addMethod" /> is not <see langword="null" /> and the type represented by the <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="initializers" /> is not assignable to the argument type of the method that <paramref name="addMethod" /> represents.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="addMethod" /> is <see langword="null" /> and no instance method named "Add" that takes one type-compatible argument exists on <paramref name="newExpression" />.Type or its base type.</exception>
        public ListInitExpression ListInit(NewExpression newExpression, MethodInfo addMethod, Expression[] initializers) => Expression.ListInit(newExpression, addMethod, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses a specified method to add elements to a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfo" /> that represents an instance method named "Add" (case insensitive), that adds an element to a collection.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.-or-
        ///               <paramref name="addMethod" /> is not <see langword="null" /> and it does not represent an instance method named "Add" (case insensitive) that takes exactly one argument.-or-
        ///               <paramref name="addMethod" /> is not <see langword="null" /> and the type represented by the <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="initializers" /> is not assignable to the argument type of the method that <paramref name="addMethod" /> represents.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="addMethod" /> is <see langword="null" /> and no instance method named "Add" that takes one type-compatible argument exists on <paramref name="newExpression" />.Type or its base type.</exception>
        public ListInitExpression ListInit(NewExpression newExpression, MethodInfo addMethod, IEnumerable<Expression> initializers) => Expression.ListInit(newExpression, addMethod, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LoopExpression" /> with the given body.</summary>
        /// <param name="body">The body of the loop.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.LoopExpression" />.</returns>
        public LoopExpression Loop(Expression body) => Expression.Loop(body);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LoopExpression" /> with the given body and break target.</summary>
        /// <param name="body">The body of the loop.</param>
        /// <param name="break">The break target used by the loop body.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.LoopExpression" />.</returns>
        public LoopExpression Loop(Expression body, LabelTarget @break) => Expression.Loop(body, @break);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LoopExpression" /> with the given body.</summary>
        /// <param name="body">The body of the loop.</param>
        /// <param name="break">The break target used by the loop body.</param>
        /// <param name="continue">The continue target used by the loop body.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.LoopExpression" />.</returns>
        public LoopExpression Loop(Expression body, LabelTarget @break, LabelTarget @continue) => Expression.Loop(body, @break, @continue);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" />, given the left and right operands, by calling an appropriate factory method.</summary>
        /// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the left operand.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the right operand.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpression" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="binaryType" /> does not correspond to a binary expression node.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        public BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right) => Expression.MakeBinary(binaryType, left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" />, given the left operand, right operand and implementing method, by calling the appropriate factory method.</summary>
        /// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the left operand.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the right operand.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that specifies the implementing method.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpression" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="binaryType" /> does not correspond to a binary expression node.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        public BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, Boolean liftToNull, MethodInfo method) => Expression.MakeBinary(binaryType, left, right, liftToNull, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" />, given the left operand, right operand, implementing method and type conversion function, by calling the appropriate factory method.</summary>
        /// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the left operand.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the right operand.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that specifies the implementing method.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that represents a type conversion function. This parameter is used only if <paramref name="binaryType" /> is <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> or compound assignment..</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpression" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="binaryType" /> does not correspond to a binary expression node.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        public BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, Boolean liftToNull, MethodInfo method, LambdaExpression conversion) => Expression.MakeBinary(binaryType, left, right, liftToNull, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlock" /> representing a catch statement with the specified elements.</summary>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.Expression.Type" /> of <see cref="T:System.Exception" /> this <see cref="T:System.Linq.Expressions.CatchBlock" /> will handle.</param>
        /// <param name="variable">A <see cref="T:System.Linq.Expressions.ParameterExpression" /> representing a reference to the <see cref="T:System.Exception" /> object caught by this handler.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <param name="filter">The body of the <see cref="T:System.Exception" /> filter.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlock" />.</returns>
        public CatchBlock MakeCatchBlock(Type type, ParameterExpression variable, Expression body, Expression filter) => Expression.MakeCatchBlock(type, variable, body, filter);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" />.</summary>
        /// <param name="delegateType">The type of the delegate used by the <see cref="T:System.Runtime.CompilerServices.CallSite" />.</param>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="arguments">The arguments to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.DelegateType" />, <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" />, and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression[] arguments) => Expression.MakeDynamic(delegateType, binder, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" />.</summary>
        /// <param name="delegateType">The type of the delegate used by the <see cref="T:System.Runtime.CompilerServices.CallSite" />.</param>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="arguments">The arguments to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.DelegateType" />, <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" />, and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, IEnumerable<Expression> arguments) => Expression.MakeDynamic(delegateType, binder, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" /> and one argument.</summary>
        /// <param name="delegateType">The type of the delegate used by the <see cref="T:System.Runtime.CompilerServices.CallSite" />.</param>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="arg0">The argument to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.DelegateType" />, <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" />, and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0) => Expression.MakeDynamic(delegateType, binder, arg0);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" /> and two arguments.</summary>
        /// <param name="delegateType">The type of the delegate used by the <see cref="T:System.Runtime.CompilerServices.CallSite" />.</param>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="arg0">The first argument to the dynamic operation.</param>
        /// <param name="arg1">The second argument to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.DelegateType" />, <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" />, and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1) => Expression.MakeDynamic(delegateType, binder, arg0, arg1);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" /> and three arguments.</summary>
        /// <param name="delegateType">The type of the delegate used by the <see cref="T:System.Runtime.CompilerServices.CallSite" />.</param>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="arg0">The first argument to the dynamic operation.</param>
        /// <param name="arg1">The second argument to the dynamic operation.</param>
        /// <param name="arg2">The third argument to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.DelegateType" />, <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" />, and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2) => Expression.MakeDynamic(delegateType, binder, arg0, arg1, arg2);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DynamicExpression" /> that represents a dynamic operation bound by the provided <see cref="T:System.Runtime.CompilerServices.CallSiteBinder" /> and four arguments.</summary>
        /// <param name="delegateType">The type of the delegate used by the <see cref="T:System.Runtime.CompilerServices.CallSite" />.</param>
        /// <param name="binder">The runtime binder for the dynamic operation.</param>
        /// <param name="arg0">The first argument to the dynamic operation.</param>
        /// <param name="arg1">The second argument to the dynamic operation.</param>
        /// <param name="arg2">The third argument to the dynamic operation.</param>
        /// <param name="arg3">The fourth argument to the dynamic operation.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DynamicExpression" /> that has <see cref="P:System.Linq.Expressions.Expression.NodeType" /> equal to <see cref="F:System.Linq.Expressions.ExpressionType.Dynamic" /> and has the <see cref="P:System.Linq.Expressions.DynamicExpression.DelegateType" />, <see cref="P:System.Linq.Expressions.DynamicExpression.Binder" />, and <see cref="P:System.Linq.Expressions.DynamicExpression.Arguments" /> set to the specified values.</returns>
        public DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2, Expression arg3) => Expression.MakeDynamic(delegateType, binder, arg0, arg1, arg2, arg3);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a jump of the specified <see cref="T:System.Linq.Expressions.GotoExpressionKind" />. The value passed to the label upon jumping can also be specified.</summary>
        /// <param name="kind">The <see cref="T:System.Linq.Expressions.GotoExpressionKind" /> of the <see cref="T:System.Linq.Expressions.GotoExpression" />.</param>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to <paramref name="kind" />, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpression MakeGoto(GotoExpressionKind kind, LabelTarget target, Expression value, Type type) => Expression.MakeGoto(kind, target, value, type);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpression" /> that represents accessing an indexed property in an object.</summary>
        /// <param name="instance">The object to which the property belongs. It should be null if the property is <see langword="static" /> (<see langword="shared" /> in Visual Basic).</param>
        /// <param name="indexer">An <see cref="T:System.Linq.Expressions.Expression" /> representing the property to index.</param>
        /// <param name="arguments">An IEnumerable&lt;Expression&gt; (IEnumerable (Of Expression) in Visual Basic) that contains the arguments that will be used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpression" />.</returns>
        public IndexExpression MakeIndex(Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments) => Expression.MakeIndex(instance, indexer, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing either a field or a property.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the object that the member belongs to. This can be null for static members.</param>
        /// <param name="member">The <see cref="T:System.Reflection.MemberInfo" /> that describes the field or property to be accessed.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.MemberExpression" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.</exception>
        public MemberExpression MakeMemberAccess(Expression expression, MemberInfo member) => Expression.MakeMemberAccess(expression, member);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpression" /> representing a try block with the specified elements.</summary>
        /// <param name="type">The result type of the try expression. If null, bodh and all handlers must have identical type.</param>
        /// <param name="body">The body of the try block.</param>
        /// <param name="finally">The body of the finally block. Pass null if the try block has no finally block associated with it.</param>
        /// <param name="fault">The body of the fault block. Pass null if the try block has no fault block associated with it.</param>
        /// <param name="handlers">A collection of <see cref="T:System.Linq.Expressions.CatchBlock" />s representing the catch statements to be associated with the try block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpression" />.</returns>
        public TryExpression MakeTry(Type type, Expression body, Expression @finally, Expression fault, IEnumerable<CatchBlock> handlers) => Expression.MakeTry(type, body, @finally, fault, handlers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" />, given an operand, by calling the appropriate factory method.</summary>
        /// <param name="unaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of unary operation.</param>
        /// <param name="operand">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the operand.</param>
        /// <param name="type">The <see cref="T:System.Type" /> that specifies the type to be converted to (pass <see langword="null" /> if not applicable).</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.UnaryExpression" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="operand" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="unaryType" /> does not correspond to a unary expression node.</exception>
        public UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type) => Expression.MakeUnary(unaryType, operand, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" />, given an operand and implementing method, by calling the appropriate factory method.</summary>
        /// <param name="unaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of unary operation.</param>
        /// <param name="operand">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the operand.</param>
        /// <param name="type">The <see cref="T:System.Type" /> that specifies the type to be converted to (pass <see langword="null" /> if not applicable).</param>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.UnaryExpression" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="operand" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="unaryType" /> does not correspond to a unary expression node.</exception>
        public UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type, MethodInfo method) => Expression.MakeUnary(unaryType, operand, type, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that represents the recursive initialization of members of a field or property.</summary>
        /// <param name="member">The <see cref="T:System.Reflection.MemberInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
        /// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the field or property that <paramref name="member" /> represents.</exception>
        public MemberMemberBinding MemberBind(MemberInfo member, MemberBinding[] bindings) => Expression.MemberBind(member, bindings);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that represents the recursive initialization of members of a field or property.</summary>
        /// <param name="member">The <see cref="T:System.Reflection.MemberInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
        /// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the field or property that <paramref name="member" /> represents.</exception>
        public MemberMemberBinding MemberBind(MemberInfo member, IEnumerable<MemberBinding> bindings) => Expression.MemberBind(member, bindings);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that represents the recursive initialization of members of a member that is accessed by using a property accessor method.</summary>
        /// <param name="propertyAccessor">The <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBinding" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="propertyAccessor" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the property accessed by the method that <paramref name="propertyAccessor" /> represents.</exception>
        public MemberMemberBinding MemberBind(MethodInfo propertyAccessor, MemberBinding[] bindings) => Expression.MemberBind(propertyAccessor, bindings);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that represents the recursive initialization of members of a member that is accessed by using a property accessor method.</summary>
        /// <param name="propertyAccessor">The <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBinding" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="propertyAccessor" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the property accessed by the method that <paramref name="propertyAccessor" /> represents.</exception>
        public MemberMemberBinding MemberBind(MethodInfo propertyAccessor, IEnumerable<MemberBinding> bindings) => Expression.MemberBind(propertyAccessor, bindings);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberInitExpression" />.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.MemberInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberInitExpression.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberInit" /> and the <see cref="P:System.Linq.Expressions.MemberInitExpression.NewExpression" /> and <see cref="P:System.Linq.Expressions.MemberInitExpression.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type that <paramref name="newExpression" />.Type represents.</exception>
        public MemberInitExpression MemberInit(NewExpression newExpression, MemberBinding[] bindings) => Expression.MemberInit(newExpression, bindings);

        /// <summary>Represents an expression that creates a new object and initializes a property of the object.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.MemberInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberInitExpression.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberInit" /> and the <see cref="P:System.Linq.Expressions.MemberInitExpression.NewExpression" /> and <see cref="P:System.Linq.Expressions.MemberInitExpression.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type that <paramref name="newExpression" />.Type represents.</exception>
        public MemberInitExpression MemberInit(NewExpression newExpression, IEnumerable<MemberBinding> bindings) => Expression.MemberInit(newExpression, bindings);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic remainder operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Modulo" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The modulus operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Modulo(Expression left, Expression right) => Expression.Modulo(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic remainder operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Modulo" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the modulus operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Modulo(Expression left, Expression right, MethodInfo method) => Expression.Modulo(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a remainder assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ModuloAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression ModuloAssign(Expression left, Expression right) => Expression.ModuloAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a remainder assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ModuloAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression ModuloAssign(Expression left, Expression right, MethodInfo method) => Expression.ModuloAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a remainder assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ModuloAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression ModuloAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.ModuloAssign(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic multiplication operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Multiply" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Multiply(Expression left, Expression right) => Expression.Multiply(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic multiplication operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Multiply" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Multiply(Expression left, Expression right, MethodInfo method) => Expression.Multiply(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a multiplication assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression MultiplyAssign(Expression left, Expression right) => Expression.MultiplyAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a multiplication assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression MultiplyAssign(Expression left, Expression right, MethodInfo method) => Expression.MultiplyAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a multiplication assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression MultiplyAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.MultiplyAssign(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a multiplication assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression MultiplyAssignChecked(Expression left, Expression right) => Expression.MultiplyAssignChecked(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a multiplication assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression MultiplyAssignChecked(Expression left, Expression right, MethodInfo method) => Expression.MultiplyAssignChecked(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a multiplication assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression MultiplyAssignChecked(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.MultiplyAssignChecked(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic multiplication operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression MultiplyChecked(Expression left, Expression right) => Expression.MultiplyChecked(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic multiplication operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression MultiplyChecked(Expression left, Expression right, MethodInfo method) => Expression.MultiplyChecked(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an arithmetic negation operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Negate" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The unary minus operator is not defined for <paramref name="expression" />.Type.</exception>
        public UnaryExpression Negate(Expression expression) => Expression.Negate(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an arithmetic negation operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Negate" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the unary minus operator is not defined for <paramref name="expression" />.Type.-or-
        ///               <paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
        public UnaryExpression Negate(Expression expression, MethodInfo method) => Expression.Negate(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an arithmetic negation operation that has overflow checking.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NegateChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The unary minus operator is not defined for <paramref name="expression" />.Type.</exception>
        public UnaryExpression NegateChecked(Expression expression) => Expression.NegateChecked(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an arithmetic negation operation that has overflow checking. The implementing method can be specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NegateChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the unary minus operator is not defined for <paramref name="expression" />.Type.-or-
        ///               <paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
        public UnaryExpression NegateChecked(Expression expression, MethodInfo method) => Expression.NegateChecked(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor that takes no arguments.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The constructor that <paramref name="constructor" /> represents has at least one parameter.</exception>
        public NewExpression New(ConstructorInfo constructor) => Expression.New(constructor);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the parameterless constructor of the specified type.</summary>
        /// <param name="type">A <see cref="T:System.Type" /> that has a constructor that takes no arguments.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property set to the <see cref="T:System.Reflection.ConstructorInfo" /> that represents the constructor without parameters for the specified type.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The type that <paramref name="type" /> represents does not have a constructor without parameters.</exception>
        public NewExpression New(Type type) => Expression.New(type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor with the specified arguments.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> and <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.-or-An element of <paramref name="arguments" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The length of <paramref name="arguments" /> does match the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.</exception>
        public NewExpression New(ConstructorInfo constructor, Expression[] arguments) => Expression.New(constructor, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor with the specified arguments.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> and <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.-or-An element of <paramref name="arguments" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <paramref name="arguments" /> parameter does not contain the same number of elements as the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.</exception>
        public NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments) => Expression.New(constructor, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> collection.</param>
        /// <param name="members">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Reflection.MemberInfo" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Members" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" />, <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> and <see cref="P:System.Linq.Expressions.NewExpression.Members" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.-or-An element of <paramref name="arguments" /> is <see langword="null" />.-or-An element of <paramref name="members" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <paramref name="arguments" /> parameter does not contain the same number of elements as the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.-or-The <paramref name="members" /> parameter does not have the same number of elements as <paramref name="arguments" />.-or-An element of <paramref name="arguments" /> has a <see cref="P:System.Linq.Expressions.Expression.Type" /> property that represents a type that is not assignable to the type of the member that is represented by the corresponding element of <paramref name="members" />.</exception>
        public NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments, IEnumerable<MemberInfo> members) => Expression.New(constructor, arguments, members);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified as an array.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> collection.</param>
        /// <param name="members">An array of <see cref="T:System.Reflection.MemberInfo" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Members" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" />, <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> and <see cref="P:System.Linq.Expressions.NewExpression.Members" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.-or-An element of <paramref name="arguments" /> is <see langword="null" />.-or-An element of <paramref name="members" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <paramref name="arguments" /> parameter does not contain the same number of elements as the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.-or-The <paramref name="members" /> parameter does not have the same number of elements as <paramref name="arguments" />.-or-An element of <paramref name="arguments" /> has a <see cref="P:System.Linq.Expressions.Expression.Type" /> property that represents a type that is not assignable to the type of the member that is represented by the corresponding element of <paramref name="members" />.</exception>
        public NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments, MemberInfo[] members) => Expression.New(constructor, arguments, members);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that represents creating an array that has a specified rank.</summary>
        /// <param name="type">A <see cref="T:System.Type" /> that represents the element type of the array.</param>
        /// <param name="bounds">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayBounds" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="bounds" /> is <see langword="null" />.-or-An element of <paramref name="bounds" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="bounds" /> does not represent an integral type.</exception>
        public NewArrayExpression NewArrayBounds(Type type, Expression[] bounds) => Expression.NewArrayBounds(type, bounds);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that represents creating an array that has a specified rank.</summary>
        /// <param name="type">A <see cref="T:System.Type" /> that represents the element type of the array.</param>
        /// <param name="bounds">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayBounds" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="bounds" /> is <see langword="null" />.-or-An element of <paramref name="bounds" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="bounds" /> does not represent an integral type.</exception>
        public NewArrayExpression NewArrayBounds(Type type, IEnumerable<Expression> bounds) => Expression.NewArrayBounds(type, bounds);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
        /// <param name="type">A <see cref="T:System.Type" /> that represents the element type of the array.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayInit" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="initializers" /> is <see langword="null" />.-or-An element of <paramref name="initializers" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="initializers" /> represents a type that is not assignable to the type <paramref name="type" />.</exception>
        public NewArrayExpression NewArrayInit(Type type, Expression[] initializers) => Expression.NewArrayInit(type, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
        /// <param name="type">A <see cref="T:System.Type" /> that represents the element type of the array.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayInit" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="initializers" /> is <see langword="null" />.-or-An element of <paramref name="initializers" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="initializers" /> represents a type that is not assignable to the type that <paramref name="type" /> represents.</exception>
        public NewArrayExpression NewArrayInit(Type type, IEnumerable<Expression> initializers) => Expression.NewArrayInit(type, initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a bitwise complement operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Not" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The unary not operator is not defined for <paramref name="expression" />.Type.</exception>
        public UnaryExpression Not(Expression expression) => Expression.Not(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a bitwise complement operation. The implementing method can be specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Not" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the unary not operator is not defined for <paramref name="expression" />.Type.-or-
        ///               <paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
        public UnaryExpression Not(Expression expression, MethodInfo method) => Expression.Not(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an inequality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The inequality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression NotEqual(Expression left, Expression right) => Expression.NotEqual(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an inequality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the inequality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression NotEqual(Expression left, Expression right, Boolean liftToNull, MethodInfo method) => Expression.NotEqual(left, right, liftToNull, method);

        /// <summary>Returns the expression representing the ones complement.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" />.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpression" />.</returns>
        public UnaryExpression OnesComplement(Expression expression) => Expression.OnesComplement(expression);

        /// <summary>Returns the expression representing the ones complement.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpression" />.</returns>
        public UnaryExpression OnesComplement(Expression expression, MethodInfo method) => Expression.OnesComplement(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise <see langword="OR" /> operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Or" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The bitwise <see langword="OR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Or(Expression left, Expression right) => Expression.Or(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise <see langword="OR" /> operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Or" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the bitwise <see langword="OR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Or(Expression left, Expression right, MethodInfo method) => Expression.Or(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise OR assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression OrAssign(Expression left, Expression right) => Expression.OrAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise OR assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression OrAssign(Expression left, Expression right, MethodInfo method) => Expression.OrAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise OR assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression OrAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.OrAssign(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a conditional <see langword="OR" /> operation that evaluates the second operand only if the first operand evaluates to <see langword="false" />.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrElse" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The bitwise <see langword="OR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
        public BinaryExpression OrElse(Expression left, Expression right) => Expression.OrElse(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a conditional <see langword="OR" /> operation that evaluates the second operand only if the first operand evaluates to <see langword="false" />.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrElse" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the bitwise <see langword="OR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="method" /> is <see langword="null" /> and <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
        public BinaryExpression OrElse(Expression left, Expression right, MethodInfo method) => Expression.OrElse(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ParameterExpression" /> node that can be used to identify a parameter or a variable in an expression tree.</summary>
        /// <param name="type">The type of the parameter or variable.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ParameterExpression" /> node with the specified name and type.</returns>
        public ParameterExpression Parameter(Type type) => Expression.Parameter(type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ParameterExpression" /> node that can be used to identify a parameter or a variable in an expression tree.</summary>
        /// <param name="type">The type of the parameter or variable.</param>
        /// <param name="name">The name of the parameter or variable, used for debugging or printing purpose only.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ParameterExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Parameter" /> and the <see cref="P:System.Linq.Expressions.Expression.Type" /> and <see cref="P:System.Linq.Expressions.ParameterExpression.Name" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> is <see langword="null" />.</exception>
        public ParameterExpression Parameter(Type type, String name) => Expression.Parameter(type, name);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the assignment of the expression followed by a subsequent decrement by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the resultant expression.</returns>
        public UnaryExpression PostDecrementAssign(Expression expression) => Expression.PostDecrementAssign(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the assignment of the expression followed by a subsequent decrement by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the resultant expression.</returns>
        public UnaryExpression PostDecrementAssign(Expression expression, MethodInfo method) => Expression.PostDecrementAssign(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the assignment of the expression followed by a subsequent increment by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the resultant expression.</returns>
        public UnaryExpression PostIncrementAssign(Expression expression) => Expression.PostIncrementAssign(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the assignment of the expression followed by a subsequent increment by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the resultant expression.</returns>
        public UnaryExpression PostIncrementAssign(Expression expression, MethodInfo method) => Expression.PostIncrementAssign(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents raising a number to a power.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Power" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The exponentiation operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="left" />.Type and/or <paramref name="right" />.Type are not <see cref="T:System.Double" />.</exception>
        public BinaryExpression Power(Expression left, Expression right) => Expression.Power(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents raising a number to a power.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Power" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the exponentiation operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="method" /> is <see langword="null" /> and <paramref name="left" />.Type and/or <paramref name="right" />.Type are not <see cref="T:System.Double" />.</exception>
        public BinaryExpression Power(Expression left, Expression right, MethodInfo method) => Expression.Power(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents raising an expression to a power and assigning the result back to the expression.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.PowerAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression PowerAssign(Expression left, Expression right) => Expression.PowerAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents raising an expression to a power and assigning the result back to the expression.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.PowerAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression PowerAssign(Expression left, Expression right, MethodInfo method) => Expression.PowerAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents raising an expression to a power and assigning the result back to the expression.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.PowerAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression PowerAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.PowerAssign(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that decrements the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the resultant expression.</returns>
        public UnaryExpression PreDecrementAssign(Expression expression) => Expression.PreDecrementAssign(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that decrements the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the resultant expression.</returns>
        public UnaryExpression PreDecrementAssign(Expression expression, MethodInfo method) => Expression.PreDecrementAssign(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that increments the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the resultant expression.</returns>
        public UnaryExpression PreIncrementAssign(Expression expression) => Expression.PreIncrementAssign(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that increments the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the resultant expression.</returns>
        public UnaryExpression PreIncrementAssign(Expression expression, MethodInfo method) => Expression.PreIncrementAssign(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a property.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> whose <see cref="P:System.Linq.Expressions.Expression.Type" /> contains a property named <paramref name="propertyName" />. This can be <see langword="null" /> for static properties.</param>
        /// <param name="propertyName">The name of a property to be accessed.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" />, the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property set to <paramref name="expression" />, and the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property denoted by <paramref name="propertyName" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="propertyName" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">No property named <paramref name="propertyName" /> is defined in <paramref name="expression" />.Type or its base types.</exception>
        public MemberExpression Property(Expression expression, String propertyName) => Expression.Property(expression, propertyName);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a property.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property equal to. This can be null for static properties.</param>
        /// <param name="property">The <see cref="T:System.Reflection.PropertyInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" /> and the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> and <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="property" /> is <see langword="null" />.-or-The property that <paramref name="property" /> represents is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic) and <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="expression" />.Type is not assignable to the declaring type of the property that <paramref name="property" /> represents.</exception>
        public MemberExpression Property(Expression expression, PropertyInfo property) => Expression.Property(expression, property);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a property by using a property accessor method.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property equal to. This can be null for static properties.</param>
        /// <param name="propertyAccessor">The <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" />, the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property set to <paramref name="expression" /> and the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="propertyAccessor" /> is <see langword="null" />.-or-The method that <paramref name="propertyAccessor" /> represents is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic) and <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="expression" />.Type is not assignable to the declaring type of the method represented by <paramref name="propertyAccessor" />.-or-The method that <paramref name="propertyAccessor" /> represents is not a property accessor method.</exception>
        public MemberExpression Property(Expression expression, MethodInfo propertyAccessor) => Expression.Property(expression, propertyAccessor);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> accessing a property.</summary>
        /// <param name="expression">The containing object of the property. This can be null for static properties.</param>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.Expression.Type" /> that contains the property.</param>
        /// <param name="propertyName">The property to be accessed.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.MemberExpression" />.</returns>
        public MemberExpression Property(Expression expression, Type type, String propertyName) => Expression.Property(expression, type, propertyName);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpression" /> representing the access to an indexed property.</summary>
        /// <param name="instance">The object to which the property belongs. If the property is static/shared, it must be null.</param>
        /// <param name="propertyName">The name of the indexer.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects that are used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpression" />.</returns>
        public IndexExpression Property(Expression instance, String propertyName, Expression[] arguments) => Expression.Property(instance, propertyName, arguments);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpression" /> representing the access to an indexed property.</summary>
        /// <param name="instance">The object to which the property belongs. If the property is static/shared, it must be null.</param>
        /// <param name="indexer">The <see cref="T:System.Reflection.PropertyInfo" /> that represents the property to index.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects that are used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpression" />.</returns>
        public IndexExpression Property(Expression instance, PropertyInfo indexer, Expression[] arguments) => Expression.Property(instance, indexer, arguments);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpression" /> representing the access to an indexed property.</summary>
        /// <param name="instance">The object to which the property belongs. If the property is static/shared, it must be null.</param>
        /// <param name="indexer">The <see cref="T:System.Reflection.PropertyInfo" /> that represents the property to index.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Linq.Expressions.Expression" /> objects that are used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpression" />.</returns>
        public IndexExpression Property(Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments) => Expression.Property(instance, indexer, arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a property or field.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> whose <see cref="P:System.Linq.Expressions.Expression.Type" /> contains a property or field named <paramref name="propertyOrFieldName" />. This can be null for static members.</param>
        /// <param name="propertyOrFieldName">The name of a property or field to be accessed.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" />, the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property set to <paramref name="expression" />, and the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> or <see cref="T:System.Reflection.FieldInfo" /> that represents the property or field denoted by <paramref name="propertyOrFieldName" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="propertyOrFieldName" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">No property or field named <paramref name="propertyOrFieldName" /> is defined in <paramref name="expression" />.Type or its base types.</exception>
        public MemberExpression PropertyOrField(Expression expression, String propertyOrFieldName) => Expression.PropertyOrField(expression, propertyOrFieldName);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an expression that has a constant value of type <see cref="T:System.Linq.Expressions.Expression" />.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Quote" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        public UnaryExpression Quote(Expression expression) => Expression.Quote(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a reference equality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression ReferenceEqual(Expression left, Expression right) => Expression.ReferenceEqual(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a reference inequality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression ReferenceNotEqual(Expression left, Expression right) => Expression.ReferenceNotEqual(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a rethrowing of an exception.</summary>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a rethrowing of an exception.</returns>
        public UnaryExpression Rethrow() => Expression.Rethrow();

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a rethrowing of an exception with a given type.</summary>
        /// <param name="type">The new <see cref="T:System.Type" /> of the expression.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a rethrowing of an exception.</returns>
        public UnaryExpression Rethrow(Type type) => Expression.Rethrow(type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a return statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Return, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpression Return(LabelTarget target) => Expression.Return(target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a return statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Return, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to <paramref name="type" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpression Return(LabelTarget target, Type type) => Expression.Return(target, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a return statement. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpression Return(LabelTarget target, Expression value) => Expression.Return(target, value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpression" /> representing a return statement with the specified type. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTarget" /> that the <see cref="T:System.Linq.Expressions.GotoExpression" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpression" /> with <see cref="P:System.Linq.Expressions.GotoExpression.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpression.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.Expression.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpression Return(LabelTarget target, Expression value, Type type) => Expression.Return(target, value, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise right-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The right-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression RightShift(Expression left, Expression right) => Expression.RightShift(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise right-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the right-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression RightShift(Expression left, Expression right, MethodInfo method) => Expression.RightShift(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise right-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression RightShiftAssign(Expression left, Expression right) => Expression.RightShiftAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise right-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression RightShiftAssign(Expression left, Expression right, MethodInfo method) => Expression.RightShiftAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise right-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression RightShiftAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.RightShiftAssign(left, right, method, conversion);

        /// <summary>Creates an instance of <see cref="T:System.Linq.Expressions.RuntimeVariablesExpression" />.</summary>
        /// <param name="variables">An array of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.RuntimeVariablesExpression.Variables" /> collection.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.RuntimeVariablesExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RuntimeVariables" /> and the <see cref="P:System.Linq.Expressions.RuntimeVariablesExpression.Variables" /> property set to the specified value.</returns>
        public RuntimeVariablesExpression RuntimeVariables(ParameterExpression[] variables) => Expression.RuntimeVariables(variables);

        /// <summary>Creates an instance of <see cref="T:System.Linq.Expressions.RuntimeVariablesExpression" />.</summary>
        /// <param name="variables">A collection of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.RuntimeVariablesExpression.Variables" /> collection.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.RuntimeVariablesExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RuntimeVariables" /> and the <see cref="P:System.Linq.Expressions.RuntimeVariablesExpression.Variables" /> property set to the specified value.</returns>
        public RuntimeVariablesExpression RuntimeVariables(IEnumerable<ParameterExpression> variables) => Expression.RuntimeVariables(variables);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic subtraction operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Subtract" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Subtract(Expression left, Expression right) => Expression.Subtract(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic subtraction operation that does not have overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Subtract" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression Subtract(Expression left, Expression right, MethodInfo method) => Expression.Subtract(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a subtraction assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression SubtractAssign(Expression left, Expression right) => Expression.SubtractAssign(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a subtraction assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression SubtractAssign(Expression left, Expression right, MethodInfo method) => Expression.SubtractAssign(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a subtraction assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression SubtractAssign(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.SubtractAssign(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a subtraction assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        public BinaryExpression SubtractAssignChecked(Expression left, Expression right) => Expression.SubtractAssignChecked(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a subtraction assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        public BinaryExpression SubtractAssignChecked(Expression left, Expression right, MethodInfo method) => Expression.SubtractAssignChecked(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a subtraction assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpression SubtractAssignChecked(Expression left, Expression right, MethodInfo method, LambdaExpression conversion) => Expression.SubtractAssignChecked(left, right, method, conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic subtraction operation that has overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression SubtractChecked(Expression left, Expression right) => Expression.SubtractChecked(left, right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic subtraction operation that has overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpression SubtractChecked(Expression left, Expression right, MethodInfo method) => Expression.SubtractChecked(left, right, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpression" /> that represents a <see langword="switch" /> statement without a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpression" />.</returns>
        public SwitchExpression Switch(Expression switchValue, SwitchCase[] cases) => Expression.Switch(switchValue, cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpression" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpression" />.</returns>
        public SwitchExpression Switch(Expression switchValue, Expression defaultBody, SwitchCase[] cases) => Expression.Switch(switchValue, defaultBody, cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpression" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpression" />.</returns>
        public SwitchExpression Switch(Expression switchValue, Expression defaultBody, MethodInfo comparison, SwitchCase[] cases) => Expression.Switch(switchValue, defaultBody, comparison, cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpression" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpression" />.</returns>
        public SwitchExpression Switch(Expression switchValue, Expression defaultBody, MethodInfo comparison, IEnumerable<SwitchCase> cases) => Expression.Switch(switchValue, defaultBody, comparison, cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpression" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="type">The result type of the switch.</param>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpression" />.</returns>
        public SwitchExpression Switch(Type type, Expression switchValue, Expression defaultBody, MethodInfo comparison, IEnumerable<SwitchCase> cases) => Expression.Switch(type, switchValue, defaultBody, comparison, cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpression" /> that represents a <see langword="switch" /> statement that has a default case..</summary>
        /// <param name="type">The result type of the switch.</param>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpression" />.</returns>
        public SwitchExpression Switch(Type type, Expression switchValue, Expression defaultBody, MethodInfo comparison, SwitchCase[] cases) => Expression.Switch(type, switchValue, defaultBody, comparison, cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchCase" /> for use in a <see cref="T:System.Linq.Expressions.SwitchExpression" />.</summary>
        /// <param name="body">The body of the case.</param>
        /// <param name="testValues">The test values of the case.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchCase" />.</returns>
        public SwitchCase SwitchCase(Expression body, Expression[] testValues) => Expression.SwitchCase(body, testValues);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchCase" /> object to be used in a <see cref="T:System.Linq.Expressions.SwitchExpression" /> object.</summary>
        /// <param name="body">The body of the case.</param>
        /// <param name="testValues">The test values of the case.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchCase" />.</returns>
        public SwitchCase SwitchCase(Expression body, IEnumerable<Expression> testValues) => Expression.SwitchCase(body, testValues);

        /// <summary>Creates an instance of <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" />.</summary>
        /// <param name="fileName">A <see cref="T:System.String" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.FileName" /> equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" /> that has the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.FileName" /> property set to the specified value.</returns>
        public SymbolDocumentInfo SymbolDocument(String fileName) => Expression.SymbolDocument(fileName);

        /// <summary>Creates an instance of <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" />.</summary>
        /// <param name="fileName">A <see cref="T:System.String" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.FileName" /> equal to.</param>
        /// <param name="language">A <see cref="T:System.Guid" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.Language" /> equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" /> that has the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.FileName" /> and <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.Language" /> properties set to the specified value.</returns>
        public SymbolDocumentInfo SymbolDocument(String fileName, Guid language) => Expression.SymbolDocument(fileName, language);

        /// <summary>Creates an instance of <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" />.</summary>
        /// <param name="fileName">A <see cref="T:System.String" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.FileName" /> equal to.</param>
        /// <param name="language">A <see cref="T:System.Guid" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.Language" /> equal to.</param>
        /// <param name="languageVendor">A <see cref="T:System.Guid" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.LanguageVendor" /> equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" /> that has the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.FileName" /> and <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.Language" /> and <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.LanguageVendor" /> properties set to the specified value.</returns>
        public SymbolDocumentInfo SymbolDocument(String fileName, Guid language, Guid languageVendor) => Expression.SymbolDocument(fileName, language, languageVendor);

        /// <summary>Creates an instance of <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" />.</summary>
        /// <param name="fileName">A <see cref="T:System.String" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.FileName" /> equal to.</param>
        /// <param name="language">A <see cref="T:System.Guid" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.Language" /> equal to.</param>
        /// <param name="languageVendor">A <see cref="T:System.Guid" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.LanguageVendor" /> equal to.</param>
        /// <param name="documentType">A <see cref="T:System.Guid" /> to set the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.DocumentType" /> equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.SymbolDocumentInfo" /> that has the <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.FileName" /> and <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.Language" /> and <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.LanguageVendor" /> and <see cref="P:System.Linq.Expressions.SymbolDocumentInfo.DocumentType" /> properties set to the specified value.</returns>
        public SymbolDocumentInfo SymbolDocument(String fileName, Guid language, Guid languageVendor, Guid documentType) => Expression.SymbolDocument(fileName, language, languageVendor, documentType);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a throwing of an exception.</summary>
        /// <param name="value">An <see cref="T:System.Linq.Expressions.Expression" />.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the exception.</returns>
        public UnaryExpression Throw(Expression value) => Expression.Throw(value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a throwing of an exception with a given type.</summary>
        /// <param name="value">An <see cref="T:System.Linq.Expressions.Expression" />.</param>
        /// <param name="type">The new <see cref="T:System.Type" /> of the expression.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents the exception.</returns>
        public UnaryExpression Throw(Expression value, Type type) => Expression.Throw(value, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpression" /> representing a try block with any number of catch statements and neither a fault nor finally block.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="handlers">The array of zero or more <see cref="T:System.Linq.Expressions.CatchBlock" /> expressions representing the catch statements to be associated with the try block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpression" />.</returns>
        public TryExpression TryCatch(Expression body, CatchBlock[] handlers) => Expression.TryCatch(body, handlers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpression" /> representing a try block with any number of catch statements and a finally block.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="finally">The body of the finally block.</param>
        /// <param name="handlers">The array of zero or more <see cref="T:System.Linq.Expressions.CatchBlock" /> expressions representing the catch statements to be associated with the try block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpression" />.</returns>
        public TryExpression TryCatchFinally(Expression body, Expression @finally, CatchBlock[] handlers) => Expression.TryCatchFinally(body, @finally, handlers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpression" /> representing a try block with a fault block and no catch statements.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="fault">The body of the fault block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpression" />.</returns>
        public TryExpression TryFault(Expression body, Expression fault) => Expression.TryFault(body, fault);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpression" /> representing a try block with a finally block and no catch statements.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="finally">The body of the finally block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpression" />.</returns>
        public TryExpression TryFinally(Expression body, Expression @finally) => Expression.TryFinally(body, @finally);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an explicit reference or boxing conversion where <see langword="null" /> is supplied if the conversion fails.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.TypeAs" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        public UnaryExpression TypeAs(Expression expression, Type type) => Expression.TypeAs(expression, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TypeBinaryExpression" /> that compares run-time type identity.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="T:System.Linq.Expressions.Expression" /> property equal to.</param>
        /// <param name="type">A <see cref="P:System.Linq.Expressions.Expression.Type" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpression.TypeOperand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.TypeBinaryExpression" /> for which the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property is equal to <see cref="M:System.Linq.Expressions.Expression.TypeEqual(System.Linq.Expressions.Expression,System.Type)" /> and for which the <see cref="T:System.Linq.Expressions.Expression" /> and <see cref="P:System.Linq.Expressions.TypeBinaryExpression.TypeOperand" /> properties are set to the specified values.</returns>
        public TypeBinaryExpression TypeEqual(Expression expression, Type type) => Expression.TypeEqual(expression, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TypeBinaryExpression" />.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpression.Expression" /> property equal to.</param>
        /// <param name="type">A <see cref="P:System.Linq.Expressions.Expression.Type" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpression.TypeOperand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.TypeBinaryExpression" /> for which the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property is equal to <see cref="F:System.Linq.Expressions.ExpressionType.TypeIs" /> and for which the <see cref="P:System.Linq.Expressions.TypeBinaryExpression.Expression" /> and <see cref="P:System.Linq.Expressions.TypeBinaryExpression.TypeOperand" /> properties are set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        public TypeBinaryExpression TypeIs(Expression expression, Type type) => Expression.TypeIs(expression, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a unary plus operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.UnaryPlus" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The unary plus operator is not defined for <paramref name="expression" />.Type.</exception>
        public UnaryExpression UnaryPlus(Expression expression) => Expression.UnaryPlus(expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a unary plus operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.UnaryPlus" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the unary plus operator is not defined for <paramref name="expression" />.Type.-or-
        ///               <paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
        public UnaryExpression UnaryPlus(Expression expression, MethodInfo method) => Expression.UnaryPlus(expression, method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an explicit unboxing.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to unbox.</param>
        /// <param name="type">The new <see cref="T:System.Type" /> of the expression.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpression" />.</returns>
        public UnaryExpression Unbox(Expression expression, Type type) => Expression.Unbox(expression, type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ParameterExpression" /> node that can be used to identify a parameter or a variable in an expression tree.</summary>
        /// <param name="type">The type of the parameter or variable.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ParameterExpression" /> node with the specified name and type</returns>
        public ParameterExpression Variable(Type type) => Expression.Variable(type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ParameterExpression" /> node that can be used to identify a parameter or a variable in an expression tree.</summary>
        /// <param name="type">The type of the parameter or variable.</param>
        /// <param name="name">The name of the parameter or variable. This name is used for debugging or printing purpose only.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ParameterExpression" /> node with the specified name and type.</returns>
        public ParameterExpression Variable(Type type, String name) => Expression.Variable(type, name);

    }
}