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
    /// Lightweight representation of object creation expression tree nodes.
    /// </summary>
    public abstract class NewExpressionSlim : ExpressionSlim, IArgumentProviderSlim
    {
        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.New;

        /// <summary>
        /// Gets the type of the new expression.
        /// </summary>
        public abstract TypeSlim Type { get; }

        /// <summary>
        /// Gets the constructor used by the object creation operation.
        /// </summary>
        public abstract ConstructorInfoSlim Constructor { get; }

        /// <summary>
        /// Gets the expressions representing the arguments passed to the constructor call.
        /// </summary>
        public ReadOnlyCollection<ExpressionSlim> Arguments => GetOrMakeArguments();

        /// <summary>
        /// Gets the members initialized by the constructor arguments.
        /// </summary>
        public abstract ReadOnlyCollection<MemberInfoSlim> Members { get; }

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
        /// <param name="args">The new arguments; can be null if no argument have changed.</param>
        /// <returns>An updated copy of the current node.</returns>
        internal abstract NewExpressionSlim Rewrite(IList<ExpressionSlim> args);

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="arguments">The <see cref="Arguments"/> child node of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public NewExpressionSlim Update(ReadOnlyCollection<ExpressionSlim> arguments)
        {
            if (Constructor != null && arguments != Arguments)
            {
                return ExpressionSlim.New(Constructor, arguments, Members);
            }
            else
            {
                return this;
            }
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree APIs.

        /// <summary>
        /// Accepts the expression tree node in the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor to process the current expression tree node.</param>
        /// <returns>Result of visiting the node.</returns>
        protected internal override ExpressionSlim Accept(ExpressionSlimVisitor visitor)
        {
            return visitor.VisitNew(this);
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
            return visitor.VisitNew(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079
    }

    internal sealed class NewValueTypeExpressionSlim : NewExpressionSlim
    {
        internal NewValueTypeExpressionSlim(TypeSlim type)
        {
            Type = type;
        }

        public override TypeSlim Type { get; }
        public override ConstructorInfoSlim Constructor => null;
        public override ReadOnlyCollection<MemberInfoSlim> Members => null;
        public override int ArgumentCount => 0;

        public override ExpressionSlim GetArgument(int index)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments() => EmptyReadOnlyCollection<ExpressionSlim>.Instance;

        internal override NewExpressionSlim Rewrite(IList<ExpressionSlim> args)
        {
            throw Invariant.Unreachable;
        }
    }

    internal abstract class NewReferenceTypeExpressionSlim : NewExpressionSlim
    {
        internal NewReferenceTypeExpressionSlim(ConstructorInfoSlim constructor)
        {
            Constructor = constructor;
        }

        public override TypeSlim Type => Constructor.DeclaringType;
        public override ConstructorInfoSlim Constructor { get; }
        public override ReadOnlyCollection<MemberInfoSlim> Members => null;
    }

    internal sealed class NewReferenceTypeExpressionSlim0 : NewReferenceTypeExpressionSlim
    {
        public NewReferenceTypeExpressionSlim0(ConstructorInfoSlim constructor)
            : base(constructor)
        {
        }

        public override int ArgumentCount => 0;

        public override ExpressionSlim GetArgument(int index)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments() => EmptyReadOnlyCollection<ExpressionSlim>.Instance;

        internal override NewExpressionSlim Rewrite(IList<ExpressionSlim> args)
        {
            throw new InvalidOperationException();
        }
    }

    internal class FullNewReferenceTypeExpressionSlim : NewReferenceTypeExpressionSlim
    {
        private readonly ReadOnlyCollection<ExpressionSlim> _arguments;

        public FullNewReferenceTypeExpressionSlim(ConstructorInfoSlim constructor, ReadOnlyCollection<ExpressionSlim> arguments)
            : base(constructor)
        {
            _arguments = arguments;
        }

        public override int ArgumentCount => _arguments.Count;

        public override ExpressionSlim GetArgument(int index) => _arguments[index];

        protected override ReadOnlyCollection<ExpressionSlim> GetOrMakeArguments() => _arguments;

        internal override NewExpressionSlim Rewrite(IList<ExpressionSlim> args)
        {
            Debug.Assert(args != null);

            return ExpressionSlim.New(Constructor, args);
        }
    }

    internal sealed class FullNewReferenceTypeExpressionSlimWithMembers : FullNewReferenceTypeExpressionSlim
    {
        public FullNewReferenceTypeExpressionSlimWithMembers(ConstructorInfoSlim constructor, ReadOnlyCollection<ExpressionSlim> arguments, ReadOnlyCollection<MemberInfoSlim> members)
            : base(constructor, arguments)
        {
            Members = members;
        }

        public override ReadOnlyCollection<MemberInfoSlim> Members { get; }

        internal override NewExpressionSlim Rewrite(IList<ExpressionSlim> args)
        {
            Debug.Assert(args != null);

            return ExpressionSlim.New(Constructor, args, Members);
        }
    }
}
