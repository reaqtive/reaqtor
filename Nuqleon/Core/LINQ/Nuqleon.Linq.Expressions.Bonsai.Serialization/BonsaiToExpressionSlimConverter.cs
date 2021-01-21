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
using System.Globalization;
using System.Linq.CompilerServices;
using System.Reflection;

using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    #region Aliases

    //
    // This makes diffing with the original Bonsai serializer easier.
    //

    using Expression = ExpressionSlim;
    using LambdaExpression = LambdaExpressionSlim;
    using NewExpression = NewExpressionSlim;
    using ParameterExpression = ParameterExpressionSlim;

    using CatchBlock = CatchBlockSlim;
    using ElementInit = ElementInitSlim;
    using LabelTarget = LabelTargetSlim;
    using MemberBinding = MemberBindingSlim;
    using SwitchCase = SwitchCaseSlim;

    using ConstructorInfo = ConstructorInfoSlim;
    using MemberInfo = MemberInfoSlim;
    using MethodInfo = MethodInfoSlim;
    using PropertyInfo = PropertyInfoSlim;
    using Type = TypeSlim;

    #endregion

    internal abstract class BonsaiToExpressionSlimConverter : Json.ExpressionVisitor<Expression>
    {
        #region Fields

        private readonly DeserializationState _state;
        private static readonly ReadOnlyCollection<Expression> s_empty = new TrueReadOnlyCollection<Expression>(Array.Empty<Expression>());

        #endregion

        #region Constructors

        protected BonsaiToExpressionSlimConverter(DeserializationState state)
        {
            _state = state;
        }

        #endregion

        #region Visitor Methods

        public override Expression VisitArray(Json.ArrayExpression node)
        {
            if (node.ElementCount == 0)
                throw new BonsaiParseException("Expected at least one JSON array element containing a discriminator.", node);

            var type = node.GetElement(0);
            if (type.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[0]' for the expression type discriminator.", node);

            var nodeType = (string)((Json.ConstantExpression)type).Value;
            return nodeType switch
            {
                Discriminators.Expression.Constant => VisitConstantExpression(node),
                Discriminators.Expression.Default => VisitDefaultExpression(node),
                Discriminators.Expression.Minus => VisitMinusExpression(node),
                Discriminators.Expression.MinusDollar => VisitMinusDollarExpression(node),
                Discriminators.Expression.Plus => VisitPlusExpression(node),
                Discriminators.Expression.PlusDollar => VisitPlusDollarExpression(node),
                Discriminators.Expression.OnesComplement => VisitUnaryExpression(node, ExpressionType.OnesComplement),
                Discriminators.Expression.Decrement => VisitUnaryExpression(node, ExpressionType.Decrement),
                Discriminators.Expression.Increment => VisitUnaryExpression(node, ExpressionType.Increment),
                Discriminators.Expression.Not => VisitUnaryExpression(node, ExpressionType.Not),
                Discriminators.Expression.Convert => VisitConvertExpression(node, isChecked: false),
                Discriminators.Expression.ConvertChecked => VisitConvertExpression(node, isChecked: true),
                Discriminators.Expression.TypeAs => VisitTypeAsExpression(node),
                Discriminators.Expression.Quote => VisitQuoteExpression(node),
                Discriminators.Expression.Multiply => VisitBinaryExpression(node, ExpressionType.Multiply),
                Discriminators.Expression.MultiplyChecked => VisitBinaryExpression(node, ExpressionType.MultiplyChecked),
                Discriminators.Expression.Divide => VisitBinaryExpression(node, ExpressionType.Divide),
                Discriminators.Expression.Modulo => VisitBinaryExpression(node, ExpressionType.Modulo),
                Discriminators.Expression.Power => VisitBinaryExpression(node, ExpressionType.Power),
                Discriminators.Expression.LeftShift => VisitBinaryExpression(node, ExpressionType.LeftShift),
                Discriminators.Expression.RightShift => VisitBinaryExpression(node, ExpressionType.RightShift),
                Discriminators.Expression.LessThan => VisitBinaryComparisonExpression(node, ExpressionType.LessThan),
                Discriminators.Expression.LessThanOrEqual => VisitBinaryComparisonExpression(node, ExpressionType.LessThanOrEqual),
                Discriminators.Expression.GreaterThan => VisitBinaryComparisonExpression(node, ExpressionType.GreaterThan),
                Discriminators.Expression.GreaterThanOrEqual => VisitBinaryComparisonExpression(node, ExpressionType.GreaterThanOrEqual),
                Discriminators.Expression.Equal => VisitBinaryComparisonExpression(node, ExpressionType.Equal),
                Discriminators.Expression.NotEqual => VisitBinaryComparisonExpression(node, ExpressionType.NotEqual),
                Discriminators.Expression.And => VisitBinaryExpression(node, ExpressionType.And),
                Discriminators.Expression.AndAlso => VisitBinaryExpression(node, ExpressionType.AndAlso),
                Discriminators.Expression.Or => VisitBinaryExpression(node, ExpressionType.Or),
                Discriminators.Expression.OrElse => VisitBinaryExpression(node, ExpressionType.OrElse),
                Discriminators.Expression.ExclusiveOr => VisitBinaryExpression(node, ExpressionType.ExclusiveOr),
                Discriminators.Expression.Coalesce => VisitCoalesceExpression(node),
                Discriminators.Expression.TypeIs => VisitTypeIsExpression(node),
                Discriminators.Expression.TypeEqual => VisitTypeEqualExpression(node),
                Discriminators.Expression.Conditional => VisitConditionalExpression(node),
                Discriminators.Expression.Lambda => VisitLambdaExpression(node),
                Discriminators.Expression.Parameter => VisitParameterExpression(node),
                Discriminators.Expression.Index => VisitIndexExpression(node),
                Discriminators.Expression.Invocation => VisitInvocationExpression(node),
                Discriminators.Expression.MemberAccess => VisitMemberAccessExpression(node),
                Discriminators.Expression.MethodCall => VisitMethodCallExpression(node),
                Discriminators.Expression.New => VisitNewExpression(node),
                Discriminators.Expression.MemberInit => VisitMemberInitExpression(node),
                Discriminators.Expression.ListInit => VisitListInitExpression(node),
                Discriminators.Expression.NewArrayInit => VisitNewArrayInitExpression(node),
                Discriminators.Expression.NewArrayBounds => VisitNewArrayBoundsExpression(node),
                Discriminators.Expression.ArrayLength => VisitUnaryExpression(node, ExpressionType.ArrayLength),
                Discriminators.Expression.ArrayIndex => VisitArrayIndexExpression(node),
                Discriminators.Expression.Block => VisitBlockExpression(node),
                Discriminators.Expression.Goto => VisitGotoExpression(node),
                Discriminators.Expression.Label => VisitLabelExpression(node),
                Discriminators.Expression.Loop => VisitLoopExpression(node),
                Discriminators.Expression.Switch => VisitSwitchExpression(node),
                Discriminators.Expression.Try => VisitTryExpression(node),
                Discriminators.Expression.Assign => VisitBinaryExpression(node, ExpressionType.Assign),
                Discriminators.Expression.AddAssign => VisitBinaryOpAssignExpression(node, ExpressionType.AddAssign),
                Discriminators.Expression.AddAssignChecked => VisitBinaryOpAssignExpression(node, ExpressionType.AddAssignChecked),
                Discriminators.Expression.AndAssign => VisitBinaryOpAssignExpression(node, ExpressionType.AndAssign),
                Discriminators.Expression.DivideAssign => VisitBinaryOpAssignExpression(node, ExpressionType.DivideAssign),
                Discriminators.Expression.ExclusiveOrAssign => VisitBinaryOpAssignExpression(node, ExpressionType.ExclusiveOrAssign),
                Discriminators.Expression.LeftShiftAssign => VisitBinaryOpAssignExpression(node, ExpressionType.LeftShiftAssign),
                Discriminators.Expression.ModuloAssign => VisitBinaryOpAssignExpression(node, ExpressionType.ModuloAssign),
                Discriminators.Expression.MultiplyAssign => VisitBinaryOpAssignExpression(node, ExpressionType.MultiplyAssign),
                Discriminators.Expression.MultiplyAssignChecked => VisitBinaryOpAssignExpression(node, ExpressionType.MultiplyAssignChecked),
                Discriminators.Expression.OrAssign => VisitBinaryOpAssignExpression(node, ExpressionType.OrAssign),
                Discriminators.Expression.PowerAssign => VisitBinaryOpAssignExpression(node, ExpressionType.PowerAssign),
                Discriminators.Expression.RightShiftAssign => VisitBinaryOpAssignExpression(node, ExpressionType.RightShiftAssign),
                Discriminators.Expression.SubtractAssign => VisitBinaryOpAssignExpression(node, ExpressionType.SubtractAssign),
                Discriminators.Expression.SubtractAssignChecked => VisitBinaryOpAssignExpression(node, ExpressionType.SubtractAssignChecked),
                Discriminators.Expression.PostDecrementAssign => VisitUnaryExpression(node, ExpressionType.PostDecrementAssign),
                Discriminators.Expression.PostIncrementAssign => VisitUnaryExpression(node, ExpressionType.PostIncrementAssign),
                Discriminators.Expression.PreDecrementAssign => VisitUnaryExpression(node, ExpressionType.PreDecrementAssign),
                Discriminators.Expression.PreIncrementAssign => VisitUnaryExpression(node, ExpressionType.PreIncrementAssign),
                Discriminators.Expression.Throw => VisitThrowExpression(node),
                Discriminators.Expression.Unbox => VisitUnboxExpression(node),
                Discriminators.Expression.IsFalse => VisitUnaryExpression(node, ExpressionType.IsFalse),
                Discriminators.Expression.IsTrue => VisitUnaryExpression(node, ExpressionType.IsTrue),
                _ => throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected expression type discriminator '{0}'.", nodeType), node),
            };
        }

        protected virtual Expression VisitArrayIndexExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for an expression of type 'ArrayIndex'.", node);

            var array = Visit(node.GetElement(1));
            var index = Visit(node.GetElement(2));
            return Expression.ArrayIndex(array, index);
        }

        protected virtual Expression VisitBinaryExpression(Json.ArrayExpression node, ExpressionType nodeType)
        {
            Expression lhs;
            Expression rhs;
            MethodInfo mtd;

            var n = node.ElementCount;
            if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);

                lhs = Visit(first);
                rhs = Visit(second);
                mtd = null;
            }
            else if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                lhs = Visit(first);
                rhs = Visit(second);
                mtd = GetMethod(third);
            }
            else
                throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Expected 3 or 4 JSON array elements for an expression of type '{0}'.", nodeType), node);

            return nodeType switch
            {
                ExpressionType.Multiply => Expression.Multiply(lhs, rhs, mtd),
                ExpressionType.MultiplyChecked => Expression.MultiplyChecked(lhs, rhs, mtd),
                ExpressionType.Divide => Expression.Divide(lhs, rhs, mtd),
                ExpressionType.Modulo => Expression.Modulo(lhs, rhs, mtd),
                ExpressionType.Power => Expression.Power(lhs, rhs, mtd),
                ExpressionType.LeftShift => Expression.LeftShift(lhs, rhs, mtd),
                ExpressionType.RightShift => Expression.RightShift(lhs, rhs, mtd),
                ExpressionType.And => Expression.And(lhs, rhs, mtd),
                ExpressionType.AndAlso => Expression.AndAlso(lhs, rhs, mtd),
                ExpressionType.Or => Expression.Or(lhs, rhs, mtd),
                ExpressionType.OrElse => Expression.OrElse(lhs, rhs, mtd),
                ExpressionType.ExclusiveOr => Expression.ExclusiveOr(lhs, rhs, mtd),
                ExpressionType.Assign => Expression.Assign(lhs, rhs),
                _ => throw new InvalidOperationException("Unexpected binary expression node."),
            };
        }

        protected virtual Expression VisitBinaryOpAssignExpression(Json.ArrayExpression node, ExpressionType nodeType)
        {
            Expression lhs;
            Expression rhs;
            MethodInfo mtd;
            LambdaExpression con;

            var n = node.ElementCount;
            if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);

                lhs = Visit(first);
                rhs = Visit(second);
                mtd = null;
                con = null;
            }
            else if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                lhs = Visit(first);
                rhs = Visit(second);
                mtd = GetMethod(third);
                con = null;
            }
            else if (n == 5)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                lhs = Visit(first);
                rhs = Visit(second);
                mtd = third.NodeType != Json.ExpressionType.Null ? GetMethod(third) : null;

                if (node.GetElement(4) is not Json.ArrayExpression fourth)
                    throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON array in 'node[4]' for the Conversion property of an expression of type '{0}'.", nodeType), node);

                con = (LambdaExpression)VisitLambdaExpression(fourth);
            }
            else
                throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Expected 3, 4, or 5 JSON array elements for an expression of type '{0}'.", nodeType), node);

            return nodeType switch
            {
                ExpressionType.AddAssign => Expression.AddAssign(lhs, rhs, mtd, con),
                ExpressionType.AddAssignChecked => Expression.AddAssignChecked(lhs, rhs, mtd, con),
                ExpressionType.AndAssign => Expression.AndAssign(lhs, rhs, mtd, con),
                ExpressionType.DivideAssign => Expression.DivideAssign(lhs, rhs, mtd, con),
                ExpressionType.ExclusiveOrAssign => Expression.ExclusiveOrAssign(rhs, lhs, mtd, con),
                ExpressionType.LeftShiftAssign => Expression.LeftShiftAssign(lhs, rhs, mtd, con),
                ExpressionType.ModuloAssign => Expression.ModuloAssign(lhs, rhs, mtd, con),
                ExpressionType.MultiplyAssign => Expression.MultiplyAssign(lhs, rhs, mtd, con),
                ExpressionType.MultiplyAssignChecked => Expression.MultiplyAssignChecked(lhs, rhs, mtd, con),
                ExpressionType.OrAssign => Expression.OrAssign(lhs, rhs, mtd, con),
                ExpressionType.PowerAssign => Expression.PowerAssign(lhs, rhs, mtd, con),
                ExpressionType.RightShiftAssign => Expression.RightShiftAssign(lhs, rhs, mtd, con),
                ExpressionType.SubtractAssign => Expression.SubtractAssign(lhs, rhs, mtd, con),
                ExpressionType.SubtractAssignChecked => Expression.SubtractAssignChecked(lhs, rhs, mtd, con),
                _ => throw new InvalidOperationException("Unexpected binary assign expression node."),
            };
        }

        protected virtual Expression VisitBinaryComparisonExpression(Json.ArrayExpression node, ExpressionType nodeType)
        {
            Expression lhs;
            Expression rhs;
            MethodInfo mtd;
            bool ltn;

            var n = node.ElementCount;
            if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);

                lhs = Visit(first);
                rhs = Visit(second);
                mtd = null;
                ltn = false;
            }
            else if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                lhs = Visit(first);
                rhs = Visit(second);
                mtd = GetMethod(third);
                ltn = false;
            }
            else if (n == 5)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);
                var fourth = node.GetElement(4);

                lhs = Visit(first);
                rhs = Visit(second);
                mtd = third.NodeType != Json.ExpressionType.Null ? GetMethod(third) : null;
                ltn = (bool)((Json.ConstantExpression)fourth).Value;
            }
            else
                throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Expected 3, 4, or 5 JSON array elements for an expression of type '{0}'.", nodeType), node);

            return nodeType switch
            {
                ExpressionType.LessThan => Expression.LessThan(lhs, rhs, ltn, mtd),
                ExpressionType.LessThanOrEqual => Expression.LessThanOrEqual(lhs, rhs, ltn, mtd),
                ExpressionType.GreaterThan => Expression.GreaterThan(lhs, rhs, ltn, mtd),
                ExpressionType.GreaterThanOrEqual => Expression.GreaterThanOrEqual(lhs, rhs, ltn, mtd),
                ExpressionType.Equal => Expression.Equal(lhs, rhs, ltn, mtd),
                ExpressionType.NotEqual => Expression.NotEqual(lhs, rhs, ltn, mtd),
                _ => throw new InvalidOperationException("Unexpected binary comparison expression node type."),
            };
        }

        protected virtual Expression VisitBlockExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 4)
                throw new BonsaiParseException("Expected 4 JSON array elements for an expression of type 'Block'.", node);

            var typ = GetType(node.GetElement(1));

            if (node.GetElement(2) is not Json.ArrayExpression varsArray)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the Variables property of an expression of type 'Block'.", node);

            if (node.GetElement(3) is not Json.ArrayExpression exprsArray)
                throw new BonsaiParseException("Expected a JSON array in 'node[3]' for the Expressions property of an expression of type 'Block'.", node);

            var @var = Push(varsArray);

            var exp = VisitElements(exprsArray);

            Pop();

            return Expression.Block(typ, @var, exp);
        }

        protected virtual Expression VisitCoalesceExpression(Json.ArrayExpression node)
        {
            ExpressionSlim lhs;
            ExpressionSlim rhs;
            LambdaExpression cnv;

            var n = node.ElementCount;
            if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);

                lhs = Visit(first);
                rhs = Visit(second);
                cnv = null;
            }
            else if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                lhs = Visit(first);
                rhs = Visit(second);
                cnv = VisitAndConvert<LambdaExpression>(third);
            }
            else
                throw new BonsaiParseException("Expected 3 or 4 JSON array elements for an expression of type 'Coalesce'.", node);

            return Expression.Coalesce(lhs, rhs, cnv);
        }

        protected virtual Expression VisitConditionalExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                var tst = Visit(first);
                var tru = Visit(second);
                var fls = Visit(third);

                return Expression.Condition(tst, tru, fls);
            }

            else if (n == 5)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);
                var fourth = node.GetElement(4);

                var tst = Visit(first);
                var tru = Visit(second);
                var fls = Visit(third);
                var typ = GetType(fourth);

                return Expression.Condition(tst, tru, fls, typ);
            }
            else
                throw new BonsaiParseException("Expected 4 or 5 JSON array elements for an expression of type 'Conditional'.", node);
        }

        protected virtual Expression VisitConstantExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for an expression of type 'Constant'.", node);

            var type = GetType(node.GetElement(2));
            var obj = VisitConstantValue(node.GetElement(1), type);
            return Expression.Constant(obj, type);
        }

        protected abstract ObjectSlim VisitConstantValue(Json.Expression value, TypeSlim type);

        protected virtual Expression VisitConvertExpression(Json.ArrayExpression node, bool isChecked)
        {
            var n = node.ElementCount;
            if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);

                var t = GetType(first);
                var e = Visit(second);

                return isChecked ? Expression.ConvertChecked(e, t) : Expression.Convert(e, t);
            }
            else if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                var t = GetType(first);
                var e = Visit(second);
                var m = GetMethod(third);

                return isChecked ? Expression.ConvertChecked(e, t, m) : Expression.Convert(e, t, m);
            }

            throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Expected 3 or 4 JSON array elements for an expression of type '{0}'.", isChecked ? "ConvertChecked" : "Convert"), node);
        }

        protected virtual Expression VisitThrowExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n == 2)
            {
                var first = node.GetElement(1);

                var o = first.NodeType != Json.ExpressionType.Null ? Visit(first) : null;

                return Expression.Throw(o);
            }
            else if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);

                var o = first.NodeType != Json.ExpressionType.Null ? Visit(first) : null;
                var t = GetType(second);

                return Expression.Throw(o, t);
            }

            throw new BonsaiParseException("Expected 2 or 3 JSON array elements for an expression of type 'Throw'.", node);
        }

        protected virtual Expression VisitUnboxExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);

                var t = GetType(first);
                var e = Visit(second);

                return Expression.Unbox(e, t);
            }

            throw new BonsaiParseException("Expected 3 JSON array elements for an expression of type 'Unbox'.", node);
        }

        protected virtual Expression VisitDefaultExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 2)
                throw new BonsaiParseException("Expected 2 JSON array elements for an expression of type 'Default'.", node);

            var type = GetType(node.GetElement(1));

            return Expression.Default(type);
        }

        protected virtual Expression VisitGotoExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n is < 3 or > 5)
                throw new BonsaiParseException("Expected 3, 4, or 5 JSON array elements for an expression of type 'Goto'.", node);

            if (node.GetElement(1) is not Json.ConstantExpression kindNode || !int.TryParse(kindNode.Value.ToString(), out int kindIndex))
                throw new BonsaiParseException("Expected a JSON number in 'node[1]' containing a valid GotoExpressionKind value for the Kind property of an expression of type 'Goto'.", node);

            var kind = (GotoExpressionKind)kindIndex;
            var target = GetLabelTarget(node.GetElement(2));
            var value = default(Expression);
            var type = default(Type);

            if (n >= 4)
            {
                var third = node.GetElement(3);
                if (third.NodeType != Json.ExpressionType.Null)
                {
                    value = Visit(third);
                }
            }

            if (n == 5)
            {
                type = GetType(node.GetElement(4));
            }

            switch (kind)
            {
                case GotoExpressionKind.Goto:
                    return Expression.Goto(target, value, type);
                case GotoExpressionKind.Return:
                    return Expression.Return(target, value, type);
                case GotoExpressionKind.Break:
                    return Expression.Break(target, value, type);
                case GotoExpressionKind.Continue:
                    if (value != default(Expression))
                        throw new BonsaiParseException("Value cannot be defined for Continue nodes.", node);

                    return Expression.Continue(target, type);
                default:
                    throw new BonsaiParseException("The JSON number in 'node[1]' contains an invalid GotoExpressionKind value for the Kind property of an expression of type 'Goto'.", node);
            }
        }

        protected virtual Expression VisitIndexExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 4)
                throw new BonsaiParseException("Expected 4 JSON array elements for an expression of type 'Index'.", node);

            var obj = Visit(node.GetElement(1));

            var idxr = GetProperty(node.GetElement(2));

            if (node.GetElement(3) is not Json.ArrayExpression argsArray)
                throw new BonsaiParseException("Expected a JSON array in 'node[3]' for the Arguments property of an expression of type 'Index'.", node);

            var args = VisitElements(argsArray);

            return Expression.MakeIndex(obj, idxr, args);
        }

        protected virtual Expression VisitInvocationExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for an expression of type 'Invoke'.", node);

            var expr = Visit(node.GetElement(1));

            if (node.GetElement(2) is not Json.ArrayExpression argsArray)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the Arguments property of an expression of type 'Invoke'.", node);

            switch (argsArray.ElementCount)
            {
                case 0:
                    return Expression.Invoke(expr);
                case 1:
                    return Expression.Invoke(expr, Visit(argsArray.GetElement(0)));
                case 2:
                    return Expression.Invoke(expr, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)));
                case 3:
                    return Expression.Invoke(expr, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)), Visit(argsArray.GetElement(2)));
                case 4:
                    return Expression.Invoke(expr, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)), Visit(argsArray.GetElement(2)), Visit(argsArray.GetElement(3)));
                case 5:
                    return Expression.Invoke(expr, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)), Visit(argsArray.GetElement(2)), Visit(argsArray.GetElement(3)), Visit(argsArray.GetElement(4)));
            }

            var args = VisitElements(argsArray);

            return Expression.Invoke(expr, args);
        }

        protected virtual Expression VisitLabelExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n is not 2 and not 3)
                throw new BonsaiParseException("Expected 2 or 3 JSON array elements for an expression of type 'Label'.", node);

            var target = GetLabelTarget(node.GetElement(1));
            var defaultValue = default(Expression);

            if (n == 3)
            {
                var second = node.GetElement(2);
                if (second.NodeType != Json.ExpressionType.Null)
                {
                    defaultValue = Visit(second);
                }
            }

            return Expression.Label(target, defaultValue);
        }

        protected virtual Expression VisitLambdaExpression(Json.ArrayExpression node)
        {
            // REVIEW: Add support for tail call and name.

            if (node.ElementCount != 4)
                throw new BonsaiParseException("Expected 4 JSON array elements for an expression of type 'Lambda'.", node);

            var par = Push((Json.ArrayExpression)node.GetElement(3));

            var typeExpr = node.GetElement(1);
            var typ = typeExpr.NodeType == Json.ExpressionType.Null ? null : GetType(typeExpr);

            var bdy = Visit(node.GetElement(2));

            Pop();

            return typ == null ? Expression.Lambda(bdy, par) : Expression.Lambda(typ, bdy, par);
        }

        protected virtual Expression VisitListInitExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for an expression of type 'ListInit'.", node);

            var newExpr = VisitAndConvert<NewExpression>(node.GetElement(1));

            if (node.GetElement(2) is not Json.ArrayExpression initializersExpr)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the Initializers property of an expression of type 'ListInit'.", node);

            var elements = VisitElements(initializersExpr, VisitElementInit);
            return Expression.ListInit(newExpr, elements);
        }

        protected virtual Expression VisitLoopExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n is not 2 and not 3 and not 4)
                throw new BonsaiParseException("Expected 2, 3, or 4 JSON array elements for an expression of type 'Loop'.", node);

            var body = Visit(node.GetElement(1));
            var breakLabel = default(LabelTarget);
            var continueLabel = default(LabelTarget);

            if (n >= 3)
            {
                var breakExpr = node.GetElement(2);

                if (breakExpr.NodeType != Json.ExpressionType.Null)
                {
                    breakLabel = GetLabelTarget(breakExpr);
                }
            }

            if (n >= 4)
            {
                var continueExpr = node.GetElement(3);

                if (continueExpr.NodeType != Json.ExpressionType.Null)
                {
                    continueLabel = GetLabelTarget(continueExpr);
                }
            }

            return Expression.Loop(body, breakLabel, continueLabel);
        }

        protected virtual Expression VisitMemberAccessExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n is not 2 and not 3)
                throw new BonsaiParseException("Expected 2 or 3 JSON array elements for an expression of type 'MemberAccess'.", node);

            var member = GetMember(node.GetElement(1));
            var obj = default(Expression);

            if (n == 3)
            {
                obj = Visit(node.GetElement(2));
            }

            return Expression.MakeMemberAccess(obj, member);
        }

        protected virtual Expression VisitMemberInitExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for an expression of type 'MemberInit'.", node);

            var newExpr = VisitAndConvert<NewExpression>(node.GetElement(1));

            if (node.GetElement(2) is not Json.ArrayExpression bindingsExpr)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the Bindings property of an expression of type 'MemberInit'.", node);

            var bindings = VisitElements(bindingsExpr, VisitMemberBinding);
            return Expression.MemberInit(newExpr, bindings);
        }

        protected virtual Expression VisitMethodCallExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n is not 3 and not 4)
                throw new BonsaiParseException("Expected 3 or 4 JSON array elements for an expression of type 'Call'.", node);

            var method = GetMethod(node.GetElement(1));

            Expression obj;
            Json.ArrayExpression argsArray;

            if (n == 3)
            {
                obj = null;
                argsArray = node.GetElement(2) as Json.ArrayExpression;
            }
            else
            {
                obj = Visit(node.GetElement(2));
                argsArray = node.GetElement(3) as Json.ArrayExpression;
            }

            if (argsArray == null)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' or 'node[3]' for the Arguments property of an expression of type 'Call'.", node);

            switch (argsArray.ElementCount)
            {
                case 0:
                    return Expression.Call(obj, method);
                case 1:
                    return Expression.Call(obj, method, Visit(argsArray.GetElement(0)));
                case 2:
                    return Expression.Call(obj, method, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)));
                case 3:
                    return Expression.Call(obj, method, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)), Visit(argsArray.GetElement(2)));
                case 4:
                    return Expression.Call(obj, method, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)), Visit(argsArray.GetElement(2)), Visit(argsArray.GetElement(3)));
                case 5:
                    return Expression.Call(obj, method, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)), Visit(argsArray.GetElement(2)), Visit(argsArray.GetElement(3)), Visit(argsArray.GetElement(4)));
            }

            var args = VisitElements(argsArray);

            return Expression.Call(obj, method, args);
        }

        protected virtual Expression VisitMinusExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n == 2)
            {
                var first = node.GetElement(1);
                var o = Visit(first);
                return Expression.Negate(o);
            }
            else if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                switch (second.NodeType)
                {
                    case Json.ExpressionType.Array:
                        var l = Visit(first);
                        var r = Visit(second);
                        return Expression.Subtract(l, r);
                    default:
                        var o = Visit(first);
                        var m = GetMethod(second);
                        return Expression.Negate(o, m);
                }
            }
            else if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                var l = Visit(first);
                var r = Visit(second);
                var m = GetMethod(third);

                return Expression.Subtract(l, r, m);
            }

            throw new BonsaiParseException("Expected 2, 3, or 4 JSON array elements for an expression of type 'Negate' or 'Subtract'.", node);
        }

        protected virtual Expression VisitMinusDollarExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n == 2)
            {
                var first = node.GetElement(1);
                var o = Visit(first);
                return Expression.NegateChecked(o);
            }
            else if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                switch (second.NodeType)
                {
                    case Json.ExpressionType.Array:
                        var l = Visit(first);
                        var r = Visit(second);
                        return Expression.SubtractChecked(l, r);
                    default:
                        var o = Visit(first);
                        var m = GetMethod(second);
                        return Expression.NegateChecked(o, m);
                }
            }
            else if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                var l = Visit(first);
                var r = Visit(second);
                var m = GetMethod(third);

                return Expression.SubtractChecked(l, r, m);
            }

            throw new BonsaiParseException("Expected 2, 3, or 4 JSON array elements for an expression of type 'NegateChecked' or 'SubtractChecked'.", node);
        }

        protected virtual Expression VisitNewArrayBoundsExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount < 2)
                throw new BonsaiParseException("Expected at least 2 JSON array elements for an expression of type 'NewArrayBounds'.", node);

            return MakeNewArray(node, Expression.NewArrayBounds);
        }

        protected virtual Expression VisitNewArrayInitExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount < 2)
                throw new BonsaiParseException("Expected at least 2 JSON array elements for an expression of type 'NewArrayInit'.", node);

            return MakeNewArray(node, Expression.NewArrayInit);
        }

        private Expression MakeNewArray(Json.ArrayExpression node, Func<Type, IEnumerable<Expression>, Expression> factory)
        {
            var elementType = GetType(node.GetElement(1));

            var n = node.ElementCount - 2;

            var args = new Expression[n];

            for (var i = 0; i < n; i++)
            {
                args[i] = Visit(node.GetElement(i + 2));
            }

            return factory(elementType, new TrueReadOnlyCollection<Expression>(/* transfer ownership */ args));
        }

        protected virtual Expression VisitNewExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n is not 2 and not 3 and not 4)
                throw new BonsaiParseException("Expected 2, 3, or 4 JSON array elements for an expression of type 'New'.", node);

            if (n == 2)
            {
                var type = GetType(node.GetElement(1));
                return Expression.New(type);
            }
            else
            {
                var constructor = GetConstructor(node.GetElement(1));

                if (node.GetElement(2) is not Json.ArrayExpression argsArray)
                    throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the Arguments property of an expression of type 'New'.", node);

                if (n == 3)
                {
                    switch (argsArray.ElementCount)
                    {
                        case 0:
                            return Expression.New(constructor);
                        case 1:
                            return Expression.New(constructor, Visit(argsArray.GetElement(0)));
                        case 2:
                            return Expression.New(constructor, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)));
                        case 3:
                            return Expression.New(constructor, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)), Visit(argsArray.GetElement(2)));
                        case 4:
                            return Expression.New(constructor, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)), Visit(argsArray.GetElement(2)), Visit(argsArray.GetElement(3)));
                        case 5:
                            return Expression.New(constructor, Visit(argsArray.GetElement(0)), Visit(argsArray.GetElement(1)), Visit(argsArray.GetElement(2)), Visit(argsArray.GetElement(3)), Visit(argsArray.GetElement(4)));
                    }

                    var args = VisitElements(argsArray);

                    return Expression.New(constructor, args);
                }
                else
                {
                    var args = VisitElements(argsArray);

                    if (node.GetElement(3) is not Json.ArrayExpression membersArray)
                        throw new BonsaiParseException("Expected a JSON array in 'node[3]' for the Members property of an expression of type 'New'.", node);

                    var members = VisitElements(membersArray, GetMember);

                    return Expression.New(constructor, args, members);
                }
            }
        }

        protected virtual Expression VisitParameterExpression(Json.ArrayExpression node)
        {
            return _state.Lookup(node);
        }

        protected virtual Expression VisitPlusExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n == 2)
            {
                var first = node.GetElement(1);
                var o = Visit(first);
                return Expression.UnaryPlus(o);
            }
            else if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                switch (second.NodeType)
                {
                    case Json.ExpressionType.Array:
                        var l = Visit(first);
                        var r = Visit(second);
                        return Expression.Add(l, r);
                    default:
                        var o = Visit(first);
                        var m = GetMethod(second);
                        return Expression.UnaryPlus(o, m);
                }
            }
            else if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                var l = Visit(first);
                var r = Visit(second);
                var m = GetMethod(third);

                return Expression.Add(l, r, m);
            }

            throw new BonsaiParseException("Expected 2, 3, or 4 JSON array elements for an expression of type 'UnaryPlus' or 'Add'.", node);
        }

        protected virtual Expression VisitPlusDollarExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);

                var l = Visit(first);
                var r = Visit(second);

                return Expression.AddChecked(l, r);
            }
            else if (n == 4)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);
                var third = node.GetElement(3);

                var l = Visit(first);
                var r = Visit(second);
                var m = GetMethod(third);

                return Expression.AddChecked(l, r, m);
            }

            throw new BonsaiParseException("Expected 2, 3, or 4 JSON array elements for an expression of type 'UnaryPlusChecked' or 'AddChecked'.", node);
        }

        protected virtual Expression VisitQuoteExpression(Json.ArrayExpression node)
        {
            var n = node.ElementCount;
            if (n == 2)
            {
                var first = node.GetElement(1);

                var o = Visit(first);

                return Expression.Quote(o);
            }

            throw new BonsaiParseException("Expected 2 JSON array elements for an expression of type 'Quote'.", node);
        }

        protected virtual Expression VisitSwitchExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 6)
                throw new BonsaiParseException("Expected 6 JSON array elements for an expression of type 'Switch'.", node);

            var switchValue = Visit(node.GetElement(1));

            if (node.GetElement(2) is not Json.ArrayExpression switchCases)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the Cases property of an expression of type 'Switch'.", node);

            var switchCasesArray = new SwitchCase[switchCases.ElementCount];
            for (int i = 0; i < switchCasesArray.Length; ++i)
            {
                if (switchCases.GetElement(i) is not Json.ArrayExpression elements)
                    throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON array in 'node[{0}]' for a 'SwitchCase' node.", i), switchCases);

                switchCasesArray[i] = VisitSwitchCaseExpression(elements);
            }

            var third = node.GetElement(3);
            var defaultBody = default(Expression);
            if (third.NodeType != Json.ExpressionType.Null)
            {
                defaultBody = Visit(third);
            }

            var fourth = node.GetElement(4);
            var comparison = default(MethodInfo);
            if (fourth.NodeType != Json.ExpressionType.Null)
            {
                comparison = GetMethod(fourth);
            }

            var fifth = node.GetElement(5);
            if (fifth.NodeType != Json.ExpressionType.Null)
            {
                var type = GetType(fifth);
                return Expression.Switch(type, switchValue, defaultBody, comparison, switchCasesArray);
            }

            return Expression.Switch(switchValue, defaultBody, comparison, switchCasesArray);
        }

        protected virtual SwitchCase VisitSwitchCaseExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 2)
                throw new BonsaiParseException("Expected 2 JSON array elements for a 'SwitchCase' node.", node);

            if (node.GetElement(0) is not Json.ArrayExpression testValues)
                throw new BonsaiParseException("Expected a JSON array in 'node[0]' for the TestValues property of a 'SwitchCase' node.", node);

            var testValuesArray = new Expression[testValues.ElementCount];
            for (int i = 0; i < testValuesArray.Length; ++i)
            {
                testValuesArray[i] = Visit(testValues.GetElement(i));
            }

            var body = Visit(node.GetElement(1));

            return Expression.SwitchCase(body, testValuesArray);
        }

        protected virtual Expression VisitTryExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 6)
                throw new BonsaiParseException("Expected 6 JSON array elements for an expression of type 'Try'.", node);

            var body = Visit(node.GetElement(1));

            // REVIEW: Consider allowing a `null` expression here as well.

            if (node.GetElement(2) is not Json.ArrayExpression handlers)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the Handlers property of an expression of type 'Try'.", node);

            var handlerCount = handlers.ElementCount;

            var handlerListReadOnly = default(ReadOnlyCollection<CatchBlock>);

            if (handlerCount > 0)
            {
                var handlerList = new CatchBlock[handlerCount];

                for (var i = 0; i < handlerCount; i++)
                {
                    if (handlers.GetElement(i) is not Json.ArrayExpression handler)
                        throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON array in 'node[{0}]' for a 'CatchBlock' node.", i), handlers);

                    var catchBlock = VisitCatchBlockExpression(handler);
                    handlerList[i] = catchBlock;
                }

                handlerListReadOnly = new TrueReadOnlyCollection<CatchBlock>(handlerList /* transfer ownership */);
            }

            var finallyExpr = node.GetElement(3);
            var @finally = default(Expression);
            if (finallyExpr.NodeType != Json.ExpressionType.Null)
            {
                @finally = Visit(finallyExpr);
            }

            var faultExpr = node.GetElement(4);
            var fault = default(Expression);
            if (faultExpr.NodeType != Json.ExpressionType.Null)
            {
                fault = Visit(faultExpr);
            }

            var type = GetType(node.GetElement(5));

            return Expression.MakeTry(type, body, @finally, fault, handlerListReadOnly);
        }

        protected virtual CatchBlock VisitCatchBlockExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 4)
                throw new BonsaiParseException("Expected 4 JSON array elements for a 'CatchBlock' node.", node);

            var type = GetType(node.GetElement(3));

            var first = node.GetElement(0);

            var variable = default(ParameterExpression);
            if (first.NodeType != Json.ExpressionType.Null)
            {
                variable = Push(Json.Expression.Array(first)).First();
            }

            var body = Visit(node.GetElement(1));

            var third = node.GetElement(2);

            var filter = default(Expression);
            if (third.NodeType != Json.ExpressionType.Null)
            {
                filter = Visit(third);
            }

            if (first.NodeType != Json.ExpressionType.Null)
            {
                Pop();
            }

            return Expression.MakeCatchBlock(type, variable, body, filter);
        }

        protected virtual Expression VisitTypeAsExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for an expression of type 'TypeAs'.", node);

            var type = GetType(node.GetElement(1));
            var operand = Visit(node.GetElement(2));

            return Expression.TypeAs(operand, type);
        }

        protected virtual Expression VisitTypeIsExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for an expression of type 'TypeIs'.", node);

            var type = GetType(node.GetElement(1));
            var operand = Visit(node.GetElement(2));

            return Expression.TypeIs(operand, type);
        }

        protected virtual Expression VisitTypeEqualExpression(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for an expression of type 'TypeEqual'.", node);

            var type = GetType(node.GetElement(1));
            var operand = Visit(node.GetElement(2));

            return Expression.TypeEqual(operand, type);
        }

        protected virtual Expression VisitUnaryExpression(Json.ArrayExpression node, ExpressionType nodeType)
        {
            Expression o;
            MethodInfo m;

            var n = node.ElementCount;
            if (n == 2)
            {
                var first = node.GetElement(1);

                o = Visit(first);
                m = null;
            }
            else if (n == 3)
            {
                var first = node.GetElement(1);
                var second = node.GetElement(2);

                o = Visit(first);
                m = GetMethod(second);
            }
            else
                throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Expected 2 or 3 JSON array elements for an expression of type '{0}'.", nodeType), node);

            return nodeType switch
            {
                ExpressionType.Not => Expression.Not(o, m),
                ExpressionType.OnesComplement => Expression.OnesComplement(o, m),
                ExpressionType.Decrement => Expression.Decrement(o, m),
                ExpressionType.Increment => Expression.Increment(o, m),
                ExpressionType.ArrayLength => Expression.ArrayLength(o),
                ExpressionType.PostDecrementAssign => Expression.PostDecrementAssign(o, m),
                ExpressionType.PostIncrementAssign => Expression.PostIncrementAssign(o, m),
                ExpressionType.PreDecrementAssign => Expression.PreDecrementAssign(o, m),
                ExpressionType.PreIncrementAssign => Expression.PreIncrementAssign(o, m),
                ExpressionType.IsFalse => Expression.IsFalse(o, m),
                ExpressionType.IsTrue => Expression.IsTrue(o, m),
                _ => throw new InvalidOperationException("Unexpected unary expression node."),
            };
        }

        public override Expression VisitConstant(Json.ConstantExpression node)
        {
            throw new BonsaiParseException("Expected a JSON array for the Bonsai representation of an expression tree node.", node);
        }

        public override Expression VisitObject(Json.ObjectExpression node)
        {
            throw new BonsaiParseException("Expected a JSON array for the Bonsai representation of an expression tree node.", node);
        }

        #endregion

        #region Helpers

        #region Visitor Helpers

        public TResult VisitAndConvert<TResult>(Json.Expression node)
            where TResult : Expression
        {
            return (TResult)Visit(node);
        }

        private ReadOnlyCollection<Expression> VisitElements(Json.ArrayExpression array)
        {
            var n = array.ElementCount;

            if (n == 0)
            {
                return s_empty;
            }

            var res = new Expression[n];

            for (var i = 0; i < n; i++)
            {
                var expression = array.GetElement(i);
                res[i] = Visit(expression);
            }

            return new TrueReadOnlyCollection<Expression>(res /* transfer ownership */);
        }

        private static ReadOnlyCollection<R> VisitElements<R>(Json.ArrayExpression array, Func<Json.Expression, R> visit)
        {
            var n = array.ElementCount;

            if (n == 0)
            {
                return EmptyReadOnlyCollection<R>.Instance;
            }

            var res = new R[n];

            for (var i = 0; i < n; i++)
            {
                var expression = array.GetElement(i);
                res[i] = visit(expression);
            }

            return new TrueReadOnlyCollection<R>(/* transfer ownership */ res);
        }

        #endregion

        #region MemberBinding Visitors

        protected virtual MemberBinding VisitMemberBinding(Json.Expression node)
        {
            if (node is not Json.ArrayExpression array)
                throw new BonsaiParseException("Expected a JSON array for the Bonsai representation of a 'MemberBinding' node.", node);

            var n = array.ElementCount;
            if (n == 0)
                throw new BonsaiParseException("Expected at least one JSON array element containing a discriminator for a 'MemberBinding' node.", node);

            var type = array.GetElement(0);
            if (type.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[0]' for the 'MemberBinding' discriminator.", node);

            var nodeType = (string)((Json.ConstantExpression)type).Value;
            return nodeType switch
            {
                Discriminators.MemberBinding.MemberAssignment => VisitMemberAssignment(array),
                Discriminators.MemberBinding.MemberMemberBinding => VisitMemberMemberBinding(array),
                Discriminators.MemberBinding.MemberListBinding => VisitMemberListBinding(array),
                _ => throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected member binding node discriminator '{0}'.", nodeType), node),
            };
        }

        protected virtual MemberBinding VisitMemberAssignment(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for a 'MemberAssignment' node.", node);

            var member = GetMember(node.GetElement(1));
            var expr = Visit(node.GetElement(2));

            return Expression.Bind(member, expr);
        }

        protected virtual MemberBinding VisitMemberListBinding(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for a 'ListBinding' node.", node);

            if (node.GetElement(2) is not Json.ArrayExpression elements)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the Initializers property of a 'ListBinding' node.", node);

            var member = GetMember(node.GetElement(1));
            var elementInits = VisitElements(elements, VisitElementInit);

            return Expression.ListBind(member, elementInits);
        }

        protected virtual MemberBinding VisitMemberMemberBinding(Json.ArrayExpression node)
        {
            if (node.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for a 'MemberBinding' node.", node);

            if (node.GetElement(2) is not Json.ArrayExpression members)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the Bindings property of a 'MemberBinding' node.", node);

            var member = GetMember(node.GetElement(1));
            var bindings = VisitElements(members, VisitMemberBinding);

            return Expression.MemberBind(member, bindings);
        }

        #endregion

        #region ElementInit Visitors

        protected virtual ElementInit VisitElementInit(Json.Expression node)
        {
            if (node is not Json.ArrayExpression array)
                throw new BonsaiParseException("Expected a JSON array for the Bonsai representation of an 'ElementInit' node.", node);

            var n = array.ElementCount;
            if (n == 0)
                throw new BonsaiParseException("Expected at least one JSON array element containing the AddMethod property for an 'ElementInit' node.", node);

            var method = GetMethod(array.GetElement(0));

            n--;

            var args = new Expression[n];

            for (var i = 0; i < n; i++)
            {
                args[i] = Visit(array.GetElement(i + 1));
            }

            var arguments = new TrueReadOnlyCollection<Expression>(/* transfer ownership */ args);

            return Expression.ElementInit(method, arguments);
        }

        #endregion

        #region State Helpers

        private MemberInfo GetMember(Json.Expression node)
        {
            return _state.GetMember(node);
        }

        private ConstructorInfo GetConstructor(Json.Expression node)
        {
            return (ConstructorInfo)GetMember(node);
        }

        private MethodInfo GetMethod(Json.Expression node)
        {
            return (MethodInfo)GetMember(node);
        }

        private PropertyInfo GetProperty(Json.Expression node)
        {
            return (PropertyInfo)GetMember(node);
        }

        private Type GetType(Json.Expression node)
        {
            return _state.GetType(node);
        }

        private LabelTarget GetLabelTarget(Json.Expression node)
        {
            return _state.GetLabelTarget(node);
        }

        private IEnumerable<ParameterExpression> Push(Json.ArrayExpression parameters)
        {
            return _state.Push(parameters);
        }

        private void Pop()
        {
            _state.Pop();
        }

        #endregion

        #endregion
    }
}
