// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - January 2014 - Created this file.
//

using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    internal sealed class ExpressionToHashedExpressionTreeConverter : ExpressionVisitor<ITree<HashedNode>>
    {
        private readonly Func<ExpressionEqualityComparator> _comparatorFactory;

        public ExpressionToHashedExpressionTreeConverter(Func<ExpressionEqualityComparator> comparatorFactory) => _comparatorFactory = comparatorFactory;

        private static int GetNodeHash(Expression e) => (int)e.NodeType * 31 + e.Type.GetHashCode();

        private static int GetNodeHash(MemberBinding b) => (int)b.BindingType * 31 + b.Member.GetHashCode();

        private static int GetNodeHash(ElementInit i) => 1983 * 31 + i.AddMethod.GetHashCode();

        protected override ITree<HashedNode> VisitBinary(BinaryExpression node)
        {
            var l = Visit(node.Left);
            var r = Visit(node.Right);
            var c = Visit(node.Conversion);

            var h = GetNodeHash(node);
            h = h * 31 + l.Value.Hash;
            h = h * 31 + r.Value.Hash;

            ITree<HashedNode>[] z;

            if (c != null)
            {
                h = h * 31 + c.Value.Hash;
                z = new ITree<HashedNode>[] { l, r, c };
            }
            else
            {
                z = new ITree<HashedNode>[] { l, r };
            }

            if (node.Method != null)
            {
                h = h * 31 + node.Method.GetHashCode();
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        protected override ITree<HashedNode> VisitBlock(BlockExpression node)
        {
            throw new NotImplementedException();
        }

        protected override ITree<HashedNode> VisitConditional(ConditionalExpression node)
        {
            var c = Visit(node.Test);
            var t = Visit(node.IfTrue);
            var f = Visit(node.IfFalse);

            var h = GetNodeHash(node);
            h = h * 31 + c.Value.Hash;
            h = h * 31 + t.Value.Hash;
            h = h * 31 + f.Value.Hash;

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                new[]
                {
                    c,
                    t,
                    f,
                }
            );
        }

        protected override ITree<HashedNode> VisitConstant(ConstantExpression node)
        {
            var h = GetNodeHash(node);
            h = h * 31 + EqualityComparer<object>.Default.GetHashCode(node.Value);

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h }
            );
        }

        protected override ITree<HashedNode> VisitDebugInfo(DebugInfoExpression node) => throw new NotImplementedException();

        protected override ITree<HashedNode> VisitDefault(DefaultExpression node)
        {
            var h = GetNodeHash(node);

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h }
            );
        }

        protected override ITree<HashedNode> VisitDynamic(DynamicExpression node) => throw new NotImplementedException();

        protected override ITree<HashedNode> VisitExtension(Expression node) => throw new NotImplementedException();

        protected override ITree<HashedNode> VisitGoto(GotoExpression node) => throw new NotImplementedException();

        protected override ITree<HashedNode> VisitIndex(IndexExpression node)
        {
            var f = Visit(node.Object);
            var a = Visit(node.Arguments);

            var h = GetNodeHash(node);
            h = h * 31 + f.Value.Hash;

            var z = new ITree<HashedNode>[a.Count + 1];
            z[0] = f;

            var i = 1;
            foreach (var b in a)
            {
                z[i] = b;
                h = h * 31 + b.Value.Hash;

                i++;
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        protected override ITree<HashedNode> VisitInvocation(InvocationExpression node)
        {
            var f = Visit(node.Expression);
            var a = Visit(node.Arguments);

            var h = GetNodeHash(node);
            h = h * 31 + f.Value.Hash;

            var z = new ITree<HashedNode>[a.Count + 1];
            z[0] = f;

            var i = 1;
            foreach (var b in a)
            {
                z[i] = b;
                h = h * 31 + b.Value.Hash;

                i++;
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        protected override ITree<HashedNode> VisitLabel(LabelExpression node) => throw new NotImplementedException();

        protected override ITree<HashedNode> VisitLambda<T>(Expression<T> node)
        {
            var b = Visit(node.Body);
            var p = Visit(node.Parameters);

            var h = GetNodeHash(node);
            h = h * 31 + node.Type.GetHashCode();
            h = h * 31 + b.Value.Hash;

            var z = new ITree<HashedNode>[p.Count + 1];
            z[0] = b;

            var i = 1;
            foreach (var q in p)
            {
                z[i] = q;
                h = h * 31 + q.Value.Hash;

                i++;
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        protected override ITree<HashedNode> VisitListInit(ListInitExpression node)
        {
            var n = VisitNew(node.NewExpression);

            var i = node.Initializers.Select(VisitElementInit).ToArray();

            var h = GetNodeHash(node);
            h = h * 31 + n.Value.Hash;

            var z = new ITree<HashedNode>[i.Length + 1];
            z[0] = n;

            var k = 1;
            foreach (var j in i)
            {
                h = h * 31 + j.Value.Hash;
                z[k] = j;

                k++;
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        private ITree<HashedNode> VisitElementInit(ElementInit node)
        {
            var a = Visit(node.Arguments);

            var h = GetNodeHash(node);

            foreach (var b in a)
            {
                h = h * 31 + b.Value.Hash;
            }

            return new Tree<HashedNode>(
                new ElementInitHashedNode(_comparatorFactory) { Initializer = node, Hash = h },
                a
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
            var e = Visit(node.Expression);

            var h = GetNodeHash(node);
            h = h * 31 + e.Value.Hash;

            return new Tree<HashedNode>(
                new MemberBindingHashedNode(_comparatorFactory) { Binding = node, Hash = h },
                new[]
                {
                    e,
                }
            );
        }

        private ITree<HashedNode> VisitMemberListBinding(MemberListBinding node)
        {
            var i = node.Initializers.Select(VisitElementInit);

            var h = GetNodeHash(node);

            foreach (var j in i)
            {
                h = h * 31 + j.Value.Hash;
            }

            return new Tree<HashedNode>(
                new MemberBindingHashedNode(_comparatorFactory) { Binding = node, Hash = h },
                i
            );
        }

        private ITree<HashedNode> VisitMemberMemberBinding(MemberMemberBinding node)
        {
            var b = node.Bindings.Select(VisitMemberBinding);

            var h = GetNodeHash(node);

            foreach (var c in b)
            {
                h = h * 31 + c.Value.Hash;
            }

            return new Tree<HashedNode>(
                new MemberBindingHashedNode(_comparatorFactory) { Binding = node, Hash = h },
                b
            );
        }

        protected override ITree<HashedNode> VisitLoop(LoopExpression node) => throw new NotImplementedException();

        protected override ITree<HashedNode> VisitMember(MemberExpression node)
        {
            var e = Visit(node.Expression);

            var h = GetNodeHash(node);

            var z = new ITree<HashedNode>[e != null ? 1 : 0];

            if (e != null)
            {
                h = h * 31 + e.Value.Hash;
                z[0] = e;
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        protected override ITree<HashedNode> VisitMemberInit(MemberInitExpression node)
        {
            var n = VisitNew(node.NewExpression);

            var h = GetNodeHash(node);
            h = h * 31 + n.Value.Hash;

            var b = node.Bindings.Select(VisitMemberBinding).ToArray();

            var z = new ITree<HashedNode>[b.Length + 1];
            z[0] = n;

            var i = 1;
            foreach (var c in b)
            {
                h = h * 31 + c.Value.Hash;
                z[i] = c;

                i++;
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        protected override ITree<HashedNode> VisitMethodCall(MethodCallExpression node)
        {
            var o = Visit(node.Object);
            var a = Visit(node.Arguments);

            var h = GetNodeHash(node);
            h = h * 31 + node.Method.GetHashCode();

            var z = new ITree<HashedNode>[a.Count + (o != null ? 1 : 0)];

            if (o != null)
            {
                z[0] = o;
                h = h * 31 + o.Value.Hash;
            }

            var i = node.Object != null ? 1 : 0;
            foreach (var b in a)
            {
                z[i] = b;
                h = h * 31 + b.Value.Hash;

                i++;
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        protected override ITree<HashedNode> VisitNew(NewExpression node)
        {
            var a = Visit(node.Arguments);

            var h = GetNodeHash(node);

            if (node.Constructor != null)
            {
                h = h * 31 + node.Constructor.GetHashCode();
            }

            var z = new ITree<HashedNode>[a.Count];

            var i = 0;
            foreach (var b in a)
            {
                z[i] = b;
                h = h * 31 + b.Value.Hash;

                i++;
            }

            if (node.Members != null)
            {
                foreach (var m in node.Members)
                {
                    h = h * 31 + m.GetHashCode();
                }
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        protected override ITree<HashedNode> VisitNewArray(NewArrayExpression node)
        {
            var e = Visit(node.Expressions);

            var h = GetNodeHash(node);

            var z = new ITree<HashedNode>[e.Count];

            var i = 0;
            foreach (var f in e)
            {
                z[i] = f;
                h = h * 31 + f.Value.Hash;

                i++;
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                z
            );
        }

        protected override ITree<HashedNode> VisitParameter(ParameterExpression node)
        {
            var h = GetNodeHash(node);

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h }
            );
        }

        protected override ITree<HashedNode> VisitRuntimeVariables(RuntimeVariablesExpression node) => throw new NotImplementedException();

        protected override ITree<HashedNode> VisitSwitch(SwitchExpression node) => throw new NotImplementedException();

        protected override ITree<HashedNode> VisitTry(TryExpression node) => throw new NotImplementedException();

        protected override ITree<HashedNode> VisitTypeBinary(TypeBinaryExpression node)
        {
            var e = Visit(node.Expression);

            var h = GetNodeHash(node);
            h = h * 31 + e.Value.Hash;
            h = h * 31 + node.TypeOperand.GetHashCode();

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                new[]
                {
                    e,
                }
            );
        }

        protected override ITree<HashedNode> VisitUnary(UnaryExpression node)
        {
            var o = Visit(node.Operand);

            var h = GetNodeHash(node);
            h = h * 31 + o.Value.Hash;

            if (node.Method != null)
            {
                h = h * 31 + node.Method.GetHashCode();
            }

            return new Tree<HashedNode>(
                new ExpressionHashedNode(_comparatorFactory) { Expression = node, Hash = h },
                new[]
                {
                    o
                }
            );
        }
    }
}
