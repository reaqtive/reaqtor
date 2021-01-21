// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Base class for partial expression evaluators making evaluation decisions based on types and members occurring in an expression tree.
    /// </summary>
    public abstract class PartialExpressionEvaluatorBase
    {
        /// <summary>
        /// Reduces the specified expression by applying partial evaluation rules.
        /// </summary>
        /// <param name="expression">Expression to apply partial evaluation rules against.</param>
        /// <returns>Resulting expression tree where partially evaluated subtrees in the specified expressions have been replaced for constant expressions.</returns>
        public Expression Reduce(Expression expression)
        {
            var i = new Impl(this);

            var res = i.Visit(expression);

            var s = i._tilingState.Pop();
            Debug.Assert(i._tilingState.Count == 0);

            if (s)
            {
                res = Evaluate(res);
            }

            return res;
        }

        /// <summary>
        /// Checks whether the specified constant expression node can be evaluated. This method provides the base case for evaluating whether a subtree can be evaluated.
        /// Implementations can make a decision based on the node type but also based on the value, e.g. if the resulting value has to be serializable.
        /// Typically, if the source tree already has constants that obey to required invariants, an implementation can simply return true.
        /// </summary>
        /// <param name="node">Constant expression node to check.</param>
        /// <returns>true if the node can be evaluated; otherwise, false.</returns>
        protected abstract bool CanEvaluate(ConstantExpression node);

        /// <summary>
        /// Checks whether an expression tree node of the specified type can be evaluated.
        /// For example, an implementation can decide to reject evaluation of a node based on serialization requirements for the evaluated constants.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if an expression tree node of the specified type can be evaluated; otherwise, false.</returns>
        protected abstract bool CanEvaluate(Type type);

        /// <summary>
        /// Checks whether an expression tree node using the specified method can be evaluated.
        /// For example, an implementation can decide to reject evaluation of a node based on side-effects occurring from invoking the specified method.
        /// </summary>
        /// <param name="method">Method to check.</param>
        /// <returns>true if an expression tree node using the specified method can be evaluated; otherwise, false.</returns>
        protected abstract bool CanEvaluate(MethodInfo method);

        /// <summary>
        /// Checks whether an expression tree node using the specified constructor can be evaluated.
        /// For example, an implementation can decide to reject evaluation of a node based on side-effects occurring from invoking the specified constructor.
        /// </summary>
        /// <param name="constructor">Constructor to check.</param>
        /// <returns>true if an expression tree node using the specified constructor can be evaluated; otherwise, false.</returns>
        protected abstract bool CanEvaluate(ConstructorInfo constructor);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Reserved language keyword 'property'.

        /// <summary>
        /// Checks whether an expression tree node using the specified property can be evaluated.
        /// For example, an implementation can decide to reject evaluation of a node based on side-effects occurring from accessing the specified property.
        /// </summary>
        /// <param name="property">Property to check.</param>
        /// <returns>true if an expression tree node using the specified property can be evaluated; otherwise, false.</returns>
        protected abstract bool CanEvaluate(PropertyInfo property);

#pragma warning restore CA1716
#pragma warning restore IDE0079

        /// <summary>
        /// Checks whether an expression tree node using the specified field can be evaluated.
        /// For example, an implementation can decide to reject evaluation of a node based on whether the field contains a literal value.
        /// </summary>
        /// <param name="field">Field to check.</param>
        /// <returns>true if an expression tree node using the specified field can be evaluated; otherwise, false.</returns>
        protected abstract bool CanEvaluate(FieldInfo field);

        /// <summary>
        /// Evaluates the specified expression.
        /// </summary>
        /// <param name="expression">Expression to evaluate.</param>
        /// <returns>Expression containing the evaluation result.</returns>
        protected virtual Expression Evaluate(Expression expression)
        {
            // NOTE: Scope analysis could be improved by tracking use sites of variables on a lookaside stack.
            //       However, in typical usage patterns the number of subtrees that can be partially evaluated
            //       is expected to be low, so the number of times free variables have to be scanned is going
            //       to be relatively small.
            if (expression != null && expression.NodeType != ExpressionType.Constant && !FreeVariableScanner.HasFreeVariables(expression))
            {
                return EvaluateCore(expression);
            }

            return expression;
        }

        /// <summary>
        /// Evaluates the specified expression.
        /// Implementations can return any expression that has a result equivalent to the specified expression.
        /// Implementations can return the original expression if evaluation is not possible (e.g. because an exception occurs).
        /// </summary>
        /// <param name="expression">Expression to evaluate.</param>
        /// <returns>Expression containing the evaluation result.</returns>
        protected abstract Expression EvaluateCore(Expression expression);

        private sealed class Impl : ExpressionVisitor
        {
            private readonly PartialExpressionEvaluatorBase _parent;
            private readonly MemberBindingEvaluator _evalBindings;
            private readonly Stack<IList<ParameterExpression>> _environment;

            public readonly Stack<bool> _tilingState;

            public Impl(PartialExpressionEvaluatorBase parent)
            {
                _parent = parent;
                _evalBindings = new MemberBindingEvaluator(this);
                _environment = new Stack<IList<ParameterExpression>>();

                _tilingState = new Stack<bool>();
            }

            public override Expression Visit(Expression node)
            {
                if (node == null)
                {
                    _tilingState.Push(true);
                    return node;
                }

                return base.Visit(node);
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                var l = Visit(node.Left);
                var c = VisitAndConvert(node.Conversion, nameof(VisitBinary));
                var r = Visit(node.Right);

                var rs = _tilingState.Pop();
                var cs = PopIfNotNull(node.Conversion);
                var ls = _tilingState.Pop();

                if (All(ls, cs, rs) && CanEvaluate(node.Method))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(ls, ref l);
                    // can't do c; wouldn't come back as a lambda expression
                    EvaluateIf(rs, ref r);
                }

                return node.Update(l, c, r);
            }

            protected override Expression VisitConditional(ConditionalExpression node)
            {
                var c = Visit(node.Test);
                var t = Visit(node.IfTrue);
                var f = Visit(node.IfFalse);

                var fs = _tilingState.Pop();
                var ts = _tilingState.Pop();
                var cs = _tilingState.Pop();

                if (All(cs, ts, fs))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(cs, ref c);
                    EvaluateIf(ts, ref t);
                    EvaluateIf(fs, ref f);
                }

                return node.Update(c, t, f);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                _tilingState.Push(_parent.CanEvaluate(node));

                return node;
            }

            protected override Expression VisitDefault(DefaultExpression node)
            {
                _tilingState.Push(true);

                return node;
            }

            protected override ElementInit VisitElementInit(ElementInit node)
            {
                var e = Visit(node.Arguments);

                var args = (IList<Expression>)e;

                var n = node.Arguments.Count;

                if (Pop(n, out var ess) && CanEvaluate(node.AddMethod))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(ess, ref args);
                }

                return node.Update(args);
            }

            protected override Expression VisitIndex(IndexExpression node)
            {
                var o = Visit(node.Object);

                var e = Visit(node.Arguments);

                var args = (IList<Expression>)e;

                var n = node.Arguments.Count;

                var all = Pop(n, out var ess);

                var os = _tilingState.Pop();
                all &= os;

                if (all && CanEvaluate(node.Indexer))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(os, ref o);
                    EvaluateIf(ess, ref args);
                }

                return node.Update(o, args);
            }

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                var o = Visit(node.Expression);

                var e = Visit(node.Arguments);

                var args = (IList<Expression>)e;

                var n = node.Arguments.Count;

                var all = Pop(n, out var ess);

                var os = _tilingState.Pop();
                all &= os;

                if (all)
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(os, ref o);
                    EvaluateIf(ess, ref args);
                }

                return node.Update(o, args);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                _environment.Push(node.Parameters);
                try
                {
                    var b = Visit(node.Body);
                    var p = VisitAndConvert(node.Parameters, nameof(VisitLambda));

                    var n = node.Parameters.Count;

                    for (var i = 0; i < n; i++)
                    {
                        _tilingState.Pop();
                    }

                    var bs = _tilingState.Pop();

                    if (bs && _parent.CanEvaluate(node.Type))
                    {
                        _tilingState.Push(true);
                    }
                    else
                    {
                        _tilingState.Push(false);

                        // no evaluation; body can contain bound parameters
                    }

                    return node.Update(b, p);
                }
                finally
                {
                    _environment.Pop();
                }
            }

            protected override Expression VisitListInit(ListInitExpression node)
            {
                var c = VisitAndConvert(node.NewExpression, nameof(VisitListInit));

                var a = Visit(node.Initializers, VisitElementInit);

                var inits = (IList<ElementInit>)a;

                var n = node.Initializers.Count;

                var all = Pop(n, out var ess);

                var cs = _tilingState.Pop();
                all &= cs;

                if (all && _parent.CanEvaluate(node.Type))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    inits = Reduce(inits, ess);
                }

                return node.Update(c, inits);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                var e = Visit(node.Expression);

                var es = _tilingState.Pop();

                if (es && CanEvaluate(node.Member))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(es, ref e);
                }

                return node.Update(e);
            }

            protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
            {
                var e = Visit(node.Expression);

                var es = _tilingState.Pop();

                if (es && CanEvaluate(node.Member)) // TODO: can't distinguish between get and set on members
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(es, ref e);
                }

                return node.Update(e);
            }

            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                var c = VisitAndConvert(node.NewExpression, nameof(VisitMemberInit));

                var a = Visit(node.Bindings, VisitMemberBinding);

                var inits = (IList<MemberBinding>)a;

                var n = node.Bindings.Count;

                var all = Pop(n, out var ess);

                var cs = _tilingState.Pop();
                all &= cs;

                if (all && _parent.CanEvaluate(node.Type))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    inits = Reduce(inits, ess);
                }

                return node.Update(c, inits);
            }

            protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
            {
                var a = Visit(node.Initializers, VisitElementInit);

                var inits = (IList<ElementInit>)a;

                var n = node.Initializers.Count;


                if (Pop(n, out var ess) && CanEvaluate(node.Member))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    inits = Reduce(inits, ess);
                }

                return node.Update(inits);
            }

            protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
            {
                var b = Visit(node.Bindings, VisitMemberBinding);

                var bindings = (IList<MemberBinding>)b;

                var n = node.Bindings.Count;


                if (Pop(n, out var ess) && CanEvaluate(node.Member))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    bindings = Reduce(bindings, ess);
                }

                return node.Update(bindings);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var o = Visit(node.Object);

                var e = Visit(node.Arguments);

                var args = (IList<Expression>)e;

                var n = node.Arguments.Count;

                var all = Pop(n, out var ess);

                var os = _tilingState.Pop();
                all &= os;

                if (all && CanEvaluate(node.Method))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(os, ref o);
                    EvaluateIf(ess, ref args);
                }

                return node.Update(o, args);
            }

            protected override Expression VisitNew(NewExpression node)
            {
                var e = Visit(node.Arguments);

                var args = (IList<Expression>)e;

                var n = node.Arguments.Count;


                if (Pop(n, out var ess) && CanEvaluate(node.Constructor))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(ess, ref args);
                }

                return node.Update(args);
            }

            protected override Expression VisitNewArray(NewArrayExpression node)
            {
                var e = Visit(node.Expressions);

                var args = (IList<Expression>)e;

                var n = node.Expressions.Count;


                if (Pop(n, out var ess) && _parent.CanEvaluate(node.Type))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(ess, ref args);
                }

                return node.Update(args);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                var bound = false;

                foreach (var frame in _environment)
                {
                    if (frame.IndexOf(node) >= 0)
                    {
                        bound = true;
                        break;
                    }
                }

                _tilingState.Push(bound);

                return node;
            }

            protected override Expression VisitTypeBinary(TypeBinaryExpression node)
            {
                var e = Visit(node.Expression);

                var es = _tilingState.Pop();

                if (es)
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(es, ref e);
                }

                return node.Update(e);
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                var o = Visit(node.Operand);

                var os = _tilingState.Pop();

                if (os && CanEvaluate(node.Method))
                {
                    _tilingState.Push(true);
                }
                else
                {
                    _tilingState.Push(false);

                    EvaluateIf(os, ref o);
                }

                return node.Update(o);
            }

            private static bool All(bool b1 = true, bool b2 = true, bool b3 = true)
            {
                return b1 && b2 && b3;
            }

            private void EvaluateIf(bool b, ref Expression e)
            {
                if (b && e != null && _parent.CanEvaluate(e.Type))
                {
                    e = _parent.Evaluate(e);
                }
            }

            private void EvaluateIf(bool[] bs, ref IList<Expression> expressions)
            {
                var n = expressions.Count;

                var res = new Expression[n];

                for (var i = 0; i < n; i++)
                {
                    var e = expressions[i];
                    EvaluateIf(bs[i], ref e);
                    res[i] = e;
                }

                expressions = res;
            }

            private bool CanEvaluate(MethodInfo method)
            {
                if (method == null)
                {
                    return true;
                }

                return _parent.CanEvaluate(method);
            }

            private bool CanEvaluate(ConstructorInfo constructor)
            {
                if (constructor == null)
                {
                    return true;
                }

                return _parent.CanEvaluate(constructor);
            }

            private bool CanEvaluate(MemberInfo member)
            {
                Debug.Assert(member != null);

                if (member.MemberType == MemberTypes.Property)
                {
                    return _parent.CanEvaluate((PropertyInfo)member);
                }
                else
                {
                    return _parent.CanEvaluate((FieldInfo)member);
                }
            }

            private bool PopIfNotNull(Expression e)
            {
                if (e == null)
                {
                    return true;
                }
                else
                {
                    return _tilingState.Pop();
                }
            }

            private bool Pop(int n, out bool[] ess)
            {
                var all = true;
                ess = new bool[n];

                for (var i = n - 1; i >= 0; i--)
                {
                    var es = _tilingState.Pop();
                    ess[i] = es;
                    all &= es;
                }

                return all;
            }

            private IList<ElementInit> Reduce(IList<ElementInit> inits, IList<bool> canReduce)
            {
                var n = inits.Count;

                var res = new ElementInit[n];

                for (var i = 0; i < n; i++)
                {
                    var r = inits[i];

                    if (canReduce[i])
                    {
                        var m = r.Arguments.Count;

                        var args = new Expression[m];

                        for (var j = 0; j < m; j++)
                        {
                            args[j] = _parent.Evaluate(r.Arguments[j]);
                        }

                        r = r.Update(args);
                    }

                    res[i] = r;
                }

                return res;
            }

            private IList<MemberBinding> Reduce(IList<MemberBinding> bindings, IList<bool> canReduce)
            {
                var n = bindings.Count;

                var res = new MemberBinding[n];

                for (var i = 0; i < n; i++)
                {
                    var r = bindings[i];

                    if (canReduce[i])
                    {
                        r = _evalBindings.Visit(r);
                    }

                    res[i] = r;
                }

                return res;
            }

            private sealed class MemberBindingEvaluator : ExpressionVisitor
            {
                private readonly Impl _parent;

                public MemberBindingEvaluator(Impl parent)
                {
                    _parent = parent;
                }

                public MemberBinding Visit(MemberBinding node)
                {
                    return VisitMemberBinding(node);
                }

                public override Expression Visit(Expression node)
                {
                    // Top-down strategy
                    return _parent._parent.Evaluate(node);
                }
            }

            #region Not supported

            protected override Expression VisitBlock(BlockExpression node)
            {
                throw new NotSupportedException();
            }

            protected override Expression VisitDebugInfo(DebugInfoExpression node)
            {
                throw new NotSupportedException();
            }

            protected override Expression VisitDynamic(DynamicExpression node)
            {
                throw new NotSupportedException();
            }

            protected override Expression VisitExtension(Expression node)
            {
                throw new NotSupportedException();
            }

            protected override Expression VisitGoto(GotoExpression node)
            {
                throw new NotSupportedException();
            }

            protected override Expression VisitLabel(LabelExpression node)
            {
                throw new NotSupportedException();
            }

            protected override Expression VisitLoop(LoopExpression node)
            {
                throw new NotSupportedException();
            }

            protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
            {
                throw new NotSupportedException();
            }

            protected override Expression VisitSwitch(SwitchExpression node)
            {
                throw new NotSupportedException();
            }

            protected override Expression VisitTry(TryExpression node)
            {
                throw new NotSupportedException();
            }

            #endregion
        }
    }
}
