// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

using System.Linq.Expressions;

using Nuqleon.DataModel.TypeSystem;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    internal class ObjectComparator : DataTypeObjectEqualityComparator
    {
        public static ObjectComparator CreateInstance()
        {
            var objectComparator = new ObjectComparator();
            var typeComparator = new TypeComparator();
            var expressionComparator = new ExpressionComparator(typeComparator, objectComparator);
            objectComparator.ExpressionComparator = expressionComparator;
            return objectComparator;
        }

        public ExpressionComparator ExpressionComparator
        {
            get;
            set;
        }

        protected override bool EqualsQuotation(object expected, object actual, QuotationDataType expectedDataType, QuotationDataType actualDataType)
        {
            return ExpressionComparator.Equals((Expression)expected, (Expression)actual);
        }
    }
}
