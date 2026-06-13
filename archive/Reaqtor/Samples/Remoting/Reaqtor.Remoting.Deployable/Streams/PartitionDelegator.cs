// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtive;

using Reaqtor.QueryEngine;

namespace Reaqtor.Remoting.Deployable.Streams
{
    internal abstract class PartitionDelegator : IDelegationTarget
    {
        private static readonly ISet<Type> _primitiveTypesForEquality = new HashSet<Type>
        {
            typeof(sbyte),
            typeof(short),
            typeof(int),
            typeof(long),

            typeof(byte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),

            typeof(decimal),

            typeof(bool),

            typeof(char),

            typeof(string),
        };

        private static readonly MethodInfo s_MakeFuncTypeTR = ((MethodInfo)ReflectionHelpers.InfoOf(() => MakeFuncType<object, object>())).GetGenericMethodDefinition();

        public bool CanDelegate(ParameterExpression node, Expression expression)
        {
            var ret = Delegate(node, expression);
            return ret != expression;
        }

        public Expression Delegate(ParameterExpression node, Expression expression)
        {
            return DelegateImpl(node, expression);
        }

        protected abstract Expression CreateSubscribableProxy(Type elementType);

        private Expression DelegateImpl(ParameterExpression node, Expression expression)
        {
            if (expression.NodeType == ExpressionType.Invoke)
            {
                var invocation = (InvocationExpression)expression;
                if (invocation.Arguments[0] == node && invocation.Expression.NodeType == ExpressionType.Parameter && ((ParameterExpression)invocation.Expression).Name == "rx://operators/filter")
                {
                    var lambda = (LambdaExpression)invocation.Arguments[1];

                    if (PredicateChecker.Instance.TryCheck(lambda, out List<PartitionInfo> lst, out Expression expr))
                    {
                        var e = CreateSubscribableProxy(lambda.Parameters.Single().Type);
                        foreach (var p in lst)
                        {
                            e =
                                Expression.Invoke(
                                    Expression.Parameter(
                                        MakeFuncType(lambda.Parameters.Single().Type, p.value.Type),
                                        "custom:partition"
                                    ),
                                    e,
                                    p.keySelector,
                                    p.equalityComparer,
                                    p.value
                                );
                        }

                        if (expr != null)
                        {
                            return Expression.Invoke(invocation.Expression, e, Expression.Lambda(expr, lambda.Parameters.Single()));
                        }
                        else
                        {
                            return e;
                        }
                    }
                }
            }

            return expression;
        }

        private struct PartitionInfo
        {
            public Expression keySelector;
            public Expression equalityComparer;
            public Expression value;
        }

        private sealed class PredicateChecker
        {
            public static readonly PredicateChecker Instance = new();

            private PredicateChecker()
            {
            }

            public bool TryCheck(LambdaExpression lambda, out List<PartitionInfo> infos, out Expression rest)
            {
                var pis = default(List<PartitionInfo>);
                var rst = default(Expression);

                Check(lambda.Body, lambda.Parameters.Single(), ref pis, ref rst);

                if (pis == null)
                {
                    infos = null;
                    rest = null;
                    return false;
                }
                else
                {
                    infos = pis;
                    rest = rst;
                    return true;
                }
            }

