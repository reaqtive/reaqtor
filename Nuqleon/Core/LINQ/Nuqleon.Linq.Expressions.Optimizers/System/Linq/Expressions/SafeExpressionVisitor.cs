// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

namespace System.Linq.Expressions
{
    /// <summary>
    /// Expression visitor that protects against rewrites of expression nodes
    /// that violate typing requirements.
    /// </summary>
    /// <example>
    /// The following nodes have typing requirements:
    /// <list type="bullet">
    ///   <item>
    ///     <see cref="BinaryExpression"/> when <see cref="BinaryExpression.Conversion"/> is not <c>null</c>
    ///   </item>
    ///   <item>
    ///     <see cref="UnaryExpression"/> when the <see cref="Expression.NodeType"/> is <see cref="ExpressionType.Quote"/>
    ///   </item>
    ///   <item>
    ///     <see cref="ListInitExpression"/> for the <see cref="ListInitExpression.NewExpression"/> child node
    ///   </item>
    ///   <item>
    ///     <see cref="MemberInitExpression"/> for the <see cref="MemberInitExpression.NewExpression"/> child node
    ///   </item>
    ///   <item>
    ///     <see cref="RuntimeVariablesExpression"/> for the <see cref="RuntimeVariablesExpression.Variables"/> child nodes
    /// </item>
    /// </list>
    /// </example>
    public class SafeExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// Visits a binary expression with a strongly typed visit to <see cref="BinaryExpression.Conversion"/>
        /// using <see cref="VisitBinaryConversion"/>.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <returns>The result of visiting the binary expression.</returns>
        /// <remarks>
        /// This method is sealed in order to guarantee expression rewrite safety by avoiding dangerous rewrites
        /// that don't take constraints on the rewrite of <see cref="BinaryExpression.Conversion"/> into consideration.
        ///
        /// In order to guarantee expression rewrite safety, override <see cref="VisitBinaryWithoutConversion"/>
        /// and <see cref="VisitBinaryConversion"/> instead.
        ///
        /// In case safety checks should be ignored by derived types, override <see cref="VisitBinaryUnsafe"/>
        /// instead.
        /// </remarks>
        protected sealed override Expression VisitBinary(BinaryExpression node) => VisitBinaryUnsafe(node);

        /// <summary>
        /// Visits a binary expression without any safety checks. See remarks on <see cref="VisitBinary"/>
        /// for more information.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <returns>The result of visiting the binary expression.</returns>
        protected virtual Expression VisitBinaryUnsafe(BinaryExpression node)
        {
            if (node.Conversion != null)
            {
                return VisitBinaryWithConversion(node);
            }
            else
            {
                return VisitBinaryWithoutConversion(node);
            }
        }

        /// <summary>
        /// Visits a binary expression that has a <see cref="BinaryExpression.Conversion"/>.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <returns>The result of visiting the binary expression.</returns>
        /// <remarks>
        /// When overriding this method, derived implementations should take care to ensure a strongly-typed
        /// visit to the <see cref="BinaryExpression.Conversion"/> property. To ensure strong typing, a call
        /// to <see cref="VisitBinaryConversion(LambdaExpression)"/> can be made.
        /// </remarks>
        protected virtual Expression VisitBinaryWithConversion(BinaryExpression node)
        {
            var left = Visit(node.Left);
            var newConversion = VisitBinaryConversion(node.Conversion);
            var right = Visit(node.Right);

            return node.Update(left, newConversion, right);
        }

        /// <summary>
        /// Visits the <see cref="BinaryExpression.Conversion"/> node in a strongly typed manner.
        /// </summary>
        /// <param name="node">The lambda expression to visit.</param>
        /// <returns>The result of visiting the lambda expression.</returns>
        protected virtual LambdaExpression VisitBinaryConversion(LambdaExpression node)
        {
            var body = Visit(node.Body);
            var parameters = VisitAndConvert(node.Parameters, nameof(VisitBinaryConversion));

            if (body == node.Body && parameters == node.Parameters)
            {
                return node;
            }

            return Expression.Lambda(node.Type, body, node.Name, node.TailCall, parameters);
        }

        /// <summary>
        /// Visits a binary expression that has no <see cref="BinaryExpression.Conversion"/>.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <returns>The result of visiting the binary expression.</returns>
        protected virtual Expression VisitBinaryWithoutConversion(BinaryExpression node) => base.VisitBinary(node);

