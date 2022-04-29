// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - December 2014 - Created this file.
//

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace System.Linq.Expressions
{
    // PERF: Consider creating specialized nodes for low expression counts and the absence of variables.

    /// <summary>
    /// Lightweight representation of a block that contains a sequence of expressions where variables can be defined.
    /// </summary>
    public class BlockExpressionSlim : ExpressionSlim
    {
        internal BlockExpressionSlim(ReadOnlyCollection<ParameterExpressionSlim> variables, ReadOnlyCollection<ExpressionSlim> expressions, TypeSlim type)
        {
            Variables = variables;
            Expressions = expressions;
            Type = type;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Block;

        /// <summary>
        /// Gets the expressions in this block.
        /// </summary>
        public ReadOnlyCollection<ExpressionSlim> Expressions { get; }

        /// <summary>
        /// Gets the variables defined in this block.
        /// </summary>
        public ReadOnlyCollection<ParameterExpressionSlim> Variables { get; }

        /// <summary>
        /// Gets the last expression in this block.
        /// </summary>
        public ExpressionSlim Result
        {
            get
            {
                Debug.Assert(Expressions.Count > 0);
#if NET6_0 || NETSTANDARD2_1
                return Expressions[^1];
#else
                return Expressions[Expressions.Count - 1];
#endif
            }
        }

        /// <summary>
        /// Gets the type of the block expression.
        /// </summary>
        public TypeSlim Type { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="variables">The <see cref="Variables" /> property of the result.</param>
        /// <param name="expressions">The <see cref="Expressions" /> property of the result.</param>
        /// <returns>This expression if no children changed, or an expression with the updated children.</returns>
        public BlockExpressionSlim Update(ReadOnlyCollection<ParameterExpressionSlim> variables, ReadOnlyCollection<ExpressionSlim> expressions)
        {
            if (variables == Variables && expressions == Expressions)
            {
                return this;
            }

            return new BlockExpressionSlim(variables, expressions, Type);
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
            return visitor.VisitBlock(this);
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
            return visitor.VisitBlock(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079
    }
}
