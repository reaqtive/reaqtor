// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices.Bonsai
{
    /// <summary>
    /// Expression slim visitor that recurses over the expression tree and attempts to return type information.
    /// </summary>
    public class TypeSlimDerivationVisitor : ExpressionSlimVisitor<TypeSlim, TypeSlim, TypeSlim, TypeSlim, TypeSlim, TypeSlim>
    {
        /// <summary>
        /// The equality comparer to use for type equality checks. Specifying a custom (pooled)
        /// comparer can significantly improve performance.
        /// </summary>
        private readonly IEqualityComparer<TypeSlim> _typeEqualityComparer = TypeSlimEqualityComparer.Default;

        /// <summary>
        /// Creates a new <see cref="TypeSlimDerivationVisitor"/> with the default equality comparer for <see cref="TypeSlim"/> instances.
        /// </summary>
        public TypeSlimDerivationVisitor()
            : this(TypeSlimEqualityComparer.Default)
        {
        }

        /// <summary>
        /// Creates a new <see cref="TypeSlimDerivationVisitor"/> with the specified equality comparer for <see cref="TypeSlim"/> instances.
        /// </summary>
        /// <param name="typeSlimEqualityComparer">The equality comparer to use for <see cref="TypeSlim"/> equality checks.</param>
        public TypeSlimDerivationVisitor(IEqualityComparer<TypeSlim> typeSlimEqualityComparer)
        {
            _typeEqualityComparer = typeSlimEqualityComparer ?? throw new ArgumentNullException(nameof(typeSlimEqualityComparer));
        }

        /// <summary>
        /// Attempt to derive a type from a binary expression.
        /// </summary>
        /// <param name="node">The binary expression.</param>
        /// <param name="left">The type derived from the left expression.</param>
        /// <param name="conversion">The type derived from the conversion expression.</param>
        /// <param name="right">The type derived from the right expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected override TypeSlim MakeBinary(BinaryExpressionSlim node, TypeSlim left, TypeSlim conversion, TypeSlim right)
        {
            return node.NodeType switch
            {
                ExpressionType.Add or
                ExpressionType.AddChecked or
                ExpressionType.Subtract or
                ExpressionType.SubtractChecked or
                ExpressionType.Multiply or
                ExpressionType.MultiplyChecked or
                ExpressionType.Divide or
                ExpressionType.Modulo or
                ExpressionType.Power => MakeBinaryArithmetic(node, left, conversion, right),

                ExpressionType.And or
                ExpressionType.Or or
                ExpressionType.ExclusiveOr or
                ExpressionType.RightShift or
                ExpressionType.LeftShift => MakeBinaryBitwise(node, left, conversion, right),

                ExpressionType.ArrayIndex => MakeBinaryArrayIndex(node, left, conversion, right),

                ExpressionType.LessThan or
                ExpressionType.LessThanOrEqual or
                ExpressionType.GreaterThan or
                ExpressionType.GreaterThanOrEqual or
                ExpressionType.Equal or
                ExpressionType.NotEqual => MakeBinaryComparison(node, left, conversion, right),

                ExpressionType.AndAlso or
                ExpressionType.OrElse => MakeBinaryConditionalLogic(node, left, conversion, right),

                ExpressionType.Coalesce => MakeBinaryCoalesce(node, left, conversion, right),

                _ => CheckDerivedType(type: null, node),
            };
        }

        /// <summary>
        /// Attempt to derive a type from an arithmetic binary expression.
        /// </summary>
        /// <param name="node">The binary expression.</param>
        /// <param name="left">The type derived from the left expression.</param>
        /// <param name="conversion">The type derived from the conversion expression.</param>
        /// <param name="right">The type derived from the right expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeBinaryArithmetic(BinaryExpressionSlim node, TypeSlim left, TypeSlim conversion, TypeSlim right)
        {
            if (node.Method != null)
            {
                return MakeBinaryMethodBased(node, left, conversion, right);
            }
            else
            {
                var result = default(TypeSlim);

                if (_typeEqualityComparer.Equals(left, right) && left.IsArithmetic())
                {
                    result = left;
                }

                return CheckDerivedType(result, node);
            }
        }

        /// <summary>
        /// Attempt to derive a type from a binary array index expression.
        /// </summary>
        /// <param name="node">The binary expression.</param>
        /// <param name="left">The type derived from the left expression.</param>
        /// <param name="conversion">The type derived from the conversion expression.</param>
        /// <param name="right">The type derived from the right expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeBinaryArrayIndex(BinaryExpressionSlim node, TypeSlim left, TypeSlim conversion, TypeSlim right)
        {
            if (left is ArrayTypeSlim arrayLeft)
            {
                return arrayLeft.ElementType;
            }

            return CheckDerivedType(type: null, node);
        }

        /// <summary>
        /// Attempt to derive a type from a binary array index expression.
        /// </summary>
        /// <param name="node">The binary expression.</param>
        /// <param name="left">The type derived from the left expression.</param>
        /// <param name="conversion">The type derived from the conversion expression.</param>
        /// <param name="right">The type derived from the right expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeBinaryBitwise(BinaryExpressionSlim node, TypeSlim left, TypeSlim conversion, TypeSlim right)
        {
            if (node.Method != null)
            {
                return MakeBinaryMethodBased(node, left, conversion, right);
            }
            else
            {
                var result = default(TypeSlim);

                if (_typeEqualityComparer.Equals(left, right) && left.IsIntegerOrBool())
                {
                    result = left;
                }

                return CheckDerivedType(result, node);
            }
        }

        /// <summary>
        /// Attempt to derive a type from a binary coalesce expression.
        /// </summary>
        /// <param name="node">The binary expression.</param>
        /// <param name="left">The type derived from the left expression.</param>
        /// <param name="conversion">The type derived from the conversion expression.</param>
        /// <param name="right">The type derived from the right expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeBinaryCoalesce(BinaryExpressionSlim node, TypeSlim left, TypeSlim conversion, TypeSlim right)
        {
            var result = default(TypeSlim);

            if (conversion != null)
            {
                result = right;
            }
            else
            {
                if (_typeEqualityComparer.Equals(left, right))
                {
                    return CheckDerivedType(left, node);
                }

                var nonNullableType = left.GetNonNullableType();
                if (left.IsNullableType() && _typeEqualityComparer.Equals(nonNullableType, right))
                {
                    result = nonNullableType;
                }
            }

            return CheckDerivedType(result, node);
        }

        /// <summary>
        /// Attempt to derive a type from a binary comparison expression.
        /// </summary>
        /// <param name="node">The binary expression.</param>
        /// <param name="left">The type derived from the left expression.</param>
        /// <param name="conversion">The type derived from the conversion expression.</param>
        /// <param name="right">The type derived from the right expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        /// <remarks>
        /// There is a known issue for deriving incorrect types for user-defined binary operators.
        /// </remarks>
        protected virtual TypeSlim MakeBinaryComparison(BinaryExpressionSlim node, TypeSlim left, TypeSlim conversion, TypeSlim right)
        {
            if (node.Method != null)
            {
                return MakeBinaryMethodBased(node, left, conversion, right);
            }
            else
            {
                var result = left.IsNullableType() && node.IsLiftedToNull
                    ? TypeSlimExtensions.BooleanType.GetNullableType()
                    : TypeSlimExtensions.BooleanType;

                return CheckDerivedType(result, node);
            }
        }

        /// <summary>
        /// Attempt to derive a type from a logical binary expression.
        /// </summary>
        /// <param name="node">The binary expression.</param>
        /// <param name="left">The type derived from the left expression.</param>
        /// <param name="conversion">The type derived from the conversion expression.</param>
        /// <param name="right">The type derived from the right expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeBinaryConditionalLogic(BinaryExpressionSlim node, TypeSlim left, TypeSlim conversion, TypeSlim right)
        {
            var result = default(TypeSlim);
            if (node.Method != null)
            {
                var returnType = node.Method.ReturnType;

                result = left.IsNullableType() && _typeEqualityComparer.Equals(returnType, left.GetNonNullableType()) ? left : returnType;
            }
            else
            {
                if (_typeEqualityComparer.Equals(left, right))
                {
                    if (left.GetNonNullableType().IsBooleanType())
                    {
                        result = left.IsNullableType() ? TypeSlimExtensions.BooleanType.GetNullableType() : TypeSlimExtensions.BooleanType;
                    }
                }
            }
            return CheckDerivedType(result, node);
        }

        /// <summary>
        /// Attempt to derive a type from a binary expression with a defined method.
        /// </summary>
        /// <param name="node">The binary expression.</param>
        /// <param name="left">The type derived from the left expression.</param>
        /// <param name="conversion">The type derived from the conversion expression.</param>
        /// <param name="right">The type derived from the right expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeBinaryMethodBased(BinaryExpressionSlim node, TypeSlim left, TypeSlim conversion, TypeSlim right)
        {
            var result = default(TypeSlim);

            var parameterTypes = node.Method.ParameterTypes;
            var returnType = node.Method.ReturnType;

            // If the left and right types exactly match the method parameters...
            // Note, in System.Linq.Expressions, this check is actually based on assignability, not equality.
            if (_typeEqualityComparer.Equals(parameterTypes[0], left) && _typeEqualityComparer.Equals(parameterTypes[1], right))
            {
                result = returnType;
            }
            // Otherwise, if the non-nullable versions of the arguments match...
            else if (_typeEqualityComparer.Equals(parameterTypes[0], left.GetNonNullableType()) && _typeEqualityComparer.Equals(parameterTypes[1], right.GetNonNullableType()))
            {
                // Boolean return types have the special case if they are not lifted to null.
                if (returnType.IsBooleanType() && !node.IsLiftedToNull)
                {
                    result = returnType;
                }
                else
                {
                    result = returnType.GetNullableType();
                }
            }

            return CheckDerivedType(result, node);
        }

        /// <summary>
        /// Attempt to derive a type from a block expression.
        /// </summary>
        /// <param name="node">The block expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitBlock(BlockExpressionSlim node)
        {
            return CheckDerivedType(node.Type ?? Visit(node.Result), node);
        }

        /// <summary>
        /// Attempt to derive a type from a block expression.
        /// </summary>
        /// <param name="node">The block expression.</param>
        /// <param name="variables">The derived types of the variable expressions.</param>
        /// <param name="expressions">The derived type of the body expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeBlock(BlockExpressionSlim node, ReadOnlyCollection<TypeSlim> variables, ReadOnlyCollection<TypeSlim> expressions)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a catch block.
        /// </summary>
        /// <param name="node">The catch block.</param>
        /// <param name="variable">The derived type of the variable expression.</param>
        /// <param name="body">The derived type of the body expression.</param>
        /// <param name="filter">The derived type of the filter expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeCatchBlock(CatchBlockSlim node, TypeSlim variable, TypeSlim body, TypeSlim filter)
        {
            // No visit method since catch blocks can only be introduced by try statements, which
            // do not use the catch block for type inference.
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a conditional expression.
        /// </summary>
        /// <param name="node">The conditional expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitConditional(ConditionalExpressionSlim node)
        {
            return CheckDerivedType(node.Type ?? Visit(node.IfTrue), node);
        }

        /// <summary>
        /// Attempt to derive a type from a conditional expression.
        /// </summary>
        /// <param name="node">The conditional expression.</param>
        /// <param name="test">The derived type of the test expression.</param>
        /// <param name="ifTrue">The derived type of the if true branch expression.</param>
        /// <param name="ifFalse">The derived type of the if false branch expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeConditional(ConditionalExpressionSlim node, TypeSlim test, TypeSlim ifTrue, TypeSlim ifFalse)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a constant expression.
        /// </summary>
        /// <param name="node">The constant expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitConstant(ConstantExpressionSlim node)
        {
            return CheckDerivedType(node.Type, node);
        }

        /// <summary>
        /// Attempt to derive a type from a constant expression.
        /// </summary>
        /// <param name="node">The constant expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeConstant(ConstantExpressionSlim node)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a default expression.
        /// </summary>
        /// <param name="node">The default expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitDefault(DefaultExpressionSlim node)
        {
            return CheckDerivedType(node.Type, node);
        }

        /// <summary>
        /// Attempt to derive a type from a default expression.
        /// </summary>
        /// <param name="node">The default expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeDefault(DefaultExpressionSlim node)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from an element init node.
        /// </summary>
        /// <param name="node">The element init node.</param>
        /// <param name="arguments">The arguments to the element init.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeElementInit(ElementInitSlim node, ReadOnlyCollection<TypeSlim> arguments)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from an goto expression.
        /// </summary>
        /// <param name="node">The goto expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitGoto(GotoExpressionSlim node)
        {
            return CheckDerivedType(node.Type, node);
        }

        /// <summary>
        /// Attempt to derive a type from an goto expression.
        /// </summary>
        /// <param name="node">The goto expression.</param>
        /// <param name="target">The derived type of the target label.</param>
        /// <param name="value">The derived type of the value expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeGoto(GotoExpressionSlim node, TypeSlim target, TypeSlim value)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from an index expression.
        /// </summary>
        /// <param name="node">The index expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitIndex(IndexExpressionSlim node)
        {
            return CheckDerivedType(node.Indexer.PropertyType, node);
        }

        /// <summary>
        /// Attempt to derive a type from an index expression.
        /// </summary>
        /// <param name="node">The index expression.</param>
        /// <param name="object">The derived type of the object expression.</param>
        /// <param name="arguments">The derived types of the argument expressions.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeIndex(IndexExpressionSlim node, TypeSlim @object, ReadOnlyCollection<TypeSlim> arguments)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from an invocation expression.
        /// </summary>
        /// <param name="node">The invocation expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitInvocation(InvocationExpressionSlim node)
        {
            var exprType = Visit(node.Expression);

            var result = default(TypeSlim);

            if (exprType.IsActionType())
            {
                result = TypeSlimExtensions.VoidType;
            }
            else
            {
                if (exprType is GenericTypeSlim genericExpressionType)
                {
                    // Check if invocation of function delegate

                    if (genericExpressionType.GenericTypeDefinition.Name.StartsWith("System.Func", StringComparison.Ordinal))
                    {
                        var lastIndex = genericExpressionType.GenericArgumentCount - 1;
                        result = genericExpressionType.GetGenericArgument(lastIndex);
                    }
                    // Otherwise assume invocation of action delegate
                    else
                    {
                        result = TypeSlimExtensions.VoidType;
                    }
                }
            }

            return CheckDerivedType(result, node);
        }

        /// <summary>
        /// Attempt to derive a type from an invocation expression.
        /// </summary>
        /// <param name="node">The invocation expression.</param>
        /// <param name="expression">The derived type of the invocation target.</param>
        /// <param name="arguments">The derived types of the invocation arguments.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeInvocation(InvocationExpressionSlim node, TypeSlim expression, ReadOnlyCollection<TypeSlim> arguments)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from an label expression.
        /// </summary>
        /// <param name="node">The label expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitLabel(LabelExpressionSlim node)
        {
            return CheckDerivedType(VisitLabelTarget(node.Target), node);
        }

        /// <summary>
        /// Attempt to derive a type from an label expression.
        /// </summary>
        /// <param name="node">The label expression.</param>
        /// <param name="target">The derived type of the target label.</param>
        /// <param name="defaultValue">The derived type of the default value expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeLabel(LabelExpressionSlim node, TypeSlim target, TypeSlim defaultValue)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from an label target.
        /// </summary>
        /// <param name="node">The label target.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitLabelTarget(LabelTargetSlim node)
        {
            return node.Type;
        }

        /// <summary>
        /// Attempt to derive a type from an label target.
        /// </summary>
        /// <param name="node">The label target.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeLabelTarget(LabelTargetSlim node)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a lambda expression.
        /// </summary>
        /// <param name="node">The lambda expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitLambda(LambdaExpressionSlim node)
        {
            if (node.DelegateType != null)
            {
                return CheckDerivedType(node.DelegateType, node);
            }

            return base.VisitLambda(node);
        }

        /// <summary>
        /// Attempt to derive a type from a lambda expression.
        /// </summary>
        /// <param name="node">The lambda expression.</param>
        /// <param name="body">The derived type of the body.</param>
        /// <param name="parameters">The derived types of the parameters.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected override TypeSlim MakeLambda(LambdaExpressionSlim node, TypeSlim body, ReadOnlyCollection<TypeSlim> parameters)
        {
            var result = default(TypeSlim);
            // Check if the body is void, in which case we know the delegate type is an Action.
            if (TypeSlimExtensions.VoidType.Equals(body) && parameters.All(t => t != null))
            {
                result = TypeSlim.Generic(TypeSlimExtensions.GetActionType(parameters.Count), parameters);
            }
            return CheckDerivedType(result, node);
        }

        /// <summary>
        /// Attempt to derive a type from a list init expression.
        /// </summary>
        /// <param name="node">The list init expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitListInit(ListInitExpressionSlim node)
        {
            return CheckDerivedType(Visit(node.NewExpression), node);
        }

        /// <summary>
        /// Attempt to derive a type from a list init expression.
        /// </summary>
        /// <param name="node">The list init expression.</param>
        /// <param name="newExpression">The derived type of the new expression.</param>
        /// <param name="initializers">The derived types of the element initializers</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeListInit(ListInitExpressionSlim node, TypeSlim newExpression, ReadOnlyCollection<TypeSlim> initializers)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a loop expression.
        /// </summary>
        /// <param name="node">The loop expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitLoop(LoopExpressionSlim node)
        {
            return CheckDerivedType(node.BreakLabel != null ? VisitLabelTarget(node.BreakLabel) : TypeSlimExtensions.VoidType, node);
        }

        /// <summary>
        /// Attempt to derive a type from a loop expression.
        /// </summary>
        /// <param name="node">The loop expression.</param>
        /// <param name="body">The derived type of the body expression.</param>
        /// <param name="breakLabel">The derived type of the break label.</param>
        /// <param name="continueLabel">The derived type of the continue label.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeLoop(LoopExpressionSlim node, TypeSlim body, TypeSlim breakLabel, TypeSlim continueLabel)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a member expression.
        /// </summary>
        /// <param name="node">The member expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitMember(MemberExpressionSlim node)
        {
            var result = default(TypeSlim);
            switch (node.Member.MemberType)
            {
                case MemberTypes.Field:
                    result = ((FieldInfoSlim)node.Member).FieldType;
                    break;
                case MemberTypes.Property:
                    result = ((PropertyInfoSlim)node.Member).PropertyType;
                    break;
            }
            return CheckDerivedType(result, node);
        }

        /// <summary>
        /// Attempt to derive a type from a member expression.
        /// </summary>
        /// <param name="node">The member expression.</param>
        /// <param name="expression">The derived type of the child expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeMember(MemberExpressionSlim node, TypeSlim expression)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a member assignment binding.
        /// </summary>
        /// <param name="node">The member assignment binding.</param>
        /// <param name="expression">The derived type of the assigned expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeMemberAssignment(MemberAssignmentSlim node, TypeSlim expression)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a member init expression.
        /// </summary>
        /// <param name="node">The member init expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitMemberInit(MemberInitExpressionSlim node)
        {
            return CheckDerivedType(Visit(node.NewExpression), node);
        }

        /// <summary>
        /// Attempt to derive a type from a member init expression.
        /// </summary>
        /// <param name="node">The member init expression.</param>
        /// <param name="newExpression">The derived type of the new expression.</param>
        /// <param name="bindings">The derived types of the member bindings.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeMemberInit(MemberInitExpressionSlim node, TypeSlim newExpression, ReadOnlyCollection<TypeSlim> bindings)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a member list binding.
        /// </summary>
        /// <param name="node">The member list binding.</param>
        /// <param name="initializers">The derived types of the element initializers.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeMemberListBinding(MemberListBindingSlim node, ReadOnlyCollection<TypeSlim> initializers)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a member member binding.
        /// </summary>
        /// <param name="node">The member member binding.</param>
        /// <param name="bindings">The derived types of the child member bindings.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeMemberMemberBinding(MemberMemberBindingSlim node, ReadOnlyCollection<TypeSlim> bindings)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a method call expression.
        /// </summary>
        /// <param name="node">The method call expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitMethodCall(MethodCallExpressionSlim node)
        {
            return CheckDerivedType(node.Method.ReturnType, node);
        }

        /// <summary>
        /// Attempt to derive a type from a method call expression.
        /// </summary>
        /// <param name="node">The method call expression.</param>
        /// <param name="object">The derived type of the instance.</param>
        /// <param name="arguments">The derived types of the arguments.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeMethodCall(MethodCallExpressionSlim node, TypeSlim @object, ReadOnlyCollection<TypeSlim> arguments)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a new expression.
        /// </summary>
        /// <param name="node">The new expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitNew(NewExpressionSlim node)
        {
            return CheckDerivedType(node.Type, node);
        }

        /// <summary>
        /// Attempt to derive a type from a new expression.
        /// </summary>
        /// <param name="node">The new expression.</param>
        /// <param name="arguments">The derived types of the arguments.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeNew(NewExpressionSlim node, ReadOnlyCollection<TypeSlim> arguments)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a new array expression.
        /// </summary>
        /// <param name="node">The new array expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitNewArray(NewArrayExpressionSlim node)
        {
            var result = default(TypeSlim);
            if (node.ElementType != null)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.NewArrayInit:
                        result = TypeSlim.Array(node.ElementType);
                        break;
                    case ExpressionType.NewArrayBounds:
                        result = TypeSlim.Array(node.ElementType, node.Expressions.Count);
                        break;
                }
            }
            return CheckDerivedType(result, node);
        }

        /// <summary>
        /// Attempt to derive a type from a new array expression.
        /// </summary>
        /// <param name="node">The new array expression.</param>
        /// <param name="expressions">The derived types of the arguments.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeNewArray(NewArrayExpressionSlim node, ReadOnlyCollection<TypeSlim> expressions)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a parameter expression.
        /// </summary>
        /// <param name="node">The parameter expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected override TypeSlim MakeParameter(ParameterExpressionSlim node)
        {
            return CheckDerivedType(node.Type, node);
        }

        /// <summary>
        /// Attempt to derive a type from a switch expression.
        /// </summary>
        /// <param name="node">The switch expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitSwitch(SwitchExpressionSlim node)
        {
            return CheckDerivedType(node.Type ?? VisitSwitchCase(node.Cases[0]), node);
        }

        /// <summary>
        /// Attempt to derive a type from a switch expression.
        /// </summary>
        /// <param name="node">The switch expression.</param>
        /// <param name="switchValue">The derived type of the switch value expression.</param>
        /// <param name="defaultBody">The derived types of the default body expressions.</param>
        /// <param name="cases">The derived types of the switch cases.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeSwitch(SwitchExpressionSlim node, TypeSlim switchValue, TypeSlim defaultBody, ReadOnlyCollection<TypeSlim> cases)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a switch case.
        /// </summary>
        /// <param name="node">The switch case.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitSwitchCase(SwitchCaseSlim node)
        {
            return Visit(node.Body);
        }

        /// <summary>
        /// Attempt to derive a type from a switch case.
        /// </summary>
        /// <param name="node">The switch case.</param>
        /// <param name="body">The derived type of the body expression.</param>
        /// <param name="testValues">The derived types of the test value expressions.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeSwitchCase(SwitchCaseSlim node, TypeSlim body, ReadOnlyCollection<TypeSlim> testValues)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a try expression.
        /// </summary>
        /// <param name="node">The try expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitTry(TryExpressionSlim node)
        {
            return CheckDerivedType(node.Type ?? Visit(node.Body), node);
        }

        /// <summary>
        /// Attempt to derive a type from a try expression.
        /// </summary>
        /// <param name="node">The try expression.</param>
        /// <param name="body">The derived type of the body expression.</param>
        /// <param name="finally">The derived type of the finally expression.</param>
        /// <param name="fault">The derived type of the fault expression.</param>
        /// <param name="handlers">The derived types of the handler expressions.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeTry(TryExpressionSlim node, TypeSlim body, TypeSlim @finally, TypeSlim fault, ReadOnlyCollection<TypeSlim> handlers)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a type binary expression.
        /// </summary>
        /// <param name="node">The type binary expression.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected internal override TypeSlim VisitTypeBinary(TypeBinaryExpressionSlim node)
        {
            var result = default(TypeSlim);
            switch (node.NodeType)
            {
                case ExpressionType.TypeEqual:
                case ExpressionType.TypeIs:
                    result = TypeSlimExtensions.BooleanType;
                    break;
            }
            return CheckDerivedType(result, node);
        }

        /// <summary>
        /// Attempt to derive a type from a type binary expression.
        /// </summary>
        /// <param name="node">The type binary expression.</param>
        /// <param name="expression">The derived type of the child expression.</param>
        /// <returns>Always throws exception.</returns>
        protected override TypeSlim MakeTypeBinary(TypeBinaryExpressionSlim node, TypeSlim expression)
        {
            throw ShouldNotDispatch();
        }

        /// <summary>
        /// Attempt to derive a type from a unary expression.
        /// </summary>
        /// <param name="node">The unary expression.</param>
        /// <param name="operand">The derived type of the operand.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected override TypeSlim MakeUnary(UnaryExpressionSlim node, TypeSlim operand)
        {
            var result = default(TypeSlim);
            switch (node.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return MakeUnaryArithmeticSigned(node, operand);
                case ExpressionType.UnaryPlus:
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                    return MakeUnaryArithmetic(node, operand);
                case ExpressionType.OnesComplement:
                    return MakeUnaryOnesComplement(node, operand);
                case ExpressionType.Not:
                    return MakeUnaryNot(node, operand);
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.TypeAs:
                    result = node.Type;
                    break;
                case ExpressionType.Quote:
                    result = TypeSlim.Generic(TypeSlimExtensions.GenericExpressionType, operand);
                    break;
                case ExpressionType.ArrayLength:
                    result = TypeSlimExtensions.IntegerType;
                    break;
            }
            return CheckDerivedType(result, node);
        }

        /// <summary>
        /// Attempt to derive a type from an arithmetic unary expression.
        /// </summary>
        /// <param name="node">The unary expression.</param>
        /// <param name="operand">The derived type of the operand.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeUnaryArithmetic(UnaryExpressionSlim node, TypeSlim operand)
        {
            if (node.Method != null)
            {
                return MakeUnaryMethodBased(node, operand);
            }
            else
            {
                var result = default(TypeSlim);

                if (operand.IsArithmetic())
                {
                    result = operand;
                }

                return CheckDerivedType(result, node);
            }
        }

        /// <summary>
        /// Attempt to derive a type from an arithmetic unary expression on signed arithmetic types.
        /// </summary>
        /// <param name="node">The unary expression.</param>
        /// <param name="operand">The derived type of the operand.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeUnaryArithmeticSigned(UnaryExpressionSlim node, TypeSlim operand)
        {
            if (node.Method != null)
            {
                return MakeUnaryMethodBased(node, operand);
            }
            else
            {
                var result = default(TypeSlim);

                if (operand.IsArithmetic() && !operand.IsUnsignedInt())
                {
                    result = operand;
                }

                return CheckDerivedType(result, node);
            }
        }

        /// <summary>
        /// Attempt to derive a type from a unary not expression.
        /// </summary>
        /// <param name="node">The unary expression.</param>
        /// <param name="operand">The derived type of the operand.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeUnaryNot(UnaryExpressionSlim node, TypeSlim operand)
        {
            if (node.Method != null)
            {
                return MakeUnaryMethodBased(node, operand);
            }
            else
            {
                var result = default(TypeSlim);

                if (operand.IsIntegerOrBool())
                {
                    result = operand;
                }

                return CheckDerivedType(result, node);
            }
        }

        /// <summary>
        /// Attempt to derive a type from a unary ones complement expression.
        /// </summary>
        /// <param name="node">The unary expression.</param>
        /// <param name="operand">The derived type of the operand.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeUnaryOnesComplement(UnaryExpressionSlim node, TypeSlim operand)
        {
            if (node.Method != null)
            {
                return MakeUnaryMethodBased(node, operand);
            }
            else
            {
                var result = default(TypeSlim);

                if (operand.IsInteger())
                {
                    result = operand;
                }

                return CheckDerivedType(result, node);
            }
        }

        /// <summary>
        /// Attempt to derive a type from a method-based unary expression.
        /// </summary>
        /// <param name="node">The unary expression.</param>
        /// <param name="operand">The derived type of the operand.</param>
        /// <returns>The derived type, or null if it cannot be determined.</returns>
        protected virtual TypeSlim MakeUnaryMethodBased(UnaryExpressionSlim node, TypeSlim operand)
        {
            var result = default(TypeSlim);

            var parameterTypes = node.Method.ParameterTypes;
            var returnType = node.Method.ReturnType;

            if (_typeEqualityComparer.Equals(parameterTypes[0], operand))
            {
                result = returnType;
            }
            else if (operand.IsNullableType()
                     && _typeEqualityComparer.Equals(parameterTypes[0], operand.GetNonNullableType())
                     && !returnType.IsNullableType()
                     && returnType.IsValueType() /* NB: This check is expensive in the current implementation. */)
            {
                result = returnType.GetNullableType();
            }

            return result;
        }

        /// <summary>
        /// Hook to check the derivation result for an expression.  Could be used to detect occurrences of `null` results.
        /// </summary>
        /// <param name="type">The derivation result.</param>
        /// <param name="node">The expression the result was derived from.</param>
        /// <returns>The derivation result.</returns>
        protected virtual TypeSlim CheckDerivedType(TypeSlim type, ExpressionSlim node)
        {
            return type;
        }

        private static Exception ShouldNotDispatch()
        {
            return new InvalidOperationException("The overridden visit method should not dispatch here.");
        }
    }
}
