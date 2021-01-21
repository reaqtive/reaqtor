// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    internal sealed class ClonedExpressionEqualityComparator : ExpressionEqualityComparator
    {
        public override bool Equals(Expression x, Expression y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            return base.Equals(x, y) && (!object.ReferenceEquals(x, y) || x is ParameterExpression);
        }

        public override bool Equals(ElementInit x, ElementInit y) => base.Equals(x, y);

        public override bool Equals(MemberBinding x, MemberBinding y) => base.Equals(x, y);
    }
}