            private void Check(Expression predicate, ParameterExpression parameter, ref List<PartitionInfo> infos, ref Expression rest)
            {
                if (predicate.NodeType == ExpressionType.AndAlso)
                {
                    var be = (BinaryExpression)predicate;
                    Check(be.Left, parameter, ref infos, ref rest);
                    Check(be.Right, parameter, ref infos, ref rest);
                }
                else
                {
                    if (rest == null)
                    {
                        var pi = CheckSingle(predicate, parameter);
                        if (pi.HasValue)
                        {
                            infos ??= new List<PartitionInfo>();

                            infos.Add(pi.Value);
                        }
                        else
                        {
                            rest = predicate;
                        }
                    }
                    else
                    {
                        rest = Expression.AndAlso(rest, predicate);
                    }
                }
            }

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            private static readonly MethodInfo s_stringEqualsStringStringStringComparison = (MethodInfo)(ReflectionHelpers.InfoOf(() => string.Equals(default(string), default(string), default(StringComparison))));
#pragma warning restore IDE0034 // Simplify 'default' expression
            private static readonly Expression s_CurrentCultureStringComparer = ((Expression<Func<IEqualityComparer<string>>>)(() => StringComparer.CurrentCulture)).Body;
            private static readonly Expression s_CurrentCultureIgnoreCaseStringComparer = ((Expression<Func<IEqualityComparer<string>>>)(() => StringComparer.CurrentCultureIgnoreCase)).Body;
            private static readonly Expression s_InvariantCultureStringComparer = ((Expression<Func<IEqualityComparer<string>>>)(() => StringComparer.InvariantCulture)).Body;
            private static readonly Expression s_InvariantCultureIgnoreCaseStringComparer = ((Expression<Func<IEqualityComparer<string>>>)(() => StringComparer.InvariantCultureIgnoreCase)).Body;
            private static readonly Expression s_OrdinalStringComparer = ((Expression<Func<IEqualityComparer<string>>>)(() => StringComparer.Ordinal)).Body;
            private static readonly Expression s_OrdinalIgnoreCaseStringComparer = ((Expression<Func<IEqualityComparer<string>>>)(() => StringComparer.OrdinalIgnoreCase)).Body;

            private static PartitionInfo? CheckSingle(Expression predicate, ParameterExpression parameter)
            {
                // x => x == <constant>?
                if (predicate.NodeType == ExpressionType.Equal)
                {
                    var bin = (BinaryExpression)predicate;
                    var lhs = bin.Left;
                    var rhs = bin.Right;

                    if (lhs.NodeType is ExpressionType.Constant or ExpressionType.Default)
                    {
                        lhs = bin.Right;
                        rhs = bin.Left;
                    }

                    if (rhs.NodeType is ExpressionType.Constant or ExpressionType.Default)
                    {
                        var con = (ConstantExpression)rhs;
                        var t = con.Type;
                        if (_primitiveTypesForEquality.Contains(t) && IsPure(lhs, parameter))
                        {
                            var defaultEq = DefaultComparerExpression(t);

                            return new PartitionInfo
                            {
                                keySelector = Expression.Quote(Expression.Lambda(lhs, parameter)),
                                equalityComparer = defaultEq,
                                value = rhs,
                            };
                        }
                    }
                }
                // string.Equals(x, y, StringComparisons.<comparison>)
                else if (predicate.NodeType == ExpressionType.Call)
                {
                    var call = (MethodCallExpression)predicate;
                    if (call.Method == s_stringEqualsStringStringStringComparison && call.Arguments[2].NodeType == ExpressionType.Constant)
                    {
                        var lhs = call.Arguments[0];
                        var rhs = call.Arguments[1];

                        if (lhs.NodeType is ExpressionType.Constant or ExpressionType.Default)
                        {
                            lhs = call.Arguments[1];
                            rhs = call.Arguments[0];
                        }

                        if (rhs.NodeType is ExpressionType.Constant or ExpressionType.Default)
                        {
                            if (IsPure(lhs, parameter))
                            {
                                var sc = (StringComparison)((ConstantExpression)call.Arguments[2]).Value;

                                var eq = sc switch
                                {
                                    StringComparison.CurrentCulture => s_CurrentCultureStringComparer,
                                    StringComparison.CurrentCultureIgnoreCase => s_CurrentCultureIgnoreCaseStringComparer,
                                    StringComparison.InvariantCulture => s_InvariantCultureStringComparer,
                                    StringComparison.InvariantCultureIgnoreCase => s_InvariantCultureIgnoreCaseStringComparer,
                                    StringComparison.Ordinal => s_OrdinalStringComparer,
                                    StringComparison.OrdinalIgnoreCase => s_OrdinalIgnoreCaseStringComparer,
                                    _ => throw new InvalidOperationException("The provided StringComparison was not recognized: " + sc),
                                };

                                return new PartitionInfo
                                {
                                    keySelector = Expression.Quote(Expression.Lambda(lhs, parameter)),
                                    equalityComparer = eq,
                                    value = rhs,
                                };
                            }
                        }
                    }
                }

                return null;
            }
        }

