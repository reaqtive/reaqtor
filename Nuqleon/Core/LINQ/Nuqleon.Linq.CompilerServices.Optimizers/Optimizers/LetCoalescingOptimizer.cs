// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Optimizer that coalesces let clauses and flattens the transparent identifiers.
    /// </summary>
    public class LetOptimizer : IOptimizer
    {
        /// <summary>
        /// Coalesces let clauses and flattens the transparent identifiers in the original query.
        /// </summary>
        /// <param name="queryTree">The query to optimize.</param>
        /// <returns>The query with the let clauses coalesced and types flattened.</returns>
        public QueryTree Optimize(QueryTree queryTree)
        {
            var optimizer = new Impl();
            return optimizer.Visit(queryTree);
        }

        private sealed class Impl : QueryVisitor
        {
            protected internal override QueryOperator VisitSelect(SelectOperator op)
            {
                var src = VisitAndConvert<MonadMember>(op.Source);
                var selector = Visit(op.Selector);

                if (TryCast<SelectOperator>(src, OperatorType.Select, out SelectOperator select1) &&
                    TryExtractNullaryLambdaBody<LambdaExpression>(select1.Selector, ExpressionType.Lambda, out LambdaExpression selector1) &&
                    TryGetNewAnonymousType(selector1.Body, out NewExpression anonCtor1) &&
                    TryExtractNullaryLambdaBody<LambdaExpression>(selector, ExpressionType.Lambda, out LambdaExpression selector2))
                {
                    if (TryGetNewAnonymousType(selector2.Body, out NewExpression anonCtor2))
                    {
                        if (anonCtor2.Members.Count > 0 && anonCtor2.Arguments[0] == selector2.Parameters[0])
                        {
                            var newArgs = new Expression[anonCtor2.Members.Count];
                            newArgs[0] = anonCtor1;

                            for (var i = 1; i < newArgs.Length; i++)
                            {
                                var visitor = new ValidateMemberAccessesAndInlineVisitor(anonCtor1, selector1.Parameters[0], selector2.Parameters[0]);
                                newArgs[i] = visitor.Visit(anonCtor2.Arguments[i]);
                                if (!visitor.Valid)
                                {
                                    return UpdateSelect(op, src, selector);
                                }
                            }

                            var updated = anonCtor2.Update(newArgs);
                            var newBody =
                                DefaultQueryExpressionFactory.Instance.LambdaAbstraction(
                                    Expression.Lambda(Expression.Lambda(updated, selector1.Parameters[0])),
                                    Array.Empty<QueryTree>()
                                );
                            return
                                op.QueryExpressionFactory.Select(
                                    op.ElementType,
                                    select1.InputElementType,
                                    select1.Source,
                                    newBody
                                );
                        }
                    }
                    else
                    {
                        var flatteningValidationVisitor = new ValidateMemberAccessVisitor(anonCtor1, selector2.Parameters[0]);
                        flatteningValidationVisitor.Visit(selector2.Body);
                        if (flatteningValidationVisitor.Valid)
                        {
                            var flattener = new AnonymousTypeFlattener(anonCtor1);
                            if (flattener.TryConstruct(out NewExpression c))
                            {
                                var t = c.Type;
                                var newParam = Expression.Parameter(t);
                                var flatteningVisitor = new InlineFlattenedTypeVisitor(anonCtor1, newParam, selector2.Parameters[0], flattener);
                                var f = flatteningVisitor.Visit(selector2.Body);

                                var child =
                                    select1.QueryExpressionFactory.Select(
                                        t,
                                        select1.InputElementType,
                                        select1.Source,
                                        DefaultQueryExpressionFactory.Instance.LambdaAbstraction(
                                            Expression.Lambda(Expression.Lambda(c, selector1.Parameters[0])),
                                            Array.Empty<QueryTree>()
                                        )
                                    );

                                return
                                    op.QueryExpressionFactory.Select(
                                        op.ElementType,
                                        t,
                                        child,
                                        DefaultQueryExpressionFactory.Instance.LambdaAbstraction(
                                            Expression.Lambda(Expression.Lambda(f, newParam)),
                                            Array.Empty<QueryTree>()
                                        )
                                    );
                            }
                        }
                    }
                }

                return UpdateSelect(op, src, selector);
            }

            private QueryOperator UpdateSelect(SelectOperator op, MonadMember src, QueryTree selector)
            {
                if (src != op.Source || selector != op.Selector)
                {
                    return MakeSelect(op, src, selector);
                }

                return op;
            }

            private static bool TryCast<T>(Expression expression, ExpressionType nodeType, out T converted)
                where T : Expression
            {
                if (expression.NodeType == nodeType)
                {
                    converted = expression as T;
                    return converted != null;
                }

                converted = null;
                return false;
            }

            private static bool TryCast<T>(QueryTree queryExpressionNode, OperatorType nodeType, out T converted)
                where T : QueryOperator
            {
                if (queryExpressionNode.QueryNodeType == QueryNodeType.Operator)
                {
                    var op = (QueryOperator)queryExpressionNode;

                    if (op.NodeType == nodeType)
                    {
                        converted = op as T;
                        return converted != null;
                    }
                }

                converted = null;
                return false;
            }

            private static bool TryExtractNullaryLambdaBody<T>(QueryTree queryTree, ExpressionType nodeType, out T result)
                where T : Expression
            {
                if (queryTree.QueryNodeType == QueryNodeType.Lambda)
                {
                    var lambda = (LambdaAbstraction)queryTree;

                    if (lambda.Parameters.Count == 0)
                    {
                        return TryCast<T>(lambda.Body.Body, nodeType, out result);
                    }
                }

                result = null;
                return false;
            }

            private static bool TryGetNewAnonymousType(Expression expression, out NewExpression result)
            {
                if (TryCast<NewExpression>(expression, ExpressionType.New, out result) && result.Members != null)
                {
                    return true;
                }

                result = null;
                return false;
            }

            private abstract class MemberChainVisitor : ExpressionVisitor
            {
                private readonly List<MemberExpression> _memberChain = new();

                protected sealed override Expression VisitMember(MemberExpression node)
                {
                    var visited = (MemberExpression)base.VisitMember(node);

                    int cnt;
                    if (visited.Expression.NodeType != ExpressionType.MemberAccess || ((cnt = _memberChain.Count) > 0 && visited.Expression != _memberChain[cnt - 1]))
                    {
                        _memberChain.Clear();
                    }

                    _memberChain.Add(visited);

                    if (TryVisitMemberChain(_memberChain, out Expression ret))
                    {
                        _memberChain.Clear();
                        return ret;
                    }

                    return visited;
                }

                protected abstract bool TryVisitMemberChain(IList<MemberExpression> memberChain, out Expression result);
            }

            private abstract class MemberChainAndNewExpressionLeafVisitor : MemberChainVisitor
            {
                private readonly NewExpression _root;

                private NewExpression _current;

                public MemberChainAndNewExpressionLeafVisitor(NewExpression root)
                {
                    if (root == null)
                        throw new ArgumentNullException(nameof(root));

                    if (root.Members == null)
                        throw new ArgumentException("Argument must be an anonymous type initializer.", nameof(root));

                    _root = root;
                }

                protected virtual bool ShouldStartTraversal(Expression expression) => true;

                protected sealed override bool TryVisitMemberChain(IList<MemberExpression> memberChain, out Expression result)
                {
                    var chainCount = memberChain.Count;
                    if (chainCount == 1)
                    {
                        var fst = memberChain.First();
                        if (!ShouldStartTraversal(fst.Expression))
                        {
                            result = fst;
                            return true;
                        }

                        _current = _root;
                    }

                    var lastMember = memberChain[chainCount - 1];

                    var memberCount = _current.Members.Count;
                    int i;
                    for (i = 0; i < memberCount; i++)
                    {
                        if (_current.Members[i] == lastMember.Member)
                        {
                            break;
                        }
                    }

                    if (i != memberCount)
                    {
                        var arg = _current.Arguments[i];

                        NewExpression next;
                        if (arg.NodeType == ExpressionType.New && (next = (NewExpression)arg).Members != null)
                        {
                            _current = next;
                            result = null;
                            return false;
                        }
                        else
                        {
                            if (TryVisitLeaf(memberChain, arg, out Expression res))
                            {
                                result = res;
                            }
                            else
                            {
                                result = lastMember;
                            }

                            return true;
                        }
                    }

                    result = lastMember;
                    return true;
                }

                protected abstract bool TryVisitLeaf(IList<MemberExpression> memberChain, Expression leaf, out Expression result);
            }

            private sealed class ValidateMemberAccessesAndInlineVisitor : MemberChainAndNewExpressionLeafVisitor
            {
                private readonly ParameterExpression _initial;
                private readonly ParameterExpression _converted;

                private int _unboundInitialParameters;
                private int _convertedScopes;

                public ValidateMemberAccessesAndInlineVisitor(NewExpression anonymousConstructor, ParameterExpression converted, ParameterExpression initial)
                    : base(anonymousConstructor)
                {
                    _initial = initial;
                    _converted = converted;
                }

                public bool Valid => _unboundInitialParameters == 0;

                protected override Expression VisitLambda<T>(Expression<T> node)
                {
                    // No checking necessary, the parameter is masked by inner scope
                    if (node.Parameters.Contains(_initial))
                        return node;

                    var convertedInScope = node.Parameters.Contains(_converted); // Because of reference equality hiding parameters from outer scopes

                    if (convertedInScope) _convertedScopes++;

                    var ret = base.VisitLambda<T>(node);

                    if (convertedInScope) _convertedScopes--;

                    return ret;
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    if (node == _initial)
                        _unboundInitialParameters++;

                    return base.VisitParameter(node);
                }

                protected override bool ShouldStartTraversal(Expression expression)
                {
                    return expression == _initial;
                }

                protected override bool TryVisitLeaf(IList<MemberExpression> memberChain, Expression leaf, out Expression result)
                {
                    // Inline atoms only. If an inlining happens, then we must have replaced an unbound parameter.
                    switch (leaf.NodeType)
                    {
                        case ExpressionType.Constant:
                        case ExpressionType.Default:
                            {
                                _unboundInitialParameters--;
                                result = leaf;
                                return true;
                            }
                        case ExpressionType.Parameter:
                            {
                                if (leaf == _converted && _convertedScopes == 0)
                                {
                                    _unboundInitialParameters--;
                                    result = _converted;
                                    return true;
                                }
                                else // This is the cheap option. We can in practice inline most parameters, but we need scope tracking to be sure.
                                {
                                    result = memberChain[memberChain.Count - 1];
                                    return false;
                                }
                            }
                        default:
                            {
                                result = memberChain[memberChain.Count - 1];
                                return false;
                            }
                    }
                }
            }

            private sealed class ValidateMemberAccessVisitor : MemberChainAndNewExpressionLeafVisitor
            {
                private readonly ParameterExpression _initial;

                private int _unboundInitialParameters;

                public ValidateMemberAccessVisitor(NewExpression anonymousConstructor, ParameterExpression initial)
                    : base(anonymousConstructor)
                {
                    _initial = initial;
                }

                public bool Valid => _unboundInitialParameters == 0;

                protected override Expression VisitLambda<T>(Expression<T> node)
                {
                    // No checking necessary, the parameter is masked by inner scope
                    if (node.Parameters.Contains(_initial))
                        return node;

                    return base.VisitLambda<T>(node);
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    if (node == _initial)
                        _unboundInitialParameters++;

                    return base.VisitParameter(node);
                }

                protected override bool ShouldStartTraversal(Expression expression) => expression == _initial;

                protected override bool TryVisitLeaf(IList<MemberExpression> memberChain, Expression leaf, out Expression result)
                {
                    _unboundInitialParameters--;
                    result = null;
                    return false;
                }
            }

            private sealed class InlineFlattenedTypeVisitor : MemberChainAndNewExpressionLeafVisitor
            {
                private readonly ParameterExpression _initial;
                private readonly ParameterExpression _converted;
                private readonly AnonymousTypeFlattener _flattener;

                public InlineFlattenedTypeVisitor(NewExpression anonymousConstructor, ParameterExpression converted, ParameterExpression initial, AnonymousTypeFlattener flattener)
                    : base(anonymousConstructor)
                {
                    _initial = initial;
                    _converted = converted;
                    _flattener = flattener;
                }

                protected override Expression VisitLambda<T>(Expression<T> node)
                {
                    // No checking necessary, the parameter is masked by inner scope
                    if (node.Parameters.Contains(_initial))
                        return node;

                    return base.VisitLambda<T>(node);
                }

                protected override bool ShouldStartTraversal(Expression expression) => expression == _initial;

                protected override bool TryVisitLeaf(IList<MemberExpression> memberChain, Expression leaf, out Expression result)
                {
                    result = _flattener.Convert(memberChain, _converted);
                    return true;
                }
            }

            private sealed class AnonymousTypeFlattener
            {
                private readonly NewExpression _expression;
                private readonly Dictionary<IEnumerable<MemberInfo>, MemberInfo[]> _mapper; // TODO trie is better asymptotically

                public AnonymousTypeFlattener(NewExpression expression)
                {
                    if (expression == null)
                        throw new ArgumentNullException(nameof(expression));

                    if (expression.Members == null)
                        throw new ArgumentException("Argument must be an anonymous type initializer.", nameof(expression));

                    _expression = expression;
                    _mapper = new Dictionary<IEnumerable<MemberInfo>, MemberInfo[]>(ExpressionChainComparer.Instance);
                }

                private static readonly string[] s_tupleItems = new string[]
                {
                    "Item1",
                    "Item2",
                    "Item3",
                    "Item4",
                    "Item5",
                    "Item6",
                    "Item7",
                    "Rest",
                };

                public bool TryConstruct(out NewExpression rewritten)
                {
                    var results = new List<MapEntry>();
                    if (TryConstruct(_expression, new List<MemberInfo>(), results))
                    {
                        rewritten = (NewExpression)ExpressionTupletizer.Pack(results.Select(x => x.ConstructorArgument));

                        var nestedTypes = Array.Empty<MemberInfo>();
                        var currentType = rewritten.Type;
                        for (int resultIdx = 0, tupleIdx = 0, count = results.Count; resultIdx < count; resultIdx++, tupleIdx++)
                        {
                            if (tupleIdx == s_tupleItems.Length - 1)
                            {
                                var temp = new MemberInfo[nestedTypes.Length + 1];
                                Array.Copy(nestedTypes, temp, nestedTypes.Length);
                                temp[nestedTypes.Length] = currentType.GetMember(s_tupleItems[tupleIdx]).Single();
                                nestedTypes = temp;
                                tupleIdx = 0;
                                currentType = currentType.GetGenericArguments()[s_tupleItems.Length - 1];
                            }

                            var members = new MemberInfo[nestedTypes.Length + 1];
                            Array.Copy(nestedTypes, members, nestedTypes.Length);

                            members[nestedTypes.Length] = currentType.GetMember(s_tupleItems[tupleIdx]).Single();

                            _mapper.Add(results[resultIdx].MemberChain, members);
                        }

                        return true;
                    }

                    rewritten = null;
                    return false;
                }

                private static bool TryConstruct(NewExpression expression, List<MemberInfo> prefix, List<MapEntry> results)
                {
                    var parameters = expression.Constructor.GetParameters();
                    for (int i = 0, count = expression.Members.Count; i < count; i++)
                    {
                        var arg = expression.Arguments[i];
                        var member = expression.Members[i];
                        prefix.Add(member);
                        NewExpression newExprArg;
                        if (arg.NodeType == ExpressionType.New && (newExprArg = (NewExpression)arg).Members != null)
                        {
                            if (!TryConstruct(newExprArg, prefix, results))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            // TODO Use pool for this
                            var builder = new StringBuilder();
                            builder.Append(prefix[0].DeclaringType.Name);

                            for (int j = 0, n = prefix.Count; j < n; j++)
                            {
                                builder.Append("__");
                                builder.Append(prefix[j].Name);
                            }

                            results.Add(new MapEntry
                            {
                                MemberChain = prefix.ToArray(),
                                Type = parameters[i].ParameterType,
                                Name = builder.ToString(),
                                ConstructorArgument = arg,
                            });
                        }
                        prefix.RemoveAt(prefix.Count - 1);
                    }

                    return true;
                }

                public Expression Convert(IEnumerable<MemberExpression> memberAccesses, Expression source)
                {
                    var temp = _mapper[memberAccesses.Select(m => m.Member)];
                    var ret = source;
                    for (var i = 0; i < temp.Length; i++)
                    {
                        ret = Expression.MakeMemberAccess(ret, temp[i]);
                    }

                    return ret;
                }

                private struct MapEntry
                {
                    public MemberInfo[] MemberChain;
                    public string Name;
                    public Type Type;
                    public Expression ConstructorArgument;
                }

                private sealed class ExpressionChainComparer : IEqualityComparer<IEnumerable<MemberInfo>>
                {
                    internal static readonly ExpressionChainComparer Instance = new();

                    private ExpressionChainComparer()
                    {
                    }

                    public bool Equals(IEnumerable<MemberInfo> x, IEnumerable<MemberInfo> y)
                    {
                        if (x == null && y == null)
                        {
                            return true;
                        }

                        if (x == null || y == null)
                        {
                            return false;
                        }

                        bool fstHasNext, sndHasNext;

                        using (var fst = x.GetEnumerator())
                        using (var snd = y.GetEnumerator())
                        {
                            while ((fstHasNext = fst.MoveNext()) & (sndHasNext = snd.MoveNext()))
                            {
                                if (fst.Current != snd.Current)
                                {
                                    return false;
                                }
                            }
                        }

                        return fstHasNext == sndHasNext;
                    }

                    public int GetHashCode(IEnumerable<MemberInfo> obj)
                    {
                        if (obj == null)
                            return 47;

                        var hcode = 0;
                        foreach (var o in obj)
                        {
                            hcode = unchecked(hcode * 29 + o.GetHashCode());
                        }

                        return hcode;
                    }
                }
            }
        }
    }
}
