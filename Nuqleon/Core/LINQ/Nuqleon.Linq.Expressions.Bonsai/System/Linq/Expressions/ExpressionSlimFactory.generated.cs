// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Factory for slim expression trees that uses the default factory methods on <see cref="T:System.Linq.Expressions.ExpressionSlim" />.
    /// </summary>
    public sealed class ExpressionSlimFactory : IExpressionSlimFactory
    {
        /// <summary>
        /// Gets the singleton instance of the factory.
        /// </summary>
        public static readonly IExpressionSlimFactory Instance = new ExpressionSlimFactory();

        private ExpressionSlimFactory() {}

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic addition operation that does not have overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Add" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Add(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Add(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic addition operation that does not have overflow checking. The implementing method can be specified.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Add" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Add(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.Add(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim AddAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.AddAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim AddAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.AddAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim AddAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.AddAssign(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim AddAssignChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.AddAssignChecked(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim AddAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.AddAssignChecked(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim AddAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.AddAssignChecked(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic addition operation that has overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim AddChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.AddChecked(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic addition operation that has overflow checking. The implementing method can be specified.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim AddChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.AddChecked(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="AND" /> operation.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.And" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The bitwise <see langword="AND" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim And(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.And(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="AND" /> operation. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.And" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the bitwise <see langword="AND" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim And(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.And(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a conditional <see langword="AND" /> operation that evaluates the second operand only if the first operand evaluates to <see langword="true" />.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAlso" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The bitwise <see langword="AND" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
        public BinaryExpressionSlim AndAlso(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.AndAlso(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a conditional <see langword="AND" /> operation that evaluates the second operand only if the first operand is resolved to true. The implementing method can be specified.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAlso" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the bitwise <see langword="AND" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="method" /> is <see langword="null" /> and <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
        public BinaryExpressionSlim AndAlso(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.AndAlso(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise AND assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim AndAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.AndAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise AND assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim AndAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.AndAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise AND assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim AndAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.AndAssign(left, right, method, conversion);
        }

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> to access an array.</summary>
        /// <param name="array">An expression representing the array to index.</param>
        /// <param name="indexes">An array that contains expressions used to index the array.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        public IndexExpressionSlim ArrayAccess(ExpressionSlim array, ExpressionSlim[] indexes)
        {
            return ExpressionSlim.ArrayAccess(array, indexes);
        }

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> to access a multidimensional array.</summary>
        /// <param name="array">An expression that represents the multidimensional array.</param>
        /// <param name="indexes">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing expressions used to index the array.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        public IndexExpressionSlim ArrayAccess(ExpressionSlim array, IEnumerable<ExpressionSlim> indexes)
        {
            return ExpressionSlim.ArrayAccess(array, indexes);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents applying an array index operator to an array of rank one.</summary>
        /// <param name="array">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="index">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ArrayIndex" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> or <paramref name="index" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" />.Type does not represent an array type.-or-
        ///               <paramref name="array" />.Type represents an array type whose rank is not 1.-or-
        ///               <paramref name="index" />.Type does not represent the <see cref="T:System.Int32" /> type.</exception>
        public BinaryExpressionSlim ArrayIndex(ExpressionSlim array, ExpressionSlim index)
        {
            return ExpressionSlim.ArrayIndex(array, index);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an expression for obtaining the length of a one-dimensional array.</summary>
        /// <param name="array">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ArrayLength" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to <paramref name="array" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" />.Type does not represent an array type.</exception>
        public UnaryExpressionSlim ArrayLength(ExpressionSlim array)
        {
            return ExpressionSlim.ArrayLength(array);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Assign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim Assign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Assign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberAssignmentSlim" /> that represents the initialization of a field or property.</summary>
        /// <param name="member">A <see cref="T:System.Reflection.MemberInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberAssignmentSlim.Expression" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberAssignmentSlim" /> that has <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> equal to <see cref="F:System.Linq.Expressions.MemberBindingType.Assignment" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberAssignmentSlim.Expression" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> or <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The property represented by <paramref name="member" /> does not have a <see langword="set" /> accessor.-or-
        ///               <paramref name="expression" />.Type is not assignable to the type of the field or property that <paramref name="member" /> represents.</exception>
        public MemberAssignmentSlim Bind(MemberInfoSlim member, ExpressionSlim expression)
        {
            return ExpressionSlim.Bind(member, expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given expressions and has no variables.</summary>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(ExpressionSlim[] expressions)
        {
            return ExpressionSlim.Block(expressions);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given expressions and has no variables.</summary>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(IEnumerable<ExpressionSlim> expressions)
        {
            return ExpressionSlim.Block(expressions);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains two expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(ExpressionSlim arg0, ExpressionSlim arg1)
        {
            return ExpressionSlim.Block(arg0, arg1);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given expressions, has no variables and has specific result type.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(TypeSlim type, ExpressionSlim[] expressions)
        {
            return ExpressionSlim.Block(type, expressions);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given expressions, has no variables and has specific result type.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(TypeSlim type, IEnumerable<ExpressionSlim> expressions)
        {
            return ExpressionSlim.Block(type, expressions);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given variables and expressions.</summary>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(IEnumerable<ParameterExpressionSlim> variables, ExpressionSlim[] expressions)
        {
            return ExpressionSlim.Block(variables, expressions);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given variables and expressions.</summary>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(IEnumerable<ParameterExpressionSlim> variables, IEnumerable<ExpressionSlim> expressions)
        {
            return ExpressionSlim.Block(variables, expressions);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains three expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <param name="arg2">The third expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            return ExpressionSlim.Block(arg0, arg1, arg2);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given variables and expressions.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(TypeSlim type, IEnumerable<ParameterExpressionSlim> variables, ExpressionSlim[] expressions)
        {
            return ExpressionSlim.Block(type, variables, expressions);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given variables and expressions.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(TypeSlim type, IEnumerable<ParameterExpressionSlim> variables, IEnumerable<ExpressionSlim> expressions)
        {
            return ExpressionSlim.Block(type, variables, expressions);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains four expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <param name="arg2">The third expression in the block.</param>
        /// <param name="arg3">The fourth expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            return ExpressionSlim.Block(arg0, arg1, arg2, arg3);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains five expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <param name="arg2">The third expression in the block.</param>
        /// <param name="arg3">The fourth expression in the block.</param>
        /// <param name="arg4">The fifth expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        public BlockExpressionSlim Block(ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            return ExpressionSlim.Block(arg0, arg1, arg2, arg3, arg4);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a break statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Break(LabelTargetSlim target)
        {
            return ExpressionSlim.Break(target);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a break statement. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Break(LabelTargetSlim target, ExpressionSlim value)
        {
            return ExpressionSlim.Break(target, value);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a break statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />.</returns>
        public GotoExpressionSlim Break(LabelTargetSlim target, TypeSlim type)
        {
            return ExpressionSlim.Break(target, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a break statement with the specified type. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Break(LabelTargetSlim target, ExpressionSlim value, TypeSlim type)
        {
            return ExpressionSlim.Break(target, value, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method that takes one argument.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0)
        {
            return ExpressionSlim.Call(method, arg0);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method that has arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in <paramref name="arguments" /> does not equal the number of parameters for the method represented by <paramref name="method" />.-or-One or more of the elements of <paramref name="arguments" /> is not assignable to the corresponding parameter for the method represented by <paramref name="method" />.</exception>
        public MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim[] arguments)
        {
            return ExpressionSlim.Call(method, arguments);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static (Shared in Visual Basic) method.</summary>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the target method.</param>
        /// <param name="arguments">A collection of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the call arguments.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        public MethodCallExpressionSlim Call(MethodInfoSlim method, IEnumerable<ExpressionSlim> arguments)
        {
            return ExpressionSlim.Call(method, arguments);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes no arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that specifies the instance for an instance method call (pass <see langword="null" /> for a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method).</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is <see langword="null" />.-or-
        ///               <paramref name="instance" /> is <see langword="null" /> and <paramref name="method" /> represents an instance method.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="instance" />.Type is not assignable to the declaring type of the method represented by <paramref name="method" />.</exception>
        public MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method)
        {
            return ExpressionSlim.Call(instance, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static method that takes two arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            return ExpressionSlim.Call(method, arg0, arg1);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that specifies the instance for an instance method call (pass <see langword="null" /> for a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method).</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" />, <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is <see langword="null" />.-or-
        ///               <paramref name="instance" /> is <see langword="null" /> and <paramref name="method" /> represents an instance method.-or-
        ///               <paramref name="arguments" /> is not <see langword="null" /> and one or more of its elements is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="instance" />.Type is not assignable to the declaring type of the method represented by <paramref name="method" />.-or-The number of elements in <paramref name="arguments" /> does not equal the number of parameters for the method represented by <paramref name="method" />.-or-One or more of the elements of <paramref name="arguments" /> is not assignable to the corresponding parameter for the method represented by <paramref name="method" />.</exception>
        public MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim[] arguments)
        {
            return ExpressionSlim.Call(instance, method, arguments);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> property equal to (pass <see langword="null" /> for a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method).</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" />, <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is <see langword="null" />.-or-
        ///               <paramref name="instance" /> is <see langword="null" /> and <paramref name="method" /> represents an instance method.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="instance" />.Type is not assignable to the declaring type of the method represented by <paramref name="method" />.-or-The number of elements in <paramref name="arguments" /> does not equal the number of parameters for the method represented by <paramref name="method" />.-or-One or more of the elements of <paramref name="arguments" /> is not assignable to the corresponding parameter for the method represented by <paramref name="method" />.</exception>
        public MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, IEnumerable<ExpressionSlim> arguments)
        {
            return ExpressionSlim.Call(instance, method, arguments);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static method that takes three arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the third argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            return ExpressionSlim.Call(method, arg0, arg1, arg2);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes two arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that specifies the instance for an instance call. (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the target method.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        public MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1)
        {
            return ExpressionSlim.Call(instance, method, arg0, arg1);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static method that takes four arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the third argument.</param>
        /// <param name="arg3">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the fourth argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3)
        {
            return ExpressionSlim.Call(method, arg0, arg1, arg2, arg3);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes three arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that specifies the instance for an instance call. (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the target method.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the third argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        public MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2)
        {
            return ExpressionSlim.Call(instance, method, arg0, arg1, arg2);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static method that takes five arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the third argument.</param>
        /// <param name="arg3">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the fourth argument.</param>
        /// <param name="arg4">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the fifth argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="method" /> is null.</exception>
        public MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4)
        {
            return ExpressionSlim.Call(method, arg0, arg1, arg2, arg3, arg4);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement.</summary>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> of <see cref="T:System.Exception" /> this <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> will handle.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        public CatchBlockSlim Catch(TypeSlim type, ExpressionSlim body)
        {
            return ExpressionSlim.Catch(type, body);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement with a reference to the caught <see cref="T:System.Exception" /> object for use in the handler body.</summary>
        /// <param name="variable">A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> representing a reference to the <see cref="T:System.Exception" /> object caught by this handler.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        public CatchBlockSlim Catch(ParameterExpressionSlim variable, ExpressionSlim body)
        {
            return ExpressionSlim.Catch(variable, body);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement with an <see cref="T:System.Exception" /> filter but no reference to the caught <see cref="T:System.Exception" /> object.</summary>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> of <see cref="T:System.Exception" /> this <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> will handle.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <param name="filter">The body of the <see cref="T:System.Exception" /> filter.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        public CatchBlockSlim Catch(TypeSlim type, ExpressionSlim body, ExpressionSlim filter)
        {
            return ExpressionSlim.Catch(type, body, filter);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement with an <see cref="T:System.Exception" /> filter and a reference to the caught <see cref="T:System.Exception" /> object.</summary>
        /// <param name="variable">A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> representing a reference to the <see cref="T:System.Exception" /> object caught by this handler.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <param name="filter">The body of the <see cref="T:System.Exception" /> filter.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        public CatchBlockSlim Catch(ParameterExpressionSlim variable, ExpressionSlim body, ExpressionSlim filter)
        {
            return ExpressionSlim.Catch(variable, body, filter);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a coalescing operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of <paramref name="left" /> does not represent a reference type or a nullable value type.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="left" />.Type and <paramref name="right" />.Type are not convertible to each other.</exception>
        public BinaryExpressionSlim Coalesce(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Coalesce(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a coalescing operation, given a conversion function.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="left" />.Type and <paramref name="right" />.Type are not convertible to each other.-or-
        ///               <paramref name="conversion" /> is not <see langword="null" /> and <paramref name="conversion" />.Type is a delegate type that does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of <paramref name="left" /> does not represent a reference type or a nullable value type.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of <paramref name="left" /> represents a type that is not assignable to the parameter type of the delegate type <paramref name="conversion" />.Type.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of <paramref name="right" /> is not equal to the return type of the delegate type <paramref name="conversion" />.Type.</exception>
        public BinaryExpressionSlim Coalesce(ExpressionSlim left, ExpressionSlim right, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.Coalesce(left, right, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that represents a conditional statement.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="test" /> or <paramref name="ifTrue" /> or <paramref name="ifFalse" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="test" />.Type is not <see cref="T:System.Boolean" />.-or-
        ///               <paramref name="ifTrue" />.Type is not equal to <paramref name="ifFalse" />.Type.</exception>
        public ConditionalExpressionSlim Condition(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse)
        {
            return ExpressionSlim.Condition(test, ifTrue, ifFalse);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that represents a conditional statement.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> property equal to.</param>
        /// <param name="type">A <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> properties set to the specified values.</returns>
        public ConditionalExpressionSlim Condition(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse, TypeSlim type)
        {
            return ExpressionSlim.Condition(test, ifTrue, ifFalse, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConstantExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> property set to the specified value.</summary>
        /// <param name="value">An <see cref="T:System.ObjectSlim" /> to set the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConstantExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Constant" /> and the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> property set to the specified value.</returns>
        public ConstantExpressionSlim Constant(ObjectSlim value)
        {
            return ExpressionSlim.Constant(value);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConstantExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</summary>
        /// <param name="value">An <see cref="T:System.ObjectSlim" /> to set the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConstantExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Constant" /> and the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="value" /> is not <see langword="null" /> and <paramref name="type" /> is not assignable from the dynamic type of <paramref name="value" />.</exception>
        public ConstantExpressionSlim Constant(ObjectSlim value, TypeSlim type)
        {
            return ExpressionSlim.Constant(value, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a continue statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Continue(LabelTargetSlim target)
        {
            return ExpressionSlim.Continue(target);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a continue statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Continue(LabelTargetSlim target, TypeSlim type)
        {
            return ExpressionSlim.Continue(target, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a type conversion operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Convert" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.</exception>
        public UnaryExpressionSlim Convert(ExpressionSlim expression, TypeSlim type)
        {
            return ExpressionSlim.Convert(expression, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a conversion operation for which the implementing method is specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Convert" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" />, <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" />, and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.-or-
        ///               <paramref name="expression" />.Type is not assignable to the argument type of the method represented by <paramref name="method" />.-or-The return type of the method represented by <paramref name="method" /> is not assignable to <paramref name="type" />.-or-
        ///               <paramref name="expression" />.Type or <paramref name="type" /> is a nullable value type and the corresponding non-nullable value type does not equal the argument type or the return type, respectively, of the method represented by <paramref name="method" />.</exception>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one method that matches the <paramref name="method" /> description was found.</exception>
        public UnaryExpressionSlim Convert(ExpressionSlim expression, TypeSlim type, MethodInfoSlim method)
        {
            return ExpressionSlim.Convert(expression, type, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a conversion operation that throws an exception if the target type is overflowed.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ConvertChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.</exception>
        public UnaryExpressionSlim ConvertChecked(ExpressionSlim expression, TypeSlim type)
        {
            return ExpressionSlim.ConvertChecked(expression, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a conversion operation that throws an exception if the target type is overflowed and for which the implementing method is specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ConvertChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" />, <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" />, and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.-or-
        ///               <paramref name="expression" />.Type is not assignable to the argument type of the method represented by <paramref name="method" />.-or-The return type of the method represented by <paramref name="method" /> is not assignable to <paramref name="type" />.-or-
        ///               <paramref name="expression" />.Type or <paramref name="type" /> is a nullable value type and the corresponding non-nullable value type does not equal the argument type or the return type, respectively, of the method represented by <paramref name="method" />.</exception>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one method that matches the <paramref name="method" /> description was found.</exception>
        public UnaryExpressionSlim ConvertChecked(ExpressionSlim expression, TypeSlim type, MethodInfoSlim method)
        {
            return ExpressionSlim.ConvertChecked(expression, type, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the decrementing of the expression by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to decrement.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the decremented expression.</returns>
        public UnaryExpressionSlim Decrement(ExpressionSlim expression)
        {
            return ExpressionSlim.Decrement(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the decrementing of the expression by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to decrement.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the decremented expression.</returns>
        public UnaryExpressionSlim Decrement(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.Decrement(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DefaultExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to the specified type.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DefaultExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Default" /> and the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to the specified type.</returns>
        public DefaultExpressionSlim Default(TypeSlim type)
        {
            return ExpressionSlim.Default(type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic division operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Divide" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The division operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Divide(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Divide(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic division operation. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Divide" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the division operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Divide(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.Divide(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a division assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.DivideAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim DivideAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.DivideAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a division assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.DivideAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim DivideAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.DivideAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a division assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.DivideAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim DivideAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.DivideAssign(left, right, method, conversion);
        }

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.ElementInitSlim" />, given an array of values as the second argument.</summary>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.ElementInitSlim.AddMethod" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to set the <see cref="P:System.Linq.Expressions.ElementInitSlim.Arguments" /> property equal to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.ElementInitSlim" /> that has the <see cref="P:System.Linq.Expressions.ElementInitSlim.AddMethod" /> and <see cref="P:System.Linq.Expressions.ElementInitSlim.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="addMethod" /> or <paramref name="arguments" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The method that addMethod represents is not named "Add" (case insensitive).-or-The method that addMethod represents is not an instance method.-or-arguments does not contain the same number of elements as the number of parameters for the method that addMethod represents.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of one or more elements of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the method that <paramref name="addMethod" /> represents.</exception>
        public ElementInitSlim ElementInit(MethodInfoSlim addMethod, ExpressionSlim[] arguments)
        {
            return ExpressionSlim.ElementInit(addMethod, arguments);
        }

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.ElementInitSlim" />, given an <see cref="T:System.Collections.Generic.IEnumerable`1" /> as the second argument.</summary>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.ElementInitSlim.AddMethod" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to set the <see cref="P:System.Linq.Expressions.ElementInitSlim.Arguments" /> property equal to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.ElementInitSlim" /> that has the <see cref="P:System.Linq.Expressions.ElementInitSlim.AddMethod" /> and <see cref="P:System.Linq.Expressions.ElementInitSlim.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="addMethod" /> or <paramref name="arguments" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The method that <paramref name="addMethod" /> represents is not named "Add" (case insensitive).-or-The method that <paramref name="addMethod" /> represents is not an instance method.-or-
        ///               <paramref name="arguments" /> does not contain the same number of elements as the number of parameters for the method that <paramref name="addMethod" /> represents.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of one or more elements of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the method that <paramref name="addMethod" /> represents.</exception>
        public ElementInitSlim ElementInit(MethodInfoSlim addMethod, IEnumerable<ExpressionSlim> arguments)
        {
            return ExpressionSlim.ElementInit(addMethod, arguments);
        }

        /// <summary>Creates an empty expression that has <see cref="T:System.Void" /> type.</summary>
        /// <returns>A <see cref="T:System.Linq.Expressions.DefaultExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Default" /> and the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <see cref="T:System.Void" />.</returns>
        public DefaultExpressionSlim Empty()
        {
            return ExpressionSlim.Empty();
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an equality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The equality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Equal(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Equal(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an equality comparison. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the equality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Equal(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method)
        {
            return ExpressionSlim.Equal(left, right, liftToNull, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="XOR" /> operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOr" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see langword="XOR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim ExclusiveOr(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.ExclusiveOr(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="XOR" /> operation, using op_ExclusiveOr for user-defined types. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOr" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the <see langword="XOR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim ExclusiveOr(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.ExclusiveOr(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise XOR assignment operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim ExclusiveOrAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.ExclusiveOrAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise XOR assignment operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim ExclusiveOrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.ExclusiveOrAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise XOR assignment operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim ExclusiveOrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.ExclusiveOrAssign(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that represents accessing a field.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Expression" /> property equal to. For <see langword="static" /> (<see langword="Shared" /> in Visual Basic), <paramref name="expression" /> must be <see langword="null" />.</param>
        /// <param name="field">The <see cref="T:System.Reflection.FieldInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Member" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" /> and the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Expression" /> and <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Member" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="field" /> is <see langword="null" />.-or-The field represented by <paramref name="field" /> is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic) and <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="expression" />.Type is not assignable to the declaring type of the field represented by <paramref name="field" />.</exception>
        public MemberExpressionSlim Field(ExpressionSlim expression, FieldInfoSlim field)
        {
            return ExpressionSlim.Field(expression, field);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a "go to" statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to the specified value, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Goto(LabelTargetSlim target)
        {
            return ExpressionSlim.Goto(target);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a "go to" statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to the specified value, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Goto(LabelTargetSlim target, TypeSlim type)
        {
            return ExpressionSlim.Goto(target, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a "go to" statement. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Goto(LabelTargetSlim target, ExpressionSlim value)
        {
            return ExpressionSlim.Goto(target, value);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a "go to" statement with the specified type. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Goto(LabelTargetSlim target, ExpressionSlim value, TypeSlim type)
        {
            return ExpressionSlim.Goto(target, value, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "greater than" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The "greater than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim GreaterThan(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.GreaterThan(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "greater than" numeric comparison. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the "greater than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim GreaterThan(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method)
        {
            return ExpressionSlim.GreaterThan(left, right, liftToNull, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "greater than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The "greater than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim GreaterThanOrEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.GreaterThanOrEqual(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "greater than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the "greater than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim GreaterThanOrEqual(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method)
        {
            return ExpressionSlim.GreaterThanOrEqual(left, right, liftToNull, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that represents a conditional block with an <see langword="if" /> statement.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" />, properties set to the specified values. The <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> property is set to default expression and the type of the resulting <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> returned by this method is <see cref="T:System.Void" />.</returns>
        public ConditionalExpressionSlim IfThen(ExpressionSlim test, ExpressionSlim ifTrue)
        {
            return ExpressionSlim.IfThen(test, ifTrue);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that represents a conditional block with <see langword="if" /> and <see langword="else" /> statements.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> properties set to the specified values. The type of the resulting <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> returned by this method is <see cref="T:System.Void" />.</returns>
        public ConditionalExpressionSlim IfThenElse(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse)
        {
            return ExpressionSlim.IfThenElse(test, ifTrue, ifFalse);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the incrementing of the expression value by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to increment.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the incremented expression.</returns>
        public UnaryExpressionSlim Increment(ExpressionSlim expression)
        {
            return ExpressionSlim.Increment(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the incrementing of the expression by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to increment.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the incremented expression.</returns>
        public UnaryExpressionSlim Increment(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.Increment(expression, method);
        }

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.InvocationExpressionSlim" /> that applies a delegate or lambda expression to a list of argument expressions.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the delegate or lambda expression to be applied.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects that represent the arguments that the delegate or lambda expression is applied to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.InvocationExpressionSlim" /> that applies the specified delegate or lambda expression to the provided arguments.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="expression" />.Type does not represent a delegate type or an <see cref="T:System.Linq.Expressions.Expression`1" />.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the delegate represented by <paramref name="expression" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="arguments" /> does not contain the same number of elements as the list of parameters for the delegate represented by <paramref name="expression" />.</exception>
        public InvocationExpressionSlim Invoke(ExpressionSlim expression, ExpressionSlim[] arguments)
        {
            return ExpressionSlim.Invoke(expression, arguments);
        }

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.InvocationExpressionSlim" /> that applies a delegate or lambda expression to a list of argument expressions.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the delegate or lambda expression to be applied to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects that represent the arguments that the delegate or lambda expression is applied to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.InvocationExpressionSlim" /> that applies the specified delegate or lambda expression to the provided arguments.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="expression" />.Type does not represent a delegate type or an <see cref="T:System.Linq.Expressions.Expression`1" />.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the delegate represented by <paramref name="expression" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="arguments" /> does not contain the same number of elements as the list of parameters for the delegate represented by <paramref name="expression" />.</exception>
        public InvocationExpressionSlim Invoke(ExpressionSlim expression, IEnumerable<ExpressionSlim> arguments)
        {
            return ExpressionSlim.Invoke(expression, arguments);
        }

        /// <summary>Returns whether the expression evaluates to false.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to evaluate.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        public UnaryExpressionSlim IsFalse(ExpressionSlim expression)
        {
            return ExpressionSlim.IsFalse(expression);
        }

        /// <summary>Returns whether the expression evaluates to false.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to evaluate.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        public UnaryExpressionSlim IsFalse(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.IsFalse(expression, method);
        }

        /// <summary>Returns whether the expression evaluates to true.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to evaluate.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        public UnaryExpressionSlim IsTrue(ExpressionSlim expression)
        {
            return ExpressionSlim.IsTrue(expression);
        }

        /// <summary>Returns whether the expression evaluates to true.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to evaluate.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        public UnaryExpressionSlim IsTrue(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.IsTrue(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> representing a label with void type and no name.</summary>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTargetSlim" />.</returns>
        public LabelTargetSlim Label()
        {
            return ExpressionSlim.Label();
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> representing a label without a default value.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> which this <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> will be associated with.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> without a default value.</returns>
        public LabelExpressionSlim Label(LabelTargetSlim target)
        {
            return ExpressionSlim.Label(target);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> representing a label with void type and the given name.</summary>
        /// <param name="name">The name of the label.</param>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTargetSlim" />.</returns>
        public LabelTargetSlim Label(String name)
        {
            return ExpressionSlim.Label(name);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> representing a label with the given type.</summary>
        /// <param name="type">The type of value that is passed when jumping to the label.</param>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTargetSlim" />.</returns>
        public LabelTargetSlim Label(TypeSlim type)
        {
            return ExpressionSlim.Label(type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> representing a label with the given default value.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> which this <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> will be associated with.</param>
        /// <param name="defaultValue">The value of this <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> when the label is reached through regular control flow.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> with the given default value.</returns>
        public LabelExpressionSlim Label(LabelTargetSlim target, ExpressionSlim defaultValue)
        {
            return ExpressionSlim.Label(target, defaultValue);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> representing a label with the given type and name.</summary>
        /// <param name="type">The type of value that is passed when jumping to the label.</param>
        /// <param name="name">The name of the label.</param>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTargetSlim" />.</returns>
        public LabelTargetSlim Label(TypeSlim type, String name)
        {
            return ExpressionSlim.Label(type, name);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="body" /> is <see langword="null" />.-or-One or more elements of <paramref name="parameters" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="parameters" /> contains more than sixteen elements.</exception>
        public LambdaExpressionSlim Lambda(ExpressionSlim body, ParameterExpressionSlim[] parameters)
        {
            return ExpressionSlim.Lambda(body, parameters);
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> properties set to the specified values.</returns>
        public LambdaExpressionSlim Lambda(ExpressionSlim body, IEnumerable<ParameterExpressionSlim> parameters)
        {
            return ExpressionSlim.Lambda(body, parameters);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> by first constructing a delegate type. It can be used when the delegate type is not known at compile time.</summary>
        /// <param name="delegateType">A <see cref="T:System.Reflection.TypeSlim" /> that represents a delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> collection.</param>
        /// <returns>An object that represents a lambda expression which has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="delegateType" /> or <paramref name="body" /> is <see langword="null" />.-or-One or more elements in <paramref name="parameters" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="delegateType" /> does not represent a delegate type.-or-
        ///               <paramref name="body" />.Type represents a type that is not assignable to the return type of the delegate type represented by <paramref name="delegateType" />.-or-
        ///               <paramref name="parameters" /> does not contain the same number of elements as the list of parameters for the delegate type represented by <paramref name="delegateType" />.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of the delegate type represented by <paramref name="delegateType" />.</exception>
        public LambdaExpressionSlim Lambda(TypeSlim delegateType, ExpressionSlim body, ParameterExpressionSlim[] parameters)
        {
            return ExpressionSlim.Lambda(delegateType, body, parameters);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> by first constructing a delegate type. It can be used when the delegate type is not known at compile time.</summary>
        /// <param name="delegateType">A <see cref="T:System.Reflection.TypeSlim" /> that represents a delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> collection.</param>
        /// <returns>An object that represents a lambda expression which has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="delegateType" /> or <paramref name="body" /> is <see langword="null" />.-or-One or more elements in <paramref name="parameters" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="delegateType" /> does not represent a delegate type.-or-
        ///               <paramref name="body" />.Type represents a type that is not assignable to the return type of the delegate type represented by <paramref name="delegateType" />.-or-
        ///               <paramref name="parameters" /> does not contain the same number of elements as the list of parameters for the delegate type represented by <paramref name="delegateType" />.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of the delegate type represented by <paramref name="delegateType" />.</exception>
        public LambdaExpressionSlim Lambda(TypeSlim delegateType, ExpressionSlim body, IEnumerable<ParameterExpressionSlim> parameters)
        {
            return ExpressionSlim.Lambda(delegateType, body, parameters);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The left-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim LeftShift(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.LeftShift(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the left-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim LeftShift(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.LeftShift(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim LeftShiftAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.LeftShiftAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim LeftShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.LeftShiftAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim LeftShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.LeftShiftAssign(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "less than" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The "less than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim LessThan(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.LessThan(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "less than" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the "less than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim LessThan(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method)
        {
            return ExpressionSlim.LessThan(left, right, liftToNull, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a " less than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The "less than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim LessThanOrEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.LessThanOrEqual(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "less than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the "less than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim LessThanOrEqual(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method)
        {
            return ExpressionSlim.LessThanOrEqual(left, right, liftToNull, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBindingSlim" /> where the member is a field or property.</summary>
        /// <param name="member">A <see cref="T:System.Reflection.MemberInfoSlim" /> that represents a field or property to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBindingSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberListBindingSlim" /> that has the <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberListBindingSlim.Initializers" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> is <see langword="null" />. -or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Reflection.FieldInfoSlim.FieldType" /> or <see cref="P:System.Reflection.PropertyInfoSlim.PropertyType" /> of the field or property that <paramref name="member" /> represents does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public MemberListBindingSlim ListBind(MemberInfoSlim member, ElementInitSlim[] initializers)
        {
            return ExpressionSlim.ListBind(member, initializers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBindingSlim" /> where the member is a field or property.</summary>
        /// <param name="member">A <see cref="T:System.Reflection.MemberInfoSlim" /> that represents a field or property to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBindingSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberListBindingSlim" /> that has the <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberListBindingSlim.Initializers" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> is <see langword="null" />. -or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Reflection.FieldInfoSlim.FieldType" /> or <see cref="P:System.Reflection.PropertyInfoSlim.PropertyType" /> of the field or property that <paramref name="member" /> represents does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public MemberListBindingSlim ListBind(MemberInfoSlim member, IEnumerable<ElementInitSlim> initializers)
        {
            return ExpressionSlim.ListBind(member, initializers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that uses specified <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to initialize a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> and <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, ElementInitSlim[] initializers)
        {
            return ExpressionSlim.ListInit(newExpression, initializers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that uses specified <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to initialize a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> and <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
        public ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, IEnumerable<ElementInitSlim> initializers)
        {
            return ExpressionSlim.ListInit(newExpression, initializers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that uses a specified method to add elements to a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents an instance method that takes one argument, that adds an element to a collection.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.-or-
        ///               <paramref name="addMethod" /> is not <see langword="null" /> and it does not represent an instance method named "Add" (case insensitive) that takes exactly one argument.-or-
        ///               <paramref name="addMethod" /> is not <see langword="null" /> and the type represented by the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of one or more elements of <paramref name="initializers" /> is not assignable to the argument type of the method that <paramref name="addMethod" /> represents.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="addMethod" /> is <see langword="null" /> and no instance method named "Add" that takes one type-compatible argument exists on <paramref name="newExpression" />.Type or its base type.</exception>
        public ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, MethodInfoSlim addMethod, ExpressionSlim[] initializers)
        {
            return ExpressionSlim.ListInit(newExpression, addMethod, initializers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that uses a specified method to add elements to a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents an instance method named "Add" (case insensitive), that adds an element to a collection.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="initializers" /> is <see langword="null" />.-or-One or more elements of <paramref name="initializers" /> are <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.-or-
        ///               <paramref name="addMethod" /> is not <see langword="null" /> and it does not represent an instance method named "Add" (case insensitive) that takes exactly one argument.-or-
        ///               <paramref name="addMethod" /> is not <see langword="null" /> and the type represented by the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of one or more elements of <paramref name="initializers" /> is not assignable to the argument type of the method that <paramref name="addMethod" /> represents.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="addMethod" /> is <see langword="null" /> and no instance method named "Add" that takes one type-compatible argument exists on <paramref name="newExpression" />.Type or its base type.</exception>
        public ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, MethodInfoSlim addMethod, IEnumerable<ExpressionSlim> initializers)
        {
            return ExpressionSlim.ListInit(newExpression, addMethod, initializers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LoopExpressionSlim" /> with the given body.</summary>
        /// <param name="body">The body of the loop.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.LoopExpressionSlim" />.</returns>
        public LoopExpressionSlim Loop(ExpressionSlim body)
        {
            return ExpressionSlim.Loop(body);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LoopExpressionSlim" /> with the given body and break target.</summary>
        /// <param name="body">The body of the loop.</param>
        /// <param name="break">The break target used by the loop body.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.LoopExpressionSlim" />.</returns>
        public LoopExpressionSlim Loop(ExpressionSlim body, LabelTargetSlim @break)
        {
            return ExpressionSlim.Loop(body, @break);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LoopExpressionSlim" /> with the given body.</summary>
        /// <param name="body">The body of the loop.</param>
        /// <param name="break">The break target used by the loop body.</param>
        /// <param name="continue">The continue target used by the loop body.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.LoopExpressionSlim" />.</returns>
        public LoopExpressionSlim Loop(ExpressionSlim body, LabelTargetSlim @break, LabelTargetSlim @continue)
        {
            return ExpressionSlim.Loop(body, @break, @continue);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" />, given the left and right operands, by calling an appropriate factory method.</summary>
        /// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the left operand.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the right operand.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="binaryType" /> does not correspond to a binary expression node.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        public BinaryExpressionSlim MakeBinary(ExpressionType binaryType, ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.MakeBinary(binaryType, left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" />, given the left operand, right operand and implementing method, by calling the appropriate factory method.</summary>
        /// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the left operand.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the right operand.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that specifies the implementing method.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="binaryType" /> does not correspond to a binary expression node.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        public BinaryExpressionSlim MakeBinary(ExpressionType binaryType, ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method)
        {
            return ExpressionSlim.MakeBinary(binaryType, left, right, liftToNull, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" />, given the left operand, right operand, implementing method and type conversion function, by calling the appropriate factory method.</summary>
        /// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the left operand.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the right operand.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that specifies the implementing method.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> that represents a type conversion function. This parameter is used only if <paramref name="binaryType" /> is <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> or compound assignment..</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="binaryType" /> does not correspond to a binary expression node.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        public BinaryExpressionSlim MakeBinary(ExpressionType binaryType, ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.MakeBinary(binaryType, left, right, liftToNull, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement with the specified elements.</summary>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> of <see cref="T:System.Exception" /> this <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> will handle.</param>
        /// <param name="variable">A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> representing a reference to the <see cref="T:System.Exception" /> object caught by this handler.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <param name="filter">The body of the <see cref="T:System.Exception" /> filter.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        public CatchBlockSlim MakeCatchBlock(TypeSlim type, ParameterExpressionSlim variable, ExpressionSlim body, ExpressionSlim filter)
        {
            return ExpressionSlim.MakeCatchBlock(type, variable, body, filter);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a jump of the specified <see cref="T:System.Linq.Expressions.GotoExpressionKind" />. The value passed to the label upon jumping can also be specified.</summary>
        /// <param name="kind">The <see cref="T:System.Linq.Expressions.GotoExpressionKind" /> of the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" />.</param>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to <paramref name="kind" />, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim MakeGoto(GotoExpressionKind kind, LabelTargetSlim target, ExpressionSlim value, TypeSlim type)
        {
            return ExpressionSlim.MakeGoto(kind, target, value, type);
        }

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> that represents accessing an indexed property in an object.</summary>
        /// <param name="instance">The object to which the property belongs. It should be null if the property is <see langword="static" /> (<see langword="shared" /> in Visual Basic).</param>
        /// <param name="indexer">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> representing the property to index.</param>
        /// <param name="arguments">An IEnumerable&lt;Expression&gt; (IEnumerable (Of Expression) in Visual Basic) that contains the arguments that will be used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        public IndexExpressionSlim MakeIndex(ExpressionSlim instance, PropertyInfoSlim indexer, IEnumerable<ExpressionSlim> arguments)
        {
            return ExpressionSlim.MakeIndex(instance, indexer, arguments);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that represents accessing either a field or a property.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the object that the member belongs to. This can be null for static members.</param>
        /// <param name="member">The <see cref="T:System.Reflection.MemberInfoSlim" /> that describes the field or property to be accessed.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.</exception>
        public MemberExpressionSlim MakeMemberAccess(ExpressionSlim expression, MemberInfoSlim member)
        {
            return ExpressionSlim.MakeMemberAccess(expression, member);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with the specified elements.</summary>
        /// <param name="type">The result type of the try expression. If null, bodh and all handlers must have identical type.</param>
        /// <param name="body">The body of the try block.</param>
        /// <param name="finally">The body of the finally block. Pass null if the try block has no finally block associated with it.</param>
        /// <param name="fault">The body of the fault block. Pass null if the try block has no fault block associated with it.</param>
        /// <param name="handlers">A collection of <see cref="T:System.Linq.Expressions.CatchBlockSlim" />s representing the catch statements to be associated with the try block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        public TryExpressionSlim MakeTry(TypeSlim type, ExpressionSlim body, ExpressionSlim @finally, ExpressionSlim fault, IEnumerable<CatchBlockSlim> handlers)
        {
            return ExpressionSlim.MakeTry(type, body, @finally, fault, handlers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />, given an operand, by calling the appropriate factory method.</summary>
        /// <param name="unaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of unary operation.</param>
        /// <param name="operand">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the operand.</param>
        /// <param name="type">The <see cref="T:System.Reflection.TypeSlim" /> that specifies the type to be converted to (pass <see langword="null" /> if not applicable).</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="operand" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="unaryType" /> does not correspond to a unary expression node.</exception>
        public UnaryExpressionSlim MakeUnary(ExpressionType unaryType, ExpressionSlim operand, TypeSlim type)
        {
            return ExpressionSlim.MakeUnary(unaryType, operand, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />, given an operand and implementing method, by calling the appropriate factory method.</summary>
        /// <param name="unaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of unary operation.</param>
        /// <param name="operand">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the operand.</param>
        /// <param name="type">The <see cref="T:System.Reflection.TypeSlim" /> that specifies the type to be converted to (pass <see langword="null" /> if not applicable).</param>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="operand" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="unaryType" /> does not correspond to a unary expression node.</exception>
        public UnaryExpressionSlim MakeUnary(ExpressionType unaryType, ExpressionSlim operand, TypeSlim type, MethodInfoSlim method)
        {
            return ExpressionSlim.MakeUnary(unaryType, operand, type, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBindingSlim" /> that represents the recursive initialization of members of a field or property.</summary>
        /// <param name="member">The <see cref="T:System.Reflection.MemberInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBindingSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBindingSlim.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBindingSlim" /> that has the <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBindingSlim" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberMemberBindingSlim.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the field or property that <paramref name="member" /> represents.</exception>
        public MemberMemberBindingSlim MemberBind(MemberInfoSlim member, MemberBindingSlim[] bindings)
        {
            return ExpressionSlim.MemberBind(member, bindings);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBindingSlim" /> that represents the recursive initialization of members of a field or property.</summary>
        /// <param name="member">The <see cref="T:System.Reflection.MemberInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBindingSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBindingSlim.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBindingSlim" /> that has the <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBindingSlim" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberMemberBindingSlim.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="member" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the field or property that <paramref name="member" /> represents.</exception>
        public MemberMemberBindingSlim MemberBind(MemberInfoSlim member, IEnumerable<MemberBindingSlim> bindings)
        {
            return ExpressionSlim.MemberBind(member, bindings);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberInitExpressionSlim" />.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBindingSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberInit" /> and the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.NewExpression" /> and <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type that <paramref name="newExpression" />.Type represents.</exception>
        public MemberInitExpressionSlim MemberInit(NewExpressionSlim newExpression, MemberBindingSlim[] bindings)
        {
            return ExpressionSlim.MemberInit(newExpression, bindings);
        }

        /// <summary>Represents an expression that creates a new object and initializes a property of the object.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBindingSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberInit" /> and the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.NewExpression" /> and <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.Bindings" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="newExpression" /> or <paramref name="bindings" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type that <paramref name="newExpression" />.Type represents.</exception>
        public MemberInitExpressionSlim MemberInit(NewExpressionSlim newExpression, IEnumerable<MemberBindingSlim> bindings)
        {
            return ExpressionSlim.MemberInit(newExpression, bindings);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic remainder operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Modulo" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The modulus operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Modulo(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Modulo(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic remainder operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Modulo" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the modulus operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Modulo(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.Modulo(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a remainder assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ModuloAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim ModuloAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.ModuloAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a remainder assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ModuloAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim ModuloAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.ModuloAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a remainder assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ModuloAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim ModuloAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.ModuloAssign(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic multiplication operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Multiply" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Multiply(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Multiply(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic multiplication operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Multiply" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Multiply(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.Multiply(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim MultiplyAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.MultiplyAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim MultiplyAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.MultiplyAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim MultiplyAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.MultiplyAssign(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim MultiplyAssignChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.MultiplyAssignChecked(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim MultiplyAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.MultiplyAssignChecked(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim MultiplyAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.MultiplyAssignChecked(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic multiplication operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim MultiplyChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.MultiplyChecked(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic multiplication operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim MultiplyChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.MultiplyChecked(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an arithmetic negation operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Negate" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The unary minus operator is not defined for <paramref name="expression" />.Type.</exception>
        public UnaryExpressionSlim Negate(ExpressionSlim expression)
        {
            return ExpressionSlim.Negate(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an arithmetic negation operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Negate" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the unary minus operator is not defined for <paramref name="expression" />.Type.-or-
        ///               <paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
        public UnaryExpressionSlim Negate(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.Negate(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an arithmetic negation operation that has overflow checking.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NegateChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The unary minus operator is not defined for <paramref name="expression" />.Type.</exception>
        public UnaryExpressionSlim NegateChecked(ExpressionSlim expression)
        {
            return ExpressionSlim.NegateChecked(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an arithmetic negation operation that has overflow checking. The implementing method can be specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NegateChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the unary minus operator is not defined for <paramref name="expression" />.Type.-or-
        ///               <paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
        public UnaryExpressionSlim NegateChecked(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.NegateChecked(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor that takes no arguments.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The constructor that <paramref name="constructor" /> represents has at least one parameter.</exception>
        public NewExpressionSlim New(ConstructorInfoSlim constructor)
        {
            return ExpressionSlim.New(constructor);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the parameterless constructor of the specified type.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that has a constructor that takes no arguments.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property set to the <see cref="T:System.Reflection.ConstructorInfoSlim" /> that represents the constructor without parameters for the specified type.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The type that <paramref name="type" /> represents does not have a constructor without parameters.</exception>
        public NewExpressionSlim New(TypeSlim type)
        {
            return ExpressionSlim.New(type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor with the specified arguments.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> and <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.-or-An element of <paramref name="arguments" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The length of <paramref name="arguments" /> does match the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.</exception>
        public NewExpressionSlim New(ConstructorInfoSlim constructor, ExpressionSlim[] arguments)
        {
            return ExpressionSlim.New(constructor, arguments);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor with the specified arguments.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> and <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.-or-An element of <paramref name="arguments" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <paramref name="arguments" /> parameter does not contain the same number of elements as the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.</exception>
        public NewExpressionSlim New(ConstructorInfoSlim constructor, IEnumerable<ExpressionSlim> arguments)
        {
            return ExpressionSlim.New(constructor, arguments);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> collection.</param>
        /// <param name="members">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Reflection.MemberInfoSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Members" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" />, <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> and <see cref="P:System.Linq.Expressions.NewExpressionSlim.Members" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.-or-An element of <paramref name="arguments" /> is <see langword="null" />.-or-An element of <paramref name="members" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <paramref name="arguments" /> parameter does not contain the same number of elements as the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.-or-The <paramref name="members" /> parameter does not have the same number of elements as <paramref name="arguments" />.-or-An element of <paramref name="arguments" /> has a <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property that represents a type that is not assignable to the type of the member that is represented by the corresponding element of <paramref name="members" />.</exception>
        public NewExpressionSlim New(ConstructorInfoSlim constructor, IEnumerable<ExpressionSlim> arguments, IEnumerable<MemberInfoSlim> members)
        {
            return ExpressionSlim.New(constructor, arguments, members);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified as an array.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> collection.</param>
        /// <param name="members">An array of <see cref="T:System.Reflection.MemberInfoSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Members" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" />, <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> and <see cref="P:System.Linq.Expressions.NewExpressionSlim.Members" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="constructor" /> is <see langword="null" />.-or-An element of <paramref name="arguments" /> is <see langword="null" />.-or-An element of <paramref name="members" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <paramref name="arguments" /> parameter does not contain the same number of elements as the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.-or-The <paramref name="members" /> parameter does not have the same number of elements as <paramref name="arguments" />.-or-An element of <paramref name="arguments" /> has a <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property that represents a type that is not assignable to the type of the member that is represented by the corresponding element of <paramref name="members" />.</exception>
        public NewExpressionSlim New(ConstructorInfoSlim constructor, IEnumerable<ExpressionSlim> arguments, MemberInfoSlim[] members)
        {
            return ExpressionSlim.New(constructor, arguments, members);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that represents creating an array that has a specified rank.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that represents the element type of the array.</param>
        /// <param name="bounds">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayBounds" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="bounds" /> is <see langword="null" />.-or-An element of <paramref name="bounds" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="bounds" /> does not represent an integral type.</exception>
        public NewArrayExpressionSlim NewArrayBounds(TypeSlim type, ExpressionSlim[] bounds)
        {
            return ExpressionSlim.NewArrayBounds(type, bounds);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that represents creating an array that has a specified rank.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that represents the element type of the array.</param>
        /// <param name="bounds">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayBounds" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="bounds" /> is <see langword="null" />.-or-An element of <paramref name="bounds" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="bounds" /> does not represent an integral type.</exception>
        public NewArrayExpressionSlim NewArrayBounds(TypeSlim type, IEnumerable<ExpressionSlim> bounds)
        {
            return ExpressionSlim.NewArrayBounds(type, bounds);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that represents the element type of the array.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayInit" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="initializers" /> is <see langword="null" />.-or-An element of <paramref name="initializers" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="initializers" /> represents a type that is not assignable to the type <paramref name="type" />.</exception>
        public NewArrayExpressionSlim NewArrayInit(TypeSlim type, ExpressionSlim[] initializers)
        {
            return ExpressionSlim.NewArrayInit(type, initializers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that represents the element type of the array.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayInit" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> or <paramref name="initializers" /> is <see langword="null" />.-or-An element of <paramref name="initializers" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property of an element of <paramref name="initializers" /> represents a type that is not assignable to the type that <paramref name="type" /> represents.</exception>
        public NewArrayExpressionSlim NewArrayInit(TypeSlim type, IEnumerable<ExpressionSlim> initializers)
        {
            return ExpressionSlim.NewArrayInit(type, initializers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a bitwise complement operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Not" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The unary not operator is not defined for <paramref name="expression" />.Type.</exception>
        public UnaryExpressionSlim Not(ExpressionSlim expression)
        {
            return ExpressionSlim.Not(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a bitwise complement operation. The implementing method can be specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Not" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the unary not operator is not defined for <paramref name="expression" />.Type.-or-
        ///               <paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
        public UnaryExpressionSlim Not(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.Not(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an inequality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The inequality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim NotEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.NotEqual(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an inequality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the inequality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim NotEqual(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method)
        {
            return ExpressionSlim.NotEqual(left, right, liftToNull, method);
        }

        /// <summary>Returns the expression representing the ones complement.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        public UnaryExpressionSlim OnesComplement(ExpressionSlim expression)
        {
            return ExpressionSlim.OnesComplement(expression);
        }

        /// <summary>Returns the expression representing the ones complement.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        public UnaryExpressionSlim OnesComplement(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.OnesComplement(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="OR" /> operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Or" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The bitwise <see langword="OR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Or(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Or(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="OR" /> operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Or" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the bitwise <see langword="OR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Or(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.Or(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise OR assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim OrAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.OrAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise OR assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim OrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.OrAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise OR assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim OrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.OrAssign(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a conditional <see langword="OR" /> operation that evaluates the second operand only if the first operand evaluates to <see langword="false" />.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrElse" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The bitwise <see langword="OR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
        public BinaryExpressionSlim OrElse(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.OrElse(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a conditional <see langword="OR" /> operation that evaluates the second operand only if the first operand evaluates to <see langword="false" />.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrElse" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the bitwise <see langword="OR" /> operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="method" /> is <see langword="null" /> and <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
        public BinaryExpressionSlim OrElse(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.OrElse(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> node that can be used to identify a parameter or a variable in an expression tree.</summary>
        /// <param name="type">The type of the parameter or variable.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> node with the specified name and type.</returns>
        public ParameterExpressionSlim Parameter(TypeSlim type)
        {
            return ExpressionSlim.Parameter(type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> node that can be used to identify a parameter or a variable in an expression tree.</summary>
        /// <param name="type">The type of the parameter or variable.</param>
        /// <param name="name">The name of the parameter or variable, used for debugging or printing purpose only.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Parameter" /> and the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> and <see cref="P:System.Linq.Expressions.ParameterExpressionSlim.Name" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="type" /> is <see langword="null" />.</exception>
        public ParameterExpressionSlim Parameter(TypeSlim type, String name)
        {
            return ExpressionSlim.Parameter(type, name);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the assignment of the expression followed by a subsequent decrement by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        public UnaryExpressionSlim PostDecrementAssign(ExpressionSlim expression)
        {
            return ExpressionSlim.PostDecrementAssign(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the assignment of the expression followed by a subsequent decrement by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        public UnaryExpressionSlim PostDecrementAssign(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.PostDecrementAssign(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the assignment of the expression followed by a subsequent increment by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        public UnaryExpressionSlim PostIncrementAssign(ExpressionSlim expression)
        {
            return ExpressionSlim.PostIncrementAssign(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the assignment of the expression followed by a subsequent increment by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        public UnaryExpressionSlim PostIncrementAssign(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.PostIncrementAssign(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising a number to a power.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Power" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The exponentiation operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="left" />.Type and/or <paramref name="right" />.Type are not <see cref="T:System.Double" />.</exception>
        public BinaryExpressionSlim Power(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Power(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising a number to a power.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Power" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the exponentiation operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-
        ///               <paramref name="method" /> is <see langword="null" /> and <paramref name="left" />.Type and/or <paramref name="right" />.Type are not <see cref="T:System.Double" />.</exception>
        public BinaryExpressionSlim Power(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.Power(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising an expression to a power and assigning the result back to the expression.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.PowerAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim PowerAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.PowerAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising an expression to a power and assigning the result back to the expression.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.PowerAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim PowerAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.PowerAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising an expression to a power and assigning the result back to the expression.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.PowerAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim PowerAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.PowerAssign(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that decrements the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        public UnaryExpressionSlim PreDecrementAssign(ExpressionSlim expression)
        {
            return ExpressionSlim.PreDecrementAssign(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that decrements the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        public UnaryExpressionSlim PreDecrementAssign(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.PreDecrementAssign(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that increments the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        public UnaryExpressionSlim PreIncrementAssign(ExpressionSlim expression)
        {
            return ExpressionSlim.PreIncrementAssign(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that increments the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        public UnaryExpressionSlim PreIncrementAssign(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.PreIncrementAssign(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that represents accessing a property.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Expression" /> property equal to. This can be null for static properties.</param>
        /// <param name="property">The <see cref="T:System.Reflection.PropertyInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Member" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" /> and the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Expression" /> and <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Member" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="property" /> is <see langword="null" />.-or-The property that <paramref name="property" /> represents is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic) and <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="expression" />.Type is not assignable to the declaring type of the property that <paramref name="property" /> represents.</exception>
        public MemberExpressionSlim Property(ExpressionSlim expression, PropertyInfoSlim property)
        {
            return ExpressionSlim.Property(expression, property);
        }

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> representing the access to an indexed property.</summary>
        /// <param name="instance">The object to which the property belongs. If the property is static/shared, it must be null.</param>
        /// <param name="indexer">The <see cref="T:System.Reflection.PropertyInfoSlim" /> that represents the property to index.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects that are used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        public IndexExpressionSlim Property(ExpressionSlim instance, PropertyInfoSlim indexer, ExpressionSlim[] arguments)
        {
            return ExpressionSlim.Property(instance, indexer, arguments);
        }

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> representing the access to an indexed property.</summary>
        /// <param name="instance">The object to which the property belongs. If the property is static/shared, it must be null.</param>
        /// <param name="indexer">The <see cref="T:System.Reflection.PropertyInfoSlim" /> that represents the property to index.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects that are used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        public IndexExpressionSlim Property(ExpressionSlim instance, PropertyInfoSlim indexer, IEnumerable<ExpressionSlim> arguments)
        {
            return ExpressionSlim.Property(instance, indexer, arguments);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an expression that has a constant value of type <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Quote" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        public UnaryExpressionSlim Quote(ExpressionSlim expression)
        {
            return ExpressionSlim.Quote(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a reference equality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim ReferenceEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.ReferenceEqual(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a reference inequality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim ReferenceNotEqual(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.ReferenceNotEqual(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a rethrowing of an exception.</summary>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a rethrowing of an exception.</returns>
        public UnaryExpressionSlim Rethrow()
        {
            return ExpressionSlim.Rethrow();
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a rethrowing of an exception with a given type.</summary>
        /// <param name="type">The new <see cref="T:System.Reflection.TypeSlim" /> of the expression.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a rethrowing of an exception.</returns>
        public UnaryExpressionSlim Rethrow(TypeSlim type)
        {
            return ExpressionSlim.Rethrow(type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a return statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Return, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Return(LabelTargetSlim target)
        {
            return ExpressionSlim.Return(target);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a return statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Return, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and a null value to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Return(LabelTargetSlim target, TypeSlim type)
        {
            return ExpressionSlim.Return(target, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a return statement. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Return(LabelTargetSlim target, ExpressionSlim value)
        {
            return ExpressionSlim.Return(target, value);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a return statement with the specified type. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        public GotoExpressionSlim Return(LabelTargetSlim target, ExpressionSlim value, TypeSlim type)
        {
            return ExpressionSlim.Return(target, value, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The right-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim RightShift(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.RightShift(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the right-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim RightShift(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.RightShift(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim RightShiftAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.RightShiftAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim RightShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.RightShiftAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim RightShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.RightShiftAssign(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic subtraction operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Subtract" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Subtract(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.Subtract(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic subtraction operation that does not have overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Subtract" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim Subtract(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.Subtract(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim SubtractAssign(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.SubtractAssign(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim SubtractAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.SubtractAssign(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim SubtractAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.SubtractAssign(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim SubtractAssignChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.SubtractAssignChecked(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim SubtractAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.SubtractAssignChecked(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        public BinaryExpressionSlim SubtractAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            return ExpressionSlim.SubtractAssignChecked(left, right, method, conversion);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic subtraction operation that has overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim SubtractChecked(ExpressionSlim left, ExpressionSlim right)
        {
            return ExpressionSlim.SubtractChecked(left, right);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic subtraction operation that has overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="left" /> or <paramref name="right" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly two arguments.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
        public BinaryExpressionSlim SubtractChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
        {
            return ExpressionSlim.SubtractChecked(left, right, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement without a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        public SwitchExpressionSlim Switch(ExpressionSlim switchValue, SwitchCaseSlim[] cases)
        {
            return ExpressionSlim.Switch(switchValue, cases);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        public SwitchExpressionSlim Switch(ExpressionSlim switchValue, ExpressionSlim defaultBody, SwitchCaseSlim[] cases)
        {
            return ExpressionSlim.Switch(switchValue, defaultBody, cases);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        public SwitchExpressionSlim Switch(ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, SwitchCaseSlim[] cases)
        {
            return ExpressionSlim.Switch(switchValue, defaultBody, comparison, cases);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        public SwitchExpressionSlim Switch(ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, IEnumerable<SwitchCaseSlim> cases)
        {
            return ExpressionSlim.Switch(switchValue, defaultBody, comparison, cases);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="type">The result type of the switch.</param>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        public SwitchExpressionSlim Switch(TypeSlim type, ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, IEnumerable<SwitchCaseSlim> cases)
        {
            return ExpressionSlim.Switch(type, switchValue, defaultBody, comparison, cases);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case..</summary>
        /// <param name="type">The result type of the switch.</param>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        public SwitchExpressionSlim Switch(TypeSlim type, ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, SwitchCaseSlim[] cases)
        {
            return ExpressionSlim.Switch(type, switchValue, defaultBody, comparison, cases);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchCaseSlim" /> for use in a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</summary>
        /// <param name="body">The body of the case.</param>
        /// <param name="testValues">The test values of the case.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchCaseSlim" />.</returns>
        public SwitchCaseSlim SwitchCase(ExpressionSlim body, ExpressionSlim[] testValues)
        {
            return ExpressionSlim.SwitchCase(body, testValues);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchCaseSlim" /> object to be used in a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> object.</summary>
        /// <param name="body">The body of the case.</param>
        /// <param name="testValues">The test values of the case.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchCaseSlim" />.</returns>
        public SwitchCaseSlim SwitchCase(ExpressionSlim body, IEnumerable<ExpressionSlim> testValues)
        {
            return ExpressionSlim.SwitchCase(body, testValues);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a throwing of an exception.</summary>
        /// <param name="value">An <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the exception.</returns>
        public UnaryExpressionSlim Throw(ExpressionSlim value)
        {
            return ExpressionSlim.Throw(value);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a throwing of an exception with a given type.</summary>
        /// <param name="value">An <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</param>
        /// <param name="type">The new <see cref="T:System.Reflection.TypeSlim" /> of the expression.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the exception.</returns>
        public UnaryExpressionSlim Throw(ExpressionSlim value, TypeSlim type)
        {
            return ExpressionSlim.Throw(value, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with any number of catch statements and neither a fault nor finally block.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="handlers">The array of zero or more <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> expressions representing the catch statements to be associated with the try block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        public TryExpressionSlim TryCatch(ExpressionSlim body, CatchBlockSlim[] handlers)
        {
            return ExpressionSlim.TryCatch(body, handlers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with any number of catch statements and a finally block.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="finally">The body of the finally block.</param>
        /// <param name="handlers">The array of zero or more <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> expressions representing the catch statements to be associated with the try block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        public TryExpressionSlim TryCatchFinally(ExpressionSlim body, ExpressionSlim @finally, CatchBlockSlim[] handlers)
        {
            return ExpressionSlim.TryCatchFinally(body, @finally, handlers);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with a fault block and no catch statements.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="fault">The body of the fault block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        public TryExpressionSlim TryFault(ExpressionSlim body, ExpressionSlim fault)
        {
            return ExpressionSlim.TryFault(body, fault);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with a finally block and no catch statements.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="finally">The body of the finally block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        public TryExpressionSlim TryFinally(ExpressionSlim body, ExpressionSlim @finally)
        {
            return ExpressionSlim.TryFinally(body, @finally);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an explicit reference or boxing conversion where <see langword="null" /> is supplied if the conversion fails.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.TypeAs" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        public UnaryExpressionSlim TypeAs(ExpressionSlim expression, TypeSlim type)
        {
            return ExpressionSlim.TypeAs(expression, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TypeBinaryExpressionSlim" /> that compares run-time type identity.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="T:System.Linq.Expressions.ExpressionSlim" /> property equal to.</param>
        /// <param name="type">A <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.TypeOperand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.TypeBinaryExpressionSlim" /> for which the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property is equal to <see cref="M:System.Linq.Expressions.Expression.TypeEqual(System.Linq.Expressions.Expression,System.Type)" /> and for which the <see cref="T:System.Linq.Expressions.ExpressionSlim" /> and <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.TypeOperand" /> properties are set to the specified values.</returns>
        public TypeBinaryExpressionSlim TypeEqual(ExpressionSlim expression, TypeSlim type)
        {
            return ExpressionSlim.TypeEqual(expression, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TypeBinaryExpressionSlim" />.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.Expression" /> property equal to.</param>
        /// <param name="type">A <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.TypeOperand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.TypeBinaryExpressionSlim" /> for which the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property is equal to <see cref="F:System.Linq.Expressions.ExpressionType.TypeIs" /> and for which the <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.Expression" /> and <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.TypeOperand" /> properties are set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        public TypeBinaryExpressionSlim TypeIs(ExpressionSlim expression, TypeSlim type)
        {
            return ExpressionSlim.TypeIs(expression, type);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a unary plus operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.UnaryPlus" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.InvalidOperationException">The unary plus operator is not defined for <paramref name="expression" />.Type.</exception>
        public UnaryExpressionSlim UnaryPlus(ExpressionSlim expression)
        {
            return ExpressionSlim.UnaryPlus(expression);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a unary plus operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.UnaryPlus" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="expression" /> is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="method" /> is not <see langword="null" /> and the method it represents returns <see langword="void" />, is not <see langword="static" /> (<see langword="Shared" /> in Visual Basic), or does not take exactly one argument.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="method" /> is <see langword="null" /> and the unary plus operator is not defined for <paramref name="expression" />.Type.-or-
        ///               <paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
        public UnaryExpressionSlim UnaryPlus(ExpressionSlim expression, MethodInfoSlim method)
        {
            return ExpressionSlim.UnaryPlus(expression, method);
        }

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an explicit unboxing.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to unbox.</param>
        /// <param name="type">The new <see cref="T:System.Reflection.TypeSlim" /> of the expression.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        public UnaryExpressionSlim Unbox(ExpressionSlim expression, TypeSlim type)
        {
            return ExpressionSlim.Unbox(expression, type);
        }

    }
}
