// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Reaqtor
{
    /// <summary>
    /// Provides a set of services for expression tree manipulation in reactive processing implementations.
    /// </summary>
    public class ReactiveExpressionServices : IReactiveExpressionServices
    {
        private readonly ConditionalWeakTable<object, Expression> _registeredObjects;
        private readonly ClosureEliminator _closureEliminator;
        private readonly KnownResourceRewriter _knownResourceRewriter;
        private readonly ClientInterfaceCallToUriRewriter _clientInterfaceCallToUriRewriter;
        private readonly KnownResourceInvocationRewriter _knownResourceInvocationRewriter;

        /// <summary>
        /// Creates a new expression service provider instance.
        /// </summary>
        /// <param name="reactiveClientInterfaceType">Interface type of the IReactiveClient variant used to obtain reactive resources.</param>
        public ReactiveExpressionServices(Type reactiveClientInterfaceType)
        {
            ReactiveClientInterfaceType = reactiveClientInterfaceType;

            _registeredObjects = new ConditionalWeakTable<object, Expression>();
            _closureEliminator = new ClosureEliminator();
            _knownResourceRewriter = new KnownResourceRewriter(this);
            _clientInterfaceCallToUriRewriter = new ClientInterfaceCallToUriRewriter(this);
            _knownResourceInvocationRewriter = new KnownResourceInvocationRewriter(this);
        }

        /// <summary>
        /// Registers an association of an object to its expression representation. 
        /// </summary>
        /// <param name="value">Object to associate with an expression representation.</param>
        /// <param name="expression">Expression representation of the object.</param>
        public virtual void RegisterObject(object value, Expression expression)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            lock (_registeredObjects)
            {
                if (!_registeredObjects.ContainsKey(value))
                {
                    _registeredObjects.Add(value, expression);
                }
            }
        }

        /// <summary>
        /// Tries to find an association of the specified object to an expression representation.
        /// </summary>
        /// <param name="value">Object to find an associated expression representation for.</param>
        /// <param name="expression">Expression representation of the object, if found.</param>
        /// <returns>true if an association was found; otherwise, false.</returns>
        public virtual bool TryGetObject(object value, out Expression expression)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            lock (_registeredObjects)
            {
                return _registeredObjects.TryGetValue(value, out expression);
            }
        }

        /// <summary>
        /// Normalizes the specified expression. This method is typically used to process expressions prior to further processing by a service.
        /// </summary>
        /// <param name="expression">Expression to normalize.</param>
        /// <returns>Normalized expression.</returns>
        public virtual Expression Normalize(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var inlinedExpressions = new ExpressionInliner(this).Inline(expression);

            var uriBased = _knownResourceRewriter.Rewrite(inlinedExpressions);

            var simplified = SimplifyUriForms(uriBased);

            var convertsRewritten = new ConvertedGlobalParameterRewriter().Visit(simplified);

            var betaReduced = BetaReducer.Reduce(convertsRewritten);
            return betaReduced;
        }

        private Expression SimplifyUriForms(Expression expression)
        {
            var clientInterfaceCallSimplified = _clientInterfaceCallToUriRewriter.Rewrite(expression);
            var knownResourceInvocationSimplified = _knownResourceInvocationRewriter.Rewrite(clientInterfaceCallSimplified);
            return knownResourceInvocationSimplified;
        }

        /// <summary>
        /// Locally evaluates any unsupported constructs in the expression (such as closures, disallowed methods, etc.) by turning them into funclet expressions.
        /// This rewrite gets called prior to further rewrite stages that normalize the expression tree.
        /// </summary>
        /// <param name="expression">Expression tree to scan for constructs that need to be funcletized.</param>
        /// <returns>Expression with unsupported constructs rewritten into funclets.</returns>
        protected virtual Expression Funcletize(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return _closureEliminator.Apply(expression);
        }

        /// <summary>
        /// Gets an expression representing a named resource.
        /// </summary>
        /// <param name="type">Type of the resource.</param>
        /// <param name="uri">Name of the resource.</param>
        /// <returns>Expression representing the named resource.</returns>
        public virtual Expression GetNamedExpression(Type type, Uri uri)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return Expression.Parameter(type, uri.ToCanonicalString());
        }

        /// <summary>
        /// Tries to extract the name of a resource from its expression representation.
        /// </summary>
        /// <param name="expression">Expression to extract a name from.</param>
        /// <param name="uri">Name of the resource, if found.</param>
        /// <returns>true if the expression represents a named resource; otherwise, false.</returns>
        public virtual bool TryGetName(Expression expression, out Uri uri)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression is ParameterExpression parameter)
            {
                uri = new Uri(parameter.Name);
                return true;
            }

            uri = null;
            return false;
        }

        /// <summary>
        /// Get the interface type of the IReactiveClient variant used to obtain reactive resources.
        /// </summary>
        private Type ReactiveClientInterfaceType { get; set; }

        private sealed class ClosureEliminator
        {
            private readonly Impl _impl = new();

            public Expression Apply(Expression expression) => _impl.Visit(expression);

            private sealed class Impl : ExpressionVisitor
            {
                protected override Expression VisitMember(MemberExpression node)
                {
                    var expression = Visit(node.Expression);
                    if (expression != null && expression.Type.IsClosureClass())
                    {
                        var value = Evaluate(node.Update(expression));
                        return Expression.Constant(value, node.Type);
                    }

                    return base.VisitMember(node);
                }

                private static object Evaluate(MemberExpression node)
                {
                    var expression = node.Expression;
                    if (node.Member is FieldInfo field && expression.NodeType == ExpressionType.Constant)
                    {
                        return field.GetValue(((ConstantExpression)expression).Value);
                    }

                    return node.Evaluate();
                }
            }
        }

        private sealed class ExpressionInliner
        {
            private readonly ReactiveExpressionServices _parent;
            private readonly Impl _impl;
            private readonly HashSet<Expression> _expressions;

            public ExpressionInliner(ReactiveExpressionServices parent)
            {
                _parent = parent;
                _impl = new Impl(this);
                _expressions = new HashSet<Expression>();
            }

            public Expression Inline(Expression expression)
            {
                if (!_expressions.Add(expression))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Expression '{0}' contains a recursive definition, which is not supported.", expression));
                }

                try
                {
                    var funcletized = _parent.Funcletize(expression);

                    var res = _impl.Visit(funcletized);
                    return res;
                }
                finally
                {
                    _expressions.Remove(expression);
                }
            }

            public bool TryGetObject(object value, out Expression expression) => _parent.TryGetObject(value, out expression);

            private sealed class Impl : CooperativeExpressionVisitor
            {
                private readonly ExpressionInliner _parent;

                public Impl(ExpressionInliner parent) => _parent = parent;

                protected override Expression VisitConstant(ConstantExpression node)
                {
                    Debug.Assert(node != null, "Node is not allowed to be null.");

                    var value = node.Value;

                    if (value != null)
                    {
                        if (_parent.TryGetObject(value, out var expression))
                        {
                            return _parent.Inline(expression);
                        }

                        if (value is IExpressible expressible)
                        {
                            return _parent.Inline(expressible.Expression);
                        }

                        //
                        // TODO: Get rid of tuple analysis here and move to appropriate layer.
                        //
                        if (node.Type.IsGenericType)
                        {
                            var genDef = node.Type.GetGenericTypeDefinition();

                            if (s_tuples.Contains(genDef))
                            {
                                return DecompileTuple(node);
                            }
                        }

                        //
                        // TODO: Revisit this. Complexity arises from delegation QC-QE using .Compile, which causes
                        //       evaluation and hence the occurrence of constant nodes that contain the result of
                        //       evaluating subexpressions.
                        //
                        if (node.Type.IsArray)
                        {
                            return DecompileArray(node);
                        }
                    }

                    return base.VisitConstant(node);
                }

                protected override Expression VisitMember(MemberExpression node)
                {
                    // This is tricky. When a compiled lambda expression is invoked and the parameter is referred from
                    // a nested function, the hoister of the lambda compiler creates a StrongBox<T> to hold its value.
                    // In delegation cases, we can encounter this strong box around the IReactiveProxy (e.g. when using
                    // a query operator like SelectMany with a collection selector using the proxy as well), so we need
                    // to shake it off for subsequent rewrites to work (e.g. when trying to convert async to sync).
                    if (typeof(IStrongBox).IsAssignableFrom(node.Member.DeclaringType) && node.Member.Name == "Value")
                    {
                        var strongBox = (ConstantExpression)node.Expression;
                        var client = ((IStrongBox)strongBox.Value).Value;
                        var constant = Expression.Constant(client, node.Type);
                        return Visit(constant);
                    }

                    return base.VisitMember(node);
                }

                private Expression DecompileArray(ConstantExpression node)
                {
                    var elementType = node.Type.GetElementType();

                    var elements = (IEnumerable)node.Value;

                    return Expression.NewArrayInit(elementType, elements.Cast<object>().Select(o => Visit(Expression.Constant(o, elementType))));
                }

                private Expression DecompileTuple(ConstantExpression node)
                {
                    var tupleType = node.Type;
                    var genArgs = node.Type.GetGenericArguments();

                    var ctor = tupleType.GetConstructor(genArgs);

                    var props = new PropertyInfo[genArgs.Length];
                    var args = new Expression[genArgs.Length];

                    for (var i = 0; i < genArgs.Length; i++)
                    {
                        var name = i == 7 ? "Rest" : "Item" + (i + 1);
                        var prop = tupleType.GetProperty(name);
                        props[i] = prop;

                        var value = prop.GetValue(node.Value);

                        if (typeof(LambdaExpression).IsAssignableFrom(prop.PropertyType))
                        {
                            var lambda = (LambdaExpression)value;
                            args[i] = Visit(Expression.Quote(lambda));
                        }
                        else
                        {
                            args[i] = Visit(Expression.Constant(value, prop.PropertyType));
                        }
                    }

                    return Expression.New(ctor, args, props);
                }

                private static readonly HashSet<Type> s_tuples = new()
                {
                    typeof(Tuple<>),
                    typeof(Tuple<,>),
                    typeof(Tuple<,,>),
                    typeof(Tuple<,,,>),
                    typeof(Tuple<,,,,>),
                    typeof(Tuple<,,,,,>),
                    typeof(Tuple<,,,,,,>),
                    typeof(Tuple<,,,,,,,>),
                };
            }
        }

        private sealed class KnownResourceRewriter
        {
            private readonly Impl _impl;

            public KnownResourceRewriter(ReactiveExpressionServices parent) => _impl = new Impl(parent);

            public Expression Rewrite(Expression expression) => _impl.Visit(expression);

            private sealed class Impl : ExpressionVisitor
            {
                private readonly ReactiveExpressionServices _parent;

                public Impl(ReactiveExpressionServices parent) => _parent = parent;

                protected override Expression VisitMethodCall(MethodCallExpression node)
                {
                    if (TryGetUri(node.Method, out var uri))
                    {
                        var obj = Visit(node.Object);
                        var args = (IEnumerable<Expression>)Visit(node.Arguments);
                        var methodParameterTypes = node.Method.GetParameters().Select(p => p.ParameterType);

                        // Remove "rx://builtin/this" parameter from KnownResource invocations
                        if (node.Method.IsDefined(typeof(ExtensionAttribute)) && _parent.ReactiveClientInterfaceType.IsAssignableFrom(args.First().Type))
                        {
                            args = args.Skip(1);
                            methodParameterTypes = methodParameterTypes.Skip(1);
                        }

                        args = args.Zip(
                            methodParameterTypes,
                            (arg, type) =>
                                (arg.Type != type) ?
                                Expression.Convert(arg, type) :
                                arg
                            ).ToList();

                        if (obj == null && !args.Any())
                        {
                            return _parent.GetNamedExpression(node.Type, uri);
                        }
                        else
                        {
                            return _parent.GetKnownResourceInvocation(uri, node.Method, obj, args, node.Type);
                        }
                    }

                    return base.VisitMethodCall(node);
                }

                protected override Expression VisitMember(MemberExpression node)
                {
                    if (TryGetUri(node.Member, out var uri))
                    {
                        var obj = Visit(node.Expression);

                        return _parent.GetKnownResourceInvocation(uri, node.Member, obj, Enumerable.Empty<Expression>(), node.Type);
                    }

                    return base.VisitMember(node);
                }

                private static bool TryGetUri(MemberInfo member, out Uri uri)
                {
                    var knownResource = member.GetCustomAttribute<KnownResourceAttribute>();
                    if (knownResource != null)
                    {
                        if (string.IsNullOrWhiteSpace(knownResource.Uri))
                        {
                            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "KnownResource attribute applied to member '{0}' has an invalid value.", member));
                        }

                        uri = new Uri(knownResource.Uri);
                        return true;
                    }

                    uri = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets an expression representing the invocation of a known resource.
        /// </summary>
        /// <param name="knownResourceUri">URI of the known resource.</param>
        /// <param name="member">Member annotated with the KnownResource attribute, targeted by this rewrite.</param>
        /// <param name="instance">Instance to invoke the member on (if any).</param>
        /// <param name="arguments">Arguments to pass to the member invocation (if any).</param>
        /// <param name="returnType">Return type of the known resource expression.</param>
        /// <returns>Expression representing the invocation of a known resource.</returns>
        protected virtual Expression GetKnownResourceInvocation(Uri knownResourceUri, MemberInfo member, Expression instance, IEnumerable<Expression> arguments, Type returnType)
        {
            var argTypes = Enumerable.Empty<Type>();
            var args = Enumerable.Empty<Expression>();

            if (instance != null)
            {
                argTypes = argTypes.Concat(new[] { instance.Type });
                args = args.Concat(new[] { instance });
            }

            argTypes = argTypes.Concat(arguments.Select(arg => arg.Type));
            args = args.Concat(arguments);

            argTypes = argTypes.Concat(new[] { returnType });

            var functionType = Expression.GetDelegateType(argTypes.ToArray());

            var function = GetNamedExpression(functionType, knownResourceUri);

            return Expression.Invoke(function, args);
        }

        /// <summary>
        /// Common base class for exprssion tree rewriters containing helper methods that are needed 
        /// for most client-aware rewrites.
        /// </summary>
        private abstract class ScopedExpressionRewriterBase : ScopedExpressionVisitor<ParameterExpression>
        {
            private readonly ReactiveExpressionServices _parent;

            protected ScopedExpressionRewriterBase(ReactiveExpressionServices parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Checks if the passed expression is the reactive client. The criteria are:
            /// - must be an unbound parameter
            /// - must be named like the currentInstanceUri ("rx://builtin/this")
            /// - the expression's type implements an IReactiveClient variant, as specified by the parent class
            /// </summary>
            protected bool IsReactiveClient(Expression expression)
            {
                if (expression != default(Expression) &&
                    expression.NodeType == ExpressionType.Parameter)
                {
                    var parameter = (ParameterExpression)expression;

                    // TODO: Instead of comparing name, better keep track of the current this parameter for the client context and
                    //       use reference quality. This could avoid possible future cross IRP scenarios.
                    return IsUnboundParameter(parameter)
                        && Constants.CurrentInstanceUri.Equals(parameter.Name, StringComparison.Ordinal)
                        && ReactiveClientInterfaceType.IsAssignableFrom(parameter.Type);
                }

                return false;
            }

            protected Type ReactiveClientInterfaceType => _parent.ReactiveClientInterfaceType;

            /// <summary>
            /// Checks if the given parameter expression is unbound.
            /// </summary>
            protected bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);

            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;
        }

        /// <summary>
        /// This rewriter rewrites calls to methods on an IReactiveClient variant that take a Uri as a parameter to an
        /// unbound parameter with just the uri as name in case the first argument was a constant uri.
        /// E.g. we rewrite:
        ///    .Call $rx://builtin/this.GetObservable(.Constant&lt;System.Uri&gt;(bing://bar/))
        ///        to
        ///    $bing://bar
        /// </summary>
        /// <remarks>
        /// Input expressions come in this way typically when code uses the context interface directly.
        /// </remarks>
        private sealed class ClientInterfaceCallToUriRewriter
        {
            private readonly ReactiveExpressionServices _parent;

            public ClientInterfaceCallToUriRewriter(ReactiveExpressionServices parent) => _parent = parent;

            public Expression Rewrite(Expression expression) => new Impl(_parent).Visit(expression);

            private sealed class Impl : ScopedExpressionRewriterBase
            {
                public Impl(ReactiveExpressionServices parent)
                    : base(parent)
                {
                }

                protected override Expression VisitMethodCall(MethodCallExpression node)
                {
                    var rewritten = base.VisitMethodCall(node);

                    if (rewritten.NodeType == ExpressionType.Call)
                    {
                        var typedRewritten = (MethodCallExpression)rewritten;

                        if (IsReactiveClient(typedRewritten.Object) &&
                            typedRewritten.Method.Name.StartsWith("Get", StringComparison.Ordinal) &&
                            typedRewritten.Arguments.Count == 1)
                        {
                            var firstArgument = typedRewritten.Arguments[0];

                            if (TryGetConstantUri(firstArgument, out var uri) && uri != default)
                            {
                                var method = GetMethodDefinition(typedRewritten.Method);

                                if (TryGetInterfaceMethod(method, ReactiveClientInterfaceType, out _))
                                {
                                    return Expression.Parameter(typedRewritten.Type, uri.ToCanonicalString());
                                }
                            }
                        }
                    }

                    return rewritten;
                }

                private static MethodInfo GetMethodDefinition(MethodInfo method)
                {
                    if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
                    {
                        return method.GetGenericMethodDefinition();
                    }

                    return method;
                }

                private static bool TryGetInterfaceMethod(MethodInfo method, Type interfaceType, out MethodInfo interfaceMethod)
                {
                    Debug.Assert(interfaceType.IsInterface);

                    var declaringType = method.DeclaringType;

                    if (declaringType == interfaceType)
                    {
                        interfaceMethod = method;
                        return true;
                    }

                    if (declaringType.GetInterfaces().Contains(interfaceType))
                    {
                        var mapping = declaringType.GetInterfaceMap(interfaceType);

                        for (var i = 0; i < mapping.InterfaceMethods.Length; i++)
                        {
                            var ifMethod = mapping.InterfaceMethods[i];
                            var targetMethod = mapping.TargetMethods[i];

                            if (targetMethod == method)
                            {
                                interfaceMethod = ifMethod;
                                return true;
                            }
                        }
                    }

                    interfaceMethod = default;
                    return false;
                }

                private static bool TryGetConstantUri(Expression expression, out Uri uri)
                {
                    if (expression != null && expression.Type == typeof(Uri))
                    {
                        if (!FreeVariableScanner.Scan(expression).Any())
                        {
                            if (expression.NodeType == ExpressionType.Constant)
                            {
                                var constExpr = (ConstantExpression)expression;
                                uri = (Uri)constExpr.Value;
                                return true;
                            }
                            else if (expression.NodeType == ExpressionType.New)
                            {
                                var newExpr = (NewExpression)expression;
                                if (newExpr.Arguments.All(a => a.NodeType == ExpressionType.Constant))
                                {
                                    uri = expression.Evaluate<Uri>();
                                    return true;
                                }
                            }
                        }
                    }

                    uri = default;
                    return false;
                }
            }
        }

        /// <summary>
        /// This rewriter rewrites invocations of known resources into an uri form.
        /// E.g.
        /// .Invoke $bing://ys(
        ///    $rx://builtin/this, // this needs to be rebound to IRP, followed by reduction into $bing://ys(42)
        ///    42)
        ///   will be rewritten to
        /// Invoke $bing://ys(42)
        /// </summary>
        /// <remarks>
        /// Input expressions come in this way when known resources on the client interface are used.
        /// E.g.:
        ///   A known resource declaration on a (generated) client context:
        ///   
        /// <code>
        ///   class MyClientContext : ClientContext
        ///   {
        ///       [KnownResource("bing://ys")]
        ///       public IAsyncReactiveQbservable&lt;int&gt; Ys(int x)
        ///       {
        ///           return base.Provider.CreateQbservable&lt;int&gt;(
        ///               Expression.Call(
        ///                   Expression.Constant(this),
        ///                   (MethodInfo)MethodBase.GetCurrentMethod(),
        ///                   Expression.Constant(x, typeof(int))
        ///               )
        ///           );
        ///       }
        ///   }
        /// </code>
        ///   
        ///  used in a query:
        ///  
        /// <code>
        ///    var ctx = new MyClientContext(...);
        ///    var ys42 = ctx.Ys(42);
        ///    var res = aQbservable.SelectMany(x => ctx.Ys(x))
        /// </code>
        /// </remarks>
        private sealed class KnownResourceInvocationRewriter
        {
            private readonly ReactiveExpressionServices _parent;

            public KnownResourceInvocationRewriter(ReactiveExpressionServices parent) => _parent = parent;

            public Expression Rewrite(Expression expression) => new Impl(_parent).Visit(expression);

            private sealed class Impl : ScopedExpressionRewriterBase
            {
                public Impl(ReactiveExpressionServices parent)
                    : base(parent)
                {
                }

                protected override Expression VisitInvocation(InvocationExpression node)
                {
                    var rewritten = base.VisitInvocation(node);

                    if (rewritten.NodeType == ExpressionType.Invoke)
                    {
                        var typedRewritten = (InvocationExpression)rewritten;

                        if (TryGetKnownResource(typedRewritten.Expression, out var resourceName))
                        {
                            Debug.Assert(typedRewritten.Arguments.Count >= 1);

                            if (IsReactiveClient(typedRewritten.Arguments[0]))
                            {
                                if (typedRewritten.Arguments.Count == 1)
                                {
                                    return Expression.Parameter(typedRewritten.Type, resourceName);
                                }
                                else
                                {
                                    return GetReducedInvocation(typedRewritten, resourceName);
                                }
                            }
                        }
                    }

                    return rewritten;
                }

                /// <summary>
                /// Checks if the passed expression looks like a known resource. The criteria are:
                /// - must be an unbound parameter
                /// - the expression's type is a delegate with a non void return type (typically a Func&lt;T1, T2, ...&gt; or 
                ///   a custom delegate in case more than 16 type parameters are being used)
                /// - the expression's type first type argument type implements an IReactiveClient variant
                /// </summary>
                private bool TryGetKnownResource(Expression expression, out string resourceName)
                {
                    var parameter = expression as ParameterExpression;

                    if (parameter != default(ParameterExpression))
                    {
                        if (IsUnboundParameter(parameter) &&
                            TryGetDelegateType(parameter.Type, out var parameterTypes, out var returnType) &&
                            returnType != typeof(void) &&
                            parameterTypes.Length > 0 &&
                            ReactiveClientInterfaceType.IsAssignableFrom(parameterTypes[0]))
                        {
                            resourceName = parameter.Name;
                            return true;
                        }
                    }

                    resourceName = null;
                    return false;
                }

                /// <summary>
                /// This method checks if the type is a delegate type and if so, it returns the parameter 
                /// types and the return type.
                /// </summary>
                private static bool TryGetDelegateType(Type type, out Type[] parameterTypes, out Type returnType)
                {
                    if (type != null &&
                        typeof(Delegate).IsAssignableFrom(type))
                    {
                        var method = type.GetMethod("Invoke");

                        parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
                        returnType = method.ReturnType;
                        return true;
                    }

                    parameterTypes = default;
                    returnType = default;
                    return false;
                }

                /// <summary>
                /// Returns a new invocation expression without the first argument and with a return type that 
                /// omits the first type parameter.
                /// </summary>
                private static InvocationExpression GetReducedInvocation(InvocationExpression invocation, string resourceName)
                {
                    // reduce argument list
                    var args = invocation.Arguments.Skip(1);

                    // build new type (Func<reduced_argument_types, returntype> or custom delegate type if count of 
                    // type arguments is too high)
                    var argTypes = args.Select(arg => arg.Type);
                    var functionType = Expression.GetDelegateType(argTypes.Concat(new[] { invocation.Type }).ToArray());
                    var function = Expression.Parameter(functionType, resourceName);

                    return Expression.Invoke(function, args);
                }
            }
        }

        /// <summary>
        /// Rewrites converted global parameter expressions to global parameters of the converted types.
        /// </summary>
        private sealed class ConvertedGlobalParameterRewriter : ScopedExpressionVisitor<ParameterExpression>
        {
            protected override Expression VisitUnary(UnaryExpression node)
            {
                if (node.NodeType == ExpressionType.Convert && node.Operand is ParameterExpression param && !TryLookup(param, out _))
                {
                    return Expression.Parameter(node.Type, param.Name);
                }

                return base.VisitUnary(node);
            }

            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;
        }
    }
}
