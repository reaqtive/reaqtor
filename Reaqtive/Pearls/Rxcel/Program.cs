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

#pragma warning disable CA1303 // No localization in sample.

using System;
using System.Globalization;
using System.Windows.Forms;

[assembly: CLSCompliant(true)]

namespace Rxcel
{
    public static partial class Program
    {
        [STAThread]
        public static void Main()
        {
            using var frm = new Form
            {
                Text = "Rxcel",
                Width = 1200,
                Height = 400,
            };

            var txt = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Left = 0,
                Top = 0,
                Width = frm.Width
            };

            frm.Controls.Add(txt);

            var dg = new DataGridView
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Left = 0,
                Top = txt.Height,
                Width = frm.Width,
                Height = frm.Height - txt.Height
            };

            var C = 10;
            var R = 10;

            var sheet = new Sheet(R, C);

            for (var c = 'A'; c < 'A' + C; c++)
            {
                dg.Columns.Add(c.ToString(), c.ToString());
            }

            for (var i = 0; i < R; i++)
            {
                var j = dg.Rows.Add();
                dg.Rows[j].HeaderCell.Value = (i + 1).ToString(CultureInfo.InvariantCulture);
            }

            for (var r = 0; r < dg.Rows.Count - 1; r++)
            {
                var row = dg.Rows[r];
                for (var c = 0; c < dg.Columns.Count; c++)
                {
                    row.Cells[c].Value = sheet[r, c];
                }
            }

            frm.Controls.Add(dg);

            txt.KeyDown += (o, e) =>
            {
                if (e.KeyCode is Keys.Enter or Keys.Return)
                {
                    if (dg.SelectedCells.Count == 1)
                    {
                        var cell = (Cell)dg.SelectedCells[0].Value;

                        cell.Value = txt.Text;

                        dg.Refresh();
                    }
                }
            };

            dg.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dg.MultiSelect = false;

            dg.SelectionChanged += (o, e) =>
            {
                if (dg.SelectedCells.Count == 1)
                {
                    var sel = (Cell)dg.SelectedCells[0].Value;
                    txt.Text = sel?.Value;
                }
                else
                {
                    txt.Text = "";
                }
            };

            var editing = default(DataGridViewCell);
            var editingCell = default(Cell);

            dg.CellBeginEdit += (o, e) =>
            {
                editing = dg.Rows[e.RowIndex].Cells[e.ColumnIndex];
                editingCell = (Cell)editing.Value;
            };

            dg.CellEndEdit += (o, e) =>
            {
                if (editing != null)
                {
                    if (editing.Value is string value)
                    {
                        editingCell.Value = value;
                        editing.Value = editingCell;
                        dg.Refresh();
                    }
                }
            };

            Application.Run(frm);
        }
    }
}
