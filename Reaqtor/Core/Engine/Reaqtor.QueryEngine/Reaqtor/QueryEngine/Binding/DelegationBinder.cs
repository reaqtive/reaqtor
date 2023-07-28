// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Binder that supports delegation of subexpressions using <see cref="IDelegationTarget"/>.
    /// </summary>
    /// <remarks>
    /// Delegation is a concept that's bigger than just a mechanism in a local query engine. In general, delegation involves
    /// checking whether an child of a tree can "absorb" any number of parent nodes. As such, it's a generalization of things
    /// such as predicate pushdown. For example, consider a query:
    ///
    /// <c>rx://filter(xs, x => x > 0)</c>
    ///
    /// In here, `xs` may support delegation which accepts the expression applied on top of itself. If `xs` support delegation,
    /// we call it a delegation target. Delegation targets can be queried for accepting an expression where the target itself
    /// is substituted as a parameter, e.g.:
    ///
    /// <c>rx://filter(@hole, x => x > 0)</c>
    ///
    /// Given `@hole` and `rx://filter(@hole, x => x > 0)`, `xs` is asked if this expression can be handled by it. If so, the
    /// expression is delegated into `xs` and the expression is rewritten to omit the parent node(s).
    ///
    /// An example of where this can be used is to push down predicates into subjects to avoid linear evaluation of predicates
    /// on subscriptions applied to them. In such a case, the delegation target matches on filters using equality checks, e.g.
    ///
    /// * p => p.Age == 21
    /// * p => string.Equals(p.Name, "Bart")
    ///
    /// etc. The subject then internally builds up routing tables by evaluating keys (p => p.Age, p => p.Name) and matching
    /// their values against constant values that occurred in queries (21, "Bart").
    /// </remarks>
    internal sealed class DelegationBinder : QueryEngineBinder
    {
        private readonly Dictionary<Expression, IDelegationTarget> targetMapper;

        public DelegationBinder(IQueryEngineRegistry registry)
            : base(registry)
        {
            targetMapper = new Dictionary<Expression, IDelegationTarget>();
        }

        protected override Expression LookupOther(string id, Type type, Type funcType) => null;

        protected override Expression LookupSubscribable(string id, Type elementType)
        {
            if (!TryGetObservableExpression(id, out Expression result, out bool isDefinition))
            {
                return null;
            }

            if (!isDefinition)
            {
                if (result.NodeType == ExpressionType.Constant)
                {
                    var c = (ConstantExpression)result;
                    if (c.Value is IDelegationTarget dt)
                    {
                        var ret = QueryEngineBinder.ToSubscribable(id, result, elementType);
                        targetMapper[ret] = dt;
                        return ret;
                    }
                }
            }

            return null;
        }

        protected override Expression LookupObserver(string id, Type elementType)
        {
            // TODO can we delegate to subjects in observer positions?
            return null;
        }

        protected override Expression LookupMultiSubjectFactory(string id, params Type[] subjectTypes) => null;

        protected override Expression LookupReliableObservable(string id, Type elementType) => null;

        protected override Expression LookupReliableObserver(string id, Type elementType) => null;

        protected override Expression LookupReliableMultiSubjectFactory(string id, Type inputType, Type outputType) => null;

        protected override Expression LookupReliableMultiSubject(string id, Type inputType, Type outputType) => null;

        protected override Expression LookupSubscription(string id) => null;

        protected override Expression LookupReliableSubscription(string id) => null;

        protected override Expression LookupSubscriptionFactory(string id, params Type[] subscriptionTypes) => null;

        protected override Expression LookupReliableSubscriptionFactory(string id, params Type[] subscriptionTypes) => null;

        protected override Expression Inline(Expression expression, IDictionary<ParameterExpression, Expression> bindings)
        {
            var prober = new DelegationProber(bindings.ToDictionary(x => x.Key, x => targetMapper[x.Value]));
            var ret = prober.Rewrite(expression);
            return ret;
        }

        /// <summary>
        /// This finds the largest subtree that any node can delegate to and applies the delegation to the tree. It does this in a
        /// two stage process. In the check stage, it builds up an auxilary stack of invocations which in each stack frame has the delegatable targets and
        /// invocation expressions that are subexpressions of the current invocation expression. If, after checking the children of an invocation
        /// it finds that exactly one can be delegated to, then it will indicate this to its parent and return. If multiple can be delegated to then
        /// it will delegate to them by visiting the children in the rewrite stage. Then it will indicate to its parent that delegation is complete for this subtree.
        /// </summary>
        private sealed class DelegationProber : ScopedExpressionVisitor<ParameterExpression>
        {
            private readonly IDictionary<ParameterExpression, IDelegationTarget> _delegatableTargets;
            private readonly Stack<StackFrame> _invocationStack;

            private RecursionState _state;

            private sealed class StackFrame
            {
                public LinkedList<ParameterExpression> DelegationTargets { get; set; }

                public LinkedList<InvocationAndState> Invocations { get; set; }
            }

            private readonly struct InvocationAndState
            {
                private InvocationAndState(InvocationSubtreeState state, ParameterExpression delegationTarget)
                {
                    State = state;
                    DelegationTarget = delegationTarget;
                }

                public static InvocationAndState CreateDisqualified()
                {
                    return new InvocationAndState(InvocationSubtreeState.Disqualified, delegationTarget: null);
                }

                public static InvocationAndState CreateFree()
                {
                    return new InvocationAndState(InvocationSubtreeState.Free, delegationTarget: null);
                }

                public static InvocationAndState CreateCandidate(ParameterExpression p)
                {
                    return new InvocationAndState(InvocationSubtreeState.Candidate, p);
                }

                public InvocationSubtreeState State { get; }

                public ParameterExpression DelegationTarget { get; }
            }

            private enum RecursionState
            {
                Check,
                Rewrite,
            }

            private enum InvocationSubtreeState
            {
                // No delegation targets in this subtree
                Free,

                // One delegation target in this subtree
                Candidate,

                // A subtree that has already been delegated. This subtree is no longer considered for delegation.
                // We may want to revisit this but this would require more sophisticated cycle detection
                Disqualified,
            }

            public DelegationProber(IDictionary<ParameterExpression, IDelegationTarget> delegatableTargets)
            {
                _delegatableTargets = delegatableTargets;
                _invocationStack = new Stack<StackFrame>();
            }

            public Expression Rewrite(Expression expression)
            {
                _state = RecursionState.Check;

                Push();

                var rewritten = Visit(expression);

                var delegated = RewriteInvocationsFromRoot(rewritten);

                _invocationStack.Pop();

                return delegated;
            }

            protected override ParameterExpression GetState(ParameterExpression parameter)
            {
                return parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (_delegatableTargets.TryGetValue(node, out _) && !TryLookup(node, out _))
                {
                    var frame = _invocationStack.Peek();
                    if (_state == RecursionState.Check)
                    {
                        frame.DelegationTargets.AddLast(node);
                    }
                    else
                    {
                        frame.DelegationTargets.RemoveFirst();
                    }
                }

                return base.VisitParameter(node);
            }

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                if (_state == RecursionState.Check)
                {
                    Push();

                    var rewritten = (InvocationExpression)base.VisitInvocation(node);

                    var frame = _invocationStack.Peek();

                    var freeCount = 0;
                    var dqCount = 0;
                    var candidateCount = 0;

                    foreach (var invocation in frame.Invocations)
                    {
                        if (invocation.State == InvocationSubtreeState.Disqualified) dqCount++;
                        else if (invocation.State == InvocationSubtreeState.Free) freeCount++;
                        else candidateCount++;
                    }

                    if (frame.DelegationTargets.Count == 0)
                    {
                        if (frame.Invocations.Count == freeCount)
                        {
                            PopAndSetResult(InvocationAndState.CreateFree());
                            return rewritten;
                        }
                        else if (candidateCount == 1 && dqCount == 0)
                        {
                            var candidate = frame.Invocations.Single(i => i.State == InvocationSubtreeState.Candidate).DelegationTarget;
                            var target = _delegatableTargets[candidate];
                            if (target.CanDelegate(candidate, rewritten))
                            {
                                PopAndSetResult(InvocationAndState.CreateCandidate(candidate));
                                return rewritten;
                            }
                            else
                            {
                                var delegated = RewriteInvocations(rewritten);
                                PopAndSetResult(InvocationAndState.CreateDisqualified());
                                return delegated;
                            }
                        }
                        else
                        {
                            var delegated = RewriteInvocations(rewritten);
                            PopAndSetResult(InvocationAndState.CreateDisqualified());
                            return delegated;
                        }
                    }
                    else if (frame.DelegationTargets.Count > 1)
                    {
                        var delegated = RewriteInvocations(rewritten);
                        PopAndSetResult(InvocationAndState.CreateDisqualified());
                        return delegated;
                    }
                    else
                    {
                        if (frame.Invocations.Count == freeCount)
                        {
                            var candidate = frame.DelegationTargets.Single();

                            var target = _delegatableTargets[candidate];
                            if (target.CanDelegate(candidate, rewritten))
                            {
                                PopAndSetResult(InvocationAndState.CreateCandidate(candidate));
                                return rewritten;
                            }
                            else
                            {
                                var delegated = RewriteInvocations(rewritten);
                                PopAndSetResult(InvocationAndState.CreateDisqualified());
                                return delegated;
                            }
                        }
                        else
                        {
                            var delegated = RewriteInvocations(rewritten);
                            PopAndSetResult(InvocationAndState.CreateDisqualified());
                            return delegated;
                        }
                    }
                }
                else
                {
                    var currFrame = _invocationStack.Peek();
                    var currInvocation = currFrame.Invocations.First();
                    currFrame.Invocations.RemoveFirst();

                    if (currInvocation.State == InvocationSubtreeState.Candidate)
                    {
                        var candidate = currInvocation.DelegationTarget;
                        var target = _delegatableTargets[candidate];
                        return target.Delegate(candidate, node);
                    }
                    else
                    {
                        return node;
                    }
                }
            }

            private void Push()
            {
                _invocationStack.Push(new StackFrame
                {
                    DelegationTargets = new LinkedList<ParameterExpression>(),
                    Invocations = new LinkedList<InvocationAndState>(),
                });
            }

            private void PopAndSetResult(InvocationAndState result)
            {
                _invocationStack.Pop();
                var prevFrame = _invocationStack.Peek();
                prevFrame.Invocations.AddLast(result);
            }

            private Expression RewriteInvocations(InvocationExpression expression)
            {
                _state = RecursionState.Rewrite;

                var ret = base.VisitInvocation(expression);

                _state = RecursionState.Check;

                return ret;
            }

            private Expression RewriteInvocationsFromRoot(Expression expression)
            {
                _state = RecursionState.Rewrite;

                var ret = Visit(expression);

                _state = RecursionState.Check;

                return ret;
            }
        }
    }
}
