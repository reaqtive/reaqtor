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
    }
}
