// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Linq.CompilerServices;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Base class for expression tree nodes.
    /// </summary>
    public abstract class ExpressionTreeNode : IEquatable<ExpressionTreeNode>
    {
        /// <summary>
        /// Creates a new expression tree node of the specified node type.
        /// </summary>
        /// <param name="nodeType">Type of the expression tree node.</param>
        protected ExpressionTreeNode(ExpressionTreeNodeType nodeType)
        {
            NodeType = nodeType;
        }

        /// <summary>
        /// Gets the type of the expression tree node.
        /// </summary>
        public ExpressionTreeNodeType NodeType { get; }

        /// <summary>
        /// Checks whether the current node is equal to the specified object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>true if the current instance and the specified object are equal; otherwise, false.</returns>
        public abstract override bool Equals(object obj);

        /// <summary>
        /// Gets a hash code representation of the current expression tree node instance.
        /// </summary>
        /// <returns>Hash code representation of the current expression tree node instance.</returns>
        public abstract override int GetHashCode();

        /// <summary>
        /// Checks whether the current node is equal to the specified expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        public virtual bool Equals(ExpressionTreeNode other)
        {
            if (other == null)
            {
                return false;
            }

            if (NodeType != other.NodeType)
            {
                return false;
            }

            return EqualsCore(other);
        }

        /// <summary>
        /// Checks whether the expression tree node is equal to the specified expression tree node.
        /// This method is used by the Equals implementation and ensures that the current instance
        /// and the specified expression tree node have the same node type.
        /// </summary>
        /// <param name="other">Expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        protected abstract bool EqualsCore(ExpressionTreeNode other);
    }

    /// <summary>
    /// Represents an expression tree node containing an expression.
    /// </summary>
    public sealed class ExpressionExpressionTreeNode : ExpressionTreeNode, IEquatable<ExpressionExpressionTreeNode>
    {
        /// <summary>
        /// Creates an expression tree node containing an expression.
        /// </summary>
        /// <param name="expression">Expression contained in the expression tree.</param>
        public ExpressionExpressionTreeNode(Expression expression)
            : base(ExpressionTreeNodeType.Expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// Gets the expression contained in the expression tree.
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        /// Checks whether the current node is equal to the specified expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        public bool Equals(ExpressionExpressionTreeNode other)
        {
            if (other == null)
            {
                return false;
            }

            return new ExpressionEqualityComparator().Equals(Expression, other.Expression);
        }

        /// <summary>
        /// Checks whether the current node is equal to the specified expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        protected override bool EqualsCore(ExpressionTreeNode other) => Equals(other as ExpressionExpressionTreeNode);

        /// <summary>
        /// Checks whether the current node is equal to the specified object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>true if the current instance and the specified object are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as ExpressionExpressionTreeNode);

        /// <summary>
        /// Gets a hash code representation of the current expression tree node instance.
        /// </summary>
        /// <returns>Hash code representation of the current expression tree node instance.</returns>
        public override int GetHashCode() => new ExpressionEqualityComparator().GetHashCode(Expression);

        /// <summary>
        /// Gets a string representation of the current expression tree node instance.
        /// </summary>
        /// <returns>String representation of the current expression tree node instance.</returns>
        public override string ToString()
        {
            var info = new ExpressionTreeInfo().Visit(Expression);

            info = info != null ? "[" + info + "]" : "";

            return Expression.NodeType + info;
        }

        private sealed class ExpressionTreeInfo : ExpressionVisitorNarrow<string>
        {
            protected override string VisitBinary(BinaryExpression node)
            {
                if (node.Method != null)
                {
                    return GetMethod(node.Method);
                }
                else
                {
                    return null;
                }
            }

            protected override string VisitConditional(ConditionalExpression node) => null;

            protected override string VisitConstant(ConstantExpression node)
            {
                var value = node.Value != null ? node.Value.ToString() : "null";
                return "(" + GetType(node.Type) + ")" + value;
            }

            protected override string VisitDefault(DefaultExpression node) => GetType(node.Type);

            protected override string VisitInvocation(InvocationExpression node) => null;

            protected override string VisitLambda<TDelegate>(Expression<TDelegate> node) => GetType(typeof(TDelegate));

            protected override string VisitListInit(ListInitExpression node) => null;

            protected override string VisitMember(MemberExpression node) => node.Member.ToCSharpString();

            protected override string VisitMemberInit(MemberInitExpression node) => null;

            protected override string VisitMethodCall(MethodCallExpression node) => GetMethod(node.Method);

            protected override string VisitNew(NewExpression node) => GetConstructor(node.Constructor);

            protected override string VisitNewArray(NewArrayExpression node) => GetType(node.Type);

            protected override string VisitParameter(ParameterExpression node) => node.Type.ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: false) + " " + node.ToString();

            protected override string VisitTypeBinary(TypeBinaryExpression node) => GetType(node.TypeOperand);

            protected override string VisitUnary(UnaryExpression node)
            {
                if (node.Method != null)
                {
                    return GetMethod(node.Method);
                }
                else
                {
                    return node.NodeType switch
                    {
                        ExpressionType.Convert or
                        ExpressionType.ConvertChecked or
                        ExpressionType.TypeAs => GetType(node.Type),
                        _ => null,
                    };
                }
            }

            private static string GetMethod(MethodInfo method) => method.ToCSharpString().EscapeFormatString();

            private static string GetConstructor(ConstructorInfo ctor) => ctor.ToCSharpString().EscapeFormatString();

            private static string GetType(Type type) => type.ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: false).EscapeFormatString();
        }
    }

    /// <summary>
    /// Represents an expression tree node containing an element initializer.
    /// </summary>
    public sealed class ElementInitExpressionTreeNode : ExpressionTreeNode, IEquatable<ElementInitExpressionTreeNode>
    {
        /// <summary>
        /// Creates an expression tree node containing an element initializer.
        /// </summary>
        /// <param name="elementInit">Element initializer contained in the expression tree.</param>
        public ElementInitExpressionTreeNode(ElementInit elementInit)
            : base(ExpressionTreeNodeType.ElementInit)
        {
            ElementInit = elementInit ?? throw new ArgumentNullException(nameof(elementInit));
        }

        /// <summary>
        /// Gets the element initializer contained in the expression tree.
        /// </summary>
        public ElementInit ElementInit { get; }

        /// <summary>
        /// Checks whether the current node is equal to the specified element initializer expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Element initializer expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        public bool Equals(ElementInitExpressionTreeNode other)
        {
            if (other == null)
            {
                return false;
            }

            return new ExpressionEqualityComparator().Equals(ElementInit, other.ElementInit);
        }

        /// <summary>
        /// Checks whether the current node is equal to the specified expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        protected override bool EqualsCore(ExpressionTreeNode other) => Equals(other as ElementInitExpressionTreeNode);

        /// <summary>
        /// Checks whether the current node is equal to the specified object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>true if the current instance and the specified object are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as ElementInitExpressionTreeNode);

        /// <summary>
        /// Gets a hash code representation of the current element initializer node instance.
        /// </summary>
        /// <returns>Hash code representation of the current element initializer node instance.</returns>
        public override int GetHashCode() => new ExpressionEqualityComparator().GetHashCode(ElementInit);

        /// <summary>
        /// Gets a string representation of the current element initializer node instance.
        /// </summary>
        /// <returns>String representation of the current element initializer node instance.</returns>
        public override string ToString() => "ElementInit[" + ElementInit.AddMethod.ToCSharpString() + "]";
    }

    /// <summary>
    /// Represents an expression tree node containing a member binding.
    /// </summary>
    public abstract class MemberBindingExpressionTreeNode : ExpressionTreeNode, IEquatable<MemberBindingExpressionTreeNode>
    {
        /// <summary>
        /// Creates an expression tree node containing a member binding.
        /// </summary>
        /// <param name="nodeType">Type of the member binding node.</param>
        /// <param name="binding">Member binding contained in the expression tree.</param>
        protected MemberBindingExpressionTreeNode(ExpressionTreeNodeType nodeType, MemberBinding binding)
            : base(nodeType)
        {
            MemberBinding = binding;
        }

        /// <summary>
        /// Gets the member binding contained in the expression tree.
        /// </summary>
        public MemberBinding MemberBinding { get; }

        /// <summary>
        /// Checks whether the current node is equal to the specified expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        protected override bool EqualsCore(ExpressionTreeNode other) => Equals(other as MemberBindingExpressionTreeNode);

        /// <summary>
        /// Checks whether the current node is equal to the specified member binding expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Member binding expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        public bool Equals(MemberBindingExpressionTreeNode other)
        {
            if (other == null)
            {
                return false;
            }

            if (NodeType != other.NodeType)
            {
                return false;
            }

            return new ExpressionEqualityComparator().Equals(MemberBinding, other.MemberBinding);
        }

        /// <summary>
        /// Checks whether the current node is equal to the specified object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>true if the current instance and the specified object are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as MemberBindingExpressionTreeNode);

        /// <summary>
        /// Gets a hash code representation of the current member binding node instance.
        /// </summary>
        /// <returns>Hash code representation of the current member binding node instance.</returns>
        public override int GetHashCode() => new ExpressionEqualityComparator().GetHashCode(MemberBinding);
    }

    /// <summary>
    /// Represents an expression tree node containing a member assignment.
    /// </summary>
    public sealed class MemberAssignmentExpressionTreeNode : MemberBindingExpressionTreeNode, IEquatable<MemberAssignmentExpressionTreeNode>
    {
        /// <summary>
        /// Creates an expression tree node containing a member assignment.
        /// </summary>
        /// <param name="memberAssignment">Member assignment contained in the expression tree.</param>
        public MemberAssignmentExpressionTreeNode(MemberAssignment memberAssignment)
            : base(ExpressionTreeNodeType.MemberAssignment, memberAssignment)
        {
            if (memberAssignment == null)
                throw new ArgumentNullException(nameof(memberAssignment));
        }

        /// <summary>
        /// Gets the member assignment contained in the expression tree.
        /// </summary>
        public MemberAssignment MemberAssignment => (MemberAssignment)MemberBinding;

        /// <summary>
        /// Checks whether the current node is equal to the specified object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>true if the current instance and the specified object are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is MemberAssignmentExpressionTreeNode n && Equals(n);

        /// <summary>
        /// Gets a hash code representation of the current member binding node instance.
        /// </summary>
        /// <returns>Hash code representation of the current member binding node instance.</returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Checks whether the current node is equal to the specified member assignment expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Member assignment expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        public bool Equals(MemberAssignmentExpressionTreeNode other) => base.Equals(other);

        /// <summary>
        /// Gets a string representation of the current member assignment node instance.
        /// </summary>
        /// <returns>String representation of the current member assignment node instance.</returns>
        public override string ToString() => "MemberAssignment[" + MemberAssignment.Member.ToCSharpString() + "]";
    }

    /// <summary>
    /// Represents an expression tree node containing a member list binding.
    /// </summary>
    public sealed class MemberListBindingExpressionTreeNode : MemberBindingExpressionTreeNode, IEquatable<MemberListBindingExpressionTreeNode>
    {
        /// <summary>
        /// Creates an expression tree node containing a member list binding.
        /// </summary>
        /// <param name="memberListBinding">Member list binding contained in the expression tree.</param>
        public MemberListBindingExpressionTreeNode(MemberListBinding memberListBinding)
            : base(ExpressionTreeNodeType.MemberListBinding, memberListBinding)
        {
            if (memberListBinding == null)
                throw new ArgumentNullException(nameof(memberListBinding));
        }

        /// <summary>
        /// Gets the member list binding contained in the expression tree.
        /// </summary>
        public MemberListBinding MemberListBinding => (MemberListBinding)MemberBinding;

        /// <summary>
        /// Checks whether the current node is equal to the specified object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>true if the current instance and the specified object are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is MemberListBindingExpressionTreeNode n && Equals(n);

        /// <summary>
        /// Gets a hash code representation of the current member binding node instance.
        /// </summary>
        /// <returns>Hash code representation of the current member binding node instance.</returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Checks whether the current node is equal to the specified member list binding expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Member list binding expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        public bool Equals(MemberListBindingExpressionTreeNode other) => base.Equals(other);

        /// <summary>
        /// Gets a string representation of the current member list binding node instance.
        /// </summary>
        /// <returns>String representation of the current member list binding node instance.</returns>
        public override string ToString() => "MemberListBinding[" + MemberListBinding.Member.ToCSharpString() + "]";
    }

    /// <summary>
    /// Represents an expression tree node containing a member member binding.
    /// </summary>
    public sealed class MemberMemberBindingExpressionTreeNode : MemberBindingExpressionTreeNode, IEquatable<MemberMemberBindingExpressionTreeNode>
    {
        /// <summary>
        /// Creates an expression tree node containing a member member binding.
        /// </summary>
        /// <param name="memberMemberBinding">Member member binding contained in the expression tree.</param>
        public MemberMemberBindingExpressionTreeNode(MemberMemberBinding memberMemberBinding)
            : base(ExpressionTreeNodeType.MemberMemberBinding, memberMemberBinding)
        {
            if (memberMemberBinding == null)
                throw new ArgumentNullException(nameof(memberMemberBinding));
        }

        /// <summary>
        /// Gets the member member binding contained in the expression tree.
        /// </summary>
        public MemberMemberBinding MemberMemberBinding => (MemberMemberBinding)MemberBinding;

        /// <summary>
        /// Checks whether the current node is equal to the specified object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>true if the current instance and the specified object are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is MemberMemberBindingExpressionTreeNode n && Equals(n);

        /// <summary>
        /// Gets a hash code representation of the current member binding node instance.
        /// </summary>
        /// <returns>Hash code representation of the current member binding node instance.</returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Checks whether the current node is equal to the specified member member binding expression tree node.
        /// Equality for expression tree nodes is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Member member binding expression tree node to compare to.</param>
        /// <returns>true if both nodes are equal; otherwise, false.</returns>
        public bool Equals(MemberMemberBindingExpressionTreeNode other) => base.Equals(other);

        /// <summary>
        /// Gets a string representation of the current member member binding node instance.
        /// </summary>
        /// <returns>String representation of the current member member binding node instance.</returns>
        public override string ToString() => "MemberMemberBinding[" + MemberMemberBinding.Member.ToCSharpString() + "]";
    }

    /// <summary>
    /// Type of an expression tree node.
    /// </summary>
    public enum ExpressionTreeNodeType
    {
        /// <summary>
        /// The expression tree node represents an expression.
        /// </summary>
        Expression,

        /// <summary>
        /// The expression tree node represents an element initializer.
        /// </summary>
        ElementInit,

        /// <summary>
        /// The expression tree node represents a member assignment.
        /// </summary>
        MemberAssignment,

        /// <summary>
        /// The expression tree node represents a member list binding.
        /// </summary>
        MemberListBinding,

        /// <summary>
        /// The expression tree node represents a member member binding.
        /// </summary>
        MemberMemberBinding,
    }
}
