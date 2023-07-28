// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Jit
{
    /// <summary>
    /// Utility to compile an expression tree with JIT compilation support
    /// for inner lambda expressions.
    /// </summary>
    internal static partial class JitCompiler
    {
        /// <summary>
        /// Prepares the specified <paramref name="expression"/> for JIT compilation support.
        /// </summary>
        /// <param name="methodTable">The parameter containing a reference to the method table.</param>
        /// <param name="analysis">The scope analysis for the expression.</param>
        /// <param name="expression">The expression to prepare for JIT compilation.</param>
        /// <param name="thunkFactory">Factory for thunk types.</param>
        /// <returns>Information used to compile the expression.</returns>
        public static JitCompilationInfo Prepare(ParameterExpression methodTable, Dictionary<object, Scope> analysis, LambdaExpression expression, IThunkFactory thunkFactory)
        {
            //
            // Rewrite the expression tree to prepare it for JIT compilation.
            // This will replace all inner lambdas with explicit calls to thunk
            // methods to create a delegate using explicit closures.
            //
            var impl = new Impl(analysis, methodTable, thunkFactory);
            var expr = impl.Visit(expression);

            //
            // Get the method table whose entries contain the thunks for the
            // inner lambdas. The rewritten expression will use array indexing
            // operations into the `mtParam` parameter to obtain the thunks.
            // The instance of the method table has to be passed to the top-
            // level compiled delegate, which is done by the caller.
            //
            var mt = impl.GetMethodTable();

            //
            // Return all the objects we've built as part of JIT compilation.
            // This enables the caller to intercept the method table we built
            // in order to perform instrumentation or to trigger compilation
            // of inner lambdas in a non-deferred manner.
            //
            return new JitCompilationInfo
            {
                MethodTableParameter = methodTable,
                Expression = expr,
                MethodTable = mt,
            };
        }

        /// <summary>
        /// Expression visitor to prepare an expression tree for use with the JIT compiler. Inner
        /// lambda expressions get registered in a methodtable and get compiled in a just-in-time
        /// manner.
        /// </summary>
        private sealed class Impl : BetterExpressionVisitor
        {
            /// <summary>
            /// Reflection info for the <see cref="RuntimeOpsEx.CreateRuntimeVariables"/> method.
            /// </summary>
            private static readonly MethodInfo s_createRuntimeVariables = typeof(RuntimeOpsEx).GetMethod(nameof(RuntimeOpsEx.CreateRuntimeVariables));

            /// <summary>
            /// Reflection info for the <see cref="RuntimeOps.CreateRuntimeVariables()"/> method.
            /// </summary>
            private static readonly MethodInfo s_createRuntimeVariablesEmpty = typeof(RuntimeOpsEx).GetMethod(nameof(RuntimeOps.CreateRuntimeVariables), Type.EmptyTypes);

            /// <summary>
            /// Reflection info for the <see cref="RuntimeOpsEx.Quote"/> method.
            /// </summary>
            private static readonly MethodInfo s_quote = typeof(RuntimeOpsEx).GetMethod(nameof(RuntimeOpsEx.Quote));

            /// <summary>
            /// Dictionary mapping nodes that introduce variable scopes to the gather scope
            /// information, used to build and access closures.
            /// </summary>
            private readonly Dictionary<object, Scope> _analysis;

            /// <summary>
            /// The parameter expression representing the <see cref="MethodTable"/> passed
            /// to the top-level lambda. The rewritten expression tree will use array indexing
            /// operations into <see cref="MethodTable.Thunks"/> to access the thunks for inner
            /// lambdas.
            /// </summary>
            private readonly ParameterExpression _methodTable;

            /// <summary>
            /// Factory for thunk types.
            /// </summary>
            private readonly IThunkFactory _thunkFactory;

            /// <summary>
            /// Dictionary mapping indexes for inner lambdas to information about the lambda.
            /// The indexes correspond to the entries in the method table passed to the top-level
            /// lambda.
            /// </summary>
            private readonly Dictionary<int, LambdaInfo> _lambdas = new();

            /// <summary>
            /// The current compiler scope to bind variable accesses against.
            /// </summary>
            private CompilerScope _scope;

            /// <summary>
            /// Creates a new expression visitor to rewrite expressions for JIT compilation
            /// support.
            /// </summary>
            /// <param name="analysis">Scope analysis of nodes in the tree.</param>
            /// <param name="methodTable">Method table parameter passed to the top-level lambda.</param>
            /// <param name="thunkFactory">Factory for thunk types.</param>
            public Impl(Dictionary<object, Scope> analysis, ParameterExpression methodTable, IThunkFactory thunkFactory)
            {
                _analysis = analysis;
                _methodTable = methodTable;
                _thunkFactory = thunkFactory;
                _scope = new CompilerScope(_methodTable);
            }

            /// <summary>
            /// Gets the method table containing thunks for the inner lambdas, which are used to
            /// create delegates with JIT support at runtime.
            /// </summary>
            /// <returns>
            /// Method table containing thunks for the inner lambdas. This object should be passed
            /// as the first argument to the compiled rewritten top-level lambda expression.
            /// </returns>
            public MethodTable GetMethodTable()
            {
                //
                // Each compiled inner lambda will have a rewritten lambda expression and a thunk
                // type. Instantiate those thunks and put them in the thunks array in the order of
                // indexes we've assigned to them (these are used within the rewritten expression
                // tree to obtain the thunks).
                //
                var count = _lambdas.Count;
                var thunks = new object[count];

                for (var i = 0; i < count; i++)
                {
                    var lambda = _lambdas[i];
                    var thunk = Activator.CreateInstance(lambda.ThunkType, new object[] { lambda.Lambda });
                    thunks[i] = thunk;
                }

                //
                // Return a method table containing the thunk instances. This instance will be
                // supplied as the first argument to the top-level compiled expression tree.
                //
                return new MethodTable(thunks);
            }

            /// <summary>
            /// Visit block expressions for scope tracking and closure emitting purposes.
            /// </summary>
            /// <param name="node">The block expression to rewrite.</param>
            /// <returns>The rewritten expression.</returns>
            protected override Expression VisitBlock(BlockExpression node)
            {
                //
                // Introduce a new scope and keep track of the original scope in order to restore
                // it after visiting child expressions.
                //
                var currentScope = _scope;
                try
                {
                    var scope = _analysis[node];
                    _scope = new CompilerScope(currentScope, scope);

                    if (!scope.HasHoistedLocals)
                    {
                        //
                        // In case there are no hoisted locals, we don't need to use a builder to
                        // instantiate a closure. Simply visit the child expressions and update.
                        //
                        var expressions = Visit(node.Expressions);

                        return node.Update(node.Variables, expressions);
                    }
                    else
                    {
                        //
                        // In case there are hoisted locals, enter the scope to obtain a builder,
                        // append the visited child expressions, and return a new block. This will
                        // add the statements needed to instantiate the closure and link it to its
                        // parent (if any).
                        //
                        var expressions = node.Expressions;
                        var n = expressions.Count;

                        //
                        // Note we don't copy hoisted locals during Enter because the storage
                        // location of these is superseded by the storage field in the closure.
                        //
                        var builder = _scope.Enter(count: n, copyLocals: false);

                        for (var i = 0; i < n; i++)
                        {
                            builder.Append(Visit(expressions[i]));
                        }

                        //
                        // Note that we don't update the original block here;  the builder will
                        // create a new block anyway, and we can simply use that as our substitute,
                        // provided the original (non-hoisted) locals are declared.
                        //
                        return builder.Finish(declareLocals: true);
                    }
                }
                finally
                {
                    _scope = currentScope;
                }
            }

            /// <summary>
            /// Visit catch blocks for scope tracking and closure emitting purposes.
            /// </summary>
            /// <param name="node">The catch block to rewrite.</param>
            /// <returns>The rewritten catch block.</returns>
            protected override CatchBlock VisitCatchBlock(CatchBlock node)
            {
                //
                // Store the filter and body in two locals. These will get overwritten by the
                // result of recursive visits and/or rewrites using the builder (see below), prior
                // to their usage to update the original expression.
                //
                var filter = node.Filter;
                var body = node.Body;

                //
                // Introduce a new scope and keep track of the original scope in order to restore
                // it after visiting child expressions.
                //
                var currentScope = _scope;
                try
                {
                    var scope = _analysis[node];
                    _scope = new CompilerScope(currentScope, scope);

                    if (!scope.HasHoistedLocals)
                    {
                        //
                        // In case there is no hoisted local, we don't need to use a builder to
                        // instantiate a closure. Simply visit the child expressions.
                        //
                        filter = Visit(filter);
                        body = Visit(body);
                    }
                    else
                    {
                        //
                        // CONSIDER: Refine the analysis step in order to keep track of the child
                        //           expressions where the variable needs to be hoisted into a
                        //           closure. Right now, we may use an unnecessary closure.
                        //

                        //
                        // In case there is a hoisted local, enter the scope to obtain a builder,
                        // append the visited child expressions, and return a new block. This will
                        // add the statements needed to instantiate the closure and link it to its
                        // parent (if any).
                        //
                        // Note that the original variable holding the caught exception instance is
                        // retained, so:
                        //
                        // * if hoisted, we copy it to the closure in Enter -and-
                        // * we don't re-declare it in Finish.
                        //
                        if (filter != null)
                        {
                            var builder = _scope.Enter(count: 1, copyLocals: true);
                            builder.Append(Visit(filter));
                            filter = builder.Finish(declareLocals: false);
                        }

                        if (body != null)
                        {
                            var builder = _scope.Enter(count: 1, copyLocals: true);
                            builder.Append(Visit(body));
                            body = builder.Finish(declareLocals: false);
                        }
                    }
                }
                finally
                {
                    _scope = currentScope;
                }

                //
                // Simply update the expression using the rewritten child expressions. Note that
                // the variable remains unchanged; its contents could be copied by the rewritten
                // child expressions in order to hoist it into a closure.
                //
                return node.Update(node.Variable, filter, body);
            }

            /// <summary>
            /// Visit lambda expression for scope tracking and closure emitting purposes.
            /// </summary>
            /// <typeparam name="T">The type of the delegate represented by the lambda expression.</typeparam>
            /// <param name="node">The lambda expression to rewrite.</param>
            /// <returns>The rewritten lambda expression.</returns>
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                //
                // Allocate a slot for information about the rewritten lambda expression and thunk
                // type. We add a temporary empty value to the dictionary slot here in order to
                // boost the count of entries. This causes the index to be logically numbered in
                // the order the lambda expressions were visited.
                //
                var index = _lambdas.Count;
                _lambdas[index] = default;

                //
                // Introduce a new scope and keep track of the original scope in order to restore
                // it after visiting child expressions.
                //
                var currentScope = _scope;
                try
                {
                    var scope = _analysis[node];
                    _scope = new CompilerScope(currentScope, scope);

                    var body = default(Expression);

                    if (!scope.HasHoistedLocals && !scope.NeedsClosure)
                    {
                        body = Visit(node.Body);
                    }
                    else
                    {
                        //
                        // Visit the body and append it to the builder. Note that hoisted locals
                        // (i.e. parameters) are copied into the closure.
                        //
                        var builder = _scope.Enter(count: 1, copyLocals: true);
                        builder.Append(Visit(node.Body));
                        body = builder.Finish(declareLocals: false);
                    }

                    //
                    // Use the closure parameter of the original scope (i.e. the parent to the
                    // scope representing the lambda) to derive information about the thunk type.
                    //
                    var closureParam = currentScope.Closure;
                    var thunkType = _thunkFactory.GetThunkType(typeof(T), closureParam.Type);
                    var innerDelegateType = GetInnerDelegateType(thunkType);

                    //
                    // Construct a lambda that's parameterized on the closure that's passed in,
                    // using the inner delegate type that the thunk expects. This becomes the
                    // rewritten lambda expression, which will be passed to the constructor of
                    // the thunk type in GetMethodTable.
                    //
                    var parameters = new ParameterExpression[node.Parameters.Count + 1];
                    parameters[0] = closureParam;
                    node.Parameters.CopyTo(parameters, index: 1);

                    var lambda = Expression.Lambda(innerDelegateType, body, parameters);
                    _lambdas[index] = new LambdaInfo(lambda, thunkType);

                    //
                    // Inside the expression tree, replace the lambda by a call to CreateDelegate
                    // on the thunk. The thunk is retrieved from the method table that's passed to
                    // the top-level lambda by indexing into the Thunks array.
                    //
                    var createDelegate = thunkType.GetMethod(nameof(ActionThunk<object>.CreateDelegate));
                    var methodTable = currentScope.Bind(_methodTable);

                    var createDelegateCall =
                        Expression.Call(
                            Expression.Convert(
                                Expression.ArrayIndex(
                                    Expression.Field(
                                        methodTable,
                                        nameof(MethodTable.Thunks)
                                    ),
                                    Expression.Constant(index)
                                ),
                                thunkType
                            ),
                            createDelegate,
                            closureParam
                        );

                    return createDelegateCall;
                }
                finally
                {
                    _scope = currentScope;
                }
            }

            /// <summary>
            /// Visit parameter expressions in order to bind them to their storage locations,
            /// which could be in closure fields.
            /// </summary>
            /// <param name="node">The variable to bind.</param>
            /// <returns>The expression providing access to the variable.</returns>
            /// <remarks>
            /// Declarations of variables are never visited by this visitor; only use sites are.
            /// The resulting expression of binding the variable provides read/write access to the
            /// storage location.
            /// </remarks>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _scope.Bind(node);
            }

            /// <summary>
            /// Visit runtime variables expressions in order to rewrite them to provide runtime
            /// access to the hoisted locals.
            /// </summary>
            /// <param name="node">The runtime variables expression to rewrite.</param>
            /// <returns>The rewritten runtime variables expression.</returns>
            /// <remarks>
            /// The result of rewriting runtime variables expressions contains a call to the
            /// runtime helper library in <see cref="RuntimeOpsEx"/>.
            /// </remarks>
            protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
            {
                var variables = node.Variables;
                var count = variables.Count;

                //
                // No visible variables, return an empty instance.
                //
                // REVIEW: Are there cases where we don't have hoisted locals and still reach this
                //         code path?
                //
                if (count == 0)
                {
                    return Expression.Call(s_createRuntimeVariablesEmpty);
                }

                //
                // Get the indexes for all variables using a parent index relative to the current
                // scope.
                //
                var indexes = new long[count];

                for (var i = 0; i < count; i++)
                {
                    var variable = variables[i];

                    var index = _scope.GetVariableAccessIndex(variable);
                    indexes[i] = index;
                }

                //
                // Return a call to CreateRuntimeVariables(IRuntimeVariables, long[]).
                //
                return
                    Expression.Call(
                        s_createRuntimeVariables,
                        _scope.Closure,
                        Expression.Constant(indexes)
                    );
            }

            /// <summary>
            /// Visit unary expressions in order to rewrite quote nodes to perform a runtime
            /// rewrite to bind variables.
            /// </summary>
            /// <param name="node">The unary expression to rewrite.</param>
            /// <returns>The rewritten unary expression.</returns>
            /// <remarks>
            /// The result of rewriting unary expressions contains a call to the runtime helper
            /// library in <see cref="RuntimeOpsEx"/>.
            /// </remarks>
            protected override Expression VisitUnary(UnaryExpression node)
            {
                if (node.NodeType == ExpressionType.Quote)
                {
                    //
                    // Note we *don't* visit the operand of the Quote node; we want to expose it
                    // as-is to the receiver.
                    //
                    var expression = Expression.Constant(node.Operand, typeof(Expression));
                    var hoistedLocals = Expression.Constant(_scope.HoistedLocals, typeof(object));
                    var closure = _scope.Closure;

                    //
                    // Return a call to Quote(Expression, object, IRuntimeVariables) where the
                    // second parameter contains the HoistedLocals instancebut hiding the runtime
                    // type (because it's internal).
                    //
                    return
                        Expression.Call(
                            s_quote,
                            expression,
                            hoistedLocals,
                            closure
                        );
                }

                return base.VisitUnary(node);
            }

            /// <summary>
            /// Gets the inner delegate type of the expression passed to the specified
            /// <paramref name="thunkType"/>.
            /// </summary>
            /// <param name="thunkType">The thunk type to get the inner delegate type for.</param>
            /// <returns>The inner delegate type used by the thunk.</returns>
            /// <remarks>
            /// Inner delegate types are equivalent delegates to the original expression type, with
            /// the exception of having an additional first closure parameter prepended to them.
            /// Because thunk types can be runtime generated, we need some reflection here.
            /// </remarks>
            private static Type GetInnerDelegateType(Type thunkType)
            {
                //
                // We want to get the delegate type passed as highlighted below:
                //
                // class SomeThunk { public SomeThunk(Expression<TInner> e) { ... } }
                //                                               ^^^^^^
                //
                var constructors = thunkType.GetConstructors();
                Debug.Assert(constructors.Length == 1, "Expected one thunk constructor.");
                var ctor = constructors[0];

                var parameters = ctor.GetParameters();
                Debug.Assert(parameters.Length == 1, "Expected one parameter to thunk constructor.");
                var expressionType = parameters[0].ParameterType;

                Debug.Assert(
                    expressionType.IsGenericType && expressionType.GetGenericTypeDefinition() == typeof(Expression<>),
                    "Expected a generic Expression<T> parameter."
                );

                return expressionType.GetGenericArguments()[0];
            }

            /// <summary>
            /// Struct containing information about an inner lambda expression.
            /// </summary>
            private readonly struct LambdaInfo
            {
                /// <summary>
                /// The rewritten inner lambda expression which can be passed to the constructor
                /// of the <see cref="ThunkType"/>.
                /// </summary>
                public readonly LambdaExpression Lambda;

                /// <summary>
                /// The thunk type compatible with the <see cref="Lambda"/> in order to perform JIT
                /// compilation.
                /// </summary>
                public readonly Type ThunkType;

                /// <summary>
                /// Creates a new value containing information about an inner lambda expression.
                /// </summary>
                /// <param name="lambda">
                /// The rewritten inner lambda expression which can be passed to the constructor of
                /// the <paramref name="thunkType"/>.
                /// </param>
                /// <param name="thunkType">
                /// The thunk type compatible with the <paramref name="lambda"/> in order to
                /// perform JIT compilation.
                /// </param>
                public LambdaInfo(LambdaExpression lambda, Type thunkType)
                {
                    Lambda = lambda;
                    ThunkType = thunkType;
                }
            }

            /// <summary>
            /// Representation of a variable scope with closure access in an expression being
            /// rewritten for JIT support. Instances of this type keep track of the locals, storage
            /// kinds for hoisted locals, parent closures, etc.
            /// </summary>
            private sealed class CompilerScope
            {
                /// <summary>
                /// The scope object containing analysis information about locals and their storage
                /// requirements.
                /// </summary>
                private readonly Scope _scope;

                /// <summary>
                /// The runtime data structure representing hoisted locals, for use by runtime
                /// operations in <see cref="RuntimeOpsEx"/>.
                /// </summary>
                private readonly HoistedLocals _hoistedLocals;

                /// <summary>
                /// Creates a new compiler scope for the top-level lambda.
                /// </summary>
                /// <param name="methodTable">The method table parameter.</param>
                public CompilerScope(ParameterExpression methodTable)
                {
                    Parent = null;
                    _scope = null;
                    Closure = methodTable;
                    _hoistedLocals = new HoistedLocals(methodTable);
                    Locals = new List<ParameterExpression>();
                }

                /// <summary>
                /// Creates a new compiler scope for inner expressions that establish a scope.
                /// </summary>
                /// <param name="parent">The parent scope.</param>
                /// <param name="scope">The scope information obtained during analysis.</param>
                public CompilerScope(CompilerScope parent, Scope scope)
                {
                    Parent = parent;
                    _scope = scope;

                    var locals = new List<ParameterExpression>();
                    var hoisted = new List<ParameterExpression>();

                    //
                    // Get all variables that need hoisting. These will be registered in a
                    // HoistedLocals instance for runtime access in Quote and RuntimeVariables
                    // nodes. The remaining variables are kept as locals.
                    //
                    foreach (var local in scope.Locals)
                    {
                        var variable = local.Key;
                        var storage = local.Value;

                        if (storage == StorageKind.Local)
                        {
                            locals.Add(variable);
                        }
                        else
                        {
                            hoisted.Add(variable);
                        }
                    }

                    _hoistedLocals = new HoistedLocals(parent._hoistedLocals, hoisted.AsReadOnly(), scope.Locals);
                    Closure = _hoistedLocals.SelfVariable;

                    Locals = locals;
                }

                /// <summary>
                /// Ges the parent scope, if any. This is used to resolve variable accesses through closure
                /// traversal, as needed.
                /// </summary>
                public CompilerScope Parent { get; }

                /// <summary>
                /// Gets the object containing information about hoisted locals, for use by runtime
                /// functions in <see cref="RuntimeOpsEx"/>.
                /// </summary>
                public HoistedLocals HoistedLocals => _hoistedLocals ?? Parent?.HoistedLocals;

                /// <summary>
                /// Gets the non-hoisted locals.
                /// </summary>
                public List<ParameterExpression> Locals { get; }

                /// <summary>
                /// Gets the variable representing the closure used to access hoisted locals. For the top-
                /// level scope, this variable represents the method table.
                /// </summary>
                public ParameterExpression Closure { get; }

                /// <summary>
                /// Creates a builder to rewrite an expression to a block that sets up the closure
                /// (if any).
                /// </summary>
                /// <param name="count">
                /// The number of rewritten child expressions of the original node that will be
                /// <see cref="Builder.Append"/>ed in the block.
                /// </param>
                /// <param name="copyLocals">
                /// Indicates whether the values of hoisted locals declared by the node should be
                /// copied to the closure. For block expressions, this value will be set to
                /// <c>false</c> because we just need to allocate space with default values. For
                /// all other nodes, this value will be set to <c>true</c> to copy the value
                /// provided.
                /// </param>
                /// <returns>A builder instance used to build the block.</returns>
                public Builder Enter(int count, bool copyLocals)
                {
                    return new Builder(this, count, copyLocals);
                }

                /// <summary>
                /// Gets the <see cref="long"/> value representing the combined parent and variable
                /// index relative to the current scope. This value is used for runtime variables
                /// expressions through the indexes passed to <see cref="RuntimeOpsEx.CreateRuntimeVariables"/>.
                /// </summary>
                /// <param name="variable">The variable to obtain the index for.</param>
                /// <returns>
                /// A <see cref="long"/> value encoding the parent index in the upper four bytes,
                /// and the variable index in the defining scope in the lower four bytes.
                /// </returns>
                public long GetVariableAccessIndex(ParameterExpression variable)
                {
                    var hoistedLocals = HoistedLocals;

                    //
                    // Walk the parent chain and keep track of the number of parent scopes we
                    // traversed.
                    //
                    var scope = default(ulong);
                    int index;

                    while (!hoistedLocals.Indexes.TryGetValue(variable, out index))
                    {
                        hoistedLocals = hoistedLocals.Parent;
                        scope++;
                    }

                    //
                    // Combine the number of parents we traversed and the index of the variable in
                    // the scope used to retrieve it at runtime through IRuntimeVariables.
                    //
                    return (long)((scope << 32) | (uint)index);
                }

                /// <summary>
                /// Binds the specified <paramref name="variable"/> to its storage location for
                /// read/write access.
                /// </summary>
                /// <param name="variable">The variable to bind.</param>
                /// <returns>The expression providing access to the variable.</returns>
                public Expression Bind(ParameterExpression variable)
                {
                    return Resolve(Closure, variable);
                }

                /// <summary>
                /// Resolves the specified <paramref name="variable"/> to its storage location,
                /// using the <paramref name="closureAccess"/> expression to access closure storage,
                /// if needed.
                /// </summary>
                /// <param name="closureAccess">The expression providing access to the closure.</param>
                /// <param name="variable">The variable to resolve to its storage location.</param>
                /// <returns>An expression providing read/write access to the variable's storage.</returns>
                /// <remarks>
                /// The method is called recursively by traversing the parent chain of compiler
                /// scopes. For each recursive call, the <paramref name="closureAccess"/> parameter
                /// gets updated to refer to the closure for the current scope, starting from the
                /// original use site. If the <paramref name="variable"/> is a local in the current
                /// scope, it gets returned as-is; otherwise, an access expression using the
                /// closure is built.
                /// </remarks>
                private Expression Resolve(Expression closureAccess, ParameterExpression variable)
                {
                    //
                    // If we reach the root scope, we simply return the closure access expression.
                    // This is the bottom of the recursion.
                    //
                    if (_scope == null)
                    {
                        return closureAccess;
                    }

                    //
                    // Get the closure information for the current scope, used to obtain variable
                    // access expressions.
                    //
                    var closure = HoistedLocals.Closure;

                    //
                    // Check if the variable is defined in the current scope. If it's a local,
                    // return it as-is (note that the rewrite doesn't take away declarations). If
                    // it's hoisted or boxed, retrieve an access expression.
                    //
                    if (_scope.Locals.TryGetValue(variable, out var kind))
                    {
                        if (kind == StorageKind.Local)
                        {
                            return variable;
                        }

                        return closure.Access(closureAccess, variable);
                    }

                    //
                    // If the variable was not found, get the closure access path to the parent
                    // and ask the parent scope to resolve the variable.
                    //
                    // E.g.     scopeC.Resolve(closureC, var)
                    //      --> scopeP.Resolve(closureP, var)
                    //
                    //   where P is the parent and C is the child,
                    //   and `closureP` = `closureC.Parent`.
                    //
                    var parentClosureAccess = closure.Access(closureAccess, Parent.Closure);
                    return Parent.Resolve(parentClosureAccess, variable);
                }
            }

            /// <summary>
            /// Builder for expression blocks used when rewriting nodes in preparation of JIT
            /// compilation support. The resulting block expression has a prologue to instantiate
            /// closure instances, copy variables (e.g. parameters passed to a lambda expression),
            /// and link the parent closure.
            /// </summary>
            private sealed class Builder
            {
                /// <summary>
                /// The scope for which we're building the comma; used to declare locals and copy
                /// locals.
                /// </summary>
                private readonly CompilerScope _scope;

                /// <summary>
                /// The comma we're building. These expressions will be used to construct a block
                /// in <see cref="Finish"/> .
                /// </summary>
                private readonly Expression[] _comma;

                /// <summary>
                /// The next index into <see cref="_comma"/> where the next expression will be
                /// added in <see cref="Append"/>.
                /// </summary>
                private int _commaIndex;

                /// <summary>
                /// Creates a new builder to rewrite the node associated with the specified
                /// <paramref name="scope"/>.
                /// </summary>
                /// <param name="scope">
                /// The scope represented by the node being rewritten.
                /// </param>
                /// <param name="expressionCount">
                /// The number of rewritten child expressions to <see cref="Append"/>.
                /// </param>
                /// <param name="copyLocals">
                /// Indicates whether declared locals that need hoisting should be copied to the
                /// allocated closure fields.
                /// </param>
                public Builder(CompilerScope scope, int expressionCount, bool copyLocals)
                {
                    _scope = scope;

                    //
                    // Some variables that are used frequently below.
                    //
                    var closure = scope.Closure;
                    var closureInfo = scope.HoistedLocals.Closure;

                    //
                    // Allocate array storage for the comma expressions.
                    //
                    var count =
                        1 /* closure allocation */ +
                        scope.HoistedLocals.Variables.Count /* init or copy locals */ +
                        expressionCount /* number of child expressions */;

                    _comma = new Expression[count];

                    //
                    // Allocate the closure instance and assign to the self-variable.
                    //
                    _comma[_commaIndex++] = Expression.Assign(scope.Closure, Expression.New(scope.Closure.Type));

                    //
                    // Allocate storage in the closure for all the hoisted variables. Assignments
                    // to the fields are performed, either copying the value of the originally
                    // declared variable (e.g. to copy parameters of a lambda or the exception
                    // caught by a catch block), or specifying the default value. Note that the
                    // assignment of default values may be non-trivial in case the variable is
                    // boxed and requires instantation of a StrongBox<T>.
                    //
                    // CONSIDER: We could omit assignments of default values to non-boxed fields
                    //           because these will get a default value anyway.
                    //
                    foreach (var variable in scope.HoistedLocals.Variables)
                    {
                        Expression value;

                        if (copyLocals)
                        {
                            value = variable;
                        }
                        else
                        {
                            value = Expression.Default(variable.Type);
                        }

                        _comma[_commaIndex++] = closureInfo.Assign(closure, variable, value);
                    }
                }

                /// <summary>
                /// Appends the specified <paramref name="expression"/> to the comma being built. Called
                /// by the rewriter for e.g. the body of a lambda or an expression in a block.
                /// </summary>
                /// <param name="expression">The expression to append.</param>
                public void Append(Expression expression)
                {
                    _comma[_commaIndex++] = expression;
                }

                /// <summary>
                /// Returns the resulting block expression containing the instantiation and
                /// initialization of the closure, the rewritten child expressions, and optional
                /// declaration of locals that aren't hoisted.
                /// </summary>
                /// <param name="declareLocals">
                /// Set to <c>true</c> if non-hoisted locals have to be declared by the resulting
                /// block, when the builder is used to rewrite a block expression. For all other
                /// nodes, a value of <c>false</c> should be used.
                /// </param>
                /// <returns>
                /// The resulting block expression for use in the rewritten expression tree.
                /// </returns>
                public Expression Finish(bool declareLocals)
                {
                    Debug.Assert(_commaIndex == _comma.Length, "Expected comma to be fully built.");

                    //
                    // Always declare the closure variable.
                    //
                    var variables = new List<ParameterExpression> { _scope.Closure };

                    if (declareLocals)
                    {
                        variables.AddRange(_scope.Locals);
                    }

                    return Expression.Block(variables, _comma);
                }
            }
        }
    }
}