        /// <summary>
        /// Visits a list initialization expression with a strongly typed visit to <see cref="ListInitExpression.NewExpression"/>
        /// using <see cref="VisitListInitNew"/>.
        /// </summary>
        /// <param name="node">The list initialization expression to visit.</param>
        /// <returns>The result of visiting the list initialization expression.</returns>
        /// <remarks>
        /// This method is sealed in order to guarantee expression rewrite safety by avoiding dangerous rewrites
        /// that don't take constraints on the rewrite of <see cref="ListInitExpression.NewExpression"/> into consideration.
        ///
        /// In order to guarantee expression rewrite safety, override <see cref="VisitListInitNew"/> and
        /// <see cref="ExpressionVisitor.VisitElementInit"/> instead.
        ///
        /// In case safety checks should be ignored by derived types, override <see cref="VisitListInitUnsafe"/>
        /// instead.
        /// </remarks>
        protected sealed override Expression VisitListInit(ListInitExpression node) => VisitListInitUnsafe(node);

        /// <summary>
        /// Visits a list initialization expression without any safety checks. See remarks on <see cref="VisitListInit"/>
        /// for more information.
        /// </summary>
        /// <param name="node">The list initialization expression to visit.</param>
        /// <returns>The result of visiting the list initialization expression.</returns>
        protected virtual Expression VisitListInitUnsafe(ListInitExpression node)
        {
            var @new = VisitListInitNew(node.NewExpression);
            var initializers = Visit(node.Initializers, VisitElementInit);
            return node.Update(@new, initializers);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Replace New suffix. (Name of expression tree node.)

        /// <summary>
        /// Visits the <see cref="ListInitExpression.NewExpression"/> node in a strongly typed manner.
        /// </summary>
        /// <param name="node">The new expression to visit.</param>
        /// <returns>The result of visiting the new expression.</returns>
        protected virtual NewExpression VisitListInitNew(NewExpression node)
        {
            var args = Visit(node.Arguments);
            return node.Update(args);
        }

#pragma warning restore CA1711
#pragma warning restore IDE0079

        /// <summary>
        /// Visits a member initialization expression with a strongly typed visit to <see cref="MemberInitExpression.NewExpression"/>
        /// using <see cref="VisitMemberInitNew"/>.
        /// </summary>
        /// <param name="node">The member initialization expression to visit.</param>
        /// <returns>The result of visiting the member initialization expression.</returns>
        /// <remarks>
        /// This method is sealed in order to guarantee expression rewrite safety by avoiding dangerous rewrites
        /// that don't take constraints on the rewrite of <see cref="MemberInitExpression.NewExpression"/> into consideration.
        ///
        /// In order to guarantee expression rewrite safety, override <see cref="VisitMemberInitNew"/> and
        /// <see cref="ExpressionVisitor.VisitMemberBinding"/> instead.
        ///
        /// In case safety checks should be ignored by derived types, override <see cref="VisitMemberInitUnsafe"/>
        /// instead.
        /// </remarks>
        protected sealed override Expression VisitMemberInit(MemberInitExpression node) => VisitMemberInitUnsafe(node);

        /// <summary>
        /// Visits a member initialization expression without any safety checks. See remarks on <see cref="VisitMemberInit"/>
        /// for more information.
        /// </summary>
        /// <param name="node">The member initialization expression to visit.</param>
        /// <returns>The result of visiting the member initialization expression.</returns>
        protected virtual Expression VisitMemberInitUnsafe(MemberInitExpression node)
        {
            var @new = VisitMemberInitNew(node.NewExpression);
            var bindings = Visit(node.Bindings, VisitMemberBinding);
            return node.Update(@new, bindings);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Replace New suffix. (Name of expression tree node.)

        /// <summary>
        /// Visits the <see cref="MemberInitExpression.NewExpression"/> node in a strongly typed manner.
        /// </summary>
        /// <param name="node">The new expression to visit.</param>
        /// <returns>The result of visiting the new expression.</returns>
        protected virtual NewExpression VisitMemberInitNew(NewExpression node)
        {
            var args = Visit(node.Arguments);
            return node.Update(args);
        }

#pragma warning restore CA1711
#pragma warning restore IDE0079

        /// <summary>
        /// Visits a runtime variables expression with a strongly typed visit to <see cref="RuntimeVariablesExpression.Variables"/>
        /// using <see cref="VisitRuntimeVariable"/>.
        /// </summary>
        /// <param name="node">The runtime variables expression to visit.</param>
        /// <returns>The result of visiting the runtime variables expression.</returns>
        /// <remarks>
        /// This method is sealed in order to guarantee expression rewrite safety by avoiding dangerous rewrites
        /// that don't take constraints on the rewrite of <see cref="RuntimeVariablesExpression.Variables"/> into consideration.
        ///
        /// In order to guarantee expression rewrite safety, override <see cref="VisitRuntimeVariable"/> instead.
        ///
        /// In case safety checks should be ignored by derived types, override <see cref="VisitRuntimeVariablesUnsafe"/>
        /// instead.
        /// </remarks>
        protected sealed override Expression VisitRuntimeVariables(RuntimeVariablesExpression node) => VisitRuntimeVariablesUnsafe(node);

        /// <summary>
        /// Visits a runtime variables expression without any safety checks. See remarks on <see cref="VisitRuntimeVariables"/>
        /// for more information.
        /// </summary>
        /// <param name="node">The runtime variables expression to visit.</param>
        /// <returns>The result of visiting the runtime variables expression.</returns>
        protected virtual Expression VisitRuntimeVariablesUnsafe(RuntimeVariablesExpression node)
        {
            var variables = Visit(node.Variables, VisitRuntimeVariable);
            return node.Update(variables);
        }

        /// <summary>
        /// Visits a variable that occurs in a <see cref="RuntimeVariablesExpression"/>.
        /// </summary>
        /// <param name="node">The variable to visit.</param>
        /// <returns>The result of visiting the variable.</returns>
        protected virtual ParameterExpression VisitRuntimeVariable(ParameterExpression node) => node;

        /// <summary>
        /// Visits a unary expression with a strongly typed visit to <see cref="UnaryExpression.Operand"/>
        /// for nodes of type <see cref="ExpressionType.Quote"/> using <see cref="VisitUnaryQuoteOperand"/>.
        /// </summary>
        /// <param name="node">The unary expression to visit.</param>
        /// <returns>The result of visiting the unary expression.</returns>
        /// <remarks>
        /// This method is sealed in order to guarantee expression rewrite safety by avoiding dangerous rewrites
        /// that don't take constraints on the rewrite of <see cref="UnaryExpression.Operand"/> into consideration.
        ///
        /// In order to guarantee expression rewrite safety, override <see cref="VisitUnaryNonQuote"/> and
        /// <see cref="VisitUnaryQuoteOperand"/> instead.
        ///
        /// In case safety checks should be ignored by derived types, override <see cref="VisitUnaryUnsafe"/>
        /// instead.
        /// </remarks>
        protected sealed override Expression VisitUnary(UnaryExpression node) => VisitUnaryUnsafe(node);

        /// <summary>
        /// Visits a unary expression without any safety checks. See remarks on <see cref="VisitUnary"/>
        /// for more information.
        /// </summary>
        /// <param name="node">The unary expression to visit.</param>
        /// <returns>The result of visiting the unary expression.</returns>
        protected virtual Expression VisitUnaryUnsafe(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Quote)
            {
                return VisitUnaryQuote(node);
            }
            else
            {
                return VisitUnaryNonQuote(node);
            }
        }

        /// <summary>
        /// Visits a unary expression of type <see cref="ExpressionType.Quote"/>.
        /// </summary>
        /// <param name="node">The unary expression to visit.</param>
        /// <returns>The result of visiting the unary expression.</returns>
        /// <remarks>
        /// When overriding this method, derived implementations should take care to ensure a strongly-typed
        /// visit to the <see cref="UnaryExpression.Operand"/> property. To ensure strong typing, a call
        /// to <see cref="VisitUnaryQuoteOperand"/> can be made.
        /// </remarks>
        protected virtual Expression VisitUnaryQuote(UnaryExpression node)
        {
            var operand = VisitUnaryQuoteOperand((LambdaExpression)node.Operand);

            return node.Update(operand);
        }

        /// <summary>
        /// Visits a unary expression that is not of type <see cref="ExpressionType.Quote"/>.
        /// </summary>
        /// <param name="node">The unary expression to visit.</param>
        /// <returns>The result of visiting the unary expression.</returns>
        protected virtual Expression VisitUnaryNonQuote(UnaryExpression node) => base.VisitUnary(node);

        /// <summary>
        /// Visits the <see cref="UnaryExpression.Operand"/> node of a <see cref="ExpressionType.Quote"/>
        /// in a strongly typed manner.
        /// </summary>
        /// <param name="node">The lambda expression to visit.</param>
        /// <returns>The result of visiting the lambda expression.</returns>
        protected virtual LambdaExpression VisitUnaryQuoteOperand(LambdaExpression node)
        {
            var body = Visit(node.Body);
            var parameters = VisitAndConvert(node.Parameters, nameof(VisitUnaryQuoteOperand));

            if (body == node.Body && parameters == node.Parameters)
            {
                return node;
            }

            return Expression.Lambda(node.Type, body, node.Name, node.TailCall, parameters);
        }
    }
}
