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
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Windows.Forms;

namespace Rxcel
{
    internal sealed class Cell : IDisposable
    {
        private readonly Sheet _sheet;
        private readonly BehaviorSubject<double?> _subject;
        private readonly SerialDisposable _subscription;
        private string _formula;
        private LambdaExpression _expr;

        public Cell(Sheet sheet)
        {
            _sheet = sheet;
            _subject = new BehaviorSubject<double?>(null);
            _subscription = new SerialDisposable();
        }

        public string Value
        {
            get => !string.IsNullOrEmpty(_formula) ? _formula : _subject.Value.ToString();

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _expr = null;
                    _formula = null;
                    _subscription.Disposable = Disposable.Empty;
                    _subject.OnNext(null);
                }
                else if (double.TryParse(value, out var v))
                {
                    _expr = null;
                    _formula = null;
                    _subscription.Disposable = Disposable.Empty;
                    _subject.OnNext(v);
                }
                else
                {
                    var text = value.TrimStart();

                    if (text.StartsWith("=", StringComparison.Ordinal))
                    {
                        text = text[1..];

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (By design to show errors to the user.)

                        try
                        {
                            var formula = new Parser().Parse(text);
                            var f = ExcelCompiler.Compile(formula);
                            _expr = f;
                            CreateSubscription(f);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Invalid formula: " + ex.Message);
                            return;
                        }

#pragma warning restore CA1031
#pragma warning restore IDE0079

                        _formula = value;
                    }
                    else
                    {
                        MessageBox.Show("Text input not supported. Did you mean to write a formula that starts with =?");
                    }
                }
            }
        }

        public override string ToString()
        {
            return _subject.Value.ToString();
        }

        private void CreateSubscription(LambdaExpression f)
        {
            var n = f.Parameters.Count;

            if (n >= CombineLatestMethods.s_combineLatest.Length)
            {
                throw new InvalidOperationException("Formula too complex."); // TODO: compile into binary tree
            }

            var mtd = CombineLatestMethods.s_combineLatest[n - 1];

            var cells = new List<IObservable<double?>>();
            var formulae = new Queue<Cell>();

            foreach (var p in f.Parameters)
            {
                if (!Parser.TryParseCell(p.Name, out var row, out var col))
                {
                    throw new InvalidOperationException("Invalid cell reference: " + p.Name);
                }

                if (row > _sheet.RowCount || col > _sheet.ColumnCount)
                {
                    throw new InvalidOperationException("Cell out of range: " + p.Name);
                }

                row--;
                col--;

                var cell = _sheet[row, col];
                cells.Add(cell._subject);
                formulae.Enqueue(cell);
            }

            var visited = new HashSet<Cell>();

            while (formulae.Count > 0)
            {
                var cell = formulae.Dequeue();

                if (visited.Add(cell))
                {
                    if (cell._expr != null)
                    {
                        foreach (var p in cell._expr.Parameters)
                        {
                            if (!Parser.TryParseCell(p.Name, out var row, out var col))
                            {
                                throw new InvalidOperationException("Invalid cell reference: " + p.Name);
                            }

                            formulae.Enqueue(_sheet[row - 1, col - 1]);
                        }
                    }
                }
            }

            if (visited.Contains(this))
            {
                throw new InvalidOperationException("Cycle detected.");
            }

            var res = Expression.Call(mtd, cells.Select(c => Expression.Constant(c)).Concat(new Expression[] { f }).ToArray());

            var src = Expression.Lambda<Func<IObservable<double?>>>(res).Compile()();

            _subscription.Disposable = src.Subscribe(_subject);
        }

        public void Dispose()
        {
            _subscription.Dispose();
            _subject.Dispose();
        }
    }
}
