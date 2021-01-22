// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Reactive Excel implementation using classic Rx to demonstrate the concepts around
// building reactive computational graphs.
//

//
// Revision history:
//
// BD - November 2014 - Created this file.
//

namespace Rxcel
{
    internal abstract class ExcelExpression
    {
        public abstract ExcelExpressionKind Kind { get; }

        protected internal abstract TResult Accept<TResult>(ExcelExpressionVisitor<TResult> visitor);

        public static CellExcelExpression Cell(string cell)
        {
            return new CellExcelExpression(cell);
        }

        public static RangeExcelExpression Range(string start, string end)
        {
            return new RangeExcelExpression(start, end);
        }

        public static NumberExcelExpression Number(double? value)
        {
            return new NumberExcelExpression(value);
        }

        public static BinaryExcelExpression Add(ExcelExpression left, ExcelExpression right)
        {
            return new BinaryExcelExpression(ExcelExpressionKind.Add, left, right);
        }

        public static BinaryExcelExpression Subtract(ExcelExpression left, ExcelExpression right)
        {
            return new BinaryExcelExpression(ExcelExpressionKind.Subtract, left, right);
        }

        public static BinaryExcelExpression Multiply(ExcelExpression left, ExcelExpression right)
        {
            return new BinaryExcelExpression(ExcelExpressionKind.Multiply, left, right);
        }

        public static BinaryExcelExpression Divide(ExcelExpression left, ExcelExpression right)
        {
            return new BinaryExcelExpression(ExcelExpressionKind.Divide, left, right);
        }

        public static BinaryExcelExpression Modulo(ExcelExpression left, ExcelExpression right)
        {
            return new BinaryExcelExpression(ExcelExpressionKind.Modulo, left, right);
        }

        public static FormulaExcelExpression Formula(string formula, params ExcelExpression[] args)
        {
            return new FormulaExcelExpression(formula, args);
        }
    }
}
