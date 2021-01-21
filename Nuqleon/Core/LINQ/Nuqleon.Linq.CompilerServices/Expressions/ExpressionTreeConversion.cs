// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections.ObjectModel;
using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    internal class ExpressionTreeConversion : ExpressionVisitorNarrow<ExpressionTree, ExpressionTree<LambdaExpression>, ExpressionTree<ParameterExpression>, ExpressionTree<NewExpression>, ElementInitExpressionTree, MemberBindingExpressionTree, MemberAssignmentExpressionTree, MemberListBindingExpressionTree, MemberMemberBindingExpressionTree>
    {
        public ElementInitExpressionTree Visit(ElementInit node) => base.VisitElementInit(node);

        public MemberBindingExpressionTree Visit(MemberBinding node) => base.VisitMemberBinding(node);

        protected override ExpressionTree MakeBinary(BinaryExpression node, ExpressionTree left, ExpressionTree<LambdaExpression> conversion, ExpressionTree right)
        {
            if (conversion != null)
            {
                return new ExpressionTree<BinaryExpression>(node, left, right, conversion);
            }
            else
            {
                return new ExpressionTree<BinaryExpression>(node, left, right);
            }
        }

        protected override ExpressionTree MakeConditional(ConditionalExpression node, ExpressionTree test, ExpressionTree ifTrue, ExpressionTree ifFalse) => new ExpressionTree<ConditionalExpression>(node, test, ifTrue, ifFalse);

        protected override ExpressionTree MakeConstant(ConstantExpression node) => new ExpressionTree<ConstantExpression>(node);

        protected override ExpressionTree MakeDefault(DefaultExpression node) => new ExpressionTree<DefaultExpression>(node);

        protected override ElementInitExpressionTree MakeElementInit(ElementInit node, ReadOnlyCollection<ExpressionTree> arguments) => new(node, arguments);

        protected override ExpressionTree MakeInvocation(InvocationExpression node, ExpressionTree expression, ReadOnlyCollection<ExpressionTree> arguments) => new ExpressionTree<InvocationExpression>(node, new[] { expression }.Concat(arguments));

        protected override ExpressionTree<LambdaExpression> MakeLambda<T>(Expression<T> node, ExpressionTree body, ReadOnlyCollection<ExpressionTree<ParameterExpression>> parameters) => new(node, new[] { body }.Concat(parameters));

        protected override ExpressionTree MakeListInit(ListInitExpression node, ExpressionTree<NewExpression> newExpression, ReadOnlyCollection<ElementInitExpressionTree> initializers) => new ExpressionTree<ListInitExpression>(node, new ExpressionTreeBase[] { newExpression }.Concat(initializers));

        protected override ExpressionTree MakeMember(MemberExpression node, ExpressionTree expression)
        {
            if (expression != null)
            {
                return new ExpressionTree<MemberExpression>(node, expression);
            }
            else
            {
                return new ExpressionTree<MemberExpression>(node);
            }
        }

        protected override MemberAssignmentExpressionTree MakeMemberAssignment(MemberAssignment node, ExpressionTree expression) => new(node, expression);

        protected override ExpressionTree MakeMemberInit(MemberInitExpression node, ExpressionTree<NewExpression> newExpression, ReadOnlyCollection<MemberBindingExpressionTree> bindings) => new ExpressionTree<MemberInitExpression>(node, new ExpressionTreeBase[] { newExpression }.Concat(bindings));

        protected override MemberListBindingExpressionTree MakeMemberListBinding(MemberListBinding node, ReadOnlyCollection<ElementInitExpressionTree> initializers) => new(node, initializers);

        protected override MemberMemberBindingExpressionTree MakeMemberMemberBinding(MemberMemberBinding node, ReadOnlyCollection<MemberBindingExpressionTree> bindings) => new(node, bindings);

        protected override ExpressionTree MakeMethodCall(MethodCallExpression node, ExpressionTree @object, ReadOnlyCollection<ExpressionTree> arguments)
        {
            if (@object != null)
            {
                return new ExpressionTree<MethodCallExpression>(node, new[] { @object }.Concat(arguments));
            }
            else
            {
                return new ExpressionTree<MethodCallExpression>(node, arguments);
            }
        }

        protected override ExpressionTree MakeNew(NewExpression node, ReadOnlyCollection<ExpressionTree> arguments) => new ExpressionTree<NewExpression>(node, arguments);

        protected override ExpressionTree MakeNewArray(NewArrayExpression node, ReadOnlyCollection<ExpressionTree> expressions) => new ExpressionTree<NewArrayExpression>(node, expressions);

        protected override ExpressionTree<ParameterExpression> MakeParameter(ParameterExpression node) => new(node);

        protected override ExpressionTree MakeTypeBinary(TypeBinaryExpression node, ExpressionTree expression) => new ExpressionTree<TypeBinaryExpression>(node, expression);

        protected override ExpressionTree MakeUnary(UnaryExpression node, ExpressionTree operand) => new ExpressionTree<UnaryExpression>(node, operand);
    }

    internal sealed class ExpressionTreeUnsupportedNode
    {
    }
}
