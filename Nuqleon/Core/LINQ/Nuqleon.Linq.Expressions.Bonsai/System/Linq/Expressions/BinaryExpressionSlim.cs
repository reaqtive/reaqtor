// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lightweight representation of binary expression tree nodes.
    /// </summary>
    public abstract class BinaryExpressionSlim : ExpressionSlim
    {
        internal BinaryExpressionSlim(ExpressionType nodeType, ExpressionSlim left, ExpressionSlim right)
        {
            NodeType = nodeType;
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Gets the expression node type.
        /// </summary>
        public override ExpressionType NodeType { get; }

        /// <summary>
        /// Gets the left child node of the binary expression.
        /// </summary>
        public ExpressionSlim Left { get; }

        /// <summary>
        /// Gets the right child node of the binary expression.
        /// </summary>
        public ExpressionSlim Right { get; }

        /// <summary>
        /// Indicates whether the binary operation is a lifted nullable operation.
        /// </summary>
        public abstract bool IsLiftedToNull { get; }

        /// <summary>
        /// Gets the method implementing the binary operation.
        /// </summary>
        public virtual MethodInfoSlim Method => null;

        /// <summary>
        /// Gets the conversion used by the binary operation, e.g. for coalescing operations.
        /// </summary>
        public virtual LambdaExpressionSlim Conversion => null;

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="left">The <see cref="Left"/> child node of the result.</param>
        /// <param name="conversion">The <see cref="Conversion"/> child node of the result.</param>
        /// <param name="right">The <see cref="Right"/> child node of the result.</param>
        /// <returns>This expression if no children are changed or an expression with the updated children.</returns>
        public BinaryExpressionSlim Update(ExpressionSlim left, LambdaExpressionSlim conversion, ExpressionSlim right)
        {
            if (left == Left && right == Right && conversion == Conversion)
            {
                return this;
            }

            return BinaryExpressionSlim.Make(NodeType, left, right, IsLiftedToNull, Method, conversion);
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
            return visitor.VisitBinary(this);
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
            return visitor.VisitBinary(this);
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079

        internal static BinaryExpressionSlim Make(ExpressionType nodeType, ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method, LambdaExpressionSlim conversion)
        {
            if (conversion == null)
            {
                if (liftToNull)
                {
                    if (method == null)
                    {
                        return new LiftedBinaryExpressionSlim(nodeType, left, right);
                    }
                    else
                    {
                        return new MethodBasedLiftedBinaryExpressionSlim(nodeType, left, right, method);
                    }
                }
                else
                {
                    if (method == null)
                    {
                        return new NonLiftedBinaryExpressionSlim(nodeType, left, right);
                    }
                    else
                    {
                        return new MethodBasedNonLiftedBinaryExpressionSlim(nodeType, left, right, method);
                    }
                }
            }

            return new FullBinaryExpressionSlim(nodeType, left, right, liftToNull, method, conversion);
        }
    }

    internal class LiftedBinaryExpressionSlim : BinaryExpressionSlim
    {
        internal LiftedBinaryExpressionSlim(ExpressionType nodeType, ExpressionSlim left, ExpressionSlim right)
            : base(nodeType, left, right)
        {
        }

        public override bool IsLiftedToNull => true;
    }

    internal sealed class MethodBasedLiftedBinaryExpressionSlim : LiftedBinaryExpressionSlim
    {
        internal MethodBasedLiftedBinaryExpressionSlim(ExpressionType nodeType, ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
            : base(nodeType, left, right)
        {
            Method = method;
        }

        public override MethodInfoSlim Method { get; }
    }

    internal class NonLiftedBinaryExpressionSlim : BinaryExpressionSlim
    {
        internal NonLiftedBinaryExpressionSlim(ExpressionType nodeType, ExpressionSlim left, ExpressionSlim right)
            : base(nodeType, left, right)
        {
        }

        public override bool IsLiftedToNull => false;
    }

    internal sealed class MethodBasedNonLiftedBinaryExpressionSlim : NonLiftedBinaryExpressionSlim
    {
        internal MethodBasedNonLiftedBinaryExpressionSlim(ExpressionType nodeType, ExpressionSlim left, ExpressionSlim right, MethodInfoSlim method)
            : base(nodeType, left, right)
        {
            Method = method;
        }

        public override MethodInfoSlim Method { get; }
    }

    internal sealed class FullBinaryExpressionSlim : BinaryExpressionSlim
    {
        internal FullBinaryExpressionSlim(ExpressionType nodeType, ExpressionSlim left, ExpressionSlim right, bool liftToNull, MethodInfoSlim method, LambdaExpressionSlim conversion)
            : base(nodeType, left, right)
        {
            IsLiftedToNull = liftToNull;
            Method = method;
            Conversion = conversion;
        }

        public override bool IsLiftedToNull { get; }
        public override MethodInfoSlim Method { get; }
        public override LambdaExpressionSlim Conversion { get; }
    }
}
