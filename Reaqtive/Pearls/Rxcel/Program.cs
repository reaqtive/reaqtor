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
#pragma warning disable CA1031 // Do not catch general exception types

#if NO_UI

using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Rxcel
{
    public static partial class Program
    {
        public static void Main()
        {
            //     |    A    |    B    |
            // ----+---------+---------+
            //   1 |         |         |
            // ----+---------+---------+
            //   2 |         |         |
            // ----+---------+---------+
            //

            const string LeftColLine = "----+";
            const string CellColLine = "---------+";

            var C = Math.Min((Console.WindowWidth - LeftColLine.Length) / CellColLine.Length, 26);
            var R = Math.Min((Console.WindowHeight - 2 /* header */ - 5 /* prompts while editing and avoid scroll */) / 2 /* lines per row */, 32);

            var sheet = new Sheet(R, C);

            void Draw()
            {
                Console.Clear();

                Console.Write(new string(' ', LeftColLine.Length - 1) + "|");

                for (var c = 'A'; c < 'A' + C; c++)
                {
                    Console.Write(Center(c.ToString(), CellColLine.Length - 1) + "|");
                }

                Console.WriteLine();

                var line = LeftColLine + string.Join("", Enumerable.Repeat(CellColLine, C));
                Console.WriteLine(line);

                var maxCellWidth = CellColLine.Length - 3 /* spaces and + */;

                for (var r = 1; r <= R; r++)
                {
                    Console.Write(" " + string.Format(CultureInfo.CurrentCulture, "{0,-" + (LeftColLine.Length - 2 /* space and + */) + "}", r) + "|");

                    for (int c = 0; c < C; c++)
                    {
                        Console.Write(" {0," + maxCellWidth + "} |", sheet[r - 1, c].ToString());
                    }

                    Console.WriteLine();
                    Console.WriteLine(line);
                }
            }

            while (true)
            {
                Draw();

                Console.Write("Enter cell (O = Open, S = Save, X = Exit): ");
                var cell = Console.ReadLine().Trim();

                var option = cell.ToUpper(CultureInfo.InvariantCulture);

                if (option == "O")
                {
                    Console.Write("Enter file name: ");
                    var file = Console.ReadLine();

                    try
                    {
                        sheet = Sheet.Load(file);
                    }
                    catch (Exception ex)
                    {
                        ShowError("Error: " + ex);
                        continue;
                    }

                    Draw();
                    continue;
                }
                else if (option == "S")
                {
                    Console.Write("Enter file name: ");
                    var file = Console.ReadLine();

                    try
                    {
                        sheet.Save(file);
                    }
                    catch (Exception ex)
                    {
                        ShowError("Error: " + ex);
                    }

                    continue;
                }
                else if (option == "X")
                {
                    return;
                }

                if (!Parser.TryParseCell(cell, out var row, out var col) || row <= 0 || row > R || col <= 0 || col > C)
                {
                    ShowError("Invalid cell. Press ENTER to retry.");
                    continue;
                }

                Draw();

                Console.WriteLine("Current formula or value: " + sheet[row - 1, col - 1].Value);

                Console.Write("Enter formula or value (use _ to cancel edit): ");
                var formula = Console.ReadLine().Trim();

                if (formula == "_")
                {
                    continue;
                }

                sheet[row - 1, col - 1].Value = formula;
            }

            static string Center(string s, int width)
            {
                if (s.Length >= width)
                {
                    return s;
                }

                int leftPadding = (width - s.Length) / 2;
                int rightPadding = width - s.Length - leftPadding;

                return new string(' ', leftPadding) + s + new string(' ', rightPadding);
            }

            static void ShowError(string msg)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);
                Console.ResetColor();
                Console.ReadLine();
            }
        }
    }
}

#else

using System;
using System.Globalization;
using System.Windows.Forms;

namespace Rxcel
{
    public static partial class Program
    {
        [STAThread]
        public static void Main()
        {
            var open = new ToolStripMenuItem("&Open") { ShortcutKeys = Keys.Control | Keys.O };
            var save = new ToolStripMenuItem("&Save") { ShortcutKeys = Keys.Control | Keys.S };

            var menu = new MenuStrip()
            {
                Items =
                {
                    new ToolStripMenuItem("&File")
                    {
                        DropDownItems =
                        {
                            open,
                            save
                        }
                    }
                }
            };

            using var frm = new Form
            {
                Text = "Rxcel",
                Width = 1200,
                Height = 400,
                MainMenuStrip = menu
            };

            var txt = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Left = 0,
                Top = 0,
                Width = frm.Width
            };

            var panel = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Left = 0,
                Top = menu.Height,
                Width = frm.Width,
                Height = frm.Height - txt.Height
            };

            panel.Controls.Add(txt);

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

            void DrawSheet()
            {
                dg.Rows.Clear();
                dg.Columns.Clear();

                for (var c = 'A'; c < 'A' + sheet.ColumnCount; c++)
                {
                    dg.Columns.Add(c.ToString(), c.ToString());
                }

                for (var i = 0; i < sheet.ColumnCount; i++)
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
            }

            DrawSheet();

            panel.Controls.Add(dg);

            open.Click += (_1, _2) =>
            {
                string file;

                using (var dlg = new OpenFileDialog())
                {
                    if (dlg.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    file = dlg.FileName;
                }

                try
                {
                    var newSheet = Sheet.Load(file);
                    if (newSheet != null)
                    {
                        sheet = newSheet;
                        DrawSheet();
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            };

            save.Click += (_1, _2) =>
            {
                string file;

                using (var dlg = new SaveFileDialog())
                {
                    if (dlg.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    file = dlg.FileName;
                }

                try
                {
                    sheet.Save(file);
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            };

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

            frm.Controls.Add(panel);
            frm.Controls.Add(menu);

            Application.Run(frm);

            static void ShowError(Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Rxcel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

#endif
