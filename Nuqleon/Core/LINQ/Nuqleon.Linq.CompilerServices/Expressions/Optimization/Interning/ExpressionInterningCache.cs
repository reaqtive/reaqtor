// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - January 2014 - Created this file.
//

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// A concurrent cache for LINQ expression trees.
    /// </summary>
    public class ExpressionInterningCache : IExpressionInterningCache
    {
        private readonly ConcurrentDictionary<HashedNode, ITree<HashedNode>> _nodes = new();
        private readonly ExpressionToHashedExpressionTreeConverter _converter;
        private readonly Func<ExpressionEqualityComparator> _comparatorFactory;

        /// <summary>
        /// Instantiates a concurrent cache for LINQ expression trees using the
        /// default expression equality comparison strategy.
        /// </summary>
        public ExpressionInterningCache()
            : this(() => new ExpressionEqualityComparator())
        {
        }

        /// <summary>
        /// Instantiates a concurrent cache for LINQ expression trees using the
        /// given expression equality comparison strategy.
        /// </summary>
        /// <param name="comparatorFactory">A factory to generate expression equality comparers.</param>
        public ExpressionInterningCache(Func<ExpressionEqualityComparator> comparatorFactory)
        {
            _converter = new ExpressionToHashedExpressionTreeConverter(comparatorFactory);
            _comparatorFactory = comparatorFactory;
        }

        /// <summary>
        /// Gets the number of entries in the cache.
        /// </summary>
        public int Count => _nodes.Count;

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear() => _nodes.Clear();

        /// <summary>
        /// Rewrites the given expression tree with nodes from the cache.
        /// </summary>
        /// <param name="expression">The expression to rewrite.</param>
        /// <returns>
        /// An equivalent expression with pre-cached nodes replacing nodes in the given tree.
        /// </returns>
        /// <remarks>
        /// As a side-effect, adds each new node to the cache.
        /// </remarks>
        public Expression GetOrAdd(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var tree = _converter.Visit(expression);
            tree = Prune(tree);

            return ((ExpressionHashedNode)tree.Value).Expression;
        }

        private ITree<HashedNode> Prune(ITree<HashedNode> tree)
        {
            if (_nodes.TryGetValue(tree.Value, out ITree<HashedNode> res))
            {
                return res;
            }

            var children = tree.Children;
            if (children.Count > 0)
            {
                children = new List<ITree<HashedNode>>(children.Select(Prune));
            }

            var hasChanged = false;
            for (int i = 0; i < tree.Children.Count; i++)
            {
                var @old = tree.Children[i];
                var @new = children[i];

                if (@old.Value.Value != @new.Value.Value)
                {
                    hasChanged = true;
                    break;
                }
            }

            if (hasChanged)
            {
                tree = Update(tree, children);
            }

            res = _nodes.GetOrAdd(tree.Value, tree);

            return res;
        }

        private ITree<HashedNode> Update(ITree<HashedNode> expr, IReadOnlyList<ITree<HashedNode>> children)
        {
            var oldExpr = expr.Value.Value;

            var newChildren = new object[children.Count];
            for (int i = 0; i < children.Count; i++)
            {
                newChildren[i] = children[i].Value.Value;
            }

            var newExpr = Update(oldExpr, newChildren);

            return expr.Value.NodeType switch
            {
                HashedNodeType.Expression =>
                    new Tree<HashedNode>(
                        new ExpressionHashedNode(_comparatorFactory) { Expression = (Expression)newExpr, Hash = expr.Value.Hash },
                        children // TODO - need to update?
                    ),
                HashedNodeType.ElementInit =>
                    new Tree<HashedNode>(
                        new ElementInitHashedNode(_comparatorFactory) { Initializer = (ElementInit)newExpr, Hash = expr.Value.Hash },
                        children // TODO - need to update?
                    ),
                HashedNodeType.MemberBinding =>
                    new Tree<HashedNode>(
                        new MemberBindingHashedNode(_comparatorFactory) { Binding = (MemberBinding)newExpr, Hash = expr.Value.Hash },
                        children // TODO - need to update?
                    ),
                _ => throw new NotImplementedException(),
            };
        }

        private static object Update(object oldExpr, object[] newChildren) => new Subst(newChildren).VisitObject(oldExpr);

        private sealed class Subst : ExpressionVisitor
        {
            private readonly object[] _newChildren;

            public Subst(object[] newChildren) => _newChildren = newChildren;

            public object VisitObject(object o)
            {
                return o switch
                {
                    Expression e => base.Visit(e),
                    ElementInit i => VisitElementInit(i),
                    MemberBinding b => VisitMemberBinding(b),
                    _ => throw new NotImplementedException(),
                };
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                var l = (Expression)_newChildren[0];
                var r = (Expression)_newChildren[1];
                var c = _newChildren.Length == 3 ? (LambdaExpression)_newChildren[2] : null;
                return node.Update(l, c, r);
            }

            protected override Expression VisitConditional(ConditionalExpression node)
            {
                var c = (Expression)_newChildren[0];
                var t = (Expression)_newChildren[1];
                var f = (Expression)_newChildren[2];
                return node.Update(c, t, f);
            }

            protected override ElementInit VisitElementInit(ElementInit node) => node.Update(_newChildren.Cast<Expression>());

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                var e = (Expression)_newChildren[0];
                var o = _newChildren.Skip(1).Cast<Expression>();
                return node.Update(e, o);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var b = (Expression)_newChildren[0];
                var p = _newChildren.Skip(1).Cast<ParameterExpression>();
                return node.Update(b, p);
            }

            protected override Expression VisitListInit(ListInitExpression node)
            {
                var n = (NewExpression)_newChildren[0];
                var i = _newChildren.Skip(1).Cast<ElementInit>();
                return node.Update(n, i);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                var e = node.Expression != null ? (Expression)_newChildren[0] : null;
                return node.Update(e);
            }

            protected override MemberAssignment VisitMemberAssignment(MemberAssignment node) => node.Update((Expression)_newChildren[0]);

            protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
            {
                var i = _newChildren.Cast<ElementInit>();
                return node.Update(i);
            }

            protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
            {
                var b = _newChildren.Cast<MemberBinding>();
                return node.Update(b);
            }

            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                var n = (NewExpression)_newChildren[0];
                var b = _newChildren.Skip(1).Cast<MemberBinding>();
                return node.Update(n, b);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var o = node.Object != null ? (Expression)_newChildren[0] : null;
                var a = node.Object != null ? _newChildren.Skip(1).Cast<Expression>() : _newChildren.Cast<Expression>();
                return node.Update(o, a);
            }

            protected override Expression VisitNew(NewExpression node) => node.Update(_newChildren.Cast<Expression>());

            protected override Expression VisitNewArray(NewArrayExpression node) => node.Update(_newChildren.Cast<Expression>());

            protected override Expression VisitTypeBinary(TypeBinaryExpression node) => node.Update((Expression)_newChildren[0]);

            protected override Expression VisitUnary(UnaryExpression node) => node.Update((Expression)_newChildren[0]);
        }
    }
}
