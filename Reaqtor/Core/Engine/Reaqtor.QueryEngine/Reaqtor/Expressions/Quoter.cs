// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;
using System.Runtime.CompilerServices;

using Reaqtive;
using Reaqtive.Expressions;

using Reaqtor.Reactive.Expressions;

namespace Reaqtor.Expressions
{
    //
    // CONSIDER: Introduce generalization in System.Linq.CompilerServices and make this class
    //           derive from it using ISubscribable<> and QuotedSubscribable<> as parameters.
    //

    /// <summary>
    /// Expression rewriter to quote <see cref="ISubscribable{T}"/> nodes in an expression tree.
    /// </summary>
    internal sealed class Quoter : ExpressionVisitor
    {
#pragma warning disable IDE0034 // Simplify 'default' expression

        /// <summary>
        /// The <see cref="MethodInfo"/> object for <see cref="QuoteFactory.Create{T, R}(T, Expression, object[])"/>.
        /// </summary>
        private static readonly MethodInfo s_factory = ((MethodInfo)ReflectionHelpers.InfoOf(() =>
                QuoteFactory.Create<_I, _Q>(default(_I), default(Expression), default(object[])))).GetGenericMethodDefinition();

        /// <summary>
        /// The <see cref="MethodInfo"/> object for <see cref="QuoteFactory.Create{T, R}(T, Expression, ValueBinding[], object[])"/>.
        /// </summary>
        private static readonly MethodInfo s_factoryScoped = ((MethodInfo)ReflectionHelpers.InfoOf(() =>
                QuoteFactory.Create<_I, _Q>(default(_I), default(Expression), default(ValueBinding[]), default(object[])))).GetGenericMethodDefinition();

#pragma warning restore IDE0034 // Simplify 'default' expression

        /// <summary>
        /// The <see cref="PropertyInfo"/> object for <see cref="IReference{T}.Value"/>.
        /// </summary>
        private static readonly PropertyInfo s_getValue = (PropertyInfo)ReflectionHelpers.InfoOf((IReference<Expression> @ref) => @ref.Value);

        /// <summary>
        /// Conditional weak table to map types onto settable properties that implement <see cref="IExpressible.Expression"/> (or null if no such property is found).
        /// </summary>
        private static readonly ConditionalWeakTable<Type, PropertyInfo> s_expressionPropertyMap = new();

        /// <summary>
        /// The <see cref="MethodInfo"/> object for the get method of the <see cref="IExpressible.Expression"/> property.
        /// </summary>
        private static readonly MethodInfo s_getExpression = typeof(IExpressible).GetProperty(nameof(IExpressible.Expression)).GetGetMethod();

        /// <summary>
        /// The expression policy used to access the expression cache.
        /// </summary>
        private readonly IExpressionPolicy _policy;

        /// <summary>
        /// The expression representing the <c>args</c> parameter passed to any of the <c>Create</c> methods
        /// on <see cref="QuoteFactory"/>, which is used to instantiate the quote.
        /// </summary>
        private readonly ConstantExpression _quoteCreationArgs;

        /// <summary>
        /// Creates a new instance of the <see cref="Quoter"/> with the specified expression <paramref name="policy"/>
        /// used to cache and evaluate expressions.
        /// </summary>
        /// <param name="policy">The expression policy used to cache and evaluate expressions.</param>
        public Quoter(IExpressionPolicy policy)
        {
            _policy = policy;
            _quoteCreationArgs = Expression.Constant(new object[] { _policy });
        }

