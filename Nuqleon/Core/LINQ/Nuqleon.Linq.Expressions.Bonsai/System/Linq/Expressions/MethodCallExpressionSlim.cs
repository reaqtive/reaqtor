// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Reflection;

namespace System.Linq.Expressions
{

    /// <summary>
    /// Lightweight representation of method call expression tree nodes.
    /// </summary>
    public abstract class MethodCallExpressionSlim : ExpressionSlim, IArgumentProviderSlim
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression.

        internal MethodCallExpressionSlim(MethodInfoSlim method)
        {
            Method = method;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Call;

#pragma warning disable CA1720 // Identifier 'object' contains type name. (Mirroring expression tree APIs.)
#pragma warning disable CA1716 // Reserved language keyword 'object'.

        /// <summary>
        /// Gets the expression representing the object instance to call the method on.
        /// </summary>
        public abstract ExpressionSlim Object { get; }

#pragma warning disable CA1716
#pragma warning restore CA1720

        /// <summary>
        /// Gets the method called by the method call operation.
        /// </summary>
        public MethodInfoSlim Method { get; }

        /// <summary>
        /// Gets the expressions representing the arguments passed to the method call.
        /// </summary>
        public ReadOnlyCollection<ExpressionSlim> Arguments => GetOrMakeArguments();

        /// <summary>
        /// Gets the number of arguments.
        /// </summary>
        public abstract int ArgumentCount { get; }

        /// <summary>
        /// Gets the argument at the specified index.
        /// </summary>
        /// <param name="index">The index of the argument to retrieve.</param>
        /// <returns>The argument at the specified index.</returns>
        public abstract ExpressionSlim GetArgument(int index);

        /// <summary>
        /// Gets or makes the arguments collection. This supports efficient layouts of subtypes.
        /// </summary>
        /// <returns>The arguments collection.</returns>
        protected abstract ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments();

        /// <summary>
        /// Called by the expression visitor to return an updated copy of the node.
        /// </summary>
        /// <param name="instance">The new instance; can be null for static methods.</param>
        /// <param name="args">The new arguments; can be null if no argument have changed.</param>
        /// <returns>An updated copy of the current node.</returns>
        internal abstract MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args);

#pragma warning disable CA1720 // Identifier 'object' contains type name. (Mirroring expression tree APIs.)

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="object">The <see cref="Object"/> child node of the result.</param>
        /// <param name="arguments">The <see cref="Arguments"/> child node of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public MethodCallExpressionSlim Update(ExpressionSlim @object, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            if (@object == Object && arguments == Arguments)
            {
                return this;
            }

            return ExpressionSlim.Call(@object, Method, arguments);
        }

#pragma warning restore CA1720

#pragma warning disable CA1062 // Omitted null checks similar to expression tree APIs.

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitMethodCall(this);
        }

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <typeparam name="TExpression">Target type for expressions.</typeparam>
        /// <typeparam name="TLambdaExpression">Target type for lambda expressions. This type has to derive from TExpression.</typeparam>
        /// <typeparam name="TParameterExpression">Target type for parameter expressions. This type has to derive from TExpression.</typeparam>
        /// <typeparam name="TNewExpression">Target type for new expressions. This type has to derive from TExpression.</typeparam>
        /// <typeparam name="TElementInit">Target type for element initializers.</typeparam>
        /// <typeparam name="TMemberBinding">Target type for member bindings.</typeparam>
        /// <typeparam name="TMemberAssignment">Target type for member assignments. This type has to derive from TMemberBinding.</typeparam>
        /// <typeparam name="TMemberListBinding">Target type for member list bindings. This type has to derive from TMemberBinding.</typeparam>
        /// <typeparam name="TMemberMemberBinding">Target type for member member bindings. This type has to derive from TMemberBinding.</typeparam>
        /// <typeparam name="TCatchBlock">Target type for catch blocks.</typeparam>
        /// <typeparam name="TSwitchCase">Target type for switch cases.</typeparam>
        /// <typeparam name="TLabelTarget">Target type for label targets.</typeparam>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override TExpression Accept<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>(ExpressionSlimVisitor<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> visitor)
        {
            return visitor.VisitMethodCall(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079
    }

    internal abstract class InstanceMethodCallExpressionSlim : MethodCallExpressionSlim
    {
        public InstanceMethodCallExpressionSlim(ExpressionSlim instance, MethodInfoSlim method)
            : base(method)
        {
            Object = instance;
        }

        public override ExpressionSlim Object { get; }
    }

    internal sealed class InstanceMethodCallExpressionSlim0 : InstanceMethodCallExpressionSlim
    {
        public InstanceMethodCallExpressionSlim0(ExpressionSlim instance, MethodInfoSlim method)
            : base(instance, method)
        {
        }

        public override int ArgumentCount => 0;

        public override ExpressionSlim GetArgument(int index)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments() => EmptyReadOnlyCollection<ExpressionSlim>.Instance;

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance != null);
            Debug.Assert(args == null || args.Count == 0);

            return ExpressionSlim.Call(instance, Method);
        }
    }

    internal sealed class InstanceMethodCallExpressionSlimN : InstanceMethodCallExpressionSlim
    {
        private readonly ReadOnlyCollection<ExpressionSlim> _arguments;

        public InstanceMethodCallExpressionSlimN(ExpressionSlim instance, MethodInfoSlim method, ReadOnlyCollection<ExpressionSlim> arguments)
            : base(instance, method)
        {
            _arguments = arguments;
        }

        public override int ArgumentCount => _arguments.Count;

        public override ExpressionSlim GetArgument(int index) => _arguments[index];

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments() => _arguments;

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance != null);
            Debug.Assert(args == null || args.Count == _arguments.Count);

            return ExpressionSlim.Call(instance, Method, args ?? _arguments);
        }
    }

    internal abstract class StaticMethodCallExpressionSlim : MethodCallExpressionSlim
    {
        public StaticMethodCallExpressionSlim(MethodInfoSlim method)
            : base(method)
        {
        }

        public override ExpressionSlim Object => null;
    }

    internal sealed class StaticMethodCallExpressionSlim0 : StaticMethodCallExpressionSlim
    {
        public StaticMethodCallExpressionSlim0(MethodInfoSlim method)
            : base(method)
        {
        }

        public override int ArgumentCount => 0;

        public override ExpressionSlim GetArgument(int index)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments() => EmptyReadOnlyCollection<ExpressionSlim>.Instance;

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            throw Invariant.Unreachable;
        }
    }

    internal sealed class StaticMethodCallExpressionSlimN : StaticMethodCallExpressionSlim
    {
        private readonly ReadOnlyCollection<ExpressionSlim> _arguments;

        public StaticMethodCallExpressionSlimN(MethodInfoSlim method, ReadOnlyCollection<ExpressionSlim> arguments)
            : base(method)
        {
            _arguments = arguments;
        }

        public override int ArgumentCount => _arguments.Count;

        public override ExpressionSlim GetArgument(int index) => _arguments[index];

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments() => _arguments;

        internal override MethodCallExpressionSlim Rewrite(ExpressionSlim instance, IList<ExpressionSlim> args)
        {
            Debug.Assert(instance == null);
            Debug.Assert(args != null);
            Debug.Assert(args.Count == _arguments.Count);

            return ExpressionSlim.Call(Method, args);
        }
    }
}
