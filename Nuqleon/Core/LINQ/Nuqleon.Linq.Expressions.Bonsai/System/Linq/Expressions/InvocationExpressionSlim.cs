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

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lightweight representation of invocation expression tree nodes.
    /// </summary>
    public abstract class InvocationExpressionSlim : ExpressionSlim, IArgumentProviderSlim
    {
        internal InvocationExpressionSlim(ExpressionSlim expression)
        {
            Expression = expression;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Invoke;

        /// <summary>
        /// Gets the expression representing the function to invoke.
        /// </summary>
        public ExpressionSlim Expression { get; }

        /// <summary>
        /// Gets the expressions representing the arguments passed to the invocation of the function.
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
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> child node of the result.</param>
        /// <param name="arguments">The <see cref="Arguments"/> child node of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public ExpressionSlim Update(ExpressionSlim expression, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            if (expression == Expression && arguments == Arguments)
            {
                return this;
            }

            return ExpressionSlim.Invoke(expression, arguments);
        }

        /// <summary>
        /// Called by the expression visitor to return an updated copy of the node.
        /// </summary>
        /// <param name="expression">The new expression representing the function to invoke.</param>
        /// <param name="args">The new arguments; can be null if no argument have changed.</param>
        /// <returns>An updated copy of the current node.</returns>
        internal abstract InvocationExpressionSlim Rewrite(ExpressionSlim expression, IList<ExpressionSlim> args);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree APIs.

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitInvocation(this);
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
            return visitor.VisitInvocation(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079
    }

    internal sealed class InvocationExpressionSlim0 : InvocationExpressionSlim
    {
        public InvocationExpressionSlim0(ExpressionSlim expression)
            : base(expression)
        {
        }

        public override int ArgumentCount => 0;

        public override ExpressionSlim GetArgument(int index)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments() => EmptyReadOnlyCollection<ExpressionSlim>.Instance;

        internal override InvocationExpressionSlim Rewrite(ExpressionSlim expression, IList<ExpressionSlim> args)
        {
            Debug.Assert(expression != null);
            Debug.Assert(args == null || args.Count == 0);

            return ExpressionSlim.Invoke(expression);
        }
    }

    internal sealed class InvocationExpressionSlimN : InvocationExpressionSlim
    {
        private readonly ReadOnlyCollection<ExpressionSlim> _arguments;

        public InvocationExpressionSlimN(ExpressionSlim expression, ReadOnlyCollection<ExpressionSlim> arguments)
            : base(expression)
        {
            _arguments = arguments;
        }

        public override int ArgumentCount => _arguments.Count;

        public override ExpressionSlim GetArgument(int index) => _arguments[index];

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments() => _arguments;

        internal override InvocationExpressionSlim Rewrite(ExpressionSlim expression, IList<ExpressionSlim> args)
        {
            Debug.Assert(expression != null);
            Debug.Assert(args == null || args.Count == _arguments.Count);

            return ExpressionSlim.Invoke(expression, args ?? _arguments);
        }
    }
}
