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

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;

namespace Rxcel
{
    internal sealed class Sheet
    {
        private readonly Cell[,] _cells;

        public Sheet(int rows, int columns)
        {
            _cells = new Cell[rows, columns];

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    _cells[r, c] = new Cell(this);
                }
            }
        }

        public int RowCount => _cells.GetLength(0);

        public int ColumnCount => _cells.GetLength(1);

        public Cell this[int row, int column] => _cells[row, column];

        public void Save(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return;
            }

            var ctor = typeof(Sheet).GetConstructor(new[] { typeof(int), typeof(int) });
            var index = typeof(Sheet).GetProperty("Item", new[] { typeof(int), typeof(int) });
            var value = typeof(Cell).GetProperty(nameof(Cell.Value));

            var sheet = Expression.Parameter(typeof(Sheet));

            var initCells = new List<Expression>();

            for (var i = 0; i < RowCount; i++)
            {
                for (var j = 0; j < ColumnCount; j++)
                {
                    var val = this[i, j].Value;

                    if (!string.IsNullOrEmpty(val))
                    {
                        initCells.Add(
                            Expression.Assign(
                                Expression.Property(
                                    Expression.MakeIndex(
                                        sheet,
                                        index,
                                        new[]
                                        {
                                            Expression.Constant(i),
                                            Expression.Constant(j)
                                        }
                                    ),
                                    value
                                ),
                                Expression.Constant(val, typeof(string))
                            )
                        );
                    }
                }
            }

            var body =
                Expression.Block(
                    new[] { sheet },
                    new Expression[]
                    {
                        Expression.Assign(
                            sheet,
                            Expression.New(
                                ctor,
                                Expression.Constant(RowCount),
                                Expression.Constant(ColumnCount)
                            )
                        ),
                        Expression.Block(initCells),
                        sheet
                    }
                );

            var expr = Expression.Lambda<Func<Sheet>>(body);
            var slim = expr.ToExpressionSlim();

            var ser = GetSerializer();

            var json = ser.Serialize(slim);

            File.WriteAllText(file, json);
        }

        public static Sheet Load(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return null;
            }

            var json = File.ReadAllText(file);

            var ser = GetSerializer();

            var slim = ser.Deserialize(json);
            var expr = (Expression<Func<Sheet>>)slim.ToExpression();

            return expr.Compile()();
        }

        private static BonsaiExpressionSerializer GetSerializer()
        {
            return new BonsaiExpressionSerializer(
                _ => o => o switch
                {
                    string s => Nuqleon.Json.Expressions.Expression.String(s),
                    int x => Nuqleon.Json.Expressions.Expression.Number(x.ToString(CultureInfo.InvariantCulture)),
                    null => Nuqleon.Json.Expressions.Expression.Null(),
                    _ => throw new NotSupportedException()
                },
                _ => e => e switch
                {
                    Nuqleon.Json.Expressions.ConstantExpression c => c.NodeType switch
                    {
                        Nuqleon.Json.Expressions.ExpressionType.String => (string)c.Value,
                        Nuqleon.Json.Expressions.ExpressionType.Number => int.Parse((string)c.Value, CultureInfo.InvariantCulture),
                        Nuqleon.Json.Expressions.ExpressionType.Null => null,
                        _ => throw new NotSupportedException()
                    },
                    _ => throw new NotSupportedException()
                }
            );
        }
    }
}
