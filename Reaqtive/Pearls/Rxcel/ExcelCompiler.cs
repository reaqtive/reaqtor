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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rxcel
{
    internal static class ExcelCompiler
    {
        public static LambdaExpression Compile(ExcelExpression expression)
        {
            var comp = new Impl();

            var res = comp.Visit(expression);

            return Expression.Lambda(res, comp._cellReferences.Values);
        }

        private sealed class Impl : ExcelExpressionVisitor<Expression>
        {
            public Dictionary<string, ParameterExpression> _cellReferences = new();

            protected internal override Expression VisitBinary(BinaryExcelExpression node)
            {
                var kind = node.Kind switch
                {
                    ExcelExpressionKind.Add => ExpressionType.Add,
                    ExcelExpressionKind.Subtract => ExpressionType.Subtract,
                    ExcelExpressionKind.Multiply => ExpressionType.Multiply,
                    ExcelExpressionKind.Divide => ExpressionType.Divide,
                    ExcelExpressionKind.Modulo => ExpressionType.Modulo,
                    _ => throw new InvalidOperationException(),
                };

                return Expression.MakeBinary(kind, Visit(node.Left), Visit(node.Right));
            }

            protected internal override Expression VisitNumber(NumberExcelExpression node)
            {
                return Expression.Constant(node.Value, typeof(double?));
            }

            protected internal override Expression VisitCell(CellExcelExpression node)
            {
                return GetCellParameter(node.Cell);
            }

            private ParameterExpression GetCellParameter(string str)
            {
                if (!_cellReferences.TryGetValue(str, out var cell))
                {
                    cell = Expression.Parameter(typeof(double?), str);
                    _cellReferences[str] = cell;
                }

                return cell;
            }

            protected internal override Expression VisitFormula(FormulaExcelExpression node)
            {
                var args = Expression.NewArrayInit(typeof(double?), node.Arguments.Select(Visit).SelectMany(Flatten).ToArray());

                var mtd = node.Name.ToUpper(CultureInfo.InvariantCulture) switch
                {
                    "SUM" => InfoOf((double?[] xs) => xs.Sum()),
                    "AVERAGE" => InfoOf((double?[] xs) => xs.Average()),
                    _ => throw new InvalidOperationException("Unknown function: " + node.Name)
                };

                return Expression.Call(mtd, args);
            }

            private static IEnumerable<Expression> Flatten(Expression e)
            {
                if (e is RuntimeVariablesExpression r)
                {
                    return r.Variables;
                }
                else
                {
                    return new[] { e };
                }
            }

            private static MethodInfo InfoOf<T, R>(Expression<Func<T, R>> f)
            {
                return ((MethodCallExpression)f.Body).Method;
            }

            protected internal override Expression VisitRange(RangeExcelExpression node)
            {
                if (!Parser.TryParseCell(node.Start, out var beginRow, out var beginCol) || !Parser.TryParseCell(node.End, out var endRow, out var endCol))
                {
                    throw new InvalidOperationException("Unexpected range.");
                }

                var firstRow = Math.Min(beginRow, endRow);
                var firstCol = Math.Min(beginCol, endCol);

                var lastRow = Math.Max(beginRow, endRow);
                var lastCol = Math.Max(beginCol, endCol);

                var ps = new List<ParameterExpression>();

                for (var r = firstRow; r <= lastRow; r++)
                {
                    for (var c = firstCol; c <= lastCol; c++)
                    {
                        var p = GetCellParameter(ToBase26(c) + r);
                        ps.Add(p);
                    }
                }

                return Expression.RuntimeVariables(ps);
            }
        }

        public static string ToBase26(int x)
        {
            var res = "";

            while (x > 0)
            {
                var r = (x - 1) % 26;

                res = ((char)(r + 'A')).ToString() + res;

                x = (x - 1) / 26;
            }

            return res;
        }
    }
}
