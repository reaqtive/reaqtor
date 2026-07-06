// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Remoting.Metadata
{
    public class PropertyComparisonValueFinder<T> : ExpressionVisitor
    {
        private readonly string _memberName;
        private bool _found;

        public PropertyComparisonValueFinder(string memberName)
        {
            _memberName = memberName;
        }

        public T Value
        {
            get;
            private set;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Debug.Assert(node != null);

            if (node.NodeType == ExpressionType.Equal)
            {
                if (IsPropertyAccess(node.Left))
                {
                    Value = !_found ? (T)node.Right.Evaluate() : default;
                    _found = true;
                }
                else if (IsPropertyAccess(node.Right))
                {
                    Value = !_found ? (T)node.Left.Evaluate() : default;
                    _found = true;
                }
            }

            return base.VisitBinary(node);
        }

        private bool IsPropertyAccess(Expression node)
        {
            if (node is MemberExpression memberAccess && memberAccess.Member.MemberType == MemberTypes.Property && memberAccess.Member.Name == _memberName)
            {
                return true;
            }

            return false;
        }
    }
}