        /// <summary>
        /// Visits a node of the expression tree in order to perform quoting. We perform a depth-first traversal
        /// prior to surrounding the node with a quote (if needed).
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>The result of quoting the node (if needed), including quoting its children (if needed).</returns>
        public override Expression Visit(Expression node)
        {
            var res = base.Visit(node);

            if (res != null)
            {
                //
                // First, check if we have an IExpressible interface implementation with a settable Expression
                // property. If so, we can quote directly without needing a wrapper type.
                //

                if (typeof(IExpressible).IsAssignableFrom(res.Type) && TryCreateExpressibleQuote(node, res, out var quoted))
                {
                    return quoted;
                }

                //
                // CONSIDER: If we need more quote types, we should generalize this mechanism.
                //

                var grpType = res.Type.FindGenericType(typeof(IGroupedSubscribable<,>));
                if (grpType != null)
                {
                    var genArgs = grpType.GetGenericArguments();
                    var quoteType = typeof(QuotedGroupedSubscribable<,>).MakeGenericType(genArgs);
                    return CreateQuote(node, res, grpType, quoteType);
                }

                var subType = res.Type.FindGenericType(typeof(ISubscribable<>));
                if (subType != null)
                {
                    var genArgs = subType.GetGenericArguments();
                    var quoteType = typeof(QuotedSubscribable<>).MakeGenericType(genArgs);
                    return CreateQuote(node, res, subType, quoteType);
                }
            }

            return res;
        }

        /// <summary>
        /// Visits a lambda expression in order to quote its <see cref="LambdaExpression.Body"/>.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="node">The lambda expression to quote.</param>
        /// <returns>The result of visiting the lambda and quoting its body (and its children).</returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            //
            // NB: This is required to keep parameters in a lambda unchanged. Notice any use of the
            //     parameters in the lambda body can get wrapped with a quote, which is okay. The
            //     original value still sees the original parameter, and the quote will include the
            //     parameter in its environment, so it gets bound upon persistence.
            //

            var body = Visit(node.Body);
            return node.Update(body, node.Parameters);
        }

        /// <summary>
        /// Visits a block expression in order to quote its <see cref="BlockExpression.Expressions"/>.
        /// </summary>
        /// <param name="node">The block expression to quote.</param>
        /// <returns>The result of visiting the block and quoting its expressions (and its children).</returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            //
            // NB: See remarks in VisitLambda<T>.
            //

