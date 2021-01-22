// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Base class for expression trees.
    /// </summary>
    public abstract class ExpressionTreeBase : Tree<ExpressionTreeNode>, IEquatable<ExpressionTreeBase>, ITyped
    {
        /// <summary>
        /// Creates a new expression tree with the specified expression tree node data and children.
        /// </summary>
        /// <param name="expression">Expression tree node data in the new tree.</param>
        /// <param name="children">Children of the new tree.</param>
        protected ExpressionTreeBase(ExpressionTreeNode expression, IEnumerable<ExpressionTreeBase> children)
            : base(expression, children)
        {
        }

        /// <summary>
        /// Creates a new expression tree with the specified expression tree node data and children.
        /// </summary>
        /// <param name="expression">Expression tree node data in the new tree.</param>
        /// <param name="children">Children of the new tree.</param>
        protected ExpressionTreeBase(ExpressionTreeNode expression, params ExpressionTreeBase[] children)
            : base(expression, children)
        {
        }

        /// <summary>
        /// Checks whether the expression tree is equal to the specified expression tree.
        /// Equality for expression trees is based on structural properties of the trees.
        /// </summary>
        /// <param name="other">Expression tree to compare to.</param>
        /// <returns>true if both trees are equal; otherwise, false.</returns>
        public virtual bool Equals(ExpressionTreeBase other)
        {
            if (other == null)
            {
                return false;
            }

            return Value.Equals(other.Value);
        }

        /// <summary>
        /// Checks whether the expression tree is equal to the specified object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>true if the current instance and the specified object are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as ExpressionTreeBase);

        /// <summary>
        /// Gets a hash code representing the expression tree.
        /// </summary>
        /// <returns>Hash code representing the expression tree.</returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Gets the type of the expression tree node.
        /// </summary>
        /// <returns>Type of the expression tree node.</returns>
        IType ITyped.GetType() => GetTypeCore();

        /// <summary>
        /// Gets the type of the expression tree node.
        /// </summary>
        /// <returns>Type of the expression tree node.</returns>
        protected abstract IType GetTypeCore();
    }

    /// <summary>
    /// Represents an expression tree containing an expression.
    /// </summary>
    public abstract class ExpressionTree : ExpressionTreeBase
    {
        internal ExpressionTree(Expression expression, IEnumerable<ExpressionTreeBase> children)
            : base(new ExpressionExpressionTreeNode(expression), children)
        {
        }

        internal ExpressionTree(Expression expression, params ExpressionTreeBase[] children)
            : base(new ExpressionExpressionTreeNode(expression), children)
        {
        }

        /// <summary>
        /// Gets the expression contained in the expression tree.
        /// </summary>
        public Expression Expression => ((ExpressionExpressionTreeNode)Value).Expression;

        /// <summary>
        /// Gets a format string with placeholders for the children. To be used with String.Format.
        /// </summary>
        /// <returns>Format string that can be used to retrieve a string representation of the current node by supplying string representations of child nodes.</returns>
        public override string ToStringFormat() => Value.ToString().EscapeFormatString() + "(" + string.Join(", ", Enumerable.Range(0, Children.Count).Select(i => "{" + i + "}")) + ")";

        /// <summary>
        /// Updates the expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public ExpressionTree Update(IEnumerable<ExpressionTree> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (ExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public ExpressionTree Update(params ExpressionTree[] children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (ExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        protected override ITree<ExpressionTreeNode> UpdateCore(IEnumerable<ITree<ExpressionTreeNode>> children) => new ExpressionTreeUpdate(children).Visit(Expression);

        /// <summary>
        /// Gets the type of the expression tree node.
        /// </summary>
        /// <returns>Type of the expression tree node.</returns>
        protected override IType GetTypeCore() => new ClrType(Expression.Type);
    }

    /// <summary>
    /// Represents an expression tree containing an expression.
    /// </summary>
    /// <typeparam name="T">Type of the expression. This type has to derive from Expression.</typeparam>
    public class ExpressionTree<T> : ExpressionTree
        where T : Expression
    {
        internal ExpressionTree(T expression, IEnumerable<ExpressionTreeBase> children)
            : base(expression, children)
        {
        }

        internal ExpressionTree(T expression, params ExpressionTreeBase[] children)
            : base(expression, children)
        {
        }

        /// <summary>
        /// Gets the expression contained in the expression tree.
        /// </summary>
        public new T Expression => (T)base.Expression;
    }

    /// <summary>
    /// Represents an expression tree containing an element initializer.
    /// </summary>
    public sealed class ElementInitExpressionTree : ExpressionTreeBase
    {
        internal ElementInitExpressionTree(ElementInit elementInit, IEnumerable<ExpressionTreeBase> arguments)
            : base(new ElementInitExpressionTreeNode(elementInit), arguments)
        {
        }

        /// <summary>
        /// Gets the element initializer contained in the expression tree.
        /// </summary>
        public ElementInit ElementInit => ((ElementInitExpressionTreeNode)Value).ElementInit;

        /// <summary>
        /// Gets a format string with placeholders for the children. To be used with String.Format.
        /// </summary>
        /// <returns>Format string that can be used to retrieve a string representation of the current node by supplying string representations of child nodes.</returns>
        public override string ToStringFormat() => Value.ToString().EscapeFormatString() + "({0})";

        /// <summary>
        /// Updates the element initializer expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public ElementInitExpressionTree Update(IEnumerable<ExpressionTree> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (ElementInitExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the element initializer expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public ElementInitExpressionTree Update(params ExpressionTree[] children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (ElementInitExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the element initializer expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        protected override ITree<ExpressionTreeNode> UpdateCore(IEnumerable<ITree<ExpressionTreeNode>> children) => new ExpressionTreeUpdate(children).Visit(ElementInit);

        /// <summary>
        /// Gets the type of the expression tree node.
        /// </summary>
        /// <returns>Type of the expression tree node.</returns>
        protected override IType GetTypeCore() => ClrType.Void;
    }

    /// <summary>
    /// Represents an expression tree containing a member binding.
    /// </summary>
    public abstract class MemberBindingExpressionTree : ExpressionTreeBase
    {
        /// <summary>
        /// Creates a new expression tree with the specified member binding expression tree node data and children.
        /// </summary>
        /// <param name="memberBinding">Member binding expression tree node data in the new tree.</param>
        /// <param name="children">Children of the new tree.</param>
        protected internal MemberBindingExpressionTree(MemberBindingExpressionTreeNode memberBinding, IEnumerable<ExpressionTreeBase> children)
            : base(memberBinding, children)
        {
        }

        /// <summary>
        /// Gets the type of the expression tree node.
        /// </summary>
        /// <returns>Type of the expression tree node.</returns>
        protected override IType GetTypeCore() => ClrType.Void;
    }

    /// <summary>
    /// Represents an expression tree containing a member assignment.
    /// </summary>
    public sealed class MemberAssignmentExpressionTree : MemberBindingExpressionTree
    {
        internal MemberAssignmentExpressionTree(MemberAssignment memberAssignment, ExpressionTree expression)
            : base(new MemberAssignmentExpressionTreeNode(memberAssignment), new[] { expression })
        {
        }

        /// <summary>
        /// Gets the member assignment contained in the expression tree.
        /// </summary>
        public MemberAssignment MemberAssignment => ((MemberAssignmentExpressionTreeNode)Value).MemberAssignment;

        /// <summary>
        /// Gets a format string with placeholders for the children. To be used with String.Format.
        /// </summary>
        /// <returns>Format string that can be used to retrieve a string representation of the current node by supplying string representations of child nodes.</returns>
        public override string ToStringFormat() => Value.ToString().EscapeFormatString() + "({0})";

        /// <summary>
        /// Updates the member assignment expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public MemberAssignmentExpressionTree Update(IEnumerable<ExpressionTree> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (MemberAssignmentExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the member assignment expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public MemberAssignmentExpressionTree Update(params ExpressionTree[] children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (MemberAssignmentExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the member assignment expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        protected override ITree<ExpressionTreeNode> UpdateCore(IEnumerable<ITree<ExpressionTreeNode>> children) => new ExpressionTreeUpdate(children).Visit(MemberAssignment);
    }

    /// <summary>
    /// Represents an expression tree containing a member list binding.
    /// </summary>
    public sealed class MemberListBindingExpressionTree : MemberBindingExpressionTree
    {
        internal MemberListBindingExpressionTree(MemberListBinding memberListBinding, IEnumerable<ElementInitExpressionTree> initializers)
            : base(new MemberListBindingExpressionTreeNode(memberListBinding), initializers)
        {
        }

        /// <summary>
        /// Gets the member list binding contained in the expression tree.
        /// </summary>
        public MemberListBinding MemberListBinding => ((MemberListBindingExpressionTreeNode)Value).MemberListBinding;

        /// <summary>
        /// Gets a format string with placeholders for the children. To be used with String.Format.
        /// </summary>
        /// <returns>Format string that can be used to retrieve a string representation of the current node by supplying string representations of child nodes.</returns>
        public override string ToStringFormat() => Value.ToString().EscapeFormatString() + "(" + string.Join(", ", Enumerable.Range(0, MemberListBinding.Initializers.Count).Select(i => "{" + i + "}")) + ")";

        /// <summary>
        /// Updates the member list binding expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public MemberListBindingExpressionTree Update(IEnumerable<ElementInitExpressionTree> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (MemberListBindingExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the member list binding expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public MemberListBindingExpressionTree Update(params ElementInitExpressionTree[] children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (MemberListBindingExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the member list binding expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        protected override ITree<ExpressionTreeNode> UpdateCore(IEnumerable<ITree<ExpressionTreeNode>> children) => new ExpressionTreeUpdate(children).Visit(MemberListBinding);
    }

    /// <summary>
    /// Represents an expression tree containing a member member binding.
    /// </summary>
    public sealed class MemberMemberBindingExpressionTree : MemberBindingExpressionTree
    {
        internal MemberMemberBindingExpressionTree(MemberMemberBinding memberMemberBinding, IEnumerable<MemberBindingExpressionTree> bindings)
            : base(new MemberMemberBindingExpressionTreeNode(memberMemberBinding), bindings)
        {
        }

        /// <summary>
        /// Gets the member member binding contained in the expression tree.
        /// </summary>
        public MemberMemberBinding MemberMemberBinding => ((MemberMemberBindingExpressionTreeNode)Value).MemberMemberBinding;

        /// <summary>
        /// Gets a format string with placeholders for the children. To be used with String.Format.
        /// </summary>
        /// <returns>Format string that can be used to retrieve a string representation of the current node by supplying string representations of child nodes.</returns>
        public override string ToStringFormat() => Value.ToString().EscapeFormatString() + "(" + string.Join(", ", Enumerable.Range(0, MemberMemberBinding.Bindings.Count).Select(i => "{" + i + "}")) + ")";

        /// <summary>
        /// Updates the member member binding expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public MemberMemberBindingExpressionTree Update(IEnumerable<MemberBindingExpressionTree> children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (MemberMemberBindingExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the member member binding expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        public MemberMemberBindingExpressionTree Update(params MemberBindingExpressionTree[] children)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            return (MemberMemberBindingExpressionTree)base.Update(children);
        }

        /// <summary>
        /// Updates the member member binding expression tree with the given children, returning a new immutable tree.
        /// </summary>
        /// <param name="children">Child nodes for the copy of the current node.</param>
        /// <returns>New immutable tree based on the current node, but with the specified child nodes.</returns>
        protected override ITree<ExpressionTreeNode> UpdateCore(IEnumerable<ITree<ExpressionTreeNode>> children) => new ExpressionTreeUpdate(children).Visit(MemberMemberBinding);
    }

    internal sealed class ExpressionTreeUpdate : ExpressionVisitorNarrow<ExpressionTree>
    {
        private readonly ExpressionTreeBase[] _children;

        public ExpressionTreeUpdate(IEnumerable<ITree<ExpressionTreeNode>> children)
        {
            _children = children.Cast<ExpressionTreeBase>().ToArray();
        }

        protected override ExpressionTree VisitBinary(BinaryExpression node)
        {
            BinaryExpression expression;
            if (node.Conversion != null)
            {
                CheckChildrenCount(3);

                var l = ExtractChildExpression(0);
                var r = ExtractChildExpression(1);
                var c = (LambdaExpression)ExtractChildExpression(2);

                expression = node.Update(l, c, r);
            }
            else
            {
                CheckChildrenCount(2);

                var l = ExtractChildExpression(0);
                var r = ExtractChildExpression(1);

                expression = node.Update(l, conversion: null, r);
            }

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitConditional(ConditionalExpression node)
        {
            CheckChildrenCount(3);

            var t = ExtractChildExpression(0);
            var p = ExtractChildExpression(1);
            var n = ExtractChildExpression(2);

            var expression = node.Update(t, p, n);

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitConstant(ConstantExpression node)
        {
            CheckChildrenCount(0);
            return CreateExpressionTree(node);
        }

        protected override ExpressionTree VisitDefault(DefaultExpression node)
        {
            CheckChildrenCount(0);
            return CreateExpressionTree(node);
        }

        protected override ExpressionTree VisitInvocation(InvocationExpression node)
        {
            CheckChildrenCount(1 + node.Arguments.Count);

            var e = ExtractChildExpression(0);
            var a = ExtractChildExpressions(1, node.Arguments.Count);

            var expression = node.Update(e, a);

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitLambda<TDelegate>(Expression<TDelegate> node)
        {
            CheckChildrenCount(1 + node.Parameters.Count);

            var e = ExtractChildExpression(0);
            var p = ExtractChildExpressions(1, node.Parameters.Count).Cast<ParameterExpression>();

            var expression = node.Update(e, p);

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitListInit(ListInitExpression node)
        {
            CheckChildrenCount(1 + node.Initializers.Count);

            var n = (NewExpression)ExtractChildExpression(0);
            var i = ExtractChildInitializers(1, node.Initializers.Count);

            var expression = node.Update(n, i);

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitMember(MemberExpression node)
        {
            MemberExpression expression;
            if (node.Expression != null)
            {
                CheckChildrenCount(1);

                var e = ExtractChildExpression(0);

                expression = node.Update(e);
            }
            else
            {
                CheckChildrenCount(0);

                expression = node;
            }

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitMemberInit(MemberInitExpression node)
        {
            CheckChildrenCount(1 + node.Bindings.Count);

            var n = (NewExpression)ExtractChildExpression(0);
            var b = ExtractChildBindings(1, node.Bindings.Count);

            var expression = node.Update(n, b);

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitMethodCall(MethodCallExpression node)
        {
            MethodCallExpression expression;
            if (node.Object != null)
            {
                CheckChildrenCount(1 + node.Arguments.Count);

                var o = ExtractChildExpression(0);
                var a = ExtractChildExpressions(1, node.Arguments.Count);

                expression = node.Update(o, a);
            }
            else
            {
                CheckChildrenCount(node.Arguments.Count);

                var a = ExtractChildExpressions(0, node.Arguments.Count);

                expression = node.Update(@object: null, a);
            }

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitNew(NewExpression node)
        {
            CheckChildrenCount(node.Arguments.Count);

            var a = ExtractChildExpressions(0, node.Arguments.Count);

            var expression = node.Update(a);

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitNewArray(NewArrayExpression node)
        {
            CheckChildrenCount(node.Expressions.Count);

            var e = ExtractChildExpressions(0, node.Expressions.Count);

            var expression = node.Update(e);

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitParameter(ParameterExpression node)
        {
            CheckChildrenCount(0);
            return CreateExpressionTree(node);
        }

        protected override ExpressionTree VisitTypeBinary(TypeBinaryExpression node)
        {
            CheckChildrenCount(1);

            var e = ExtractChildExpression(0);

            var expression = node.Update(e);

            return CreateExpressionTree(expression);
        }

        protected override ExpressionTree VisitUnary(UnaryExpression node)
        {
            CheckChildrenCount(1);

            var o = ExtractChildExpression(0);

            var expression = node.Update(o);

            return CreateExpressionTree(expression);
        }

        public ElementInitExpressionTree Visit(ElementInit node)
        {
            CheckChildrenCount(node.Arguments.Count);

            var a = ExtractChildExpressions(0, node.Arguments.Count);

            var elementInit = node.Update(a);

            return new ElementInitExpressionTree(elementInit, _children);
        }

        public MemberAssignmentExpressionTree Visit(MemberAssignment node)
        {
            CheckChildrenCount(1);

            var a = ExtractChildExpression(0);

            var memberAssignment = node.Update(a);

            return new MemberAssignmentExpressionTree(memberAssignment, _children.Cast<ExpressionTree>().Single());
        }

        public MemberListBindingExpressionTree Visit(MemberListBinding node)
        {
            CheckChildrenCount(node.Initializers.Count);

            var i = ExtractChildInitializers(0, node.Initializers.Count);

            var memberListBinding = node.Update(i);

            return new MemberListBindingExpressionTree(memberListBinding, _children.Cast<ElementInitExpressionTree>());
        }

        public MemberMemberBindingExpressionTree Visit(MemberMemberBinding node)
        {
            CheckChildrenCount(node.Bindings.Count);

            var b = ExtractChildBindings(0, node.Bindings.Count);

            var memberMemberBinding = node.Update(b);

            return new MemberMemberBindingExpressionTree(memberMemberBinding, _children.Cast<MemberBindingExpressionTree>());
        }

        private Expression ExtractChildExpression(int index)
        {
            var child = _children[index];
            var node = (ExpressionExpressionTreeNode)child.Value;
            return node.Expression;
        }

        private IEnumerable<Expression> ExtractChildExpressions(int startIndex, int length) => Enumerable.Range(startIndex, length).Select(ExtractChildExpression);

        private MemberBinding ExtractChildBinding(int index)
        {
            var child = _children[index];
            var node = (MemberBindingExpressionTreeNode)child.Value;
            return node.MemberBinding;
        }

        private IEnumerable<MemberBinding> ExtractChildBindings(int startIndex, int length) => Enumerable.Range(startIndex, length).Select(ExtractChildBinding);

        private ElementInit ExtractChildInitializer(int index)
        {
            var child = _children[index];
            var node = (ElementInitExpressionTreeNode)child.Value;
            return node.ElementInit;
        }

        private IEnumerable<ElementInit> ExtractChildInitializers(int startIndex, int length) => Enumerable.Range(startIndex, length).Select(ExtractChildInitializer);

        private void CheckChildrenCount(int expectedCount)
        {
            if (_children.Length != expectedCount)
                throw Errors.InvalidChildNodeCount();
        }

        private ExpressionTree CreateExpressionTree<TExpression>(TExpression expression)
            where TExpression : Expression
        {
            return new ExpressionTree<TExpression>(expression, _children);
        }
    }
}
