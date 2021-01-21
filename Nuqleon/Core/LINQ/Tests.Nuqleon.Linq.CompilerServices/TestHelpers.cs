// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Linq;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    public static class TestHelpers
    {
        public static Expression Clone(this Expression expression)
        {
            var cloner = new ExpressionCloner();
            return cloner.Visit(expression);
        }

        private class ExpressionCloner : ScopedExpressionVisitor<ParameterExpression>
        {
            protected override Expression VisitBinary(BinaryExpression node)
            {
                return Expression.MakeBinary(node.NodeType, Visit(node.Left), Visit(node.Right), node.IsLiftedToNull, node.Method, VisitAndConvert<LambdaExpression>(node.Conversion, "VisitBinary"));
            }

            protected override Expression VisitBlockCore(BlockExpression node) => throw new NotImplementedException();

            protected override CatchBlock VisitCatchBlockCore(CatchBlock node) => throw new NotImplementedException();

            protected override Expression VisitConditional(ConditionalExpression node)
            {
                return Expression.Condition(Visit(node.Test), Visit(node.IfTrue), Visit(node.IfFalse), node.Type);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                return Expression.Constant(node.Value, node.Type);
            }

            protected override Expression VisitDebugInfo(DebugInfoExpression node) => throw new NotImplementedException();

            protected override Expression VisitDefault(DefaultExpression node)
            {
                return Expression.Default(node.Type);
            }

            protected override Expression VisitDynamic(DynamicExpression node) => throw new NotImplementedException();

            protected override ElementInit VisitElementInit(ElementInit node)
            {
                return Expression.ElementInit(node.AddMethod, Visit(node.Arguments));
            }

            protected override Expression VisitExtension(Expression node) => throw new NotImplementedException();

            protected override Expression VisitGoto(GotoExpression node) => throw new NotImplementedException();

            protected override Expression VisitIndex(IndexExpression node)
            {
                return Expression.MakeIndex(Visit(node.Object), node.Indexer, Visit(node.Arguments));
            }

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                return Expression.Invoke(Visit(node.Expression), Visit(node.Arguments));
            }

            protected override Expression VisitLabel(LabelExpression node) => throw new NotImplementedException();

            protected override LabelTarget VisitLabelTarget(LabelTarget node) => throw new NotImplementedException();

            protected override Expression VisitLambdaCore<T>(Expression<T> node)
            {
                return Expression.Lambda<T>(Visit(node.Body), node.Name, node.TailCall, VisitAndConvert<ParameterExpression>(node.Parameters, "VisitLambda"));
            }

            protected override Expression VisitListInit(ListInitExpression node)
            {
                return Expression.ListInit(VisitAndConvert<NewExpression>(node.NewExpression, "VisitListInit"), node.Initializers.Select(VisitElementInit));
            }

            protected override Expression VisitLoop(LoopExpression node) => throw new NotImplementedException();

            protected override Expression VisitMember(MemberExpression node)
            {
                return Expression.MakeMemberAccess(Visit(node.Expression), node.Member);
            }

            protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
            {
                return Expression.Bind(node.Member, Visit(node.Expression));
            }

            protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
            {
                return Expression.MemberBind(node.Member, node.Bindings.Select(VisitMemberBinding));
            }

            protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
            {
                return Expression.ListBind(node.Member, node.Initializers.Select(VisitElementInit));
            }

            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                return Expression.MemberInit(VisitAndConvert(node.NewExpression, "VisitMemberInit"), node.Bindings.Select(VisitMemberBinding));
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                return Expression.Call(Visit(node.Object), node.Method, Visit(node.Arguments));
            }

            protected override Expression VisitNew(NewExpression node)
            {
                if (node.Constructor == null)
                {
                    return Expression.New(node.Type);
                }
                else if (node.Members != null)
                {
                    return Expression.New(node.Constructor, Visit(node.Arguments), node.Members);
                }
                else
                {
                    return Expression.New(node.Constructor, Visit(node.Arguments));
                }
            }

            protected override Expression VisitNewArray(NewArrayExpression node)
            {
                return node.NodeType switch
                {
                    ExpressionType.NewArrayBounds => Expression.NewArrayBounds(node.Type.GetElementType(), Visit(node.Expressions)),
                    ExpressionType.NewArrayInit => Expression.NewArrayInit(node.Type.GetElementType(), Visit(node.Expressions)),
                    _ => throw new NotImplementedException(),
                };
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (TryLookup(node, out var res))
                {
                    return res;
                }
                else
                {
                    return Expression.Parameter(node.Type, node.Name);
                }
            }

            protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node) => throw new NotImplementedException();

            protected override Expression VisitSwitch(SwitchExpression node) => throw new NotImplementedException();

            protected override SwitchCase VisitSwitchCase(SwitchCase node) => throw new NotImplementedException();

            protected override Expression VisitTry(TryExpression node) => throw new NotImplementedException();

            protected override Expression VisitTypeBinary(TypeBinaryExpression node)
            {
                return node.NodeType switch
                {
                    ExpressionType.TypeEqual => Expression.TypeEqual(Visit(node.Expression), node.TypeOperand),
                    ExpressionType.TypeIs => Expression.TypeIs(Visit(node.Expression), node.TypeOperand),
                    _ => throw new NotImplementedException(),
                };
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                return Expression.MakeUnary(node.NodeType, Visit(node.Operand), node.Type, node.Method);
            }

            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;
        }
    }
}
