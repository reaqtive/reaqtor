// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Visitor to replace parameter occurrences.
//
// BD - October 2014
//

using System.Linq.Expressions;

namespace OperatorFusion
{
    internal sealed class CloseStateParameter : ExpressionVisitor
    {
        private readonly ParameterExpression _original;
        private readonly ParameterExpression _replacement;

        public CloseStateParameter(ParameterExpression original, ParameterExpression replacement)
        {
            _original = original;
            _replacement = replacement;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression == _original && node.Member.DeclaringType == node.Expression.Type)
            {
                var m = node.Member;
                var f = _replacement.Type.GetField(m.Name);

                return Expression.MakeMemberAccess(_replacement, f);
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node == _original)
            {
                return _replacement;
            }

            return base.VisitParameter(node);
        }
    }
}
