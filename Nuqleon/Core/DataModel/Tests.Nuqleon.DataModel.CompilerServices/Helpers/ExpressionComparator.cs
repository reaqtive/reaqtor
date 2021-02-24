// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    internal class ExpressionComparator : ExpressionEqualityComparator
    {
        private readonly IEqualityComparer<object> _objectComparer;

        public ExpressionComparator(TypeComparator typeComparer, ObjectComparator objectComparer)
            : base(typeComparer, typeComparer.MemberComparer, objectComparer, EqualityComparer<CallSiteBinder>.Default)
        {
            objectComparer.ExpressionComparator = this;
            _objectComparer = objectComparer;
        }

        public static ExpressionComparator CreateInstance()
        {
            return new ExpressionComparator(new TypeComparator(), ObjectComparator.CreateInstance());
        }

        public override bool Equals(Expression x, Expression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.NodeType != y.NodeType)
            {
                var xValue = x.Evaluate();
                var yValue = y.Evaluate();
                return _objectComparer.Equals(xValue, yValue);
            }

            return base.Equals(x, y);
        }
    }
}
