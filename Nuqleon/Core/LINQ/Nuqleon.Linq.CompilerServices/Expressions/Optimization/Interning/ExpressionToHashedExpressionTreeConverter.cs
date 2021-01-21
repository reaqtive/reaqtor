// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - January 2014 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    internal sealed class ExpressionToHashedExpressionTreeConverter : ExpressionVisitor<ITree<HashedNode>>
    {
        private readonly Func<ExpressionEqualityComparator> _comparatorFactory;

        public ExpressionToHashedExpressionTreeConverter(Func<ExpressionEqualityComparator> comparatorFactory) => _comparatorFactory = comparatorFactory;

        private static Hasher GetNodeHash(Expression expression)
        {
            var h = new Hasher();

            h.Add((int)expression.NodeType);
            h.Add(expression.Type.GetHashCode());

            return h;
        }

        private static Hasher GetNodeHash(MemberBinding binding)
        {
            var h = new Hasher();

            h.Add((int)binding.BindingType);
            h.Add(binding.Member.GetHashCode());

            return h;
        }

        private static int OneIfNotNull(object o) => o != null ? 1 : 0;

        private ITree<HashedNode> CreateTree(Expression node, ref Hasher h, params ITree<HashedNode>[] children)
        {
            return new Tree<HashedNode>(
                new ExpressionHashedNode(node, h.ToHashCode(), _comparatorFactory),
                ToReadOnly(children)
            );
        }

        private ITree<HashedNode> CreateTree(MemberBinding node, ref Hasher h, params ITree<HashedNode>[] children)
        {
            return new Tree<HashedNode>(
                new MemberBindingHashedNode(node, h.ToHashCode(), _comparatorFactory),
                ToReadOnly(children)
            );
        }

        private static ReadOnlyCollection<ITree<HashedNode>> ToReadOnly(params ITree<HashedNode>[] children)
        {
            // NB: This is safe because none of the arrays are referenced elsewhere, so no mutations can happen.
            return new TrueReadOnlyCollection<ITree<HashedNode>>(children);
        }

        private ITree<HashedNode> Visit(Expression node, ref Hasher h)
        {
            var tree = Visit(node);

            h.Add(tree.Value.Hash);

            return tree;
        }

        private void Visit(Expression node, ref Hasher h, ITree<HashedNode>[] children, ref int i)
        {
            children[i++] = Visit(node, ref h);
        }

        private void VisitIfNotNull(Expression node, ref Hasher h, ITree<HashedNode>[] children, ref int i)
        {
            if (node != null)
            {
                children[i++] = Visit(node, ref h);
            }
        }

        private void Visit(IEnumerable<Expression> nodes, ref Hasher h, ITree<HashedNode>[] children, ref int i)
        {
            foreach (var node in nodes)
            {
                children[i++] = Visit(node, ref h);
            }
        }

        protected override ITree<HashedNode> VisitBinary(BinaryExpression node)
        {
            var h = GetNodeHash(node);

            h.Add(node.Method);

            var children = new ITree<HashedNode>[2 + OneIfNotNull(node.Conversion)];

            int i = 0;

            Visit(node.Left, ref h, children, ref i);
            Visit(node.Right, ref h, children, ref i);
            VisitIfNotNull(node.Conversion, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitBlock(BlockExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[node.Variables.Count + node.Expressions.Count];

            var i = 0;

            Visit(node.Variables, ref h, children, ref i);
            Visit(node.Expressions, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitConditional(ConditionalExpression node)
        {
            var h = GetNodeHash(node);

            var testTree = Visit(node.Test, ref h);
            var ifTrueTree = Visit(node.IfTrue, ref h);
            var ifFalseTree = Visit(node.IfFalse, ref h);

            return CreateTree(node, ref h, testTree, ifTrueTree, ifFalseTree);
        }

        protected override ITree<HashedNode> VisitConstant(ConstantExpression node)
        {
            var h = GetNodeHash(node);

            h.Add(EqualityComparer<object>.Default.GetHashCode(node.Value));

            return CreateTree(node, ref h);
        }

        protected override ITree<HashedNode> VisitDebugInfo(DebugInfoExpression node)
            => throw new NotSupportedException("Expression interning does not support DebugInfo nodes. Considering stripping these nodes from expression trees using a visitor pass.");

        protected override ITree<HashedNode> VisitDefault(DefaultExpression node)
        {
            var h = GetNodeHash(node);

            return CreateTree(node, ref h);
        }

        protected override ITree<HashedNode> VisitDynamic(DynamicExpression node)
        {
            var h = GetNodeHash(node);

            h.Add(node.DelegateType);

            var children = new ITree<HashedNode>[1 + node.Arguments.Count];

            var i = 0;

            children[i++] = new Tree<HashedNode>(new CallSiteHashedNode(node.Binder, h.ToHashCode(), _comparatorFactory));
            Visit(node.Arguments, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitExtension(Expression node)
            => throw new NotSupportedException("Expression interning does not support Extension nodes. Considering reducing these nodes using a visitor pass.");

        protected override ITree<HashedNode> VisitGoto(GotoExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[1 + OneIfNotNull(node.Value)];

            var i = 0;

            children[i++] = VisitLabelTarget(node.Target);
            VisitIfNotNull(node.Value, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitIndex(IndexExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[1 + node.Arguments.Count];

            var i = 0;

            Visit(node.Object, ref h, children, ref i);
            Visit(node.Arguments, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitInvocation(InvocationExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[1 + node.Arguments.Count];

            var i = 0;

            Visit(node.Expression, ref h, children, ref i);
            Visit(node.Arguments, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitLabel(LabelExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[1 + OneIfNotNull(node.DefaultValue)];

            var i = 0;

            children[i++] = VisitLabelTarget(node.Target);
            VisitIfNotNull(node.DefaultValue, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        private ITree<HashedNode> VisitLabelTarget(LabelTarget node)
        {
            // NB: All labels with the same type compare equal for the low-pass hash filter. Subsequent
            //     equality checks on parent trees can overrule this.

            var h = new Hasher();

            h.Add(node.Type);

            return new Tree<HashedNode>(new LabelTargetHashedNode(node, h.ToHashCode(), _comparatorFactory));
        }

        private void VisitLabelTargetIfNotNull(LabelTarget node, ref Hasher h, ITree<HashedNode>[] children, ref int i)
        {
            if (node != null)
            {
                var target = VisitLabelTarget(node);
                children[i++] = target;
                h.Add(target.Value.Hash);
            }
        }

        protected override ITree<HashedNode> VisitLambda<T>(Expression<T> node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[node.Parameters.Count + 1];

            var i = 0;

            Visit(node.Body, ref h, children, ref i);
            Visit(node.Parameters, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitListInit(ListInitExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[node.Initializers.Count + 1];

            children[0] = Visit(node.NewExpression, ref h);

            var i = 1;

            foreach (var initializer in node.Initializers)
            {
                var initializerTree = VisitElementInit(initializer);
                children[i++] = initializerTree;
                h.Add(initializerTree.Value.Hash);
            }

            return CreateTree(node, ref h, children);
        }

        private ITree<HashedNode> VisitElementInit(ElementInit node)
        {
            var h = new Hasher();

            h.Add(node.AddMethod);

            var children = new ITree<HashedNode>[node.Arguments.Count];

            var i = 0;

            Visit(node.Arguments, ref h, children, ref i);

            return new Tree<HashedNode>(
                new ElementInitHashedNode(node, h.ToHashCode(), _comparatorFactory),
                ToReadOnly(children)
            );
        }

        private ITree<HashedNode> VisitMemberBinding(MemberBinding node)
        {
            return node.BindingType switch
            {
                MemberBindingType.Assignment => VisitMemberAssignment((MemberAssignment)node),
                MemberBindingType.ListBinding => VisitMemberListBinding((MemberListBinding)node),
                MemberBindingType.MemberBinding => VisitMemberMemberBinding((MemberMemberBinding)node),
                _ => throw new NotImplementedException(),
            };
        }

        private ITree<HashedNode> VisitMemberAssignment(MemberAssignment node)
        {
            var h = GetNodeHash(node);

            var expressionTree = Visit(node.Expression, ref h);

            return CreateTree(node, ref h, expressionTree);
        }

        private ITree<HashedNode> VisitMemberListBinding(MemberListBinding node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[node.Initializers.Count];

            var i = 0;

            foreach (var initializer in node.Initializers)
            {
                var initializerTree = VisitElementInit(initializer);
                children[i++] = initializerTree;
                h.Add(initializerTree.Value.Hash);
            }

            return CreateTree(node, ref h, children);
        }

        private ITree<HashedNode> VisitMemberMemberBinding(MemberMemberBinding node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[node.Bindings.Count];

            var i = 0;

            foreach (var binding in node.Bindings)
            {
                var bindingTree = VisitMemberBinding(binding);
                children[i++] = bindingTree;
                h.Add(bindingTree.Value.Hash);
            }

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitLoop(LoopExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[1 + OneIfNotNull(node.BreakLabel) + OneIfNotNull(node.ContinueLabel)];

            var i = 0;

            Visit(node.Body, ref h, children, ref i);
            VisitLabelTargetIfNotNull(node.BreakLabel, ref h, children, ref i);
            VisitLabelTargetIfNotNull(node.ContinueLabel, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitMember(MemberExpression node)
        {
            var h = GetNodeHash(node);

            h.Add(node.Member);

            if (node.Expression != null)
            {
                var expressionTree = Visit(node.Expression, ref h);
                return CreateTree(node, ref h, expressionTree);
            }
            else
            {
                return CreateTree(node, ref h);
            }
        }

        protected override ITree<HashedNode> VisitMemberInit(MemberInitExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[node.Bindings.Count + 1];

            var newExpressionTree = Visit(node.NewExpression, ref h);
            children[0] = newExpressionTree;

            var i = 1;

            foreach (var binding in node.Bindings)
            {
                var memberBindingTree = VisitMemberBinding(binding);
                h.Add(memberBindingTree.Value.Hash);
                children[i++] = memberBindingTree;
            }

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitMethodCall(MethodCallExpression node)
        {
            var h = GetNodeHash(node);

            h.Add(node.Method);

            var children = new ITree<HashedNode>[OneIfNotNull(node.Object) + node.Arguments.Count];

            var i = 0;

            VisitIfNotNull(node.Object, ref h, children, ref i);

            foreach (var argument in node.Arguments)
            {
                children[i++] = Visit(argument, ref h);
            }

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitNew(NewExpression node)
        {
            var h = GetNodeHash(node);

            h.Add(node.Constructor);

            var children = new ITree<HashedNode>[node.Arguments.Count];

            var i = 0;

            Visit(node.Arguments, ref h, children, ref i);

            if (node.Members != null)
            {
                foreach (var m in node.Members)
                {
                    h.Add(m);
                }
            }

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitNewArray(NewArrayExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[node.Expressions.Count];

            var i = 0;

            Visit(node.Expressions, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitParameter(ParameterExpression node)
        {
            var h = GetNodeHash(node);

            return CreateTree(node, ref h);
        }

        protected override ITree<HashedNode> VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[node.Variables.Count];

            var i = 0;

            Visit(node.Variables, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        protected override ITree<HashedNode> VisitSwitch(SwitchExpression node)
        {
            var h = GetNodeHash(node);

            h.Add(node.Comparison);

            var children = new ITree<HashedNode>[1 + node.Cases.Count + OneIfNotNull(node.DefaultBody)];

            children[0] = Visit(node.SwitchValue, ref h);

            var i = 1;

            foreach (var @case in node.Cases)
            {
                var caseTree = VisitSwitchCase(@case);
                children[i++] = caseTree;
                h.Add(caseTree.Value.Hash);
            }

            VisitIfNotNull(node.DefaultBody, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        private ITree<HashedNode> VisitSwitchCase(SwitchCase node)
        {
            var h = new Hasher();

            var children = new ITree<HashedNode>[1 + node.TestValues.Count];

            var i = 0;

            Visit(node.Body, ref h, children, ref i);
            Visit(node.TestValues, ref h, children, ref i);

            return new Tree<HashedNode>(
                new SwitchCaseHashedNode(node, h.ToHashCode(), _comparatorFactory),
                ToReadOnly(children)
            );
        }

        protected override ITree<HashedNode> VisitTry(TryExpression node)
        {
            var h = GetNodeHash(node);

            var children = new ITree<HashedNode>[1 + node.Handlers.Count + OneIfNotNull(node.Finally) + OneIfNotNull(node.Fault)];

            var i = 0;

            Visit(node.Body, ref h, children, ref i);

            foreach (var handler in node.Handlers)
            {
                var catchBlockTree = VisitCatchBlock(handler);
                h.Add(catchBlockTree.Value.Hash);
                children[i++] = catchBlockTree;
            }

            VisitIfNotNull(node.Finally, ref h, children, ref i);
            VisitIfNotNull(node.Fault, ref h, children, ref i);

            return CreateTree(node, ref h, children);
        }

        private ITree<HashedNode> VisitCatchBlock(CatchBlock node)
        {
            var h = new Hasher();

            h.Add(node.Test);

            var children = new ITree<HashedNode>[1 + OneIfNotNull(node.Variable) + OneIfNotNull(node.Filter)];

            var i = 0;

            Visit(node.Body, ref h, children, ref i);
            VisitIfNotNull(node.Variable, ref h, children, ref i);
            VisitIfNotNull(node.Filter, ref h, children, ref i);

            return new Tree<HashedNode>(
                new CatchBlockHashedNode(node, h.ToHashCode(), _comparatorFactory),
                ToReadOnly(children)
            );
        }

        protected override ITree<HashedNode> VisitTypeBinary(TypeBinaryExpression node)
        {
            var h = GetNodeHash(node);

            var expressionTree = Visit(node.Expression, ref h);

            h.Add(node.TypeOperand);

            return CreateTree(node, ref h, expressionTree);
        }

        protected override ITree<HashedNode> VisitUnary(UnaryExpression node)
        {
            var h = GetNodeHash(node);

            h.Add(node.Method);

            if (node.Operand != null)
            {
                var operandTree = Visit(node.Operand, ref h);

                return CreateTree(node, ref h, operandTree);
            }

            return CreateTree(node, ref h);
        }

        private struct Hasher
        {
#if NET5_0 || NETSTANDARD3_1
            private HashCode _hashCode;

            public void Add(int h) => _hashCode.Add(h);

            public int ToHashCode() => _hashCode.ToHashCode();
#else
            private int _hashCode;

            public void Add(int h) => _hashCode = _hashCode * 31 + h;

            public int ToHashCode() => _hashCode;
#endif

            public void Add<T>(T obj)
            {
                if (obj != null)
                {
                    Add(obj.GetHashCode());
                }
            }
        }
    }
}
