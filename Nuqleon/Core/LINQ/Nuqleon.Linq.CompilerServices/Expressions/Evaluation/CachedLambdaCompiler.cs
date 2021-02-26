// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Provides a set of compilation methods for lambda expressions that support caching the compiled delegate.
    /// </summary>
    public static class CachedLambdaCompiler
    {
        /// <summary>
        /// Compiles the specified lambda expression by hoisting constants from the expression and consulting
        /// the cache for the resulting templatized lambda expression. This technique increases the likelihood
        /// for a cache hit.
        /// </summary>
        /// <param name="expression">Lambda expression to compile.</param>
        /// <param name="cache">Cache to hold the compiled delegate.</param>
        /// <returns>Compiled delegate for the specified lambda expression.</returns>
        public static Delegate Compile(this LambdaExpression expression, ICompiledDelegateCache cache)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return CompileImpl(expression, cache, outliningEnabled: true, hoister: null);
        }

        /// <summary>
        /// Compiles the specified lambda expression by hoisting constants from the expression and consulting
        /// the cache for the resulting templatized lambda expression. This technique increases the likelihood
        /// for a cache hit.
        /// </summary>
        /// <param name="expression">Lambda expression to compile.</param>
        /// <param name="cache">Cache to hold the compiled delegate.</param>
        /// <param name="outliningEnabled">Specifies whether nested lambda expressions should be outlined into delegate constants by recursive compilation using the cache.</param>
        /// <returns>Compiled delegate for the specified lambda expression.</returns>
        public static Delegate Compile(this LambdaExpression expression, ICompiledDelegateCache cache, bool outliningEnabled)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return CompileImpl(expression, cache, outliningEnabled, hoister: null);
        }

        /// <summary>
        /// Compiles the specified lambda expression by hoisting constants from the expression and consulting
        /// the cache for the resulting templatized lambda expression. This technique increases the likelihood
        /// for a cache hit.
        /// </summary>
        /// <param name="expression">Lambda expression to compile.</param>
        /// <param name="cache">Cache to hold the compiled delegate.</param>
        /// <param name="outliningEnabled">Specifies whether nested lambda expressions should be outlined into delegate constants by recursive compilation using the cache.</param>
        /// <param name="hoister">Constant hoister used to selectively hoist constants in the specified expression.</param>
        /// <returns>Compiled delegate for the specified lambda expression.</returns>
        public static Delegate Compile(this LambdaExpression expression, ICompiledDelegateCache cache, bool outliningEnabled, IConstantHoister hoister)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));
            if (hoister == null)
                throw new ArgumentNullException(nameof(hoister));

            return CompileImpl(expression, cache, outliningEnabled, hoister);
        }

        /// <summary>
        /// Compiles the specified lambda expression by hoisting constants from the expression and consulting
        /// the cache for the resulting templatized lambda expression. This technique increases the likelihood
        /// for a cache hit.
        /// </summary>
        /// <typeparam name="T">Delegate type of the lambda expression.</typeparam>
        /// <param name="expression">Lambda expression to compile.</param>
        /// <param name="cache">Cache to hold the compiled delegate.</param>
        /// <returns>Compiled delegate for the specified lambda expression.</returns>
        public static T Compile<T>(this Expression<T> expression, ICompiledDelegateCache cache)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return (T)(object)CompileImpl(expression, cache, outliningEnabled: true, hoister: null);
        }

        /// <summary>
        /// Compiles the specified lambda expression by hoisting constants from the expression and consulting
        /// the cache for the resulting templatized lambda expression. This technique increases the likelihood
        /// for a cache hit.
        /// </summary>
        /// <typeparam name="T">Delegate type of the lambda expression.</typeparam>
        /// <param name="expression">Lambda expression to compile.</param>
        /// <param name="cache">Cache to hold the compiled delegate.</param>
        /// <param name="outliningEnabled">Specifies whether nested lambda expressions should be outlined into delegate constants by recursive compilation using the cache.</param>
        /// <returns>Compiled delegate for the specified lambda expression.</returns>
        public static T Compile<T>(this Expression<T> expression, ICompiledDelegateCache cache, bool outliningEnabled)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return (T)(object)CompileImpl(expression, cache, outliningEnabled, hoister: null);
        }

        /// <summary>
        /// Compiles the specified lambda expression by hoisting constants from the expression and consulting
        /// the cache for the resulting templatized lambda expression. This technique increases the likelihood
        /// for a cache hit.
        /// </summary>
        /// <typeparam name="T">Delegate type of the lambda expression.</typeparam>
        /// <param name="expression">Lambda expression to compile.</param>
        /// <param name="cache">Cache to hold the compiled delegate.</param>
        /// <param name="outliningEnabled">Specifies whether nested lambda expressions should be outlined into delegate constants by recursive compilation using the cache.</param>
        /// <param name="hoister">Constant hoister used to selectively hoist constants in the specified expression.</param>
        /// <returns>Compiled delegate for the specified lambda expression.</returns>
        public static T Compile<T>(this Expression<T> expression, ICompiledDelegateCache cache, bool outliningEnabled, IConstantHoister hoister)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));
            if (hoister == null)
                throw new ArgumentNullException(nameof(hoister));

            return (T)(object)CompileImpl(expression, cache, outliningEnabled, hoister);
        }

        private static Delegate CompileImpl(LambdaExpression expression, ICompiledDelegateCache cache, bool outliningEnabled, IConstantHoister hoister)
        {
            //
            // E.g. xs => xs.Bar(x => x > 0).Foo(x => x * 2)
            //
            var expr = expression;

            if (outliningEnabled)
            {
                //
                // E.g. xs => xs.Bar(delegate1).Foo(delegate2)  where delegate1 and delegate2 are constants
                //
                expr = Outline(expression, cache, hoister);
            }

            var template = ExpressionTemplatizer.Templatize(expr, hoister ?? SimpleHoister.Instance);

            //
            // E.g.  outline |                                  template expressions
            //      ---------+------------------------------------------------------------------------------------------
            //          Y    | (a, b) => xs => xs.Bar(a).Foo(b)  and recursively  c => x => x > c  and  d => x => x * d
            //          N    | (c, d) => xs => xs.Bar(x => x > c).Foo(x => x * d)
            //
            var lambda = template.Template;
            var argument = template.Argument;

            Delegate res;

            if (lambda.Parameters.Count == 0)
            {
                //
                // No template parameters, hence no tuple packing required.
                //
                res = cache.GetOrAdd(expr);
            }
            else
            {
                //
                // E.g. without outlining:  t => xs => xs.Bar(x => x > t.Item1).Foo(x => x * t.Item2)
                //
                var cachedDelegate = cache.GetOrAdd(lambda);

                //
                // E.g. new Tuple<int, int>(0, 2)
                //
                var tupleArgument = TupleEvaluator.Instance.Visit(argument);

                res = (Delegate)cachedDelegate.DynamicInvoke(new object[] { tupleArgument });
            }

            return res;
        }

        /// <summary>
        /// Outlines nested lambda expressions for consideration in the constant hoisting step. Only non-quoted
        /// lambda expressions without a closure will be considered for hoisting and subsequent caching.
        /// </summary>
        /// <param name="lambda">Lambda expression to perform outlining of inner lambda expressions on.</param>
        /// <param name="cache">Cache to hold any recursively compiled delegates.</param>
        /// <param name="hoister">Constant hoister used to selectively hoist constants in the specified expression.</param>
        /// <returns>Original lambda expression with outlined inner lambda expressions substituted for constant delegate expressions.</returns>
        /// <remarks>Future extensions can provide for a means to control the outlining policy, e.g. to insert a thunk to defer recursive compilation.</remarks>
        private static LambdaExpression Outline(LambdaExpression lambda, ICompiledDelegateCache cache, IConstantHoister hoister)
        {
            var oldBody = lambda.Body;

            var newBody = ExpressionOutliner.Outline(oldBody, cache, hoister);

            if (ReferenceEquals(oldBody, newBody))
            {
                return lambda;
            }

            var result = Expression.Lambda(lambda.Type, newBody, lambda.Name, lambda.TailCall, lambda.Parameters);

            return result;
        }

        private static class ExpressionTemplatizer
        {
            public static ExpressionTemplate Templatize(Expression e, IConstantHoister hoister)
            {
                var env = hoister.Hoist(e);

                var bindings = env.Bindings;
                var n = bindings.Count;

                var res = new ExpressionTemplate();

                if (n == 0)
                {
                    res.Template = Expression.Lambda(e);
                }
                else
                {
                    var parameters = new ParameterExpression[n];
                    var arguments = new Expression[n];

                    for (var i = 0; i < n; i++)
                    {
                        var c = bindings[i];
                        parameters[i] = c.Parameter;
                        arguments[i] = c.Value;
                    }

                    //
                    // In case you wonder why we're not building a LambdaExpression from the parameters
                    // and the visited body, there are two reasons.
                    //
                    //
                    // The first reason is due to the way the LambdaCompiler generates code for closures,
                    // which can be illustrated with this example:
                    //
                    //   (c1, c2, c3) => (arg1, arg2) => f(arg1, c1, arg2, c2, c3)
                    //
                    // In here, the outermost lambda contains the hoisted constants, and the innermost
                    // lambda matches the original lambda's signature. When we compile this higher-order
                    // expression and then invoke the outer delegate to re-supply the constants, we end
                    // up with a delegate whose target is a System.Runtime.CompilerServices.Closure which
                    // contains two fields:
                    //
                    //   object[] Constants;
                    //   object[] Locals;
                    //
                    // Due to constant hoisting, the first array is empty. The second array will hold
                    // the variables that are closed over, in the form of StrongBox<T> objects, so we
                    // end up with the Locals containing:
                    //
                    //   new object[]
                    //   {
                    //       new StrongBox<T1> { Value = c1 },
                    //       new StrongBox<T2> { Value = c2 },
                    //       new StrongBox<T3> { Value = c3 },
                    //   }
                    //
                    // Uses of c1, c2, and c3 inside the inner lambda will effectively become accesses
                    // to the closure using a field traversal like this:
                    //
                    //   ((StrongBox<T1>)closure.Locals[0]).Value
                    //
                    // For N constants we have N allocations of a StrongBox<T>. If instead we use a
                    // single tuple to hold all of the constants, we reduce this cost slightly, at the
                    // expense of requiring one more property lookup to access the constant at runtime.
                    //
                    // NB: We could consider using a ValueTuple in the future (which was added to .NET
                    //     much later than the original implementation of this library) to avoid the
                    //     cost of accessing properties, though we should have a hard look at code gen
                    //     to a) make sure that the JITted code does not already elide the call, and
                    //     more importantly b) that no copies of ValueTuple values are made, and c) that
                    //     we don't end up just boxing the ValueTuple and thus undo the potential gains.
                    //
                    //
                    // The second (and original) reason is quite subtle. For lambda expressions with an
                    // arity of 17 and beyond the Expression.Lambda factory method will use lightweight
                    // code generation to create a delegate type. The code for this can be found in:
                    //
                    //  %DDROOT%\sources\ndp\fx\src\Core\Microsoft\Scripting\Compiler\AssemblyGen.cs
                    //
                    // The dynamic assembly used to host those delegate types is generated with the Run
                    // option rather than RunAndCollect. If we end up creating a lambda expression that
                    // uses LCG-generated types that are marked as RunAndCollect, an exception occurs:
                    //
                    //  System.NotSupportedException: A non-collectible assembly may not reference a collectible assembly.
                    //    at System.Reflection.Emit.ModuleBuilder.GetTypeRef(RuntimeModule module, String strFullName, RuntimeModule refedModule, String strRefedModuleFileName, Int32 tkResolution)
                    //    at System.Reflection.Emit.ModuleBuilder.GetTypeRefNested(Type type, Module refedModule, String strRefedModuleFileName)
                    //    at System.Reflection.Emit.ModuleBuilder.GetTypeTokenWorkerNoLock(Type type, Boolean getGenericDefinition)
                    //    at System.Reflection.Emit.ModuleBuilder.GetTypeTokenInternal(Type type, Boolean getGenericDefinition)
                    //    at System.Reflection.Emit.SignatureHelper.AddOneArgTypeHelperWorker(Type clsArgument, Boolean lastWasGenericInst)
                    //    at System.Reflection.Emit.SignatureHelper.AddOneArgTypeHelperWorker(Type clsArgument, Boolean lastWasGenericInst)
                    //    at System.Reflection.Emit.SignatureHelper.AddOneArgTypeHelper(Type clsArgument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
                    //    at System.Reflection.Emit.SignatureHelper.AddArguments(Type[] arguments, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
                    //    at System.Reflection.Emit.SignatureHelper.GetMethodSigHelper(Module scope, CallingConventions callingConvention, Int32 cGenericParam, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
                    //    at System.Reflection.Emit.MethodBuilder.GetMethodSignature()
                    //    at System.Reflection.Emit.MethodBuilder.GetTokenNoLock()
                    //    at System.Reflection.Emit.MethodBuilder.GetToken()
                    //    at System.Reflection.Emit.MethodBuilder.SetImplementationFlags(MethodImplAttributes attributes)
                    //    at System.Linq.Expressions.Compiler.DelegateHelpers.MakeNewCustomDelegate(Type[] types)
                    //    at System.Linq.Expressions.Compiler.DelegateHelpers.MakeDelegateType(Type[] types)
                    //    at System.Linq.Expressions.Expression.Lambda(Expression body, String name, Boolean tailCall, IEnumerable`1 parameters)
                    //    at System.Linq.Expressions.Expression.Lambda(Expression body, ParameterExpression[] parameters)
                    //
                    // To work around this limitation, we sidestep the creation of a lambda altogether
                    // and use a specialized overload to Pack that builds a tupletized lambda from the
                    // specified body and parameters collection.
                    //
                    res.Template = ExpressionTupletizer.Pack(env.Expression, parameters);
                    res.Argument = ExpressionTupletizer.Pack(arguments, setNewExpressionMembers: false);
                }

                return res;
            }
        }

        private sealed class SimpleHoister : IConstantHoister
        {
            public static readonly IConstantHoister Instance = new SimpleHoister();

            public IExpressionWithEnvironment Hoist(Expression expression)
            {
                var impl = new Impl();
                var res = impl.Visit(expression);
                return new ExpressionWithEnvironment(res, impl.bindings);
            }

            private sealed class Impl : ExpressionVisitor
            {
                private const string ParameterPrefix = "@p";
                private const int CachedParameterPrefixCount = 16;
                private static readonly string[] ParameterPrefixes = new string[CachedParameterPrefixCount]
                {
                    "@p0",
                    "@p1",
                    "@p2",
                    "@p3",
                    "@p4",
                    "@p5",
                    "@p6",
                    "@p7",
                    "@p8",
                    "@p9",
                    "@p10",
                    "@p11",
                    "@p12",
                    "@p13",
                    "@p14",
                    "@p15"
                };

                public readonly List<Binding> bindings = new();

                protected override Expression VisitConstant(ConstantExpression node) => MakeBinding(node);

                /*
                 * Can be enabled to make more nodes match but needs more evaluator support.
                 *
                protected override Expression VisitDefault(DefaultExpression node)
                {
                    return MakeBinding(node);
                }
                 */

                private Expression MakeBinding(Expression node)
                {
                    var n = bindings.Count;
                    var name = n < CachedParameterPrefixCount ? ParameterPrefixes[n] : ParameterPrefix + n;

                    var paramExpr = Expression.Parameter(node.Type, name);

                    bindings.Add(new Binding(paramExpr, node));

                    return paramExpr;
                }
            }
        }

        private struct ExpressionTemplate
        {
            public LambdaExpression Template;
            public Expression Argument;
        }

        /// <summary>
        /// Simple recursive evaluator for tuple creation expressions produced by calls to Tupletize,
        /// therefore only containing Constant and New nodes.
        /// </summary>
        private sealed class TupleEvaluator : PartialExpressionVisitor<object>
        {
            public static readonly TupleEvaluator Instance = new();

            protected override object VisitConstant(ConstantExpression node) => node.Value;

            protected override object VisitNew(NewExpression node)
            {
                var argExps = node.Arguments;
                var n = argExps.Count;

                var args = new object[n];

                for (var i = 0; i < n; i++)
                {
                    var argExp = argExps[i];

                    var arg = Visit(argExp);
                    args[i] = arg;
                }

                //
                // No need to unwrap the TargetInvocationException. Tuple constructors don't throw.
                //
                return node.Constructor.Invoke(args);
            }
        }

        /// <summary>
        /// Outliner for nested lambda expressions that can be reduced to constant delegates. Eligible
        /// lambda expressions should not have closures or occur inside a quotation.
        /// </summary>
        private static class ExpressionOutliner
        {
            /// <summary>
            /// Outlines nested lambda expressions in the specified expression. If the expression is a
            /// lambda expression itself, it will be considered for rewriting as well. Care should be
            /// taken when calling this method as not to trigger a stack overflow.
            /// </summary>
            /// <param name="expression">Expression to apply nested lambda expression outlining on.</param>
            /// <param name="cache">Cache to hold any recursively compiled delegates.</param>
            /// <param name="hoister">Constant hoister used to selectively hoist constants in the specified expression.</param>
            /// <returns>Original expression with outlining steps applied.</returns>
            public static Expression Outline(Expression expression, ICompiledDelegateCache cache, IConstantHoister hoister) => new Visitor(cache, hoister).Visit(expression);

            private sealed class Visitor : ExpressionVisitor
            {
                private readonly ICompiledDelegateCache _cache;
                private readonly IConstantHoister _hoister;

                public Visitor(ICompiledDelegateCache cache, IConstantHoister hoister)
                {
                    _cache = cache;
                    _hoister = hoister;
                }

                protected override Expression VisitUnary(UnaryExpression node)
                {
                    if (node.NodeType == ExpressionType.Quote)
                    {
                        return node;
                    }

                    return base.VisitUnary(node);
                }

                protected override Expression VisitLambda<T>(Expression<T> node)
                {
                    if (!FreeVariableScanner.HasFreeVariables(node))
                    {
                        var d = CompileImpl(node, _cache, outliningEnabled: true, _hoister);
                        var c = Expression.Constant(d, node.Type);
                        return c;
                    }

                    return base.VisitLambda<T>(node);
                }
            }
        }
    }
}
