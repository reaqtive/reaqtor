// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - January 2014 - Created this file.
//

using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    internal enum HashedNodeType
    {
        Expression,
        MemberBinding,
        ElementInit,
    }

    internal abstract class HashedNode
    {
        public abstract HashedNodeType NodeType { get; }

        public int Hash { get; set; }

        public abstract object Value { get; }
    }

    internal sealed class ExpressionHashedNode : HashedNode
    {
        private readonly Func<ExpressionEqualityComparator> _comparatorFactory;

        public ExpressionHashedNode(Func<ExpressionEqualityComparator> comparatorFactory) => _comparatorFactory = comparatorFactory;

        public override HashedNodeType NodeType => HashedNodeType.Expression;

        public override object Value => Expression;

        public Expression Expression;

        public override int GetHashCode() => Hash;

        public override bool Equals(object obj) => obj is ExpressionHashedNode other && _comparatorFactory().Equals(Expression, other.Expression);

        public override string ToString() => Expression.ToString();
    }

    internal sealed class MemberBindingHashedNode : HashedNode
    {
        private readonly Func<ExpressionEqualityComparator> _comparatorFactory;

        public MemberBindingHashedNode(Func<ExpressionEqualityComparator> comparatorFactory) => _comparatorFactory = comparatorFactory;

        public override HashedNodeType NodeType => HashedNodeType.MemberBinding;

        public override object Value => Binding;

        public MemberBinding Binding;

        public override int GetHashCode() => Hash;

        public override bool Equals(object obj) => obj is MemberBindingHashedNode other && _comparatorFactory().Equals(Binding, other.Binding);

        public override string ToString() => Binding.ToString();
    }

    internal sealed class ElementInitHashedNode : HashedNode
    {
        private readonly Func<ExpressionEqualityComparator> _comparatorFactory;

        public ElementInitHashedNode(Func<ExpressionEqualityComparator> comparatorFactory) => _comparatorFactory = comparatorFactory;

        public override HashedNodeType NodeType => HashedNodeType.ElementInit;

        public override object Value => Initializer;

        public ElementInit Initializer;

        public override int GetHashCode() => Hash;

        public override bool Equals(object obj) => obj is ElementInitHashedNode other && _comparatorFactory().Equals(Initializer, other.Initializer);

        public override string ToString() => Initializer.ToString();
    }
}
