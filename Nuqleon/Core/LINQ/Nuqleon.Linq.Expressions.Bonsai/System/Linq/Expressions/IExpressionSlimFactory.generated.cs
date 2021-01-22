// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Interface for expression factories.
    /// </summary>
    public interface IExpressionSlimFactory
    {
        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic addition operation that does not have overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Add" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Add(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic addition operation that does not have overflow checking. The implementing method can be specified.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Add" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Add(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AddAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AddAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AddAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AddAssignChecked(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AddAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an addition assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AddAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic addition operation that has overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AddChecked(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic addition operation that has overflow checking. The implementing method can be specified.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AddChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="AND" /> operation.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.And" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim And(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="AND" /> operation. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.And" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim And(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a conditional <see langword="AND" /> operation that evaluates the second operand only if the first operand evaluates to <see langword="true" />.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAlso" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AndAlso(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a conditional <see langword="AND" /> operation that evaluates the second operand only if the first operand is resolved to true. The implementing method can be specified.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAlso" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AndAlso(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise AND assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AndAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise AND assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AndAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise AND assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim AndAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> to access an array.</summary>
        /// <param name="array">An expression representing the array to index.</param>
        /// <param name="indexes">An array that contains expressions used to index the array.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        IndexExpressionSlim ArrayAccess(ExpressionSlim array, params ExpressionSlim[] indexes);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> to access a multidimensional array.</summary>
        /// <param name="array">An expression that represents the multidimensional array.</param>
        /// <param name="indexes">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing expressions used to index the array.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        IndexExpressionSlim ArrayAccess(ExpressionSlim array, IEnumerable<ExpressionSlim> indexes);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents applying an array index operator to an array of rank one.</summary>
        /// <param name="array">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="index">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ArrayIndex" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ArrayIndex(ExpressionSlim array, ExpressionSlim index);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an expression for obtaining the length of a one-dimensional array.</summary>
        /// <param name="array">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ArrayLength" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to <paramref name="array" />.</returns>
        UnaryExpressionSlim ArrayLength(ExpressionSlim array);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Assign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Assign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberAssignmentSlim" /> that represents the initialization of a field or property.</summary>
        /// <param name="member">A <see cref="T:System.Reflection.MemberInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberAssignmentSlim.Expression" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberAssignmentSlim" /> that has <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> equal to <see cref="F:System.Linq.Expressions.MemberBindingType.Assignment" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberAssignmentSlim.Expression" /> properties set to the specified values.</returns>
        MemberAssignmentSlim Bind(MemberInfoSlim member, ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given expressions and has no variables.</summary>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(params ExpressionSlim[] expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given expressions and has no variables.</summary>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(IEnumerable<ExpressionSlim> expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains two expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(ExpressionSlim arg0, ExpressionSlim arg1);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given expressions, has no variables and has specific result type.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(TypeSlim type, params ExpressionSlim[] expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given expressions, has no variables and has specific result type.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(TypeSlim type, IEnumerable<ExpressionSlim> expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given variables and expressions.</summary>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(IEnumerable<ParameterExpressionSlim> variables, params ExpressionSlim[] expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given variables and expressions.</summary>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(IEnumerable<ParameterExpressionSlim> variables, IEnumerable<ExpressionSlim> expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains three expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <param name="arg2">The third expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given variables and expressions.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(TypeSlim type, IEnumerable<ParameterExpressionSlim> variables, params ExpressionSlim[] expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains the given variables and expressions.</summary>
        /// <param name="type">The result type of the block.</param>
        /// <param name="variables">The variables in the block.</param>
        /// <param name="expressions">The expressions in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(TypeSlim type, IEnumerable<ParameterExpressionSlim> variables, IEnumerable<ExpressionSlim> expressions);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains four expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <param name="arg2">The third expression in the block.</param>
        /// <param name="arg3">The fourth expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BlockExpressionSlim" /> that contains five expressions and has no variables.</summary>
        /// <param name="arg0">The first expression in the block.</param>
        /// <param name="arg1">The second expression in the block.</param>
        /// <param name="arg2">The third expression in the block.</param>
        /// <param name="arg3">The fourth expression in the block.</param>
        /// <param name="arg4">The fifth expression in the block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.BlockExpressionSlim" />.</returns>
        BlockExpressionSlim Block(ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a break statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and a null value to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Break(LabelTargetSlim target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a break statement. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Break(LabelTargetSlim target, ExpressionSlim value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a break statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />.</returns>
        GotoExpressionSlim Break(LabelTargetSlim target, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a break statement with the specified type. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Break, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Break(LabelTargetSlim target, ExpressionSlim value, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method that takes one argument.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method that has arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(MethodInfoSlim method, params ExpressionSlim[] arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static (Shared in Visual Basic) method.</summary>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the target method.</param>
        /// <param name="arguments">A collection of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the call arguments.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(MethodInfoSlim method, IEnumerable<ExpressionSlim> arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes no arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that specifies the instance for an instance method call (pass <see langword="null" /> for a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method).</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static method that takes two arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that specifies the instance for an instance method call (pass <see langword="null" /> for a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method).</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" />, <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, params ExpressionSlim[] arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> property equal to (pass <see langword="null" /> for a <see langword="static" /> (<see langword="Shared" /> in Visual Basic) method).</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" />, <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, IEnumerable<ExpressionSlim> arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static method that takes three arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the third argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes two arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that specifies the instance for an instance call. (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the target method.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static method that takes four arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the third argument.</param>
        /// <param name="arg3">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the fourth argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a method that takes three arguments.</summary>
        /// <param name="instance">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that specifies the instance for an instance call. (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the target method.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the third argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(ExpressionSlim instance, MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that represents a call to a static method that takes five arguments.</summary>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the third argument.</param>
        /// <param name="arg3">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the fourth argument.</param>
        /// <param name="arg4">The <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the fifth argument.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.ObjectSlim" /> and <see cref="P:System.Linq.Expressions.MethodCallExpressionSlim.Method" /> properties set to the specified values.</returns>
        MethodCallExpressionSlim Call(MethodInfoSlim method, ExpressionSlim arg0, ExpressionSlim arg1, ExpressionSlim arg2, ExpressionSlim arg3, ExpressionSlim arg4);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement.</summary>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> of <see cref="T:System.Exception" /> this <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> will handle.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        CatchBlockSlim Catch(TypeSlim type, ExpressionSlim body);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement with a reference to the caught <see cref="T:System.Exception" /> object for use in the handler body.</summary>
        /// <param name="variable">A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> representing a reference to the <see cref="T:System.Exception" /> object caught by this handler.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        CatchBlockSlim Catch(ParameterExpressionSlim variable, ExpressionSlim body);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement with an <see cref="T:System.Exception" /> filter but no reference to the caught <see cref="T:System.Exception" /> object.</summary>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> of <see cref="T:System.Exception" /> this <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> will handle.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <param name="filter">The body of the <see cref="T:System.Exception" /> filter.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        CatchBlockSlim Catch(TypeSlim type, ExpressionSlim body, ExpressionSlim filter);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement with an <see cref="T:System.Exception" /> filter and a reference to the caught <see cref="T:System.Exception" /> object.</summary>
        /// <param name="variable">A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> representing a reference to the <see cref="T:System.Exception" /> object caught by this handler.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <param name="filter">The body of the <see cref="T:System.Exception" /> filter.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        CatchBlockSlim Catch(ParameterExpressionSlim variable, ExpressionSlim body, ExpressionSlim filter);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a coalescing operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Coalesce(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a coalescing operation, given a conversion function.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Coalesce(ExpressionSlim left, ExpressionSlim right, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that represents a conditional statement.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> properties set to the specified values.</returns>
        ConditionalExpressionSlim Condition(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that represents a conditional statement.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> property equal to.</param>
        /// <param name="type">A <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> properties set to the specified values.</returns>
        ConditionalExpressionSlim Condition(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConstantExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> property set to the specified value.</summary>
        /// <param name="value">An <see cref="T:System.ObjectSlim" /> to set the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConstantExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Constant" /> and the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> property set to the specified value.</returns>
        ConstantExpressionSlim Constant(ObjectSlim value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConstantExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</summary>
        /// <param name="value">An <see cref="T:System.ObjectSlim" /> to set the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConstantExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Constant" /> and the <see cref="P:System.Linq.Expressions.ConstantExpressionSlim.Value" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</returns>
        ConstantExpressionSlim Constant(ObjectSlim value, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a continue statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and a null value to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Continue(LabelTargetSlim target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a continue statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and a null value to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Continue(LabelTargetSlim target, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a type conversion operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Convert" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</returns>
        UnaryExpressionSlim Convert(ExpressionSlim expression, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a conversion operation for which the implementing method is specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Convert" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" />, <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" />, and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        UnaryExpressionSlim Convert(ExpressionSlim expression, TypeSlim type, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a conversion operation that throws an exception if the target type is overflowed.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ConvertChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</returns>
        UnaryExpressionSlim ConvertChecked(ExpressionSlim expression, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a conversion operation that throws an exception if the target type is overflowed and for which the implementing method is specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ConvertChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" />, <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" />, and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        UnaryExpressionSlim ConvertChecked(ExpressionSlim expression, TypeSlim type, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the decrementing of the expression by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to decrement.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the decremented expression.</returns>
        UnaryExpressionSlim Decrement(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the decrementing of the expression by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to decrement.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the decremented expression.</returns>
        UnaryExpressionSlim Decrement(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.DefaultExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to the specified type.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.DefaultExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Default" /> and the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to the specified type.</returns>
        DefaultExpressionSlim Default(TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic division operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Divide" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Divide(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic division operation. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Divide" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Divide(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a division assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.DivideAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim DivideAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a division assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.DivideAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim DivideAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a division assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.DivideAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim DivideAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.ElementInitSlim" />, given an array of values as the second argument.</summary>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.ElementInitSlim.AddMethod" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to set the <see cref="P:System.Linq.Expressions.ElementInitSlim.Arguments" /> property equal to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.ElementInitSlim" /> that has the <see cref="P:System.Linq.Expressions.ElementInitSlim.AddMethod" /> and <see cref="P:System.Linq.Expressions.ElementInitSlim.Arguments" /> properties set to the specified values.</returns>
        ElementInitSlim ElementInit(MethodInfoSlim addMethod, params ExpressionSlim[] arguments);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.ElementInitSlim" />, given an <see cref="T:System.Collections.Generic.IEnumerable`1" /> as the second argument.</summary>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.ElementInitSlim.AddMethod" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to set the <see cref="P:System.Linq.Expressions.ElementInitSlim.Arguments" /> property equal to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.ElementInitSlim" /> that has the <see cref="P:System.Linq.Expressions.ElementInitSlim.AddMethod" /> and <see cref="P:System.Linq.Expressions.ElementInitSlim.Arguments" /> properties set to the specified values.</returns>
        ElementInitSlim ElementInit(MethodInfoSlim addMethod, IEnumerable<ExpressionSlim> arguments);

        /// <summary>Creates an empty expression that has <see cref="T:System.Void" /> type.</summary>
        /// <returns>A <see cref="T:System.Linq.Expressions.DefaultExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Default" /> and the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <see cref="T:System.Void" />.</returns>
        DefaultExpressionSlim Empty();

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an equality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Equal(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an equality comparison. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Equal(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="XOR" /> operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOr" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ExclusiveOr(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="XOR" /> operation, using op_ExclusiveOr for user-defined types. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOr" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ExclusiveOr(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise XOR assignment operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ExclusiveOrAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise XOR assignment operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ExclusiveOrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise XOR assignment operation, using op_ExclusiveOr for user-defined types.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ExclusiveOrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that represents accessing a field.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Expression" /> property equal to. For <see langword="static" /> (<see langword="Shared" /> in Visual Basic), <paramref name="expression" /> must be <see langword="null" />.</param>
        /// <param name="field">The <see cref="T:System.Reflection.FieldInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Member" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" /> and the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Expression" /> and <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Member" /> properties set to the specified values.</returns>
        MemberExpressionSlim Field(ExpressionSlim expression, FieldInfoSlim field);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a "go to" statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to the specified value, and a null value to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Goto(LabelTargetSlim target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a "go to" statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to the specified value, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and a null value to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Goto(LabelTargetSlim target, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a "go to" statement. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Goto(LabelTargetSlim target, ExpressionSlim value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a "go to" statement with the specified type. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Goto, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Goto(LabelTargetSlim target, ExpressionSlim value, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "greater than" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim GreaterThan(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "greater than" numeric comparison. The implementing method can be specified.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim GreaterThan(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "greater than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim GreaterThanOrEqual(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "greater than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim GreaterThanOrEqual(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that represents a conditional block with an <see langword="if" /> statement.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" />, properties set to the specified values. The <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> property is set to default expression and the type of the resulting <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> returned by this method is <see cref="T:System.Void" />.</returns>
        ConditionalExpressionSlim IfThen(ExpressionSlim test, ExpressionSlim ifTrue);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that represents a conditional block with <see langword="if" /> and <see langword="else" /> statements.</summary>
        /// <param name="test">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpressionSlim.IfFalse" /> properties set to the specified values. The type of the resulting <see cref="T:System.Linq.Expressions.ConditionalExpressionSlim" /> returned by this method is <see cref="T:System.Void" />.</returns>
        ConditionalExpressionSlim IfThenElse(ExpressionSlim test, ExpressionSlim ifTrue, ExpressionSlim ifFalse);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the incrementing of the expression value by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to increment.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the incremented expression.</returns>
        UnaryExpressionSlim Increment(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the incrementing of the expression by 1.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to increment.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the incremented expression.</returns>
        UnaryExpressionSlim Increment(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.InvocationExpressionSlim" /> that applies a delegate or lambda expression to a list of argument expressions.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the delegate or lambda expression to be applied.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects that represent the arguments that the delegate or lambda expression is applied to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.InvocationExpressionSlim" /> that applies the specified delegate or lambda expression to the provided arguments.</returns>
        InvocationExpressionSlim Invoke(ExpressionSlim expression, params ExpressionSlim[] arguments);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.InvocationExpressionSlim" /> that applies a delegate or lambda expression to a list of argument expressions.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the delegate or lambda expression to be applied to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects that represent the arguments that the delegate or lambda expression is applied to.</param>
        /// <returns>An <see cref="T:System.Linq.Expressions.InvocationExpressionSlim" /> that applies the specified delegate or lambda expression to the provided arguments.</returns>
        InvocationExpressionSlim Invoke(ExpressionSlim expression, IEnumerable<ExpressionSlim> arguments);

        /// <summary>Returns whether the expression evaluates to false.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to evaluate.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        UnaryExpressionSlim IsFalse(ExpressionSlim expression);

        /// <summary>Returns whether the expression evaluates to false.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to evaluate.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        UnaryExpressionSlim IsFalse(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Returns whether the expression evaluates to true.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to evaluate.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        UnaryExpressionSlim IsTrue(ExpressionSlim expression);

        /// <summary>Returns whether the expression evaluates to true.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to evaluate.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        UnaryExpressionSlim IsTrue(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> representing a label with void type and no name.</summary>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTargetSlim" />.</returns>
        LabelTargetSlim Label();

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> representing a label without a default value.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> which this <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> will be associated with.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> without a default value.</returns>
        LabelExpressionSlim Label(LabelTargetSlim target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> representing a label with void type and the given name.</summary>
        /// <param name="name">The name of the label.</param>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTargetSlim" />.</returns>
        LabelTargetSlim Label(String name);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> representing a label with the given type.</summary>
        /// <param name="type">The type of value that is passed when jumping to the label.</param>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTargetSlim" />.</returns>
        LabelTargetSlim Label(TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> representing a label with the given default value.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> which this <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> will be associated with.</param>
        /// <param name="defaultValue">The value of this <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> when the label is reached through regular control flow.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LabelExpressionSlim" /> with the given default value.</returns>
        LabelExpressionSlim Label(LabelTargetSlim target, ExpressionSlim defaultValue);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> representing a label with the given type and name.</summary>
        /// <param name="type">The type of value that is passed when jumping to the label.</param>
        /// <param name="name">The name of the label.</param>
        /// <returns>The new <see cref="T:System.Linq.Expressions.LabelTargetSlim" />.</returns>
        LabelTargetSlim Label(TypeSlim type, String name);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> properties set to the specified values.</returns>
        LambdaExpressionSlim Lambda(ExpressionSlim body, params ParameterExpressionSlim[] parameters);

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.NodeType" /> property equal to Lambda and the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> properties set to the specified values.</returns>
        LambdaExpressionSlim Lambda(ExpressionSlim body, IEnumerable<ParameterExpressionSlim> parameters);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> by first constructing a delegate type. It can be used when the delegate type is not known at compile time.</summary>
        /// <param name="delegateType">A <see cref="T:System.Reflection.TypeSlim" /> that represents a delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> collection.</param>
        /// <returns>An object that represents a lambda expression which has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> properties set to the specified values.</returns>
        LambdaExpressionSlim Lambda(TypeSlim delegateType, ExpressionSlim body, params ParameterExpressionSlim[] parameters);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> by first constructing a delegate type. It can be used when the delegate type is not known at compile time.</summary>
        /// <param name="delegateType">A <see cref="T:System.Reflection.TypeSlim" /> that represents a delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> collection.</param>
        /// <returns>An object that represents a lambda expression which has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpressionSlim.Parameters" /> properties set to the specified values.</returns>
        LambdaExpressionSlim Lambda(TypeSlim delegateType, ExpressionSlim body, IEnumerable<ParameterExpressionSlim> parameters);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim LeftShift(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim LeftShift(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim LeftShiftAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim LeftShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise left-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim LeftShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "less than" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim LessThan(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "less than" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim LessThan(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a " less than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim LessThanOrEqual(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a "less than or equal" numeric comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim LessThanOrEqual(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBindingSlim" /> where the member is a field or property.</summary>
        /// <param name="member">A <see cref="T:System.Reflection.MemberInfoSlim" /> that represents a field or property to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBindingSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberListBindingSlim" /> that has the <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberListBindingSlim.Initializers" /> properties set to the specified values.</returns>
        MemberListBindingSlim ListBind(MemberInfoSlim member, params ElementInitSlim[] initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBindingSlim" /> where the member is a field or property.</summary>
        /// <param name="member">A <see cref="T:System.Reflection.MemberInfoSlim" /> that represents a field or property to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBindingSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberListBindingSlim" /> that has the <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberListBindingSlim.Initializers" /> properties set to the specified values.</returns>
        MemberListBindingSlim ListBind(MemberInfoSlim member, IEnumerable<ElementInitSlim> initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that uses specified <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to initialize a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> and <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> properties set to the specified values.</returns>
        ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, params ElementInitSlim[] initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that uses specified <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to initialize a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInitSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> and <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> properties set to the specified values.</returns>
        ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, IEnumerable<ElementInitSlim> initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that uses a specified method to add elements to a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents an instance method that takes one argument, that adds an element to a collection.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property set to the specified value.</returns>
        ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, MethodInfoSlim addMethod, params ExpressionSlim[] initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that uses a specified method to add elements to a collection.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents an instance method named "Add" (case insensitive), that adds an element to a collection.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.Initializers" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpressionSlim.NewExpression" /> property set to the specified value.</returns>
        ListInitExpressionSlim ListInit(NewExpressionSlim newExpression, MethodInfoSlim addMethod, IEnumerable<ExpressionSlim> initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LoopExpressionSlim" /> with the given body.</summary>
        /// <param name="body">The body of the loop.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.LoopExpressionSlim" />.</returns>
        LoopExpressionSlim Loop(ExpressionSlim body);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LoopExpressionSlim" /> with the given body and break target.</summary>
        /// <param name="body">The body of the loop.</param>
        /// <param name="break">The break target used by the loop body.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.LoopExpressionSlim" />.</returns>
        LoopExpressionSlim Loop(ExpressionSlim body, LabelTargetSlim @break);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.LoopExpressionSlim" /> with the given body.</summary>
        /// <param name="body">The body of the loop.</param>
        /// <param name="break">The break target used by the loop body.</param>
        /// <param name="continue">The continue target used by the loop body.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.LoopExpressionSlim" />.</returns>
        LoopExpressionSlim Loop(ExpressionSlim body, LabelTargetSlim @break, LabelTargetSlim @continue);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" />, given the left and right operands, by calling an appropriate factory method.</summary>
        /// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the left operand.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the right operand.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        BinaryExpressionSlim MakeBinary(ExpressionType binaryType, ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" />, given the left operand, right operand and implementing method, by calling the appropriate factory method.</summary>
        /// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the left operand.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the right operand.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that specifies the implementing method.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        BinaryExpressionSlim MakeBinary(ExpressionType binaryType, ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" />, given the left operand, right operand, implementing method and type conversion function, by calling the appropriate factory method.</summary>
        /// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the left operand.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the right operand.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that specifies the implementing method.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> that represents a type conversion function. This parameter is used only if <paramref name="binaryType" /> is <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> or compound assignment..</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        BinaryExpressionSlim MakeBinary(ExpressionType binaryType, ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> representing a catch statement with the specified elements.</summary>
        /// <param name="type">The <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> of <see cref="T:System.Exception" /> this <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> will handle.</param>
        /// <param name="variable">A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> representing a reference to the <see cref="T:System.Exception" /> object caught by this handler.</param>
        /// <param name="body">The body of the catch statement.</param>
        /// <param name="filter">The body of the <see cref="T:System.Exception" /> filter.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.CatchBlockSlim" />.</returns>
        CatchBlockSlim MakeCatchBlock(TypeSlim type, ParameterExpressionSlim variable, ExpressionSlim body, ExpressionSlim filter);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a jump of the specified <see cref="T:System.Linq.Expressions.GotoExpressionKind" />. The value passed to the label upon jumping can also be specified.</summary>
        /// <param name="kind">The <see cref="T:System.Linq.Expressions.GotoExpressionKind" /> of the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" />.</param>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to <paramref name="kind" />, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim MakeGoto(GotoExpressionKind kind, LabelTargetSlim target, ExpressionSlim value, TypeSlim type);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> that represents accessing an indexed property in an object.</summary>
        /// <param name="instance">The object to which the property belongs. It should be null if the property is <see langword="static" /> (<see langword="shared" /> in Visual Basic).</param>
        /// <param name="indexer">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> representing the property to index.</param>
        /// <param name="arguments">An IEnumerable&lt;Expression&gt; (IEnumerable (Of Expression) in Visual Basic) that contains the arguments that will be used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        IndexExpressionSlim MakeIndex(ExpressionSlim instance, PropertyInfoSlim indexer, IEnumerable<ExpressionSlim> arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that represents accessing either a field or a property.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the object that the member belongs to. This can be null for static members.</param>
        /// <param name="member">The <see cref="T:System.Reflection.MemberInfoSlim" /> that describes the field or property to be accessed.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        MemberExpressionSlim MakeMemberAccess(ExpressionSlim expression, MemberInfoSlim member);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with the specified elements.</summary>
        /// <param name="type">The result type of the try expression. If null, bodh and all handlers must have identical type.</param>
        /// <param name="body">The body of the try block.</param>
        /// <param name="finally">The body of the finally block. Pass null if the try block has no finally block associated with it.</param>
        /// <param name="fault">The body of the fault block. Pass null if the try block has no fault block associated with it.</param>
        /// <param name="handlers">A collection of <see cref="T:System.Linq.Expressions.CatchBlockSlim" />s representing the catch statements to be associated with the try block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        TryExpressionSlim MakeTry(TypeSlim type, ExpressionSlim body, ExpressionSlim @finally, ExpressionSlim fault, IEnumerable<CatchBlockSlim> handlers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />, given an operand, by calling the appropriate factory method.</summary>
        /// <param name="unaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of unary operation.</param>
        /// <param name="operand">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the operand.</param>
        /// <param name="type">The <see cref="T:System.Reflection.TypeSlim" /> that specifies the type to be converted to (pass <see langword="null" /> if not applicable).</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        UnaryExpressionSlim MakeUnary(ExpressionType unaryType, ExpressionSlim operand, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />, given an operand and implementing method, by calling the appropriate factory method.</summary>
        /// <param name="unaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of unary operation.</param>
        /// <param name="operand">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> that represents the operand.</param>
        /// <param name="type">The <see cref="T:System.Reflection.TypeSlim" /> that specifies the type to be converted to (pass <see langword="null" /> if not applicable).</param>
        /// <param name="method">The <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>The <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that results from calling the appropriate factory method.</returns>
        UnaryExpressionSlim MakeUnary(ExpressionType unaryType, ExpressionSlim operand, TypeSlim type, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBindingSlim" /> that represents the recursive initialization of members of a field or property.</summary>
        /// <param name="member">The <see cref="T:System.Reflection.MemberInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBindingSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBindingSlim.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBindingSlim" /> that has the <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBindingSlim" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberMemberBindingSlim.Bindings" /> properties set to the specified values.</returns>
        MemberMemberBindingSlim MemberBind(MemberInfoSlim member, params MemberBindingSlim[] bindings);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBindingSlim" /> that represents the recursive initialization of members of a field or property.</summary>
        /// <param name="member">The <see cref="T:System.Reflection.MemberInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> property equal to.</param>
        /// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBindingSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBindingSlim.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBindingSlim" /> that has the <see cref="P:System.Linq.Expressions.MemberBindingSlim.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBindingSlim" /> and the <see cref="P:System.Linq.Expressions.MemberBindingSlim.Member" /> and <see cref="P:System.Linq.Expressions.MemberMemberBindingSlim.Bindings" /> properties set to the specified values.</returns>
        MemberMemberBindingSlim MemberBind(MemberInfoSlim member, IEnumerable<MemberBindingSlim> bindings);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberInitExpressionSlim" />.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBindingSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberInit" /> and the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.NewExpression" /> and <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.Bindings" /> properties set to the specified values.</returns>
        MemberInitExpressionSlim MemberInit(NewExpressionSlim newExpression, params MemberBindingSlim[] bindings);

        /// <summary>Represents an expression that creates a new object and initializes a property of the object.</summary>
        /// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.NewExpression" /> property equal to.</param>
        /// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBindingSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.Bindings" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberInitExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberInit" /> and the <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.NewExpression" /> and <see cref="P:System.Linq.Expressions.MemberInitExpressionSlim.Bindings" /> properties set to the specified values.</returns>
        MemberInitExpressionSlim MemberInit(NewExpressionSlim newExpression, IEnumerable<MemberBindingSlim> bindings);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic remainder operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Modulo" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Modulo(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic remainder operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Modulo" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Modulo(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a remainder assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ModuloAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ModuloAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a remainder assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ModuloAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ModuloAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a remainder assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ModuloAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ModuloAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic multiplication operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Multiply" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Multiply(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic multiplication operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Multiply" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Multiply(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim MultiplyAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim MultiplyAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim MultiplyAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim MultiplyAssignChecked(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim MultiplyAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a multiplication assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim MultiplyAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic multiplication operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim MultiplyChecked(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic multiplication operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim MultiplyChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an arithmetic negation operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Negate" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        UnaryExpressionSlim Negate(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an arithmetic negation operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Negate" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        UnaryExpressionSlim Negate(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an arithmetic negation operation that has overflow checking.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NegateChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        UnaryExpressionSlim NegateChecked(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an arithmetic negation operation that has overflow checking. The implementing method can be specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NegateChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        UnaryExpressionSlim NegateChecked(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor that takes no arguments.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property set to the specified value.</returns>
        NewExpressionSlim New(ConstructorInfoSlim constructor);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the parameterless constructor of the specified type.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that has a constructor that takes no arguments.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property set to the <see cref="T:System.Reflection.ConstructorInfoSlim" /> that represents the constructor without parameters for the specified type.</returns>
        NewExpressionSlim New(TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor with the specified arguments.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> and <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        NewExpressionSlim New(ConstructorInfoSlim constructor, params ExpressionSlim[] arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor with the specified arguments.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> and <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> properties set to the specified values.</returns>
        NewExpressionSlim New(ConstructorInfoSlim constructor, IEnumerable<ExpressionSlim> arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> collection.</param>
        /// <param name="members">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Reflection.MemberInfoSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Members" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" />, <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> and <see cref="P:System.Linq.Expressions.NewExpressionSlim.Members" /> properties set to the specified values.</returns>
        NewExpressionSlim New(ConstructorInfoSlim constructor, IEnumerable<ExpressionSlim> arguments, IEnumerable<MemberInfoSlim> members);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified as an array.</summary>
        /// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> collection.</param>
        /// <param name="members">An array of <see cref="T:System.Reflection.MemberInfoSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Members" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpressionSlim.Constructor" />, <see cref="P:System.Linq.Expressions.NewExpressionSlim.Arguments" /> and <see cref="P:System.Linq.Expressions.NewExpressionSlim.Members" /> properties set to the specified values.</returns>
        NewExpressionSlim New(ConstructorInfoSlim constructor, IEnumerable<ExpressionSlim> arguments, params MemberInfoSlim[] members);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that represents creating an array that has a specified rank.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that represents the element type of the array.</param>
        /// <param name="bounds">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayBounds" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> property set to the specified value.</returns>
        NewArrayExpressionSlim NewArrayBounds(TypeSlim type, params ExpressionSlim[] bounds);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that represents creating an array that has a specified rank.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that represents the element type of the array.</param>
        /// <param name="bounds">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayBounds" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> property set to the specified value.</returns>
        NewArrayExpressionSlim NewArrayBounds(TypeSlim type, IEnumerable<ExpressionSlim> bounds);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that represents the element type of the array.</param>
        /// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayInit" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> property set to the specified value.</returns>
        NewArrayExpressionSlim NewArrayInit(TypeSlim type, params ExpressionSlim[] initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> that represents the element type of the array.</param>
        /// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> collection.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayInit" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpressionSlim.Expressions" /> property set to the specified value.</returns>
        NewArrayExpressionSlim NewArrayInit(TypeSlim type, IEnumerable<ExpressionSlim> initializers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a bitwise complement operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Not" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        UnaryExpressionSlim Not(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a bitwise complement operation. The implementing method can be specified.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Not" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        UnaryExpressionSlim Not(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an inequality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim NotEqual(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an inequality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="liftToNull">
        ///   <see langword="true" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="true" />; <see langword="false" /> to set <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" /> to <see langword="false" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim NotEqual(ExpressionSlim left, ExpressionSlim right, Boolean liftToNull, MethodInfoSlim method);

        /// <summary>Returns the expression representing the ones complement.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        UnaryExpressionSlim OnesComplement(ExpressionSlim expression);

        /// <summary>Returns the expression representing the ones complement.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        UnaryExpressionSlim OnesComplement(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="OR" /> operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Or" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Or(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise <see langword="OR" /> operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Or" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Or(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise OR assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim OrAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise OR assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim OrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise OR assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim OrAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a conditional <see langword="OR" /> operation that evaluates the second operand only if the first operand evaluates to <see langword="false" />.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrElse" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim OrElse(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a conditional <see langword="OR" /> operation that evaluates the second operand only if the first operand evaluates to <see langword="false" />.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrElse" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim OrElse(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> node that can be used to identify a parameter or a variable in an expression tree.</summary>
        /// <param name="type">The type of the parameter or variable.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> node with the specified name and type.</returns>
        ParameterExpressionSlim Parameter(TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> node that can be used to identify a parameter or a variable in an expression tree.</summary>
        /// <param name="type">The type of the parameter or variable.</param>
        /// <param name="name">The name of the parameter or variable, used for debugging or printing purpose only.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.ParameterExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Parameter" /> and the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> and <see cref="P:System.Linq.Expressions.ParameterExpressionSlim.Name" /> properties set to the specified values.</returns>
        ParameterExpressionSlim Parameter(TypeSlim type, String name);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the assignment of the expression followed by a subsequent decrement by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        UnaryExpressionSlim PostDecrementAssign(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the assignment of the expression followed by a subsequent decrement by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        UnaryExpressionSlim PostDecrementAssign(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the assignment of the expression followed by a subsequent increment by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        UnaryExpressionSlim PostIncrementAssign(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the assignment of the expression followed by a subsequent increment by 1 of the original expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        UnaryExpressionSlim PostIncrementAssign(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising a number to a power.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Power" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Power(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising a number to a power.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Power" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Power(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising an expression to a power and assigning the result back to the expression.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.PowerAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim PowerAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising an expression to a power and assigning the result back to the expression.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.PowerAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim PowerAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents raising an expression to a power and assigning the result back to the expression.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.PowerAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim PowerAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that decrements the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        UnaryExpressionSlim PreDecrementAssign(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that decrements the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        UnaryExpressionSlim PreDecrementAssign(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that increments the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        UnaryExpressionSlim PreIncrementAssign(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that increments the expression by 1 and assigns the result back to the expression.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to apply the operations on.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> that represents the implementing method.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the resultant expression.</returns>
        UnaryExpressionSlim PreIncrementAssign(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that represents accessing a property.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Expression" /> property equal to. This can be null for static properties.</param>
        /// <param name="property">The <see cref="T:System.Reflection.PropertyInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Member" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.MemberExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" /> and the <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Expression" /> and <see cref="P:System.Linq.Expressions.MemberExpressionSlim.Member" /> properties set to the specified values.</returns>
        MemberExpressionSlim Property(ExpressionSlim expression, PropertyInfoSlim property);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> representing the access to an indexed property.</summary>
        /// <param name="instance">The object to which the property belongs. If the property is static/shared, it must be null.</param>
        /// <param name="indexer">The <see cref="T:System.Reflection.PropertyInfoSlim" /> that represents the property to index.</param>
        /// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects that are used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        IndexExpressionSlim Property(ExpressionSlim instance, PropertyInfoSlim indexer, params ExpressionSlim[] arguments);

        /// <summary>Creates an <see cref="T:System.Linq.Expressions.IndexExpressionSlim" /> representing the access to an indexed property.</summary>
        /// <param name="instance">The object to which the property belongs. If the property is static/shared, it must be null.</param>
        /// <param name="indexer">The <see cref="T:System.Reflection.PropertyInfoSlim" /> that represents the property to index.</param>
        /// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Linq.Expressions.ExpressionSlim" /> objects that are used to index the property.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.IndexExpressionSlim" />.</returns>
        IndexExpressionSlim Property(ExpressionSlim instance, PropertyInfoSlim indexer, IEnumerable<ExpressionSlim> arguments);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an expression that has a constant value of type <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Quote" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        UnaryExpressionSlim Quote(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a reference equality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ReferenceEqual(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a reference inequality comparison.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim ReferenceNotEqual(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a rethrowing of an exception.</summary>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a rethrowing of an exception.</returns>
        UnaryExpressionSlim Rethrow();

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a rethrowing of an exception with a given type.</summary>
        /// <param name="type">The new <see cref="T:System.Reflection.TypeSlim" /> of the expression.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a rethrowing of an exception.</returns>
        UnaryExpressionSlim Rethrow(TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a return statement.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Return, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and a null value to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Return(LabelTargetSlim target);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a return statement with the specified type.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Return, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and a null value to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Return(LabelTargetSlim target, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a return statement. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Return(LabelTargetSlim target, ExpressionSlim value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> representing a return statement with the specified type. The value passed to the label upon jumping can be specified.</summary>
        /// <param name="target">The <see cref="T:System.Linq.Expressions.LabelTargetSlim" /> that the <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> will jump to.</param>
        /// <param name="value">The value that will be passed to the associated label upon jumping.</param>
        /// <param name="type">An <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.GotoExpressionSlim" /> with <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Kind" /> equal to Continue, the <see cref="P:System.Linq.Expressions.GotoExpressionSlim.Target" /> property set to <paramref name="target" />, the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property set to <paramref name="type" />, and <paramref name="value" /> to be passed to the target label upon jumping.</returns>
        GotoExpressionSlim Return(LabelTargetSlim target, ExpressionSlim value, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim RightShift(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim RightShift(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim RightShiftAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim RightShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a bitwise right-shift assignment operation.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShiftAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim RightShiftAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic subtraction operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Subtract" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Subtract(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic subtraction operation that does not have overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Subtract" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim Subtract(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim SubtractAssign(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim SubtractAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that does not have overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssign" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim SubtractAssign(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim SubtractAssignChecked(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim SubtractAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents a subtraction assignment operation that has overflow checking.</summary>
        /// <param name="left">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractAssignChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Conversion" /> properties set to the specified values.</returns>
        BinaryExpressionSlim SubtractAssignChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method, LambdaExpressionSlim conversion);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic subtraction operation that has overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> properties set to the specified values.</returns>
        BinaryExpressionSlim SubtractChecked(ExpressionSlim left, ExpressionSlim right);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that represents an arithmetic subtraction operation that has overflow checking.</summary>
        /// <param name="left">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" /> property equal to.</param>
        /// <param name="right">A <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        BinaryExpressionSlim SubtractChecked(ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement without a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        SwitchExpressionSlim Switch(ExpressionSlim switchValue, params SwitchCaseSlim[] cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        SwitchExpressionSlim Switch(ExpressionSlim switchValue, ExpressionSlim defaultBody, params SwitchCaseSlim[] cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        SwitchExpressionSlim Switch(ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, params SwitchCaseSlim[] cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        SwitchExpressionSlim Switch(ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, IEnumerable<SwitchCaseSlim> cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case.</summary>
        /// <param name="type">The result type of the switch.</param>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        SwitchExpressionSlim Switch(TypeSlim type, ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, IEnumerable<SwitchCaseSlim> cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> that represents a <see langword="switch" /> statement that has a default case..</summary>
        /// <param name="type">The result type of the switch.</param>
        /// <param name="switchValue">The value to be tested against each case.</param>
        /// <param name="defaultBody">The result of the switch if <paramref name="switchValue" /> does not match any of the cases.</param>
        /// <param name="comparison">The equality comparison method to use.</param>
        /// <param name="cases">The set of cases for this switch expression.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</returns>
        SwitchExpressionSlim Switch(TypeSlim type, ExpressionSlim switchValue, ExpressionSlim defaultBody, MethodInfoSlim comparison, params SwitchCaseSlim[] cases);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchCaseSlim" /> for use in a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" />.</summary>
        /// <param name="body">The body of the case.</param>
        /// <param name="testValues">The test values of the case.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchCaseSlim" />.</returns>
        SwitchCaseSlim SwitchCase(ExpressionSlim body, params ExpressionSlim[] testValues);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.SwitchCaseSlim" /> object to be used in a <see cref="T:System.Linq.Expressions.SwitchExpressionSlim" /> object.</summary>
        /// <param name="body">The body of the case.</param>
        /// <param name="testValues">The test values of the case.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.SwitchCaseSlim" />.</returns>
        SwitchCaseSlim SwitchCase(ExpressionSlim body, IEnumerable<ExpressionSlim> testValues);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a throwing of an exception.</summary>
        /// <param name="value">An <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the exception.</returns>
        UnaryExpressionSlim Throw(ExpressionSlim value);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a throwing of an exception with a given type.</summary>
        /// <param name="value">An <see cref="T:System.Linq.Expressions.ExpressionSlim" />.</param>
        /// <param name="type">The new <see cref="T:System.Reflection.TypeSlim" /> of the expression.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents the exception.</returns>
        UnaryExpressionSlim Throw(ExpressionSlim value, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with any number of catch statements and neither a fault nor finally block.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="handlers">The array of zero or more <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> expressions representing the catch statements to be associated with the try block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        TryExpressionSlim TryCatch(ExpressionSlim body, params CatchBlockSlim[] handlers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with any number of catch statements and a finally block.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="finally">The body of the finally block.</param>
        /// <param name="handlers">The array of zero or more <see cref="T:System.Linq.Expressions.CatchBlockSlim" /> expressions representing the catch statements to be associated with the try block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        TryExpressionSlim TryCatchFinally(ExpressionSlim body, ExpressionSlim @finally, params CatchBlockSlim[] handlers);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with a fault block and no catch statements.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="fault">The body of the fault block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        TryExpressionSlim TryFault(ExpressionSlim body, ExpressionSlim fault);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TryExpressionSlim" /> representing a try block with a finally block and no catch statements.</summary>
        /// <param name="body">The body of the try block.</param>
        /// <param name="finally">The body of the finally block.</param>
        /// <returns>The created <see cref="T:System.Linq.Expressions.TryExpressionSlim" />.</returns>
        TryExpressionSlim TryFinally(ExpressionSlim body, ExpressionSlim @finally);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an explicit reference or boxing conversion where <see langword="null" /> is supplied if the conversion fails.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="T:System.Reflection.TypeSlim" /> to set the <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.TypeAs" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> properties set to the specified values.</returns>
        UnaryExpressionSlim TypeAs(ExpressionSlim expression, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TypeBinaryExpressionSlim" /> that compares run-time type identity.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="T:System.Linq.Expressions.ExpressionSlim" /> property equal to.</param>
        /// <param name="type">A <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.TypeOperand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.TypeBinaryExpressionSlim" /> for which the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property is equal to <see cref="M:System.Linq.Expressions.Expression.TypeEqual(System.Linq.Expressions.Expression,System.Type)" /> and for which the <see cref="T:System.Linq.Expressions.ExpressionSlim" /> and <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.TypeOperand" /> properties are set to the specified values.</returns>
        TypeBinaryExpressionSlim TypeEqual(ExpressionSlim expression, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.TypeBinaryExpressionSlim" />.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.Expression" /> property equal to.</param>
        /// <param name="type">A <see cref="P:System.Linq.Expressions.ExpressionSlim.Type" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.TypeOperand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.TypeBinaryExpressionSlim" /> for which the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property is equal to <see cref="F:System.Linq.Expressions.ExpressionType.TypeIs" /> and for which the <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.Expression" /> and <see cref="P:System.Linq.Expressions.TypeBinaryExpressionSlim.TypeOperand" /> properties are set to the specified values.</returns>
        TypeBinaryExpressionSlim TypeIs(ExpressionSlim expression, TypeSlim type);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a unary plus operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.UnaryPlus" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property set to the specified value.</returns>
        UnaryExpressionSlim UnaryPlus(ExpressionSlim expression);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents a unary plus operation.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> property equal to.</param>
        /// <param name="method">A <see cref="T:System.Reflection.MethodInfoSlim" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> property equal to.</param>
        /// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that has the <see cref="P:System.Linq.Expressions.ExpressionSlim.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.UnaryPlus" /> and the <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpressionSlim.Method" /> properties set to the specified values.</returns>
        UnaryExpressionSlim UnaryPlus(ExpressionSlim expression, MethodInfoSlim method);

        /// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" /> that represents an explicit unboxing.</summary>
        /// <param name="expression">An <see cref="T:System.Linq.Expressions.ExpressionSlim" /> to unbox.</param>
        /// <param name="type">The new <see cref="T:System.Reflection.TypeSlim" /> of the expression.</param>
        /// <returns>An instance of <see cref="T:System.Linq.Expressions.UnaryExpressionSlim" />.</returns>
        UnaryExpressionSlim Unbox(ExpressionSlim expression, TypeSlim type);

    }
}
