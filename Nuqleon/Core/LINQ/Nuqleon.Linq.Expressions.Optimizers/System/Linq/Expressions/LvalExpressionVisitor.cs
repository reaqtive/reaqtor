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

using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Expression visitor with tracking of "left values" (<c>lval</c>s) to guarantee
    /// rewrite safety.
    /// </summary>
    /// <remarks>
    /// Nodes of type <see cref="IndexExpression"/>, <see cref="MemberExpression"/>, and <see cref="ParameterExpression"/>
    /// can be used as assignment targets and will be visited using specialized <see cref="VisitIndex(IndexExpression, bool)"/>,
    /// <see cref="VisitMember(MemberExpression, bool)"/>, and <see cref="VisitParameter(ParameterExpression, bool)"/> methods.
    /// </remarks>
    public class LvalExpressionVisitor : SafeExpressionVisitor
    {
        // REVIEW: Should we keep the `sealed override` + `virtual VisitXyzCore` pattern to raise awareness
        //         of base class behavior or should it be obvious that one should call the base method when
        //         overriding a method? How about consistency with the `isLval` methods? Should these also
        //         have corresponding `Core` methods, or should they be named as such?

        /// <summary>
        /// Visits an expression that occurs in an assignment target position.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The result of visiting the expression.</returns>
        protected virtual Expression VisitLval(Expression node)
        {
            if (node != null)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Index:
                        return VisitIndex((IndexExpression)node, isLval: true);
                    case ExpressionType.MemberAccess:
                        return VisitMember((MemberExpression)node, isLval: true);
                    case ExpressionType.Parameter:
                        return VisitParameter((ParameterExpression)node, isLval: true);
                }
            }

            return Visit(node);
        }

        /// <summary>
        /// Visits a member expression and visits the <see cref="MemberExpression.Expression"/> of a value type
        /// using <see cref="VisitLval"/>. It is assumed that the member expression is not the target of an
        /// assignment itself.
        /// </summary>
        /// <param name="node">The member expression to visit.</param>
        /// <returns>The result of visiting the member expression.</returns>
        protected sealed override Expression VisitMember(MemberExpression node) => VisitMember(node, isLval: IsValueInstance(node.Expression));

        /// <summary>
        /// Visits a member expression with the specified <paramref name="isLval"/> behavior.
        /// See <see cref="VisitMember(MemberExpression)"/> for more information about visitor behavior.
        /// </summary>
        /// <param name="node">The member expression to visit.</param>
        /// <param name="isLval">Indicates whether the member expression occurs in an assignment target position.</param>
        /// <returns>The result of visiting the member expression.</returns>
        protected virtual Expression VisitMember(MemberExpression node, bool isLval)
        {
            var expression = isLval ? VisitLval(node.Expression) : Visit(node.Expression);

            return node.Update(expression);
        }

        /// <summary>
        /// Visits an index expression and visits the <see cref="IndexExpression.Object"/> of a value type
        /// using <see cref="VisitLval"/> and <see cref="IndexExpression.Arguments"/> that are passed to
        /// <c>ref</c> or <c>out</c> parameters using <see cref="VisitLval"/>. It is assumed that the index
        /// expression is not the target of an assignment itself.
        /// </summary>
        /// <param name="node">The index expression to visit.</param>
        /// <returns>The result of visiting the index expression.</returns>
        protected sealed override Expression VisitIndex(IndexExpression node) => VisitIndex(node, isLval: IsValueInstance(node.Object));

        /// <summary>
        /// Visits an index expression with the specified <paramref name="isLval"/> behavior.
        /// See <see cref="VisitIndex(IndexExpression)"/> for more information about visitor behavior.
        /// </summary>
        /// <param name="node">The index expression to visit.</param>
        /// <param name="isLval">Indicates whether the index expression occurs in an assignment target position.</param>
        /// <returns>The result of visiting the index expression.</returns>
        protected virtual Expression VisitIndex(IndexExpression node, bool isLval)
        {
            var @object = isLval ? VisitLval(node.Object) : Visit(node.Object);

            if (node.Indexer != null)
            {
                var indexParameters = node.Indexer.GetIndexParameters();

                var arguments = VisitArguments(node.Arguments, indexParameters);

                return node.Update(@object, arguments);
            }
            else
            {
                return node.Update(@object, Visit(node.Arguments));
            }
        }

        /// <summary>
        /// Visits a binary expression that has a <see cref="BinaryExpression.Conversion"/>, determines whether
        /// it is an assignment expression, and calls <see cref="VisitBinaryWithConversion(BinaryExpression, bool)"/>.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <returns>The result of visiting the binary expression.</returns>
        protected sealed override Expression VisitBinaryWithConversion(BinaryExpression node) => VisitBinaryWithConversion(node, IsAssignment(node.NodeType));

        /// <summary>
        /// Visits a binary expression that has a <see cref="BinaryExpression.Conversion"/> using the specified
        /// assignment behavior. If the node represents an assignment, the <see cref="BinaryExpression.Left"/> node
        /// is visited using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <param name="isAssignment">Indicates if the node represents an assignment.</param>
        /// <returns>The result of visiting the binary expression.</returns>
        protected virtual Expression VisitBinaryWithConversion(BinaryExpression node, bool isAssignment)
        {
            if (isAssignment)
            {
                var left = VisitLval(node.Left);
                var conversion = base.VisitBinaryConversion(node.Conversion);
                var right = Visit(node.Right);
                return node.Update(left, conversion, right);
            }
            else
            {
                return base.VisitBinaryWithConversion(node);
            }
        }

        /// <summary>
        /// Visits a binary expression that has no <see cref="BinaryExpression.Conversion"/>, determines whether
        /// it is an assignment expression, and calls <see cref="VisitBinaryWithoutConversion(BinaryExpression, bool)"/>.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <returns>The result of visiting the binary expression.</returns>
        protected sealed override Expression VisitBinaryWithoutConversion(BinaryExpression node) => VisitBinaryWithoutConversion(node, IsAssignment(node.NodeType));

        /// <summary>
        /// Visits a binary expression that has no <see cref="BinaryExpression.Conversion"/> using the specified
        /// assignment behavior. If the node represents an assignment, the <see cref="BinaryExpression.Left"/> node
        /// is visited using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <param name="isAssignment">Indicates if the node represents an assignment.</param>
        /// <returns>The result of visiting the binary expression.</returns>
        protected virtual Expression VisitBinaryWithoutConversion(BinaryExpression node, bool isAssignment)
        {
            if (isAssignment)
            {
                var left = VisitLval(node.Left);
                var right = Visit(node.Right);
                return node.Update(left, conversion: null, right);
            }
            else
            {
                return base.VisitBinaryWithoutConversion(node);
            }
        }

        /// <summary>
        /// Visits a unary expression that's not a <see cref="ExpressionType.Quote"/>, determines whether
        /// it is an assignment expression, and calls <see cref="VisitUnaryNonQuote(UnaryExpression, bool)"/>.
        /// </summary>
        /// <param name="node">The unary expression to visit.</param>
        /// <returns>The result of visiting the unary expression.</returns>
        protected sealed override Expression VisitUnaryNonQuote(UnaryExpression node) => VisitUnaryNonQuote(node, IsAssignment(node.NodeType));

        /// <summary>
        /// Visits a unary expression that's not a <see cref="ExpressionType.Quote"/> using the specified
        /// assignment behavior. If the node represents an assignment, the <see cref="UnaryExpression.Operand"/>
        /// is visited using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The unary expression to visit.</param>
        /// <param name="isAssignment">Indicates if the node represents an assignment.</param>
        /// <returns>The result of visiting the unary expression.</returns>
        protected virtual Expression VisitUnaryNonQuote(UnaryExpression node, bool isAssignment)
        {
            if (isAssignment)
            {
                var operand = VisitLval(node.Operand);
                return node.Update(operand);
            }
            else
            {
                return base.VisitUnaryNonQuote(node);
            }
        }

        /// <summary>
        /// Visits an invocation expression and visits <see cref="InvocationExpression.Arguments"/> that are passed
        /// to <c>ref</c> or <c>out</c> parameters using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The invocation expression to visit.</param>
        /// <returns>The result of visiting the invocation expression.</returns>
        /// <remarks>
        /// This method is sealed in order to guarantee proper assignment target tracking. In order
        /// to guarantee assignment target tracking, override <see cref="VisitInvocationCore"/> instead.
        /// </remarks>
        protected sealed override Expression VisitInvocation(InvocationExpression node) => VisitInvocationCore(node);

        /// <summary>
        /// Visits an invocation expression and visits <see cref="InvocationExpression.Arguments"/> that are passed
        /// to <c>ref</c> or <c>out</c> parameters using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The invocation expression to visit.</param>
        /// <returns>The result of visiting the invocation expression.</returns>
        protected virtual Expression VisitInvocationCore(InvocationExpression node)
        {
            var expression = Visit(node.Expression);

            var parameters = GetInvokeMethod(expression).GetParameters();

            var arguments = VisitArguments(node.Arguments, parameters);

            return node.Update(expression, arguments);
        }

        /// <summary>
        /// Visits a new expression and visits <see cref="NewExpression.Arguments"/> that are passed to <c>ref</c>
        /// or <c>out</c> parameters using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The new expression to visit.</param>
        /// <returns>The result of visiting the new expression.</returns>
        protected sealed override Expression VisitNew(NewExpression node) => VisitNewCore(node);

        /// <summary>
        /// Visits a new expression and visits <see cref="NewExpression.Arguments"/> that are passed to <c>ref</c>
        /// or <c>out</c> parameters using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The new expression to visit.</param>
        /// <returns>The result of visiting the new expression.</returns>
        protected virtual Expression VisitNewCore(NewExpression node)
        {
            if (node.Constructor != null)
            {
                var parameters = node.Constructor.GetParameters();

                var arguments = VisitArguments(node.Arguments, parameters);

                return node.Update(arguments);
            }
            else
            {
                return base.VisitNew(node);
            }
        }

        /// <summary>
        /// Visits a method call expression and visits the <see cref="MethodCallExpression.Object"/> of a value
        /// type using <see cref="VisitLval"/> and <see cref="MethodCallExpression.Arguments"/> that are passed
        /// to <c>ref</c> or <c>out</c> parameters using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The method call expression to visit.</param>
        /// <returns>The result of visiting the method call expression.</returns>
        /// <remarks>
        /// This method is sealed in order to guarantee proper assignment target tracking. In order
        /// to guarantee assignment target tracking, override <see cref="VisitMethodCallCore"/> instead.
        /// </remarks>
        protected sealed override Expression VisitMethodCall(MethodCallExpression node) => VisitMethodCallCore(node);

        /// <summary>
        /// Visits a method call expression and visits the <see cref="MethodCallExpression.Object"/> of a value
        /// type using <see cref="VisitLval"/> and <see cref="MethodCallExpression.Arguments"/> that are passed
        /// to <c>ref</c> or <c>out</c> parameters using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The method call expression to visit.</param>
        /// <returns>The result of visiting the method call expression.</returns>
        protected virtual Expression VisitMethodCallCore(MethodCallExpression node)
        {
            var oldObject = node.Object;
            var newObject = IsValueInstance(oldObject) ? VisitLval(oldObject) : Visit(oldObject);

            var parameters = node.Method.GetParameters();

            var arguments = VisitArguments(node.Arguments, parameters);

            return node.Update(newObject, arguments);
        }

        /// <summary>
        /// Visits a parameter expression that does not occur as the target of an assignment.
        /// </summary>
        /// <param name="node">The parameter expression to visit.</param>
        /// <returns>The result of visiting the parameter expression.</returns>
        protected sealed override Expression VisitParameter(ParameterExpression node) => VisitParameter(node, isLval: false);

        /// <summary>
        /// Visits a parameter expression with the specified <paramref name="isLval"/> behavior.
        /// </summary>
        /// <param name="node">The parameter expression to visit.</param>
        /// <param name="isLval">Indicates whether the parameter expression occurs in an assignment target position.</param>
        /// <returns>The result of visiting the parameter expression.</returns>
        protected virtual Expression VisitParameter(ParameterExpression node, bool isLval)
        {
            return base.VisitParameter(node);
        }

        /// <summary>
        /// Visits a runtime variable expression assuming it may get assigned to.
        /// </summary>
        /// <param name="node">The runtime variable expression to visit.</param>
        /// <returns>The result of visiting the runtime variable expression.</returns>
        protected override ParameterExpression VisitRuntimeVariable(ParameterExpression node)
        {
            var res = VisitLval(node);

            if (res.NodeType != ExpressionType.Parameter)
            {
                throw new InvalidOperationException($"The node '{node}' must rewrite to the same type '{typeof(ParameterExpression)}' in '{nameof(VisitRuntimeVariable)}'.");
            }

            return (ParameterExpression)res;
        }

        /// <summary>
        /// Visits a dynamic expression and visits <see cref="DynamicExpression.Arguments"/> that are passed
        /// to <c>ref</c> or <c>out</c> parameters using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The dynamic expression to visit.</param>
        /// <returns>The result of visiting the dynamic expression.</returns>
        /// <remarks>
        /// This method is sealed in order to guarantee proper assignment target tracking. In order
        /// to guarantee assignment target tracking, override <see cref="VisitDynamicCore"/> instead.
        /// </remarks>
        protected sealed override Expression VisitDynamic(DynamicExpression node) => VisitDynamicCore(node);

        /// <summary>
        /// Visits a dynamic expression and visits <see cref="DynamicExpression.Arguments"/> that are passed
        /// to <c>ref</c> or <c>out</c> parameters using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The dynamic expression to visit.</param>
        /// <returns>The result of visiting the dynamic expression.</returns>
        protected virtual Expression VisitDynamicCore(DynamicExpression node)
        {
            var delegateType = node.DelegateType;

            var invokeMethod = delegateType.GetMethod("Invoke");

            var arguments = VisitArguments(node.Arguments, invokeMethod.GetParameters(), skip: 1);

            return node.Update(arguments);
        }

        /// <summary>
        /// Visits the expression arguments using the corresponding parameters to determine whether each
        /// argument is assigned to a <c>ref</c> or <c>out</c> parameter. Arguments that are passed by
        /// reference get visited using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="arguments">The argument expressions to visit.</param>
        /// <param name="parameters">The parameters corresponding to the arguments.</param>
        /// <returns>The result of visiting the expression arguments.</returns>
        protected ReadOnlyCollection<Expression> VisitArguments(ReadOnlyCollection<Expression> arguments, ParameterInfo[] parameters) => VisitArguments(arguments, parameters, skip: 0);

        /// <summary>
        /// Visits the expression <paramref name="arguments"/> using the corresponding <paramref name="parameters"/>
        /// to determine whether each argument is assigned to a <c>ref</c> or <c>out</c> parameter. Arguments that are
        /// passed by reference get visited using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="arguments">The argument expressions to visit.</param>
        /// <param name="parameters">The parameters corresponding to the arguments.</param>
        /// <param name="skip">The number of parameters to skip when pairing them with corresponding arguments.</param>
        /// <returns>The result of visiting the expression arguments.</returns>
        protected virtual ReadOnlyCollection<Expression> VisitArguments(ReadOnlyCollection<Expression> arguments, ParameterInfo[] parameters, int skip)
        {
            var res = default(Expression[]);

            for (int i = 0, n = arguments.Count; i < n; i++)
            {
                var parameter = parameters[skip + i];

                var oldArgument = arguments[i];
                var newArgument = VisitArgument(oldArgument, parameter);

                if (res == null && oldArgument != newArgument)
                {
                    res = new Expression[n];

                    for (var j = 0; j < i; j++)
                    {
                        res[j] = arguments[j];
                    }
                }

                if (res != null)
                {
                    res[i] = newArgument;
                }
            }

            return res != null ? new ReadOnlyCollection<Expression>(res) : arguments;
        }

        /// <summary>
        /// Visits the expression <paramref name="argument"/> using the corresponding <paramref name="parameter"/>
        /// to determine whether the argument is assigned to a <c>ref</c> or <c>out</c> parameter. An argument that 
        /// is passed by reference gets visited using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="argument">The argument expression to visit.</param>
        /// <param name="parameter">The parameter corresponding to the argument.</param>
        /// <returns>The result of visiting the expression arguments.</returns>
        protected virtual Expression VisitArgument(Expression argument, ParameterInfo parameter)
        {
            return parameter.ParameterType.IsByRef ? VisitLval(argument) : Visit(argument);
        }

        /// <summary>
        /// Checks if the specified node type represents an assignment expression node.
        /// </summary>
        /// <param name="nodeType">The node type to check.</param>
        /// <returns>true if the specified node type represents an assignment expression node; otherwise, false.</returns>
        protected static bool IsAssignment(ExpressionType nodeType) => nodeType
            is ExpressionType.Assign
            or ExpressionType.AddAssign
            or ExpressionType.AddAssignChecked
            or ExpressionType.SubtractAssign
            or ExpressionType.SubtractAssignChecked
            or ExpressionType.MultiplyAssign
            or ExpressionType.MultiplyAssignChecked
            or ExpressionType.DivideAssign
            or ExpressionType.ModuloAssign
            or ExpressionType.PowerAssign
            or ExpressionType.LeftShiftAssign
            or ExpressionType.RightShiftAssign
            or ExpressionType.AndAssign
            or ExpressionType.OrAssign
            or ExpressionType.ExclusiveOrAssign
            or ExpressionType.PostDecrementAssign
            or ExpressionType.PostIncrementAssign
            or ExpressionType.PreDecrementAssign
            or ExpressionType.PreIncrementAssign;

        /// <summary>
        /// Checks if the specified node represents a value instance and requires a visit using <see cref="VisitLval"/>.
        /// </summary>
        /// <param name="node">The node to check.</param>
        /// <returns>true if the node represents a value instance; otherwise, false.</returns>
        private static bool IsValueInstance(Expression node) => node?.Type.IsValueType ?? false;

        /// <summary>
        /// Gets the delegate invocation method of the specified expression that represents an invocation target.
        /// </summary>
        /// <param name="expression">The invocation target expression.</param>
        /// <returns>The method representing the delegate invocation method of the invocation target expression.</returns>
        protected static MethodInfo GetInvokeMethod(Expression expression)
        {
            var type = expression.Type;

            if (!expression.Type.IsSubclassOf(typeof(MulticastDelegate)))
            {
                var exprType = TypeUtils.FindGenericType(typeof(Expression<>), expression.Type);

                if (exprType == null)
                {
                    throw new InvalidOperationException($"Type '{type}' is not a delegate type or a lambda expression type.");
                }

                type = exprType.GetGenericArguments()[0];
            }

            return type.GetMethod("Invoke");
        }
    }
}
