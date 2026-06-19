// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Reaqtor.TestingFramework
{
    public class StructuralServiceOperationEqualityComparer : ServiceOperationEqualityComparer
    {
        public override bool Equals(ServiceOperation x, ServiceOperation y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.ToString() == y.ToString() && base.Equals(x, y);
        }

        protected override bool Equals(Expression x, Expression y)
        {
            var comparer = new ExpressionEqualityComparer(() => new Comparator(new StructuralTypeEqualityComparator()));
            var res = comparer.Equals(x, y);
            return res;
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            public Comparator(StructuralTypeEqualityComparator typeComparer)
                : base(typeComparer, typeComparer.MemberComparer, EqualityComparer<object>.Default, EqualityComparer<CallSiteBinder>.Default)
            {
            }

            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }
    }
}