        private static Type MakeFuncType(Type T, Type R)
        {
            return (Type)s_MakeFuncTypeTR.MakeGenericMethod(T, R).Invoke(null, Array.Empty<object>());
        }

        private static Type MakeFuncType<T, R>()
        {
            return typeof(Func<ISubscribable<T>, Expression<Func<T, R>>, IEqualityComparer<R>, R, ISubscribable<T>>);
        }

        private static readonly MethodInfo s_defaultComparerExpression = ((MethodInfo)ReflectionHelpers.InfoOf(() => DefaultComparerExpression<object>())).GetGenericMethodDefinition();

        private static Expression DefaultComparerExpression(Type t)
        {
            return (Expression)s_defaultComparerExpression.MakeGenericMethod(new[] { t }).Invoke(null, Array.Empty<object>());
        }

        private static Expression DefaultComparerExpression<T>()
        {
            return ((Expression<Func<IEqualityComparer<T>>>)(() => EqualityComparer<T>.Default)).Body;
        }

        private static bool IsPure(Expression expr, ParameterExpression parameter)
        {
            return new SimplePurityAnalyzer(parameter).Visit(expr);
        }

        private sealed class SimplePurityAnalyzer : ExpressionVisitor<bool>
        {
            private readonly ParameterExpression _parameter;

            public SimplePurityAnalyzer(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            protected override bool VisitBinary(BinaryExpression node) => false;

            protected override bool VisitBlock(BlockExpression node) => false;

            protected override bool VisitConditional(ConditionalExpression node) => false;

            protected override bool VisitConstant(ConstantExpression node) => false;

            protected override bool VisitDebugInfo(DebugInfoExpression node) => false;

            protected override bool VisitDefault(DefaultExpression node) => false;

            protected override bool VisitDynamic(DynamicExpression node) => false;

            protected override bool VisitExtension(Expression node) => false;

            protected override bool VisitGoto(GotoExpression node) => false;

            protected override bool VisitIndex(IndexExpression node) => false;

            protected override bool VisitInvocation(InvocationExpression node) => false;

            protected override bool VisitLabel(LabelExpression node) => false;

            protected override bool VisitLambda<T>(Expression<T> node) => false;

            protected override bool VisitListInit(ListInitExpression node) => false;

            protected override bool VisitLoop(LoopExpression node) => false;

            protected override bool VisitMember(MemberExpression node)
            {
                if (node.Member.MemberType == MemberTypes.Property)
                {
                    var t = node.Member.DeclaringType;
                    return (t.IsAnonymousType() || t.IsRecordType()) && base.Visit(node.Expression);
                }

                return false;
            }

            protected override bool VisitMemberInit(MemberInitExpression node) => false;

            protected override bool VisitMethodCall(MethodCallExpression node) => false;

            protected override bool VisitNew(NewExpression node) => false;

            protected override bool VisitNewArray(NewArrayExpression node) => false;

            protected override bool VisitParameter(ParameterExpression node) => node == _parameter;

            protected override bool VisitRuntimeVariables(RuntimeVariablesExpression node) => false;

            protected override bool VisitSwitch(SwitchExpression node) => false;

            protected override bool VisitTry(TryExpression node) => false;

            protected override bool VisitTypeBinary(TypeBinaryExpression node) => false;

            protected override bool VisitUnary(UnaryExpression node) => false;
        }
    }
}