            var expressions = Visit(node.Expressions);
            return node.Update(node.Variables, expressions);
        }

        /// <summary>
        /// Visits a catch block in order to quote its <see cref="CatchBlock.Body"/> and <see cref="CatchBlock.Filter"/>.
        /// </summary>
        /// <param name="node">The catch block to quote.</param>
        /// <returns>The result of visiting the catch block and quoting its body and filter (and their children).</returns>
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            //
            // NB: See remarks in VisitLambda<T>.
            //

            var filter = Visit(node.Filter);
            var body = Visit(node.Body);
            return node.Update(node.Variable, filter, body);
        }

        /// <summary>
        /// Visits <see cref="ListInitExpression"/> nodes to make sure the <see cref="ListInitExpression.NewExpression"/> nodes
        /// don't get rewritten to another node type.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>The result of visiting the node.</returns>
        protected override Expression VisitListInit(ListInitExpression node)
        {
            return node.Update(node.NewExpression.Update(Visit(node.NewExpression.Arguments)), Visit(node.Initializers, VisitElementInit));
        }

        /// <summary>
        /// Visits <see cref="MemberInitExpression"/> nodes to make sure the <see cref="MemberInitExpression.NewExpression"/> nodes
        /// don't get rewritten to another node type.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>The result of visiting the node.</returns>
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            return node.Update(node.NewExpression.Update(Visit(node.NewExpression.Arguments)), Visit(node.Bindings, VisitMemberBinding));
        }

        /// <summary>
        /// Creates an expression representing a quote around the specified <paramref name="rewritten"/>
        /// expression, exposing the <paramref name="original"/> expression on the quote through the
        /// <see cref="IExpressible.Expression"/> property (possibly wrapped by an invocation expression
        /// to apply variables from the environment).
        /// </summary>
        /// <param name="original">The original expression without quoted children.</param>
        /// <param name="rewritten">The expression being quoted, after recursively quoting its children.</param>
        /// <param name="type">The interface type of the quote.</param>
        /// <param name="quoteType">The concrete type of the quote to insert.</param>
        /// <returns>An expression representing a quote.</returns>
        private Expression CreateQuote(Expression original, Expression rewritten, Type type, Type quoteType)
        {
            var discardable = _policy.InMemoryCache.Create(original);
            var reconstructer = Expression.MakeMemberAccess(Expression.Constant(discardable), s_getValue);

            var freeVariables = ExpressionHelpers.FindFreeVariables(rewritten);

            Expression res;

            if (freeVariables.Count > 0)
            {
                var ctor = s_factoryScoped.MakeGenericMethod(type, quoteType);

                var env = GetEnvironmentExpression(freeVariables);

                res = Expression.Call(ctor, rewritten, reconstructer, env, _quoteCreationArgs);
            }
            else
            {
                var ctor = s_factory.MakeGenericMethod(type, quoteType);

                res = Expression.Call(ctor, rewritten, reconstructer, _quoteCreationArgs);
            }

            if (res.Type != original.Type)
            {
                return rewritten;
            }

            return res;
        }

        /// <summary>
        /// Tries to attach a quote to the specified <paramref name="rewritten"/> expression (which should have a static type that
        /// implements <see cref="IExpressible"/>) and referring to the specified <paramref name="original"/> expression. A quote will
        /// be created and returned in <paramref name="quoted"/> if the <paramref name="rewritten"/> expression's implementation of
        /// the <see cref="IExpressible.Expression"/> property supports assignment. Otherwise, the method returns <c>false</c>.
        /// </summary>
        /// <param name="original">The original expression prior to recursive quoting, used as the value of the quote.</param>
        /// <param name="rewritten">The rewritten expression after recursive quoting, to try to attach the quote to.</param>
        /// <param name="quoted">The result of quoting the <paramref name="rewritten"/> expression, if possible; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the <paramref name="rewritten"/> expression was successfully modified with a quote in <paramref name="quoted"/>; otherwise, <c>null</c>.</returns>
        private static bool TryCreateExpressibleQuote(Expression original, Expression rewritten, out Expression quoted)
        {
            var type = rewritten.Type;

            if (!s_expressionPropertyMap.TryGetValue(type, out var expressionProperty))
            {
                expressionProperty = s_expressionPropertyMap.GetValue(type, GetWriteableExpressionProperty);
            }

            if (expressionProperty != null)
            {
                var quote = GetQuote(original);

                //
                // We can trivially inject a quote using MemberInitExpression nodes by simply adding a binding for the settable
                // property. For any other expression type, we have to resort to a BlockExpression with a temporary variable.
                //

                if (rewritten.NodeType == ExpressionType.New)
                {
                    var newExpr = (NewExpression)rewritten;
                    quoted = Expression.MemberInit(newExpr, Expression.Bind(expressionProperty, quote));
                }
                else if (rewritten.NodeType == ExpressionType.MemberInit)
                {
                    var initExpr = (MemberInitExpression)rewritten;
                    quoted = initExpr.Update(initExpr.NewExpression, initExpr.Bindings.Concat(new[] { Expression.Bind(expressionProperty, quote) }));
                }
                else
                {
                    var temp = Expression.Parameter(rewritten.Type);
                    quoted = Expression.Block(new[] { temp }, Expression.Assign(temp, rewritten), Expression.Assign(Expression.Property(temp, expressionProperty), quote), temp);
                }

                return true;
            }

            quoted = null;
            return false;
        }

        /// <summary>
        /// Gets the property implementing <see cref="IExpressible.Expression"/> on the specified <paramref name="type"/> if the
        /// property is writeable.
        /// </summary>
        /// <param name="type">The type on which to look for a writeable implementation of the <see cref="IExpressible.Expression"/> property.</param>
        /// <returns>The writeable <see cref="IExpressible.Expression"/> property implementation, if found; otherwise, <c>null</c>.</returns>
        private static PropertyInfo GetWriteableExpressionProperty(Type type)
        {
            var expressionProp = default(PropertyInfo);

            //
            // Get the interface map for the IExpressible interface in order to get an exact match for the Expression property.
            //

            var map = type.GetInterfaceMap(typeof(IExpressible));

            //
            // Interface maps only contain entries for methods, so look for the getter of the Expression property.
            //

            var getExpressionSlot = Array.IndexOf(map.InterfaceMethods, s_getExpression);

            if (getExpressionSlot >= 0)
            {
                //
                // If we found the slot implementing the Expression property getter, match it to the corresponding method.
                //

                var getExpressionImpl = map.TargetMethods[getExpressionSlot];

                //
                // Find the property that has the getter as its get method. Note we support looking for private methods as
                // well in order to allow for explicit interface implementations. We'll only set the property variable if
                // we find it to be writeable.
                //

                var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var prop in props)
                {
                    if (prop.GetGetMethod() == getExpressionImpl && prop.CanWrite)
                    {
                        expressionProp = prop;
                        break;
                    }
                }
            }

            //
            // Return the property, or null.
            //

            return expressionProp;
        }

        /// <summary>
        /// Gets an expression representing a quote for the expression specified in <paramref name="node"/>. If the node
        /// contains unbound parameters, an environment of <see cref="ValueBinding"/> elements will be created and fed to
        /// a <see cref="QuoteHelpers.GetQuote(Expression, ValueBinding[])"/> helper method. Otherwise, the given node is
        /// simply wrapped in a <see cref="ConstantExpression"/>.
        /// </summary>
        /// <param name="node">The node to get a quote for.</param>
        /// <returns>An expression representing a quote for the specified expression.</returns>
        private static Expression GetQuote(Expression node)
        {
            var freeVariables = ExpressionHelpers.FindFreeVariables(node);

            if (freeVariables.Count > 0)
            {
                var env = GetEnvironmentExpression(freeVariables);

                return Expression.Call(QuoteHelpers.s_GetQuote, Expression.Constant(node, typeof(Expression)), env);
            }
            else
            {
                return Expression.Constant(node, typeof(Expression));
            }
        }

        /// <summary>
        /// Gets an expression representing the instantiation and initialization of a <see cref="ValueBinding"/>
        /// environment array containing the specified free variables.
        /// </summary>
        /// <param name="freeVariables">The free variables to include in the environment.</param>
        /// <returns>An expression representing the environment.</returns>
        private static Expression GetEnvironmentExpression(ICollection<ParameterExpression> freeVariables)
        {
            var n = freeVariables.Count;

            var exprs = new Expression[n];

            var i = 0;
            foreach (var freeVariable in freeVariables)
            {
                var variable = Expression.Constant(freeVariable, typeof(ParameterExpression));
                var value = freeVariable.Type.IsValueType ? Expression.Convert(freeVariable, typeof(object)) : (Expression)freeVariable;
                exprs[i++] = Expression.New(ValueBinding.Constructor, variable, value);
            }

            var env = Expression.NewArrayInit(typeof(ValueBinding), exprs);

            return env;
        }

        /// <summary>
        /// Sentinel type used for InfoOf.
        /// </summary>
        private interface _I
        {
        }

        /// <summary>
        /// Sentinel type used for InfoOf. Never called.
        /// </summary>
        private sealed class _Q : Quoted<_I>, _I
        {
            /// <summary>
            /// Never called.
            /// </summary>
            /// <param name="value">Ignored.</param>
            /// <param name="expression">Ignored.</param>
            public _Q(_I value, Expression expression)
                : base(value, expression)
            {
            }
        }
    }

    /// <summary>
    /// Provides a set of helper methods that are used at runtime to construct quotes.
    /// </summary>
    internal static class QuoteHelpers
    {
        /// <summary>
        /// The <see cref="MethodInfo"/> object representing the <see cref="GetQuote"/> method.
        /// </summary>
        public static readonly MethodInfo s_GetQuote = typeof(QuoteHelpers).GetMethod(nameof(GetQuote));

        /// <summary>
        /// Gets an expression representing a quote for the specified <paramref name="expression"/> where all variable
        /// bindings specified in <paramref name="bindings"/> have been applied.
        /// </summary>
        /// <param name="expression">The expression to return a fully bound quote for.</param>
        /// <param name="bindings">Array with bindings of variables to values.</param>
        /// <returns>An expression representing a quote in which all variables are fully bound.</returns>
        public static Expression GetQuote(Expression expression, ValueBinding[] bindings)
        {
            var n = bindings.Length;

            var parameters = new ParameterExpression[n];
            var values = new ConstantExpression[n];

            for (var i = 0; i < n; i++)
            {
                var binding = bindings[i];
                var variable = binding.Variable;

                parameters[i] = variable;
                values[i] = Expression.Constant(binding.Value, variable.Type);
            }

            return BetaReducer.Reduce(Expression.Invoke(Expression.Lambda(expression, parameters), values));
        }
    }
}
