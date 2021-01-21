// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Provides query expression rewriting facilities for queries targeting Azure table storage by pushing down as much of the query as possible to the remote provider, while retaining the remainder portion for local evaluation.
    /// </summary>
    internal class AzureTableQueryRewriter : ExpressionVisitor
    {
        /// <summary>
        /// The method info of IQueryable.Any(Func&lt;T, bool&gt; predicate)
        /// </summary>
        private static readonly Lazy<MethodInfo> s_anyPredicate = new(() => ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<T> q) => q.Any(_ => true))).GetGenericMethodDefinition());

        /// <summary>
        /// The method info of IQueryable.SingleOrDefault(Func&lt;T, bool&gt; predicate) 
        /// </summary>
        private static readonly Lazy<MethodInfo> s_singleOrDefaultPredicate = new(() => ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<T> q) => q.SingleOrDefault(_ => true))).GetGenericMethodDefinition());

        /// <summary>
        /// The method info of IQueryable.SingleOrDefault() 
        /// </summary>
        private static readonly Lazy<MethodInfo> s_singleOrDefault = new(() => ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<T> q) => q.SingleOrDefault())).GetGenericMethodDefinition());

        /// <summary>
        /// The method info of IQueryable.Where(Func&lt;T, bool&gt; predicate) 
        /// </summary>
        private static readonly Lazy<MethodInfo> s_where = new(() => ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<T> q) => q.Where(_ => true))).GetGenericMethodDefinition());

        // Contrary to the documentation on the MSDN website, Select doesn't seem to work as expected.
        // private static readonly Lazy<MethodInfo> s_select = new Lazy<MethodInfo>(() => ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<T> q) => q.Select(_ => _))).GetGenericMethodDefinition());

        /// <summary>
        /// The method info of IQueryable.Count() 
        /// </summary>
        private static readonly Lazy<MethodInfo> s_count = new(() => ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<T> q) => q.Count())).GetGenericMethodDefinition());

        /// <summary>
        /// The method info of IQueryable.ToLocal()
        /// </summary>
        private static readonly Lazy<Expression<Func<IQueryable<T>, IQueryable<T>>>> s_toLocal = new(() => xs => xs.ToLocal());

        /// <summary>
        /// Flag to detect whether the wildcard argument of an IQueryable expression has been hidden with the generic argument from IQueryable.
        /// </summary>
        private bool _isHiding;

        /// <summary>
        /// Visits a method call expression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The result of the visit to the method call expression.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.IsGenericMethod)
            {
                var genDef = node.Method.GetGenericMethodDefinition();

                // TOOD: Investigate remote select statements further.
                // Note: Contrary to the documentation on the MSDN website, Select doesn't seem to work as expected.
                if (genDef == s_singleOrDefaultPredicate.Value)
                {
                    return VisitMethodCallSingleOrDefault(node);
                }
                else if (genDef == s_anyPredicate.Value)
                {
                    return VisitMethodCallAny(node);
                }
                else if (genDef == s_where.Value)
                {
                    return base.VisitMethodCall(node);
                }
                else if (genDef.DeclaringType == typeof(Queryable))
                {
                    var args = Visit(node.Arguments);
                    var newArgs = args.AsEnumerable();

                    var queryable = args[0].Type.FindGenericType(typeof(IQueryable<>));
                    if (queryable != null)
                    {
                        var source = HideQueryable(args[0]);
                        newArgs = new[] { source }.Concat(args.Skip(1));
                    }

                    return node.Update(null, newArgs);
                }
            }

            return base.VisitMethodCall(node);
        }

        /// <summary>
        /// Visits a method call expression to SingleOrDefault.
        /// </summary>
        /// <param name="node">The method call expression to visit.</param>
        /// <returns>The result of the visit.</returns>
        private Expression VisitMethodCallSingleOrDefault(MethodCallExpression node)
        {
            var args = Visit(node.Arguments);

            var genArg = node.Method.GetGenericArguments()[0];

            var whereMtd = s_where.Value.MakeGenericMethod(genArg);
            var where = Expression.Call(whereMtd, args[0], args[1]);

            var hidden = HideQueryable(where);

            var singleMtd = s_singleOrDefault.Value.MakeGenericMethod(genArg);
            var single = Expression.Call(singleMtd, hidden);

            return single;
        }

        /// <summary>
        /// Visits a method call expression to Any.
        /// </summary>
        /// <param name="node">The method call expression to visit.</param>
        /// <returns>The result of the visit.</returns>
        private Expression VisitMethodCallAny(MethodCallExpression node)
        {
            var args = Visit(node.Arguments);

            var genArg = node.Method.GetGenericArguments()[0];

            var whereMtd = s_where.Value.MakeGenericMethod(genArg);
            var where = Expression.Call(whereMtd, args[0], args[1]);

            var hidden = HideQueryable(where);

            var countMtd = s_count.Value.MakeGenericMethod(genArg);
            var count = Expression.Call(countMtd, hidden);

            var result = Expression.GreaterThan(count, Expression.Constant(0));

            return result;
        }

        /// <summary>
        /// Replaces wildcard types in an expression with the generic argument of IQueryable&lt;T&gt;
        /// </summary>
        /// <param name="expression">The expression to replace wildcard types in.</param>
        /// <returns>The expression with wildcard types replaced.</returns>
        private Expression HideQueryable(Expression expression)
        {
            if (_isHiding)
            {
                return expression;
            }

            var genArg = expression.Type.FindGenericType(typeof(IQueryable<>)).GetGenericArguments()[0];

            var subst = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
            {
                { typeof(T), genArg },
            });

            var hide = subst.Apply(s_toLocal.Value);

            var result = BetaReducer.Reduce(Expression.Invoke(hide, expression), BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.ExactlyOnce);

            _isHiding = true;

            return result;
        }
    }
}
