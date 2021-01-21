// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionDebugger
{
    public class ExpressionInstrumenter
    {
        public Expression Instrument(Expression expression)
        {
            var impl = new Impl(this);
            return impl.Visit(expression);
        }

        protected virtual bool ShouldInstrument(Expression expression, out InstrumentationKind kind)
        {
            kind = InstrumentationKind.Block;
            return true;
        }

        protected virtual Expression InstrumentCore(Expression expression, InstrumentationKind kind)
        {
            var exit = default(Expression);
            if (TryGetEnterExpression(expression, out var enter) || TryGetExitExpression(expression, out exit))
            {
                switch (kind)
                {
                    case InstrumentationKind.Block:
                        {
                            if (expression.Type != typeof(void))
                            {
                                var t = Expression.Parameter(expression.Type, "__t");
                                return Expression.Block(expression.Type, new[] { t }, enter, Expression.Assign(t, expression), exit, t);
                            }
                            else
                            {
                                return Expression.Block(typeof(void), enter, expression, exit);
                            }
                        }
                    case InstrumentationKind.TryFinally:
                        {
                            return Expression.Block(expression.Type, enter, Expression.TryFinally(expression, exit));
                        }
                    case InstrumentationKind.TryCatchFinally:
                        {
                            if (TryGetCatchBlocks(expression, out var handlers) && handlers.Length > 0)
                            {
                                // NB: We require the handlers to be of the same type as the try block; but the derived class can inspect the node to figure it out.

                                return Expression.Block(expression.Type, enter, Expression.TryCatchFinally(expression, exit, handlers));
                            }
                            else
                            {
                                return Expression.Block(expression.Type, enter, Expression.TryFinally(expression, exit));
                            }
                        }
                    default:
                        throw new NotSupportedException();
                }
            }

            return expression;
        }

        protected virtual bool TryGetEnterExpression(Expression expression, out Expression enterExpression)
        {
            enterExpression = null;
            return false;
        }

        protected virtual bool TryGetExitExpression(Expression expression, out Expression exitExpression)
        {
            exitExpression = null;
            return false;
        }

        protected virtual bool TryGetCatchBlocks(Expression expression, out CatchBlock[] catchBlock)
        {
            catchBlock = null;
            return false;
        }

        private sealed class Impl : ExpressionVisitor
        {
            private readonly ExpressionInstrumenter _parent;
            private readonly LambdaDispatch _lambdaVisitor;
            private bool _inLval;

            public Impl(ExpressionInstrumenter parent)
            {
                _parent = parent;
                _lambdaVisitor = new LambdaDispatch(Visit);
            }

            public override Expression Visit(Expression node)
            {
                if (node == null)
                {
                    return null;
                }

                if (_parent.ShouldInstrument(node, out var kind))
                {
                    return _parent.InstrumentCore(node, kind);
                }

                return base.Visit(node);
            }

            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                var newArgs = VisitNewArguments(node.NewExpression);
                var bindings = Visit(node.Bindings, VisitMemberBinding);
                return node.Update(node.NewExpression.Update(newArgs), bindings);
            }

            protected override Expression VisitListInit(ListInitExpression node)
            {
                var newArgs = VisitNewArguments(node.NewExpression);
                var initializers = Visit(node.Initializers, VisitElementInit);
                return node.Update(node.NewExpression.Update(newArgs), initializers);
            }

            private IEnumerable<Expression> VisitNewArguments(NewExpression node)
            {
                if (node.Arguments.Count == 0)
                {
                    return node.Arguments;
                }
                else
                {
                    Debug.Assert(node.Constructor != null);

                    return VisitArguments(node.Constructor.GetParameters(), node.Arguments);
                }
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                var left = node.NodeType switch
                {
                    ExpressionType.Assign or
                    ExpressionType.AddAssign or
                    ExpressionType.AddAssignChecked or
                    ExpressionType.AndAssign or
                    ExpressionType.DivideAssign or
                    ExpressionType.ExclusiveOrAssign or
                    ExpressionType.LeftShiftAssign or
                    ExpressionType.ModuloAssign or
                    ExpressionType.MultiplyAssign or
                    ExpressionType.MultiplyAssignChecked or
                    ExpressionType.OrAssign or
                    ExpressionType.PowerAssign or
                    ExpressionType.RightShiftAssign or
                    ExpressionType.SubtractAssign or
                    ExpressionType.SubtractAssignChecked => VisitLVal(node.Left),
                    _ => Visit(node.Left),
                };

                var conversion = default(LambdaExpression);

                if (node.Conversion != null)
                {
                    conversion = _lambdaVisitor.VisitAndConvert(node.Conversion, nameof(VisitBinary));
                }

                var right = Visit(node.Right);

                return node.Update(left, conversion, right);
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.PostDecrementAssign:
                    case ExpressionType.PostIncrementAssign:
                    case ExpressionType.PreDecrementAssign:
                    case ExpressionType.PreIncrementAssign:
                        return node.Update(VisitLVal(node.Operand));
                }

                if (node.NodeType == ExpressionType.Quote && node.Operand.NodeType == ExpressionType.Lambda)
                {
                    var lambda = _lambdaVisitor.Visit(node.Operand);
                    return node.Update(lambda);
                }

                return base.VisitUnary(node);
            }

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                var parameters = node.Type.GetMethod("Invoke").GetParameters();

                Expression expression;

                if (node.Expression.NodeType == ExpressionType.Lambda)
                {
                    expression = _lambdaVisitor.Visit(node.Expression);
                }
                else
                {
                    expression = Visit(node.Expression);
                }

                var args = VisitArguments(parameters, node.Arguments);
                return node.Update(expression, args);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                Expression obj;

                if (_inLval && node.Method.DeclaringType.IsArray && node.Method.Name == "Get")
                {
                    // NB: This node can be used as an lval, so we make sure not to rewrite its target object as an rval.
                    //     However, it seems we can do away with this here because arrays are reference types. Can we?

                    obj = VisitLVal(node.Object);
                }
                else
                {
                    obj = Visit(node.Object);
                }

                var args = VisitArguments(node.Method.GetParameters(), node.Arguments);

                return node.Update(obj, args);
            }

            protected override Expression VisitIndex(IndexExpression node)
            {
                Expression obj;

                if (_inLval)
                {
                    obj = VisitLVal(node.Object);
                }
                else
                {
                    obj = Visit(node.Object);
                }

                var args = VisitArguments(node.Indexer.GetGetMethod(nonPublic: true).GetParameters(), node.Arguments);

                return node.Update(obj, args);
            }

            protected override Expression VisitNew(NewExpression node)
            {
                return node.Update(VisitNewArguments(node));
            }

            protected override ElementInit VisitElementInit(ElementInit node)
            {
                var args = VisitArguments(node.AddMethod.GetParameters(), node.Arguments);
                return node.Update(args);
            }

            protected override Expression VisitDynamic(DynamicExpression node)
            {
                // NB: We have to be conservative here; any argument could be passed in a ref position.

                var args = node.Arguments.Select(VisitLVal);
                return node.Update(args);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                return node.Update(Visit(node.Body), node.Parameters);
            }

            protected override Expression VisitBlock(BlockExpression node)
            {
                return node.Update(node.Variables, Visit(node.Expressions));
            }

            protected override CatchBlock VisitCatchBlock(CatchBlock node)
            {
                return node.Update(node.Variable, Visit(node.Filter), Visit(node.Body));
            }

            private Expression VisitLVal(Expression node)
            {
                var wasInLval = _inLval;

                _inLval = true;

                var res = Visit(node);

                _inLval = wasInLval;

                return res;
            }

            private IEnumerable<Expression> VisitArguments(ParameterInfo[] parameters, ReadOnlyCollection<Expression> arguments)
            {
                Debug.Assert(parameters.Length == arguments.Count);

                for (var i = 0; i < parameters.Length; i++)
                {
                    var par = parameters[i];
                    var arg = arguments[i];

                    if (par.ParameterType.IsByRef)
                    {
                        yield return VisitLVal(arg);
                    }
                    else
                    {
                        yield return Visit(arg);
                    }
                }
            }

            private sealed class LambdaDispatch : ExpressionVisitor
            {
                private readonly Func<Expression, Expression> _visit;

                public LambdaDispatch(Func<Expression, Expression> visit)
                {
                    _visit = visit;
                }

                protected override Expression VisitLambda<T>(Expression<T> node)
                {
                    return node.Update(_visit(node.Body), node.Parameters);
                }
            }
        }
    }

    public enum InstrumentationKind
    {
        Block,
        TryFinally, // NB: Can we used to build `using` semantics too.
        TryCatchFinally,
    }
}
