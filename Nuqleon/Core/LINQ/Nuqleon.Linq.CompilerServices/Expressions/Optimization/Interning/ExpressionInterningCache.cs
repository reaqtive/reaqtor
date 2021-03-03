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
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;
using System.Memory;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// A concurrent cache for LINQ expression trees.
    /// </summary>
    public class ExpressionInterningCache : IExpressionInterningCache, IClearable
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

            var oldChildren = tree.Children;

            List<ITree<HashedNode>> newChildren = null;

            for (int i = 0, n = oldChildren.Count; i < n; i++)
            {
                var oldChild = oldChildren[i];
                var newChild = Prune(oldChild);

                if (newChildren != null)
                {
                    newChildren.Add(newChild);
                }
                else
                {
                    if (oldChild.Value.Value != newChild.Value.Value)
                    {
                        newChildren = new List<ITree<HashedNode>>(n);

                        for (var j = 0; j < i; j++)
                        {
                            newChildren.Add(oldChildren[j]);
                        }

                        newChildren.Add(newChild);
                    }
                }
            }

            if (newChildren != null)
            {
                tree = Update(tree, newChildren);
            }

            res = _nodes.GetOrAdd(tree.Value, tree);

            return res;
        }

        private ITree<HashedNode> Update(ITree<HashedNode> expr, IReadOnlyList<ITree<HashedNode>> children)
        {
            var oldExpr = expr.Value.Value;

            var n = children.Count;

            var newChildren = new object[n];
            for (int i = 0; i < n; i++)
            {
                newChildren[i] = children[i].Value.Value;
            }

            var newExpr = Update(oldExpr, newChildren);

            return expr.Value.NodeType switch
            {
                HashedNodeType.Expression =>
                    new Tree<HashedNode>(
                        new ExpressionHashedNode((Expression)newExpr, expr.Value.Hash, _comparatorFactory),
                        children
                    ),
                HashedNodeType.ElementInit =>
                    new Tree<HashedNode>(
                        new ElementInitHashedNode((ElementInit)newExpr, expr.Value.Hash, _comparatorFactory),
                        children
                    ),
                HashedNodeType.MemberBinding =>
                    new Tree<HashedNode>(
                        new MemberBindingHashedNode((MemberBinding)newExpr, expr.Value.Hash, _comparatorFactory),
                        children
                    ),
                HashedNodeType.SwitchCase =>
                    new Tree<HashedNode>(
                        new SwitchCaseHashedNode((SwitchCase)newExpr, expr.Value.Hash, _comparatorFactory),
                        children
                    ),
                HashedNodeType.CatchBlock =>
                    new Tree<HashedNode>(
                        new CatchBlockHashedNode((CatchBlock)newExpr, expr.Value.Hash, _comparatorFactory),
                        children
                    ),
                // NB: LabelTarget and CallSiteBinder do not have children.
                _ => throw new NotImplementedException(),
            };
        }

        private static object Update(object oldExpr, object[] newChildren) => new Subst(newChildren).VisitObject(oldExpr);

        private sealed class Subst : DynamicExpressionVisitor
        {
            private readonly object[] _newChildren;

            public Subst(object[] newChildren) => _newChildren = newChildren;

            public object VisitObject(object o)
            {
                return o switch
                {
                    Expression expr => base.Visit(expr),
                    ElementInit init => VisitElementInit(init),
                    MemberBinding binding => VisitMemberBinding(binding),
                    SwitchCase @case => VisitSwitchCase(@case),
                    CatchBlock catchBlock => VisitCatchBlock(catchBlock),
                    // NB: LabelTarget and CallSiteBinder do not have children.
                    _ => throw new NotImplementedException(),
                };
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                var i = 0;

                var left = Get<Expression>(ref i);
                var right = Get<Expression>(ref i);
                var conversion = GetIfNotNull(node.Conversion, ref i);

                return node.Update(left, conversion, right);
            }

            protected override Expression VisitBlock(BlockExpression node)
            {
                var i = 0;

                var variables = Get(node.Variables, ref i);
                var expressions = Get(node.Expressions, ref i);

                return node.Update(variables, expressions);
            }

            protected override Expression VisitConditional(ConditionalExpression node)
            {
                var test = (Expression)_newChildren[0];
                var ifTrue = (Expression)_newChildren[1];
                var ifFalse = (Expression)_newChildren[2];

                return node.Update(test, ifTrue, ifFalse);
            }

            protected override Expression VisitDynamic(DynamicExpression node)
            {
                var i = 0;

                var binder = Get<CallSiteBinder>(ref i);
                var arguments = Get(node.Arguments, ref i);

                return Expression.MakeDynamic(node.DelegateType, binder, arguments);
            }

            protected override ElementInit VisitElementInit(ElementInit node)
            {
                var arguments = _newChildren.Cast<Expression>();

                return node.Update(arguments);
            }

            protected override Expression VisitGoto(GotoExpression node)
            {
                var i = 0;

                var target = Get<LabelTarget>(ref i);
                var value = GetIfNotNull(node.Value, ref i);

                return node.Update(target, value);
            }

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                var i = 0;

                var expression = Get<Expression>(ref i);
                var arguments = Get(node.Arguments, ref i);

                return node.Update(expression, arguments);
            }

            protected override Expression VisitLabel(LabelExpression node)
            {
                var i = 0;

                var target = Get<LabelTarget>(ref i);
                var defaultValue = GetIfNotNull(node.DefaultValue, ref i);

                return node.Update(target, defaultValue);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var i = 0;

                var body = Get<Expression>(ref i);
                var parameters = Get(node.Parameters, ref i);

                return node.Update(body, parameters);
            }

            protected override Expression VisitListInit(ListInitExpression node)
            {
                var i = 0;

                var newExpression = Get<NewExpression>(ref i);
                var initializers = Get(node.Initializers, ref i);

                return node.Update(newExpression, initializers);
            }

            protected override Expression VisitLoop(LoopExpression node)
            {
                var i = 0;

                var body = Get<Expression>(ref i);
                var breakLabel = GetIfNotNull(node.BreakLabel, ref i);
                var continueLabel = GetIfNotNull(node.ContinueLabel, ref i);

                return node.Update(breakLabel, continueLabel, body);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                var i = 0;

                var expression = GetIfNotNull(node.Expression, ref i);

                return node.Update(expression);
            }

            protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
            {
                var expression = (Expression)_newChildren[0];

                return node.Update(expression);
            }

            protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
            {
                var initializers = _newChildren.Cast<ElementInit>();

                return node.Update(initializers);
            }

            protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
            {
                var bindings = _newChildren.Cast<MemberBinding>();

                return node.Update(bindings);
            }

            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                var i = 0;

                var newExpression = Get<NewExpression>(ref i);
                var bindings = Get(node.Bindings, ref i);

                return node.Update(newExpression, bindings);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var i = 0;

                var @object = GetIfNotNull(node.Object, ref i);
                var arguments = Get(node.Arguments, ref i);

                return node.Update(@object, arguments);
            }

            protected override Expression VisitNew(NewExpression node)
            {
                var arguments = _newChildren.Cast<Expression>();

                return node.Update(arguments);
            }

            protected override Expression VisitNewArray(NewArrayExpression node)
            {
                var expressions = _newChildren.Cast<Expression>();

                return node.Update(expressions);
            }

            protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
            {
                var variables = _newChildren.Cast<ParameterExpression>();

                return node.Update(variables);
            }

            protected override Expression VisitSwitch(SwitchExpression node)
            {
                var i = 0;

                var testValue = Get<Expression>(ref i);
                var cases = Get(node.Cases, ref i);
                var defaultBody = GetIfNotNull(node.DefaultBody, ref i);

                return node.Update(testValue, cases, defaultBody);
            }

            protected override SwitchCase VisitSwitchCase(SwitchCase node)
            {
                var i = 0;

                var body = Get<Expression>(ref i);
                var testValues = Get(node.TestValues, ref i);

                return node.Update(testValues, body);
            }

            protected override Expression VisitTry(TryExpression node)
            {
                var i = 0;

                var body = Get<Expression>(ref i);
                var handlers = Get(node.Handlers, ref i);
                var @finally = GetIfNotNull(node.Finally, ref i);
                var fault = GetIfNotNull(node.Fault, ref i);

                return node.Update(body, handlers, @finally, fault);
            }

            protected override CatchBlock VisitCatchBlock(CatchBlock node)
            {
                var i = 0;

                var body = Get<Expression>(ref i);
                var variable = GetIfNotNull(node.Variable, ref i);
                var filter = GetIfNotNull(node.Filter, ref i);

                return node.Update(variable, filter, body);
            }

            protected override Expression VisitTypeBinary(TypeBinaryExpression node)
            {
                var expression = (Expression)_newChildren[0];

                return node.Update(expression);
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                var res = node;

                if (node.Operand != null)
                {
                    var operand = (Expression)_newChildren[0];
                    res = node.Update(operand);
                }

                return res;
            }

            private T Get<T>(ref int i)
                where T : class
            {
                return (T)_newChildren[i++];
            }

            private T GetIfNotNull<T>(T expression, ref int i)
                where T : class
            {
                if (expression != null)
                {
                    return (T)_newChildren[i++];
                }

                return expression;
            }

            private T[] Get<T>(ReadOnlyCollection<T> nodes, ref int i)
                where T : class
            {
                var n = nodes.Count;

                var res = new T[n];

                for (var j = 0; j < n; j++)
                {
                    res[j] = (T)_newChildren[i++];
                }

                return res;
            }
        }
    }
}
