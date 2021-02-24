// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // No null checks in visitor methods.

using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Base class for expression visitors used to check uses of type members against a list of allowed members.
    /// </summary>
    public abstract class ExpressionMemberAllowListScannerBase : ExpressionVisitor
    {
        /// <summary>
        /// Checks whether the binary expression node's method, if any, is on the list of allowed members.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected override Expression VisitBinary(BinaryExpression node) => CheckExpression(node, node.Method, base.VisitBinary);

        /// <summary>
        /// Checks whether the element initializer node's add method is on the list of allowed members.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected override ElementInit VisitElementInit(ElementInit node) => CheckElementInit(node, base.VisitElementInit);

        /// <summary>
        /// Checks whether the index expression node's indexer is on the list of allowed members.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected override Expression VisitIndex(IndexExpression node) => CheckExpression(node, node.Indexer, base.VisitIndex);

        /// <summary>
        /// Checks whether the member binding node's member is on the list of allowed members.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected override MemberBinding VisitMemberBinding(MemberBinding node) => CheckMemberBinding(node, base.VisitMemberBinding);

        /// <summary>
        /// Checks whether the member expression node's member is on the list of allowed members.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected override Expression VisitMember(MemberExpression node) => CheckExpression(node, node.Member, base.VisitMember);

        /// <summary>
        /// Checks whether the method call expression node's method is on the list of allowed members.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node) => CheckExpression(node, node.Method, base.VisitMethodCall);

        /// <summary>
        /// Checks whether the new expression node's constructor is on the list of allowed members.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected override Expression VisitNew(NewExpression node) => CheckExpression(node, node.Constructor, base.VisitNew);

        /// <summary>
        /// Checks whether the unary expression node's method, if any, is on the list of allowed members.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected override Expression VisitUnary(UnaryExpression node) => CheckExpression(node, node.Method, base.VisitUnary);

        /// <summary>
        /// Checks whether a member used in an expression is on the list of allowed members.
        /// </summary>
        /// <typeparam name="T">Type of the expression to check.</typeparam>
        /// <param name="expression">Expression to check.</param>
        /// <param name="member">Member used by the expression.</param>
        /// <param name="visit">Function to continue the visit of the node.</param>
        /// <returns>The original node if the expression was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected virtual Expression CheckExpression<T>(T expression, MemberInfo member, Func<T, Expression> visit)
            where T : Expression
        {
            if (member != null)
            {
                if (!Check(member))
                {
                    return ResolveExpression(expression, member, visit);
                }
            }

            return visit(expression);
        }

        /// <summary>
        /// Checks whether the member used in a member binding is on the list of allowed members.
        /// </summary>
        /// <typeparam name="T">Type of the member binding to check.</typeparam>
        /// <param name="memberBinding">Member binding to check.</param>
        /// <param name="visit">Function to continue the visit of the node.</param>
        /// <returns>The original node if the member binding was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected virtual MemberBinding CheckMemberBinding<T>(T memberBinding, Func<T, MemberBinding> visit)
            where T : MemberBinding
        {
            if (!Check(memberBinding.Member))
            {
                return ResolveMemberBinding(memberBinding, visit);
            }

            return visit(memberBinding);
        }

        /// <summary>
        /// Checks whether the add method used in an element initializer is on the list of allowed members.
        /// </summary>
        /// <param name="initializer">Element initializer to check.</param>
        /// <param name="visit">Function to continue the visit of the node.</param>
        /// <returns>The original node if the element initializer was accepted by the checker; otherwise, a rewritten result or an exception.</returns>
        protected virtual ElementInit CheckElementInit(ElementInit initializer, Func<ElementInit, ElementInit> visit)
        {
            if (!Check(initializer.AddMethod))
            {
                return ResolveElementInit(initializer, visit);
            }

            return visit(initializer);
        }

        /// <summary>
        /// Checks whether the specified member is supported.
        /// </summary>
        /// <param name="member">Member to check.</param>
        /// <returns>true if the member is supported; otherwise, false.</returns>
        protected abstract bool Check(MemberInfo member);

        /// <summary>
        /// Resolves an expression whose member was rejected by the checker.
        /// </summary>
        /// <typeparam name="T">Type the expression to resolve.</typeparam>
        /// <param name="expression">Expression to resolve.</param>
        /// <param name="member">Member that was rejected.</param>
        /// <param name="visit">Function to continue the visit of the node.</param>
        /// <returns>Resolved expression, or an exception. By default, the method throws a NotSupportedException.</returns>
        /// <exception cref="NotSupportedException">The specified expression was rejected.</exception>
        protected virtual Expression ResolveExpression<T>(T expression, MemberInfo member, Func<T, Expression> visit)
            where T : Expression
        {
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Expression '{0}' uses '{1}' which is not allowed.", expression.ToCSharpString(), member.ToCSharpString()));
        }

        /// <summary>
        /// Resolves a member binding whose member was rejected by the checker.
        /// </summary>
        /// <typeparam name="T">Type of the member binding to resolve.</typeparam>
        /// <param name="binding">Binding to resolve.</param>
        /// <param name="visit">Function to continue the visit of the node.</param>
        /// <returns>Resolved member binding, or an exception. By default, the method throws a NotSupportedException.</returns>
        /// <exception cref="NotSupportedException">The specified member binding was rejected.</exception>
        protected virtual MemberBinding ResolveMemberBinding<T>(T binding, Func<T, MemberBinding> visit)
            where T : MemberBinding
        {
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Member binding '{0}' uses '{1}' which is not allowed.", binding, binding.Member.ToCSharpString()));
        }

        /// <summary>
        /// Resolves an element initializer whose add method was rejected by the checker.
        /// </summary>
        /// <param name="initializer">Element initializer to resolve.</param>
        /// <param name="visit">Function to continue the visit of the node.</param>
        /// <returns>Resolved element initializer, or an exception. By default, the method throws a NotSupportedException.</returns>
        /// <exception cref="NotSupportedException">The specified element initializer was rejected.</exception>
        protected virtual ElementInit ResolveElementInit(ElementInit initializer, Func<ElementInit, ElementInit> visit)
        {
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Element initializer '{0}' uses '{1}' which is not allowed.", initializer, initializer.AddMethod.ToCSharpString()));
        }
    }
}
