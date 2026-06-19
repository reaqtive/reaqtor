// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - January 2014 - Created this file.
//

using System.Linq.CompilerServices;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
    internal enum HashedNodeType
    {
        Expression,
        MemberBinding,
        ElementInit,
        SwitchCase,
        CatchBlock,
        LabelTarget,
        CallSite,
    }

    internal abstract class HashedNode
    {
        protected HashedNode(int hash) => Hash = hash;

        public abstract HashedNodeType NodeType { get; }

        public int Hash { get; }

        public abstract object Value { get; }

        public sealed override string ToString() => Value.ToString();
    }

    internal abstract class HashedNode<T> : HashedNode, IEquatable<T>
        where T : HashedNode<T>
    {
        private readonly Func<ExpressionEqualityComparator> _comparatorFactory;

        public HashedNode(int hash, Func<ExpressionEqualityComparator> comparatorFactory)
            : base(hash)
        {
            _comparatorFactory = comparatorFactory;
        }

        public sealed override bool Equals(object obj) => Equals(obj as T);

        public bool Equals(T other)
        {
            if (other == null)
            {
                return false;
            }

            var eq = _comparatorFactory();

            //
            // NB: We take a conservative stance here and will never consider two nodes that have any labels to be
            //     equal. This limits the effectiveness of interning but avoids the complexity of having to define
            //     proper label scopes conform the rules of the expression compiler. We can only consider equality
            //     of nodes containing labels if no labels can "escape" by branching to a LabelTarget defined in a
            //     surrounding expression. E.g.
            //
            //       label1: ...
            //
            //       {
            //           goto label1;
            //       }
            //
            //     If the block can compare equal to another block with the same structure but using a different
            //     instance of the label, a substitution would break the reference equality of labels in an outer
            //     tree, thus breaking the branch.
            //
            //     Conversely, if a label is defined in a subexpession but branched to from an outer expression,
            //     it is equally unsafe to allow for substitutions. E.g.
            //
            //       goto label1;
            //
            //       {
            //           label1: ...
            //       }
            //
            //     The same remarks hold.
            //
            //     However, we can allow for one well-known case where no labels can escape, i.e. lambda expression
            //     nodes. The has the benefit that interning for top-level lambdas or nested lambdas can still be
            //     effective. Also note that we can still intern children of nodes that contain labels, e.g.
            //
            //       label1: ...
            //
            //       {
            //           f(42)
            //           goto label1;
            //       }
            //
            //     If `f(42)` or `42` can be reused from an interned expression, we can still rewrite the tree. We
            //     will never intern the surrounding tree with a label in it though.
            //

            return Equals(eq, other) && (other.Value is LambdaExpression || !eq.HasLabels);
        }

        public sealed override int GetHashCode() => Hash;

        protected abstract bool Equals(ExpressionEqualityComparator comparator, T other);
    }

    internal sealed class ExpressionHashedNode : HashedNode<ExpressionHashedNode>
    {
        public ExpressionHashedNode(Expression node, int hash, Func<ExpressionEqualityComparator> comparatorFactory)
            : base(hash, comparatorFactory)
        {
            Expression = node;
        }

        public override HashedNodeType NodeType => HashedNodeType.Expression;

        public override object Value => Expression;

        public Expression Expression { get; }

        protected override bool Equals(ExpressionEqualityComparator comparator, ExpressionHashedNode other) => comparator.Equals(Expression, other.Expression);
    }

    internal sealed class MemberBindingHashedNode : HashedNode<MemberBindingHashedNode>
    {
        public MemberBindingHashedNode(MemberBinding binding, int hash, Func<ExpressionEqualityComparator> comparatorFactory)
            : base(hash, comparatorFactory)
        {
            Binding = binding;
        }

        public override HashedNodeType NodeType => HashedNodeType.MemberBinding;

        public override object Value => Binding;

        public MemberBinding Binding { get; }

        protected override bool Equals(ExpressionEqualityComparator comparator, MemberBindingHashedNode other) => comparator.Equals(Binding, other.Binding);
    }

    internal sealed class ElementInitHashedNode : HashedNode<ElementInitHashedNode>
    {
        public ElementInitHashedNode(ElementInit initializer, int hash, Func<ExpressionEqualityComparator> comparatorFactory)
            : base(hash, comparatorFactory)
        {
            Initializer = initializer;
        }

        public override HashedNodeType NodeType => HashedNodeType.ElementInit;

        public override object Value => Initializer;

        public ElementInit Initializer { get; }

        protected override bool Equals(ExpressionEqualityComparator comparator, ElementInitHashedNode other) => comparator.Equals(Initializer, other.Initializer);
    }

    internal sealed class SwitchCaseHashedNode : HashedNode<SwitchCaseHashedNode>
    {
        public SwitchCaseHashedNode(SwitchCase switchCase, int hash, Func<ExpressionEqualityComparator> comparatorFactory)
            : base(hash, comparatorFactory)
        {
            SwitchCase = switchCase;
        }

        public override HashedNodeType NodeType => HashedNodeType.SwitchCase;

        public override object Value => SwitchCase;

        public SwitchCase SwitchCase { get; }

        protected override bool Equals(ExpressionEqualityComparator comparator, SwitchCaseHashedNode other) => comparator.Equals(SwitchCase, other.SwitchCase);
    }

    internal sealed class CatchBlockHashedNode : HashedNode<CatchBlockHashedNode>
    {
        public CatchBlockHashedNode(CatchBlock catchBlock, int hash, Func<ExpressionEqualityComparator> comparatorFactory)
            : base(hash, comparatorFactory)
        {
            CatchBlock = catchBlock;
        }

        public override HashedNodeType NodeType => HashedNodeType.CatchBlock;

        public override object Value => CatchBlock;

        public CatchBlock CatchBlock { get; }

        protected override bool Equals(ExpressionEqualityComparator comparator, CatchBlockHashedNode other) => comparator.Equals(CatchBlock, other.CatchBlock);
    }

    internal sealed class LabelTargetHashedNode : HashedNode<LabelTargetHashedNode>
    {
        public LabelTargetHashedNode(LabelTarget labelTarget, int hash, Func<ExpressionEqualityComparator> comparatorFactory)
            : base(hash, comparatorFactory)
        {
            LabelTarget = labelTarget;
        }

        public override HashedNodeType NodeType => HashedNodeType.LabelTarget;

        public override object Value => LabelTarget;

        public LabelTarget LabelTarget { get; }

        // NB: By design; only the ExpressionEqualityComparator can decide on equality given a parent expression where the branch structure can
        //     be analyzed to match labels.

        protected override bool Equals(ExpressionEqualityComparator comparator, LabelTargetHashedNode other) => false;
    }

    internal sealed class CallSiteHashedNode : HashedNode<CallSiteHashedNode>
    {
        public CallSiteHashedNode(CallSiteBinder binder, int hash, Func<ExpressionEqualityComparator> comparatorFactory)
            : base(hash, comparatorFactory)
        {
            Binder = binder;
        }

        public override HashedNodeType NodeType => HashedNodeType.CallSite;

        public override object Value => Binder;

        public CallSiteBinder Binder { get; }

        protected override bool Equals(ExpressionEqualityComparator comparator, CallSiteHashedNode other) => comparator.Equals(Binder, other.Binder);
    }
}
