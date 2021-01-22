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
    internal abstract class ExcelExpressionVisitor<TResult>
    {
        public TResult Visit(ExcelExpression node)
        {
            return node.Accept(this);
        }

        protected internal abstract TResult VisitBinary(BinaryExcelExpression node);
        protected internal abstract TResult VisitNumber(NumberExcelExpression node);
        protected internal abstract TResult VisitCell(CellExcelExpression node);
        protected internal abstract TResult VisitRange(RangeExcelExpression node);
        protected internal abstract TResult VisitFormula(FormulaExcelExpression node);
    }
}
