// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    internal sealed class ShallowExpressionTreeNodeEqualityComparer : IEqualityComparer<ExpressionTreeNode>
    {
        internal static readonly IEqualityComparer<ExpressionTreeNode> Instance = new ShallowExpressionTreeNodeEqualityComparer();
        private readonly ShallowExpressionEquals _equals = new();

        private ShallowExpressionTreeNodeEqualityComparer()
        {
        }

        public bool Equals(ExpressionTreeNode x, ExpressionTreeNode y)
        {
            if (x.NodeType != y.NodeType)
                return false;

            return x.NodeType switch
            {
                ExpressionTreeNodeType.Expression => Equals((ExpressionExpressionTreeNode)x, (ExpressionExpressionTreeNode)y),
                ExpressionTreeNodeType.ElementInit => Equals((ElementInitExpressionTreeNode)x, (ElementInitExpressionTreeNode)y),
                ExpressionTreeNodeType.MemberAssignment or ExpressionTreeNodeType.MemberListBinding or ExpressionTreeNodeType.MemberMemberBinding => Equals((MemberBindingExpressionTreeNode)x, (MemberBindingExpressionTreeNode)y),
                _ => throw new NotImplementedException(),
            };
        }

        private bool Equals(ExpressionExpressionTreeNode x, ExpressionExpressionTreeNode y) => _equals.Equals(x.Expression, y.Expression);

        private bool Equals(ElementInitExpressionTreeNode x, ElementInitExpressionTreeNode y) => _equals.Equals(x.ElementInit, y.ElementInit);

        private bool Equals(MemberBindingExpressionTreeNode x, MemberBindingExpressionTreeNode y) => _equals.Equals(x.MemberBinding, y.MemberBinding);

        public int GetHashCode(ExpressionTreeNode obj) => obj.NodeType switch
        {
            ExpressionTreeNodeType.Expression => GetHashCode((ExpressionExpressionTreeNode)obj),
            ExpressionTreeNodeType.ElementInit => GetHashCode((ElementInitExpressionTreeNode)obj),
            ExpressionTreeNodeType.MemberAssignment or ExpressionTreeNodeType.MemberListBinding or ExpressionTreeNodeType.MemberMemberBinding => GetHashCode((MemberBindingExpressionTreeNode)obj),
            _ => throw new NotImplementedException(),
        };

        private int GetHashCode(ExpressionExpressionTreeNode obj) => _equals.GetHashCode(obj.Expression);

        private int GetHashCode(ElementInitExpressionTreeNode obj) => _equals.GetHashCode(obj.ElementInit);

        private int GetHashCode(MemberBindingExpressionTreeNode obj) => _equals.GetHashCode(obj.MemberBinding);

        private sealed class ShallowExpressionEquals : ExpressionEqualityComparator
        {
            public override int GetHashCode(Expression obj) => base.GetHashCode(obj) << 7 + (int)obj.NodeType;

            protected override bool EqualsBinary(BinaryExpression x, BinaryExpression y) => x.Method == y.Method;

            protected override bool EqualsConditional(ConditionalExpression x, ConditionalExpression y) => true;

            protected override bool EqualsConstant(ConstantExpression x, ConstantExpression y) => x.Type == y.Type && EqualityComparer<object>.Default.Equals(x.Value, y.Value);

            protected override bool EqualsDefault(DefaultExpression x, DefaultExpression y) => x.Type == y.Type;

            protected override bool EqualsInvocation(InvocationExpression x, InvocationExpression y) => true;

            protected override bool EqualsLambda(LambdaExpression x, LambdaExpression y) => true;

            protected override bool EqualsListInit(ListInitExpression x, ListInitExpression y) => true;

            protected override bool EqualsMember(MemberExpression x, MemberExpression y) => x.Member == y.Member;

            protected override bool EqualsMemberInit(MemberInitExpression x, MemberInitExpression y) => true;

            protected override bool EqualsMethodCall(MethodCallExpression x, MethodCallExpression y) => x.Method == y.Method;

            protected override bool EqualsNew(NewExpression x, NewExpression y) => x.Constructor == y.Constructor;

            protected override bool EqualsNewArray(NewArrayExpression x, NewArrayExpression y) => x.Type == y.Type;

            protected override bool EqualsParameter(ParameterExpression x, ParameterExpression y) => object.ReferenceEquals(x, y);

            protected override bool EqualsTypeBinary(TypeBinaryExpression x, TypeBinaryExpression y) => x.TypeOperand == y.TypeOperand;

            protected override bool EqualsUnary(UnaryExpression x, UnaryExpression y)
            {
                var res = true;

                switch (x.NodeType)
                {
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                    case ExpressionType.TypeAs:
                        res = x.Type == y.Type;
                        break;
                }

                res = res && x.Method == y.Method;

                return res;
            }

            public override bool Equals(ElementInit x, ElementInit y) => x.AddMethod == y.AddMethod;

            protected override bool EqualsMemberAssignment(MemberAssignment x, MemberAssignment y) => x.Member == y.Member;

            protected override bool EqualsMemberListBinding(MemberListBinding x, MemberListBinding y) => x.Member == y.Member;

            protected override bool EqualsMemberMemberBinding(MemberMemberBinding x, MemberMemberBinding y) => x.Member == y.Member;

            protected override int GetHashCodeBinary(BinaryExpression obj) => obj.Method != null ? obj.Method.GetHashCode() : 0;

            protected override int GetHashCodeConditional(ConditionalExpression obj) => 0;

            protected override int GetHashCodeConstant(ConstantExpression obj) => obj.Type.GetHashCode() * 17 + EqualityComparer<object>.Default.GetHashCode(obj.Value);

            protected override int GetHashCodeDefault(DefaultExpression obj) => obj.Type.GetHashCode();

            protected override int GetHashCodeInvocation(InvocationExpression obj) => 0;

            protected override int GetHashCodeLambda(LambdaExpression obj) => 0;

            protected override int GetHashCodeListInit(ListInitExpression obj) => 0;

            protected override int GetHashCodeMember(MemberExpression obj) => obj.Member.GetHashCode();

            protected override int GetHashCodeMemberInit(MemberInitExpression obj) => 0;

            protected override int GetHashCodeMethodCall(MethodCallExpression obj) => obj.Method.GetHashCode();

            protected override int GetHashCodeNew(NewExpression obj) => obj.Constructor.GetHashCode();

            protected override int GetHashCodeNewArray(NewArrayExpression obj) => obj.Type.GetHashCode();

            protected override int GetHashCodeParameter(ParameterExpression obj) => obj.GetHashCode();

            protected override int GetHashCodeTypeBinary(TypeBinaryExpression obj) => obj.TypeOperand.GetHashCode();

            protected override int GetHashCodeUnary(UnaryExpression obj)
            {
                var res = 0;

                switch (obj.NodeType)
                {
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                    case ExpressionType.TypeAs:
                        res += obj.Type.GetHashCode();
                        break;
                }

                if (obj.Method != null)
                {
                    res = res * 17 + obj.Method.GetHashCode();
                }

                return res;
            }

            public override int GetHashCode(ElementInit obj) => obj.AddMethod.GetHashCode();

            protected override int GetHashCodeMemberAssignment(MemberAssignment obj) => obj.Member.GetHashCode();

            protected override int GetHashCodeMemberListBinding(MemberListBinding obj) => obj.Member.GetHashCode();

            protected override int GetHashCodeMemberMemberBinding(MemberMemberBinding obj) => obj.Member.GetHashCode();
        }
    }
}
