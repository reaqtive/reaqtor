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
    internal sealed class RangeExcelExpression : ExcelExpression
    {
        internal RangeExcelExpression(string start, string end)
        {
            Start = start;
            End = end;
        }

        public string Start { get; }

        public string End { get; }

        public override ExcelExpressionKind Kind => ExcelExpressionKind.Range;

        protected internal override TResult Accept<TResult>(ExcelExpressionVisitor<TResult> visitor)
        {
            return visitor.VisitRange(this);
        }

        public override string ToString()
        {
            return Start + ":" + End;
        }
    }
}
