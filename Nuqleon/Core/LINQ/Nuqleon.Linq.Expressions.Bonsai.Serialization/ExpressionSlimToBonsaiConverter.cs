// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    #region Aliases

    //
    // This makes diffing with the original Bonsai serializer easier.
    //

    using Visitor = ExpressionSlimVisitor<Json.Expression, Json.Expression, Json.Expression, Json.Expression, Json.Expression, Json.Expression>;

    using Object = ObjectSlim;

    using Type = System.Reflection.TypeSlim;
    using MemberInfo = System.Reflection.MemberInfoSlim;

    using LabelTarget = System.Linq.Expressions.LabelTargetSlim;

    using Expression = System.Linq.Expressions.ExpressionSlim;
    using BinaryExpression = System.Linq.Expressions.BinaryExpressionSlim;
    using BlockExpression = System.Linq.Expressions.BlockExpressionSlim;
    using ConditionalExpression = System.Linq.Expressions.ConditionalExpressionSlim;
    using ConstantExpression = System.Linq.Expressions.ConstantExpressionSlim;
    using DefaultExpression = System.Linq.Expressions.DefaultExpressionSlim;
    using IndexExpression = System.Linq.Expressions.IndexExpressionSlim;
    using InvocationExpression = System.Linq.Expressions.InvocationExpressionSlim;
    using LambdaExpression = System.Linq.Expressions.LambdaExpressionSlim;
    using LabelExpression = System.Linq.Expressions.LabelExpressionSlim;
    using ListInitExpression = System.Linq.Expressions.ListInitExpressionSlim;
    using MemberExpression = System.Linq.Expressions.MemberExpressionSlim;
    using MemberInitExpression = System.Linq.Expressions.MemberInitExpressionSlim;
    using MethodCallExpression = System.Linq.Expressions.MethodCallExpressionSlim;
    using NewExpression = System.Linq.Expressions.NewExpressionSlim;
    using NewArrayExpression = System.Linq.Expressions.NewArrayExpressionSlim;
    using ParameterExpression = System.Linq.Expressions.ParameterExpressionSlim;
    using TypeBinaryExpression = System.Linq.Expressions.TypeBinaryExpressionSlim;
    using UnaryExpression = System.Linq.Expressions.UnaryExpressionSlim;

    using ElementInit = System.Linq.Expressions.ElementInitSlim;

    using MemberAssignment = System.Linq.Expressions.MemberAssignmentSlim;
    using MemberListBinding = System.Linq.Expressions.MemberListBindingSlim;
    using MemberMemberBinding = System.Linq.Expressions.MemberMemberBindingSlim;

    #endregion

    internal abstract class ExpressionSlimToBonsaiConverter : Visitor
    {
        #region Fields

        private readonly SerializationState _state;

        #endregion

        #region Constructors

        protected ExpressionSlimToBonsaiConverter(SerializationState state)
        {
            _state = state;
        }

        #endregion

        #region Visit Methods

        public override Json.Expression Visit(Expression node)
        {
            if (node == null)
            {
                return Json.Expression.Null();
            }

            return base.Visit(node);
        }

        protected override Json.Expression MakeBinary(BinaryExpression node, Json.Expression left, Json.Expression conversion, Json.Expression right)
        {
            return node.NodeType switch
            {
                ExpressionType.Add => VisitBinarySimple(node, Discriminators.Expression.PlusDiscriminator, left, right),
                ExpressionType.AddChecked => VisitBinarySimple(node, Discriminators.Expression.PlusDollarDiscriminator, left, right),
                ExpressionType.Subtract => VisitBinarySimple(node, Discriminators.Expression.MinusDiscriminator, left, right),
                ExpressionType.SubtractChecked => VisitBinarySimple(node, Discriminators.Expression.MinusDollarDiscriminator, left, right),
                ExpressionType.Multiply => VisitBinarySimple(node, Discriminators.Expression.MultiplyDiscriminator, left, right),
                ExpressionType.MultiplyChecked => VisitBinarySimple(node, Discriminators.Expression.MultiplyCheckedDiscriminator, left, right),
                ExpressionType.Divide => VisitBinarySimple(node, Discriminators.Expression.DivideDiscriminator, left, right),
                ExpressionType.Modulo => VisitBinarySimple(node, Discriminators.Expression.ModuloDiscriminator, left, right),
                ExpressionType.Power => VisitBinarySimple(node, Discriminators.Expression.PowerDiscriminator, left, right),
                ExpressionType.RightShift => VisitBinarySimple(node, Discriminators.Expression.RightShiftDiscriminator, left, right),
                ExpressionType.LeftShift => VisitBinarySimple(node, Discriminators.Expression.LeftShiftDiscriminator, left, right),
                ExpressionType.LessThan => VisitBinaryComparison(node, Discriminators.Expression.LessThanDiscriminator, left, right),
                ExpressionType.LessThanOrEqual => VisitBinaryComparison(node, Discriminators.Expression.LessThanOrEqualDiscriminator, left, right),
                ExpressionType.GreaterThan => VisitBinaryComparison(node, Discriminators.Expression.GreaterThanDiscriminator, left, right),
                ExpressionType.GreaterThanOrEqual => VisitBinaryComparison(node, Discriminators.Expression.GreaterThanOrEqualDiscriminator, left, right),
                ExpressionType.Equal => VisitBinaryComparison(node, Discriminators.Expression.EqualDiscriminator, left, right),
                ExpressionType.NotEqual => VisitBinaryComparison(node, Discriminators.Expression.NotEqualDiscriminator, left, right),
                ExpressionType.And => VisitBinarySimple(node, Discriminators.Expression.AndDiscriminator, left, right),
                ExpressionType.AndAlso => VisitBinarySimple(node, Discriminators.Expression.AndAlsoDiscriminator, left, right),
                ExpressionType.Or => VisitBinarySimple(node, Discriminators.Expression.OrDiscriminator, left, right),
                ExpressionType.OrElse => VisitBinarySimple(node, Discriminators.Expression.OrElseDiscriminator, left, right),
                ExpressionType.ExclusiveOr => VisitBinarySimple(node, Discriminators.Expression.ExclusiveOrDiscriminator, left, right),
                ExpressionType.Coalesce => VisitBinaryCoalesce(node, left, conversion, right),
                ExpressionType.ArrayIndex => VisitBinarySimple(node, Discriminators.Expression.ArrayIndexDiscriminator, left, right),
                ExpressionType.Assign => VisitBinarySimple(node, Discriminators.Expression.AssignDiscriminator, left, right),
                ExpressionType.AddAssign => VisitBinaryOpAssign(node, Discriminators.Expression.AddAssignDiscriminator, left, conversion, right),
                ExpressionType.AddAssignChecked => VisitBinaryOpAssign(node, Discriminators.Expression.AddAssignCheckedDiscriminator, left, conversion, right),
                ExpressionType.AndAssign => VisitBinaryOpAssign(node, Discriminators.Expression.AndAssignDiscriminator, left, conversion, right),
                ExpressionType.DivideAssign => VisitBinaryOpAssign(node, Discriminators.Expression.DivideAssignDiscriminator, left, conversion, right),
                ExpressionType.ExclusiveOrAssign => VisitBinaryOpAssign(node, Discriminators.Expression.ExclusiveOrAssignDiscriminator, left, conversion, right),
                ExpressionType.LeftShiftAssign => VisitBinaryOpAssign(node, Discriminators.Expression.LeftShiftAssignDiscriminator, left, conversion, right),
                ExpressionType.ModuloAssign => VisitBinaryOpAssign(node, Discriminators.Expression.ModuloAssignDiscriminator, left, conversion, right),
                ExpressionType.MultiplyAssign => VisitBinaryOpAssign(node, Discriminators.Expression.MultiplyAssignDiscriminator, left, conversion, right),
                ExpressionType.MultiplyAssignChecked => VisitBinaryOpAssign(node, Discriminators.Expression.MultiplyAssignCheckedDiscriminator, left, conversion, right),
                ExpressionType.OrAssign => VisitBinaryOpAssign(node, Discriminators.Expression.OrAssignDiscriminator, left, conversion, right),
                ExpressionType.PowerAssign => VisitBinaryOpAssign(node, Discriminators.Expression.PowerAssignDiscriminator, left, conversion, right),
                ExpressionType.RightShiftAssign => VisitBinaryOpAssign(node, Discriminators.Expression.RightShiftAssignDiscriminator, left, conversion, right),
                ExpressionType.SubtractAssign => VisitBinaryOpAssign(node, Discriminators.Expression.SubtractAssignDiscriminator, left, conversion, right),
                ExpressionType.SubtractAssignChecked => VisitBinaryOpAssign(node, Discriminators.Expression.SubtractAssignCheckedDiscriminator, left, conversion, right),
                _ => throw new NotImplementedException(),
            };
        }

        private Json.Expression VisitBinaryOpAssign(BinaryExpression node, Json.Expression discriminator, Json.Expression left, Json.Expression conversion, Json.Expression right)
        {
            if (node.Method != null)
            {
                var method = AddMember(node.Method);

                if (node.Conversion != null)
                {
                    return Json.Expression.Array(
                        discriminator,
                        left,
                        right,
                        method,
                        conversion
                    );
                }

                return Json.Expression.Array(
                    discriminator,
                    left,
                    right,
                    method
                );
            }
            else
            {
                if (node.Conversion != null)
                {
                    return Json.Expression.Array(
                        discriminator,
                        left,
                        right,
                        Json.Expression.Null(),
                        conversion
                    );
                }

                return Json.Expression.Array(
                    discriminator,
                    left,
                    right
                );
            }
        }

        private static Json.Expression VisitBinaryCoalesce(BinaryExpression node, Json.Expression left, Json.Expression conversion, Json.Expression right)
        {
            if (node.Conversion != null)
            {
                return Json.Expression.Array(
                    Discriminators.Expression.CoalesceDiscriminator,
                    left,
                    right,
                    conversion
                );
            }
            else
            {
                return Json.Expression.Array(
                    Discriminators.Expression.CoalesceDiscriminator,
                    left,
                    right
                );
            }
        }

        private Json.Expression VisitBinaryComparison(BinaryExpression node, Json.Expression discriminator, Json.Expression left, Json.Expression right)
        {
            Debug.Assert(node.Conversion == null);

            if (node.IsLiftedToNull)
            {
                var method = node.Method != null ? AddMember(node.Method) : Json.Expression.Null();

                return Json.Expression.Array(
                    discriminator,
                    left,
                    right,
                    method,
                    Json.Expression.Boolean(node.IsLiftedToNull)
                );
            }
            else if (node.Method != null)
            {
                var method = AddMember(node.Method);

                return Json.Expression.Array(
                    discriminator,
                    left,
                    right,
                    method
                );
            }
            else
            {
                return Json.Expression.Array(
                    discriminator,
                    left,
                    right
                );
            }
        }

        private Json.Expression VisitBinarySimple(BinaryExpression node, Json.Expression discriminator, Json.Expression left, Json.Expression right)
        {
            Debug.Assert(node.Conversion == null);

            if (node.Method != null)
            {
                var method = AddMember(node.Method);

                return Json.Expression.Array(
                    discriminator,
                    left,
                    right,
                    method
                );
            }
            else
            {
                return Json.Expression.Array(
                    discriminator,
                    left,
                    right
                );
            }
        }

        protected override Json.Expression VisitBlock(BlockExpression node)
        {
            var variables = Push(node.Variables);

            var expressions = Visit(node.Expressions);

            Pop();

            return MakeBlock(node, variables, expressions);
        }

        protected override Json.Expression MakeBlock(BlockExpression node, ReadOnlyCollection<Json.Expression> variables, ReadOnlyCollection<Json.Expression> expressions)
        {
            var type = _state.AddType(node.Type);

            return Json.Expression.Array(
                Discriminators.Expression.BlockDiscriminator,
                type,
                Json.Expression.Array(variables),
                Json.Expression.Array(expressions)
            );
        }

        protected override Json.Expression VisitCatchBlock(CatchBlockSlim node)
        {
            var variable = default(Json.Expression);
            if (node.Variable != null)
            {
                variable = Push(new[] { node.Variable })[0];
            }

            var body = Visit(node.Body);
            var filter = Visit(node.Filter);

            if (node.Variable != null)
            {
                Pop();
            }

            return MakeCatchBlock(node, variable, body, filter);
        }

        protected override Json.Expression MakeCatchBlock(CatchBlockSlim node, Json.Expression variable, Json.Expression body, Json.Expression filter)
        {
            var type = AddType(node.Test);

            return Json.Expression.Array(
                variable ?? Json.Expression.Null(),
                body,
                filter,
                type
            );
        }

        protected override Json.Expression VisitConditional(ConditionalExpression node)
        {
            if (node.Type != null && !_state.IsV08)
            {
                var test = Visit(node.Test);
                var ifTrue = Visit(node.IfTrue);
                var ifFalse = Visit(node.IfFalse);
                var type = _state.AddType(node.Type);

                return Json.Expression.Array(
                    Discriminators.Expression.ConditionalDiscriminator,
                    test,
                    ifTrue,
                    ifFalse,
                    type
                );
            }
            else
            {
                return base.VisitConditional(node);
            }
        }

        protected override Json.Expression MakeConditional(ConditionalExpression node, Json.Expression test, Json.Expression ifTrue, Json.Expression ifFalse)
        {
            return Json.Expression.Array(
                Discriminators.Expression.ConditionalDiscriminator,
                test,
                ifTrue,
                ifFalse
            );
        }

        protected override Json.Expression MakeConstant(ConstantExpression node)
        {
            var value = VisitConstantValue(node.Value);
            var type = AddType(node.Type);

            return Json.Expression.Array(
                Discriminators.Expression.ConstantDiscriminator,
                value,
                type
            );
        }

        protected abstract Json.Expression VisitConstantValue(Object value);

        protected override Json.Expression MakeDefault(DefaultExpression node)
        {
            var type = AddType(node.Type);

            return Json.Expression.Array(
                Discriminators.Expression.DefaultDiscriminator,
                type
            );
        }

        protected override Json.Expression MakeElementInit(ElementInit node, ReadOnlyCollection<Json.Expression> arguments)
        {
            var n = arguments.Count;

            var args = new Json.Expression[n + 1];

            args[0] = AddMember(node.AddMethod);

            for (var i = 0; i < n; i++)
            {
                args[i + 1] = arguments[i];
            }

            return Json.Expression.Array(
                args
            );
        }

        protected override Json.Expression MakeGoto(GotoExpressionSlim node, Json.Expression target, Json.Expression value)
        {
            var kind = ((int)node.Kind).ToJsonNumber();

            if (node.Type == null)
            {
                return Json.Expression.Array(
                    Discriminators.Expression.GotoDiscriminator,
                    kind,
                    target,
                    value
                );
            }

            var type = AddType(node.Type);
            return Json.Expression.Array(
                Discriminators.Expression.GotoDiscriminator,
                kind,
                target,
                value,
                type
            );
        }

        protected override Json.Expression MakeIndex(IndexExpression node, Json.Expression @object, ReadOnlyCollection<Json.Expression> arguments)
        {
            if (_state.IsV08)
            {
                throw new NotSupportedException("Index expressions can only be serialized in Bonsai v0.9 or later.");
            }

            var indexer = AddMember(node.Indexer);

            return Json.Expression.Array(
                Discriminators.Expression.IndexDiscriminator,
                @object,
                indexer,
                Json.Expression.Array(arguments)
            );
        }

        protected override Json.Expression MakeInvocation(InvocationExpression node, Json.Expression expression, ReadOnlyCollection<Json.Expression> arguments)
        {
            return Json.Expression.Array(
                Discriminators.Expression.InvocationDiscriminator,
                expression,
                Json.Expression.Array(arguments)
            );
        }

        protected override Json.Expression MakeLabel(LabelExpression node, Json.Expression target, Json.Expression defaultValue)
        {
            return Json.Expression.Array(
                Discriminators.Expression.LabelDiscriminator,
                target,
                defaultValue
            );
        }

        protected override Json.Expression MakeLabelTarget(LabelTarget node)
        {
            return AddLabelTarget(node);
        }

        protected override Json.Expression VisitLambda(LambdaExpression node)
        {
            var parameters = Push(node.Parameters);

            var body = Visit(node.Body);

            Pop();

            return MakeLambda(node, body, parameters);
        }

        protected override Json.Expression MakeLambda(LambdaExpression node, Json.Expression body, ReadOnlyCollection<Json.Expression> parameters)
        {
            var delegateType = node.DelegateType == null ? Json.Expression.Null() : AddType(node.DelegateType);

            return Json.Expression.Array(
                Discriminators.Expression.LambdaDiscriminator,
                delegateType,
                body,
                Json.Expression.Array(parameters)
            );
        }

        protected override Json.Expression MakeListInit(ListInitExpression node, Json.Expression newExpression, ReadOnlyCollection<Json.Expression> initializers)
        {
            return Json.Expression.Array(
                Discriminators.Expression.ListInitDiscriminator,
                newExpression,
                Json.Expression.Array(initializers)
            );
        }

        protected override Json.Expression MakeLoop(LoopExpressionSlim node, Json.Expression body, Json.Expression breakLabel, Json.Expression continueLabel)
        {
            if (node.ContinueLabel != null)
            {
                return Json.Expression.Array(
                    Discriminators.Expression.LoopDiscriminator,
                    body,
                    node.BreakLabel != null ? breakLabel : Json.Expression.Null(),
                    continueLabel
                );
            }

            if (node.BreakLabel != null)
            {
                return Json.Expression.Array(
                    Discriminators.Expression.LoopDiscriminator,
                    body,
                    breakLabel
                );
            }

            return Json.Expression.Array(
                Discriminators.Expression.LoopDiscriminator,
                body
            );
        }

        protected override Json.Expression MakeMember(MemberExpression node, Json.Expression expression)
        {
            var member = AddMember(node.Member);

            if (node.Expression != null)
            {
                return Json.Expression.Array(
                    Discriminators.Expression.MemberAccessDiscriminator,
                    member,
                    expression
                );
            }
            else
            {
                return Json.Expression.Array(
                    Discriminators.Expression.MemberAccessDiscriminator,
                    member
                );
            }
        }

        protected override Json.Expression MakeMemberAssignment(MemberAssignment node, Json.Expression expression)
        {
            var member = AddMember(node.Member);

            return Json.Expression.Array(
                Discriminators.MemberBinding.MemberAssignmentDiscriminator,
                member,
                expression
            );
        }

        protected override Json.Expression MakeMemberInit(MemberInitExpression node, Json.Expression newExpression, ReadOnlyCollection<Json.Expression> bindings)
        {
            return Json.Expression.Array(
                Discriminators.Expression.MemberInitDiscriminator,
                newExpression,
                Json.Expression.Array(bindings)
            );
        }

        protected override Json.Expression MakeMemberListBinding(MemberListBinding node, ReadOnlyCollection<Json.Expression> initializers)
        {
            var member = AddMember(node.Member);

            return Json.Expression.Array(
                Discriminators.MemberBinding.MemberListBindingDiscriminator,
                member,
                Json.Expression.Array(initializers)
            );
        }

        protected override Json.Expression MakeMemberMemberBinding(MemberMemberBinding node, ReadOnlyCollection<Json.Expression> bindings)
        {
            var member = AddMember(node.Member);

            return Json.Expression.Array(
                Discriminators.MemberBinding.MemberMemberBindingDiscriminator,
                member,
                Json.Expression.Array(bindings)
            );
        }

        protected override Json.Expression MakeMethodCall(MethodCallExpression node, Json.Expression @object, ReadOnlyCollection<Json.Expression> arguments)
        {
            var method = AddMember(node.Method);

            if (node.Object != null)
            {
                return Json.Expression.Array(
                    Discriminators.Expression.MethodCallDiscriminator,
                    method,
                    @object,
                    Json.Expression.Array(arguments)
                );
            }
            else
            {
                return Json.Expression.Array(
                    Discriminators.Expression.MethodCallDiscriminator,
                    method,
                    Json.Expression.Array(arguments)
                );
            }
        }

        protected override Json.Expression MakeNew(NewExpression node, ReadOnlyCollection<Json.Expression> arguments)
        {
            if (node.Constructor != null)
            {
                var ctor = AddMember(node.Constructor);

                // TODO: Bonsai v0.8 does not support arrays with 3 elements
                if (node.Members != null)
                {
                    var n = node.Members.Count;

                    var members = new Json.Expression[n];

                    for (var i = 0; i < n; i++)
                    {
                        members[i] = AddMember(node.Members[i]);
                    }

                    return Json.Expression.Array(
                        Discriminators.Expression.NewDiscriminator,
                        ctor,
                        Json.Expression.Array(arguments),
                        Json.Expression.Array(members)
                    );
                }
                else
                {
                    return Json.Expression.Array(
                        Discriminators.Expression.NewDiscriminator,
                        ctor,
                        Json.Expression.Array(arguments)
                    );
                }
            }
            else
            {
                var type = AddType(node.Type);

                return Json.Expression.Array(
                    Discriminators.Expression.NewDiscriminator,
                    type
                );
            }
        }

        protected override Json.Expression MakeNewArray(NewArrayExpression node, ReadOnlyCollection<Json.Expression> expressions)
        {
            var discriminator = node.NodeType switch
            {
                ExpressionType.NewArrayBounds => Discriminators.Expression.NewArrayBoundsDiscriminator,
                ExpressionType.NewArrayInit => Discriminators.Expression.NewArrayInitDiscriminator,
                _ => throw new InvalidOperationException("Expected one of new array bounds expression or new array init expression."),
            };
            var count = expressions.Count;
            var n = 2 + count;

            var args = new Json.Expression[n];

            args[0] = discriminator;
            args[1] = AddType(node.ElementType);

            for (var i = 0; i < count; i++)
            {
                args[i + 2] = expressions[i];
            }

            return Json.Expression.Array(
                args
            );
        }

        protected override Json.Expression MakeParameter(ParameterExpression node)
        {
            return _state.Lookup(node);
        }

        protected override Json.Expression MakeSwitch(SwitchExpressionSlim node, Json.Expression switchValue, Json.Expression defaultBody, ReadOnlyCollection<Json.Expression> cases)
        {
            var member = (Json.Expression)Json.Expression.Null();
            var type = (Json.Expression)Json.Expression.Null();

            if (node.Comparison != null)
            {
                member = AddMember(node.Comparison);
            }

            if (node.Type != null)
            {
                type = AddType(node.Type);
            }

            return Json.Expression.Array(
                Discriminators.Expression.SwitchDiscriminator,
                switchValue,
                Json.Expression.Array(cases),
                defaultBody,
                member,
                type
            );
        }

        protected override Json.Expression MakeSwitchCase(SwitchCaseSlim node, Json.Expression body, ReadOnlyCollection<Json.Expression> testValues)
        {
            return Json.Expression.Array(
                Json.Expression.Array(testValues),
                body
            );
        }

        protected override Json.Expression MakeTry(TryExpressionSlim node, Json.Expression body, Json.Expression @finally, Json.Expression fault, ReadOnlyCollection<Json.Expression> handlers)
        {
            var type = AddType(node.Type);

            return Json.Expression.Array(
                Discriminators.Expression.TryDiscriminator,
                body,
                Json.Expression.Array(handlers),
                @finally,
                fault,
                type
            );
        }

        protected override Json.Expression MakeTypeBinary(TypeBinaryExpression node, Json.Expression expression)
        {
            var typeOperand = AddType(node.TypeOperand);
            var discriminator = node.NodeType switch
            {
                ExpressionType.TypeIs => Discriminators.Expression.TypeIsDiscriminator,
                ExpressionType.TypeEqual => Discriminators.Expression.TypeEqualDiscriminator,
                _ => throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected type binary node type '{0}'.", node.NodeType)),
            };
            return Json.Expression.Array(
                discriminator,
                typeOperand,
                expression
            );
        }

        protected override Json.Expression MakeUnary(UnaryExpression node, Json.Expression operand)
        {
            return node.NodeType switch
            {
                ExpressionType.Negate => VisitUnarySimple(node, Discriminators.Expression.MinusDiscriminator, operand),
                ExpressionType.NegateChecked => VisitUnarySimple(node, Discriminators.Expression.MinusDollarDiscriminator, operand),
                ExpressionType.UnaryPlus => VisitUnarySimple(node, Discriminators.Expression.PlusDiscriminator, operand),
                ExpressionType.OnesComplement => VisitUnarySimple(node, Discriminators.Expression.OnesComplementDiscriminator, operand),
                ExpressionType.Decrement => VisitUnarySimple(node, Discriminators.Expression.DecrementDiscriminator, operand),
                ExpressionType.Increment => VisitUnarySimple(node, Discriminators.Expression.IncrementDiscriminator, operand),
                ExpressionType.Not => VisitUnarySimple(node, Discriminators.Expression.NotDiscriminator, operand),
                ExpressionType.Convert => VisitUnarySimple(node, Discriminators.Expression.ConvertDiscriminator, operand),
                ExpressionType.ConvertChecked => VisitUnarySimple(node, Discriminators.Expression.ConvertCheckedDiscriminator, operand),
                ExpressionType.TypeAs => VisitUnarySimple(node, Discriminators.Expression.TypeAsDiscriminator, operand),
                ExpressionType.Quote => VisitUnarySimple(node, Discriminators.Expression.QuoteDiscriminator, operand),
                ExpressionType.ArrayLength => VisitUnarySimple(node, Discriminators.Expression.ArrayLengthDiscriminator, operand),
                ExpressionType.PostDecrementAssign => VisitUnarySimple(node, Discriminators.Expression.PostDecrementAssignDiscriminator, operand),
                ExpressionType.PostIncrementAssign => VisitUnarySimple(node, Discriminators.Expression.PostIncrementAssignDiscriminator, operand),
                ExpressionType.PreDecrementAssign => VisitUnarySimple(node, Discriminators.Expression.PreDecrementAssignDiscriminator, operand),
                ExpressionType.PreIncrementAssign => VisitUnarySimple(node, Discriminators.Expression.PreIncrementAssignDiscriminator, operand),
                ExpressionType.Throw => VisitUnarySimple(node, Discriminators.Expression.ThrowDiscriminator, operand),
                ExpressionType.Unbox => VisitUnarySimple(node, Discriminators.Expression.UnboxDiscriminator, operand),
                ExpressionType.IsFalse => VisitUnarySimple(node, Discriminators.Expression.IsFalseDiscriminator, operand),
                ExpressionType.IsTrue => VisitUnarySimple(node, Discriminators.Expression.IsTrueDiscriminator, operand),
                _ => throw new NotImplementedException(),
            };
        }

        private Json.Expression VisitUnarySimple(UnaryExpression node, Json.Expression discriminator, Json.Expression operand)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.TypeAs:
                case ExpressionType.Unbox:
                    {
                        var type = AddType(node.Type);

                        if (node.Method != null)
                        {
                            var method = AddMember(node.Method);

                            return Json.Expression.Array(
                                discriminator,
                                type,
                                operand,
                                method
                            );
                        }
                        else
                        {
                            return Json.Expression.Array(
                                discriminator,
                                type,
                                operand
                            );
                        }
                    }
                case ExpressionType.Throw:
                    {
                        if (node.Type == typeof(void).ToTypeSlim())
                        {
                            return Json.Expression.Array(
                                discriminator,
                                operand
                            );
                        }
                        else
                        {
                            var type = AddType(node.Type);

                            return Json.Expression.Array(
                                discriminator,
                                operand,
                                type
                            );
                        }
                    }
                default:
                    {
                        if (node.Method != null)
                        {
                            var method = AddMember(node.Method);

                            return Json.Expression.Array(
                                discriminator,
                                operand,
                                method
                            );
                        }
                        else
                        {
                            return Json.Expression.Array(
                                discriminator,
                                operand
                            );
                        }
                    }
            }
        }

        #endregion

        #region State Helpers

        private Json.Expression AddType(Type type)
        {
            return _state.AddType(type);
        }

        private Json.Expression AddMember(MemberInfo member)
        {
            return _state.AddMember(member);
        }

        private Json.Expression AddLabelTarget(LabelTarget labelTarget)
        {
            return _state.AddLabelTarget(labelTarget);
        }

        private ReadOnlyCollection<Json.Expression> Push(IList<ParameterExpression> parameters)
        {
            return _state.Push(parameters);
        }

        private void Pop()
        {
            _state.Pop();
        }

        #endregion
    }
}
