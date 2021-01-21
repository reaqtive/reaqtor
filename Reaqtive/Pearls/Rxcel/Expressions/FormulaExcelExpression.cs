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

using System.Linq;

namespace Rxcel
{
    internal sealed class FormulaExcelExpression : ExcelExpression
    {
        internal FormulaExcelExpression(string name, params ExcelExpression[] args)
        {
            Name = name;
            Arguments = args;
        }

        public override ExcelExpressionKind Kind => ExcelExpressionKind.Formula;

        public string Name { get; }

        public ExcelExpression[] Arguments { get; }

        protected internal override TResult Accept<TResult>(ExcelExpressionVisitor<TResult> visitor)
        {
            return visitor.VisitFormula(this);
        }

        public override string ToString()
        {
            return Name + "(" + string.Join(", ", Arguments.Select(a => a.ToString())) + ")";
        }
    }
}
